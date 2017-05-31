using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    public class JSONParser
    {
        private JSONModelBuilder jMB;
        
        public JSONParser()
        {
            jMB = new JSONModelBuilder();        
        }

        public IJSONContainer FromJsonText(string jsonText)
        {
            int len = jsonText.Length;
            int i = 0;
            while (i < len)
            {             
                switch (jsonText[i])
                {
                    case '"':
                        {
                            string str = ReadCompleteString( jsonText, i );
                            jMB.HandleString(str);
                            i = i + str.Length + 1;
                            break;
                        }

                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        {
                            string numberStr = ReadCompleteNumber(jsonText, i);
                            int intValue;
                            double doubleValue;

                            if (int.TryParse(numberStr, out intValue))
                                jMB.HandlePrimitiveValue(intValue);
                            else if (double.TryParse(numberStr, out doubleValue))
                                jMB.HandlePrimitiveValue(doubleValue);
                            else
                            {
                                StringBuilder sbEx = new StringBuilder();
                                sbEx.Append("Weird Number : '");
                                sbEx.Append(numberStr);
                                sbEx.Append("' JSONParser can not handle this.");
                                throw new Exception(sbEx.ToString());
                            }
                            i = i + numberStr.Length - 1;
                            break;
                        }

                    case 'T':
                    case 't':
                    case 'F':
                    case 'f':
                        {
                            bool boolValue = ReadCompleteBool(jsonText, i);
                            jMB.HandlePrimitiveValue(boolValue);
                            if (boolValue)
                                i = i + 3;
                            else
                                i = i + 4;
                            break;
                        }
                    case 'n':
                        {
                            jMB.HandlePrimitiveValue(ReadNull(jsonText, i));
                            i = i + 3;
                            break;
                        }

                    case '{':
                        {
                            jMB.StartObject();
                            break;
                        }
                    case '}':
                        {
                            jMB.CloseObject();
                            break;
                        }

                    case '[':
                        {
                            jMB.StartArray();
                            break;
                        }

                    case ']':
                        {
                            jMB.CloseArray();
                            break;
                        }
                    
                    case ':':
                        {
                            jMB.HandleColon();
                            break;
                        }

                    case ',':
                        {
                            jMB.HandleComma();
                            break;
                        }

                    case ' ':
                        {
                            break;
                        }

                    default:
                        {
                            StringBuilder sbEx = new StringBuilder();
                            sbEx.Append("Current character: '");
                            sbEx.Append(jsonText[i]);
                            sbEx.Append("' JSONParser can not handle the character");
                            throw new Exception(sbEx.ToString());                           
                        }
                }
                i++;
            }
            return (jMB.HandleEndOfText());
            //return jMB.getJsonTree();
        }
        
        public string ReadCompleteString(string jsonText, int startlocation)
        {
            StringBuilder sb = new StringBuilder();

            int jsonTextLength = jsonText.Length;
            int i = startlocation + 1;
            char lastchar = jsonText[i];
            bool bstringEnded = false;

            while (i < jsonTextLength)
            {
                char currentChar = (jsonText[i++]);
                if ((currentChar == '"') && (lastchar != '\\'))
                {
                    bstringEnded = true;
                    break;
                }
                sb.Append(currentChar);
                lastchar = currentChar;
            }

            if (!bstringEnded)
                throw new Exception("Incorrect JSON Syntax: Incomplete String Exists in the JSON Text.");

            return sb.ToString();
        }

        public object ReadNull(string jsonText, int startlocation)
        {
            int jsonTextLength = jsonText.Length;
            int i = startlocation;

            StringBuilder sbBoolstring = new StringBuilder();

            for (int j = 0; j < 4; j++)
            {
                if (i >= jsonText.Length)
                    break;

                sbBoolstring.Append(jsonText[i++]);
            }

            if (sbBoolstring.ToString().ToLower() == "null")
                return null;
            
            throw new Exception("Incorrect JSON Syntax: Unsupported value");
        }

        public string ReadCompleteNumber(string jsonText, int startlocation)
        {
            StringBuilder sb = new StringBuilder();
            int jsonTextLength = jsonText.Length;
            int i = startlocation;
            while (i < jsonTextLength)
            {
                char currentChar = (jsonText[i++]);
                if ((currentChar >= '0') && (currentChar <= '9') || (currentChar == '.'))
                    sb.Append(currentChar);
                else
                    break;
            }

            return sb.ToString();
        }

        public bool ReadCompleteBool(string jsonText, int startlocation)
        {   
            int jsonTextLength = jsonText.Length;
            int i = startlocation;

            StringBuilder sbBoolstring = new StringBuilder();
            
            for (int j = 0; j < 4; j++)
            {
                if (i >= jsonText.Length)
                    break;

                sbBoolstring.Append( jsonText[i++]);
            }

            if (sbBoolstring.ToString().ToLower() == "true")
                return true;
            else
            {
                sbBoolstring.Append(jsonText[i++]);
                if (sbBoolstring.ToString().ToLower() == "false")
                    return false;
            }
            throw new Exception("Incorrect JSON Syntax: Unsupported value");
        }
    }
}