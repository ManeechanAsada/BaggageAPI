using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class APIPassengerUpdateRequest
    {
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        string _gender_type_rcd;
        public string gender_type_rcd
        {
            get { return _gender_type_rcd; }
            set { _gender_type_rcd = value; }
        }
        DateTime _date_of_birth;
        public DateTime date_of_birth
        {
            get { return _date_of_birth; }
            set { _date_of_birth = value; }
        }
        string _nationality_rcd = string.Empty;
        public string nationality_rcd
        {
            get { return _nationality_rcd; }
            set { _nationality_rcd = value; }
        }
        string _passport_issue_country_rcd = string.Empty;
        public string passport_issue_country_rcd
        {
            get { return _passport_issue_country_rcd; }
            set { _passport_issue_country_rcd = value; }
        }
        DateTime _passport_expiry_date;
        public DateTime passport_expiry_date
        {
            get { return _passport_expiry_date; }
            set { _passport_expiry_date = value; }
        }
        string _passport_number = string.Empty;
        public string passport_number
        {
            get { return _passport_number; }
            set { _passport_number = value; }
        }

        //18-04-2013 : add document type
        string _document_type_rcd = string.Empty;
        public string document_type_rcd
        {
            get { return _document_type_rcd; }
            set { _document_type_rcd = value; }
        }

        string _known_traveler_number;
        public string known_traveler_number
        {
            get { return _known_traveler_number; }
            set { _known_traveler_number = value; }
        }

    }

}
