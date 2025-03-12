using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using tikSystem.Web.Library.agentservice;
using System.IO;

namespace tikSystem.Web.Library
{
    public class ServiceClient : IClientCore
    {
        public object objService = null;


        #region IClientCore Members

        public string ServiceAuthentication(string agencyCode, string agencyLogon, string agencyPasseword, string selectedAgency)
        {
            string result = "";
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ServiceAuthentication(agencyCode, agencyLogon, agencyPasseword, selectedAgency);
                    break;
            }

            return result;
        }

        public DataSet GetAirport(string language)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAirport(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAirport(language);
                    break;
            }

            return ds;
        }

        public Routes GetOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag)
        {
            Routes routes = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    routes = objAgent.GetOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    routes = objtikAeroWebService.GetOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag);
                    break;
            }


            return routes;
        }

        public Routes GetDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag)
        {
            Routes routes = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    routes = objAgent.GetDestination(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    routes = objtikAeroWebService.GetDestination(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag);
                    break;
            }
            return routes;
        }

        public DataSet GetAgencyCode(string agencyCode)
        {
            DataSet ds = null;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAgencyCode(agencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAgencyCode(agencyCode);
                    break;
            }

            return ds;
        }

        public bool ReleaseFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.ReleaseFlightInventorySession(sessionId, flightId, bookingClasss, bookingId, releaseTimeOut, ReleaseInventory, ReleaseBookingLock);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ReleaseFlightInventorySession(sessionId, flightId, bookingClasss, bookingId, releaseTimeOut, ReleaseInventory, ReleaseBookingLock);
                    break;
            }

            return result;
        }
        public bool ReleaseSessionlessFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock, string strToken)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.ReleaseSessionlessFlightInventorySession(sessionId, flightId, bookingClasss, bookingId, releaseTimeOut, ReleaseInventory, ReleaseBookingLock, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ReleaseSessionlessFlightInventorySession(sessionId, flightId, bookingClasss, bookingId, releaseTimeOut, ReleaseInventory, ReleaseBookingLock, string.Empty);
                    break;
            }
            return result;
        }

        public string GetCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetCompactFlightAvailability(Origin, Destination, DepartDateFrom, DepartDateTo, ReturnDateFrom, ReturnDateTo, DateBooking, Adult, Child, Infant, Other, OtherPassengerType, BoardingClass, BookingClass, DayTimeIndicator, AgencyCode, CurrencyCode, FlightId, FareId, MaxAmount, NonStopOnly, IncludeCancelled, IncludeCancelled, IncludeWaitlisted, IncludeSoldOut, Refundable, GroupFares, ItFaresOnly, bStaffFares, bApplyFareLogic, bUnknownTransit, strTransitPoint, dteReturnFrom, dteReturnTo, dtReturn, bMapWithFares, bReturnRefundable, strReturnDayTimeIndicator, PromotionCode, iFareLogic, strSearchType);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetCompactFlightAvailability(Origin, Destination, DepartDateFrom, DepartDateTo, ReturnDateFrom, ReturnDateTo, DateBooking, Adult, Child, Infant, Other, OtherPassengerType, BoardingClass, BookingClass, DayTimeIndicator, AgencyCode, CurrencyCode, FlightId, FareId, MaxAmount, NonStopOnly, IncludeCancelled, IncludeCancelled, IncludeWaitlisted, IncludeSoldOut, Refundable, GroupFares, ItFaresOnly, bStaffFares, bApplyFareLogic, bUnknownTransit, strTransitPoint, dteReturnFrom, dteReturnTo, dtReturn, bMapWithFares, bReturnRefundable, strReturnDayTimeIndicator, PromotionCode, iFareLogic, strSearchType);
                    break;
            }


            return result;
        }
        public string GetSessionlessCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType, string strToken)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.GetSessionlessCompactFlightAvailability(Origin, Destination, DepartDateFrom, DepartDateTo, ReturnDateFrom, ReturnDateTo, DateBooking, Adult, Child, Infant, Other, OtherPassengerType, BoardingClass, BookingClass, DayTimeIndicator, AgencyCode, CurrencyCode, FlightId, FareId, MaxAmount, NonStopOnly, IncludeCancelled, IncludeCancelled, IncludeWaitlisted, IncludeSoldOut, Refundable, GroupFares, ItFaresOnly, bStaffFares, bApplyFareLogic, bUnknownTransit, strTransitPoint, dteReturnFrom, dteReturnTo, dtReturn, bMapWithFares, bReturnRefundable, strReturnDayTimeIndicator, PromotionCode, iFareLogic, strSearchType, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetSessionlessCompactFlightAvailability(Origin, Destination, DepartDateFrom, DepartDateTo, ReturnDateFrom, ReturnDateTo, DateBooking, Adult, Child, Infant, Other, OtherPassengerType, BoardingClass, BookingClass, DayTimeIndicator, AgencyCode, CurrencyCode, FlightId, FareId, MaxAmount, NonStopOnly, IncludeCancelled, IncludeCancelled, IncludeWaitlisted, IncludeSoldOut, Refundable, GroupFares, ItFaresOnly, bStaffFares, bApplyFareLogic, bUnknownTransit, strTransitPoint, dteReturnFrom, dteReturnTo, dtReturn, bMapWithFares, bReturnRefundable, strReturnDayTimeIndicator, PromotionCode, iFareLogic, strSearchType, strToken);
                    break;
            }


            return result;
        }

        public string FlightAdd(string agencyCode, string currency, string flightXml, string bookingID, short adults, short children, short infants, short others, string strOthers, string userId, string strIpAddress, string strLanguageCode, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.FlightAdd(agencyCode, currency, flightXml, bookingID, adults, children, infants, others, strOthers, userId, strIpAddress, strLanguageCode, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.FlightAdd(agencyCode, currency, flightXml, bookingID, adults, children, infants, others, strOthers, userId, strIpAddress, strLanguageCode, bNoVat);
                    break;
            }


            return result;
        }
        public string GetClient(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            if (string.IsNullOrEmpty(clientId) == false && DataHelper.IsGUID(clientId) == false)
            { return string.Empty; }
            else if (string.IsNullOrEmpty(passengerId) == false && DataHelper.IsGUID(passengerId) == false)
            { return string.Empty; }
            else if (string.IsNullOrEmpty(clientNumber) == false && DataHelper.IsNumeric(clientNumber) == false)
            { return string.Empty; }
            else
            {
                string result = string.Empty;
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "1":
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        result = objAgent.GetClient(clientId, clientNumber, passengerId, bShowRemark);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        //Call new webservice
                        clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                        result = objtikAeroWebService.GetClient(clientId, clientNumber, passengerId, bShowRemark);
                        break;
                }

                return result;
            }
        }
        public DataSet GetClientPassenger(string bookingId, string clientProfileId, string clientNumber)
        {
            if (string.IsNullOrEmpty(bookingId) == false && DataHelper.IsGUID(bookingId) == false)
            { return null; }
            else if (string.IsNullOrEmpty(clientProfileId) == false && DataHelper.IsGUID(clientProfileId) == false)
            { return null; }
            else if (string.IsNullOrEmpty(clientNumber) == false && DataHelper.IsNumeric(clientNumber) == false)
            { return null; }
            else
            {
                DataSet result = null;
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "1":
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        result = objAgent.GetClientPassenger(bookingId, clientProfileId, clientNumber);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        //Call new webservice
                        clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                        result = objtikAeroWebService.GetClientPassenger(bookingId, clientProfileId, clientNumber);
                        break;
                }
                return result;
            }
        }
        public DataSet GetTransaction(string strOrigin,
                                         string strDestination,
                                         string strAirline,
                                         string strFlight,
                                         string strSegmentType,
                                         string strClientProfileId,
                                         string strPassengerProfileId,
                                         string strVendor,
                                         string strCreditDebit,
                                         DateTime dtFlightFrom,
                                         DateTime dtFlightTo,
                                         DateTime dtTransactionFrom,
                                         DateTime dtTransactionTo,
                                         DateTime dtExpiryFrom,
                                         DateTime dtExpiryTo,
                                         DateTime dtVoidFrom,
                                         DateTime dtVoidTo,
                                         short iBatch,
                                         bool bAllVoid,
                                         bool bAllExpired,
                                         bool bAuto,
                                         bool bManual,
                                         bool bAllPoint)
        {
            DataSet result = null;
            string response = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    response = objAgent.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom, dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo, dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);

                    if (!string.IsNullOrEmpty(response))
                    {
                        StringReader reader = new StringReader(response);
                        result.ReadXml(reader, XmlReadMode.IgnoreSchema);
                    }

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    response = objtikAeroWebService.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom, dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo, dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);

                    if (!string.IsNullOrEmpty(response))
                    {
                        StringReader reader = new StringReader(response);
                        result.ReadXml(reader, XmlReadMode.IgnoreSchema);
                    }

                    break;
            }
            return result;

        }
        public Titles GetPassengerTitles(string language)
        {
            Titles titles = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    titles = objAgent.GetPassengerTitles(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    titles = objtikAeroWebService.GetPassengerTitles(language);
                    break;
            }

            return titles;
        }
        public Documents GetDocumentType(string language)
        {
            Documents documents = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    documents = objAgent.GetDocumentType(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    documents = objtikAeroWebService.GetDocumentType(language);
                    break;
            }
            return documents;
        }
        public Countries GetCountry(string language)
        {
            Countries countries = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    countries = objAgent.GetCountry(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    countries = objtikAeroWebService.GetCountry(language);
                    break;
            }

            return countries;
        }
        public Languages GetLanguages(string language)
        {
            Languages languages = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    languages = objAgent.GetLanguages(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    languages = objtikAeroWebService.GetLanguages(language);
                    break;
            }

            return languages;
        }
        public DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass, string strLanguage)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetSeatMap(origin, destination, flightId, boardingClass, bookingClass, strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetSeatMap(origin, destination, flightId, boardingClass, bookingClass, strLanguage);
                    break;
            }

            return ds;
        }
        public DataSet GetSeatMapLayout(string flightId, string origin, string destination, string boardingClass, string configuration, string strLanguage)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetSeatMapLayout(flightId, origin, destination, boardingClass, configuration, strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    break;
            }

            return ds;
        }
        public string GetFormOfPayments(string language)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetFormOfPayments(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetFormOfPayments(language);
                    break;
            }
            return result;
        }
        public string GetFormOfPaymentSubTypes(string type, string language)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetFormOfPaymentSubTypes(type, language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetFormOfPaymentSubTypes(type, language);
                    break;
            }

            return result;
        }
        public string SaveBooking(string bookingId, string header, string segment, string passenger, string remark, string payment, string mapping, string service, string tax, string fee, bool createTickets, bool readBooking, bool readOnly, string strLanguage)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SaveBooking(bookingId, header, segment, passenger, remark, payment, mapping, service, tax, fee, createTickets, readBooking, readOnly, strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SaveBooking(bookingId, header, segment, passenger, remark, payment, mapping, service, tax, fee, createTickets, readBooking, readOnly, strLanguage);
                    break;
            }

            return result;
        }

        public bool SaveBookingHeader(string header)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SaveBookingHeader(header);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SaveBookingHeader(header);
                    break;
            }

            return result;
        }


        public string CalculateNewFees(string bookingId, string AgencyCode, string header, string segment, string passenger, string fees, string currency, string remark, string payment, string mapping, string service, string tax, bool checkBooking, bool checkSegment, bool checkName, bool checkSeat, string strLanguage, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {

                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.CalculateNewFees(bookingId, AgencyCode, header, segment, passenger, fees, currency, remark, payment, mapping, service, tax, checkBooking, checkSegment, checkName, checkSeat, strLanguage, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.CalculateNewFees(bookingId, AgencyCode, header, segment, passenger, fees, currency, remark, payment, mapping, service, tax, checkBooking, checkSegment, checkName, checkSeat, strLanguage, bNoVat);
                    break;
            }

            return result;
        }

        public double CalculateExchange(string currencyFrom, string currencyTo, double amount, string systemCurrency, DateTime dateOfExchange, bool reverse)
        {
            Double ret = 0D;
            switch (ConfigurationManager.AppSettings["Service"])
            {

                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ret = objAgent.CalculateExchange(currencyFrom, currencyTo, amount, systemCurrency, dateOfExchange, reverse);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ret = objtikAeroWebService.CalculateExchange(currencyFrom, currencyTo, amount, systemCurrency, dateOfExchange, reverse);
                    break;
            }

            return ret;
        }

        public string GetVouchers(string recordLocator, string voucherNumber, string voucherID, string status, string recipient, string fOPSubType, string clientProfileId, string currency, string password, bool includeOpenVoucher, bool includeExpiredVoucher, bool includeUsedVoucher, bool includeVoidedVoucher, bool includeRefundable, bool includeFareOnly, bool write, Mappings mappings, Fees fees)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetVouchers(recordLocator, voucherNumber, voucherID, status, recipient, fOPSubType, clientProfileId, currency, password, includeOpenVoucher, includeExpiredVoucher, includeUsedVoucher, includeVoidedVoucher, includeRefundable, includeFareOnly, write, mappings, fees);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetVouchers(recordLocator, voucherNumber, voucherID, status, recipient, fOPSubType, clientProfileId, currency, password, includeOpenVoucher, includeExpiredVoucher, includeUsedVoucher, includeVoidedVoucher, includeRefundable, includeFareOnly, write, mappings, fees);
                    break;
            }

            return result;
        }
        public bool SavePayment(string bookingId, Mappings mappings, Fees fees, Payments payment, Vouchers refundVoucher)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SavePayment(bookingId, mappings, fees, payment, refundVoucher);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SavePayment(bookingId, mappings, fees, payment, refundVoucher);
                    break;
            }

            return result;
        }
        public DataSet GetFormOfPaymentSubtypeFees(string formOfPayment, string formOfPaymentSubtype, string currencyRcd, string agency, DateTime feeDate)
        {
            DataSet ds = null;
            try
            {
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "1":
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        ds = objAgent.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubtype, currencyRcd, agency, feeDate);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        //Call new webservice
                        clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                        ds = objtikAeroWebService.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubtype, currencyRcd, agency, feeDate);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return ds;
        }

        public string GetItinerary(string bookingId, string language, string passengerId, string agencyCode)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetItinerary(bookingId, language, passengerId, agencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetItinerary(bookingId, language, passengerId, agencyCode);
                    break;
            }
            return result;
        }

        public string ViewBookingChange(string bookingId, string language, string agencyCode)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.ViewBookingChange(bookingId, language, agencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ViewBookingChange(bookingId, language, agencyCode);
                    break;
            }
            return result;
        }

        public string ItineraryRead(string recordLocator, string language, string passengerId, string agencyCode)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ItineraryRead(recordLocator, language, passengerId, agencyCode);
                    break;
            }
            return result;
        }

        public bool QueueMail(string strFromAddress, string strFromName, string strToAddress, string strToAddressCC, string strToAddressBCC, string strReplyToAddress, string strSubject, string strBody, string strDocumentType, string strAttachmentStream, string strAttachmentFileName, string strAttachmentFileType, string strAttachmentParser, bool bHtmlBody, bool bConvertAttachmentFromHTML2PDF, bool bRemoveFromQueue, string strUserId, string strBookingId, string strVoucherId, string strBookingSegmentID, string strPassengerId, string strClientProfileId, string strDocumentId, string strLanguageCode)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {

                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.QueueMail(strFromAddress,
                                                 strFromName,
                                                 strToAddress,
                                                 strToAddressCC,
                                                 strToAddressBCC,
                                                 strReplyToAddress,
                                                 strSubject,
                                                 strBody,
                                                 strDocumentType,
                                                 strAttachmentStream,
                                                 strAttachmentFileName,
                                                 strAttachmentFileType,
                                                 strAttachmentParser,
                                                 bHtmlBody,
                                                 bConvertAttachmentFromHTML2PDF,
                                                 bRemoveFromQueue,
                                                 strUserId,
                                                 strBookingId,
                                                 strVoucherId,
                                                 strBookingSegmentID,
                                                 strPassengerId,
                                                 strClientProfileId,
                                                 strDocumentId,
                                                 strLanguageCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.QueueMail(strFromAddress,
                                 strFromName,
                                 strToAddress,
                                 strToAddressCC,
                                 strToAddressBCC,
                                 strReplyToAddress,
                                 strSubject,
                                 strBody,
                                 strDocumentType,
                                 strAttachmentStream,
                                 strAttachmentFileName,
                                 strAttachmentFileType,
                                 strAttachmentParser,
                                 bHtmlBody,
                                 bConvertAttachmentFromHTML2PDF,
                                 bRemoveFromQueue,
                                 strUserId,
                                 strBookingId,
                                 strVoucherId,
                                 strBookingSegmentID,
                                 strPassengerId,
                                 strClientProfileId,
                                 strDocumentId,
                                 strLanguageCode);
                    break;
            }


            return result;
        }

        public DataSet ClientLogon(string ClientNumber, string ClientPassword)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.ClientLogon(ClientNumber, ClientPassword);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.ClientLogon(ClientNumber, ClientPassword);
                    break;
            }

            return ds;
        }

        public DataSet GetBookings(string Airline,
                                    string FlightNumber,
                                    string FlightId,
                                    DateTime FlightFrom,
                                    DateTime FlightTo,
                                    string RecordLocator,
                                    string Origin,
                                    string Destination,
                                    string PassengerName,
                                    string SeatNumber,
                                    string TicketNumber,
                                    string PhoneNumber,
                                    string AgencyCode,
                                    string ClientNumber,
                                    string MemberNumber,
                                    string ClientId,
                                    bool ShowHistory,
                                    string Language,
                                    bool bIndividual,
                                    bool bGroup,
                                    DateTime CreateFrom,
                                    DateTime CreateTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetBookings(Airline,
                                            FlightNumber,
                                            FlightId,
                                            FlightFrom,
                                            FlightTo,
                                            RecordLocator,
                                            Origin,
                                            Destination,
                                            PassengerName,
                                            SeatNumber,
                                            TicketNumber,
                                            PhoneNumber,
                                            AgencyCode,
                                            ClientNumber,
                                            MemberNumber,
                                            ClientId,
                                            ShowHistory,
                                            Language,
                                            bIndividual,
                                            bGroup,
                                            CreateFrom,
                                            CreateTo);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetBookings(Airline,
                                                        FlightNumber,
                                                        FlightId,
                                                        FlightFrom,
                                                        FlightTo,
                                                        RecordLocator,
                                                        Origin,
                                                        Destination,
                                                        PassengerName,
                                                        SeatNumber,
                                                        TicketNumber,
                                                        PhoneNumber,
                                                        AgencyCode,
                                                        ClientNumber,
                                                        MemberNumber,
                                                        ClientId,
                                                        ShowHistory,
                                                        Language,
                                                        bIndividual,
                                                        bGroup,
                                                        CreateFrom,
                                                        CreateTo);
                    break;
            }
            return ds;
        }

        public DataSet GetBookingsThisUser(string agencyCode, string userId, string airline, string flightNumber, DateTime flightFrom,
            DateTime flightTo, string recordLocator, string origin, string destination, string passengerName, string seatNumber,
            string ticketNumber, string phoneNumber, DateTime createFrom, DateTime createTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "0":
                    ds = new DataSet();
                    break;
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetBookingsThisUser(agencyCode, userId, airline, flightNumber, flightFrom,
                            flightTo, recordLocator, origin, destination, passengerName, seatNumber,
                            ticketNumber, phoneNumber, createFrom, createTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    ds = new DataSet();
                    break;
            }
            return ds;
        }

        public DataSet ClientRead(string clientProfileID)
        {
            DataSet ds = null;
            if (DataHelper.IsGUID(clientProfileID) == true)
            {
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "0":
                        ds = new DataSet();
                        break;
                    case "1":
                        //Call Old webservice
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        ds = objAgent.ClientRead(clientProfileID);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        ds = new DataSet();
                        break;
                }
            }

            return ds;
        }

        public bool AddClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.AddClientProfile(client, passengers, remarks);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.AddClientProfile(client, passengers, remarks);
                    break;
            }
            return result;
        }

        public bool EditClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.EditClientProfile(client, passengers, remarks);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.EditClientProfile(client, passengers, remarks);
                    break;
            }
            return result;
        }

        public bool ClientSave(string xmlClient, string xmlPassenger, string xmlBookingRemark)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.ClientSave(xmlClient, xmlPassenger, xmlBookingRemark);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.ClientSave(xmlClient, xmlPassenger, xmlBookingRemark);
                    break;
            }
            return result;
        }
        public bool AddClientPassengerList(string xmlClient, string xmlPassenger, string xmlBookingRemark)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.AddClientPassengerList(xmlClient, xmlPassenger, xmlBookingRemark);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.AddClientPassengerList(xmlClient, xmlPassenger, xmlBookingRemark);
                    break;
            }
            return result;
        }

        public DataSet MemberLevelRead(string strMemberLevelCode, string strMemberLevel, string strStatus, bool bWrite)
        {

            DataSet dsMember = null;

            AgentService objAgent = new AgentService();
            objAgent.objService = (TikAeroXMLwebservice)objService;
            dsMember = objAgent.MemberLevelRead(strMemberLevelCode, strMemberLevel, strStatus, bWrite);
            if (objAgent != null)
            {
                objAgent.Dispose();
            }
            return dsMember;
        }
        public DataSet PassengerRoleRead(string strPaxRoleCode, string strPaxRole, string strStatus, bool bWrite, string strLanguage)
        {

            DataSet dsMember = null;
            AgentService objAgent = new AgentService();
            objAgent.objService = (TikAeroXMLwebservice)objService;
            dsMember = objAgent.PassengerRoleRead(strPaxRoleCode, strPaxRole, strStatus, bWrite, strLanguage);

            if (objAgent != null)
            {
                objAgent.Dispose();
            }
            return dsMember;
        }
        public string GetTransaction(string strOrigin, string strDestination, string strAirline, string strFlight, string strSegmentType,
            string strClientProfileId, string strPassengerProfileId, string strVendor, string strCreditDebit, DateTime dtFlightFrom,
            DateTime dtFlightTo, DateTime dtTransactionFrom, DateTime dtTransactionTo, DateTime dtExpiryFrom, DateTime dtExpiryTo,
            DateTime dtVoidFrom, DateTime dtVoidTo, int iBatch, bool bAllVoid, bool bAllExpired, bool bAuto, bool bManual, bool bAllPoint)
        {
            string FFPXml = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "0":
                    FFPXml = "";
                    break;
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    FFPXml = objAgent.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType,
                        strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom,
                        dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo,
                        dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    FFPXml = objtikAeroWebService.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType,
                        strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom,
                        dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo,
                        dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);
                    break;
            }
            return FFPXml;
        }
        public DataSet TicketRead(ref string strBookingId, ref string strPassengerId, ref string strSegmentId, ref string strTicketNumber, ref string xmlTaxes,
            ref bool bReadOnly, ref bool bReturnTax)
        {
            DataSet dsTicket = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "0":
                    dsTicket = null;
                    break;
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsTicket = objAgent.TicketRead(ref strBookingId, ref strPassengerId, ref strSegmentId, ref strTicketNumber, ref xmlTaxes, ref bReadOnly, ref bReturnTax);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    dsTicket = null;
                    break;
            }
            return dsTicket;
        }
        public bool CheckUniqueMailAddress(string strMail, string strClientProfileId)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.CheckUniqueMailAddress(strMail, strClientProfileId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.CheckUniqueMailAddress(strMail, strClientProfileId);
                    break;
            }
            return result;
        }
        public DataSet AccuralQuote(string strPassenger, string strMapping, string strClientProfileId)
        {
            DataSet dsAccural = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsAccural = objAgent.AccuralQuote(strPassenger, strMapping, strClientProfileId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    dsAccural = objtikAeroWebService.AccuralQuote(strPassenger, strMapping, strClientProfileId);
                    break;
            }
            return dsAccural;
        }
        public DataSet GetFlightDailyCount(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo)
        {
            DataSet dsAccural = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsAccural = objAgent.GetFlightDailyCount(dtFrom, dtTo, strFrom, strTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    dsAccural = objtikAeroWebService.GetFlightDailyCount(dtFrom, dtTo, strFrom, strTo);
                    break;
            }
            return dsAccural;
        }
        public string GetFlightDailyCountXML(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo, string strToken)
        {
            string strAccural = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strAccural = objAgent.GetFlightDailyCountXML(dtFrom, dtTo, strFrom, strTo, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strAccural = objtikAeroWebService.GetFlightDailyCountXML(dtFrom, dtTo, strFrom, strTo, strToken);
                    break;
            }
            return strAccural;
        }
        public DataSet GetBinRangeSearch(string strCardType, string strStatusCode)
        {
            DataSet dsBin = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsBin = objAgent.GetBinRangeSearch(strCardType, strStatusCode);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    dsBin = objtikAeroWebService.GetBinRangeSearch(strCardType, strStatusCode);
                    break;
            }
            return dsBin;
        }
        public DataSet GetSessionlessBinRangeSearch(string strCardType, string strStatusCode, string strToken)
        {
            DataSet dsBin = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsBin = objAgent.GetSessionlessBinRangeSearch(strCardType, strStatusCode, strToken);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    dsBin = objtikAeroWebService.GetSessionlessBinRangeSearch(strCardType, strStatusCode, strToken);
                    break;
            }
            return dsBin;
        }
        public DataSet BookingLogon(string strRecordLocator, string strNameOrPhone)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.BookingLogon(strRecordLocator, strNameOrPhone);
                    break;
                case "2":
                    break;
            }
            return ds;
        }
        public string GetBookingSegmentCheckIn(string strBookingId, string strClientId, string strLanguageCode)
        {
            string str = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    str = objAgent.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    str = objtikAeroWebService.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
                    break;
            }
            return str;
        }

        public string BoardPassenger(string strFlightID, string strOrigin, string strBoard, string strUserId, bool bBoard)
        {
            string str = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    str = objAgent.BoardPassenger(strFlightID, strOrigin, strBoard, strUserId, bBoard);
                    break;
                case "2":
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //str = objtikAeroWebService.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
                    break;
            }
            return str;
        }

        public string OffLoadPassenger(string strBookingId, string strFlightId, string strPassengerId, bool autoBaggageFlag, string strUserId)
        {
            string str = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    str = objAgent.OffloadPassenger(strBookingId, strFlightId, strPassengerId, autoBaggageFlag, strUserId);
                    break;
                case "2":
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //str = objtikAeroWebService.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
                    break;
            }
            return str;
        }
        public string NoShowPassenger(string strBookingId, string strFlightId, string strPassengerId, bool autoBaggageFlag, string strUserId)
        {
            string str = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    str = objAgent.NoShowPassenger(strBookingId, strFlightId, strPassengerId, autoBaggageFlag, strUserId);
                    break;
                case "2":
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //str = objtikAeroWebService.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
                    break;
            }
            return str;
        }

        public string GetPassengerDetails(string strPassengerId,
                                          string strBookingSegmentId,
                                          string strFlightId,
                                          string strCheckinPassengers,
                                          bool bPassenger,
                                          bool bRemarks,
                                          bool bService,
                                          bool bBaggage,
                                          bool bSegment,
                                          bool bFee,
                                          bool bBookingDetails,
                                          string strLangaugeCode,
                                          string strOrigin)
        {
            string str = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    str = objAgent.GetPassengerDetails(strPassengerId,
                                                           strBookingSegmentId,
                                                           strFlightId,
                                                           strCheckinPassengers,
                                                           bPassenger,
                                                           bRemarks,
                                                           bService,
                                                           bBaggage,
                                                           bSegment,
                                                           bFee,
                                                           bBookingDetails,
                                                           strLangaugeCode,
                                                           strOrigin);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    str = objtikAeroWebService.GetPassengerDetails(strPassengerId,
                                                       strBookingSegmentId,
                                                       strFlightId,
                                                       strCheckinPassengers,
                                                       bPassenger,
                                                       bRemarks,
                                                       bService,
                                                       bBaggage,
                                                       bSegment,
                                                       bFee,
                                                       bBookingDetails,
                                                       strLangaugeCode,
                                                       strOrigin);
                    break;

            }
            return str;
        }
        public bool CheckInSave(string strMappingXml, string strBaggageXml, string strSeatAssignmentXml, string strPassengerXml, string strServiceXml, string strRemarkXml, string strBookingSegmentXml, string strFeeXml)
        {
            bool success = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    success = objAgent.CheckInSave(strMappingXml,
                                                strBaggageXml,
                                                strSeatAssignmentXml,
                                                strPassengerXml,
                                                strServiceXml,
                                                strRemarkXml,
                                                strBookingSegmentXml,
                                                strFeeXml);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    success = objtikAeroWebService.CheckInSave(strMappingXml,
                                            strBaggageXml,
                                            strSeatAssignmentXml,
                                            strPassengerXml,
                                            strServiceXml,
                                            strRemarkXml,
                                            strBookingSegmentXml,
                                            strFeeXml);
                    break;

            }
            return success;

        }
        public string SaveBookingCreditCard(string bookingId,
                                            BookingHeader header,
                                            Itinerary segment,
                                            Passengers passenger,
                                            Remarks remark,
                                            Payments payment,
                                            Mappings mapping,
                                            Services service,
                                            Taxes tax,
                                            Fees fee,
                                            Fees paymentFee,
                                            string securityToken,
                                            string authenticationToken,
                                            string commerceIndicator,
                                            string bookingReference,
                                            string requestSource,
                                            bool createTickets,
                                            bool readBooking,
                                            bool readOnly,
                                            string strLanguage)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SaveBookingCreditCard(bookingId,
                                                            header,
                                                            segment,
                                                            passenger,
                                                            remark,
                                                            payment,
                                                            mapping,
                                                            service,
                                                            tax,
                                                            fee,
                                                            paymentFee,
                                                            securityToken,
                                                            authenticationToken,
                                                            commerceIndicator,
                                                            bookingReference,
                                                            requestSource,
                                                            createTickets,
                                                            readBooking,
                                                            readOnly,
                                                            strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SaveBookingCreditCard(bookingId,
                                                                       header,
                                                                       segment,
                                                                       passenger,
                                                                       remark,
                                                                       payment,
                                                                       mapping,
                                                                       service,
                                                                       tax,
                                                                       fee,
                                                                       paymentFee,
                                                                       securityToken,
                                                                       authenticationToken,
                                                                       commerceIndicator,
                                                                       bookingReference,
                                                                       requestSource,
                                                                       createTickets,
                                                                       readBooking,
                                                                       readOnly,
                                                                       strLanguage);
                    break;

            }
            return result;
        }

        public string SaveBookingPayment(string bookingId,
                                           BookingHeader header,
                                           Itinerary segment,
                                           Passengers passenger,
                                           Remarks remark,
                                           Payments payment,
                                           Mappings mapping,
                                           Services service,
                                           Taxes tax,
                                           Fees fee,
                                           Fees paymentFee,
                                           bool createTickets,
                                           bool readBooking,
                                           bool readOnly,
                                           string strLanguage)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SaveBookingPayment(bookingId,
                                                         header,
                                                         segment,
                                                         passenger,
                                                         remark,
                                                         payment,
                                                         mapping,
                                                         service,
                                                         tax,
                                                         fee,
                                                         paymentFee,
                                                         createTickets,
                                                         readBooking,
                                                         readOnly,
                                                         strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SaveBookingPayment(bookingId,
                                                    header,
                                                    segment,
                                                    passenger,
                                                    remark,
                                                    payment,
                                                    mapping,
                                                    service,
                                                    tax,
                                                    fee,
                                                    paymentFee,
                                                    createTickets,
                                                    readBooking,
                                                    readOnly,
                                                    strLanguage);
                    break;

            }
            return result;
        }
        public string SaveBookingMultipleFormOfPayment(string bookingId,
                                                       BookingHeader header,
                                                       Itinerary segment,
                                                       Passengers passenger,
                                                       Remarks remark,
                                                       Payments payment,
                                                       Mappings mapping,
                                                       Services service,
                                                       Taxes tax,
                                                       Fees fee,
                                                       Fees paymentFee,
                                                       bool createTickets,
                                                       bool readBooking,
                                                       bool readOnly,
                                                       string securityToken,
                                                       string authenticationToken,
                                                       string commerceIndicator,
                                                       string strRequestSource,
                                                       string strLanguage)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SaveBookingMultipleFormOfPayment(bookingId,
                                                                         header,
                                                                         segment,
                                                                         passenger,
                                                                         remark,
                                                                         payment,
                                                                         mapping,
                                                                         service,
                                                                         tax,
                                                                         fee,
                                                                         paymentFee,
                                                                         createTickets,
                                                                         readBooking,
                                                                         readOnly,
                                                                         securityToken,
                                                                         authenticationToken,
                                                                         commerceIndicator,
                                                                         strRequestSource,
                                                                         strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SaveBookingMultipleFormOfPayment(bookingId,
                                                                                    header,
                                                                                    segment,
                                                                                    passenger,
                                                                                    remark,
                                                                                    payment,
                                                                                    mapping,
                                                                                    service,
                                                                                    tax,
                                                                                    fee,
                                                                                    paymentFee,
                                                                                    createTickets,
                                                                                    readBooking,
                                                                                    readOnly,
                                                                                    securityToken,
                                                                                    authenticationToken,
                                                                                    commerceIndicator,
                                                                                    strRequestSource,
                                                                                    strLanguage);
                    break;

            }
            return result;
        }

        public string GetServiceFeesByGroups(BookingHeader header, Itinerary itinerary, string serviceGroup)
        {
            string strResult = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.GetServiceFeesByGroups(header, itinerary, serviceGroup);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.GetServiceFeesByGroups(header, itinerary, serviceGroup);
                    break;
            }

            return strResult;
        }

        public Double ExchangeRateRead(string strOriginCurrencyCode, string strDestCurrencyCode)
        {
            Double ret = 0D;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "0":
                    break;
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ret = objAgent.GetExchangeRateRead(strOriginCurrencyCode, strDestCurrencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    break;
            }
            return ret;
        }
        // Start Yai Add Account Topup
        public bool AgencyAccountAdd(string strAgencyCode, string strCurrency, string strUserId, string strComment, double dAmount, string strExternalReference, string strInternalReference, string strTransactionReference, bool bExternalTopup)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.AgencyAccountAdd(strAgencyCode, strCurrency, strUserId, strComment, dAmount, strExternalReference, strInternalReference, strTransactionReference, bExternalTopup);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.AgencyAccountAdd(strAgencyCode, strCurrency, strUserId, strComment, dAmount, strExternalReference, strInternalReference, strTransactionReference, bExternalTopup);
                    break;

            }
            return bResult;
        }
        public bool AgencyAccountVoid(string strAgencyAccountId, string strUserId)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.AgencyAccountVoid(strAgencyAccountId, strUserId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.AgencyAccountVoid(strAgencyAccountId, strUserId);
                    break;
            }
            return bResult;
        }
        public DataSet ExternalPaymentListAgencyTopUp(string strAgencyCode)
        {
            DataSet dsResult = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    dsResult = objAgent.ExternalPaymentListAgencyTopUp(strAgencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    dsResult = objtikAeroWebService.ExternalPaymentListAgencyTopUp(strAgencyCode);
                    break;
            }
            return dsResult;
        }
        // End Yai Add Account Topup

        public string GetRecordLocator(ref string recordLocator, ref int bookingNumber)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetRecordLocator(ref recordLocator, ref bookingNumber);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    break;
            }
            return result;
        }
        public DataSet GetSessionlessOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    ds = objAgent.GetSessionlessOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetSessionlessOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                    break;
            }


            return ds;
        }

        public DataSet GetSessionlessDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    ds = objAgent.GetSessionlessDestination(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetSessionlessDestination(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                    break;
            }
            return ds;
        }

        // Tai Add New
        // Tai add GetTourOperators
        public string GetTourOperators(string language)
        {
            String strResult = "";
            //if (_objService != null)
            //{ strResult = _objService.GetTourOperators(language); }


            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.GetTourOperators(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                    //case "2":
                    ////Call new webservice
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //result = objtikAeroWebService.gett(clientId, clientNumber, passengerId, bShowRemark);
                    //break;
            }


            return strResult;
        }
        // End Tai GetTourOperators
        public string GetVendorTourOperator(string strVendorRcd)
        {
            String strResult = "";
            //if (_objService != null)
            //{ strResult = _objService.GetVendorTourOperator(strVendorRcd); }


            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.GetVendorTourOperator(strVendorRcd);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                    //case "2":
                    ////Call new webservice
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //result = objtikAeroWebService.gett(clientId, clientNumber, passengerId, bShowRemark);
                    //break;
            }


            return strResult;
        }
        public string GetTourOperatorCodeMappingRead(string strTourOperatorId, Boolean bInclude)
        {
            String strResult = "";

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.GetTourOperatorCodeMappingRead(strTourOperatorId, bInclude); ;
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                    //case "2":
                    ////Call new webservice
                    //clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    //result = objtikAeroWebService.gett(clientId, clientNumber, passengerId, bShowRemark);
                    //break;
            }

            return strResult;
        }
        //End Tai Add new
        public string GetActiveBookings(Guid gClientProfileId)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetActiveBookings(gClientProfileId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetActiveBookings(gClientProfileId);
                    break;

            }
            return result;
        }
        public string GetFlownBookings(Guid gClientProfileId)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetFlownBookings(gClientProfileId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetFlownBookings(gClientProfileId);
                    break;

            }
            return result;
        }
        public string GetBookingClasses(string strBoardingclass, string language)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetBookingClasses(strBoardingclass, language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetBookingClasses(strBoardingclass, language);
                    break;
            }

            return result;
        }
        public string GetBooking(Guid bookingId)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetBooking(bookingId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetBooking(bookingId);
                    break;
            }

            return result;
        }
        public string GetFlightsFLIFO(string originRcd,
                                        string destinationRcd,
                                        string airlineCode,
                                        string flightNumber,
                                        DateTime flightFrom,
                                        DateTime flightTo,
                                        string languageCode,
                                        string token)
        {
            string strResult = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.GetFlightsFLIFO(originRcd, destinationRcd, airlineCode, flightNumber, flightFrom, flightTo, languageCode, token);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.GetFlightsFLIFO(originRcd, destinationRcd, airlineCode, flightNumber, flightFrom, flightTo, languageCode, token);
                    break;
            }


            return strResult;
        }
        public string VoucherTemplateList(string voucherTemplateId, string voucherTemplate, DateTime fromDate, DateTime toDate, bool write, string status, string language)
        {
            string strResult = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.VoucherTemplateList(voucherTemplateId, voucherTemplate, fromDate, toDate, write, status, language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.VoucherTemplateList(voucherTemplateId, voucherTemplate, fromDate, toDate, write, status, language);
                    break;
            }


            return strResult;
        }

        public bool SaveVoucher(Vouchers vouchers)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.SaveVoucher(vouchers);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.SaveVoucher(vouchers);
                    break;
            }


            return bResult;
        }
        public string VoucherPaymentCreditCard(Payments payment, Vouchers vouchers)
        {
            string strResult = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.VoucherPaymentCreditCard(payment, vouchers);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.VoucherPaymentCreditCard(payment, vouchers);
                    break;
            }


            return strResult;
        }
        public string ReadVoucher(Guid voucherId, double voucherNumber)
        {
            string strResult = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strResult = objAgent.ReadVoucher(voucherId, voucherNumber);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.ReadVoucher(voucherId, voucherNumber);
                    break;
            }


            return strResult;
        }

        public bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.VoidVoucher(voucherId, userId, voidDate);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.VoidVoucher(voucherId, userId, voidDate);
                    break;
            }
            return bResult;
        }
        public bool AgencyRegistrationInsert(string agencyName,
                                            string legalName,
                                            string agencyType,
                                            string IATA,
                                            string taxId,
                                            string mail,
                                            string fax,
                                            string phone,
                                            string address1,
                                            string address2,
                                            string street,
                                            string state,
                                            string district,
                                            string province,
                                            string city,
                                            string zipCode,
                                            string poBox,
                                            string website,
                                            string contactPerson,
                                            string lastName,
                                            string firstName,
                                            string title,
                                            string userLogon,
                                            string password,
                                            string country,
                                            string currency,
                                            string language,
                                            string comment)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.AgencyRegistrationInsert(agencyName,
                                                                legalName,
                                                                agencyType,
                                                                IATA,
                                                                taxId,
                                                                mail,
                                                                fax,
                                                                phone,
                                                                address1,
                                                                address2,
                                                                street,
                                                                state,
                                                                district,
                                                                province,
                                                                city,
                                                                zipCode,
                                                                poBox,
                                                                website,
                                                                contactPerson,
                                                                lastName,
                                                                firstName,
                                                                title,
                                                                userLogon,
                                                                password,
                                                                country,
                                                                currency,
                                                                language,
                                                                comment);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.AgencyRegistrationInsert(agencyName,
                                                                legalName,
                                                                agencyType,
                                                                IATA,
                                                                taxId,
                                                                mail,
                                                                fax,
                                                                phone,
                                                                address1,
                                                                address2,
                                                                street,
                                                                state,
                                                                district,
                                                                province,
                                                                city,
                                                                zipCode,
                                                                poBox,
                                                                website,
                                                                contactPerson,
                                                                lastName,
                                                                firstName,
                                                                title,
                                                                userLogon,
                                                                password,
                                                                country,
                                                                currency,
                                                                language,
                                                                comment);
                    break;
            }
            return bResult;
        }
        public bool LowFareFinderAllow(string agencyCode, string token)
        {
            bool result = true;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.LowFareFinderAllow(agencyCode, token);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.LowFareFinderAllow(agencyCode, token);
                    break;
            }


            return result;
        }
        public string GetPassenger(string airline,
                                    string flightNumber,
                                    Guid flightID,
                                    DateTime flightFrom,
                                    DateTime flightTo,
                                    string recordLocator,
                                    string origin,
                                    string destination,
                                    string passengerName,
                                    string seatNumber,
                                    string ticketNumber,
                                    string phoneNumber,
                                    string passengerStatus,
                                    string checkInStatus,
                                    string clientNumber,
                                    string memberNumber,
                                    Guid clientID,
                                    Guid passengerId,
                                    bool booked,
                                    bool listed,
                                    bool eTicketOnly,
                                    bool includeCancelled,
                                    bool openSegments,
                                    bool showHistory,
                                    string language)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetPassenger(airline,
                                                   flightNumber,
                                                   flightID,
                                                   flightFrom,
                                                   flightTo,
                                                   recordLocator,
                                                   origin,
                                                   destination,
                                                   passengerName,
                                                   seatNumber,
                                                   ticketNumber,
                                                   phoneNumber,
                                                   passengerStatus,
                                                   checkInStatus,
                                                   clientNumber,
                                                   memberNumber,
                                                   clientID,
                                                   passengerId,
                                                   booked,
                                                   listed,
                                                   eTicketOnly,
                                                   includeCancelled,
                                                   openSegments,
                                                   showHistory,
                                                   language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetPassenger(airline,
                                                               flightNumber,
                                                               flightID,
                                                               flightFrom,
                                                               flightTo,
                                                               recordLocator,
                                                               origin,
                                                               destination,
                                                               passengerName,
                                                               seatNumber,
                                                               ticketNumber,
                                                               phoneNumber,
                                                               passengerStatus,
                                                               checkInStatus,
                                                               clientNumber,
                                                               memberNumber,
                                                               clientID,
                                                               passengerId,
                                                               booked,
                                                               listed,
                                                               eTicketOnly,
                                                               includeCancelled,
                                                               openSegments,
                                                               showHistory,
                                                               language);
                    break;

            }
            return result;
        }
        public string GetQueueCount(string agency, bool unassigned)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetQueueCount(agency, unassigned);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetQueueCount(agency, unassigned);
                    break;

            }
            return result;
        }
        public string BoardingClassRead(string boardingClassCode, string boardingClass, string sortSeq, string status, bool bWrite)
        {
            string strResult = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    strResult = objAgent.BoardingClassRead(boardingClassCode, boardingClass, sortSeq, status, bWrite);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strResult = objtikAeroWebService.BoardingClassRead(boardingClassCode, boardingClass, sortSeq, status, bWrite);
                    break;
            }
            return strResult;
        }
        #endregion

        #region Service Client Helper
        public void initializeWebService(string agencyCode, ref Agents agents)
        {
            WebService Service = new WebService();
            objService = Service.InitialWebServices(string.Empty, string.Empty, string.Empty, agencyCode, ref agents);
        }

        public void initializeWebService(string agencyCode, string agencyLogon, string agencyPassword, string selectedAgency, ref Agents agents)
        {
            WebService Service = new WebService();
            objService = Service.InitialWebServices(agencyCode, agencyLogon, agencyPassword, selectedAgency, ref agents);
        }
        #endregion

        #region IClientCore Report Members


        public DataSet GetTicketsIssued(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsIssued(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination,
                        strAgency, strAirline, strFlight);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsIssued(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination,
                        strAgency, strAirline, strFlight);
                    break;
            }

            return ds;
        }

        public DataSet GetTicketsUsed(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsUsed(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination,
                        strAgency, strAirline, strFlight);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsUsed(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination,
                        strAgency, strAirline, strFlight);
                    break;
            }

            return ds;
        }

        public DataSet GetTicketsRefunded(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsRefunded(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin,
                        strDestination, strAgency, strAirline, strFlight);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsRefunded(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin,
                        strDestination, strAgency, strAirline, strFlight);
                    break;
            }

            return ds;
        }

        public DataSet GetTicketsCancelled(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, int intTicketonly, int intRefundable, string strProfileID, string strTicketNumber, string strFirstName, string strLastName, string strPassengerId, string strBookingSegmentID)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsCancelled(strOrigin, strDestination, strAgency, strAirline, strFlight, dtReportFrom, dtReportTo,
                        dtFlightFrom, dtFlightTo, intTicketonly, intRefundable, strProfileID, strTicketNumber, strFirstName, strLastName,
                        strPassengerId, strBookingSegmentID);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsCancelled(strOrigin, strDestination, strAgency, strAirline, strFlight, dtReportFrom, dtReportTo,
                        dtFlightFrom, dtFlightTo, intTicketonly, intRefundable, strProfileID, strTicketNumber, strFirstName, strLastName,
                        strPassengerId, strBookingSegmentID);
                    break;
            }

            return ds;
        }

        public DataSet GetTicketsNotFlown(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, bool bUnflown, bool bNoShow)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsNotFlown(strOrigin, strDestination, strAgency, strAirline, strFlight, dtReportFrom,
                        dtReportTo, dtFlightFrom, dtFlightTo, bUnflown, bNoShow);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsNotFlown(strOrigin, strDestination, strAgency, strAirline, strFlight, dtReportFrom,
                        dtReportTo, dtFlightFrom, dtFlightTo, bUnflown, bNoShow);
                    break;
            }

            return ds;
        }

        public DataSet GetTicketsExpired(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketsExpired(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin,
                        strDestination, strAgency, strAirline, strFlight);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketsExpired(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin,
                        strDestination, strAgency, strAirline, strFlight);
                    break;
            }

            return ds;
        }

        public DataSet GetCashbookPayments(string strAgency, string strGroup, string strUserId, DateTime dtPaymentFrom, DateTime dtPaymentTo, string strCashbookId)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetCashbookPayments(strAgency, strGroup, strUserId, dtPaymentFrom, dtPaymentTo, strCashbookId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetCashbookPayments(strAgency, strGroup, strUserId, dtPaymentFrom, dtPaymentTo, strCashbookId);
                    break;
            }

            return ds;
        }

        public DataSet GetCashbookCharges(string XmlCashbookCharges, string strCashbookId)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetCashbookCharges(XmlCashbookCharges, strCashbookId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetCashbookCharges(XmlCashbookCharges, strCashbookId);
                    break;
            }

            return ds;
        }

        public DataSet GetBookingFeeAccounted(string strAgencyCode, string strUserId, string strFee, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetBookingFeeAccounted(strAgencyCode, strUserId, strFee, dtFrom, dtTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetBookingFeeAccounted(strAgencyCode, strUserId, strFee, dtFrom, dtTo);
                    break;
            }

            return ds;
        }

        public DataSet CreditCardPayment(ref string strCCNumber, ref string strTransType, ref string strTransStatus, ref DateTime dtFrom, ref DateTime dtTo, ref string strCCType, ref string strAgency)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.CreditCardPayment(ref strCCNumber, ref strTransType, ref strTransStatus, ref dtFrom, ref dtTo, ref strCCType, ref strAgency);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.CreditCardPayment(ref strCCNumber, ref strTransType, ref strTransStatus, ref dtFrom, ref dtTo, ref strCCType, ref strAgency);
                    break;
            }

            return ds;
        }

        public DataSet GetOutstanding(string strAgencyCode, string strAirline, string strFlightNumber, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, bool bOffices, bool bAgencies, bool bLastTwentyFourHours, bool bTicketedOnly, int iOlderThanHours, string strLanguage, bool bAccountsPayable)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetOutstanding(strAgencyCode, strAirline, strFlightNumber, dtFlightFrom, dtFlightTo,
                        strOrigin, strDestination, bOffices, bAgencies, bLastTwentyFourHours, bTicketedOnly, iOlderThanHours,
                        strLanguage, bAccountsPayable);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetOutstanding(strAgencyCode, strAirline, strFlightNumber, dtFlightFrom, dtFlightTo,
                        strOrigin, strDestination, bOffices, bAgencies, bLastTwentyFourHours, bTicketedOnly, iOlderThanHours,
                        strLanguage, bAccountsPayable);
                    break;
            }

            return ds;
        }

        public DataSet GetAgencyAccountTransactions(string agencyCode, DateTime dateFrom, DateTime dateTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAgencyAccountTransactions(agencyCode, dateFrom, dateTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAgencyAccountTransactions(agencyCode, dateFrom, dateTo);
                    break;
            }

            return ds;
        }

        public bool CompleteRemark(string XmlRemarks, string RemarkId, string UserId)
        {
            bool success = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    success = objAgent.CompleteRemark(XmlRemarks, RemarkId, UserId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    success = objtikAeroWebService.CompleteRemark(XmlRemarks, RemarkId, UserId);
                    break;
            }

            return success;
        }

        public DataSet GetActivities(string AgencyCode, string RemarkType, string Nickname, DateTime TimelimitFrom, DateTime TimelimitTo, bool PendingOnly, bool IncompleteOnly, bool IncludeRemarks, bool showUnassigned)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetActivities(AgencyCode, RemarkType, Nickname, TimelimitFrom, TimelimitTo, PendingOnly, IncompleteOnly, IncludeRemarks, showUnassigned);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetActivities(AgencyCode, RemarkType, Nickname, TimelimitFrom, TimelimitTo, PendingOnly, IncompleteOnly, IncludeRemarks, showUnassigned);
                    break;
            }

            return ds;
        }

        public DataSet GetCashbookPaymentsSummary(string XmlCashbookPaymentsAll)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetCashbookPaymentsSummary(XmlCashbookPaymentsAll);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetCashbookPaymentsSummary(XmlCashbookPaymentsAll);
                    break;
            }

            return ds;
        }

        public DataSet GetAgencyAccountTopUp(string strAgencyCode, string strCurrency, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAgencyAccountTopUp(strAgencyCode, strCurrency, dtFrom, dtTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAgencyAccountTopUp(strAgencyCode, strCurrency, dtFrom, dtTo);
                    break;
            }

            return ds;
        }

        public Users TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword)
        {
            Users users = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    users = objAgent.TravelAgentLogon(agencyCode, agentLogon, agentPassword);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    users = objtikAeroWebService.TravelAgentLogon(agencyCode, agentLogon, agentPassword);
                    break;
            }

            return users;
        }

        public void InitializeUserAccountID(string UserAccountId)
        {
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    objAgent.InitializeUserAccountID(UserAccountId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    objtikAeroWebService.InitializeUserAccountID(UserAccountId);
                    break;
            }
        }

        public Agents GetAgencySessionProfile(string AgencyCode, string UserAccountID)
        {
            if (string.IsNullOrEmpty(UserAccountID) == false && DataHelper.IsGUID(UserAccountID) == false)
            {
                return null;
            }
            else
            {
                Agents agents = null;
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "1":
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        agents = objAgent.GetAgencySessionProfile(AgencyCode, UserAccountID);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        //Call new webservice
                        clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                        agents = objtikAeroWebService.GetAgencySessionProfile(AgencyCode, UserAccountID);
                        break;
                }

                return agents;
            }
        }

        public DataSet GetCorporateSessionProfile(string clientId, string LastName)
        {
            if (string.IsNullOrEmpty(clientId) == false && DataHelper.IsGUID(clientId) == false)
            {
                return null;
            }
            else
            {
                DataSet ds = null;
                switch (ConfigurationManager.AppSettings["Service"])
                {
                    case "1":
                        AgentService objAgent = new AgentService();
                        objAgent.objService = (TikAeroXMLwebservice)objService;
                        ds = objAgent.GetCorporateSessionProfile(clientId, LastName);
                        if (objAgent != null)
                        {
                            objAgent.Dispose();
                        }
                        break;
                    case "2":
                        //Call new webservice
                        clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                        ds = objtikAeroWebService.GetCorporateSessionProfile(clientId, LastName);
                        break;
                }

                return ds;
            }
        }

        public DataSet GetTicketSales(string AgencyCode, string UserId, string Origin, string Destination, string Airline, string FlightNumber, DateTime FlightFrom, DateTime FlightTo, DateTime TicketingFrom, DateTime TicketingTo, string PassengerType, string Language)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetTicketSales(AgencyCode, UserId, Origin, Destination, Airline, FlightNumber, FlightFrom,
                        FlightTo, TicketingFrom, TicketingTo, PassengerType, Language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetTicketSales(AgencyCode, UserId, Origin, Destination, Airline, FlightNumber, FlightFrom,
                        FlightTo, TicketingFrom, TicketingTo, PassengerType, Language);
                    break;
            }

            return ds;
        }

        public DataSet GetAgencyTicketSales(string strAgency, string strCurrency, DateTime dtSalesFrom, DateTime dtSalesTo)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAgencyTicketSales(strAgency, strCurrency, dtSalesFrom, dtSalesTo);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAgencyTicketSales(strAgency, strCurrency, dtSalesFrom, dtSalesTo);
                    break;
            }

            return ds;
        }

        public DataSet GetServiceFees(ref string strOrigin, ref string strDestination, ref string strCurrency, ref string strAgency, ref string strServiceGroup, ref DateTime dtFee)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetServiceFees(ref strOrigin, ref strDestination, ref strCurrency, ref strAgency, ref strServiceGroup, ref dtFee);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetServiceFees(ref strOrigin, ref strDestination, ref strCurrency, ref strAgency, ref strServiceGroup, ref dtFee);
                    break;
            }

            return ds;
        }

        public DataSet GetClientSessionProfile(string clientProfileId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ClientCore B2A
        public DataSet GetAgencyAccountBalance(string strAgencyCode, string strAgencyName, string strCurrency, string strConsolidatorAgency)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetAgencyAccountBalance(strAgencyCode, strAgencyName, strCurrency, strConsolidatorAgency);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetAgencyAccountBalance(strAgencyCode, strAgencyName, strCurrency, strConsolidatorAgency);
                    break;
            }
            return ds;
        }

        public bool AddNewAgency(string strXmlAgency, string strAgencyCode, string strDefaultUserLogon, string strDefaultPassword, string strDefaultLastName, string strDefaultFirstName, string strCreateUser, short iChangeBooking, short iChangeSegment, short iDeleteSegment, short iTicketIssue)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.AddNewAgency(strXmlAgency, strAgencyCode, strDefaultUserLogon, strDefaultPassword, strDefaultLastName, strDefaultFirstName, strCreateUser, iChangeBooking, iChangeSegment, iDeleteSegment, iTicketIssue);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.AddNewAgency(strXmlAgency, strAgencyCode, strDefaultUserLogon, strDefaultPassword, strDefaultLastName, strDefaultFirstName, strCreateUser, iChangeBooking, iChangeSegment, iDeleteSegment, iTicketIssue);
                    break;
            }
            return bResult;
        }

        public bool AdjustSubAgencyAccountBalance(string strXmlChildAccountBalance, string strXmlParentAccountBalance)
        {
            bool bResult = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    bResult = objAgent.AdjustSubAgencyAccountBalance(strXmlChildAccountBalance, strXmlParentAccountBalance);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    bResult = objtikAeroWebService.AdjustSubAgencyAccountBalance(strXmlChildAccountBalance, strXmlParentAccountBalance);
                    break;
            }
            return bResult;
        }

        public DataSet AgencyRead(string strAgencyCode, bool bKeepSession)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.AgencyRead(strAgencyCode, bKeepSession);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.AgencyRead(strAgencyCode, bKeepSession);
                    break;
            }
            return ds;
        }
        #endregion

        #region IClientCore User Members

        public string GetUserList(string UserLogon, string UserCode, string LastName, string FirstName, string AgencyCode, string StatusCode)
        {
            string result = String.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetUserList(UserLogon, UserCode, LastName, FirstName, AgencyCode, StatusCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetUserList(UserLogon, UserCode, LastName, FirstName, AgencyCode, StatusCode);
                    break;
            }

            return result;
        }

        public string UserRead(string UserID)
        {
            string result = String.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.UserRead(UserID);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.UserRead(UserID);
                    break;
            }

            return result;
        }

        public bool UserSave(string strUsersXml, string agencyCode)
        {
            bool success = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    success = objAgent.UserSave(strUsersXml, agencyCode);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    success = objtikAeroWebService.UserSave(strUsersXml, agencyCode);
                    break;
            }

            return success;
        }
        public string GetAvailabilityAgencyLogin(string Origin,
                                                string Destination,
                                                DateTime DepartDateFrom,
                                                DateTime DepartDateTo,
                                                DateTime ReturnDateFrom,
                                                DateTime ReturnDateTo,
                                                DateTime DateBooking,
                                                short Adult,
                                                short Child,
                                                short Infant,
                                                short Other,
                                                string OtherPassengerType,
                                                string BoardingClass,
                                                string BookingClass,
                                                string DayTimeIndicator,
                                                string AgencyCode,
                                                string Password,
                                                string FlightId,
                                                string FareId,
                                                double MaxAmount,
                                                bool NonStopOnly,
                                                bool IncludeDeparted,
                                                bool IncludeCancelled,
                                                bool IncludeWaitlisted,
                                                bool IncludeSoldOut,
                                                bool Refundable,
                                                bool GroupFares,
                                                bool ItFaresOnly,
                                                bool bStaffFares,
                                                bool bApplyFareLogic,
                                                bool bUnknownTransit,
                                                string strTransitPoint,
                                                DateTime dteReturnFrom,
                                                DateTime dteReturnTo,
                                                string dtReturn,
                                                bool bMapWithFares,
                                                bool bReturnRefundable,
                                                string strReturnDayTimeIndicator,
                                                string PromotionCode,
                                                short iFareLogic,
                                                string strSearchType,
                                                bool bLowest,
                                                bool bLowestClass,
                                                bool bLowestGroup,
                                                bool bShowClosed,
                                                bool bSort,
                                                bool bDelet,
                                                bool bSkipFarelogic,
                                                string strLanguage,
                                                string strIpAddress,
                                                ref string strCurrencyCode,
                                                bool bNoVat)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetAvailabilityAgencyLogin(Origin,
                                                                Destination,
                                                                DepartDateFrom,
                                                                DepartDateTo,
                                                                ReturnDateFrom,
                                                                ReturnDateTo,
                                                                DateBooking,
                                                                Adult,
                                                                Child,
                                                                Infant,
                                                                Other,
                                                                OtherPassengerType,
                                                                BoardingClass,
                                                                BookingClass,
                                                                DayTimeIndicator,
                                                                AgencyCode,
                                                                Password,
                                                                FlightId,
                                                                FareId,
                                                                MaxAmount,
                                                                NonStopOnly,
                                                                IncludeDeparted,
                                                                IncludeCancelled,
                                                                IncludeWaitlisted,
                                                                IncludeSoldOut,
                                                                Refundable,
                                                                GroupFares,
                                                                ItFaresOnly,
                                                                bStaffFares,
                                                                bApplyFareLogic,
                                                                bUnknownTransit,
                                                                strTransitPoint,
                                                                dteReturnFrom,
                                                                dteReturnTo,
                                                                dtReturn,
                                                                bMapWithFares,
                                                                bReturnRefundable,
                                                                strReturnDayTimeIndicator,
                                                                PromotionCode,
                                                                iFareLogic,
                                                                strSearchType,
                                                                bLowest,
                                                                bLowestClass,
                                                                bLowestGroup,
                                                                bShowClosed,
                                                                bSort,
                                                                bDelet,
                                                                bSkipFarelogic,
                                                                strLanguage,
                                                                strIpAddress,
                                                                ref strCurrencyCode,
                                                                bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetAvailabilityAgencyLogin(Origin,
                                                                            Destination,
                                                                            DepartDateFrom,
                                                                            DepartDateTo,
                                                                            ReturnDateFrom,
                                                                            ReturnDateTo,
                                                                            DateBooking,
                                                                            Adult,
                                                                            Child,
                                                                            Infant,
                                                                            Other,
                                                                            OtherPassengerType,
                                                                            BoardingClass,
                                                                            BookingClass,
                                                                            DayTimeIndicator,
                                                                            AgencyCode,
                                                                            Password,
                                                                            FlightId,
                                                                            FareId,
                                                                            MaxAmount,
                                                                            NonStopOnly,
                                                                            IncludeDeparted,
                                                                            IncludeCancelled,
                                                                            IncludeWaitlisted,
                                                                            IncludeSoldOut,
                                                                            Refundable,
                                                                            GroupFares,
                                                                            ItFaresOnly,
                                                                            bStaffFares,
                                                                            bApplyFareLogic,
                                                                            bUnknownTransit,
                                                                            strTransitPoint,
                                                                            dteReturnFrom,
                                                                            dteReturnTo,
                                                                            dtReturn,
                                                                            bMapWithFares,
                                                                            bReturnRefundable,
                                                                            strReturnDayTimeIndicator,
                                                                            PromotionCode,
                                                                            iFareLogic,
                                                                            strSearchType,
                                                                            bLowest,
                                                                            bLowestClass,
                                                                            bLowestGroup,
                                                                            bShowClosed,
                                                                            bSort,
                                                                            bDelet,
                                                                            bSkipFarelogic,
                                                                            strLanguage,
                                                                            strIpAddress,
                                                                            ref strCurrencyCode,
                                                                            bNoVat);
                    break;
            }

            return result;
        }
        public string CalculateSpecialServiceFees(string AgencyCode, string currency, string bookingId, string header, string service, string fees, string remark, string mapping, string strLanguage, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.CalculateSpecialServiceFees(AgencyCode, currency, bookingId, header, service, fees, remark, mapping, strLanguage, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.CalculateSpecialServiceFees(AgencyCode, currency, bookingId, header, service, fees, remark, mapping, strLanguage, bNoVat);
                    break;
            }

            return result;
        }
        public string SavePayment(string bookingId,
                                 BookingHeader header,
                                 Itinerary segment,
                                 Passengers passenger,
                                 Payments payment,
                                 Mappings mapping,
                                 Fees fee,
                                 Fees paymentFee,
                                 bool createTickets)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SavePayment(bookingId, header, segment, passenger, payment, mapping, fee, paymentFee, createTickets);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SavePayment(bookingId, header, segment, passenger, payment, mapping, fee, paymentFee, createTickets);
                    break;
            }

            return result;
        }
        public string SingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SingleFlightQuoteSummary(flights, passengers, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
                    if (objAgent != null)
                    { objAgent.Dispose(); }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SingleFlightQuoteSummary(flights, passengers, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
                    break;
            }

            return result;
        }
        public string SessionlessSingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strToken, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    using (AgentService objAgent = new AgentService())
                    {
                        result = objAgent.SessionlessSingleFlightQuoteSummary(flights, passengers, strAgencyCode, strToken, strLanguage, strCurrencyCode, bNoVat);
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SessionlessSingleFlightQuoteSummary(flights, passengers, strAgencyCode, strToken, strLanguage, strCurrencyCode, bNoVat);
                    break;
            }

            return result;
        }
        public DataSet GetBookingHistory(string bookingId)
        {
            DataSet ds = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ds = objAgent.GetBookingHistory(bookingId);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ds = objtikAeroWebService.GetBookingHistory(bookingId);
                    break;
            }

            return ds;
        }
        public string SegmentFee(string strAgencyCode, string strCurrency, string[] strFeeRcdGroup, Mappings mappings, int iPassengerNumber, int iInfantNumber, string strLanguage, bool specialService, bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SegmentFee(strAgencyCode, strCurrency, strFeeRcdGroup, mappings, iPassengerNumber, iInfantNumber, strLanguage, specialService, bNoVat);
                    if (objAgent != null)
                    { objAgent.Dispose(); }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SegmentFee(strAgencyCode, strCurrency, strFeeRcdGroup, mappings, iPassengerNumber, iInfantNumber, strLanguage, specialService, bNoVat);
                    break;
            }

            return result;
        }
        public string SpecialServiceRead(string strSpecialServiceCode,
                                         string strSpecialServiceGroupCode,
                                         string strSpecialService,
                                         string strStatus,
                                         bool bTextAllowed,
                                         bool bTextRequired,
                                         bool bInventoryControl,
                                         bool bServiceOnRequest,
                                         bool bManifest,
                                         bool bWrite,
                                         string strLanguage)
        {

            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.SpecialServiceRead(strSpecialServiceCode,
                                                                strSpecialServiceGroupCode,
                                                                strSpecialService,
                                                                strStatus,
                                                                bTextAllowed,
                                                                bTextRequired,
                                                                bInventoryControl,
                                                                bServiceOnRequest,
                                                                bManifest,
                                                                bWrite,
                                                                strLanguage);
                    if (objAgent != null)
                    { objAgent.Dispose(); }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SpecialServiceRead(strSpecialServiceCode,
                                                                strSpecialServiceGroupCode,
                                                                strSpecialService,
                                                                strStatus,
                                                                bTextAllowed,
                                                                bTextRequired,
                                                                bInventoryControl,
                                                                bServiceOnRequest,
                                                                bManifest,
                                                                bWrite,
                                                                strLanguage);
                    break;
            }

            return result;
        }
        public Currencies GetCurrencies(string language)
        {
            Currencies currencies = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    currencies = objAgent.GetCurrencies(language);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    currencies = objtikAeroWebService.GetCurrencies(language);
                    break;
            }

            return currencies;
        }
        public string GetSessionlessCurrencies(string language, string strToken)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.GetSessionlessCurrencies(language, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetSessionlessCurrencies(language, strToken);
                    break;
            }

            return result;
        }

        public bool InsertPaymentApproval(string strPaymentApprovalID, string strCardRcd, string strCardNumber, string strNameOnCard, int iExpiryMonth, int iExpiryYear, int iIssueMonth, int iIssueYear, int iIssueNumber, string strPaymentStateCode, string strBookingPaymentId, string strCurrencyRcd, double dPaymentAmount, string strIpAddress)
        {
            bool success = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    success = objAgent.InsertPaymentApproval(strPaymentApprovalID,
                        strCardRcd,
                        strCardNumber,
                        strNameOnCard,
                        iExpiryMonth,
                        iExpiryYear,
                        iIssueMonth,
                        iIssueYear,
                        iIssueNumber,
                        strPaymentStateCode,
                        strBookingPaymentId,
                        strCurrencyRcd,
                        dPaymentAmount,
                        strIpAddress);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    success = objtikAeroWebService.InsertPaymentApproval(strPaymentApprovalID,
                        strCardRcd,
                        strCardNumber,
                        strNameOnCard,
                        iExpiryMonth,
                        iExpiryYear,
                        iIssueMonth,
                        iIssueYear,
                        iIssueNumber,
                        strPaymentStateCode,
                        strBookingPaymentId,
                        strCurrencyRcd,
                        dPaymentAmount,
                        strIpAddress);
                    break;

            }
            return success;

        }
        public bool UpdatePaymentApproval(string strApprovalCode,
                    string strPaymentReference,
                    string strTransactionReference,
                    string strTransactionDescription,
                    string strAvsCode,
                    string strAvsResponse,
                    string strCvvCode,
                    string strCvvResponse,
                    string strErrorCode,
                    string strErrorResponse,
                    string strPaymentStateCode,
                    string strResponseCode,
                    string strReturnCode,
                    string strResponseText,
                    string strPaymentApprovalId,
                    string strRequestStreamText,
                    string strReplyStreamText,
                    string strCardNumber,
                    string strCardType)
        {
            bool success = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    success = objAgent.UpdatePaymentApproval(strApprovalCode,
                        strPaymentReference,
                        strTransactionReference,
                        strTransactionDescription,
                        strAvsCode,
                        strAvsResponse,
                        strCvvCode,
                        strCvvResponse,
                        strErrorCode,
                        strErrorResponse,
                        strPaymentStateCode,
                        strResponseCode,
                        strReturnCode,
                        strResponseText,
                        strPaymentApprovalId,
                        strRequestStreamText,
                        strReplyStreamText,
                        strCardNumber,
                        strCardType);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    success = objtikAeroWebService.UpdatePaymentApproval(strApprovalCode,
                        strPaymentReference,
                        strTransactionReference,
                        strTransactionDescription,
                        strAvsCode,
                        strAvsResponse,
                        strCvvCode,
                        strCvvResponse,
                        strErrorCode,
                        strErrorResponse,
                        strPaymentStateCode,
                        strResponseCode,
                        strReturnCode,
                        strResponseText,
                        strPaymentApprovalId,
                        strRequestStreamText,
                        strReplyStreamText,
                        strCardNumber,
                        strCardType);
                    break;

            }
            return success;
        }

        public Double GetExchangeRateRead(string strOriginCurrencyCode, string strDestCurrencyCode)
        {
            Double ret = 0D;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    ret = objAgent.GetExchangeRateRead(strOriginCurrencyCode, strDestCurrencyCode);
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    ret = objtikAeroWebService.GetExchangeRateRead(strOriginCurrencyCode, strDestCurrencyCode);
                    break;
            }
            return ret;
        }
        public string GetFlightAvailability(string Origin,
                                                string Destination,
                                                DateTime DateDepartFrom,
                                                DateTime DateDepartTo,
                                                DateTime DateReturnFrom,
                                                DateTime DateReturnTo,
                                                DateTime DateBooking,
                                                short Adult,
                                                short Child,
                                                short Infant,
                                                short Other,
                                                string OtherPassengerType,
                                                string BoardingClass,
                                                string BookingClass,
                                                string DayTimeIndicator,
                                                string AgencyCode,
                                                string CurrencyCode,
                                                string FlightId,
                                                string FareId,
                                                double MaxAmount,
                                                bool NonStopOnly,
                                                bool IncludeDeparted,
                                                bool IncludeCancelled,
                                                bool IncludeWaitlisted,
                                                bool IncludeSoldOut,
                                                bool Refundable,
                                                bool GroupFares,
                                                bool ItFaresOnly,
                                                string PromotionCode,
                                                string FareType,
                                                bool FareLogic,
                                                bool ReturnFlight,
                                                bool bLowest,
                                                bool bLowestClass,
                                                bool bLowestGroup,
                                                bool bShowClosed,
                                                bool bSort,
                                                bool bDelete,
                                                bool bSkipFareLogic,
                                                string strLanguage,
                                                string strIpAddress,
                                                bool bReturnRefundable,
                                                bool bNoVat,
                                                Int32 iDayRange)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetFlightAvailability(Origin,
                                                                Destination,
                                                                DateDepartFrom,
                                                                DateDepartTo,
                                                                DateReturnFrom,
                                                                DateReturnTo,
                                                                DateBooking,
                                                                Adult,
                                                                Child,
                                                                Infant,
                                                                Other,
                                                                OtherPassengerType,
                                                                BoardingClass,
                                                                BookingClass,
                                                                DayTimeIndicator,
                                                                AgencyCode,
                                                                CurrencyCode,
                                                                FlightId,
                                                                FareId,
                                                                MaxAmount,
                                                                NonStopOnly,
                                                                IncludeDeparted,
                                                                IncludeCancelled,
                                                                IncludeWaitlisted,
                                                                IncludeSoldOut,
                                                                Refundable,
                                                                GroupFares,
                                                                ItFaresOnly,
                                                                PromotionCode,
                                                                FareType,
                                                                FareLogic,
                                                                ReturnFlight,
                                                                bLowest,
                                                                bLowestClass,
                                                                bLowestGroup,
                                                                bShowClosed,
                                                                bSort,
                                                                bDelete,
                                                                bSkipFareLogic,
                                                                strLanguage,
                                                                strIpAddress,
                                                                bReturnRefundable,
                                                                bNoVat,
                                                                iDayRange);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetFlightAvailability(Origin,
                                                                            Destination,
                                                                            DateDepartFrom,
                                                                            DateDepartTo,
                                                                            DateReturnFrom,
                                                                            DateReturnTo,
                                                                            DateBooking,
                                                                            Adult,
                                                                            Child,
                                                                            Infant,
                                                                            Other,
                                                                            OtherPassengerType,
                                                                            BoardingClass,
                                                                            BookingClass,
                                                                            DayTimeIndicator,
                                                                            AgencyCode,
                                                                            CurrencyCode,
                                                                            FlightId,
                                                                            FareId,
                                                                            MaxAmount,
                                                                            NonStopOnly,
                                                                            IncludeDeparted,
                                                                            IncludeCancelled,
                                                                            IncludeWaitlisted,
                                                                            IncludeSoldOut,
                                                                            Refundable,
                                                                            GroupFares,
                                                                            ItFaresOnly,
                                                                            PromotionCode,
                                                                            FareType,
                                                                            FareLogic,
                                                                            ReturnFlight,
                                                                            bLowest,
                                                                            bLowestClass,
                                                                            bLowestGroup,
                                                                            bShowClosed,
                                                                            bSort,
                                                                            bDelete,
                                                                            bSkipFareLogic,
                                                                            strLanguage,
                                                                            strIpAddress,
                                                                            bReturnRefundable,
                                                                            bNoVat,
                                                                            iDayRange);
                    break;
            }


            return result;
        }

        public string GetLowFareFinder(string Origin,
                                        string Destination,
                                        DateTime DateDepartFrom,
                                        DateTime DateDepartTo,
                                        DateTime DateReturnFrom,
                                        DateTime DateReturnTo,
                                        DateTime DateBooking,
                                        short Adult,
                                        short Child,
                                        short Infant,
                                        short Other,
                                        string OtherPassengerType,
                                        string BoardingClass,
                                        string BookingClass,
                                        string DayTimeIndicator,
                                        string AgencyCode,
                                        string CurrencyCode,
                                        string FlightId,
                                        string FareId,
                                        double MaxAmount,
                                        bool NonStopOnly,
                                        bool IncludeDeparted,
                                        bool IncludeCancelled,
                                        bool IncludeWaitlisted,
                                        bool IncludeSoldOut,
                                        bool Refundable,
                                        bool GroupFares,
                                        bool ItFaresOnly,
                                        string PromotionCode,
                                        string FareType,
                                        bool FareLogic,
                                        bool ReturnFlight,
                                        bool bLowest,
                                        bool bLowestClass,
                                        bool bLowestGroup,
                                        bool bShowClosed,
                                        bool bSort,
                                        bool bDelete,
                                        bool bSkipFareLogic,
                                        string strLanguage,
                                        string strIpAddress,
                                        bool bReturnRefundable,
                                        bool bNoVat,
                                        Int32 iDayRange)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetLowFareFinder(Origin,
                                                        Destination,
                                                        DateDepartFrom,
                                                        DateDepartTo,
                                                        DateReturnFrom,
                                                        DateReturnTo,
                                                        DateBooking,
                                                        Adult,
                                                        Child,
                                                        Infant,
                                                        Other,
                                                        OtherPassengerType,
                                                        BoardingClass,
                                                        BookingClass,
                                                        DayTimeIndicator,
                                                        AgencyCode,
                                                        CurrencyCode,
                                                        FlightId,
                                                        FareId,
                                                        MaxAmount,
                                                        NonStopOnly,
                                                        IncludeDeparted,
                                                        IncludeCancelled,
                                                        IncludeWaitlisted,
                                                        IncludeSoldOut,
                                                        Refundable,
                                                        GroupFares,
                                                        ItFaresOnly,
                                                        PromotionCode,
                                                        FareType,
                                                        FareLogic,
                                                        ReturnFlight,
                                                        bLowest,
                                                        bLowestClass,
                                                        bLowestGroup,
                                                        bShowClosed,
                                                        bSort,
                                                        bDelete,
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetLowFareFinder(Origin,
                                                                    Destination,
                                                                    DateDepartFrom,
                                                                    DateDepartTo,
                                                                    DateReturnFrom,
                                                                    DateReturnTo,
                                                                    DateBooking,
                                                                    Adult,
                                                                    Child,
                                                                    Infant,
                                                                    Other,
                                                                    OtherPassengerType,
                                                                    BoardingClass,
                                                                    BookingClass,
                                                                    DayTimeIndicator,
                                                                    AgencyCode,
                                                                    CurrencyCode,
                                                                    FlightId,
                                                                    FareId,
                                                                    MaxAmount,
                                                                    NonStopOnly,
                                                                    IncludeDeparted,
                                                                    IncludeCancelled,
                                                                    IncludeWaitlisted,
                                                                    IncludeSoldOut,
                                                                    Refundable,
                                                                    GroupFares,
                                                                    ItFaresOnly,
                                                                    PromotionCode,
                                                                    FareType,
                                                                    FareLogic,
                                                                    ReturnFlight,
                                                                    bLowest,
                                                                    bLowestClass,
                                                                    bLowestGroup,
                                                                    bShowClosed,
                                                                    bSort,
                                                                    bDelete,
                                                                    bSkipFareLogic,
                                                                    strLanguage,
                                                                    strIpAddress,
                                                                    bReturnRefundable,
                                                                    bNoVat,
                                                                    iDayRange);
                    break;
            }


            return result;
        }
        public string GetSessionlessFlightAvailability(string Origin,
                                                            string Destination,
                                                            DateTime DateDepartFrom,
                                                            DateTime DateDepartTo,
                                                            DateTime DateReturnFrom,
                                                            DateTime DateReturnTo,
                                                            DateTime DateBooking,
                                                            short Adult,
                                                            short Child,
                                                            short Infant,
                                                            short Other,
                                                            string OtherPassengerType,
                                                            string BoardingClass,
                                                            string BookingClass,
                                                            string DayTimeIndicator,
                                                            string AgencyCode,
                                                            string CurrencyCode,
                                                            string FlightId,
                                                            string FareId,
                                                            double MaxAmount,
                                                            bool NonStopOnly,
                                                            bool IncludeDeparted,
                                                            bool IncludeCancelled,
                                                            bool IncludeWaitlisted,
                                                            bool IncludeSoldOut,
                                                            bool Refundable,
                                                            bool GroupFares,
                                                            bool ItFaresOnly,
                                                            string PromotionCode,
                                                            string FareType,
                                                            bool FareLogic,
                                                            bool ReturnFlight,
                                                            bool bLowest,
                                                            bool bLowestClass,
                                                            bool bLowestGroup,
                                                            bool bShowClosed,
                                                            bool bSort,
                                                            bool bDelete,
                                                            bool bSkipFareLogic,
                                                            string strLanguage,
                                                            string strIpAddress,
                                                            string strToken,
                                                            bool bReturnRefundable,
                                                            bool bNoVat,
                                                            Int32 iDayRange)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.GetSessionlessFlightAvailability(Origin,
                                                                        Destination,
                                                                        DateDepartFrom,
                                                                        DateDepartTo,
                                                                        DateReturnFrom,
                                                                        DateReturnTo,
                                                                        DateBooking,
                                                                        Adult,
                                                                        Child,
                                                                        Infant,
                                                                        Other,
                                                                        OtherPassengerType,
                                                                        BoardingClass,
                                                                        BookingClass,
                                                                        DayTimeIndicator,
                                                                        AgencyCode,
                                                                        CurrencyCode,
                                                                        FlightId,
                                                                        FareId,
                                                                        MaxAmount,
                                                                        NonStopOnly,
                                                                        IncludeDeparted,
                                                                        IncludeCancelled,
                                                                        IncludeWaitlisted,
                                                                        IncludeSoldOut,
                                                                        Refundable,
                                                                        GroupFares,
                                                                        ItFaresOnly,
                                                                        PromotionCode,
                                                                        FareType,
                                                                        FareLogic,
                                                                        ReturnFlight,
                                                                        bLowest,
                                                                        bLowestClass,
                                                                        bLowestGroup,
                                                                        bShowClosed,
                                                                        bSort,
                                                                        bDelete,
                                                                        bSkipFareLogic,
                                                                        strLanguage,
                                                                        strIpAddress,
                                                                        strToken,
                                                                        bReturnRefundable,
                                                                        bNoVat,
                                                                        iDayRange);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetSessionlessFlightAvailability(Origin,
                                                                                    Destination,
                                                                                    DateDepartFrom,
                                                                                    DateDepartTo,
                                                                                    DateReturnFrom,
                                                                                    DateReturnTo,
                                                                                    DateBooking,
                                                                                    Adult,
                                                                                    Child,
                                                                                    Infant,
                                                                                    Other,
                                                                                    OtherPassengerType,
                                                                                    BoardingClass,
                                                                                    BookingClass,
                                                                                    DayTimeIndicator,
                                                                                    AgencyCode,
                                                                                    CurrencyCode,
                                                                                    FlightId,
                                                                                    FareId,
                                                                                    MaxAmount,
                                                                                    NonStopOnly,
                                                                                    IncludeDeparted,
                                                                                    IncludeCancelled,
                                                                                    IncludeWaitlisted,
                                                                                    IncludeSoldOut,
                                                                                    Refundable,
                                                                                    GroupFares,
                                                                                    ItFaresOnly,
                                                                                    PromotionCode,
                                                                                    FareType,
                                                                                    FareLogic,
                                                                                    ReturnFlight,
                                                                                    bLowest,
                                                                                    bLowestClass,
                                                                                    bLowestGroup,
                                                                                    bShowClosed,
                                                                                    bSort,
                                                                                    bDelete,
                                                                                    bSkipFareLogic,
                                                                                    strLanguage,
                                                                                    strIpAddress,
                                                                                    strToken,
                                                                                    bReturnRefundable,
                                                                                    bNoVat,
                                                                                    iDayRange);
                    break;
            }


            return result;
        }

        public string GetSessionlessLowFareFinder(string Origin,
                                                    string Destination,
                                                    DateTime DateDepartFrom,
                                                    DateTime DateDepartTo,
                                                    DateTime DateReturnFrom,
                                                    DateTime DateReturnTo,
                                                    DateTime DateBooking,
                                                    short Adult,
                                                    short Child,
                                                    short Infant,
                                                    short Other,
                                                    string OtherPassengerType,
                                                    string BoardingClass,
                                                    string BookingClass,
                                                    string DayTimeIndicator,
                                                    string AgencyCode,
                                                    string CurrencyCode,
                                                    string FlightId,
                                                    string FareId,
                                                    double MaxAmount,
                                                    bool NonStopOnly,
                                                    bool IncludeDeparted,
                                                    bool IncludeCancelled,
                                                    bool IncludeWaitlisted,
                                                    bool IncludeSoldOut,
                                                    bool Refundable,
                                                    bool GroupFares,
                                                    bool ItFaresOnly,
                                                    string PromotionCode,
                                                    string FareType,
                                                    bool FareLogic,
                                                    bool ReturnFlight,
                                                    bool bLowest,
                                                    bool bLowestClass,
                                                    bool bLowestGroup,
                                                    bool bShowClosed,
                                                    bool bSort,
                                                    bool bDelete,
                                                    bool bSkipFareLogic,
                                                    string strLanguage,
                                                    string strIpAddress,
                                                    string strToken,
                                                    bool bReturnRefundable,
                                                    bool bNoVat,
                                                    Int32 iDayRange)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.GetSessionlessLowFareFinder(Origin,
                                                                Destination,
                                                                DateDepartFrom,
                                                                DateDepartTo,
                                                                DateReturnFrom,
                                                                DateReturnTo,
                                                                DateBooking,
                                                                Adult,
                                                                Child,
                                                                Infant,
                                                                Other,
                                                                OtherPassengerType,
                                                                BoardingClass,
                                                                BookingClass,
                                                                DayTimeIndicator,
                                                                AgencyCode,
                                                                CurrencyCode,
                                                                FlightId,
                                                                FareId,
                                                                MaxAmount,
                                                                NonStopOnly,
                                                                IncludeDeparted,
                                                                IncludeCancelled,
                                                                IncludeWaitlisted,
                                                                IncludeSoldOut,
                                                                Refundable,
                                                                GroupFares,
                                                                ItFaresOnly,
                                                                PromotionCode,
                                                                FareType,
                                                                FareLogic,
                                                                ReturnFlight,
                                                                bLowest,
                                                                bLowestClass,
                                                                bLowestGroup,
                                                                bShowClosed,
                                                                bSort,
                                                                bDelete,
                                                                bSkipFareLogic,
                                                                strLanguage,
                                                                strIpAddress,
                                                                strToken,
                                                                bReturnRefundable,
                                                                bNoVat,
                                                                iDayRange);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetSessionlessLowFareFinder(Origin,
                                                                            Destination,
                                                                            DateDepartFrom,
                                                                            DateDepartTo,
                                                                            DateReturnFrom,
                                                                            DateReturnTo,
                                                                            DateBooking,
                                                                            Adult,
                                                                            Child,
                                                                            Infant,
                                                                            Other,
                                                                            OtherPassengerType,
                                                                            BoardingClass,
                                                                            BookingClass,
                                                                            DayTimeIndicator,
                                                                            AgencyCode,
                                                                            CurrencyCode,
                                                                            FlightId,
                                                                            FareId,
                                                                            MaxAmount,
                                                                            NonStopOnly,
                                                                            IncludeDeparted,
                                                                            IncludeCancelled,
                                                                            IncludeWaitlisted,
                                                                            IncludeSoldOut,
                                                                            Refundable,
                                                                            GroupFares,
                                                                            ItFaresOnly,
                                                                            PromotionCode,
                                                                            FareType,
                                                                            FareLogic,
                                                                            ReturnFlight,
                                                                            bLowest,
                                                                            bLowestClass,
                                                                            bLowestGroup,
                                                                            bShowClosed,
                                                                            bSort,
                                                                            bDelete,
                                                                            bSkipFareLogic,
                                                                            strLanguage,
                                                                            strIpAddress,
                                                                            strToken,
                                                                            bReturnRefundable,
                                                                            bNoVat,
                                                                            iDayRange);
                    break;
            }


            return result;
        }
        public Fees GetSessionlessFeesDefinition(string strLanguage, string strToken)
        {
            Fees objFees = null;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objFees = objAgent.GetSessionlessFeesDefinition(strLanguage, strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    objFees = objtikAeroWebService.GetSessionlessFeesDefinition(strLanguage, strToken);

                    break;
            }

            return objFees;
        }
        public Fees GetFee(string strFee, string strCurrency, string strAgencyCode, string strClass, string strFareBasis, string strOrigin, string strDestination, string strFlightNumber, DateTime dtDeparture, string strLanguage, bool bNoVat)
        {
            Fees objFee = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    objFee = objAgent.GetFee(strFee, strCurrency, strAgencyCode, strClass, strFareBasis, strOrigin, strDestination, strFlightNumber, dtDeparture, strLanguage, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    objFee = objtikAeroWebService.GetFee(strFee, strCurrency, strAgencyCode, strClass, strFareBasis, strOrigin, strDestination, strFlightNumber, dtDeparture, strLanguage, bNoVat);
                    break;
            }

            return objFee;
        }
        public string GetBaggageFeeOptions(Mappings mappings, Guid gSegmentId, Guid gPassengerId, string strAgencyCode, string strLanguage, long lMaxunits, Fees fees, bool bApplySegmentFee, bool bNoVat)
        {
            string strFees = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    strFees = objAgent.GetBaggageFeeOptions(mappings, gSegmentId, gPassengerId, strAgencyCode, strLanguage, lMaxunits, fees, bApplySegmentFee, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strFees = objtikAeroWebService.GetBaggageFeeOptions(mappings, gSegmentId, gPassengerId, strAgencyCode, strLanguage, lMaxunits, fees, bApplySegmentFee, bNoVat);
                    break;
            }

            return strFees;
        }
        public string SessionlessExternalPaymentAddPayment(string strBookingId,
                                                            string strAgencyCode,
                                                            string strFormOfPayment,
                                                            string strCurrencyCode,
                                                            decimal dAmount,
                                                            string strFormOfPaymentSubtype,
                                                            string strUserId,
                                                            string strDocumentNumber,
                                                            string strApprovalCode,
                                                            string strRemark,
                                                            string strLanguage,
                                                            DateTime dtPayment,
                                                            bool bReturnItinerary,
                                                            string strToken)
        {
            string result = string.Empty;

            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    result = objAgent.SessionlessExternalPaymentAddPayment(strBookingId,
                                                                            strAgencyCode,
                                                                            strFormOfPayment,
                                                                            strCurrencyCode,
                                                                            dAmount,
                                                                            strFormOfPaymentSubtype,
                                                                            strUserId,
                                                                            strDocumentNumber,
                                                                            strApprovalCode,
                                                                            strRemark,
                                                                            strLanguage,
                                                                            dtPayment,
                                                                            bReturnItinerary,
                                                                            strToken);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.SessionlessExternalPaymentAddPayment(strBookingId,
                                                                                        strAgencyCode,
                                                                                        strFormOfPayment,
                                                                                        strCurrencyCode,
                                                                                        dAmount,
                                                                                        strFormOfPaymentSubtype,
                                                                                        strUserId,
                                                                                        strDocumentNumber,
                                                                                        strApprovalCode,
                                                                                        strRemark,
                                                                                        strLanguage,
                                                                                        dtPayment,
                                                                                        bReturnItinerary,
                                                                                        strToken);
                    break;
            }


            return result;
        }
        public string GetAvailabilety(string strFlightID, string strOriginRcd, string strDestinationRcd, string strSpecialServiceRcd, string strBoardingClassRcd)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetAvailabilety(strFlightID, strOriginRcd, strDestinationRcd, strSpecialServiceRcd, strBoardingClassRcd);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetAvailabilety(strFlightID, strOriginRcd, strDestinationRcd, strSpecialServiceRcd, strBoardingClassRcd);
                    break;
            }

            return result;
        }

        public Int32 GetInfantCapacity(string flightId, string originRcd, string destinationRcd, string boardingClass)
        {
            Int32 result = 0;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetInfantCapacity(flightId, originRcd, destinationRcd, boardingClass);
                    break;
            }

            return result;
        }

        public string AddFee(string bookingId,
                            string AgencyCode,
                            BookingHeader header,
                            Itinerary segment,
                            Passengers passenger,
                            string strFeeCode,
                            Fees fees,
                            string currency,
                            Remarks remark,
                            Payments payment,
                            Mappings mapping,
                            Services service,
                            Taxes tax,
                            string strLanguage,
                            bool bNoVat)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    //Call Old webservice
                    AgentService objAgent = new AgentService();

                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.AddFee(bookingId, AgencyCode, header, segment, passenger, strFeeCode, fees, currency, remark, payment, mapping, service, tax, strLanguage, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    //Call new webservice
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.AddFee(bookingId, AgencyCode, header, segment, passenger, strFeeCode, fees, currency, remark, payment, mapping, service, tax, strLanguage, bNoVat);
                    break;
            }

            return result;
        }
        public string GetPassengerRole(string strLanguage)
        {
            string result = string.Empty;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    result = objAgent.GetPassengerRole(strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.GetPassengerRole(strLanguage);
                    break;
            }
            return result;
        }
        public Services GetSpecialServices(string strLanguage)
        {
            Services services = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;
                    services = objAgent.GetSpecialServices(strLanguage);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    services = objtikAeroWebService.GetSpecialServices(strLanguage);
                    break;
            }
            return services;
        }
        public string GetFlightSummary(Passengers passengers, Flights flights, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            string strSummary = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    strSummary = objAgent.GetFlightSummary(passengers, flights, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    strSummary = objtikAeroWebService.GetFlightSummary(passengers, flights, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
                    break;
            }
            return strSummary;
        }
        #endregion

        #region IClientCore Change of booking
        public string RemarkAdd(string remarkType,
                                string bookingRemarkId,
                                string bookingId,
                                string clientProfileId,
                                string nickname,
                                string remarkText,
                                string agencyCode,
                                string addedBy,
                                string userId,
                                bool bProtected,
                                bool warning,
                                bool processMessage,
                                bool systemRemark,
                                DateTime timelimit,
                                DateTime timelimitUTC)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.RemarkAdd(remarkType,
                                                bookingRemarkId,
                                                bookingId,
                                                clientProfileId,
                                                nickname,
                                                remarkText,
                                                agencyCode,
                                                addedBy,
                                                userId,
                                                bProtected,
                                                warning,
                                                processMessage,
                                                systemRemark,
                                                timelimit,
                                                timelimitUTC);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.RemarkAdd(remarkType,
                                                            bookingRemarkId,
                                                            bookingId,
                                                            clientProfileId,
                                                            nickname,
                                                            remarkText,
                                                            agencyCode,
                                                            addedBy,
                                                            userId,
                                                            bProtected,
                                                            warning,
                                                            processMessage,
                                                            systemRemark,
                                                            timelimit,
                                                            timelimitUTC);
                    break;

            }
            return result;
        }

        public bool RemarkDelete(Guid bookingRemarkId)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.RemarkDelete(bookingRemarkId);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.RemarkDelete(bookingRemarkId);
                    break;

            }
            return result;
        }
        public bool RemarkComplete(Guid bookingRemarkId, Guid userId)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.RemarkComplete(bookingRemarkId, userId);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.RemarkComplete(bookingRemarkId, userId);
                    break;

            }
            return result;
        }

        public string RemarkRead(string remarkId,
                                string bookingId,
                                string bookingReference,
                                double bookingNumber,
                                bool readOnly)
        {
            string result = null;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.RemarkRead(remarkId,
                                                bookingId,
                                                bookingReference,
                                                bookingNumber,
                                                readOnly);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.RemarkRead(remarkId,
                                                            bookingId,
                                                            bookingReference,
                                                            bookingNumber,
                                                            readOnly);
                    break;

            }
            return result;
        }

        public bool RemarkSave(Remarks remarks)
        {
            bool result = false;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.RemarkSave(remarks);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.RemarkSave(remarks);
                    break;

            }
            return result;
        }

        public int UpdatePassengerDocumentDetails(Passengers passengers)
        {
            int result = 0;
            switch (ConfigurationManager.AppSettings["Service"])
            {
                case "1":
                    AgentService objAgent = new AgentService();
                    objAgent.objService = (TikAeroXMLwebservice)objService;

                    result = objAgent.UpdatePassengerDocumentDetails(passengers);

                    if (objAgent != null)
                    {
                        objAgent.Dispose();
                    }
                    break;
                case "2":
                    clstikAeroWebService objtikAeroWebService = new clstikAeroWebService();
                    result = objtikAeroWebService.UpdatePassengerDocumentDetails(passengers);
                    break;

            }
            return result;
        }

        #endregion
    }

}
