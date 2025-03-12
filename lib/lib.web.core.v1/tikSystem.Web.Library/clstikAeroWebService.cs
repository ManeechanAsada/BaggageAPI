using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;
using System.Text;
using System.Reflection;
using tikSystem.Web.Library.FlifoService;

namespace tikSystem.Web.Library
{
    public class clstikAeroWebService : IClientCore
    {
        #region IClientCore Members

        public System.Data.DataSet GetAirport(string language)
        {
            DataSet ds = null;
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            if (language.Length == 0)
            { language = "EN"; }

            ds = objService.GetAirlines(language, CreateToken());
            if (objService != null)
            {
                objService = null;
            }

            return ds;
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
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Route[] wsRoutes = objService.GetOrigins(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, CreateToken());
                    if (wsRoutes != null && wsRoutes.Length > 0)
                    {
                        Route r;
                        for (int i = 0; i < wsRoutes.Length; i++)
                        {
                            r = new Route();
                            r.origin_rcd = wsRoutes[i].origin_rcd;
                            r.display_name = wsRoutes[i].display_name;
                            r.country_rcd = wsRoutes[i].country_rcd;
                            r.currency_rcd = wsRoutes[i].currency_rcd;
                            r.routes_tot = wsRoutes[i].routes_tot;
                            r.routes_avl = wsRoutes[i].routes_avl;
                            r.routes_b2c = wsRoutes[i].routes_b2c;
                            r.routes_b2b = wsRoutes[i].routes_b2b;
                            r.routes_b2s = wsRoutes[i].routes_b2s;
                            r.routes_api = wsRoutes[i].routes_api;
                            r.routes_b2t = wsRoutes[i].routes_b2t;
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
                Routes routes = null;

                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    if (string.IsNullOrEmpty(language))
                    {
                        language = "EN";
                    }
                    tikAeroWebService.Route[] wsRoutes = objService.GetDestination(language, b2cFlag, b2bFlag, b2eFlag, b2sFlag, apiFlag, CreateToken());
                    if (wsRoutes != null && wsRoutes.Length > 0)
                    {
                        Route r;
                        routes = new Routes();
                        for (int i = 0; i < wsRoutes.Length; i++)
                        {
                            r = new Route();
                            r.origin_rcd = wsRoutes[i].origin_rcd;
                            r.destination_rcd = wsRoutes[i].destination_rcd;
                            r.segment_change_fee_flag = wsRoutes[i].segment_change_fee_flag;
                            r.transit_flag = wsRoutes[i].transit_flag;
                            r.direct_flag = wsRoutes[i].direct_flag;
                            r.avl_flag = wsRoutes[i].avl_flag;
                            r.b2c_flag = wsRoutes[i].b2c_flag;
                            r.b2b_flag = wsRoutes[i].b2b_flag;
                            r.b2t_flag = wsRoutes[i].b2t_flag;
                            r.day_range = wsRoutes[i].day_range;
                            r.show_redress_number_flag = wsRoutes[i].show_redress_number_flag;
                            r.require_passenger_title_flag = wsRoutes[i].require_passenger_title_flag;
                            r.require_passenger_gender_flag = wsRoutes[i].require_passenger_gender_flag;
                            r.require_date_of_birth_flag = wsRoutes[i].require_date_of_birth_flag;
                            r.require_document_details_flag = wsRoutes[i].require_document_details_flag;
                            r.require_passenger_weight_flag = wsRoutes[i].require_passenger_weight_flag;
                            r.special_service_fee_flag = wsRoutes[i].special_service_fee_flag;
                            r.show_insurance_on_web_flag = wsRoutes[i].show_insurance_on_web_flag;
                            r.display_name = wsRoutes[i].display_name;
                            r.currency_rcd = wsRoutes[i].currency_rcd;
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

        public System.Data.DataSet GetAgencyCode(string agencyCode)
        {
            string strResult = string.Empty;
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            DataSet ds = new DataSet();
            strResult = objService.GetAgencyCode(agencyCode, CreateToken());
            ds.ReadXml(new StringReader(strResult));
            if (objService != null)
            {
                objService = null;
            }

            return ds;
        }

        public bool ReleaseFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock)
        {
            bool result = false;
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            result = objService.ReleaseFlightInventorySession(sessionId,
                                                            flightId,
                                                            bookingClasss,
                                                            bookingId,
                                                            releaseTimeOut,
                                                            ReleaseInventory,
                                                            ReleaseBookingLock,
                                                            CreateToken());
            if (objService != null)
            {
                objService = null;
            }

            return result;
        }
        public bool ReleaseSessionlessFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock, string strToken)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    return objService.ReleaseFlightInventorySession(bookingId, "", "", "", false, true, false, CreateToken());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string FlightAdd(string agencyCode, string currency, string flightXml, string bookingID, short adults, short children, short infants, short others, string strOthers, string userId, string strIpAddress, string strLanguageCode, bool bNoVat)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.AddFlight(agencyCode, currency, flightXml, bookingID, adults, children, infants, others, strOthers, userId, CreateToken(), strIpAddress, strLanguageCode, bNoVat);

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string GetClient(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.GetClient(clientId, clientNumber, passengerId, bShowRemark, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public Titles GetPassengerTitles(string language)
        {
            try
            {
                Titles titles = new Titles();
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Title[] titlesService = objService.GetPassengerTitles(language, CreateToken());
                    if (titlesService != null && titlesService.Length > 0)
                    {
                        Title t;
                        for (int i = 0; i < titlesService.Length; i++)
                        {
                            t = new Title();

                            t.title_rcd = titlesService[i].title_rcd;
                            t.display_name = titlesService[i].display_name;
                            t.gender_type_rcd = titlesService[i].gender_type_rcd;

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
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Document[] docs = objService.GetDocumentType(language, CreateToken());
                    if (docs != null && docs.Length > 0)
                    {
                        Document d;
                        for (int i = 0; i < docs.Length; i++)
                        {
                            d = new Document();

                            d.document_type_rcd = docs[i].document_type_rcd;
                            d.display_name = docs[i].display_name;

                            documents.Add(d);
                            d = null;
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
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Country[] country = objService.GetCountry(language, CreateToken());
                    if (country != null && country.Length > 0)
                    {
                        Country c;
                        for (int i = 0; i < country.Length; i++)
                        {
                            c = new Country();

                            c.country_rcd = country[i].country_rcd;
                            c.display_name = country[i].display_name;
                            c.status_code = country[i].status_code;
                            c.currency_rcd = country[i].currency_rcd;
                            c.vat_display = country[i].vat_display;
                            c.country_code_long = country[i].country_code_long;
                            c.country_number = country[i].country_number;
                            c.phone_prefix = country[i].phone_prefix;
                            c.issue_country_flag = country[i].issue_country_flag;
                            c.residence_country_flag = country[i].residence_country_flag;
                            c.nationality_country_flag = country[i].nationality_country_flag;
                            c.address_country_flag = country[i].address_country_flag;
                            c.agency_country_flag = country[i].agency_country_flag;

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
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Language[] objLanguage = objService.GetLanguages(language, CreateToken());
                    if (objLanguage != null && objLanguage.Length > 0)
                    {
                        Language l;
                        for (int i = 0; i < objLanguage.Length; i++)
                        {
                            l = new Language();

                            l.language_rcd = objLanguage[i].language_rcd;
                            l.display_name = objLanguage[i].display_name;
                            l.character_set = objLanguage[i].character_set;

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

        public System.Data.DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass, string strLanguage)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            DataSet ds;

            ds = objService.GetSeatMap(origin, destination, flightId, boardingClass, bookingClass, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return ds;
        }

        public string GetFormOfPayments(string language)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.GetFormOfPayments(language, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string GetFormOfPaymentSubTypes(string type, string language)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.GetFormOfPaymentSubTypes(type, language, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string SaveBooking(string bookingId, string header, string segment, string passenger, string remark, string payment, string mapping, string service, string tax, string fee, bool createTickets, bool readBooking, bool readOnly, string strLanguage)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            string result = string.Empty;
            string strBooking = "<Booking>" +
                                 header +
                                 segment +
                                 passenger +
                                 remark +
                                 payment +
                                 mapping +
                                 service +
                                 tax +
                                 fee +
                                 "</Booking>";

            result = objService.SaveBooking(SecurityHelper.CompressString(strBooking), createTickets, readBooking, readOnly, CreateToken());

            if (objService != null)
            {
                objService = null;
            }
            if (result.Length > 0)
            {
                return SecurityHelper.DecompressString(result);
            }
            else
            {
                return string.Empty;
            }
        }

        public bool SaveBookingHeader(string header)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            return false;
        }

        public string CalculateNewFees(string bookingId, string AgencyCode, string header, string segment, string passenger, string fees, string currency, string remark, string payment, string mapping, string service, string tax, bool checkBooking, bool checkSegment, bool checkName, bool checkSeat, string strLanguage, bool bNoVat)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            DataSet ds = new DataSet();

            string result = string.Empty;
            string strBooking = "<Booking>" +
                                 header +
                                 segment +
                                 passenger +
                                 remark +
                                 payment +
                                 mapping +
                                 service +
                                 tax +
                                 fees +
                                 "</Booking>";

            ds.ReadXml(new StringReader(strBooking));
            result = objService.CalculateNewFees(AgencyCode, currency, ds, checkBooking, checkSegment, checkName, checkSeat, false, strLanguage, CreateToken(), bNoVat);

            ds.Dispose();

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string GetVouchers(string recordLocator, string voucherNumber, string voucherID, string status, string recipient, string fOPSubType, string clientProfileId, string currency, string password, bool includeOpenVoucher, bool includeExpiredVoucher, bool includeUsedVoucher, bool includeVoidedVoucher, bool includeRefundable, bool includeFareOnly, bool write, Mappings mappings, Fees fees)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = null;

            result = objService.GetVouchers(recordLocator, voucherNumber, voucherID, status, recipient, fOPSubType, clientProfileId
                , currency, password, includeOpenVoucher, includeExpiredVoucher, includeUsedVoucher, includeVoidedVoucher
                , includeRefundable, includeFareOnly, write, string.Empty, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public bool SavePayment(string bookingId, Mappings mappings, Fees fees, Payments payment, Vouchers refundVoucher)
        {
            bool bResult = false;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenUser"]) == false & string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"]) == false)
            {
                string strKey = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"] + System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"];
                tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
                Library objLi = new Library();

                Allocations objAllocation = objLi.GetPaymentAllocation(mappings, fees, payment[0].update_by);

                string strBooking = "<Booking>" +
                                         XmlHelper.Serialize(payment, false) +
                                         XmlHelper.Serialize(objAllocation, false) +
                                         XmlHelper.Serialize(refundVoucher, false) +
                                     "</Booking>";

                bResult = objService.PaymentSave(SecurityHelper.EncryptString(strBooking, strKey), CreateToken());

                if (objService != null)
                {
                    objService = null;
                }
            }

            return bResult;
        }

        public System.Data.DataSet GetFormOfPaymentSubtypeFees(string formOfPayment, string formOfPaymentSubtype, string currencyRcd, string agency, DateTime feeDate)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            DataSet ds = null;

            ds = objService.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubtype, currencyRcd, agency, feeDate, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return ds;
        }

        public string GetItinerary(string bookingId, string language, string passengerId, string agencyCode)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = null;

            result = objService.GetItinerary(bookingId, language, passengerId, agencyCode, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string ItineraryRead(string recordLocator, string language, string passengerId, string agencyCode)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = null;

            result = objService.ItineraryRead(recordLocator, language, passengerId, agencyCode, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public bool QueueMail(string strFromAddress, string strFromName, string strToAddress, string strToAddressCC, string strToAddressBCC, string strReplyToAddress, string strSubject, string strBody, string strDocumentType, string strAttachmentStream, string strAttachmentFileName, string strAttachmentFileType, string strAttachmentParser, bool bHtmlBody, bool bConvertAttachmentFromHTML2PDF, bool bRemoveFromQueue, string strUserId, string strBookingId, string strVoucherId, string strBookingSegmentID, string strPassengerId, string strClientProfileId, string strDocumentId, string strLanguageCode)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            bool result = false;

            //Compress Body string.

            result = objService.QueueMail(strFromAddress,
                                     strFromName,
                                     strToAddress,
                                     strToAddressCC,
                                     strToAddressBCC,
                                     strReplyToAddress,
                                     strSubject,
                                     SecurityHelper.CompressString(strBody),
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
                                     strLanguageCode,
                                     CreateToken());

            if (objService != null)
            {
                objService = null;
            }
            return result;
        }

        public System.Data.DataSet GetClientPassenger(string bookingId, string clientProfileId, string clientNumber)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            DataSet result = null;

            result = objService.GetClientPassenger(bookingId, clientProfileId, clientNumber, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public string ServiceAuthentication(string agencyCode, string agencyLogon, string agencyPasseword, string selectedAgency)
        {
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenUser"]) == false & string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"]) == false)
            {
                string strKey = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"] + System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"];
                tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
                string strAuthen = "<Authen>" +
                                        "<agencyCode>" + agencyCode + "</agencyCode>" +
                                        "<strUsername>" + agencyLogon + "</strUsername>" +
                                        "<password>" + agencyPasseword + "</password>" +
                                    "</Authen>";
                string result = null;

                result = objService.ServiceAuthentication(SecurityHelper.EncryptString(strAuthen, strKey), CreateToken());

                if (objService != null)
                {
                    objService = null;
                }

                return SecurityHelper.DecryptString(result, strKey);
            }
            else
            {
                return string.Empty;
            }
        }

        public System.Data.DataSet ClientLogon(string ClientNumber, string ClientPassword)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result;
            DataSet ds = new DataSet();

            result = objService.ClientLogon(ClientNumber, ClientPassword, CreateToken());
            if (string.IsNullOrEmpty(result) == false)
            {
                using (StringReader sr = new StringReader(result))
                {
                    ds.ReadXml(sr);
                    ds.DataSetName = "Booking";
                }
            }
            if (objService != null)
            {
                objService = null;
            }

            return ds;
        }

        public System.Data.DataSet GetBookings(string Airline,
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
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                ds = objService.GetBookings(Airline,
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
                                            CreateTo,
                                            CreateToken());

            }
            return ds;
        }

        public System.Data.DataSet GetBookingsThisUser(string agencyCode, string userId, string airline, string flightNumber, DateTime flightFrom, DateTime flightTo, string recordLocator, string origin, string destination, string passengerName, string seatNumber, string ticketNumber, string phoneNumber, DateTime createFrom, DateTime createTo)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet ClientRead(string clientProfileID)
        {
            throw new NotImplementedException();
        }

        public bool AddClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    string strXml = "<ClientProfile>" +
                                        XmlHelper.Serialize(client, false) +
                                        XmlHelper.Serialize(passengers, false) +
                                        XmlHelper.Serialize(remarks, false) +
                                    "</ClientProfile>";
                    return objService.AddClientProfile(strXml,
                                                       CreateToken());
                }
            }
            catch
            {
                throw;
            }
        }

        public bool EditClientProfile(Client client, Passengers passengers, Remarks remarks)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    string strXml = "<ClientProfile>" +
                                        XmlHelper.Serialize(client, false) +
                                        XmlHelper.Serialize(passengers, false) +
                                        XmlHelper.Serialize(remarks, false) +
                                    "</ClientProfile>";

                    return objService.EditClientProfile(strXml,
                                                        CreateToken());
                }
            }
            catch
            {
                throw;
            }
        }

        public bool ClientSave(string xmlClient, string xmlPassenger, string xmlBookingRemark)//ClientProfile clientProfileRequest
        {
            throw new NotImplementedException();
        }
        public bool AddClientPassengerList(string xmlClient, string xmlPassenger, string xmlBookingRemark)
        {
            throw new NotImplementedException();
        }
        public string GetTransaction(string strOrigin, string strDestination, string strAirline, string strFlight, string strSegmentType, string strClientProfileId, string strPassengerProfileId, string strVendor, string strCreditDebit, DateTime dtFlightFrom, DateTime dtFlightTo, DateTime dtTransactionFrom, DateTime dtTransactionTo, DateTime dtExpiryFrom, DateTime dtExpiryTo, DateTime dtVoidFrom, DateTime dtVoidTo, int iBatch, bool bAllVoid, bool bAllExpired, bool bAuto, bool bManual, bool bAllPoint)
        {
            //throw new NotImplementedException();
            DataSet ds = null;
            short sBatch = 0;
            short.TryParse(iBatch.ToString(), out sBatch);
            using (tikAeroWebService.tikAeroWebService objSevice = new tikAeroWebService.tikAeroWebService())
            {
                ds = objSevice.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom, dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo, dtVoidFrom, dtVoidTo, sBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint, CreateToken());
            }
            return ds.GetXml();
        }

        public System.Data.DataSet TicketRead(ref string strBookingId, ref string strPassengerId, ref string strSegmentId, ref string strTicketNumber, ref string xmlTaxes, ref bool bReadOnly, ref bool bReturnTax)
        {
            throw new NotImplementedException();
        }

        public bool CheckUniqueMailAddress(string strMail, string strClientProfileId)
        {
            bool success = false;
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                success = objService.CheckUniqueMailAddress(strMail, strClientProfileId, CreateToken());

            }
            return success;
        }

        public System.Data.DataSet AccuralQuote(string strPassenger, string strMapping, string strClientProfileId)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet GetFlightDailyCount(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo)
        {
            throw new NotImplementedException();
        }
        public string GetFlightDailyCountXML(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo, string strToken)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet GetBinRangeSearch(string strCardType, string strStatusCode)
        {
            throw new NotImplementedException();
        }
        public System.Data.DataSet GetSessionlessBinRangeSearch(string strCardType, string strStatusCode, string strToken)
        {
            throw new NotImplementedException();
        }
        public tikAeroWebService.WsWrapper BookingLogon(string strRecordLocator, string strNameOrPhone, string strAgencyCode)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            return objService.BookingLogon(strRecordLocator, strNameOrPhone, strAgencyCode, CreateToken());
        }

        public string GetBookingSegmentCheckIn(string strBookingId, string strClientId, string strLanguageCode)
        {
            throw new NotImplementedException();
        }

        public string GetPassengerDetails(string strPassengerId, string strBookingSegmentId, string strFlightId, string strCheckinPassengers, bool bPassenger, bool bRemarks, bool bService, bool bBaggage, bool bSegment, bool bFee, bool bBookingDetails, string strLangaugeCode, string strOrigin)
        {
            throw new NotImplementedException();
        }

        public bool CheckInSave(string strMappingXml, string strBaggageXml, string strSeatAssignmentXml, string strPassengerXml, string strServiceXml, string strRemarkXml, string strBookingSegmentXml, string strFeeXml)
        {
            throw new NotImplementedException();
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
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            string result = string.Empty;
            //Encrypr CC card number
            //EncryptCCValue(payment);
            string strBooking = "<Booking>" +
                                XmlHelper.Serialize(header, false) +
                                XmlHelper.Serialize(segment, false) +
                                XmlHelper.Serialize(passenger, false) +
                                XmlHelper.Serialize(remark, false) +
                                XmlHelper.Serialize(payment, false) +
                                XmlHelper.Serialize(mapping, false) +
                                XmlHelper.Serialize(service, false) +
                                XmlHelper.Serialize(tax, false) +
                                XmlHelper.Serialize(fee, false) +
                                XmlHelper.Serialize(paymentFee, false).Replace("<ArrayOfFee>", "<ArrayOfPaymentFee>").Replace("</ArrayOfFee>", "</ArrayOfPaymentFee>").Replace("<Fee>", "<PaymentFee>").Replace("</Fee>", "</PaymentFee>") +
                                "</Booking>";

            result = objService.SaveBookingCreditCard(SecurityHelper.CompressString(strBooking),
                                                      securityToken,
                                                      authenticationToken,
                                                      commerceIndicator,
                                                      bookingReference,
                                                      requestSource,
                                                      createTickets,
                                                      readBooking,
                                                      readOnly,
                                                      CreateToken());

            if (objService != null)
            {
                objService = null;
            }
            if (result.Length > 0)
            {
                return SecurityHelper.DecompressString(result);
            }
            else
            {
                return string.Empty;
            }
        }

        public string SaveBookingPayment(string bookingId, BookingHeader header, Itinerary segment, Passengers passenger, Remarks remark, Payments payment, Mappings mapping, Services service, Taxes tax, Fees fee, Fees paymentFee, bool createTickets, bool readBooking, bool readOnly, string strLanguage)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            string result = string.Empty;
            string strBooking = "<Booking>" +
                                XmlHelper.Serialize(header, false) +
                                XmlHelper.Serialize(segment, false) +
                                XmlHelper.Serialize(passenger, false) +
                                XmlHelper.Serialize(remark, false) +
                                XmlHelper.Serialize(payment, false) +
                                XmlHelper.Serialize(mapping, false) +
                                XmlHelper.Serialize(service, false) +
                                XmlHelper.Serialize(tax, false) +
                                XmlHelper.Serialize(fee, false) +
                                XmlHelper.Serialize(paymentFee, false).Replace("<ArrayOfFee>", "<ArrayOfPaymentFee>").Replace("</ArrayOfFee>", "</ArrayOfPaymentFee>").Replace("<Fee>", "<PaymentFee>").Replace("</Fee>", "</PaymentFee>") +
                                "</Booking>";


            result = objService.SaveBookingPayment(SecurityHelper.CompressString(strBooking),
                                                    createTickets,
                                                    readBooking,
                                                    readOnly,
                                                    CreateToken());

            if (objService != null)
            {
                objService = null;
            }
            if (result.Length > 0)
            {
                return SecurityHelper.DecompressString(result);
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
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            string result = string.Empty;
            string strBooking = "<Booking>" +
                                XmlHelper.Serialize(header, false) +
                                XmlHelper.Serialize(segment, false) +
                                XmlHelper.Serialize(passenger, false) +
                                XmlHelper.Serialize(remark, false) +
                                XmlHelper.Serialize(payment, false) +
                                XmlHelper.Serialize(mapping, false) +
                                XmlHelper.Serialize(service, false) +
                                XmlHelper.Serialize(tax, false) +
                                XmlHelper.Serialize(fee, false) +
                                XmlHelper.Serialize(paymentFee, false).Replace("<ArrayOfFee>", "<ArrayOfPaymentFee>").Replace("</ArrayOfFee>", "</ArrayOfPaymentFee>").Replace("<Fee>", "<PaymentFee>").Replace("</Fee>", "</PaymentFee>") +
                                "</Booking>";


            result = objService.SaveBookingMultipleFormOfPayment(bookingId,
                                                                 SecurityHelper.CompressString(strBooking),
                                                                 createTickets,
                                                                 securityToken,
                                                                 authenticationToken,
                                                                 commerceIndicator,
                                                                 strRequestSource,
                                                                 strLanguage,
                                                                 CreateToken());

            if (objService != null)
            {
                objService = null;
            }
            if (result.Length > 0)
            {
                return SecurityHelper.DecompressString(result);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetServiceFeesByGroups(BookingHeader header, Itinerary itinerary, string serviceGroup)
        {
            return string.Empty;
        }
        public Users TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword)
        {
            try
            {
                Users users = new Users();
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.User[] serviceUsers = objService.TravelAgentLogon(agencyCode, agentLogon, agentPassword, CreateToken());
                    if (serviceUsers != null && serviceUsers.Length > 0)
                    {
                        User u;
                        for (int i = 0; i < serviceUsers.Length; i++)
                        {
                            u = new User();
                            u.user_account_id = serviceUsers[i].user_account_id;
                            u.user_logon = serviceUsers[i].user_logon;
                            u.lastname = serviceUsers[i].lastname;
                            u.firstname = serviceUsers[i].firstname;
                            u.email_address = serviceUsers[i].email_address;
                            u.language_rcd = serviceUsers[i].language_rcd;
                            u.make_bookings_for_others_flag = serviceUsers[i].make_bookings_for_others_flag;
                            u.address_default_code = serviceUsers[i].address_default_code;
                            u.update_booking_flag = serviceUsers[i].update_booking_flag;
                            u.change_segment_flag = serviceUsers[i].change_segment_flag;
                            u.delete_segment_flag = serviceUsers[i].delete_segment_flag;
                            u.issue_ticket_flag = serviceUsers[i].issue_ticket_flag;
                            u.counter_sales_report_flag = serviceUsers[i].counter_sales_report_flag;
                            u.status_code = serviceUsers[i].status_code;
                            u.create_by = serviceUsers[i].create_by;
                            u.update_by = serviceUsers[i].update_by;
                            u.system_admin_flag = serviceUsers[i].system_admin_flag;
                            u.mon_flag = serviceUsers[i].mon_flag;
                            u.tue_flag = serviceUsers[i].tue_flag;
                            u.wed_flag = serviceUsers[i].wed_flag;
                            u.thu_flag = serviceUsers[i].thu_flag;
                            u.fri_flag = serviceUsers[i].fri_flag;
                            u.sat_flag = serviceUsers[i].sat_flag;
                            u.sun_flag = serviceUsers[i].sun_flag;
                            u.create_date_time = serviceUsers[i].create_date_time;
                            u.update_date_time = serviceUsers[i].update_date_time;
                            users.Add(u);
                            u = null;
                        }
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
            throw new NotImplementedException();
        }
        public Agents GetAgencySessionProfile(string AgencyCode, string UserAccountID)
        {
            try
            {
                Agents agents = new Agents();
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Agent[] ags = objService.GetAgencySessionProfile(AgencyCode, UserAccountID, CreateToken());
                    if (ags != null && ags.Length > 0)
                    {
                        Agent ag;
                        for (int i = 0; i < ags.Length; i++)
                        {
                            ag = new Agent();

                            ag.agency_code = ags[i].agency_code;
                            ag.agency_logon = ags[i].agency_logon;
                            ag.agency_password = ags[i].agency_password;
                            ag.agency_name = ags[i].agency_name;
                            ag.airport_rcd = ags[i].airport_rcd;
                            ag.ag_language_rcd = ags[i].ag_language_rcd;
                            ag.default_e_ticket_flag = ags[i].default_e_ticket_flag;
                            ag.email = ags[i].email;
                            ag.currency_rcd = ags[i].currency_rcd;
                            ag.country_rcd = ags[i].country_rcd;
                            ag.agency_payment_type_rcd = ags[i].agency_payment_type_rcd;
                            ag.status_code = ags[i].status_code;
                            ag.default_show_passenger_flag = ags[i].default_show_passenger_flag;
                            ag.default_auto_print_ticket_flag = ags[i].default_auto_print_ticket_flag;
                            ag.default_ticket_on_save_flag = ags[i].default_ticket_on_save_flag;
                            ag.web_agency_flag = ags[i].web_agency_flag;
                            ag.own_agency_flag = ags[i].own_agency_flag;
                            ag.b2b_credit_card_payment_flag = ags[i].b2b_credit_card_payment_flag;
                            ag.b2b_voucher_payment_flag = ags[i].b2b_voucher_payment_flag;
                            ag.b2b_eft_flag = ags[i].b2b_eft_flag;
                            ag.b2b_post_paid_flag = ags[i].b2b_post_paid_flag;
                            ag.b2b_allow_seat_assignment_flag = ags[i].b2b_allow_seat_assignment_flag;
                            ag.b2b_allow_cancel_segment_flag = ags[i].b2b_allow_cancel_segment_flag;
                            ag.b2b_allow_change_flight_flag = ags[i].b2b_allow_change_flight_flag;
                            ag.b2b_allow_name_change_flag = ags[i].b2b_allow_name_change_flag;
                            ag.b2b_allow_change_details_flag = ags[i].b2b_allow_change_details_flag;
                            ag.ticket_stock_flag = ags[i].ticket_stock_flag;
                            ag.b2b_allow_split_flag = ags[i].b2b_allow_split_flag;
                            ag.b2b_allow_service_flag = ags[i].b2b_allow_service_flag;
                            ag.b2b_group_waitlist_flag = ags[i].b2b_group_waitlist_flag;
                            ag.avl_show_net_total_flag = ags[i].avl_show_net_total_flag;
                            ag.default_user_account_id = ags[i].default_user_account_id;
                            ag.merchant_id = ags[i].merchant_id;
                            ag.default_customer_document_id = ags[i].default_customer_document_id;
                            ag.default_small_itinerary_document_id = ags[i].default_small_itinerary_document_id;
                            ag.default_internal_itinerary_document_id = ags[i].default_internal_itinerary_document_id;
                            ag.payment_default_code = ags[i].payment_default_code;
                            ag.agency_type_code = ags[i].agency_type_code;
                            ag.user_account_id = ags[i].user_account_id;
                            ag.user_logon = ags[i].user_logon;
                            ag.user_code = ags[i].user_code;
                            ag.lastname = ags[i].lastname;
                            ag.firstname = ags[i].firstname;
                            ag.language_rcd = ags[i].language_rcd;
                            ag.make_bookings_for_others_flag = ags[i].make_bookings_for_others_flag;
                            ag.origin_rcd = ags[i].origin_rcd;
                            ag.outstanding_invoice = ags[i].outstanding_invoice;
                            ag.booking_payment = ags[i].booking_payment;
                            ag.agency_account = ags[i].agency_account;
                            ag.company_client_profile_id = ags[i].company_client_profile_id;
                            ag.invoice_days = ags[i].invoice_days;
                            ag.address_line1 = ags[i].address_line1;
                            ag.address_line2 = ags[i].address_line2;
                            ag.city = ags[i].city;
                            ag.bank_code = ags[i].bank_code;
                            ag.bank_name = ags[i].bank_name;
                            ag.bank_account = ags[i].bank_account;
                            ag.contact_person = ags[i].contact_person;
                            ag.district = ags[i].district;
                            ag.phone = ags[i].phone;
                            ag.fax = ags[i].fax;
                            ag.po_box = ags[i].po_box;
                            ag.province = ags[i].province;
                            ag.state = ags[i].state;
                            ag.street = ags[i].street;
                            ag.zip_code = ags[i].zip_code;
                            ag.consolidator_flag = ags[i].consolidator_flag;
                            ag.b2b_credit_agency_and_invoice_flag = ags[i].b2b_credit_agency_and_invoice_flag;
                            ag.b2b_download_sales_report_flag = ags[i].b2b_download_sales_report_flag;
                            ag.b2b_show_remarks_flag = ags[i].b2b_show_remarks_flag;
                            ag.private_fares_flag = ags[i].private_fares_flag;
                            ag.b2b_allow_group_flag = ags[i].b2b_allow_group_flag;
                            ag.b2b_allow_waitlist_flag = ags[i].b2b_allow_waitlist_flag;
                            ag.b2b_bsp_billing_flag = ags[i].b2b_bsp_billing_flag;
                            ag.b2b_bsp_from_date = ags[i].b2b_bsp_from_date;
                            ag.iata_number = ags[i].iata_number;
                            ag.send_mailto_all_passenger = ags[i].send_mailto_all_passenger;

                            ag.allow_add_segment_flag = ags[i].allow_add_segment_flag;
                            ag.individual_firmed_flag = ags[i].individual_firmed_flag;
                            ag.individual_waitlist_flag = ags[i].individual_waitlist_flag;
                            ag.group_firmed_flag = ags[i].group_firmed_flag;
                            ag.group_waitlist_flag = ags[i].group_waitlist_flag;
                            ag.disable_changes_through_b2c_flag = ags[i].disable_changes_through_b2c_flag;
                            ag.disable_web_checkin_flag = ags[i].disable_web_checkin_flag;
                            ag.commission_percentage = 0;

                            //waiting for implement
                            //ag.column_1_tax_rcd = ags[i].column_1_tax_rcd
                            //ag.column_2_tax_rcd = ags[i].column_2_tax_rcd;
                            //ag.column_3_tax_rcd = ags[i].column_3_tax_rcd;

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
            throw new NotImplementedException();
        }

        public DataSet GetTicketSales(string AgencyCode, string UserId, string Origin, string Destination, string Airline, string FlightNumber, DateTime FlightFrom, DateTime FlightTo, DateTime TicketingFrom, DateTime TicketingTo, string PassengerType, string Language)
        {
            throw new NotImplementedException();
        }

        public DataSet GetAgencyTicketSales(string strAgency, string strCurrency, DateTime dtSalesFrom, DateTime dtSalesTo)
        {
            throw new NotImplementedException();
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
            string result;

            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            result = objService.GetAvailabilityAgencyLogin(Origin,
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
                                                             strSearchType,
                                                             bLowest,
                                                             bLowestClass,
                                                             bLowestGroup,
                                                             bShowClosed,
                                                             bSort,
                                                             bDelet,
                                                             strLanguage,
                                                             strIpAddress,
                                                             false,
                                                             CreateToken(),
                                                             ref strCurrencyCode,
                                                             bNoVat);
            if (objService != null)
            {
                objService = null;
            }
            Library objLi = new Library();
            if (result.Length == 3)
            { return result; }
            else
            { return SecurityHelper.DecompressString(result); }

        }
        public string CalculateSpecialServiceFees(string AgencyCode, string currency, string bookingId, string header, string service, string fees, string remark, string mapping, string strLanguage, bool bNoVat)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            DataSet ds = new DataSet();

            string result = string.Empty;
            string strBooking = "<Booking>" +
                                 header +
                                 remark +
                                 mapping +
                                 service +
                                 fees +
                                 "</Booking>";

            ds.ReadXml(new StringReader(strBooking));
            result = objService.CalculateNewFees(AgencyCode, currency, ds, false, false, false, false, true, strLanguage, CreateToken(), bNoVat);

            ds.Dispose();

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }
        // Start Account Topup
        public bool AgencyAccountAdd(string strAgencyCode, string strCurrency, string strUserId, string strComment, double dAmount, string strExternalReference, string strInternalReference, string strTransactionReference, bool bExternalTopup)
        {
            throw new NotImplementedException();
        }
        public bool AgencyAccountVoid(string strAgencyAccountId, string strUserId)
        {
            throw new NotImplementedException();
        }

        public DataSet ExternalPaymentListAgencyTopUp(string strAgencyCode)
        {
            throw new NotImplementedException();
        }
        // End Account Topup
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
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                if ((DataHelper.DateDifferent(DateDepartFrom, DateDepartTo).Days > 31) ||
                    (DataHelper.DateDifferent(DateReturnFrom, DateReturnTo).Days > 31))
                {
                    return string.Empty;
                }
                else
                {
                    //Search Avalability
                    return objService.GetAvailability(Origin,
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
                                                    false,
                                                    FareLogic,
                                                    false,
                                                    string.Empty,
                                                    DateTime.MinValue,
                                                    DateTime.MinValue,
                                                    string.Empty,
                                                    true,
                                                    bReturnRefundable,
                                                    string.Empty,
                                                    PromotionCode,
                                                    FareType,
                                                    bLowest,
                                                    bLowestClass,
                                                    bLowestGroup,
                                                    bShowClosed,
                                                    bSort,
                                                    bDelete,
                                                    strLanguage,
                                                    strIpAddress,
                                                    false,
                                                    CreateToken(),
                                                    bNoVat);

                }
            }
        }
        // End Account Topup
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
            throw new NotImplementedException();
        }

        public DataSet GetSessionlessOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            throw new NotImplementedException();
        }

        public DataSet GetSessionlessDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken)
        {
            throw new NotImplementedException();
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
                                                           bool bSkipFareLogin,
                                                           string strLanguage,
                                                           string strIpAddress,
                                                           string strToken,
                                                           bool bReturnRefundable,
                                                           bool bNoVat,
                                                           Int32 iDayRange)
        {

            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                if ((DataHelper.DateDifferent(DateDepartFrom, DateDepartTo).Days > 31) ||
                    (DataHelper.DateDifferent(DateReturnFrom, DateReturnTo).Days > 31))
                {
                    return string.Empty;
                }
                else
                {
                    //Search Avalability
                    return objService.GetAvailability(Origin,
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
                                                    false,
                                                    FareLogic,
                                                    false,
                                                    string.Empty,
                                                    DateTime.MinValue,
                                                    DateTime.MinValue,
                                                    string.Empty,
                                                    true,
                                                    bReturnRefundable,
                                                    string.Empty,
                                                    PromotionCode,
                                                    FareType,
                                                    bLowest,
                                                    bLowestClass,
                                                    bLowestGroup,
                                                    bShowClosed,
                                                    bSort,
                                                    bDelete,
                                                    strLanguage,
                                                    strIpAddress,
                                                    false,
                                                    CreateToken(),
                                                    bNoVat);

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
                                                bool bSkipFareLogin,
                                                string strLanguage,
                                                string strIpAddress,
                                                string strToken,
                                                bool bReturnRefundable,
                                                bool bNoVat,
                                                Int32 iDayRange)
        {

            throw new NotImplementedException();

        }
        public string GetSessionlessCurrencies(string language, string strToken)
        {
            throw new NotImplementedException();
        }
        public DataSet PassengerRoleRead(string strPaxRoleCode, string strPaxRole, string strStatus, bool bWrite, string strLanguage)
        {
            throw new NotImplementedException();
        }

        public double CalculateExchange(string currencyFrom, string currencyTo, double amount, string systemCurrency, DateTime dateOfExchange, bool reverse)
        {
            throw new NotImplementedException();
        }
        public string GetActiveBookings(Guid gClientProfileId)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.GetActiveBookings(gClientProfileId.ToString(),
                                                  CreateToken());
            if (objService != null)
            {
                objService = null;
            }
            if (string.IsNullOrEmpty(result) == false)
            {
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetFlownBookings(Guid gClientProfileId)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.GetFlownBookings(gClientProfileId.ToString(),
                                                 CreateToken());
            if (objService != null)
            {
                objService = null;
            }
            if (string.IsNullOrEmpty(result) == false)
            {
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetBookingClasses(string strBoardingclass, string language)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            //result = objService.GetPassengerTitles(language, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }
        public string GetBooking(Guid bookingId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        public string VoucherTemplateList(string voucherTemplateId, string voucherTemplate, DateTime fromDate, DateTime toDate, bool write, string status, string language)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.VoucherTemplateList(voucherTemplateId, voucherTemplate, fromDate, toDate, write, status, language, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public bool SaveVoucher(Vouchers vouchers)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            bool bResult = false;
            string strVoucher = "<Vouchers>" +
                                 XmlHelper.Serialize(vouchers, false) +
                                 "</Vouchers>";

            bResult = objService.SaveVoucher(strVoucher, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return bResult;
        }

        public string VoucherPaymentCreditCard(Payments payment, Vouchers vouchers)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            string strResult = string.Empty;
            string strVoucher = "<Vouchers>" +
                                 XmlHelper.Serialize(vouchers, false) +
                                 XmlHelper.Serialize(payment, false) +
                                 "</Vouchers>";

            strResult = objService.VoucherPaymentCreditCard(strVoucher, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return strResult;
        }
        public string ReadVoucher(Guid voucherId, double voucherNumber)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            string result = string.Empty;

            result = objService.ReadVoucher(voucherId, voucherNumber, CreateToken());

            if (objService != null)
            {
                objService = null;
            }

            return result;
        }

        public bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();

            bool bResult = false;

            bResult = objService.VoidVoucher(voucherId, userId, voidDate, CreateToken());

            if (objService != null)
            {
                objService = null;
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
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.AgencyRegistrationInsert(CreateToken(),
                                                            agencyName,
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
        }
        public bool LowFareFinderAllow(string agencyCode, string strToken)
        {
            throw new NotImplementedException();
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
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.GetPassenger(airline,
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
                                            language,
                                            CreateToken());

            }
        }
        public string GetQueueCount(string agency, bool unassigned)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.GetQueueCount(agency, unassigned, CreateToken());
            }
        }
        #endregion

        #region Helper
        public string CreateToken()
        {
            string token = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"].ToString() +
               System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"].ToString();
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(token);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }

            return sb.ToString();
        }
        public string GenerateBase64StringSHA1(object classObject)
        {
            string allString = ConvertObjectToString(classObject);
            Type t = classObject.GetType();
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1Service = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            return Convert.ToBase64String(sha1Service.ComputeHash(Encoding.ASCII.GetBytes(allString)));
        }
        public string ConvertObjectToString(object classObject)
        {
            DateTime dt;
            string allString = "";
            Type t = classObject.GetType();

            FieldInfo[] fi = classObject.GetType().GetFields(BindingFlags.Public
                                                         | BindingFlags.Instance
                                                         | BindingFlags.NonPublic
                                                         | BindingFlags.Static);

            foreach (FieldInfo field in fi)
            {
                switch (field.Name.ToLower())
                {
                    case "create_byfield":
                    case "create_by_namefield":
                    case "create_date_timefield":
                    case "update_byfield":
                    case "update_by_namefield":
                    case "update_date_timefield":
                    case "create_by":
                    case "create_by_name":
                    case "create_date_time":
                    case "update_by":
                    case "update_by_name":
                    case "update_date_time":
                        break;
                    default:
                        object objValue = field.GetValue(classObject);
                        if (objValue != null)
                        {
                            Type t2 = objValue.GetType();
                            if (t2.IsArray)
                            {
                                //Recursive
                                foreach (object objInside in (object[])objValue)
                                {
                                    allString += ConvertObjectToString(objInside);
                                }
                            }
                            else
                            {
                                if (field.FieldType == Type.GetType("System.DateTime"))
                                {
                                    dt = (DateTime)objValue;
                                    allString += string.Format("{0}-{1}-{2} {3}:{4}:{5}", dt.Year.ToString("0000"), dt.Month.ToString("00"), dt.Day.ToString("00"), dt.Hour.ToString("00"), dt.Minute.ToString("00"), dt.Second.ToString("00"));
                                }
                                else if (field.FieldType == Type.GetType("System.Boolean"))
                                {
                                    allString += ((bool)objValue) ? "1" : "0";
                                }
                                else if (field.FieldType == Type.GetType("System.Decimal"))
                                {
                                    allString += ((decimal)objValue).ToString("0.00");
                                }
                                else
                                {
                                    allString += objValue.ToString().Trim();
                                }
                            }
                        }
                        break;
                }
            }
            return allString;
        }
        #endregion

        #region IClientCore Report Members


        public DataSet GetTicketsIssued(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTicketsUsed(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTicketsRefunded(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTicketsCancelled(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, int intTicketonly, int intRefundable, string strProfileID, string strTicketNumber, string strFirstName, string strLastName, string strPassengerId, string strBookingSegmentID)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTicketsNotFlown(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight, DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, bool bUnflown, bool bNoShow)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTicketsExpired(DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight)
        {
            throw new NotImplementedException();
        }

        public DataSet GetCashbookPayments(string strAgency, string strGroup, string strUserId, DateTime dtPaymentFrom, DateTime dtPaymentTo, string strCashbookId)
        {
            throw new NotImplementedException();
        }

        public DataSet GetCashbookCharges(string XmlCashbookCharges, string strCashbookId)
        {
            throw new NotImplementedException();
        }

        public DataSet GetBookingFeeAccounted(string strAgencyCode, string strUserId, string strFee, DateTime dtFrom, DateTime dtTo)
        {
            throw new NotImplementedException();
        }

        public DataSet CreditCardPayment(ref string strCCNumber, ref string strTransType, ref string strTransStatus, ref DateTime dtFrom, ref DateTime dtTo, ref string strCCType, ref string strAgency)
        {
            throw new NotImplementedException();
        }

        public DataSet GetOutstanding(string strAgencyCode, string strAirline, string strFlightNumber, DateTime dtFlightFrom, DateTime dtFlightTo, string strOrigin, string strDestination, bool bOffices, bool bAgencies, bool bLastTwentyFourHours, bool bTicketedOnly, int iOlderThanHours, string strLanguage, bool bAccountsPayable)
        {
            throw new NotImplementedException();
        }

        public DataSet GetAgencyAccountTransactions(string agencyCode, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }

        public bool CompleteRemark(string XmlRemarks, string RemarkId, string UserId)
        {
            throw new NotImplementedException();
        }

        public DataSet GetActivities(string AgencyCode, string RemarkType, string Nickname, DateTime TimelimitFrom, DateTime TimelimitTo, bool PendingOnly, bool IncompleteOnly, bool IncludeRemarks, bool showUnassigned)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.GetActivities(AgencyCode,
                                                RemarkType,
                                                Nickname,
                                                TimelimitFrom,
                                                TimelimitTo,
                                                PendingOnly,
                                                IncompleteOnly,
                                                IncludeRemarks,
                                                showUnassigned,
                                                CreateToken());
            }
        }

        public DataSet GetCashbookPaymentsSummary(string XmlCashbookPaymentsAll)
        {
            throw new NotImplementedException();
        }

        public DataSet GetAgencyAccountTopUp(string strAgencyCode, string strCurrency, DateTime dtFrom, DateTime dtTo)
        {
            throw new NotImplementedException();
        }

        public DataSet GetServiceFees(ref string strOrigin, ref string strDestination, ref string strCurrency, ref string strAgency, ref string strServiceGroup, ref DateTime dtFee)
        {
            throw new NotImplementedException();
        }

        public DataSet GetClientSessionProfile(string clientProfileId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ClientCore B2A
        public DataSet GetAgencyAccountBalance(string strAgencyCode, string strAgencyName, string strCurrency, string strConsolidatorAgency)
        {
            throw new NotImplementedException();
        }

        public bool AddNewAgency(string strXmlAgency, string strAgencyCode, string strDefaultUserLogon, string strDefaultPassword, string strDefaultLastName, string strDefaultFirstName, string strCreateUser, short iChangeBooking, short iChangeSegment, short iDeleteSegment, short iTicketIssue)
        {
            throw new NotImplementedException();
        }

        public bool AdjustSubAgencyAccountBalance(string strXmlChildAccountBalance, string strXmlParentAccountBalance)
        {
            throw new NotImplementedException();
        }

        public DataSet AgencyRead(string strAgencyCode, bool bKeepSession)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IClientCore User Members

        public string GetUserList(string UserLogon, string UserCode, string LastName, string FirstName, string AgencyCode, string StatusCode)
        {
            throw new NotImplementedException();
        }

        public string UserRead(string UserID)
        {
            throw new NotImplementedException();
        }

        public bool UserSave(string strUsersXml, string agencyCode)
        {
            bool success = false;
            try
            {
                User user = (User)XmlHelper.Deserialize(strUsersXml, typeof(User));
                if (user != null)
                {
                    using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                    {
                        tikAeroWebService.User u = new tikAeroWebService.User();
                        u.user_account_id = user.user_account_id;
                        u.user_logon = user.user_logon;
                        u.lastname = user.lastname;
                        u.firstname = user.firstname;
                        u.email_address = user.email_address;
                        u.language_rcd = user.language_rcd;
                        u.make_bookings_for_others_flag = user.make_bookings_for_others_flag;
                        u.address_default_code = user.address_default_code;
                        u.update_booking_flag = user.update_booking_flag;
                        u.change_segment_flag = user.change_segment_flag;
                        u.delete_segment_flag = user.delete_segment_flag;
                        u.issue_ticket_flag = user.issue_ticket_flag;
                        u.counter_sales_report_flag = user.counter_sales_report_flag;
                        u.status_code = user.status_code;
                        u.create_by = user.create_by;
                        u.update_by = user.update_by;
                        u.system_admin_flag = user.system_admin_flag;
                        u.mon_flag = user.mon_flag;
                        u.tue_flag = user.tue_flag;
                        u.wed_flag = user.wed_flag;
                        u.thu_flag = user.thu_flag;
                        u.fri_flag = user.fri_flag;
                        u.sat_flag = user.sat_flag;
                        u.sun_flag = user.sun_flag;
                        u.create_date_time = user.create_date_time;
                        u.update_date_time = user.update_date_time;
                        success = objService.UserSave(u, agencyCode, CreateToken());
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }
        public string GetCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType)
        {
            throw new NotImplementedException();
        }
        public string GetSessionlessCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType, string strToken)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        public string SingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            throw new NotImplementedException();
        }
        public string SessionlessSingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strToken, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            throw new NotImplementedException();
        }
        public DataSet GetBookingHistory(string strBookingId)
        {
            throw new NotImplementedException();
        }
        public string SegmentFee(string strAgencyCode, string strCurrency, string[] strFeeRcdGroup, Mappings mappings, int iPassengerNumber, int iInfantNumber, string strLanguage, bool specialService, bool bNoVat)
        {
            tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
            tikAeroWebService.SegmentFeeRequest request = new tikAeroWebService.SegmentFeeRequest();
            tikAeroWebService.SegmentFeeResponse response = new tikAeroWebService.SegmentFeeResponse();
            tikAeroWebService.SegmentService[] segmentService = null;
            string result = string.Empty;

            //Add to segment service
            segmentService = new tikAeroWebService.SegmentService[mappings.Count];
            for (int i = 0; i < mappings.Count; i++)
            {
                segmentService[i] = new tikAeroWebService.SegmentService();
                segmentService[i].origin_rcd = mappings[i].origin_rcd;
                segmentService[i].destination_rcd = mappings[i].destination_rcd;
                segmentService[i].od_origin_rcd = mappings[i].od_origin_rcd;
                segmentService[i].od_destination_rcd = mappings[i].od_destination_rcd;
                segmentService[i].booking_class_rcd = mappings[i].booking_class_rcd;
                segmentService[i].fare_code = mappings[i].fare_code;
                segmentService[i].airline_rcd = mappings[i].airline_rcd;
                segmentService[i].flight_number = mappings[i].flight_number;
                segmentService[i].departure_date = mappings[i].departure_date;
            }
            request.AgencyCode = strAgencyCode;
            request.CurrencyCode = strCurrency;
            request.LanguageCode = strLanguage;
            request.SegmentService = segmentService;
            request.ServiceCode = strFeeRcdGroup;

            if (specialService == true)
            {
                response = objService.SpecialServiceFee(request, CreateToken(), bNoVat);
            }
            else
            {
                response = objService.SegmentFee(request, CreateToken(), bNoVat);
            }


            if (response.Success == true)
            {
                #region Generate XML
                using (StringWriter stw = new StringWriter())
                {
                    using (XmlWriter xtw = XmlWriter.Create(stw))
                    {
                        xtw.WriteStartElement("ServiceFees");
                        {
                            for (int i = 0; i < response.ServiceFee.Length; i++)
                            {
                                xtw.WriteStartElement(response.ServiceFee[i].special_service_rcd);
                                {
                                    xtw.WriteStartElement("fee_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].special_service_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("display_name");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].display_name);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("origin_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].origin_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("destination_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].destination_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("od_origin_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].od_origin_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("od_destination_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].od_destination_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("booking_class_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].booking_class_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("fare_code");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].fare_code);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("airline_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].airline_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("flight_number");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].flight_number);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("departure_date");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].departure_date);
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("currency_rcd");
                                    {
                                        xtw.WriteValue(response.ServiceFee[i].currency_rcd);
                                    }
                                    xtw.WriteEndElement();
                                    if (response.ServiceFee[i].fee_amount > 0)
                                    {
                                        xtw.WriteStartElement("fee_amount");
                                        {
                                            xtw.WriteValue(response.ServiceFee[i].fee_amount);
                                        }
                                        xtw.WriteEndElement();
                                    }
                                    if (response.ServiceFee[i].fee_amount_incl > 0)
                                    {
                                        xtw.WriteStartElement("fee_amount_incl");
                                        {
                                            xtw.WriteValue(response.ServiceFee[i].fee_amount_incl);
                                        }
                                        xtw.WriteEndElement();
                                    }

                                    if (response.ServiceFee[i].total_fee_amount > 0)
                                    {
                                        xtw.WriteStartElement("total_fee_amount");
                                        {
                                            xtw.WriteValue(response.ServiceFee[i].total_fee_amount);
                                        }
                                        xtw.WriteEndElement();
                                    }
                                    if (response.ServiceFee[i].total_fee_amount_incl > 0)
                                    {
                                        xtw.WriteStartElement("total_fee_amount_incl");
                                        {
                                            xtw.WriteValue(response.ServiceFee[i].total_fee_amount_incl);
                                        }
                                        xtw.WriteEndElement();
                                    }
                                    xtw.WriteStartElement("service_on_request");
                                    {
                                        xtw.WriteValue(Convert.ToByte(response.ServiceFee[i].service_on_request_flag));
                                    }
                                    xtw.WriteEndElement();
                                    xtw.WriteStartElement("cut_off_time");
                                    {
                                        xtw.WriteValue(Convert.ToByte(response.ServiceFee[i].cut_off_time));
                                    }
                                    xtw.WriteEndElement();
                                }
                                xtw.WriteEndElement();
                            }
                        }
                        xtw.WriteEndElement();
                    }
                    return stw.ToString();
                }
                #endregion
            }
            else
            {
                return string.Empty;
            }
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

            throw new NotImplementedException();
        }
        public Currencies GetCurrencies(string language)
        {
            try
            {
                Currencies currencies = new Currencies();
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Currency[] objCurrencies = objService.GetCurrencies(language, CreateToken());
                    if (objCurrencies != null && objCurrencies.Length > 0)
                    {
                        Currency c;
                        for (int i = 0; i < objCurrencies.Length; i++)
                        {
                            c = new Currency();
                            c.currency_rcd = objCurrencies[i].currency_rcd;
                            c.currency_number = objCurrencies[i].currency_number;
                            c.display_name = objCurrencies[i].display_name;
                            c.max_voucher_value = objCurrencies[i].max_voucher_value;
                            c.rounding_rule = objCurrencies[i].rounding_rule;
                            c.number_of_decimals = objCurrencies[i].number_of_decimals;
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
        public bool InsertPaymentApproval(string strPaymentApprovalID, string strCardRcd, string strCardNumber, string strNameOnCard, int iExpiryMonth, int iExpiryYear, int iIssueMonth, int iIssueYear, int iIssueNumber, string strPaymentStateCode, string strBookingPaymentId, string strCurrencyRcd, double dPaymentAmount, string strIpAddress)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Double GetExchangeRateRead(string strOriginCurrencyCode, string strDestCurrencyCode)
        {
            throw new NotImplementedException();
        }
        public Fees GetSessionlessFeesDefinition(string strLanguage, string strToken)
        {
            throw new NotImplementedException();
        }
        public Fees GetFee(string strFee,
                            string strCurrency,
                            string strAgencyCode,
                            string strClass,
                            string strFareBasis,
                            string strOrigin,
                            string strDestination,
                            string strFlightNumber,
                            DateTime dtDeparture,
                            string strLanguage,
                            bool bNoVat)
        {
            string strResult = string.Empty;
            Fees fees = new Fees();
            if (string.IsNullOrEmpty(strAgencyCode) == false &&
                string.IsNullOrEmpty(strCurrency) == false)
            {
                tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
                tikAeroWebService.GetFeeRequest request = new tikAeroWebService.GetFeeRequest();
                //Assing request Parameter.
                request.FeeRcd = strFee;
                request.CurrencyRcd = strCurrency;
                request.AgencyCode = strAgencyCode;
                request.BookingClass = strClass;
                request.FareBasis = strFareBasis;
                request.OriginRcd = strOrigin;
                request.DestinationRcd = strDestination;
                request.FlightNumber = strFlightNumber;
                request.DepartureDate = dtDeparture;
                request.LanguageCode = strLanguage;

                tikAeroWebService.GetFeeReponse response = objService.GetFee(request, CreateToken(), bNoVat);
                if (response.Success == true)
                {
                    tikAeroWebService.FeeView[] FeesView = response.Fees;
                    if (FeesView != null && FeesView.Length > 0)
                    {
                        Fee f;
                        for (int i = 0; i < FeesView.Length; i++)
                        {
                            f = new Fee();

                            f.fee_id = FeesView[i].fee_id;
                            f.fee_rcd = FeesView[i].fee_rcd;
                            f.currency_rcd = FeesView[i].currency_rcd;
                            f.display_name = FeesView[i].display_name;
                            f.fee_category_rcd = FeesView[i].fee_category_rcd;

                            f.fee_amount = FeesView[i].fee_amount;
                            f.fee_amount_incl = FeesView[i].fee_amount_incl;
                            f.vat_percentage = FeesView[i].vat_percentage;

                            f.fee_percentage = FeesView[i].fee_percentage;

                            f.minimum_fee_amount_flag = FeesView[i].minimum_fee_amount_flag;
                            f.od_flag = FeesView[i].od_flag;

                            fees.Add(f);
                            f = null;
                        }
                    }
                }
            }
            return fees;
        }
        public string GetBaggageFeeOptions(Mappings mappings, Guid gSegmentId, Guid gPassengerId, string strAgencyCode, string strLanguage, long lMaxunits, Fees fees, bool bApplySegmentFee, bool bNoVat)
        {
            string strResult = string.Empty;
            if (string.IsNullOrEmpty(strAgencyCode) == false &&
               gSegmentId.Equals(Guid.Empty) == false &&
               gPassengerId.Equals(Guid.Empty) == false &&
               mappings.Count > 0)
            {
                tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService();
                tikAeroWebService.BaggageFeeRequest request = new tikAeroWebService.BaggageFeeRequest();

                request.AgencyCode = strAgencyCode;
                request.LanguageCode = strLanguage;
                request.PassengerId = gPassengerId;
                request.BookingSegmentId = gSegmentId;
                request.MaxUnit = Convert.ToInt32(lMaxunits);


                //Fill mapping view.
                request.Mappings = new tikAeroWebService.MappingView[mappings.Count];
                for (int i = 0; i < mappings.Count; i++)
                {
                    request.Mappings[i] = new tikAeroWebService.MappingView();

                    request.Mappings[i].booking_segment_id = mappings[i].booking_segment_id;
                    request.Mappings[i].passenger_type_rcd = mappings[i].passenger_type_rcd;
                    request.Mappings[i].passenger_id = mappings[i].passenger_id;
                    request.Mappings[i].origin_rcd = mappings[i].origin_rcd;
                    request.Mappings[i].destination_rcd = mappings[i].destination_rcd;
                    request.Mappings[i].od_origin_rcd = mappings[i].od_origin_rcd;
                    request.Mappings[i].od_destination_rcd = mappings[i].od_destination_rcd;
                    request.Mappings[i].booking_class_rcd = mappings[i].booking_class_rcd;
                    request.Mappings[i].currency_rcd = mappings[i].currency_rcd;
                    request.Mappings[i].fare_code = mappings[i].fare_code;
                    request.Mappings[i].airline_rcd = mappings[i].airline_rcd;
                    request.Mappings[i].flight_number = mappings[i].flight_number;
                    request.Mappings[i].departure_date = mappings[i].departure_date;
                    request.Mappings[i].piece_allowance = mappings[i].piece_allowance;
                    request.Mappings[i].baggage_weight = mappings[i].baggage_weight;
                    request.Mappings[i].agency_code = mappings[i].agency_code;
                }

                tikAeroWebService.BaggageFeeResponse response = objService.GetBaggageFee(request, CreateToken(), bNoVat);
                if (response.Success == true)
                {
                    tikAeroWebService.BaggageFee[] baggageFee = response.BaggageFees;
                    //Convert Baggage Fee object to xml
                    using (StringWriter stw = new StringWriter())
                    {
                        using (XmlWriter xtw = XmlWriter.Create(stw))
                        {
                            xtw.WriteStartElement("BaggageFees");
                            {
                                for (int i = 0; i < baggageFee.Length; i++)
                                {
                                    xtw.WriteStartElement("Fee");
                                    {
                                        xtw.WriteStartElement("baggage_fee_option_id");
                                        {
                                            xtw.WriteValue(baggageFee[i].baggage_fee_option_id.ToString());
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("passenger_id");
                                        {
                                            xtw.WriteValue(baggageFee[i].passenger_id.ToString());
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("booking_segment_id");
                                        {
                                            xtw.WriteValue(baggageFee[i].booking_segment_id.ToString());
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("fee_id");
                                        {
                                            xtw.WriteValue(baggageFee[i].fee_id.ToString());
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("fee_rcd");
                                        {
                                            xtw.WriteValue(baggageFee[i].fee_rcd);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("fee_category_rcd");
                                        {
                                            xtw.WriteValue(baggageFee[i].fee_category_rcd);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("currency_rcd");
                                        {
                                            xtw.WriteValue(baggageFee[i].currency_rcd);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("display_name");
                                        {
                                            xtw.WriteValue(baggageFee[i].display_name);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("number_of_units");
                                        {
                                            xtw.WriteValue(baggageFee[i].number_of_units);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("fee_amount");
                                        {
                                            xtw.WriteValue(baggageFee[i].fee_amount);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("fee_amount_incl");
                                        {
                                            xtw.WriteValue(baggageFee[i].fee_amount_incl);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("total_amount");
                                        {
                                            xtw.WriteValue(baggageFee[i].total_amount);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("total_amount_incl");
                                        {
                                            xtw.WriteValue(baggageFee[i].total_amount_incl);
                                        }
                                        xtw.WriteEndElement();
                                        xtw.WriteStartElement("vat_percentage");
                                        {
                                            xtw.WriteValue(baggageFee[i].vat_percentage);
                                        }
                                        xtw.WriteEndElement();
                                    }
                                    xtw.WriteEndElement();
                                }
                            }
                            xtw.WriteEndElement();
                        }
                        return stw.ToString();
                    }
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
            throw new NotImplementedException();
        }
        public string GetAvailabilety(string strFlightID,
                               string strOriginRcd,
                               string strDestinationRcd,
                               string strSpecialServiceRcd,
                               string strBoardingClassRcd)
        {
            throw new NotImplementedException();
        }
        public Int32 GetInfantCapacity(string strFlightID,
                               string strOriginRcd,
                               string strDestinationRcd,
                               string strBoardingClassRcd)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                Int32 totalResult = 0;
                totalResult = objService.GetInfantCapacity(strFlightID, strOriginRcd, strDestinationRcd, strBoardingClassRcd, CreateToken());

                return totalResult;
            }
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
            throw new NotImplementedException();
        }
        public string GetPassengerRole(string strLanguage)
        {
            throw new NotImplementedException();
        }
        public Services GetSpecialServices(string strLanguage)
        {
            try
            {
                Services services = new Services();
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    tikAeroWebService.Service[] service = objService.GetSpecialServices(strLanguage, CreateToken());
                    if (service != null && service.Length > 0)
                    {
                        Service sv;
                        for (int i = 0; i < service.Length; i++)
                        {
                            sv = new Service();

                            sv.special_service_rcd = service[i].special_service_rcd;
                            sv.display_name = service[i].display_name;
                            sv.help_text = service[i].help_text;
                            sv.special_service_group_rcd = service[i].special_service_group_rcd;
                            sv.text_allowed_flag = service[i].text_allowed_flag;
                            sv.inventory_control_flag = service[i].inventory_control_flag;
                            sv.manifest_flag = service[i].manifest_flag;
                            sv.text_required_flag = service[i].text_required_flag;
                            sv.service_on_request_flag = service[i].service_on_request_flag;
                            sv.include_passenger_name_flag = service[i].include_passenger_name_flag;
                            sv.include_flight_segment_flag = service[i].include_flight_segment_flag;
                            sv.include_action_code_flag = service[i].include_action_code_flag;
                            sv.include_number_of_service_flag = service[i].include_number_of_service_flag;
                            sv.include_catering_flag = service[i].include_catering_flag;
                            sv.include_passenger_assistance_flag = service[i].include_passenger_assistance_flag;
                            sv.service_supported_flag = service[i].service_supported_flag;
                            sv.send_interline_reply_flag = service[i].send_interline_reply_flag;
                            sv.cut_off_time = service[i].cut_off_time;
                            sv.status_code = service[i].status_code;

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
            throw new RowNotInTableException();
        }
        public string GetFlightSummary(Passengers passengers, Flights flights, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    //Fill flight information
                    tikAeroWebService.Flight[] objFlight = new tikAeroWebService.Flight[flights.Count];
                    for (int i = 0; i < objFlight.Length; i++)
                    {
                        objFlight[i] = new tikAeroWebService.Flight();
                        objFlight[i].flight_id = flights[i].flight_id;
                        objFlight[i].fare_id = flights[i].fare_id;
                        objFlight[i].booking_class_rcd = flights[i].booking_class_rcd;
                        objFlight[i].origin_rcd = flights[i].origin_rcd;
                        objFlight[i].destination_rcd = flights[i].destination_rcd;
                        objFlight[i].departure_date = flights[i].departure_date;
                        objFlight[i].flight_connection_id = flights[i].flight_connection_id;
                        objFlight[i].od_origin_rcd = flights[i].od_origin_rcd;
                        objFlight[i].od_destination_rcd = flights[i].od_destination_rcd;
                    }
                    //Fill Passenger information
                    tikAeroWebService.Passenger[] objPassengers = new tikAeroWebService.Passenger[passengers.Count];
                    for (int i = 0; i < objPassengers.Length; i++)
                    {
                        objPassengers[i] = new tikAeroWebService.Passenger();
                        objPassengers[i].passenger_id = passengers[i].passenger_id;
                        objPassengers[i].passenger_type_rcd = passengers[i].passenger_type_rcd;
                    }

                    return objService.GetFlightSummary(objFlight,
                                                        objPassengers,
                                                        strAgencyCode,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        DateTime.MinValue,
                                                        string.Empty,
                                                        strCurrencyCode,
                                                        false,
                                                        false,
                                                        false,
                                                        string.Empty,
                                                        0,
                                                        string.Empty,
                                                        strLanguage,
                                                        CreateToken());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFlightSummaryAgencyLogin(Passengers passengers, Flights flights, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat, string password)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    //Fill flight information
                    tikAeroWebService.Flight[] objFlight = new tikAeroWebService.Flight[flights.Count];
                    for (int i = 0; i < objFlight.Length; i++)
                    {
                        objFlight[i] = new tikAeroWebService.Flight();
                        objFlight[i].flight_id = flights[i].flight_id;
                        objFlight[i].fare_id = flights[i].fare_id;
                        objFlight[i].boarding_class_rcd = flights[i].boarding_class_rcd;
                        objFlight[i].booking_class_rcd = flights[i].booking_class_rcd;
                        objFlight[i].origin_rcd = flights[i].origin_rcd;
                        objFlight[i].destination_rcd = flights[i].destination_rcd;
                        objFlight[i].departure_date = flights[i].departure_date;
                        objFlight[i].flight_connection_id = flights[i].flight_connection_id;
                        objFlight[i].transit_flight_id = flights[i].transit_flight_id;
                        objFlight[i].transit_airport_rcd = flights[i].transit_airport_rcd;
                        objFlight[i].transit_boarding_class_rcd = flights[i].transit_boarding_class_rcd;
                        objFlight[i].transit_booking_class_rcd = flights[i].transit_booking_class_rcd;
                        objFlight[i].od_origin_rcd = flights[i].od_origin_rcd;
                        objFlight[i].od_destination_rcd = flights[i].od_destination_rcd;
                    }
                    //Fill Passenger information
                    tikAeroWebService.Passenger[] objPassengers = new tikAeroWebService.Passenger[passengers.Count];
                    for (int i = 0; i < objPassengers.Length; i++)
                    {
                        objPassengers[i] = new tikAeroWebService.Passenger();
                        objPassengers[i].passenger_id = passengers[i].passenger_id;
                        objPassengers[i].passenger_type_rcd = passengers[i].passenger_type_rcd;
                    }

                    return objService.GetFlightSummaryAgencyLogin(objFlight,
                                                        objPassengers,
                                                        strAgencyCode,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        DateTime.MinValue,
                                                        string.Empty,
                                                        strCurrencyCode,
                                                        false,
                                                        false,
                                                        false,
                                                        string.Empty,
                                                        0,
                                                        string.Empty,
                                                        strLanguage,
                                                        CreateToken(),
                                                        password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetSingleFlight(string strFlightId,
                                    string strAirline,
                                    string strFlightNumber,
                                    DateTime dtFlightFrom,
                                    DateTime dtFlightTo,
                                    string strLanguage,
                                    string strOrigin,
                                    string strDestination,
                                    bool bWrite,
                                    bool bEmptyRs,
                                    string strScheduleId,
                                    bool bSingle)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    return objService.GetSingleFlight(strFlightId,
                                                      strAirline,
                                                      strFlightNumber,
                                                      dtFlightFrom,
                                                      dtFlightTo,
                                                      strLanguage,
                                                      strOrigin,
                                                      strDestination,
                                                      bWrite,
                                                      bEmptyRs,
                                                      strScheduleId,
                                                      bSingle,
                                                      CreateToken());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.RemarkAdd(remarkType,
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
                                            timelimitUTC,
                                            CreateToken());
            }
        }

        public bool RemarkDelete(Guid bookingRemarkId)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.RemarkDelete(bookingRemarkId, CreateToken());
            }
        }
        public bool RemarkComplete(Guid bookingRemarkId, Guid userId)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.RemarkComplete(bookingRemarkId, userId, CreateToken());
            }
        }

        public string RemarkRead(string remarkId,
                                string bookingId,
                                string bookingReference,
                                double bookingNumber,
                                bool readOnly)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                return objService.RemarkRead(remarkId,
                                            bookingId,
                                            bookingReference,
                                            bookingNumber,
                                            readOnly,
                                            CreateToken());
            }
        }

        public bool RemarkSave(Remarks remarks)
        {
            using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
            {
                //Create Remark XML.
                string strBooking = "<Booking>" +
                                        XmlHelper.Serialize(remarks, false) +
                                    "</Booking>";

                return objService.RemarkSave(strBooking, CreateToken());
            }
        }

        public string BoardingClassRead(string boardingClassCode, string boardingClass, string sortSeq, string status, bool bWrite)
        {
            throw new NotImplementedException();
        }
        #endregion


        public string ViewBookingChange(string bookingID, string languageCode, string AgencyCode)
        {
            throw new NotImplementedException();
        }

        public FlightsResponse GetFlightPassengerAPI(string origin_rcd,
            string destination_rcd,
            string airline_rcd,
            string flight_number,
            DateTime departure_date_from,
            DateTime departure_date_to,
            string username,
            string password)
        {
            FlifoService.FlifoService proxy = new FlifoService.FlifoService();
            FlifoService.UserCredentials credential = new FlifoService.UserCredentials();
            credential.UserName = username;
            credential.Password = password;

            FlifoService.FlightsRequest request = new FlifoService.FlightsRequest();
            request.origin_rcd = origin_rcd;
            request.destination_rcd = destination_rcd;
            request.airline_rcd = airline_rcd;
            request.flight_number = flight_number;

            request.departure_date_from = departure_date_from;
            request.departure_date_to = departure_date_to;

            FlifoService.FlightsResponse response = proxy.get_flights(request, GenerateBase64StringSHA1(request), credential);
            return response;
        }

        public string GetPassengerManifest(string origin_rcd,
                    string destination_rcd,
                    string airline_rcd,
                    string flight_number,
                    string departure_date_from,
                    string departure_date_to,
                    string username,
                    string password,
                    bool bReturnServices,
                    bool bReturnBagTags,
                    bool bReturnRemarks,
                    bool bNotCheckedIn,
                    bool bCheckedIn,
                    bool bOffloaded,
                    bool bNoShow,
                    bool bInfants,
                    bool bConfirmed,
                    bool bWaitlisted,
                    bool bCancelled,
                    bool bStandby,
                    bool bIndividual,
                    bool bGroup,
                    bool bTransit)
        {
            tikAeroWebService.tikAeroWebService service = new tikAeroWebService.tikAeroWebService();

            return service.GetPassengersManifest(
                origin_rcd,
                destination_rcd,
                airline_rcd,
                flight_number,
                departure_date_from,
                departure_date_to,
                username,
                password,
                CreateToken(),
                bReturnServices,
                bReturnBagTags,
                bReturnRemarks,
                bNotCheckedIn,
                bCheckedIn,
                bOffloaded,
                bNoShow,
                bInfants,
                bConfirmed,
                bWaitlisted,
                bCancelled,
                bStandby,
                bIndividual,
                bGroup,
                bTransit);
        }
    }
}
