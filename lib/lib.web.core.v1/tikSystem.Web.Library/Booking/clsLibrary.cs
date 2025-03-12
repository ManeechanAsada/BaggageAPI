using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel;

namespace tikSystem.Web.Library
{
    public class Library
    {
        public string GenerateControlString(string ControlName, string Parameter)
        {
            Page pageHolder = new Page();
            using (UserControl viewControl = (UserControl)pageHolder.LoadControl(ControlName))
            {
                Type ctrlType;
                FieldInfo field;

                string strResult = string.Empty;

                if (Parameter.Length > 0)
                {
                    ctrlType = viewControl.GetType();
                    field = ctrlType.GetField("_parameter");
                    field.SetValue(viewControl, Parameter);
                }

                pageHolder.Controls.Add(viewControl);
                using (StringWriter output = new StringWriter())
                {
                    HttpContext.Current.Server.Execute(pageHolder, output, false);

                    pageHolder.Dispose();
                    viewControl.Dispose();

                    return output.ToString();
                }
            }
        }
        public string FindAirportName(DataSet ds, string airportRcd, string strSearchField)
        {
            if (airportRcd.Length > 0 && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    if (dr[strSearchField].ToString().Equals(airportRcd))
                    {
                        return dr["display_name"].ToString();
                    }
                }
            }
            return string.Empty;
        }
        public string FindOriginName(Routes origin, string airportRcd)
        {
            return FindAirportName(origin, airportRcd, "origin_rcd");
        }
        public string FindDestinationName(Routes destination, string airportRcd)
        {
            return FindAirportName(destination, airportRcd, "destination_rcd");
        }
        public string FindOriginCountry(Routes origin, string airportRcd)
        {
            if (airportRcd.Length > 0 && origin != null && origin.Count > 0)
            {
                for (int i = 0; i < origin.Count; i++)
                {
                    if (string.IsNullOrEmpty(origin[i].origin_rcd) == false && origin[i].origin_rcd.Equals(airportRcd))
                    {
                        return origin[i].country_rcd;
                    }
                }
            }
            return string.Empty;
        }
        public decimal CalOutStandingBalance(Quotes quotes,
                                             Fees fees,
                                             Payments payments)
        {
            decimal TotalPayment = 0;

            decimal TotalTicket = 0;
            decimal QuoteNotRefund = 0;
            decimal QuoteRefund = 0;
            decimal QuoteTaxRefund = 0;
            decimal QuoteTaxNotRefund = 0;
            decimal FeeAmount = 0;


            //Payment
            if (payments != null)
            {
                foreach (Payment p in payments)
                {
                    if (p.void_date_time == DateTime.MinValue)
                    {
                        TotalPayment = TotalPayment + p.payment_amount;
                    }
                }
            }

            if (quotes != null)
            {
                //Quote Refund
                foreach (Quote q in quotes)
                {
                    if (q.charge_type == "REFUND")
                    {
                        //QuoteRefund = QuoteRefund + q.charge_amount;
                        QuoteRefund = QuoteRefund + q.charge_amount_incl;
                    }
                }
                //Quote not Refund
                foreach (Quote q in quotes)
                {
                    if (q.charge_type != "REFUND" && q.charge_type == "FARE")
                    {
                        //QuoteNotRefund = QuoteNotRefund + q.charge_amount;
                        QuoteNotRefund = QuoteNotRefund + q.charge_amount_incl;
                    }
                }
                //Quote Tax Refund
                foreach (Quote q in quotes)
                {
                    if (q.charge_type == "REFUND")
                    {
                        //QuoteTaxRefund = QuoteTaxRefund + q.tax_amount;
                        QuoteTaxRefund = QuoteTaxRefund + q.total_amount;
                    }
                }
                //Quote Tax not Refund
                foreach (Quote q in quotes)
                {
                    if (q.charge_type != "REFUND" && q.charge_type != "FARE")
                    {
                        //QuoteTaxNotRefund = QuoteTaxNotRefund + q.tax_amount;
                        QuoteTaxNotRefund = QuoteTaxNotRefund + q.total_amount;
                    }
                }
            }

            //CalFee
            if (fees != null)
            {
                foreach (Fee f in fees)
                {
                    if (f.void_date_time == DateTime.MinValue)
                    {
                        FeeAmount = FeeAmount + f.fee_amount_incl;
                    }
                }
            }
            TotalTicket = ((QuoteNotRefund - QuoteRefund) + (QuoteTaxNotRefund - QuoteTaxRefund)) + FeeAmount;
            return TotalTicket - TotalPayment;
        }

        public string GenerateCobToken(string strBookingid, string ClientProfileId, string strUserId, string strAgencyId, string strLanguage, int iShowHeader)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"] != null & System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"].Length > 0 & System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"].Length > 0)
                {

                    string strValue = System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"] + "|" +
                                      string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "|" +
                                      strBookingid + "|" +
                                      ClientProfileId + "|" +
                                      strUserId + "|" +
                                      strAgencyId + "|" +
                                      strLanguage + "|" +
                                      iShowHeader;

                    return SecurityHelper.EncryptString(strValue, System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"]);
                }
            }

            return string.Empty;
        }

        #region Add object function
        public void AddAgent(string strXml, Agents agents)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Agent ag;
            foreach (XPathNavigator n in nv.Select("TikAero/Agent"))
            {
                ag = (Agent)XmlHelper.Deserialize(n.OuterXml, typeof(Agent));
                if (ag != null)
                {
                    agents.Add(ag);
                    ag = null;
                }
            }
        }
        public void AddPassengers(string strXml, Passengers uv)
        {
            XPathDocument xmlDoc;
            XPathNavigator nv;

            xmlDoc = new XPathDocument(new StringReader(strXml));
            nv = xmlDoc.CreateNavigator();
            Passenger ps;
            foreach (XPathNavigator n in nv.Select("Booking/Passenger"))
            {
                ps = (Passenger)XmlHelper.Deserialize(n.OuterXml, typeof(Passenger));
                if (ps != null)
                {
                    uv.Add(ps);
                    ps = null;
                }
            }
        }
        public void AddItinerary(string strXml, Itinerary uv)
        {
            XPathDocument xmlDoc;
            XPathNavigator nv;

            xmlDoc = new XPathDocument(new StringReader(strXml));
            nv = xmlDoc.CreateNavigator();
            FlightSegment fs;
            foreach (XPathNavigator n in nv.Select("Booking/FlightSegment"))
            {
                fs = (FlightSegment)XmlHelper.Deserialize(n.OuterXml, typeof(FlightSegment));
                if (fs != null)
                {
                    uv.Add(fs);
                    fs = null;
                }
            }
        }
        public void AddItinerary(string strXml, Itinerary uv, string strBookingSegmentId)
        {
            XPathDocument xmlDoc;
            XPathNavigator nv;

            xmlDoc = new XPathDocument(new StringReader(strXml));
            nv = xmlDoc.CreateNavigator();
            FlightSegment fs;
            foreach (XPathNavigator n in nv.Select("Booking/FlightSegment[booking_segment_id = '" + strBookingSegmentId + "']"))
            {
                fs = (FlightSegment)XmlHelper.Deserialize(n.OuterXml, typeof(FlightSegment));
                if (fs != null)
                {
                    uv.Add(fs);
                    fs = null;
                }
            }
        }
        public void AddMappings(string strXml, Mappings uv)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Mapping mp;
            foreach (XPathNavigator n in nv.Select("Booking/Mapping"))
            {
                mp = (Mapping)XmlHelper.Deserialize(n.OuterXml, typeof(Mapping));
                if (mp != null)
                {
                    uv.Add(mp);
                    mp = null;
                }
            }
        }
        public void AddMappings(string strXml, Mappings uv, string strBookingSegmentId)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Mapping mp;
            foreach (XPathNavigator n in nv.Select("Booking/Mapping[booking_segment_id = '" + strBookingSegmentId + "']"))
            {
                mp = (Mapping)XmlHelper.Deserialize(n.OuterXml, typeof(Mapping));
                if (mp != null)
                {
                    uv.Add(mp);
                    mp = null;
                }
            }
        }
        public void AddBookingHeader(string strXml, ref BookingHeader uv)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            foreach (XPathNavigator n in nv.Select("Booking/BookingHeader"))
            {
                uv = (BookingHeader)XmlHelper.Deserialize(n.OuterXml, uv.GetType());
            }

        }
        public void AddPayments(string strXml, Payments uv)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Payment pt;
            foreach (XPathNavigator n in nv.Select("Booking/Payment"))
            {
                pt = (Payment)XmlHelper.Deserialize(n.OuterXml, typeof(Payment));
                if (pt != null)
                {
                    uv.Add(pt);
                    pt = null;
                }
            }
        }
        public void AddRemarks(string strXml, Remarks uv)
        {

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Remark rk;
            foreach (XPathNavigator n in nv.Select("Booking/Remark"))
            {
                rk = (Remark)XmlHelper.Deserialize(n.OuterXml, typeof(Remark));
                if (rk != null)
                {
                    uv.Add(rk);
                    rk = null;
                }
            }
        }
        public void AddTaxes(string strXml, Taxes uv)
        {

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Tax tx;
            foreach (XPathNavigator n in nv.Select("Booking/Tax"))
            {
                tx = (Tax)XmlHelper.Deserialize(n.OuterXml, typeof(Tax));
                if (tx != null)
                {
                    uv.Add(tx);
                    tx = null;
                }
            }
        }
        public void AddQuotes(string strXml, Quotes uv)
        {

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Quote qo;
            foreach (XPathNavigator n in nv.Select("Booking/Quote"))
            {
                qo = (Quote)XmlHelper.Deserialize(n.OuterXml, typeof(Quote));
                if (qo != null)
                {
                    uv.Add(qo);
                    qo = null;
                }
            }
        }

        public void AddServices(string strXml, Services uv)
        {

            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Service sr;
            foreach (XPathNavigator n in nv.Select("Booking/Service"))
            {
                sr = (Service)XmlHelper.Deserialize(n.OuterXml, typeof(Service));
                if (sr != null)
                {
                    uv.Add(sr);
                    sr = null;
                }
            }
        }

        public void AddServices(string strXml, Services uv, string strBookingSegmentId)
        {
            XPathDocument xmlDoc = new XPathDocument(new StringReader(strXml));
            XPathNavigator nv = xmlDoc.CreateNavigator();

            Service sr;
            foreach (XPathNavigator n in nv.Select("Booking/Service[booking_segment_id = '" + strBookingSegmentId + "']"))
            {
                sr = (Service)XmlHelper.Deserialize(n.OuterXml, typeof(Service));
                if (sr != null)
                {
                    uv.Add(sr);
                    sr = null;
                }
            }
        }

        #endregion

        #region XML management function
        public enum xmlReturnType
        {
            value = 0,
            OuterXml = 1,
            InnerXml = 2,
        }
        public string RenderHtml(XslTransform objTransform, XsltArgumentList objArgument, string xml)
        {
            using (StringReader srd = new StringReader(xml))
            {
                XPathDocument objXml = new XPathDocument(srd);
                using (StringWriter objWriter = new StringWriter())
                {
                    try
                    {
                        objTransform.Transform(objXml, objArgument, objWriter);
                    }
                    catch(Exception ex)
                    { }
                    finally
                    { }

                    return objWriter.ToString();
                }
            }
        }
        public string getXPathNodevalue(XPathNavigator n, string SelectExpression, xmlReturnType rType)
        {
            XPathNodeIterator ni = n.Select(SelectExpression);
            string result = string.Empty;

            while (ni.MoveNext())
            {
                if (rType == xmlReturnType.value)
                { result = ni.Current.Value; }
                else if (rType == xmlReturnType.InnerXml)
                { result = ni.Current.InnerXml; }
                else
                { result = ni.Current.OuterXml; }
            }
            return result;
        }

        public string ExtractItinerary(XPathDocument xmlDoc, string strSelect)
        {
            XPathNavigator nv = xmlDoc.CreateNavigator();
            return nv.SelectSingleNode(strSelect).OuterXml;
        }

        public void FillBooking(string xml, ref BookingHeader bookingHeader,
                                            ref Passengers passengers,
                                            ref Itinerary itinerary,
                                            ref Mappings mappings,
                                            ref Payments payments,
                                            ref Remarks remarks,
                                            ref Taxes taxes,
                                            ref Quotes quotes,
                                            ref Fees fees,
                                            ref Services services)
        {

            using (StringReader srd = new StringReader(xml))
            {
                FillBooking(srd, ref bookingHeader, ref passengers, ref itinerary, ref mappings, ref payments, ref remarks, ref taxes, ref quotes, ref fees, ref services);
            }
        }

        public void FillBooking(StringReader srd,
                                ref BookingHeader bookingHeader,
                                ref Passengers passengers,
                                ref Itinerary itinerary,
                                ref Mappings mappings,
                                ref Payments payments,
                                ref Remarks remarks,
                                ref Taxes taxes,
                                ref Quotes quotes,
                                ref Fees fees,
                                ref Services services)
        {

            //Clear value before fill.
            if (passengers != null)
            {
                passengers.Clear();
            }
            if (itinerary != null)
            {
                itinerary.Clear();
            }
            if (mappings != null)
            {
                mappings.Clear();
            }
            if (payments != null)
            {
                payments.Clear();
            }
            if (remarks != null)
            {
                remarks.Clear();
            }
            if (taxes != null)
            {
                taxes.Clear();
            }
            if (quotes != null)
            {
                quotes.Clear();
            }
            if (fees != null)
            {
                fees.Clear();
            }
            if (services != null)
            {
                services.Clear();
            }

            using (XmlReader reader = XmlReader.Create(srd))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "BookingHeader":
                                if (bookingHeader != null)
                                { bookingHeader = (BookingHeader)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(BookingHeader)); }
                                else
                                { reader.Read(); }
                                break;
                            case "Passenger":
                                if (passengers != null)
                                {
                                    Passenger ps = (Passenger)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Passenger));
                                    if (ps != null)
                                    {
                                        passengers.Add(ps);
                                        ps = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "FlightSegment":
                                if (itinerary != null)
                                {
                                    FlightSegment fs = (FlightSegment)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(FlightSegment));
                                    if (fs != null)
                                    {
                                        itinerary.Add(fs);
                                        fs = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Mapping":
                                if (mappings != null)
                                {
                                    Mapping mp = (Mapping)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Mapping));
                                    if (mp != null)
                                    {
                                        mappings.Add(mp);
                                        mp = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Payment":
                                if (payments != null)
                                {
                                    Payment p = (Payment)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Payment));
                                    if (p != null)
                                    {
                                        payments.Add(p);
                                        p = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Remark":
                                if (remarks != null)
                                {
                                    Remark rm = (Remark)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Remark));
                                    if (rm != null)
                                    {
                                        remarks.Add(rm);
                                        rm = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Tax":
                                if (taxes != null)
                                {
                                    Tax tx = (Tax)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Tax));
                                    if (tx != null)
                                    {
                                        taxes.Add(tx);
                                        tx = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Quote":
                                if (quotes != null)
                                {
                                    Quote qu = (Quote)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Quote));
                                    if (qu != null)
                                    {
                                        quotes.Add(qu);
                                        qu = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Fee":
                                if (fees != null)
                                {
                                    Fee fe = (Fee)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Fee));
                                    if (fe != null)
                                    {
                                        fees.Add(fe);
                                        fe = null;
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                                break;
                            case "Service":
                                if (services != null)
                                {
                                    Service se = (Service)XmlHelper.Deserialize(reader.ReadOuterXml(), typeof(Service));
                                    if (se != null)
                                    {
                                        services.Add(se);
                                        se = null;
                                    }
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

        public void ClearBooking(ref BookingHeader bookingHeader,
                                 ref Itinerary itinerary,
                                 ref Passengers passengers,
                                 ref Quotes quotes,
                                 ref Fees fees,
                                 ref Mappings mappings,
                                 ref Services services,
                                 ref Remarks remarks,
                                 ref Payments payments,
                                 ref Taxes taxes)
        {
            //Clear Booking Header
            if (bookingHeader != null)
            {
                bookingHeader.booking_id = Guid.Empty;
            }
            //Clear Passenger
            if (passengers != null)
            {
                passengers.Clear();
            }
            //Add FlightSegment
            if (itinerary != null)
            {
                itinerary.Clear();
            }
            //Add Passenger segment mapping
            if (mappings != null)
            {
                mappings.Clear();
            }
            //Add Payment
            if (payments != null)
            {
                payments.Clear();
            }
            //Add Remark
            if (remarks != null)
            {
                remarks.Clear();
            }
            //Add Taxes
            if (taxes != null)
            {
                taxes.Clear();
            }
            //Add Quotes
            if (quotes != null)
            {
                quotes.Clear();
            }
            //Add Fees
            if (fees != null)
            {
                fees.Clear();
            }
            //Add Services
            if (services != null)
            {
                services.Clear();
            }
        }
        public void RetrivedSegmentMappingXml(XmlWriter xtw, Itinerary itinerary, Mappings mappings, string selectedFlightId)
        {

            xtw.WriteStartElement("Booking");
            {
                xtw.WriteStartElement("Setting");
                {
                    xtw.WriteStartElement("selected_flight_id");
                    {
                        xtw.WriteValue(selectedFlightId);
                    }
                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                BuiltBookingXml(xtw, null, itinerary, null, null, null, mappings, null, null, null, null);
            }
            xtw.WriteEndElement();
        }

        #endregion

        #region Function Helper
        public void BuiltBookingXml(XmlWriter xtw,
                                        string xml,
                                        bool bShowItinerary,
                                        bool bShowPassenger,
                                        bool bShowHeader,
                                        bool bShowQuote,
                                        bool bShowFee,
                                        bool bShowMapping,
                                        bool bShowService,
                                        bool bShowRemark,
                                        bool bShowPayment,
                                        bool bShowTax)
        {
            Library li = new Library();

            //Get Booking header
            XmlReaderSettings setting = new XmlReaderSettings();
            setting.IgnoreWhitespace = true;
            using (XmlReader reader = XmlReader.Create(xml, setting))
            {
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("BookingHeader") && bShowHeader == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("FlightSegment") && bShowItinerary == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("passenger") && bShowPassenger == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("quote") && bShowQuote == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("fee") && bShowFee == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("mapping") && bShowMapping == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("service") && bShowService == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("remark") && bShowRemark == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("payment") && bShowPayment == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("tax") && bShowTax == true)
                    {
                        xtw.WriteNode(reader, false);
                    }
                    else
                    {
                        reader.Read();
                    }
                }
            }
        }

        public void BuiltBookingXml(XmlWriter xtw,
                                        BookingHeader bookingHeader,
                                        Itinerary itinerary,
                                        Passengers passengers,
                                        Quotes quotes,
                                        Fees fees,
                                        Mappings mappings,
                                        Services services,
                                        Remarks remarks,
                                        Payments payments,
                                        Taxes taxes)
        {
            Library li = new Library();

            XmlReaderSettings xmlsetting = new XmlReaderSettings();
            xmlsetting.IgnoreWhitespace = true;
            //Get Booking header
            if (bookingHeader != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(bookingHeader, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("BookingHeader"))
                        {
                            xtw.WriteNode(reader, false);
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (itinerary != null)
            {
                //Get itinerary Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(itinerary, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfFlightSegment"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (passengers != null)
            {
                //Get Passenger Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(passengers, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfPassenger"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (quotes != null)
            {
                //Get Quote Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(quotes, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfQuote"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }

            if (fees != null)
            {
                //Get Fee Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(fees, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfFee"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (mappings != null)
            {
                //Get mapping Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(mappings, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfMapping"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (services != null)
            {
                //Get service Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(services, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfService"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (remarks != null)
            {
                //Get remark Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(remarks, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfRemark"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
            if (payments != null)
            {
                //Get payment Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(payments, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfPayment"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }

            if (taxes != null)
            {
                //Get taxes Information
                using (XmlReader reader = XmlReader.Create(new StringReader(XmlHelper.Serialize(taxes, false)), xmlsetting))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("ArrayOfTax"))
                        {
                            xtw.WriteRaw(reader.ReadInnerXml());
                            break;
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        public string BuiltBookingXml(string xml,
                                    bool bShowItinerary,
                                    bool bShowPassenger,
                                    bool bShowHeader,
                                    bool bShowQuote,
                                    bool bShowFee,
                                    bool bShowMapping,
                                    bool bShowService,
                                    bool bShowRemark,
                                    bool bShowPayment,
                                    bool bShowTax,
                                    bool bIncludeBookingTag)
        {
            XPathDocument xmlDoc;
            XPathNavigator nv;

            StringBuilder str = new StringBuilder();
            Library li = new Library();

            if (bIncludeBookingTag == true)
            {
                str.Append("<Booking>");
            }

            xmlDoc = new XPathDocument(new StringReader(xml));

            if (bShowHeader == true)
            {
                //Get Booking header
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/BookingHeader", Library.xmlReturnType.OuterXml));
            }
            if (bShowItinerary == true)
            {
                //Get itinerary Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/itinerary", Library.xmlReturnType.InnerXml));
            }
            if (bShowPassenger == true)
            {
                //Get Passenger Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/passengers", Library.xmlReturnType.InnerXml));
            }
            if (bShowQuote == true)
            {
                //Get Quote Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/quotes", Library.xmlReturnType.InnerXml));
            }

            if (bShowFee == true)
            {
                //Get Fee Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/fees", Library.xmlReturnType.InnerXml));
            }
            if (bShowMapping == true)
            {
                //Get mapping Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/mappings", Library.xmlReturnType.InnerXml));
            }
            if (bShowService == true)
            {
                //Get service Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/services", Library.xmlReturnType.InnerXml));
            }
            if (bShowRemark == true)
            {
                //Get remark Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/remarks", Library.xmlReturnType.InnerXml));
            }
            if (bShowPayment == true)
            {
                //Get payment Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/payments", Library.xmlReturnType.InnerXml));
            }

            if (bShowTax == true)
            {
                //Get payment Information
                nv = xmlDoc.CreateNavigator();
                str.Append(li.getXPathNodevalue(nv, "Booking/taxes", Library.xmlReturnType.InnerXml));
            }
            xmlDoc = null;
            if (bIncludeBookingTag == true)
            {
                str.Append("</Booking>");
            }


            return str.ToString();
        }
        public string BuiltBookingXml(BookingHeader bookingHeader,
                                      Itinerary itinerary,
                                      Passengers passengers,
                                      Quotes quotes,
                                      Fees fees,
                                      Mappings mappings,
                                      Services services,
                                      Remarks remarks,
                                      Payments payments,
                                      Taxes taxes,
                                      bool bIncludeBookingTag)
        {
            StringBuilder str = new StringBuilder();
            Library li = new Library();

            if (bIncludeBookingTag == true)
            {
                str.Append("<Booking>");
            }

            if (bookingHeader != null)
            {
                //Get Booking header
                str.Append(XmlHelper.Serialize(bookingHeader, false));
            }
            if (itinerary != null)
            {
                //Get itinerary Information
                str.Append(XmlHelper.Serialize(itinerary, false));
            }
            if (passengers != null)
            {
                //Get Passenger Information
                str.Append(XmlHelper.Serialize(passengers, false));
            }
            if (quotes != null)
            {
                //Get Quote Information
                str.Append(XmlHelper.Serialize(quotes, false));
            }

            if (fees != null)
            {
                //Get Fee Information
                str.Append(XmlHelper.Serialize(fees, false));
            }
            if (mappings != null)
            {
                //Get mapping Information
                str.Append(XmlHelper.Serialize(mappings, false));
            }
            if (services != null)
            {
                //Get service Information
                str.Append(XmlHelper.Serialize(services, false));
            }
            if (remarks != null)
            {
                //Get remark Information
                str.Append(XmlHelper.Serialize(remarks, false));
            }
            if (payments != null)
            {
                //Get payment Information
                str.Append(XmlHelper.Serialize(payments, false));
            }

            if (taxes != null)
            {
                //Get taxes Information
                str.Append(XmlHelper.Serialize(taxes, false));
            }

            if (bIncludeBookingTag == true)
            {
                str.Append("</Booking>");
            }

            //Replace string.
            str.Replace("<ArrayOfFlightSegment>", string.Empty);
            str.Replace("</ArrayOfFlightSegment>", string.Empty);

            str.Replace("<ArrayOfPassenger>", string.Empty);
            str.Replace("</ArrayOfPassenger>", string.Empty);

            str.Replace("<ArrayOfQuote>", string.Empty);
            str.Replace("</ArrayOfQuote>", string.Empty);

            str.Replace("<ArrayOfFee>", string.Empty);
            str.Replace("</ArrayOfFee>", string.Empty);

            str.Replace("<ArrayOfMapping>", string.Empty);
            str.Replace("</ArrayOfMapping>", string.Empty);

            str.Replace("<ArrayOfService>", string.Empty);
            str.Replace("</ArrayOfService>", string.Empty);

            str.Replace("<ArrayOfRemark>", string.Empty);
            str.Replace("</ArrayOfRemark>", string.Empty);

            str.Replace("<ArrayOfPayment>", string.Empty);
            str.Replace("</ArrayOfPayment>", string.Empty);

            str.Replace("<ArrayOfTax>", string.Empty);
            str.Replace("</ArrayOfTax>", string.Empty);

            return str.ToString();
        }

        public APIResult BuildAPIResultXML(APIFlightSegments flights, APIPassengerMappings mappings, APIRouteConfigs routes, APIPassengerServices services, APIPassengerFees fees, APISeatMaps seatmaps, APIMessageResults message_results, APIErrors errors)
        {
            APIResult result = new APIResult();
            result.APIFlightSegments = flights;
            result.APIPassengerMappings = mappings;
            result.APIRouteConfigs = routes;
            result.APIPassengerServices = services;
            result.APIPassengerFees = fees;
            result.APISeatMaps = seatmaps;
            result.APIMessageResults = message_results;
            result.APIErrors = errors;
            return result;
        }

        public APIResult BuildAPIResultExtensionXML(APIFlightSegments flights, APIPassengerMappings mappings, APIRouteConfigs routes, APIPassengerServices services, APIPassengerAddresses address, APIPassengerDocuments docs, APIPassengerFees fees, APISeatMaps seatmaps, APIMessageResults message_results, APIErrors errors)
        {
            APIResult result = new APIResult();
            result.APIFlightSegments = flights;
            result.APIPassengerMappings = mappings;
            result.APIRouteConfigs = routes;
            result.APIPassengerServices = services;
            result.APIPassengerAddresses = address;
            result.APIPassengerDocuments = docs;
            result.APIPassengerFees = fees;
            result.APISeatMaps = seatmaps;
            result.APIMessageResults = message_results;
            result.APIErrors = errors;
            return result;
        }

        public APIResultMessage BuildAPIResultXML(APIErrors errors)
        {
            APIResultMessage result = new APIResultMessage();
            result.APIErrors = errors;
            return result;
        }

        public APIResult BuildAPIResultBagsXML(APIFlightSegments flights, APIPassengerMappings mappings, APIRouteConfigs routes, APIPassengerServices services, APIPassengerFees fees, APIPassengerBaggages bags, APISeatMaps seatmaps, APIMessageResults message_results, APIErrors errors)
        {
            APIResult result = new APIResult();
            result.APIFlightSegments = flights;
            result.APIPassengerMappings = mappings;
            result.APIRouteConfigs = routes;
            result.APIPassengerServices = services;
            result.APIPassengerFees = fees;
            result.APIPassengerBaggages = bags;
            result.APISeatMaps = seatmaps;
            result.APIMessageResults = message_results;
            result.APIErrors = errors;
            return result;
        }


        public bool isVoucherDuplicate(Vouchers vouchers, Vouchers objVouchers)
        {
            bool result = false;

            if (vouchers == null || vouchers.Count == 0)
            { result = false; }
            else
            {

                foreach (Voucher mv in objVouchers)
                {
                    foreach (Voucher v in vouchers)
                    {
                        if (v.voucher_number.Equals(mv.voucher_number) == true)
                        {
                            result = true;
                            break;
                        }
                    }
                    if (result == true)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public Allocations GetPaymentAllocation(Mappings mappings, Fees fees, Guid gUserId)
        {
            decimal dPaid = 0;
            decimal dCharge = 0;
            decimal dNet = 0;
            decimal dOutstanding = 0;

            Allocations allocations = new Allocations();


            if (mappings != null && mappings.Count > 0)
            {
                foreach (Mapping mp in mappings)
                {
                    dNet = mp.net_total;
                    if (mp.exclude_pricing_flag == 1)
                    {
                        dCharge = mp.payment_amount;
                    }
                    else if (mp.refund_date_time != DateTime.MinValue)
                    {
                        dCharge = mp.refund_charge;
                    }
                    else
                    {
                        decimal dExchangePaid = 0;
                        foreach (Mapping em in mappings)
                        {
                            if (em.exchanged_date_time != DateTime.MinValue && mp.booking_segment_id == em.exchanged_segment_id && mp.passenger_id == em.passenger_id)
                            {
                                dExchangePaid = em.payment_amount;
                                break;
                            }
                        }
                        dCharge = mp.net_total - dExchangePaid;
                    }
                    dPaid = mp.payment_amount;
                    dOutstanding = dCharge - dPaid;
                    if (dOutstanding != 0)
                    {
                        Allocation allocation = new Allocation();
                        allocation.passenger_id = mp.passenger_id;
                        allocation.booking_segment_id = mp.booking_segment_id;
                        allocation.user_id = gUserId;
                        allocation.currency_rcd = mp.currency_rcd;
                        allocation.sales_amount = Convert.ToDouble(dOutstanding);
                        allocations.Add(allocation);
                        allocation = null;
                    }
                }
            }
            if (fees != null && fees.Count > 0)
            {
                foreach (Fee f in fees)
                {
                    if (f.void_date_time == DateTime.MinValue)
                    {
                        dCharge = f.fee_amount_incl;
                    }
                    else
                    {
                        dCharge = 0;
                    }
                    dPaid = f.payment_amount;
                    dOutstanding = dCharge - dPaid;

                    if (dOutstanding != 0)
                    {
                        Allocation allocation = new Allocation();
                        allocation.booking_fee_id = f.booking_fee_id;
                        allocation.fee_id = f.fee_id;
                        allocation.user_id = gUserId;
                        allocation.currency_rcd = f.currency_rcd;
                        allocation.sales_amount = Convert.ToDouble(dOutstanding);

                        allocations.Add(allocation);
                        allocation = null;
                    }
                }
            }
            return allocations;
        }

        public bool ValidSave(BookingHeader bookingHeader,
                               Itinerary itinerary,
                               Passengers passengers,
                               Mappings mappings)
        {
            try
            {
                if (bookingHeader == null)
                { return false; }
                else if (itinerary == null || itinerary.Count == 0)
                { return false; }
                else if (passengers == null)
                { return false; }
                else if (mappings == null)
                { return false; }
                else if (string.IsNullOrEmpty(itinerary.FindUSSegment()) == false)
                { return false; }
                else
                {
                    bool bGroupBooking = false;
                    //Validate Currency Code.
                    if (string.IsNullOrEmpty(bookingHeader.group_name) == false)
                    {
                        bGroupBooking = true;
                    }

                    //validate passenger and mapping information.
                    if (passengers.Valid(bGroupBooking) == false)
                    {
                        return false;
                    }
                    if (passengers.ValidDateOfBirth(mappings[0].departure_date) == false)
                    {
                        return false;
                    }
                    else if (mappings.Valid(bGroupBooking, bookingHeader.currency_rcd) == false)
                    {
                        return false;
                    }
                    else if (mappings.ValidDateOfBirth(mappings[0].departure_date) == false)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime GetFirstDateOfWeek(System.Globalization.CultureInfo ci, DateTime dtCurrentDateTime)
        {
            DayOfWeek currentfdayname = ci.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek currenttdayname = ci.Calendar.GetDayOfWeek(dtCurrentDateTime);

            int diff = currentfdayname - currenttdayname;
            if (diff <= 0)
            {
                return dtCurrentDateTime.AddDays(diff);
            }
            else
            {
                return dtCurrentDateTime.AddDays(diff - 7);
            }
        }
        public DateTime GetEndDateOfWeek(System.Globalization.CultureInfo ci, DateTime dtCurrentDateTime)
        {
            DayOfWeek currentfdayname = ci.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek currenttdayname = ci.Calendar.GetDayOfWeek(dtCurrentDateTime);

            int diff = currentfdayname - currenttdayname;
            if (diff <= 0)
            {
                return dtCurrentDateTime.AddDays(diff + 6);
            }
            else
            {
                return dtCurrentDateTime.AddDays(diff - 1);
            }
        }
        public TimeSpan DateDifferent(DateTime dtStart, DateTime dtEnd)
        {
            return dtEnd.Subtract(dtStart);
        }
        public decimal ReadVoucherAmount(Vouchers objVoucher, Guid gVoucherId)
        {
            for (int i = 0; i < objVoucher.Count; i++)
            {
                if (objVoucher[i].voucher_id.Equals(gVoucherId))
                {
                    return objVoucher[i].voucher_value - objVoucher[i].payment_total;
                }
            }
            return 0;
        }
        public void WriteLog(string Path, string strMessage)
        {
            if (string.IsNullOrEmpty(strMessage) == false)
            {
                System.IO.StreamWriter stw = null;
                try
                {
                    using (stw = new System.IO.StreamWriter(Path, true))
                    {
                        stw.WriteLine(strMessage);
                        stw.Flush();
                    }
                }
                catch
                {
                    if (stw != null)
                    {
                        stw.Close();
                    }
                }
            }
        }

        public void SetCreateUpdateInformation(Guid guUserId,
                                                BookingHeader bookingHeader,
                                                Itinerary itinerary,
                                                Passengers passengers,
                                                Fees fees,
                                                Mappings mappings,
                                                Services services,
                                                Remarks remarks,
                                                Payments payments,
                                                Taxes taxes)
        {
            try
            {
                //Booking header
                if (bookingHeader.create_by == Guid.Empty)
                {
                    bookingHeader.create_by = guUserId;
                    bookingHeader.create_date_time = DateTime.Now;
                }
                bookingHeader.update_by = guUserId;
                bookingHeader.update_date_time = DateTime.Now;

                //Flight Segment
                foreach (FlightSegment f in itinerary)
                {
                    if (f.create_by == Guid.Empty)
                    {
                        f.create_by = guUserId;
                        f.create_date_time = DateTime.Now;
                    }
                    f.update_by = guUserId;
                    f.update_date_time = DateTime.Now;
                }

                //Passenger
                foreach (Passenger p in passengers)
                {
                    if (p.create_by == Guid.Empty)
                    {
                        p.create_by = guUserId;
                        p.create_date_time = DateTime.Now;
                    }
                    p.update_by = guUserId;
                    p.update_date_time = DateTime.Now;
                }

                foreach (Fee f in fees)
                {
                    if (f.create_by == Guid.Empty)
                    {
                        f.create_by = guUserId;
                        f.create_date_time = DateTime.Now;
                    }
                    f.update_by = guUserId;
                    f.update_date_time = DateTime.Now;
                }

                foreach (Mapping mp in mappings)
                {
                    if (mp.create_by == Guid.Empty)
                    {
                        mp.create_by = guUserId;
                        mp.create_date_time = DateTime.Now;
                    }
                    mp.update_by = guUserId;
                    mp.update_date_time = DateTime.Now;
                }

                foreach (Service s in services)
                {
                    if (s.create_by == Guid.Empty)
                    {
                        s.create_by = guUserId;
                        s.create_date_time = DateTime.Now;
                    }
                    s.update_by = guUserId;
                    s.update_date_time = DateTime.Now;
                }

                foreach (Remark r in remarks)
                {
                    if (r.create_by == Guid.Empty)
                    {
                        r.create_by = guUserId;
                        r.create_date_time = DateTime.Now;
                    }
                    r.update_by = guUserId;
                    r.update_date_time = DateTime.Now;
                }
                foreach (Payment p in payments)
                {
                    if (p.create_by == Guid.Empty)
                    {
                        p.create_by = guUserId;
                        p.create_date_time = DateTime.Now;
                    }
                    p.update_by = guUserId;
                    p.update_date_time = DateTime.Now;
                }

                foreach (Tax t in taxes)
                {
                    if (t.create_by == Guid.Empty)
                    {
                        t.create_by = guUserId;
                        t.create_date_time = DateTime.Now;
                    }
                    t.update_by = guUserId;
                    t.update_date_time = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper
        private string FindAirportName(Routes routes, string airportRcd, string strSearchField)
        {
            if (airportRcd.Length > 0 && routes != null && routes.Count > 0)
            {
                for (int i = 0; i < routes.Count; i++)
                {
                    if (string.IsNullOrEmpty(routes[i].origin_rcd) == false && routes[i].origin_rcd.Equals(airportRcd) && strSearchField.Equals("origin_rcd"))
                    {
                        return routes[i].display_name;
                    }
                    else if (string.IsNullOrEmpty(routes[i].destination_rcd) == false && routes[i].destination_rcd.Equals(airportRcd) && strSearchField.Equals("destination_rcd"))
                    {
                        return routes[i].display_name;
                    }
                }
            }
            return string.Empty;
        }


        public List<object> ObjectsBinding(object objs, object obj)
        {
            List<object> list = new List<object>();
            if (objs is IEnumerable)
            {
                foreach (object o in (objs as IEnumerable))
                {
                    list.Add(ObjectBinding(objs, obj));
                }
            }
            else
            {
                list.Add(ObjectBinding(objs, obj));
            }

            return list;
        }

        public object ObjectBinding(object source, object destination)
        {
            Type type = destination.GetType();
            Type typeSource = source.GetType();
            PropertyInfo[] props = type.GetProperties();
            for (int j = 0; j < props.Length; j++)
            {
                PropertyInfo prop = type.GetProperty(props[j].Name);
                PropertyInfo propSource = typeSource.GetProperty(props[j].Name);
                if (propSource != null)
                    prop.SetValue(destination, propSource.GetValue(source, null), null);
            }
            return destination;
        }

        public object ObjectBinding(DataRow source, object destination)
        {
            try
            {
                Type type = destination.GetType();
                PropertyInfo[] props = type.GetProperties();
                for (int j = 0; j < props.Length; j++)
                {
                    PropertyInfo prop = type.GetProperty(props[j].Name);

                    DataColumn column = source.Table.Columns[props[j].Name];
                    if (column != null)
                    {
                        if (!source.IsNull(column))
                        {
                            object value = source[column];
                            if (value != null)
                            {
                                if (prop.PropertyType == typeof(System.Int16))
                                    prop.SetValue(destination, Convert.ToInt16(value), null);
                                else if (prop.PropertyType == typeof(System.Int32))
                                    prop.SetValue(destination, Convert.ToInt32(value), null);
                                else if (prop.PropertyType == typeof(System.Int64))
                                    prop.SetValue(destination, Convert.ToInt64(value), null);
                                else if (prop.PropertyType == typeof(System.UInt16))
                                    prop.SetValue(destination, Convert.ToUInt16(value), null);
                                else if (prop.PropertyType == typeof(System.UInt32))
                                    prop.SetValue(destination, Convert.ToUInt32(value), null);
                                else if (prop.PropertyType == typeof(System.UInt64))
                                    prop.SetValue(destination, Convert.ToUInt64(value), null);
                                else if (prop.PropertyType == typeof(System.Decimal))
                                    prop.SetValue(destination, Convert.ToDecimal(value), null);
                                else if (prop.PropertyType == typeof(System.Char))
                                    prop.SetValue(destination, Convert.ToChar(value), null);
                                else if (prop.PropertyType == typeof(System.Boolean))
                                    prop.SetValue(destination, Convert.ToBoolean(Convert.ToInt16(value)), null);
                                else if (prop.PropertyType == typeof(System.Byte))
                                    prop.SetValue(destination, Convert.ToByte(value), null);
                                else if (prop.PropertyType == typeof(System.Guid))
                                    prop.SetValue(destination, new Guid(value.ToString()), null);
                                else if (prop.PropertyType == typeof(System.DateTime))
                                    prop.SetValue(destination, Convert.ToDateTime(value), null);
                                else
                                    prop.SetValue(destination, value, null);
                            }
                        }
                    }
                }
                return destination;
            }
            catch (Exception ex)
            {
                return destination;
            }
        }

        public bool isCollection(object o)
        {
            return typeof(ICollection).IsAssignableFrom(o.GetType())
                || typeof(ICollection<>).IsAssignableFrom(o.GetType());
        }

        public static object GetAppSetting(string key, Type type)
        {
            object result = null;
            try
            {
                string value = string.Format("{0}", ConfigurationManager.AppSettings[key]);
                result = ChangeType(value, type);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null) throw new ArgumentNullException("conversionType");
            if (conversionType.IsGenericType &&
              conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
        #endregion

    }
    public static class DataHelper
    {
        #region Conversion
        public static string DBToString(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return string.Empty;
                }
                else
                {
                    return dr[strValue].ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static byte DBToByte(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToByte(dr[strValue]);
                }
            }
            else
            {
                return 0;
            }
        }
        public static DateTime DBToDateTime(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return Convert.ToDateTime(dr[strValue]);
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static Guid DBToGuid(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return Guid.Empty;
                }
                else
                {
                    if (IsGUID(dr[strValue].ToString()) == true)
                    {
                        return new Guid(dr[strValue].ToString());
                    }
                    else
                    {
                        return Guid.Empty;
                    }
                }
            }
            else
            {
                return Guid.Empty;
            }

        }
        public static bool DBToBoolean(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return false;
                }
                else
                {
                    if (Convert.ToByte(dr[strValue]) == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public static Int16 DBToInt16(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt16(dr[strValue]);
                }
            }
            else
            {
                return 0;
            }
        }
        public static Int32 DBToInt32(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(dr[strValue]);
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal DBToDecimal(DataRow dr, string strValue)
        {
            if (dr.Table.Columns.Contains(strValue) == true)
            {
                if (dr[strValue] == null || dr[strValue] is System.DBNull)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(dr[strValue]);
                }
            }
            else
            {
                return 0;
            }
        }

        /// Converts a DataRow object to an object type
        /// </summary>
        /// <param name="dataRow">The datarow object to convert</param>
        /// <param name="objectType">The object type to convert</param>
        public static void ConvertDataRowtoObject(DataRow dataRow, Object objectType)
        {
            //--- o type Enecessário para obter as propriedades do objecto
            Type t = objectType.GetType();

            //--- obtem as propriedades o objecto
            PropertyInfo[] propertiesList = t.GetProperties();

            foreach (PropertyInfo properties in propertiesList)
            {
                try
                {
                    //--- coloca o valor da datarow na propriedade correcta do objecto
                    t.InvokeMember(properties.Name, BindingFlags.SetProperty, null,
                                    objectType,
                                    new object[] { dataRow[properties.Name] });
                }
                catch (Exception ex)
                {
                    //--- Se deu erro Eporque a propriedade não existe na datarow ou porque o valor Enulo
                    if (ex.ToString() != null) { }
                }
            }
        }
        #endregion

        #region Validation
        public static bool IsGUID(string expression)
        {
            if (expression != null)
            {
                System.Text.RegularExpressions.Regex guidRegEx = new System.Text.RegularExpressions.Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
                return guidRegEx.IsMatch(expression);
            }
            return false;
        }
        public static bool IsNumeric(string strNumber)
        {
            decimal iResult;
            return decimal.TryParse(strNumber, out iResult);
        }
        public static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
        public static long GetDateNumber(DateTime dt)
        {
            if (dt.Equals(DateTime.MinValue) == false)
            {
                return Convert.ToInt32(string.Format("{0:yyyyMMdd}", dt));
            }
            return 0;
        }
        public static bool ValidateEnglishCharacter(string text)
        {
            Regex strRegEx = new Regex(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$");
            return strRegEx.IsMatch(text);
        }
        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
        public static bool ValidateDOB(DateTime departureDate, DateTime birthDate, string passenger_type_rcd)
        {
            bool valid = false;
            if (ConfigurationManager.AppSettings["EnableValidateDOB"] == null)
                valid = true;

            if (ConfigurationHelper.ToBoolean("EnableValidateDOB"))
            {
                int maxAge = ConfigurationHelper.ToInt32("MaxYearAge") * 12;
                int minAdult = ConfigurationHelper.ToInt32("MinYearAdult") * 12;
                int minChild = ConfigurationHelper.ToInt32("MinYearChild") * 12;
                int minINF = ConfigurationHelper.ToInt32("MinDaysINF");
                int DiffMonth = MonthDifference(departureDate, birthDate);

                if (passenger_type_rcd.ToUpper() == "ADULT")
                {
                    if (maxAge > DiffMonth && DiffMonth > minAdult)
                        valid = true;
                }
                else if (passenger_type_rcd.ToUpper() == "CHD")
                {
                    if (minAdult > DiffMonth && DiffMonth > minChild)
                        valid = true;
                }
                else if (passenger_type_rcd.ToUpper() == "INF")
                {
                    if (minChild > DiffMonth && DiffMonth >= 0)
                        valid = true;

                    if (valid && (departureDate - birthDate).Days < minINF)
                        valid = false;
                }
            }
            return valid;
        }
        public static string ValidateData(string data, Type type)
        {
            if (data != null)
            {
                if (data.Length == 0)
                {
                    if (type.Equals(typeof(int)) ||
                        type.Equals(typeof(Int16)) ||
                        type.Equals(typeof(Int32)) ||
                        type.Equals(typeof(Int64)) ||
                        type.Equals(typeof(uint)) ||
                        type.Equals(typeof(UInt16)) ||
                        type.Equals(typeof(UInt32)) ||
                        type.Equals(typeof(UInt64)) ||
                        type.Equals(typeof(byte)) ||
                        type.Equals(typeof(Byte)) ||
                        type.Equals(typeof(sbyte)) ||
                        type.Equals(typeof(SByte)) ||
                        type.Equals(typeof(Single)) ||
                        type.Equals(typeof(double)) ||
                        type.Equals(typeof(Double)) ||
                        type.Equals(typeof(decimal)) ||
                        type.Equals(typeof(Decimal)) ||
                        type.Equals(typeof(long)) ||
                        type.Equals(typeof(ulong)) ||
                        type.Equals(typeof(short)) ||
                        type.Equals(typeof(ushort)) ||
                        type.Equals(typeof(float))) data = "0";

                    else if (type.Equals(typeof(bool)) ||
                        type.Equals(typeof(Boolean))) data = "false";

                    else if (type.Equals(typeof(Guid))) data = Guid.Empty.ToString();

                    else if (type.Equals(typeof(DateTime))) data = DateTime.MinValue.ToString("yyyy-MM-dd");
                }
            }
            return data;
        }
        #endregion

        #region Date Helper
        public static DateTime ParseDate(string strDate)
        {
            DateTime dt = DateTime.MinValue;
            ConvertDateTime(strDate, out dt);
            return dt;
        }
        public static DateTime ParseDateString(string strDate)
        {
            DateTime dtDate = DateTime.MinValue;
            if (DateTime.TryParse(strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2), out dtDate) == true)
            {
                return dtDate;
            }

            return DateTime.MinValue;
        }
        public static bool DateValid(string strDate)
        {
            DateTime dt;
            return ConvertDateTime(strDate, out dt);
        }
        private static bool ConvertDateTime(string strDate, out DateTime dtDateTime)
        {
            dtDateTime = DateTime.MinValue;
            if (strDate.Length == 8)
            {
                return DateTime.TryParse(strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2), out dtDateTime);
            }
            else if (strDate.Length > 8)
            {
                return DateTime.TryParse(strDate, out dtDateTime);
            }
            else
            {
                return false;
            }
        }
        public static DateTime ConvertDate(string strDate, string format)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParseExact(strDate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
            return result;
        }
        public static TimeSpan DateDifferent(DateTime dtStart, DateTime dtEnd)
        {
            return dtEnd.Subtract(dtStart);
        }

        #endregion

        #region HTML Helper

        public static string GetClientIpAddress()
        {

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            ipAddress = context.Request.ServerVariables["HTTP_X_REAL_IP"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return "127.0.0.1";
        }
        #endregion
    }

    public static class XmlHelper
    {
        public static string XpathValueNullToEmpty(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "")
            {
                return n.SelectSingleNode(strName).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static decimal XpathValueNullToZero(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "" && DataHelper.IsNumeric(n.SelectSingleNode(strName).InnerXml) == true)
            {
                return Convert.ToDecimal(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return 0;
            }
        }
        public static Guid XpathValueNullToGUID(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "" && DataHelper.IsGUID(n.SelectSingleNode(strName).InnerXml) == true)
            {
                return new Guid(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return Guid.Empty;
            }
        }
        public static DateTime XpathValueNullToDateTime(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "")
            {
                return DataHelper.ParseDate(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime XpathValueDateStringToDateTime(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "")
            {
                string strDate = n.SelectSingleNode(strName).InnerXml;
                strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);
                return DataHelper.ParseDate(strDate);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static Int16 XpathValueNullToInt16(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "" && DataHelper.IsNumeric(n.SelectSingleNode(strName).InnerXml) == true)
            {
                return Convert.ToInt16(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return 0;
            }
        }
        public static int XpathValueNullToInt(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "" && DataHelper.IsNumeric(n.SelectSingleNode(strName).InnerXml) == true)
            {
                return Convert.ToInt32(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return 0;
            }
        }
        public static Byte XpathValueNullToByte(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "")
            {
                return Convert.ToByte(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return 0;
            }
        }
        public static bool XpathValueNullToBoolean(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "")
            {
                return Convert.ToBoolean(XpathValueNullToInt(n, strName));
            }
            else
            {
                return false;
            }
        }
        public static decimal XpathValueNullToDecimal(XPathNavigator n, string strName)
        {
            if (n.SelectSingleNode(strName) != null && n.SelectSingleNode(strName).InnerXml != "" && DataHelper.IsNumeric(n.SelectSingleNode(strName).InnerXml) == true)
            {
                return Convert.ToDecimal(n.SelectSingleNode(strName).InnerXml);
            }
            else
            {
                return 0;
            }
        }
        public static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (DataHelper.IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        public static string Serialize(object o, bool withXmlHeader)
        {
            if (o != null)
            {
                XmlSerializer s = new XmlSerializer(o.GetType());
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlWriterSettings writerSettings = new XmlWriterSettings();
                if (withXmlHeader == true)
                { writerSettings.OmitXmlDeclaration = false; }
                else
                { writerSettings.OmitXmlDeclaration = true; }

                StringWriter writer = new StringWriter();
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, writerSettings))
                {
                    serializer.Serialize(xmlWriter, o, ns);
                }

                return writer.ToString();
            }

            return string.Empty;

        }
        public static object Deserialize(string xml, Type t)
        {
            try
            {
                if (!string.IsNullOrEmpty(xml))
                {
                    xml = xml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
                    using (StringReader reader = new StringReader(xml))
                    {
                        XmlSerializer serializer = new XmlSerializer(t);
                        return serializer.Deserialize(reader);
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static object BindObject(object output, XElement input)
        {
            try
            {
                PropertyInfo[] props = output.GetType().GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    PropertyInfo prop = output.GetType().GetProperty(props[i].Name);
                    string value = input.Element(props[i].Name) != null ? input.Element(props[i].Name).Value : null;
                    if (value != null)
                    {
                        object t = (object)TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(DataHelper.ValidateData(value,prop.PropertyType));
                        prop.SetValue(output, Convert.ChangeType(t, prop.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return output;
        }

        public static XElement Sort(XElement element)
        {
            return new XElement(element.Name,
                    from child in element.Elements()
                    orderby child.Name.ToString()
                    select Sort(child));
        }

        public static XDocument Sort(XDocument file)
        {
            return new XDocument(Sort(file.Root));
        }
    }
    public static class WebServiceHelper
    {
        public static agentservice.TikAeroXMLwebservice AgentService(System.Net.CookieContainer cc, string strPassport, string currencyRcd)
        {
            agentservice.TikAeroXMLwebservice objService = new agentservice.TikAeroXMLwebservice();

            objService.AgentAuthHeaderValue.AgencyCode = ConfigurationManager.AppSettings["DefaultAgencyCode"];
            objService.AgentAuthHeaderValue.AgencyPassport = strPassport;
            objService.AgentAuthHeaderValue.AgencyCurrencyRcd = currencyRcd;

            objService.Credentials = System.Net.CredentialCache.DefaultCredentials;
            objService.CookieContainer = cc;

            return objService;
        }
    }
    public static class SecurityHelper
    {
        public static string EncryptString(string Message, string Passphrase)
        {
            string strResult = string.Empty;
            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            using (System.Security.Cryptography.MD5CryptoServiceProvider HashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                using (System.Security.Cryptography.TripleDESCryptoServiceProvider TDESAlgorithm = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
                {
                    // Step 3. Setup the encoder
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = System.Security.Cryptography.CipherMode.ECB;
                    TDESAlgorithm.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                    // Step 4. Convert the input string to a byte[]
                    byte[] DataToEncrypt = UTF8.GetBytes(Message);

                    // Step 5. Attempt to encrypt the string
                    try
                    {
                        using (System.Security.Cryptography.ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor())
                        {
                            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                        }
                    }
                    catch
                    { strResult = string.Empty; }
                }

            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }
        public static string DecryptString(string Message, string Passphrase)
        {
            string strResult = string.Empty;
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            using (System.Security.Cryptography.MD5CryptoServiceProvider HashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                using (System.Security.Cryptography.TripleDESCryptoServiceProvider TDESAlgorithm = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
                {
                    // Step 3. Setup the decoder
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = System.Security.Cryptography.CipherMode.ECB;
                    TDESAlgorithm.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                    try
                    {
                        // Step 4. Convert the input string to a byte[]
                        byte[] DataToDecrypt = Convert.FromBase64String(Message);

                        // Step 5. Attempt to decrypt the string
                        using (System.Security.Cryptography.ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor())
                        {
                            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                        }
                        strResult = UTF8.GetString(Results);
                    }
                    catch
                    { strResult = string.Empty; }
                }
            }

            // Step 6. Return the decrypted string in UTF8 format
            return strResult;
        }
        public static string EncryptStringSHA1(string strToEncryp)
        {
            string hashString = string.Empty;
            using (System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {

                try
                {
                    System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();

                    byte[] bytes = ue.GetBytes(strToEncryp);
                    byte[] hashBytes = sha1.ComputeHash(bytes);
                    // Convert the encrypted bytes back to a string (base 16)             

                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
                    }
                }
                catch
                { }
            }

            //Return encrypted value.
            if (string.IsNullOrEmpty(hashString) == false)
            {
                return hashString.PadLeft(32, '0');
            }
            else
            {
                return string.Empty;
            }
        }
        public static string EncryptStringSHA512(string strToEncryp)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] hash = shaM.ComputeHash(Encoding.UTF8.GetBytes(strToEncryp));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        public static string MD5Encrypt(string parameters)
        {
            //MD5 Encrypt
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding enc = new UTF8Encoding();
            byte[] input = null;
            byte[] output = null;
            System.Text.StringBuilder hash = new StringBuilder();

            input = enc.GetBytes(parameters);
            output = md5.ComputeHash(input);

            foreach (byte byt in output)
            {
                hash.Append(byt.ToString("x2"));
            }

            return hash.ToString();
        }

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;

                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                byte[] gzBuffer = new byte[compressed.Length + 4];
                System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

                return Convert.ToBase64String(gzBuffer);
            }
        }
        public static string DecompressString(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
        public static string GenerateSessionlessToken()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["UseSessionless"] != null && Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["UseSessionless"]) == 1)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"] != null & System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"].Length > 0 & System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"].Length > 0)
                    {
                        string strValue = System.Configuration.ConfigurationManager.AppSettings["AffixEncrypt"] + "|" + string.Format("{0:u}", DateTime.Now);
                        return SecurityHelper.EncryptString(strValue, System.Configuration.ConfigurationManager.AppSettings["PrefixEncrypt"]);
                    }
                }
            }

            return string.Empty;
        }

        public static string GenerateSessionlessToken(string usesessionless, string prefix, string affix)
        {
            if (usesessionless != null && Convert.ToInt16(usesessionless) == 1)
            {
                if (affix != null & prefix != null)
                {
                    if (affix.Length > 0 & prefix.Length > 0)
                    {
                        string strValue = affix + "|" + string.Format("{0:u}", DateTime.Now);
                        return SecurityHelper.EncryptString(strValue, prefix);
                    }
                }
            }

            return string.Empty;
        }
    }
    public static class ConfigurationHelper
    {
        public static bool ToBoolean(System.Collections.Specialized.NameValueCollection setting, string name)
        {
            if (setting[name] != null)
            {
                return Convert.ToBoolean(setting[name]);
            }
            return false;
        }
        public static string ToString(System.Collections.Specialized.NameValueCollection setting, string name)
        {
            if (setting[name] != null)
            {
                return setting[name].ToString();
            }
            return string.Empty;
        }
        public static int ToInt32(System.Collections.Specialized.NameValueCollection setting, string name)
        {
            if (setting[name] != null)
            {
                return Convert.ToInt32(setting[name]);
            }
            return 0;
        }
        public static byte ToByte(System.Collections.Specialized.NameValueCollection setting, string name)
        {
            if (setting[name] != null)
            {
                return Convert.ToByte(setting[name]);
            }
            return 0;
        }


        public static bool ToBoolean(string name)
        {

            if (ConfigurationManager.AppSettings[name] != null)
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings[name]);
            }
            return false;
        }
        public static string ToString(string name)
        {
            if (ConfigurationManager.AppSettings[name] != null)
            {
                return ConfigurationManager.AppSettings[name].ToString();
            }
            return string.Empty;
        }
        public static int ToInt32(string name)
        {

            if (ConfigurationManager.AppSettings[name] != null)
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[name]);
            }
            return 0;
        }
        public static Byte ToByte(string name)
        {
            if (ConfigurationManager.AppSettings[name] != null)
            {
                return Convert.ToByte(ConfigurationManager.AppSettings[name]);
            }
            return 0;
        }
    }
}
