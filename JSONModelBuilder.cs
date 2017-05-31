using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    class JSONModelBuilder : IJSONModelBuilder
    {
        private StringBuilder stringHolder;
        private Stack<IJSONContainer> jsonStack;
        private TokenStateManager TSM;

        public JSONModelBuilder()
        {
            stringHolder = new StringBuilder();            
            TSM = new TokenStateManager();
            jsonStack     = new Stack<IJSONContainer>();            
        }
        public void StartObject()
        {
            if (!TSM.IsGoodForObjectCreation())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed left curly bracket.");

            JSONObject jObj = new JSONObject();
            jsonStack.Push(jObj);            

            TSM.SetLastToken(JSONTOKEN.LEFTCURLY);
            return;
        }
        public void CloseObject()
        {
            if (!TSM.IsGoodForObjectClosure())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed right curly bracket.");

            CloseValue();

            jsonStack.Peek().SetAsComplete();
            if (jsonStack.Count > 1)            
            {                
                IJSONContainer iJV = jsonStack.Pop();
                jsonStack.Peek().AddChild(iJV);
            }
            
            TSM.SetLastToken(JSONTOKEN.RIGHTCURLY);
            return;
        }

        private void CloseValue()
        {
            if (jsonStack.Peek() is JSONProperty)
            {
                JSONProperty jProp = (JSONProperty)jsonStack.Peek();
                if (jProp.state == JSONProperty.JSONPROPERTYSTATE.NAMESET)
                {
                    if (stringHolder.Length > 0)
                    {                        
                        jProp.SetValue(stringHolder.ToString());
                        stringHolder.Remove(0, stringHolder.Length);
                        TSM.SetLastToken(JSONTOKEN.STRINGVALUE);
                    }
                    else
                        throw new Exception("Incorrect JSON Syntax: Property has no value");
                }

                if (jProp.state == JSONProperty.JSONPROPERTYSTATE.COMPLETE)
                {
                    jsonStack.Peek().SetAsComplete();
                    IJSONContainer iJV = jsonStack.Pop();
                    jsonStack.Peek().AddChild(iJV);
                }
            }

            if (jsonStack.Peek() is JSONArray)
            {
                JSONArray jArr = (JSONArray)jsonStack.Peek();
                if (stringHolder.Length > 0)
                {
                    jArr.AddChild(stringHolder.ToString());
                    stringHolder.Remove(0, stringHolder.Length);
                    TSM.SetLastToken(JSONTOKEN.STRINGVALUE);
                }
            }
        }
        
        public void HandleString(string stringVal)
        {
            if (!TSM.IsGoodForString())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed string.");

            stringHolder.Append(stringVal);
            TSM.SetSubToken(JSONSUBTOKEN.STRING);
            return;
        }

        public void HandleColon()
        {
            if (!TSM.IsGoodForColon())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed colon.");

            string propertyName = stringHolder.ToString();
            stringHolder.Remove(0, stringHolder.Length);

            if(!IsPropertyNameGood(propertyName))
                throw new Exception("Incorrect JSON Syntax: Incorrect property name.");

            JSONProperty jProp = new JSONProperty(propertyName);
            jsonStack.Push(jProp);
            
            TSM.SetLastToken(JSONTOKEN.PROPERTYNAME);
            return;
        }

        public IJSONContainer HandleEndOfText()
        {
            if (stringHolder.Length > 0)
            {
                if (jsonStack.Count == 0)
                {
                    JSONEmptyContainer jEmpty = new JSONEmptyContainer();
                    jEmpty.AddChild(stringHolder.ToString());
                    jEmpty.SetAsComplete();
                    jsonStack.Push(jEmpty);
                }                
                else
                    throw new Exception("Incorrect JSON Syntax: Incomplete JSON Text.");
            }

            if (jsonStack.Count > 1) 
                throw new Exception("Incorrect JSON Syntax: Incomplete JSON Text.");

            if(!jsonStack.Peek().IsComplete())
                throw new Exception("Incomplete object/array.");

            return (jsonStack.Pop());            
        }

        public void HandleComma()
        {
            CloseValue();
            TSM.SetLastToken(JSONTOKEN.COMMA);
            return;
        }
        
        public void StartArray()
        {
            if (!TSM.IsGoodForArrayCreation())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed left curly bracket.");

            JSONArray jArr = new JSONArray();
            jsonStack.Push(jArr);

            TSM.SetLastToken(JSONTOKEN.LEFTSQUARE);
            return;
        }

	    public void CloseArray()
        {
            if(!TSM.IsGoodForArrayClosure())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed right square bracket.");

            CloseValue();
            jsonStack.Peek().SetAsComplete();
            if (jsonStack.Count > 1)
            {                
                IJSONContainer iJV = jsonStack.Pop();
                jsonStack.Peek().AddChild(iJV);
            }
            
            TSM.SetLastToken(JSONTOKEN.RIGHTSQUARE);
            return;
        }
        
        public void HandlePrimitiveValue(object primitiveVal)
        {
            if (!TSM.IsGoodForPrimitives())
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed primitive value.");

            if (jsonStack.Count == 0)
            {
                JSONEmptyContainer jEmpty = new JSONEmptyContainer();
                jEmpty.AddChild(primitiveVal);
                jEmpty.SetAsComplete();
                jsonStack.Push(jEmpty);
            }
            else if (jsonStack.Peek() is JSONProperty)
            {
                JSONProperty jProp = (JSONProperty)jsonStack.Peek();
                if (jProp.state == JSONProperty.JSONPROPERTYSTATE.NAMESET)
                {
                    jProp.SetValue(primitiveVal);
                }
            }
            else if (jsonStack.Peek() is JSONArray)
            {
                JSONArray jArr = (JSONArray)jsonStack.Peek();
                jArr.AddChild(primitiveVal);
            }
            else
                throw new Exception("Incorrect JSON Syntax: Incorrectly placed primitive value. List of primitives not placed in array or object.");

            TSM.SetLastToken(JSONTOKEN.PRIMITIVEVALUE);
            return;
        }
        
        private bool IsPropertyNameGood(string propertyName)
        {
            if (propertyName.Length > 0)
                return true;
            else
                return false;
        }
        
        class TokenStateManager
        {
            private JSONTOKEN lastToken;
            private JSONSUBTOKEN subToken;   
            
            public TokenStateManager()
            {
                lastToken = JSONTOKEN.NOTOKEN;
                subToken = JSONSUBTOKEN.NONE;
            }

            public void SetLastToken(JSONTOKEN jToken)
            {
                lastToken = jToken;
                subToken = JSONSUBTOKEN.NONE;
            }

            public JSONTOKEN GetLastToken()
            {
                return lastToken;
            }

            public bool IsGoodForObjectCreation()
            {
                if (lastToken == JSONTOKEN.NOTOKEN || 
                    lastToken == JSONTOKEN.LEFTSQUARE ||
                    lastToken == JSONTOKEN.PROPERTYNAME ||
                    lastToken == JSONTOKEN.COMMA)                    
                    return true;
                else
                    return false;
            }

            public bool IsGoodForArrayCreation()
            {
                if (lastToken == JSONTOKEN.NOTOKEN || 
                    lastToken == JSONTOKEN.LEFTSQUARE ||
                    lastToken == JSONTOKEN.PROPERTYNAME ||
                    lastToken == JSONTOKEN.COMMA)                    
                    return true;
                else
                    return false;
            }
            
            public bool IsGoodForObjectClosure()
            {
                if (   lastToken == JSONTOKEN.LEFTCURLY 
                    || lastToken == JSONTOKEN.RIGHTCURLY 
                    || lastToken == JSONTOKEN.RIGHTSQUARE
                    || lastToken == JSONTOKEN.PRIMITIVEVALUE
                    || lastToken == JSONTOKEN.STRINGVALUE
                    || subToken  == JSONSUBTOKEN.STRING
                    )
                    return true;
                else
                    return false;
            }

            public bool IsGoodForString()
            {
                if (lastToken == JSONTOKEN.NOTOKEN || 
                    lastToken == JSONTOKEN.LEFTCURLY ||
                    lastToken == JSONTOKEN.LEFTSQUARE ||
                    lastToken == JSONTOKEN.COMMA ||
                    lastToken == JSONTOKEN.PROPERTYNAME)
                    return true;
                else
                    return false;
            }

            public bool IsGoodForColon()
            {
                if (subToken == JSONSUBTOKEN.STRING && 
                    lastToken != JSONTOKEN.PROPERTYNAME)
                    return true;
                else
                    return false;
            }

            public bool IsGoodForArrayClosure()
            {
                if (   lastToken == JSONTOKEN.LEFTSQUARE
                    || lastToken == JSONTOKEN.RIGHTCURLY
                    || lastToken == JSONTOKEN.RIGHTSQUARE
                    || lastToken == JSONTOKEN.PRIMITIVEVALUE
                    || lastToken == JSONTOKEN.STRINGVALUE
                    ||  subToken == JSONSUBTOKEN.STRING
                    )
                    return true;
                else
                    return false;
            }

            public bool IsGoodForPrimitives()
            {
                if (   lastToken == JSONTOKEN.NOTOKEN
                    || lastToken == JSONTOKEN.LEFTSQUARE
                    || lastToken == JSONTOKEN.PROPERTYNAME
                    || lastToken == JSONTOKEN.COMMA
                    )
                    return true;
                else
                    return false;
            }
            
            public void SetSubToken(JSONSUBTOKEN subToken_in)
            {
                subToken = subToken_in;
            }
        }

        enum JSONSUBTOKEN
        {
            NONE,
            STRING
        };

        enum JSONTOKEN
        {
            NOTOKEN,
            LEFTCURLY,
            RIGHTCURLY,
            LEFTSQUARE,
            RIGHTSQUARE,
            COMMA,
            PROPERTYNAME,
            STRINGVALUE,
            PRIMITIVEVALUE
        };
    }
}
