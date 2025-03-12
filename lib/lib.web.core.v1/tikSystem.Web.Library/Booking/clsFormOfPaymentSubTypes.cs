using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace tikSystem.Web.Library
{
    public class FormOfPaymentSubTypes : LibraryBase
    {
        public FormOfPaymentSubType this[int Index]
        {
            get { return (FormOfPaymentSubType)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(FormOfPaymentSubType Value)
        {
            return this.List.Add(Value);
        }
        public void Read(string type, string language)
        {
            ServiceClient objClientService = new ServiceClient();
            string strXml;

            if (string.IsNullOrEmpty(language))
            {
                language = "EN";
            }
            objClientService.objService = objService;
            strXml = objClientService.GetFormOfPaymentSubTypes(type, language);

            if (string.IsNullOrEmpty(strXml) == false)
            {
                using (StringReader srd = new StringReader(strXml))
                {
                    XPathDocument xmlDoc = new XPathDocument(srd);
                    XPathNavigator nv = xmlDoc.CreateNavigator();
                    FormOfPaymentSubType st;
                    foreach (XPathNavigator n in nv.Select("FormOfPayments/GetFormOfPayments"))
                    {
                        st = new FormOfPaymentSubType();

                        st.form_of_payment_subtype_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_subtype_rcd");
                        st.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                        st.form_of_payment_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd");
                        st.expiry_days = XmlHelper.XpathValueNullToInt16(n, "expiry_days");
                        st.card_code = XmlHelper.XpathValueNullToEmpty(n, "card_code");
                        st.voucher_reference = XmlHelper.XpathValueNullToInt(n, "voucher_reference");
                        st.cvv_required_flag = XmlHelper.XpathValueNullToByte(n, "cvv_required_flag");
                        st.validate_document_number_flag = XmlHelper.XpathValueNullToByte(n, "validate_document_number_flag");
                        st.display_cvv_flag = XmlHelper.XpathValueNullToByte(n, "display_cvv_flag");
                        st.multiple_payment_flag = XmlHelper.XpathValueNullToByte(n, "multiple_payment_flag");
                        st.approval_code_required_flag = XmlHelper.XpathValueNullToByte(n, "approval_code_required_flag");
                        st.display_approval_code_flag = XmlHelper.XpathValueNullToByte(n, "display_approval_code_flag");
                        st.display_expiry_date_flag = XmlHelper.XpathValueNullToByte(n, "display_expiry_date_flag");
                        st.expiry_date_required_flag = XmlHelper.XpathValueNullToByte(n, "expiry_date_required_flag");
                        st.travel_agency_payment_flag = XmlHelper.XpathValueNullToByte(n, "travel_agency_payment_flag");
                        st.agency_payment_flag = XmlHelper.XpathValueNullToByte(n, "agency_payment_flag");
                        st.internet_payment_flag = XmlHelper.XpathValueNullToByte(n, "internet_payment_flag");
                        st.refund_payment_flag = XmlHelper.XpathValueNullToByte(n, "refund_payment_flag");
                        st.address_required_flag = XmlHelper.XpathValueNullToByte(n, "address_required_flag");
                        st.display_address_flag = XmlHelper.XpathValueNullToByte(n, "display_address_flag");
                        st.show_pos_indictor_flag = XmlHelper.XpathValueNullToByte(n, "show_pos_indictor_flag");
                        st.require_pos_indicator_flag = XmlHelper.XpathValueNullToByte(n, "require_pos_indicator_flag");
                        st.display_issue_date_flag = XmlHelper.XpathValueNullToByte(n, "display_issue_date_flag");
                        st.display_issue_number_flag = XmlHelper.XpathValueNullToByte(n, "display_issue_number_flag");

                        Add(st);
                        st = null;
                    }
                }
             }
            objClientService.objService = null;
        }
        public string GetXml()
        {
            if (this.Count > 0)
            {
                StringBuilder stb = new StringBuilder();
                using (System.IO.StringWriter stw = new System.IO.StringWriter(stb))
                {
                    System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(stw);

                    xtw.WriteStartElement("FormOfPayments");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("GetFormOfPayments");
                            {
                                if (string.IsNullOrEmpty(this[i].form_of_payment_subtype_rcd) == false)
                                {
                                    xtw.WriteStartElement("form_of_payment_subtype_rcd");
                                    xtw.WriteValue(this[i].form_of_payment_subtype_rcd);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].display_name) == false)
                                {
                                    xtw.WriteStartElement("display_name");
                                    xtw.WriteValue(this[i].display_name);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].form_of_payment_rcd) == false)
                                {
                                    xtw.WriteStartElement("form_of_payment_rcd");
                                    xtw.WriteValue(this[i].form_of_payment_rcd);
                                    xtw.WriteEndElement();
                                }

                                xtw.WriteStartElement("expiry_days");
                                xtw.WriteValue(this[i].expiry_days);
                                xtw.WriteEndElement();

                                if (string.IsNullOrEmpty(this[i].card_code) == false)
                                {
                                    xtw.WriteStartElement("card_code");
                                    xtw.WriteValue(this[i].card_code);
                                    xtw.WriteEndElement();
                                }

                                xtw.WriteStartElement("voucher_reference");
                                xtw.WriteValue(this[i].voucher_reference);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("cvv_required_flag");
                                xtw.WriteValue(this[i].cvv_required_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("validate_document_number_flag");
                                xtw.WriteValue(this[i].validate_document_number_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_cvv_flag");
                                xtw.WriteValue(this[i].display_cvv_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("multiple_payment_flag");
                                xtw.WriteValue(this[i].multiple_payment_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("approval_code_required_flag");
                                xtw.WriteValue(this[i].approval_code_required_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_approval_code_flag");
                                xtw.WriteValue(this[i].display_approval_code_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_expiry_date_flag");
                                xtw.WriteValue(this[i].display_expiry_date_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("expiry_date_required_flag");
                                xtw.WriteValue(this[i].expiry_date_required_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("travel_agency_payment_flag");
                                xtw.WriteValue(this[i].travel_agency_payment_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("agency_payment_flag");
                                xtw.WriteValue(this[i].agency_payment_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("internet_payment_flag");
                                xtw.WriteValue(this[i].internet_payment_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("refund_payment_flag");
                                xtw.WriteValue(this[i].refund_payment_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("address_required_flag");
                                xtw.WriteValue(this[i].address_required_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_address_flag");
                                xtw.WriteValue(this[i].display_address_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("show_pos_indictor_flag");
                                xtw.WriteValue(this[i].show_pos_indictor_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("require_pos_indicator_flag");
                                xtw.WriteValue(this[i].require_pos_indicator_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_issue_date_flag");
                                xtw.WriteValue(this[i].display_issue_date_flag);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_issue_number_flag");
                                xtw.WriteValue(this[i].display_issue_number_flag);
                                xtw.WriteEndElement();
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
    }
}
