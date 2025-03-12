using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace tikSystem.Web.Library
{
    public class Remark
    {
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        protected string _id;

        public Guid booking_remark_id
        {
            get { return _booking_remark_id; }
            set { _booking_remark_id = value; }
        }
        protected Guid _booking_remark_id;

        public string remark_type_rcd
        {
            get { return _remark_type_rcd; }
            set { _remark_type_rcd = value; }
        }
        protected string _remark_type_rcd = string.Empty;

        public DateTime timelimit_date_time
        {
            get { return _timelimit_date_time; }
            set { _timelimit_date_time = value; }
        }
        protected DateTime _timelimit_date_time ;

        public DateTime temp_timelimit_date_time
        {
            get { return _temp_timelimit_date_time; }
            set { _temp_timelimit_date_time = value; }
        }
        protected DateTime _temp_timelimit_date_time;

        public byte complete_flag
        {
            get { return _complete_flag; }
            set { _complete_flag = value; }
        }
        protected byte _complete_flag;

        public string remark_text
        {
            get { return _remark_text; }
            set { _remark_text = value; }
        }
        protected string _remark_text = string.Empty;

        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        protected Guid _booking_id = Guid.Empty;

        public string added_by
        {
            get { return _added_by; }
            set { _added_by = value; }
        }
        protected string _added_by = string.Empty;

        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        protected Guid _client_profile_id = Guid.Empty;

        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        protected string _agency_code = string.Empty;

        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        protected Guid _create_by;

        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        protected DateTime _create_date_time;

        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        protected Guid _update_by;

        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        protected DateTime _update_date_time;

        byte _system_flag;
        public byte system_flag
        {
            get { return _system_flag; }
            set { _system_flag = value; }
        }

        DateTime _utc_timelimit_date_time;
        public DateTime utc_timelimit_date_time
        {
            get { return _utc_timelimit_date_time; }
            set { _utc_timelimit_date_time = value; }
        }

        string _nickname;
        public string nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        byte _protected_flag;
        public byte protected_flag
        {
            get { return _protected_flag; }
            set { _protected_flag = value; }
        }

        byte _warning_flag;
        public byte warning_flag
        {
            get { return _warning_flag; }
            set { _warning_flag = value; }
        }

        byte _process_message_flag;
        public byte process_message_flag
        {
            get { return _process_message_flag; }
            set { _process_message_flag = value; }
        }

        Guid _split_booking_id;
        public Guid split_booking_id { get { return _split_booking_id; } set { _split_booking_id = value; } }

        string _display_name;
        public string display_name { get { return _display_name; } set { _display_name = value; } }

        string _create_name;
        public string create_name { get { return _create_name; } set { _create_name = value; } }

        string _update_name;
        public string update_name { get { return _update_name; } set { _update_name = value; } }
    } 

}
