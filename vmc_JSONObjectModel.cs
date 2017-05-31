using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    public class JSONObjectModel
    {
        public IJSONNode Value;
        public enum PARSING_POSITION_STATE
        {
            Initial,
            Object_Started, // {
            Object_Ended,   // }
            Array_Started,  // [
            Array_Ended,    // ]
            Element_Value_Ended, // ,
            Property_Name_Started,  // "
            Property_Name_Ended,     // "
            Property_Value_Started, // :
            Property_Value_Ended, // ,
            String_Value_Started, // "
            String_Value_Ended // "
        };
        
        public JSONObjectModel(string jsonText)
        {
        }

        public static Object FromJSONText(string jsonText_in)
        {
            Stack<PARSING_POSITION_STATE> parsing_pos_states = new Stack<PARSING_POSITION_STATE>();
            parsing_pos_states.Push(PARSING_POSITION_STATE.Initial);
            int len = jsonText_in.Length;
            JSONObject jObj = null;
            StringBuilder current_string = new StringBuilder("");
            Object current_parent;
            Stack<Object> parsing_pos_states = new Stack<Object>();
            for (int i = 0; i < len; i++)
            {
                if (jsonText_in[i] == '{')
                {
                    if ( parsing_pos_states.Peek() == PARSING_POSITION_STATE.Initial ||
                            parsing_pos_states.Peek() == PARSING_POSITION_STATE.Array_Started ||
                            parsing_pos_states.Peek() == PARSING_POSITION_STATE.Property_Value_Started)
                    {
                        parsing_pos_states.Push(PARSING_POSITION_STATE.Object_Started);
                        jObj = new JSONObject();
                    }
                }
                else if (jsonText_in[i] == '}')
                {
                    if (parsing_pos_states.Peek() == PARSING_POSITION_STATE.Object_Started)
                    {
                        parsing_pos_states.Pop();
                        UseString(current_parent, current_string, PARSING_POSITION_STATE.Object_Started);
                    }
                }
                else if (jsonText_in[i] == '[')
                {
                    if (parsing_pos_states.Peek() == PARSING_POSITION_STATE.Initial ||
                            parsing_pos_states.Peek() == PARSING_POSITION_STATE.Array_Started ||
                            parsing_pos_states.Peek() == PARSING_POSITION_STATE.Property_Value_Started)
                    {
                        parsing_pos_states.Push(PARSING_POSITION_STATE.Array_Started);
                    }
                }
                else if (jsonText_in[i] == ']')
                {
                    if (parsing_pos_states.Peek() == PARSING_POSITION_STATE.Array_Started)
                    {
                        parsing_pos_states.Pop();
                    }
                }
                else if (jsonText_in[i] == ':')
                {
                    if (parsing_pos_states.Peek() == PARSING_POSITION_STATE.Property_Name_Ended)
                    {
                        parsing_pos_states.Pop();
                        parsing_pos_states.Push(PARSING_POSITION_STATE.Property_Value_Started);
                    }
                }
                else if (jsonText_in[i] == '"')
                {
                    if (parsing_pos_states.Peek() == PARSING_POSITION_STATE.Property_Name_Started ||
                        parsing_pos_states.Peek() == PARSING_POSITION_STATE.Property_Value_Started||
                        parsing_pos_states.Peek() == PARSING_POSITION_STATE.String_Value_Started)
                    {
                        current_string.Append(jsonText_in[i]);
                    }
                }
            }
            return jObj;
        }
        
        public string ToJsonText()
        {
            JSONWriter jw = new JSONWriter();
            return jw.ToJsonText(Value);
        }
    }
}
