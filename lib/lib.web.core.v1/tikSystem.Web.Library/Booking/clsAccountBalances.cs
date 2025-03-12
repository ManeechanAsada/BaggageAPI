using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using tikSystem.Web.Library;
// Start Yai Add Account Topup
using System.Xml.Serialization;
// End Yai Add Account Topup

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class AccountBalances : CollectionBase
    {
        // Start Yai Add Account Topup
        [XmlIgnore]
        public agentservice.TikAeroXMLwebservice objService = null;
        // End Yai Add Account Topup
        public AccountBalance this[int index]
        {
            get { return (AccountBalance)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(AccountBalance value)
        {
            return this.List.Add(value);
        }
        // Start Yai Add Account Topup
        public bool AddAgencyAccountRequest(string strAgencyCode, string strCurrency, string strUserId, double dAmount, string strExternalReference)
        {
            bool bResult = false;
            ServiceClient obj = new ServiceClient();
            try
            {
                obj.objService = objService;
                bResult = obj.AgencyAccountAdd(strAgencyCode,
                                            strCurrency,
                                            strUserId,
                                            string.Empty,
                                            dAmount,
                                            strExternalReference,
                                            string.Empty,
                                            string.Empty,
                                            true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n" + e.StackTrace);
            }
            return bResult;
        }

        public bool VoidAgencyAccount(string strAgencyAccountId, string strUserId)
        {
            bool bResult = false;
            ServiceClient obj = new ServiceClient();
            try
            {
                obj.objService = objService;
                bResult = obj.AgencyAccountVoid(strAgencyAccountId, strUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n" + e.StackTrace);
            }
            return bResult;
        }

        public string ExternalPaymentListAgencyTopUp(string strAgencyCode)
        {
            DataSet dsResult = null;
            string strXml = string.Empty;
            ServiceClient obj = new ServiceClient();
            try
            {
                obj.objService = objService;
                dsResult = obj.ExternalPaymentListAgencyTopUp(strAgencyCode);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            if (dr["create_date_time"] != null && dr["create_date_time"].ToString().Trim() != "")
                            {
                                dr["create_date_time"] = ((DateTime)dr["create_date_time"]).ToLocalTime();
                            }
                        }
                        strXml = dsResult.GetXml();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n" + e.StackTrace);
            }
            return strXml;
        }

        public bool ExternalPaymentSearchAgencyTopUp(string strAgencyCode, string strAgencyAccountId)
        {
            bool bResult = false;
            ServiceClient obj = new ServiceClient();
            try
            {
                obj.objService = objService;
                DataSet dsResult = obj.ExternalPaymentListAgencyTopUp(strAgencyCode);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            if (dr["agency_account_id"] != null && dr["agency_account_id"].ToString().Trim() == strAgencyAccountId)
                                bResult = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n" + e.StackTrace);
            }
            return bResult;
        }
        // End Yai Add Account Topup
    }
}
