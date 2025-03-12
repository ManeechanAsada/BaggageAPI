using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Country
    {
        string _country_rcd;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
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
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        string _vat_display;
        public string vat_display
        {
            get { return _vat_display; }
            set { _vat_display = value; }
        }
        string _country_code_long;
        public string country_code_long
        {
            get { return _country_code_long; }
            set { _country_code_long = value; }
        }
        string _country_number;
        public string country_number
        {
            get { return _country_number; }
            set { _country_number = value; }
        }
        string _phone_prefix;
        public string phone_prefix
        {
            get { return _phone_prefix; }
            set { _phone_prefix = value; }
        }
        byte _issue_country_flag;
        public byte issue_country_flag
        {
            get { return _issue_country_flag; }
            set { _issue_country_flag = value; }
        }
        byte _residence_country_flag;
        public byte residence_country_flag
        {
            get { return _residence_country_flag; }
            set { _residence_country_flag = value; }
        }
        byte _nationality_country_flag;
        public byte nationality_country_flag
        {
            get { return _nationality_country_flag; }
            set { _nationality_country_flag = value; }
        }
        byte _address_country_flag;
        public byte address_country_flag
        {
            get { return _address_country_flag; }
            set { _address_country_flag = value; }
        }
        byte _agency_country_flag;
        public byte agency_country_flag
        {
            get { return _agency_country_flag; }
            set { _agency_country_flag = value; }
        }
    }
}
