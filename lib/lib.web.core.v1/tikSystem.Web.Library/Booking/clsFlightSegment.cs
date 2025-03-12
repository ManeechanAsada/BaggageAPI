using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace tikSystem.Web.Library
{
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "",
                 IsNullable = false)] 
    public class FlightSegment : Flight
    {
        #region "Property"

        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
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
        string _marketing_airline_rcd;
        public string marketing_airline_rcd
        {
            get { return _marketing_airline_rcd; }
            set { _marketing_airline_rcd = value; }
        }
        string _marketing_flight_number;
        public string marketing_flight_number
        {
            get { return _marketing_flight_number; }
            set { _marketing_flight_number = value; }
        }
        string _flight_check_in_status_rcd;
        public string flight_check_in_status_rcd
        {
            get { return _flight_check_in_status_rcd; }
            set { _flight_check_in_status_rcd = value; }
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
        byte _temp_seatmap_flag;
        public byte temp_seatmap_flag
        {
            get { return _temp_seatmap_flag; }
            set { _temp_seatmap_flag = value; }
        }
        byte _allow_web_checkin_flag;
        public byte allow_web_checkin_flag
        {
            get { return _allow_web_checkin_flag; }
            set { _allow_web_checkin_flag = value; }
        }
        Int16 _close_web_sales_flag = 0;
        public Int16 close_web_sales_flag
        {
            get { return _close_web_sales_flag; }
            set { _close_web_sales_flag = value; }
        }
        Int16 _exclude_quote_flag = 0;
        public Int16 exclude_quote_flag
        {
            get { return _exclude_quote_flag; }
            set { _exclude_quote_flag = value; }
        }
        double _currency_rate;
        public double currency_rate
        {
            get { return _currency_rate; }
            set { _currency_rate = value; }
        }
        byte _open_sequence;
        public byte open_sequence
        {
            get { return _open_sequence; }
            set { _open_sequence = value; }
        }
        byte _number_of_stops;
        public byte number_of_stops
        {
            get { return _number_of_stops; }
            set { _number_of_stops = value; }
        }
        string _status_name;
        public string status_name
        {
            get { return _status_name; }
            set { _status_name = value; }
        }
        string _origin_terminal;
        public string origin_terminal
        {
            get { return _origin_terminal; }
            set { _origin_terminal = value; }
        }
        string _destination_terminal;
        public string destination_terminal
        {
            get { return _destination_terminal; }
            set { _destination_terminal = value; }
        }
        string _origin_gate;
        public string origin_gate
        {
            get { return _origin_gate; }
            set { _origin_gate = value; }
        }
        string _destination_gate;
        public string destination_gate
        {
            get { return _destination_gate; }
            set { _destination_gate = value; }
        }
        #endregion
    }
}
