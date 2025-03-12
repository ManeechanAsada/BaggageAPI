using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    [XmlRoot("UserList")]
    public class User
    {
        #region Property
        Guid _user_account_id;
        public Guid user_account_id
        {
            get { return _user_account_id; }
            set { _user_account_id = value; }
        }
        string _user_logon = string.Empty;
        public string user_logon
        {
            get { return _user_logon; }
            set { _user_logon = value; }
        }

        string _user_code = string.Empty;
        public string user_code
        {
            get { return _user_code; }
            set { _user_code = value; }
        }
        string _lastname = string.Empty;
        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        string _firstname = string.Empty;
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        string _email_address = string.Empty;
        public string email_address
        {
            get { return _email_address; }
            set { _email_address = value; }
        }
        string _status_code = "A";
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }
        string _user_password = string.Empty;
        public string user_password
        {
            get { return _user_password; }
            set { _user_password = value; }
        }
        string _language_rcd = "EN";
        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }
        Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        byte _system_admin_flag = 0;
        public byte system_admin_flag
        {
            get { return _system_admin_flag; }
            set { _system_admin_flag = value; }
        }

        byte _make_bookings_for_others_flag = 0;
        public byte make_bookings_for_others_flag
        {
            get { return _make_bookings_for_others_flag; }
            set { _make_bookings_for_others_flag = value; }
        }

        string _address_default_code = "";
        public string address_default_code
        {
            get { return _address_default_code; }
            set { _address_default_code = value; }
        }

        byte _change_segment_flag = 0;
        public byte change_segment_flag
        {
            get { return _change_segment_flag; }
            set { _change_segment_flag = value; }
        }

        byte _delete_segment_flag = 0;
        public byte delete_segment_flag
        {
            get { return _delete_segment_flag; }
            set { _delete_segment_flag = value; }
        }

        byte _update_booking_flag = 0;
        public byte update_booking_flag
        {
            get { return _update_booking_flag; }
            set { _update_booking_flag = value; }
        }

        byte _issue_ticket_flag = 0;
        public byte issue_ticket_flag
        {
            get { return _issue_ticket_flag; }
            set { _issue_ticket_flag = value; }
        }

        byte _counter_sales_report_flag = 0;
        public byte counter_sales_report_flag
        {
            get { return _counter_sales_report_flag; }
            set { _counter_sales_report_flag = value; }
        }
        byte _mon_flag = 0;
        public byte mon_flag
        {
            get { return _mon_flag; }
            set { _mon_flag = value; }
        }
        byte _tue_flag = 0;
        public byte tue_flag
        {
            get { return _tue_flag; }
            set { _tue_flag = value; }
        }
        byte _wed_flag = 0;
        public byte wed_flag
        {
            get { return _wed_flag; }
            set { _wed_flag = value; }
        }
        byte _thu_flag = 0;
        public byte thu_flag
        {
            get { return _thu_flag; }
            set { _thu_flag = value; }
        }
        byte _fri_flag = 0;
        public byte fri_flag
        {
            get { return _fri_flag; }
            set { _fri_flag = value; }
        }
        byte _sat_flag = 0;
        public byte sat_flag
        {
            get { return _sat_flag; }
            set { _sat_flag = value; }
        }
        byte _sun_flag = 0;
        public byte sun_flag
        {
            get { return _sun_flag; }
            set { _sun_flag = value; }
        }
        string _password_b2e = string.Empty;
        public string password_b2e
        {
            get { return _password_b2e; }
            set { _password_b2e = value; }
        }
        string _new_password_b2e = string.Empty;
        public string new_password_b2e
        {
            get { return _new_password_b2e; }
            set { _new_password_b2e = value; }
        }
        string _isUser = string.Empty;
        public string isUser
        {
            get { return _isUser; }
            set { _isUser = value; }
        }
        string _isURL = string.Empty;
        public string isURL
        {
            get { return _isURL; }
            set { _isURL = value; }
        }
        byte _logoYeti_flag = 0;
        public byte logoYeti_flag
        {
            get { return _logoYeti_flag; }
            set { _logoYeti_flag = value; }
        }
        byte _logoTara_flag = 0;
        public byte logoTara_flag
        {
            get { return _logoTara_flag; }
            set { _logoTara_flag = value; }
        }
        #endregion
    }
}
