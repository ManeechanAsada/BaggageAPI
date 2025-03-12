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
    public class Quote
    {
        #region property
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }

        string _passenger_type_rcd;
        public string passenger_type_rcd
        {
            get { return _passenger_type_rcd; }
            set { _passenger_type_rcd = value; }
        }
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        string _charge_type;
        public string charge_type
        {
            get { return _charge_type; }
            set { _charge_type = value; }
        }

        string _charge_name;
        public string charge_name
        {
            get { return _charge_name; }
            set { _charge_name = value; }
        }

        decimal _charge_amount;
        public decimal charge_amount
        {
            get { return _charge_amount; }
            set { _charge_amount = value; }
        }
        decimal _total_amount;
        public decimal total_amount
        {
            get { return _total_amount; }
            set { _total_amount = value; }
        }

        decimal _tax_amount;
        public decimal tax_amount
        {
            get { return _tax_amount; }
            set { _tax_amount = value; }
        }
        int _passenger_count;
        public int passenger_count
        {
            get { return _passenger_count; }
            set { _passenger_count = value; }
        }

        int _sort_sequence;
        public int sort_sequence
        {
            get { return _sort_sequence; }
            set { _sort_sequence = value; }
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
        decimal _redemption_points;
        public decimal redemption_points
        {
            get { return _redemption_points; }
            set { _redemption_points = value; }
        }
        decimal _charge_amount_incl;
        public decimal charge_amount_incl
        {
            get { return _charge_amount_incl; }
            set { _charge_amount_incl = value; }
        }
        #endregion
    }
}
