using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIPassengerMapping
    {
        #region "Property"
        Guid _booking_id;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }

        string _record_locator;
        public string record_locator
        {
            get { return _record_locator; }
            set { _record_locator = value; }
        }

        string _language_rcd;
        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }

        string _agency_code;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }

        string _group_name;
        public string group_name
        {
            get { return _group_name; }
            set { _group_name = value; }
        }

        string _contact_name;
        public string contact_name
        {
            get { return _contact_name; }
            set { _contact_name = value; }
        }

        string _contact_email;
        public string contact_email
        {
            get { return _contact_email; }
            set { _contact_email = value; }
        }

        string _phone_mobile;
        public string phone_mobile
        {
            get { return _phone_mobile; }
            set { _phone_mobile = value; }
        }

        string _phone_home;
        public string phone_home
        {
            get { return _phone_home; }
            set { _phone_home = value; }
        }

        string _phone_business;
        public string phone_business
        {
            get { return _phone_business; }
            set { _phone_business = value; }
        }
        #endregion

        string _received_from;
        public string received_from
        {
            get { return _received_from; }
            set { _received_from = value; }
        }

        string _phone_fax;
        public string phone_fax
        {
            get { return _phone_fax; }
            set { _phone_fax = value; }
        }

        string _vendor_rcd;
        public string vendor_rcd
        {
            get { return _vendor_rcd; }
            set { _vendor_rcd = value; }
        }

        string _tour_operator_locator;
        public string tour_operator_locator
        {
            get { return _tour_operator_locator; }
            set { _tour_operator_locator = value; }
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

        string _seat_number;
        public string seat_number
        {
            get { return _seat_number; }
            set { _seat_number = value; }
        }

        int _seat_row;
        public int seat_row
        {
            get { return _seat_row; }
            set { _seat_row = value; }
        }

        string _seat_column;
        public string seat_column
        {
            get { return _seat_column; }
            set { _seat_column = value; }
        }

        decimal _baggage_weight;
        public decimal baggage_weight
        {
            get { return _baggage_weight; }
            set { _baggage_weight = value; }
        }

        int _piece_allowance;
        public int piece_allowance
        {
            get { return _piece_allowance; }
            set { _piece_allowance = value; }
        }

        string _airline_rcd;
        public string airline_rcd
        {
            get { return _airline_rcd; }
            set { _airline_rcd = value; }
        }

        string _flight_number;
        public string flight_number
        {
            get { return _flight_number; }
            set { _flight_number = value; }
        }

        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }

        DateTime _departure_date;
        public DateTime departure_date
        {
            get { return _departure_date; }
            set { _departure_date = value; }
        }

        string _lastname;
        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }

        string _firstname;
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        string _gender_type_rcd;
        public string gender_type_rcd
        {
            get { return _gender_type_rcd; }
            set { _gender_type_rcd = value; }
        }

        string _title_rcd;
        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }

        string _passenger_type_rcd;
        public string passenger_type_rcd
        {
            get { return _passenger_type_rcd; }
            set { _passenger_type_rcd = value; }
        }

        string _boarding_pass_number;
        public string boarding_pass_number
        {
            get { return _boarding_pass_number; }
            set { _boarding_pass_number = value; }
        }

        int _check_in_sequence;
        public int check_in_sequence
        {
            get { return _check_in_sequence; }
            set { _check_in_sequence = value; }
        }

        int _group_sequence;
        public int group_sequence
        {
            get { return _group_sequence; }
            set { _group_sequence = value; }
        }

        byte _standby_flag;
        public byte standby_flag
        {
            get { return _standby_flag; }
            set { _standby_flag = value; }
        }

        string _priority_code;
        public string priority_code
        {
            get { return _priority_code; }
            set { _priority_code = value; }
        }

        DateTime _date_of_birth;
        public DateTime date_of_birth
        {
            get { return _date_of_birth; }
            set { _date_of_birth = value; }
        }

        string _passenger_status_rcd;
        public string passenger_status_rcd
        {
            get { return _passenger_status_rcd; }
            set { _passenger_status_rcd = value; }
        }

        string _ticket_number = string.Empty;
        public string ticket_number
        {
            get { return _ticket_number; }
            set { _ticket_number = value; }
        }

        string _coupon_number = string.Empty;
        public string coupon_number
        {
            get { return _coupon_number; }
            set { _coupon_number = value; }
        }

        byte _e_ticket_flag;
        public byte e_ticket_flag
        {
            get { return _e_ticket_flag; }
            set { _e_ticket_flag = value; }
        }

        string _fare_type_rcd = string.Empty;
        public string fare_type_rcd
        {
            get { return _fare_type_rcd; }
            set { _fare_type_rcd = value; }
        }

        string _booking_class_rcd;
        public string booking_class_rcd
        {
            get { return _booking_class_rcd; }
            set { _booking_class_rcd = value; }
        }

        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
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

        string _passenger_check_in_status_rcd;
        public string passenger_check_in_status_rcd
        {
            get { return _passenger_check_in_status_rcd; }
            set { _passenger_check_in_status_rcd = value; }
        }

        string _onward_airline_rcd;
        public string onward_airline_rcd
        {
            get { return _onward_airline_rcd; }
            set { _onward_airline_rcd = value; }
        }

        string _onward_flight_number;
        public string onward_flight_number
        {
            get { return _onward_flight_number; }
            set { _onward_flight_number = value; }
        }

        DateTime _onward_departure_date;
        public DateTime onward_departure_date
        {
            get { return _onward_departure_date; }
            set { _onward_departure_date = value; }
        }

        int _onward_departure_time;
        public int onward_departure_time
        {
            get { return _onward_departure_time; }
            set { _onward_departure_time = value; }
        }

        string _onward_origin_rcd;
        public string onward_origin_rcd
        {
            get { return _onward_origin_rcd; }
            set { _onward_origin_rcd = value; }
        }

        string _onward_destination_rcd;
        public string onward_destination_rcd
        {
            get { return _onward_destination_rcd; }
            set { _onward_destination_rcd = value; }
        }
        string _onward_booking_class_rcd;
        public string onward_booking_class_rcd
        {
            get { return _onward_booking_class_rcd; }
            set { _onward_booking_class_rcd = value; }
        }

        string _onward_segment_status_rcd;
        public string onward_segment_status_rcd
        {
            get { return _onward_segment_status_rcd; }
            set { _onward_segment_status_rcd = value; }
        }

        string _previous_airline_rcd;
        public string previous_airline_rcd
        {
            get { return _previous_airline_rcd; }
            set { _previous_airline_rcd = value; }
        }

        string _previous_flight_number;
        public string previous_flight_number
        {
            get { return _previous_flight_number; }
            set { _previous_flight_number = value; }
        }

        DateTime _previous_departure_date;
        public DateTime previous_departure_date
        {
            get { return _previous_departure_date; }
            set { _previous_departure_date = value; }
        }

        int _previous_departure_time;
        public int previous_departure_time
        {
            get { return _previous_departure_time; }
            set { _previous_departure_time = value; }
        }

        string _previous_origin_rcd;
        public string previous_origin_rcd
        {
            get { return _previous_origin_rcd; }
            set { _previous_origin_rcd = value; }
        }

        string _previous_destination_rcd;
        public string previous_destination_rcd
        {
            get { return _previous_destination_rcd; }
            set { _previous_destination_rcd = value; }
        }

        int _previous_booking_class_rcd_sort;
        public int previous_booking_class_rcd_sort
        {
            get { return _previous_booking_class_rcd_sort; }
            set { _previous_booking_class_rcd_sort = value; }
        }

        string _previous_segment_status_rcd;
        public string previous_segment_status_rcd
        {
            get { return _previous_segment_status_rcd; }
            set { _previous_segment_status_rcd = value; }
        }

        DateTime _arrival_date;
        public DateTime arrival_date
        {
            get { return _arrival_date; }
            set { _arrival_date = value; }
        }

        string _departure_time;
        public string departure_time
        {
            get { return _departure_time; }
            set { _departure_time = value; }
        }

        string _arrival_time;
        public string arrival_time
        {
            get { return _arrival_time; }
            set { _arrival_time = value; }
        }

        long _client_number = 0;
        public long client_number
        {
            get { return _client_number; }
            set { _client_number = value; }
        }

        string _member_number = string.Empty;
        public string member_number
        {
            get { return _member_number; }
            set { _member_number = value; }
        }

        string _member_level_rcd = string.Empty;
        public string member_level_rcd
        {
            get { return _member_level_rcd; }
            set { _member_level_rcd = value; }
        }

        string _nationality_rcd;
        public string nationality_rcd
        {
            get { return _nationality_rcd; }
            set { _nationality_rcd = value; }
        }

        string _passport_number;
        public string passport_number
        {
            get { return _passport_number; }
            set { _passport_number = value; }
        }

        DateTime _passport_issue_date;
        public DateTime passport_issue_date
        {
            get { return _passport_issue_date; }
            set { _passport_issue_date = value; }
        }

        DateTime _passport_expiry_date;
        public DateTime passport_expiry_date
        {
            get { return _passport_expiry_date; }
            set { _passport_expiry_date = value; }
        }

        string _passport_issue_place;
        public string passport_issue_place
        {
            get { return _passport_issue_place; }
            set { _passport_issue_place = value; }
        }

        string _passport_birth_place;
        public string passport_birth_place
        {
            get { return _passport_birth_place; }
            set { _passport_birth_place = value; }
        }

        byte _wheelchair_flag;
        public byte wheelchair_flag
        {
            get { return _wheelchair_flag; }
            set { _wheelchair_flag = value; }
        }

        byte _vip_flag;
        public byte vip_flag
        {
            get { return _vip_flag; }
            set { _vip_flag = value; }
        }

        decimal _passenger_weight;
        public decimal passenger_weight
        {
            get { return _passenger_weight; }
            set { _passenger_weight = value; }
        }

        string _residence_country_rcd = string.Empty;
        public string residence_country_rcd
        {
            get { return _residence_country_rcd; }
            set { _residence_country_rcd = value; }
        }

        string _document_type_rcd = string.Empty;
        public string document_type_rcd
        {
            get { return _document_type_rcd; }
            set { _document_type_rcd = value; }
        }

        int _group_count;
        public int group_count
        {
            get { return _group_count; }
            set { _group_count = value; }
        }

        decimal _ticket_total;
        public decimal ticket_total
        {
            get { return _ticket_total; }
            set { _ticket_total = value; }
        }

        decimal _ticket_payment_total;
        public decimal ticket_payment_total
        {
            get { return _ticket_payment_total; }
            set { _ticket_payment_total = value; }
        }

        decimal _fee_total;
        public decimal fee_total
        {
            get { return _fee_total; }
            set { _fee_total = value; }
        }

        decimal _fee_payment_total;
        public decimal fee_payment_total
        {
            get { return _fee_payment_total; }
            set { _fee_payment_total = value; }
        }
        string _boarding_time;
        public string boarding_time
        {
            get { return _boarding_time; }
            set { _boarding_time = value; }
        }
        string _boarding_gate;
        public string boarding_gate
        {
            get { return _boarding_gate; }
            set { _boarding_gate = value; }
        }

        string _passport_issue_country_rcd;
        public string passport_issue_country_rcd
        {
            get { return _passport_issue_country_rcd; }
            set { _passport_issue_country_rcd = value; }
        }
        string _passport_issue_country_display_name;
        public string passport_issue_country_display_name
        {
            get { return _passport_issue_country_display_name; }
            set { _passport_issue_country_display_name = value; }
        }

        string _known_traveler_number;
        public string known_traveler_number
        {
            get { return _known_traveler_number; }
            set { _known_traveler_number = value; }
        }
        string _flight_connection_id;
        public string flight_connection_id
        {
            get { return _flight_connection_id; }
            set { _flight_connection_id = value; }
        }

    }
}
