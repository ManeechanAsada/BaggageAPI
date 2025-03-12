using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Text;
using System.Configuration;
using tikSystem.Web.Library;
using tikSystem.Web.Library.agentservice;
using tikAEROWebCheckinAPI.Classes;
using Core.Lib.Crypto;
using System.Xml;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;
using DataAccessControl;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Xml.Linq;
//using DataAccessControl;
//using DataAccessControl;

namespace tikAEROWebCheckinAPI.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]  
    public class CheckinService : System.Web.Services.WebService
    {
        #region Checkin

        private void ConvertGuidsToLower(XElement xml)
        {
            // List of GUID element names
            string[] guidElementNames = {
                "booking_segment_id",
                "booking_id",
                "passenger_id"
            };

            foreach (string guidElementName in guidElementNames)
            {
                foreach (XElement element in xml.Descendants(guidElementName))
                {
                    if (!string.IsNullOrEmpty(element.Value) && IsGuid(element.Value))
                    {
                        element.Value = element.Value.ToLower();
                    }
                }
            }
        }

        private bool IsGuid(string value)
        {
            try
            {
                Guid guid = new Guid(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetBookingSegmentCheckin(string pnr)
        {
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            if (string.IsNullOrEmpty(pnr))
            {
                throw new ArgumentException("pnr cannot be null or empty.", nameof(pnr));
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("get_booking_segment_check_in_api", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PNR", pnr);

                        conn.Open();
                        using (XmlReader reader = cmd.ExecuteXmlReader())
                        {
                            if (reader.Read())
                            {
                                XElement xml = XElement.Load(reader);

                                // Convert GUID elements to lowercase
                                ConvertGuidsToLower(xml);

                                return xml.ToString(SaveOptions.None);
                            }
                            else
                            {
                                LogHelper.writeToLogFile("Logon", "", "110", "get_booking_segment_check_in got null", string.Empty);

                                return "";
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                LogHelper.writeToLogFile("Logon", "", "110", sqlEx.Message, string.Empty);

                return "";
            }
            catch (Exception ex)
            {
                LogHelper.writeToLogFile("Logon", "", "110", ex.Message, string.Empty);

                return "";
            }
        }

        [WebMethod(EnableSession = true, Description = "Step 1 : Logon system")]
        public APIResult Logon(string strBookingRef, string strLastName)
        {
            Library objLi = new Library();
            Booking objBooking = new Booking();
            string strXml = string.Empty;
            string bookingSegments = string.Empty;

            bool isParameterWellForm = true;
            APIFlightSegments fss = new APIFlightSegments();
            APIPassengerFees fees = new APIPassengerFees();

            APIErrors errors = new APIErrors();
            string parametersForLog = strBookingRef + "|" + strLastName;

            if (string.IsNullOrEmpty(strBookingRef))
            {
                isParameterWellForm = false;
                GetAPIErrors("110", errors);
                LogHelper.writeToLogFile("Logon", parametersForLog, "110", "BookingRef is empty", string.Empty);
            }
            else if (strBookingRef.Length == 10)
            {
                strBookingRef = strBookingRef.Substring(0, 9);
            }

            if (isParameterWellForm)
            {
                try
                {
                    // call IBE Web servcie
                    InitializeService();
                    objBooking.objService = (TikAeroXMLwebservice)Session["AgentService"];

                    Flights objFlights = new Flights();
                    objFlights.objService = (TikAeroXMLwebservice)Session["AgentService"];

                    // call directly SP
                    string xmlResult = GetBookingSegmentCheckin(strBookingRef);

                    if (!string.IsNullOrEmpty(xmlResult))
                    {
                        Session["CheckInFlight"] = xmlResult;

                        XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlResult));
                        if (GetOutstandingBalance(xmlDoc) <= 0)
                        {
                            XPathNavigator nv = xmlDoc.CreateNavigator();
                            if (nv.Select("Booking/Mapping[segment_status_rcd = 'HK'][flight_check_in_status_rcd = 'OPEN']").Count == 0)
                            {
                                GetAPIErrors("103", errors); //Flight not open for checkin
                                LogHelper.writeToLogFile("Logon", parametersForLog, "103", "Flight not open for checkin", string.Empty);
                            }
                            else
                            {
                                XPathExpression xe = nv.Compile("Booking/Mapping");
                                xe.AddSort("booking_segment_id", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);

                                foreach (XPathNavigator n in nv.Select(xe))
                                {
                                    string strBookingSegmentId = objLi.getXPathNodevalue(n, "booking_segment_id", Library.xmlReturnType.value);
                                    if (bookingSegments.IndexOf(strBookingSegmentId) < 0)
                                    {
                                        //GetFlightInformation(strBookingSegmentId);
                                        bookingSegments += strBookingSegmentId + "|";
                                    }
                                }

                                string strGetPassengerDetailXML = string.Empty;
                                string[] strBookingSegmentIds = bookingSegments.Split('|');
                                if (strBookingSegmentIds.Length > 0)
                                {
                                    foreach (string bookSegment in strBookingSegmentIds)
                                    {
                                        if (bookingSegments != "")
                                        {
                                            strGetPassengerDetailXML = GetFlightInformation(bookSegment);
                                            GetAPIFlightSegments(bookSegment, strGetPassengerDetailXML, xmlResult, fss);

                                            GetAPIPassengerFees(bookSegment, strGetPassengerDetailXML, fees);

                                            // Session["strGetPassengerDetailXML"] = strGetPassengerDetailXML;

                                        }
                                    }

                                    LogHelper.writeToLogFile("Logon:", parametersForLog, " 000", "", "");
                                    GetAPIErrors("000", errors);
                                }
                            }
                        }
                        else
                        {
                            GetAPIErrors("102", errors); //Have outstanding balance cannnot login
                            LogHelper.writeToLogFile("Logon", parametersForLog, "102", "Have outstanding balance cannnot login", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("101", errors);
                        LogHelper.writeToLogFile("Logon", parametersForLog, "101", "BookingLogon fail (dataset is null)", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("100", errors);
                    LogHelper.writeToLogFile("Logon", parametersForLog, "100", ex.Message, ex.StackTrace);
                }
            }

            return objLi.BuildAPIResultXML(fss, null, null, null, null, null, null, errors);
        }

        //[WebMethod(EnableSession = true, Description = "Step 1 : Logon system")]
        public APIResult Logon_(string strBookingRef, string strLastName)
        {
            Library objLi = new Library();
            Booking objBooking = new Booking();
            string strXml = string.Empty;
            string bookingSegments = string.Empty;

            bool isParameterWellForm = true;
            APIFlightSegments fss = new APIFlightSegments();
            APIPassengerFees fees = new APIPassengerFees();

            APIErrors errors = new APIErrors();
            string parametersForLog = strBookingRef + "|" + strLastName;

            if (string.IsNullOrEmpty(strBookingRef))
            {
                isParameterWellForm = false;
                GetAPIErrors("110", errors);
                LogHelper.writeToLogFile("Logon", parametersForLog, "110", "BookingRef is empty", string.Empty);
            }
            else if (strBookingRef.Length == 10)
            {
                strBookingRef = strBookingRef.Substring(0, 9);
            }

            if (isParameterWellForm)
            {
                try
                {
                    InitializeService();
                    objBooking.objService = (TikAeroXMLwebservice)Session["AgentService"];

                    DataSet ds = objBooking.BookingLogon(strBookingRef.Trim(), strLastName.Trim());

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string booking_id = ds.Tables[0].Rows[0]["booking_id"].ToString();

                            bool bLockedBooking = IsBookingLocked(booking_id);

                            string xml = GetBookingSegmentCheckin(strBookingRef);

                            // verify Booking is locked
                            if (!bLockedBooking)
                            {
                                Flights objFlights = new Flights();
                                objFlights.objService = (TikAeroXMLwebservice)Session["AgentService"];
                                //string xmlResult = objFlights.GetBookingSegmentCheckIn(booking_id,
                                //                                                       string.Empty,
                                //                                                       string.Empty);

                                string xmlResult = xml;
                                //exec dbo.get_booking_segment_check_in 'A9938812-DFEA-499B-A895-E099AFAA7346',NULL,NULL

                                Session["CheckInFlight"] = xmlResult;
                                XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlResult));
                                if (GetOutstandingBalance(xmlDoc) <= 0)
                                {
                                    XPathNavigator nv = xmlDoc.CreateNavigator();
                                    if (nv.Select("Booking/Mapping[segment_status_rcd = 'HK'][flight_check_in_status_rcd = 'OPEN']").Count == 0)
                                    {
                                        GetAPIErrors("103", errors); //Flight not open for checkin
                                        LogHelper.writeToLogFile("Logon", parametersForLog, "103", "Flight not open for checkin", string.Empty);
                                    }
                                    else
                                    {
                                        XPathExpression xe = nv.Compile("Booking/Mapping");
                                        xe.AddSort("booking_segment_id", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);

                                        foreach (XPathNavigator n in nv.Select(xe))
                                        {
                                            string strBookingSegmentId = objLi.getXPathNodevalue(n, "booking_segment_id", Library.xmlReturnType.value);
                                            if (bookingSegments.IndexOf(strBookingSegmentId) < 0)
                                            {
                                                //GetFlightInformation(strBookingSegmentId);
                                                bookingSegments += strBookingSegmentId + "|";
                                            }
                                        }

                                        string strGetPassengerDetailXML = string.Empty;
                                        string[] strBookingSegmentIds = bookingSegments.Split('|');
                                        if (strBookingSegmentIds.Length > 0)
                                        {
                                            foreach (string bookSegment in strBookingSegmentIds)
                                            {
                                                if (bookingSegments != "")
                                                {
                                                    strGetPassengerDetailXML = GetFlightInformation(bookSegment);
                                                    GetAPIFlightSegments(bookSegment, strGetPassengerDetailXML, xmlResult, fss);
                                                    GetAPIPassengerFees(bookSegment, strGetPassengerDetailXML, fees);

                                                    Session["strGetPassengerDetailXML"] = strGetPassengerDetailXML;

                                                }
                                            }

                                            LogHelper.writeToLogFile("Logon:", strBookingRef, " 000", "", "");
                                            GetAPIErrors("000", errors);
                                        }
                                    }
                                }
                                else
                                {
                                    GetAPIErrors("102", errors); //Have outstanding balance cannnot login
                                    LogHelper.writeToLogFile("Logon", parametersForLog, "102", "Have outstanding balance cannnot login", string.Empty);
                                }
                            }
                            else
                            {
                                // Booking is locked
                                GetAPIErrors("112", errors);
                            }
                        }
                        else
                        {
                            GetAPIErrors("101", errors);
                            LogHelper.writeToLogFile("Logon", parametersForLog, "101", "BookingLogon fail (dataset has no table/row)", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("101", errors);
                        LogHelper.writeToLogFile("Logon", parametersForLog, "101", "BookingLogon fail (dataset is null)", string.Empty);
                    }



                }
                catch (Exception ex)
                {
                    GetAPIErrors("100", errors);
                    LogHelper.writeToLogFile("Logon", parametersForLog, "100", ex.Message, ex.StackTrace);
                }
            }

            return objLi.BuildAPIResultXML(fss, null, null, null, fees,null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Step 2 : Read passenger information")]
        public APIResult GetPassengers(string strBookingSegmentId)
        {
            // remove session of seat both auto assign and select seat
            if (Session["SaveMapping"] != null)
                Session.Remove("SaveMapping");

            // fix case sensitive
            if (!string.IsNullOrEmpty(strBookingSegmentId))
                strBookingSegmentId = strBookingSegmentId.ToLower();

            Library objLi = new Library();
            string strGetPassengerDetailXML = string.Empty;

            APIPassengerMappings mappings = new APIPassengerMappings();
            APIPassengerServices services = new APIPassengerServices();

            APIPassengerAddresses addresses = new APIPassengerAddresses();
            APIPassengerDocuments docs = new APIPassengerDocuments();



            APIErrors errors = new APIErrors();
            bool isParameterWellForm = true;
            string codeMessage = "201"; // not found passenger information
            string parametersForLog = strBookingSegmentId;

            if (string.IsNullOrEmpty(strBookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("210", errors);
                LogHelper.writeToLogFile("GetPassengers", parametersForLog, "210", "BookingSegmentId is empty", string.Empty);
            }

            if (isParameterWellForm)
            {
                try
                {
                    //InitializeService();
                    strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);
                   
                    if (strGetPassengerDetailXML != "<Booking />")
                    {
                        GetAPIPassengerMappings(strBookingSegmentId, "", strGetPassengerDetailXML, mappings);
                        GetAPIPassengerServices(strBookingSegmentId, "", strGetPassengerDetailXML, services);
                        GetAPIPassengerAddresses(strBookingSegmentId, strGetPassengerDetailXML, addresses);
                        GetAPIPassengerDocuments(strBookingSegmentId, strGetPassengerDetailXML, docs);

                        codeMessage = "000";

                    }

                    GetAPIErrors(codeMessage, errors);


                    if (codeMessage != "000")
                    {
                        LogHelper.writeToLogFile("GetPassengers", parametersForLog, "201", "Not found passenger information", string.Empty);
                    }
                    
                }
                catch(Exception ex)
                {
                    GetAPIErrors("200", errors);
                    LogHelper.writeToLogFile("GetPassengers catch", parametersForLog, "200", ex.Message, ex.StackTrace);
                }
            }

            LogHelper.writeToLogFile("GetPassengers", parametersForLog, codeMessage, "", string.Empty);
            return objLi.BuildAPIResultExtensionXML(null, mappings, null, services, addresses,docs,null, null, null, errors);
        }

       [WebMethod(EnableSession = true, Description = "Step 3 : Assign seat for passenger")]
        public APIResult AssignSeats(string strBookingSegmentId, string[] strPassengerIds)
        {
            // fix case sensitive
            if (!string.IsNullOrEmpty(strBookingSegmentId))
                strBookingSegmentId = strBookingSegmentId.ToLower();

            if (strPassengerIds != null && strPassengerIds.Length > 0)
            {
                for (int i = 0; i < strPassengerIds.Length; i++)
                {
                    strPassengerIds[i] = strPassengerIds[i].ToLower();
                }
            }

            Mappings objMappings = new Mappings();
            objMappings = (Mappings)Session["Mappings"];

            Passengers paxs = new Passengers();
            paxs = (Passengers)Session["Passengers"];

            Mappings objSaveMapping = new Mappings();
            CheckInPassengers objCki = new CheckInPassengers();
            APIPassengerMappings mappingsSeatResult = new APIPassengerMappings();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            DataSet ds = new DataSet();

            bool SuccessAssignSeat = false;
            bool isParameterWellForm = true;
            string strGetPassengerDetailXML = string.Empty;
            string strPassengerIdsForXML = string.Empty;
            bool isContainAdult = false;
            int iAdult = 0;
            int iChild = 0;
            int iInfant = 0;
            string strNotInfant = string.Empty;
            string strInfant = string.Empty;
            string[] strNotInfants = null;
            string[] strInfants = null;
            bool isAllowed = false;
            bool isIncludeOFFLOADED = false;
            string message = "301";
            bool isIncludeSeatAssigned = false;
            string parametersForLog = string.Empty;
            bool IsTicket = false;

            parametersForLog += "strBookingSegmentId : " + strBookingSegmentId + "::: passengerId :";
            foreach (string passengerId in strPassengerIds)
            {
                parametersForLog += passengerId + "|";
            }

            if (string.IsNullOrEmpty(strBookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("310", errors);
                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "310", "BookingSegmentId is empty", string.Empty);
            }
            else if (strPassengerIds == null || strPassengerIds.Length == 0)
            {
                isParameterWellForm = false;
                GetAPIErrors("312", errors);
                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "312", "PassengerIds is empty", string.Empty);
            }

            if (isParameterWellForm)
            {
                try
                {
                    if (objMappings != null && objMappings.Count > 0)
                    {
                        //check passenger type & OFFLOADED for seat assign from web config
                        string strPassengerTypeBlock = string.Empty;
                        bool isSeatOFFLOADEDAllowed = false;
                        bool isAllowCHDCheckinAlone = false;

                        if (System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"] != null)
                        {
                            strPassengerTypeBlock = System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"].ToUpper();
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"] != null)
                        {
                            isSeatOFFLOADEDAllowed = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"]);
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"] != null)
                        {
                            isAllowCHDCheckinAlone = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"]);
                        }

                        //check passenger condition -- not allow CHD and INF checkin independently
                        strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);
                        XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
                        XPathNavigator nv = xmlDoc.CreateNavigator();

                        foreach (string strPassengerId in strPassengerIds)
                        {
                            foreach (Mapping m in objMappings)
                            {
                                if (m.booking_segment_id.Equals(new Guid(strBookingSegmentId)) &&
                                   (strPassengerId.Length == 0 || m.passenger_id.Equals(new Guid(strPassengerId))) &&
                                    !strPassengerTypeBlock.Contains(m.passenger_type_rcd.ToUpper()))
                                //&& string.IsNullOrEmpty(m.seat_number))
                                {
                                    // before checkin should be ticket
                                    if (m.e_ticket_flag == 1 && m.ticket_number.Length > 0)
                                    {
                                        IsTicket = true;
                                        if (m.passenger_check_in_status_rcd == null || m.passenger_check_in_status_rcd.Length == 0)
                                        {
                                            isAllowed = true;
                                        }

                                        if (m.passenger_check_in_status_rcd != null && m.passenger_check_in_status_rcd.ToUpper().Equals("NOSHOW"))
                                        {
                                            isAllowed = true;
                                        }


                                        //yo 31-01
                                        if (m.passenger_check_in_status_rcd == "OFFLOADED")
                                        {
                                            if (!isSeatOFFLOADEDAllowed)
                                            {
                                                isIncludeOFFLOADED = true;
                                            }
                                            else
                                            {
                                                isAllowed = true;
                                            }
                                        }

                                        //yo 10-02
                                        if (!string.IsNullOrEmpty(m.seat_number))
                                        {
                                            isIncludeSeatAssigned = true;
                                        }

                                        if (isAllowed)
                                        {
                                            switch (m.passenger_type_rcd.ToUpper())
                                            {
                                                case "CHD":
                                                    if (m.free_seating_flag != 1)//yo 30-01
                                                    {
                                                        strNotInfant += strPassengerId + "|";
                                                    }
                                                    iChild++;
                                                    break;
                                                case "INF":
                                                    if (m.free_seating_flag != 1)//yo 30-01
                                                    {
                                                        strInfant += strPassengerId + "|";
                                                    }
                                                    iInfant++;
                                                    break;
                                                default:
                                                    isContainAdult = true;
                                                    if (m.free_seating_flag != 1)//yo 30-01
                                                    {
                                                        strNotInfant += strPassengerId + "|";
                                                    }
                                                    iAdult++;

                                                    break;
                                            }

                                            objSaveMapping.Add(m);
                                        }

                                    }
                                    else
                                    {
                                        IsTicket = false;
                                    }

                                    strPassengerIdsForXML += m.passenger_id + "|";
                                }
                            }
                        }

                        // if valid
                        if (IsTicket)
                        {
                            Session["SaveMapping"] = objSaveMapping;

                            if (isIncludeOFFLOADED)//!isAllowed
                            {
                                GetAPIErrors("305", errors); //OFFLOADED passenger can't checkin
                                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "305", "OFFLOADED passenger can't checkin", string.Empty);
                            }
                            else if (isIncludeSeatAssigned)
                            {
                                GetAPIErrors("314", errors); //Passengers already have been assigned seat
                                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "314", "Passengers already have been assigned seat", string.Empty);
                            }
                            else if (!isContainAdult && !isAllowCHDCheckinAlone)
                            {
                                GetAPIErrors("303", errors); //Child or infant can't assign seat independently
                                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "303", "Child or infant can't assign seat independently", string.Empty);
                            }
                            else if (iInfant > iAdult)
                            {
                                GetAPIErrors("304", errors); //Number of adults need to be higher than the number of infants
                                LogHelper.writeToLogFile("AssignSeats", parametersForLog, "304", "Number of adults need to be higher than the number of infants", string.Empty);
                            }
                            else
                            {
                                strNotInfants = strNotInfant.Split('|');
                                strInfants = strInfant.Split('|');

                                if (objSaveMapping.Count > 0 && objSaveMapping[0].free_seating_flag == 0)
                                {
                                    //check allpaxhaveseat
                                    bool allHaveSeat = false;
                                    bool paxHaveSeat = false;
                                    foreach (Mapping mp in objSaveMapping)
                                    {
                                        paxHaveSeat = objCki.AllPaxHaveSeat(objSaveMapping, mp.booking_segment_id, mp.passenger_id.ToString());
                                        if (paxHaveSeat)
                                        {
                                            allHaveSeat = true;
                                        }
                                    }

                                    if (!allHaveSeat)
                                    {
                                        Itinerary it = (Itinerary)Session["Itinerary"];

                                        ServiceClient objClient = new ServiceClient();
                                        objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];

                                        //Load Seat map.
                                        ds = objClient.GetSeatMapLayout(it[0].flight_id.ToString(), it[0].origin_rcd, it[0].destination_rcd, it[0].boarding_class_rcd, string.Empty, tikAEROWebCheckinAPI.Classes.Language.CurrentCode());
                                        if (ds != null && ds.Tables.Count > 0)
                                        {
                                            switch (ConfigurationManager.AppSettings["SeatAssignType"].ToUpper())
                                            {
                                                case "P":
                                                    SuccessAssignSeat = objCki.AssignSeatPercentage(ds.Tables[0], ref objSaveMapping, ref message, false, false);
                                                    break;
                                                case "BB":
                                                    SuccessAssignSeat = objCki.AssignSeatByBay(ds.Tables[0], ref objSaveMapping, ref message, false, false);
                                                    break;
                                                case "FB":
                                                    SuccessAssignSeat = objCki.AssignSeatFromBack(ds.Tables[0], ref objSaveMapping, ref message, false, false);
                                                    break;
                                                case "M":
                                                    SuccessAssignSeat = objCki.AssignSeatFromMiddle(ds.Tables[0], ref objSaveMapping, message, false, false);
                                                    break;
                                                default:
                                                    SuccessAssignSeat = false;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    message = "307";//free_seating_flag = 1;This flight is free-seating.
                                }

                                //add log
                                if (objSaveMapping != null)
                                {
                                    string parametersForLog4seat = "";
                                    foreach (Mapping m in objSaveMapping)
                                    {
                                        parametersForLog4seat += "pax id : " + m.passenger_id + " | seat number : " + m.seat_number + "|";
                                    }

                                    LogHelper.writeToLogFile("AssignSeats", parametersForLog4seat, "", "Log for assign seat", string.Empty);

                                    Session["SaveMapping"] = objSaveMapping;
                                }

                                if (SuccessAssignSeat)
                                {
                                    strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);

                                    string[] passIDs = strPassengerIdsForXML.Split('|');
                                    if (passIDs.Length > 0)
                                    {
                                        foreach (string passID in passIDs)
                                        {
                                            if (passID != "")
                                            {
                                                GetAPIPassengerMappings(strBookingSegmentId, passID, strGetPassengerDetailXML, mappingsSeatResult);
                                            }
                                        }
                                    }

                                    GetAPIErrors("000", errors);
                                    
                                }
                                else
                                {
                                    //GetAPIErrors("301", errors);
                                    GetAPIErrors(message, errors);//301

                                    if (message == "301")
                                    {
                                        LogHelper.writeToLogFile("AssignSeats", parametersForLog, "301", "Passengers could not be assigned seat", string.Empty);
                                    }
                                    else if (message == "307")
                                    {
                                        LogHelper.writeToLogFile("AssignSeats", parametersForLog, "307", "This flight is free-seating", string.Empty);
                                    }
                                }

                            }
                        }
                        else
                        {
                            GetAPIErrors("315", errors);
                            LogHelper.writeToLogFile("AssignSeats", parametersForLog, "315", "Ticket number is empty.", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("302", errors);
                        LogHelper.writeToLogFile("AssignSeats", parametersForLog, "302", "objMappings is null", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("300", errors);
                    LogHelper.writeToLogFile("AssignSeats", parametersForLog, "300", ex.Message, ex.StackTrace);
                }
            }

           // LogHelper.writeToLogFile("AssignSeats", "", "", "", "");
            return objLi.BuildAPIResultXML(null, mappingsSeatResult, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Step 4 : Check in")]
        public APIResult Commit(string strBookingSegmentId, string[] strPassengerIds)
        {
            // fix case sensitive
            if (!string.IsNullOrEmpty(strBookingSegmentId))
                strBookingSegmentId = strBookingSegmentId.ToLower();

            if (strPassengerIds != null && strPassengerIds.Length > 0)
            {
                for (int i = 0; i < strPassengerIds.Length; i++)
                {
                    strPassengerIds[i] = strPassengerIds[i].ToLower();
                }
            }

            Mappings objSaveMapping = new Mappings();
            objSaveMapping = (Mappings)Session["SaveMapping"];
            
            Mappings objMappings = new Mappings();
            objMappings = (Mappings)Session["Mappings"];
            
            Passengers paxs = new Passengers();
            paxs = (Passengers)Session["Passengers"];
            
            CheckInPassengers objCki = new CheckInPassengers();
            APIPassengerMappings mappings = new APIPassengerMappings();
            APIResult result = new APIResult();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool bResult = false;
            bool isParameterWellForm = true;
            bool isContainAdult = false;
            int iAdult = 0;
            int iChild = 0;
            int iInfant = 0;
          //  int iAttempt = 0;
            bool isAllowed = false;
            bool isNeedSeatAssign = false;
            bool isIncludeOFFLOADED = false;
            bool isIncludeNotAllowed = false;
            bool isInObjSaveMapping = false;
            string parametersForLog = string.Empty;
            string strPassengerIdsForXML = string.Empty;
            string strGetPassengerDetailXML = string.Empty;
            bool PreSeat = false;
            bool PreCommit = false;
            bool bAllowCommitFromPreCommit = true;
            bool IsNoShowAllowedCommit = false;
            bool NotAllowedNoShowCommit = false;

            parametersForLog += "strBookingSegmentId : " + strBookingSegmentId + ":::passengerId :";
            foreach (string passengerId in strPassengerIds)
            {
                parametersForLog += passengerId + "|";
            }

            if (string.IsNullOrEmpty(strBookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("410", errors);
                LogHelper.writeToLogFile("Commit", parametersForLog, "410", "BookingSegmentId is empty", string.Empty);
            }
            else if (strPassengerIds == null || strPassengerIds.Length == 0)
            {
                isParameterWellForm = false;
                GetAPIErrors("411", errors);
                LogHelper.writeToLogFile("Commit", parametersForLog, "411", "PassengerIds is empty", string.Empty);
            }

            if (isParameterWellForm)
            {
                try
                {
                    if (objMappings != null)
                    {
                        //check passenger type & OFFLOADED for seat assign from web config
                        string strPassengerTypeBlock = string.Empty;
                        bool isSeatOFFLOADEDAllowed = false;
                        bool isAllowCHDCheckinAlone = false;

                        if (System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"] != null)
                        {
                            strPassengerTypeBlock = System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"].ToUpper();
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"] != null)
                        {
                            isSeatOFFLOADEDAllowed = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"]);
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"] != null)
                        {
                            isAllowCHDCheckinAlone = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"]);
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["IsNoShowAllowedCommit"] != null)
                        {
                            IsNoShowAllowedCommit = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsNoShowAllowedCommit"]);
                        }

                        //check passenger condition -- not allow CHD and INF checkin independently
                        strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);

                        XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
                        XPathNavigator nv = xmlDoc.CreateNavigator();

                        foreach (string strPassengerId in strPassengerIds)
                        {
                            //check passenger condition -- not allow CHD and INF checkin independently
                            foreach (XPathNavigator n2 in nv.Select("Booking/Passenger[passenger_id='" + strPassengerId + "']"))
                            {
                                switch (XmlHelper.XpathValueNullToEmpty(n2, "passenger_type_rcd").ToUpper())
                                {
                                    case "CHD":
                                        iChild++;
                                        break;
                                    case "INF":
                                        iInfant++;
                                        break;
                                    default:
                                        isContainAdult = true;
                                        iAdult++;
                                        break;
                                }
                            }

                            if (objSaveMapping != null)
                            {
                                foreach (Mapping m in objSaveMapping)
                                {
                                    if (m.booking_segment_id.Equals(new Guid(strBookingSegmentId)) &&
                                       (strPassengerId.Length == 0 || m.passenger_id.Equals(new Guid(strPassengerId))) &&
                                        !strPassengerTypeBlock.Contains(m.passenger_type_rcd.ToUpper()))
                                    {
                                        if (m.e_ticket_flag == 1 && m.ticket_number.Length > 0)
                                        {
                                            // allow noshow checkin 
                                            if (m.passenger_check_in_status_rcd == null || m.passenger_check_in_status_rcd.Length == 0 || m.passenger_check_in_status_rcd.Equals("NOSHOW"))
                                            {
                                                isAllowed = true;
                                            }

                                            if (m.passenger_check_in_status_rcd != null && m.passenger_check_in_status_rcd.ToUpper().Equals("NOSHOW"))
                                            {
                                                isAllowed = true;
                                            }

                                            //yo 30-01
                                            if (m.free_seating_flag == 0 && string.IsNullOrEmpty(m.seat_number))
                                            {
                                                isNeedSeatAssign = true;
                                            }
                                            else
                                            {
                                                //  parametersForLog += "booking id : " + m.booking_id + " | pax id : " + m.passenger_id + " | seat number : " + m.seat_number + "|";
                                            }

                                            //yo 31-01
                                            if (m.passenger_check_in_status_rcd == "OFFLOADED")
                                            {
                                                if (!isSeatOFFLOADEDAllowed)
                                                {
                                                    isIncludeOFFLOADED = true;
                                                }
                                                else
                                                {
                                                    isAllowed = true;
                                                }
                                            }

                                            if (m.passenger_check_in_status_rcd == "NOSHOW")
                                            {
                                                if (IsNoShowAllowedCommit)
                                                {
                                                    isAllowed = true;
                                                }
                                                else
                                                {
                                                    NotAllowedNoShowCommit = true;
                                                    isAllowed = false;
                                                }
                                            }

                                            if (isAllowed && isNeedSeatAssign == false)
                                            {
                                                m.passenger_check_in_status_rcd = "CHECKED";
                                                m.passenger_status_rcd = "OK";
                                                m.standby_flag = 0;
                                                m.check_in_user_code = "WEB";
                                                m.check_in_by = m.passenger_id;
                                                m.check_in_code = "WEB";
                                                m.check_in_date_time = DateTime.Today;

                                                if (m.hand_luggage_flag == 1)
                                                {
                                                    m.hand_number_of_pieces = paxs.Count;
                                                    m.hand_baggage_weight = paxs.Count + Convert.ToDouble(ConfigurationManager.AppSettings["HandWeight"]);
                                                }
                                                //objSaveMapping.Add(m);
                                            }
                                            else
                                            {
                                                //yo 09-02
                                                isIncludeNotAllowed = true;
                                            }

                                            //yo 13-02
                                            isInObjSaveMapping = true;
                                        }

                                        strPassengerIdsForXML += m.passenger_id + "|";
                                    }

                                    if (isIncludeNotAllowed) break;
                                }
                            }

                            //passsengerId not in objSaveMapping //yo 13-02
                            if (!isInObjSaveMapping)
                            {
                                //find again in objMappings
                                foreach (Mapping mm in objMappings)
                                {
                                    if (mm.booking_segment_id.Equals(new Guid(strBookingSegmentId)) &&
                                   (strPassengerId.Length == 0 || mm.passenger_id.Equals(new Guid(strPassengerId))) &&
                                    !strPassengerTypeBlock.Contains(mm.passenger_type_rcd.ToUpper()))
                                    {
                                        if (mm.e_ticket_flag == 1 && mm.ticket_number.Length > 0)
                                        {

                                            if (mm.passenger_check_in_status_rcd == null || mm.passenger_check_in_status_rcd.Length == 0)
                                            {
                                                isAllowed = true;
                                            }

                                            if (mm.passenger_check_in_status_rcd != null && mm.passenger_check_in_status_rcd.ToUpper().Equals("NOSHOW"))
                                            {
                                                isAllowed = true;
                                            }


                                            if (mm.free_seating_flag == 0 && string.IsNullOrEmpty(mm.seat_number))
                                            {
                                                isNeedSeatAssign = true;
                                            }
                                            else
                                            {
                                            }

                                            if (mm.passenger_check_in_status_rcd == "OFFLOADED")
                                            {
                                                if (!isSeatOFFLOADEDAllowed)
                                                {
                                                    isIncludeOFFLOADED = true;
                                                }
                                                else
                                                {
                                                    isAllowed = true;
                                                }
                                            }

                                            if (mm.passenger_check_in_status_rcd == "NOSHOW")
                                            {
                                                if (IsNoShowAllowedCommit)
                                                {
                                                    isAllowed = true;
                                                }
                                                else
                                                {
                                                    isAllowed = false;
                                                    NotAllowedNoShowCommit = true;
                                                }
                                            }

                                            if (isAllowed && isNeedSeatAssign == false)
                                            {
                                                mm.passenger_check_in_status_rcd = "CHECKED";
                                                mm.passenger_status_rcd = "OK";
                                                mm.standby_flag = 0;
                                                mm.check_in_user_code = "WEB";
                                                mm.check_in_by = mm.passenger_id;
                                                mm.check_in_code = "WEB";
                                                mm.check_in_date_time = DateTime.Today;

                                                if (mm.hand_luggage_flag == 1)
                                                {
                                                    mm.hand_number_of_pieces = paxs.Count;
                                                    mm.hand_baggage_weight = paxs.Count + Convert.ToDouble(ConfigurationManager.AppSettings["HandWeight"]);
                                                }

                                                if (objSaveMapping == null)
                                                {
                                                    objSaveMapping = new Mappings();
                                                }

                                                objSaveMapping.Add(mm);
                                            }
                                            else
                                            {
                                                isIncludeNotAllowed = true;
                                            }
                                        }

                                        strPassengerIdsForXML += mm.passenger_id + "|";
                                    }

                                    if (isIncludeNotAllowed) break;
                                }
                            }

                            //checking on checkin status of updated objMapping retrieved from session 18-02-2013
                            objMappings = (Mappings)Session["Mappings"];


                            isInObjSaveMapping = false;
                            //end check passsengerId not in objSaveMapping

                            if (isIncludeNotAllowed) break;

                        }

                        if (isIncludeOFFLOADED)//!IsAllowed
                        {
                            GetAPIErrors("405", errors); //OFFLOADED passenger can't checkin
                            LogHelper.writeToLogFile("Commit", parametersForLog, "405", "OFFLOADED passenger can't checkin", string.Empty);
                        }
                        else if (iAdult + iChild + iInfant == 0)
                        {
                            GetAPIErrors("406", errors);
                            LogHelper.writeToLogFile("Commit", parametersForLog, "406", "Number of passengers could not be zero", string.Empty);
                        }
                        else if (NotAllowedNoShowCommit)
                        {
                            GetAPIErrors("413", errors); 
                            LogHelper.writeToLogFile("Commit", parametersForLog, "413", "Passenger NOSHOW can not check in.", string.Empty);
                        }
                        else if (isIncludeNotAllowed)
                        {
                            GetAPIErrors("407", errors);
                            LogHelper.writeToLogFile("Commit", parametersForLog, "407", "Invalid passenger check in condition", string.Empty);
                        }
                        else if (!isContainAdult && !isAllowCHDCheckinAlone)
                        {
                            GetAPIErrors("403", errors); //Child or infant can't checkin independently
                            LogHelper.writeToLogFile("Commit", parametersForLog, "403", "Child or infant can't checkin independently", string.Empty);
                        }
                        else if (iInfant > iAdult)
                        {
                            GetAPIErrors("404", errors); //Number of adults need to be higher than the number of infants
                            LogHelper.writeToLogFile("Commit", parametersForLog, "404", "Number of adults need to be higher than the number of infants", string.Empty);
                        }
                        else if (isNeedSeatAssign)
                        {
                            GetAPIErrors("402", errors); //Required assign seat before check in.
                            LogHelper.writeToLogFile("Commit", parametersForLog, "402", "Required assign seat before check in", string.Empty);
                        }
                        // valid all condition .... process commit
                        else
                        {
                            // check pre seat or not
                            if (System.Configuration.ConfigurationManager.AppSettings["PreSeat"] != null)
                            {
                                PreSeat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PreSeat"]);
                            }
                            // check pre commit or not
                            if (System.Configuration.ConfigurationManager.AppSettings["PreCommit"] != null)
                            {
                                PreCommit = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PreCommit"]);
                            }
                            if (PreCommit)
                            {
                                // process pre check dup commit
                                // save pre commit
                                for (int i = 0; i < strPassengerIds.Length; i++)
                                {
                                    // this for auto assign seat, if auto assign seat 1A display 1A to user then should be save 1A with commit also.  ==> retry problem.
                                    bool bAllowPassengerCommitTemp = SavePreCommit(strBookingSegmentId, strPassengerIds[i]);

                                    if (bAllowPassengerCommitTemp == true)
                                    {
                                    }
                                    else
                                    {
                                        bAllowCommitFromPreCommit = false;
                                        break;
                                    }
                                }
                            }
                            if (bAllowCommitFromPreCommit)
                            {
                                objCki.objService = (TikAeroXMLwebservice)Session["AgentService"];

                                bResult = objCki.CheckInSave(XmlHelper.Serialize(objSaveMapping, false),
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty);

                                if (bResult) // checkin successful
                                {
                                    //Reload Booking information.
                                    Session.Remove("Mappings");
                                    Session.Remove("Itinerary");
                                    Session.Remove("Passengers");
                                    Session.Remove("FlightSegment");

                                    strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);

                                    string[] passIDs = strPassengerIdsForXML.Split('|');
                                    if (passIDs.Length > 0)
                                    {
                                        foreach (string passID in passIDs)
                                        {
                                            if (passID != "")
                                            {
                                                GetAPIPassengerMappings(strBookingSegmentId, passID, strGetPassengerDetailXML, mappings);
                                            }
                                        }
                                    }

                                    GetAPIErrors("000", errors);
                                    LogHelper.writeToLogFile("Commit:", parametersForLog, " ", "000", string.Empty);

                                }
                                else  // checkin NOT successful
                                {
                                    // no need to use on 2022
                                    //if it fails on commit then try to re-assign seat and commit again.
                                    //if (System.Configuration.ConfigurationManager.AppSettings["attemptCommit"] != null)

                                    if (!bResult)  // checkin NOT successful
                                    {
                                        GetAPIErrors("401", errors);
                                        LogHelper.writeToLogFile("Commit", parametersForLog, "401", "Passengers could not check in", string.Empty);
                                    }

                                }
                            }
                            //not valid pre commit
                            else
                            {
                                //clear seat from DB
                                if (PreSeat)
                                {
                                    for (int k = 0; k < strPassengerIds.Length; k++)
                                        RemoveAutoSeat(strPassengerIds[k]);
                                }

                                //clear pre commit from DB
                                for (int k = 0; k < strPassengerIds.Length; k++)
                                    RemovePrecommit(strPassengerIds[k]);

                                GetAPIErrors("412", errors);
                                LogHelper.writeToLogFile("Commit", parametersForLog, "412", "Some passengers were commtited already", string.Empty);
                            }
                        }
                    }
                    else
                    {
                        GetAPIErrors("402", errors);//Required assign seat before check in.
                        LogHelper.writeToLogFile("Commit", parametersForLog, "402", "Required assign seat before check in", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("400", errors);
                    LogHelper.writeToLogFile("Commit", parametersForLog, "400", ex.Message, ex.StackTrace);
                }
                finally
                {
                    //utydev-187
                    if (!bResult)  // checkin NOT successful need to clear
                    {
                        //clear seat from DB
                        if (PreSeat)
                        {
                            for (int k = 0; k < strPassengerIds.Length; k++)
                                RemoveAutoSeat(strPassengerIds[k]);
                        }

                        //clear pre commit from DB
                        if (PreCommit)
                        {
                            for (int k = 0; k < strPassengerIds.Length; k++)
                                RemovePrecommit(strPassengerIds[k]);
                        }
                    }
                }
            }
            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Step 5 : Get information for boardingpass printing")]
        public APIResult GetBoardingPasses(string strBookingSegmentId, string[] strPassengerIds)
        {
            // fix case sensitive
            if (!string.IsNullOrEmpty(strBookingSegmentId))
                strBookingSegmentId = strBookingSegmentId.ToLower();

            if (strPassengerIds != null && strPassengerIds.Length > 0)
            {
                for (int i = 0; i < strPassengerIds.Length; i++)
                {
                    strPassengerIds[i] = strPassengerIds[i].ToLower();
                }
            }

            Library objLi = new Library();
            Helper objHelper = new Helper();
            CheckInPassengers ckps = new CheckInPassengers();
            APIPassengerMappings mappings = new APIPassengerMappings();
            APIPassengerServices services = new APIPassengerServices();
            APIErrors errors = new APIErrors();
            
            string strGetPassengerDetailXML = string.Empty;
            string codeMessage = "502"; // Required check in before get boarding pass
            string parametersForLog = string.Empty;
            bool isParameterWellForm = true;

            parametersForLog += "strBookingSegmentId : " + strBookingSegmentId + ":::passengerId :";
            foreach (string passengerId in strPassengerIds)
            {
                parametersForLog += passengerId + "|";
            }

            if (string.IsNullOrEmpty(strBookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("510", errors);
                LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "510", "BookingSegmentId is empty", string.Empty);
            }
            else if (strPassengerIds == null || strPassengerIds.Length == 0)
            {
                isParameterWellForm = false;
                GetAPIErrors("511", errors);
                LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "511", "PassengerIds is empty", string.Empty);
            }

            if (isParameterWellForm)
            {
                try
                {
                    if (Session["CheckInFlight"] != null)
                    {
                        objHelper.GetFlightInformation(ref ckps,
                                                        strBookingSegmentId,
                                                        Session["CheckInFlight"].ToString());

                        ckps.objService = (TikAeroXMLwebservice)Session["AgentService"];

                        XPathDocument xmlDoc = new XPathDocument(new StringReader(ckps.GetPassengerDetails("EN")));
                        XPathNavigator nv = xmlDoc.CreateNavigator();
                        //StringBuilder stb = new StringBuilder();

                        strGetPassengerDetailXML = GetFlightInformation(strBookingSegmentId);

                        if (strGetPassengerDetailXML != "<Booking />")
                        {
                            //stb.Append("<Booking>");
                            foreach (string strPassengerId in strPassengerIds)
                            {

                                foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
                                {
                                    if (strPassengerId.Equals(objLi.getXPathNodevalue(n, "passenger_id", Library.xmlReturnType.value)))
                                    {
                                        //stb.Append(n.OuterXml);

                                        if (objLi.getXPathNodevalue(n, "passenger_check_in_status_rcd", Library.xmlReturnType.value) == "CHECKED")
                                        {
                                            GetAPIPassengerMappings(strBookingSegmentId, strPassengerId, strGetPassengerDetailXML, mappings);
                                            GetAPIPassengerServices(strBookingSegmentId, strPassengerId, strGetPassengerDetailXML, services);
                                            codeMessage = "000";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            codeMessage = "503";
                            LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "503", "strGetPassengerDetailXML is empty", string.Empty);
                        }

                        GetAPIErrors(codeMessage, errors);
                        if (codeMessage == "502")
                        {
                            LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "502", "Required check in before get boarding pass", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("501", errors);
                        LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "501", "Session[CheckInFlight] is null", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("500", errors);
                    LogHelper.writeToLogFile("GetBoardingPasses", parametersForLog, "500", ex.Message, ex.StackTrace);
                }

                //stb.Append("</Booking>");
                //return stb.ToString();
            }
            LogHelper.writeToLogFile("GetBoardingPasses", "", "", "", "");
            return objLi.BuildAPIResultXML(null, mappings, null, services, null, null, null, errors);
        }

        #endregion

        #region Token

        [WebMethod(EnableSession = true, Description = "Initial Token")]
        public APIResultMessage InitialToken(string strAgencyCode, string strAgencyLogon, string strAgencyPassword)
        {
            Library objLi = new Library();
            Users users = new Users();
            Agents agents = new Agents();
            String strResult = String.Empty;
            String strGetPassengerDetailXML = String.Empty;

            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            bool isParameterWellForm = true;
            String parametersForLog = String.Empty;
            String userId = String.Empty;
            String codeMessage = "950";
            String hky = String.Empty;


            if (string.IsNullOrEmpty(strAgencyCode))
            {
                isParameterWellForm = false;
                GetAPIErrors("951", errors);
                LogHelper.writeToLogFile("InitialToken", parametersForLog, "951", "Invalid AgencyCode parameter", string.Empty);
            }
            else if (string.IsNullOrEmpty(strAgencyLogon))
            {
                isParameterWellForm = false;
                GetAPIErrors("952", errors);
                LogHelper.writeToLogFile("InitialToken", parametersForLog, "952", "Invalid AgencyLogon parameter", string.Empty);
            }
            else if (string.IsNullOrEmpty(strAgencyPassword))
            {
                isParameterWellForm = false;
                GetAPIErrors("953", errors);
                LogHelper.writeToLogFile("InitialToken", parametersForLog, "953", "Invalid AgencyPassword parameter", string.Empty);
            }

            if (isParameterWellForm)
            {
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    try
                    {
                        String hk = String.Empty;
                        ServiceClient objService = new ServiceClient();
                        objService.initializeWebService(strAgencyCode, ref agents);

                        if (agents != null && agents.Count > 0)
                        {
                            if (agents[0].api_flag == 1)
                            {
                                users = objService.TravelAgentLogon(strAgencyCode, strAgencyLogon, strAgencyPassword);

                                if (users != null && users.Count > 0)
                                {
                                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                                    {
                                        hk = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                                    }

                                    userId = users[0].user_account_id.ToString();


                                    String temp = StringCipher.Encrypt(userId, hk);
                                    codeMessage = "000";

                                    GetAPIErrors(codeMessage, temp, errors);

                                }
                                else
                                {
                                    codeMessage = "950";
                                }
                            }
                            else
                            {
                                codeMessage = "954";
                            }
                        }

                        if (codeMessage != "000")
                        {
                            LogHelper.writeToLogFile("InitialToken", parametersForLog, "950", "Invalid initial token", string.Empty);
                            GetAPIErrors(codeMessage, errors);
                        }

                    }
                    catch (Exception ex)
                    {
                        GetAPIErrors("950", errors);
                        LogHelper.writeToLogFile("InitialToken", parametersForLog, "950", ex.Message, ex.StackTrace);
                    }
                }
                else // use new token
                {
                    bool verifySql = false;
                    string errorcode = string.Empty;
                    List<string> checkSQLStrings = new List<string>();
                    checkSQLStrings.Add(strAgencyCode);
                    checkSQLStrings.Add(strAgencyLogon);
                    checkSQLStrings.Add(strAgencyPassword);

                    verifySql = IsContainSQLStatement(checkSQLStrings, out errorcode);
                    if (!verifySql)
                    {
                        isParameterWellForm = false;
                    }

                    if (isParameterWellForm)
                    {
                        try
                        {
                            string token = GetToken(strAgencyCode, strAgencyLogon, strAgencyPassword);

                            if (!string.IsNullOrEmpty(token))
                            {
                                codeMessage = "000";
                                GetAPIErrors(codeMessage, token, errors);
                            }
                            else
                            {
                                codeMessage = "960";
                                GetAPIErrors(codeMessage, "Get token failed.", errors);
                            }
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors("950", errors);
                            LogHelper.writeToLogFile("InitialToken", parametersForLog, "950", ex.Message, ex.StackTrace);
                        }
                    }

                }

            }

            LogHelper.writeToLogFile("InitialToken:", strAgencyCode +"|" +strAgencyLogon + "|" +strAgencyPassword, codeMessage,"","");
            return objLi.BuildAPIResultXML(errors);
        }

        [WebMethod(EnableSession = true, Description = "Board Passenger")]
        public APIResultMessage BoardPassengers(string strBookingId, string strBookingSegmentId, string strPassengerId, string strBoarding, bool boardFlag, string strToken)
        {
            Library objLi = new Library();
            Helper hlper = new Helper();
            Users users = new Users();
            String strResult = String.Empty;
            String strGetPassengerDetailXML = String.Empty;

            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
            string codeMessage = "907";
            String hky = String.Empty;
            string errorResult = "";
            Token token = new Token();

            parametersForLog = strPassengerId + "|" + strBoarding;

            if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
            {
                hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
            }
            try
            {
                // String temp = StringCipher.Decrypt(strToken, hky);

                String userIdFromToken = string.Empty;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    // old token
                    userIdFromToken = StringCipher.Decrypt(strToken, hky);
                }
                else // new token
                {
                    token = Authentication(strToken);
                    userIdFromToken = token.UserId;
                }

                if (hlper.IsValid(userIdFromToken))
                {
                    if (string.IsNullOrEmpty(strBookingId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("906", errors);
                        // LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "906", "Invalid BookingID parameter", string.Empty);
                        errorResult = "906";
                    }
                    else if (string.IsNullOrEmpty(strBookingSegmentId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("310", errors);
                        //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "310", "Invalid bookingsegmentid parameter", string.Empty);
                        errorResult = "310";
                    }
                    else if (string.IsNullOrEmpty(strPassengerId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("312", errors);
                        //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "312", "Invalid passengerid parameter", string.Empty);
                        errorResult = "312";
                    }
                    else if (string.IsNullOrEmpty(strBoarding))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("900", errors);
                        //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "900", "Invalid sequence or seat parameter", string.Empty);
                        errorResult = "900";
                    }

                    if (isParameterWellForm)
                    {
                        try
                        {
                            String xmlBooking = String.Empty;
                            InitializeService();
                            ServiceClient objClient = new ServiceClient();
                            objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];


                            Guid guidBooking = new Guid(strBookingId);
                            xmlBooking = objClient.GetBooking(guidBooking);

                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.LoadXml(xmlBooking);
                            XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlBooking));
                            XPathNavigator nv = xmlDoc.CreateNavigator();

                            if (xmlBooking != "<Booking />")
                            {
                                string userId = userIdFromToken;
                                string lockName = xmldoc.SelectSingleNode("Booking/BookingHeader/lock_name").InnerText;

                                if (lockName.Trim().Equals(""))
                                {
                                    foreach (XPathNavigator f in nv.Select("Booking/FlightSegment"))
                                    {
                                        if (strBookingSegmentId.ToLower().Equals(objLi.getXPathNodevalue(f, "booking_segment_id", Library.xmlReturnType.value).ToLower()) && objLi.getXPathNodevalue(f, "segment_status_rcd", Library.xmlReturnType.value).ToLower().ToString().ToUpper().Equals("HK"))
                                        {
                                            if (objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "OPEN" || objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "CLOSED")
                                            {
                                                foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
                                                {
                                                    if (strPassengerId.ToLower() == objLi.getXPathNodevalue(n, "passenger_id", Library.xmlReturnType.value).ToString().ToLower() && objLi.getXPathNodevalue(f, "booking_segment_id", Library.xmlReturnType.value).ToLower().Equals(objLi.getXPathNodevalue(n, "booking_segment_id", Library.xmlReturnType.value).ToLower()))
                                                    {
                                                        String flightID = objLi.getXPathNodevalue(n, "flight_id", Library.xmlReturnType.value);
                                                        String originRCD = objLi.getXPathNodevalue(n, "origin_rcd", Library.xmlReturnType.value);

                                                        // IsAlreadyAssignSeat
                                                        if (string.IsNullOrEmpty(objLi.getXPathNodevalue(n, "seat_number", Library.xmlReturnType.value)))
                                                        {
                                                            codeMessage = "912";
                                                            errorResult = codeMessage;
                                                            //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "912", "Required assign seat before Boarded", string.Empty);
                                                        }
                                                        else
                                                        {
                                                            bool validStrBoard = false;
                                                            int intBoardWithSeat = Regex.Matches(strBoarding, @"[a-zA-Z]").Count;
                                                            if (intBoardWithSeat > 0)
                                                            {
                                                                if (objLi.getXPathNodevalue(n, "seat_number", Library.xmlReturnType.value).ToString().ToUpper().Equals(strBoarding.ToUpper()))
                                                                {
                                                                    validStrBoard = true;
                                                                }
                                                                else
                                                                {
                                                                    codeMessage = "913";
                                                                    // LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "913", "Seat number mismatch", string.Empty);

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (objLi.getXPathNodevalue(n, "check_in_sequence", Library.xmlReturnType.value).ToString().Equals(strBoarding))
                                                                {
                                                                    validStrBoard = true;
                                                                }
                                                                else
                                                                {
                                                                    codeMessage = "914";
                                                                    // LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "914", "Checkin sequence mismatch", string.Empty);

                                                                }

                                                            }

                                                            if (validStrBoard)
                                                            {
                                                                if (boardFlag)
                                                                {
                                                                    if (objLi.getXPathNodevalue(n, "passenger_check_in_status_rcd", Library.xmlReturnType.value) == "CHECKED")
                                                                    {
                                                                        strResult = objClient.BoardPassenger(flightID, originRCD, strBoarding, userId, boardFlag);

                                                                        if (strResult.Equals("0"))
                                                                        {
                                                                            codeMessage = "000";
                                                                        }
                                                                        else
                                                                        {
                                                                            codeMessage = "904";
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        codeMessage = "901";
                                                                        //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "901", "Passenger check in status is not CHECKED", string.Empty);
                                                                    }
                                                                }

                                                                else
                                                                {
                                                                    if (objLi.getXPathNodevalue(n, "passenger_check_in_status_rcd", Library.xmlReturnType.value) == "BOARDED")
                                                                    {
                                                                        strResult = objClient.BoardPassenger(flightID, originRCD, strBoarding, userId, boardFlag);

                                                                        if (strResult.Equals("1"))
                                                                        {
                                                                            codeMessage = "000";
                                                                        }
                                                                        else
                                                                        {
                                                                            codeMessage = "905";
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        codeMessage = "902";
                                                                        //   LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "902", "Passenger check in status is not BOARDED", string.Empty);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                codeMessage = "909";
                                                //   LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "909", "Flight not open or closed for board or un-board", string.Empty);
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    codeMessage = "911";
                                    //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "911", "Booking is in use on AVANTIK", string.Empty);
                                }
                            }
                            else
                            {
                                codeMessage = "908";
                                // LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "908", "Booking not found", string.Empty);
                            }

                            GetAPIErrors(codeMessage, errors);
                            errorResult = codeMessage;
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors("907", errors);
                            //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "907", ex.Message, ex.StackTrace);
                            errorResult = "907";
                        }
                    }
                }
                else
                {
                    GetAPIErrors(token.ResponseCode, errors);
                    //  LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "910", "Invalid token", string.Empty);
                    errorResult = "910";
                }
            }
            catch
            {
                GetAPIErrors("910", errors);
                //   LogHelper.writeToLogFile("BoardPassengers", parametersForLog, "910", "Invalid token", string.Empty);
                errorResult = "910";
            }
            LogHelper.writeToLogFile("BoardPassengers", parametersForLog, errorResult, "", "");
            return objLi.BuildAPIResultXML(errors);
        }

        [WebMethod(EnableSession = true, Description = "OffLoad Passenger")]
        public APIResultMessage OffLoadPassenger(string strBookingId, string strFlightId, string[] strPassengerIds, bool BaggageFlag, string strToken)
        {
            Library objLi = new Library();
            Helper hlper = new Helper();
            Users users = new Users();
            String strResult = String.Empty;
            String strGetPassengerDetailXML = String.Empty;
            strBookingId = strBookingId.ToLower();
            strFlightId = strFlightId.ToLower();
            // strPassengerId = strPassengerId.ToLower();
            //Lower case
            if (strPassengerIds != null && strPassengerIds.Length > 0)
            {
                for (int i = 0; i < strPassengerIds.Length; i++)
                {
                    strPassengerIds[i] = strPassengerIds[i].ToLower();
                }
            }

            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
            string codeMessage = "956";
            String hky = String.Empty;
            string errorResult = "";
            Token token = new Token();

            if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
            {
                hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
            }

            foreach (string passengerId in strPassengerIds)
            {
                parametersForLog += passengerId;
            }

            try
            {
                // String temp = StringCipher.Decrypt(strToken, hky);
                String userIdFromToken = string.Empty;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    // old token
                    userIdFromToken = StringCipher.Decrypt(strToken, hky);
                }
                else // new token
                {
                    token = Authentication(strToken);
                    userIdFromToken = token.UserId;
                }


                if (hlper.IsValid(userIdFromToken))
                {
                    if (string.IsNullOrEmpty(strBookingId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("906", errors);
                        //  LogHelper.writeToLogFile("OffLoadPassenger", parametersForLog, "906", "Invalid BookingID parameter", string.Empty);
                        errorResult = "906";
                    }
                    else if (string.IsNullOrEmpty(strFlightId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("712", errors);
                        // LogHelper.writeToLogFile("OffLoadPassenger", parametersForLog, "712", "FlightID is empty", string.Empty);
                        errorResult = "712";
                    }
                    else if (strPassengerIds == null || strPassengerIds.Length == 0)
                    {
                        isParameterWellForm = false;
                        GetAPIErrors("312", errors);
                        // LogHelper.writeToLogFile("OffLoadPassenger", parametersForLog, "312", "Invalid PassengerID parameter", string.Empty);
                        errorResult = "312";
                    }

                    if (isParameterWellForm)
                    {
                        try
                        {
                            String xmlBooking = String.Empty;
                            InitializeService();
                            ServiceClient objClient = new ServiceClient();
                            objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];


                            Guid guidBooking = new Guid(strBookingId);
                            xmlBooking = objClient.GetBooking(guidBooking);

                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.LoadXml(xmlBooking);
                            XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlBooking));
                            XPathNavigator nv = xmlDoc.CreateNavigator();

                            if (xmlBooking != "<Booking />")
                            {
                                string userId = userIdFromToken;
                                string lockName = xmldoc.SelectSingleNode("Booking/BookingHeader/lock_name").InnerText;

                                if (lockName.Trim().Equals(""))
                                {
                                    foreach (string strPassengerId in strPassengerIds)
                                    {
                                        foreach (XPathNavigator f in nv.Select("Booking/FlightSegment"))
                                        {
                                            if ((objLi.getXPathNodevalue(f, "flight_id", Library.xmlReturnType.value) == strFlightId && objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "OPEN") || (objLi.getXPathNodevalue(f, "flight_id", Library.xmlReturnType.value) == strFlightId && objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "CLOSED"))
                                            {
                                                foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
                                                {
                                                    if (strPassengerId.Equals(objLi.getXPathNodevalue(n, "passenger_id", Library.xmlReturnType.value)) && strFlightId.Equals(objLi.getXPathNodevalue(n, "flight_id", Library.xmlReturnType.value)))
                                                    {
                                                        strResult = objClient.OffLoadPassenger(strBookingId, strFlightId, strPassengerId, BaggageFlag, userId);

                                                        if (strResult.Equals("True"))
                                                        {
                                                            codeMessage = "000";
                                                        }
                                                        else
                                                        {
                                                            codeMessage = "956";
                                                        }

                                                        break;
                                                    }

                                                }

                                                break;
                                            }
                                            else
                                            {
                                                codeMessage = "955";
                                                // LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "955", "Flight not open or closed for offload", string.Empty);
                                                errorResult = "955";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    codeMessage = "911";
                                    // LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "911", "Booking is in use on AVANTIK", string.Empty);
                                    errorResult = "911";
                                }
                            }
                            else
                            {
                                codeMessage = "908";
                                //  LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "908", "Booking not found", string.Empty);
                                errorResult = "908";
                            }

                            GetAPIErrors(codeMessage, errors);
                            errorResult = codeMessage;
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors("956", errors);
                            errorResult = "956";
                            //  LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "956", ex.Message, ex.StackTrace);
                        }
                    }
                }
                else
                {
                    GetAPIErrors(token.ResponseCode, errors);
                    errorResult = token.ResponseCode;
                    // LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "910", "Invalid token", string.Empty);
                }
            }
            catch
            {
                GetAPIErrors("956", errors);
                errorResult = "956";
                // LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, "956", "Can not offload passenger", string.Empty);
            }

            LogHelper.writeToLogFile("OffloadPassenger", parametersForLog, errorResult, "", "");

            return objLi.BuildAPIResultXML(errors);
        }

        [WebMethod(EnableSession = true, Description = "NoShow Passenger")]
        public APIResultMessage NoShowPassenger(string strBookingId, string strFlightId, string[] strPassengerIds, bool BaggageFlag, string strToken)
        {
            Library objLi = new Library();
            Helper hlper = new Helper();
            Users users = new Users();
            String strResult = String.Empty;
            String strGetPassengerDetailXML = String.Empty;
            strBookingId = strBookingId.ToLower();
            strFlightId = strFlightId.ToLower();
            string errorResult = "";
            Token token = new Token();

            //Lower case
            if (strPassengerIds != null && strPassengerIds.Length > 0)
            {
                for (int i = 0; i < strPassengerIds.Length; i++)
                {
                    strPassengerIds[i] = strPassengerIds[i].ToLower();
                }
            }

            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
            string codeMessage = "961";
            String hky = String.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
            {
                hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
            }

            if (string.IsNullOrEmpty(strToken))
            {
                GetAPIErrors(codeMessage, errors);
                return objLi.BuildAPIResultXML(errors);
            }
            else if (strToken.Trim().Length > 250)
            {
                GetAPIErrors(codeMessage, errors);
                return objLi.BuildAPIResultXML(errors);
            }

            try
            {
                String userIdFromToken = string.Empty;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    // old token
                    userIdFromToken = StringCipher.Decrypt(strToken, hky);
                }
                else // new token
                {
                    token = Authentication(strToken);
                    userIdFromToken = token.UserId;
                }

                foreach (string passengerId in strPassengerIds)
                {
                    parametersForLog += passengerId;
                }

                if (hlper.IsValid(userIdFromToken))
                {
                    if (string.IsNullOrEmpty(strBookingId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors(codeMessage, errors);
                        //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Invalid BookingID parameter", string.Empty);
                        errorResult = "Invalid BookingID parameter";
                    }
                    else if (string.IsNullOrEmpty(strFlightId))
                    {
                        isParameterWellForm = false;
                        GetAPIErrors(codeMessage, errors);
                        // LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "FlightID is empty", string.Empty);
                        errorResult = "FlightID is empty";
                    }
                    else if (strPassengerIds == null || strPassengerIds.Length == 0)
                    {
                        isParameterWellForm = false;
                        GetAPIErrors(codeMessage, errors);
                        //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Invalid PassengerID parameter", string.Empty);
                        errorResult = "Invalid PassengerID parameter";
                    }

                    if (isParameterWellForm)
                    {
                        try
                        {
                            String xmlBooking = String.Empty;
                            InitializeService();
                            ServiceClient objClient = new ServiceClient();
                            objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];


                            Guid guidBooking = new Guid(strBookingId);
                            xmlBooking = objClient.GetBooking(guidBooking);

                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.LoadXml(xmlBooking);
                            XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlBooking));
                            XPathNavigator nv = xmlDoc.CreateNavigator();

                            if (xmlBooking != "<Booking />")
                            {
                                string userId = userIdFromToken;
                                string lockName = xmldoc.SelectSingleNode("Booking/BookingHeader/lock_name").InnerText;

                                if (lockName.Trim().Equals(""))
                                {
                                    foreach (string strPassengerId in strPassengerIds)
                                    {
                                        foreach (XPathNavigator f in nv.Select("Booking/FlightSegment"))
                                        {
                                            if ((objLi.getXPathNodevalue(f, "flight_id", Library.xmlReturnType.value) == strFlightId && objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "OPEN") || (objLi.getXPathNodevalue(f, "flight_id", Library.xmlReturnType.value) == strFlightId && objLi.getXPathNodevalue(f, "flight_check_in_status_rcd", Library.xmlReturnType.value) == "CLOSED"))
                                            {
                                                foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
                                                {
                                                    if (strPassengerId.Equals(objLi.getXPathNodevalue(n, "passenger_id", Library.xmlReturnType.value)) && strFlightId.Equals(objLi.getXPathNodevalue(n, "flight_id", Library.xmlReturnType.value)))
                                                    {
                                                        strResult = objClient.NoShowPassenger(strBookingId, strFlightId, strPassengerId, BaggageFlag, userId);

                                                        if (strResult.Equals("True"))
                                                        {
                                                            codeMessage = "000";
                                                        }

                                                        break;
                                                    }

                                                }

                                                break;
                                            }
                                            else
                                            {
                                                // codeMessage = "956";
                                                //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Flight not open or closed for NoShow", string.Empty);
                                                errorResult += "Flight not open";
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    //  codeMessage = "956";
                                    errorResult += "Booking is in use";
                                    //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Booking is in use on AVANTIK", string.Empty);
                                }
                            }
                            else
                            {
                                // codeMessage = "956";
                                errorResult += "Booking not found";
                                // LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Booking not found", string.Empty);
                            }

                            if (codeMessage != "000")
                            {
                                //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Can not NoShow passenger", string.Empty);
                                errorResult += "Can not NoShow passenger";
                            }

                            GetAPIErrors(codeMessage, errors);
                            errorResult = codeMessage;
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors(codeMessage, errors);
                            //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", ex.Message, ex.StackTrace);
                            errorResult += ex.Message;
                        }
                    }
                }
                else
                {
                    GetAPIErrors(token.ResponseCode, errors);
                    // LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "960", string.Empty);
                }
            }
            catch
            {
                GetAPIErrors(codeMessage, errors);
                //  LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, "", "Can not set NOSHOW passenger", string.Empty);
                errorResult += "Can not set NOSHOW passenger";
            }

            LogHelper.writeToLogFile("NoShowPassenger", parametersForLog, codeMessage, errorResult, "");

            return objLi.BuildAPIResultXML(errors);
        }

        #endregion

        [WebMethod(EnableSession = true, Description = "Change passenger passport information")]
        public APIResult UpdatePassengerDocumentDetails(APIPassengerUpdateRequests request)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool bFoundError = false;
            int iResult = 0;
            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    if (request != null)
                        if (request.Count > 0)
                        {
                            Passengers paxs = new Passengers();
                            tikSystem.Web.Library.Passenger pax = null;
                            //Fill in Request object.
                            for (int i = 0; i < request.Count; i++)
                            {
                                pax = new tikSystem.Web.Library.Passenger();
                                if (request[i].passenger_id.Equals(Guid.Empty))
                                {
                                    GetAPIErrors("602", errors);
                                    LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", request[i].passenger_id.ToString(), "602", "PassengerId is empty", string.Empty);
                                    bFoundError = true;
                                }
                                else if (!string.IsNullOrEmpty(request[i].document_type_rcd)
                                    && request[i].document_type_rcd.Length > 1)
                                {
                                    GetAPIErrors("958", errors);
                                    LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", request[i].document_type_rcd, "958", "Document Type is invalid", string.Empty);
                                    bFoundError = true;
                                }
                                else
                                {
                                    pax.passenger_id = request[i].passenger_id;
                                }

                                if (request[i].date_of_birth.Equals(DateTime.MinValue) == false)
                                {
                                    pax.date_of_birth = request[i].date_of_birth;
                                }

                                if (request[i].passport_expiry_date.Equals(DateTime.MinValue) == false)
                                {
                                    pax.passport_expiry_date = request[i].passport_expiry_date;
                                }

                                if (!string.IsNullOrEmpty(request[i].gender_type_rcd))
                                    pax.gender_type_rcd = request[i].gender_type_rcd.ToUpper();

                                if (!string.IsNullOrEmpty(request[i].nationality_rcd))
                                    pax.nationality_rcd = request[i].nationality_rcd.ToUpper();

                                if (!string.IsNullOrEmpty(request[i].passport_issue_country_rcd))
                                    pax.passport_issue_country_rcd = request[i].passport_issue_country_rcd.ToUpper();

                                if (!string.IsNullOrEmpty(request[i].passport_number))
                                    pax.passport_number = request[i].passport_number.ToUpper();


                                //18-04-2013 : add document type and document number
                                if (!string.IsNullOrEmpty(request[i].document_type_rcd))
                                    pax.document_type_rcd = request[i].document_type_rcd.ToUpper();
                                else
                                    pax.document_type_rcd = "P";

                                //add known_traveler_number
                                if (!string.IsNullOrEmpty(request[i].known_traveler_number))
                                    pax.known_traveler_number = request[i].known_traveler_number;


                                if (bFoundError == false)
                                { paxs.Add(pax); }
                            }

                            if (bFoundError == false)
                            {
                                ServiceClient objService = new ServiceClient();
                                objService.objService = (TikAeroXMLwebservice)Session["AgentService"];
                                iResult = objService.UpdatePassengerDocumentDetails(paxs);
                                
                                if (iResult == request.Count)
                                {

                                    GetAPIErrors("000", errors);
                                    //Update new information to Mapping session.
                                    Mappings objMappings = (Mappings)Session["Mappings"];
                                    for (int i = 0; i < request.Count; i++)
                                    {
                                        for (int j = 0; j < objMappings.Count; j++)
                                        {
                                            if (request[i].passenger_id.Equals(objMappings[j].passenger_id))
                                            {
                                                objMappings[j].gender_type_rcd = request[i].gender_type_rcd;
                                                objMappings[j].date_of_birth = request[i].date_of_birth;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GetAPIErrors("600", errors);
                                    LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", "iResult = " + iResult.ToString() + "", "600", "iResult is not equal Request", string.Empty);
                                }
                            }
                        }
                        else
                        {
                            GetAPIErrors("601", errors);
                            LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", "request.Count = " + request.Count.ToString() + "", "601", "Result is empty", string.Empty);
                        }
                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("600", errors);
                LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", "", "600", ex.Message, ex.StackTrace);
            }

            LogHelper.writeToLogFile("UpdatePassengerDocumentDetails", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Get seat map information")]
        public APIResult GetSeatMap(string originRcd, string destinationRcd, Guid flightId, string boardingClass, string bookingClass)
        {
            Mappings objMappings = new Mappings();
            objMappings = (Mappings)Session["Mappings"];

            Library objLi = new Library();
            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            APISeatMaps seatmaps = new APISeatMaps();
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
            string codeMessage = "715"; // No seat map information found

            parametersForLog += originRcd + "|" + destinationRcd + "|" + flightId + "|" + boardingClass + "|" + bookingClass;


            if (string.IsNullOrEmpty(originRcd))
            {
                isParameterWellForm = false;
                GetAPIErrors("710", errors);
                LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "710", "Parameter OriginRcd is empty", string.Empty);
            }
            else if (string.IsNullOrEmpty(destinationRcd))
            {
                isParameterWellForm = false;
                GetAPIErrors("711", errors);
                LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "711", "Parameter DestionationRcd is empty", string.Empty);
            }
            else if (flightId.Equals(Guid.Empty))
            {
                isParameterWellForm = false;
                GetAPIErrors("712", errors);
                LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "712", "Parameter FlightId is empty", string.Empty);
            }
            else if (string.IsNullOrEmpty(boardingClass))
            {
                isParameterWellForm = false;
                GetAPIErrors("713", errors);
                LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "713", "Parameter BoardingClass is empty", string.Empty);
            }
            else if (string.IsNullOrEmpty(bookingClass))
            {
                isParameterWellForm = false;
                GetAPIErrors("714", errors);
                LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "714", "Parameter BookingClass is empty", string.Empty);
            }

            if (isParameterWellForm)
            {
                try
                {
                    if (objMappings != null && objMappings.Count > 0)
                    {
                        ServiceClient objClient = new ServiceClient();
                        objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];

                        ds = objClient.GetSeatMap(originRcd, destinationRcd, flightId.ToString(), boardingClass, bookingClass, tikAEROWebCheckinAPI.Classes.Language.CurrentCode());

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ds.DataSetName = "SeatMaps";
                            GetAPISeatMaps(ds.Tables[0].Rows[0]["flight_id"].ToString(), ds.GetXml(), seatmaps);
                            codeMessage = "000";
                        }
                        else
                        {
                            codeMessage = "715";
                        }

                        GetAPIErrors(codeMessage, errors);

                        if (codeMessage != "000")
                        {
                            LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "715", "No seat map information found", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("701", errors);
                        LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "701", "objMappings is null", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("700", errors);
                    LogHelper.writeToLogFile("GetSeatMap", parametersForLog, "500", ex.Message, ex.StackTrace);
                }
            }

            if (seatmaps.Count == 0)
            {
                seatmaps = null;
            }
            LogHelper.writeToLogFile("GetSeatMap", "", "", "", "");
            return objLi.BuildAPIResultXML(null, null, null, null, null, seatmaps, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Get seat map information for transit flight")]
        public APIResult GetSeatMapLayout(string flightId, string originRcd, string destinationRcd, string boardingClass, string bookingClass)
        {
            Mappings objMappings = new Mappings();
            objMappings = (Mappings)Session["Mappings"];

            Library objLi = new Library();
            DataSet ds = new DataSet();
            APIErrors errors = new APIErrors();
            APISeatMaps seatmaps = new APISeatMaps();
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
            string codeMessage = "725"; // No seat map information found
            parametersForLog += originRcd + "|" + destinationRcd + "|" + flightId + "|" + boardingClass + "|" + bookingClass;


            if (string.IsNullOrEmpty(flightId))
            {
                isParameterWellForm = false;
                GetAPIErrors("720", errors);
                LogHelper.writeToLogFile("GetSeatMapLayout", parametersForLog, "720", "Invalid flightId", string.Empty);
            }
            else if (string.IsNullOrEmpty(originRcd))
            {
                isParameterWellForm = false;
                GetAPIErrors("721", errors);
                LogHelper.writeToLogFile("GetSeatMapLayout", parametersForLog, "721", "Invalid originRcd", string.Empty);
            }
            else if (string.IsNullOrEmpty(destinationRcd))
            {
                isParameterWellForm = false;
                GetAPIErrors("722", errors);
                LogHelper.writeToLogFile("GetSeatMapLayout", parametersForLog, "722", "Invalid destinationRcd", string.Empty);
            }
            else if (string.IsNullOrEmpty(boardingClass))
            {
                isParameterWellForm = false;
                GetAPIErrors("723", errors);
                LogHelper.writeToLogFile("GetSeatMapLayout", parametersForLog, "723", "Invalid boardingClass", string.Empty);
            }

            string configuration = string.Empty;

            if (isParameterWellForm)
            {
                try
                {
                    if (objMappings != null && objMappings.Count > 0)
                    {
                        ServiceClient objClient = new ServiceClient();
                        objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];

                        ds = objClient.GetSeatMapLayout(flightId.ToUpper(), originRcd, destinationRcd, boardingClass, configuration, tikAEROWebCheckinAPI.Classes.Language.CurrentCode());

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ds.DataSetName = "SeatMapLayout";
                            GetAPISeatMaps(ds.Tables[0].Rows[0]["flight_id"].ToString(), ds.GetXml(), seatmaps);
                            codeMessage = "000";
                        }
                        else
                        {
                            codeMessage = "725";
                        }

                        GetAPIErrors(codeMessage, errors);

                        if (codeMessage != "000")
                        {
                            LogHelper.writeToLogFile("SeatMapLayout", parametersForLog, "725", "No seat map information found", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("701", errors);
                        LogHelper.writeToLogFile("SeatMapLayout", parametersForLog, "701", "objMappings is null", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    GetAPIErrors("700", errors);
                    LogHelper.writeToLogFile("SeatMapLayout", parametersForLog, "700", ex.Message, ex.StackTrace);
                }
            }

            if (seatmaps.Count == 0)
            {
                seatmaps = null;
            }

            LogHelper.writeToLogFile("SeatMapLayout", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, seatmaps, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Manually seat select")]
        public APIResult SelectSeat(SeatRequests seat_request)
        {
            Mappings objSaveMapping = new Mappings();

            Mappings objMappings = new Mappings();
            objMappings = (Mappings)Session["Mappings"];


            string strCheckInFlightSession = string.Empty;
            string strGetPassengerDetailXML = string.Empty;
            
            APIErrors errors = new APIErrors();
            APIMessageResults message_results = new APIMessageResults();
            Library objLi = new Library();
            
            bool isParameterWellForm = true;
            string parametersForLog = string.Empty;
          //  bool isCalculateSeatFee = true;
            bool bSuccess = true;
            bool isIncludeOFFLOADED = false;
            bool isAllowed = false;
            bool isIncludeSeatAssigned = false;
            bool isContainAdult = false;
            int iAdult = 0;
            int iChild = 0;
            int iInfant = 0;
            bool isSeatMatchInfant = false;
            bool isFreeSeating = false;
            bool isDuplicateSeat = false;
            bool PreSeat = true;

            for(int i=0; i< seat_request.Count;i++)
            {
                parametersForLog += seat_request[i].BookingSegmentId + "," + seat_request[i].PassengerId + "," + seat_request[i].SeatRow + "," + seat_request[i].SeatColumn + "\n";
            }

            if (seat_request == null || seat_request.Count == 0)
            {
                isParameterWellForm = false;
                GetAPIErrors("810", errors);
                LogHelper.writeToLogFile("SelectSeat", parametersForLog, "810", "SeatRequest is empty", string.Empty);
            }
            
            if (isParameterWellForm)
            {
                try
                {
                    if (objMappings != null && objMappings.Count > 0)//802
                    {
                            //check passenger type & OFFLOADED for seat assign from web config
                            string strPassengerTypeBlock = string.Empty;
                            bool isSeatOFFLOADEDAllowed = false;
                            bool isAllowCHDCheckinAlone = false;

                            if (System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"] != null)
                            {
                                strPassengerTypeBlock = System.Configuration.ConfigurationManager.AppSettings["SeatPassengerTypeBlock"].ToUpper();
                            }
                            if (System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"] != null)
                            {
                                isSeatOFFLOADEDAllowed = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"]);
                            }
                            if (System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"] != null)
                            {
                                isAllowCHDCheckinAlone = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isAllowCHDCheckinAlone"]);
                            }


                        if (seat_request != null && seat_request.Count > 0)
                        {
                            for (int i = 0; i < seat_request.Count; i++)
                            {
                                if (seat_request[i].BookingSegmentId.Equals(Guid.Empty))
                                {
                                    GetAPIErrors("811", errors);
                                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "811", "One of the request supply an invalid BookingSegmentId", string.Empty);
                                    bSuccess = false;
                                    break;
                                }
                                else if (seat_request[i].PassengerId.Equals(Guid.Empty))
                                {
                                    GetAPIErrors("812", errors);
                                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "812", "One of the request supply an invalid PassengerId", string.Empty);
                                    bSuccess = false;
                                    break;
                                }
                            }
                        }

                        // Validation blocked seat

                        // 1.find var to get seatmap
                        APIFlightSegments objFss = new APIFlightSegments();
                        objFss = (APIFlightSegments)Session["FlightSegment"];

                        string flightId = string.Empty;
                        string originRcd = string.Empty;
                        string destinationRcd = string.Empty;
                        string boardingClass = string.Empty;

                        bool bIsValidRequest = false;
                        bSuccess = false;

                        for (int i = 0; i < seat_request.Count; i++)
                        {
                            foreach (APIFlightSegment f in objFss)
                            {
                                if (seat_request[i].BookingSegmentId == f.booking_segment_id)
                                {
                                    flightId = f.flight_id.ToString();
                                    originRcd = f.origin_rcd;
                                    destinationRcd = f.destination_rcd;

                                    if(!string.IsNullOrEmpty(f.boarding_class_rcd))
                                        boardingClass = f.boarding_class_rcd;
                                    else
                                        boardingClass = "Y";

                                    bIsValidRequest = true;
                                    break;
                                    
                                }
                            }

                            if(bIsValidRequest == true)
                                break;

                        }

                        //2.get seatmap
                        if (bIsValidRequest)
                        {
                            ServiceClient objClient = new ServiceClient();
                            objClient.objService = (TikAeroXMLwebservice)Session["AgentService"];

                            DataSet ds = objClient.GetSeatMapLayout(flightId, originRcd, destinationRcd, boardingClass, string.Empty, "EN");

                            // 3.find blocked flag
                            for (int i = 0; i < seat_request.Count; i++)
                            {
                                string strFilter = "blocked_flag = 1 AND seat_row =" + seat_request[i].SeatRow + " AND seat_column = '" + seat_request[i].SeatColumn + "'";

                                var dv = ds.Tables[0].DefaultView;
                                dv.RowFilter = strFilter;

                                int intBlockedFlag = dv.Count;

                                if (intBlockedFlag > 0)
                                {
                                    GetAPIErrors("813", errors);
                                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "813", "Blocked seat can not be assigned.", string.Empty);
                                    bSuccess = false;
                                    break;
                                }
                                else
                                {
                                    bSuccess = true;
                                }
                            }
                        }

                       //When all validation pass do the seat assignment.
                       if (bSuccess == true)
                        {
                            //sort request by passenger_type_rcd
                            string adultPassengerId = string.Empty;
                            string childPassengerId = string.Empty;
                            string infantPassengerId = string.Empty;
                            SeatRequests seats_sort = new SeatRequests();
                            foreach (Mapping m in objMappings)
                            {
                                switch (m.passenger_type_rcd.ToUpper())
                                {
                                    case "CHD":
                                        childPassengerId += m.passenger_id + "|";
                                        break;
                                    case "INF":
                                        infantPassengerId += m.passenger_id + "|";
                                        break;
                                    default:
                                        adultPassengerId += m.passenger_id + "|";
                                        break;
                                }
                            }

                            string[] adultIds = adultPassengerId.Split('|');
                            string[] childIds = childPassengerId.Split('|');
                            string[] infantIds = infantPassengerId.Split('|');

                            if (adultIds.Length > 0)
                            {
                                for (int a = 0; a < adultIds.Length; a++)
                                {
                                    if (!string.IsNullOrEmpty(adultIds[a]))
                                    {
                                        for (int aa = 0; aa < seat_request.Count; aa++)
                                        {
                                            if (seat_request[aa].PassengerId.ToString() == adultIds[a])
                                            {
                                                iAdult++;
                                                isContainAdult = true;
                                                isDuplicateSeat = false;
                                                seats_sort.Add(seat_request[aa]);
                                            }
                                        }
                                    }
                                }

                                for (int b = 0; b < childIds.Length; b++)
                                {
                                    if (!string.IsNullOrEmpty(childIds[b]))
                                    {
                                        for (int bb = 0; bb < seat_request.Count; bb++)
                                        {
                                            if (seat_request[bb].PassengerId.ToString() == childIds[b])
                                            {
                                                iChild++;
                                                seats_sort.Add(seat_request[bb]);
                                            }
                                        }
                                    }
                                }

                                for (int c = 0; c < infantIds.Length; c++)
                                {
                                    if (!string.IsNullOrEmpty(infantIds[c]))
                                    {
                                        for (int cc = 0; cc < seat_request.Count; cc++)
                                        {
                                            if (seat_request[cc].PassengerId.ToString() == infantIds[c])
                                            {
                                                iInfant++;
                                                seats_sort.Add(seat_request[cc]);
                                            }
                                        }
                                    }
                                }
                            }

                            //find mapping that filled seat already
                            Mappings objMappingFillSeat = new Mappings();

                            for (int i = 0; i < seats_sort.Count; i++)
                            {
                                for (int j = 0; j < objMappings.Count; j++)
                                {
                                    if (seats_sort[i].PassengerId.Equals(objMappings[j].passenger_id) &&
                                       seats_sort[i].BookingSegmentId.Equals(objMappings[j].booking_segment_id)
                                       )
                                    {
                                        objMappingFillSeat.Add(objMappings[j]);
                                    }
                                }
                            }

                            #region Fill Seat Information to Mapping
                            //Fill Seat Information.
                            for (int i = 0; i < seats_sort.Count; i++)
                            {
                                for (int j = 0; j < objMappingFillSeat.Count; j++)
                                {
                                    if (seats_sort[i].PassengerId.Equals(objMappingFillSeat[j].passenger_id) &&
                                        seats_sort[i].BookingSegmentId.Equals(objMappingFillSeat[j].booking_segment_id)
                                        && !strPassengerTypeBlock.Contains(objMappingFillSeat[j].passenger_type_rcd.ToUpper()) && bSuccess)
                                    {
                                        if (objMappingFillSeat[j].e_ticket_flag == 1 && objMappingFillSeat[j].ticket_number.Length > 0)
                                        {
                                            if (objMappingFillSeat[j].passenger_check_in_status_rcd == null || objMappingFillSeat[j].passenger_check_in_status_rcd.Length == 0)
                                            {
                                                isAllowed = true;
                                                isSeatMatchInfant = false;
                                            }

                                            //05-03
                                            if (objMappingFillSeat[j].passenger_check_in_status_rcd == "OFFLOADED")
                                            {
                                                if (!isSeatOFFLOADEDAllowed)
                                                {
                                                    isIncludeOFFLOADED = true;
                                                }
                                                else
                                                {
                                                    isAllowed = true;
                                                }
                                            }

                                            //yo 10-02
                                            if (!string.IsNullOrEmpty(objMappingFillSeat[j].seat_number))
                                            {
                                                isIncludeSeatAssigned = true;
                                            }

                                            if (objMappingFillSeat[j].free_seating_flag == 1)
                                            {
                                                isFreeSeating = true;
                                            }
                                        }

                                        if (isIncludeOFFLOADED)//!isAllowed
                                        {
                                            GetAPIErrors("805", errors); //OFFLOADED passenger can't checkin
                                            LogHelper.writeToLogFile("SelectSeat", parametersForLog, "805", "OFFLOADED passenger can't checkin", string.Empty);
                                            bSuccess = false;
                                        }
                                        else if (isFreeSeating)
                                        {
                                            GetAPIErrors("807", errors); //This flight is free seating
                                            LogHelper.writeToLogFile("SelectSeat", parametersForLog, "807", "This flight is free seating", string.Empty);
                                            bSuccess = false;
                                        }
                                        else if (isIncludeSeatAssigned)
                                        {
                                            GetAPIErrors("806", errors); //Passengers already have been assigned seat
                                            LogHelper.writeToLogFile("SelectSeat", parametersForLog, "806", "Passengers already have been assigned seat", string.Empty);
                                            bSuccess = false;
                                        }
                                        else if (!isContainAdult && !isAllowCHDCheckinAlone)
                                        {
                                            GetAPIErrors("803", errors); //Child or infant can't assign seat independently
                                            LogHelper.writeToLogFile("SelectSeat", parametersForLog, "803", "Child or infant can't assign seat independently", string.Empty);
                                            bSuccess = false;
                                        }
                                        else if (iInfant > iAdult)
                                        {
                                            GetAPIErrors("804", errors); //Number of adults need to be higher than the number of infants
                                            LogHelper.writeToLogFile("SelectSeat", parametersForLog, "804", "Number of adults need to be higher than the number of infants", string.Empty);
                                            bSuccess = false;
                                        }
                                        else if (objMappingFillSeat[j].passenger_type_rcd.ToUpper() == "INF")
                                        {
                                            //check infant seat match adult seat
                                            foreach (Mapping m in objMappingFillSeat)
                                            {
                                                string reqSeatNumber = seats_sort[j].SeatRow + seats_sort[j].SeatColumn;
                                                if (m.passenger_type_rcd.ToUpper() == "ADULT" && m.seat_number == reqSeatNumber)
                                                {
                                                    //found seat for infant
                                                    isSeatMatchInfant = true;
                                                }
                                            }

                                            if (!isSeatMatchInfant)
                                            {
                                                GetAPIErrors("801", errors); //Infant's seat does not match adult's seat
                                                LogHelper.writeToLogFile("SelectSeat", parametersForLog, "801", "Infant's seat is not match with adult's seat", string.Empty);
                                                bSuccess = false;
                                            }
                                            else
                                            {
                                                //start select seat
                                                if (objSaveMapping.Count > 0)
                                                {
                                                    foreach (Mapping map in objSaveMapping)
                                                    {
                                                        if ((map.seat_number == seats_sort[i].SeatRow.ToString() + seats_sort[i].SeatColumn) && (map.passenger_type_rcd.ToUpper() == "INF"))
                                                        {
                                                            isDuplicateSeat = true;
                                                        }
                                                    }
                                                }

                                                if (!isDuplicateSeat)
                                                {
                                                    objMappingFillSeat[j].seat_column = seats_sort[i].SeatColumn;
                                                    objMappingFillSeat[j].seat_row = seats_sort[i].SeatRow;

                                                    if (seat_request[i].SeatRow == 0 && string.IsNullOrEmpty(seats_sort[i].SeatColumn))
                                                    {
                                                        objMappingFillSeat[j].seat_number = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        objMappingFillSeat[j].seat_number = seats_sort[i].SeatRow.ToString() + seats_sort[i].SeatColumn;
                                                    }
                                                    objMappingFillSeat[j].seat_fee_rcd = seats_sort[i].SeatFeeRcd;

                                                    objSaveMapping.Add(objMappingFillSeat[j]);
                                                }
                                                else
                                                {
                                                    //duplicate seat
                                                    GetAPIErrors("809", errors); //Duplicate seat selected
                                                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "809", "Duplicate seat selected", string.Empty);
                                                    bSuccess = false;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //start select seat adult
                                            if (objSaveMapping.Count > 0)
                                            {
                                                foreach (Mapping map in objSaveMapping)
                                                {
                                                    if (map.seat_number == seats_sort[i].SeatRow.ToString() + seats_sort[i].SeatColumn)
                                                    {
                                                        isDuplicateSeat = true;
                                                    }
                                                }
                                            }

                                            if (!isDuplicateSeat)
                                            {
                                                objMappingFillSeat[j].seat_column = seats_sort[i].SeatColumn;
                                                objMappingFillSeat[j].seat_row = seats_sort[i].SeatRow;

                                                if (seat_request[i].SeatRow == 0 && string.IsNullOrEmpty(seats_sort[i].SeatColumn))
                                                {
                                                    objMappingFillSeat[j].seat_number = string.Empty;
                                                }
                                                else
                                                {
                                                    objMappingFillSeat[j].seat_number = seats_sort[i].SeatRow.ToString() + seats_sort[i].SeatColumn;
                                                }

                                                objMappingFillSeat[j].seat_fee_rcd = seats_sort[i].SeatFeeRcd;

                                                //utydev-187
                                                // pre seat work well NO need config
                                                //if (System.Configuration.ConfigurationManager.AppSettings["PreSeat"] != null)
                                                //{
                                                //    PreSeat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PreSeat"]);
                                                //}


                                                if (PreSeat)
                                                {
                                                    bool seatItsAvailable = SaveAutoSeat(objMappingFillSeat[j].flight_id.ToString(), objMappingFillSeat[j].passenger_id.ToString(), objMappingFillSeat[j].passenger_type_rcd, objMappingFillSeat[j].seat_number);

                                                    if (seatItsAvailable == false)
                                                    {
                                                        //found SeatDup via check from DB

                                                        objMappingFillSeat[j].seat_number = string.Empty;
                                                        objMappingFillSeat[j].seat_row = 0;
                                                        objMappingFillSeat[j].seat_column = string.Empty;

                                                        GetAPIErrors("814", errors); //Duplicate seat selected
                                                        LogHelper.writeToLogFile("SelectSeat", parametersForLog, "814", "please choose another one.", string.Empty);
                                                        bSuccess = false;

                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //pre seat ok then seatItsAvailable == true
                                                        objSaveMapping.Add(objMappingFillSeat[j]);
                                                    }
                                                }
                                                else
                                                {
                                                    // proces select seat without pre seat
                                                    objSaveMapping.Add(objMappingFillSeat[j]);
                                                }
                                            }
                                            else
                                            {
                                                //duplicate seat
                                                GetAPIErrors("809", errors); //Duplicate seat selected
                                                LogHelper.writeToLogFile("SelectSeat", parametersForLog, "809", "Duplicate seat selected", string.Empty);
                                                bSuccess = false;
                                            }

                                        }                                       
                                    }
                                    else { }

                                }
                            }
                            #endregion

                            if (bSuccess)
                            {
                                if (objSaveMapping.Count == 0)
                                {
                                    GetAPIErrors("808", errors); //Passengers do not match booking_segment_id
                                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "807", "Passengers do not match booking_segment_id", string.Empty);
                                    bSuccess = false;
                                }
                                else
                                {
                                    Session["SaveMapping"] = objSaveMapping;
                                }
                            }
                        }                           
                    }
                    else
                    {
                        GetAPIErrors("802", errors);
                        LogHelper.writeToLogFile("SelectSeat", parametersForLog, "802", "Session[Mappings] is null", string.Empty);
                        bSuccess = false;
                    }

                    //If failed write to log file.
                    if (bSuccess == true)
                    {
                        APIMessageResult message_result = new APIMessageResult();
                        message_result.message_result = "Process complete";
                        message_results.Add(message_result);
                        GetAPIErrors("000", errors);                     
                    }

                }
                catch (Exception ex)
                {
                    //clear seat from DB //utydev-187
                    if (PreSeat)
                    {
                        foreach (Mapping m in objSaveMapping)
                        {
                            if (m.passenger_type_rcd != "INF")
                                RemoveAutoSeat(m.passenger_id.ToString());
                        }
                    }

                    GetAPIErrors("800", errors);
                    LogHelper.writeToLogFile("SelectSeat", parametersForLog, "800", ex.Message, ex.StackTrace);
                    bSuccess = false;
                }
                finally
                {
                }

            }

            if (message_results.Count == 0)
            {
                message_results = null;
            }

            LogHelper.writeToLogFile("SelectSeat", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, message_results, errors);
        }

        [WebMethod(EnableSession = true, Description = "Add additional passenger address to support PNRGOV message")]
        public APIResult AddPassengerAddress(string strPassengerId, 
            string strBookingSegmentId, string strPassengerProfileId, 
             string addressTypeRcd, string passengerAddress, 
            string state, string city,
             string zipCode, string countryRcd)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool result = false;


            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    if (string.IsNullOrEmpty(strPassengerId))
                    {
                        GetAPIErrors("312", errors);
                        LogHelper.writeToLogFile("AddPassengerAddress", "", "312", "strPassengerId is empty", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(strBookingSegmentId))
                    {
                        GetAPIErrors("210", errors);
                        LogHelper.writeToLogFile("AddPassengerAddress", "", "210", "strBookingSegmentId is empty", string.Empty);
                    }
                    else
                    {

                        result = InsertPassengerAddress(new Guid(strPassengerId), new Guid(strBookingSegmentId), new Guid(strPassengerProfileId)
                                , addressTypeRcd, passengerAddress, state, city, zipCode, countryRcd);

                        if (result == true)
                        {
                            GetAPIErrors("000", errors);
                        }
                        else
                        {
                            GetAPIErrors("", errors);
                        }
                    }

                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("AddPassengerAddress", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("AddPassengerAddress", "", "500", ex.Message, ex.StackTrace);
            }

            LogHelper.writeToLogFile("AddPassengerAddress", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Update additional passenger address to support PNRGOV message")]
        public APIResult UpdatePassengerAddress(string strPassengerAddressId, string passengerAddress, 
            string state, string city,
             string zipCode, string countryRcd)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool result = false;
            //int iResult = 0;
            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    result = UpdatePassengerAddress(new Guid(strPassengerAddressId),
                    passengerAddress, state, city, zipCode, countryRcd);

                    if (result == true)
                    {
                        GetAPIErrors("000", errors);
                    }
                    else
                    {
                        GetAPIErrors("", errors);
                    }

                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("UpdatePassengerAddress", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("UpdatePassengerAddress", "", "500", ex.Message, ex.StackTrace);
            }

            LogHelper.writeToLogFile("UpdatePassengerAddress", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Delete additional passenger address to support PNRGOV message")]
        public APIResult DeletePassengerAddress(string strPassengerAddressId)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool result = false;
            //int iResult = 0;
            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    result = DeletePassengerAddress(new Guid(strPassengerAddressId));

                    if (result == true)
                    {
                        GetAPIErrors("000", errors);
                    }
                    else
                    {
                        GetAPIErrors("", errors);
                    }

                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("DeletePassengerAddress", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("DeletePassengerAddress", "", "500", ex.Message, ex.StackTrace);
            }
            LogHelper.writeToLogFile("DeletePassengerAddress", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Add additional passenger document to support PNRGOV message")]
        public APIResult AddPassengerDocument(string strPassengerId, string strBookingSegmentId, 
            string documentTypeRcd,
             string documentNumber, 
            DateTime documentIssueDate, 
            DateTime documentExpiryDate, 
            string issuePlace, 
            string issueCountry, 
            string birthPlace, 
            string nationalityRcd, 
            string genderTypeRcd)
            
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
           // DateTime issueDate = new DateTime();
            bool result = false;

            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    result = InsertPassengerDocument(
                        new Guid(strPassengerId), 
                        new Guid(strBookingSegmentId), 
                        documentTypeRcd,
                        documentNumber,
                        documentIssueDate, 
                        documentExpiryDate, 
                        issuePlace, 
                        issueCountry,
                        birthPlace, 
                        nationalityRcd, 
                        genderTypeRcd);

                    if (result == true)
                    {
                        GetAPIErrors("000", errors);
                    }
                    else
                    {
                        GetAPIErrors("", errors);
                    }


                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("AddPassengerDocument", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("AddPassengerDocument", "", "500", ex.Message, ex.StackTrace);
            }
            LogHelper.writeToLogFile("AddPassengerDocument", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Update additional passenger document to support PNRGOV message")]
        public APIResult UpdatePassengerDocument(string strDocumentId, string documentTypeRcd,
             string documentNumber, DateTime documentIssueDate, DateTime documentExpiryDate, 
             string issuePlace, string issueCountry, string birthPlace, string nationalityRcd, string genderTypeRcd)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool result = false;
            //int iResult = 0;
            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    result = UpdatePassengerDocument(
                        new Guid(strDocumentId),
                        documentTypeRcd,
                        documentNumber,
                        documentIssueDate,
                        documentExpiryDate,
                        issuePlace,
                        issueCountry,
                        birthPlace,
                        nationalityRcd,
                        genderTypeRcd);

                    if (result == true)
                    {
                        GetAPIErrors("000", errors);
                    }
                    else
                    {
                        GetAPIErrors("", errors);
                    }


                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("UpdatePassengerDocument", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("UpdatePassengerDocument", "", "500", ex.Message, ex.StackTrace);
            }
            LogHelper.writeToLogFile("UpdatePassengerDocument", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }


        [WebMethod(EnableSession = true, Description = "Delete additional passenger document to support PNRGOV message")]
        public APIResult DeletePassengerDocument(string strDocumentId)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();

            bool result = false;
            //int iResult = 0;
            try
            {
                if (Session["CheckInFlight"] != null)
                {
                    result = DeletePassengerDocument(new Guid(strDocumentId));
                    if (result == true)
                    {
                        GetAPIErrors("000", errors);
                    }
                    else
                    {
                        GetAPIErrors("", errors);
                    }

                }
                else
                {
                    GetAPIErrors("957", errors);
                    LogHelper.writeToLogFile("DeletePassengerDocument", "Session[CheckInFlight]", "501", "Session[CheckInFlight] is null", string.Empty);
                }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("DeletePassengerDocument", "", "500", ex.Message, ex.StackTrace);
            }
            LogHelper.writeToLogFile("DeletePassengerDocument", "", "", "", "");

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }


        [WebMethod(EnableSession = true, Description = "Set Flight CheckIn Status")]
        public APIResult SetFlightCheckInStatus(string strFlightId, string strOrigin,
                                                string strDestination, string departureDate,
                                                string airline_rcd, string flight_number,
                                                string status, string allow_web_checkin,
                                                string planned_departure_time, string planned_arrival_time,
                                                string estimated_departure_time, string estimated_arrival_time,
                                                string actual_departure_time, string actual_arrival_time, 
                                                string boarding_gate, string arrival_gate, string strToken)

        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            String hky = String.Empty;
            Helper hlper = new Helper();
            Users users = new Users();
            string user_id = String.Empty;
            String temp = string.Empty;
            DateTime dtDepartureDate = new DateTime();
            bool result = false;
            Token token = new Token();
            string parametersForLog = string.Empty;

            parametersForLog = strFlightId + "," + strOrigin + "," + strDestination + "," + departureDate + ","
                + airline_rcd + "," + flight_number + "," + status + "," + allow_web_checkin + "," + planned_departure_time + "," + planned_arrival_time + "," + boarding_gate + "," + arrival_gate;

            LogHelper.writeToLogFile("SetFlightCheckInStatus", parametersForLog, "", "", "");

            try
            {
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    // get user id
                    user_id = StringCipher.Decrypt(strToken, hky);
                }
                else // new token
                {
                    token = Authentication(strToken);
                    if(token.ResponseCode == "000")
                         user_id = token.UserId;
                }
            }
            catch
            {
                GetAPIErrors("910", errors);
                LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "910", "Invalid token", string.Empty);
            }

            try
            {
                if (hlper.IsValid(user_id))
                {
                    if (string.IsNullOrEmpty(strFlightId))
                    {
                        GetAPIErrors("1005", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "strFlightId is empty", string.Empty);
                    }
                    else if (!hlper.IsValid(strFlightId))
                    {
                        GetAPIErrors("1003", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid strFlightId", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(strOrigin))
                    {
                        GetAPIErrors("1006", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "strOrigin is empty", string.Empty);
                    }
                    else if (strOrigin.Trim().Length == 0)
                    {
                        GetAPIErrors("1006", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "strOrigin is empty", string.Empty);
                    }
                    else if (strOrigin.Trim().Length > 4)
                    {
                        GetAPIErrors("1015", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "Invalid code.", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(strDestination))
                    {
                        GetAPIErrors("1007", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "strDestination is empty", string.Empty);
                    }
                    else if (strDestination.Trim().Length == 0)
                    {
                        GetAPIErrors("1007", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "strDestination is empty", string.Empty);
                    }
                    else if (strDestination.Trim().Length > 4)
                    {
                        GetAPIErrors("1015", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "Invalid code.", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(airline_rcd))
                    {
                        GetAPIErrors("1008", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "airline_rcd is empty", string.Empty);
                    }
                    else if (airline_rcd.Trim().Length == 0)
                    {
                        GetAPIErrors("1008", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "airline_rcd is empty", string.Empty);
                    }
                    else if (airline_rcd.Trim().Length > 4)
                    {
                        GetAPIErrors("1015", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "Invalid code.", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(flight_number))
                    {
                        GetAPIErrors("1009", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "flight_number is empty", string.Empty);
                    }
                    else if (flight_number.Trim().Length == 0)
                    {
                        GetAPIErrors("1009", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "flight_number is empty", string.Empty);
                    }
                    else if (flight_number.Trim().Length > 5)
                    {
                        GetAPIErrors("1015", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "Invalid code.", string.Empty);
                    }
                    else if (string.IsNullOrEmpty(departureDate))
                    {
                        GetAPIErrors("1010", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "departure Date is empty", string.Empty);
                    }
                    else if (!IsValidDateTime(departureDate))
                    {
                        GetAPIErrors("1013", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "1013", "Invalid datetime", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(planned_departure_time) && !IsValidTime(planned_departure_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(planned_arrival_time) && !IsValidTime(planned_arrival_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(estimated_departure_time) && !IsValidTime(estimated_departure_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(estimated_arrival_time) && !IsValidTime(estimated_arrival_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(actual_departure_time) && !IsValidTime(actual_departure_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if (!string.IsNullOrEmpty(actual_arrival_time) && !IsValidTime(actual_arrival_time))
                    {
                        GetAPIErrors("1011", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid time", string.Empty);
                    }
                    else if ((!string.IsNullOrEmpty(allow_web_checkin) && allow_web_checkin.ToUpper() != "TRUE") && (!string.IsNullOrEmpty(allow_web_checkin) && allow_web_checkin.ToUpper() != "FALSE"))
                    {
                        GetAPIErrors("1012", errors);
                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "invalid request format", string.Empty);
                    }
                    else
                    {
                        // valid flight id format guid
                        Guid guidOutput;
                        if (!TryParseGuid(strFlightId, out guidOutput))
                        {
                            GetAPIErrors("1003", errors);
                            LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "flight id is invalid format", string.Empty);
                        }
                        else
                        {
                            // valid date
                            DateTime dtTemp;
                            if (DateTime.TryParse(departureDate, out dtTemp))
                            {
                                if (IsValidDateTime(departureDate))
                                {
                                    dtDepartureDate = DateTime.ParseExact(departureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    // valid status 0=NULL, 1=OPEN, 2=CLOSED, 3=DISPATCHED,4=FLOWN,5=RESETFLOWN
                                    if (string.IsNullOrEmpty(status) || status == "0" || status == "1" || status == "2" || status == "3" || status == "4" || status == "5")
                                    {
                                        //prepare gate
                                        string new_boarding_gate = boarding_gate;
                                        string new_arrival_gate = arrival_gate;

                                        //prepare flight_check_in_status_rcd
                                        string new_flight_check_in_status_rcd = CorrectFlightCheckInStatus(status);

                                        //prepare allow_web_checkin
                                        string new_allow_web_checkin = GetAllowWebCheckinFlag(allow_web_checkin);

                                        // prepare flight time
                                        string new_planned_departure_time = string.Empty;
                                        string new_planned_arrival_time = string.Empty;
                                        string new_estimated_departure_time = string.Empty;
                                        string new_estimated_arrival_time = string.Empty;
                                        string new_actual_departure_time = string.Empty;
                                        string new_actual_arrival_time = string.Empty;

                                        GetNewFlightTime(planned_departure_time, planned_arrival_time, estimated_departure_time, estimated_arrival_time, actual_departure_time, actual_arrival_time,
                                            out new_planned_departure_time, out new_planned_arrival_time, out new_estimated_departure_time, out new_estimated_arrival_time, out new_actual_departure_time, out new_actual_arrival_time);

                                        //for find original data
                                        string original_flight_check_in_status_rcd = string.Empty;
                                        int original_planned_departure_time = 0;
                                        int original_planned_arrival_time = 0;

                                        int original_estimated_departure_time = 0;
                                        int original_estimated_arrival_time = 0;

                                        int original_actual_departure_time = 0;
                                        int original_actual_arrival_time = 0;


                                        string original_boarding_gate = string.Empty;
                                        string original_arrival_gate = string.Empty;

                                        //verify and get original data
                                        APIError dBVerifyError = VerifyExistingFlight(new Guid(strFlightId), strOrigin, strDestination, flight_number, dtDepartureDate,
                                            new_flight_check_in_status_rcd, out original_flight_check_in_status_rcd,
                                            out original_planned_departure_time, out original_planned_arrival_time,
                                            out original_estimated_departure_time, out original_estimated_arrival_time,
                                            out original_actual_departure_time, out original_actual_arrival_time,
                                            out original_boarding_gate, out original_arrival_gate);

                                        // verify status should correct compare with the old one.

                                        if (dBVerifyError.code == "000")
                                        {
                                            //define change flag
                                            string flight_status_changed_flag = "0";
                                            string time_changed_flag = "0";
                                            string gate_changed_flag = "0";

                                            string WhatChange = GetWhatIsChange(original_flight_check_in_status_rcd, new_flight_check_in_status_rcd,
                                                original_planned_departure_time, new_planned_departure_time,
                                                original_planned_arrival_time, new_planned_arrival_time,

                                                original_estimated_departure_time, new_estimated_departure_time,
                                                original_estimated_arrival_time, new_estimated_arrival_time,

                                                original_actual_departure_time, new_actual_departure_time,
                                                original_actual_arrival_time, new_actual_arrival_time,

                                                original_boarding_gate, new_boarding_gate,
                                                original_arrival_gate, new_arrival_gate);

                                            // find chage flag for insert flight_change
                                            if (WhatChange.Contains("status"))
                                                flight_status_changed_flag = "1";
                                            if (WhatChange.Contains("time"))
                                                time_changed_flag = "1";
                                            if (WhatChange.Contains("gate"))
                                                gate_changed_flag = "1";

                                            // processs to update
                                            result = SetFlightCheckInStatusToDB(new Guid(strFlightId), strOrigin, strDestination, dtDepartureDate, airline_rcd, flight_number, user_id,
                                                new_flight_check_in_status_rcd, original_flight_check_in_status_rcd, new_allow_web_checkin,
                                                new_planned_departure_time, new_planned_arrival_time,
                                                new_estimated_departure_time, new_estimated_arrival_time,
                                                new_actual_departure_time, new_actual_arrival_time,
                                                new_boarding_gate, new_arrival_gate,
                                                flight_status_changed_flag, time_changed_flag, gate_changed_flag);

                                            if (result == true)
                                            {
                                                GetAPIErrors("000", errors);
                                            }
                                            else
                                            {
                                                GetAPIErrors("1002", errors);
                                            }
                                        }
                                        else
                                        {
                                            errors.Add(dBVerifyError);
                                            // GetAPIErrors("1004", errors);
                                            LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", dBVerifyError.message, string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        GetAPIErrors("1001", errors);
                                        LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "", "status is invalid", string.Empty);
                                    }
                                }
                                else
                                {
                                    GetAPIErrors("1013", errors);
                                    LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "1013", "Invalid datetime", string.Empty);
                                }
                            }
                            else
                            {
                                GetAPIErrors("1013", errors);
                                LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "1013", "Invalid datetime", string.Empty);
                            }

                        }

                    }
                }
                else
                {
                    GetAPIErrors(token.ResponseCode, errors);
                    LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "910", "Invalid token", string.Empty);
                }
                
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "500", ex.Message, ex.StackTrace);
            }

            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        //set flight status
        [WebMethod(EnableSession = true, Description = "Set Flight Status")]
        public APIResult SetFlightStatus(string flight_id, string flight_status, string token)
        {
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            String hky = String.Empty;
            Helper hlper = new Helper();
            Users users = new Users();
            string user_id = string.Empty;
            String temp = string.Empty;
           // DateTime dtDepartureDate = new DateTime();
            bool result = false;
            Token newtoken = new Token();

            string parametersForLog = flight_id + "|" + flight_status  ;


            try
            {
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    // get user id
                    user_id = StringCipher.Decrypt(token, hky);
                }
                else // new token
                {
                    newtoken = Authentication(token);
                    user_id = newtoken.UserId;
                }
            }
            catch
            {
                GetAPIErrors("910", errors);
                LogHelper.writeToLogFile("SetFlightCheckInStatus", "", "910", "Invalid token", string.Empty);
            }

            try
            {
                    if (hlper.IsValid(user_id))
                    {
                        if (string.IsNullOrEmpty(flight_id))
                        {
                            GetAPIErrors("1005", errors);
                            LogHelper.writeToLogFile("SetFlightStatus", "", "", "flight_id is empty", string.Empty);
                        }
                        else
                        {
                            // valid flight id format guid
                            Guid guidOutput;
                            if (!TryParseGuid(flight_id, out guidOutput))
                            {
                                GetAPIErrors("1003", errors);
                                LogHelper.writeToLogFile("SetFlightStatus", "", "", "flight id is invalid format", string.Empty);
                            }
                            else
                            {
                                //1=ACTIVE,2=INACTIVE,3=DEPARTED,4=CANCELLED
                                if (flight_status == "1" || flight_status == "2" || flight_status == "3" || flight_status == "4")
                                {
                                    if (GetFlightById(new Guid(flight_id)))
                                    {
                                        result = SetFlightStatusToDB(new Guid(flight_id), flight_status, user_id);

                                        if (result == true)
                                        {
                                            GetAPIErrors("000", errors);
                                        }
                                        else
                                        {
                                            GetAPIErrors("1002", errors);
                                        }

                                    }
                                    else
                                    {
                                        GetAPIErrors("1004", errors);
                                        LogHelper.writeToLogFile("SetFlightStatus", "", "", "Flight id not found", string.Empty);
                                    }
                                }
                                else
                                {
                                    GetAPIErrors("1001", errors);
                                    LogHelper.writeToLogFile("SetFlightStatus", "", "", "status is invalid", string.Empty);
                                }

                            }

                        }
                    }
                    else
                    {
                        GetAPIErrors(newtoken.ResponseCode, errors);
                        LogHelper.writeToLogFile("SetFlightStatus", "", "910", "Invalid token", string.Empty);
                    }
            }
            catch (Exception ex)
            {
                GetAPIErrors("", errors);
                LogHelper.writeToLogFile("SetFlightStatus", "", "500", ex.Message, ex.StackTrace);
            }

            LogHelper.writeToLogFile("SetFlightStatus", "", "", "", "");
            return objLi.BuildAPIResultXML(null, null, null, null, null, null, null, errors);
        }

        #region Baggage
        
        [WebMethod(EnableSession = true, Description = "Requires Token to add a baggage")]
        public APIResult AddBaggage(string BookingSegmentId, string PassengerId, decimal BaggageWeight,
            string FinalDestinationCode, string AirlineCode,string BaggageType, string BaggageStatus, 
            string BaggageWeightUnit, string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction ="AddBaggage";

            string baggage_id = string.Empty;
            bool isParameterWellForm = true;
            string default_error_code = "1100";
            string parametersForLog = string.Empty;
            int res = 0;
            string userid = string.Empty;
            string str_SP_name = "insert_baggage_api";
            Token token = new DataAccessControl.Token();
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand(str_SP_name, conn);

            bool isFlightConnection = false;
            string final_flight_number = string.Empty;

            //keep log request
            //  parametersForLog = BookingSegmentId.ToLower() + "|" + PassengerId.ToLower() + "|" + AirlineCode + "|" + FinalDestinationCode + "|" + BaggageWeight + "|" + BaggageType + "|" + BaggageStatus + "|" + BaggageWeightUnit;
            parametersForLog = (BookingSegmentId ?? string.Empty).ToLower() + " | " +
                    (PassengerId ?? string.Empty).ToLower() + " | " +
                    AirlineCode  + " | " +
                    FinalDestinationCode  + " | " +
                    BaggageWeight  + " | " +
                    BaggageType  + " | " +
                    BaggageStatus  + " | " +
                    BaggageWeightUnit ;

            bool keepLogRequest = false;
            bool KeepLogResponse = false;
            string retrievedBookingId = "";
            string retrievedBookingSegmentId = "";
            string retrievedPassengerId = "";
            string retrievedFlightConnectionId = "";
            bool IsFinalDestinationCode = false;

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            //if (keepLogRequest)
            //{
            //    LogHelper.writeToLogFile(strFunction, "start:"+parametersForLog, "", "Bagg request", string.Empty);
            //}

            // verify data request
            if (string.IsNullOrEmpty(BookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1101", errors);
            }
            else if (!objHelper.IsValid(BookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1101", errors);
            }
            else if (string.IsNullOrEmpty(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1102", errors);
            }
            else if (!objHelper.IsValid(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1102", errors);
            }
            else if (!IsValidString(BaggageType))
            {
                isParameterWellForm = false;
                GetAPIErrors("1104", errors);
            }
            //else if (!IsValidString(BaggageStatus))
            //{
            //    isParameterWellForm = false;
            //    GetAPIErrors("1105", errors);
            //}
            else if (!IsValidString(AirlineCode))
            {
                isParameterWellForm = false;
                GetAPIErrors("1106", errors);
            }
            else if (!ValidBaggageWeight(BaggageWeight))
            {
                isParameterWellForm = false;
                GetAPIErrors("1103", errors);
            }
            //ValidDecimalPlace
            else if (!ValidDecimalPlace(BaggageWeight))
            {
                isParameterWellForm = false;
                GetAPIErrors("1121", errors);
            }
            else if (!ValidBaggageUnit(BaggageWeightUnit))
            {
                isParameterWellForm = false;
                GetAPIErrors("1120", errors);
            }
            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {
                    baggage_id = Guid.NewGuid().ToString();

                    // check connecting fligh
                    //Itinerary itinerary = Session["Itinerary"] as Itinerary;
                    //isFlightConnection = CheckFlightConnection(itinerary);                  

                    if (!string.IsNullOrEmpty(FinalDestinationCode))
                    {
                        if (!IsValidString(FinalDestinationCode))
                        {
                            isParameterWellForm = false;
                            GetAPIErrors("1127", errors);
                            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, null, null, null, errors);
                        }
                        else
                        {
                            IsFinalDestinationCode = true;
                        }
                    }

                   // final_flight_number = FingFinalDestination(itinerary, FinalDestinationCode);


                    //if (itinerary == null )
                    //{
                    //    isParameterWellForm = false;
                    //    GetAPIErrors("957", errors);
                    //    return objLi.BuildAPIResultBagsXML(null, null, null, null, null, null, null, null, errors);
                    //}
                    //valid add bag from sql
                    APIError dBVerifyError = VerifyBaggageCheckin(BookingSegmentId, PassengerId, BaggageStatus, BaggageType, AirlineCode, null, BaggageWeight, FinalDestinationCode, out retrievedBookingId,out retrievedBookingSegmentId,out retrievedPassengerId, out retrievedFlightConnectionId);

                    if (dBVerifyError.code != "000")
                    {
                        isParameterWellForm = false;
                    }


                    if (isParameterWellForm)
                    {
                       
                        try
                        {
                            sqlComm.CommandType = CommandType.StoredProcedure;

                            conn.Open();

                            sqlComm.Parameters.AddWithValue("@baggage_id", baggage_id);
                            sqlComm.Parameters.AddWithValue("@passenger_id", PassengerId);
                            sqlComm.Parameters.AddWithValue("@booking_segment_id", BookingSegmentId);
                            sqlComm.Parameters.AddWithValue("@baggage_tag", null);
                            sqlComm.Parameters.AddWithValue("@create_by", userid);
                            sqlComm.Parameters.AddWithValue("@bagtag_type_rcd", BaggageType.Trim().ToUpper());
                            sqlComm.Parameters.AddWithValue("@airline_rcd", AirlineCode.Trim().ToUpper());
                            sqlComm.Parameters.AddWithValue("@manual_tag_flag", 0);
                            sqlComm.Parameters.AddWithValue("@baggage_weight", BaggageWeight);

                            if(!string.IsNullOrEmpty(BaggageStatus))
                                sqlComm.Parameters.AddWithValue("@bagtag_status_rcd", BaggageStatus.Trim().ToUpper());
                            else
                                sqlComm.Parameters.AddWithValue("@bagtag_status_rcd", "");

                            sqlComm.Parameters.AddWithValue("@baggage_weight_unit", BaggageWeightUnit);

                            // final destination code
                            if (IsFinalDestinationCode)
                            {
                                sqlComm.Parameters.AddWithValue("@final_destination_rcd ", FinalDestinationCode.Trim().ToUpper());
                                sqlComm.Parameters.AddWithValue("@final_airline_rcd", AirlineCode.Trim().ToUpper());
                            }
                            else
                            {
                                sqlComm.Parameters.AddWithValue("@final_destination_rcd ", null);
                            }

                            res = sqlComm.ExecuteNonQuery();

                            if (res == -1 || res >= 0) // success
                            {                              

                                if (IsFinalDestinationCode)
                                {
                                    // get response connecting
                                    GetDirectlyDbAPIPassengerBaggagesConnecting("", PassengerId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, bags);
                                }
                                else
                                {
                                    // get response normal
                                    GetDirectlyDbAPIPassengerBaggages(string.Empty, PassengerId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, bags);

                                }

                                if (bags != null && bags.Count > 0)
                                {
                                    GetAPIErrors("000", errors);

                                    if (KeepLogResponse && bags.Count > 0)
                                    {
                                        StringBuilder bagg = new StringBuilder();
                                        StringBuilder segg = new StringBuilder();
                                        StringBuilder pass = new StringBuilder();

                                        string idd = bags[0].booking_id.ToString(); 

                                        foreach (APIPassengerBaggage bag in bags)
                                        {
                                            if (segg.Length > 0) segg.Append(",");
                                            if (bagg.Length > 0) bagg.Append(",");
                                            if (pass.Length > 0) pass.Append(",");

                                            segg.Append(bag.booking_segment_id);
                                            bagg.Append(bag.baggage_id);
                                            pass.Append(bag.passenger_id);
                                        }

                                        LogHelper.writeToLogFile(strFunction, "", "000",
                                            "addbag Id:" + idd + " | Seg:" + segg.ToString() +
                                            " | Pass:" + pass.ToString() + " | Bag:" + bagg.ToString(), "");
                                    }

                                }
                                else
                                {
                                    GetAPIErrors(default_error_code, errors);
                                    LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code,  "add error", string.Empty);
                                }
                            }
                            else
                            {
                                GetAPIErrors(default_error_code, errors);
                                LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "add error", string.Empty);
                            }

                            sqlComm.Parameters.Clear();
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors("9999", errors);
                            LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, ex.Message, ex.StackTrace);
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }
                    }
                    else //error from DB verify
                    {
                        errors.Add(dBVerifyError);
                        LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "add error", string.Empty);
                    }
                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, "finish id:" + (retrievedBookingId ?? string.Empty).ToLower() + " | "+ parametersForLog, errors[0].code, errors[0].message, string.Empty);
            }

            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, bags, null, null, errors);
        }




        [WebMethod(EnableSession = true, Description = "Requires Token to update a baggage")]
        public APIResult UpdateBaggage(string BaggageId, decimal? BaggageWeight,string BaggageType, string BaggageStatus,string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction = "UpdateBaggage";
            bool isParameterWellForm = true;
            string default_error_code = "1119";
            string parametersForLog = string.Empty;
            int res = 0;
            string userid = string.Empty;
            string str_SP_name = "update_baggage_api";

            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand(str_SP_name, conn);
            Token token = new DataAccessControl.Token();
            bool isFlightConnection = false;

            //keep log request
           // parametersForLog = BaggageId + "|" + BaggageType + "|" + BaggageWeight + "|" + BaggageStatus ;
            parametersForLog = (BaggageId ?? string.Empty).ToLower() + " | " +
                 BaggageWeight + " | " +
                 BaggageType + " | " +
                 BaggageStatus;

            bool keepLogRequest = false;
            bool KeepLogResponse = false;
            string retrievedBookingId = "";
            string retrievedBookingSegmentId = "";
            string retrievedPassengerId = "";
            string retrievedFlightConnectionId = "";

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            //if (keepLogRequest)
            //{
            //    LogHelper.writeToLogFile(strFunction, parametersForLog, "", "log request", string.Empty);
            //}
            // verify data request
            if (string.IsNullOrEmpty(BaggageId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1111", errors);
            }
            else if (!objHelper.IsValid(BaggageId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1111", errors);
            }
            else if (!ValidUpdateBaggageWeight(BaggageWeight))
            {
                isParameterWellForm = false;
                GetAPIErrors("1103", errors);
            }
            //ValidDecimalPlace
            else if (!ValidDecimalPlace(BaggageWeight))
            {
                isParameterWellForm = false;
                GetAPIErrors("1121", errors);
            }
            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {
                    if ((!string.IsNullOrEmpty(BaggageStatus) && IsValidString(BaggageStatus)) || (!string.IsNullOrEmpty(BaggageType) && IsValidString(BaggageType)) || BaggageWeight != null)
                    {
                        decimal valueBaggageWegiht = BaggageWeight ?? 0;

                        //valid add bag from sql
                        APIError dBVerifyError = VerifyBaggageCheckin(null, null, BaggageStatus, BaggageType, string.Empty, BaggageId, valueBaggageWegiht, string.Empty, out retrievedBookingId, out retrievedBookingSegmentId,out retrievedPassengerId,out retrievedFlightConnectionId);

                        if (dBVerifyError.code != "000")
                        {
                            isParameterWellForm = false;
                        }


                        if (isParameterWellForm)
                        {                          
                            //update baggage
                            try
                            {
                                sqlComm.CommandType = CommandType.StoredProcedure;

                                conn.Open();

                                sqlComm.Parameters.AddWithValue("@baggage_id", BaggageId);
                                sqlComm.Parameters.AddWithValue("@create_by", userid);

                                if(!string.IsNullOrEmpty(BaggageType))
                                    sqlComm.Parameters.AddWithValue("@bagtag_type_rcd", BaggageType.Trim().ToUpper());

                                if (valueBaggageWegiht > 0)
                                    sqlComm.Parameters.AddWithValue("@baggage_weight", valueBaggageWegiht);

                                if (!string.IsNullOrEmpty(BaggageStatus))
                                    sqlComm.Parameters.AddWithValue("@bagtag_status_rcd", BaggageStatus.Trim().ToUpper());

                                // flight connecting do not use any more  move sp
                                if (!string.IsNullOrEmpty(retrievedFlightConnectionId) && IsGuid(retrievedFlightConnectionId))
                                {
                                    sqlComm.Parameters.AddWithValue("@is_connecting_flight ", "Y");
                                }
                                else
                                {
                                    sqlComm.Parameters.AddWithValue("@is_connecting_flight", DBNull.Value);
                                }

                                res = sqlComm.ExecuteNonQuery();

                                if (res == -1 || res >= 0)  // success
                                {

                                    if (!string.IsNullOrEmpty(retrievedFlightConnectionId) && IsGuid(retrievedFlightConnectionId))
                                    {
                                        // get response
                                        GetDirectlyDbAPIPassengerBaggagesConnecting(BaggageId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, bags);
                                    }
                                    else
                                    {
                                        // get response
                                        GetDirectlyDbAPIPassengerBaggages(BaggageId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, bags);

                                    }
                                    if (bags != null && bags.Count > 0)
                                    {
                                        GetAPIErrors("000", errors);

                                        //if (KeepLogResponse)
                                        //{
                                        //    LogHelper.writeToLogFile(strFunction, parametersForLog, "000", "update success", string.Empty);
                                        //}
                                    }
                                    else
                                    {
                                        GetAPIErrors(default_error_code, errors);
                                        LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "update error", string.Empty);
                                    }
                                }
                                else
                                {
                                    GetAPIErrors(default_error_code, errors);
                                    LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "update error", string.Empty);
                                }

                                sqlComm.Parameters.Clear();
                            }
                            catch (Exception ex)
                            {
                                GetAPIErrors("9999", errors);
                                LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, ex.Message, ex.StackTrace);
                            }
                            finally
                            {
                                if (conn.State == ConnectionState.Open)
                                {
                                    conn.Close();
                                }
                            }
                        }
                        else //error from DB verify
                        {
                            errors.Add(dBVerifyError);
                            LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                        }
                    }
                    else
                    {
                        GetAPIErrors("1122", errors);
                        LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                    }
                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }


            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, "finish id:"+ (retrievedBookingId ?? string.Empty).ToLower() +
                      " | " + (retrievedBookingSegmentId ?? string.Empty).ToLower() +
                      " | " + (retrievedPassengerId ?? string.Empty).ToLower() +
                      " | " + parametersForLog, errors[0].code, errors[0].message, string.Empty);
            }

            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, bags, null, null, errors);
        }

       // [WebMethod(EnableSession = true, Description = "Baggage offloaded pax")]
        public APIResult BaggageOffloadedPax(string BookingSegmentId, string PassengerId, string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction = "BaggageOffloadedPax";
            bool isParameterWellForm = true;
            string default_error_code = "1100";
            string parametersForLog = string.Empty;
            int res = 0;
            string userid = string.Empty;
            string str_SP_name = "update_baggage_api";

            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand(str_SP_name, conn);
            Token token = new DataAccessControl.Token();

            //keep log request
            parametersForLog = BookingSegmentId + "|" + PassengerId ;

            bool keepLogRequest = false;
            bool KeepLogResponse = false;

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, parametersForLog, "", "log request", string.Empty);
            }

            // verify data request
            if (string.IsNullOrEmpty(BookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1101", errors);
            }
            else if (!objHelper.IsValid(BookingSegmentId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1101", errors);
            }
            else if (string.IsNullOrEmpty(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1102", errors);
            }
            else if (!objHelper.IsValid(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1102", errors);
            }
            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {
                    //valid add bag from sql
                    string retrievedBookingId;
                    string retrievedBookingSegmentId ;
                    string retrievedPassengerId ;
                    string retrievedFlightConnectionId;

                    APIError dBVerifyError = VerifyBaggageCheckin(BookingSegmentId, PassengerId, string.Empty, string.Empty, string.Empty, string.Empty, 0,"", out  retrievedBookingId,out retrievedBookingSegmentId, out retrievedPassengerId, out retrievedFlightConnectionId);

                    if (dBVerifyError.code != "000")
                    {
                        isParameterWellForm = false;
                    }

                    if (isParameterWellForm)
                    {
                        // baggage offloaded pax
                        try
                        {
                            sqlComm.CommandType = CommandType.StoredProcedure;

                            conn.Open();

                            sqlComm.Parameters.AddWithValue("@create_by", userid);
                            sqlComm.Parameters.AddWithValue("@passenger_id", PassengerId);
                            sqlComm.Parameters.AddWithValue("@segment_id", BookingSegmentId);

                            res = sqlComm.ExecuteNonQuery();

                            if (res == -1) // success
                            {
                                // get response
                                GetDirectlyDbAPIPassengerBaggages(string.Empty, PassengerId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, bags);

                                if (bags != null && bags.Count > 0)
                                {
                                    GetAPIErrors("000", errors);

                                    if (KeepLogResponse)
                                    {
                                        LogHelper.writeToLogFile(strFunction, "", "000", "success", string.Empty);
                                    }
                                }
                                else
                                {
                                    GetAPIErrors(default_error_code, errors);
                                    LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                                }
                            }
                            else
                            {
                                GetAPIErrors(default_error_code, errors);
                                LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                            }

                            sqlComm.Parameters.Clear();
                        }
                        catch (Exception ex)
                        {
                            GetAPIErrors("9999", errors);
                            LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, ex.Message, ex.StackTrace);
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }
                    }
                    else //error from DB verify
                    {
                        errors.Add(dBVerifyError);
                        LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                    }
                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }

            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, bags, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Requires Token to get baggage by passenger / baggage / booking")]
        public APIResult GetBaggage(string BaggageId, string PassengerId, string BookingId,string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction = "GetBaggage";
            bool isParameterWellForm = true;
            string default_error_code = "1114";
            string parametersForLog = string.Empty;
            string userid = string.Empty;

            //keep log request
            // parametersForLog = BaggageId + "|"  + PassengerId;

            parametersForLog = "bag: " + (BaggageId ?? string.Empty).ToLower() + " | " +
                  "pass:" + (PassengerId ?? string.Empty).ToLower() + " | " +
                  "book:" + (BookingId ?? string.Empty).ToLower();

            bool keepLogRequest = false;
            bool KeepLogResponse = false;
            Token token = new DataAccessControl.Token();

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            //if (keepLogRequest)
            //{
            //    LogHelper.writeToLogFile(strFunction, parametersForLog, "", "log request", string.Empty);
            //}

            // verify data request
            if (string.IsNullOrEmpty(BaggageId) && string.IsNullOrEmpty(PassengerId) && string.IsNullOrEmpty(BookingId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1110", errors); //"BookingId or Baggageid or Passengerid  is required.";
            }
            else if(!string.IsNullOrEmpty(BaggageId) && !string.IsNullOrEmpty(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1117", errors);
            }
            else if (!string.IsNullOrEmpty(BaggageId) && !string.IsNullOrEmpty(BookingId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1117", errors);
            }
            else if (!string.IsNullOrEmpty(PassengerId) && !string.IsNullOrEmpty(BookingId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1117", errors);
            }
            else if (!string.IsNullOrEmpty(BaggageId) && !string.IsNullOrEmpty(PassengerId) && !string.IsNullOrEmpty(BookingId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1117", errors);
            }
            else if (!string.IsNullOrEmpty(BaggageId) && !objHelper.IsValid(BaggageId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1111", errors);
            }
            else if (!string.IsNullOrEmpty(PassengerId) && !objHelper.IsValid(PassengerId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1112", errors);
            }
            else if (!string.IsNullOrEmpty(BookingId) && !objHelper.IsValid(BookingId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1112", errors);
            }
            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {
                    if (isParameterWellForm)
                    {
                        //get response
                        GetDirectlyDbAPIPassengerBaggages(BaggageId, PassengerId, string.Empty, BookingId, string.Empty, string.Empty, string.Empty, bags);

                        if (bags != null && bags.Count > 0)
                        {
                            GetAPIErrors("000", errors);

                            //if (KeepLogResponse)
                            //{
                            //    LogHelper.writeToLogFile(strFunction, parametersForLog, "000", "get bag success", string.Empty);
                            //}
                        }
                        else
                        {
                            GetAPIErrors(default_error_code, errors);
                            LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "get bag error not found", string.Empty);
                        }
                    }
                    else //error from DB verify
                    {
                       // errors.Add(dBVerifyError);
                        LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, "get bag error from DB verify", string.Empty);
                    }
                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }
            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, "finish " + parametersForLog, errors[0].code, errors[0].message, string.Empty);
            }
            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, bags, null, null, errors);
        }

        [WebMethod(EnableSession = true, Description = "Requires Token to get baggage list")]
        public APIResult GetBaggageList(string FlightId,string Origin,string Destination,string BaggageStatus, string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction = "GetBaggageList";
            bool isParameterWellForm = true;
            string default_error_code = "1114";
            string parametersForLog = string.Empty;
            string userid = string.Empty;
            Token token = new DataAccessControl.Token();
            //keep log request
            parametersForLog = FlightId ;

            bool keepLogRequest = false;
            bool KeepLogResponse = false;

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, parametersForLog, "", "log request", string.Empty);
            }

            // verify data request
            if (string.IsNullOrEmpty(FlightId)) // always required flight id
            {
                isParameterWellForm = false;
                GetAPIErrors("1113", errors);
            }
            else if (!objHelper.IsValid(FlightId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1113", errors);
            }
            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {
                    if (isParameterWellForm)
                    {
                        // get response
                        GetDirectlyDbAPIPassengerBaggages(string.Empty, string.Empty, FlightId, string.Empty, Origin, Destination, BaggageStatus, bags);

                        if (bags != null && bags.Count > 0)
                        {
                            GetAPIErrors("000", errors);

                            if (KeepLogResponse)
                            {
                                LogHelper.writeToLogFile(strFunction, "", "000", "success", string.Empty);
                            }
                        }
                        else
                        {
                            GetAPIErrors(default_error_code, errors);
                            LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                        }
                    }
                    else //error from DB verify
                    {
                        LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                    }
                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }

            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, bags, null, null, errors);
        }


        [WebMethod(EnableSession = true, Description = "Requires Token to insert history print baggage")]
        public APIResult InsertHistoryPrintBaggage(string BaggageId, string Token)
        {
            Helper objHelper = new Helper();
            APIPassengerBaggages bags = new APIPassengerBaggages();
            APIErrors errors = new APIErrors();
            Library objLi = new Library();
            string strFunction = "InsertHistoryPrintBaggage";
            bool isParameterWellForm = true;
            string default_error_code = "1119";
            string parametersForLog = string.Empty;
            int res = 0;
            string userid = string.Empty;
            string str_SP_name = "insert_history_baggage_api";

            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand(str_SP_name, conn);
            Token token = new DataAccessControl.Token();

            //keep log request
            parametersForLog = BaggageId  ;

            bool keepLogRequest = false;
            bool KeepLogResponse = false;

            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"] != null)
            {
                keepLogRequest = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogRequest"]);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"] != null)
            {
                KeepLogResponse = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KeepLogResponse"]);
            }

            if (keepLogRequest)
            {
                LogHelper.writeToLogFile(strFunction, parametersForLog, "", "log request", string.Empty);
            }
            // verify data request
            if (string.IsNullOrEmpty(BaggageId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1111", errors);
            }
            else if (!objHelper.IsValid(BaggageId))
            {
                isParameterWellForm = false;
                GetAPIErrors("1111", errors);
            }

            //ValidDecimalPlace

            else if (string.IsNullOrEmpty(Token))
            {
                isParameterWellForm = false;
                GetAPIErrors("910", errors);
            }
            else
            {
                bool isValidToken = true;
                string UseNewToken = "1";
                if (System.Configuration.ConfigurationManager.AppSettings["UseNewToken"] != null)
                {
                    UseNewToken = System.Configuration.ConfigurationManager.AppSettings["UseNewToken"].ToString();
                }
                // use new token by default
                if (UseNewToken.Equals("0"))
                {
                    string hky = string.Empty;
                    // old token
                    if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
                    {
                        hky = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
                    }
                    try
                    {
                        userid = StringCipher.Decrypt(Token, hky);
                    }
                    catch
                    {
                        isValidToken = false;
                    }
                }
                else // new token
                {
                    token = Authentication(Token);
                    userid = token.UserId;
                }

                if (isValidToken && objHelper.IsValid(userid))
                {

                    //valid add bag from sql
                    string retrievedBookingId;
                    string retrievedBookingSegmentId;
                    string retrievedPassengerId;
                    string retrievedFlightConnectionId;
                    APIError dBVerifyError = VerifyBaggageCheckin(null, null, null, null, string.Empty, BaggageId,0, string.Empty, out retrievedBookingId,out retrievedBookingSegmentId,out retrievedPassengerId,out retrievedFlightConnectionId);

                        if (dBVerifyError.code != "000")
                        {
                            isParameterWellForm = false;
                        }

                        if (isParameterWellForm)
                        {
                            //update history
                            try
                            {
                                sqlComm.CommandType = CommandType.StoredProcedure;

                                conn.Open();

                                sqlComm.Parameters.AddWithValue("@baggage_id", BaggageId);
                                sqlComm.Parameters.AddWithValue("@create_by", userid);

                                res = sqlComm.ExecuteNonQuery();
                            if (res == 1) // success
                            {
                                GetAPIErrors("000", errors);
                            }

                            sqlComm.Parameters.Clear();
                            }
                            catch (Exception ex)
                            {
                                GetAPIErrors("9999", errors);
                                LogHelper.writeToLogFile(strFunction, parametersForLog, errors[0].code, ex.Message, ex.StackTrace);
                            }
                            finally
                            {
                                if (conn.State == ConnectionState.Open)
                                {
                                    conn.Close();
                                }
                            }
                        }
                        else //error from DB verify
                        {
                            errors.Add(dBVerifyError);
                            LogHelper.writeToLogFile(strFunction, "", errors[0].code, "error", string.Empty);
                        }

                }
                else //error from get token 
                {
                    GetAPIErrors(token.ResponseCode, errors);
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

            return objLi.BuildAPIResultBagsXML(null, null, null, null, null, null, null, null, errors);
        }

        #endregion




        //directly to db     
        private APIError VerifyBaggageCheckin(string booking_segment_id, string passenger_id, string bagtag_status_rcd, string bagtag_type_rcd, string airline_rcd, string baggage_id, decimal baggage_weight, string final_destination, out string strBooking_id,out string strBooking_segment_id,out string strPassenger_id,out string strFlight_connection_id)
        {
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string str_SP_name = "verify_baggage_checkin_api";

            APIError error = new APIError();
            error.code = "000";
            error.message = string.Empty;

            strBooking_id = string.Empty;
            Guid booking_id = Guid.Empty;

            strBooking_segment_id = string.Empty;
            Guid booking_segment_id_out = Guid.Empty;

            strPassenger_id = string.Empty;
            Guid passenger_id_out = Guid.Empty;

            strFlight_connection_id = string.Empty;
            Guid flightConnectionId_out = Guid.Empty;

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection connection = new SqlConnection(connectionString);

            if (!string.IsNullOrEmpty(booking_segment_id))
                sqlCommand.Parameters.AddWithValue("@booking_segment_id", booking_segment_id);

            if (!string.IsNullOrEmpty(passenger_id))
                sqlCommand.Parameters.AddWithValue("@passenger_id", passenger_id);

            if (!string.IsNullOrEmpty(bagtag_status_rcd))
                sqlCommand.Parameters.AddWithValue("@bagtag_status_rcd", bagtag_status_rcd);

            if (!string.IsNullOrEmpty(bagtag_type_rcd))
                sqlCommand.Parameters.AddWithValue("@bagtag_type_rcd", bagtag_type_rcd);

            if (!string.IsNullOrEmpty(baggage_id))
                sqlCommand.Parameters.AddWithValue("@baggage_id", baggage_id);

            if (baggage_weight > 0)
                sqlCommand.Parameters.AddWithValue("@baggage_weight", baggage_weight);

            if (!string.IsNullOrEmpty(final_destination))
                sqlCommand.Parameters.AddWithValue("@final_destination", final_destination);

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                sqlCommand.Connection = connection;
                sqlCommand.CommandText = str_SP_name;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter result_code = new SqlParameter("@result_code", System.Data.SqlDbType.VarChar, 5);
                result_code.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(result_code);

                SqlParameter result_message = new SqlParameter("@result_message", System.Data.SqlDbType.VarChar, 250);
                result_message.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(result_message);

                //get booking_id
                SqlParameter booking_id_out = new SqlParameter("@booking_id_out", SqlDbType.UniqueIdentifier);
                booking_id_out.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(booking_id_out);

                SqlParameter sql_booking_segment_id_out = new SqlParameter("@booking_segment_id_out", SqlDbType.UniqueIdentifier);
                sql_booking_segment_id_out.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(sql_booking_segment_id_out);

                SqlParameter sql_passenger_id_out = new SqlParameter("@passenger_id_out", SqlDbType.UniqueIdentifier);
                sql_passenger_id_out.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(sql_passenger_id_out);

                SqlParameter sql_flightConnectionId_out = new SqlParameter("@flight_connection_id_out", SqlDbType.UniqueIdentifier);
                sql_flightConnectionId_out.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(sql_flightConnectionId_out);



                sqlCommand.ExecuteNonQuery();

                if (!string.IsNullOrEmpty(result_code.Value.ToString()))
                    error.code = result_code.Value.ToString();
                if (!string.IsNullOrEmpty(result_message.Value.ToString()))
                    error.message = result_message.Value.ToString();

                if (booking_id_out.Value != null && booking_id_out.Value != DBNull.Value)
                {
                    booking_id = (Guid)booking_id_out.Value;
                    strBooking_id = booking_id.ToString();
                }
                if (sql_booking_segment_id_out.Value != null && sql_booking_segment_id_out.Value != DBNull.Value)
                {
                    booking_segment_id_out = (Guid)sql_booking_segment_id_out.Value;
                    strBooking_segment_id = booking_segment_id_out.ToString();
                }
                if (sql_passenger_id_out.Value != null && sql_passenger_id_out.Value != DBNull.Value)
                {
                    passenger_id_out = (Guid)sql_passenger_id_out.Value;
                    strPassenger_id = passenger_id_out.ToString();
                }
                if (sql_flightConnectionId_out.Value != null && sql_flightConnectionId_out.Value != DBNull.Value)
                {
                    flightConnectionId_out = (Guid)sql_flightConnectionId_out.Value;
                    strFlight_connection_id = flightConnectionId_out.ToString();
                }
            }
            catch (SqlException ex)
            {
                error.code = "9999";
                error.message = "API connection error.";
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            return error;
        }

        private bool InsertPassengerAddress(Guid? passenger_id, Guid? booking_segment_id, Guid? passenger_profile_id, 
             string address_type_rcd, string passenger_address, string passenger_state, string city,
            string zip_Code, string country_rcd)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid =  ConfigurationManager.AppSettings["UserId"];

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("add_passenger_address", conn);

            sqlComm.Parameters.AddWithValue("@passenger_id", passenger_id);
            sqlComm.Parameters.AddWithValue("@booking_segment_id", booking_segment_id);
            sqlComm.Parameters.AddWithValue("@passenger_profile_id", passenger_profile_id);
            sqlComm.Parameters.AddWithValue("@address_type_rcd", address_type_rcd);
            sqlComm.Parameters.AddWithValue("@passenger_address", passenger_address);
            sqlComm.Parameters.AddWithValue("@passenger_state", passenger_state);
            sqlComm.Parameters.AddWithValue("@city", city);
            sqlComm.Parameters.AddWithValue("@zip_Code", zip_Code);
            sqlComm.Parameters.AddWithValue("@country_rcd", country_rcd);
            sqlComm.Parameters.AddWithValue("@user_id", userid);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool UpdatePassengerAddress( Guid? passenger_address_id,string passenger_address, 
            string passenger_state, string city,string zip_Code, string country_rcd)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid = ConfigurationManager.AppSettings["UserId"];

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("update_passenger_address", conn);

            sqlComm.Parameters.AddWithValue("@passenger_address_id", passenger_address_id);
            sqlComm.Parameters.AddWithValue("@passenger_address", passenger_address);
            sqlComm.Parameters.AddWithValue("@passenger_state", passenger_state);
            sqlComm.Parameters.AddWithValue("@city", city);
            sqlComm.Parameters.AddWithValue("@zip_Code", zip_Code);
            sqlComm.Parameters.AddWithValue("@country_rcd", country_rcd);
            sqlComm.Parameters.AddWithValue("@update_by", userid);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeletePassengerAddress(Guid? passenger_address_id)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid = ConfigurationManager.AppSettings["UserId"];
            int resultCount = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("delete_passenger_address", conn);

            sqlComm.Parameters.AddWithValue("@passenger_address_id", passenger_address_id);
            sqlComm.Parameters.AddWithValue("@result_count", resultCount);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InsertPassengerDocument(
            Guid? passenger_id, 
            Guid? booking_segment_id, 
            string document_type_rcd,
            string document_number, 
            DateTime issue_date, 
            DateTime expiry_date, 
            string issue_place, 
            string issue_country, 
            string birth_place, 
            string nationality_rcd, 
            string gender_type_rcd)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid = ConfigurationManager.AppSettings["UserId"];

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("add_passenger_document", conn);

            sqlComm.Parameters.AddWithValue("@passenger_id", passenger_id);
            sqlComm.Parameters.AddWithValue("@booking_segment_id", booking_segment_id);
            sqlComm.Parameters.AddWithValue("@document_type_rcd", document_type_rcd);
            sqlComm.Parameters.AddWithValue("@document_number", document_number);

            if (issue_date.Equals(DateTime.MinValue) == false)
            {
                sqlComm.Parameters.AddWithValue("@issue_date", issue_date);
            }

            if (expiry_date.Equals(DateTime.MinValue) == false)
            {
                sqlComm.Parameters.AddWithValue("@expiry_date", expiry_date);
            }


            //sqlComm.Parameters.AddWithValue("@date_of_birth", date_of_birth);
            sqlComm.Parameters.AddWithValue("@issue_place", issue_place);
            sqlComm.Parameters.AddWithValue("@issue_country", issue_country);
            sqlComm.Parameters.AddWithValue("@birth_place", birth_place);
            sqlComm.Parameters.AddWithValue("@nationality_rcd", nationality_rcd);
            sqlComm.Parameters.AddWithValue("@status_code", "A");
            sqlComm.Parameters.AddWithValue("@gender_type_rcd", gender_type_rcd);

            sqlComm.Parameters.AddWithValue("@user_id", userid);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool UpdatePassengerDocument(
           Guid? document_id,
           string document_type_rcd,
           string document_number,
           DateTime issue_date,
           DateTime expiry_date,
           string issue_place,
           string issue_country,
           string birth_place,
           string nationality_rcd,
           string gender_type_rcd)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid = ConfigurationManager.AppSettings["UserId"];

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("update_passenger_document", conn);

            sqlComm.Parameters.AddWithValue("@document_id", document_id);
            sqlComm.Parameters.AddWithValue("@document_type_rcd", document_type_rcd);
            sqlComm.Parameters.AddWithValue("@document_number", document_number);

            if (issue_date.Equals(DateTime.MinValue) == false)
            {
                sqlComm.Parameters.AddWithValue("@issue_date", issue_date);
            }

            if (expiry_date.Equals(DateTime.MinValue) == false)
            {
                sqlComm.Parameters.AddWithValue("@expiry_date", expiry_date);
            }

            sqlComm.Parameters.AddWithValue("@issue_place", issue_place);
            sqlComm.Parameters.AddWithValue("@issue_country", issue_country);
            sqlComm.Parameters.AddWithValue("@birth_place", birth_place);
            sqlComm.Parameters.AddWithValue("@nationality_rcd", nationality_rcd);
            sqlComm.Parameters.AddWithValue("@gender_type_rcd", gender_type_rcd);
            sqlComm.Parameters.AddWithValue("@status_code", "A");
            sqlComm.Parameters.AddWithValue("@user_id", userid);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeletePassengerDocument(Guid? document_id)
        {
            int retVal = 0;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            userid = ConfigurationManager.AppSettings["UserId"];

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand("delete_passenger_document", conn);

            sqlComm.Parameters.AddWithValue("@document_id", document_id);
            sqlComm.Parameters.AddWithValue("@status_code", "D");
            sqlComm.Parameters.AddWithValue("@user_id", userid);

            sqlComm.CommandType = CommandType.StoredProcedure;
            conn.Open();

            retVal = sqlComm.ExecuteNonQuery();
            conn.Close();

            if (retVal == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string CorrectFlightCheckInStatus(string status)
        {
            string flight_check_in_status_rcd = "";

            if (status == "0")
            {
                flight_check_in_status_rcd = "RESET";
            }
            else if (status == "1")
            {
                flight_check_in_status_rcd = "OPEN";
            }
            else if (status == "2")
            {
                flight_check_in_status_rcd = "CLOSED";
            }
            else if (status == "3")
            {
                flight_check_in_status_rcd = "DISPATCHED";
            }
            else if (status == "4")
            {
                flight_check_in_status_rcd = "FLOWN";
            }
            else if (status == "5")
            {
                flight_check_in_status_rcd = "RESETFLOWN";
            }

            return flight_check_in_status_rcd;
        }

        private APIError VerifyExistingFlight(Guid? flight_id, string origin, string destination, string flightnumber, DateTime dtDepartureDate, 
            string new_status, out string old_flight_check_in_status_rcd, 
            out int old_planned_departure_time, out int old_planned_arrival_time,
            out int old_estimated_departure_time, out int old_estimated_arrival_time,
            out int old_actual_departure_time, out int old_actual_arrival_time,
            out string old_boarding_gate,out string old_arrival_gate)
        {
            APIError error = new APIError();
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            old_planned_departure_time = 0;
            old_planned_arrival_time = 0;

            old_estimated_departure_time = 0;
            old_estimated_arrival_time = 0;

            old_actual_departure_time = 0;
            old_actual_arrival_time = 0;

            old_flight_check_in_status_rcd = string.Empty;

            old_boarding_gate = string.Empty;
            old_arrival_gate = string.Empty;

            SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@flight_id",flight_id),
               new SqlParameter("@origin_rcd",origin),
               new SqlParameter("@destination_rcd",destination),
               new SqlParameter("@departure_date",dtDepartureDate),
               new SqlParameter("@flight_number",flightnumber),
               new SqlParameter("@flight_check_in_status_rcd",new_status)
            };

            SqlConnection conn = null;
            string query = @"verify_open_flight_checkin_api";

            try
            {
                conn = new SqlConnection(connectionString);

                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(sqlParameter);
                cmd.CommandType = CommandType.StoredProcedure;

                #region old code
                //SqlParameter result_code = new SqlParameter("@result_code", System.Data.SqlDbType.VarChar, 5);
                //result_code.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(result_code);

                //SqlParameter result_message = new SqlParameter("@result_message", System.Data.SqlDbType.VarChar, 250);
                //result_message.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(result_message);
                //cmd.ExecuteNonQuery();
                //string xx  = result_code.Value.ToString() + "," + result_message.Value.ToString();

                #endregion

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        error.code = Convert.ToString(reader["result_code"]);
                        error.message = Convert.ToString(reader["result_message"]);

                        if (error.code == "000")
                        {
                            if (reader["planned_departure_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["planned_departure_time"].ToString()))
                                old_planned_departure_time = Convert.ToInt16(reader["planned_departure_time"]);
                            if (reader["planned_arrival_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["planned_arrival_time"].ToString()))
                                old_planned_arrival_time = Convert.ToInt16(reader["planned_arrival_time"]);

                            if (reader["estimated_departure_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["estimated_departure_time"].ToString()))
                                old_estimated_departure_time = Convert.ToInt16(reader["estimated_departure_time"]);
                            if (reader["estimated_arrival_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["estimated_arrival_time"].ToString()))
                                old_estimated_arrival_time = Convert.ToInt16(reader["estimated_arrival_time"]);

                            if (reader["actual_departure_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["actual_departure_time"].ToString()))
                                old_actual_departure_time = Convert.ToInt16(reader["actual_departure_time"]);
                            if (reader["actual_arrival_time"] != DBNull.Value && !string.IsNullOrEmpty(reader["actual_arrival_time"].ToString()))
                                old_actual_arrival_time = Convert.ToInt16(reader["actual_arrival_time"]);

                            old_flight_check_in_status_rcd = Convert.ToString(reader["flight_check_in_status_rcd"]);

                            old_boarding_gate = Convert.ToString(reader["boarding_gate"]);
                            old_arrival_gate = Convert.ToString(reader["arrival_gate"]);
                        }

                    }
                }


                /*
                if (error.code == "000")
                {
                    if (string.IsNullOrEmpty(old_flight_check_in_status_rcd) && !new_status.Equals("OPEN"))
                    {
                        error.code = "";
                        error.message = "The flight_check_in_status_rcd should be OPEN.";
                    }
                    else if (old_flight_check_in_status_rcd.Equals("OPEN"))
                    {
                        if (new_status.Equals("OPEN") || new_status.Equals("CLOSED") || new_status.Equals("RESET"))
                        {
                        }
                        else
                        {
                            error.code = "";
                            error.message = "The flight_check_in_status_rcd should be CLOSED.";
                        }
                    }
                    else if (old_flight_check_in_status_rcd.Equals("CLOSED"))
                    {
                        if (new_status.Equals("CLOSED") || new_status.Equals("DISPATCHED") || new_status.Equals("FLOWN") || new_status.Equals("RESET"))
                        {
                        }
                        else
                        {
                            error.code = "";
                            error.message = "The flight_check_in_status_rcd should be DISPATCHED or FLOWN.";
                        }
                    }
                    else if (old_flight_check_in_status_rcd.Equals("DISPATCHED"))
                    {
                        if (new_status.Equals("DISPATCHED") || new_status.Equals("FLOWN") || new_status.Equals("RESET"))
                        {
                        }
                        else
                        {
                            error.code = "";
                            error.message = "The flight_check_in_status_rcd should be FLOWN.";
                        }
                    }
                    else if (old_flight_check_in_status_rcd.Equals("FLOWN"))
                    {
                        if (new_status.Equals("FLOWN") || new_status.Equals("RESETFLOWN") || new_status.Equals("RESET"))
                        {
                        }
                        else
                        {
                            error.code = "";
                            error.message = "The flight_check_in_status_rcd should be RESETFLOWN.";
                        }
                    }
                }
                */
            }
            catch (SqlException ex)
            {
                error.code = "9999";
            }
            finally
            {
                conn.Close();
            }
            return error;
        }

        private bool GetExistingFlight(Guid? flight_id, string origin, string destination, string flightnumber, DateTime dtDepartureDate, out int origin_sequence, out int number_of_leg)
        {
            bool result = false;
            origin_sequence = 0;
            number_of_leg = 0;

            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@flight_id",flight_id),
               new SqlParameter("@origin_rcd",origin),
               new SqlParameter("@destination_rcd",destination),
               new SqlParameter("@departure_date",dtDepartureDate),
               new SqlParameter("@flight_number",flightnumber)
            };

            //  SqlDataReader reader = null;
            SqlConnection conn = null;
            string query = @"get_existing_flight";

            try
            {
                conn = new SqlConnection(connectionString);

                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(sqlParameter);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ResultCountParam1 = new SqlParameter("@origin_sequence", System.Data.SqlDbType.Int);
                ResultCountParam1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ResultCountParam1);

                SqlParameter ResultCountParam2 = new SqlParameter("@legs", System.Data.SqlDbType.Int);
                ResultCountParam2.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ResultCountParam2);

                cmd.ExecuteNonQuery();

                origin_sequence = System.Convert.ToInt32(ResultCountParam1.Value.ToString());
                number_of_leg = System.Convert.ToInt32(ResultCountParam2.Value.ToString());

                if (origin_sequence > 0)
                    result = true;

            }
            catch (System.Exception ex) { }
            finally
            {
                conn.Close();
            }

            return result;
        }

        private bool GetFlightCheckIn(Guid? flight_id, string origin, string destination, string flightnumber, string airlinecode, DateTime dtDepartureDate)
        {
            bool result = false;

            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@flight_id",flight_id),
               new SqlParameter("@origin",origin),
               new SqlParameter("@destination",destination),
               new SqlParameter("@departuredate",dtDepartureDate),
               new SqlParameter("@airlinecode",airlinecode),
               new SqlParameter("@flightnumber",flightnumber)
            };

            Connection connection = new Connection(connectionString);
           
            result = connection.IsFlightFound(sqlParameter);

            return result;
        }

        private bool GetFlightById(Guid? flight_id)
        {
            bool result = false;

            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@flight_id",flight_id)
            };

            Connection connection = new Connection(connectionString);

            result = connection.IsFlightFoundById(sqlParameter);

            return result;
        }

        private bool IsBookingLocked(string booking_id)
        {
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string query = "SELECT lock_date_time FROM booking_lock_queue WITH (NOLOCK) WHERE booking_id = @booking_id";

            string lock_date_time = string.Empty;
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@booking_id", booking_id.ToString());

                        cmd.CommandType = CommandType.Text;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if(reader["lock_date_time"] != DBNull.Value)
                                    lock_date_time = Convert.ToString(reader["lock_date_time"]);
                            }
                        }
                        else
                        {
                            //
                        }

                        if (!string.IsNullOrEmpty(lock_date_time))
                            result = true;

                        reader.Close();
                        con.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            return result;
        }

        private string GetWhatIsChange(string original_flight_check_in_status_rcd,
            string flight_check_in_status_rcd,

            int original_planned_departure_time, 
            string new_planned_departure_time, 
            int original_planned_arrival_time, 
            string new_planned_arrival_time,

            int original_estimated_departure_time,
            string new_estimated_departure_time,
            int original_estimated_arrival_time,
            string new_estimated_arrival_time,

            int original_actual_departure_time,
            string new_actual_departure_time,
            int original_actual_arrival_time,
            string new_actual_arrival_time,

            string original_boarding_gate, 
            string boarding_gate,
            string original_arrival_gate, 
            string arrival_gate)
        {
            string WhatChange = string.Empty;

            //convert time to int for compare to find flag
            int I_new_planned_departure_time = 0;
            if (!string.IsNullOrEmpty(new_planned_departure_time))
                I_new_planned_departure_time = Convert.ToInt16(new_planned_departure_time);
            int I_new_planned_arrival_time = 0;
            if (!string.IsNullOrEmpty(new_planned_arrival_time))
                I_new_planned_arrival_time = Convert.ToInt16(new_planned_arrival_time);

            int I_new_estimated_departure_time = 0;
            if (!string.IsNullOrEmpty(new_estimated_departure_time))
                I_new_estimated_departure_time = Convert.ToInt16(new_estimated_departure_time);
            int I_new_estimated_arrival_time = 0;
            if (!string.IsNullOrEmpty(new_estimated_arrival_time))
                I_new_estimated_arrival_time = Convert.ToInt16(new_estimated_arrival_time);

            int I_new_actual_departure_time = 0;
            if (!string.IsNullOrEmpty(new_actual_departure_time))
                I_new_actual_departure_time = Convert.ToInt16(new_actual_departure_time);
            int I_new_actual_arrival_time = 0;
            if (!string.IsNullOrEmpty(new_actual_arrival_time))
                I_new_actual_arrival_time = Convert.ToInt16(new_actual_arrival_time);


            if (I_new_planned_departure_time != 0 && I_new_planned_departure_time != original_planned_departure_time)
            {
                WhatChange = "time";
            }
            else if (I_new_planned_arrival_time != 0 &&  I_new_planned_arrival_time != original_planned_arrival_time)
            {
                WhatChange = "time";
            }

            if (I_new_estimated_departure_time != 0 && I_new_estimated_departure_time != original_estimated_departure_time)
            {
                WhatChange = "time";
            }
            if (I_new_estimated_arrival_time != 0 && I_new_estimated_arrival_time != original_estimated_arrival_time)
            {
                WhatChange = "time";
            }

            if (I_new_actual_departure_time != 0 && I_new_actual_departure_time != original_actual_departure_time)
            {
                WhatChange = "time";
            }
            if (I_new_actual_arrival_time != 0 && I_new_actual_arrival_time != original_actual_arrival_time)
            {
                WhatChange = "time";
            }


            if (!string.IsNullOrEmpty(flight_check_in_status_rcd) )
            {
                if (!flight_check_in_status_rcd.ToUpper().Equals(original_flight_check_in_status_rcd.ToUpper()))
                {
                    WhatChange += "status";
                }
            }

            if (!string.IsNullOrEmpty(boarding_gate) )
            {
                if (!boarding_gate.ToUpper().Equals(original_boarding_gate.ToUpper()))
                {
                    WhatChange += "gate";
                }
            }

            if (!string.IsNullOrEmpty(arrival_gate) )
            {
                if (!arrival_gate.ToUpper().Equals(original_arrival_gate.ToUpper()))
                {
                    WhatChange += "gate";
                }
            }

            return WhatChange;
        }


        private bool SetFlightCheckInStatusToDB(Guid? flight_id, string origin_rcd,
                                       string destination_rcd, DateTime departure_date, string airline_rcd, string flight_number, string user_id,
            string flight_check_in_status_rcd, string original_flight_check_in_status_rcd,string allow_web_checkin,
            string planned_departure_time, string planned_arrival_time,
            string estimated_departure_time, string estimated_arrival_time,
            string actual_departure_time, string actual_arrival_time, string boarding_gate, string arrival_gate,
            string flight_status_changed_flag, string time_changed_flag, string gate_changed_flag)
        {
            const string sqlQuery = "open_flights_for_checkin_api";
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            bool result = false;

            bool verifySql = false;
            string errorcode = string.Empty;
            List<string> checkSQLStrings = new List<string>();
            checkSQLStrings.Add(boarding_gate);
            checkSQLStrings.Add(arrival_gate);

            verifySql = IsContainSQLStatement(checkSQLStrings, out errorcode);

            bool alowedUpdate = ValidParameterToUpdate(flight_check_in_status_rcd, allow_web_checkin, planned_departure_time, planned_arrival_time,
                estimated_departure_time, estimated_arrival_time, actual_departure_time, actual_arrival_time, boarding_gate, arrival_gate);
           
            if (verifySql && alowedUpdate)
            {
                if(string.IsNullOrEmpty(flight_check_in_status_rcd))
                {
                    flight_check_in_status_rcd = original_flight_check_in_status_rcd;
                }

               SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@airline_rcd",airline_rcd.ToUpper()),
               new SqlParameter("@flight_id",flight_id),
               new SqlParameter("@origin_rcd",origin_rcd.ToUpper()),
               new SqlParameter("@user_id",user_id),
               new SqlParameter("@destination_rcd",destination_rcd.ToUpper()),
               new SqlParameter("@flight_check_in_status",flight_check_in_status_rcd),
               new SqlParameter("@allow_web_checkin",allow_web_checkin),

               new SqlParameter("@planned_departure_time",planned_departure_time),
               new SqlParameter("@planned_arrival_time",planned_arrival_time),

               new SqlParameter("@estimated_departure_time",estimated_departure_time),
               new SqlParameter("@estimated_arrival_time",estimated_arrival_time),

               new SqlParameter("@actual_departure_time",actual_departure_time),
               new SqlParameter("@actual_arrival_time",actual_arrival_time),

               new SqlParameter("@boarding_gate",boarding_gate),
               new SqlParameter("@arrival_gate",arrival_gate),

               new SqlParameter("@flight_status_changed_flag",flight_status_changed_flag),
               new SqlParameter("@time_changed_flag",time_changed_flag),
               new SqlParameter("@gate_changed_flag",gate_changed_flag)
            };

                Connection connection = new Connection(connectionString);
                string execResult = connection.ExecuteQueryReturn(sqlQuery, sqlParameter);

                if (execResult.Contains("000"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                    LogHelper.writeToLogFile("SetFlightCheckInStatus", "", execResult, "error", flight_id + "|" + origin_rcd + destination_rcd);
                }
            }
            else
            {
               // Not Implement Exception Jan 2023
            }
        
            return result;
        }

        private bool SetFlightStatusToDB(Guid? flight_id, string str_flight_status_rcd, string user_id)
        {
            const string sqlQuery = "set_flights_status_checkin_api";
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            string flight_status_rcd = string.Empty;

            if (str_flight_status_rcd == "1")
            {
                flight_status_rcd = "ACTIVE";
            }
            else if (str_flight_status_rcd == "2")
            {
                flight_status_rcd = "INACTIVE";
            }
            else if (str_flight_status_rcd == "3")
            {
                flight_status_rcd = "DEPARTED";
            }
            else if (str_flight_status_rcd == "4")
            {
                flight_status_rcd = "CANCELLED";
            }

            SqlParameter[] sqlParameter = new SqlParameter[] {
               new SqlParameter("@flight_id",flight_id),
               new SqlParameter("@flight_status_rcd",flight_status_rcd),
               new SqlParameter("@user_id",user_id)

            };

            Connection connection = new Connection(connectionString);
            bool result = connection.executeQuery(sqlQuery, sqlParameter);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetAllowWebCheckinFlag(string allow_web_checkin)
        {
            string allow_web_checkinTmp = "";
            if (string.IsNullOrEmpty(allow_web_checkin))
            {
                allow_web_checkinTmp = "";
            }
            else if (allow_web_checkin.ToUpper() == "TRUE")
            {
                allow_web_checkinTmp = "1";
            }
            else if (allow_web_checkin.ToUpper() == "FALSE")
            {
                allow_web_checkinTmp = "0";
            }
            else
            {
                allow_web_checkinTmp = "";
            }
            return allow_web_checkinTmp;
        }

        private void GetNewFlightTime(string planned_departure_time, string planned_arrival_time,string estimated_departure_time,string estimated_arrival_time, string actual_departure_time, string actual_arrival_time,
            out string planned_departure_timeTmp,out string planned_arrival_timeTmp, out string estimated_departure_timeTmp, out string estimated_arrival_timeTmp, out string actual_departure_timeTmp, out string actual_arrival_timeTmp)
        {
             planned_departure_timeTmp = string.Empty;
             planned_arrival_timeTmp = string.Empty;
             estimated_departure_timeTmp = string.Empty;
             estimated_arrival_timeTmp = string.Empty;
             actual_departure_timeTmp = string.Empty;
             actual_arrival_timeTmp = string.Empty;

            if (!string.IsNullOrEmpty(planned_departure_time) && planned_departure_time.All(char.IsDigit))
            {
                if (planned_departure_time != "0000")
                    planned_departure_timeTmp = planned_departure_time;
                else
                    planned_departure_timeTmp = null;
            }
            else
            {
                planned_departure_timeTmp = null;
            }
            if (!string.IsNullOrEmpty(estimated_departure_time) && estimated_departure_time.All(char.IsDigit))
            {
                if (estimated_departure_time != "0000")
                    estimated_departure_timeTmp = estimated_departure_time;
                else
                    estimated_departure_timeTmp = null;
            }
            else
            {
                estimated_departure_timeTmp = null;
            }

            if (!string.IsNullOrEmpty(actual_departure_time) && actual_departure_time.All(char.IsDigit))
            {
                if (actual_departure_time != "0000")
                    actual_departure_timeTmp = actual_departure_time;
                else
                    actual_departure_timeTmp = null;
            }
            else
            {
                actual_departure_timeTmp = null;
            }
            if (!string.IsNullOrEmpty(planned_arrival_time) && planned_arrival_time.All(char.IsDigit))
            {
                if (planned_arrival_time != "0000")
                    planned_arrival_timeTmp = planned_arrival_time;
                else
                    planned_arrival_timeTmp = null;
            }
            else
            {
                planned_arrival_timeTmp = null;
            }
            if (!string.IsNullOrEmpty(estimated_arrival_time) && estimated_arrival_time.All(char.IsDigit))
            {
                if (estimated_arrival_time != "0000")
                    estimated_arrival_timeTmp = estimated_arrival_time;
                else
                    estimated_arrival_timeTmp = null;
            }
            else
            {
                estimated_arrival_timeTmp = null;
            }
            if (!string.IsNullOrEmpty(actual_arrival_time) && actual_arrival_time.All(char.IsDigit))
            {
                if (actual_arrival_time != "0000")
                    actual_arrival_timeTmp = actual_arrival_time;
                else
                    actual_arrival_timeTmp = null;
            }
            else
            {
                actual_arrival_timeTmp = null;
            }
        }

        private bool ValidParameterToUpdate(string status,string allow_web_checkin,string planned_departure_time, string planned_arrival_time,
            string estimated_departure_time,string estimated_arrival_time,string actual_departure_time,
            string actual_arrival_time,string boarding_gate,string arrival_gate)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(status) || 
                !string.IsNullOrEmpty(allow_web_checkin) || 
                !string.IsNullOrEmpty(planned_departure_time) ||
                !string.IsNullOrEmpty(planned_arrival_time) || 
                !string.IsNullOrEmpty(estimated_departure_time) || 
                !string.IsNullOrEmpty(estimated_arrival_time) ||
                !string.IsNullOrEmpty(actual_departure_time) ||
                !string.IsNullOrEmpty(actual_arrival_time) ||
                !string.IsNullOrEmpty(boarding_gate) ||
                !string.IsNullOrEmpty(arrival_gate) 
                )
            {
                result = true;
            }

                return result;
        }

        private DataSet GetBaggageTag(string baggage_id, string passenger_id,string booking_id,string is_connecting_flight)
        {
            DataSet ds = new DataSet();
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string str_SP_name = "get_baggage_tag_api";

            APIError error = new APIError();
            error.code = "000";
            error.message = string.Empty;

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection connection = new SqlConnection(connectionString);

                if (!string.IsNullOrEmpty(booking_id))
                    sqlCommand.Parameters.AddWithValue("@booking_id", booking_id);
                else
                    sqlCommand.Parameters.AddWithValue("@booking_id", null);

                if (!string.IsNullOrEmpty(passenger_id))
                    sqlCommand.Parameters.AddWithValue("@passenger_id", passenger_id);
                else
                    sqlCommand.Parameters.AddWithValue("@passenger_id", null);

            if (!string.IsNullOrEmpty(baggage_id))
                    sqlCommand.Parameters.AddWithValue("@baggage_id", baggage_id);
            else
                sqlCommand.Parameters.AddWithValue("@baggage_id", null);

            if (!string.IsNullOrEmpty(is_connecting_flight))
                    sqlCommand.Parameters.AddWithValue("@is_connecting_flight", "Y");

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                sqlCommand.Connection = connection;
                sqlCommand.CommandText = str_SP_name;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    adapter.Fill(ds);
                }
            }
            finally
            {
                if (sqlCommand.Connection != null)
                    sqlCommand.Connection.Dispose();
            }

            if (sqlCommand.Connection.State == ConnectionState.Open)
            {
                sqlCommand.Connection.Close();
            }

            return ds;
        }

        private DataSet GetBaggageTagByFlight(string flight_id, string origin,string destionation,string baggage_status)
        {
            DataSet ds = new DataSet();
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string str_SP_name = "get_check_in_baggages";

            APIError error = new APIError();
            error.code = "000";
            error.message = string.Empty;

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection connection = new SqlConnection(connectionString);

            // required filed from SP
            sqlCommand.Parameters.AddWithValue("@flightid", flight_id);

            if (!string.IsNullOrEmpty(origin))
                sqlCommand.Parameters.AddWithValue("@origin", origin.Trim());
            if (!string.IsNullOrEmpty(destionation))
                sqlCommand.Parameters.AddWithValue("@destination", destionation.Trim());
            if (!string.IsNullOrEmpty(baggage_status))
                sqlCommand.Parameters.AddWithValue("@status", baggage_status.Trim());

            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                sqlCommand.Connection = connection;
                sqlCommand.CommandText = str_SP_name;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    adapter.Fill(ds);
                }
            }
            finally
            {
                if (sqlCommand.Connection != null)
                    sqlCommand.Connection.Dispose();
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            return ds;
        }

        private bool ValidBaggageUnit(string unit)
        {
            bool result = false;

            string[] arrType = new string[] { "KGS"};

            if (!string.IsNullOrEmpty(unit))
            {
                int pos = Array.IndexOf(arrType, unit.ToUpper());
                if (pos > -1)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ValidAirlineCode(string code)
        {
            bool result = false;
            string airlineCode = string.Empty;
            string airline = string.Empty;

            //read from config
            if (System.Configuration.ConfigurationManager.AppSettings["AirlineCode"] != null)
            {
                airline = System.Configuration.ConfigurationManager.AppSettings["AirlineCode"];

                if (!string.IsNullOrEmpty(airline))
                    airlineCode = airline;
            }

            if (code.ToUpper().Equals(airlineCode))
            {
                result = true;
            }

            return result;
        }

        private bool IsValidString(string text)
        {
            if (text != null && text.Trim().Length > 0)
            {
                return true;
            }
            return false;
        }

        private bool ValidBaggageWeight(decimal? weight)
        {
            bool result = false;
            if (weight >= 0 && weight <= 100)
            {
                result = true;
            }
            return result;
        }

        private bool ValidUpdateBaggageWeight(decimal? weight)
        {
            bool result = false;
            if (weight != null)
            {
                if (weight > 0 && weight <= 100)
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool ValidDecimalPlace(decimal? weight)
        {
            bool result = false;
            decimal value = weight ?? 0;

            if (decimal.Round(value, 1) == value)
            {
                result = true;
            }
            return result;
        }

        private bool ValidDecimal(string weight)
        {
            bool result = false;
            decimal value;

            if (Decimal.TryParse(weight, out value))
                result = true;

            return result;
        }

        public bool IsValidDateTime(string departureDate)
        {
            bool r = false;
            DateTime dtTemp;
            Regex checktime = new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");
           // r = checktime.IsMatch(departureDate);
            if (DateTime.TryParse(departureDate, out dtTemp))
            {
                r = true;
            }
            return r;
        }

        public bool IsValidTime(string thetime)
        {
            //^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$
            Regex checktime =
             new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3])[0-5][0-9]$");

            return checktime.IsMatch(thetime);
        }

        public static bool TryParseGuid(string guidString, out Guid guid)
        {
            if (guidString == null) throw new ArgumentNullException("guidString");
            try
            {
                guid = new Guid(guidString);
                return true;
            }
            catch (FormatException)
            {
                guid = default(Guid);
                return false;
            }
        }

        #region DupSeat 2022 utydev-187
        private bool RemoveAutoSeat(string passengerId)
        {
            bool result = true;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            try
            {
                string strSql = @"delete from dbo.tb_pre_save_seat where passenger_id = @passenger_id";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(strSql, connection))
                    {
                        command.Parameters.AddWithValue("@passenger_id", passengerId);

                        command.CommandType = CommandType.Text;

                        int x = command.ExecuteNonQuery();

                        if (x != 1)
                            result = false;

                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
            }

            return result;
        }

        private bool RemovePrecommit(string passengerId)
        {
            bool result = true;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            try
            {
                string strSql = @"delete from dbo.tb_pre_save_commit where passenger_id = @passenger_id";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(strSql, connection))
                    {
                        command.Parameters.AddWithValue("@passenger_id", passengerId);

                        command.CommandType = CommandType.Text;

                        int x = command.ExecuteNonQuery();

                        if (x != 1)
                            result = false;

                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
            }

            return result;
        }

        private bool SaveAutoSeat(string flightId, string passengerId, string passengerType, string seat)
        {
            bool result = true;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            // every 10 minutes will delete old data
            int intervalDelete = -10;
            if (System.Configuration.ConfigurationManager.AppSettings["intervalDelete"] != null)
            {
                intervalDelete = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["intervalDelete"]);
            }

            try
            {
                string strSql = @"pre_save_seat_api";

                if (passengerType != "INF")
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(strSql, connection))
                        {
                            command.Parameters.AddWithValue("@flight_id", flightId);
                            command.Parameters.AddWithValue("@passenger_id", passengerId);

                            if (!string.IsNullOrEmpty(seat))
                                command.Parameters.AddWithValue("@seat_number", seat.Trim());

                            command.Parameters.AddWithValue("@intervalDelete", intervalDelete);
                            command.CommandType = CommandType.StoredProcedure;

                            int x = command.ExecuteNonQuery();

                            if (x != -1)
                                result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
            }

            return result;
        }

        private bool SavePreCommit(string bookingSegmentId, string passengerId)
        {
            bool result = true;
            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            int intervalDelete = -10;
            if (System.Configuration.ConfigurationManager.AppSettings["intervalDelete"] != null)
            {
                intervalDelete = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["intervalDelete"]);
            }

            try
            {
                string strSql = @"pre_save_commit_api";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(strSql, connection))
                    {
                        command.Parameters.AddWithValue("@booking_segment_id", bookingSegmentId);
                        command.Parameters.AddWithValue("@passenger_id", passengerId);
                        command.Parameters.AddWithValue("@intervalDelete", intervalDelete);

                        command.CommandType = CommandType.StoredProcedure;

                        int x = command.ExecuteNonQuery();

                        if (x != -1)
                            result = false;

                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
            }

            return result;
        }

        #endregion

        #region Token

        private string GetToken(string agencyCode, string userLogon, string userPassword)
        {
            string userId = string.Empty;
            string strEncryptKey = string.Empty;
            string strHashing = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
            {
                strEncryptKey = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
            }

            try
            {
                // slow time for geting the new token value NOT dulicate
                Random random = new Random();
                int randomNumber = random.Next(1000, 1500);

                Thread.Sleep(randomNumber);

                Agency agent = TravelAgentLogon(agencyCode, userLogon, userPassword);

                if (agent != null && agent.Users != null && agent.Users.Count > 0)
                {
                    userId = agent.Users[0].user_account_id.ToString();

                    //if (agent.web_agency_flag == 1)
                    {
                        if (agent.api_flag == 1)
                        {
                            DateTime dtCurrentTime = DateTime.Now;
                            string strTime = dtCurrentTime.ToString("yyyy-MM-dd HH:mm:ss tt");

                            // param combined with userid agencycode and current time
                            string strParams = string.Format("{0}|{1}|{2}", new string[]
                                               { userId, agencyCode,strTime });

                            strHashing = SecurityHelper.EncryptString(strParams, strEncryptKey);
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
            }

            return strHashing;
        }

        private Token Authentication(string strToken)
        {
            string userId = string.Empty;
            string agencyCode = string.Empty;
            string strTime = string.Empty;
            string strTimeOut = string.Empty;
            string strEncryptKey = string.Empty;
            double timePoint = 0;
            Token token = new Token();

            if (System.Configuration.ConfigurationManager.AppSettings["hashkey"] != null)
            {
                strEncryptKey = System.Configuration.ConfigurationManager.AppSettings["hashkey"].ToString();
            }
            if (System.Configuration.ConfigurationManager.AppSettings["strTimeOut"] != null)
            {
                // unit in Seconds
                strTimeOut = System.Configuration.ConfigurationManager.AppSettings["strTimeOut"].ToString();
            }
            else
            {
                // 20 minutes
                strTimeOut = "1200";
            }
            try
            {
                string decryStr = SecurityHelper.DecryptString(strToken, strEncryptKey);

                if (!string.IsNullOrEmpty(decryStr))
                {
                    userId = decryStr.Split('|')[0];
                    agencyCode = decryStr.Split('|')[1];
                    strTime = decryStr.Split('|')[2];

                    if (!String.IsNullOrEmpty(strTimeOut))
                        timePoint = Int32.Parse(strTimeOut);

                    //convert string to date to point 1
                    DateTime dtCheckPoint1;
                    dtCheckPoint1 = new DateTime();
                    dtCheckPoint1 = DateTime.ParseExact(strTime, "yyyy-MM-dd HH:mm:ss tt", null);
                    
                    //grab current date to point 1=2
                    DateTime dtNowCheckPoint2 = DateTime.Now;

                    // cal time point1 and point 2
                    if (Math.Ceiling(dtNowCheckPoint2.Subtract(dtCheckPoint1).TotalSeconds) > timePoint)
                    {
                        token.ResponseSuccess = false;
                        token.ResponseMessage = "Security token timeout.";
                        token.ResponseCode = "959";
                    }
                    else
                    {
                        string strParams = string.Format("{0}|{1}|{2}", new string[]
                        { userId, agencyCode,strTime });

                        string hashing = SecurityHelper.EncryptString(strParams, strEncryptKey);

                        // valid token
                        if (strToken == hashing)
                        {
                            //ok
                            token.UserId = userId;
                            token.ResponseSuccess = true;
                            token.ResponseCode = "000";
                            token.ResponseMessage = "success";
                        }
                        else
                        {
                            // not ok
                            token.ResponseSuccess = false;
                            token.ResponseMessage = "Invalid security token.";
                            token.ResponseCode = "950";
                        }
                    }
                }
                else
                {
                    token.ResponseSuccess = false;
                    token.ResponseMessage = "Invalid security token.";
                    token.ResponseCode = "950";
                }
            }
            catch (System.Exception ex) { }

            return token;
        }
        
        private Agency TravelAgentLogon(string agencyCode, string userLogon, string userPassword)
        {
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            string userid = string.Empty;
            string query = "get_logon_agent";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@agency", agencyCode);
                        cmd.Parameters.AddWithValue("@user", userLogon);
                        cmd.Parameters.AddWithValue("@password", userPassword);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if(reader["user_account_id"] != DBNull.Value)
                                    userid = Convert.ToString(reader["user_account_id"]);
                            }
                        }
                        else
                        {
                            //
                        }

                        reader.Close();
                        con.Close();
                    }
                }
            }
            catch(System.Exception ex)
            {

            }

            if (!string.IsNullOrEmpty(userid))
            {
                Agency agency = GetAgencySessionProfile(agencyCode, userid);

                agency.Users = UserRead(userid);

                return agency;
            }
            else
            {
                return null;
            }

        }

        private Agency GetAgencySessionProfile(string strAgencyCode, string strUserId)
        {
            string connectionString = Helper.ToConnectionString("SQLConnectionString");
            DataSet ds = new DataSet("Agency");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand("get_agency_session_profile", conn);
                sqlComm.Parameters.AddWithValue("@UserId", strUserId);
                sqlComm.Parameters.AddWithValue("@AgencyCode", strAgencyCode);

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            Agency obj = new Agency();
            IList<Agency> a = DataTableToList<Agency>(ds.Tables[0], obj);

            return a[0];
        }

        private IList<Agency> DataTableToList<Agency>(DataTable table, Agency obj)
        {
            try
            {
                List<Agency> list = new List<Agency>();

                foreach (var row in table.AsEnumerable())
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        private IList<DataAccessControl.User> UserRead(string strUserId)
        {
            IList<DataAccessControl.User> users = new List<DataAccessControl.User>();
            string strSQL = "";


            strSQL = "SELECT" +
             "     a.user_account_id,a.user_logon,a.user_code,a.user_password,a.status_code," +
             "     y.version_major," +
             "     y.version_minor," +
             "     y.version_revision," +
             "     s.user_logon_session_id," +
             "     create_name = " +
             "                 (SELECT" +
             "                      c.lastname + ' ' + c.firstname " +
             "                  FROM" +
             "                      dbo.user_account c WITH(NOLOCK)" +
             "                  WHERE" +
             "                      c.user_account_id = a.create_by" +
             "                  )," +
             "     update_name = " +
             "                 (SELECT" +
             "                      u.lastname + ' ' + u.firstname " +
             "                  FROM" +
             "                      dbo.user_account u WITH(NOLOCK)" +
             "                  WHERE" +
             "                      u.user_account_id = a.update_by" +
             "                  )";

            strSQL = strSQL +
                     "FROM" +
                     "    dbo.user_account a WITH(NOLOCK)" +
                     "INNER JOIN" +
                     "    dbo.system_settings y WITH(NOLOCK)" +
                     "    ON  0 = 0" +
                     "LEFT JOIN" +
                     "    dbo.user_logon_session s WITH(NOLOCK)" +
                     "    ON  s.user_account_id = a.user_account_id " +
                     "WHERE" +
                     "    a.user_account_id = '" + strUserId + "'";



            string connectionString = Helper.ToConnectionString("SQLConnectionString");

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand(strSQL, conn);
            SqlDataReader r;
            conn.Open();
            r = sqlComm.ExecuteReader();

            List<DataAccessControl.User> AgencyList = new List<DataAccessControl.User>();
            users = DataReaderMapToList<DataAccessControl.User>(r);

            conn.Close();

            return users;
        }

        private static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();

                foreach (var Prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyObj = obj.GetType().GetProperty(Prop.Name);
                        propertyObj.SetValue(obj, Convert.ChangeType(dr[Prop.Name], propertyObj.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private bool IsContainSQLStatement(List<string> strSQL, out string errorMessage)
        {
            errorMessage = String.Empty;

            foreach (string str in strSQL)
            {
                string strSQLQuery = string.Empty;

                if(!string.IsNullOrEmpty(str))
                    strSQLQuery = str.Trim().ToLower();

                if (strSQLQuery.IndexOf(";") > -1)
                {
                    errorMessage = "The string text must not contain ;";
                    return false;
                }

                if (strSQLQuery.IndexOf("--") > -1)
                {
                    errorMessage = "The string text must not contain --";
                    return false;
                }

                if (strSQLQuery.IndexOf("@@") > -1)
                {
                    errorMessage += "The string text must not contain @@";
                    return false;
                }
                if (strSQLQuery.IndexOf("update") > -1 && strSQLQuery.Contains("set"))
                {
                    errorMessage = "The string text must not contain UPDATE";
                    return false;
                }

                if (strSQLQuery.IndexOf("delete") > -1 && strSQLQuery.Contains("from"))
                {
                    errorMessage = "The string text must not contain DELETE";
                    return false;
                }

                if (strSQLQuery.IndexOf("drop") > -1 && strSQLQuery.Contains("table") )
                {
                    errorMessage = "The string text must not contain DROP";
                    return false;
                }

                if (strSQLQuery.IndexOf("create") > -1 && strSQLQuery.Contains("table") )
                {
                    errorMessage = "The string text must not contain CREATE";
                    return false;
                }

                if (strSQLQuery.IndexOf("insert") > -1 && strSQLQuery.Contains("values"))
                {
                    errorMessage += "The string text must not contain INSERT";
                    return false;
                }

                if (strSQLQuery.IndexOf("alter") > -1 && strSQLQuery.Contains("table"))
                {
                    errorMessage += "The string text must not contain ALTER";
                    return false;
                }
                if (strSQLQuery.IndexOf("truncate") > -1 && strSQLQuery.Contains("table"))
                {
                    errorMessage += "The string text must not contain TRUNCATE";
                    return false;
                }

                if (strSQLQuery.IndexOf("exec") > -1)
                {
                    errorMessage += "The string text must not contain EXEC";
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Helper
        private void InitializeService()
        {

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            Booking objBooking = new Booking();
            objBooking = (Booking)Session["Booking"];

            Library objLi = new Library();

            if (objBooking == null)
            {
                objBooking = new Booking();
            }

            if (Session["CheckInFlight"] != null)
            {
                Session.Remove("CheckInFlight");
            }
            if (Session["Mappings"] != null)
            {
                Session.Remove("Mappings");
            }
            if (Session["Itinerary"] != null)
            {
                Session.Remove("Itinerary");
            }
            if (Session["Passengers"] != null)
            {
                Session.Remove("Passengers");
            }

            if (Session.IsNewSession == true || Session["Booking"] == null)
            {
                Agents objAgent = new Agents();
                WebCheckinAPIVariable objUv = new WebCheckinAPIVariable();

                if (ConfigurationManager.AppSettings["Service"] == "1")
                {
                    //Initialize Agentservice
                    if (Session["AgentService"] == null)
                    {
                        ServiceClient objService = new ServiceClient();
                        objUv.UserId = new Guid(ConfigurationManager.AppSettings["UserId"]);
                        objService.initializeWebService(ConfigurationManager.AppSettings["DefaultAgencyCode"], ref objAgent);
                        Session["AgentService"] = objService.objService;
                        objService = null;
                    }
                }
                else
                {
                    //Get B2c agency information
                    if (objAgent == null || objAgent.Count == 0)
                    {
                        //objAgent.GetAgencyInformation(ConfigurationManager.AppSettings["DefaultAgencyCode"]);
                        //objBooking.UserId = new Guid(ConfigurationManager.AppSettings["UserId"]);
                        //objAgent.AgencyRead(ConfigurationManager.AppSettings["DefaultAgencyCode"], string.Empty);
                        //objUv.UserId = new Guid(ConfigurationManager.AppSettings["UserId"]);
                    }
                }

                Session["Booking"] = objBooking;
            }

        }
        private string SingleFlight(XPathNavigator nv)
        {
            string tempSegmentId = string.Empty;
            string segmentId = string.Empty;

            XPathExpression xe = nv.Compile("Booking/Mapping");
            Library objLi = new Library();

            xe.AddSort("booking_segment_id", XmlSortOrder.Ascending, XmlCaseOrder.None, string.Empty, XmlDataType.Text);
            foreach (XPathNavigator n in nv.Select(xe))
            {
                segmentId = objLi.getXPathNodevalue(n, "booking_segment_id", Library.xmlReturnType.value);
                if (tempSegmentId.Length == 0)
                { tempSegmentId = segmentId; }

                if (tempSegmentId != segmentId)
                { return string.Empty; }
            }

            return tempSegmentId;
        }
        private string GetFlightInformation(string strBookingSegmentId)
        {
            Library objLi = new Library();
            Helper objHelper = new Helper();

            Mappings mps = new Mappings();
            Itinerary it = new Itinerary();
            Passengers paxs = new Passengers();

            CheckInPassengers ckps = new CheckInPassengers();

            objHelper.GetFlightInformation(ref ckps, strBookingSegmentId, Session["CheckInFlight"].ToString());

            ckps.objService = (TikAeroXMLwebservice)Session["AgentService"];

            string strXml = ckps.GetPassengerDetails("EN");
           
            if (strBookingSegmentId.Length == 0)
            {
                objLi.AddItinerary(strXml, it);
                objLi.AddMappings(strXml, mps);
            }
            else
            {
                objLi.AddItinerary(strXml, it, strBookingSegmentId);
                objLi.AddMappings(strXml, mps, strBookingSegmentId);
            }
            objLi.AddPassengers(strXml, paxs);

            Session["Mappings"] = mps;
            Session["Itinerary"] = it;
            Session["Passengers"] = paxs;

            return strXml;
        }


        private decimal GetOutstandingBalance(XPathDocument xmlDoc)
        {
            XPathNavigator nv = xmlDoc.CreateNavigator();
            Library objLi = new Library();
            decimal dAmount = 0;

            //fixed Booking/Mapping net_total(ticket_total) = null  from GDS && PNL
            foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
            {
                if (string.IsNullOrEmpty(objLi.getXPathNodevalue(n, "ticket_total", Library.xmlReturnType.value)))
                {
                    dAmount = (0 + Convert.ToDecimal(objLi.getXPathNodevalue(n, "fee_total", Library.xmlReturnType.value))) -
                              (Convert.ToDecimal(objLi.getXPathNodevalue(n, "ticket_payment_total", Library.xmlReturnType.value)) + Convert.ToDecimal(objLi.getXPathNodevalue(n, "fee_payment_total", Library.xmlReturnType.value)));
                }
                else
                {
                    dAmount = (Convert.ToDecimal(objLi.getXPathNodevalue(n, "ticket_total", Library.xmlReturnType.value)) + Convert.ToDecimal(objLi.getXPathNodevalue(n, "fee_total", Library.xmlReturnType.value))) -
                              (Convert.ToDecimal(objLi.getXPathNodevalue(n, "ticket_payment_total", Library.xmlReturnType.value)) + Convert.ToDecimal(objLi.getXPathNodevalue(n, "fee_payment_total", Library.xmlReturnType.value)));
                }
            }

            return dAmount;
        }

        private APIFlightSegments GetAPIFlightSegments(string bookingSegmentId, string strGetPassengerDetailXML, string strCheckInFlightXML, APIFlightSegments fss)
        {
            Library objLi = new Library();
            //APIFlightSegment fs;

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            XPathDocument xmlDoc2 = new XPathDocument(new StringReader(strCheckInFlightXML));
            XPathNavigator nv2 = xmlDoc2.CreateNavigator();


            foreach (XPathNavigator n in nv.Select("Booking/FlightSegment[booking_segment_id = '"+ bookingSegmentId + "']"))
            {
                APIFlightSegment fs = new APIFlightSegment();
                fs.booking_segment_id           = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                fs.flight_id                    = XmlHelper.XpathValueNullToGUID(n, "flight_id");
                fs.flight_connection_id         = XmlHelper.XpathValueNullToEmpty(n, "flight_connection_id");
                fs.airline_rcd                  = XmlHelper.XpathValueNullToEmpty(n, "airline_rcd");
                fs.flight_number                = XmlHelper.XpathValueNullToEmpty(n, "flight_number");
                fs.departure_date               = XmlHelper.XpathValueNullToDateTime(n, "departure_date");
              //  fs.departure_time               = XmlHelper.XpathValueNullToEmpty(n, "departure_time").ToString();
                //fs.departure_time               = int.Parse(XmlHelper.XpathValueNullToZero(n, "departure_time").ToString());
                fs.booking_class_rcd            = XmlHelper.XpathValueNullToEmpty(n, "booking_class_rcd");
                fs.boarding_class_rcd           = XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd");
                fs.segment_status_rcd           = XmlHelper.XpathValueNullToEmpty(n, "segment_status_rcd");
                fs.booking_id                   = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                fs.number_of_units              = int.Parse(XmlHelper.XpathValueNullToZero(n, "number_of_units").ToString());
                fs.journey_time                 = int.Parse(XmlHelper.XpathValueNullToZero(n, "journey_time").ToString());
             //   fs.arrival_time                 = XmlHelper.XpathValueNullToEmpty(n, "arrival_time").ToString();
                fs.arrival_date                 = XmlHelper.XpathValueNullToDateTime(n, "arrival_date");
                fs.origin_rcd                   = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                fs.destination_rcd              = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                fs.segment_change_status_rcd    = XmlHelper.XpathValueNullToEmpty(n, "segment_change_status_rcd");
                fs.info_segment_flag            = byte.Parse(XmlHelper.XpathValueNullToZero(n, "info_segment_flag").ToString());
                fs.high_priority_waitlist_flag  = byte.Parse(XmlHelper.XpathValueNullToZero(n, "high_priority_waitlist_flag").ToString());
                fs.od_origin_rcd                = XmlHelper.XpathValueNullToEmpty(n, "od_origin_rcd");
                fs.flight_check_in_status_rcd   = XmlHelper.XpathValueNullToEmpty(n, "flight_check_in_status_rcd");
                fs.od_destination_rcd           = XmlHelper.XpathValueNullToEmpty(n, "od_destination_rcd");
                fs.origin_name                  = XmlHelper.XpathValueNullToEmpty(n, "origin_name");
                fs.destination_name             = XmlHelper.XpathValueNullToEmpty(n, "destination_name");
                fs.segment_status_name          = XmlHelper.XpathValueNullToEmpty(n, "segment_status_name");
                fs.seatmap_flag                 = byte.Parse(XmlHelper.XpathValueNullToZero(n, "seatmap_flag").ToString());
                fs.allow_web_checkin_flag       = byte.Parse(XmlHelper.XpathValueNullToZero(n, "allow_web_checkin_flag").ToString());
                fs.transit_points               = XmlHelper.XpathValueNullToEmpty(n, "transit_points");
                fs.transit_points_name          = XmlHelper.XpathValueNullToEmpty(n, "transit_points_name");

                foreach (XPathNavigator n2 in nv2.Select("Booking/Mapping[booking_segment_id = '" + bookingSegmentId + "'][booking_id = '" + fs.booking_id.ToString() + "']"))
                {
                    fs.require_open_status_flag         = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_open_status_flag").ToString());
                    fs.require_ticket_number_flag       = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_ticket_number_flag").ToString());
                    fs.require_passenger_title_flag     = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_passenger_title_flag").ToString());
                    fs.require_passenger_gender_flag    = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_passenger_gender_flag").ToString());
                    fs.require_date_of_birth_flag       = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_date_of_birth_flag").ToString());
                    fs.require_document_details_flag    = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_document_details_flag").ToString());
                    fs.require_passenger_weight_flag    = short.Parse(XmlHelper.XpathValueNullToZero(n2, "require_passenger_weight_flag").ToString());
                    fs.show_redress_number_flag         = short.Parse(XmlHelper.XpathValueNullToZero(n2, "show_redress_number_flag").ToString());

                    fs.nautical_miles                   = int.Parse(XmlHelper.XpathValueNullToZero(n2, "nautical_miles").ToString());
                    fs.flight_information_1             = XmlHelper.XpathValueNullToEmpty(n2, "flight_information_1");
                    fs.flight_information_2             = XmlHelper.XpathValueNullToEmpty(n2, "flight_information_2");
                    fs.flight_information_3             = XmlHelper.XpathValueNullToEmpty(n2, "flight_information_3");
                    fs.min_transit_minutes              = int.Parse(XmlHelper.XpathValueNullToZero(n2, "min_transit_minutes").ToString());
                    fs.max_transit_minutes              = int.Parse(XmlHelper.XpathValueNullToZero(n2, "max_transit_minutes").ToString());
                    fs.close_web_check_in               = XmlHelper.XpathValueNullToEmpty(n2, "close_web_check_in");
                    fs.paper_ticket_warning_flag        = short.Parse(XmlHelper.XpathValueNullToZero(n2, "paper_ticket_warning_flag").ToString());
                  //  fs.departure_time                   = XmlHelper.XpathValueNullToEmpty(n2, "departure_time");
                   // fs.arrival_time                     = XmlHelper.XpathValueNullToEmpty(n2, "arrival_time");


                    object departureTimeObj = XmlHelper.XpathValueNullToEmpty(n2, "departure_time");
                    string departureTimeString = departureTimeObj != null ? departureTimeObj.ToString() : "0";

                    int departureTime;
                    bool departureParseSuccess = int.TryParse(departureTimeString, out departureTime);
                    if (departureParseSuccess)
                    {
                        if (departureTime > 0)
                            fs.departure_time = departureTime.ToString("0000");
                        else
                            fs.departure_time = "";
                    }
                    else
                    {
                        fs.departure_time = "";
                    }



                    object arrivalTimeObj = XmlHelper.XpathValueNullToEmpty(n2, "arrival_time");
                    string arrivalTimeString = arrivalTimeObj != null ? arrivalTimeObj.ToString() : "0";

                    int arrivalTime;
                    bool arrivalParseSuccess = int.TryParse(arrivalTimeString, out arrivalTime);
                    if (arrivalParseSuccess)
                    {
                        if (arrivalTime > 0)
                            fs.arrival_time = arrivalTime.ToString("0000");
                        else
                            fs.arrival_time = "";
                    }
                    else
                    {
                        fs.arrival_time = "";
                    }




                    string passenger_id = objLi.getXPathNodevalue(n2, "passenger_id", Library.xmlReturnType.value);
                    foreach (XPathNavigator n3 in nv.Select("Booking/Mapping[booking_segment_id = '" + bookingSegmentId + "'][passenger_id = '" + passenger_id + "']"))
                    {
                        fs.currency_rcd                 = XmlHelper.XpathValueNullToEmpty(n3, "currency_rcd");
                        fs.excess_baggage_charge_amount = decimal.Parse(XmlHelper.XpathValueNullToZero(n3, "excess_baggage_charge_amount").ToString());
                    }
                
                }

                fss.Add(fs);

                Session["FlightSegment"] = fss;
            }
            return fss;
        }
        private APIPassengerMappings GetAPIPassengerMappings(string bookingSegmentId, string passengerId, string strGetPassengerDetailXML, APIPassengerMappings mappings)
        {
            Library objLi = new Library();
            //APIPassengerMapping mapping;

            Mappings mm = new Mappings();
            mm = (Mappings)Session["SaveMapping"];

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/Mapping[booking_segment_id = '" + bookingSegmentId + "']"))
            {
                APIPassengerMapping mapping = new APIPassengerMapping();
                mapping.passenger_id        = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                mapping.booking_segment_id  = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");

                foreach (XPathNavigator n2 in nv.Select("Booking/Mapping[booking_segment_id = '" + bookingSegmentId + "'][passenger_id = '"+ mapping.passenger_id.ToString() +"']"))
                {
                    mapping.booking_id                      = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                    mapping.record_locator                  = XmlHelper.XpathValueNullToEmpty(n2, "record_locator_display");
                    mapping.language_rcd                    = XmlHelper.XpathValueNullToEmpty(n2, "language_rcd");
                    mapping.agency_code                     = XmlHelper.XpathValueNullToEmpty(n2, "agency_code");

                    mapping.seat_number = XmlHelper.XpathValueNullToEmpty(n2, "seat_number");
                    mapping.seat_row = int.Parse(XmlHelper.XpathValueNullToZero(n2, "seat_row").ToString());
                    mapping.seat_column = XmlHelper.XpathValueNullToEmpty(n2, "seat_column");

                    mapping.passenger_check_in_status_rcd = XmlHelper.XpathValueNullToEmpty(n2, "passenger_check_in_status_rcd");
                    mapping.passenger_status_rcd = XmlHelper.XpathValueNullToEmpty(n2, "passenger_status_rcd");
                    mapping.standby_flag = byte.Parse(XmlHelper.XpathValueNullToZero(n, "standby_flag").ToString());
                    mapping.flight_connection_id = XmlHelper.XpathValueNullToEmpty(n2, "flight_connection_id").ToString();


                    if (mm != null)
                    {
                        foreach (Mapping m in mm)
                        {
                            if (m.passenger_id == mapping.passenger_id)
                            {
                                //assign seat
                                mapping.seat_number                     = m.seat_number;
                                mapping.seat_row                        = m.seat_row;
                                mapping.seat_column                     = m.seat_column;

                                //commit
                                mapping.passenger_check_in_status_rcd   = m.passenger_check_in_status_rcd;
                                mapping.passenger_status_rcd            = m.passenger_status_rcd;
                                mapping.standby_flag                    = m.standby_flag;
                            }
                        }
                    }
                    
                    mapping.baggage_weight                  = decimal.Parse(XmlHelper.XpathValueNullToZero(n2, "baggage_weight").ToString());
                    mapping.piece_allowance                 = int.Parse(XmlHelper.XpathValueNullToZero(n2, "piece_allowance").ToString());
                    mapping.airline_rcd                     = XmlHelper.XpathValueNullToEmpty(n2, "airline_rcd");
                    mapping.flight_number                   = XmlHelper.XpathValueNullToEmpty(n2, "flight_number");
                    mapping.boarding_class_rcd              = XmlHelper.XpathValueNullToEmpty(n2, "boarding_class_rcd");
                    mapping.departure_date                  = XmlHelper.XpathValueNullToDateTime(n2, "departure_date");
                    mapping.lastname                        = XmlHelper.XpathValueNullToEmpty(n2, "lastname");
                    mapping.firstname                       = XmlHelper.XpathValueNullToEmpty(n2, "firstname");
                    mapping.gender_type_rcd                 = XmlHelper.XpathValueNullToEmpty(n2, "gender_type_rcd");
                    mapping.title_rcd                       = XmlHelper.XpathValueNullToEmpty(n2, "title_rcd");
                    mapping.passenger_type_rcd              = XmlHelper.XpathValueNullToEmpty(n2, "passenger_type_rcd");
                    mapping.boarding_pass_number            = XmlHelper.XpathValueNullToEmpty(n2, "boarding_pass_number");
                    mapping.check_in_sequence               = int.Parse(XmlHelper.XpathValueNullToZero(n2, "check_in_sequence").ToString());
                    mapping.group_sequence                  = int.Parse(XmlHelper.XpathValueNullToZero(n2, "group_sequence").ToString());
                    
                    mapping.e_ticket_flag                   = byte.Parse(XmlHelper.XpathValueNullToZero(n, "e_ticket_flag").ToString());
                    mapping.fare_type_rcd                   = XmlHelper.XpathValueNullToEmpty(n2, "fare_type_rcd");
                    mapping.booking_class_rcd               = XmlHelper.XpathValueNullToEmpty(n2, "booking_class_rcd");
                    mapping.currency_rcd                    = XmlHelper.XpathValueNullToEmpty(n2, "currency_rcd");
                    mapping.origin_rcd                      = XmlHelper.XpathValueNullToEmpty(n2, "origin_rcd");
                    mapping.destination_rcd                 = XmlHelper.XpathValueNullToEmpty(n2, "destination_rcd");
                    
                    mapping.client_number                   = long.Parse(XmlHelper.XpathValueNullToZero(n2, "client_number").ToString());
                    mapping.vip_flag                        = byte.Parse(XmlHelper.XpathValueNullToZero(n, "vip_flag").ToString());
                    mapping.passenger_weight                = XmlHelper.XpathValueNullToZero(n2, "passenger_weight");
                    
                    mapping.ticket_number                   = XmlHelper.XpathValueNullToEmpty(n2, "ticket_number");
                    
                    //mapping
                    mapping.priority_code                   = XmlHelper.XpathValueNullToEmpty(n2, "priority_code");
                    mapping.date_of_birth                   = XmlHelper.XpathValueNullToDateTime(n2, "date_of_birth");
                    mapping.onward_airline_rcd              = XmlHelper.XpathValueNullToEmpty(n2, "onward_airline_rcd");
                    mapping.onward_flight_number            = XmlHelper.XpathValueNullToEmpty(n2, "onward_flight_number");
                    mapping.onward_departure_date           = XmlHelper.XpathValueNullToDateTime(n2, "onward_departure_date");
                    mapping.onward_departure_time           = int.Parse(XmlHelper.XpathValueNullToZero(n2, "onward_departure_time").ToString());
                    mapping.onward_origin_rcd               = XmlHelper.XpathValueNullToEmpty(n2, "onward_origin_rcd");
                    mapping.onward_destination_rcd          = XmlHelper.XpathValueNullToEmpty(n2, "onward_destination_rcd");
                    mapping.onward_booking_class_rcd        = XmlHelper.XpathValueNullToEmpty(n2, "onward_booking_class_rcd");
                    //mapping
                    mapping.group_name                      = XmlHelper.XpathValueNullToEmpty(n2, "group_name");
                    mapping.contact_name                    = XmlHelper.XpathValueNullToEmpty(n2, "contact_name");
                    mapping.contact_email                   = XmlHelper.XpathValueNullToEmpty(n2, "contact_email");
                    mapping.phone_mobile                    = XmlHelper.XpathValueNullToEmpty(n2, "phone_mobile");
                    mapping.phone_home                      = XmlHelper.XpathValueNullToEmpty(n2, "phone_home");
                    mapping.phone_business                  = XmlHelper.XpathValueNullToEmpty(n2, "phone_business");
                    mapping.received_from                   = XmlHelper.XpathValueNullToEmpty(n2, "received_from");
                    mapping.phone_fax                       = XmlHelper.XpathValueNullToEmpty(n2, "phone_fax");
                    mapping.vendor_rcd                      = XmlHelper.XpathValueNullToEmpty(n2, "vendor_rcd");
                    mapping.tour_operator_locator           = XmlHelper.XpathValueNullToEmpty(n2, "tour_operator_locator");

                    mapping.coupon_number                   = XmlHelper.XpathValueNullToEmpty(n2, "coupon_number");
                    mapping.onward_segment_status_rcd       = XmlHelper.XpathValueNullToEmpty(n2, "onward_segment_status_rcd");
                    mapping.previous_airline_rcd            = XmlHelper.XpathValueNullToEmpty(n2, "previous_airline_rcd");
                    mapping.previous_flight_number          = XmlHelper.XpathValueNullToEmpty(n2, "previous_flight_number");
                    mapping.previous_departure_date         = XmlHelper.XpathValueNullToDateTime(n2, "previous_departure_date");
                    mapping.previous_departure_time         = int.Parse(XmlHelper.XpathValueNullToZero(n2, "previous_departure_time").ToString());
                    mapping.previous_origin_rcd             = XmlHelper.XpathValueNullToEmpty(n2, "previous_origin_rcd");
                    mapping.previous_destination_rcd        = XmlHelper.XpathValueNullToEmpty(n2, "previous_destination_rcd");
                    mapping.previous_booking_class_rcd_sort = int.Parse(XmlHelper.XpathValueNullToZero(n2, "previous_booking_class_rcd_sort").ToString());
                    mapping.previous_segment_status_rcd     = XmlHelper.XpathValueNullToEmpty(n2, "previous_segment_status_rcd");
                    mapping.arrival_date                    = XmlHelper.XpathValueNullToDateTime(n2, "arrival_date");

                    //  int x = int.Parse(XmlHelper.XpathValueNullToZero(n2, "departure_time").ToString());
                    //  string formattedValue = x.ToString("0000");
                    //  mapping.departure_time = formattedValue;// int.Parse(XmlHelper.XpathValueNullToZero(n2, "departure_time").ToString());
                    //  mapping.arrival_time                    = int.Parse(XmlHelper.XpathValueNullToZero(n2, "arrival_time").ToString()).ToString("0000");

                    object departureTimeObj = XmlHelper.XpathValueNullToZero(n2, "departure_time");
                    string departureTimeString = departureTimeObj != null ? departureTimeObj.ToString() : "0";

                    int departureTime;
                    bool departureParseSuccess = int.TryParse(departureTimeString, out departureTime);
                    if (departureParseSuccess)
                    {
                        if (departureTime > 0)
                            mapping.departure_time = departureTime.ToString("0000");
                        else
                            mapping.departure_time = "";
                    }
                    else
                    {
                        mapping.departure_time = "";
                    }

                    object arrivalTimeObj = XmlHelper.XpathValueNullToZero(n2, "arrival_time");
                    string arrivalTimeString = arrivalTimeObj != null ? arrivalTimeObj.ToString() : "0";

                    int arrivalTime;
                    bool arrivalParseSuccess = int.TryParse(arrivalTimeString, out arrivalTime);
                    if (arrivalParseSuccess)
                    {
                        if (arrivalTime > 0)
                            mapping.arrival_time = arrivalTime.ToString("0000");
                        else
                            mapping.arrival_time = "";
                    }
                    else
                    {
                        mapping.arrival_time = "";
                    }


                    mapping.group_count                     = int.Parse(XmlHelper.XpathValueNullToZero(n2, "group_count").ToString());
                    mapping.ticket_total                    = XmlHelper.XpathValueNullToZero(n2, "ticket_total");
                    mapping.ticket_payment_total            = XmlHelper.XpathValueNullToZero(n2, "ticket_payment_total");
                    mapping.fee_total                       = XmlHelper.XpathValueNullToZero(n2, "fee_total");
                    mapping.fee_payment_total               = XmlHelper.XpathValueNullToZero(n2, "fee_payment_total");

                    mapping.boarding_gate                   = XmlHelper.XpathValueNullToEmpty(n2, "boarding_gate");
                    // mapping.boarding_time                   = int.Parse(XmlHelper.XpathValueNullToZero(n2, "boarding_time").ToString()).ToString("0000");
                    object boardingTimeObj = XmlHelper.XpathValueNullToZero(n2, "boarding_time");
                    string boardingTimeString = boardingTimeObj != null ? boardingTimeObj.ToString() : "0";

                    int boardingTime;
                    bool boardingParseSuccess = int.TryParse(boardingTimeString, out boardingTime);
                    if (boardingParseSuccess)
                    {
                        if (boardingTime > 0)
                            mapping.boarding_time = boardingTime.ToString("0000");
                        else
                            mapping.boarding_time = "";
                    }
                    else
                    {
                        mapping.boarding_time = "";
                    }


                    foreach (XPathNavigator n3 in nv.Select("Booking/Passenger[booking_id = '" + mapping.booking_id.ToString() + "'][passenger_id = '" + mapping.passenger_id.ToString() + "']"))
                    {
                        //passenger
                        mapping.member_number                       = XmlHelper.XpathValueNullToEmpty(n3, "member_number");
                        mapping.member_level_rcd                    = XmlHelper.XpathValueNullToEmpty(n3, "member_level_rcd");
                        mapping.nationality_rcd                     = XmlHelper.XpathValueNullToEmpty(n3, "nationality_rcd");
                        mapping.passport_number                     = XmlHelper.XpathValueNullToEmpty(n3, "passport_number");
                        mapping.passport_issue_date                 = XmlHelper.XpathValueNullToDateTime(n3, "passport_issue_date");
                        mapping.passport_expiry_date                = XmlHelper.XpathValueNullToDateTime(n3, "passport_expiry_date");
                        mapping.passport_issue_place                = XmlHelper.XpathValueNullToEmpty(n3, "passport_issue_place");
                        mapping.passport_birth_place                = XmlHelper.XpathValueNullToEmpty(n3, "passport_birth_place");
                        mapping.wheelchair_flag                     = byte.Parse(XmlHelper.XpathValueNullToZero(n3, "wheelchair_flag").ToString());
                        mapping.residence_country_rcd               = XmlHelper.XpathValueNullToEmpty(n3, "residence_country_rcd");
                        mapping.document_type_rcd                   = XmlHelper.XpathValueNullToEmpty(n3, "document_type_rcd");
                        mapping.passport_issue_country_rcd          = XmlHelper.XpathValueNullToEmpty(n3, "passport_issue_country_rcd");
                        mapping.known_traveler_number = XmlHelper.XpathValueNullToEmpty(n3, "known_traveler_number");
                    }
                }

                if (passengerId == "")
                {
                    mappings.Add(mapping);
                }
                else
                {
                    if (mapping.passenger_id.ToString() == passengerId)
                    {
                        mappings.Add(mapping);
                    }
                }

                //mappings.Add(mapping);
            }

            return mappings;
        }
        private APIRouteConfigs GetAPIRouteConfigs(APIRouteConfigs routes)
        {
            return routes;
        }
        private APIPassengerServices GetAPIPassengerServices(string bookingSegmentId, string passengerId, string strGetPassengerDetailXML, APIPassengerServices services)
        {
            Library objLi = new Library();
            //APIPassengerService service;

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/Mapping[booking_segment_id = '" + bookingSegmentId + "']"))
            {
               
                Guid mapping_passenger_id                            = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                Guid mapping_booking_segment_id                      = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");

                foreach (XPathNavigator n2 in nv.Select("Booking/Service[booking_segment_id = '" + mapping_booking_segment_id.ToString() + "'][passenger_id = '"+mapping_passenger_id.ToString()+"']"))
                {
                    APIPassengerService service = new APIPassengerService();
                    service.passenger_id = mapping_passenger_id;
                    service.booking_segment_id = mapping_booking_segment_id;
                    service.passenger_segment_service_id        = XmlHelper.XpathValueNullToGUID(n2, "passenger_segment_service_id");
                    service.special_service_status_rcd          = XmlHelper.XpathValueNullToEmpty(n2, "special_service_status_rcd");
                    service.special_service_change_status_rcd   = XmlHelper.XpathValueNullToEmpty(n2, "special_service_change_status_rcd");
                    service.special_service_rcd                 = XmlHelper.XpathValueNullToEmpty(n2, "special_service_rcd");
                    service.service_text                        = XmlHelper.XpathValueNullToEmpty(n2, "service_text");
                    service.flight_id                           = XmlHelper.XpathValueNullToEmpty(n2, "flight_id");
                    service.fee_id                              = XmlHelper.XpathValueNullToEmpty(n2, "fee_id");
                    service.number_of_units                     = short.Parse(XmlHelper.XpathValueNullToZero(n2, "number_of_units").ToString());
                    service.origin_rcd                          = XmlHelper.XpathValueNullToEmpty(n2, "origin_rcd");
                    service.destination_rcd                     = XmlHelper.XpathValueNullToEmpty(n2, "destination_rcd");
                    service.display_name                        = XmlHelper.XpathValueNullToEmpty(n2, "display_name");

                    services.Add(service);
                }

                //if (passengerId == "")
                //{
                //    services.Add(service);
                //}
                //else
                //{
                //    if (service.passenger_id.ToString() == passengerId)
                //    {
                //        services.Add(service);
                //    }
                //}
               // services.Add(service);
            }

            return services;
        }

        private APIPassengerFees GetAPIPassengerFees(string bookingSegmentId, string strGetPassengerDetailXML, APIPassengerFees fees)
        {
            Library objLi = new Library();
            //APIPassengerFee fee;

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/Fee[booking_segment_id = '" + bookingSegmentId + "']"))
            {
                APIPassengerFee fee = new APIPassengerFee();
                fee.booking_fee_id                  = XmlHelper.XpathValueNullToGUID(n, "booking_fee_id");
                fee.fee_amount                      = XmlHelper.XpathValueNullToZero(n, "fee_amount");
                fee.booking_id                      = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                fee.passenger_id                    = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                fee.currency_rcd                    = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                fee.acc_fee_amount                  = XmlHelper.XpathValueNullToZero(n, "acc_fee_amount");
                fee.fee_id                          = XmlHelper.XpathValueNullToGUID(n, "fee_id");
                fee.fee_rcd                         = XmlHelper.XpathValueNullToEmpty(n, "fee_rcd");
                fee.display_name                    = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                fee.payment_amount                  = XmlHelper.XpathValueNullToZero(n, "payment_amount");
                fee.booking_segment_id              = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                fee.agency_code                     = XmlHelper.XpathValueNullToEmpty(n, "agency_code");
                fee.fee_category_rcd                = XmlHelper.XpathValueNullToEmpty(n, "fee_category_rcd");

                foreach (XPathNavigator n2 in nv.Select("Booking/Service[fee_id = '" + fee.fee_id.ToString() + "']"))
                {
                    fee.passenger_segment_service_id = XmlHelper.XpathValueNullToGUID(n2, "passenger_segment_service_id");
                }
                
                //fee.origin_rcd                      = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                //fee.destination_rcd                 = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");

                //foreach (XPathNavigator n2 in nv.Select("Booking/Mapping[booking_segment_id = '" + fee.booking_segment_id.ToString() + "'][passenger_id = '" + fee.passenger_id.ToString() + "']"))
                //{
                //    fee.od_origin_rcd               = XmlHelper.XpathValueNullToEmpty(n2, "od_origin_rcd");
                //    fee.od_destination_rcd          = XmlHelper.XpathValueNullToEmpty(n2, "od_destination_rcd");
                //}

                fees.Add(fee);
            }

            return fees;
        }

        private APIPassengerAddresses GetAPIPassengerAddresses(string bookingSegmentId, string strGetPassengerDetailXML, APIPassengerAddresses addresses)
        {
            Library objLi = new Library();

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/PassengerAddress[booking_segment_id = '" + bookingSegmentId + "']"))
            {
                APIPassengerAddress addr = new APIPassengerAddress();
                addr.passenger_address_id = XmlHelper.XpathValueNullToGUID(n, "passenger_address_id");
                addr.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                addr.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                addr.passenger_profile_id = XmlHelper.XpathValueNullToGUID(n, "passenger_profile_id");
                addr.address_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "address_type_rcd");
                addr.passenger_address = XmlHelper.XpathValueNullToEmpty(n, "passenger_address");
                addr.passenger_state = XmlHelper.XpathValueNullToEmpty(n, "passenger_state");
                addr.city = XmlHelper.XpathValueNullToEmpty(n, "city");
                addr.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "country_rcd");

                addresses.Add(addr);
            }

            return addresses;
        }

        private APIPassengerDocuments GetAPIPassengerDocuments(string bookingSegmentId, string strGetPassengerDetailXML, APIPassengerDocuments docs)
        {
            Library objLi = new Library();

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/PassengerDocument[booking_segment_id = '" + bookingSegmentId + "']"))
            {
                APIPassengerDocument doc = new APIPassengerDocument();
                doc.document_id = XmlHelper.XpathValueNullToGUID(n, "document_id");
                doc.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                doc.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                doc.document_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "document_type_rcd");
                doc.document_number = XmlHelper.XpathValueNullToEmpty(n, "document_number");
                doc.issue_place = XmlHelper.XpathValueNullToEmpty(n, "issue_place");
                doc.issue_country = XmlHelper.XpathValueNullToEmpty(n, "issue_country");
                doc.birth_place = XmlHelper.XpathValueNullToEmpty(n, "birth_place");
                doc.nationality_rcd = XmlHelper.XpathValueNullToEmpty(n, "nationality_rcd");
                doc.issue_date = XmlHelper.XpathValueNullToDateTime(n,"issue_date");
                doc.expiry_date = XmlHelper.XpathValueNullToDateTime(n, "expiry_date");
                doc.status_code = XmlHelper.XpathValueNullToEmpty(n, "status_code");

                docs.Add(doc);
            }

            return docs;
        }

        private APIPassengerBaggages GetDirectlyDbAPIPassengerBaggages(string baggageId, string passengerId, string flightId, string bookingId,string origin,string destination,string baggageStatus, APIPassengerBaggages bags)
        {
            Library objLi = new Library();
            DataSet ds;
            bool bTemp = false;

            if (!string.IsNullOrEmpty(bookingId))
            {
                // baggageId  passengerId  bookingId
                ds = GetBaggageTag(string.Empty, string.Empty, bookingId,"");
            }
            else if (!string.IsNullOrEmpty(passengerId))
            {
                ds = GetBaggageTag(string.Empty, passengerId,string.Empty,"");
            }
            else if (!string.IsNullOrEmpty(baggageId))
            {
                ds = GetBaggageTag(baggageId, string.Empty, string.Empty,"");
            }
            else // get by flight
            {
                ds = GetBaggageTagByFlight(flightId,origin,destination,baggageStatus);
                bTemp = true;
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                XPathDocument xmlDoc = new XPathDocument(new StringReader(ds.GetXml()));
                XPathNavigator nv = xmlDoc.CreateNavigator();

                foreach (XPathNavigator n in nv.Select("NewDataSet/Table"))
                {
                    APIPassengerBaggage bag = new APIPassengerBaggage();
                    bag.baggage_id = XmlHelper.XpathValueNullToGUID(n, "baggage_id");
                    bag.booking_id = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                    bag.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                    bag.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                    bag.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd");
                    bag.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                    bag.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                    bag.baggage_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_type_rcd");
                    bag.baggage_status_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_status_rcd");
                    bag.baggage_tag = XmlHelper.XpathValueNullToEmpty(n, "baggage_tag");
                    string count_print = XmlHelper.XpathValueNullToEmpty(n, "count_print");

                    bag.checkin_sequence_number = XmlHelper.XpathValueNullToEmpty(n, "check_in_sequence");

                    int count_printInt;

                    if (!string.IsNullOrEmpty(count_print))
                    {
                        if (!int.TryParse(count_print, out count_printInt))
                        {
                            // Handle the error if action_code is not a valid integer
                            throw new FormatException("The action_code is not a valid integer.");
                        }

                        if(count_printInt == 1)                        
                            bag.bagtag_print_status = "PRINTED";                       
                        else if(count_printInt > 1)
                            bag.bagtag_print_status = "REPRINTED";
                        else
                            bag.bagtag_print_status = "";

                        bag.number_of_bagtag_print = count_print;
                    }

                    /*
                    if (action_code.Equals("1"))
                        bag.bagtag_print_status = "PRINT";
                    else if (action_code.Equals("2"))
                        bag.bagtag_print_status = "REPRINT";
                    else
                        bag.bagtag_print_status = string.Empty;
*/
                  //  if (action_code.Equals("1"))
                     //   bag.bagtag_print_status = "REPRINTED";
                   // else if (action_code.Equals("0"))
                    //    bag.bagtag_print_status = "PRINTED";
                   // else
                     //   bag.bagtag_print_status = "PRINTED";

                    // avg_baggage_weight  baggage_weight
                    if (bTemp)
                    {
                        bag.baggage_weight = XmlHelper.XpathValueNullToEmpty(n, "avg_baggage_weight");
                    }
                    else
                    {
                        bag.baggage_weight = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight");
                    }

                    bag.baggage_weight_unit = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight_unit");
                    bag.airline_rcd = XmlHelper.XpathValueNullToEmpty(n, "airline_rcd");
                    bag.flight_number = XmlHelper.XpathValueNullToEmpty(n, "flight_number");
                    bag.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                    bag.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                    bag.departure_date = XmlHelper.XpathValueNullToDateTime(n, "departure_date");
                    bag.create_by = XmlHelper.XpathValueNullToGUID(n, "create_by");
                    bag.create_date_time = XmlHelper.XpathValueNullToDateTime(n, "create_date_time");
                    bag.update_by = XmlHelper.XpathValueNullToGUID(n, "update_by");
                    bag.update_date_time = XmlHelper.XpathValueNullToDateTime(n, "update_date_time");
                    bag.record_locator = XmlHelper.XpathValueNullToEmpty(n, "record_locator");

                    bags.Add(bag);
                }
            }

            return bags;
        }

        private APIPassengerBaggages GetDirectlyDbAPIPassengerBaggagesConnecting(string baggageId, string passengerId, string flightId, string bookingId, string origin, string destination, string baggageStatus, APIPassengerBaggages bags)
        {
            Library objLi = new Library();
            DataSet ds;
            bool bTemp = false;
            string bagtag_print_status = "";
            int number_of_bagtag_print = 0;

            if (!string.IsNullOrEmpty(bookingId))
            {
                // baggageId  passengerId  bookingId
                ds = GetBaggageTag(string.Empty, string.Empty, bookingId,"");
            }
            else if (!string.IsNullOrEmpty(passengerId))
            {
                ds = GetBaggageTag(string.Empty, passengerId, string.Empty,"");
            }
            else if (!string.IsNullOrEmpty(baggageId))
            {
                ds = GetBaggageTag(baggageId, string.Empty, string.Empty,"Y");
            }
            else // get by flight
            {
                ds = GetBaggageTagByFlight(flightId, origin, destination, baggageStatus);
                bTemp = true;
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                XPathDocument xmlDoc = new XPathDocument(new StringReader(ds.GetXml()));
                XPathNavigator nv = xmlDoc.CreateNavigator();

                foreach (XPathNavigator n in nv.Select("NewDataSet/Table"))
                {
                    APIPassengerBaggage bag = new APIPassengerBaggage();
                    bag.baggage_id = XmlHelper.XpathValueNullToGUID(n, "baggage_id");
                    bag.booking_id = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                    bag.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                    bag.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                    bag.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd");
                    bag.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                    bag.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                    bag.baggage_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_type_rcd");
                    bag.baggage_status_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_status_rcd");
                    bag.baggage_tag = XmlHelper.XpathValueNullToEmpty(n, "baggage_tag");
                    string count_print = XmlHelper.XpathValueNullToEmpty(n, "count_print");

                    bag.checkin_sequence_number = XmlHelper.XpathValueNullToEmpty(n, "check_in_sequence");

                    int count_printInt;

                    if (!string.IsNullOrEmpty(count_print))
                    {
                        if (!int.TryParse(count_print, out count_printInt))
                        {
                            // Handle the error if action_code is not a valid integer
                            throw new FormatException("The action_code is not a valid integer.");
                        }

                        if (count_printInt == 1)
                        {
                            bagtag_print_status = "PRINTED";
                            bag.bagtag_print_status = bagtag_print_status;
                        }
                        else if (count_printInt > 1)
                        {
                            bagtag_print_status = "REPRINTED";
                            bag.bagtag_print_status = bagtag_print_status;
                        }
                        else
                            bag.bagtag_print_status = bagtag_print_status;

                        if (count_printInt > 0)
                        {
                            number_of_bagtag_print = count_printInt;
                            bag.number_of_bagtag_print = number_of_bagtag_print + "";
                        }
                        else
                        {
                            bag.number_of_bagtag_print = number_of_bagtag_print + "";

                        }
                    }


                    // avg_baggage_weight  baggage_weight
                    if (bTemp)
                    {
                        bag.baggage_weight = XmlHelper.XpathValueNullToEmpty(n, "avg_baggage_weight");
                    }
                    else
                    {
                        bag.baggage_weight = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight");
                    }

                    bag.baggage_weight_unit = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight_unit");
                    bag.airline_rcd = XmlHelper.XpathValueNullToEmpty(n, "airline_rcd");
                    bag.flight_number = XmlHelper.XpathValueNullToEmpty(n, "flight_number");
                    bag.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                    bag.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                    bag.departure_date = XmlHelper.XpathValueNullToDateTime(n, "departure_date");
                    bag.create_by = XmlHelper.XpathValueNullToGUID(n, "create_by");
                    bag.create_date_time = XmlHelper.XpathValueNullToDateTime(n, "create_date_time");
                    bag.update_by = XmlHelper.XpathValueNullToGUID(n, "update_by");
                    bag.update_date_time = XmlHelper.XpathValueNullToDateTime(n, "update_date_time");
                    bag.record_locator = XmlHelper.XpathValueNullToEmpty(n, "record_locator");

                    bags.Add(bag);
                }
            }

            return bags;
        }

        private APIPassengerBaggages GetAPIPassengerBaggages(string bookingId, string bookingSegmentId, string passengerId, string strGetPassengerDetailXML, APIPassengerBaggages bags)
        {
            Library objLi = new Library();
            //APIPassengerFee fee;

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strGetPassengerDetailXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/Baggage[booking_segment_id = '" + bookingSegmentId + "'][passenger_id='" + passengerId + "']"))
            {
                APIPassengerBaggage bag = new APIPassengerBaggage();
                bag.baggage_id = XmlHelper.XpathValueNullToGUID(n, "baggage_id");
                bag.booking_id = new Guid(bookingId);
                bag.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                bag.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                bag.title_rcd = XmlHelper.XpathValueNullToEmpty(n, "title_rcd");
                bag.firstname = XmlHelper.XpathValueNullToEmpty(n, "firstname");
                bag.lastname = XmlHelper.XpathValueNullToEmpty(n, "lastname");
                bag.baggage_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_type_rcd");
                bag.baggage_status_rcd = XmlHelper.XpathValueNullToEmpty(n, "bagtag_status_rcd");
                bag.baggage_tag = XmlHelper.XpathValueNullToEmpty(n, "baggage_tag");
                bag.baggage_weight = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight");
                bag.baggage_weight_unit = XmlHelper.XpathValueNullToEmpty(n, "baggage_weight_unit");
                bag.airline_rcd = XmlHelper.XpathValueNullToEmpty(n, "airline_rcd");
                bag.flight_number = XmlHelper.XpathValueNullToEmpty(n, "flight_number");
                bag.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                bag.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                bag.departure_date = XmlHelper.XpathValueNullToDateTime(n, "departure_date");
                bag.create_by = XmlHelper.XpathValueNullToGUID(n, "create_by");
                bag.create_date_time = XmlHelper.XpathValueNullToDateTime(n, "create_date_time");
                bag.update_by = XmlHelper.XpathValueNullToGUID(n, "update_by");
                bag.update_date_time = XmlHelper.XpathValueNullToDateTime(n, "update_date_time");

                bags.Add(bag);
            }

            return bags;
        }


        private APIErrors GetAPIErrors(string code, APIErrors errors)
        {
            string message = string.Empty;

            string ServerName = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["ServerName"] != null)
            {
                ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            }


            switch (code)
            {
                case "000" :
                    message = "Successful transaction request";
                    break;
                case "100":
                    message = "Logon fail";
                    break;
                case "101":
                    message = "Not found booking logon information";
                    break;
                case "102":
                    message = "Have outstanding balance cannnot login";
                    break;
                case "103":
                    message = "Flight not open for checkin";
                    break;
                case "110":
                    message = "Invalid bookingref parameter";
                    break;
                case "111":
                    message = "Invalid lastname parameter";
                    break;
                case "112":
                    message = "Booking is locked.";
                    break;
                case "200":
                    message = "GetPassengers error";
                    break;
                case "201":
                    message = "Not found passenger information";
                    break;
                case "210":
                    message = "Invalid bookingsegmentid parameter";
                    break;
                case "300":
                    message = "AssignSeats error";
                    break;
                case "301":
                    message = "Passengers could not be assigned seat";
                    break;
                case "302":
                    message = "Required logon before assign seat";
                    break;
                case "303":
                    message = "Child or infant can't assign seat independently";
                    break;
                case "304":
                    message = "Number of adults need to be higher than the number of infants";
                    break;
                case "305":
                    message = "OFFLOADED passenger can't checkin";
                    break;
                case "306":
                    message = "Not enough seat to assign";
                    break;
                case "307":
                    message = "This flight is free-seating";
                    break;
                case "310":
                    message = "Invalid BookingSegmentId parameter";
                    break;
                case "311":
                    message = "Invalid userid parameter";
                    break;
                case "312":
                    message = "Invalid PassengerId parameter";
                    break;
                case "313":
                    message = "Passengers could not be zero";
                    break;
                case "314":
                    message = "Passengers already have been assigned seat";
                    break;
                case "315":
                    message = "Ticket number is empty";
                    break;
                case "400":
                    message = "Commit error";
                    break;
                case "401":
                    message = "Passengers could not checked in.";
                    break;
                case "402":
                    message = "Required assign seat before check in";
                    break;
                case "403":
                    message = "Child or infant can't checkin independently";
                    break;
                case "404":
                    message = "Number of adults need to be higher than the number of infants";
                    break;
                case "405":
                    message = "OFFLOADED passenger can't checkin";
                    break;
                case "407":
                    message = "Invalid passenger check in condition";
                    break;
                case "410":
                    message = "Invalid bookingsegmentid parameter";
                    break;
                case "411":
                    message = "Invalid passengerids parameter";
                    break;
                case "412":
                    message = "Some passengerids were committed already.";
                    break;
                case "413":
                    message = "Passenger NOSHOW can not check in.";
                    break;
                case "500":
                    message = "GetBoardingPassess error";
                    break;
                case "501":
                    message = "Required logon before get boarding pass";
                    break;
                case "502":
                    message = "Required check in before get boarding pass";
                    break;
                case "503":
                    message = "Not found passenger information";
                    break;
                case "510":
                    message = "Invalid bookingsegmentid parameter";
                    break;
                case "511":
                    message = "Invalid passengerids parameter";
                    break;
                case "600":
                    message = "Some of the passenger information can not be edit";
                    break;
                case "601":
                    message = "Passenger input not found";
                    break;
                case "602":
                    message = "Passenger ID is required";
                    break;
                case "603":
                    message = "Date of birth must be the past date";
                    break;
                case "604":
                    message = "Expiry date must be future date";
                    break;
                //getseatmap
                case "700":
                    message = "GetSeatMap error";
                    break;
                case "701":
                    message = "Required logon before select seat";
                    break;
                case "710":
                    message = "Parameter OriginRcd is empty";
                    break;
                case "711":
                    message = "Parameter DestionationRcd is empty";
                    break;
                case "712":
                    message = "Parameter FlightId is empty";
                    break;
                case "713":
                    message = "Parameter BoardingClass is empty";
                    break;
                case "714":
                    message = "Parameter BookingClass is empty";
                    break;
                case "715":
                    message = "No seat map information found";
                    break;
                //selectseat
                case "800":
                    message = "Select seat error";
                    break;
                case "801":
                    message = "Infant's seat does not match adult's seat";
                    break;
                case "802":
                    message = "Required logon before select seat";//"Required get passengers information before select seat";
                    break;
                case "803":
                    message = "Child or infant can't assign seat independently";
                    break;
                case "804":
                    message = "Number of adults need to be higher than the number of infants";
                    break;
                case "805":
                    message = "OFFLOADED passenger can't checkin";
                    break;
                case "806":
                    message = "Passengers already have been assigned seat";
                    break;
                case "807":
                    message = "This flight is free seating";
                    break;
                case "808":
                    message = "Passengers do not match booking_segment_id";
                    break;
                case "809":
                    message = "Duplicate seat selected";
                    break;
                case "810":
                    message = "SeatRequest is empty";
                    break;
                case "811":
                    message = "One of the request supply an invalid BookingSegmentId";
                    break;
                case "812":
                    message = "One of the request supply an invalid PassengerId";
                    break;
                case "813":
                    message = "Blocked seat can not be assigned.";
                    break;
                case "814":
                    message = "The seat selected is no longer available. Please make another seat selection.";
                    break;
                //selectseat
                case "900":
                    message = "Invalid sequence or seat parameter";
                    break;
                case "901":
                    message = "Passenger check in status is not CHECKED";
                    break;
                case "902":
                    message = "Passenger check in status is not BOARDED";
                    break;
                case "903":
                    message = "Cannot board or un-board passenger";
                    break;
                case "904":
                    message = "Cannot board passenger";
                    break;
                case "905":
                    message = "Cannot un-board passenger";
                    break;
                case "906":
                    message = "Invalid BookingId parameter";
                    break;
                case "907":
                    message = "BoardPassenger error";
                    break;
                case "908":
                    message = "Booking is not found";
                    break;
                case "909":
                    message = "Flight not open or closed for board or un-board";
                    break;
                case "910":
                    message = "Invalid token";
                    break;
                case "911":
                    message = "Booking is in use on AVANTIK";
                    break;
                case "912":
                    message = "Required assign seat before Boarded";
                    break;
                case "913":
                    message = "Mismatch seat number";
                    break;
                case "914":
                    message = "Mismatch checkin sequence";
                    break;
                case "950":
                    message = "Invalid initial token";
                    break;
                case "951":
                    message = "Invalid AgencyCode parameter";
                    break;
                case "952":
                    message = "Invalid AgencyLogon parameter";
                    break;
                case "953":
                    message = "Invalid AgencyPassword parameter";
                    break;
                case "954":
                    message = "Agency is not allowed to use web check-in api";
                    break;
                case "955":
                    message = "Flight not open or closed for offload";
                    break;
                case "956":
                    message = "Can not offload passenger";
                    break;
                case "957":
                    message = "Logon Required";
                    break;
                case "958":
                    message = "Document Type is invalid";
                    break;
                case "959":
                    message = "Initial token timeout.";
                    break;
                case "960":
                    message = "Get token failed.";
                    break;
                case "961":
                    message = "Can not NOSHOW passenger";
                    break;
                case "1001":
                    message = "Status should be 0,1,2,3,4,5 OR empty, that 0=RESET, 1=OPEN, 2=CLOSED, 3=DISPATCHED,4=FLOWN,5=RESETFLOWN OR ignore blank value on update.";
                    break;
                case "1002":
                    message = "Update fail";
                    break;
                case "1003":
                    message = "Invalid flight id format";
                    break;
                case "1004":
                    message = "Flight not found";
                    break;
                case "1005":
                    message = "Required Flight Id";
                    break;
                case "1006":
                    message = "Required Origin";
                    break;
                case "1007":
                    message = "Required Destination";
                    break;
                case "1008":
                    message = "Required Airline Code";
                    break;
                case "1009":
                    message = "Required Flight Number";
                    break;
                case "1010":
                    message = "Required Departure Date";
                    break;
                case "1011":
                    message = "Invalid time format, time format should be between 0000 - 2359";
                    break;
                case "1012":
                    message = "Invalid request format,allow_web_checkin should be true or false";
                    break;
                case "1013":
                    message = "Invalid datetime format,departureDate should be date format, ex: 2018-12-30";
                    break;
                case "1014":
                    message = "Required Airport Code";
                    break;
                case "1015":
                    message = "Invalid Code.";
                    break;
                //baggage
                case "1100":
                    message = "Process Baggage error";
                    break;
                case "1101":
                    message = "BookingSegmentId is empty or invalid Guid format";
                    break;
                case "1102":
                    message = "PassengerId is empty or invalid Guid format";
                    break;
                case "1103":
                    message = "BaggageWeight should be between 1 - 100";
                    break;
                case "1104":
                    message = "BaggageType is empty";
                    break;
                case "1105":
                    message = "BaggageTagStatus is empty";
                    break;
                case "1106":
                    message = "AirlineCode is empty";
                    break;
                case "1107":
                    message = "NumberOfBaggage must be numeric";
                    break;
                case "1108":
                    message = "NumberOfBaggage should be 1 - 10";
                    break;
                case "1109":
                    message = "Invalid AirlineCode";
                    break;
                case "1110":
                    message = "BookingId or BaggageId or PassengerId is required.";
                    break;
                case "1111":
                    message = "BaggageId is invalid Guid format";
                    break;
                case "1112":
                    message = "PassengerId is invalid Guid format";
                    break;
                case "1113":
                    message = "FlightId is invalid Guid format";
                    break;
                case "1114":
                    message = "Baggage not found.";
                    break;
                case "1115":
                    message = "Final destination code is required.";
                    break;
                case "1116":
                    message = "Final destination code is required for transit flight.";
                    break;
                case "1117":
                    message = "Please provide only one of parameters: BookingId or PassengerId or BaggageId.";
                    break;
                case "1118":
                    message = "Please provide only one of parameters: FlightId or Origin or Destination od BaggageStatus.";
                    break;
                case "1119":
                    message = "Update Baggage fail.";
                    break; 
                case "1120":
                    message = "Baggage unit should be KGS.";
                    break;
                case "1121":
                    message = "Baggage weight should be 1 decimal place.";
                    break;
                case "1122":
                    message = "At lease one of the update element value is required.";
                    break;
                case "1123":
                    message = "Invalid origin and destination code."; 
                    break;
                case "1124":
                    message = "Flight FLOWN already,It can not set RESET,OPEN or CLOSED status.";
                    break;
                case "1125":
                    message = "Flight checkin status can not set RESETFLOWN.";
                    break;
                case "1126":
                    message = "All passengers should be BOARDED before set flight checkin status is DISPATCHED/FLOWN.";
                    break;
                case "1127":
                    message = "Please provide final destination.";
                    break;
                case "1128":
                    message = "Process baggage error";
                    break;
                case "3122":
                    message = "Request data is invalid.";
                    break;
                case "9999":
                    message = "API connection error.";
                    break;
                default:
                    message = "Internal error.";
                    break;
            }

            APIError error = new APIError();
            error.code = code;
            error.message = message;
            error.serverId = ServerName;
            errors.Add(error);

            return errors;
        }

        private APIErrors GetAPIErrors(string code, string newmessage, APIErrors errors)
        {
            string message = string.Empty;
            APIError error = new APIError();
            error.code = code;
            error.message = newmessage;
            errors.Add(error);
            return errors;
        }

        private APISeatMaps GetAPISeatMaps(string flight_id, string strSeatMapXML, APISeatMaps seatmaps)
        {
            Library objLi = new Library();
            //APISeatMap seatmap;

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strSeatMapXML));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            XPathNodeIterator nodes = nv.Select("SeatMaps/SeatMap[flight_id = '" + flight_id + "']");
            if (nodes.Count == 0)
            {
                //seatmap layout for transit flight
                nodes = nv.Select("SeatMapLayout/Attribute[flight_id = '" + flight_id + "']");
            }
            //foreach (XPathNavigator n in nv.Select("SeatMaps/SeatMap[flight_id = '" + flight_id + "']"))
            foreach (XPathNavigator n in nodes)
            {
                APISeatMap seatmap = new APISeatMap();
                seatmap.flight_id = XmlHelper.XpathValueNullToGUID(n, "flight_id");
                seatmap.free_seating_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "free_seating_flag").ToString());
                seatmap.flight_check_in_status_rcd = XmlHelper.XpathValueNullToEmpty(n, "flight_check_in_status_rcd");
                seatmap.origin_rcd = XmlHelper.XpathValueNullToEmpty(n, "origin_rcd");
                seatmap.destination_rcd = XmlHelper.XpathValueNullToEmpty(n, "destination_rcd");
                seatmap.aircraft_configuration_code = XmlHelper.XpathValueNullToEmpty(n, "aircraft_configuration_code");
                seatmap.number_of_bays = int.Parse(XmlHelper.XpathValueNullToZero(n, "number_of_bays").ToString());
                seatmap.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd");
                seatmap.number_of_rows = int.Parse(XmlHelper.XpathValueNullToZero(n, "number_of_rows").ToString());
                seatmap.number_of_columns = int.Parse(XmlHelper.XpathValueNullToZero(n, "number_of_columns").ToString());
                seatmap.layout_row = int.Parse(XmlHelper.XpathValueNullToZero(n, "layout_row").ToString());
                seatmap.layout_column = int.Parse(XmlHelper.XpathValueNullToZero(n, "layout_column").ToString());
                seatmap.location_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "location_type_rcd");
                seatmap.seat_column = XmlHelper.XpathValueNullToEmpty(n, "seat_column");
                seatmap.seat_row = XmlHelper.XpathValueNullToEmpty(n, "seat_row");
                seatmap.stretcher_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "stretcher_flag").ToString());
                seatmap.handicapped_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "handicapped_flag").ToString());
                seatmap.no_child_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "no_child_flag").ToString());
                seatmap.bassinet_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "bassinet_flag").ToString());
                seatmap.no_infant_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "no_infant_flag").ToString());
                seatmap.infant_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "infant_flag").ToString());
                seatmap.emergency_exit_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "emergency_exit_flag").ToString());
                seatmap.unaccompanied_minors_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "unaccompanied_minors_flag").ToString());
                seatmap.window_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "window_flag").ToString());
                seatmap.aisle_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "aisle_flag").ToString());
                seatmap.block_b2c_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "block_b2c_flag").ToString());
                seatmap.block_b2b_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "block_b2b_flag").ToString());
                seatmap.blocked_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "blocked_flag").ToString());
                seatmap.low_comfort_flag = int.Parse(XmlHelper.XpathValueNullToZero(n, "low_comfort_flag").ToString());
                seatmap.passenger_count = int.Parse(XmlHelper.XpathValueNullToZero(n, "passenger_count").ToString());

                seatmaps.Add(seatmap);
            }

            return seatmaps;
        }

        public DataSet CacheDestination()
        {
            DataSet ds = new DataSet();
            try
            {
                Library objLi = new Library();
                ds = (DataSet)HttpRuntime.Cache["destination-" + tikAEROWebCheckinAPI.Classes.Language.CurrentCode().ToUpper()];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    ServiceClient objClient = new ServiceClient();

                    Booking objBooking = new Booking();
                    //ds = objClient.GetSessionlessDestination(tikAEROWebCheckinAPI.Classes.Language.CurrentCode().ToUpper(), true, false, false, false, false, objBooking.GenerateSessionlessToken());
                    ds = objClient.GetSessionlessDestination(tikAEROWebCheckinAPI.Classes.Language.CurrentCode().ToUpper(), true, false, false, false, false, SecurityHelper.GenerateSessionlessToken());

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        HttpRuntime.Cache.Insert("destination-" + tikAEROWebCheckinAPI.Classes.Language.CurrentCode().ToUpper(), ds, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    return ds;
                }
            }
            catch (Exception ex)
            {
                //Helper objHp = new Helper();
                //objHp.SendErrorEmail(ex, string.Empty);
                throw ex;
            }
        }

        private bool IsSuccess(APIResult result)
        {
            bool bResult = false;
            if (result != null && result.APIErrors[0].code.Equals("000"))
            {
                bResult = true;
            }
            return bResult;
        }

        private bool IsClearSeat(Mappings mappings)
        {
            bool bResult = false;
            if (mappings != null)
            {
                foreach(Mapping mapping in mappings)
                {
                    if (!string.IsNullOrEmpty(mapping.seat_number))
                    {
                        mapping.seat_number = string.Empty;
                        mapping.seat_row    = 0;
                        mapping.seat_column = string.Empty;
                    }
                }

                Session["mappings"] = mappings;
                bResult = true;
            }
            return bResult;
        }

        private bool CheckFlightConnection(Itinerary itinerary)
        {
            bool isFlightConnection = false;
           
            if (itinerary != null)
            {
                foreach (object item in itinerary)
                {
                    FlightSegment fs = item as FlightSegment;
                    if (fs != null && fs.flight_connection_id != Guid.Empty)
                    {
                        isFlightConnection = true;
                        break;
                    }
                }
            }

            return isFlightConnection;
        }

        private string  FingFinalDestination(Itinerary itinerary, string final_destination_rcd)
        {
            string final_flight_number = string.Empty;

            if (itinerary != null)
            {
                foreach (object item in itinerary)
                {
                    FlightSegment fs = item as FlightSegment;
                    if (fs != null && fs.destination_rcd.Equals(final_destination_rcd))
                    {
                        final_flight_number = fs.flight_number;
                        break;
                    }
                }
            }

            return final_flight_number;
        }

     



        #endregion

    }
}
