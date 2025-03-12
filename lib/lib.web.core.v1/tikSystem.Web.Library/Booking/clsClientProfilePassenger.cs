using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class ClientProfilePassenger
    {
        public Guid client_profile_id { get; set; }
        public Guid passenger_profile_id { get; set; }
        public string title_rcd { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string language_rcd { get; set; }
        public string nationality_rcd { get; set; }
        public string passenger_weight { get; set; }
        public string gender_type_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
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
    }
}
