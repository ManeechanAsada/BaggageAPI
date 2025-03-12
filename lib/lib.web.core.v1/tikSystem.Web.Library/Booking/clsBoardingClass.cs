using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class BoardingClass
    {
        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }

        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }

        string _status_code;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }

        DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }

        Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }

        DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }

        Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }

        string _create_name;
        public string create_name
        {
            get { return _create_name; }
            set { _create_name = value; }
        }

        string _update_name;
        public string update_name
        {
            get { return _update_name; }
            set { _update_name = value; }
        }
    }
}
