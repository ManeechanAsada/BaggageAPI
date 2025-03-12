using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIRouteConfig
    {
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
    }
}
