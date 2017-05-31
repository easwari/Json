using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    public class JSONArray: IJSONContainer
    {
        private List<object> elements;
        private bool bIsComplete;

        public JSONArray()
        {
            elements = new List<object>();
            bIsComplete = false;
        }

        public string ToJsonText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (elements.Count > 0)
            {
                foreach (object value in elements)
                {
                    sb.Append(StringMaker.MakeStringFromJson(value));                    
                    sb.Append(", ");
                }

                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append("]");
            return sb.ToString();
        }

        public void AddChild(object value)
        {
            elements.Add(value);
            return;
        }
        public bool IsComplete()
        {
            return bIsComplete;
        }

        public void SetAsComplete()
        {
            bIsComplete = true;
        }

    }
}