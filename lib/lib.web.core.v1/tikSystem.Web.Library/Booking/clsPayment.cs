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
    public class Payment
    {
        Guid _booking_payment_id;
        public Guid booking_payment_id
        {
            get { return _booking_payment_id; }
            set { _booking_payment_id = value; }
        }
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        Guid _booking_id;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        Guid _voucher_payment_id;
        public Guid voucher_payment_id
        {
            get { return _voucher_payment_id; }
            set { _voucher_payment_id = value; }
        }
        string _form_of_payment_rcd = string.Empty;
        public string form_of_payment_rcd
        {
            get { return _form_of_payment_rcd; }
            set { _form_of_payment_rcd = value; }
        }
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        string _receive_currency_rcd;
        public string receive_currency_rcd
        {
            get { return _receive_currency_rcd; }
            set { _receive_currency_rcd = value; }
        }
        string _agency_payment_type_rcd;
        public string agency_payment_type_rcd
        {
            get { return _agency_payment_type_rcd; }
            set { _agency_payment_type_rcd = value; }
        }
        string _agency_code = string.Empty;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        string _debit_agency_code = string.Empty;
        public string debit_agency_code
        {
            get { return _debit_agency_code; }
            set { _debit_agency_code = value; }
        }
        decimal _payment_amount;
        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }
        decimal _receive_payment_amount;
        public decimal receive_payment_amount
        {
            get { return _receive_payment_amount; }
            set { _receive_payment_amount = value; }
        }
        decimal _acct_payment_amount;
        public decimal acct_payment_amount
        {
            get { return _acct_payment_amount; }
            set { _acct_payment_amount = value; }
        }
        Guid _payment_by;
        public Guid payment_by
        {
            get { return _payment_by; }
            set { _payment_by = value; }
        }
        DateTime _payment_date_time;
        public DateTime payment_date_time
        {
            get { return _payment_date_time; }
            set { _payment_date_time = value; }
        }
        DateTime _payment_due_date_time;
        public DateTime payment_due_date_time
        {
            get { return _payment_due_date_time; }
            set { _payment_due_date_time = value; }
        }
        decimal _document_amount;
        public decimal document_amount
        {
            get { return _document_amount; }
            set { _document_amount = value; }
        }
        Guid _void_by = Guid.Empty;
        public Guid void_by
        {
            get { return _void_by; }
            set { _void_by = value; }
        }
        DateTime _valid_to_date;
        public DateTime valid_to_date
        {
            get { return _valid_to_date; }
            set { _valid_to_date = value; }
        } 
        int _expiry_month;
        public int expiry_month
        {
            get { return _expiry_month; }
            set { _expiry_month = value; }
        }
        int _expiry_year;
        public int expiry_year
        {
            get { return _expiry_year; }
            set { _expiry_year = value; }
        }
        DateTime _void_date_time = DateTime.MinValue;
        public DateTime void_date_time
        {
            get { return _void_date_time; }
            set { _void_date_time = value; }
        }
        string _record_locator;
        public string record_locator
        {
            get { return _record_locator; }
            set { _record_locator = value; }
        }
        string _cvv_code;
        public string cvv_code
        {
            get { return _cvv_code; }
            set { _cvv_code = value; }
        }
        string _name_on_card;
        public string name_on_card
        {
            get { return _name_on_card; }
            set { _name_on_card = value; }
        }
        string _document_number = string.Empty;
        public string document_number
        {
            get { return _document_number; }
            set { _document_number = value; }
        }
        string _form_of_payment_subtype_rcd = string.Empty;
        public string form_of_payment_subtype_rcd
        {
            get { return _form_of_payment_subtype_rcd; }
            set { _form_of_payment_subtype_rcd = value; }
        }
        string _city = string.Empty;
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        string _state = string.Empty;
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        string _street = string.Empty;
        public string street
        {
            get { return _street; }
            set { _street = value; }
        }
        string _address_line1 = string.Empty;
        public string address_line1
        {
            get { return _address_line1; }
            set { _address_line1 = value; }
        }
        string _zip_code = string.Empty;
        public string zip_code
        {
            get { return _zip_code; }
            set { _zip_code = value; }
        }
        string _country_rcd = string.Empty;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }
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
        string _pos_indicator = string.Empty;
        public string pos_indicator
        {
            get { return _pos_indicator; }
            set { _pos_indicator = value; }
        }
        int _issue_month = 0;
        public int issue_month
        {
            get { return _issue_month; }
            set { _issue_month = value; }
        }
        int _issue_year = 0;
        public int issue_year
        {
            get { return _issue_year; }
            set { _issue_year = value; }
        }
        string _issue_number = string.Empty;
        public string issue_number
        {
            get { return _issue_number; }
            set { _issue_number = value;}
        }
        protected Guid _client_profile_id = Guid.Empty;
        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        protected string _transaction_reference = string.Empty;
        public string transaction_reference
        {
            get { return _transaction_reference; }
            set { _transaction_reference = value; }
        }
        protected string _approval_code = string.Empty;
        public string approval_code
        {
            get { return _approval_code; }
            set { _approval_code = value; }
        }
        string _bank_name = string.Empty;
        public string bank_name
        {
            get { return _bank_name; }
            set { _bank_name = value; }
        }
        string _bank_code = string.Empty;
        public string bank_code
        {
            get { return _bank_code; }
            set { _bank_code = value; }
        }
        string _bank_iban = string.Empty;
        public string bank_iban
        {
            get { return _bank_iban; }
            set { _bank_iban = value; }
        }
        string _ip_address = string.Empty;
        public string ip_address
        {
            get { return _ip_address; }
            set { _ip_address = value; }
        }
        string _payment_reference = string.Empty;
        public string payment_reference
        {
            get { return _payment_reference; }
            set { _payment_reference = value; }
        }
        decimal _allocated_amount;
        public decimal allocated_amount
        {
            get { return _allocated_amount; }
            set { _allocated_amount = value; }
        }

        string _payment_type_rcd;
        public string payment_type_rcd
        {
            get { return _payment_type_rcd; }
            set { _payment_type_rcd = value; }
        }
        decimal _refunded_amount;
        public decimal refunded_amount
        {
            get { return _refunded_amount; }
            set { _refunded_amount = value; }
        }
        decimal _fee_amount_incl;
        public decimal fee_amount_incl
        {
            get { return _fee_amount_incl; }
            set { _fee_amount_incl = value; }
        }

        decimal _fee_amount;
        public decimal fee_amount
        {
            get { return _fee_amount; }
            set { _fee_amount = value; }
        }
        string _multiple_payment_flag;
        public string multiple_payment_flag
        {
            get { return _multiple_payment_flag; }
            set { _multiple_payment_flag = value; }
        }

        #region Method
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
                throw new ArgumentException("CompareTo is not possible !");
            }
        }
        #endregion
    }

    [Serializable()]
    public class PaymentUpdate
    {
        public Guid booking_payment_id
        {
            get { return _booking_payment_id; }
            set { _booking_payment_id = value; }
        }
        protected Guid _booking_payment_id;

        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        protected Guid _booking_segment_id;

        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        protected Guid _booking_id;

        public Guid voucher_payment_id
        {
            get { return _voucher_payment_id; }
            set { _voucher_payment_id = value; }
        }
        protected Guid _voucher_payment_id;

        public string form_of_payment_rcd
        {
            get { return _form_of_payment_rcd; }
            set { _form_of_payment_rcd = value; }
        }
        protected string _form_of_payment_rcd = string.Empty;

        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        protected string _currency_rcd;

        public string receive_currency_rcd
        {
            get { return _receive_currency_rcd; }
            set { _receive_currency_rcd = value; }
        }
        protected string _receive_currency_rcd;

        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        protected string _agency_code = string.Empty;

        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }
        protected decimal _payment_amount;

        public decimal receive_payment_amount
        {
            get { return _receive_payment_amount; }
            set { _receive_payment_amount = value; }
        }
        protected decimal _receive_payment_amount;

        public decimal acct_payment_amount
        {
            get { return _acct_payment_amount; }
            set { _acct_payment_amount = value; }
        }
        protected decimal _acct_payment_amount;

        public Guid payment_by
        {
            get { return _payment_by; }
            set { _payment_by = value; }
        }
        protected Guid _payment_by;

        public System.DateTime payment_date_time
        {
            get { return _payment_date_time; }
            set { _payment_date_time = value; }
        }
        protected System.DateTime _payment_date_time;

        public System.DateTime payment_due_date_time
        {
            get { return _payment_due_date_time; }
            set { _payment_due_date_time = value; }
        }
        protected System.DateTime _payment_due_date_time;


        public decimal document_amount
        {
            get { return _document_amount; }
            set { _document_amount = value; }
        }
        protected decimal _document_amount;

        public Guid void_by
        {
            get { return _void_by; }
            set { _void_by = value; }
        }
        protected Guid _void_by = Guid.Empty;

        public int expiry_month
        {
            get { return _expiry_month; }
            set { _expiry_month = value; }
        }
        protected int _expiry_month;

        public int expiry_year
        {
            get { return _expiry_year; }
            set { _expiry_year = value; }
        }
        protected int _expiry_year;

        public System.DateTime void_date_time
        {
            get { return _void_date_time; }
            set { _void_date_time = value; }
        }
        protected System.DateTime _void_date_time = DateTime.MinValue;

        public string cvv_code
        {
            get { return _cvv_code; }
            set { _cvv_code = value; }
        }
        protected string _cvv_code;

        public string name_on_card
        {
            get { return _name_on_card; }
            set { _name_on_card = value; }
        }
        protected string _name_on_card;

        public string document_number
        {
            get { return _document_number; }
            set { _document_number = value; }
        }
        protected string _document_number;

        public string form_of_payment_subtype_rcd
        {
            get { return _form_of_payment_subtype_rcd; }
            set { _form_of_payment_subtype_rcd = value; }
        }
        protected string _form_of_payment_subtype_rcd;

        public string form_of_payment_subtype
        {
            get { return _form_of_payment_subtype; }
            set { _form_of_payment_subtype = value; }
        }
        protected string _form_of_payment_subtype;

        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        protected string _city = string.Empty;

        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        protected string _state = string.Empty;

        public string street
        {
            get { return _street; }
            set { _street = value; }
        }
        protected string _street = string.Empty;

        public string address_line1
        {
            get { return _address_line1; }
            set { _address_line1 = value; }
        }
        protected string _address_line1 = string.Empty;

        public string address_line2
        {
            get { return _address_line2; }
            set { _address_line2 = value; }
        }
        protected string _address_line2 = string.Empty;

        public string zip_code
        {
            get { return _zip_code; }
            set { _zip_code = value; }
        }
        protected string _zip_code = string.Empty;

        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }
        protected string _country_rcd = string.Empty;

        public string pos_indicator
        {
            get { return _pos_indicator; }
            set { _pos_indicator = value; }
        }
        protected string _pos_indicator = string.Empty;

        public int issue_month
        {
            get { return _issue_month; }
            set { _issue_month = value; }
        }
        protected int _issue_month = 0;

        public int issue_year
        {
            get { return _issue_year; }
            set { _issue_year = value; }
        }
        protected int _issue_year = 0;

        public string issue_number
        {
            get { return _issue_number; }
            set { _issue_number = value; }
        }
        protected string _issue_number = string.Empty;

        protected Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        protected Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        protected DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        protected DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        protected byte _manual_creditcard_transaction_flag;
        public byte manual_creditcard_transaction_flag
        {
            get { return _manual_creditcard_transaction_flag; }
            set { _manual_creditcard_transaction_flag = value; }
        }

        protected string _booking_reference;
        public string booking_reference
        {
            get { return _booking_reference; }
            set { _booking_reference = value; }
        }

        protected Guid _client_profile_id = Guid.Empty;
        public Guid client_profile_id
        {
            get { return _client_profile_id; }
            set { _client_profile_id = value; }
        }
        protected string _transaction_reference = string.Empty;
        public string transaction_reference
        {
            get { return _transaction_reference; }
            set { _transaction_reference = value; }
        }
        protected string _approval_code = string.Empty;
        public string approval_code
        {
            get { return _approval_code; }
            set { _approval_code = value; }
        }
        string _bank_name = string.Empty;
        public string bank_name
        {
            get { return _bank_name; }
            set { _bank_name = value; }
        }
        string _bank_code = string.Empty;
        public string bank_code
        {
            get { return _bank_code; }
            set { _bank_code = value; }
        }
        string _bank_iban = string.Empty;
        public string bank_iban
        {
            get { return _bank_iban; }
            set { _bank_iban = value; }
        }
        string _ip_address = string.Empty;
        public string ip_address
        {
            get { return _ip_address; }
            set { _ip_address = value; }
        }

        string _payment_reference = string.Empty;
        public string payment_reference
        {
            get { return _payment_reference; }
            set { _payment_reference = value; }
        }
        decimal _allocated_amount;
        public decimal allocated_amount
        {
            get { return _allocated_amount; }
            set { _allocated_amount = value; }
        }
        public string form_of_payment
        {
            get { return _form_of_payment; }
            set { _form_of_payment = value; }
        }
        protected string _form_of_payment = string.Empty;

    }

}
