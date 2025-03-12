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
using System.Collections.Specialized;
using tikSystem.Web.Library;
using tikSystem.Web.Library.agentservice;
using System.Text;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using NLog;

namespace tikAEROWebCheckinAPI.Classes
{
    public class Helper
    {
        public static string ToConnectionString(string strName)
        {
            string strConfigValue = ConfigurationManager.AppSettings[strName];
            if (string.IsNullOrEmpty(strConfigValue) == false)
            {
                return strConfigValue;
            }
            else
            {
                return string.Empty;
            }
        }

        private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

        public void GetFlightInformation(ref CheckInPassengers ckps, string strBookingSegmentId, string strCheckInXml)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strCheckInXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Library objLi = new Library();

            string strGroupSequence = string.Empty;
            string strPath;

            if (strBookingSegmentId.Length == 0)
            { strPath = "Booking/Mapping"; }
            else
            { strPath = "Booking/Mapping[booking_segment_id = '" + strBookingSegmentId + "']"; }

            CheckinPassenger ckp;
            foreach (XPathNavigator n in nv.Select(strPath))
            {
                strGroupSequence = objLi.getXPathNodevalue(n, "group_sequence", Library.xmlReturnType.value);
                ckp = new CheckinPassenger();
                ckp.booking_segment_id = new Guid(objLi.getXPathNodevalue(n, "booking_segment_id", Library.xmlReturnType.value));
                ckp.passenger_id = new Guid(objLi.getXPathNodevalue(n, "passenger_id", Library.xmlReturnType.value));
                ckp.group_sequence = (strGroupSequence.Length > 0) ? Convert.ToInt16(strGroupSequence) : 0;
                ckps.Add(ckp);
            }
        }

        public bool IsValid(string candidate)
        {
            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class Language
    {
        public static string Value(string key, string defaultValue)
        {
            StringDictionary sd = (StringDictionary)HttpContext.Current.Session["Language"];

            string result;

            if (key.Length == 0 || sd == null)
            { result = defaultValue; }
            else
            {
                if (sd[key] != null)
                { result = sd[key]; }
                else
                { result = defaultValue; }
            }

            return result;
        }
        public static string CurrentCode()
        {
            if (HttpContext.Current.Session["LanCode"] != null)
            {
                string[] strLangCode = HttpContext.Current.Session["LanCode"].ToString().Split('-');
                if (strLangCode.Length == 0)
                {
                    strLangCode = ConfigurationManager.AppSettings["DefaultLanguage"].Split('-');
                }
                return strLangCode[0];
            }
            else
            {
                return ConfigurationManager.AppSettings["DefaultLanguage"].Split('-')[0].ToUpper();
            }
        }
        public static string CurrentFullCode()
        {
            if (HttpContext.Current.Session["LanCode"] != null)
            {
                string strLangCode = HttpContext.Current.Session["LanCode"].ToString();
                if (strLangCode.Length == 0)
                {
                    strLangCode = ConfigurationManager.AppSettings["DefaultLanguage"];
                }
                return strLangCode;
            }
            else
            {
                HttpContext.Current.Session["LanCode"] = ConfigurationManager.AppSettings["DefaultLanguage"];
                return HttpContext.Current.Session["LanCode"].ToString();
            }
        }
    }
    public class LangaugeHelper
    {
        public string get(string key, string defaultValue)
        {
            return Classes.Language.Value(key, defaultValue);
        }
    }

    public static class LogHelper
    {
        public static ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void writeToLogFile(string methodName, string parameters, string errorCode, string errorMessage, string errorStackTrace)
        {
            string strLogMessage = string.Empty;
            string logFilePath = string.Empty;
           // StreamWriter swLog = null;
            string strSessionId = string.Empty;
            string ip = string.Empty;
            string ServerName = "";

            try
            {
                strSessionId = HttpContext.Current.Session.SessionID;

                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                ServerName = Environment.MachineName;

            }
            catch (System.Exception ex)
            {
            }

            logFilePath = HttpContext.Current.Server.MapPath("~") + @"\logs\" + "log_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".txt";

            strLogMessage = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}{8}",
            "UTC Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + Environment.NewLine,
            "Brisbane Date : " + DateTime.Now.AddHours(10).ToString("dd/MM/yyyy HH:mm:ss") + Environment.NewLine,
            "IPAddress : " + ip + Environment.NewLine,
            "ServerName: " + ServerName + Environment.NewLine,
            "SessionId : " + strSessionId + Environment.NewLine,
            "MethodName : " + methodName + Environment.NewLine,
            "Parameters : " + parameters + Environment.NewLine,
            "ErrorCode :" + errorCode + Environment.NewLine,
            "ErrorMessage : " + errorMessage + Environment.NewLine,
            "ErrorStackTrace : " + errorStackTrace + Environment.NewLine);

            try
            {
                //if (!File.Exists(logFilePath))
                //{
                //    swLog = new StreamWriter(logFilePath);
                //}
                //else
                //{
                //    swLog = File.AppendText(logFilePath);
                //}

                //swLog.WriteLine(strLogMessage);
                //swLog.WriteLine();
                //swLog.Flush();
                //swLog.Close();
                _logger.Info(strLogMessage);
            }
            catch
            {
                //if (swLog != null)
                //{
                //    swLog.Close();
                //}
            }

        } 

    }
}