using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIFlightSegment
    {
        #region "Property"
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        Guid _flight_id;
        public Guid flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }

        string _flight_connection_id;////Guid
        public string flight_connection_id
        {
            get { return _flight_connection_id; }
            set { _flight_connection_id = value; }
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

        string _departure_time;
        public string departure_time
        {
            get { return _departure_time; }
            set { _departure_time = value; }
        }

        string _booking_class_rcd;
        public string booking_class_rcd
        {
            get { return _booking_class_rcd; }
            set { _booking_class_rcd = value; }
        }

        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }

        string _segment_status_rcd;
        public string segment_status_rcd
        {
            get { return _segment_status_rcd; }
            set { _segment_status_rcd = value; }
        }

        Guid _booking_id;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }

        int _number_of_units;
        public int number_of_units
        {
            get { return _number_of_units; }
            set { _number_of_units = value; }
        }

        int _journey_time;
        public int journey_time
        {
            get { return _journey_time; }
            set { _journey_time = value; }
        }

        string _arrival_time;
        public string arrival_time
        {
            get { return _arrival_time; }
            set { _arrival_time = value; }
        }
        DateTime _arrival_date;
        public DateTime arrival_date
        {
            get { return _arrival_date; }
            set { _arrival_date = value; }
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

        string _segment_change_status_rcd;
        public string segment_change_status_rcd
        {
            get { return _segment_change_status_rcd; }
            set { _segment_change_status_rcd = value; }
        }

        byte _info_segment_flag;
        public byte info_segment_flag
        {
            get { return _info_segment_flag; }
            set { _info_segment_flag = value; }
        }

        byte _high_priority_waitlist_flag;
        public byte high_priority_waitlist_flag
        {
            get { return _high_priority_waitlist_flag; }
            set { _high_priority_waitlist_flag = value; }
        }

        string _od_origin_rcd;
        public string od_origin_rcd
        {
            get { return _od_origin_rcd; }
            set { _od_origin_rcd = value; }
        }

        string _flight_check_in_status_rcd;
        public string flight_check_in_status_rcd
        {
            get { return _flight_check_in_status_rcd; }
            set { _flight_check_in_status_rcd = value; }
        }

        string _od_destination_rcd;
        public string od_destination_rcd
        {
            get { return _od_destination_rcd; }
            set { _od_destination_rcd = value; }
        }

        string _origin_name;
        public string origin_name
        {
            get { return _origin_name; }
            set { _origin_name = value; }
        }

        string _destination_name;
        public string destination_name
        {
            get { return _destination_name; }
            set { _destination_name = value; }
        }

        string _segment_status_name;
        public string segment_status_name
        {
            get { return _segment_status_name; }
            set { _segment_status_name = value; }
        }

        byte _seatmap_flag;
        public byte seatmap_flag
        {
            get { return _seatmap_flag; }
            set { _seatmap_flag = value; }
        }

        byte _allow_web_checkin_flag;
        public byte allow_web_checkin_flag
        {
            get { return _allow_web_checkin_flag; }
            set { _allow_web_checkin_flag = value; }
        }

        string _transit_points = string.Empty;
        public string transit_points
        {
            get { return _transit_points; }
            set { _transit_points = value; }
        }

        string _transit_points_name = string.Empty;
        public string transit_points_name
        {
            get { return _transit_points_name; }
            set { _transit_points_name = value; }
        }

        //routeconfig
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        decimal _excess_baggage_charge_amount;
        public decimal excess_baggage_charge_amount
        {
            get { return _excess_baggage_charge_amount; }
            set { _excess_baggage_charge_amount = value; }
        }

        int _nautical_miles;
        public int nautical_miles
        {
            get { return _nautical_miles; }
            set { _nautical_miles = value; }
        }

        string _flight_information_1;
        public string flight_information_1
        {
            get { return _flight_information_1; }
            set { _flight_information_1 = value; }
        }

        string _flight_information_2;
        public string flight_information_2
        {
            get { return _flight_information_2; }
            set { _flight_information_2 = value; }
        }

        string _flight_information_3;
        public string flight_information_3
        {
            get { return _flight_information_3; }
            set { _flight_information_3 = value; }
        }

        int _min_transit_minutes;
        public int min_transit_minutes
        {
            get { return _min_transit_minutes; }
            set { _min_transit_minutes = value; }
        }

        int _max_transit_minutes;
        public int max_transit_minutes
        {
            get { return _max_transit_minutes; }
            set { _max_transit_minutes = value; }
        }

        string _close_web_check_in;
        public string close_web_check_in
        {
            get { return _close_web_check_in; }
            set { _close_web_check_in = value; }
        }

        Int16 _paper_ticket_warning_flag = 0;
        public Int16 paper_ticket_warning_flag
        {
            get { return _paper_ticket_warning_flag; }
            set { _paper_ticket_warning_flag = value; }
        }

        Int16 _require_ticket_number_flag = 0;
        public Int16 require_ticket_number_flag
        {
            get { return _require_ticket_number_flag; }
            set { _require_ticket_number_flag = value; }
        }

        Int16 _require_passenger_title_flag = 0;
        public Int16 require_passenger_title_flag
        {
            get { return _require_passenger_title_flag; }
            set { _require_passenger_title_flag = value; }
        }

        Int16 _require_passenger_gender_flag = 0;
        public Int16 require_passenger_gender_flag
        {
            get { return _require_passenger_gender_flag; }
            set { _require_passenger_gender_flag = value; }
        }

        Int16 _require_date_of_birth_flag = 0;
        public Int16 require_date_of_birth_flag
        {
            get { return _require_date_of_birth_flag; }
            set { _require_date_of_birth_flag = value; }
        }

        Int16 _require_document_details_flag = 0;
        public Int16 require_document_details_flag
        {
            get { return _require_document_details_flag; }
            set { _require_document_details_flag = value; }
        }

        Int16 _require_passenger_weight_flag = 0;
        public Int16 require_passenger_weight_flag
        {
            get { return _require_passenger_weight_flag; }
            set { _require_passenger_weight_flag = value; }
        }

        Int16 _require_open_status_flag = 0;
        public Int16 require_open_status_flag
        {
            get { return _require_open_status_flag; }
            set { _require_open_status_flag = value; }
        }

        Int16 _show_redress_number_flag = 0;
        public Int16 show_redress_number_flag
        {
            get { return _show_redress_number_flag; }
            set { _show_redress_number_flag = value; }
        }
        #endregion
    }
}
