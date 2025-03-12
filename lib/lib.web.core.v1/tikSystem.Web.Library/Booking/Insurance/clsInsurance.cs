using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Insurance
    {   
        #region Property
        string _policy_number;
        public string policy_number
        {
            get { return _policy_number; }
            set { _policy_number = value; }
        }
        string _title_rcd = string.Empty;
        public string title_rcd
        {
            get { return _title_rcd; }
            set { _title_rcd = value; }
        }
        string _lastname = string.Empty;
        public string lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        string _firstname = string.Empty;
        public string firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        string _flight_Type;
        public string flight_Type
        {
            get { return _flight_Type; }
            set { _flight_Type = value; }
        }
        string _flight_trip;
        public string flight_trip
        {
            get { return _flight_trip; }
            set { _flight_trip = value; }
        }
        Fee _Fee;
        public Fee Fee
        {
            get { return _Fee; }
            set { _Fee = value; }
        }
        // ACEQuotePassenger
        private ACEQuotePassengers _ACEQuotePassengers;
        public ACEQuotePassengers ACEQuotePassengers
        {
            get{ return _ACEQuotePassengers;}
            set{  _ACEQuotePassengers = value;}
        }

        #endregion
        
        #region Response Status
        string _error_code;
        public string error_code
        {
            get { return _error_code; }
            set { _error_code = value; }
        }
        string _error_message;
        public string error_message
        {
            get { return _error_message; }
            set { _error_message = value; }
        }
        bool _fee_found;
        public bool fee_found
        {
            get { return _fee_found; }
            set { _fee_found = value; }
        }
        #endregion
    }
}
