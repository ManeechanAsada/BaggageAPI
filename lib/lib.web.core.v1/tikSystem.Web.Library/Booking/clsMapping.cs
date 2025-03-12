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
    public class Mapping : FlightSegment
    {
        #region Passenger information
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
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
        string _middlename = string.Empty;
        public string middlename
        {
            get { return _middlename; }
            set { _middlename = value; }
        }
        string _document_type_rcd = string.Empty;
        public string document_type_rcd
        {
            get { return _document_type_rcd; }
            set { _document_type_rcd = value; }
        }
        string _passport_number = string.Empty;
        public string passport_number
        {
            get { return _passport_number; }
            set { _passport_number = value; }
        }
       string _passport_issue_place = string.Empty;
        public string passport_issue_place
        {
            get { return _passport_issue_place; }
            set { _passport_issue_place = value; }
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
        string _sendmail = String.Empty;
        public string sendmail
        {
            get { return _sendmail; }
            set { _sendmail = value; }
        }

        long _client_number ;
        public long client_number
        {
            get { return _client_number; }
            set { _client_number = value; }
        }

        Guid _passenger_profile_id ;
        public Guid passenger_profile_id
        {
            get { return _passenger_profile_id; }
            set { _passenger_profile_id = value; }
        }

        Guid _client_profile_id;
        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        string _employee_number;
        public string employee_number
        {
            get { return _employee_number; }
            set { _employee_number = value; }
        }

        string _nationality_rcd = string.Empty;
        public string nationality_rcd
        {
            get { return _nationality_rcd; }
            set { _nationality_rcd = value; }
        }

        string _contact_email = string.Empty;
        public string contact_email
        {
            get { return _contact_email; }
            set { _contact_email = value; }
        }

        #endregion
        #region Ticket information
        string _TicketAndSeatNumber;
        public string TicketAndSeatNumber
        {
            get { return _TicketAndSeatNumber; }
            set { _TicketAndSeatNumber = value; }
        }
        string _inventory_class_rcd;
        public string inventory_class_rcd
        {
            get { return _inventory_class_rcd; }
            set { _inventory_class_rcd = value; }
        }
        string _record_locator;
        public string record_locator
        {
            get { return _record_locator; }
            set { _record_locator = value; }
        }
        string _seat_number = string.Empty;
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
        string _seat_column = string.Empty;
        public string seat_column
        {
            get { return _seat_column; }
            set { _seat_column = value; }
        }
        decimal _net_total;
        public decimal net_total
        {
            get { return _net_total; }
            set { _net_total = value; }
        }
        decimal _tax_amount;
        public decimal tax_amount
        {
            get { return _tax_amount; }
            set { _tax_amount = value; }
        }
        decimal _fare_amount;
        public decimal fare_amount
        {
            get { return _fare_amount; }
            set { _fare_amount = value; }
        }
        decimal _yq_amount;
        public decimal yq_amount
        {
            get { return _yq_amount; }
            set { _yq_amount = value; }
        }
        decimal _base_ticketing_fee_amount;
        public decimal base_ticketing_fee_amount
        {
            get { return _base_ticketing_fee_amount; }
            set { _base_ticketing_fee_amount = value; }
        }
        decimal _ticketing_fee_amount;
        public decimal ticketing_fee_amount
        {
            get { return _ticketing_fee_amount; }
            set { _ticketing_fee_amount = value; }
        }

        decimal _reservation_fee_amount;
        public decimal reservation_fee_amount
        {
            get { return _reservation_fee_amount; }
            set { _reservation_fee_amount = value; }
        }
        decimal _commission_amount;
        public decimal commission_amount
        {
            get { return _commission_amount; }
            set { _commission_amount = value; }
        }
        decimal _fare_vat;
        public decimal fare_vat
        {
            get { return _fare_vat; }
            set { _fare_vat = value; }
        }
        decimal _tax_vat;
        public decimal tax_vat
        {
            get { return _tax_vat; }
            set { _tax_vat = value; }
        }
        decimal _yq_vat;
        public decimal yq_vat
        {
            get { return _yq_vat; }
            set { _yq_vat = value; }
        }
        decimal _ticketing_fee_vat;
        public decimal ticketing_fee_vat
        {
            get { return _ticketing_fee_vat; }
            set { _ticketing_fee_vat = value; }
        }
        decimal _reservation_fee_vat;
        public decimal reservation_fee_vat
        {
            get { return _reservation_fee_vat; }
            set { _reservation_fee_vat = value; }
        }
        decimal _fare_amount_incl;
        public decimal fare_amount_incl
        {
            get { return _fare_amount_incl; }
            set { _fare_amount_incl = value; }
        }
        decimal _tax_amount_incl;
        public decimal tax_amount_incl
        {
            get { return _tax_amount_incl; }
            set { _tax_amount_incl = value; }
        }
        decimal _yq_amount_incl;
        public decimal yq_amount_incl
        {
            get { return _yq_amount_incl; }
            set { _yq_amount_incl = value; }
        }
        decimal _public_fare_amount_incl;
        public decimal public_fare_amount_incl
        {
            get { return _public_fare_amount_incl; }
            set { _public_fare_amount_incl = value; }
        }
        decimal _public_fare_amount;
        public decimal public_fare_amount
        {
            get { return _public_fare_amount; }
            set { _public_fare_amount = value; }
        }
        decimal _commission_amount_incl;
        public decimal commission_amount_incl
        {
            get { return _commission_amount_incl; }
            set { _commission_amount_incl = value; }
        }
        decimal _commission_percentage;
        public decimal commission_percentage
        {
            get { return _commission_percentage; }
            set { _commission_percentage = value; }
        }
        decimal _ticketing_fee_amount_incl;
        public decimal ticketing_fee_amount_incl
        {
            get { return _ticketing_fee_amount_incl; }
            set { _ticketing_fee_amount_incl = value; }
        }
        decimal _reservation_fee_amount_incl;
        public decimal reservation_fee_amount_incl
        {
            get { return _reservation_fee_amount_incl; }
            set { _reservation_fee_amount_incl = value; }
        }
        decimal _acct_net_total;
        public decimal acct_net_total
        {
            get { return _acct_net_total; }
            set { _acct_net_total = value; }
        }
        decimal _acct_tax_amount;
        public decimal acct_tax_amount
        {
            get { return _acct_tax_amount; }
            set { _acct_tax_amount = value; }
        }
        decimal _acct_fare_amount;
        public decimal acct_fare_amount
        {
            get { return _acct_fare_amount; }
            set { _acct_fare_amount = value; }
        }
        decimal _acct_yq_amount;
        public decimal acct_yq_amount
        {
            get { return _acct_yq_amount; }
            set { _acct_yq_amount = value; }
        }
        decimal _acct_ticketing_fee_amount;
        public decimal acct_ticketing_fee_amount
        {
            get { return _acct_ticketing_fee_amount; }
            set { _acct_ticketing_fee_amount = value; }
        }
        decimal _acct_reservation_fee_amount;
        public decimal acct_reservation_fee_amount
        {
            get { return _acct_reservation_fee_amount; }
            set { _acct_reservation_fee_amount = value; }
        }
        decimal _acct_commission_amount;
        public decimal acct_commission_amount
        {
            get { return _acct_commission_amount; }
            set { _acct_commission_amount = value; }
        }
        decimal _acct_fare_amount_incl;
        public decimal acct_fare_amount_incl
        {
            get { return _acct_fare_amount_incl; }
            set { _acct_fare_amount_incl = value; }
        }
        decimal _acct_tax_amount_incl;
        public decimal acct_tax_amount_incl
        {
            get { return _acct_tax_amount_incl; }
            set { _acct_tax_amount_incl = value; }
        }
        decimal _acct_yq_amount_incl;
        public decimal acct_yq_amount_incl
        {
            get { return _acct_yq_amount_incl; }
            set { _acct_yq_amount_incl = value; }
        }
        decimal _acct_commission_amount_incl;
        public decimal acct_commission_amount_incl
        {
            get { return _acct_commission_amount_incl; }
            set { _acct_commission_amount_incl = value; }
        }
        decimal _acct_ticketing_fee_amount_incl;
        public decimal acct_ticketing_fee_amount_incl
        {
            get { return _acct_ticketing_fee_amount_incl; }
            set { _acct_ticketing_fee_amount_incl = value; }
        }
        decimal _acct_reservation_fee_amount_incl;
        public decimal acct_reservation_fee_amount_incl
        {
            get { return _acct_reservation_fee_amount_incl; }
            set { _acct_reservation_fee_amount_incl = value; }
        }
        
        string _fare_code;
        public string fare_code
        {
            get { return _fare_code; }
            set { _fare_code = value; }
        }
        DateTime _fare_date_time;
        public DateTime fare_date_time
        {
            get { return _fare_date_time; }
            set { _fare_date_time = value; }
        }
        
        byte _e_ticket_flag;
        public byte e_ticket_flag
        {
            get { return _e_ticket_flag; }
            set { _e_ticket_flag = value; }
        }
        byte _standby_flag;
        public byte standby_flag
        {
            get { return _standby_flag; }
            set { _standby_flag = value; }
        }
        byte _transferable_fare_flag;
        public byte transferable_fare_flag
        {
            get { return _transferable_fare_flag; }
            set { _transferable_fare_flag = value; }
        }
        string _passenger_check_in_status_rcd;
        public string passenger_check_in_status_rcd
        {
            get { return _passenger_check_in_status_rcd; }
            set { _passenger_check_in_status_rcd = value; }
        }
        string _priority_code;
        public string priority_code
        {
            get { return _priority_code; }
            set { _priority_code = value; }
        }
        string _agency_code;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        
        string _ticket_number = string.Empty;
        public string ticket_number
        {
            get { return _ticket_number; }
            set { _ticket_number = value; }
        }
        DateTime _ticketing_date_time;
        public DateTime ticketing_date_time
        {
            get { return _ticketing_date_time; }
            set { _ticketing_date_time = value; }
        }
        Guid _ticketing_by;
        public Guid ticketing_by
        {
            get { return _ticketing_by; }
            set { _ticketing_by = value; }
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
        Guid _unload_by;
        public Guid unload_by
        {
            get { return _unload_by; }
            set { _unload_by = value; }
        }
        DateTime _unload_date_time;
        public DateTime unload_date_time
        {
            get { return _unload_date_time; }
            set { _unload_date_time = value; }
        }
        string _unload_comment;
        public string unload_comment
        {
            get { return _unload_comment; }
            set { _unload_comment = value; }
        }
        int _number_of_pieces;
        public int number_of_pieces
        {
            get { return _number_of_pieces; }
            set { _number_of_pieces = value; }
        }
        decimal _baggage_weight;
        public decimal baggage_weight
        {
            get { return _baggage_weight; }
            set { _baggage_weight = value; }
        }
        decimal _excess_baggage_weight;
        public decimal excess_baggage_weight
        {
            get { return _excess_baggage_weight; }
            set { _excess_baggage_weight = value; }
        }
        decimal _check_in_baggage_weight;
        public decimal check_in_baggage_weight
        {
            get { return _check_in_baggage_weight; }
            set { _check_in_baggage_weight = value; }
        }
        string _check_in_user_code;
        public string check_in_user_code
        {
            get { return _check_in_user_code; }
            set { _check_in_user_code = value; }
        }
        Guid _check_in_by;
        public Guid check_in_by
        {
            get { return _check_in_by; }
            set { _check_in_by = value; }
        }
        string _check_in_code;
        public string check_in_code
        {
            get { return _check_in_code; }
            set { _check_in_code = value; }
        }
        Guid _cancel_by;
        public Guid cancel_by
        {
            get { return _cancel_by; }
            set { _cancel_by = value; }
        }
        DateTime _exchanged_date_time;
        public DateTime exchanged_date_time
        {
            get { return _exchanged_date_time; }
            set { _exchanged_date_time = value; }
        }
        DateTime _cancel_date_time;
        public DateTime cancel_date_time
        {
            get { return _cancel_date_time; }
            set { _cancel_date_time = value; }
        }
        string _fare_description;
        public string fare_description
        {
            get { return _fare_description; }
            set { _fare_description = value; }
        }
        string _restriction_text;
        public string restriction_text
        {
            get { return _restriction_text; }
            set { _restriction_text = value; }
        }
        string _fare_line;
        public string fare_line
        {
            get { return _fare_line; }
            set { _fare_line = value; }
        }
        string _form_of_payment_rcd;
        public string form_of_payment_rcd
        {
            get { return _form_of_payment_rcd; }
            set { _form_of_payment_rcd = value; }
        }
        string _master_passenger_type_rcd;
        public string master_passenger_type_rcd
        {
            get { return _master_passenger_type_rcd; }
            set { _master_passenger_type_rcd = value; }
        }
        string _endorsement_text;
        public string endorsement_text
        {
            get { return _endorsement_text; }
            set { _endorsement_text = value; }
        }
        byte _exclude_pricing_flag;
        public byte exclude_pricing_flag
        {
            get { return _exclude_pricing_flag; }
            set { _exclude_pricing_flag = value; }
        }
        string _fare_note;
        public string fare_note
        {
            get { return _fare_note; }
            set { _fare_note = value; }
        }
        string _ticketing_name;
        public string ticketing_name
        {
            get { return _ticketing_name; }
            set { _ticketing_name = value; }
        }
        string _create_name;
        public string create_name
        {
            get { return _create_name; }
            set { _create_name = value; }
        }
        string _update_name;
        public string update_name
        {
            get { return _update_name; }
            set { _update_name = value; }
        }
        string _cancel_name;
        public string cancel_name
        {
            get { return _cancel_name; }
            set { _cancel_name = value; }
        }
        DateTime _void_date_time;
        public DateTime void_date_time
        {
            get { return _void_date_time; }
            set { _void_date_time = value; }
        }
        decimal _refund_charge;
        public decimal refund_charge
        {
            get { return _refund_charge; }
            set { _refund_charge = value; }
        }
        decimal _refundable_amount;
        public decimal refundable_amount
        {
            get { return _refundable_amount; }
            set { _refundable_amount = value; }
        }
        DateTime _refund_date_time;
        public DateTime refund_date_time
        {
            get { return _refund_date_time; }
            set { _refund_date_time = value; }
        }
        decimal _payment_amount;
        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }
        int _MappingSort;
        public int MappingSort
        {
            get { return _MappingSort; }
            set { _MappingSort = value; }
        }
        DateTime _check_in_date_time;
        public DateTime check_in_date_time
        {
            get { return _check_in_date_time; }
            set { _check_in_date_time = value; }
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
        string _e_ticket_status;
        public string e_ticket_status
        {
            get { return _e_ticket_status; }
            set { _e_ticket_status = value; }
        }
        byte _hand_luggage_flag;
        public byte hand_luggage_flag
        {
            get { return _hand_luggage_flag; }
            set { _hand_luggage_flag = value; }
        }
        int _hand_number_of_pieces;
        public int hand_number_of_pieces
        {
            get { return _hand_number_of_pieces; }
            set { _hand_number_of_pieces = value; }
        }
        double _hand_baggage_weight;
        public double hand_baggage_weight
        {
            get { return _hand_baggage_weight; }
            set { _hand_baggage_weight = value; }
        }
        byte _free_seating_flag;
        public byte free_seating_flag
        {
            get { return _free_seating_flag; }
            set { _free_seating_flag = value; }
        }
        string _fare_type_rcd = string.Empty;
        public string fare_type_rcd
        {
            get { return _fare_type_rcd; }
            set { _fare_type_rcd = value; }
        }
        double _redemption_points;
        public double redemption_points
        {
            get { return _redemption_points; }
            set { _redemption_points = value; }
        }
        string _seat_fee_rcd = string.Empty;
        public string seat_fee_rcd
        {
            get { return _seat_fee_rcd; }
            set { _seat_fee_rcd = value; }
        }
        Int16 _refund_with_charge_hours;
        public Int16 refund_with_charge_hours
        {
            get { return _refund_with_charge_hours; }
            set { _refund_with_charge_hours = value; }
        }
        Int16 _refund_not_possible_hours;
        public Int16 refund_not_possible_hours
        {
            get { return _refund_not_possible_hours; }
            set { _refund_not_possible_hours = value; }
        }
        byte _duty_travel_flag;
        public byte duty_travel_flag
        {
            get { return _duty_travel_flag; }
            set { _duty_travel_flag = value; }
        }
        DateTime _not_valid_after_date;
        public DateTime not_valid_after_date
        {
            get { return _not_valid_after_date; }
            set { _not_valid_after_date = value; }
        }
        DateTime _not_valid_before_date;
        public DateTime not_valid_before_date
        {
            get { return _not_valid_before_date; }
            set { _not_valid_before_date = value; }
        }
        byte _advanced_seating_flag;
        public byte advanced_seating_flag
        {
            get { return _advanced_seating_flag; }
            set { _advanced_seating_flag = value; }
        }
        byte _fare_column;
        public byte fare_column
        {
            get { return _fare_column; }
            set { _fare_column = value; }
        }
        #endregion
        #region Ignore XML Section
        bool _SeatConfirm = false;
        [XmlIgnoreAttribute]
        public bool SeatConfirm
        {
            get { return _SeatConfirm; }
            set { _SeatConfirm = value; }
        }
        string _temp_seat_fee_rcd = string.Empty;
        [XmlIgnoreAttribute]
        public string temp_seat_fee_rcd
        {
            get { return _temp_seat_fee_rcd; }
            set { _temp_seat_fee_rcd = value; }
        }
        string _temp_seat_number = string.Empty;
        [XmlIgnoreAttribute]
        public string temp_seat_number
        {
            get { return _temp_seat_number; }
            set { _temp_seat_number = value; }
        }
        int _temp_seat_row;
        [XmlIgnoreAttribute]
        public int temp_seat_row
        {
            get { return _temp_seat_row; }
            set { _temp_seat_row = value; }
        }
        string _temp_seat_column = string.Empty;
        [XmlIgnoreAttribute]
        public string temp_seat_column
        {
            get { return _temp_seat_column; }
            set { _temp_seat_column = value; }
        }
        Int16 _piece_allowance;
        public Int16 piece_allowance
        {
            get { return _piece_allowance; }
            set { _piece_allowance = value; }
        }
        int _boarding_time;
        public int boarding_time
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
        byte _it_fare_flag;
        public byte it_fare_flag
        {
            get { return _it_fare_flag; }
            set { _it_fare_flag = value; }
        }
        #endregion
    }
}
