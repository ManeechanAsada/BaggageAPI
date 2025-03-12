using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class PassengersManifest : LibraryBase
    {
        public PassengersManifest()
        {
         
        }
        string _token = string.Empty;
        public PassengersManifest(string strToken)
        {
            _token = strToken;
        }
        public PassengerManifest this[int Index]
        {
            get { return (PassengerManifest)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(PassengerManifest Value)
        {
            return this.List.Add(Value);
        }
        #region method
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
            try
            {
                string strXML = string.Empty;
                ServiceClient obj = new ServiceClient();
                obj.objService = objService;

                strXML = obj.GetPassenger(airline,
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

                obj.objService = null;

                return strXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
