using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data;

namespace tikSystem.Web.Library
{
    public class Routes : LibraryBase
    {
        //Token of Old webservice
        string _token = string.Empty;

        public Routes()
        { }
        public Routes(string token)
        {
            _token = token;
        }
        public Route this[int Index]
        {
            get { return (Route)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Route Value)
        {
            return this.List.Add(Value);
        }
       
        #region Method
        
        public string GetXml()
        {
            if (this.Count > 0)
            {
                StringBuilder stb = new StringBuilder();
                using (System.IO.StringWriter stw = new System.IO.StringWriter(stb))
                {
                    System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(stw);

                    if (string.IsNullOrEmpty(this[0].currency_rcd) == false)
                    { xtw.WriteStartElement("Origins"); }
                    else
                    { xtw.WriteStartElement("Destinations"); }
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("AirportOrigins");
                            {
                                xtw.WriteStartElement("origin_rcd");
                                xtw.WriteValue(this[i].origin_rcd);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_name");
                                xtw.WriteValue(this[i].display_name);
                                xtw.WriteEndElement();

                                #region Origin Information
                                if (string.IsNullOrEmpty(this[i].currency_rcd) == false)
                                {
                                    xtw.WriteStartElement("currency_rcd");
                                    xtw.WriteValue(this[i].currency_rcd);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("country_rcd");
                                    xtw.WriteValue(this[i].country_rcd);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_tot");
                                    xtw.WriteValue(this[i].routes_tot.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_avl");
                                    xtw.WriteValue(this[i].routes_avl.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_b2c");
                                    xtw.WriteValue(this[i].routes_b2c.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_b2b");
                                    xtw.WriteValue(this[i].routes_b2b.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_b2s");
                                    xtw.WriteValue(this[i].routes_b2s.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_api");
                                    xtw.WriteValue(this[i].routes_api.ToString());
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("routes_b2t");
                                    xtw.WriteValue(this[i].routes_b2t.ToString());
                                    xtw.WriteEndElement();
                                }
                                #endregion
                                #region Destination Information
                                if (string.IsNullOrEmpty(this[i].destination_rcd) == false)
                                {
                                    xtw.WriteStartElement("destination_rcd");
                                    xtw.WriteValue(this[i].destination_rcd);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("segment_change_fee_flag");
                                    xtw.WriteValue(this[i].segment_change_fee_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("transit_flag");
                                    xtw.WriteValue(this[i].transit_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("direct_flag");
                                    xtw.WriteValue(this[i].direct_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("avl_flag");
                                    xtw.WriteValue(this[i].avl_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("b2c_flag");
                                    xtw.WriteValue(this[i].b2c_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("b2b_flag");
                                    xtw.WriteValue(this[i].b2b_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("b2t_flag");
                                    xtw.WriteValue(this[i].b2t_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("day_range");
                                    xtw.WriteValue(this[i].day_range);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("show_redress_number_flag");
                                    xtw.WriteValue(this[i].show_redress_number_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("require_passenger_title_flag");
                                    xtw.WriteValue(this[i].require_passenger_title_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("require_passenger_gender_flag");
                                    xtw.WriteValue(this[i].require_passenger_gender_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("require_date_of_birth_flag");
                                    xtw.WriteValue(this[i].require_date_of_birth_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("require_document_details_flag");
                                    xtw.WriteValue(this[i].require_document_details_flag);
                                    xtw.WriteEndElement();

                                    xtw.WriteStartElement("require_passenger_weight_flag");
                                    xtw.WriteValue(this[i].require_passenger_weight_flag);
                                    xtw.WriteEndElement();
                                }
                                #endregion
                            }
                            xtw.WriteEndElement();
                        }
                    }
                    xtw.WriteEndElement();
                    xtw.Close();
                    xtw.Flush();
                }
                return stb.ToString();
            }
            return string.Empty;
        }
        public short GetDayRange(string strOriginRcd, string strDestinationRcd)
        {
            if (this != null && this.Count > 0 && String.IsNullOrEmpty(strOriginRcd) == false && String.IsNullOrEmpty(strDestinationRcd) == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].origin_rcd.Equals(strOriginRcd) == true & this[i].destination_rcd.Equals(strDestinationRcd) == true)
                    {
                        return this[i].day_range;
                    }
                }
            }
            return 0;
        }

        public Route GetRoute(string strOriginRcd, string strDestinationRcd)
        {
            if (this != null && this.Count > 0 && String.IsNullOrEmpty(strOriginRcd) == false && String.IsNullOrEmpty(strDestinationRcd) == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].origin_rcd.Equals(strOriginRcd) == true & this[i].destination_rcd.Equals(strDestinationRcd) == true)
                    {
                        return this[i];
                    }
                }
            }
            return null;
        }

        public string GetCurrencyCode(string originRcd, string destinationRcd)
        {
            if (this != null && this.Count > 0 && String.IsNullOrEmpty(originRcd) == false && String.IsNullOrEmpty(destinationRcd) == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].origin_rcd.Equals(originRcd) == true & this[i].destination_rcd.Equals(destinationRcd) == true)
                    {
                        return this[i].currency_rcd;
                    }
                }
            }
            return string.Empty;
        }
        public string GetCurrencyCode(string originRcd)
        {
            if (this != null && this.Count > 0 && String.IsNullOrEmpty(originRcd) == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].origin_rcd.Equals(originRcd) == true)
                    {
                        return this[i].currency_rcd;
                    }
                }
            }
            return string.Empty;
        }
        public Boolean AllowedInsuranceOnWeb(string originRcd, string destinationRcd)
        {
            if (this != null && this.Count > 0 && String.IsNullOrEmpty(originRcd) == false && String.IsNullOrEmpty(destinationRcd) == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].origin_rcd.Equals(originRcd) == true & this[i].destination_rcd.Equals(destinationRcd) == true)
                    {
                        return this[i].show_insurance_on_web_flag;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
