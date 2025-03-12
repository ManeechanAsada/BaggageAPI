using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    public class Service
    {
        #region Main Information
        Guid _passenger_segment_service_id = Guid.Empty;
        public Guid passenger_segment_service_id
        {
            get { return _passenger_segment_service_id; }
            set { _passenger_segment_service_id = value; }
        }
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        string _special_service_status_rcd = string.Empty;
        public string special_service_status_rcd
        {
            get { return _special_service_status_rcd; }
            set { _special_service_status_rcd = value; }
        }
        string _special_service_change_status_rcd = string.Empty;
        public string special_service_change_status_rcd
        {
            get { return _special_service_change_status_rcd; }
            set { _special_service_change_status_rcd = value; }
        }
        string _special_service_rcd = string.Empty;
        public string special_service_rcd
        {
            get { return _special_service_rcd; }
            set { _special_service_rcd = value; }
        }
        string _service_text;
        public string service_text
        {
            get { return _service_text; }
            set { _service_text = value; }
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
        string _flight_id;
        public string flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }
        string _fee_id;
        public string fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }
        Int16 _number_of_units;
        public Int16 number_of_units
        {
            get { return _number_of_units; }
            set { _number_of_units = value; }
        }
        string _origin_rcd = string.Empty;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }
        string _destination_rcd = string.Empty;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }
        bool _new_record = false;
        [XmlIgnoreAttribute]
        public bool new_record
        {
            get { return _new_record; }
            set { _new_record = value; }
        }
        string _display_name = string.Empty;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        int _cut_off_time = 0;
        public int cut_off_time
        {
            get { return _cut_off_time; }
            set { _cut_off_time = value; }
        }
        string _status_code;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }
        string _help_text;
        public string help_text
        {
            get { return _help_text; }
            set { _help_text = value; }
        }
        string _special_service_group_rcd;
        public string special_service_group_rcd
        {
            get { return _special_service_group_rcd; }
            set { _special_service_group_rcd = value; }
        }

        Guid _account_fee_by;
        public Guid account_fee_by
        {
            get { return _account_fee_by; }
            set { _account_fee_by = value; }
        }

        DateTime _account_fee_date_time;
        public DateTime account_fee_date_time
        {
            get { return _account_fee_date_time; }
            set { _account_fee_date_time = value; }
        }

        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }
        #endregion

        #region Flag Information
        public byte service_on_request_flag
        {
            get { return _service_on_request_flag; }
            set { _service_on_request_flag = value; }
        }
        protected byte _service_on_request_flag;
        byte _text_allowed_flag;
        public byte text_allowed_flag
        {
            get { return _text_allowed_flag; }
            set { _text_allowed_flag = value; }
        }
        byte _manifest_flag;
        public byte manifest_flag
        {
            get { return _manifest_flag; }
            set { _manifest_flag = value; }
        }
        byte _text_required_flag;
        public byte text_required_flag
        {
            get { return _text_required_flag; }
            set { _text_required_flag = value; }
        }
        byte _include_passenger_name_flag;
        public byte include_passenger_name_flag
        {
            get { return _include_passenger_name_flag; }
            set { _include_passenger_name_flag = value; }
        }
        byte _include_flight_segment_flag;
        public byte include_flight_segment_flag
        {
            get { return _include_flight_segment_flag; }
            set { _include_flight_segment_flag = value; }
        }
        byte _include_action_code_flag;
        public byte include_action_code_flag
        {
            get { return _include_action_code_flag; }
            set { _include_action_code_flag = value; }
        }
        byte _include_number_of_service_flag;
        public byte include_number_of_service_flag
        {
            get { return _include_number_of_service_flag; }
            set { _include_number_of_service_flag = value; }
        }
        byte _include_catering_flag;
        public byte include_catering_flag
        {
            get { return _include_catering_flag; }
            set { _include_catering_flag = value; }
        }
        byte _include_passenger_assistance_flag;
        public byte include_passenger_assistance_flag
        {
            get { return _include_passenger_assistance_flag; }
            set { _include_passenger_assistance_flag = value; }
        }
        byte _service_supported_flag;
        public byte service_supported_flag
        {
            get { return _service_supported_flag; }
            set { _service_supported_flag = value; }
        }
        byte _send_interline_reply_flag;
        public byte send_interline_reply_flag
        {
            get { return _send_interline_reply_flag; }
            set { _send_interline_reply_flag = value; }
        }
        byte _inventory_control_flag;
        public byte inventory_control_flag
        {
            get { return _inventory_control_flag; }
            set { _inventory_control_flag = value; }
        }
        #endregion
    }
}
