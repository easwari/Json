using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONUtil
{
    public interface IJSONModelBuilder
    {
        void StartObject();
	    void CloseObject();
	    void StartArray();
	    void CloseArray();
	    void HandleComma();
	    void HandleColon();
        void HandleString(string stringVal);	    
        void HandlePrimitiveValue(object value);        
        IJSONContainer HandleEndOfText();        
    }
}
