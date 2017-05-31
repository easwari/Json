using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{    
    public class JSONProperty: IJSONContainer
    {
        private StringBuilder name;
        private object value;
        public JSONPROPERTYSTATE state;
        private bool bIsComplete;

        public JSONProperty()
        {
            name = new StringBuilder();            
            value = null;
            state = JSONPROPERTYSTATE.NEW;
            bIsComplete = false; 
        }

        public JSONProperty(string name_in)
        {
            name = new StringBuilder();
            name.Append(name_in);
            value = null;
            state = JSONPROPERTYSTATE.NAMESET;
        }

        public void SetName(string name_in)
        {
            name.Append(name_in);
            return;
        }

        public string GetName()
        {
            return name.ToString();
        }

        public void SetValue(object value_in)
        {
            value = value_in;
            state = JSONPROPERTYSTATE.COMPLETE;
        }

        public object GetValue()
        {
            return value;
        }

        public string ToJsonText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            sb.Append(GetName());
            sb.Append("\": ");
            sb.Append(StringMaker.MakeStringFromJson(value));
            return sb.ToString();
        }

        public void AddChild(object value)
        {
            SetValue(value);
        }

        public bool IsComplete()
        {
            return bIsComplete;
        }

        public void SetAsComplete()
        {
            bIsComplete = true;
        }


        public enum JSONPROPERTYSTATE
        {
            NEW,
            NAMESET,
            COMPLETE
        };
    }
}
