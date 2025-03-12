using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Text;


namespace tikSystem.Web.Library
{
    public class Reports : LibraryBase
    {
        #region Method
        public string GetActivities(string AgencyCode, 
                                        string RemarkType, 
                                        string Nickname, 
                                        DateTime TimelimitFrom, 
                                        DateTime TimelimitTo,
                                        bool PendingOnly, 
                                        bool IncompleteOnly, 
                                        bool IncludeRemarks,
                                        bool showUnassigned)
        {
            this.Clear();
            string result = "";
            int iConfigValue = 0;
            if (ConfigurationManager.AppSettings["Service"] != null)
            {
                iConfigValue = Convert.ToInt16(ConfigurationManager.AppSettings["Service"]);
            }

            switch (iConfigValue)
            {
                case 1:
                    //Old Web service
                    result = GetActivitie(AgencyCode,
                                         RemarkType,
                                         Nickname,
                                         TimelimitFrom,
                                         TimelimitTo,
                                         PendingOnly,
                                         IncompleteOnly,
                                         IncludeRemarks,
                                         showUnassigned);
                    break;
                default:
                    //new Web service
                    result = GetActivitiesWS(AgencyCode,
                                        RemarkType,
                                        Nickname,
                                        TimelimitFrom,
                                        TimelimitTo,
                                        PendingOnly,
                                        IncompleteOnly,
                                        IncludeRemarks,
                                        showUnassigned);
                    break;
            }
            return result;
        }
        #endregion

        #region COM Call
        #endregion

        #region Old Web Service (This section will be remove when the new one is stable)
        private string GetActivitie(string AgencyCode,
                                        string RemarkType,
                                        string Nickname,
                                        DateTime TimelimitFrom,
                                        DateTime TimelimitTo,
                                        bool PendingOnly,
                                        bool IncompleteOnly,
                                        bool IncludeRemarks,
                                        bool showUnassigned)
        {
            try
            {
                if (objService != null)
                {
                    DataSet ds = objService.GetActivities(AgencyCode,
                                         RemarkType,
                                         Nickname,
                                         TimelimitFrom,
                                         TimelimitTo,
                                         PendingOnly,
                                         IncompleteOnly,
                                         IncludeRemarks);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.GetXml();
                    }
                    else
                        return "";
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region New Webservice call
        public string GetActivitiesWS(string AgencyCode,
                                        string RemarkType,
                                        string Nickname,
                                        DateTime TimelimitFrom,
                                        DateTime TimelimitTo,
                                        bool PendingOnly,
                                        bool IncompleteOnly,
                                        bool IncludeRemarks,
                                        bool showUnassigned)
        {
            string result = "";
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                result = objService.GetActivities(AgencyCode,
                                                    RemarkType,
                                                    Nickname,
                                                    TimelimitFrom,
                                                    TimelimitTo,
                                                    PendingOnly,
                                                    IncompleteOnly,
                                                    IncludeRemarks,
                                                    showUnassigned,base.CreateToken()).GetXml();

            }
            return result;
        }
        #endregion

        #region Old Code

        #region Public Members
        public DataSet GetTicketsIssued(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo
            , string strOrigin, string dtrDestination, string strAgency, string strAirline, string strFlight)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsIssued(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, 
                strOrigin, dtrDestination, strAgency, strAirline, strFlight);
        }

        public DataSet GetTicketsUsed(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo
            , string strOrigin, string dtrDestination, string strAgency, string strAirline, string strFlight)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsUsed(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, 
                strOrigin, dtrDestination, strAgency, strAirline, strFlight);
        }

        public DataSet GetTicketsRefunded(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo
            , string strOrigin, string dtrDestination, string strAgency, string strAirline, string strFlight)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsRefunded(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo,
                strOrigin, dtrDestination, strAgency, strAirline, strFlight);
        }

        public DataSet GetTicketsCancelled(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, 
            DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, int intTicketonly, int intRefundable, 
            string strProfileID, string strTicketNumber, string strFirstName, string strLastName, string strPassengerId, string strBookingSegmentID)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsCancelled(strOrigin, strDestination, strAgency, strAirline, strFlight, 
                dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, intTicketonly, intRefundable, 
                strProfileID, strTicketNumber, strFirstName, strLastName, strPassengerId, strBookingSegmentID);
        }

        public DataSet GetTicketsNotFlown(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, 
            DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, bool bUnflown, bool bNoShow)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsNotFlown(strOrigin, strDestination, strAgency, strAirline, strFlight, 
                dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, bUnflown, bNoShow);
        }

        public DataSet GetTicketsExpired(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, 
            string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetTicketsExpired(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination, 
                strAgency, strAirline, strFlight);
        }

        public DataSet GetCashbookPayments(string strAgency, string strGroup, string strUserId, DateTime dtPaymentFrom, 
            DateTime dtPaymentTo, string strCashbookId)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetCashbookPayments(strAgency, strGroup, strUserId, dtPaymentFrom, dtPaymentTo, strCashbookId);
        }

        public DataSet GetCashbookCharges(string XmlCashbookCharges, string strCashbookId)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetCashbookCharges(XmlCashbookCharges, strCashbookId);
        }

        public DataSet GetBookingFeeAccounted(string strAgencyCode, string strUserId, string strFee, DateTime dtFrom, DateTime dtTo)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetBookingFeeAccounted(strAgencyCode, strUserId, strFee, dtFrom, dtTo);
        }

        public DataSet CreditCardPayment(string strCCNumber, string strTransType, string strTransStatus, DateTime dtFrom, DateTime dtTo, string strCCType, string strAgency)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();
            string _strCCNumber = strCCNumber;
            string _strTransType = strTransType;
            string _strTransStatus = strTransStatus;
            DateTime _dtFrom = dtFrom;
            DateTime _dtTo = dtTo;
            string _strCCType = strCCType;
            string _strAgency = strAgency;

            objClient.objService = objService;
            return objClient.CreditCardPayment(ref _strCCNumber, ref _strTransType, ref _strTransStatus, ref _dtFrom, ref _dtTo, ref _strCCType, ref _strAgency);
        }

        public DataSet GetBookings(string Airline, string FlightNumber, string FlightId, DateTime FlightFrom, DateTime FlightTo, 
            string RecordLocator, string Origin, string Destination, string PassengerName, string SeatNumber, string TicketNumber, 
            string PhoneNumber, string AgencyCode, string ClientNumber, string MemberNumber, string ClientId, bool ShowHistory, 
            string Language, bool bIndividual, bool bGroup, DateTime CreateFrom, DateTime CreateTo)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            return objClient.GetBookings(Airline, FlightNumber, FlightId, FlightFrom, FlightTo, 
                                    RecordLocator, Origin, Destination, PassengerName, SeatNumber, TicketNumber, 
                                    PhoneNumber, AgencyCode, ClientNumber, MemberNumber, ClientId, ShowHistory, 
                                    Language, bIndividual, bGroup, CreateFrom, CreateTo);
        }

        public DataSet GetOutstanding(string strAgencyCode, string strAirline, string strFlightNumber, DateTime dtFlightFrom, DateTime dtFlightTo, 
            string strOrigin, string strDestination, bool bOffices, bool bAgencies, bool bLastTwentyFourHours, bool bTicketedOnly, int iOlderThanHours, 
            string strLanguage, bool bAccountsPayable)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetOutstanding(strAgencyCode, strAirline, strFlightNumber, dtFlightFrom, dtFlightTo, strOrigin, 
                strDestination, bOffices, bAgencies, bLastTwentyFourHours, bTicketedOnly, iOlderThanHours, strLanguage, bAccountsPayable);
        }

        public DataSet GetAgencyAccountTransactions(string agencyCode, DateTime dateFrom, DateTime dateTo)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetAgencyAccountTransactions(agencyCode, dateFrom, dateTo);
        }

        public bool CompleteRemark(string XmlRemarks, string RemarkId, string UserId)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.CompleteRemark(XmlRemarks, RemarkId, UserId);
        }

        public DataSet GetCashbookPaymentsSummary(string XmlCashbookPaymentsAll)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetCashbookPaymentsSummary(XmlCashbookPaymentsAll);
        }

        public DataSet GetAgencyAccountTopUp(string strAgencyCode, string strCurrency, DateTime dtFrom, DateTime dtTo)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetAgencyAccountTopUp(strAgencyCode, strCurrency, dtFrom, dtTo);
        }

        public DataSet GetAgencyTicketSales(string strAgency, string strCurrency, DateTime dtSalesFrom, DateTime dtSalesTo)
        {
            ServiceClient objClient = new ServiceClient();
            Library objLi = new Library();

            objClient.objService = objService;
            return objClient.GetAgencyTicketSales(strAgency, strCurrency, dtSalesFrom, dtSalesTo);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            objService = null;
        }

        #endregion

        #endregion
    }
}
