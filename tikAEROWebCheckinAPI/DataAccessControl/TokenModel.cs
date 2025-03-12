using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessControl
{
    public class Agency
    {
        public string agency_code { get; set; }
        public string currency_rcd { get; set; }
        public string agency_payment_type_rcd { get; set; }
        public string airport_rcd { get; set; }
        public string country_rcd { get; set; }
        public string language_rcd { get; set; }
        public string agency_password { get; set; }

        public Guid default_user_account_id { get; set; }
        public string user_logon { get; set; }
        public string agency_logon { get; set; }
        public string agency_name { get; set; }
        public string ag_language_rcd { get; set; }
        public byte default_e_ticket_flag { get; set; }
        public string email { get; set; }
        public string status_code { get; set; }
        public Guid merchant_id { get; set; }
        public string notify_by { get; set; }
        public Guid default_customer_document_id { get; set; }
        public Guid default_small_itinerary_document_id { get; set; }
        public Guid default_internal_itinerary_document_id { get; set; }
        public string payment_default_code { get; set; }
        public string agency_type_code { get; set; }
        public Guid user_account_id { get; set; }
        public string user_code { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string firstname { get; set; }
        public string origin_rcd { get; set; }
        public decimal outstanding_invoice { get; set; }
        public decimal booking_payment { get; set; }
        public decimal agency_account { get; set; }
        public string website_address { get; set; }
        public string tty_address { get; set; }
        public DateTime create_date_time { get; set; }
        public DateTime update_date_time { get; set; }
        public string cashbook_closing_rcd { get; set; }
        public Guid cashbook_closing_id { get; set; }
        public Guid create_by { get; set; }
        public int agency_timezone { get; set; }
        public int system_setting_timezone { get; set; }
        public Guid company_client_profile_id { get; set; }
        public Guid client_profile_id { get; set; }
        public string invoice_days { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string bank_account { get; set; }
        public string contact_person { get; set; }
        public string district { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string po_box { get; set; }
        public string province { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string zip_code { get; set; }
        public DateTime b2b_bsp_from_date { get; set; }
        public string iata_number { get; set; }
        public byte send_mailto_all_passenger { get; set; }
        public string legal_name { get; set; }
        public string tax_id { get; set; }
        public DateTime tax_id_verified_date_time { get; set; }
        public byte airport_ticket_office_flag { get; set; }
        public byte city_sales_office_flag { get; set; }
        public Guid update_by { get; set; }
        public byte default_ticket_on_payment_flag { get; set; }
        public byte default_payment_on_save_flag { get; set; }
        public byte email_invoice_flag { get; set; }
        public byte log_availability_flag { get; set; }
        public string export_cycle_code { get; set; }
        public string pos_indicator { get; set; }
        public string cashbook_agency_group_rcd { get; set; }
        public decimal withholding_tax_percentage { get; set; }
        public byte commission_topup_flag { get; set; }
        public byte receive_commission_flag { get; set; }
        public string consolidator_agency_code { get; set; }
        public string accounting_email { get; set; }
        public string external_ar_account { get; set; }
        public Guid tax_id_verified_by { get; set; }
        public string bank_iban { get; set; }
        public decimal commission_percentage { get; set; }
        public string create_name { get; set; }
        public string update_name { get; set; }
        public byte process_baggage_tag_flag { get; set; }
        public byte process_refund_flag { get; set; }
        public string column_1_tax_rcd { get; set; }
        public string column_2_tax_rcd { get; set; }
        public string column_3_tax_rcd { get; set; }

        public byte default_show_passenger_flag { get; set; }
        public byte default_auto_print_ticket_flag { get; set; }
        public byte default_ticket_on_save_flag { get; set; }
        public byte web_agency_flag { get; set; }
        public byte own_agency_flag { get; set; }
        public byte b2b_eft_flag { get; set; }
        public byte b2b_credit_card_payment_flag { get; set; }
        public byte b2b_voucher_payment_flag { get; set; }
        public byte b2b_post_paid_flag { get; set; }
        public byte b2b_allow_seat_assignment_flag { get; set; }
        public byte b2b_allow_cancel_segment_flag { get; set; }
        public byte b2b_allow_change_flight_flag { get; set; }
        public byte b2b_allow_name_change_flag { get; set; }
        public byte b2b_allow_change_details_flag { get; set; }
        public byte allow_noshow_cancel_segment_flag { get; set; }
        public byte allow_noshow_change_flight_flag { get; set; }
        public byte balance_lock_flag { get; set; }
        public byte issue_ticket_flag { get; set; }
        public byte ticket_stock_flag { get; set; }
        public byte b2b_allow_split_flag { get; set; }
        public byte b2b_allow_service_flag { get; set; }
        public byte b2b_group_waitlist_flag { get; set; }
        public byte avl_show_net_total_flag { get; set; }
        public byte make_bookings_for_others_flag { get; set; }
        public byte consolidator_flag { get; set; }
        public byte b2b_credit_agency_and_invoice_flag { get; set; }
        public byte b2b_download_sales_report_flag { get; set; }
        public byte b2b_show_remarks_flag { get; set; }
        public byte private_fares_flag { get; set; }
        public byte b2b_allow_group_flag { get; set; }
        public byte b2b_allow_waitlist_flag { get; set; }
        public byte b2b_bsp_billing_flag { get; set; }
        public byte use_origin_currency_flag { get; set; }
        public byte no_vat_flag { get; set; }
        public byte allow_no_tax { get; set; }
        public byte allow_add_segment_flag { get; set; }
        public byte individual_firmed_flag { get; set; }
        public byte individual_waitlist_flag { get; set; }
        public byte group_firmed_flag { get; set; }
        public byte group_waitlist_flag { get; set; }
        public byte disable_changes_through_b2c_flag { get; set; }
        public byte disable_web_checkin_flag { get; set; }
        public byte api_flag { get; set; }
        public byte neutral_currency_flag { get; set; }
        public string title_rcd { get; set; }
        public string comment { get; set; }

        public byte allow_change_passenger_information_flag { get; set; }

        public string change_of_booking_agency_code { get; set; }

        public IList<User> Users { get; set; }
    }

    public class User
    {
        public Guid user_account_id { get; set; }
        public string user_logon { get; set; }
        public string user_code { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string email_address { get; set; }
        public string status_code { get; set; }
        public string user_password { get; set; }
        public string language_rcd { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public byte system_admin_flag { get; set; }
        public byte make_bookings_for_others_flag { get; set; }
        public string address_default_code { get; set; }
        public byte change_segment_flag { get; set; }
        public byte delete_segment_flag { get; set; }
        public byte update_booking_flag { get; set; }
        public byte issue_ticket_flag { get; set; }
        public byte counter_sales_report_flag { get; set; }
        public byte mon_flag { get; set; }
        public byte tue_flag { get; set; }
        public byte wed_flag { get; set; }
        public byte thu_flag { get; set; }
        public byte fri_flag { get; set; }
        public byte sat_flag { get; set; }
        public byte sun_flag { get; set; }
    }

    public class Token
    {
        public string UserId { get; set; }
        public bool ResponseSuccess { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
    }

}
