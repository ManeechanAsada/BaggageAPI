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
    public class Client : Passenger
    {
        #region Property
        string _status_code = "A";
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }

        string _client_password = "";
        public string client_password
        {
            get { return _client_password; }
            set { _client_password = value; }
        }

        byte _company_flag;
        public byte company_flag
        {
            get { return _company_flag; }
            set { _company_flag = value; }
        }

        DateTime _profile_on_hold_date_time = DateTime.MinValue;
        public DateTime profile_on_hold_date_time
        {
            get { return _profile_on_hold_date_time; }
            set { _profile_on_hold_date_time = value; }
        }

        string _profile_on_hold_comment = string.Empty;
        public string profile_on_hold_comment
        {
            get { return _profile_on_hold_comment; }
            set { _profile_on_hold_comment = value; }
        }
        Guid _profile_on_hold_by;
        public Guid profile_on_hold_by
        {
            get { return _profile_on_hold_by; }
            set { _profile_on_hold_by = value; }
        }

        Guid _company_client_profile_id;
        public Guid company_client_profile_id
        {
            get { return _company_client_profile_id; }
            set { _company_client_profile_id = value; }
        }

        double _ffp_total = 0;
        public double ffp_total
        {
            get { return _ffp_total; }
            set { _ffp_total = value; }
        }
        double _ffp_period = 0;
        public double ffp_period
        {
            get { return _ffp_period; }
            set { _ffp_period = value; }
        }
        double _ffp_balance = 0;
        public double ffp_balance
        {
            get { return _ffp_balance; }
            set { _ffp_balance = value; }
        }
        string _client_type_rcd = string.Empty;
        public string client_type_rcd
        {
            get { return _client_type_rcd; }
            set { _client_type_rcd = value; }
        }

        DateTime _member_since_date = DateTime.MinValue;
        public DateTime member_since_date
        {
            get { return _member_since_date; }
            set { _member_since_date = value; }
        }

        string _member_level_display_name = string.Empty;
        public string member_level_display_name
        {
            get { return _member_level_display_name; }
            set { _member_level_display_name = value; }
        }

        double _keep_point = 0;
        public double keep_point
        {
            get { return _keep_point; }
            set { _keep_point = value; }
        }

        string _origin_rcd;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }

        string _destination_rcd;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }

        private short _lock_on_to_book_flag = 0;
        public short lock_on_to_book_flag
        {
            get { return _lock_on_to_book_flag; }
            set { _lock_on_to_book_flag = value; }
        }

        private short _lock_on_to_report_flag = 0;
        public short lock_on_to_report_flag
        {
            get { return _lock_on_to_report_flag; }
            set { _lock_on_to_report_flag = value; }
        }

        private bool _allow_ffp_redemption_flag = false;
        public bool allow_ffp_redemption_flag
        {
            get { return _allow_ffp_redemption_flag; }
            set { _allow_ffp_redemption_flag = value; }
        }
        #endregion
    }
}
