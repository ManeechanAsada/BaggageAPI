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
    public class Voucher : VoucherTemplate
    {
        public Guid voucher_id
        {
            get { return _voucher_id; }
            set { _voucher_id = value; }
        }
        protected Guid _voucher_id;

        public string voucher_number
        {
            get { return _voucher_number; }
            set { _voucher_number = value; }
        }
        protected string _voucher_number = string.Empty;

        

        public decimal payment_total
        {
            get { return _payment_total; }
            set { _payment_total = value; }
        }
        protected decimal _payment_total;

        

        public string voucher_status_rcd
        {
            get { return _voucher_status_rcd; }
            set { _voucher_status_rcd = value; }
        }
        protected string _voucher_status_rcd = string.Empty;

        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        protected DateTime _create_date_time;

        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        protected DateTime _update_date_time;
        
        public DateTime expiry_date_time
        {
            get { return _expiry_date_time; }
            set { _expiry_date_time = value; }
        }
        protected DateTime _expiry_date_time;
         
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        protected Guid _create_by;

        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        protected Guid _update_by;

        public byte refundable_flag
        {
            get { return _refundable_flag; }
            set { _refundable_flag = value; }
        }
        protected byte _refundable_flag;

        public byte percentage_flag
        {
            get { return _percentage_flag; }
            set { _percentage_flag = value; }
        }
        protected byte _percentage_flag;

        public byte single_flight_flag
        {
            get { return _single_flight_flag; }
            set { _single_flight_flag = value; }
        }
        protected byte _single_flight_flag;

        public byte single_passenger_flag
        {
            get { return _single_passenger_flag; }
            set { _single_passenger_flag = value; }
        }
        protected byte _single_passenger_flag;


        public string recipient_name
        {
            get { return _recipient_name; }
            set { _recipient_name = value; }
        }
        protected string _recipient_name;

        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }
        protected string _agency_code;

        protected string _voucher_password;
        public string voucher_password
        {
            get { return _voucher_password; }
            set { _voucher_password = value; }
        }

        protected string _fee_rcd;
        public string fee_rcd
        {
            get { return _fee_rcd; }
            set { _fee_rcd = value; }
        }

        protected string _fee_display_name;
        public string fee_display_name
        {
            get { return _fee_display_name; }
            set { _fee_display_name = value; }
        }

        protected decimal _fee_amount_incl;
        public decimal fee_amount_incl
        {
            get { return _fee_amount_incl; }
            set { _fee_amount_incl = value; }
        }

        protected decimal _fee_amount;
        public decimal fee_amount
        {
            get { return _fee_amount; }
            set { _fee_amount = value; }
        }
    }
}
