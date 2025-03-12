using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;

namespace tikSystem.Web.Library
{
    public class Availabilities : Itinerary
    {
        string _token = string.Empty;
        public Availabilities()
        { }
        public Availabilities(string token)
        {
            _token = token;
        }
        public new Availability this[int index]
        {
            get
            {
                return (Availability)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }
        public int Add(Availability Value)
        {
            return (List.Add(Value));
        }

        #region Method
        public string GetLowestFareGroupAvailability(string strOriginRcd,
                                                    string strDestinationRcd,
                                                    string strDepart,
                                                    string strReturn,
                                                    string strOriginName,
                                                    string strDestinationName,
                                                    bool bOneWay,
                                                    short iDayRange,
                                                    short iAdult,
                                                    short iChild,
                                                    short iInfant,
                                                    short iOther,
                                                    string strOtherPaxType,
                                                    string strBoardingClass,
                                                    string strAgencyCode,
                                                    string strCurrencyRcd,
                                                    bool bGroupBooking,
                                                    string strPromoCode,
                                                    bool bFareLogic,
                                                    bool bReturnFlight,
                                                    string strIpAddress,
                                                    string strLanguage,
                                                    bool bSkipFareLogic,
                                                    short iNumberOfColumn,
                                                    string strSearchType,
                                                    decimal dclOutSelectedFare,
                                                    Guid gOutSelectedFlightId,
                                                    Guid gOutSelectedFareId,
                                                    decimal dclReturnSelectedFare,
                                                    Guid gReturnSelectedFlightId,
                                                    Guid gReturnSelectedFareId,
                                                    bool bReturnRefundable,
                                                    bool bNoVat,
                                                    bool skipOutBoundDayRange,
                                                    int iTabNumber)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {
                bool bLowestGroup = true;
                bool bLowestFare = false;
                bool bLowestClass = false;

                if (strSearchType == "POINT")
                {
                    bLowestGroup = true;
                    bLowestFare = true;
                    bLowestClass = true;
                }
                else
                {
                    if (iNumberOfColumn == 0)
                    {
                        bLowestGroup = true;
                        bLowestFare = true;
                        bLowestClass = false;
                    }
                }

                //Find Flight Availability.
                string strResult = GetFlightAvailability(strOriginRcd,
                                                        strDestinationRcd,
                                                        dtDepartFrom,
                                                        dtDepartTo,
                                                        dtReturnFrom,
                                                        dtReturnTo,
                                                        DateTime.MinValue,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        strBoardingClass,
                                                        string.Empty,
                                                        string.Empty,
                                                        strAgencyCode,
                                                        strCurrencyRcd,
                                                        string.Empty,
                                                        string.Empty,
                                                        0.0,
                                                        false,
                                                        false,
                                                        false,
                                                        true,
                                                        true,
                                                        false,
                                                        bGroupBooking,
                                                        false,
                                                        strPromoCode,
                                                        strSearchType,
                                                        bFareLogic,
                                                        (bOneWay == true) ? false : true,
                                                        bLowestFare, //bLowest
                                                        bLowestClass, //bLowestClass
                                                        bLowestGroup, //bLowestGroup
                                                        false, //bShowClosed
                                                        false, //bSort
                                                        true, //bDelete
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);
                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    return GroupLowestFlightAvailability(SecurityHelper.DecompressString(strResult),
                                                        strOriginRcd,
                                                        strDestinationRcd,
                                                        strOriginName,
                                                        strDestinationName,
                                                        strCurrencyRcd,
                                                        DataHelper.ParseDate(strDepart),
                                                        DataHelper.ParseDate(strReturn),
                                                        iNumberOfColumn,
                                                        strSearchType,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        bOneWay,
                                                        "LF",
                                                        dclOutSelectedFare,
                                                        gOutSelectedFlightId,
                                                        gOutSelectedFareId,
                                                        dclReturnSelectedFare,
                                                        gReturnSelectedFlightId,
                                                        gReturnSelectedFareId,
                                                        iDayRange,
                                                        iTabNumber);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetAllFareAvailability(string strOriginRcd,
                                            string strDestinationRcd,
                                            string strDepart,
                                            string strReturn,
                                            string strOriginName,
                                            string strDestinationName,
                                            bool bOneWay,
                                            short iDayRange,
                                            short iAdult,
                                            short iChild,
                                            short iInfant,
                                            short iOther,
                                            string strOtherPaxType,
                                            string strBoardingClass,
                                            string strAgencyCode,
                                            string strCurrencyRcd,
                                            bool bGroupBooking,
                                            string strPromoCode,
                                            bool bFareLogic,
                                            bool bReturnFlight,
                                            string strIpAddress,
                                            string strLanguage,
                                            bool bSkipFareLogic,
                                            short iNumberOfColumn,
                                            string strSearchType,
                                            decimal dclOutSelectedFare,
                                            Guid gOutSelectedFlightId,
                                            Guid gOutSelectedFareId,
                                            decimal dclReturnSelectedFare,
                                            Guid gReturnSelectedFlightId,
                                            Guid gReturnSelectedFareId,
                                            bool skipGroupLowestFlightAvailability,
                                            bool bReturnRefundable,
                                            bool bNoVat,
                                            bool skipOutBoundDayRange,
                                            int iTabNumber)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {
                //Find Flight Availability.
                string strResult = GetFlightAvailability(strOriginRcd,
                                                        strDestinationRcd,
                                                        dtDepartFrom,
                                                        dtDepartTo,
                                                        dtReturnFrom,
                                                        dtReturnTo,
                                                        DateTime.MinValue,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        strBoardingClass,
                                                        string.Empty,
                                                        string.Empty,
                                                        strAgencyCode,
                                                        strCurrencyRcd,
                                                        string.Empty,
                                                        string.Empty,
                                                        0.0,
                                                        false,
                                                        false,
                                                        false,
                                                        true,
                                                        true,
                                                        false,
                                                        bGroupBooking,
                                                        false,
                                                        strPromoCode,
                                                        strSearchType,
                                                        bFareLogic,
                                                        (bOneWay == true) ? false : true,
                                                        true, //bLowest
                                                        true, //bLowestClass
                                                        true, //bLowestGroup
                                                        false, //bShowClosed
                                                        false, //bSort
                                                        true, //bDelete
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);

                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    if (skipGroupLowestFlightAvailability == false)
                    {
                        return GroupLowestFlightAvailability(SecurityHelper.DecompressString(strResult),
                                                            strOriginRcd,
                                                            strDestinationRcd,
                                                            strOriginName,
                                                            strDestinationName,
                                                            strCurrencyRcd,
                                                            DataHelper.ParseDate(strDepart),
                                                            DataHelper.ParseDate(strReturn),
                                                            iNumberOfColumn,
                                                            strSearchType,
                                                            iAdult,
                                                            iChild,
                                                            iInfant,
                                                            iOther,
                                                            strOtherPaxType,
                                                            bOneWay,
                                                            "AF",
                                                            dclOutSelectedFare,
                                                            gOutSelectedFlightId,
                                                            gOutSelectedFareId,
                                                            dclReturnSelectedFare,
                                                            gReturnSelectedFlightId,
                                                            gReturnSelectedFareId,
                                                            iDayRange,
                                                            iTabNumber);
                    }
                    else
                    {
                        return SecurityHelper.DecompressString(strResult);
                    }
                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetLowestFareAvailability(string strOriginRcd,
                                            string strDestinationRcd,
                                            string strDepart,
                                            string strReturn,
                                            string strOriginName,
                                            string strDestinationName,
                                            bool bOneWay,
                                            short iDayRange,
                                            short iAdult,
                                            short iChild,
                                            short iInfant,
                                            short iOther,
                                            string strOtherPaxType,
                                            string strBoardingClass,
                                            string strAgencyCode,
                                            string strCurrencyRcd,
                                            bool bGroupBooking,
                                            string strPromoCode,
                                            bool bFareLogic,
                                            bool bReturnFlight,
                                            string strIpAddress,
                                            string strLanguage,
                                            bool bSkipFareLogic,
                                            short iNumberOfColumn,
                                            string strSearchType,
                                            decimal dclOutSelectedFare,
                                            Guid gOutSelectedFlightId,
                                            Guid gOutSelectedFareId,
                                            decimal dclReturnSelectedFare,
                                            Guid gReturnSelectedFlightId,
                                            Guid gReturnSelectedFareId,
                                            bool skipGroupLowestFlightAvailability,
                                            bool bReturnRefundable,
                                            bool bNoVat,
                                            bool skipOutBoundDayRange,
                                            int iTabNumber)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {
                //Find Flight Availability.
                string strResult = GetFlightAvailability(strOriginRcd,
                                                        strDestinationRcd,
                                                        dtDepartFrom,
                                                        dtDepartTo,
                                                        dtReturnFrom,
                                                        dtReturnTo,
                                                        DateTime.MinValue,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        strBoardingClass,
                                                        string.Empty,
                                                        string.Empty,
                                                        strAgencyCode,
                                                        strCurrencyRcd,
                                                        string.Empty,
                                                        string.Empty,
                                                        0.0,
                                                        true,//NonStopOnly
                                                        false,//IncludeDeparted
                                                        false,//IncludeCancelled
                                                        true,//IncludeWaitlisted
                                                        true,//IncludeSoldOut
                                                        false,//Refundable
                                                        bGroupBooking,
                                                        false,//ItFareOnly
                                                        strPromoCode,
                                                        strSearchType,
                                                        bFareLogic,
                                                        (bOneWay == true) ? false : true,
                                                        true, //bLowest
                                                        false, //bLowestClass
                                                        true, //bLowestGroup
                                                        false, //bShowClosed
                                                        false, //bSort
                                                        true, //bDelete
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);

                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    if (skipGroupLowestFlightAvailability == false)
                    {
                        return GroupLowestFlightAvailability(SecurityHelper.DecompressString(strResult),
                                                        strOriginRcd,
                                                        strDestinationRcd,
                                                        strOriginName,
                                                        strDestinationName,
                                                        strCurrencyRcd,
                                                        DataHelper.ParseDate(strDepart),
                                                        DataHelper.ParseDate(strReturn),
                                                        iNumberOfColumn,
                                                        strSearchType,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        bOneWay,
                                                        "AF",
                                                        dclOutSelectedFare,
                                                        gOutSelectedFlightId,
                                                        gOutSelectedFareId,
                                                        dclReturnSelectedFare,
                                                        gReturnSelectedFlightId,
                                                        gReturnSelectedFareId,
                                                        iDayRange,
                                                        iTabNumber);
                    }
                    else
                    {
                        return SecurityHelper.DecompressString(strResult);
                    }
                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetFlexibleFareAvailability(string strOriginRcd,
                                    string strDestinationRcd,
                                    string strDepart,
                                    string strReturn,
                                    string strOriginName,
                                    string strDestinationName,
                                    bool bOneWay,
                                    short iDayRange,
                                    short iAdult,
                                    short iChild,
                                    short iInfant,
                                    short iOther,
                                    string strOtherPaxType,
                                    string strBoardingClass,
                                    string strAgencyCode,
                                    string strCurrencyRcd,
                                    bool bGroupBooking,
                                    string strPromoCode,
                                    bool bFareLogic,
                                    bool bReturnFlight,
                                    string strIpAddress,
                                    string strLanguage,
                                    bool bSkipFareLogic,
                                    short iNumberOfColumn,
                                    string strSearchType,
                                    decimal dclOutSelectedFare,
                                    Guid gOutSelectedFlightId,
                                    Guid gOutSelectedFareId,
                                    decimal dclReturnSelectedFare,
                                    Guid gReturnSelectedFlightId,
                                    Guid gReturnSelectedFareId,
                                    bool skipGroupLowestFlightAvailability,
                                    bool bReturnRefundable,
                                    bool bNoVat,
                                    bool skipOutBoundDayRange,
                                    int iTabNumber)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {
                //Find Flight Availability.
                string strResult = GetFlightAvailability(strOriginRcd,
                                                        strDestinationRcd,
                                                        dtDepartFrom,
                                                        dtDepartTo,
                                                        dtReturnFrom,
                                                        dtReturnTo,
                                                        DateTime.MinValue,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        strBoardingClass,
                                                        string.Empty,
                                                        string.Empty,
                                                        strAgencyCode,
                                                        strCurrencyRcd,
                                                        string.Empty,
                                                        string.Empty,
                                                        0.0,
                                                        true,//NonStopOnly
                                                        false,//IncludeDeparted
                                                        false,//IncludeCancelled
                                                        true,//IncludeWaitlisted
                                                        true,//IncludeSoldOut
                                                        true,//Refundable
                                                        bGroupBooking,
                                                        false,//ItFareOnly
                                                        strPromoCode,
                                                        strSearchType,
                                                        bFareLogic,
                                                        (bOneWay == true) ? false : true,
                                                        false, //bLowest
                                                        false, //bLowestClass
                                                        false, //bLowestGroup
                                                        false, //bShowClosed
                                                        false, //bSort
                                                        true, //bDelete
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);

                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    if (skipGroupLowestFlightAvailability == false)
                    {
                        return GroupLowestFlightAvailability(SecurityHelper.DecompressString(strResult),
                                                            strOriginRcd,
                                                            strDestinationRcd,
                                                            strOriginName,
                                                            strDestinationName,
                                                            strCurrencyRcd,
                                                            DataHelper.ParseDate(strDepart),
                                                            DataHelper.ParseDate(strReturn),
                                                            iNumberOfColumn,
                                                            strSearchType,
                                                            iAdult,
                                                            iChild,
                                                            iInfant,
                                                            iOther,
                                                            strOtherPaxType,
                                                            bOneWay,
                                                            "AF",
                                                            dclOutSelectedFare,
                                                            gOutSelectedFlightId,
                                                            gOutSelectedFareId,
                                                            dclReturnSelectedFare,
                                                            gReturnSelectedFlightId,
                                                            gReturnSelectedFareId,
                                                            iDayRange,
                                                            iTabNumber);
                    }
                    else
                    {
                        return SecurityHelper.DecompressString(strResult);
                    }
                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetITFareAvailability(string strOriginRcd,
                            string strDestinationRcd,
                            string strDepart,
                            string strReturn,
                            string strOriginName,
                            string strDestinationName,
                            bool bOneWay,
                            short iDayRange,
                            short iAdult,
                            short iChild,
                            short iInfant,
                            short iOther,
                            string strOtherPaxType,
                            string strBoardingClass,
                            string strAgencyCode,
                            string strCurrencyRcd,
                            bool bGroupBooking,
                            string strPromoCode,
                            bool bFareLogic,
                            bool bReturnFlight,
                            string strIpAddress,
                            string strLanguage,
                            bool bSkipFareLogic,
                            short iNumberOfColumn,
                            string strSearchType,
                            decimal dclOutSelectedFare,
                            Guid gOutSelectedFlightId,
                            Guid gOutSelectedFareId,
                            decimal dclReturnSelectedFare,
                            Guid gReturnSelectedFlightId,
                            Guid gReturnSelectedFareId,
                            bool skipGroupLowestFlightAvailability,
                            bool bReturnRefundable,
                            bool bNoVat,
                            bool skipOutBoundDayRange,
                            int iTabNumber)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {
                //Find Flight Availability.
                string strResult = GetFlightAvailability(strOriginRcd,
                                                        strDestinationRcd,
                                                        dtDepartFrom,
                                                        dtDepartTo,
                                                        dtReturnFrom,
                                                        dtReturnTo,
                                                        DateTime.MinValue,
                                                        iAdult,
                                                        iChild,
                                                        iInfant,
                                                        iOther,
                                                        strOtherPaxType,
                                                        strBoardingClass,
                                                        string.Empty,
                                                        string.Empty,
                                                        strAgencyCode,
                                                        strCurrencyRcd,
                                                        string.Empty,
                                                        string.Empty,
                                                        0.0,
                                                        true,//NonStopOnly
                                                        false,//IncludeDeparted
                                                        false,//IncludeCancelled
                                                        true,//IncludeWaitlisted
                                                        true,//IncludeSoldOut
                                                        false,//Refundable
                                                        bGroupBooking,
                                                        true,//ItFareOnly
                                                        strPromoCode,
                                                        strSearchType,
                                                        bFareLogic,
                                                        (bOneWay == true) ? false : true,
                                                        false, //bLowest
                                                        true, //bLowestClass
                                                        true, //bLowestGroup
                                                        false, //bShowClosed
                                                        false, //bSort
                                                        true, //bDelete
                                                        bSkipFareLogic,
                                                        strLanguage,
                                                        strIpAddress,
                                                        bReturnRefundable,
                                                        bNoVat,
                                                        iDayRange);

                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    if (skipGroupLowestFlightAvailability != false)
                    {
                        return GroupLowestFlightAvailability(SecurityHelper.DecompressString(strResult),
                                                            strOriginRcd,
                                                            strDestinationRcd,
                                                            strOriginName,
                                                            strDestinationName,
                                                            strCurrencyRcd,
                                                            DataHelper.ParseDate(strDepart),
                                                            DataHelper.ParseDate(strReturn),
                                                            iNumberOfColumn,
                                                            strSearchType,
                                                            iAdult,
                                                            iChild,
                                                            iInfant,
                                                            iOther,
                                                            strOtherPaxType,
                                                            bOneWay,
                                                            "AF",
                                                            dclOutSelectedFare,
                                                            gOutSelectedFlightId,
                                                            gOutSelectedFareId,
                                                            dclReturnSelectedFare,
                                                            gReturnSelectedFlightId,
                                                            gReturnSelectedFareId,
                                                            iDayRange,
                                                            iTabNumber);
                    }
                    else
                    {
                        return SecurityHelper.DecompressString(strResult);
                    }

                }
                else
                {
                    return strResult;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetLowFareFinderAvailability(string strOriginRcd,
                                                   string strDestinationRcd,
                                                   string strDepart,
                                                   string strReturn,
                                                   string strOriginName,
                                                   string strDestinationName,
                                                   bool bOneWay,
                                                   short iDayRange,
                                                   short iAdult,
                                                   short iChild,
                                                   short iInfant,
                                                   short iOther,
                                                   string strOtherPaxType,
                                                   string strBoardingClass,
                                                   string strAgencyCode,
                                                   string strCurrencyRcd,
                                                   bool bGroupBooking,
                                                   string strPromoCode,
                                                   bool bFareLogic,
                                                   bool bReturnFlight,
                                                   string strIpAddress,
                                                   string strLanguage,
                                                   bool bSkipFareLogic,
                                                   short iNumberOfColumn,
                                                   string strSearchType,
                                                   DayOfWeek BeginingDayOfWeeks,
                                                   bool bCalendarMode,
                                                   string selectDate,
                                                   bool bReturnRefundable,
                                                   bool bNoVat,
                                                   bool skipOutBoundDayRange)
        {
            DateTime dtDepartFrom = DateTime.MinValue;
            DateTime dtDepartTo = DateTime.MinValue;
            DateTime dtReturnFrom = DateTime.MinValue;
            DateTime dtReturnTo = DateTime.MinValue;

            if (ValidateSearchAvailability(strOriginRcd,
                                           strDestinationRcd,
                                           strDepart,
                                           strReturn,
                                           strOriginName,
                                           strDestinationName,
                                           bOneWay,
                                           skipOutBoundDayRange,
                                           iDayRange,
                                           ref dtDepartFrom,
                                           ref dtDepartTo,
                                           ref dtReturnFrom,
                                           ref dtReturnTo) == true)
            {

                //Find Flight Availability.
                string strResult = string.Empty;
                if ((DataHelper.DateDifferent(dtDepartFrom, dtDepartTo).Days > 31) ||
                        (DataHelper.DateDifferent(dtReturnFrom, dtReturnTo).Days > 31))
                {
                    return string.Empty;
                }
                else
                {
                    //Search Avalability
                    ServiceClient srvClient = new ServiceClient();
                    strResult = srvClient.GetSessionlessLowFareFinder(strOriginRcd,
                                                                       strDestinationRcd,
                                                                       dtDepartFrom,
                                                                       dtDepartTo,
                                                                       dtReturnFrom,
                                                                       dtReturnTo,
                                                                       DateTime.MinValue,
                                                                       iAdult,
                                                                       iChild,
                                                                       iInfant,
                                                                       iOther,
                                                                       strOtherPaxType,
                                                                       strBoardingClass,
                                                                       string.Empty,
                                                                       string.Empty,
                                                                       strAgencyCode,
                                                                       strCurrencyRcd,
                                                                       string.Empty,
                                                                       string.Empty,
                                                                       0,
                                                                       false,
                                                                       false,
                                                                       false,
                                                                       true,
                                                                       true,
                                                                       false,
                                                                       bGroupBooking,
                                                                       false,
                                                                       strPromoCode,
                                                                       strSearchType,
                                                                       bFareLogic,
                                                                       (bOneWay == true) ? false : true,
                                                                       true, //bLowest
                                                                       false, //bLowestClass
                                                                       true, //bLowestGroup
                                                                       false, //bShowClosed
                                                                       false, //bSort
                                                                       true, //bDelete
                                                                       bSkipFareLogic,
                                                                       strLanguage,
                                                                       strIpAddress,
                                                                       _token,
                                                                       bReturnRefundable,
                                                                       bNoVat,
                                                                       iDayRange);
                }

                //Group Xml
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    StringBuilder stb = new StringBuilder();
                    DateTime dtDepart = DataHelper.ParseDate(strDepart);
                    DateTime dtReturn = DataHelper.ParseDate(strReturn);
                    CreateLowFareFinderXml(stb,
                                            SecurityHelper.DecompressString(strResult),
                                            strCurrencyRcd,
                                            BeginingDayOfWeeks,
                                            strOriginRcd,
                                            strDestinationRcd,
                                            strOriginName,
                                            strDestinationName,
                                            dtDepart,
                                            dtReturn,
                                            iAdult,
                                            iChild,
                                            iInfant,
                                            iOther,
                                            strOtherPaxType,
                                            iDayRange,
                                            bCalendarMode,
                                            bOneWay,
                                            selectDate);
                    return stb.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }


        public bool ReleaseFlightInventorySession(string bookingId)
        {
            ServiceClient srvClient = new ServiceClient();
            return srvClient.ReleaseSessionlessFlightInventorySession(string.Empty,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      bookingId,
                                                                      false,
                                                                      true,
                                                                      false,
                                                                      string.Empty);

        }


        public bool LowFareFinderAllow(string agencyCode)
        {
            try
            {
                if (ConfigurationHelper.ToByte("OffLowFareFinder") == 1)
                {
                    return false;
                }
                else
                {
                    IClientCore obj = new ServiceClient();
                    return obj.LowFareFinderAllow(agencyCode, _token);
                }
            }
            catch
            {
                return true;
            }
        }
        #endregion

        #region Helper
        private string GroupLowestFlightAvailability(string availabilityXml,
                                                       string strOrigin,
                                                       string strDestination,
                                                       string strOriginName,
                                                       string strDestinationName,
                                                       string currencyCode,
                                                       DateTime dtDepaturedate,
                                                       DateTime dtArrivalTime,
                                                       short iNumberOfColumn,
                                                       string strSearchType,
                                                       short sAdult,
                                                       short sChild,
                                                       short sInfant,
                                                       short sOther,
                                                       string strOtherPaxType,
                                                       bool bOneWay,
                                                       string SearchFareType,
                                                       decimal dclOutSelectedFare,
                                                       Guid gOutSelectedFlightId,
                                                       Guid gOutSelectedFareId,
                                                       decimal dclReturnSelectedFare,
                                                       Guid gReturnSelectedFlightId,
                                                       Guid gReturnSelectedFareId,
                                                       int searchDayRange,
                                                       int tabNumber)
        {
            StringBuilder stb = new StringBuilder();
            XPathDocument xmlDoc;

            using (StringWriter stw = new StringWriter(stb))
            {
                XmlTextWriter xtw = new XmlTextWriter(stw);
                xtw.WriteStartElement("Availability");
                {
                    //Start Construct availability xml group.
                    using (StringReader srd = new StringReader(availabilityXml))
                    {
                        if (srd.Peek() > -1)
                        {
                            using (XmlReader reader = XmlReader.Create(srd))
                            {
                                while (!reader.EOF)
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (reader.Name)
                                        {
                                            case "AvailabilityOutbound":
                                                xtw.WriteStartElement("AvailabilityOutbound");
                                                {
                                                    xtw.WriteStartElement("FlightGroup");
                                                    {
                                                        xtw.WriteStartElement("one_way");
                                                        xtw.WriteValue(bOneWay.ToString());
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("flight_type");
                                                        xtw.WriteValue("Outward");
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("origin_rcd");
                                                        xtw.WriteValue(strOrigin);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("destination_rcd");
                                                        xtw.WriteValue(strDestination);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("origin_name");
                                                        xtw.WriteValue(strOriginName);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("destination_name");
                                                        xtw.WriteValue(strDestinationName);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("currency_rcd");
                                                        xtw.WriteValue(currencyCode);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("departure_date");
                                                        xtw.WriteValue(dtDepaturedate);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("fare_type_rcd");
                                                        xtw.WriteValue(strSearchType);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("number_of_adult");
                                                        xtw.WriteValue(sAdult);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("number_of_child");
                                                        xtw.WriteValue(sChild);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("number_of_infant");
                                                        xtw.WriteValue(sInfant);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("number_of_other");
                                                        xtw.WriteValue(sOther);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("other_type_display");
                                                        xtw.WriteValue(strOtherPaxType);
                                                        xtw.WriteEndElement();

                                                        xtw.WriteStartElement("search_day_range");
                                                        xtw.WriteValue(searchDayRange);
                                                        xtw.WriteEndElement();

                                                        if (dclOutSelectedFare > 0)
                                                        {
                                                            xtw.WriteStartElement("fare");
                                                            xtw.WriteValue(dclOutSelectedFare);
                                                            xtw.WriteEndElement();
                                                        }

                                                        if (gOutSelectedFlightId.Equals(Guid.Empty) == false)
                                                        {
                                                            xtw.WriteStartElement("selected_flight_id");
                                                            xtw.WriteValue(gOutSelectedFlightId.ToString());
                                                            xtw.WriteEndElement();
                                                        }

                                                        if (gOutSelectedFareId.Equals(Guid.Empty) == false)
                                                        {
                                                            xtw.WriteStartElement("selected_fare_id");
                                                            xtw.WriteValue(gOutSelectedFareId.ToString());
                                                            xtw.WriteEndElement();
                                                        }

                                                        using (XmlReader r = reader.ReadSubtree())
                                                        {
                                                            if (r.IsEmptyElement == false)
                                                            {
                                                                if (tabNumber > 0)
                                                                {
                                                                    //Extract tab information from flight schedule information.
                                                                    GetAvailableTab(xtw,
                                                                                    strOrigin,
                                                                                    strDestination,
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    dtDepaturedate.AddDays(-tabNumber),
                                                                                    dtDepaturedate.AddDays(tabNumber),
                                                                                    string.Empty);
                                                                }

                                                                xmlDoc = new XPathDocument(r);
                                                                GroupFlight(ref xtw, xmlDoc, true, iNumberOfColumn, strSearchType, true, SearchFareType);
                                                                xmlDoc = null;
                                                            }
                                                        }
                                                        reader.Read();
                                                    }
                                                    xtw.WriteEndElement();
                                                }
                                                xtw.WriteEndElement();
                                                break;
                                            case "AvailabilityReturn":

                                                xtw.WriteStartElement("AvailabilityReturn");
                                                {
                                                    xtw.WriteStartElement("FlightGroup");
                                                    {
                                                        xtw.WriteStartElement("one_way");
                                                        xtw.WriteValue(bOneWay);
                                                        xtw.WriteEndElement();// End OneWay

                                                        xtw.WriteStartElement("flight_type");
                                                        xtw.WriteValue("Return");
                                                        xtw.WriteEndElement();// End max column

                                                        xtw.WriteStartElement("origin_rcd");
                                                        xtw.WriteValue(strDestination);
                                                        xtw.WriteEndElement();// End origin_rcd

                                                        xtw.WriteStartElement("destination_rcd");
                                                        xtw.WriteValue(strOrigin);
                                                        xtw.WriteEndElement();// End destination_rcd

                                                        xtw.WriteStartElement("origin_name");
                                                        xtw.WriteValue(strDestinationName);
                                                        xtw.WriteEndElement();// End origin_name

                                                        xtw.WriteStartElement("destination_name");
                                                        xtw.WriteValue(strOriginName);
                                                        xtw.WriteEndElement();// End destination_name

                                                        xtw.WriteStartElement("currency_rcd");
                                                        xtw.WriteValue(currencyCode);
                                                        xtw.WriteEndElement();// End max column

                                                        xtw.WriteStartElement("departure_date");
                                                        xtw.WriteValue(dtArrivalTime);
                                                        xtw.WriteEndElement();// End departure_date column

                                                        xtw.WriteStartElement("fare_type_rcd");
                                                        xtw.WriteValue(strSearchType);
                                                        xtw.WriteEndElement();// End fare_type_rcd column

                                                        xtw.WriteStartElement("number_of_adult");
                                                        xtw.WriteValue(sAdult);
                                                        xtw.WriteEndElement();// End number_of_adult column

                                                        xtw.WriteStartElement("number_of_child");
                                                        xtw.WriteValue(sChild);
                                                        xtw.WriteEndElement();// End number_of_child column

                                                        xtw.WriteStartElement("number_of_infant");
                                                        xtw.WriteValue(sInfant);
                                                        xtw.WriteEndElement();// End number_of_infant column

                                                        xtw.WriteStartElement("number_of_other");
                                                        xtw.WriteValue(sOther);
                                                        xtw.WriteEndElement();// End number_of_infant column

                                                        xtw.WriteStartElement("search_day_range");
                                                        xtw.WriteValue(searchDayRange);
                                                        xtw.WriteEndElement();

                                                        if (dclReturnSelectedFare > 0)
                                                        {
                                                            xtw.WriteStartElement("fare");
                                                            xtw.WriteValue(dclReturnSelectedFare);
                                                            xtw.WriteEndElement();
                                                        }

                                                        if (gReturnSelectedFlightId.Equals(Guid.Empty) == false)
                                                        {
                                                            xtw.WriteStartElement("selected_flight_id");
                                                            xtw.WriteValue(gReturnSelectedFlightId.ToString());
                                                            xtw.WriteEndElement();
                                                        }

                                                        if (gReturnSelectedFareId.Equals(Guid.Empty) == false)
                                                        {
                                                            xtw.WriteStartElement("selected_fare_id");
                                                            xtw.WriteValue(gReturnSelectedFareId.ToString());
                                                            xtw.WriteEndElement();
                                                        }
                                                        using (XmlReader r = reader.ReadSubtree())
                                                        {
                                                            if (r.IsEmptyElement == false)
                                                            {
                                                                if (tabNumber > 0)
                                                                {
                                                                    //Extract tab information from flight schedule information.
                                                                    GetAvailableTab(xtw,
                                                                                    strDestination,
                                                                                    strOrigin,
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    dtArrivalTime.AddDays(-tabNumber),
                                                                                    dtArrivalTime.AddDays(tabNumber),
                                                                                    string.Empty);
                                                                }

                                                                xmlDoc = new XPathDocument(r);
                                                                GroupFlight(ref xtw, xmlDoc, false, iNumberOfColumn, strSearchType, true, SearchFareType);
                                                                xmlDoc = null;
                                                            }
                                                        }
                                                        reader.Read();
                                                    }
                                                    xtw.WriteEndElement();
                                                }
                                                xtw.WriteEndElement();
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
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
                xtw.WriteEndElement();
                xtw.Close();
                xtw.Flush();
            }

            return stb.ToString();
        }
        private void GroupFlight(ref XmlTextWriter objXmlWriter,
                               XPathDocument xmlDoc,
                               bool Outward,
                               short iNumberOfColumn,
                               string strSearchType,
                               bool bFromReader,
                               string SearchFareType)
        {
            string strFlightId = string.Empty;
            string strTransitFlightID = string.Empty;

            string pathSelection;
            string strDepartureDate = string.Empty;

            StringBuilder tempFlightIDTransitFlightID = new StringBuilder();
            Library li = new Library();
            int MaxColumn = 0;
            int sStartWith = 0;

            if (strSearchType != "POINT")
            {
                MaxColumn = iNumberOfColumn;
                sStartWith = (iNumberOfColumn == 0) ? 0 : 1;
            }

            if (Outward == true)
            {
                if (bFromReader == true)
                { pathSelection = "AvailabilityOutbound/AvailabilityFlight"; }
                else
                { pathSelection = "Availability/AvailabilityOutbound/AvailabilityFlight"; }
            }
            else
            {
                if (bFromReader == true)
                { pathSelection = "AvailabilityReturn/AvailabilityFlight"; }
                else
                { pathSelection = "Availability/AvailabilityReturn/AvailabilityFlight"; }
            }

            XPathNavigator nv = xmlDoc.CreateNavigator();
            foreach (XPathNavigator n in nv.Select(pathSelection))
            {
                strFlightId = n.SelectSingleNode("flight_id").InnerXml;
                strTransitFlightID = n.SelectSingleNode("transit_flight_id").InnerXml;
                strDepartureDate = n.SelectSingleNode("departure_date").InnerXml;

                if (tempFlightIDTransitFlightID.ToString().Contains(strFlightId + "|" + strTransitFlightID + "|" + strDepartureDate) == false)
                {
                    //Add Flight Information

                    tempFlightIDTransitFlightID.Append(strFlightId);
                    tempFlightIDTransitFlightID.Append("|");
                    tempFlightIDTransitFlightID.Append(strTransitFlightID);
                    tempFlightIDTransitFlightID.Append("|");
                    tempFlightIDTransitFlightID.Append(strDepartureDate);
                    tempFlightIDTransitFlightID.Append("{}");

                    objXmlWriter.WriteStartElement("flight");
                    {
                        objXmlWriter.WriteStartElement("max_column");
                        objXmlWriter.WriteValue(MaxColumn);
                        objXmlWriter.WriteEndElement();// End max column

                        objXmlWriter.WriteStartElement("flight_id");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_id"));
                        objXmlWriter.WriteEndElement();// End flight_id

                        objXmlWriter.WriteStartElement("airline_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "airline_rcd"));
                        objXmlWriter.WriteEndElement();// End airline_rcd

                        objXmlWriter.WriteStartElement("airline_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "airline_name"));
                        objXmlWriter.WriteEndElement();// End airline_name

                        objXmlWriter.WriteStartElement("flight_number");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_number"));
                        objXmlWriter.WriteEndElement();// End flight_number

                        objXmlWriter.WriteStartElement("operating_airline_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "operating_airline_rcd"));
                        objXmlWriter.WriteEndElement();// End airline_rcd

                        objXmlWriter.WriteStartElement("operating_airline_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "operating_airline_name"));
                        objXmlWriter.WriteEndElement();// End airline_name

                        objXmlWriter.WriteStartElement("operating_flight_number");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "operating_flight_number"));
                        objXmlWriter.WriteEndElement();// End flight_number

                        objXmlWriter.WriteStartElement("origin_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "origin_rcd"));
                        objXmlWriter.WriteEndElement();// End origin_rcd

                        objXmlWriter.WriteStartElement("origin_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "origin_name"));
                        objXmlWriter.WriteEndElement();// End origin_name

                        objXmlWriter.WriteStartElement("destination_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "destination_rcd"));
                        objXmlWriter.WriteEndElement();// End destination_rcd

                        objXmlWriter.WriteStartElement("destination_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "destination_name"));
                        objXmlWriter.WriteEndElement();// End destination_name

                        objXmlWriter.WriteStartElement("departure_date");
                        objXmlWriter.WriteValue(strDepartureDate);
                        objXmlWriter.WriteEndElement();// End departure_date

                        objXmlWriter.WriteStartElement("arrival_date");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "arrival_date"));
                        objXmlWriter.WriteEndElement();// End arrival_date

                        objXmlWriter.WriteStartElement("planned_departure_time");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "planned_departure_time"));
                        objXmlWriter.WriteEndElement();// End planned_departure_time

                        objXmlWriter.WriteStartElement("planned_arrival_time");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "planned_arrival_time"));
                        objXmlWriter.WriteEndElement();// End planned_arrival_time

                        objXmlWriter.WriteStartElement("flight_comment");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_comment"));
                        objXmlWriter.WriteEndElement();// End flight_comment

                        // Start add new field for transit point
                        objXmlWriter.WriteStartElement("transit_points");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_points"));
                        objXmlWriter.WriteEndElement();

                        objXmlWriter.WriteStartElement("transit_points_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_points_name"));
                        objXmlWriter.WriteEndElement();
                        // End add new field for transit point

                        //Transit flight information
                        objXmlWriter.WriteStartElement("transit_flight_id");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_id"));
                        objXmlWriter.WriteEndElement();// End transit_flight_id

                        objXmlWriter.WriteStartElement("transit_airline_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_airline_rcd"));
                        objXmlWriter.WriteEndElement();// End transit_airline_rcd 

                        objXmlWriter.WriteStartElement("transit_airport_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd"));
                        objXmlWriter.WriteEndElement();// End transit_airport_rcd 

                        objXmlWriter.WriteStartElement("transit_name");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_name"));
                        objXmlWriter.WriteEndElement();// End transit_name 

                        objXmlWriter.WriteStartElement("transit_flight_number");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_number"));
                        objXmlWriter.WriteEndElement();// End transit_flight_number

                        objXmlWriter.WriteStartElement("transit_flight_status_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_status_rcd"));
                        objXmlWriter.WriteEndElement();// End transit_flight_status_rcd 

                        objXmlWriter.WriteStartElement("transit_departure_date");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_departure_date"));
                        objXmlWriter.WriteEndElement();// End transit_departure_date

                        objXmlWriter.WriteStartElement("transit_arrival_date");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_arrival_date"));
                        objXmlWriter.WriteEndElement();// End transit_arrival_date

                        objXmlWriter.WriteStartElement("transit_planned_departure_time");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_planned_departure_time"));
                        objXmlWriter.WriteEndElement();// End transit_planned_departure_time

                        objXmlWriter.WriteStartElement("transit_planned_arrival_time");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_planned_arrival_time"));
                        objXmlWriter.WriteEndElement();// End transit_planned_arrival_time

                        objXmlWriter.WriteStartElement("transit_fare_code");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_fare_code"));
                        objXmlWriter.WriteEndElement();// End transit_fare_code 

                        objXmlWriter.WriteStartElement("transit_fare_id");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_fare_id"));
                        objXmlWriter.WriteEndElement();// End transit_fare_id

                        objXmlWriter.WriteStartElement("transit_bookable_capacity");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_bookable_capacity"));
                        objXmlWriter.WriteEndElement();// End transit_bookable_capacity

                        objXmlWriter.WriteStartElement("transit_adult_fare");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_adult_fare"));
                        objXmlWriter.WriteEndElement();// End transit_adult_fare

                        objXmlWriter.WriteStartElement("transit_child_fare");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_child_fare"));
                        objXmlWriter.WriteEndElement();// End transit_child_fare

                        objXmlWriter.WriteStartElement("transit_infant_fare");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_infant_fare"));
                        objXmlWriter.WriteEndElement();// End transit_infant_fare

                        objXmlWriter.WriteStartElement("transit_other_fare");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_other_fare"));
                        objXmlWriter.WriteEndElement();// End transit_other_fare

                        objXmlWriter.WriteStartElement("transit_flight_comment");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_comment"));
                        objXmlWriter.WriteEndElement();// End transit_flight_comment

                        objXmlWriter.WriteStartElement("transit_flight_duration");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_duration"));
                        objXmlWriter.WriteEndElement();// End transit_flight_duration

                        objXmlWriter.WriteStartElement("transit_aircraft_type_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_aircraft_type_rcd"));
                        objXmlWriter.WriteEndElement();// End transit_aircraft_type_rcd

                        objXmlWriter.WriteStartElement("flight_duration");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_duration"));
                        objXmlWriter.WriteEndElement();// End flight_duration

                        objXmlWriter.WriteStartElement("flight_information_1");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_information_1"));
                        objXmlWriter.WriteEndElement();// End flight_information_1

                        objXmlWriter.WriteStartElement("aircraft_type_rcd");
                        objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "aircraft_type_rcd"));
                        objXmlWriter.WriteEndElement();// End aircraft_type_rcd

                        //Search Fare Group by SearchFareType AF = AllFare and LF = LowFare
                        if (SearchFareType != null && (SearchFareType == "AF" || strSearchType == "POINT"))
                        {
                            FindAllFares(ref objXmlWriter,
                                     xmlDoc,
                                     strFlightId,
                                     strTransitFlightID,
                                     n.SelectSingleNode("nesting_string").InnerXml,
                                     pathSelection);
                        }
                        else
                        {
                            GetGroupFare(ref objXmlWriter,
                                      xmlDoc,
                                      strFlightId,
                                      strTransitFlightID,
                                      strDepartureDate,
                                      n.SelectSingleNode("nesting_string").InnerXml,
                                      pathSelection,
                                      sStartWith,
                                      MaxColumn);
                        }



                        objXmlWriter.WriteEndElement();//End Flight Element
                    }
                }
            }
        }
        private void GetGroupFare(ref XmlTextWriter objXmlWriter,
                                    XPathDocument xmlDoc,
                                    string FlightId,
                                    string TransitFlightID,
                                    string strFlightDate,
                                    string strNestString,
                                    string pathSelection,
                                    int sStartWith,
                                    int MaxColumn)
        {
            bool bFoundFlight = false;
            Library li = new Library();
            XPathNavigator nvFare = xmlDoc.CreateNavigator();

            objXmlWriter.WriteStartElement("fare");
            {
                for (int i = sStartWith; i <= MaxColumn; i++)
                {
                    bFoundFlight = false;
                    foreach (XPathNavigator x in nvFare.Select(pathSelection + "[flight_id = '" + FlightId + "'][fare_column = " + i.ToString() + "][transit_flight_id = '" + TransitFlightID + "'][departure_date = '" + strFlightDate + "']"))
                    {
                        bFoundFlight = true;
                        GetXmlGroup(ref  objXmlWriter, x, i, FlightId, TransitFlightID);
                    }

                    //Adding the empty fare column.
                    if (bFoundFlight == false)
                    {

                        GetXmlGroup(ref objXmlWriter, null, i, FlightId, TransitFlightID);

                    }
                }
                objXmlWriter.WriteEndElement();//End fare Element
            }
        }

        private void FindAllFares(ref XmlTextWriter objXmlWriter,
                                        XPathDocument xmlDoc,
                                        string FlightId,
                                        string TransitFlightID,
                                        string strNestString,
                                        string pathSelection)
        {

            XPathNavigator nvFare = xmlDoc.CreateNavigator();
            objXmlWriter.WriteStartElement("fare");
            {
                foreach (XPathNavigator x in nvFare.Select(pathSelection + "[flight_id = '" + FlightId + "'][transit_flight_id = '" + TransitFlightID + "']"))
                {
                    GetXmlGroup(ref  objXmlWriter, x, 0, FlightId, TransitFlightID);
                }
                objXmlWriter.WriteEndElement();//End fare Element
            }
        }
        private bool ValidateSearchAvailability(string strOrigin,
                                                string strDestination,
                                                string strDepartDate,
                                                string strReturnDate,
                                                string strOriginName,
                                                string strDestinationName,
                                                bool bOneWay,
                                                bool skipOutBoundDayRange,
                                                int iDayRange,
                                                ref DateTime dtDepartFrom,
                                                ref DateTime dtDepartTo,
                                                ref DateTime dtReturnFrom,
                                                ref DateTime dtReturnTo)
        {
            if (string.IsNullOrEmpty(strOrigin) == true ||
                string.IsNullOrEmpty(strDestination) == true ||
                string.IsNullOrEmpty(strDepartDate) == true ||
                strDepartDate.Length != 8 ||
                string.IsNullOrEmpty(strOriginName) == true ||
                string.IsNullOrEmpty(strDestinationName) == true ||
                DataHelper.DateValid(strDepartDate) == false ||
                (!bOneWay && strReturnDate.Length != 8) ||
                (!bOneWay && DataHelper.DateValid(strReturnDate) == false) ||
                (string.IsNullOrEmpty(strReturnDate) == false && (Convert.ToInt32(strDepartDate) > Convert.ToInt32(strReturnDate)) && !bOneWay))
            {
                return false;
            }
            else
            {
                //if the selected date is less than today date then take today date.
                if (Convert.ToInt32(strDepartDate) < Convert.ToInt32(string.Format("{0:yyyyMMdd}", DateTime.Now)))
                {
                    dtDepartFrom = DateTime.Now;
                }
                else
                {
                    dtDepartFrom = DataHelper.ParseDate(strDepartDate);
                }
                if (iDayRange > -1)
                {
                    if (skipOutBoundDayRange == false | bOneWay == true)
                    {
                        // If oneway or not allowd fare logic, then allowed day range.
                        //Add number of days serarch
                        dtDepartTo = dtDepartFrom.AddDays(iDayRange);
                        dtDepartFrom = dtDepartFrom.AddDays(-iDayRange);
                    }
                    else
                    {
                        //If round trip and allowed fare logic, then the deprture date can't search for day range.
                        dtDepartTo = dtDepartFrom;
                    }

                    if (bOneWay == false)
                    {
                        dtReturnFrom = DataHelper.ParseDate(strReturnDate);
                        //Add number of days serarch
                        dtReturnTo = dtReturnFrom.AddDays(iDayRange);
                        dtReturnFrom = dtReturnFrom.AddDays(-iDayRange);
                    }
                }
                else
                {
                    //Take every day in the month
                    dtDepartTo = new DateTime(dtDepartFrom.Year, dtDepartFrom.Month, DateTime.DaysInMonth(dtDepartFrom.Year, dtDepartFrom.Month));
                    if (bOneWay == false)
                    {
                        dtReturnFrom = DataHelper.ParseDate(strReturnDate);
                        //Add number of days serarch
                        dtReturnTo = new DateTime(dtReturnFrom.Year, dtReturnFrom.Month, DateTime.DaysInMonth(dtReturnFrom.Year, dtReturnFrom.Month));

                    }
                }
                return true;
            }
        }
        private void GetXmlGroup(ref XmlTextWriter objXmlWriter, XPathNavigator x, int iposition, string FlightId, string TransitFlightID)
        {
            Library li = new Library();

            Int16 fullFlightFlag = 0;
            Int16 classOpenFlag = 0;
            Int16 closeWebSales = 0;
            string transitClassOpenFlag = "";

            objXmlWriter.WriteStartElement("group");
            {

                if (x != null)
                {
                    fullFlightFlag = Convert.ToInt16(li.getXPathNodevalue(x, "full_flight_flag", Library.xmlReturnType.value));
                    classOpenFlag = Convert.ToInt16(li.getXPathNodevalue(x, "class_open_flag", Library.xmlReturnType.value));
                    //check transit class
                    if (li.getXPathNodevalue(x, "transit_class_open_flag", Library.xmlReturnType.value) != null)
                    {
                        transitClassOpenFlag = XmlHelper.XpathValueNullToEmpty(x, "transit_class_open_flag");
                    }
                    if (transitClassOpenFlag != "")
                    {
                        if (classOpenFlag == 1 && transitClassOpenFlag == "1")
                        {
                            classOpenFlag = 1;
                        }
                        else if ((classOpenFlag == 1 && transitClassOpenFlag == "0") || (classOpenFlag == 0 && transitClassOpenFlag == "1"))
                        {
                            classOpenFlag = 0;
                        }
                    }
                    closeWebSales = Convert.ToInt16(li.getXPathNodevalue(x, "close_web_sales", Library.xmlReturnType.value));
                }

                objXmlWriter.WriteStartElement("flight_id");
                if (x == null) { objXmlWriter.WriteValue(FlightId); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "flight_id")); }
                objXmlWriter.WriteEndElement();//End flight_id

                objXmlWriter.WriteStartElement("transit_flight_id");
                if (x == null) { objXmlWriter.WriteValue(TransitFlightID); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "transit_flight_id")); }
                objXmlWriter.WriteEndElement();//End transit_flight_id

                objXmlWriter.WriteStartElement("fare_id");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "fare_id")); }
                objXmlWriter.WriteEndElement();//End fare_id

                objXmlWriter.WriteStartElement("transit_fare_id");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "transit_fare_id")); }
                objXmlWriter.WriteEndElement();//End transit_fare_id

                objXmlWriter.WriteStartElement("fare_column");
                if (x == null) { objXmlWriter.WriteValue(0); }
                else { objXmlWriter.WriteValue(iposition); }
                objXmlWriter.WriteEndElement();//End fare_column

                objXmlWriter.WriteStartElement("fare_code");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "fare_code")); }
                objXmlWriter.WriteEndElement();//End fare_code

                objXmlWriter.WriteStartElement("adult_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "adult_fare")); }
                objXmlWriter.WriteEndElement();//End adult_fare

                objXmlWriter.WriteStartElement("child_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "child_fare")); }
                objXmlWriter.WriteEndElement();//End child_fare

                objXmlWriter.WriteStartElement("infant_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "infant_fare")); }
                objXmlWriter.WriteEndElement();//End infant_fare

                objXmlWriter.WriteStartElement("other_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "other_fare")); }
                objXmlWriter.WriteEndElement();//End infant_fare

                objXmlWriter.WriteStartElement("total_adult_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "total_adult_fare")); }
                objXmlWriter.WriteEndElement();//End total_adult_fare

                objXmlWriter.WriteStartElement("total_child_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "total_child_fare")); }
                objXmlWriter.WriteEndElement();//End total_child_fare

                objXmlWriter.WriteStartElement("total_infant_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "total_infant_fare")); }
                objXmlWriter.WriteEndElement();//End total_infant_fare

                objXmlWriter.WriteStartElement("total_other_fare");
                if (x == null) { objXmlWriter.WriteValue("0"); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToZero(x, "total_other_fare")); }
                objXmlWriter.WriteEndElement();//End total_other_fare

                objXmlWriter.WriteStartElement("full_flight_flag");
                objXmlWriter.WriteValue(fullFlightFlag);
                objXmlWriter.WriteEndElement();//End full_flight_flag

                objXmlWriter.WriteStartElement("class_open_flag");
                objXmlWriter.WriteValue(classOpenFlag);
                objXmlWriter.WriteEndElement();//End class_open_flag

                objXmlWriter.WriteStartElement("close_web_sales");
                objXmlWriter.WriteValue(closeWebSales);
                objXmlWriter.WriteEndElement();//End close_web_sales

                objXmlWriter.WriteStartElement("waitlist_open_flag");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "waitlist_open_flag")); }
                objXmlWriter.WriteEndElement();//End waitlist_open_flag

                objXmlWriter.WriteStartElement("booking_class_rcd");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "booking_class_rcd")); }
                objXmlWriter.WriteEndElement();//End booking_class_rcd

                objXmlWriter.WriteStartElement("boarding_class_rcd");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(x, "boarding_class_rcd")); }
                objXmlWriter.WriteEndElement();//End boarding_class_rcd

                objXmlWriter.WriteStartElement("transit_booking_class_rcd");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_booking_class_rcd").InnerXml); }
                objXmlWriter.WriteEndElement();//End booking_class_rcd

                objXmlWriter.WriteStartElement("transit_boarding_class_rcd");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_boarding_class_rcd").InnerXml); }
                objXmlWriter.WriteEndElement();//End boarding_class_rcd

                objXmlWriter.WriteStartElement("restriction_text");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("restriction_text").InnerXml); }
                objXmlWriter.WriteEndElement();//End restriction_text

                objXmlWriter.WriteStartElement("endorsement_text");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("endorsement_text").InnerXml); }
                objXmlWriter.WriteEndElement();//End endorsement_text

                objXmlWriter.WriteStartElement("fare_type_rcd");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("fare_type_rcd").InnerXml); }
                objXmlWriter.WriteEndElement();//End fare_type_rcd

                objXmlWriter.WriteStartElement("redemption_points");
                if (x == null) { objXmlWriter.WriteValue(0); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("redemption_points").InnerXml); }
                objXmlWriter.WriteEndElement();//End redemption_points

                objXmlWriter.WriteStartElement("transit_redemption_points");
                if (x == null) { objXmlWriter.WriteValue(0); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_redemption_points").InnerXml); }
                objXmlWriter.WriteEndElement();//End transit_redemption_points

                objXmlWriter.WriteStartElement("promotion_code");
                if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                else { objXmlWriter.WriteValue(x.SelectSingleNode("promotion_code").InnerXml); }
                objXmlWriter.WriteEndElement();//End promotion_code      

            }
            objXmlWriter.WriteEndElement();//End group
        }

        private void GroupLowestFareFinderMode(ref XmlTextWriter objXmlWriter,
                                                  XPathDocument xmlDoc,
                                                  string SearchCurrencyRcd,
                                                  bool Outward,
                                                  DayOfWeek BeginingDayOfWeeks,
                                                  string originRcd,
                                                  string destinationRcd,
                                                  string strOriginName,
                                                  string strDestinationName,
                                                  Int16 iAdult,
                                                  Int16 iChild,
                                                  Int16 iInfant,
                                                  Int16 iOther,
                                                  string strOtherType,
                                                  short iSearchRange,
                                                  DateTime dtSelectedDate)
        {

            Library objLi = new Library();
            XPathExpression exp;
            XPathNavigator nv = xmlDoc.CreateNavigator();

            DateTime dt = DateTime.MinValue;
            DateTime DayEndOfWeek = DateTime.MinValue;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US", true);
            ci.DateTimeFormat.FirstDayOfWeek = BeginingDayOfWeeks;

            if (Outward == true)
            {
                objXmlWriter.WriteStartElement("flight_departure");
                exp = nv.Compile("AvailabilityOutbound/AvailabilityFlight[full_flight_flag = 0][class_open_flag = 1][close_web_sales = 0]");
            }
            else
            {
                objXmlWriter.WriteStartElement("flight_return");
                exp = nv.Compile("AvailabilityReturn/AvailabilityFlight[full_flight_flag = 0][class_open_flag = 1][close_web_sales = 0]");
            }
            {
                exp.AddSort("departure_date", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);
                exp.AddSort("total_adult_fare", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Number);

                //Setting Section.
                CreateLFFSettingField(objXmlWriter, Outward, originRcd, destinationRcd, strOriginName, strDestinationName, iAdult, iChild, iInfant, iOther, strOtherType, iSearchRange, dtSelectedDate);

                foreach (XPathNavigator n in nv.Select(exp))
                {
                    if (dt != Convert.ToDateTime(XmlHelper.XpathValueNullToEmpty(n, "departure_date")))
                    {
                        dt = XmlHelper.XpathValueNullToDateTime(n, "departure_date");

                        //Fill Flight Information.
                        CreateLFFFlightField(n, objXmlWriter, SearchCurrencyRcd, dt, false);
                    }
                }
            }
            objXmlWriter.WriteEndElement();//End
        }
        private void CreateLowFareFinderXml(StringBuilder stb,
                                            string strXML,
                                            string strCurrencyRcd,
                                            DayOfWeek BeginingDayOfWeeks,
                                            string originRcd,
                                            string destinationRcd,
                                            string strOriginName,
                                            string strDestinationName,
                                            DateTime departureDate,
                                            DateTime returnDate,
                                            Int16 iAdult,
                                            Int16 iChild,
                                            Int16 iInfant,
                                            Int16 iOther,
                                            string strOtherType,
                                            short iSearchRange,
                                            bool bCalendarMode,
                                            bool bOneWay,
                                            string selectDate)
        {
            if (stb != null)
            {
                XPathDocument xmlDoc;
                using (StringWriter stw = new StringWriter(stb))
                {
                    XmlTextWriter objXmlWriter = new XmlTextWriter(stw);

                    objXmlWriter.WriteStartElement("availability_lowfare");
                    {
                        using (StringReader srd = new StringReader(strXML))
                        {
                            using (XmlReader reader = XmlReader.Create(srd))
                            {
                                while (!reader.EOF)
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (reader.Name)
                                        {
                                            case "AvailabilityOutbound":
                                                if (bCalendarMode == true)
                                                {
                                                    if (reader.IsEmptyElement == false)
                                                    {
                                                        xmlDoc = new XPathDocument(new StringReader(reader.ReadOuterXml()));
                                                        GroupLowestFareFinderCalMode(ref objXmlWriter,
                                                                                    xmlDoc,
                                                                                    strCurrencyRcd,
                                                                                    true,
                                                                                    BeginingDayOfWeeks,
                                                                                    originRcd,
                                                                                    destinationRcd,
                                                                                    strOriginName,
                                                                                    strDestinationName,
                                                                                    iAdult,
                                                                                    iChild,
                                                                                    iInfant,
                                                                                    iOther,
                                                                                    strOtherType,
                                                                                    iSearchRange,
                                                                                    departureDate);
                                                    }
                                                    else
                                                    {
                                                        //Generate empty field.
                                                        GroupLowestFareFinderEmptyCalMode(ref objXmlWriter,
                                                                                        strCurrencyRcd,
                                                                                        true,
                                                                                        BeginingDayOfWeeks,
                                                                                        originRcd,
                                                                                        destinationRcd,
                                                                                        strOriginName,
                                                                                        strDestinationName,
                                                                                        departureDate,
                                                                                        iAdult,
                                                                                        iChild,
                                                                                        iInfant,
                                                                                        iOther,
                                                                                        strOtherType,
                                                                                        iSearchRange);
                                                        reader.Read();
                                                    }

                                                }
                                                else
                                                {
                                                    if (reader.IsEmptyElement == false)
                                                    {
                                                        xmlDoc = new XPathDocument(new StringReader(reader.ReadOuterXml()));
                                                        GroupLowestFareFinderMode(ref objXmlWriter,
                                                                                    xmlDoc,
                                                                                    strCurrencyRcd,
                                                                                    true,
                                                                                    BeginingDayOfWeeks,
                                                                                    originRcd,
                                                                                    destinationRcd,
                                                                                    strOriginName,
                                                                                    strDestinationName,
                                                                                    iAdult,
                                                                                    iChild,
                                                                                    iInfant,
                                                                                    iOther,
                                                                                    strOtherType,
                                                                                    iSearchRange,
                                                                                    departureDate);
                                                    }
                                                    else
                                                    {
                                                        reader.Read();
                                                    }
                                                }

                                                xmlDoc = null;
                                                break;
                                            case "AvailabilityReturn":
                                                if (bOneWay == false)
                                                {
                                                    if (bCalendarMode == true)
                                                    {
                                                        if (reader.IsEmptyElement == false)
                                                        {
                                                            xmlDoc = new XPathDocument(new StringReader(reader.ReadOuterXml()));
                                                            GroupLowestFareFinderCalMode(ref objXmlWriter,
                                                                                        xmlDoc,
                                                                                        strCurrencyRcd,
                                                                                        false,
                                                                                        BeginingDayOfWeeks,
                                                                                        originRcd,
                                                                                        destinationRcd,
                                                                                        strOriginName,
                                                                                        strDestinationName,
                                                                                        iAdult,
                                                                                        iChild,
                                                                                        iInfant,
                                                                                        iOther,
                                                                                        strOtherType,
                                                                                        iSearchRange,
                                                                                        returnDate);
                                                        }
                                                        else
                                                        {
                                                            //Generate empty field.
                                                            GroupLowestFareFinderEmptyCalMode(ref objXmlWriter,
                                                                                            strCurrencyRcd,
                                                                                            false,
                                                                                            BeginingDayOfWeeks,
                                                                                            originRcd,
                                                                                            destinationRcd,
                                                                                            strOriginName,
                                                                                            strDestinationName,
                                                                                            returnDate,
                                                                                            iAdult,
                                                                                            iChild,
                                                                                            iInfant,
                                                                                            iOther,
                                                                                            strOtherType,
                                                                                            iSearchRange);
                                                            reader.Read();
                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (reader.IsEmptyElement == false)
                                                        {
                                                            xmlDoc = new XPathDocument(new StringReader(reader.ReadOuterXml()));
                                                            GroupLowestFareFinderMode(ref objXmlWriter,
                                                                                xmlDoc,
                                                                                strCurrencyRcd,
                                                                                false,
                                                                                BeginingDayOfWeeks,
                                                                                originRcd,
                                                                                destinationRcd,
                                                                                strOriginName,
                                                                                strDestinationName,
                                                                                iAdult,
                                                                                iChild,
                                                                                iInfant,
                                                                                iOther,
                                                                                strOtherType,
                                                                                iSearchRange,
                                                                                returnDate);
                                                        }
                                                        else
                                                        {
                                                            reader.Read();
                                                        }

                                                    }

                                                    xmlDoc = null;
                                                }
                                                else
                                                {
                                                    reader.Read();
                                                }
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        reader.Read();
                                    }
                                }
                            }
                        }
                    }
                    objXmlWriter.WriteEndElement();//End availability_lowfare
                    objXmlWriter.Flush();
                    objXmlWriter.Close();
                }
            }
        }
        private void CreateLFFFlightField(XPathNavigator n, XmlTextWriter objXmlWriter, string SearchCurrencyRcd, DateTime dt, bool bFirstFlightOfDay)
        {
            byte iClose = XmlHelper.XpathValueNullToByte(n, "close_web_sales");
            byte iFull = XmlHelper.XpathValueNullToByte(n, "full_flight_flag");
            byte iOpen = XmlHelper.XpathValueNullToByte(n, "class_open_flag");

            objXmlWriter.WriteStartElement("flight");
            {
                objXmlWriter.WriteStartElement("first_flight_of_day");
                objXmlWriter.WriteValue(Convert.ToByte(bFirstFlightOfDay));
                objXmlWriter.WriteEndElement();//first_flight_of_day

                objXmlWriter.WriteStartElement("origin_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "origin_rcd"));
                objXmlWriter.WriteEndElement();//End origin_rcd

                objXmlWriter.WriteStartElement("destination_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "destination_rcd"));
                objXmlWriter.WriteEndElement();//End destination_rcd

                objXmlWriter.WriteStartElement("origin_name");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "origin_name"));
                objXmlWriter.WriteEndElement();//End origin_name

                objXmlWriter.WriteStartElement("destination_name");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "destination_name"));
                objXmlWriter.WriteEndElement();//End destination_name

                objXmlWriter.WriteStartElement("departure_date");
                objXmlWriter.WriteValue(dt);
                objXmlWriter.WriteEndElement();//departure_date

                objXmlWriter.WriteStartElement("arrival_date");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToDateTime(n, "arrival_date"));
                objXmlWriter.WriteEndElement();//arrival_date

                objXmlWriter.WriteStartElement("day_of_week");
                objXmlWriter.WriteValue(dt.DayOfWeek.ToString());
                objXmlWriter.WriteEndElement();//day_of_week

                objXmlWriter.WriteStartElement("adult_fare");
                objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "adult_fare")));
                objXmlWriter.WriteEndElement();//adult_fare

                objXmlWriter.WriteStartElement("child_fare");
                objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "child_fare")));
                objXmlWriter.WriteEndElement();//child_fare

                objXmlWriter.WriteStartElement("infant_fare");
                objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "infant_fare")));
                objXmlWriter.WriteEndElement();//infant_fare

                objXmlWriter.WriteStartElement("other_fare");
                objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "other_fare")));
                objXmlWriter.WriteEndElement();//other_fare

                objXmlWriter.WriteStartElement("total_adult_fare");
                if (iOpen == 1 && iClose == 0 && iFull == 0)
                {
                    objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "total_adult_fare")));
                }
                else
                {
                    objXmlWriter.WriteValue("N/A");
                }
                objXmlWriter.WriteEndElement();//total_adult_fare

                objXmlWriter.WriteStartElement("total_child_fare");
                if (iOpen == 1 && iClose == 0 && iFull == 0)
                {
                    objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "total_child_fare")));
                }
                else
                {
                    objXmlWriter.WriteValue("N/A");
                }
                objXmlWriter.WriteEndElement();//total_child_fare

                objXmlWriter.WriteStartElement("total_infant_fare");
                if (iOpen == 1 && iClose == 0 && iFull == 0)
                {
                    objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "total_infant_fare")));
                }
                else
                {
                    objXmlWriter.WriteValue("N/A");
                }
                objXmlWriter.WriteEndElement();//total_infant_fare

                objXmlWriter.WriteStartElement("total_other_fare");
                if (iOpen == 1 && iClose == 0 && iFull == 0)
                {
                    objXmlWriter.WriteValue(string.Format("{0:0.00}", XmlHelper.XpathValueNullToZero(n, "total_other_fare")));
                }
                else
                {
                    objXmlWriter.WriteValue("N/A");
                }
                objXmlWriter.WriteEndElement();//total_other_fare

                objXmlWriter.WriteStartElement("booking_class_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "booking_class_rcd"));
                objXmlWriter.WriteEndElement();//booking_class_rcd

                objXmlWriter.WriteStartElement("boarding_class_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd"));
                objXmlWriter.WriteEndElement();//boarding_class_rcd

                objXmlWriter.WriteStartElement("currency_rcd");
                objXmlWriter.WriteValue(SearchCurrencyRcd);
                objXmlWriter.WriteEndElement();//currency_rcd

                objXmlWriter.WriteStartElement("planned_departure_time");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToInt16(n, "planned_departure_time"));
                objXmlWriter.WriteEndElement();//planned_departure_time

                objXmlWriter.WriteStartElement("planned_arrival_time");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToInt16(n, "planned_arrival_time"));
                objXmlWriter.WriteEndElement();//planned_arrival_time

                objXmlWriter.WriteStartElement("fare_id");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "fare_id"));
                objXmlWriter.WriteEndElement();//fare_id

                objXmlWriter.WriteStartElement("flight_id");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_id"));
                objXmlWriter.WriteEndElement();//flight_id

                objXmlWriter.WriteStartElement("airline_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "airline_rcd"));
                objXmlWriter.WriteEndElement();//airline_rcd

                objXmlWriter.WriteStartElement("airline_name");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "airline_name"));
                objXmlWriter.WriteEndElement();//airline_name

                objXmlWriter.WriteStartElement("flight_number");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "flight_number"));
                objXmlWriter.WriteEndElement();//flight_number

                objXmlWriter.WriteStartElement("full_flight_flag");
                objXmlWriter.WriteValue(iFull);
                objXmlWriter.WriteEndElement();//full_flight_flag

                objXmlWriter.WriteStartElement("class_open_flag");
                objXmlWriter.WriteValue(iOpen);
                objXmlWriter.WriteEndElement();//class_open_flag

                objXmlWriter.WriteStartElement("close_web_sales");
                objXmlWriter.WriteValue(iClose);
                objXmlWriter.WriteEndElement();//close_web_sales

                objXmlWriter.WriteStartElement("waitlist_open_flag");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "waitlist_open_flag"));
                objXmlWriter.WriteEndElement();//waitlist_open_flag

                objXmlWriter.WriteStartElement("transit_departure_date");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_departure_date"));
                objXmlWriter.WriteEndElement();//transit_departure_date

                objXmlWriter.WriteStartElement("transit_airport_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_airport_rcd"));
                objXmlWriter.WriteEndElement();//transit_airport_rcd

                objXmlWriter.WriteStartElement("transit_fare_id");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_fare_id"));
                objXmlWriter.WriteEndElement();//transit_fare_id

                objXmlWriter.WriteStartElement("transit_flight_id");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_id"));
                objXmlWriter.WriteEndElement();//transit_flight_id

                objXmlWriter.WriteStartElement("transit_arrival_date");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_arrival_date"));
                objXmlWriter.WriteEndElement();//transit_arrival_date

                objXmlWriter.WriteStartElement("transit_planned_departure_time");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_planned_departure_time"));
                objXmlWriter.WriteEndElement();//transit_planned_departure_time

                objXmlWriter.WriteStartElement("transit_planned_arrival_time");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_planned_arrival_time"));
                objXmlWriter.WriteEndElement();//transit_planned_arrival_time

                objXmlWriter.WriteStartElement("transit_airline_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_airline_rcd"));
                objXmlWriter.WriteEndElement();//transit_airline_rcd

                objXmlWriter.WriteStartElement("transit_flight_number");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_flight_number"));
                objXmlWriter.WriteEndElement();//transit_flight_number

                objXmlWriter.WriteStartElement("transit_name");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_name"));
                objXmlWriter.WriteEndElement();//transit_name

                objXmlWriter.WriteStartElement("transit_points");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_points"));
                objXmlWriter.WriteEndElement();//transit_point

                objXmlWriter.WriteStartElement("transit_points_name");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_points_name"));
                objXmlWriter.WriteEndElement();//transit_point_name

                objXmlWriter.WriteStartElement("transit_boarding_class_rcd");
                objXmlWriter.WriteValue(XmlHelper.XpathValueNullToEmpty(n, "transit_boarding_class_rcd"));
                objXmlWriter.WriteEndElement();//transit_boarding_class_rcd
            }
            objXmlWriter.WriteEndElement();//End flight
        }
        private void CreateLFFSettingField(XmlTextWriter objXmlWriter,
                                            bool Outward,
                                            string originRcd,
                                            string destinationRcd,
                                            string strOriginName,
                                            string strDestinationName,
                                            Int16 iAdult,
                                            Int16 iChild,
                                            Int16 iInfant,
                                            Int16 iOther,
                                            string strOtherType,
                                            Int16 iSearchRange,
                                            DateTime dtSelected)
        {
            objXmlWriter.WriteStartElement("setting");
            {
                objXmlWriter.WriteStartElement("flight_type");
                {
                    if (Outward == true)
                    { objXmlWriter.WriteValue("Outward"); }
                    else
                    { objXmlWriter.WriteValue("Return"); }
                }
                objXmlWriter.WriteEndElement();//End flight_type


                objXmlWriter.WriteStartElement("origin_rcd");
                {
                    if (Outward == true)
                    { objXmlWriter.WriteValue(originRcd); }
                    else
                    { objXmlWriter.WriteValue(destinationRcd); }

                }
                objXmlWriter.WriteEndElement();//End origin_rcd
                objXmlWriter.WriteStartElement("destination_rcd");
                {
                    if (Outward == true)
                    { objXmlWriter.WriteValue(destinationRcd); }
                    else
                    { objXmlWriter.WriteValue(originRcd); }
                }
                objXmlWriter.WriteEndElement();//End destination_rcd


                objXmlWriter.WriteStartElement("OriginName");
                {
                    if (Outward == true)
                    { objXmlWriter.WriteValue(strOriginName); }
                    else
                    { objXmlWriter.WriteValue(strDestinationName); }

                }
                objXmlWriter.WriteEndElement();//End OriginName
                objXmlWriter.WriteStartElement("DestinationName");
                {
                    if (Outward == true)
                    { objXmlWriter.WriteValue(strDestinationName); }
                    else
                    { objXmlWriter.WriteValue(strOriginName); }
                }
                objXmlWriter.WriteEndElement();//End DestinationName

                objXmlWriter.WriteStartElement("number_of_adult");
                {
                    objXmlWriter.WriteValue(iAdult);
                }
                objXmlWriter.WriteEndElement();//End number_of_adult
                objXmlWriter.WriteStartElement("number_of_child");
                {
                    objXmlWriter.WriteValue(iChild);
                }
                objXmlWriter.WriteEndElement();//End number_of_child
                objXmlWriter.WriteStartElement("number_of_infant");
                {
                    objXmlWriter.WriteValue(iInfant);
                }
                objXmlWriter.WriteEndElement();//End number_of_infant
                objXmlWriter.WriteStartElement("number_of_other");
                {
                    objXmlWriter.WriteValue(iOther);
                }
                objXmlWriter.WriteEndElement();//End number_of_other
                objXmlWriter.WriteStartElement("other_passenger_type");
                {
                    objXmlWriter.WriteValue(strOtherType);
                }
                objXmlWriter.WriteEndElement();//End other_passenger_type
                objXmlWriter.WriteStartElement("LowFareFinderRange");
                {
                    objXmlWriter.WriteValue(iSearchRange);
                }
                objXmlWriter.WriteEndElement();//End LowFareFinderRange

                objXmlWriter.WriteStartElement("departure_date");
                {
                    objXmlWriter.WriteValue(dtSelected);
                }
                objXmlWriter.WriteEndElement();//End departure_date
            }
            objXmlWriter.WriteEndElement();//End setting
        }
        private void CreateLFFEmptyField(XmlTextWriter objXmlWriter, string SearchCurrencyRcd, DateTime dt, bool bFirstFlightOfDay)
        {
            objXmlWriter.WriteStartElement("flight");
            {
                objXmlWriter.WriteStartElement("first_flight_of_day");
                objXmlWriter.WriteValue(Convert.ToByte(bFirstFlightOfDay));
                objXmlWriter.WriteEndElement();//first_flight_of_day

                objXmlWriter.WriteStartElement("departure_date");
                objXmlWriter.WriteValue(dt);
                objXmlWriter.WriteEndElement();//departure_date

                objXmlWriter.WriteStartElement("day_of_week");
                objXmlWriter.WriteValue(dt.DayOfWeek.ToString());
                objXmlWriter.WriteEndElement();//day_of_week

                objXmlWriter.WriteStartElement("full_flight_flag");
                objXmlWriter.WriteValue(1);
                objXmlWriter.WriteEndElement();//full_flight_flag

                objXmlWriter.WriteStartElement("class_open_flag");
                objXmlWriter.WriteValue(0);
                objXmlWriter.WriteEndElement();//class_open_flag

                objXmlWriter.WriteStartElement("close_web_sales");
                objXmlWriter.WriteValue(1);
                objXmlWriter.WriteEndElement();//close_web_sales

                objXmlWriter.WriteStartElement("total_adult_fare");
                objXmlWriter.WriteValue("N/A");
                objXmlWriter.WriteEndElement();//total_adult_fare
            }
            objXmlWriter.WriteEndElement();//End flight
        }
        private void GroupLowestFareFinderCalMode(ref XmlTextWriter objXmlWriter,
                                                    XPathDocument xmlDoc,
                                                    string SearchCurrencyRcd,
                                                    bool Outward,
                                                    DayOfWeek BeginingDayOfWeeks,
                                                    string originRcd,
                                                    string destinationRcd,
                                                    string strOriginName,
                                                    string strDestinationName,
                                                    Int16 iAdult,
                                                    Int16 iChild,
                                                    Int16 iInfant,
                                                    Int16 iOther,
                                                    string strOtherType,
                                                    short iSearchRange,
                                                    DateTime dtSelectedDate)
        {
            Library objLi = new Library();
            XPathExpression exp;
            XPathNavigator nv = xmlDoc.CreateNavigator();

            DateTime dt = DateTime.MinValue;
            DateTime dtCurrent = DateTime.MinValue;
            DateTime DayEndOfWeek = DateTime.MinValue;

            bool bBeginingOfWeek = true;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US", true);
            ci.DateTimeFormat.FirstDayOfWeek = BeginingDayOfWeeks;

            if (Outward == true)
            {
                objXmlWriter.WriteStartElement("flight_departure");
                exp = nv.Compile("AvailabilityOutbound/AvailabilityFlight");
            }
            else
            {
                objXmlWriter.WriteStartElement("flight_return");
                exp = nv.Compile("AvailabilityReturn/AvailabilityFlight");
            }
            {
                exp.AddSort("departure_date", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);
                exp.AddSort("total_adult_fare", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Number);

                //Setting Section.
                CreateLFFSettingField(objXmlWriter, Outward, originRcd, destinationRcd, strOriginName, strDestinationName, iAdult, iChild, iInfant, iOther, strOtherType, iSearchRange, dtSelectedDate);
                foreach (XPathNavigator n in nv.Select(exp))
                {
                    dtCurrent = XmlHelper.XpathValueNullToDateTime(n, "departure_date");
                    if (iSearchRange == -1)
                    {
                        //Check for the Empty date and add the N/A value in.
                        if (dt.Equals(DateTime.MinValue) && dtCurrent.Day != 1)
                        {
                            DateTime dtDayCount = new DateTime(dtCurrent.Year, dtCurrent.Month, 1);
                            while (dtDayCount < dtCurrent)
                            {
                                CreateCalendarDate(ref objXmlWriter, ref bBeginingOfWeek, ref DayEndOfWeek, dt, dtDayCount, ci, null, objLi, SearchCurrencyRcd);
                                dtDayCount = dtDayCount.AddDays(1);
                            }
                        }
                        else if (dt.Equals(DateTime.MinValue) == false && dt.AddDays(1).Equals(dtCurrent) == false && dt != dtCurrent)
                        {
                            //Set between date
                            DateTime dtDayCount = dt.AddDays(1);
                            while (dtDayCount < dtCurrent)
                            {
                                CreateCalendarDate(ref objXmlWriter, ref bBeginingOfWeek, ref DayEndOfWeek, dt, dtDayCount, ci, null, objLi, SearchCurrencyRcd);
                                dtDayCount = dtDayCount.AddDays(1);
                            }
                        }
                    }

                    //Process normal available flight date.
                    CreateCalendarDate(ref objXmlWriter, ref bBeginingOfWeek, ref DayEndOfWeek, dt, dtCurrent, ci, n, objLi, SearchCurrencyRcd);
                    dt = dtCurrent;
                }

                if (iSearchRange == -1)
                {
                    //Set Day until end of the month.
                    int endDayOfMonth = DateTime.DaysInMonth(dtCurrent.Year, dtCurrent.Month);
                    if (endDayOfMonth != dtCurrent.Day & dtCurrent.Equals(DateTime.MinValue) == false)
                    {
                        for (int i = dtCurrent.Day; i < endDayOfMonth; i++)
                        {
                            dtCurrent = dtCurrent.AddDays(1);
                            CreateCalendarDate(ref objXmlWriter, ref bBeginingOfWeek, ref DayEndOfWeek, dt, dtCurrent, ci, null, objLi, SearchCurrencyRcd);
                        }
                    }
                }
                if (bBeginingOfWeek == false)
                {
                    //Called when it the end of the loop but it not the end of week day yet.
                    objXmlWriter.WriteEndElement();//End Week
                }
            }
            objXmlWriter.WriteEndElement();//End
        }
        private void GroupLowestFareFinderEmptyCalMode(ref XmlTextWriter objXmlWriter,
                                                       string SearchCurrencyRcd,
                                                       bool Outward,
                                                       DayOfWeek BeginingDayOfWeeks,
                                                       string originRcd,
                                                       string destinationRcd,
                                                       string strOriginName,
                                                       string strDestinationName,
                                                       DateTime flightDate,
                                                       Int16 iAdult,
                                                       Int16 iChild,
                                                       Int16 iInfant,
                                                       Int16 iOther,
                                                       string strOtherType,
                                                       short iSearchRange)
        {
            Library objLi = new Library();

            DateTime dt = DateTime.MinValue;
            DateTime dtCurrent = DateTime.MinValue;
            DateTime DayEndOfWeek = DateTime.MinValue;

            bool bBeginingOfWeek = true;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US", true);
            ci.DateTimeFormat.FirstDayOfWeek = BeginingDayOfWeeks;

            if (Outward == true)
            {
                objXmlWriter.WriteStartElement("flight_departure");
            }
            else
            {
                objXmlWriter.WriteStartElement("flight_return");
            }
            {
                //Setting Section.
                CreateLFFSettingField(objXmlWriter, Outward, originRcd, destinationRcd, strOriginName, strDestinationName, iAdult, iChild, iInfant, iOther, strOtherType, iSearchRange, DateTime.MinValue);
                //Generate empty month calendar
                DateTime dtDayCount = new DateTime(flightDate.Year, flightDate.Month, 1);
                dtCurrent = new DateTime(dtDayCount.Year, dtDayCount.Month, DateTime.DaysInMonth(dtDayCount.Year, dtDayCount.Month));
                while (dtDayCount <= dtCurrent)
                {
                    CreateCalendarDate(ref objXmlWriter, ref bBeginingOfWeek, ref DayEndOfWeek, dt, dtDayCount, ci, null, objLi, SearchCurrencyRcd);
                    dtDayCount = dtDayCount.AddDays(1);
                }
                int endDayOfMonth = DateTime.DaysInMonth(dtCurrent.Year, dtCurrent.Month);
                if (endDayOfMonth == dtCurrent.Day)
                {
                    objXmlWriter.WriteEndElement();//close xml when it goto end of month.
                }
            }
            objXmlWriter.WriteEndElement();//End
        }
        private void CreateCalendarDate(ref XmlTextWriter objXmlWriter,
                                        ref bool bBeginingOfWeek,
                                        ref DateTime DayEndOfWeek,
                                        DateTime dt,
                                        DateTime dtCurrent,
                                        System.Globalization.CultureInfo ci,
                                        XPathNavigator n,
                                        Library objLi,
                                        string currencyRcd)
        {
            if (dtCurrent > DayEndOfWeek && DayEndOfWeek.Equals(DateTime.MinValue) == false)
            {
                //Create end week tag
                objXmlWriter.WriteEndElement();//End Week
                bBeginingOfWeek = true;
            }

            if (bBeginingOfWeek == true)
            {
                //Create start week tag
                objXmlWriter.WriteStartElement("week");
                bBeginingOfWeek = false;

                DayEndOfWeek = objLi.GetEndDateOfWeek(ci, dtCurrent);
            }

            if (dt != dtCurrent)
            {
                //Create flight xml
                if (n != null)
                {
                    CreateLFFFlightField(n, objXmlWriter, currencyRcd, dtCurrent, true);
                }
                else
                {
                    CreateLFFEmptyField(objXmlWriter, currencyRcd, dtCurrent, true);
                }

            }
            else
            {
                if (n != null)
                {
                    CreateLFFFlightField(n, objXmlWriter, currencyRcd, dtCurrent, false);
                }
                else
                {
                    CreateLFFEmptyField(objXmlWriter, currencyRcd, dtCurrent, false);
                }
            }
        }
        private string GetFlightAvailability(string Origin,
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
            if ((DataHelper.DateDifferent(DateDepartFrom, DateDepartTo).Days > 31) ||
                    (DataHelper.DateDifferent(DateReturnFrom, DateReturnTo).Days > 31))
            {
                return string.Empty;
            }
            else
            {
                //Search Avalability
                ServiceClient srvClient = new ServiceClient();
                return srvClient.GetSessionlessFlightAvailability(Origin,
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
                                                                _token,
                                                                bReturnRefundable,
                                                                bNoVat,
                                                                iDayRange);
            }
        }

        private void GetAvailableTab(XmlTextWriter objXmlWriter,
                                        string originRcd,
                                        string destinationRcd,
                                        string airlineCode,
                                        string flightNumber,
                                        DateTime flightFrom,
                                        DateTime flightTo,
                                        string languageCode)
        {
            try
            {
                ServiceClient obj = new ServiceClient();

                string strXML = obj.GetFlightDailyCountXML(flightFrom, flightTo, originRcd, destinationRcd, _token);

                if (string.IsNullOrEmpty(strXML) == false)
                {
                    //Generate availability tab base on availabile flight Schedule.
                    using (StringReader sr = new StringReader(strXML))
                    {
                        XPathDocument xmlDoc = new XPathDocument(sr);
                        XPathNavigator nv = xmlDoc.CreateNavigator();
                        XPathExpression sorter = nv.Compile("FlightDailyCounts/FlightDailyCount");

                        sorter.AddSort("date_from", XmlSortOrder.Ascending, XmlCaseOrder.None, null, XmlDataType.Text);
                        foreach (XPathNavigator n in nv.Select(sorter))
                        {
                            objXmlWriter.WriteStartElement("Tab");
                            {
                                objXmlWriter.WriteStartElement("number_of_flight");
                                {
                                    objXmlWriter.WriteValue(XmlHelper.XpathValueNullToInt(n, "flight_count"));
                                }
                                objXmlWriter.WriteEndElement();
                                objXmlWriter.WriteStartElement("departure_date");
                                {
                                    objXmlWriter.WriteValue(XmlHelper.XpathValueNullToDateTime(n, "date_from"));
                                }
                                objXmlWriter.WriteEndElement();
                            }
                            objXmlWriter.WriteEndElement();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetNumberOfFlight(XPathNavigator nv, string strDepartureDate)
        {
            return nv.Select("Flights/Details[utc_departure_date = '" + strDepartureDate + "']").Count;
        }
        #endregion
    }
}
