using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace tikSystem.Web.Library
{
    public class Agent
    {
        #region Property
        string _agency_code = string.Empty;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }

        string _currency_rcd = string.Empty;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        string _agency_payment_type_rcd = string.Empty;
        public string agency_payment_type_rcd
        {
            get { return _agency_payment_type_rcd; }
            set { _agency_payment_type_rcd = value; }
        }

        string _airport_rcd = string.Empty;
        public string airport_rcd
        {
            get { return _airport_rcd; }
            set { _airport_rcd = value; }
        }

        string _country_rcd = string.Empty;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }

        string _language_rcd = string.Empty;
        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }

        string _agency_password = string.Empty;
        public string agency_password
        {
            get { return _agency_password; }
            set { _agency_password = value; }
        }

        Guid _default_user_account_id = Guid.Empty;
        public Guid default_user_account_id
        {
            get { return _default_user_account_id; }
            set { _default_user_account_id = value; }
        }

        string _user_logon = string.Empty;
        public string user_logon
        {
            get { return _user_logon; }
            set { _user_logon = value; }
        }

        string _agency_logon = string.Empty;
        public string agency_logon
        {
            get { return _agency_logon; }
            set { _agency_logon = value; }
        }

        string _agency_name = string.Empty;
        public string agency_name
        {
            get { return _agency_name; }
            set { _agency_name = value; }
        }

        string _ag_language_rcd = string.Empty;
        public string ag_language_rcd
        {
            get { return _ag_language_rcd; }
            set { _ag_language_rcd = value; }
        }

        byte _default_e_ticket_flag = 0;
        public byte default_e_ticket_flag
        {
            get { return _default_e_ticket_flag; }
            set { _default_e_ticket_flag = value; }
        }

        string _email = string.Empty;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        string _status_code = string.Empty;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }
        Guid _merchant_id = Guid.Empty;
        public Guid merchant_id
        {
            get { return _merchant_id; }
            set { _merchant_id = value; }
        }
        string _notify_by = string.Empty;
        public string notify_by
        {
            get { return _notify_by; }
            set { _notify_by = value; }
        }
        Guid _default_customer_document_id = Guid.Empty;
        public Guid default_customer_document_id
        {
            get { return _default_customer_document_id; }
            set { _default_customer_document_id = value; }
        }
        Guid _default_small_itinerary_document_id = Guid.Empty;
        public Guid default_small_itinerary_document_id
        {
            get { return _default_small_itinerary_document_id; }
            set { _default_small_itinerary_document_id = value; }
        }
        Guid _default_internal_itinerary_document_id = Guid.Empty;
        public Guid default_internal_itinerary_document_id
        {
            get { return _default_internal_itinerary_document_id; }
            set { _default_internal_itinerary_document_id = value; }
        }
        string _payment_default_code = string.Empty;
        public string payment_default_code
        {
            get { return _payment_default_code; }
            set { _payment_default_code = value; }
        }
        string _agency_type_code = string.Empty;
        public string agency_type_code
        {
            get { return _agency_type_code; }
            set { _agency_type_code = value; }
        }
        Guid _user_account_id = Guid.Empty;
        public Guid user_account_id
        {
            get { return _user_account_id; }
            set { _user_account_id = value; }
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
        string _middlename = string.Empty;
        public string middlename
        {
            get { return _middlename; }
            set { _middlename = value; }
        }
        string _firstname = string.Empty;
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        string _origin_rcd = string.Empty;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }
        decimal _outstanding_invoice = 0;
        public decimal outstanding_invoice
        {
            get { return _outstanding_invoice; }
            set { _outstanding_invoice = value; }
        }
        decimal _booking_payment = 0;
        public decimal booking_payment
        {
            get { return _booking_payment; }
            set { _booking_payment = value; }
        }
        decimal _agency_account = 0;
        public decimal agency_account
        {
            get { return _agency_account; }
            set { _agency_account = value; }
        }
        protected string _website_address = string.Empty;
        public string website_address
        {
            get { return _website_address; }
            set { _website_address = value; }
        }
        protected string _tty_address = string.Empty;
        public string tty_address
        {
            get { return _tty_address; }
            set { _tty_address = value; }
        }
        protected DateTime _create_date_time = DateTime.MinValue;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        protected DateTime _update_date_time = DateTime.MinValue;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        protected string _cashbook_closing_rcd = string.Empty;
        public string cashbook_closing_rcd
        {
            get { return _cashbook_closing_rcd; }
            set { _cashbook_closing_rcd = value; }
        }
        protected Guid _cashbook_closing_id = Guid.Empty;
        public Guid cashbook_closing_id
        {
            get { return _cashbook_closing_id; }
            set { _cashbook_closing_id = value; }
        }
        protected Guid _create_by = Guid.Empty;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        int _agency_timezone = 0;
        public int agency_timezone
        {
            get { return _agency_timezone; }
            set { _agency_timezone = value; }
        }
        int _system_setting_timezone = 0;
        public int system_setting_timezone
        {
            get { return _system_setting_timezone; }
            set { _system_setting_timezone = value; }
        }
        Guid _company_client_profile_id = Guid.Empty;
        public Guid company_client_profile_id
        {
            get { return _company_client_profile_id; }
            set { _company_client_profile_id = value; }
        }
        Guid _client_profile_id = Guid.Empty;
        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        string _invoice_days = string.Empty;
        public string invoice_days
        {
            get { return _invoice_days; }
            set { _invoice_days = value; }
        }

        string _address_line1 = string.Empty;
        public string address_line1
        {
            get { return _address_line1; }
            set { _address_line1 = value; }
        }

        string _address_line2 = string.Empty;
        public string address_line2
        {
            get { return _address_line2; }
            set { _address_line2 = value; }
        }

        string _city = string.Empty;
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        string _bank_code = string.Empty;
        public string bank_code
        {
            get { return _bank_code; }
            set { _bank_code = value; }
        }

        string _bank_name = string.Empty;
        public string bank_name
        {
            get { return _bank_name; }
            set { _bank_name = value; }
        }

        string _bank_account = string.Empty;
        public string bank_account
        {
            get { return _bank_account; }
            set { _bank_account = value; }
        }

        string _contact_person = string.Empty;
        public string contact_person
        {
            get { return _contact_person; }
            set { _contact_person = value; }
        }

        string _district = string.Empty;
        public string district
        {
            get { return _district; }
            set { _district = value; }
        }

        string _phone = string.Empty;
        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        string _fax = string.Empty;
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        string _po_box = string.Empty;
        public string po_box
        {
            get { return _po_box; }
            set { _po_box = value; }
        }

        string _province = string.Empty;
        public string province
        {
            get { return _province; }
            set { _province = value; }
        }

        string _state;
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        string _street = string.Empty;
        public string street
        {
            get { return _street; }
            set { _street = value; }
        }

        string _zip_code = string.Empty;
        public string zip_code
        {
            get { return _zip_code; }
            set { _zip_code = value; }
        }

        DateTime _b2b_bsp_from_date = DateTime.MinValue;
        public DateTime b2b_bsp_from_date
        {
            get { return _b2b_bsp_from_date; }
            set { _b2b_bsp_from_date = value; }
        }

        string _iata_number = string.Empty;
        public string iata_number
        {
            get { return _iata_number; }
            set { _iata_number = value; }
        }

        byte _send_mailto_all_passenger = 0;
        public byte send_mailto_all_passenger
        {
            get { return _send_mailto_all_passenger; }
            set { _send_mailto_all_passenger = value; }
        }

        string _legal_name = string.Empty;
        public string legal_name
        {
            get { return _legal_name; }
            set { _legal_name = value; }
        }

        string _tax_id = string.Empty;
        public string tax_id
        {
            get { return _tax_id; }
            set { _tax_id = value; }
        }

        DateTime _tax_id_verified_date_time = DateTime.MinValue;
        public DateTime tax_id_verified_date_time
        {
            get { return _tax_id_verified_date_time; }
            set { _tax_id_verified_date_time = value; }
        }

        protected byte _airport_ticket_office_flag = 0;
        public byte airport_ticket_office_flag
        {
            get { return _airport_ticket_office_flag; }
            set { _airport_ticket_office_flag = value; }
        }

        protected byte _city_sales_office_flag = 0;
        public byte city_sales_office_flag
        {
            get { return _city_sales_office_flag; }
            set { _city_sales_office_flag = value; }
        }

        protected Guid _update_by = Guid.Empty;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }

        protected byte _default_ticket_on_payment_flag = 0;
        public byte default_ticket_on_payment_flag
        {
            get { return _default_ticket_on_payment_flag; }
            set { _default_ticket_on_payment_flag = value; }
        }

        protected byte _default_payment_on_save_flag = 0;
        public byte default_payment_on_save_flag
        {
            get { return _default_payment_on_save_flag; }
            set { _default_payment_on_save_flag = value; }
        }

        protected byte _email_invoice_flag = 0;
        public byte email_invoice_flag
        {
            get { return _email_invoice_flag; }
            set { _email_invoice_flag = value; }
        }

        protected byte _log_availability_flag = 0;
        public byte log_availability_flag
        {
            get { return _log_availability_flag; }
            set { _log_availability_flag = value; }
        }

        protected string _export_cycle_code = string.Empty;
        public string export_cycle_code
        {
            get { return _export_cycle_code; }
            set { _export_cycle_code = value; }
        }

        protected string _pos_indicator = string.Empty;
        public string pos_indicator
        {
            get { return _pos_indicator; }
            set { _pos_indicator = value; }
        }

        protected string _cashbook_agency_group_rcd = string.Empty;
        public string cashbook_agency_group_rcd
        {
            get { return _cashbook_agency_group_rcd; }
            set { _cashbook_agency_group_rcd = value; }
        }

        protected decimal _withholding_tax_percentage;
        public decimal withholding_tax_percentage
        {
            get { return _withholding_tax_percentage; }
            set { _withholding_tax_percentage = value; }
        }

        protected byte _commission_topup_flag = 0;
        public byte commission_topup_flag
        {
            get { return _commission_topup_flag; }
            set { _commission_topup_flag = value; }
        }

        protected byte _receive_commission_flag = 0;
        public byte receive_commission_flag
        {
            get { return _receive_commission_flag; }
            set { _receive_commission_flag = value; }
        }

        protected string _consolidator_agency_code = string.Empty;
        public string consolidator_agency_code
        {
            get { return _consolidator_agency_code; }
            set { _consolidator_agency_code = value; }
        }

        protected string _accounting_email = string.Empty;
        public string accounting_email
        {
            get { return _accounting_email; }
            set { _accounting_email = value; }
        }

        protected string _external_ar_account = string.Empty;
        public string external_ar_account
        {
            get { return _external_ar_account; }
            set { _external_ar_account = value; }
        }

        Guid _tax_id_verified_by = Guid.Empty;
        public Guid tax_id_verified_by
        {
            get { return _tax_id_verified_by; }
            set { _tax_id_verified_by = value; }
        }

        protected string _bank_iban = string.Empty;
        public string bank_iban
        {
            get { return _bank_iban; }
            set { _bank_iban = value; }
        }

        protected decimal _commission_percentage = 0;
        public decimal commission_percentage
        {
            get { return _commission_percentage; }
            set { _commission_percentage = value; }
        }

        protected string _create_name = string.Empty;
        public string create_name
        {
            get { return _create_name; }
            set { _create_name = value; }
        }

        protected string _update_name = string.Empty;
        public string update_name
        {
            get { return _update_name; }
            set { _update_name = value; }
        }
        protected byte _process_baggage_tag_flag;
        public byte process_baggage_tag_flag
        {
            get { return _process_baggage_tag_flag; }
            set { _process_baggage_tag_flag = value; }
        }
        protected byte _process_refund_flag;
        public byte process_refund_flag
        {
            get { return _process_refund_flag; }
            set { _process_refund_flag = value; }
        }

        protected string _column_1_tax_rcd = string.Empty;
        public string column_1_tax_rcd
        {
            get { return _column_1_tax_rcd; }
            set { _column_1_tax_rcd = value; }
        }

        protected string _column_2_tax_rcd = string.Empty;
        public string column_2_tax_rcd
        {
            get { return _column_2_tax_rcd; }
            set { _column_2_tax_rcd = value; }
        }

        protected string _column_3_tax_rcd = string.Empty;
        public string column_3_tax_rcd
        {
            get { return _column_3_tax_rcd; }
            set { _column_3_tax_rcd = value; }
        }

        #endregion

        #region Flag information
        byte _default_show_passenger_flag = 0;
        public byte default_show_passenger_flag
        {
            get { return _default_show_passenger_flag; }
            set { _default_show_passenger_flag = value; }
        }

        byte _default_auto_print_ticket_flag = 0;
        public byte default_auto_print_ticket_flag
        {
            get { return _default_auto_print_ticket_flag; }
            set { _default_auto_print_ticket_flag = value; }
        }

        byte _default_ticket_on_save_flag = 0;
        public byte default_ticket_on_save_flag
        {
            get { return _default_ticket_on_save_flag; }
            set { _default_ticket_on_save_flag = value; }
        }

        byte _web_agency_flag = 0;
        public byte web_agency_flag
        {
            get { return _web_agency_flag; }
            set { _web_agency_flag = value; }
        }

        byte _own_agency_flag = 0;
        public byte own_agency_flag
        {
            get { return _own_agency_flag; }
            set { _own_agency_flag = value; }
        }

        byte _b2b_eft_flag = 0;
        public byte b2b_eft_flag
        {
            get { return _b2b_eft_flag; }
            set { _b2b_eft_flag = value; }
        }

        byte _b2b_credit_card_payment_flag = 0;
        public byte b2b_credit_card_payment_flag
        {
            get { return _b2b_credit_card_payment_flag; }
            set { _b2b_credit_card_payment_flag = value; }
        }

        byte _b2b_voucher_payment_flag = 0;
        public byte b2b_voucher_payment_flag
        {
            get { return _b2b_voucher_payment_flag; }
            set { _b2b_voucher_payment_flag = value; }
        }

        byte _b2b_post_paid_flag = 0;
        public byte b2b_post_paid_flag
        {
            get { return _b2b_post_paid_flag; }
            set { _b2b_post_paid_flag = value; }
        }

        byte _b2b_allow_seat_assignment_flag = 0;
        public byte b2b_allow_seat_assignment_flag
        {
            get { return _b2b_allow_seat_assignment_flag; }
            set { _b2b_allow_seat_assignment_flag = value; }
        }

        byte _b2b_allow_cancel_segment_flag = 0;
        public byte b2b_allow_cancel_segment_flag
        {
            get { return _b2b_allow_cancel_segment_flag; }
            set { _b2b_allow_cancel_segment_flag = value; }
        }

        byte _b2b_allow_change_flight_flag = 0;
        public byte b2b_allow_change_flight_flag
        {
            get { return _b2b_allow_change_flight_flag; }
            set { _b2b_allow_change_flight_flag = value; }
        }

        byte _b2b_allow_name_change_flag = 0;
        public byte b2b_allow_name_change_flag
        {
            get { return _b2b_allow_name_change_flag; }
            set { _b2b_allow_name_change_flag = value; }
        }

        byte _b2b_allow_change_details_flag = 0;
        public byte b2b_allow_change_details_flag
        {
            get { return _b2b_allow_change_details_flag; }
            set { _b2b_allow_change_details_flag = value; }
        }

        byte _allow_noshow_cancel_segment_flag = 0;
        public byte allow_noshow_cancel_segment_flag
        {
            get { return _allow_noshow_cancel_segment_flag; }
            set { _allow_noshow_cancel_segment_flag = value; }
        }

        byte _allow_noshow_change_flight_flag = 0;
        public byte allow_noshow_change_flight_flag
        {
            get { return _allow_noshow_change_flight_flag; }
            set { _allow_noshow_change_flight_flag = value; }
        }

        byte _balance_lock_flag = 0;
        public byte balance_lock_flag
        {
            get { return _balance_lock_flag; }
            set { _balance_lock_flag = value; }
        }

        byte _issue_ticket_flag = 1;
        public byte issue_ticket_flag
        {
            get { return _issue_ticket_flag; }
            set { _issue_ticket_flag = value; }
        }

        byte _ticket_stock_flag = 0;
        public byte ticket_stock_flag
        {
            get { return _ticket_stock_flag; }
            set { _ticket_stock_flag = value; }
        }

        byte _b2b_allow_split_flag = 0;
        public byte b2b_allow_split_flag
        {
            get { return _b2b_allow_split_flag; }
            set { _b2b_allow_split_flag = value; }
        }

        byte _b2b_allow_service_flag = 0;
        public byte b2b_allow_service_flag
        {
            get { return _b2b_allow_service_flag; }
            set { _b2b_allow_service_flag = value; }
        }

        byte _b2b_group_waitlist_flag = 0;
        public byte b2b_group_waitlist_flag
        {
            get { return _b2b_group_waitlist_flag; }
            set { _b2b_group_waitlist_flag = value; }
        }

        byte _avl_show_net_total_flag = 0;
        public byte avl_show_net_total_flag
        {
            get { return _avl_show_net_total_flag; }
            set { _avl_show_net_total_flag = value; }
        }

        byte _make_bookings_for_others_flag = 0;
        public byte make_bookings_for_others_flag
        {
            get { return _make_bookings_for_others_flag; }
            set { _make_bookings_for_others_flag = value; }
        }

        byte _consolidator_flag = 0;
        public byte consolidator_flag
        {
            get { return _consolidator_flag; }
            set { _consolidator_flag = value; }
        }

        byte _b2b_credit_agency_and_invoice_flag = 0;
        public byte b2b_credit_agency_and_invoice_flag
        {
            get { return _b2b_credit_agency_and_invoice_flag; }
            set { _b2b_credit_agency_and_invoice_flag = value; }
        }

        byte _b2b_download_sales_report_flag = 0;
        public byte b2b_download_sales_report_flag
        {
            get { return _b2b_download_sales_report_flag; }
            set { _b2b_download_sales_report_flag = value; }
        }

        byte _b2b_show_remarks_flag = 0;
        public byte b2b_show_remarks_flag
        {
            get { return _b2b_show_remarks_flag; }
            set { _b2b_show_remarks_flag = value; }
        }

        byte _private_fares_flag = 0;
        public byte private_fares_flag
        {
            get { return _private_fares_flag; }
            set { _private_fares_flag = value; }
        }

        byte _b2b_allow_group_flag = 0;
        public byte b2b_allow_group_flag
        {
            get { return _b2b_allow_group_flag; }
            set { _b2b_allow_group_flag = value; }
        }

        byte _b2b_allow_waitlist_flag = 0;
        public byte b2b_allow_waitlist_flag
        {
            get { return _b2b_allow_waitlist_flag; }
            set { _b2b_allow_waitlist_flag = value; }
        }

        byte _b2b_bsp_billing_flag = 0;
        public byte b2b_bsp_billing_flag
        {
            get { return _b2b_bsp_billing_flag; }
            set { _b2b_bsp_billing_flag = value; }
        }

        byte _use_origin_currency_flag = 0;
        public byte use_origin_currency_flag
        {
            get { return _use_origin_currency_flag; }
            set { _use_origin_currency_flag = value; }
        }

        byte _no_vat_flag = 0;
        public byte no_vat_flag
        {
            get { return _no_vat_flag; }
            set { _no_vat_flag = value; }
        }

        byte _allow_no_tax = 0;
        public byte allow_no_tax
        {
            get { return _allow_no_tax; }
            set { _allow_no_tax = value; }
        }

        byte _allow_add_segment_flag = 0;
        public byte allow_add_segment_flag
        {
            get { return _allow_add_segment_flag; }
            set { _allow_add_segment_flag = value; }
        }
        byte _individual_firmed_flag = 0;
        public byte individual_firmed_flag
        {
            get { return _individual_firmed_flag; }
            set { _individual_firmed_flag = value; }
        }
        byte _individual_waitlist_flag = 0;
        public byte individual_waitlist_flag
        {
            get { return _individual_waitlist_flag; }
            set { _individual_waitlist_flag = value; }
        }
        byte _group_firmed_flag = 0;
        public byte group_firmed_flag
        {
            get { return _group_firmed_flag; }
            set { _group_firmed_flag = value; }
        }
        byte _group_waitlist_flag = 0;
        public byte group_waitlist_flag
        {
            get { return _group_waitlist_flag; }
            set { _group_waitlist_flag = value; }
        }
        byte _disable_changes_through_b2c_flag = 0;
        public byte disable_changes_through_b2c_flag
        {
            get { return _disable_changes_through_b2c_flag; }
            set { _disable_changes_through_b2c_flag = value; }
        }
        byte _disable_web_checkin_flag = 0;
        public byte disable_web_checkin_flag
        {
            get { return _disable_web_checkin_flag; }
            set { _disable_web_checkin_flag = value; }
        }
        byte _api_flag = 0;
        public byte api_flag
        {
            get { return _api_flag; }
            set { _api_flag = value; }
        }

        byte _neutral_currency_flag = 0;
        public byte neutral_currency_flag
        {
            get { return _neutral_currency_flag; }
            set { _neutral_currency_flag = value; }
        }

        string _title_rcd;
        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }

        string _comment;
        public string comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        byte _allow_change_passenger_information_flag = 0;
        public byte allow_change_passenger_information_flag
        {
            get { return _allow_change_passenger_information_flag; }
            set { _allow_change_passenger_information_flag = value; }
        }
        #endregion
    }
}
