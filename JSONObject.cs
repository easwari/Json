using System;
using System.Collections.Generic;
using System.Text;

namespace JSONUtil
{
    public class JSONObject : IJSONContainer
    {
        private bool bIsComplete;
        private List<JSONProperty> propertyList;

        public JSONObject()
        {
            propertyList = new List<JSONProperty>();
            bIsComplete = false;
        }

        public string ToJsonText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (propertyList.Count > 0)
            {
                foreach (JSONProperty ijv in propertyList)
                {
                    sb.Append(ijv.ToJsonText());
                    sb.Append(", ");
                }
                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append("}");
            return sb.ToString();
        }

        public void AddChild(object value)
        {
            if (value is JSONProperty)
            {
                propertyList.Add((JSONProperty)value);
                return;
            }
            else
            {
                throw new Exception("Incorrect JSON Syntax: Non-property value can not be added to object");
            }
            
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