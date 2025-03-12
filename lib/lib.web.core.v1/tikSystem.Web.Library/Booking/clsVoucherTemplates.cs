using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class VoucherTemplates : CollectionBase
    {
        public agentservice.TikAeroXMLwebservice objService = null;

        public VoucherTemplate this[int index]
        {
            get { return (VoucherTemplate)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(VoucherTemplate Value)
        {
            return this.List.Add(Value);
        }
        #region Method
        public string VoucherTemplateList(string voucherTemplateId, string voucherTemplate, DateTime fromDate, DateTime toDate, bool write, string status, string language)
        {
            string strXml = string.Empty;

            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            strXml = objClient.VoucherTemplateList(voucherTemplateId, voucherTemplate, fromDate, toDate, write, status, language);

            return strXml;
        }
        public void Fill(string xml)
        {
            if (xml.Length > 0)
            {
                using (System.IO.StringReader sr = new System.IO.StringReader(xml))
                {
                    System.Xml.XPath.XPathDocument XmlDoc = new System.Xml.XPath.XPathDocument(sr);
                    System.Xml.XPath.XPathNavigator nv = XmlDoc.CreateNavigator();

                    VoucherTemplate objVoucher;
                    foreach (System.Xml.XPath.XPathNavigator n in nv.Select("Template/Voucher"))
                    {
                        objVoucher = (VoucherTemplate)XmlHelper.Deserialize(n.OuterXml, typeof(VoucherTemplate));
                        if (objVoucher != null)
                        {
                            Add(objVoucher);
                        }
                        objVoucher = null;
                    }
                }
            }
        }
        #endregion
    }
}
