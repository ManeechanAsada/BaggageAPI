using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using tikSystem.Web.Library;
using tikSystem.Web.Library.agentservice;

/// <summary>
/// Summary description for clsWebService
/// </summary>
namespace tikSystem.Web.Library
{
    public class WebService
    {
        private AgentAuthHeader WebServiceHeader = new AgentAuthHeader();

        public TikAeroXMLwebservice InitialWebServices(string agencyCode, string agencyLogon, string agencyPassword, string selectedAgency, ref Agents objUv)
        {
            string strXMLResult = string.Empty;
            string AgencyCode = string.Empty;
            string AgencyLogon = string.Empty;
            string AgencyPassword = string.Empty;
            try
            {
                TikAeroXMLwebservice objService = new TikAeroXMLwebservice();
                CookieContainer cc = new CookieContainer();

                string strPassport = string.Empty;
                //string strWebServiceUrl;

                AgencyCode = (agencyCode == "") ? ConfigurationManager.AppSettings["DefaultAgencyCode"] : agencyCode;
                AgencyLogon = (agencyLogon == "") ? ConfigurationManager.AppSettings["DefaultAgencyLogon"] : agencyLogon;
                AgencyPassword = (agencyPassword == "") ? ConfigurationManager.AppSettings["DefaultAgencyPassword"] : agencyPassword;
                //strWebServiceUrl = ConfigurationManager.AppSettings["AgentService"];

                if (selectedAgency == string.Empty)
                {
                    selectedAgency = AgencyCode;
                }

                strXMLResult = objService.AgencyDetails(AgencyCode, AgencyLogon, AgencyPassword, ref strPassport, selectedAgency);

                if (!strXMLResult.Equals("0") && !strXMLResult.Contains("Error:"))
                {
                    Library li = new Library();
                    li.AddAgent(strXMLResult, objUv);
                    li = null;
                    if (objUv.Count > 0)
                    {
                        WebServiceHeader.AgencyCode = objUv[0].agency_code;
                        WebServiceHeader.AgencyPassport = strPassport;
                        WebServiceHeader.AgencyCurrencyRcd = objUv[0].currency_rcd;

                        objService.AgentAuthHeaderValue = WebServiceHeader;
                        objService.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        objService.CookieContainer = cc;
                    }
                }

                return objService;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}

