using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    class JSONEmptyContainer : IJSONContainer
    {
        private object value;
        private bool bIsComplete;

        public JSONEmptyContainer()
        {
            bIsComplete = false;
        }

        public string ToJsonText()
        {
            return (StringMaker.MakeStringFromJson(value));
        }

        public void AddChild(object value_in)
        {            
            value = value_in;
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
