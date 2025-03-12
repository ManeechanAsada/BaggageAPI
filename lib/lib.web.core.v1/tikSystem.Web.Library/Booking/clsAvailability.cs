using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    public class Availability : FlightSegment
    {
        #region Flight Information
        bool _returnbound_flight;
        public bool returnbound_flight
        {
            get { return _returnbound_flight; }
            set { _returnbound_flight = value; }
        }
        string _nesting_string;
        public string nesting_string
        {
            get { return _nesting_string; }
            set { _nesting_string = value; }
        }
        
        byte _close_web_sales;
        public byte close_web_sales
        {
            get { return _close_web_sales; }
            set { _close_web_sales = value; }
        }
        
        decimal _adult_fare;
        public decimal adult_fare
        {
            get { return _adult_fare; }
            set { _adult_fare = value; }
        }
        decimal _child_fare;
        public decimal child_fare
        {
            get { return _child_fare; }
            set { _child_fare = value; }
        }
        decimal _infant_fare;
        public decimal infant_fare
        {
            get { return _infant_fare; }
            set { _infant_fare = value; }
        }
        decimal _other_fare;
        public decimal other_fare
        {
            get { return _other_fare; }
            set { _other_fare = value; }
        }
        decimal _total_adult_fare;
        public decimal total_adult_fare
        {
            get { return _total_adult_fare; }
            set { _total_adult_fare = value; }
        }
        decimal _total_child_fare;
        public decimal total_child_fare
        {
            get { return _total_child_fare; }
            set { _total_child_fare = value; }
        }
        decimal _total_infant_fare;
        public decimal total_infant_fare
        {
            get { return _total_infant_fare; }
            set { _total_infant_fare = value; }
        }
        decimal _total_other_fare;
        public decimal total_other_fare
        {
            get { return _total_other_fare; }
            set { _total_other_fare = value; }
        }
        byte _fare_column;
        public byte fare_column
        {
            get { return _fare_column; }
            set { _fare_column = value; }
        }
        string _flight_comment;
        public string flight_comment
        {
            get { return _flight_comment; }
            set { _flight_comment = value; }
        }
        
        string _restriction_text;
        public string restriction_text
        {
            get { return _restriction_text; }
            set { _restriction_text = value; }
        }
        string _endorsement_text;
        public string endorsement_text
        {
            get { return _endorsement_text; }
            set { _endorsement_text = value; }
        }
        string _fare_type_rcd;
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
        decimal _transit_redemption_points;
        public decimal transit_redemption_points
        {
            get { return _transit_redemption_points; }
            set { _transit_redemption_points = value; }
        }
        Int16 _flight_duration;
        public Int16 flight_duration
        {
            get { return _flight_duration; }
            set { _flight_duration = value; }
        }
        string _promotion_code;
        public string promotion_code
        {
            get { return _promotion_code; }
            set { _promotion_code = value; }
        }
        int _nested_book_available;
        public int nested_book_available
        {
            get { return _nested_book_available; }
            set { _nested_book_available = value; }
        }
        string _flight_information_1;
        public string flight_information_1
        {
            get { return _flight_information_1; }
            set { _flight_information_1 = value; }
        }
        
        #endregion
        #region transit information
        Int16 _transit_planned_departure_time;
        public Int16 transit_planned_departure_time
        {
            get { return _transit_planned_departure_time; }
            set { _transit_planned_departure_time = value; }
        }
        Int16 _transit_planned_arrival_time;
        public Int16 transit_planned_arrival_time
        {
            get { return _transit_planned_arrival_time; }
            set { _transit_planned_arrival_time = value; }
        }
        string _transit_name;
        public string transit_name
        {
            get { return _transit_name; }
            set { _transit_name = value; }
        }
        string _transit_flight_comment;
        public string transit_flight_comment
        {
            get { return _transit_flight_comment; }
            set { _transit_flight_comment = value; }
        }
        string _transit_airline_rcd;
        public string transit_airline_rcd
        {
            get { return _transit_airline_rcd; }
            set { _transit_airline_rcd = value; }
        }
        string _transit_flight_number;
        public string transit_flight_number
        {
            get { return _transit_flight_number; }
            set { _transit_flight_number = value; }
        }
        string _transit_flight_status_rcd;
        public string transit_flight_status_rcd
        {
            get { return _transit_flight_status_rcd; }
            set { _transit_flight_status_rcd = value; }
        }
        Int16 _transit_flight_duration;
        public Int16 transit_flight_duration
        {
            get { return _transit_flight_duration; }
            set { _transit_flight_duration = value; }
        }
        string _transit_aircraft_type_rcd;
        public string transit_aircraft_type_rcd
        {
            get { return _transit_aircraft_type_rcd; }
            set { _transit_aircraft_type_rcd = value; }
        }
        string _transit_nested_book_available;
        public string transit_nested_book_available
        {
            get { return _transit_nested_book_available; }
            set { _transit_nested_book_available = value; }
        }
        byte _transit_waitlist_open_flag;
        public byte transit_waitlist_open_flag
        {
            get { return _transit_waitlist_open_flag; }
            set { _transit_waitlist_open_flag = value; }
        }
        decimal _transit_adult_fare;
        public decimal transit_adult_fare
        {
            get { return _transit_adult_fare; }
            set { _transit_adult_fare = value; }
        }
        byte _transit_class_open_flag
        {
            get { return _transit_class_open_flag; }
            set { _transit_class_open_flag = value; }
        }
        #endregion
        #region Flag inforamtion
        byte _full_flight_flag;
        public byte full_flight_flag
        {
            get { return _full_flight_flag; }
            set { _full_flight_flag = value; }
        }
        byte _class_open_flag;
        public byte class_open_flag
        {
            get { return _class_open_flag; }
            set { _class_open_flag = value; }
        }
        byte _waitlist_open_flag;
        public byte waitlist_open_flag
        {
            get { return _waitlist_open_flag; }
            set { _waitlist_open_flag = value; }
        }
        Int16 _filter_logic_flag;
        public Int16 filter_logic_flag
        {
            get { return _filter_logic_flag; }
            set { _filter_logic_flag = value; }
        }
        byte _corporate_fare_flag;
        public byte corporate_fare_flag
        {
            get { return _corporate_fare_flag; }
            set { _corporate_fare_flag = value; }
        }
        byte _refundable_flag;
        public byte refundable_flag
        {
            get { return _refundable_flag; }
            set { _refundable_flag = value; }
        }
        #endregion
    }
}
