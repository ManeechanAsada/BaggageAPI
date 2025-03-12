using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tikSystem.Web.Library
{
    public class VoucherTemplate
    {
        Guid _voucher_template_id;
        public Guid voucher_template_id
        {
            get { return _voucher_template_id; }
            set { _voucher_template_id = value; }
        }

        string _form_of_payment_rcd = string.Empty;
        public string form_of_payment_rcd
        {
            get { return _form_of_payment_rcd; }
            set { _form_of_payment_rcd = value; }
        }

        string _form_of_payment_subtype_rcd;
        public string form_of_payment_subtype_rcd
        {
            get { return _form_of_payment_subtype_rcd; }
            set { _form_of_payment_subtype_rcd = value; }
        }

        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        
        DateTime _valid_to_date;
        public DateTime valid_to_date
        {
            get { return _valid_to_date; }
            set { _valid_to_date = value; }
        }

        int _valid_days;
        public int valid_days
        {
            get { return _valid_days; }
            set { _valid_days = value; }
        }

        DateTime _valid_from_date;
        public DateTime valid_from_date
        {
            get { return _valid_from_date; }
            set { _valid_from_date = value; }
        }

        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }

        string _status_code;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }

        string _origins;
        public string origins
        {
            get { return _origins; }
            set { _origins = value; }
        }
        string _destinations;
        public string destinations
        {
            get { return _destinations; }
            set { _destinations = value; }
        }

        Int16 _passenger_segments;
        public Int16 passenger_segments
        {
            get { return _passenger_segments; }
            set { _passenger_segments = value; }
        }

        string _voucher_use_code;
        public string voucher_use_code
        {
            get { return _voucher_use_code; }
            set { _voucher_use_code = value; }
        }

        byte _recipient_only_flag;
        public byte recipient_only_flag
        {
            get { return _recipient_only_flag; }
            set { _recipient_only_flag = value; }
        }

        decimal _discount_percentage;
        public decimal discount_percentage
        {
            get { return _discount_percentage; }
            set { _discount_percentage = value; }
        }

        string _valid_for_class;
        public string valid_for_class
        {
            get { return _valid_for_class; }
            set { _valid_for_class = value; }
        }

        byte _ticket_flag;
        public byte ticket_flag
        {
            get { return _ticket_flag; }
            set { _ticket_flag = value; }
        }

        byte _seat_fee_flag;
        public byte seat_fee_flag
        {
            get { return _seat_fee_flag; }
            set { _seat_fee_flag = value; }
        }

        byte _other_fee_flag;
        public byte other_fee_flag
        {
            get { return _other_fee_flag; }
            set { _other_fee_flag = value; }
        }

        byte _b2c_flag;
        public byte b2c_flag
        {
            get { return _b2c_flag; }
            set { _b2c_flag = value; }
        }

        byte _b2b_flag;
        public byte b2b_flag
        {
            get { return _b2b_flag; }
            set { _b2b_flag = value; }
        }

        byte _b2e_flag;
        public byte b2e_flag
        {
            get { return _b2e_flag; }
            set { _b2e_flag = value; }
        }

        byte _airline_flag;
        public byte airline_flag
        {
            get { return _airline_flag; }
            set { _airline_flag = value; }
        }

        byte _fare_only_flag;
        public byte fare_only_flag
        {
            get { return _fare_only_flag; }
            set { _fare_only_flag = value; }
        }
        
        decimal _voucher_value;
        public decimal voucher_value
        {
            get { return _voucher_value; }
            set { _voucher_value = value; }
        }

        decimal _charge_amount;
        public decimal charge_amount
        {
            get { return _charge_amount; }
            set { _charge_amount = value; }
        }

        byte _multiple_payment_flag;
        public byte multiple_payment_flag
        {
            get { return _multiple_payment_flag; }
            set { _multiple_payment_flag = value; }
        }
        
    }
}
