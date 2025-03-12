using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using tikSystem.Web.Library.agentservice;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Linq;

namespace tikSystem.Web.Library
{
    public class AgentService : IClientCore, IDisposable
    {
        private TikAeroXMLwebservice _objService = null;
        public TikAeroXMLwebservice objService
        {
            get { return _objService; }
            set { _objService = value; }
        }

        #region IClientCore Members

        public DataSet GetAirport(string language)
        {
            try
            {
                if (language.Length == 0)
                { language = "EN"; }
                return _objService.GetAirlines(language);
            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }
        }

        public Routes GetOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag)
        {
            try
            {
                Routes routes = null;

                if (language.Length == 0)
                {
                    language = "EN";
                }

                DataSet ds = null;
                if (_objService == null)
                {
                    using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
                    {
                        ds = service.GetSessionlessAirportOrigins(language,
                                                                b2cFlag,
                                                                b2bFlag,
                                                                b2eFlag,
                                                                b2sFlag,
                                                                apiFlag,
                                                                SecurityHelper.GenerateSessionlessToken());



                    }
                }
                else
                {
                    ds = _objService.ReturnAirportOrigins(language,
                                                          b2cFlag,
                                                          b2bFlag,
                                                          b2eFlag,
                                                          b2sFlag,
                                                          apiFlag);
                }

                //Fill dataset to route object.
                using (ds)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Route r;
                        routes = new Routes();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            r = new Route();
                            r.origin_rcd = DataHelper.DBToString(dr, "origin_rcd");
                            r.display_name = DataHelper.DBToString(dr, "display_name");
                            r.country_rcd = DataHelper.DBToString(dr, "country_rcd");
                            r.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                            r.routes_tot = DataHelper.DBToByte(dr, "routes_tot");
                            r.routes_avl = DataHelper.DBToByte(dr, "routes_avl");
                            r.routes_b2c = DataHelper.DBToByte(dr, "routes_b2c");
                            r.routes_b2b = DataHelper.DBToByte(dr, "routes_b2b");
                            r.routes_b2s = DataHelper.DBToByte(dr, "routes_b2s");
                            r.routes_api = DataHelper.DBToByte(dr, "routes_api");
                            r.routes_b2t = DataHelper.DBToByte(dr, "routes_b2t");
                            routes.Add(r);
                            r = null;
                        }
                    }
                }

                return routes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Routes GetDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag)
        {
            try
            {
                Routes routes = new Routes();
                DataSet ds = null;
                if (string.IsNullOrEmpty(language))
                {
                    language = "EN";
                }

                if (_objService == null)
                {
                    using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
                    {
                        ds = service.GetSessionlessAirportDestinations(language,
                                                                        b2cFlag,
                                                                        b2bFlag,
                                                                        b2eFlag,
                                                                        b2sFlag,
                                                                        apiFlag,
                                                                        SecurityHelper.GenerateSessionlessToken());
                    }
                }
                else
                {
                    ds = _objService.ReturnAirportDestinations(language,
                                                                b2cFlag,
                                                                b2bFlag,
                                                                b2eFlag,
                                                                b2sFlag,
                                                                apiFlag);
                }


                //Fill dataset to route object.
                using (ds)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Route r;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            r = new Route();
                            r.origin_rcd = DataHelper.DBToString(dr, "origin_rcd");
                            r.destination_rcd = DataHelper.DBToString(dr, "destination_rcd");
                            r.segment_change_fee_flag = DataHelper.DBToBoolean(dr, "segment_change_fee_flag");
                            r.transit_flag = DataHelper.DBToBoolean(dr, "transit_flag");
                            r.direct_flag = DataHelper.DBToBoolean(dr, "direct_flag");
                            r.avl_flag = DataHelper.DBToBoolean(dr, "avl_flag");
                            r.b2c_flag = DataHelper.DBToBoolean(dr, "b2c_flag");
                            r.b2b_flag = DataHelper.DBToBoolean(dr, "b2b_flag");
                            r.b2t_flag = DataHelper.DBToBoolean(dr, "b2t_flag");
                            r.day_range = DataHelper.DBToInt16(dr, "day_range");
                            r.show_redress_number_flag = DataHelper.DBToBoolean(dr, "show_redress_number_flag");
                            r.require_passenger_title_flag = DataHelper.DBToBoolean(dr, "require_passenger_title_flag");
                            r.require_passenger_gender_flag = DataHelper.DBToBoolean(dr, "require_passenger_gender_flag");
                            r.require_date_of_birth_flag = DataHelper.DBToBoolean(dr, "require_date_of_birth_flag");
                            r.require_document_details_flag = DataHelper.DBToBoolean(dr, "require_document_details_flag");
                            r.require_passenger_weight_flag = DataHelper.DBToBoolean(dr, "require_passenger_weight_flag");
                            r.special_service_fee_flag = DataHelper.DBToBoolean(dr, "special_service_fee_flag");
                            r.show_insurance_on_web_flag = DataHelper.DBToBoolean(dr, "show_insurance_on_web_flag");
                            r.display_name = DataHelper.DBToString(dr, "display_name");
                            r.country_rcd = DataHelper.DBToString(dr, "destination_country_rcd");
                            r.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                            routes.Add(r);
                            r = null;
                        }
                    }
                }
                return routes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAgencyCode(string agencyCode)
        {
            return null;
        }

        public bool ReleaseFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock)
        {
            try
            {
                return _objService.ReleaseFlightInventorySession(bookingId);
            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }
        }
        public bool ReleaseSessionlessFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock, string strToken)
        {
            try
            {

                using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
                {
                    return service.ReleaseSessionlessFlightInventorySession(bookingId, SecurityHelper.GenerateSessionlessToken());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

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
                                                bool bSkipFareLogin,
                                                string strLanguage,
                                                string strIpAddress,
                                                bool bReturnRefundable,
                                                bool bNoVat,
                                                Int32 iDayRange)
        {

            string result = string.Empty;

            try
            {
                result = _objService.GetFlightAvailability(Origin,
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
                                                            bSkipFareLogin,
                                                            strLanguage,
                                                            strIpAddress,
                                                            bReturnRefundable,
                                                            bNoVat,
                                                            iDayRange);

            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }

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
                                        bool bSkipFareLogin,
                                        string strLanguage,
                                        string strIpAddress,
                                        bool bReturnRefundable,
                                        bool bNoVat,
                                        Int32 iDayRange)
        {

            string result = string.Empty;

            try
            {
                result = _objService.GetLowFareFinder(Origin,
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
                                                    bSkipFareLogin,
                                                    strLanguage,
                                                    strIpAddress,
                                                    bReturnRefundable,
                                                    bNoVat,
                                                    iDayRange);

            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }

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

            using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
            {
                if ((DataHelper.DateDifferent(DateDepartFrom, DateDepartTo).Days > 31) ||
                    (DataHelper.DateDifferent(DateReturnFrom, DateReturnTo).Days > 31))
                {
                    return string.Empty;
                }
                else
                {
                    //Search Avalability
                    return service.GetSessionlessFlightAvailability(Origin,
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
                                                                    SecurityHelper.GenerateSessionlessToken(),
                                                                    bReturnRefundable,
                                                                    bNoVat,
                                                                    iDayRange);
                }
            }
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
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                try
                {
                    result = objService.GetSessionlessLowFareFinder(Origin,
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

                }
                catch
                {
                    result = string.Empty;
                }
            }

            return result;

        }
        public string GetCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType)
        {
            string result = string.Empty;
            try
            {
                string bookingId = Guid.NewGuid().ToString();
                result = _objService.GetCompactFlightAvailability(true,
                                                                  bookingId,
                                                                  Origin,
                                                                  Destination,
                                                                  DepartDateFrom,
                                                                  DepartDateTo,
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
                                                                  strSearchType);

            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }

            return result;

        }
        public string GetSessionlessCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType, string strToken)
        {
            string result = string.Empty;
            try
            {
                using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
                {
                    string bookingId = Guid.NewGuid().ToString();
                    result = objService.GetSessionlessCompactFlightAvailability(true,
                                                                              bookingId,
                                                                              Origin,
                                                                              Destination,
                                                                              DepartDateFrom,
                                                                              DepartDateTo,
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
                                                                              strSearchType,
                                                                              strToken);

                }
            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }

            return result;

        }
        public string FlightAdd(string agencyCode, string currency, string flightXml, string bookingID, short adults, short children, short infants, short others, string strOthers, string userId, string strIpAddress, string strLanguageCode, bool bNoVat)
        {
            DataSet ds = null;
            string fareId = string.Empty;
            string boardingClass = string.Empty;
            string bookingClass = string.Empty;
            string result = string.Empty;

            if (_objService != null)
            { ds = _objService.AddFlight(flightXml, string.Empty, bookingID, string.Empty, string.Empty, agencyCode, DateTime.MinValue, string.Empty, ref fareId, ref boardingClass, ref bookingClass, adults, children, infants, others, strOthers, userId, strIpAddress, strLanguageCode, currency, bNoVat); }

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                { result = ds.GetXml(); }
                ds.Dispose();
            }
            return result;
        }
        public string GetClient(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            string result = string.Empty;
            if (_objService != null)
            { result = _objService.GetClient(clientId, clientNumber, passengerId); }

            return result;
        }
        public DataSet GetClientPassenger(string bookingId, string clientProfileId, string clientNumber)
        {
            DataSet result = null;
            if (_objService != null)
            { result = _objService.GetClientPassenger(bookingId, clientProfileId, clientNumber); }

            return result;
        }
        public Titles GetPassengerTitles(string language)
        {
            try
            {
                Titles titles = new Titles();
                if (_objService != null)
                {
                    System.Data.DataSet ds = _objService.GetPassengerTitles(language);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Title t;
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            t = new Title();
                            t.title_rcd = DataHelper.DBToString(dr, "title_rcd");
                            t.display_name = DataHelper.DBToString(dr, "display_name");
                            t.gender_type_rcd = DataHelper.DBToString(dr, "gender_type_rcd");
                            titles.Add(t);
                            t = null;
                        }
                    }
                }
                return titles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Documents GetDocumentType(string language)
        {
            try
            {
                Documents documents = new Documents();
                if (_objService != null)
                {
                    System.Data.DataSet ds = _objService.GetDocumentType(language);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Document doc;
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            doc = new Document();
                            doc.document_type_rcd = DataHelper.DBToString(dr, "document_type_rcd");
                            doc.display_name = DataHelper.DBToString(dr, "display_name");
                            documents.Add(doc);

                            doc = null;
                        }
                    }
                }
                return documents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Countries GetCountry(string language)
        {
            try
            {
                Countries countries = new Countries();
                System.Data.DataSet ds = null;
                using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
                {
                    ds = service.GetSessionlessCountry(language, SecurityHelper.GenerateSessionlessToken());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Country c;
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            c = new Country();
                            c.country_rcd = DataHelper.DBToString(dr, "country_rcd");
                            c.display_name = DataHelper.DBToString(dr, "display_name");
                            c.status_code = DataHelper.DBToString(dr, "status_code");
                            c.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                            c.vat_display = DataHelper.DBToString(dr, "vat_display");
                            c.country_code_long = DataHelper.DBToString(dr, "country_code_long");
                            c.country_number = DataHelper.DBToString(dr, "country_number");
                            c.phone_prefix = DataHelper.DBToString(dr, "phone_prefix");
                            c.issue_country_flag = DataHelper.DBToByte(dr, "issue_country_flag");
                            c.residence_country_flag = DataHelper.DBToByte(dr, "residence_country_flag");
                            c.nationality_country_flag = DataHelper.DBToByte(dr, "nationality_country_flag");
                            c.address_country_flag = DataHelper.DBToByte(dr, "address_country_flag");
                            c.agency_country_flag = DataHelper.DBToByte(dr, "agency_country_flag");
                            countries.Add(c);
                            c = null;
                        }
                    }
                }
                return countries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Languages GetLanguages(string language)
        {
            try
            {
                Languages languages = new Languages();
                if (_objService != null)
                {
                    System.Data.DataSet ds = _objService.GetLanguages(language);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Language l;
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            l = new Language();
                            l.language_rcd = DataHelper.DBToString(dr, "language_rcd");
                            l.display_name = DataHelper.DBToString(dr, "display_name");
                            l.character_set = DataHelper.DBToString(dr, "character_set");

                            languages.Add(l);
                            l = null;
                        }
                    }
                }
                return languages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass, string strLanguage)
        {
            DataSet dsResult = null;
            if (_objService != null)
            {
                dsResult = _objService.GetSeatMap(origin, destination, flightId, boardingClass, bookingClass, strLanguage);
            }

            return dsResult;
        }
        public DataSet GetSeatMapLayout(string flightId, string origin, string destination, string boardingClass, string configuration, string strLanguage)
        {
            DataSet dsResult = null;
            if (_objService != null)
            {
                dsResult = _objService.GetSeatMapLayout(flightId, origin, destination, boardingClass, configuration, strLanguage);
            }

            return dsResult;
        }
        public string GetFormOfPayments(string language)
        {
            string strResult = null;
            if (_objService != null)
            {
                strResult = _objService.GetFormOfPayments(language);
            }
            return strResult;
        }
        public string GetFormOfPaymentSubTypes(string type, string language)
        {
            string strResult = null;
            if (_objService != null)
            {
                strResult = _objService.GetFormOfPaymentSubTypes(type, language);
            }
            return strResult;
        }
        public string SaveBooking(string bookingId, string header, string segment, string passenger, string remark, string payment, string mapping, string service, string tax, string fee, bool createTickets, bool readBooking, bool readOnly, string strLanguage)
        {
            DataSet dsResult = null;

            if (_objService != null)
            {
                dsResult = _objService.SaveBooking(bookingId, header, segment, passenger, payment, remark, mapping, fee, tax, "", service, createTickets, "", strLanguage);
                //update approval_flag
                //_objService.UpdateApproval(bookingId, 1);
            }

            if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                return dsResult.GetXml();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool SaveBookingHeader(string header)
        {
            return _objService.SaveBookingHeader(header);
        }

        public double CalculateExchange(string currencyFrom, string currencyTo, double amount, string systemCurrency, DateTime dateOfExchange, bool reverse)
        {
            double Result = 0D;

            if (_objService != null)
            { Result = _objService.CalculateExchange(currencyFrom, currencyTo, amount, systemCurrency, dateOfExchange, reverse); }

            return Result;
        }

        public string CalculateNewFees(string bookingId, string AgencyCode, string header, string segment, string passenger, string fees, string currency, string remark, string payment, string mapping, string service, string tax, bool checkBooking, bool checkSegment, bool checkName, bool checkSeat, string strLanguage, bool bNoVat)
        {
            string Result = string.Empty;

            if (_objService != null)
            { Result = (string)_objService.CalculateNewFees(bookingId, AgencyCode, header, segment, passenger, payment, remark, mapping, fees, tax, "", service, currency, checkBooking, checkSegment, checkName, checkSeat, strLanguage, bNoVat); }

            return Result;
        }
        public string GetVouchers(string recordLocator,
                                    string voucherNumber,
                                    string voucherID,
                                    string status,
                                    string recipient,
                                    string fOPSubType,
                                    string clientProfileId,
                                    string currency,
                                    string password,
                                    bool includeOpenVoucher,
                                    bool includeExpiredVoucher,
                                    bool includeUsedVoucher,
                                    bool includeVoidedVoucher,
                                    bool includeRefundable,
                                    bool includeFareOnly,
                                    bool write,
                                    Mappings mappings,
                                    Fees fees)
        {
            string Result = string.Empty;
            string bookingFeeId = string.Empty;

            if (_objService != null)
            {
                Result = _objService.GetVouchers(recordLocator,
                                                voucherNumber,
                                                voucherID,
                                                status,
                                                recipient,
                                                fOPSubType,
                                                clientProfileId,
                                                currency,
                                                password,
                                                includeExpiredVoucher,
                                                includeUsedVoucher,
                                                includeVoidedVoucher,
                                                includeRefundable,
                                                includeFareOnly,
                                                ref write,
                                                XmlHelper.Serialize(mappings, false),
                                                XmlHelper.Serialize(fees, false));
            }

            return Result;

        }
        public bool SavePayment(string bookingId, Mappings mappings, Fees fees, Payments payments, Vouchers refundVoucher)
        {
            bool result = false;

            if (_objService != null)
            {
                result = _objService.PaymentSave(bookingId,
                                                XmlHelper.Serialize(mappings, false),
                                                XmlHelper.Serialize(fees, false),
                                                XmlHelper.Serialize(payments, false));
            }

            return result;
        }
        public DataSet GetFormOfPaymentSubtypeFees(string formOfPayment, string formOfPaymentSubtype, string currencyRcd, string agency, DateTime feeDate)
        {
            DataSet ds = null;
            try
            {
                if (_objService != null)
                {
                    ds = _objService.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubtype, currencyRcd, agency, feeDate);
                    ds.DataSetName = "FormOfPaymentSubtypeFees";
                }
            }
            catch
            {
                throw;
            }
            return ds;
        }
        public string ValidCreditCard(string bookingId, Mappings mapping, Fees fees, Payments payments, Itinerary segments, Vouchers voucher, string securityToken, string authenticationToken, string commerceIndicator, string bookingReference)
        {
            DataSet ds = null;

            if (_objService != null)
            {
                ds = _objService.ValidCreditCard(bookingId,
                                                XmlHelper.Serialize(mapping, false),
                                                XmlHelper.Serialize(fees, false),
                                                XmlHelper.Serialize(payments, false),
                                                XmlHelper.Serialize(segments, false),
                                                securityToken,
                                                authenticationToken,
                                                commerceIndicator,
                                                bookingReference);
            }

            return ds.GetXml();
        }
        public string GetItinerary(string bookingId, string language, string passengerId, string agencyCode)
        {
            string Result = string.Empty;
            string bookingFeeId = string.Empty;

            if (_objService != null)
            { Result = _objService.GetItinerary(bookingId, passengerId, language); }

            return Result;
        }
        public string ItineraryRead(string recordLocator, string language, string passengerId, string agencyCode)
        {
            return string.Empty;
        }
        public bool QueueMail(string strFromAddress, string strFromName, string strToAddress, string strToAddressCC, string strToAddressBCC, string strReplyToAddress, string strSubject, string strBody, string strDocumentType, string strAttachmentStream, string strAttachmentFileName, string strAttachmentFileType, string strAttachmentParser, bool bHtmlBody, bool bConvertAttachmentFromHTML2PDF, bool bRemoveFromQueue, string strUserId, string strBookingId, string strVoucherId, string strBookingSegmentID, string strPassengerId, string strClientProfileId, string strDocumentId, string strLanguageCode)
        {
            bool result = false;

            if (_objService != null)
            {
                result = _objService.QueueMail(strFromAddress,
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
            }

            return result;
        }

        public DataSet ClientLogon(string ClientNumber, string ClientPassword)
        {
            DataSet ds = new DataSet();

            if (_objService != null)
            {
                ds = _objService.ClientLogon(ClientNumber, ClientPassword);
            }

            return ds;
        }

        public DataSet GetBookings(string Airline, string FlightNumber, string FlightId, DateTime FlightFrom, DateTime FlightTo,
            string RecordLocator, string Origin, string Destination, string PassengerName, string SeatNumber, string TicketNumber,
            string PhoneNumber, string AgencyCode, string ClientNumber, string MemberNumber, string ClientId, bool ShowHistory,
            string Language, bool bIndividual, bool bGroup, DateTime CreateFrom, DateTime CreateTo)
        {
            DataSet ds = new DataSet();

            if (_objService != null)
            {
                ds = _objService.GetBookings(Airline, FlightNumber, FlightId, FlightFrom, FlightTo,
                        RecordLocator, Origin, Destination, PassengerName, SeatNumber, TicketNumber,
                        PhoneNumber, AgencyCode, ClientNumber, MemberNumber, ClientId, ShowHistory,
                        Language, bIndividual, bGroup, CreateFrom, CreateTo);
            }

            return ds;
        }

        public DataSet GetBookingsThisUser(string agencyCode, string userId, string airline, string flightNumber, DateTime flightFrom,
            DateTime flightTo, string recordLocator, string origin, string destination, string passengerName, string seatNumber,
            string ticketNumber, string phoneNumber, DateTime createFrom, DateTime createTo)
        {
            DataSet ds = new DataSet();

            if (_objService != null)
            {
                ds = _objService.GetBookingsThisUser(ref agencyCode, ref userId, ref airline, ref flightNumber, ref flightFrom,
                        ref flightTo, ref recordLocator, ref origin, ref destination, ref passengerName, ref seatNumber,
                        ref ticketNumber, ref phoneNumber, ref createFrom, ref createTo);
            }

            return ds;
        }

        public DataSet ClientRead(string clientProfileID)
        {
            DataSet ds = new DataSet();

            if (_objService != null)
            {
                ds = _objService.ClientRead(clientProfileID);
            }

            return ds;
        }

        public bool AddClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            return ClientSave(XmlHelper.Serialize(client, false),
                              XmlHelper.Serialize(passengers, false),
                              "");
        }

        public bool EditClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            return ClientSave(XmlHelper.Serialize(client, false),
                              XmlHelper.Serialize(passengers, false),
                              "");
        }

        public bool ClientSave(string xmlClient, string xmlPassenger, string xmlBookingRemark)
        {
            bool result = false;

            if (_objService != null)
            { result = _objService.ClientSave(ref xmlClient, ref xmlPassenger, ref xmlBookingRemark); }

            return result;
        }
        public bool AddClientPassengerList(string xmlClient, string xmlPassenger, string xmlBookingRemark)
        {
            bool result = false;

            if (_objService != null)
            {
                result = _objService.AddClientPassengerList(ref xmlPassenger);
            }

            return result;
        }
        public DataSet MemberLevelRead(string strMemberLevelCode, string strMemberLevel, string strStatus, bool bWrite)
        {

            DataSet dsMember = null;
            if (_objService != null)
            {
                dsMember = _objService.MemberLevelRead(strMemberLevelCode, strMemberLevel, strStatus, bWrite);
            }

            return dsMember;
        }
        public DataSet PassengerRoleRead(string strPaxRoleCode, string strPaxRole, string strStatus, bool bWrite, string strLanguage)
        {

            DataSet dsMember = null;
            if (_objService != null)
            {
                dsMember = _objService.PassengerRoleRead(strPaxRoleCode, strPaxRole, strStatus, bWrite, strLanguage);
            }

            return dsMember;
        }
        public string GetTransaction(string strOrigin, string strDestination, string strAirline, string strFlight, string strSegmentType,
            string strClientProfileId, string strPassengerProfileId, string strVendor, string strCreditDebit, DateTime dtFlightFrom,
            DateTime dtFlightTo, DateTime dtTransactionFrom, DateTime dtTransactionTo, DateTime dtExpiryFrom, DateTime dtExpiryTo,
            DateTime dtVoidFrom, DateTime dtVoidTo, int iBatch, bool bAllVoid, bool bAllExpired, bool bAuto, bool bManual, bool bAllPoint)
        {
            string FFPTransection = "";

            if (_objService != null)
            {
                FFPTransection = _objService.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType,
                    strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom,
                    dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo,
                    dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);
            }

            return FFPTransection;
        }
        public DataSet TicketRead(ref string strBookingId, ref string strPassengerId, ref string strSegmentId, ref string strTicketNumber, ref string xmlTaxes,
            ref bool bReadOnly, ref bool bReturnTax)
        {
            DataSet dsTicket = null;

            if (_objService != null)
            {
                dsTicket = _objService.TicketRead(ref strBookingId, ref strPassengerId, ref strSegmentId, ref strTicketNumber, ref xmlTaxes,
                    ref bReadOnly, ref bReturnTax);
            }

            return dsTicket;
        }
        public bool CheckUniqueMailAddress(string strMail, string strClientProfileId)
        {
            bool result = false;

            if (_objService != null)
            {
                result = _objService.CheckUniqueMailAddress(strMail, strClientProfileId);
            }

            return result;
        }
        public DataSet AccuralQuote(string strPassenger, string strMapping, string strClientProfileId)
        {
            DataSet dsMember = null;
            if (_objService != null)
            {
                dsMember = _objService.AccuralQuote(strPassenger, strMapping, strClientProfileId);
            }

            return dsMember;
        }
        public DataSet GetFlightDailyCount(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo)
        {
            if (_objService != null)
            {
                return _objService.GetFlightDailyCount(dtFrom, dtTo, strFrom, strTo);
            }
            else
            {
                return null;
            }
        }
        public string GetFlightDailyCountXML(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo, string strToken)
        {
            using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
            {
                return service.GetFlightDailyCountXML(dtFrom, dtTo, strFrom, strTo, strToken);
            }
        }
        public DataSet GetBinRangeSearch(string strCardType, string strStatusCode)
        {
            if (_objService != null)
            {
                return _objService.GetBinRangeSearch(strCardType, strStatusCode);
            }
            else
            {
                return null;
            }
        }
        public DataSet GetSessionlessBinRangeSearch(string strCardType, string strStatusCode, string strToken)
        {
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                return objService.GetSessionlessBinRangeSearch(strCardType, strStatusCode, strToken);
            }
        }
        public DataSet BookingLogon(string strRecordLocator, string strNameOrPhone)
        {
            if (_objService != null)
            {
                return _objService.BookingLogon(ref strRecordLocator, ref strNameOrPhone);
            }
            else
            {
                return null;
            }
        }
        public string GetBookingSegmentCheckIn(string strBookingId, string strClientId, string strLanguageCode)
        {
            if (_objService != null)
            {
                return _objService.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
            }
            else
            {
                return string.Empty;
            }
        }
        public string BoardPassenger(string strFlightID, string strOrigin, string strBoard, string strUserId, bool bBoard)
        {
            if (_objService != null)
            {
                return _objService.BoardPassengers(strFlightID, strOrigin, strBoard, strUserId, bBoard);
            }
            else
            {
                return string.Empty;
            }
        }
        public string OffloadPassenger(string strBookingId, string strFlightId, string strPassengerId, bool autoBaggageFlag, string strUserId)
        {
            if (_objService != null)
            {
                return _objService.OffLoadPassenger(strBookingId, strFlightId, strPassengerId, autoBaggageFlag, strUserId);
            }
            else
            {
                return string.Empty;
            }
        }
        public string NoShowPassenger(string strBookingId, string strFlightId, string strPassengerId, bool autoBaggageFlag, string strUserId)
        {
            if (_objService != null)
            {
                return _objService.NoShowPassenger(strBookingId, strFlightId, strPassengerId, autoBaggageFlag, strUserId);
            }
            else
            {
                return string.Empty;
            }
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
            if (_objService != null)
            {
                return _objService.GetPassengerDetails(strPassengerId,
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
            }
            else
            {
                return string.Empty;
            }
        }
        public bool CheckInSave(string strMappingXml, string strBaggageXml, string strSeatAssignmentXml, string strPassengerXml, string strServiceXml, string strRemarkXml, string strBookingSegmentXml, string strFeeXml)
        {
            if (_objService != null)
            {
                try
                {
                    return _objService.CheckInSave(strMappingXml, strBaggageXml, strSeatAssignmentXml, strPassengerXml, strServiceXml, strRemarkXml, strBookingSegmentXml, strFeeXml);
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
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
            DataSet dsResult = null;
            if (_objService != null)
            {
                dsResult = _objService.SaveBookingCreditCard(bookingId,
                                                            XmlHelper.Serialize(header, false),
                                                            XmlHelper.Serialize(segment, false),
                                                            XmlHelper.Serialize(passenger, false),
                                                            XmlHelper.Serialize(payment, false),
                                                            XmlHelper.Serialize(remark, false),
                                                            XmlHelper.Serialize(mapping, false),
                                                            XmlHelper.Serialize(fee, false),
                                                            XmlHelper.Serialize(paymentFee, false),
                                                            XmlHelper.Serialize(tax, false),
                                                            XmlHelper.Serialize(service, false),
                                                            createTickets,
                                                            securityToken,
                                                            authenticationToken,
                                                            commerceIndicator,
                                                            requestSource,
                                                            strLanguage);

                //update approval_flag
                //_objService.UpdateApproval(bookingId, 1);

                return dsResult.GetXml();
            }
            else
            {
                return string.Empty;
            }
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
            DataSet dsResult = null;

            if (_objService != null)
            {
                dsResult = _objService.SaveBookingPayment(bookingId,
                                                          XmlHelper.Serialize(header, false),
                                                          XmlHelper.Serialize(segment, false),
                                                          XmlHelper.Serialize(passenger, false),
                                                          XmlHelper.Serialize(payment, false),
                                                          XmlHelper.Serialize(remark, false),
                                                          XmlHelper.Serialize(mapping, false),
                                                          XmlHelper.Serialize(fee, false),
                                                          XmlHelper.Serialize(paymentFee, false),
                                                          XmlHelper.Serialize(tax, false),
                                                          XmlHelper.Serialize(service, false),
                                                          createTickets,
                                                          strLanguage);
                //update approval_flag
                //_objService.UpdateApproval(bookingId, 1);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                { return dsResult.GetXml(); }
                else
                { return string.Empty; }
            }
            else
            {
                return string.Empty;
            }
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
            DataSet dsResult = null;

            if (_objService != null)
            {
                dsResult = _objService.SaveBookingMultipleFormOfPayment(bookingId,
                                                                      XmlHelper.Serialize(header, false),
                                                                      XmlHelper.Serialize(segment, false),
                                                                      XmlHelper.Serialize(passenger, false),
                                                                      XmlHelper.Serialize(payment, false),
                                                                      XmlHelper.Serialize(remark, false),
                                                                      XmlHelper.Serialize(mapping, false),
                                                                      XmlHelper.Serialize(fee, false),
                                                                      XmlHelper.Serialize(paymentFee, false),
                                                                      XmlHelper.Serialize(tax, false),
                                                                      XmlHelper.Serialize(service, false),
                                                                      createTickets,
                                                                      securityToken,
                                                                      authenticationToken,
                                                                      commerceIndicator,
                                                                      strRequestSource,
                                                                      strLanguage);
                //update approval_flag
                //_objService.UpdateApproval(bookingId, 1);


                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                { return dsResult.GetXml(); }
                else
                { return string.Empty; }
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetServiceFeesByGroups(BookingHeader header, Itinerary itinerary, string serviceGroup)
        {

            if (_objService != null)
            {
                StringBuilder stb = new StringBuilder();
                string[] arrServiceGroup = serviceGroup.Split(',');

                stb.Append("<services>");
                for (int i = 0; i < itinerary.Count; i++)
                {
                    for (int j = 0; j < arrServiceGroup.Length; j++)
                    {
                        stb.Append("<service>");
                        stb.Append("<origin_rcd>" + itinerary[i].origin_rcd + "</origin_rcd>");
                        stb.Append("<destination_rcd>" + itinerary[i].destination_rcd + "</destination_rcd>");
                        stb.Append("<currency_rcd>" + header.currency_rcd + "</currency_rcd>");
                        stb.Append("<agency_code>" + header.agency_code + "</agency_code>");
                        stb.Append("<service_group>" + arrServiceGroup[j].Trim() + "</service_group>");
                        stb.Append("<departure_date>" + itinerary[i].departure_date.Year.ToString() + "-" +
                                                        itinerary[i].departure_date.Month.ToString() + "-" +
                                                        itinerary[i].departure_date.Day.ToString() + "</departure_date>");
                        stb.Append("</service>");
                    }
                }
                stb.Append("</services>");

                return _objService.GetServiceFeesByGroups(stb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public DataSet GetTicketsIssued(DateTime dtReportFrom,
            DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination,
            string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsIssued(dtReportFrom, dtReportTo, dtFlightFrom,
                    dtFlightTo, strOrigin, strDestination, strAgency, strAirline, strFlight);
            }

            return ds;
        }

        public DataSet GetTicketsUsed(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo,
            string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsUsed(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, strOrigin, strDestination,
                    strAgency, strAirline, strFlight);
            }

            return ds;
        }

        public DataSet GetTicketsRefunded(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo,
            string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsRefunded(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo,
                    strOrigin, strDestination, strAgency, strAirline, strFlight);
            }

            return ds;
        }

        public DataSet GetTicketsCancelled(string strOrigin,
                    string strDestination,
                    string strAgency,
                    string strAirline,
                    string strFlight,
                    DateTime dtReportFrom,
                    DateTime dtReportTo,
                    DateTime dtFlightFrom,
                    DateTime dtFlightTo,
                    int intTicketonly,
                    int intRefundable,
                    string strProfileID,
                    string strTicketNumber,
                    string strFirstName,
                    string strLastName,
                    string strPassengerId,
                    string strBookingSegmentID)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsCancelled(strOrigin, strDestination, strAgency, strAirline, strFlight, dtReportFrom,
                    dtReportTo, dtFlightFrom, dtFlightTo, intTicketonly, intRefundable, strProfileID, strTicketNumber, strFirstName,
                    strLastName, strPassengerId, strBookingSegmentID);
            }

            return ds;
        }

        public DataSet GetTicketsNotFlown(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight,
            DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, bool bUnflown, bool bNoShow)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsNotFlown(strOrigin, strDestination, strAgency, strAirline, strFlight,
                    dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo, bUnflown, bNoShow);
            }

            return ds;
        }

        public DataSet GetTicketsExpired(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo,
            string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketsExpired(dtReportFrom, dtReportTo, dtFlightFrom, dtFlightTo,
                    strOrigin, strDestination, strAgency, strAirline, strFlight);
            }

            return ds;
        }

        public DataSet GetCashbookPayments(string strAgency, string strGroup, string strUserId, DateTime dtPaymentFrom, DateTime dtPaymentTo, string strCashbookId)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetCashbookPayments(strAgency, strGroup, strUserId, dtPaymentFrom, dtPaymentTo, strCashbookId);
            }

            return ds;
        }

        public DataSet GetCashbookCharges(string XmlCashbookCharges, string strCashbookId)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetCashbookCharges(XmlCashbookCharges, strCashbookId);
            }

            return ds;
        }

        public DataSet GetBookingFeeAccounted(string strAgencyCode, string strUserId, string strFee, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetBookingFeeAccounted(strAgencyCode, strUserId, strFee, dtFrom, dtTo);
            }

            return ds;
        }

        public DataSet CreditCardPayment(ref string strCCNumber, ref string strTransType, ref string strTransStatus, ref DateTime dtFrom, ref DateTime dtTo, ref string strCCType, ref string strAgency)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.CreditCardPayment(ref strCCNumber, ref strTransType, ref strTransStatus, ref dtFrom, ref dtTo, ref strCCType, ref strAgency);
            }

            return ds;
        }

        public DataSet GetOutstanding(string strAgencyCode, string strAirline, string strFlightNumber, DateTime dtFlightFrom, DateTime dtFlightTo,
            string strOrigin, string strDestination, bool bOffices, bool bAgencies, bool bLastTwentyFourHours, bool bTicketedOnly, int iOlderThanHours,
            string strLanguage, bool bAccountsPayable)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetOutstanding(strAgencyCode, strAirline, strFlightNumber, dtFlightFrom, dtFlightTo, strOrigin,
                    strDestination, bOffices, bAgencies, bLastTwentyFourHours, bTicketedOnly, iOlderThanHours, strLanguage, bAccountsPayable);
            }

            return ds;
        }

        public DataSet GetAgencyAccountTransactions(string agencyCode, DateTime dateFrom, DateTime dateTo)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetAgencyAccountTransactions(agencyCode, dateFrom, dateTo);
            }

            return ds;
        }

        public bool CompleteRemark(string XmlRemarks, string RemarkId, string UserId)
        {
            bool success = false;
            if (_objService != null)
            {
                success = _objService.CompleteRemark(XmlRemarks, RemarkId, UserId);
            }

            return success;
        }

        public DataSet GetActivities(string AgencyCode,
                                    string RemarkType,
                                    string Nickname,
                                    DateTime TimelimitFrom,
                                    DateTime TimelimitTo,
                                    bool PendingOnly,
                                    bool IncompleteOnly,
                                    bool IncludeRemarks,
                                    bool showUnassigned)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetActivities(AgencyCode,
                                                RemarkType,
                                                Nickname,
                                                TimelimitFrom,
                                                TimelimitTo,
                                                PendingOnly,
                                                IncompleteOnly,
                                                IncludeRemarks);
            }

            return ds;
        }

        public DataSet GetCashbookPaymentsSummary(string XmlCashbookPaymentsAll)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetCashbookPaymentsSummary(XmlCashbookPaymentsAll);
            }

            return ds;
        }

        public DataSet GetAgencyAccountTopUp(string strAgencyCode, string strCurrency, DateTime dtFrom, DateTime dtTo)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetAgencyAccountTopUp(strAgencyCode, strCurrency, dtFrom, dtTo);
            }

            return ds;
        }

        public string ServiceAuthentication(string agencyCode, string agencyLogon, string agencyPasseword, string selectedAgency)
        {
            throw new NotImplementedException();
        }

        public Users TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword)
        {
            try
            {
                Users users = new Users();
                if (_objService != null)
                {
                    //Call old webservice
                    string xml = _objService.TravelAgentLogon(agencyCode, agentLogon, agentPassword);
                    if (xml != "")
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(xml);
                        XPathNavigator nv = xDoc.CreateNavigator();
                        User u = new User();
                        u.user_account_id = XmlHelper.XpathValueNullToGUID(nv, "UsersList/UserList/user_account_id");
                        u.user_logon = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/user_logon");
                        u.lastname = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/lastname");
                        u.firstname = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/firstname");
                        u.user_password = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/user_password");
                        u.email_address = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/email_address");
                        u.language_rcd = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/language_rcd");
                        u.make_bookings_for_others_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/make_bookings_for_others_flag");
                        u.address_default_code = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/address_default_code");
                        u.update_booking_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/update_booking_flag");
                        u.change_segment_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/change_segment_flag");
                        u.delete_segment_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/delete_segment_flag");
                        u.issue_ticket_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/issue_ticket_flag");
                        u.counter_sales_report_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/counter_sales_report_flag");
                        u.status_code = XmlHelper.XpathValueNullToEmpty(nv, "UsersList/UserList/status_code");
                        u.create_by = XmlHelper.XpathValueNullToGUID(nv, "UsersList/UserList/create_by");
                        u.update_by = XmlHelper.XpathValueNullToGUID(nv, "UsersList/UserList/update_by");
                        u.system_admin_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/system_admin_flag");
                        u.mon_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/mon_flag");
                        u.tue_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/tue_flag");
                        u.wed_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/wed_flag");
                        u.thu_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/thu_flag");
                        u.fri_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/fri_flag");
                        u.sat_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/sat_flag");
                        u.sun_flag = XmlHelper.XpathValueNullToByte(nv, "UsersList/UserList/sun_flag");
                        u.create_date_time = XmlHelper.XpathValueNullToDateTime(nv, "UsersList/UserList/create_date_time");
                        u.update_date_time = XmlHelper.XpathValueNullToDateTime(nv, "UsersList/UserList/update_date_time");
                        // Add all value to object
                        users.Add(u);
                        u = null;
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InitializeUserAccountID(string UserAccountId)
        {
            if (_objService != null)
            {
                _objService.InitializeUserAccountID(UserAccountId);
            }
        }

        public Agents GetAgencySessionProfile(string AgencyCode, string UserAccountID)
        {
            try
            {
                Agents agents = new Agents();
                if (_objService != null)
                {
                    DataSet ds = _objService.GetAgencySessionProfile(AgencyCode, UserAccountID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Agent ag;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ag = new Agent();

                            ag.agency_code = DataHelper.DBToString(dr, "agency_code");
                            ag.agency_logon = DataHelper.DBToString(dr, "agency_logon");
                            ag.agency_password = DataHelper.DBToString(dr, "agency_password");
                            ag.agency_name = DataHelper.DBToString(dr, "agency_name");
                            ag.airport_rcd = DataHelper.DBToString(dr, "airport_rcd");
                            ag.ag_language_rcd = DataHelper.DBToString(dr, "ag_language_rcd");
                            ag.default_e_ticket_flag = DataHelper.DBToByte(dr, "default_e_ticket_flag");
                            ag.email = DataHelper.DBToString(dr, "email");
                            ag.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                            ag.country_rcd = DataHelper.DBToString(dr, "country_rcd");
                            ag.agency_payment_type_rcd = DataHelper.DBToString(dr, "agency_payment_type_rcd");
                            ag.status_code = DataHelper.DBToString(dr, "status_code");
                            ag.default_show_passenger_flag = DataHelper.DBToByte(dr, "default_show_passenger_flag");
                            ag.default_auto_print_ticket_flag = DataHelper.DBToByte(dr, "default_auto_print_ticket_flag");
                            ag.default_ticket_on_save_flag = DataHelper.DBToByte(dr, "default_ticket_on_save_flag");
                            ag.web_agency_flag = DataHelper.DBToByte(dr, "web_agency_flag");
                            ag.own_agency_flag = DataHelper.DBToByte(dr, "own_agency_flag");
                            ag.b2b_credit_card_payment_flag = DataHelper.DBToByte(dr, "b2b_credit_card_payment_flag");
                            ag.b2b_voucher_payment_flag = DataHelper.DBToByte(dr, "b2b_voucher_payment_flag");
                            ag.b2b_eft_flag = DataHelper.DBToByte(dr, "b2b_eft_flag");
                            ag.b2b_post_paid_flag = DataHelper.DBToByte(dr, "b2b_post_paid_flag");
                            ag.b2b_allow_seat_assignment_flag = DataHelper.DBToByte(dr, "b2b_allow_seat_assignment_flag");
                            ag.b2b_allow_cancel_segment_flag = DataHelper.DBToByte(dr, "b2b_allow_cancel_segment_flag");
                            ag.b2b_allow_change_flight_flag = DataHelper.DBToByte(dr, "b2b_allow_change_flight_flag");
                            ag.b2b_allow_name_change_flag = DataHelper.DBToByte(dr, "b2b_allow_name_change_flag");
                            ag.b2b_allow_change_details_flag = DataHelper.DBToByte(dr, "b2b_allow_change_details_flag");
                            ag.ticket_stock_flag = DataHelper.DBToByte(dr, "ticket_stock_flag");
                            ag.b2b_allow_split_flag = DataHelper.DBToByte(dr, "b2b_allow_split_flag");
                            ag.b2b_allow_service_flag = DataHelper.DBToByte(dr, "b2b_allow_service_flag");
                            ag.b2b_group_waitlist_flag = DataHelper.DBToByte(dr, "b2b_group_waitlist_flag");
                            ag.avl_show_net_total_flag = DataHelper.DBToByte(dr, "avl_show_net_total_flag");
                            ag.default_user_account_id = DataHelper.DBToGuid(dr, "default_user_account_id");
                            ag.merchant_id = DataHelper.DBToGuid(dr, "merchant_id");
                            ag.default_customer_document_id = DataHelper.DBToGuid(dr, "default_customer_document_id");
                            ag.default_small_itinerary_document_id = DataHelper.DBToGuid(dr, "default_small_itinerary_document_id");
                            ag.default_internal_itinerary_document_id = DataHelper.DBToGuid(dr, "default_internal_itinerary_document_id");
                            ag.payment_default_code = DataHelper.DBToString(dr, "payment_default_code");
                            ag.agency_type_code = DataHelper.DBToString(dr, "agency_type_code");
                            ag.user_account_id = DataHelper.DBToGuid(dr, "user_account_id");
                            ag.user_logon = DataHelper.DBToString(dr, "user_logon");
                            ag.user_code = DataHelper.DBToString(dr, "user_code");
                            ag.lastname = DataHelper.DBToString(dr, "lastname");
                            ag.firstname = DataHelper.DBToString(dr, "firstname");
                            ag.language_rcd = DataHelper.DBToString(dr, "language_rcd");
                            ag.make_bookings_for_others_flag = DataHelper.DBToByte(dr, "make_bookings_for_others_flag");
                            ag.origin_rcd = DataHelper.DBToString(dr, "origin_rcd");
                            ag.outstanding_invoice = DataHelper.DBToDecimal(dr, "outstanding_invoice");
                            ag.booking_payment = DataHelper.DBToDecimal(dr, "booking_payment");
                            ag.agency_account = DataHelper.DBToDecimal(dr, "agency_account");
                            ag.company_client_profile_id = DataHelper.DBToGuid(dr, "company_client_profile_id");
                            ag.invoice_days = DataHelper.DBToString(dr, "invoice_days");
                            ag.address_line1 = DataHelper.DBToString(dr, "address_line1");
                            ag.address_line2 = DataHelper.DBToString(dr, "address_line2");
                            ag.city = DataHelper.DBToString(dr, "city");
                            ag.bank_code = DataHelper.DBToString(dr, "bank_code");
                            ag.bank_name = DataHelper.DBToString(dr, "bank_name");
                            ag.bank_account = DataHelper.DBToString(dr, "bank_account");
                            ag.contact_person = DataHelper.DBToString(dr, "contact_person");
                            ag.district = DataHelper.DBToString(dr, "district");
                            ag.phone = DataHelper.DBToString(dr, "phone");
                            ag.fax = DataHelper.DBToString(dr, "fax");
                            ag.po_box = DataHelper.DBToString(dr, "po_box");
                            ag.province = DataHelper.DBToString(dr, "province");
                            ag.state = DataHelper.DBToString(dr, "state");
                            ag.street = DataHelper.DBToString(dr, "street");
                            ag.zip_code = DataHelper.DBToString(dr, "zip_code");
                            ag.consolidator_flag = DataHelper.DBToByte(dr, "consolidator_flag");
                            ag.b2b_credit_agency_and_invoice_flag = DataHelper.DBToByte(dr, "b2b_credit_agency_and_invoice_flag");
                            ag.b2b_download_sales_report_flag = DataHelper.DBToByte(dr, "b2b_download_sales_report_flag");
                            ag.b2b_show_remarks_flag = DataHelper.DBToByte(dr, "b2b_show_remarks_flag");
                            ag.private_fares_flag = DataHelper.DBToByte(dr, "private_fares_flag");
                            ag.b2b_allow_group_flag = DataHelper.DBToByte(dr, "b2b_allow_group_flag");
                            ag.b2b_allow_waitlist_flag = DataHelper.DBToByte(dr, "b2b_allow_waitlist_flag");
                            ag.b2b_bsp_billing_flag = DataHelper.DBToByte(dr, "b2b_bsp_billing_flag");
                            ag.b2b_bsp_from_date = DataHelper.DBToDateTime(dr, "b2b_bsp_from_date");
                            ag.iata_number = DataHelper.DBToString(dr, "iata_number");
                            ag.send_mailto_all_passenger = DataHelper.DBToByte(dr, "send_mailto_all_passenger");
                            ag.website_address = DataHelper.DBToString(dr, "website_address");
                            ag.tty_address = DataHelper.DBToString(dr, "tty_address");
                            ag.create_date_time = DataHelper.DBToDateTime(dr, "create_date_time");
                            ag.update_date_time = DataHelper.DBToDateTime(dr, "update_date_time");
                            ag.cashbook_closing_rcd = DataHelper.DBToString(dr, "cashbook_closing_rcd");
                            ag.cashbook_closing_id = DataHelper.DBToGuid(dr, "cashbook_closing_id");
                            ag.create_by = DataHelper.DBToGuid(dr, "create_by");
                            ag.legal_name = DataHelper.DBToString(dr, "legal_name");
                            ag.tax_id = DataHelper.DBToString(dr, "tax_id");
                            ag.tax_id_verified_date_time = DataHelper.DBToDateTime(dr, "tax_id_verified_date_time");
                            ag.no_vat_flag = DataHelper.DBToByte(dr, "no_vat_flag");
                            ag.allow_no_tax = DataHelper.DBToByte(dr, "allow_no_tax");

                            ag.allow_add_segment_flag = DataHelper.DBToByte(dr, "allow_add_segment_flag");
                            ag.individual_firmed_flag = DataHelper.DBToByte(dr, "individual_firmed_flag");
                            ag.individual_waitlist_flag = DataHelper.DBToByte(dr, "individual_waitlist_flag");
                            ag.group_firmed_flag = DataHelper.DBToByte(dr, "group_firmed_flag");
                            ag.group_waitlist_flag = DataHelper.DBToByte(dr, "group_waitlist_flag");
                            ag.disable_changes_through_b2c_flag = DataHelper.DBToByte(dr, "disable_changes_through_b2c_flag");
                            ag.disable_web_checkin_flag = DataHelper.DBToByte(dr, "disable_web_checkin_flag");
                            ag.commission_percentage = DataHelper.DBToDecimal(dr, "commission_percentage");
                            ag.balance_lock_flag = DataHelper.DBToByte(dr, "balance_lock_flag");

                            ag.column_1_tax_rcd = DataHelper.DBToString(dr, "column_1_tax_rcd");
                            ag.column_2_tax_rcd = DataHelper.DBToString(dr, "column_2_tax_rcd");
                            ag.column_3_tax_rcd = DataHelper.DBToString(dr, "column_3_tax_rcd");

                            ag.allow_noshow_change_flight_flag = DataHelper.DBToByte(dr, "allow_noshow_change_flight_flag");
                            ag.allow_noshow_cancel_segment_flag = DataHelper.DBToByte(dr, "allow_noshow_cancel_segment_flag");


                            agents.Add(ag);
                            ag = null;
                        }
                    }
                }

                return agents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCorporateSessionProfile(string clientId, string LastName)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetCorporateSessionProfile(clientId, LastName);
            }

            return ds;
        }

        public DataSet GetTicketSales(string AgencyCode, string UserId, string Origin, string Destination, string Airline, string FlightNumber,
            DateTime FlightFrom, DateTime FlightTo, DateTime TicketingFrom, DateTime TicketingTo, string PassengerType, string Language)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetTicketSales(AgencyCode, UserId, Origin, Destination, Airline, FlightNumber, FlightFrom, FlightTo,
                    TicketingFrom, TicketingTo, PassengerType, Language);
            }

            return ds;
        }

        public DataSet GetAgencyTicketSales(string strAgency, string strCurrency, DateTime dtSalesFrom, DateTime dtSalesTo)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetAgencyTicketSales(strAgency, strCurrency, dtSalesFrom, dtSalesTo);
            }

            return ds;
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
            if (_objService != null)
            {
                return _objService.SavePayment(bookingId,
                                               XmlHelper.Serialize(header, false),
                                               XmlHelper.Serialize(segment, false),
                                               XmlHelper.Serialize(passenger, false),
                                               XmlHelper.Serialize(payment, false),
                                               XmlHelper.Serialize(mapping, false),
                                               XmlHelper.Serialize(fee, false),
                                               XmlHelper.Serialize(paymentFee, false),
                                               createTickets);
            }
            else
            {
                return string.Empty;
            }
        }
        public string SingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            if (_objService != null)
            {
                return _objService.SingleFlightQuoteSummary(XmlHelper.Serialize(flights, false),
                                                            XmlHelper.Serialize(passengers, false),
                                                            strAgencyCode,
                                                            strLanguage,
                                                            strCurrencyCode,
                                                            bNoVat);
            }
            else
            {
                return string.Empty;
            }
        }
        public string SessionlessSingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strToken, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                return objService.SessionlessSingleFlightQuoteSummary(XmlHelper.Serialize(flights, false),
                                                                    XmlHelper.Serialize(passengers, false),
                                                                    strAgencyCode,
                                                                    strToken,
                                                                    strLanguage,
                                                                    strCurrencyCode,
                                                                    bNoVat);
            }
        }

        public bool InsertPaymentApproval(string strPaymentApprovalID,
                    string strCardRcd,
                    string strCardNumber,
                    string strNameOnCard,
                    int iExpiryMonth,
                    int iExpiryYear,
                    int iIssueMonth,
                    int iIssueYear,
                    int iIssueNumber,
                    string strPaymentStateCode,
                    string strBookingPaymentId,
                    string strCurrencyRcd,
                    double dPaymentAmount,
                    string strIpAddress)
        {
            bool result = false;

            if (_objService != null)
            {
                result = _objService.InsertPaymentApproval(strPaymentApprovalID,
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
            }
            return result;
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
            bool result = false;

            if (_objService != null)
            {
                result = _objService.UpdatePaymentApproval(strApprovalCode,
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
            }
            return result;
        }


        public Double GetExchangeRateRead(string strOriginCurrencyCode, string strDestCurrencyCode)
        {
            Double ret = 0D;

            if (_objService != null)
            {
                ret = _objService.ExchangeRateRead(strOriginCurrencyCode, strDestCurrencyCode);
            }

            return ret;
        }

        // Start Yai Add Account Topup
        public bool AgencyAccountAdd(string strAgencyCode, string strCurrency, string strUserId, string strComment, double dAmount, string strExternalReference, string strInternalReference, string strTransactionReference, bool bExternalTopup)
        {
            bool bResult = false;
            if (_objService != null)
            {
                bResult = _objService.AgencyAccountAdd(strAgencyCode, strCurrency, strUserId, strComment, dAmount, strExternalReference, strInternalReference, strTransactionReference, bExternalTopup);
            }
            return bResult;
        }

        public bool AgencyAccountVoid(string strAgencyAccountId, string strUserId)
        {
            bool bResult = false;
            if (_objService != null)
            {
                bResult = _objService.AgencyAccountVoid(strAgencyAccountId, strUserId);
            }
            return bResult;
        }

        public DataSet ExternalPaymentListAgencyTopUp(string strAgencyCode)
        {
            DataSet dsResult = null;
            if (_objService != null)
            {
                dsResult = _objService.ExternalPaymentListAgencyTopUp(strAgencyCode);
            }
            return dsResult;
        }
        // End Yai Add Account Topup

        public string GetRecordLocator(ref string recordLocator, ref int bookingNumber)
        {
            if (_objService != null)
            {
                _objService.GetRecordLocator(ref recordLocator, ref bookingNumber);
            }
            return recordLocator;
        }
        // Tai Add New
        // Tai add GetTourOperators
        public string GetTourOperators(string language)
        {
            String strResult = "";
            if (_objService != null)
            { strResult = _objService.GetTourOperators(language); }

            return strResult;
        }
        // End Tai GetTourOperators
        public string GetVendorTourOperator(string strVendorRcd)
        {
            String strResult = "";
            if (_objService != null)
            { strResult = _objService.GetVendorTourOperator(strVendorRcd); }

            return strResult;
        }
        public string GetTourOperatorCodeMappingRead(string strTourOperatorId, Boolean bInclude)
        {
            String strResult = "";
            if (_objService != null)
            { strResult = _objService.GetTourOperatorCodeMappingRead(strTourOperatorId, bInclude); }
            return strResult;
        }

        //End Tai Add new
        public DataSet GetSessionlessOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            if (language.Length == 0)
            { language = "EN"; }
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                try
                {
                    return objService.GetSessionlessAirportOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                }
                catch
                {
                    return null;
                }
            }

        }

        public DataSet GetSessionlessDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            if (language.Length == 0)
            { language = "EN"; }
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                try
                {
                    return objService.GetSessionlessAirportDestinations(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, strToken);
                }
                catch
                {
                    return null;
                }
            }
        }

        public Fees GetSessionlessFeesDefinition(string strLanguage, string strToken)
        {
            Fees objFees = null;
            Fee f = null;
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                Library objLi = new Library();
                string strXml = objService.GetSessionlessFeesDefinition(strLanguage, strToken);
                if (string.IsNullOrEmpty(strXml) == false)
                {
                    objFees = new Fees();
                    using (StringReader rd = new StringReader(strXml))
                    {
                        using (XmlReader reader = XmlReader.Create(rd))
                        {
                            while (!reader.EOF)
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "BookingFees")
                                {
                                    f = new Fee();
                                    using (XmlReader SubReader = reader.ReadSubtree())
                                    {
                                        while (!SubReader.EOF)
                                        {
                                            switch (SubReader.Name)
                                            {
                                                case "fee_rcd":
                                                    f.fee_rcd = SubReader.ReadInnerXml();
                                                    break;
                                                case "display_name":
                                                    f.display_name = SubReader.ReadInnerXml();
                                                    break;
                                                case "manual_fee_flag":
                                                    f.manual_fee_flag = Convert.ToByte(SubReader.ReadInnerXml());
                                                    break;
                                                case "od_flag":
                                                    f.od_flag = Convert.ToByte(SubReader.ReadInnerXml());
                                                    break;
                                                case "skip_fare_allowance_flag":
                                                    f.skip_fare_allowance_flag = Convert.ToByte(SubReader.ReadInnerXml());
                                                    break;
                                                case "fee_level":
                                                    f.fee_level = SubReader.ReadInnerXml();
                                                    break;
                                                case "fee_category_rcd":
                                                    f.fee_category_rcd = SubReader.ReadInnerXml();
                                                    break;
                                                case "fee_calculation_rcd":
                                                    f.fee_calculation_rcd = SubReader.ReadInnerXml();
                                                    break;
                                                default:
                                                    SubReader.Read();
                                                    break;
                                            }
                                        }
                                    }
                                    objFees.Add(f);
                                }
                                else
                                {
                                    reader.Read();
                                }
                            }
                        }
                    }
                }
            }
            return objFees;
        }
        public Fees GetFee(string strFee, string strCurrency, string strAgencyCode, string strClass, string strFareBasis, string strOrigin, string strDestination, string strFlightNumber, DateTime dtDeparture, string strLanguage, bool bNoVat)
        {
            Library objLi = new Library();
            Fees objFees = new Fees();
            if (_objService != null)
            {
                string xmlResult = objService.GetFees(strCurrency,
                                                        dtDeparture,
                                                        strAgencyCode,
                                                        strFee,
                                                        strClass,
                                                        strFareBasis,
                                                        strOrigin,
                                                        strDestination,
                                                        strFlightNumber,
                                                        strLanguage,
                                                        bNoVat);

                if (!string.IsNullOrEmpty(xmlResult))
                {
                    XElement eles = XElement.Parse(xmlResult);
                    foreach (XElement e in eles.Elements("BookingFees"))
                    {
                        Fee fee = new Fee();
                        PropertyInfo[] props = fee.GetType().GetProperties();
                        for (int i = 0; i < props.Length; i++)
                        {
                            PropertyInfo prop = fee.GetType().GetProperty(props[i].Name);
                            string value = e.Element(props[i].Name) != null ? e.Element(props[i].Name).Value : null;
                            if (value != null)
                            {
                                object t = (object)TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(value.ToString());
                                prop.SetValue(fee, Convert.ChangeType(t, prop.PropertyType), null);
                            }
                        }
                        objFees.Add(fee);
                    }
                    //    using (StringReader srd = new StringReader(xmlResult))
                    //    {
                    //        XPathDocument xmlDoc = new XPathDocument(srd);
                    //        XPathNavigator nv = xmlDoc.CreateNavigator();

                    //        Fee f;
                    //        foreach (XPathNavigator n in nv.Select("NewDataSet/BookingFees"))
                    //        {
                    //            f = new Fee();
                    //            f.fee_id = XmlHelper.XpathValueNullToGUID(n, "fee_id");
                    //            f.fee_rcd = XmlHelper.XpathValueNullToEmpty(n, "fee_rcd");
                    //            f.currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                    //            f.fee_amount = XmlHelper.XpathValueNullToZero(n, "fee_amount");
                    //            f.fee_amount_incl = XmlHelper.XpathValueNullToZero(n, "fee_amount_incl");
                    //            f.vat_percentage = XmlHelper.XpathValueNullToZero(n, "vat_percentage");
                    //            f.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                    //            f.fee_category_rcd = XmlHelper.XpathValueNullToEmpty(n, "fee_category_rcd");
                    //            f.minimum_fee_amount_flag = XmlHelper.XpathValueNullToByte(n, "minimum_fee_amount_flag");
                    //            f.fee_percentage = XmlHelper.XpathValueNullToZero(n, "fee_percentage");
                    //            f.od_flag = XmlHelper.XpathValueNullToByte(n, "od_flag");

                    //            objFees.Add(f);
                    //        }
                    //    }
                }
            }
            return objFees;
        }
        public string GetBaggageFeeOptions(Mappings mappings, Guid gSegmentId, Guid gPassengerId, string strAgencyCode, string strLanguage, long lMaxunits, Fees fees, bool bApplySegmentFee, bool bNoVat)
        {
            Fees objFees = new Fees();
            if (_objService != null)
            {
                string strSegmentId = string.Empty;
                string strPassengerId = string.Empty;

                if (gSegmentId.Equals(Guid.Empty) == false)
                {
                    strSegmentId = gSegmentId.ToString();
                }
                if (gPassengerId.Equals(Guid.Empty) == false)
                {
                    strPassengerId = gPassengerId.ToString();
                }

                return _objService.GetBaggageFeeOptions(XmlHelper.Serialize(mappings, false),
                                                        strSegmentId,
                                                        strPassengerId,
                                                        strAgencyCode,
                                                        strLanguage,
                                                        lMaxunits,
                                                        XmlHelper.Serialize(fees, false),
                                                        bApplySegmentFee,
                                                        bNoVat);
            }
            return string.Empty;
        }

        public string GetAvailabilety(string strFlightID, string strOriginRcd, string strDestinationRcd, string strSpecialServiceRcd, string strBoardingClassRcd)
        {
            string strResult = "";
            if (_objService != null)
            { strResult = _objService.GetAvailabilety(strFlightID, strOriginRcd, strDestinationRcd, strSpecialServiceRcd, strBoardingClassRcd); }

            return strResult;
        }


        public string AddFee(string bookingId, string AgencyCode, BookingHeader header, Itinerary segment, Passengers passenger, string strFeeCode, Fees fees, string currency, Remarks remark, Payments payment, Mappings mapping, Services service, Taxes tax, string strLanguage, bool bNoVat)
        {
            string Result = string.Empty;

            if (_objService != null)
            {
                Result = (string)_objService.AddFee(AgencyCode,
                                                    currency,
                                                    bookingId,
                                                    strFeeCode,
                                                    XmlHelper.Serialize(header, false),
                                                    XmlHelper.Serialize(segment, false),
                                                    XmlHelper.Serialize(passenger, false),
                                                    XmlHelper.Serialize(fees, false),
                                                    XmlHelper.Serialize(remark, false),
                                                    XmlHelper.Serialize(payment, false),
                                                    XmlHelper.Serialize(mapping, false),
                                                    XmlHelper.Serialize(service, false),
                                                    XmlHelper.Serialize(tax, false),
                                                    strLanguage,
                                                    bNoVat);
            }

            return Result;
        }
        public string GetActiveBookings(Guid gClientProfileId)
        {
            throw new NotImplementedException();
        }
        public string GetFlownBookings(Guid gClientProfileId)
        {
            throw new NotImplementedException();
        }
        public string GetBookingClasses(string strBoardingclass, string language)
        {
            DataSet dsResult = null;
            if (_objService != null)
            { dsResult = _objService.GetBookingClasses(strBoardingclass, language); }

            return dsResult.GetXml();
        }
        public DataSet ReadFormOfPayment(string strType)
        {
            DataSet dsResult = null;
            dsResult = _objService.ReadFormOfPayment(strType);

            return dsResult;
        }
        public string GetAirportTimezone(string strAirport)
        {
            String zoneResult = "";
            zoneResult = _objService.GetAirportTimezone(strAirport);


            return zoneResult;
        }
        public string GetBooking(Guid bookingId)
        {
            DataSet dsResult = null;
            if (_objService != null)
            { dsResult = _objService.GetBooking(bookingId.ToString()); }

            return dsResult.GetXml();
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
            using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
            {
                return service.GetFlightsFLIFO(originRcd,
                                                destinationRcd,
                                                airlineCode,
                                                flightNumber,
                                                flightFrom,
                                                flightTo,
                                                languageCode,
                                                token);
            }
        }
        public string VoucherTemplateList(string voucherTemplateId, string voucherTemplate, DateTime fromDate, DateTime toDate, bool write, string status, string language)
        {
            throw new NotImplementedException();
        }

        public bool SaveVoucher(Vouchers vouchers)
        {
            throw new NotImplementedException();
        }

        public string VoucherPaymentCreditCard(Payments payment, Vouchers vouchers)
        {
            throw new NotImplementedException();
        }

        public string ReadVoucher(Guid voucherId, double voucherNumber)
        {
            throw new NotImplementedException();
        }

        public bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate)
        {
            throw new NotImplementedException();
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
            try
            {
                return _objService.AgencyRegistrationInsert(agencyName,
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
            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }
        }

        public bool LowFareFinderAllow(string agencyCode, string strToken)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                return true;
            }
            else
            {
                using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
                {
                    try
                    {
                        return objService.LowFareFinderAllow(agencyCode, strToken);
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
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
            throw new NotImplementedException();
        }
        public string GetQueueCount(string agency, bool unassigned)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IClientCore User Members
        public string GetUserList(string UserLogon, string UserCode, string LastName, string FirstName, string AgencyCode, string StatusCode)
        {
            string result = String.Empty;
            if (_objService != null)
            {
                result = _objService.GetUserList(UserLogon, UserCode, LastName, FirstName, AgencyCode, StatusCode);
            }

            return result;
        }

        public string UserRead(string UserID)
        {
            string result = String.Empty;
            if (_objService != null)
            {
                result = _objService.UserRead(UserID);
            }

            return result;
        }

        public bool UserSave(string strUsersXml, string agencyCode)
        {
            try
            {
                bool success = false;
                if (string.IsNullOrEmpty(strUsersXml) == false)
                {
                    if (_objService != null)
                    {
                        success = _objService.UserSave(strUsersXml, agencyCode);
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetServiceFees(ref string strOrigin, ref string strDestination, ref string strCurrency, ref string strAgency,
            ref string strServiceGroup, ref System.DateTime dtFee)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetServiceFees(ref strOrigin, ref strDestination, ref strCurrency, ref strAgency,
                                    ref strServiceGroup, ref dtFee);
            }

            return ds;
        }

        public DataSet GetClientSessionProfile(string clientProfileId)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetClientSessionProfile(clientProfileId);
            }

            return ds;
        }
        public DataSet GetBookingHistory(string bookingId)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetBookingHistory(bookingId);
            }

            return ds;
        }
        #endregion

        #region ClientCore B2A
        public DataSet GetAgencyAccountBalance(string strAgencyCode, string strAgencyName, string strCurrency, string strConsolidatorAgency)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.GetAgencyAccountBalance(strAgencyCode, strAgencyName, strCurrency, strConsolidatorAgency);
            }

            return ds;
        }

        public bool AddNewAgency(string strXmlAgency, string strAgencyCode, string strDefaultUserLogon, string strDefaultPassword, string strDefaultLastName, string strDefaultFirstName, string strCreateUser, short iChangeBooking, short iChangeSegment, short iDeleteSegment, short iTicketIssue)
        {
            bool bResult = false;
            if (_objService != null)
            {
                bResult = _objService.AddNewAgency(strXmlAgency, strAgencyCode, strDefaultUserLogon, strDefaultPassword, strDefaultLastName, strDefaultFirstName, strCreateUser, iChangeBooking, iChangeSegment, iDeleteSegment, iTicketIssue);
            }

            return bResult;
        }

        public bool AdjustSubAgencyAccountBalance(string strXmlChildAccountBalance, string strXmlParentAccountBalance)
        {
            bool bResult = false;
            if (_objService != null)
            {
                bResult = _objService.AdjustSubAgencyAccountBalance(strXmlChildAccountBalance, strXmlParentAccountBalance);
            }

            return bResult;
        }

        public DataSet AgencyRead(string strAgencyCode, bool bKeepSession)
        {
            DataSet ds = null;
            if (_objService != null)
            {
                ds = _objService.AgencyRead(strAgencyCode, bKeepSession);
            }

            return ds;
        }
        #endregion

        #region Helper
        private string GroupXML(string strXml, string strPath)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            StringWriter objWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(objWriter);
            Library objLi = new Library();

            xmlWriter.WriteStartElement("Booking");
            {
                foreach (XPathNavigator n in nv.Select(strPath))
                {
                    xmlWriter.WriteStartElement("AvailabilityFlight");
                    {
                        xmlWriter.WriteStartElement("airline_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "airline_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_number");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_number", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("booking_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "booking_class_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("boarding_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "boarding_class_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_id", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("origin_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "origin_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("destination_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "destination_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("origin_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "origin_name", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("destination_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "destination_name", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("departure_date");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "departure_date", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("planned_departure_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "planned_departure_time", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("planned_arrival_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "planned_arrival_time", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_id", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_code");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_code", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        // Start add new field for transit point
                        xmlWriter.WriteStartElement("transit_points");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_points", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_points_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_points_name", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();
                        // End add new field for transit point

                        xmlWriter.WriteStartElement("transit_flight_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_id", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_booking_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_booking_class_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_boarding_class_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_boarding_class_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_airport_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_airport_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_departure_date");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_departure_date", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_planned_departure_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_planned_departure_time", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_planned_arrival_time");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_planned_arrival_time", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_fare_id");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_fare_id", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_name");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_name", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("nesting_string");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "nesting_string", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("full_flight_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "full_flight_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("class_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "class_open_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("close_web_sales");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "close_web_sales", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("waitlist_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "waitlist_open_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("currency_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "currency_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("adult_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "adult_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("child_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "child_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("infant_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "infant_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("other_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "other_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("total_adult_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "total_adult_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("total_child_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "total_child_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("total_infant_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "total_infant_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("total_other_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "total_other_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_column");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_column", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_comment");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_comment", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_flight_comment");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_comment", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("filter_logic_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "filter_logic_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("restriction_text");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "restriction_text", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("endorsement_text");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "endorsement_text", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("fare_type_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "fare_type_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("redemption_points");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "redemption_points", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_redemption_points");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_redemption_points", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_duration");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_duration", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("promotion_code");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "promotion_code", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("nested_book_available");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "nested_book_available", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_airline_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_airline_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_flight_number");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_number", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_flight_status_rcd");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_status_rcd", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_flight_duration");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_flight_duration", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_nested_book_available");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_nested_book_available", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_waitlist_open_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_waitlist_open_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("transit_adult_fare");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "transit_adult_fare", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("flight_information_1");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "flight_information_1", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("corporate_fare_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "corporate_fare_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("refundable_flag");
                        xmlWriter.WriteValue(objLi.getXPathNodevalue(n, "refundable_flag", Library.xmlReturnType.value));
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            xmlWriter.Close();

            objWriter.Flush();
            objWriter.Close();

            return objWriter.ToString();
        }
        public string GetAvailabilityAgencyLogin(string Origin,
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
            throw new NotImplementedException();
        }
        public string CalculateSpecialServiceFees(string AgencyCode, string currency, string bookingId, string header, string service, string fees, string remark, string mapping, string strLanguage, bool bNoVat)
        {
            string Result = string.Empty;

            if (_objService != null)
            { Result = (string)_objService.CalculateSpecialServiceFees(AgencyCode, currency, bookingId, header, service, fees, remark, mapping, strLanguage, bNoVat); }

            return Result;
        }
        public string SegmentFee(string strAgencyCode, string strCurrency, string[] strFeeRcdGroup, Mappings mappings, int iPassengerNumber, int iInfantNumber, string strLanguage, bool specialService, bool bNoVat)
        {
            StringBuilder stbSegmentId = new StringBuilder();
            StringBuilder stb = new StringBuilder();
            string Result = string.Empty;
            //Create xml.
            if (mappings.Count > 0 && strFeeRcdGroup.Length > 0)
            {
                stb.Append("<booking>");
                for (int i = 0; i < strFeeRcdGroup.Length; i++)
                {
                    stbSegmentId.Remove(0, stbSegmentId.Length);
                    stb.Append("<" + strFeeRcdGroup[i] + ">");
                    foreach (Mapping mp in mappings)
                    {
                        if (stbSegmentId.ToString().IndexOf(mp.booking_segment_id.ToString()) < 0)
                        {
                            stbSegmentId.Append(mp.booking_segment_id.ToString() + "|");

                            stb.Append("<segment_fee>");
                            stb.Append("<flight_connection_id>" + ((mp.flight_connection_id.Equals(Guid.Empty)) ? string.Empty : mp.flight_connection_id.ToString()) + "</flight_connection_id>");
                            stb.Append("<origin_rcd>" + mp.origin_rcd + "</origin_rcd>");
                            stb.Append("<destination_rcd>" + mp.destination_rcd + "</destination_rcd>");
                            stb.Append("<od_origin_rcd>" + mp.od_origin_rcd + "</od_origin_rcd>");
                            stb.Append("<od_destination_rcd>" + mp.od_destination_rcd + "</od_destination_rcd>");
                            stb.Append("<booking_class_rcd>" + mp.booking_class_rcd + "</booking_class_rcd>");
                            stb.Append("<fare_code>" + mp.fare_code + "</fare_code>");
                            stb.Append("<airline_rcd>" + mp.airline_rcd + "</airline_rcd>");
                            stb.Append("<flight_number>" + mp.flight_number + "</flight_number>");
                            stb.Append("<departure_date>" + mp.departure_date.Year.ToString() + "-" +
                                                            mp.departure_date.Month.ToString() + "-" +
                                                            mp.departure_date.Day.ToString() + "</departure_date>");
                            stb.Append("<pax_count>" + iPassengerNumber + "</pax_count>");
                            stb.Append("<inf_count>" + iInfantNumber + "</inf_count>");

                            stb.Append("</segment_fee>");
                        }
                    }
                    stb.Append("</" + strFeeRcdGroup[i] + ">");
                }
                stb.Append("</booking>");

                if (_objService != null)
                {
                    if (specialService == false)
                    {
                        Result = (string)_objService.SegmentFee(strAgencyCode, strCurrency, stb.ToString(), strLanguage, bNoVat);
                    }
                    else
                    {
                        Result = (string)_objService.SpecialServiceFee(strAgencyCode, strCurrency, stb.ToString(), strLanguage, bNoVat);
                    }

                }
            }

            return Result;
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

            string Result = string.Empty;

            if (_objService != null)
            {
                Result = (string)_objService.SpecialServiceRead(strSpecialServiceCode,
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
            }

            return Result;
        }
        public Currencies GetCurrencies(string language)
        {
            try
            {
                Currencies currencies = new Currencies();
                DataSet ds = null;
                using (tikSystem.Web.Library.agentservice.TikAeroXMLwebservice service = new tikSystem.Web.Library.agentservice.TikAeroXMLwebservice())
                {
                    ds = service.GetSessionlessCurrencies(language,
                                                          SecurityHelper.GenerateSessionlessToken());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Currency c;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            c = new Currency();
                            c.currency_rcd = DataHelper.DBToString(dr, "currency_rcd");
                            c.currency_number = DataHelper.DBToString(dr, "currency_number");
                            c.display_name = DataHelper.DBToString(dr, "display_name");
                            c.max_voucher_value = DataHelper.DBToDecimal(dr, "max_voucher_value");
                            c.rounding_rule = DataHelper.DBToDecimal(dr, "rounding_rule");
                            c.number_of_decimals = DataHelper.DBToInt16(dr, "number_of_decimals");

                            currencies.Add(c);
                            c = null;
                        }
                    }
                }

                return currencies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSessionlessCurrencies(string language, string strToken)
        {
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                DataSet dsResult = null;
                dsResult = objService.GetSessionlessCurrencies(language, strToken);
                return dsResult.GetXml();
            }
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
            if (strLanguage.Length == 0)
            { strLanguage = "EN"; }
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                try
                {
                    return objService.SessionlessExternalPaymentAddPayment(strBookingId,
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
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string GetPassengerRole(string strLanguage)
        {
            string strMember = string.Empty; ;
            if (_objService != null)
            {
                strMember = _objService.GetPassengerRole(strLanguage);
            }
            return strMember;
        }
        public Services GetSpecialServices(string strLanguage)
        {
            try
            {
                Services services = new Services();
                string strXml = string.Empty;
                if (objService != null)
                {
                    strXml = objService.GetSpecialServices(strLanguage);
                    using (System.IO.StringReader sr = new System.IO.StringReader(strXml))
                    {
                        System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(sr);
                        System.Xml.XPath.XPathNavigator nv = xmlDoc.CreateNavigator();
                        Service sv;
                        foreach (System.Xml.XPath.XPathNavigator n in nv.Select("NewDataSet/Service"))
                        {
                            sv = new Service();

                            sv.special_service_rcd = XmlHelper.XpathValueNullToEmpty(n, "special_service_rcd");
                            sv.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                            sv.help_text = XmlHelper.XpathValueNullToEmpty(n, "help_text");
                            sv.special_service_group_rcd = XmlHelper.XpathValueNullToEmpty(n, "special_service_group_rcd");
                            sv.text_allowed_flag = XmlHelper.XpathValueNullToByte(n, "text_allowed_flag");
                            sv.inventory_control_flag = XmlHelper.XpathValueNullToByte(n, "inventory_control_flag");
                            sv.manifest_flag = XmlHelper.XpathValueNullToByte(n, "manifest_flag");
                            sv.text_required_flag = XmlHelper.XpathValueNullToByte(n, "text_required_flag");
                            sv.service_on_request_flag = XmlHelper.XpathValueNullToByte(n, "service_on_request_flag");
                            sv.include_passenger_name_flag = XmlHelper.XpathValueNullToByte(n, "include_passenger_name_flag");
                            sv.include_flight_segment_flag = XmlHelper.XpathValueNullToByte(n, "include_flight_segment_flag");
                            sv.include_action_code_flag = XmlHelper.XpathValueNullToByte(n, "include_action_code_flag");
                            sv.include_number_of_service_flag = XmlHelper.XpathValueNullToByte(n, "include_number_of_service_flag");
                            sv.include_catering_flag = XmlHelper.XpathValueNullToByte(n, "include_catering_flag");
                            sv.include_passenger_assistance_flag = XmlHelper.XpathValueNullToByte(n, "include_passenger_assistance_flag");
                            sv.service_supported_flag = XmlHelper.XpathValueNullToByte(n, "service_supported_flag");
                            sv.send_interline_reply_flag = XmlHelper.XpathValueNullToByte(n, "send_interline_reply_flag");
                            sv.cut_off_time = XmlHelper.XpathValueNullToInt(n, "cut_off_time");
                            sv.status_code = XmlHelper.XpathValueNullToEmpty(n, "status_code");

                            services.Add(sv);
                            sv = null;
                        }
                    }
                }

                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdatePassengerDocumentDetails(Passengers passengers)
        {
            int iResult = 0;
            if (objService != null)
            {
                agentservice.Passenger[] pax = new agentservice.Passenger[passengers.Count];
                if (passengers.Count > 0)
                {
                    for (int i = 0; i < passengers.Count; i++)
                    {
                        pax[i] = new agentservice.Passenger();
                        pax[i].passenger_id = passengers[i].passenger_id;
                        pax[i].gender_type_rcd = passengers[i].gender_type_rcd;
                        pax[i].date_of_birth = passengers[i].date_of_birth;
                        pax[i].nationality_rcd = passengers[i].nationality_rcd;
                        pax[i].passport_issue_country_rcd = passengers[i].passport_issue_country_rcd;
                        pax[i].passport_expiry_date = passengers[i].passport_expiry_date;
                        pax[i].passport_number = passengers[i].passport_number;

                        //18-04-2013 : add doucment type for checkinAPI
                        pax[i].document_type_rcd = passengers[i].document_type_rcd;

                        //add known_traveler_number
                        pax[i].known_traveler_number = passengers[i].known_traveler_number;

                    }
                    if (pax.Length == passengers.Count)
                    {
                        iResult = objService.UpdatePassengerCheckinDetails(pax);
                    }
                }
            }
            return iResult;
        }
        public string GetFlightSummary(Passengers passengers,
                                        Flights flights,
                                        string strAgencyCode,
                                        string strLanguage,
                                        string strCurrencyCode,
                                        bool bNoVat)
        {
            using (agentservice.TikAeroXMLwebservice objService = new agentservice.TikAeroXMLwebservice())
            {
                return objService.SessionlessSingleFlightQuoteSummary(XmlHelper.Serialize(flights, false),
                                                                      XmlHelper.Serialize(passengers, false),
                                                                      strAgencyCode,
                                                                      SecurityHelper.GenerateSessionlessToken(),
                                                                      strLanguage,
                                                                      strCurrencyCode,
                                                                      bNoVat);
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _objService = null;
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
            throw new NotImplementedException();
        }

        public bool RemarkDelete(Guid bookingRemarkId)
        {
            throw new NotImplementedException();
        }
        public bool RemarkComplete(Guid bookingRemarkId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public string RemarkRead(string remarkId,
                                string bookingId,
                                string bookingReference,
                                double bookingNumber,
                                bool readOnly)
        {
            throw new NotImplementedException();
        }

        public bool RemarkSave(Remarks remarks)
        {
            throw new NotImplementedException();
        }
        public string BoardingClassRead(string boardingClassCode, string boardingClass, string sortSeq, string status, bool bWrite)
        {
            using (TikAeroXMLwebservice objService = new TikAeroXMLwebservice())
            {
                try
                {
                    return objService.BoardingClassRead(boardingClassCode,
                                                        boardingClass,
                                                        sortSeq,
                                                        status,
                                                        bWrite,
                                                        SecurityHelper.GenerateSessionlessToken());
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion


        public string ViewBookingChange(string bookingID, string languageCode, string AgencyCode)
        {
            string Result = string.Empty;
            string bookingFeeId = string.Empty;

            if (_objService != null)
            { Result = _objService.ViewBookingChange(bookingID, languageCode, AgencyCode); }

            return Result;
        }
    }
}
