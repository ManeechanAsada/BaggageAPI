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
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Fees : LibraryBase
    {
        public Fee this[int index]
        {
            get { return (Fee)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Fee value)
        {
            return this.List.Add(value);
        }

        public void Remove(Fee value)
        {
            this.List.Remove(value);
        }

        #region Method
        public List<Fee> GetFeesByCode(string fee_rcd)
        {
            return this.OfType<Fee>().Where(f => f.fee_rcd == fee_rcd).ToList<Fee>();
        }

        public void RemoveSpecialService(string[] array)
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Fee f = this[i];
                if (array.Contains<string>(f.fee_rcd))
                {
                    this.Remove(f);
                }
            }
        }

        public void RemoveSeat()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Fee f = this[i];
                if (!string.IsNullOrEmpty(f.fee_category_rcd) && f.fee_category_rcd.ToUpper().Equals("SEAT"))
                {
                    this.Remove(f);
                }
            }
        }

        public void RemoveFees(Fees fees)
        {
            if (fees == null) return;
            for (int i = this.Count - 1; i > -1; i--)
            {
                Fee f = this[i];
                for (int j = 0; j < fees.Count; j++)
                {
                    Fee b = fees[j];
                    if (f.fee_rcd.Equals(b.fee_rcd))
                    {
                        this.Remove(f);
                    }
                }
            }
        }

        public void RemoveFeesByCode(string fee_rcd)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].fee_rcd == fee_rcd)
                {
                    this.RemoveAt(i);
                    if (this.Count > 0) i--;
                }
            }
        }

        public void RemoveEmptyFees()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].booking_fee_id == Guid.Empty)
                {
                    this.RemoveAt(i);
                    if (this.Count > 0) i--;
                }
            }
        }

        public string SegmentFee(Agent agent,
                                 Routes routes,
                                 BookingHeader header,
                                 Mappings mappings,
                                 string[] serviceGroup,
                                 Services SpecialServiceRef,
                                 int iPassengerNumber,
                                 int iInfantNumber,
                                 string strLanguage,
                                 bool bCalculateSpecialService,
                                 bool bNoVat)
        {
            Library objLi = new Library();

            string strResult = string.Empty;
            string strAgencyCode = string.Empty;

            // check agency is b2s or not
            strAgencyCode = (agent.agency_code.Equals("B2S")) ? header.booking_source_rcd : header.agency_code;

            if (mappings.Count > 0 && serviceGroup.Length > 0)
            {
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;

                //Filter special service base on special service on route flag.
                if (bCalculateSpecialService == true)
                {
                    if (agent.b2b_allow_service_flag == 1)
                    {
                        //Filter Selected mapping.
                        Mappings mps = new Mappings();
                        for (int i = 0; i < mappings.Count; i++)
                        {
                            for (int j = 0; j < routes.Count; j++)
                            {
                                if (mappings[i].origin_rcd.Equals(routes[j].origin_rcd) && mappings[i].destination_rcd.Equals(routes[j].destination_rcd))
                                {
                                    if (routes[j].special_service_fee_flag == true)
                                    {
                                        mps.Add(mappings[i]);

                                    }
                                }
                            }
                        }

                        if (mps != null && mps.Count > 0)
                        {
                            strResult = objClient.SegmentFee(strAgencyCode,
                                                            header.currency_rcd,
                                                            serviceGroup,
                                                            mps,
                                                            iPassengerNumber,
                                                            iInfantNumber,
                                                            strLanguage,
                                                            bCalculateSpecialService,
                                                            bNoVat);
                        }

                    }
                }
                else
                {
                    strResult = objClient.SegmentFee(strAgencyCode,
                                                    header.currency_rcd,
                                                    serviceGroup,
                                                    mappings,
                                                    iPassengerNumber,
                                                    iInfantNumber,
                                                    strLanguage,
                                                    bCalculateSpecialService,
                                                    bNoVat);
                }

                if ((SpecialServiceRef != null && SpecialServiceRef.Count > 0) && strResult.Length > 0)
                {
                    // Insert missing field into xml.
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNodeList nl;

                    xmlDoc.LoadXml(strResult);
                    nl = xmlDoc.SelectNodes("ServiceFees/*");

                    if (nl.Count > 0)
                    {
                        XmlElement eFeeRcd;
                        XmlElement eDisplay;
                        XmlElement eOnRequest;
                        XmlElement eCutOffTime;
                        XmlElement eSpecialServiceGroupRcd;

                        //Find Special service displayname

                        foreach (XmlNode n in nl)
                        {
                            for (int i = 0; i < SpecialServiceRef.Count; i++)
                            {
                                if (SpecialServiceRef[i].special_service_rcd.Equals(n.Name))
                                {
                                    //Add Fee_Rcd
                                    eFeeRcd = xmlDoc.CreateElement("fee_rcd");
                                    eFeeRcd.InnerText = n.Name;
                                    n.AppendChild(eFeeRcd);

                                    eDisplay = xmlDoc.CreateElement("display_name");
                                    eDisplay.InnerText = SpecialServiceRef[i].display_name;
                                    n.AppendChild(eDisplay);

                                    eOnRequest = xmlDoc.CreateElement("service_on_request_flag");
                                    eOnRequest.InnerText = SpecialServiceRef[i].service_on_request_flag.ToString();
                                    n.AppendChild(eOnRequest);

                                    eCutOffTime = xmlDoc.CreateElement("cut_off_time");
                                    eCutOffTime.InnerText = SpecialServiceRef[i].cut_off_time.ToString();
                                    n.AppendChild(eCutOffTime);

                                    eSpecialServiceGroupRcd = xmlDoc.CreateElement("special_service_group_rcd");
                                    eSpecialServiceGroupRcd.InnerText = SpecialServiceRef[i].special_service_group_rcd.ToString();
                                    n.AppendChild(eSpecialServiceGroupRcd);

                                    eFeeRcd = null;

                                    objClient.objService = null;
                                    strResult = xmlDoc.SelectSingleNode("ServiceFees").OuterXml;
                                    break;
                                }
                            }

                        }
                    }
                    else
                    { strResult = string.Empty; }
                }
            }
            return strResult;
        }

        public void GetBaggageFeeOptions(Mappings mappings, Guid gSegmentId, Guid gPassengerId, string strAgencyCode, string strLanguage, long lMaxunits, Fees fees, bool bApplySegmentFee, bool bNoVat)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            string strFees = objClient.GetBaggageFeeOptions(mappings, gSegmentId, gPassengerId, strAgencyCode, strLanguage, lMaxunits, fees, bApplySegmentFee, bNoVat);
            Library objLi = new Library();
            if (string.IsNullOrEmpty(strFees) == false)
            {
                using (StringReader srd = new StringReader(strFees))
                {
                    XPathDocument xmlDoc = new XPathDocument(srd);
                    XPathNavigator nv = xmlDoc.CreateNavigator();
                    Fee f;
                    foreach (XPathNavigator n in nv.Select("BaggageFees/Fee"))
                    {
                        f = new Fee();

                        f.passenger_id = XmlHelper.XpathValueNullToGUID(n, "passenger_id");
                        f.booking_segment_id = XmlHelper.XpathValueNullToGUID(n, "booking_segment_id");
                        f.fee_id = XmlHelper.XpathValueNullToGUID(n, "fee_id");
                        f.fee_rcd = XmlHelper.XpathValueNullToEmpty(n, "fee_rcd");
                        f.fee_category_rcd = XmlHelper.XpathValueNullToEmpty(n, "fee_category_rcd");
                        f.currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "currency_rcd");
                        f.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                        f.number_of_units = XmlHelper.XpathValueNullToInt16(n, "number_of_units");
                        f.fee_amount = XmlHelper.XpathValueNullToZero(n, "fee_amount");
                        f.fee_amount_incl = XmlHelper.XpathValueNullToZero(n, "fee_amount_incl");
                        f.total_amount = XmlHelper.XpathValueNullToZero(n, "total_amount");
                        f.total_amount_incl = XmlHelper.XpathValueNullToZero(n, "total_amount_incl");
                        f.vat_percentage = XmlHelper.XpathValueNullToZero(n, "vat_percentage");
                        f.baggage_fee_option_id = XmlHelper.XpathValueNullToGUID(n, "baggage_fee_option_id");
                        Add(f);
                    }


                }
            }
        }

        public void GetFeeDefinition(string strFeeRcd,
                                    string strCurrencyCode,
                                    string strAgencyCode,
                                    string strClass,
                                    string strFareBasis,
                                    string strOrigin,
                                    string strDestination,
                                    string strFlightNumber,
                                    DateTime dtDate,
                                    string strLanguage,
                                    bool bNoVat)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            Fees objFees = objClient.GetFee(strFeeRcd, strCurrencyCode, strAgencyCode, strClass, strFareBasis, strOrigin, strDestination, strFlightNumber, dtDate, strLanguage, bNoVat);
            if (objFees != null && objFees.Count > 0)

            {
                for (int i = 0; i < objFees.Count; i++)
                {
                    Add(objFees[i]);

                }
            }
        }

        public void ClearInsuranceFee(Fee InsuranceFee)
        {
            if (this != null && InsuranceFee != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].fee_rcd.Equals(InsuranceFee.fee_rcd) &&
                        this[i].passenger_id.Equals(InsuranceFee.passenger_id) &&
                        this[i].booking_segment_id.Equals(InsuranceFee.booking_segment_id))
                    {
                        this.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public string GetBaggageFeeXml()
        {
            using (StringWriter stw = new StringWriter())
            {
                using (XmlWriter xtw = XmlWriter.Create(stw))
                {
                    xtw.WriteStartElement("BaggageFees");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Fee");
                            {
                                xtw.WriteStartElement("baggage_fee_option_id");
                                {
                                    xtw.WriteValue(this[i].baggage_fee_option_id.ToString());
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("passenger_id");
                                {
                                    xtw.WriteValue(this[i].passenger_id.ToString());
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("booking_segment_id");
                                {
                                    xtw.WriteValue(this[i].booking_segment_id.ToString());
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_id");
                                {
                                    xtw.WriteValue(this[i].fee_id.ToString());
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_rcd");
                                {
                                    xtw.WriteValue(this[i].fee_rcd);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_category_rcd");
                                {
                                    xtw.WriteValue(this[i].fee_category_rcd);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("currency_rcd");
                                {
                                    xtw.WriteValue(this[i].currency_rcd);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("display_name");
                                {
                                    xtw.WriteValue(this[i].display_name);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("number_of_units");
                                {
                                    xtw.WriteValue(this[i].number_of_units);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_amount");
                                {
                                    xtw.WriteValue(this[i].fee_amount);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_amount_incl");
                                {
                                    xtw.WriteValue(this[i].fee_amount_incl);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("total_amount");
                                {
                                    xtw.WriteValue(this[i].total_amount);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("total_amount_incl");
                                {
                                    xtw.WriteValue(this[i].total_amount_incl);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("vat_percentage");
                                {
                                    xtw.WriteValue(this[i].vat_percentage);
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
        public void GetFormOfPaymentSubTypeFee(string formOfPayment, string formOfPaymentSubType, string currencyRcd, string agencyCode, DateTime feeDate)
        {
            ServiceClient objClient = new ServiceClient();
            try
            {
                objClient.objService = objService;
                DataSet ds = objClient.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubType, currencyRcd, agencyCode, feeDate);
                Library objLi = new Library();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Fee f;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        f = new Fee();

                        f.fee_id = new Guid(dr["fee_id"].ToString());
                        f.booking_fee_id = Guid.NewGuid();
                        f.fee_rcd = dr["fee_rcd"].ToString();
                        f.currency_rcd = dr["currency_rcd"].ToString();
                        f.display_name = dr["display_name"].ToString();
                        f.fee_percentage = Convert.ToDecimal(dr["fee_percentage"]);
                        f.fee_amount = Convert.ToDecimal(dr["fee_amount"]);
                        f.fee_amount_incl = Convert.ToDecimal(dr["fee_amount_incl"]);
                        f.vat_percentage = Convert.ToDecimal(dr["vat_percentage"]);
                        f.minimum_fee_amount_flag = Convert.ToByte(dr["minimum_fee_amount_flag"]);
                        f.od_flag = Convert.ToByte(dr["od_flag"]);
                        f.fee_level = dr["fee_level"].ToString();

                        Add(f);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void CalculateSpecialServiceFee(string agencyCode,
                                                string currency,
                                                BookingHeader header,
                                                Services service,
                                                Remarks remark,
                                                Mappings mapping,
                                                string languageCode,
                                                bool bNoVat)
        {
            ServiceClient objCient = new ServiceClient();
            string serviceFeesXml = string.Empty;
            objCient.objService = objService;
            serviceFeesXml = objCient.CalculateSpecialServiceFees(agencyCode,
                                                                  currency,
                                                                  header.booking_id.ToString(),
                                                                  XmlHelper.Serialize(header, false),
                                                                  XmlHelper.Serialize(service, false),
                                                                  XmlHelper.Serialize(this, false),
                                                                  XmlHelper.Serialize(remark, false),
                                                                  XmlHelper.Serialize(mapping, false),
                                                                  languageCode,
                                                                  bNoVat);
            if (string.IsNullOrEmpty(serviceFeesXml) == false)
            {
                this.Clear();
                AddFees(serviceFeesXml, false);
            }
        }
        public bool CalculateSeatFee(string agencyCode,
                                    string currency,
                                    string bookingId,
                                    BookingHeader header,
                                    Itinerary itinerary,
                                    Passengers passengers,
                                    Services service,
                                    Remarks remark,
                                    Mappings mapings,
                                    string languageCode,
                                    bool bNoVat)
        {
            ServiceClient objCient = new ServiceClient();
            string serviceFeesXml = string.Empty;
            objCient.objService = objService;
            serviceFeesXml = objCient.CalculateNewFees(bookingId,
                                                        agencyCode,
                                                        XmlHelper.Serialize(header, false),
                                                        XmlHelper.Serialize(itinerary, false),
                                                        XmlHelper.Serialize(passengers, false),
                                                        XmlHelper.Serialize(this, false),
                                                        currency,
                                                        XmlHelper.Serialize(remark, false),
                                                        string.Empty,
                                                        XmlHelper.Serialize(mapings, false),
                                                        XmlHelper.Serialize(service, false),
                                                        string.Empty,
                                                        false,
                                                        false,
                                                        false,
                                                        true,
                                                        languageCode,
                                                        bNoVat);

            if (string.IsNullOrEmpty(serviceFeesXml) == false)
            {
                this.Clear();
                AddFees(serviceFeesXml, false);
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetXml()
        {
            using (StringWriter stw = new StringWriter())
            {
                using (XmlWriter xtw = XmlWriter.Create(stw))
                {
                    xtw.WriteStartElement("Fees");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Fee");
                            {
                                xtw.WriteStartElement("fee_id");
                                {
                                    xtw.WriteValue(this[i].fee_id.ToString());
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_rcd");
                                {
                                    xtw.WriteValue(this[i].fee_rcd);
                                }
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("currency_rcd");
                                {
                                    xtw.WriteValue(this[i].currency_rcd);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("display_name");
                                {
                                    xtw.WriteValue(this[i].display_name);
                                }
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("fee_amount");
                                {
                                    xtw.WriteValue(this[i].fee_amount);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_amount_incl");
                                {
                                    xtw.WriteValue(this[i].fee_amount_incl);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("fee_percentage");
                                {
                                    xtw.WriteValue(this[i].fee_percentage);
                                }
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("vat_percentage");
                                {
                                    xtw.WriteValue(this[i].vat_percentage);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("minimum_fee_amount_flag");
                                {
                                    xtw.WriteValue(this[i].minimum_fee_amount_flag);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("od_flag");
                                {
                                    xtw.WriteValue(this[i].od_flag);
                                }
                                xtw.WriteEndElement();
                                if (string.IsNullOrEmpty(this[i].fee_level) == false)
                                {
                                    xtw.WriteStartElement("fee_level");
                                    {
                                        xtw.WriteValue(this[i].fee_level);
                                    }
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].fee_category_rcd) == false)
                                {
                                    xtw.WriteStartElement("fee_category_rcd");
                                    {
                                        xtw.WriteValue(this[i].fee_category_rcd);
                                    }
                                    xtw.WriteEndElement();
                                }

                                xtw.WriteStartElement("charge_amount");
                                {
                                    xtw.WriteValue(this[i].charge_amount);
                                }
                                xtw.WriteEndElement();
                                xtw.WriteStartElement("charge_amount_incl");
                                {
                                    xtw.WriteValue(this[i].charge_amount_incl);
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
        public bool GetCreditCardFee(BookingHeader bookingHeader,
                                    Passengers passengers,
                                    Itinerary itinerary,
                                    Quotes quotes,
                                    Fees fees,
                                    Payments payments,
                                    string formOfPaymentRcd,
                                    string cardTypeRcd,
                                    string currencyCode,
                                    Guid userId,
                                    string formOfPaymentFeeXml,
                                    decimal OutStandingBalance)
        {
            return GetPaymentFee(formOfPaymentRcd,
                                 cardTypeRcd,
                                 bookingHeader,
                                 passengers,
                                 itinerary,
                                 quotes,
                                 fees,
                                 payments,
                                 currencyCode,
                                 userId,
                                 formOfPaymentFeeXml,
                                 OutStandingBalance);
        }

        public bool GetPaymentFee(string formOfPayment,
                                  string formOfPaymentSubType,
                                  BookingHeader bookingHeader,
                                  Passengers passengers,
                                  Itinerary itinerary,
                                  Quotes quotes,
                                  Fees fees,
                                  Payments payments,
                                  string currencyCode,
                                  Guid userId,
                                  string formOfPaymentFeeXml,
                                  decimal OutStandingBalance)
        {
            string strXML = string.Empty;

            Library objLi = new Library();
            Payments objPayments = new Payments();
            Fee fee;

            string strFeeLevel = string.Empty;

            short sOdFlag = 0;
            short sMinimumFeeFlag = 0;

            decimal dFeePercentage = 0;
            decimal dFeeAmount = 0;
            decimal dFeeAmountIncl = 0;

            if (formOfPayment == string.Empty)
                formOfPayment = "CC";
            try
            {
                if (string.IsNullOrEmpty(formOfPaymentFeeXml))
                {
                    ServiceClient objClient = new ServiceClient();
                    objClient.objService = objService;

                    DataSet ds = objClient.GetFormOfPaymentSubtypeFees(formOfPayment, formOfPaymentSubType, currencyCode, bookingHeader.agency_code, DateTime.MinValue);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        ds.DataSetName = "FormOfPaymentSubtypeFees";
                        strXML = ds.GetXml();
                    }

                }
                else
                {
                    strXML = formOfPaymentFeeXml;
                }

                if (string.IsNullOrEmpty(strXML) == false)
                {
                    XPathDocument xmlDoc = new XPathDocument(new StringReader(strXML));
                    XPathNavigator nv = xmlDoc.CreateNavigator();
                    if (bookingHeader != null &&
                        itinerary != null &&
                        quotes != null &&
                        fees != null &&
                        passengers != null &&
                        passengers.Count > 0 &&
                        itinerary.Count > 0)
                    {
                        if (OutStandingBalance == 0)
                        {
                            OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                        }
                        foreach (XPathNavigator n in nv.Select("FormOfPaymentSubtypeFees/Fee[fee_rcd = '" + formOfPaymentSubType + "'][currency_rcd = '" + currencyCode + "']"))
                        {

                            objPayments.FindFee(n,
                                               ref strFeeLevel,
                                               ref sOdFlag,
                                               ref sMinimumFeeFlag,
                                               ref dFeePercentage,
                                               ref dFeeAmount,
                                               ref dFeeAmountIncl);

                            objPayments.calCalculateCreditCardFee(ref dFeeAmount,
                                                                  ref dFeeAmountIncl,
                                                                  itinerary,
                                                                  dFeePercentage,
                                                                  strFeeLevel,
                                                                  sOdFlag,
                                                                  sMinimumFeeFlag,
                                                                  OutStandingBalance,
                                                                  (short)bookingHeader.number_of_adults,
                                                                  (short)bookingHeader.number_of_children,
                                                                  (short)bookingHeader.number_of_infants);

                            fee = new Fee();

                            fee.acct_fee_amount = dFeeAmount;
                            fee.acct_fee_amount_incl = dFeeAmountIncl;
                            fee.booking_fee_id = Guid.NewGuid();
                            fee.booking_id = bookingHeader.booking_id;
                            fee.currency_rcd = bookingHeader.currency_rcd;
                            fee.display_name = objLi.getXPathNodevalue(n, "display_name", Library.xmlReturnType.value);
                            fee.fee_amount = dFeeAmount;
                            fee.fee_amount_incl = dFeeAmountIncl;
                            fee.fee_id = new Guid(objLi.getXPathNodevalue(n, "fee_id", Library.xmlReturnType.value));
                            fee.fee_rcd = objLi.getXPathNodevalue(n, "fee_rcd", Library.xmlReturnType.value);
                            fee.passenger_id = passengers[0].passenger_id;
                            fee.booking_segment_id = itinerary[0].booking_segment_id;
                            fee.vat_percentage = Convert.ToDecimal(objLi.getXPathNodevalue(n, "vat_percentage", Library.xmlReturnType.value));
                            fee.void_date_time = DateTime.MinValue;
                            fee.create_by = userId;
                            fee.create_date_time = DateTime.Now;
                            fee.update_by = userId;
                            fee.update_date_time = DateTime.Now;
                            this.Add(fee);
                            fee = null;

                            break;
                        }

                        xmlDoc = null;
                        nv = null;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddFees(string strXml, bool bBookingUri)
        {
            using (System.IO.StringReader sr = new System.IO.StringReader(strXml))
            {
                XPathDocument xmlDoc = new XPathDocument(sr);
                XPathNavigator nv = xmlDoc.CreateNavigator();

                string selectUri = string.Empty;

                if (bBookingUri == true)
                {
                    selectUri = "Booking/Fee";
                }
                else
                {
                    selectUri = "Fees/Fee";
                }

                Fee fe;
                foreach (XPathNavigator n in nv.Select(selectUri))
                {
                    fe = (Fee)XmlHelper.Deserialize(n.OuterXml, typeof(Fee));
                    Add(fe);
                    fe = null;
                }
            }
        }

        public string GetFormOfPaymentFeeXML(string formOfPaymentRcd,
                                                string formOfPaymentSubType,
                                                BookingHeader bookingHeader,
                                                Itinerary itinerary,
                                                Quotes quotes,
                                                Fees fees,
                                                Payments payments)
        {
            ServiceClient objClient = new ServiceClient();
            Payments objPayments = new Payments();
            Library objLi = new Library();
            StringBuilder strFeeXML = new StringBuilder();
            objClient.objService = objService;
            string strFeeLevel = string.Empty;

            short sOdFlag = 0;
            short sMinimumFeeFlag = 0;

            decimal dFeePercentage = 0;
            decimal dFeeAmount = 0;
            decimal dFeeAmountIncl = 0;
            decimal OutStandingBalance = 0;
            string strXML = string.Empty;
            DataSet ds = objClient.GetFormOfPaymentSubtypeFees(formOfPaymentRcd, formOfPaymentSubType, bookingHeader.currency_rcd, bookingHeader.agency_code, DateTime.MinValue);
            StringBuilder str = new StringBuilder();

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.DataSetName = "FormOfPaymentSubtypeFees";
                strXML = ds.GetXml();
            }

            if (string.IsNullOrEmpty(strXML) == false)
            {
                XPathDocument xmlDoc = new XPathDocument(new StringReader(strXML));
                XPathNavigator nv = xmlDoc.CreateNavigator();
                if (OutStandingBalance == 0)
                {
                    OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                }
                foreach (XPathNavigator n in nv.Select("FormOfPaymentSubtypeFees/Fee[fee_rcd = '" + formOfPaymentSubType + "'][currency_rcd = '" + bookingHeader.currency_rcd + "']"))
                {

                    objPayments.FindFee(n,
                                       ref strFeeLevel,
                                       ref sOdFlag,
                                       ref sMinimumFeeFlag,
                                       ref dFeePercentage,
                                       ref dFeeAmount,
                                       ref dFeeAmountIncl);

                    objPayments.calCalculateCreditCardFee(ref dFeeAmount,
                                                          ref dFeeAmountIncl,
                                                          itinerary,
                                                          dFeePercentage,
                                                          strFeeLevel,
                                                          sOdFlag,
                                                          sMinimumFeeFlag,
                                                          OutStandingBalance,
                                                          (short)bookingHeader.number_of_adults,
                                                          (short)bookingHeader.number_of_children,
                                                          (short)bookingHeader.number_of_infants);
                    str.Append("<fee_rcd>");
                    str.Append(XmlHelper.XpathValueNullToEmpty(n, "fee_rcd"));
                    str.Append("</fee_rcd>");
                    str.Append("<display_name>");
                    str.Append(XmlHelper.XpathValueNullToEmpty(n, "display_name"));
                    str.Append("</display_name>");
                    str.Append("<fee_amount_incl>");
                    str.Append(dFeeAmountIncl.ToString());
                    str.Append("</fee_amount_incl>");
                    str.Append("<fee_amount>");
                    str.Append(dFeeAmount.ToString());
                    str.Append("</fee_amount>");
                    break;
                }
            }

            return str.ToString();
        }

        public void GetFeeDisplay(ref Fees fees,
                                    string strSSRFees)
        {
            if (fees == null || string.IsNullOrEmpty(strSSRFees)) return;

            XElement x = XElement.Parse(strSSRFees);

            for (int i = 0; i < fees.Count; i++)
            {
                Fee f = fees[i];
                XElement r = x.XPathSelectElement("ServiceFees/*[fee_rcd = '" + f.fee_rcd + "']");
                if (r != null) f.display_name = r.Element("display_name").Value;
            }

        }

        public void GetServiceDisplay(ref Services services,
                                    string strSSRFees)
        {
            if (services == null || string.IsNullOrEmpty(strSSRFees)) return;

            XElement x = XElement.Parse(strSSRFees);

            for (int i = 0; i < services.Count; i++)
            {
                Service s = services[i];
                XElement r = x.XPathSelectElement("ServiceFees/*[fee_rcd = '" + s.special_service_rcd + "']");
                if (r != null) s.service_text = r.Element("display_name").Value;
            }

        }

        #endregion

        internal string GetFormOfPaymentFeeXML()
        {
            throw new NotImplementedException();
        }
    }
}
