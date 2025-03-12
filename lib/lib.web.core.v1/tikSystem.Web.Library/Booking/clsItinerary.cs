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
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    public class Itinerary : Flights
    {
        #region Property
        bool _IsInternationalFlight = false;
        [XmlIgnoreAttribute]
        public bool IsInternationalFlight
        {
            get { return _IsInternationalFlight; }
            set { _IsInternationalFlight = value; }
        }
        #endregion

        public Itinerary(){}

        public new FlightSegment this[int index]
        {
            get { return (FlightSegment)this.List[index];}
            set { this.List[index] = value; }
        }
        public int Add(FlightSegment Value)
        {
            return this.List.Add(Value);
        }

        #region Method
        public bool IsConnectingFlight(string odOriginRcd, string odDestinationRcd)
        {
            if (this.Count > 0)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].od_origin_rcd.Equals(odOriginRcd) & this[i].od_destination_rcd.Equals(odDestinationRcd))
                    {
                        if (this[i].flight_connection_id.Equals(Guid.Empty) == false)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool FillExtendedSegmentInformation(Flights flight)
        {
            try
            {
                if (this.Count > 0 && flight != null && flight.Count > 0)
                {
                    for (int i = 0; i < flight.Count; i++)
                    {
                        for (int j = 0; j < this.Count; j++)
                        {
                            if (flight[i].flight_id.Equals(this[j].flight_id))
                            {
                                //Fill Aricraft Type information.
                                if (string.IsNullOrEmpty(flight[i].aircraft_type_rcd) == false)
                                {
                                    this[j].aircraft_type_rcd = flight[i].aircraft_type_rcd;
                                }

                                //Fill Transit point information
                                if (string.IsNullOrEmpty(flight[i].transit_points) == false)
                                {
                                    this[j].transit_points = flight[i].transit_points;
                                    this[j].transit_points_name = flight[i].transit_points_name;
                                }
                                
                            }
                        }
                        
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool FillDepartureDateDetail()
        {
            try
            {
                if (this.Count > 0)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        this[i].departure_dayOfWeek = (Byte)this[i].departure_date.DayOfWeek;
                        this[i].departure_month = (Byte)this[i].departure_date.Month;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string GetBookingSegmentIdComboFlight(string strBookingSegmentId)
        {
            Guid flight_connection_id = Guid.Empty;
            string booking_segment_id = string.Empty;

            // find flight_connection_id
            if (this.Count > 0)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].booking_segment_id.Equals(new Guid(strBookingSegmentId)))
                    {
                        if (!this[i].flight_connection_id.Equals(Guid.Empty))
                            flight_connection_id = this[i].flight_connection_id;
                    }
                }
            }

            //find booking_segment_id
            if (!flight_connection_id.Equals(Guid.Empty))
            {
                if (this.Count > 0)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (this[i].flight_connection_id.Equals(flight_connection_id) && !this[i].booking_segment_id.Equals(strBookingSegmentId))
                        {
                            booking_segment_id = this[i].booking_segment_id.ToString();
                        }
                    }
                }
            }

            return booking_segment_id;
        }
        public FlightSegment GetFlightSegment(Guid bookingSegmentId)
        {
            //find Itinerary
            if (!bookingSegmentId.Equals(Guid.Empty))
            {
                if (this.Count > 0)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (this[i].booking_segment_id.Equals(bookingSegmentId))
                        {
                            return this[i];
                        }
                    }
                }
            }
            return null;
        }
        public Itinerary GetOdSegment(string originRcd, string destinationRcd)
        {
            Itinerary itinerary = new Itinerary();
            try
            {

                for (int i = 0; i < this.Count; i++)
                {
                    if (originRcd.Equals(this[i].od_origin_rcd) & destinationRcd.Equals(this[i].od_destination_rcd))
                    {
                        itinerary.Add(this[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itinerary;
        }
        public FlightSegment GetOdFirstSegment(string originRcd, string destinationRcd)
        {
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (originRcd.Equals(this[i].od_origin_rcd) & destinationRcd.Equals(this[i].od_destination_rcd))
                    {
                        return this[i];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public void FillFeesOD(ref Fees fees)
        {
            if (fees == null) return;
            for (int i = 0; i < this.Count; i++)
            {
                Guid booking_segment_id = this[i].booking_segment_id;
                for (int j = 0; j < fees.Count; j++)
                {
                    if (string.IsNullOrEmpty(fees[j].od_origin_rcd) && booking_segment_id.Equals(fees[j].booking_segment_id))
                    {
                        fees[j].od_origin_rcd = this[i].od_origin_rcd;
                        fees[j].od_destination_rcd = this[i].od_destination_rcd;
                    }
                }
            }
        }

        #endregion
        #region Helper
        public void FillAirportName(Routes objOrigin, Routes objDestination)
        {
            Library objLi = new Library();
            foreach (FlightSegment f in this)
            {
                f.origin_name = objLi.FindOriginName(objOrigin, f.origin_rcd.Trim());
                f.destination_name = objLi.FindDestinationName(objDestination, f.destination_rcd.Trim());
                //add for Multi Stop
                if (string.IsNullOrEmpty(f.transit_points) == false)
                {
                    f.transit_points_name = objLi.FindOriginName(objOrigin, f.transit_points.Trim());
                }
            }
        }
        public string FindUSSegment()
        {
            try
            {
                if (this != null && this.Count > 0)
                {
                    System.Text.StringBuilder stb = new System.Text.StringBuilder();
                    foreach (FlightSegment se in this)
                    {
                        if (se.segment_status_rcd == "US")
                        {
                            stb.Append(se.airline_rcd + " " + se.flight_number);
                        }
                    }
                    return stb.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Function FindUSSegment : " + e.Message + "\n" + e.StackTrace);
            }

        }
        public void FillIsInternationalFlight(Routes RoutesOrigin, Routes RoutesDestination, string OriginRcd, string DestinationRcd, string Country)
        {
            Library objLi = new Library();
            bool international = true;
            string CountryOrigin = string.Empty;
            string CountryDestination = string.Empty;
            //get Country Origin from route
            for (int i = 0; i < RoutesOrigin.Count; i++)
            {
                if (RoutesOrigin[i].origin_rcd == OriginRcd)
                    CountryOrigin = RoutesOrigin[i].country_rcd.ToUpper();
            }
            //get Country Destination from route
            for (int i = 0; i < RoutesDestination.Count; i++)
            {
                if (RoutesDestination[i].destination_rcd == DestinationRcd)
                    CountryDestination = RoutesDestination[i].country_rcd.ToUpper();
            }
            if (!String.IsNullOrEmpty(CountryOrigin) && !String.IsNullOrEmpty(CountryDestination))
            {
                // check country origin with country Agency AND country destination 
                if (Country.Equals(CountryOrigin) && Country.Equals(CountryDestination))
                {
                    international = false;
                }
            }
            else
            {
                for (int i = 0; i <= RoutesDestination.Count; i++)
                {
                    if (RoutesDestination[i].origin_rcd == OriginRcd & RoutesDestination[i].destination_rcd == DestinationRcd)
                    {
                        if (RoutesDestination[i].require_document_details_flag == true)
                        {
                            international = true;
                        }
                        else
                        {
                            international = false;
                        }
                        break;
                    }
                }
            }

            this._IsInternationalFlight = international;
        }
        #endregion
    }
}
