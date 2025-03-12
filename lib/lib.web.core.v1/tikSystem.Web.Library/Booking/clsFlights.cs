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
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using tikSystem.Web.Library;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Flights : Routes
    {
        //Token of Old webservice
        string _token = string.Empty;

        [XmlIgnore]
        public new Flight this[int index]
        {
            get
            {
                return (Flight)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }
        public int Add(Flight Value)
        {
            return (List.Add(Value));
        }
        public Flights()
        { }
        public Flights(string token)
        {
            _token = token;
        }
        #region Method
        public string FlightSummary(Passengers passengers, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            ServiceClient service = new ServiceClient();
            return service.GetFlightSummary(passengers, this, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
        }

        #endregion
        public string AddFlight(string agencyCode, 
                                string currency, 
                                string bookingID, 
                                short adults, 
                                short children, 
                                short infants, 
                                short others, 
                                string strOthers,
                                string userId, 
                                string strIpAddress, 
                                string strLanguageCode,
                                bool bNoVat)
        {
            ServiceClient obj = new ServiceClient();

            string result = string.Empty;
            obj.objService = objService;
           
            try
            {
                if (this.Count > 0)
                {
                    result = XmlHelper.Serialize(this, true);
                    obj.objService = objService;
                    result = obj.FlightAdd(agencyCode, 
                                            currency, 
                                            result, 
                                            bookingID, 
                                            adults, 
                                            children, 
                                            infants, 
                                            others, 
                                            strOthers, 
                                            userId, 
                                            strIpAddress, 
                                            strLanguageCode,
                                            bNoVat);                
                }
                else
                {
                    return "{202}"; //No selected flight information.
                }
                
            }
            catch (Exception e)
            { 
                throw new Exception(e.Message + "\n" + e.StackTrace); 
            }

            return result;
        }

        public string GetFlightTimeLowestFare(string strAgencyCode,
                                              string strBoardingClass, 
                                              string strFlightDate,
                                              string strOriginRcd,
                                              string strDestinationRcd,
                                              string strCurrenyRcd,
                                              string strPromoCode,
                                              string strSearchType,
                                              Int16 iDayRange,
                                              Int16 iAdult,
                                              Int16 iChild,
                                              Int16 iInfant,
                                              Int16 iOther,
                                              Int16 iFareLogic,
                                              string strOtherType,
                                              string strToken)
        {
            ServiceClient obj = new ServiceClient();
            StringBuilder stb = new StringBuilder();
            string result = null;

            try
            {
                string BoardingClass = strBoardingClass;
                string BookingClass = string.Empty;

                bool bRefundable = false;
                bool bGroupFare = false;
                bool bItFareOnly = false;
                bool bStaffFare = false;
                DateTime dtDepartFrom = DateTime.MinValue;
                DateTime dtDepartTo = DateTime.MinValue;
                DateTime dtReturnFrom = DateTime.MinValue;
                DateTime dtReturnTo = DateTime.MinValue;

                if (strFlightDate.Length == 8)
                {
                    dtDepartFrom = new DateTime(Convert.ToInt16(strFlightDate.Substring(0, 4)), Convert.ToInt16(strFlightDate.Substring(4, 2)), Convert.ToInt16(strFlightDate.Substring(6, 2)));
                    dtDepartTo = dtDepartFrom;

                    //Add number of days serarch
                    dtDepartFrom = dtDepartFrom.AddDays(-iDayRange);
                    dtDepartTo = dtDepartTo.AddDays(iDayRange);
                }

                result = obj.GetSessionlessCompactFlightAvailability(strOriginRcd,
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
                                                                    strOtherType,
                                                                    BoardingClass,
                                                                    BookingClass,
                                                                    string.Empty,
                                                                    strAgencyCode,
                                                                    strCurrenyRcd,
                                                                    string.Empty,
                                                                    string.Empty,
                                                                    0.0,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    true,
                                                                    bRefundable,
                                                                    bGroupFare,
                                                                    bItFareOnly,
                                                                    bStaffFare,
                                                                    false,
                                                                    false,
                                                                    string.Empty,
                                                                    DateTime.MinValue,
                                                                    DateTime.MinValue,
                                                                    string.Empty,
                                                                    true,
                                                                    false,
                                                                    string.Empty,
                                                                    strPromoCode,
                                                                    iFareLogic,
                                                                    strSearchType,
                                                                    strToken);

                //Reconstruct Xml
                StringBuilder tempFlightIDTransitFlightID = new StringBuilder();
                string strFlight_id = string.Empty;
                string strTransitFlight_id = string.Empty;
                string strDate = string.Empty;
                string strTempAvailability = string.Empty;
                decimal dclTotalAdultFare = 0;
                int iCount = 0;

                XPathDocument xmlDoc = new XPathDocument(new StringReader(result));
                XPathNavigator nv = xmlDoc.CreateNavigator();
                XPathExpression exp;
                stb.Append("<Booking>");
                foreach (XPathNavigator n in nv.Select("Booking/AvailabilityFlight"))
                {
                    strFlight_id = n.SelectSingleNode("flight_id").InnerXml;
                    strTransitFlight_id = n.SelectSingleNode("transit_flight_id").InnerXml;
                    strDate = n.SelectSingleNode("departure_date").InnerXml;

                    if (tempFlightIDTransitFlightID.ToString().Contains(strFlight_id + "|" + strTransitFlight_id + "|" + strDate) == false)
                    {
                        tempFlightIDTransitFlightID.Append(strFlight_id);
                        tempFlightIDTransitFlightID.Append("|");
                        tempFlightIDTransitFlightID.Append(strTransitFlight_id);
                        tempFlightIDTransitFlightID.Append("|");
                        tempFlightIDTransitFlightID.Append(strDate);
                        tempFlightIDTransitFlightID.Append("{}");

                        exp = nv.Compile("Booking/AvailabilityFlight[flight_id = '" + strFlight_id + "'][transit_flight_id = '" + strTransitFlight_id + "'][departure_date = '" + strDate + "']");
                        exp.AddSort("departure_date", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);
                        exp.AddSort("total_adult_fare", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Number);
                        dclTotalAdultFare = 0;
                        strTempAvailability = string.Empty;
                        iCount = nv.Select(exp).Count;
                        foreach (XPathNavigator x in nv.Select(exp))
                        {
                            iCount = --iCount;
                            if(Convert.ToInt16(x.SelectSingleNode("full_flight_flag").InnerXml) == 0 && Convert.ToInt16(x.SelectSingleNode("class_open_flag").InnerXml) == 1 && Convert.ToInt16(x.SelectSingleNode("close_web_sales").InnerXml) == 0)
                            {
                                if ((Convert.ToDecimal("0" + x.SelectSingleNode("total_adult_fare").InnerXml) < dclTotalAdultFare) ||
                                (dclTotalAdultFare == 0))
                                {
                                    dclTotalAdultFare = Convert.ToDecimal("0" + x.SelectSingleNode("total_adult_fare").InnerXml);
                                    strTempAvailability = x.OuterXml;
                                }
                            }
                            else if (iCount == 0 && strTempAvailability.Length == 0)
                            {
                                dclTotalAdultFare = Convert.ToDecimal("0" + x.SelectSingleNode("total_adult_fare").InnerXml);
                                strTempAvailability = x.OuterXml;
                            }
                        }

                        if (strTempAvailability.Length > 0)
                        { stb.Append(strTempAvailability); }
                    }
                }
                stb.Append("</Booking>");
            }
            catch (Exception e)
            { throw new Exception(e.Message + "\n" + e.StackTrace); }

            return stb.ToString();
        }
        public string GetBookingSegmentCheckIn(string strBookingId, string strClientId, string strLanguageCode)
        {
            ServiceClient obj = new ServiceClient();

            obj.objService = objService;
            return obj.GetBookingSegmentCheckIn(strBookingId, strClientId, strLanguageCode);
        }
        public string GetQuoteSummary(Passengers passengers, string strAgencyCode, string strLanguage, string strToken, string strCurrencyCode, bool bNoVat)
        {

            ServiceClient obj = new ServiceClient();
            Booking objBooking = new Booking();
            if (strToken.Length == 0)
            {
                obj.objService = objService;
                return obj.SingleFlightQuoteSummary(this, passengers, strAgencyCode, strLanguage, strCurrencyCode, bNoVat);
            }
            else
            {
                return obj.SessionlessSingleFlightQuoteSummary(this, passengers, strAgencyCode, strToken, strLanguage, strCurrencyCode, bNoVat);
            }

        }
        #region Helper
        private void GetXmlGroup(ref XmlTextWriter objXmlWriter, XPathNavigator x, int iposition, string FlightId,string TransitFlightID)
        {
            Library li = new Library();

            Int16 fullFlightFlag = 0;
            Int16 classOpenFlag = 0;
            Int16 closeWebSales = 0;
            string transitClassOpenFlag = "";
       
            objXmlWriter.WriteStartElement("group");
            {
                
                   if (x != null) {
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
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("flight_id").InnerXml); }
                    objXmlWriter.WriteEndElement();//End flight_id

                    objXmlWriter.WriteStartElement("transit_flight_id");
                    if (x == null) { objXmlWriter.WriteValue(TransitFlightID);}
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_flight_id").InnerXml); }
                    objXmlWriter.WriteEndElement();//End transit_flight_id

                    objXmlWriter.WriteStartElement("fare_id");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("fare_id").InnerXml); }
                    objXmlWriter.WriteEndElement();//End fare_id

                    objXmlWriter.WriteStartElement("transit_fare_id");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_fare_id").InnerXml); }
                    objXmlWriter.WriteEndElement();//End transit_fare_id

                    objXmlWriter.WriteStartElement("fare_column");
                    if (x == null) { objXmlWriter.WriteValue(0); }
                    else { objXmlWriter.WriteValue(iposition); }
                    objXmlWriter.WriteEndElement();//End fare_column

                    objXmlWriter.WriteStartElement("fare_code");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("fare_code").InnerXml); }
                    objXmlWriter.WriteEndElement();//End fare_code

                    objXmlWriter.WriteStartElement("adult_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("adult_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End adult_fare

                    objXmlWriter.WriteStartElement("child_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("child_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End child_fare

                    objXmlWriter.WriteStartElement("infant_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("infant_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End infant_fare

                    objXmlWriter.WriteStartElement("other_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("other_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End infant_fare

                    objXmlWriter.WriteStartElement("total_adult_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("total_adult_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End total_adult_fare

                    objXmlWriter.WriteStartElement("total_child_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("total_child_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End total_child_fare

                    objXmlWriter.WriteStartElement("total_infant_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("total_infant_fare").InnerXml); }
                    objXmlWriter.WriteEndElement();//End total_infant_fare

                    objXmlWriter.WriteStartElement("total_other_fare");
                    if (x == null) { objXmlWriter.WriteValue("0"); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("total_other_fare").InnerXml); }
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
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("waitlist_open_flag").InnerXml); }
                    objXmlWriter.WriteEndElement();//End waitlist_open_flag

                    objXmlWriter.WriteStartElement("booking_class_rcd");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("booking_class_rcd").InnerXml); }
                    objXmlWriter.WriteEndElement();//End booking_class_rcd

                    objXmlWriter.WriteStartElement("boarding_class_rcd");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("boarding_class_rcd").InnerXml); }
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
                    if (x == null){ objXmlWriter.WriteValue(0); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("transit_redemption_points").InnerXml); }
                    objXmlWriter.WriteEndElement();//End transit_redemption_points

                    objXmlWriter.WriteStartElement("promotion_code");
                    if (x == null) { objXmlWriter.WriteValue(string.Empty); }
                    else { objXmlWriter.WriteValue(x.SelectSingleNode("promotion_code").InnerXml); }
                    objXmlWriter.WriteEndElement();//End promotion_code      

            }
            objXmlWriter.WriteEndElement();//End group
        }
        #endregion
    }
}
