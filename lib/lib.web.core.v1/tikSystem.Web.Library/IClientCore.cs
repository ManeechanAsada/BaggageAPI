using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace tikSystem.Web.Library
{

    public interface IClientCore
    {
        DataSet GetAirport(string language);

        Routes GetOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag);
        Routes GetDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag);
        DataSet GetSessionlessOrigins(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken);
        DataSet GetSessionlessDestination(string language, bool b2cFlag, bool b2bFlag, bool b2eFlag, bool b2sFlag, bool apiFlag, string strToken);

        DataSet GetAgencyCode(string agencyCode);
        bool ReleaseFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock);
        bool ReleaseSessionlessFlightInventorySession(string sessionId, string flightId, string bookingClasss, string bookingId, bool releaseTimeOut, bool ReleaseInventory, bool ReleaseBookingLock, string strToken);
        string GetFlightAvailability(string Origin,
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
                                    Int32 iDayRange);

        string GetLowFareFinder(string Origin,
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
                                Int32 iDayRange);

        string GetSessionlessFlightAvailability(string Origin,
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
                                                Int32 iDayRange);
        string GetSessionlessLowFareFinder(string Origin,
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
                                            Int32 iDayRange);

        string GetCompactFlightAvailability(string Origin, string Destination, DateTime DateDepartFrom, DateTime DateDepartTo, DateTime DateReturnFrom, DateTime DateReturnTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType);
        string GetSessionlessCompactFlightAvailability(string Origin, string Destination, DateTime DepartDateFrom, DateTime DepartDateTo, DateTime ReturnDateFrom, DateTime ReturnDateTo, DateTime DateBooking, short Adult, short Child, short Infant, short Other, string OtherPassengerType, string BoardingClass, string BookingClass, string DayTimeIndicator, string AgencyCode, string CurrencyCode, string FlightId, string FareId, double MaxAmount, bool NonStopOnly, bool IncludeDeparted, bool IncludeCancelled, bool IncludeWaitlisted, bool IncludeSoldOut, bool Refundable, bool GroupFares, bool ItFaresOnly, bool bStaffFares, bool bApplyFareLogic, bool bUnknownTransit, string strTransitPoint, DateTime dteReturnFrom, DateTime dteReturnTo, string dtReturn, bool bMapWithFares, bool bReturnRefundable, string strReturnDayTimeIndicator, string PromotionCode, short iFareLogic, string strSearchType, string strToken);
        string FlightAdd(string agencyCode, string currency, string flightXml, string bookingID, short adults, short children, short infants, short others, string strOthers, string userId, string strIpAddress, string strLanguageCode, bool bNoVat);
        string GetClient(string clientId, string clientNumber, string passengerId, bool bShowRemark);
        Titles GetPassengerTitles(string language);
        Documents GetDocumentType(string language);
        Countries GetCountry(string language);
        Languages GetLanguages(string language);
        DataSet GetSeatMap(string origin, string destination, string flightId, string boardingClass, string bookingClass, string strLanguage);
        string GetFormOfPayments(string language);
        string GetFormOfPaymentSubTypes(string type, string language);

        string SaveBooking(string bookingId, string header, string segment, string passenger, string remark, string payment, string mapping, string service, string tax, string fee, bool createTickets, bool readBooking, bool readOnly, string strLanguage);
        string CalculateNewFees(string bookingId, string AgencyCode, string header, string segment, string passenger, string fees, string currency, string remark, string payment, string mapping, string service, string tax, bool checkBooking, bool checkSegment, bool checkName, bool checkSeat, string strLanguage, bool bNoVat);
        string CalculateSpecialServiceFees(string AgencyCode, string currency, string bookingId, string header, string service, string fees, string remark, string mapping, string strLanguage, bool bNoVat);
        string GetVouchers(string recordLocator,
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
                            Fees fees);
        bool SavePayment(string bookingId, Mappings mappings, Fees fees, Payments payment, Vouchers refundVoucher);
        DataSet GetFormOfPaymentSubtypeFees(string formOfPayment, string formOfPaymentSubtype, string currencyRcd, string agency, DateTime feeDate);
        string GetItinerary(string bookingId, string language, string passengerId, string agencyCode);
        string ItineraryRead(string recordLocator, string language, string passengerId, string agencyCode);
        bool QueueMail(string strFromAddress, string strFromName, string strToAddress, string strToAddressCC, string strToAddressBCC, string strReplyToAddress, string strSubject, string strBody, string strDocumentType, string strAttachmentStream, string strAttachmentFileName, string strAttachmentFileType, string strAttachmentParser, bool bHtmlBody, bool bConvertAttachmentFromHTML2PDF, bool bRemoveFromQueue, string strUserId, string strBookingId, string strVoucherId, string strBookingSegmentID, string strPassengerId, string strClientProfileId, string strDocumentId, string strLanguageCode);
        DataSet GetClientPassenger(string bookingId, string clientProfileId, string clientNumber);
        string ServiceAuthentication(string agencyCode, string agencyLogon, string agencyPasseword, string selectedAgency);
        DataSet ClientLogon(string ClientNumber, string ClientPassword);
        DataSet GetBookings(string Airline,
                            string FlightNumber,
                            string FlightId,
                            System.DateTime FlightFrom,
                            System.DateTime FlightTo,
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
                            System.DateTime CreateFrom,
                            System.DateTime CreateTo);

        DataSet GetBookingsThisUser(string agencyCode, string userId, string airline, string flightNumber,
                                    System.DateTime flightFrom, System.DateTime flightTo, string recordLocator, string origin, string destination,
                                    string passengerName, string seatNumber, string ticketNumber, string phoneNumber, System.DateTime createFrom,
                                    System.DateTime createTo);
        DataSet ClientRead(string clientProfileID);

        bool AddClientProfile(Client client, Passengers passengers, Remarks remarks);
        bool EditClientProfile(Client client, Passengers passengers, Remarks remarks);
        bool AddClientPassengerList(string xmlClient, string xmlPassenger, string xmlBookingRemark);
        bool ClientSave(string xmlClient, string xmlPassenger, string xmlBookingRemark);
        string GetTransaction(string strOrigin, string strDestination, string strAirline, string strFlight, string strSegmentType, string strClientProfileId,
                            string strPassengerProfileId, string strVendor, string strCreditDebit, System.DateTime dtFlightFrom, System.DateTime dtFlightTo,
                            System.DateTime dtTransactionFrom, System.DateTime dtTransactionTo, System.DateTime dtExpiryFrom, System.DateTime dtExpiryTo,
                            System.DateTime dtVoidFrom, System.DateTime dtVoidTo, int iBatch, bool bAllVoid, bool bAllExpired, bool bAuto, bool bManual, bool bAllPoint);
        DataSet TicketRead(ref string strBookingId, ref string strPassengerId, ref string strSegmentId, ref string strTicketNumber, ref string xmlTaxes,
                           ref bool bReadOnly, ref bool bReturnTax);
        bool CheckUniqueMailAddress(string strMail, string strClientProfileId);
        DataSet AccuralQuote(string strPassenger, string strMapping, string strClientProfileId);
        DataSet GetFlightDailyCount(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo);
        string GetFlightDailyCountXML(DateTime dtFrom, DateTime dtTo, string strFrom, string strTo, string strToken);
        DataSet GetBinRangeSearch(string strCardType, string strStatusCode);
        DataSet GetSessionlessBinRangeSearch(string strCardType, string strStatusCode, string strToken);
        string GetBookingSegmentCheckIn(string strBookingId, string strClientId, string strLanguageCode);
        string GetPassengerDetails(string strPassengerId,
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
                                   string strOrigin);
        bool CheckInSave(string strMappingXml,
                        string strBaggageXml,
                        string strSeatAssignmentXml,
                        string strPassengerXml,
                        string strServiceXml,
                        string strRemarkXml,
                        string strBookingSegmentXml,
                        string strFeeXml);

        string SaveBookingCreditCard(string bookingId,
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
                                    string strLanguage);
        string SaveBookingPayment(string bookingId,
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
                                    string strLanguage);
        string SaveBookingMultipleFormOfPayment(string bookingId,
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
                                                string strLanguage);
        string GetServiceFeesByGroups(BookingHeader header, Itinerary itinerary, string serviceGroup);
        DataSet GetTicketsIssued(DateTime dtReportFrom,
                                DateTime dtReportTo,
                                DateTime dtFlightFrom,
                                DateTime dtFlightTo,
                                string strOrigin,
                                string strDestination,
                                string strAgency,
                                string strAirline,
                                string strFlight);
        DataSet GetTicketsUsed(DateTime dtReportFrom,
                                DateTime dtReportTo,
                                DateTime dtFlightFrom,
                                DateTime dtFlightTo,
                                string strOrigin,
                                string strDestination,
                                string strAgency,
                                string strAirline,
                                string strFlight);
        DataSet GetTicketsRefunded(DateTime dtReportFrom,
                                    DateTime dtReportTo,
                                    DateTime dtFlightFrom,
                                    DateTime dtFlightTo,
                                    string strOrigin,
                                    string strDestination,
                                    string strAgency,
                                    string strAirline,
                                    string strFlight);
        DataSet GetTicketsCancelled(string strOrigin,
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
                                    string strBookingSegmentID);
        DataSet GetTicketsNotFlown(string strOrigin, string strDestination, string strAgency, string strAirline, string strFlight,
        DateTime dtReportFrom, DateTime dtReportTo, DateTime dtFlightFrom, DateTime dtFlightTo, bool bUnflown, bool bNoShow);
        DataSet GetTicketsExpired(DateTime dtReportFrom,
                                    DateTime dtReportTo,
                                    DateTime dtFlightFrom,
                                    DateTime dtFlightTo,
                                    string strOrigin,
                                    string strDestination,
                                    string strAgency,
                                    string strAirline,
                                    string strFlight);
        DataSet GetCashbookPayments(string strAgency,
                                    string strGroup,
                                    string strUserId,
                                    DateTime dtPaymentFrom,
                                    DateTime dtPaymentTo,
                                    string strCashbookId);
        DataSet GetCashbookCharges(string XmlCashbookCharges, string strCashbookId);
        DataSet GetBookingFeeAccounted(string strAgencyCode, string strUserId, string strFee, DateTime dtFrom, DateTime dtTo);
        DataSet CreditCardPayment(ref string strCCNumber,
                                ref string strTransType,
                                ref string strTransStatus,
                                ref DateTime dtFrom,
                                ref DateTime dtTo,
                                ref string strCCType,
                                ref string strAgency);
        DataSet GetOutstanding(string strAgencyCode,
                                string strAirline,
                                string strFlightNumber,
                                DateTime dtFlightFrom,
                                DateTime dtFlightTo,
                                string strOrigin,
                                string strDestination,
                                bool bOffices,
                                bool bAgencies,
                                bool bLastTwentyFourHours,
                                bool bTicketedOnly,
                                int iOlderThanHours,
                                string strLanguage,
                                bool bAccountsPayable);
        DataSet GetAgencyAccountTransactions(string agencyCode, DateTime dateFrom, DateTime dateTo);
        bool CompleteRemark(string XmlRemarks, string RemarkId, string UserId);
        DataSet GetActivities(string AgencyCode,
                            string RemarkType,
                            string Nickname,
                            DateTime TimelimitFrom,
                            DateTime TimelimitTo,
                            bool PendingOnly,
                            bool IncompleteOnly,
                            bool IncludeRemarks,
                            bool showUnassigned);
        DataSet GetCashbookPaymentsSummary(string XmlCashbookPaymentsAll);
        DataSet GetAgencyAccountTopUp(string strAgencyCode, string strCurrency, DateTime dtFrom, DateTime dtTo);
        Users TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword);
        void InitializeUserAccountID(string UserAccountId);
        Agents GetAgencySessionProfile(string AgencyCode, string UserAccountID);
        DataSet GetCorporateSessionProfile(string clientId, string LastName);
        DataSet GetTicketSales(string AgencyCode, string UserId, string Origin, string Destination, string Airline, string FlightNumber,
            DateTime FlightFrom, DateTime FlightTo, DateTime TicketingFrom, DateTime TicketingTo, string PassengerType, string Language);
        DataSet GetAgencyTicketSales(string strAgency, string strCurrency, DateTime dtSalesFrom, DateTime dtSalesTo);
        string GetUserList(string UserLogon, string UserCode, string LastName, string FirstName, string AgencyCode, string StatusCode);
        string UserRead(string UserID);
        bool UserSave(string strUsersXml, string agencyCode);
        DataSet GetServiceFees(ref string strOrigin, ref string strDestination, ref string strCurrency, ref string strAgency,
            ref string strServiceGroup, ref System.DateTime dtFee);
        DataSet GetClientSessionProfile(string clientProfileId);
        DataSet GetBookingHistory(string bookingId);
        string GetAvailabilityAgencyLogin(string Origin,
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
                                            bool bNoVat);
        bool InsertPaymentApproval(string strPaymentApprovalID, string strCardRcd, string strCardNumber, string strNameOnCard, int iExpiryMonth, int iExpiryYear, int iIssueMonth, int iIssueYear, int iIssueNumber, string strPaymentStateCode, string strBookingPaymentId, string strCurrencyRcd, double dPaymentAmount, string strIpAddress);
        bool UpdatePaymentApproval(string strApprovalCode,
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
                    string strCardType);
        Double GetExchangeRateRead(string strOriginCurrencyCode, string strDestCurrencyCode);
        // Start Yai Add Account Topup
        bool AgencyAccountAdd(string strAgencyCode, string strCurrency, string strUserId, string strComment, double dAmount, string strExternalReference, string strInternalReference, string strTransactionReference, bool bExternalTopup);
        bool AgencyAccountVoid(string strAgencyAccountId, string strUserId);
        DataSet ExternalPaymentListAgencyTopUp(string strAgencyCode);
        // End Yai Add Account Topup
        string GetSessionlessCurrencies(string language, string strToken);
        DataSet PassengerRoleRead(string strPaxRoleCode, string strPaxRole, string strStatus, bool bWrite, string strLanguage);
        string GetBookingClasses(string strBoardingclass, string language);
        bool AgencyRegistrationInsert(string agencyName,
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
                                    string comment);
        bool LowFareFinderAllow(string agencyCode, string strToken);

        string ViewBookingChange(string bookingID, string languageCode, string AgencyCode);
        #region IClient B2A
        DataSet GetAgencyAccountBalance(string strAgencyCode, string strAgencyName, string strCurrency, string strConsolidatorAgency);
        bool AddNewAgency(string strXmlAgency, string strAgencyCode, string strDefaultUserLogon, string strDefaultPassword, string strDefaultLastName, string strDefaultFirstName, string strCreateUser, short iChangeBooking, short iChangeSegment, short iDeleteSegment, short iTicketIssue);
        bool AdjustSubAgencyAccountBalance(string strXmlChildAccountBalance, string strXmlParentAccountBalance);
        DataSet AgencyRead(string strAgencyCode, bool bKeepSession);
        string SavePayment(string bookingId,
                          BookingHeader header,
                          Itinerary segment,
                          Passengers passenger,
                          Payments payment,
                          Mappings mapping,
                          Fees fee,
                          Fees paymentFee,
                          bool createTickets);
        string SingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat);
        string SessionlessSingleFlightQuoteSummary(Flights flights, Passengers passengers, string strAgencyCode, string strToken, string strLanguage, string strCurrencyCode, bool bNoVat);
        string SegmentFee(string strAgencyCode, string strCurrency, string[] strFeeRcdGroup, Mappings mappings, int iPassengerNumber, int iInfantNumber, string strLanguage, bool specialService, bool bNoVat);
        string SpecialServiceRead(string strSpecialServiceCode,
                                         string strSpecialServiceGroupCode,
                                         string strSpecialService,
                                         string strStatus,
                                         bool bTextAllowed,
                                         bool bTextRequired,
                                         bool bInventoryControl,
                                         bool bServiceOnRequest,
                                         bool bManifest,
                                         bool bWrite,
                                         string strLanguage);
        Currencies GetCurrencies(string language);
        Fees GetSessionlessFeesDefinition(string strLanguage, string strToken);
        Fees GetFee(string strFee, string strCurrency, string strAgencyCode, string strClass, string strFareBasis, string strOrigin, string strDestination, string strFlightNumber, DateTime dtDeparture, string strLanguage, bool bNoVat);
        string GetBaggageFeeOptions(Mappings mappings, Guid gSegmentId, Guid gPassengerId, string strAgencyCode, string strLanguage, long lMaxunits, Fees fees, bool bApplySegmentFee, bool bNoVat);
        string SessionlessExternalPaymentAddPayment(string strBookingId,
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
                                                    string strToken);
        string GetAvailabilety(string strFlightID,
                               string strOriginRcd,
                               string strDestinationRcd,
                               string strSpecialServiceRcd,
                               string strBoardingClassRcd);


        string AddFee(string bookingId,
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
                      bool bNoVat);
        string GetPassengerRole(string strLanguage);
        Services GetSpecialServices(string strLanguage);
        string GetActiveBookings(Guid gClientProfileId);
        string GetFlownBookings(Guid gClientProfileId);
        string GetBooking(Guid bookingId);
        string GetFlightsFLIFO(string originRcd,
                                string destinationRcd,
                                string airlineCode,
                                string flightNumber,
                                DateTime flightFrom,
                                DateTime flightTo,
                                string languageCode,
                                string token);

        string VoucherTemplateList(string voucherTemplateId,
                                          string voucherTemplate,
                                          DateTime fromDate,
                                          DateTime toDate,
                                          bool write,
                                          string status,
                                          string language);

        bool SaveVoucher(Vouchers vouchers);
        string VoucherPaymentCreditCard(Payments payment, Vouchers vouchers);
        string ReadVoucher(Guid voucherId, double voucherNumber);
        bool VoidVoucher(Guid voucherId, Guid userId, DateTime voidDate);

        string RemarkAdd(string remarkType,
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
                        DateTime timelimitUTC);

        bool RemarkDelete(Guid bookingRemarkId);
        bool RemarkComplete(Guid bookingRemarkId, Guid userId);

        string RemarkRead(string remarkId,
                            string bookingId,
                            string bookingReference,
                            double bookingNumber,
                            bool readOnly);

        bool RemarkSave(Remarks remarks);

        string GetPassenger(string airline,
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
                            string language);

        string GetQueueCount(string agency, bool unassigned);
        string BoardingClassRead(string boardingClassCode, string boardingClass, string sortSeq, string status, bool bWrite);
        int UpdatePassengerDocumentDetails(Passengers passengers);
        string GetFlightSummary(Passengers passengers, Flights flights, string strAgencyCode, string strLanguage, string strCurrencyCode, bool bNoVat);
        #endregion
    }
}
