using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIError
    {
        string _code;
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

        string _message;
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        string _serverId;
        public string serverId
        {
            get { return _serverId; }
            set { _serverId = value; }
        }

    }
}
