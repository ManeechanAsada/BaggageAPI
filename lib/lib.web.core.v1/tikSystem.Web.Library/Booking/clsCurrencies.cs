using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data;

namespace tikSystem.Web.Library
{
    public class Currencies : LibraryBase
    {
        //Token of Old webservice
        string _token = string.Empty;
        public Currencies()
        { }
        public Currencies(string strToken)
        {
            _token = strToken;
        }
        public Currency this[int Index]
        {
            get { return (Currency)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Currency Value)
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

                    xtw.WriteStartElement("Currencies");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Currency");
                            {
                                if (string.IsNullOrEmpty(this[i].currency_rcd) == false)
                                {
                                    xtw.WriteStartElement("currency_rcd");
                                    xtw.WriteValue(this[i].currency_rcd);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].display_name) == false)
                                {
                                    xtw.WriteStartElement("display_name");
                                    xtw.WriteValue(this[i].display_name);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].display_code) == false)
                                {
                                    xtw.WriteStartElement("display_code");
                                    xtw.WriteValue(this[i].display_code);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].currency_number) == false)
                                {
                                    xtw.WriteStartElement("currency_number");
                                    xtw.WriteValue(this[i].currency_number);
                                    xtw.WriteEndElement();
                                }

                                if (this[i].max_voucher_value != null)
                                {
                                    xtw.WriteStartElement("max_voucher_value");
                                    xtw.WriteValue(this[i].max_voucher_value);
                                    xtw.WriteEndElement();
                                }

                                if (this[i].number_of_decimals != null)
                                {
                                    xtw.WriteStartElement("number_of_decimals");
                                    xtw.WriteValue(this[i].number_of_decimals);
                                    xtw.WriteEndElement();
                                }

                                if (this[i].rounding_rule != null)
                                {
                                    xtw.WriteStartElement("rounding_rule");
                                    xtw.WriteValue(this[i].rounding_rule);
                                    xtw.WriteEndElement();
                                }

                               
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
    }
}
