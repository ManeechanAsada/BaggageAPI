using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for clsHeader
/// </summary>
namespace tikSystem.Web.Library
{
    [Serializable()]
    public class BookingHeader
    {
        #region Property
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        protected Guid _booking_id;

        public string booking_source_rcd
        {
            get { return _booking_source_rcd; }
            set { _booking_source_rcd = value; }
        }
        protected string _booking_source_rcd;

        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        protected string _currency_rcd;

        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        protected Guid _client_profile_id = Guid.Empty;

        public long booking_number
        {
            get { return _booking_number; }
            set { _booking_number = value; }
        }
        protected long _booking_number = 0;

        public string record_locator
        {
            get { return _record_locator; }
            set { _record_locator = value; }
        }
        protected string _record_locator = string.Empty;

        public int number_of_adults
        {
            get { return _number_of_adults; }
            set { _number_of_adults = value; }
        }
        protected int _number_of_adults = 0;

        public int number_of_children
        {
            get { return _number_of_children; }
            set { _number_of_children = value; }
        }
        protected int _number_of_children = 0;

        public int number_of_infants
        {
            get { return _number_of_infants; }
            set { _number_of_infants = value; }
        }
        protected int _number_of_infants = 0;

        public int number_of_others
        {
            get { return _number_of_others; }
            set { _number_of_others = value; }
        }
        protected int _number_of_others = 0;

        public string passenger_other_type_rcd
        {
            get { return _passenger_other_type_rcd; }
            set { _passenger_other_type_rcd = value; }
        }
        protected string _passenger_other_type_rcd = string.Empty;

        public string passenger_other_display
        {
            get { return _passenger_other_display; }
            set { _passenger_other_display = value; }
        }
        protected string _passenger_other_display = string.Empty;

        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }
        protected string _language_rcd = string.Empty;

        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        protected string _agency_code = string.Empty;

        public string contact_name
        {
            get { return _contact_name; }
            set { _contact_name = value; }
        }
        protected string _contact_name = string.Empty;

        public string contact_email
        {
            get { return _contact_email; }
            set { _contact_email = value; }
        }
        protected string _contact_email = string.Empty;

        public string phone_mobile
        {
            get { return _phone_mobile; }
            set { _phone_mobile = value; }
        }
        protected string _phone_mobile = string.Empty;

        public string phone_home
        {
            get { return _phone_home; }
            set { _phone_home = value; }
        }
        protected string _phone_home = string.Empty;

        public string phone_business
        {
            get { return _phone_business; }
            set { _phone_business = value; }
        }
        protected string _phone_business = string.Empty;

        public string received_from
        {
            get { return _received_from; }
            set { _received_from = value; }
        }
        protected string _received_from = "WEB";

        public string phone_fax
        {
            get { return _phone_fax; }
            set { _phone_fax = value; }
        }
        protected string _phone_fax = string.Empty;

        public string phone_search
        {
            get { return _phone_search; }
            set { _phone_search = value; }
        }
        protected string _phone_search = string.Empty;

        public string comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        protected string _comment = string.Empty;

        public byte notify_by_email_flag
        {
            get { return _notify_by_email_flag; }
            set { _notify_by_email_flag = value; }
        }
        protected byte _notify_by_email_flag;

        public byte notify_by_sms_flag
        {
            get { return _notify_by_sms_flag; }
            set { _notify_by_sms_flag = value; }
        }
        protected byte _notify_by_sms_flag;

        public string group_name
        {
            get { return _group_name; }
            set { _group_name = value; }
        }
        protected string _group_name = string.Empty;

        public byte group_booking_flag
        {
            get { return _group_booking_flag; }
            set { _group_booking_flag = value; }
        }
        protected byte _group_booking_flag = 0;

        public string agency_name
        {
            get { return _agency_name; }
            set { _agency_name = value; }
        }
        protected string _agency_name = string.Empty;

        public byte own_agency_flag
        {
            get { return _own_agency_flag; }
            set { _own_agency_flag = value; }
        }
        protected byte _own_agency_flag;

        public byte web_agency_flag
        {
            get { return _web_agency_flag; }
            set { _web_agency_flag = value; }
        }
        protected byte _web_agency_flag;

        public long client_number
        {
            get { return _client_number; }
            set { _client_number = value; }
        }
        protected long _client_number;

        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        protected string _lastname = string.Empty;

        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        protected string _firstname = string.Empty;

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        protected string _city = string.Empty;

        public string create_name
        {
            get { return _create_name; }
            set { _create_name = value; }
        }
        protected string _create_name = string.Empty;

        public string street
        {
            get { return _street; }
            set { _street = value; }
        }
        protected string _street = string.Empty;

        public DateTime lock_date_time
        {
            get { return _lock_date_time; }
            set { _lock_date_time = value; }
        }
        protected DateTime _lock_date_time;

        public string middlename
        {
            get { return _middlename; }
            set { _middlename = value; }
        }
        protected string _middlename = string.Empty;

        public string address_line1
        {
            get { return _address_line1; }
            set { _address_line1 = value; }
        }
        protected string _address_line1 = string.Empty;

        public string address_line2
        {
            get { return _address_line2; }
            set { _address_line2 = value; }
        }
        protected string _address_line2 = string.Empty;

        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        protected string _state = string.Empty;

        public string district
        {
            get { return _district; }
            set { _district = value; }
        }
        protected string _district = string.Empty;

        public string province
        {
            get { return _province; }
            set { _province = value; }
        }
        protected string _province = string.Empty;

        public string zip_code
        {
            get { return _zip_code; }
            set { _zip_code = value; }
        }
        protected string _zip_code = string.Empty;

        protected string _po_box = string.Empty;
        public string po_box
        {
            get { return _po_box; }
            set { _po_box = value; }
        }

        protected string _country_rcd = string.Empty;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }

        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }
        protected string _title_rcd = string.Empty;

        protected string _external_payment_reference = string.Empty;
        public string external_payment_reference
        {
            get { return _external_payment_reference; }
            set { _external_payment_reference = value; }
        }
        protected Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        protected Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        protected DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        protected DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }

        private string _cost_center = string.Empty;
        public string cost_center
        {
            get
            { return _cost_center; }
            set
            { _cost_center = value; }
        }

        private string _purchase_order = string.Empty;
        public string purchase_order
        {
            get
            { return _purchase_order; }
            set
            { _purchase_order = value; }
        }

        private string _project_number = string.Empty;
        public string project_number
        {
            get
            { return _project_number; }

            set
            { _project_number = value; }
        }

        string _lock_name = string.Empty;
        public string lock_name
        {
            get { return _lock_name; }
            set { _lock_name = value; }
        }

        string _ip_address = string.Empty;
        public string ip_address
        {
            get { return _ip_address; }
            set { _ip_address = value; }
        }

        protected byte _approval_flag;
        public byte approval_flag
        {
            get { return _approval_flag; }
            set { _approval_flag = value; }
        }
        string _invoice_receiver = string.Empty;
        public string invoice_receiver
        {
            get { return _invoice_receiver; }
            set { _invoice_receiver = value; }
        }
        string _tax_id = string.Empty;
        public string tax_id
        {
            get { return _agency_tax_id; }
            set { _agency_tax_id = value; }
        }
        string _agency_tax_id = string.Empty;
        public string agency_tax_id
        {
            get { return _agency_tax_id; }
            set { _agency_tax_id = value; }
        }
        protected byte _newsletter_flag;
        public byte newsletter_flag
        {
            get { return _newsletter_flag; }
            set { _newsletter_flag = value; }
        }
        string _contact_email_cc = string.Empty;
        public string contact_email_cc
        {
            get { return _contact_email_cc; }
            set { _contact_email_cc = value; }
        }
        string _mobile_email = string.Empty;
        public string mobile_email
        {
            get { return _mobile_email; }
            set { _mobile_email = value; }
        }
        string _vendor_rcd = string.Empty;
        public string vendor_rcd
        {
            get { return _vendor_rcd; }
            set { _vendor_rcd = value; }
        }
        DateTime _booking_date_time;
        public DateTime booking_date_time
        {
            get { return _booking_date_time; }
            set { _booking_date_time = value; }
        }
        string _notify_by_rcd;
        public string notify_by_rcd
        {
            get { return _notify_by_rcd; }
            set { _notify_by_rcd = value; }
        }
        byte _no_vat_flag;
        public byte no_vat_flag
        {
            get { return _no_vat_flag; }
            set { _no_vat_flag = value; }
        }

        public string company_name
        {
            get { return _company_name; }
            set { _company_name = value; }
        }
        protected string _company_name = string.Empty;

        public byte business_flag
        {
            get { return _business_flag; }
            set { _business_flag = value; }
        }
        protected byte _business_flag;

        protected byte _disable_changes_through_b2c_flag;
        public byte disable_changes_through_b2c_flag
        {
            get { return _disable_changes_through_b2c_flag; }
            set { _disable_changes_through_b2c_flag = value; }
        }
        #endregion

    }
}

