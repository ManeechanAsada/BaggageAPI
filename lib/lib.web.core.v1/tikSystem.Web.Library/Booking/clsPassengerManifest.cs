using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class PassengerManifest : Passenger
    {

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
        
        string _contact_email;
        public string contact_email
        {
            get { return _contact_email; }
            set { _contact_email = value; }
        }

        string _group_name;
        public string group_name
        {
            get { return _group_name; }
            set { _group_name = value; }
        }

        byte _group_booking_flag;
        public byte group_booking_flag
        {
            get { return _group_booking_flag; }
            set { _group_booking_flag = value; }
        }
        string _comment;
        public string comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        string _booking_segment_id;
        public string booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
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
        DateTime _departure_date;
        public DateTime departure_date
        {
            get { return _departure_date; }
            set { _departure_date = value; }
        }

        int _departure_time;
        public int departure_time
        {
            get { return _departure_time; }
            set { _departure_time = value; }
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

        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }

        string _booking_class_rcd;
        public string booking_class_rcd
        {
            get { return _booking_class_rcd; }
            set { _booking_class_rcd = value; }
        }


        string _inventory_class_rcd;
        public string inventory_class_rcd
        {
            get { return _inventory_class_rcd; }
            set { _inventory_class_rcd = value; }
        }

        string _segment_status_rcd;
        public string segment_status_rcd
        {
            get { return _segment_status_rcd; }
            set { _segment_status_rcd = value; }
        }
        Guid _fare_id;
        public Guid fare_id
        {
            get { return _fare_id; }
            set { _fare_id = value; }
        }

        decimal _net_total;
        public decimal net_total
        {
            get { return _net_total; }
            set { _net_total = value; }
        }
        decimal _fare_amount;
        public decimal fare_amount
        {
            get { return _fare_amount; }
            set { _fare_amount = value; }
        }

        DateTime _fare_date_time;
        public DateTime fare_date_time
        {
            get { return _fare_date_time; }
            set { _fare_date_time = value; }
        }
        string _fare_type_rcd;
        public string fare_type_rcd
        {
            get { return _fare_type_rcd; }
            set { _fare_type_rcd = value; }
        }

        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        string _ticket_number;
        public string ticket_number
        {
            get { return _ticket_number; }
            set { _ticket_number = value; }
        }
        byte _e_ticket_flag;
        public byte e_ticket_flag
        {
            get { return _e_ticket_flag; }
            set { _e_ticket_flag = value; }
        }

        Guid _ticket_id;
        public Guid ticket_id
        {
            get { return _ticket_id; }
            set { _ticket_id = value; }
        }
        string _seat_number = string.Empty;
        public string seat_number
        {
            get { return _seat_number; }
            set { _seat_number = value; }
        }

        decimal _baggage_weight;
        public decimal baggage_weight
        {
            get { return _baggage_weight; }
            set { _baggage_weight = value; }
        }
        Int16 _piece_allowance;
        public Int16 piece_allowance
        {
            get { return _piece_allowance; }
            set { _piece_allowance = value; }
        }

        decimal _check_in_baggage_weight;
        public decimal check_in_baggage_weight
        {
            get { return _check_in_baggage_weight; }
            set { _check_in_baggage_weight = value; }
        }

        double _hand_baggage_weight;
        public double hand_baggage_weight
        {
            get { return _hand_baggage_weight; }
            set { _hand_baggage_weight = value; }
        }

        string _coupon_number = string.Empty;
        public string coupon_number
        {
            get { return _coupon_number; }
            set { _coupon_number = value; }
        }

    }
}
