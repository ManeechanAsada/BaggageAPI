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
    public class AgencyAccount
	{
		public string agency_code {
			get { return _agency_code; }
			set { _agency_code = value; }
		}
		protected string _agency_code = string.Empty;

		public string agency_name {
			get { return _agency_name; }
			set { _agency_name = value; }
		}
		protected string _agency_name = string.Empty;

		public byte own_agency_flag {
			get { return _own_agency_flag; }
			set { _own_agency_flag = value; }
		}

		protected byte _own_agency_flag = 0;
		public byte airport_ticket_office_flag {
			get { return _airport_ticket_office_flag; }
			set { _airport_ticket_office_flag = value; }
		}

		protected byte _airport_ticket_office_flag = 0;
		public byte city_sales_office_flag {
			get { return _city_sales_office_flag; }
			set { _city_sales_office_flag = value; }
		}

		protected byte _city_sales_office_flag = 0;
		public byte web_agency_flag {
			get { return _web_agency_flag; }
			set { _web_agency_flag = value; }
		}

		protected byte _web_agency_flag = 0;
		public Guid create_by {
			get { return _create_by; }
			set { _create_by = value; }
		}

		protected Guid _create_by = Guid.Empty;
		public string airport_rcd {
			get { return _airport_rcd; }
			set { _airport_rcd = value; }
		}

		protected string _airport_rcd = string.Empty;
		public DateTime create_date_time {
			get { return _create_date_time; }
			set { _create_date_time = value; }
		}

		protected DateTime _create_date_time;
		public Guid update_by {
			get { return _update_by; }
			set { _update_by = value; }
		}

		protected Guid _update_by = Guid.Empty;
		public DateTime update_date_time {
			get { return _update_date_time; }
			set { _update_date_time = value; }
		}

		protected DateTime _update_date_time;
		public string language_rcd {
			get { return _language_rcd; }
			set { _language_rcd = value; }
		}

		protected string _language_rcd = string.Empty;
		public string agency_logon {
			get { return _agency_logon; }
			set { _agency_logon = value; }
		}

		protected string _agency_logon = string.Empty;
		public string agency_password {
			get { return _agency_password; }
			set { _agency_password = value; }
		}

		protected string _agency_password = string.Empty;
		public byte default_e_ticket_flag {
			get { return _default_e_ticket_flag; }
			set { _default_e_ticket_flag = value; }
		}

		protected byte _default_e_ticket_flag = 0;
		public byte default_show_passenger_flag {
			get { return _default_show_passenger_flag; }
			set { _default_show_passenger_flag = value; }
		}

		protected byte _default_show_passenger_flag = 0;
		public byte default_auto_print_ticket_flag {
			get { return _default_auto_print_ticket_flag; }
			set { _default_auto_print_ticket_flag = value; }
		}

		protected byte _default_auto_print_ticket_flag = 0;
		public byte default_ticket_on_save_flag {
			get { return _default_ticket_on_save_flag; }
			set { _default_ticket_on_save_flag = value; }
		}

		protected byte _default_ticket_on_save_flag = 0;
		public byte default_ticket_on_payment_flag {
			get { return _default_ticket_on_payment_flag; }
			set { _default_ticket_on_payment_flag = value; }
		}

		protected byte _default_ticket_on_payment_flag = 0;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
		}

		protected string _status_code = string.Empty;
		public string contact_person {
			get { return _contact_person; }
			set { _contact_person = value; }
		}

		protected string _contact_person = string.Empty;
		public string address_line1 {
			get { return _address_line1; }
			set { _address_line1 = value; }
		}

		protected string _address_line1 = string.Empty;
		public string address_line2 {
			get { return _address_line2; }
			set { _address_line2 = value; }
		}

		protected string _address_line2 = string.Empty;
		public string street {
			get { return _street; }
			set { _street = value; }
		}

		protected string _street = string.Empty;
		public string state {
			get { return _state; }
			set { _state = value; }
		}

		protected string _state = string.Empty;
		public string district {
			get { return _district; }
			set { _district = value; }
		}

		protected string _district = string.Empty;
		public string province {
			get { return _province; }
			set { _province = value; }
		}

		protected string _province = string.Empty;
		public string city {
			get { return _city; }
			set { _city = value; }
		}

		protected string _city = string.Empty;
		public string zip_code {
			get { return _zip_code; }
			set { _zip_code = value; }
		}

		protected string _zip_code = string.Empty;
		public string po_box {
			get { return _po_box; }
			set { _po_box = value; }
		}

		protected string _po_box = string.Empty;
		public string iata_number {
			get { return _iata_number; }
			set { _iata_number = value; }
		}

		protected string _iata_number = string.Empty;
		public string phone {
			get { return _phone; }
			set { _phone = value; }
		}

		protected string _phone = string.Empty;
		public string fax {
			get { return _fax; }
			set { _fax = value; }
		}

		protected string _fax = string.Empty;
		public string email {
			get { return _email; }
			set { _email = value; }
		}

		protected string _email = string.Empty;
		public string website_address {
			get { return _website_address; }
			set { _website_address = value; }
		}

		protected string _website_address = string.Empty;
		public string currency_rcd {
			get { return _currency_rcd; }
			set { _currency_rcd = value; }
		}

		protected string _currency_rcd = string.Empty;
		public string country_rcd {
			get { return _country_rcd; }
			set { _country_rcd = value; }
		}

		protected string _country_rcd = string.Empty;
		public string agency_payment_type_rcd {
			get { return _agency_payment_type_rcd; }
			set { _agency_payment_type_rcd = value; }
		}

		protected string _agency_payment_type_rcd = string.Empty;
		public string tax_id {
			get { return _tax_id; }
			set { _tax_id = value; }
		}

		protected string _tax_id = string.Empty;
		public string legal_name {
			get { return _legal_name; }
			set { _legal_name = value; }
		}

		protected string _legal_name = string.Empty;
		public byte default_payment_on_save_flag {
			get { return _default_payment_on_save_flag; }
			set { _default_payment_on_save_flag = value; }
		}

		protected byte _default_payment_on_save_flag = 0;
		public byte receive_commission_flag {
			get { return _receive_commission_flag; }
			set { _receive_commission_flag = value; }
		}

		protected byte _receive_commission_flag = 0;
		public int invoice_days {
			get { return _invoice_days; }
			set { _invoice_days = value; }
		}

		protected int _invoice_days;
		public byte checkin_hide_birthdate_flag {
			get { return _checkin_hide_birthdate_flag; }
			set { _checkin_hide_birthdate_flag = value; }
		}

		protected byte _checkin_hide_birthdate_flag = 0;
		public byte checkin_hide_ticket_flag {
			get { return _checkin_hide_ticket_flag; }
			set { _checkin_hide_ticket_flag = value; }
		}

		protected byte _checkin_hide_ticket_flag = 0;
		public byte checkin_hide_payment_flag {
			get { return _checkin_hide_payment_flag; }
			set { _checkin_hide_payment_flag = value; }
		}

		protected byte _checkin_hide_payment_flag = 0;
		public byte checkin_hide_eticket_flag {
			get { return _checkin_hide_eticket_flag; }
			set { _checkin_hide_eticket_flag = value; }
		}

		protected byte _checkin_hide_eticket_flag = 0;
		public byte hide_history_remarks_flag {
			get { return _hide_history_remarks_flag; }
			set { _hide_history_remarks_flag = value; }
		}

		protected byte _hide_history_remarks_flag = 0;
		public decimal commission_percentage {
			get { return _commission_percentage; }
			set { _commission_percentage = value; }
		}

		protected decimal _commission_percentage;
		public string external_ar_account {
			get { return _external_ar_account; }
			set { _external_ar_account = value; }
		}

		protected string _external_ar_account = string.Empty;
		public string default_fare_type {
			get { return _default_fare_type; }
			set { _default_fare_type = value; }
		}

		protected string _default_fare_type = string.Empty;
		public string tty_address {
			get { return _tty_address; }
			set { _tty_address = value; }
		}

		protected string _tty_address = string.Empty;
		public byte manual_creditcard_handling_flag {
			get { return _manual_creditcard_handling_flag; }
			set { _manual_creditcard_handling_flag = value; }
		}

		protected byte _manual_creditcard_handling_flag = 0;
		public byte prompt_address_for_cc_flag {
			get { return _prompt_address_for_cc_flag; }
			set { _prompt_address_for_cc_flag = value; }
		}

		protected byte _prompt_address_for_cc_flag = 0;
		public byte hide_airimp_remarks_flag {
			get { return _hide_airimp_remarks_flag; }
			set { _hide_airimp_remarks_flag = value; }
		}

		protected byte _hide_airimp_remarks_flag = 0;
		public byte exclude_availability_quote_flag {
			get { return _exclude_availability_quote_flag; }
			set { _exclude_availability_quote_flag = value; }
		}

		protected byte _exclude_availability_quote_flag = 0;
		public string merchant_account {
			get { return _merchant_account; }
			set { _merchant_account = value; }
		}

		protected string _merchant_account = string.Empty;
		public string ticket_agency_address {
			get { return _ticket_agency_address; }
			set { _ticket_agency_address = value; }
		}

		protected string _ticket_agency_address = string.Empty;
		public byte auto_account_fee_flag {
			get { return _auto_account_fee_flag; }
			set { _auto_account_fee_flag = value; }
		}

		protected byte _auto_account_fee_flag = 0;
		public byte ticket_require_payment_flag {
			get { return _ticket_require_payment_flag; }
			set { _ticket_require_payment_flag = value; }
		}

		protected byte _ticket_require_payment_flag = 0;
		public string pos_indicator {
			get { return _pos_indicator; }
			set { _pos_indicator = value; }
		}

		protected string _pos_indicator = string.Empty;
		public string export_cycle_code {
			get { return _export_cycle_code; }
			set { _export_cycle_code = value; }
		}

		protected string _export_cycle_code = string.Empty;
		public byte b2b_credit_agency_and_invoice_flag {
			get { return _b2b_credit_agency_and_invoice_flag; }
			set { _b2b_credit_agency_and_invoice_flag = value; }
		}

		protected byte _b2b_credit_agency_and_invoice_flag = 0;
		public byte b2b_credit_card_payment_flag {
			get { return _b2b_credit_card_payment_flag; }
			set { _b2b_credit_card_payment_flag = value; }
		}

		protected byte _b2b_credit_card_payment_flag = 0;
		public byte b2b_voucher_payment_flag {
			get { return _b2b_voucher_payment_flag; }
			set { _b2b_voucher_payment_flag = value; }
		}

		protected byte _b2b_voucher_payment_flag = 0;
		public byte b2b_post_paid_flag {
			get { return _b2b_post_paid_flag; }
			set { _b2b_post_paid_flag = value; }
		}

		protected byte _b2b_post_paid_flag = 0;
		public byte b2b_download_sales_report_flag {
			get { return _b2b_download_sales_report_flag; }
			set { _b2b_download_sales_report_flag = value; }
		}

		protected byte _b2b_download_sales_report_flag = 0;
		public byte b2b_allow_seat_assignment_flag {
			get { return _b2b_allow_seat_assignment_flag; }
			set { _b2b_allow_seat_assignment_flag = value; }
		}

		protected byte _b2b_allow_seat_assignment_flag = 0;
		public byte b2b_allow_cancel_segment_flag {
			get { return _b2b_allow_cancel_segment_flag; }
			set { _b2b_allow_cancel_segment_flag = value; }
		}

		protected byte _b2b_allow_cancel_segment_flag = 0;
		public byte b2b_allow_change_flight_flag {
			get { return _b2b_allow_change_flight_flag; }
			set { _b2b_allow_change_flight_flag = value; }
		}

		protected byte _b2b_allow_change_flight_flag = 0;
		public byte b2b_allow_name_change_flag {
			get { return _b2b_allow_name_change_flag; }
			set { _b2b_allow_name_change_flag = value; }
		}

		protected byte _b2b_allow_name_change_flag = 0;
		public byte b2b_allow_change_details_flag {
			get { return _b2b_allow_change_details_flag; }
			set { _b2b_allow_change_details_flag = value; }
		}

		protected byte _b2b_allow_change_details_flag = 0;
		public byte b2b_show_remarks_flag {
			get { return _b2b_show_remarks_flag; }
			set { _b2b_show_remarks_flag = value; }
		}

		protected byte _b2b_show_remarks_flag = 0;
		public byte online_creditcard_handling_flag {
			get { return _online_creditcard_handling_flag; }
			set { _online_creditcard_handling_flag = value; }
		}

		protected byte _online_creditcard_handling_flag = 0;
		public byte ticket_stock_flag {
			get { return _ticket_stock_flag; }
			set { _ticket_stock_flag = value; }
		}

		protected byte _ticket_stock_flag = 0;
		public byte b2b_allow_waitlist_flag {
			get { return _b2b_allow_waitlist_flag; }
			set { _b2b_allow_waitlist_flag = value; }
		}

		protected byte _b2b_allow_waitlist_flag = 0;
		public byte b2b_allow_group_flag {
			get { return _b2b_allow_group_flag; }
			set { _b2b_allow_group_flag = value; }
		}

		protected byte _b2b_allow_group_flag = 0;
		public byte b2b_allow_split_flag {
			get { return _b2b_allow_split_flag; }
			set { _b2b_allow_split_flag = value; }
		}

		protected byte _b2b_allow_split_flag = 0;
		public byte b2b_allow_service_flag {
			get { return _b2b_allow_service_flag; }
			set { _b2b_allow_service_flag = value; }
		}

		protected byte _b2b_allow_service_flag = 0;
		public byte private_fares_flag {
			get { return _private_fares_flag; }
			set { _private_fares_flag = value; }
		}

		protected byte _private_fares_flag = 0;
		public byte process_ret_flag {
			get { return _process_ret_flag; }
			set { _process_ret_flag = value; }
		}

		protected byte _process_ret_flag = 0;
		public byte b2b_bsp_billing_flag {
			get { return _b2b_bsp_billing_flag; }
			set { _b2b_bsp_billing_flag = value; }
		}

		protected byte _b2b_bsp_billing_flag = 0;
		public DateTime b2b_bsp_from_date {
			get { return _b2b_bsp_from_date; }
			set { _b2b_bsp_from_date = value; }
		}

		protected DateTime _b2b_bsp_from_date;
		public byte avl_show_net_total_flag {
			get { return _avl_show_net_total_flag; }
			set { _avl_show_net_total_flag = value; }
		}

		protected byte _avl_show_net_total_flag = 0;
		public byte b2b_group_waitlist_flag {
			get { return _b2b_group_waitlist_flag; }
			set { _b2b_group_waitlist_flag = value; }
		}

		protected byte _b2b_group_waitlist_flag = 0;
		public byte email_invoice_flag {
			get { return _email_invoice_flag; }
			set { _email_invoice_flag = value; }
		}

		protected byte _email_invoice_flag = 0;
		public string accounting_email {
			get { return _accounting_email; }
			set { _accounting_email = value; }
		}

		protected string _accounting_email = string.Empty;
		public byte log_availability_flag {
			get { return _log_availability_flag; }
			set { _log_availability_flag = value; }
		}

		protected byte _log_availability_flag = 0;
		public decimal withholding_tax_percentage {
			get { return _withholding_tax_percentage; }
			set { _withholding_tax_percentage = value; }
		}

		protected decimal _withholding_tax_percentage;
		public byte commission_topup_flag {
			get { return _commission_topup_flag; }
			set { _commission_topup_flag = value; }
		}

		protected byte _commission_topup_flag = 0;
		public byte balance_lock_flag {
			get { return _balance_lock_flag; }
			set { _balance_lock_flag = value; }
		}

		protected byte _balance_lock_flag = 0;
		public string comment {
			get { return _comment; }
			set { _comment = value; }
		}

		protected string _comment = string.Empty;
		public Guid default_user_account_id {
			get { return _default_user_account_id; }
			set { _default_user_account_id = value; }
		}

		protected Guid _default_user_account_id = Guid.Empty;
		public Guid merchant_id {
			get { return _merchant_id; }
			set { _merchant_id = value; }
		}

		protected Guid _merchant_id = Guid.Empty;
		public string cashbook_closing_rcd {
			get { return _cashbook_closing_rcd; }
			set { _cashbook_closing_rcd = value; }
		}

		protected string _cashbook_closing_rcd = string.Empty;
		public Guid cashbook_closing_id {
			get { return _cashbook_closing_id; }
			set { _cashbook_closing_id = value; }
		}

		protected Guid _cashbook_closing_id = Guid.Empty;
		public string cashbook_agency_group_rcd {
			get { return _cashbook_agency_group_rcd; }
			set { _cashbook_agency_group_rcd = value; }
		}

		protected string _cashbook_agency_group_rcd = string.Empty;
		public byte consolidator_flag {
			get { return _consolidator_flag; }
			set { _consolidator_flag = value; }
		}

		protected byte _consolidator_flag = 0;
		public string consolidator_agency_code {
			get { return _consolidator_agency_code; }
			set { _consolidator_agency_code = value; }
		}

		protected string _consolidator_agency_code = string.Empty;
		public Guid tax_id_verified_by {
			get { return _tax_id_verified_by; }
			set { _tax_id_verified_by = value; }
		}
		protected Guid _tax_id_verified_by = Guid.Empty;

		public DateTime tax_id_verified_date_time {
			get { return _tax_id_verified_date_time; }
			set { _tax_id_verified_date_time = value; }
		}
		protected DateTime _tax_id_verified_date_time;

        public byte individual_firmed_flag
        {
            get { return _individual_firmed_flag; }
            set { _individual_firmed_flag = value; }
        }
        protected byte _individual_firmed_flag = 0;

        public byte individual_waitlist_flag
        {
            get { return _individual_waitlist_flag; }
            set { _individual_waitlist_flag = value; }
        }
        protected byte _individual_waitlist_flag = 0;

        public byte group_firmed_flag
        {
            get { return _group_firmed_flag; }
            set { _group_firmed_flag = value; }
        }
        protected byte _group_firmed_flag = 0;

        public byte group_waitlist_flag
        {
            get { return _group_waitlist_flag; }
            set { _group_waitlist_flag = value; }
        }
        protected byte _group_waitlist_flag = 0;
    }
}
