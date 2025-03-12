using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Title
    {
        string _title_rcd;
        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        string _gender_type_rcd;
        public string gender_type_rcd
        {
            get { return _gender_type_rcd; }
            set { _gender_type_rcd = value; }
        }
    }
}
