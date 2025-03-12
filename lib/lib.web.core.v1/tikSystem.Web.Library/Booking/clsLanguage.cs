using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Language
    {
        string _language_rcd;
        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        string _character_set;
        public string character_set
        {
            get { return _character_set; }
            set { _character_set = value; }
        }
    }
}
