using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Currency
    {
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        decimal _max_voucher_value;
        public decimal max_voucher_value
        {
            get { return _max_voucher_value; }
            set { _max_voucher_value = value; }
        }
        decimal _rounding_rule;
        public decimal rounding_rule
        {
            get { return _rounding_rule; }
            set { _rounding_rule = value; }
        }
        Int16 _number_of_decimals;
        public Int16 number_of_decimals
        {
            get { return _number_of_decimals; }
            set { _number_of_decimals = value; }
        }
        string _currency_number;
        public string currency_number
        {
            get { return _currency_number; }
            set { _currency_number = value; }
        }
        string _display_code;
        public string display_code
        {
            get { return _display_code; }
            set { _display_code = value; }
        }
    }
}
