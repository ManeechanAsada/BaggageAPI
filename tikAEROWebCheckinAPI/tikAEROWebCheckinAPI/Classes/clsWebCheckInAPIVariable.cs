using System;
using System.Collections.Generic;
using System.Web;
using tikSystem.Web.Library;

namespace tikAEROWebCheckinAPI.Classes
{
    public class WebCheckinAPIVariable : MAVariable
    {
        Guid _UserId = Guid.Empty;
        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        string _agency_code = string.Empty;
        public string Agency_Code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
    }
}