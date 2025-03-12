using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public abstract class People
    {
        #region General Information
        
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
        string _middleName = string.Empty;
        public string middlename
        {
            get { return _middleName; }
            set { _middleName = value; }
        }
        string _language_rcd;
        public string language_rcd
        {
            get { return _language_rcd; }
            set { _language_rcd = value; }
        }
        string _nationality_rcd = string.Empty;
        public string nationality_rcd
        {
            get { return _nationality_rcd; }
            set { _nationality_rcd = value; }
        }
        decimal _passenger_weight;
        public decimal passenger_weight
        {
            get { return _passenger_weight; }
            set { _passenger_weight = value; }
        }
        string _gender_type_rcd;
        public string gender_type_rcd
        {
            get { return _gender_type_rcd; }
            set { _gender_type_rcd = value; }
        }
        string _passenger_type_rcd;
        public string passenger_type_rcd
        {
            get { return _passenger_type_rcd; }
            set { _passenger_type_rcd = value; }
        }
        Guid _client_profile_id;
        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }

        long _client_number = 0;
        public long client_number
        {
            get { return _client_number; }
            set { _client_number = value; }
        }
        #endregion
        #region Address information
        string _address_line1 = string.Empty;
        public string address_line1
        {
            get { return _address_line1; }
            set { _address_line1 = value; }
        }

        string _address_line2 = string.Empty;
        public string address_line2
        {
            get { return _address_line2; }
            set { _address_line2 = value; }
        }
        string _state = string.Empty;
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        string _district = string.Empty;
        public string district
        {
            get { return _district; }
            set { _district = value; }
        }
        string _province = string.Empty;
        public string province
        {
            get { return _province; }
            set { _province = value; }
        }
        string _zip_code = string.Empty;
        public string zip_code
        {
            get { return _zip_code; }
            set { _zip_code = value; }
        }
        string _po_box = string.Empty;
        public string po_box
        {
            get { return _po_box; }
            set { _po_box = value; }
        }
        string _country_rcd = string.Empty;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }
        string _street = string.Empty;
        public string street
        {
            get { return _street; }
            set { _street = value; }
        }
        string _city = string.Empty;
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        #endregion
        #region Document Information
        string _document_type_rcd;
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
        string _residence_country_rcd = string.Empty;
        public string residence_country_rcd
        {
            get { return _residence_country_rcd; }
            set { _residence_country_rcd = value; }
        }
        string _passport_number = string.Empty;
        public string passport_number
        {
            get { return _passport_number; }
            set { _passport_number = value; }
        }

        DateTime _passport_issue_date;
        public DateTime passport_issue_date
        {
            get { return _passport_issue_date; }
            set { _passport_issue_date = value; }
        }

        DateTime _passport_expiry_date;
        public DateTime passport_expiry_date
        {
            get { return _passport_expiry_date; }
            set { _passport_expiry_date = value; }
        }
        string _passport_issue_place = string.Empty;
        public string passport_issue_place
        {
            get { return _passport_issue_place; }
            set { _passport_issue_place = value; }
        }
        string _passport_birth_place;
        public string passport_birth_place
        {
            get { return _passport_birth_place; }
            set { _passport_birth_place = value; }
        }
        DateTime _date_of_birth;
        public DateTime date_of_birth
        {
            get { return _date_of_birth; }
            set { _date_of_birth = value; }
        }
        string _passport_issue_country_rcd = string.Empty;
        public string passport_issue_country_rcd
        {
            get { return _passport_issue_country_rcd; }
            set { _passport_issue_country_rcd = value; }
        }
        #endregion
        #region Contact Information
        string _contact_name = string.Empty;
        public string contact_name
        {
            get { return _contact_name; }
            set { _contact_name = value; }
        }
        string _contact_email = string.Empty;
        public string contact_email
        {
            get { return _contact_email; }
            set { _contact_email = value; }
        }
        string _mobile_email = string.Empty;
        public string mobile_email
        {
            get { return _mobile_email; }
            set { _mobile_email = value; }
        }
        string _phone_mobile = string.Empty;
        public string phone_mobile
        {
            get { return _phone_mobile; }
            set { _phone_mobile = value; }
        }
        string _phone_home = string.Empty;
        public string phone_home
        {
            get { return _phone_home; }
            set { _phone_home = value; }
        }

        string _phone_fax = string.Empty;
        public string phone_fax
        {
            get { return _phone_fax; }
            set { _phone_fax = value; }
        }
        string _phone_business = string.Empty;
        public string phone_business
        {
            get { return _phone_business; }
            set { _phone_business = value; }
        }

        string _received_from = string.Empty;
        public string received_from
        {
            get { return _received_from; }
            set { _received_from = value; }
        }
        #endregion
        #region Update Information
        Guid _create_by;
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
        Guid _update_by;
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
        #endregion


        #region Sort Function
        public int CompareTo(object obj, string Property)
        {
            try
            {
                Type type = this.GetType();
                System.Reflection.PropertyInfo propertie = type.GetProperty(Property);


                Type type2 = obj.GetType();
                System.Reflection.PropertyInfo propertie2 = type2.GetProperty(Property);

                object[] index = null;

                object Obj1 = propertie.GetValue(this, index);
                object Obj2 = propertie2.GetValue(obj, index);

                IComparable Ic1 = (IComparable)Obj1;
                IComparable Ic2 = (IComparable)Obj2;

                int returnValue = Ic1.CompareTo(Ic2);

                return returnValue;

            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
