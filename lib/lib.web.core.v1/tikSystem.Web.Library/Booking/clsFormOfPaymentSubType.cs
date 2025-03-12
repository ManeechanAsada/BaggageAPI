using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class FormOfPaymentSubType
    {
        public string form_of_payment_subtype_rcd { get; set; }
        public string display_name { get; set; }
        public string form_of_payment_rcd { get; set; }
        public Int16 expiry_days { get; set; }
        public string card_code { get; set; }
        public long voucher_reference { get; set; }
        public byte cvv_required_flag { get; set; }
        public byte validate_document_number_flag { get; set; }
        public byte display_cvv_flag { get; set; }
        public byte multiple_payment_flag { get; set; }
        public byte approval_code_required_flag { get; set; }
        public byte display_approval_code_flag { get; set; }
        public byte display_expiry_date_flag { get; set; }
        public byte expiry_date_required_flag { get; set; }
        public byte travel_agency_payment_flag { get; set; }
        public byte agency_payment_flag { get; set; }
        public byte internet_payment_flag { get; set; }
        public byte refund_payment_flag { get; set; }
        public byte address_required_flag { get; set; }
        public byte display_address_flag { get; set; }
        public byte show_pos_indictor_flag { get; set; }
        public byte require_pos_indicator_flag { get; set; }
        public byte display_issue_date_flag { get; set; }
        public byte display_issue_number_flag { get; set; }
    }
}
