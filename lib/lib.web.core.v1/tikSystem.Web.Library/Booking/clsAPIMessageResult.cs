using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIMessageResult
    {
        #region "Property"
        string _message_result;
        public string message_result
        {
            get { return _message_result; }
            set { _message_result = value; }
        }
        #endregion
    }
}
