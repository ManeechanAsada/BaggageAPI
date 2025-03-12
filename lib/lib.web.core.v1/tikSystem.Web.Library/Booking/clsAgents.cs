using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Runtime.InteropServices;

namespace tikSystem.Web.Library
{
    public class Agents : LibraryBase
    {
        public Agent this[int index]
        {
            get { return (Agent)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(Agent value)
        {
            return this.List.Add(value);
        }

        #region Method
        
        public void AddAgent(XPathNodeIterator xn)
        {
            if (xn != null)
            {
                Agent ag;
                foreach (XPathNavigator n in xn)
                {
                    ag = new Agent();

                    ag.agency_code = XmlHelper.XpathValueNullToEmpty(n, "agency_code");
                    ag.agency_logon = XmlHelper.XpathValueNullToEmpty(n, "agency_logon");
                    ag.agency_password = XmlHelper.XpathValueNullToEmpty(n, "agency_password");
                    ag.agency_name = XmlHelper.XpathValueNullToEmpty(n, "agency_name");
                    ag.airport_rcd = XmlHelper.XpathValueNullToEmpty(n, "airport_rcd");
                    ag.ag_language_rcd = XmlHelper.XpathValueNullToEmpty(n, "ag_language_rcd");
                    ag.default_e_ticket_flag = XmlHelper.XpathValueNullToByte(n, "default_e_ticket_flag");
                    ag.email = XmlHelper.XpathValueNullToEmpty(n, "email");
                    ag.currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                    ag.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");
                    ag.agency_payment_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "agency_payment_type_rcd");
                    ag.status_code = XmlHelper.XpathValueNullToEmpty(n, "status_code");
                    ag.default_show_passenger_flag = XmlHelper.XpathValueNullToByte(n, "default_show_passenger_flag");
                    ag.default_auto_print_ticket_flag = XmlHelper.XpathValueNullToByte(n, "default_auto_print_ticket_flag");
                    ag.default_ticket_on_save_flag = XmlHelper.XpathValueNullToByte(n, "default_ticket_on_save_flag");
                    ag.web_agency_flag = XmlHelper.XpathValueNullToByte(n, "web_agency_flag");
                    ag.own_agency_flag = XmlHelper.XpathValueNullToByte(n, "own_agency_flag");
                    ag.b2b_credit_card_payment_flag = XmlHelper.XpathValueNullToByte(n, "b2b_credit_card_payment_flag");
                    ag.b2b_voucher_payment_flag = XmlHelper.XpathValueNullToByte(n, "b2b_voucher_payment_flag");
                    ag.b2b_eft_flag = XmlHelper.XpathValueNullToByte(n, "b2b_eft_flag");
                    ag.b2b_post_paid_flag = XmlHelper.XpathValueNullToByte(n, "b2b_post_paid_flag");
                    ag.b2b_allow_seat_assignment_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_seat_assignment_flag");
                    ag.b2b_allow_cancel_segment_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_cancel_segment_flag");
                    ag.b2b_allow_change_flight_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_change_flight_flag");
                    ag.b2b_allow_name_change_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_name_change_flag");
                    ag.b2b_allow_change_details_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_change_details_flag");
                    ag.ticket_stock_flag = XmlHelper.XpathValueNullToByte(n, "ticket_stock_flag");
                    ag.b2b_allow_split_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_split_flag");
                    ag.b2b_allow_service_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_service_flag");
                    ag.b2b_group_waitlist_flag = XmlHelper.XpathValueNullToByte(n, "b2b_group_waitlist_flag");
                    ag.avl_show_net_total_flag = XmlHelper.XpathValueNullToByte(n, "avl_show_net_total_flag");
                    ag.default_user_account_id = XmlHelper.XpathValueNullToGUID(n, "default_user_account_id");
                    ag.merchant_id = XmlHelper.XpathValueNullToGUID(n, "merchant_id");
                    ag.default_customer_document_id = XmlHelper.XpathValueNullToGUID(n, "default_customer_document_id");
                    ag.default_small_itinerary_document_id = XmlHelper.XpathValueNullToGUID(n, "default_small_itinerary_document_id");
                    ag.default_internal_itinerary_document_id = XmlHelper.XpathValueNullToGUID(n, "default_internal_itinerary_document_id");
                    ag.payment_default_code = XmlHelper.XpathValueNullToEmpty(n, "payment_default_code");
                    ag.agency_type_code = XmlHelper.XpathValueNullToEmpty(n, "agency_type_code");
                    ag.user_account_id = XmlHelper.XpathValueNullToGUID(n, "user_account_id");
                    ag.user_logon = XmlHelper.XpathValueNullToEmpty(n, "user_logon");
                    ag.user_code = XmlHelper.XpathValueNullToEmpty(n, "user_code");
                    ag.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                    ag.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                    ag.language_rcd = XmlHelper.XpathValueNullToEmpty(n, "language_rcd");
                    ag.make_bookings_for_others_flag = XmlHelper.XpathValueNullToByte(n, "make_bookings_for_others_flag");
                    ag.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                    ag.outstanding_invoice = XmlHelper.XpathValueNullToDecimal(n, "outstanding_invoice");
                    ag.booking_payment = XmlHelper.XpathValueNullToDecimal(n, "booking_payment");
                    ag.agency_account = XmlHelper.XpathValueNullToDecimal(n, "agency_account");
                    ag.company_client_profile_id = XmlHelper.XpathValueNullToGUID(n, "company_client_profile_id");
                    ag.invoice_days = XmlHelper.XpathValueNullToEmpty(n, "invoice_days");
                    ag.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "address_line1");
                    ag.address_line2 = XmlHelper.XpathValueNullToEmpty(n, "address_line2");
                    ag.city = XmlHelper.XpathValueNullToEmpty(n, "city");
                    ag.bank_code = XmlHelper.XpathValueNullToEmpty(n, "bank_code");
                    ag.bank_name = XmlHelper.XpathValueNullToEmpty(n, "bank_name");
                    ag.bank_account = XmlHelper.XpathValueNullToEmpty(n, "bank_account");
                    ag.contact_person = XmlHelper.XpathValueNullToEmpty(n, "contact_person");
                    ag.district = XmlHelper.XpathValueNullToEmpty(n, "district");
                    ag.phone = XmlHelper.XpathValueNullToEmpty(n, "phone");
                    ag.fax = XmlHelper.XpathValueNullToEmpty(n, "fax");
                    ag.po_box = XmlHelper.XpathValueNullToEmpty(n, "po_box");
                    ag.province = XmlHelper.XpathValueNullToEmpty(n, "province");
                    ag.state = XmlHelper.XpathValueNullToEmpty(n, "state");
                    ag.street = XmlHelper.XpathValueNullToEmpty(n, "street");
                    ag.zip_code = XmlHelper.XpathValueNullToEmpty(n, "zip_code");
                    ag.consolidator_flag = XmlHelper.XpathValueNullToByte(n, "consolidator_flag");
                    ag.b2b_credit_agency_and_invoice_flag = XmlHelper.XpathValueNullToByte(n, "b2b_credit_agency_and_invoice_flag");
                    ag.b2b_download_sales_report_flag = XmlHelper.XpathValueNullToByte(n, "b2b_download_sales_report_flag");
                    ag.b2b_show_remarks_flag = XmlHelper.XpathValueNullToByte(n, "b2b_show_remarks_flag");
                    ag.private_fares_flag = XmlHelper.XpathValueNullToByte(n, "private_fares_flag");
                    ag.b2b_allow_group_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_group_flag");
                    ag.b2b_allow_waitlist_flag = XmlHelper.XpathValueNullToByte(n, "b2b_allow_waitlist_flag");
                    ag.b2b_bsp_billing_flag = XmlHelper.XpathValueNullToByte(n, "b2b_bsp_billing_flag");
                    ag.b2b_bsp_from_date = XmlHelper.XpathValueNullToDateTime(n, "b2b_bsp_from_date");
                    ag.iata_number = XmlHelper.XpathValueNullToEmpty(n, "iata_number");
                    ag.send_mailto_all_passenger = XmlHelper.XpathValueNullToByte(n, "send_mailto_all_passenger");
                    ag.website_address = XmlHelper.XpathValueNullToEmpty(n, "website_address");
                    ag.tty_address = XmlHelper.XpathValueNullToEmpty(n, "tty_address");
                    ag.create_date_time = XmlHelper.XpathValueNullToDateTime(n, "create_date_time");
                    ag.update_date_time = XmlHelper.XpathValueNullToDateTime(n, "update_date_time");
                    ag.cashbook_closing_rcd = XmlHelper.XpathValueNullToEmpty(n, "cashbook_closing_rcd");
                    ag.cashbook_closing_id = XmlHelper.XpathValueNullToGUID(n, "cashbook_closing_id");
                    ag.create_by = XmlHelper.XpathValueNullToGUID(n, "create_by");
                    ag.legal_name = XmlHelper.XpathValueNullToEmpty(n, "legal_name");
                    ag.tax_id = XmlHelper.XpathValueNullToEmpty(n, "tax_id");
                    ag.tax_id_verified_date_time = XmlHelper.XpathValueNullToDateTime(n, "tax_id_verified_date_time");
                    ag.no_vat_flag = XmlHelper.XpathValueNullToByte(n, "no_vat_flag");
                    ag.allow_no_tax = XmlHelper.XpathValueNullToByte(n, "allow_no_tax");

                    ag.allow_add_segment_flag = XmlHelper.XpathValueNullToByte(n, "allow_add_segment_flag");
                    ag.individual_firmed_flag = XmlHelper.XpathValueNullToByte(n, "individual_firmed_flag");
                    ag.individual_waitlist_flag = XmlHelper.XpathValueNullToByte(n, "individual_waitlist_flag");
                    ag.group_firmed_flag = XmlHelper.XpathValueNullToByte(n, "group_firmed_flag");
                    ag.group_waitlist_flag = XmlHelper.XpathValueNullToByte(n, "group_waitlist_flag");
                    ag.disable_changes_through_b2c_flag = XmlHelper.XpathValueNullToByte(n, "disable_changes_through_b2c_flag");
                    ag.disable_web_checkin_flag = XmlHelper.XpathValueNullToByte(n, "disable_web_checkin_flag");
                    ag.commission_percentage = XmlHelper.XpathValueNullToDecimal(n, "commission_percentage");
                    ag.balance_lock_flag = XmlHelper.XpathValueNullToByte(n, "balance_lock_flag");
                    ag.process_baggage_tag_flag = XmlHelper.XpathValueNullToByte(n, "process_baggage_tag_flag");
                    ag.api_flag = XmlHelper.XpathValueNullToByte(n, "api_flag");

                    Add(ag);
                    ag = null;
                }
            }
        }
        public bool Register(string agencyType)
        {
            try
            {
                ServiceClient obj = new ServiceClient();
                if(this.Count > 0)
                {
                    return obj.AgencyRegistrationInsert(this[0].agency_name,
                                                        this[0].legal_name,
                                                        agencyType,
                                                        this[0].iata_number,
                                                        this[0].tax_id,
                                                        this[0].email,
                                                        this[0].fax,
                                                        this[0].phone,
                                                        this[0].address_line1,
                                                        this[0].address_line2,
                                                        this[0].street,
                                                        this[0].state,
                                                        this[0].district,
                                                        this[0].province,
                                                        this[0].city,
                                                        this[0].zip_code,
                                                        this[0].po_box,
                                                        this[0].website_address,
                                                        this[0].contact_person,
                                                        this[0].lastname,
                                                        this[0].firstname,
                                                        this[0].title_rcd,
                                                        this[0].user_logon,
                                                        this[0].agency_password,
                                                        this[0].country_rcd,
                                                        this[0].currency_rcd,
                                                        this[0].language_rcd,
                                                        this[0].comment);

                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper
        private bool ValidateUserCredential(string strAgencyCode, string strUserName, string strPassword)
        {
            if (Count > 0)
            {
                if (this[0].agency_code == strAgencyCode && this[0].agency_logon.Equals(strUserName) && this[0].agency_password.Equals(strPassword))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
