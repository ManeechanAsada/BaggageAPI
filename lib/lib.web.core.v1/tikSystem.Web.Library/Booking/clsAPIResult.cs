using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class APIResult
    {
        APIFlightSegments _flight_segments;
        public APIFlightSegments APIFlightSegments
        {
            get { return _flight_segments; }
            set { _flight_segments = value; }
        }

        APIPassengerMappings _mappings;
        public APIPassengerMappings APIPassengerMappings
        {
            get { return _mappings; }
            set { _mappings = value; }
        }

        APIRouteConfigs _routes;
        public APIRouteConfigs APIRouteConfigs
        {
            get { return _routes; }
            set { _routes = value; }
        }

        APIPassengerServices _services;
        public APIPassengerServices APIPassengerServices
        {
            get { return _services; }
            set { _services = value; }
        }

        APIPassengerAddresses _address;
        public APIPassengerAddresses APIPassengerAddresses
        {
            get { return _address; }
            set { _address = value; }
        }

        APIPassengerDocuments _doc;
        public APIPassengerDocuments APIPassengerDocuments
        {
            get { return _doc; }
            set { _doc = value; }
        }


        APIPassengerFees _fees;
        public APIPassengerFees APIPassengerFees
        {
            get { return _fees; }
            set { _fees = value; }
        }

        APIPassengerBaggages _bags;
        public APIPassengerBaggages APIPassengerBaggages
        {
            get { return _bags; }
            set { _bags = value; }
        }

        //implement for seat assign 03-04-2013
        APISeatMaps _seatmaps;
        public APISeatMaps APISeatMaps
        {
            get { return _seatmaps; }
            set { _seatmaps = value; }
        }

        APIMessageResults _message_results;
        public APIMessageResults APIMessageResults
        {
            get { return _message_results; }
            set { _message_results = value; }
        }

        APIErrors _errors;
        public APIErrors APIErrors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
    public class APIResultMessage
    {
        APIErrors _errors;
        public APIErrors APIErrors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}
