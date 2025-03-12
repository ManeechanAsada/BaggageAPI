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
    public class Fee
    {
        #region Property
        Guid _booking_fee_id = Guid.Empty;
        public Guid booking_fee_id
        {
            get { return _booking_fee_id; }
            set { _booking_fee_id = value; }
        }
        decimal _fee_amount;
        public decimal fee_amount
        {
            get { return _fee_amount; }
            set { _fee_amount = value; }
        }
        Guid _booking_id = Guid.Empty;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        decimal _acct_fee_amount;
        public decimal acct_fee_amount
        {
            get { return _acct_fee_amount; }
            set { _acct_fee_amount = value; }
        }
        Guid _fee_id = Guid.Empty;
        public Guid fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }
        decimal _vat_percentage;
        public decimal vat_percentage
        {
            get { return _vat_percentage; }
            set { _vat_percentage = value; }
        }
        decimal _fee_amount_incl;
        public decimal fee_amount_incl
        {
            get { return _fee_amount_incl; }
            set { _fee_amount_incl = value; }
        }
        decimal _acct_fee_amount_incl;
        public decimal acct_fee_amount_incl
        {
            get { return _acct_fee_amount_incl; }
            set { _acct_fee_amount_incl = value; }
        }
        string _fee_rcd;
        public string fee_rcd
        {
            get { return _fee_rcd; }
            set { _fee_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
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
        DateTime _void_date_time = DateTime.MinValue;
        public DateTime void_date_time
        {
            get { return _void_date_time; }
            set { _void_date_time = value; }
        }
        protected Guid _void_by = Guid.Empty;
        public Guid void_by
        {
            get { return _void_by; }
            set { _void_by = value; }
        }
        decimal _payment_amount;
        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
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
        protected Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        protected string _agency_code = string.Empty;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        Guid _passenger_segment_service_id = Guid.Empty;
        public Guid passenger_segment_service_id
        {
            get { return _passenger_segment_service_id; }
            set { _passenger_segment_service_id = value; }
        }
        string _fee_category_rcd;
        public string fee_category_rcd
        {
            get { return _fee_category_rcd; }
            set { _fee_category_rcd = value; }
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
        string _od_origin_rcd;
        public string od_origin_rcd
        {
            get { return _od_origin_rcd; }
            set { _od_origin_rcd = value; }
        }
        string _od_destination_rcd;
        public string od_destination_rcd
        {
            get { return _od_destination_rcd; }
            set { _od_destination_rcd = value; }
        }
        decimal _number_of_units;
        public decimal number_of_units
        {
            get { return _number_of_units; }
            set { _number_of_units = value; }
        }
        decimal _total_amount;
        public decimal total_amount
        {
            get { return _total_amount; }
            set { _total_amount = value; }
        }
        decimal _total_amount_incl;
        public decimal total_amount_incl
        {
            get { return _total_amount_incl; }
            set { _total_amount_incl = value; }
        }
        byte _manual_fee_flag;
        public byte manual_fee_flag
        {
            get { return _manual_fee_flag; }
            set { _manual_fee_flag = value; }
        }
        byte _od_flag;
        public byte od_flag
        {
            get { return _od_flag; }
            set { _od_flag = value; }
        }
        byte _skip_fare_allowance_flag;
        public byte skip_fare_allowance_flag
        {
            get { return _skip_fare_allowance_flag; }
            set { _skip_fare_allowance_flag = value; }
        }
        string _fee_level;
        public string fee_level
        {
            get { return _fee_level; }
            set { _fee_level = value; }
        }
        string _fee_calculation_rcd;
        public string fee_calculation_rcd
        {
            get { return _fee_calculation_rcd; }
            set { _fee_calculation_rcd = value; }
        }
        byte _minimum_fee_amount_flag;
        public byte minimum_fee_amount_flag
        {
            get { return _minimum_fee_amount_flag; }
            set { _minimum_fee_amount_flag = value; }
        }
        decimal _fee_percentage;
        public decimal fee_percentage
        {
            get { return _fee_percentage; }
            set { _fee_percentage = value; }
        }
        string _change_comment;
        public string change_comment
        {
            get { return _change_comment; }
            set { _change_comment = value; }
        }
        string _comment;
        public string comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        decimal _total_fee_amount;
        public decimal total_fee_amount
        {
            get { return _total_fee_amount; }
            set { _total_fee_amount = value; }
        }
        decimal _total_fee_amount_incl;
        public decimal total_fee_amount_incl
        {
            get { return _total_fee_amount_incl; }
            set { _total_fee_amount_incl = value; }
        }
        string _units;
        public string units
        {
            get { return _units; }
            set { _units = value; }
        }
        string _charge_currency_rcd;
        public string charge_currency_rcd
        {
            get { return _charge_currency_rcd; }
            set { _charge_currency_rcd = value; }
        }

        decimal _charge_amount;
        public decimal charge_amount
        {
            get { return _charge_amount; }
            set { _charge_amount = value; }
        }

        decimal _charge_amount_incl;
        public decimal charge_amount_incl
        {
            get { return _charge_amount_incl; }
            set { _charge_amount_incl = value; }
        }

        string _document_number;
        public string document_number
        {
            get { return _document_number; }
            set { _document_number = value; }
        }

        DateTime _document_date_time;
        public DateTime document_date_time
        {
            get { return _document_date_time; }
            set { _document_date_time = value; }
        }

        string _external_reference;
        public string external_reference
        {
            get { return _external_reference; }
            set { _external_reference = value; }
        }

        string _vendor_rcd;
        public string vendor_rcd
        {
            get { return _vendor_rcd; }
            set { _vendor_rcd = value; }
        }
        Guid _baggage_fee_option_id;
        public Guid baggage_fee_option_id
        {
            get { return _baggage_fee_option_id; }
            set { _baggage_fee_option_id = value; }
        }
        bool _new_record = false;
        [XmlIgnoreAttribute]
        public bool new_record
        {
            get { return _new_record; }
            set { _new_record = value; }
        }
        bool _selected_fee = false;
        [XmlIgnoreAttribute]
        public bool selected_fee
        {
            get { return _selected_fee; }
            set { _selected_fee = value; }
        }
        #endregion
    }
}
