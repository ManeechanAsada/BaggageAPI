using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class ClientProfile
    {
        public Guid client_profile_id { get; set; }
        public string title_rcd { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string language_rcd { get; set; }
        public string nationality_rcd { get; set; }
        public string passenger_weight { get; set; }
        public string gender_type_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public bool zip_code { get; set; }
        public string po_box { get; set; }
        public bool country_rcd { get; set; }
        public bool Street { get; set; }
        public string City { get; set; }
        public string document_type_rcd { get; set; }
        public string document_number { get; set; }
        public string residence_country_rcd { get; set; }

        public string passport_number { get; set; }
        public string passport_issue_date { get; set; }
        public string passport_expiry_date { get; set; }
        public string passport_issue_place { get; set; }
        public string passport_birth_place { get; set; }
        public string date_of_birth { get; set; }
        public string passport_issue_country_rcd { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string mobile_email { get; set; }
        public string phone_mobile { get; set; }
        public string phone_home { get; set; }
        public string phone_fax { get; set; }
        public string phone_business { get; set; }
        public string employee_number { get; set; }
        public byte wheelchair_flag { get; set; }
        public byte vip_flag { get; set; }
        public string member_level_rcd { get; set; }
        public string member_number { get; set; }
        public byte window_seat_flag { get; set; }
        public string redress_number { get; set; }
        public string status_code { get; set; }
        public string client_password { get; set; }
        public byte company_flag { get; set; }
        public string client_type_rcd { get; set; }
        public string member_since_date { get; set; }
    }
}
