using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIPassengerFee
    {
        #region Property
        Guid _booking_fee_id = Guid.Empty;
        public Guid booking_fee_id
        {
            get { return _booking_fee_id; }
            set { _booking_fee_id = value; }
        }

        decimal _fee_amount;
        public decimal fee_amount
        {
            get { return _fee_amount; }
            set { _fee_amount = value; }
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

        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        decimal _acc_fee_amount;
        public decimal acc_fee_amount
        {
            get { return _acc_fee_amount; }
            set { _acc_fee_amount = value; }
        }

        Guid _fee_id = Guid.Empty;
        public Guid fee_id
        {
            get { return _fee_id; }
            set { _fee_id = value; }
        }

        string _fee_rcd;
        public string fee_rcd
        {
            get { return _fee_rcd; }
            set { _fee_rcd = value; }
        }

        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }

        decimal _payment_amount;
        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }

        protected Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        protected string _agency_code = string.Empty;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }

        Guid _passenger_segment_service_id = Guid.Empty;
        public Guid passenger_segment_service_id
        {
            get { return _passenger_segment_service_id; }
            set { _passenger_segment_service_id = value; }
        }

        string _fee_category_rcd;
        public string fee_category_rcd
        {
            get { return _fee_category_rcd; }
            set { _fee_category_rcd = value; }
        }

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

        string _od_origin_rcd;
        public string od_origin_rcd
        {
            get { return _od_origin_rcd; }
            set { _od_origin_rcd = value; }
        }

        string _od_destination_rcd;
        public string od_destination_rcd
        {
            get { return _od_destination_rcd; }
            set { _od_destination_rcd = value; }
        }
        #endregion
    }
}
