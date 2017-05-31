using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    class StringMaker
    {
        public static string MakeStringFromJson(object value)
        {
            StringBuilder sb = new StringBuilder();

            if (value == null)
                sb.Append("null");

            else if (value is JSONArray)
                sb.Append(((JSONArray)value).ToJsonText());

            else if (value is JSONObject)
                sb.Append(((JSONObject)value).ToJsonText());

            else if (value is string)
            {
                sb.Append("\"");
                sb.Append(value.ToString());
                sb.Append("\"");
            }
            else
                sb.Append(value.ToString());

            return sb.ToString();
        }
    }
}
