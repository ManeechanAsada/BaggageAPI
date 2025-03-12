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
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.XPath;
using System.Linq;
using tikSystem.Web.Library.SSRInventoryService;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Services : LibraryBase
    {
        public Service this[int index]
        {
            get { return (Service)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Service value)
        {
            return this.List.Add(value);
        }
        public int Add(Service value, Itinerary itinerary, Int16 serviceOnRequest)
        {
            if (serviceOnRequest == 1)
            {
                value.special_service_status_rcd = "RQ";
            }
            else
            {
                FlightSegment segment = itinerary.GetFlightSegment(value.booking_segment_id);
                if (segment != null)
                {
                    value.special_service_status_rcd = (segment.segment_status_rcd == "PO" ? "HK" : segment.segment_status_rcd) ;
                    
                }
            }
            return this.List.Add(value);
        }
        public void Remove(Service value)
        {
            this.List.Remove(value);
        }

        #region Method
        public string GetServiceFeeXml(BookingHeader header, Itinerary itinerary, string serviceGroup)
        {
            ServiceClient objClient = new ServiceClient();

            objClient.objService = objService;
            return objClient.GetServiceFeesByGroups(header, itinerary, serviceGroup);
        }
        public DataSet GetServiceFees(ref string strOrigin, ref string strDestination, ref string strCurrency, ref string strAgency, ref string strServiceGroup, ref DateTime dtFee)
        {
            ServiceClient objClient = new ServiceClient();

            objClient.objService = objService;
            return objClient.GetServiceFees(ref strOrigin, ref strDestination, ref strCurrency, ref strAgency, ref strServiceGroup, ref dtFee);
        }
 
        public void AddServices(string strXml)
        {
            try
            {
                using (System.IO.StringReader sr = new System.IO.StringReader(strXml))
                {
                    XPathDocument xmlDoc = new XPathDocument(sr);
                    XPathNavigator nv = xmlDoc.CreateNavigator();

                    Service sv;
                    foreach (XPathNavigator n in nv.Select("Booking/Service"))
                    {
                        sv = (Service)XmlHelper.Deserialize(n.OuterXml, typeof(Service));
                        if (sv != null)
                        {
                            Add(sv);
                            sv = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public void Remove(string[] array)
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Service s = this[i];
                if (array.Contains<string>(s.special_service_rcd))
                {
                    this.Remove(s);
                }
            }
        }

        #endregion

        #region SSRInventoryService
        public SSRInventoryResponse GetSSRInventoryService(string agencyCode, string password, string flightID)
        {
            SSRInventoryService.SSRInventoryService ssrService = new SSRInventoryService.SSRInventoryService();
            SSRInventoryService.UserCredential credential = new SSRInventoryService.UserCredential();
            credential.AgencyCode = agencyCode;
            credential.Password = password;

            SSRInventoryService.SSRInventoryRequest request = new SSRInventoryService.SSRInventoryRequest();
            request.FlightId = flightID;

            SSRInventoryService.SSRInventoryService service = new SSRInventoryService.SSRInventoryService();
            SSRInventoryService.SSRInventoryResponse response = service.GetSSRInventory(request, credential);

            return response;
        }

        #endregion

    }
}
