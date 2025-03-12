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
    public class Flight : Route
    {
        #region "Property"
        Guid _flight_id;
        public Guid flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }
        Guid _exchanged_segment_id;
        public Guid exchanged_segment_id
        {
            get { return _exchanged_segment_id; }
            set { _exchanged_segment_id = value; }
        }
        string _airline_rcd;
        public string airline_rcd
        {
            get { return _airline_rcd; }
            set { _airline_rcd = value; }
        }
        string _airline_name;
        public string airline_name
        {
            get { return _airline_name; }
            set { _airline_name = value; }
        }
        string _flight_number;
        public string flight_number
        {
            get { return _flight_number; }
            set { _flight_number = value; }
        }
        string _operating_airline_rcd;
        public string operating_airline_rcd
        {
            get { return _operating_airline_rcd; }
            set { _operating_airline_rcd = value; }
        }
        string _operating_airline_name;
        public string operating_airline_name
        {
            get { return _operating_airline_name; }
            set { _operating_airline_name = value; }
        }
        string _operating_flight_number;
        public string operating_flight_number
        {
            get { return _operating_flight_number; }
            set { _operating_flight_number = value; }
        }
        byte _refundable_flag;
        public byte refundable_flag
        {
            get { return _refundable_flag; }
            set { _refundable_flag = value; }
        }
        byte _group_flag;
        public byte group_flag
        {
            get { return _group_flag; }
            set { _group_flag = value; }
        }
        byte _non_revenue_flag;
        public byte non_revenue_flag
        {
            get { return _non_revenue_flag; }
            set { _non_revenue_flag = value; }
        }
        byte _eticket_flag;
        public byte eticket_flag
        {
            get { return _eticket_flag; }
            set { _eticket_flag = value; }
        }
        byte _fare_reduction;
        public byte fare_reduction
        {
            get { return _fare_reduction; }
            set { _fare_reduction = value; }
        }
        DateTime _departure_date;
        public DateTime departure_date
        {
            get { return _departure_date; }
            set { _departure_date = value; }
        }
        DateTime _arrival_date;
        public DateTime arrival_date
        {
            get { return _arrival_date; }
            set { _arrival_date = value; }
        }
        string _od_origin_rcd;
        public string od_origin_rcd
        {
            get { return _od_origin_rcd; }
            set { _od_origin_rcd = value; }
        }
        string _od_destination_rcd;
        public string od_destination_rcd
        {
            get { return _od_destination_rcd; }
            set { _od_destination_rcd = value; }
        }
        byte _waitlist_flag;
        public byte waitlist_flag
        {
            get { return _waitlist_flag; }
            set { _waitlist_flag = value; }
        }
        byte _overbook_flag;
        public byte overbook_flag
        {
            get { return _overbook_flag; }
            set { _overbook_flag = value; }
        }
        Guid _flight_connection_id;
        public Guid flight_connection_id
        {
            get { return _flight_connection_id; }
            set { _flight_connection_id = value; }
        }
        byte _advanced_purchase_flag;
        public byte advanced_purchase_flag
        {
            get { return _advanced_purchase_flag; }
            set { _advanced_purchase_flag = value; }
        }
        int _journey_time;
        public int journey_time
        {
            get { return _journey_time; }
            set { _journey_time = value; }
        }
        int _departure_time;
        public int departure_time
        {
            get { return _departure_time; }
            set { _departure_time = value; }
        }
        int _arrival_time;
        public int arrival_time
        {
            get { return _arrival_time; }
            set { _arrival_time = value; }
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
        // Start add new properties for Multi stop
        string _transit_points;
        public string transit_points
        {
            get { return _transit_points; }
            set { _transit_points = value; }
        }
        string _transit_points_name;
        public string transit_points_name
        {
            get { return _transit_points_name; }
            set { _transit_points_name = value; }
        }
        string _transit_airport_rcd;
        public string transit_airport_rcd
        {
            get { return _transit_airport_rcd; }
            set { _transit_airport_rcd = value; }
        }
        string _transit_boarding_class_rcd;
        public string transit_boarding_class_rcd
        {
            get { return _transit_boarding_class_rcd; }
            set { _transit_boarding_class_rcd = value; }
        }
        string _transit_booking_class_rcd;
        public string transit_booking_class_rcd
        {
            get { return _transit_booking_class_rcd; }
            set { _transit_booking_class_rcd = value; }
        }
        Guid _transit_flight_id;
        public Guid transit_flight_id
        {
            get { return _transit_flight_id; }
            set { _transit_flight_id = value; }
        }
        Guid _transit_fare_id;
        public Guid transit_fare_id
        {
            get { return _transit_fare_id; }
            set { _transit_fare_id = value; }
        }
        DateTime _transit_departure_date;
        public DateTime transit_departure_date
        {
            get { return _transit_departure_date; }
            set { _transit_departure_date = value; }
        }
        DateTime _transit_arrival_date;
        public DateTime transit_arrival_date
        {
            get { return _transit_arrival_date; }
            set { _transit_arrival_date = value; }
        }
        // End add new properties for Multi stop
        //Later with the new webservice have to move to flight segment object.
        Guid _fare_id;
        public Guid fare_id
        {
            get { return _fare_id; }
            set { _fare_id = value; }
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
        //Later with the new webservice have to move to flight segment object.
        string _aircraft_type_rcd;
        public string aircraft_type_rcd
        {
            get { return _aircraft_type_rcd; }
            set { _aircraft_type_rcd = value; }
        }
        Int16 _planned_departure_time;
        public Int16 planned_departure_time
        {
            get { return _planned_departure_time; }
            set { _planned_departure_time = value; }
        }
        Int16 _planned_arrival_time;
        public Int16 planned_arrival_time
        {
            get { return _planned_arrival_time; }
            set { _planned_arrival_time = value; }
        }
        byte _departure_dayOfWeek;
        public byte departure_dayOfWeek
        {
            get { return _departure_dayOfWeek; }
            set { _departure_dayOfWeek = value; }
        }
        byte _arrival_dayOfWeek;
        public byte arrival_dayOfWeek
        {
            get { return _arrival_dayOfWeek; }
            set { _arrival_dayOfWeek = value; }
        }
        int _departure_month;
        public int departure_month
        {
            get { return _departure_month; }
            set { _departure_month = value; }
        }

        #endregion
        
    }
}
