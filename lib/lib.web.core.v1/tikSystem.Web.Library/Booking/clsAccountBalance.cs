using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace tikSystem.Web.Library
{
    public class AccountBalance
    {
        #region Property
        Guid _agency_account_id = Guid.Empty;
        public Guid agency_account_id
        {
            get { return _agency_account_id; }
            set { _agency_account_id = value; }
        }

        private string _agency_code;
        public string agency_code
        {
            get { return _agency_code; }
            set { _agency_code = value; }
        }

        private DateTime _payment_date_time;
        public DateTime payment_date_time
        {
            get { return _payment_date_time; }
            set { _payment_date_time = value; }
        }

        private decimal _payment_amount;
        public decimal payment_amount
        {
            get { return _payment_amount; }
            set { _payment_amount = value; }
        }

        Guid _payment_by = Guid.Empty;
        public Guid payment_by
        {
            get { return _payment_by; }
            set { _payment_by = value; }
        }

        private byte _void_flag;
        public byte void_flag
        {
            get { return _void_flag; }
            set { _void_flag = value; }
        }

        private DateTime _void_date_time;
        public DateTime void_date_time
        {
            get { return _void_date_time; }
            set { _void_date_time = value; }
        }

        Guid _void_by = Guid.Empty;
        public Guid void_by
        {
            get { return _void_by; }
            set { _void_by = value; }
        }

        private string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }

        private DateTime _topup_commisstion_date;
        public DateTime topup_commisstion_date
        {
            get { return _topup_commisstion_date; }
            set { _topup_commisstion_date = value; }
        }

        private decimal _topup_withholding_tax_amount;
        public decimal topup_withholding_tax_amount
        {
            get { return _topup_withholding_tax_amount; }
            set { _topup_withholding_tax_amount = value; }
        }

        private decimal _topup_amount_incl;
        public decimal topup_amount_incl
        {
            get { return _topup_amount_incl; }
            set { _topup_amount_incl = value; }
        }

        private string _topup_comment;
        public string topup_comment
        {
            get { return _topup_comment; }
            set { _topup_comment = value; }
        }

        private string _payment_agency_code;
        public string payment_agency_code
        {
            get { return _payment_agency_code; }
            set { _payment_agency_code = value; }
        }

        private string _void_user;
        public string void_user
        {
            get { return _void_user; }
            set { _void_user = value; }
        }       
        #endregion
    }
}
