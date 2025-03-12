using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class CheckinPassenger : Passenger
    {
        Guid _booking_segment_id = Guid.Empty;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        double _Fare = 0;
        public double Fare
        {
            get { return _Fare; }
            set { _Fare = value; }
        }
        decimal _yq_amount = 0;
        public decimal yq_amount
        {
            get { return _yq_amount; }
            set { _yq_amount = value; }
        }
        decimal _acct_fare_amount = 0;
        public decimal acct_fare_amount
        {
            get { return _acct_fare_amount; }
            set { _acct_fare_amount = value; }
        }
        decimal _acct_yq_amount = 0;
        public decimal acct_yq_amount
        {
            get { return _acct_yq_amount; }
            set { _acct_yq_amount = value; }
        }
        int _group_sequence;
        public int group_sequence
        {
            get { return _group_sequence; }
            set { _group_sequence = value; }
        }
    }
}
