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
    public class Tax
    {
        #region Property
        Guid _passenger_segment_tax_id;
        public Guid passenger_segment_tax_id
        {
            get { return _passenger_segment_tax_id; }
            set { _passenger_segment_tax_id = value; }
        }
        decimal _tax_amount;
        public decimal tax_amount
        {
            get { return _tax_amount; }
            set { _tax_amount = value; }
        }
        decimal _tax_amount_incl;
        public decimal tax_amount_incl
        {
            get { return _tax_amount_incl; }
            set { _tax_amount_incl = value; }
        }
        decimal _acct_amount;
        public decimal acct_amount
        {
            get { return _acct_amount; }
            set { _acct_amount = value; }
        }
        decimal _acct_amount_incl;
        public decimal acct_amount_incl
        {
            get { return _acct_amount_incl; }
            set { _acct_amount_incl = value; }
        }
        decimal _sales_amount;
        public decimal sales_amount
        {
            get { return _sales_amount; }
            set { _sales_amount = value; }
        }
        decimal _sales_amount_incl;
        public decimal sales_amount_incl
        {
            get { return _sales_amount_incl; }
            set { _sales_amount_incl = value; }
        }
        string _tax_rcd;
        public string tax_rcd
        {
            get { return _tax_rcd; }
            set { _tax_rcd = value; }
        }
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        Guid _tax_id;
        public Guid tax_id
        {
            get { return _tax_id; }
            set { _tax_id = value; }
        }
        Guid _booking_segment_id;
        public Guid booking_segment_id
        {
            get { return _booking_segment_id; }
            set { _booking_segment_id = value; }
        }
        string _tax_currency_rcd;
        public string tax_currency_rcd
        {
            get { return _tax_currency_rcd; }
            set { _tax_currency_rcd = value; }
        }
        string _sales_currency_rcd;
        public string sales_currency_rcd
        {
            get { return _sales_currency_rcd; }
            set { _sales_currency_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        string _summarize_up;
        public string summarize_up
        {
            get { return _summarize_up; }
            set { _summarize_up = value; }
        }
        string _coverage_type;
        public string coverage_type
        {
            get { return _coverage_type; }
            set { _coverage_type = value; }
        }
        Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        decimal _vat_percentage;
        public decimal vat_percentage
        {
            get { return _vat_percentage; }
            set { _vat_percentage = value; }
        }
        #endregion
    }
}
