using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;

namespace tikSystem.Web.Library
{
    public class Countries : LibraryBase
    {
        //Token of Old webservice
        string _token = string.Empty;
        public Countries()
        { }
        public Countries(string strToken)
        {
            _token = strToken;
        }
        public Country this[int Index]
        {
            get { return (Country)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Country Value)
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

                    xtw.WriteStartElement("Countries");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Countrys");
                            {
                                GetXMLElement(xtw, this[i]);
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
        public string GetDataSetXml()
        {
            if (this.Count > 0)
            {
                StringBuilder stb = new StringBuilder();
                using (System.IO.StringWriter stw = new System.IO.StringWriter(stb))
                {
                    System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(stw);

                    xtw.WriteStartElement("NewDataSet");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Countrys");
                            {
                                GetXMLElement(xtw, this[i]);
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
        #endregion

        #region Helper
        private void GetXMLElement(System.Xml.XmlTextWriter xtw, Country country)
        {
            if (string.IsNullOrEmpty(country.country_rcd) == false)
            {
                xtw.WriteStartElement("country_rcd");
                xtw.WriteValue(country.country_rcd);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.display_name) == false)
            {
                xtw.WriteStartElement("display_name");
                xtw.WriteValue(country.display_name);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.status_code) == false)
            {
                xtw.WriteStartElement("status_code");
                xtw.WriteValue(country.status_code);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.currency_rcd) == false)
            {
                xtw.WriteStartElement("currency_rcd");
                xtw.WriteValue(country.currency_rcd);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.vat_display) == false)
            {
                xtw.WriteStartElement("vat_display");
                xtw.WriteValue(country.vat_display);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.country_code_long) == false)
            {
                xtw.WriteStartElement("country_code_long");
                xtw.WriteValue(country.country_code_long);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.country_number) == false)
            {
                xtw.WriteStartElement("country_number");
                xtw.WriteValue(country.country_number);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(country.phone_prefix) == false)
            {
                xtw.WriteStartElement("phone_prefix");
                xtw.WriteValue(country.phone_prefix);
                xtw.WriteEndElement();
            }

            xtw.WriteStartElement("issue_country_flag");
            xtw.WriteValue(country.issue_country_flag);
            xtw.WriteEndElement();

            xtw.WriteStartElement("residence_country_flag");
            xtw.WriteValue(country.residence_country_flag);
            xtw.WriteEndElement();

            xtw.WriteStartElement("nationality_country_flag");
            xtw.WriteValue(country.nationality_country_flag);
            xtw.WriteEndElement();

            xtw.WriteStartElement("address_country_flag");
            xtw.WriteValue(country.address_country_flag);
            xtw.WriteEndElement();

            xtw.WriteStartElement("agency_country_flag");
            xtw.WriteValue(country.agency_country_flag);
            xtw.WriteEndElement();
        }
        #endregion
    }
}
