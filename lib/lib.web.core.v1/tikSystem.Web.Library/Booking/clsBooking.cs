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
using System.Xml.Serialization;
using System.Xml.XPath;

namespace tikSystem.Web.Library
{
    public class Booking : LibraryBase
    {
        #region Booking object
        Guid _UserId = Guid.Empty;
        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        string _PaymentReference;
        public string PaymentReference
        {
            get { return _PaymentReference; }
            set { _PaymentReference = value; }
        }
        string _PaymentGateway;
        public string PaymentGateway
        {
            get { return _PaymentGateway; }
            set { _PaymentGateway = value; }
        }
        string _formOfPaymentFee;
        public string formOfPaymentFee
        {
            get { return _formOfPaymentFee; }
            set { _formOfPaymentFee = value; }
        }

        BookingHeader _bookingHeader;
        public BookingHeader BookingHeader
        {
            get { return _bookingHeader; }
            set { _bookingHeader = value; }
        }

        Itinerary _itinerary;
        public Itinerary Itinerary
        {
            get { return _itinerary; }
            set { _itinerary = value; }
        }

        Passengers _Passengers;
        public Passengers Passengers
        {
            get { return _Passengers; }
            set { _Passengers = value; }
        }

        Mappings _Mappings;
        public Mappings Mappings
        {
            get { return _Mappings; }
            set { _Mappings = value; }
        }

        Quotes _Quotes;
        public Quotes Quotes
        {
            get { return _Quotes; }
            set { _Quotes = value; }
        }

        Fees _Fees;
        public Fees Fees
        {
            get { return _Fees; }
            set { _Fees = value; }
        }

        Taxes _Taxes;
        public Taxes Taxes
        {
            get { return _Taxes; }
            set { _Taxes = value; }
        }
        Payments _Payments;
        public Payments Payments
        {
            get { return _Payments; }
            set { _Payments = value; }
        }

        Remarks _Remarks;
        public Remarks Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        Services _Services;
        public Services Services
        {
            get { return _Services; }
            set { _Services = value; }
        }

        public string ErrorCode;
        public string ErrorMessage;

        string _ActiveXml = string.Empty;
        string _FlownXml = string.Empty;

        #endregion

        #region Method
        
        public string GetBookings(string airline,
                                    string flightNumber,
                                    string flightId,
                                    DateTime flightFrom,
                                    DateTime flightTo,
                                    string recordLocator,
                                    string origin,
                                    string destination,
                                    string passengerName,
                                    string seatNumber,
                                    string ticketNumber,
                                    string phoneNumber,
                                    string agencyCode,
                                    string clientNumber,
                                    string memberNumber,
                                    string clientId,
                                    bool showHistory,
                                    string language,
                                    bool bIndividual,
                                    bool bGroup,
                                    DateTime createFrom,
                                    DateTime createTo)
        {
            try
            {
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;

                string strResult = string.Empty;
                using (DataSet ds = objClient.GetBookings(airline,
                                                        flightNumber,
                                                        flightId,
                                                        flightFrom,
                                                        flightTo,
                                                        recordLocator,
                                                        origin,
                                                        destination,
                                                        passengerName,
                                                        seatNumber,
                                                        ticketNumber,
                                                        phoneNumber,
                                                        agencyCode,
                                                        clientNumber,
                                                        memberNumber,
                                                        clientId,
                                                        showHistory,
                                                        language,
                                                        bIndividual,
                                                        bGroup,
                                                        createFrom,
                                                        createTo))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strResult = ds.GetXml();
                    }
                }
                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public new void Clear()
        {
            Library objLi = new Library();
            objLi.ClearBooking(ref _bookingHeader,
                                ref _itinerary,
                                ref _Passengers,
                                ref _Quotes,
                                ref _Fees,
                                ref _Mappings,
                                ref _Services,
                                ref _Remarks,
                                ref _Payments,
                                ref _Taxes);
        }
        public DateTime GetBookingTimeLimit(Remarks remarks, int agencyTimeZone, int systemSettingTimeZone, DateTime dtDepartureDate)
        {
            DateTime dtCalculated = DateTime.MinValue;

            int agencyCalculateTime;

            //Check the timezone to used for calculate
            if (agencyTimeZone > 0)
            {
                agencyCalculateTime = agencyTimeZone;
            }
            else
            {
                agencyCalculateTime = systemSettingTimeZone;
            }

            //Check if there is a Remark.
            if (remarks != null && remarks.Count > 0)
            {
                for (int i = 0; i < remarks.Count; i++)
                {
                    if (remarks[i].remark_type_rcd.Equals("TKTL"))
                    {
                        if (remarks[i].timelimit_date_time.Equals(DateTime.MinValue))
                        {
                            dtCalculated = remarks[i].utc_timelimit_date_time.AddMinutes(agencyCalculateTime);
                            if (dtCalculated > dtDepartureDate)
                            {
                                dtCalculated = dtDepartureDate;
                            }
                            remarks[i].timelimit_date_time = dtCalculated;
                        }
                        else
                        {
                            dtCalculated = remarks[i].timelimit_date_time;
                        }

                        break;
                    }
                }
            }

            return dtCalculated;
        }
        public string GetActiveBookings(Guid gClientProfileId)
        {
            Library objLi = new Library();
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            return objClient.GetActiveBookings(gClientProfileId);
        }
        public string GetFlownBookings(Guid gClientProfileId)
        {
            Library objLi = new Library();
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            return objClient.GetFlownBookings(gClientProfileId);
        }
        public bool BookingRead(Guid bookingId)
        {
            if (bookingId.Equals(Guid.Empty))
            {
                return false;
            }
            else
            {
                //Call webservice for read booking.
                ServiceClient obj = new ServiceClient();
                Library li = new Library();

                string strBookingXml = string.Empty;
                obj.objService = objService;

                strBookingXml = obj.GetBooking(bookingId);

                if (string.IsNullOrEmpty(strBookingXml) == false)
                {
                    System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(new StringReader(strBookingXml));
                    System.Xml.XPath.XPathNavigator nv = xmlDoc.CreateNavigator();
                    if (nv.Select("ErrorResponse/Error").Count == 0)
                    {
                        _bookingHeader = new BookingHeader();
                        _Passengers = new Passengers();
                        _itinerary = new Itinerary();
                        _Mappings = new Mappings();
                        _Payments = new Payments();
                        _Remarks = new Remarks();
                        _Taxes = new Taxes();
                        _Quotes = new Quotes();
                        _Fees = new Fees();
                        _Services = new Services();

                        li.FillBooking(strBookingXml,
                                       ref _bookingHeader,
                                       ref _Passengers,
                                       ref _itinerary,
                                       ref _Mappings,
                                       ref _Payments,
                                       ref _Remarks,
                                       ref _Taxes,
                                       ref _Quotes,
                                       ref _Fees,
                                       ref _Services);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Function

        public string SaveBooking(bool isCreatedTicket,
                                    ref BookingHeader bookingHeader,
                                    ref Itinerary itinerary,
                                    ref Passengers passengers,
                                    ref Quotes quotes,
                                    ref Fees fees,
                                    ref Mappings mappings,
                                    ref Services services,
                                    ref Remarks remarks,
                                    ref Payments payments,
                                    ref Taxes taxes,
                                    string strLanguage)
        {
            ServiceClient objClient = new ServiceClient();
            string result = string.Empty;
            objClient.objService = objService;
            Library objLi = new Library();

            result = objClient.SaveBooking(bookingHeader.booking_id.ToString(),
                                         XmlHelper.Serialize(bookingHeader, false),
                                         XmlHelper.Serialize(itinerary, false),
                                         XmlHelper.Serialize(passengers, false),
                                         XmlHelper.Serialize(remarks, false),
                                         XmlHelper.Serialize(payments, false),
                                         XmlHelper.Serialize(mappings, false),
                                         XmlHelper.Serialize(services, false),
                                         XmlHelper.Serialize(taxes, false),
                                         XmlHelper.Serialize(fees, false),
                                         isCreatedTicket,
                                         false,
                                         false,
                                         strLanguage);
            if (string.IsNullOrEmpty(result) == false)
            {
                XPathDocument xmlDoc = new XPathDocument(new StringReader(result));
                XPathNavigator nv = xmlDoc.CreateNavigator();

                if (nv.Select("Booking/BookingHeader").Count > 0)
                {
                    //Get Xml String and return out to generate control
                    objLi.FillBooking(result,
                                      ref bookingHeader,
                                      ref passengers,
                                      ref itinerary,
                                      ref mappings,
                                      ref payments,
                                      ref remarks,
                                      ref taxes,
                                      ref quotes,
                                      ref fees,
                                      ref services);
                }
            }

            return result;
        }

        public bool SaveBookingHeader(ref BookingHeader bookingHeader)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            Library objLi = new Library();
            string header = XmlHelper.Serialize(bookingHeader, false);

            return objClient.SaveBookingHeader(header);
        }

        public DataSet BookingLogon(string strRecordLocator, string strNameOrPhone)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            return objClient.BookingLogon(strRecordLocator, strNameOrPhone);
        }
        public string GetRecordLocator(ref string recordLocator, ref int bookingNumber)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            return objClient.GetRecordLocator(ref recordLocator, ref bookingNumber);
        }
        public bool Booked(Flights objFlights,
                            string agencyCode,
                            string currency,
                            string bookingID,
                            short adults,
                            short children,
                            short infants,
                            short others,
                            string strOthers,
                            string userId,
                            string strIpAddress,
                            string strLanguage,
                            bool bNoVat)
        {
            try
            {
                if (objFlights != null)
                {
                    ServiceClient obj = new ServiceClient();
                    Library li = new Library();

                    string strFlightXml = string.Empty;
                    obj.objService = objService;

                    strFlightXml = XmlHelper.Serialize(objFlights, true);
                    obj.objService = objService;
                    strFlightXml = obj.FlightAdd(agencyCode,
                                                currency,
                                                strFlightXml,
                                                bookingID,
                                                adults,
                                                children,
                                                infants,
                                                others,
                                                strOthers,
                                                userId,
                                                strIpAddress,
                                                strLanguage,
                                                bNoVat);

                    if (string.IsNullOrEmpty(strFlightXml) == false)
                    {
                        System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(new StringReader(strFlightXml));
                        System.Xml.XPath.XPathNavigator nv = xmlDoc.CreateNavigator();
                        if (nv.Select("ErrorResponse/Error").Count == 0)
                        {
                            _bookingHeader = new BookingHeader();
                            _Passengers = new Passengers();
                            _itinerary = new Itinerary();
                            _Mappings = new Mappings();
                            _Payments = new Payments();
                            _Remarks = new Remarks();
                            _Taxes = new Taxes();
                            _Quotes = new Quotes();
                            _Fees = new Fees();
                            _Services = new Services();

                            li.FillBooking(strFlightXml,
                                           ref _bookingHeader,
                                           ref _Passengers,
                                           ref _itinerary,
                                           ref _Mappings,
                                           ref _Payments,
                                           ref _Remarks,
                                           ref _Taxes,
                                           ref _Quotes,
                                           ref _Fees,
                                           ref _Services);

                            //Fill Transit point information.
                            _itinerary.FillExtendedSegmentInformation(objFlights);
                            _itinerary.FillDepartureDateDetail();
                            UpdateBookedInformation(objFlights, adults, children, infants, strIpAddress, currency);
                            return true;
                        }
                        else
                        {
                            ErrorCode = XmlHelper.XpathValueNullToEmpty(nv, "ErrorResponse/Error/ErrorCode");
                            ErrorMessage = XmlHelper.XpathValueNullToEmpty(nv, "ErrorResponse/Error/Message");

                            return false;
                        }
                    }
                    else
                    {
                        ErrorCode = "{204}";
                        ErrorMessage = "Select flight failed please try again.";
                        return false;
                    }
                }
                else
                {
                    ErrorCode = "{202}";
                    ErrorMessage = "Please Select Flight.";

                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Function Booked : " + e.Message + "\n" + e.StackTrace);
            }
        }
        public bool ReadBookingHistory(string strClientProfileId)
        {
            string strResult = string.Empty;
            if (objService != null)
            {
                strResult = objService.GetBookingHistorysList(strClientProfileId);
                if (string.IsNullOrEmpty(strResult) == false)
                {
                    strResult = SecurityHelper.DecompressString(strResult);

                    //Extract Xml 
                    using (StringReader srd = new StringReader(strResult))
                    {
                        using (XmlReader reader = XmlReader.Create(srd))
                        {
                            while (!reader.EOF)
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name == "BookingActive")
                                    {
                                        _ActiveXml = reader.ReadInnerXml();
                                    }
                                    else if (reader.Name == "BookingFlown")
                                    {
                                        _FlownXml = reader.ReadInnerXml();
                                    }
                                    else
                                    {
                                        reader.Read();
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public string ActiveXml()
        {
            return "<ClientBookings>" + _ActiveXml + "</ClientBookings>";
        }
        public string FlownXml()
        {
            return "<ClientBookings>" + _FlownXml + "</ClientBookings>";
        }
        #endregion
        #region Helper
        private void UpdateBookedInformation(Flights objFlights,
                                            int adults,
                                            int children,
                                            int infants,
                                            string strIpAddress,
                                            string strCurrency)
        {
            // Start add for Multi Stop
            for (int i = 0; i < _itinerary.Count; i++)
            {
                for (int j = 0; j < objFlights.Count; j++)
                {
                    if (objFlights[j].flight_id == _itinerary[i].flight_id &&
                        string.IsNullOrEmpty(objFlights[j].transit_points) == false)
                    {
                        _itinerary[i].transit_points = objFlights[j].transit_points;
                    }
                }
            }

            //Have to Fix to B2C
            _bookingHeader.booking_source_rcd = "B2C";
            _bookingHeader.number_of_adults = adults;
            _bookingHeader.number_of_children = children;
            _bookingHeader.number_of_infants = infants;

            _bookingHeader.ip_address = strIpAddress;
            _bookingHeader.currency_rcd = strCurrency;
        }
        #endregion
    }
}
