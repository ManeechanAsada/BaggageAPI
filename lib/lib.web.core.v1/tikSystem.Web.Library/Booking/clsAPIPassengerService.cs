using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class APIPassengerService
    {
        //checkin API
        #region Property
        Guid _passenger_segment_service_id = Guid.Empty;
        public Guid passenger_segment_service_id
        {
            get { return _passenger_segment_service_id; }
            set { _passenger_segment_service_id = value; }
        }

        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        string _special_service_status_rcd = string.Empty;
        public string special_service_status_rcd
        {
            get { return _special_service_status_rcd; }
            set { _special_service_status_rcd = value; }
        }

        string _special_service_change_status_rcd = string.Empty;
        public string special_service_change_status_rcd
        {
            get { return _special_service_change_status_rcd; }
            set { _special_service_change_status_rcd = value; }
        }

        string _special_service_rcd = string.Empty;
        public string special_service_rcd
        {
            get { return _special_service_rcd; }
            set { _special_service_rcd = value; }
        }

        string _service_text;
        public string service_text
        {
            get { return _service_text; }
            set { _service_text = value; }
        }

        string _flight_id;
        public string flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }

        string _fee_id;
        public string fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }

        Int16 _number_of_units;
        public Int16 number_of_units
        {
            get { return _number_of_units; }
            set { _number_of_units = value; }
        }

        string _origin_rcd = string.Empty;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }

        string _destination_rcd = string.Empty;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }

        string _display_name = string.Empty;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        #endregion
    }


    public class APIPassengerAddress
    {
        //checkin API
        #region Property
        Guid _passenger_address_id = Guid.Empty;
        public Guid passenger_address_id
        {
            get { return _passenger_address_id; }
            set { _passenger_address_id = value; }
        }

        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        Guid _passenger_profile_id = Guid.Empty;
        public Guid passenger_profile_id
        {
            get { return _passenger_profile_id; }
            set { _passenger_profile_id = value; }
        }


        string _address_type_rcd = string.Empty;
        public string address_type_rcd
        {
            get { return _address_type_rcd; }
            set { _address_type_rcd = value; }
        }

        string _passenger_address = string.Empty;
        public string passenger_address
        {
            get { return _passenger_address; }
            set { _passenger_address = value; }
        }

        string _passenger_state = string.Empty;
        public string passenger_state
        {
            get { return _passenger_state; }
            set { _passenger_state = value; }
        }

        string _city = string.Empty;
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        string _zip_Code = string.Empty;
        public string zip_Code
        {
            get { return _zip_Code; }
            set { _zip_Code = value; }
        }

        string _country_rcd = string.Empty;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }

        #endregion
    }


    public class APIPassengerDocument
    {
        //checkin API
        #region Property
        Guid _document_id = Guid.Empty;
        public Guid document_id
        {
            get { return _document_id; }
            set { _document_id = value; }
        }

        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        string _document_type_rcd = string.Empty;
        public string document_type_rcd
        {
            get { return _document_type_rcd; }
            set { _document_type_rcd = value; }
        }

        string _document_number = string.Empty;
        public string document_number
        {
            get { return _document_number; }
            set { _document_number = value; }
        }

        string _issue_place = string.Empty;
        public string issue_place
        {
            get { return _issue_place; }
            set { _issue_place = value; }
        }

        string _issue_country = string.Empty;
        public string issue_country
        {
            get { return _issue_country; }
            set { _issue_country = value; }
        }

        string _birth_place = string.Empty;
        public string birth_place
        {
            get { return _birth_place; }
            set { _birth_place = value; }
        }

        string _nationality_rcd = string.Empty;
        public string nationality_rcd
        {
            get { return _nationality_rcd; }
            set { _nationality_rcd = value; }
        }

        string _status_code = string.Empty;
        public string status_code
        {
            get { return _status_code; }
            set { _status_code = value; }
        }

        DateTime _issue_date;
        public DateTime issue_date
        {
            get { return _issue_date; }
            set { _issue_date = value; }
        }

        private DateTime _expiry_date;
        public DateTime expiry_date
        {
            get { return _expiry_date; }
            set { _expiry_date = value; }
        }


        #endregion
    }

    public class APIPassengerBaggage
    {
        //checkin API
        #region Property
        Guid _baggage_id = Guid.Empty;
        public Guid baggage_id
        {
            get { return _baggage_id; }
            set { _baggage_id = value; }
        }

        Guid _booking_id = Guid.Empty;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }

        Guid _passenger_id = Guid.Empty;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }

        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        string _title_rcd = string.Empty;
        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }
        string _firstname = string.Empty;
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        string _lastname = string.Empty;
        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }


        string _airline_rcd = string.Empty;
        public string airline_rcd
        {
            get { return _airline_rcd; }
            set { _airline_rcd = value; }
        }

        string _flight_number = string.Empty;
        public string flight_number
        {
            get { return _flight_number; }
            set { _flight_number = value; }
        }

        string _origin_rcd = string.Empty;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }

        string _destination_rcd = string.Empty;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }

        DateTime _departure_date;
        public DateTime departure_date
        {
            get { return _departure_date; }
            set { _departure_date = value; }
        }


        string _baggage_type_rcd = string.Empty;
        public string baggage_type_rcd
        {
            get { return _baggage_type_rcd; }
            set { _baggage_type_rcd = value; }
        }

        string _baggage_status_rcd = string.Empty;
        public string baggage_status_rcd
        {
            get { return _baggage_status_rcd; }
            set { _baggage_status_rcd = value; }
        }

        string _baggage_tag = string.Empty;
        public string baggage_tag
        {
            get { return _baggage_tag; }
            set { _baggage_tag = value; }
        }
        //baggage_weight
        string _baggage_weight = string.Empty;
        public string baggage_weight
        {
            get { return _baggage_weight; }
            set { _baggage_weight = value; }
        }

        string _baggage_weight_unit = string.Empty;
        public string baggage_weight_unit
        {
            get { return _baggage_weight_unit; }
            set { _baggage_weight_unit = value; }
        }

        Guid _create_by = Guid.Empty;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }

        DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }

        Guid _update_by = Guid.Empty;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }

        DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }

        string _record_locator = string.Empty;
        public string record_locator
        {
            get { return _record_locator; }
            set { _record_locator = value; }
        }

        string _bagtag_print_status = string.Empty;
        public string bagtag_print_status
        {
            get { return _bagtag_print_status; }
            set { _bagtag_print_status = value; }
        }

        string _number_of_bagtag_print = string.Empty;
        public string number_of_bagtag_print
        {
            get { return _number_of_bagtag_print; }
            set { _number_of_bagtag_print = value; }
        }

        //checkin_sequence_number	
        string _checkin_sequence_number = string.Empty;
        public string checkin_sequence_number
        {
            get { return _checkin_sequence_number; }
            set { _checkin_sequence_number = value; }
        }
        #endregion
    }


}
