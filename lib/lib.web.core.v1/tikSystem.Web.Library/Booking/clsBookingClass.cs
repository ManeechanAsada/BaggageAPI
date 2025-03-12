using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class BookingClass
    {
        string _booking_class_rcd;
        public string booking_class_rcd
        {
            get { return _booking_class_rcd; }
            set { _booking_class_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }
        int _sort_sequence;
        public int sort_sequence
        {
            get { return _sort_sequence; }
            set { _sort_sequence = value; }
        }
        string _nesting_string;
        public string nesting_string
        {
            get { return _nesting_string; }
            set { _nesting_string = value; }
        }
        decimal _sales_notification_percentage;
        public decimal sales_notification_percentage
        {
            get { return _sales_notification_percentage; }
            set { _sales_notification_percentage = value; }
        }
        decimal _cancel_notification_percentage;
        public decimal cancel_notification_percentage
        {
            get { return _cancel_notification_percentage; }
            set { _cancel_notification_percentage = value; }
        }
        byte _controlling_class_flag;
        public byte controlling_class_flag
        {
            get { return _controlling_class_flag; }
            set { _controlling_class_flag = value; }
        }
        int _length_nesting_string;
        public int length_nesting_string
        {
            get { return _length_nesting_string; }
            set { _length_nesting_string = value; }
        }
    }
}
