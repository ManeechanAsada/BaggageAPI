using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Allocation
    {
        Guid _booking_payment_id;
        public Guid booking_payment_id
        {
            get { return _booking_payment_id; }
            set { _booking_payment_id = value; }
        }
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        Guid _booking_fee_id;
        public Guid booking_fee_id
        {
            get { return _booking_fee_id; }
            set { _booking_fee_id = value; }
        }
        Guid _voucher_id;
        public Guid voucher_id
        {
            get { return _voucher_id; }
            set { _voucher_id = value; }
        }
        Guid _fee_id;
        public Guid fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }
        Guid _user_id;
        public Guid user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        double _sales_amount;
        public double sales_amount
        {
            get { return _sales_amount; }
            set { _sales_amount = value; }
        }
        double _payment_amount;
        public double payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }
        double _account_amount;
        public double account_amount
        {
            get { return _account_amount; }
            set { _account_amount = value; }
        }
    }

}
