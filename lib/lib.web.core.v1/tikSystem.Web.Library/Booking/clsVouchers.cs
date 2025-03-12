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
using System.Xml.XPath;
using System.IO;
using System.Text;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Vouchers : CollectionBase
    {
        public agentservice.TikAeroXMLwebservice objService = null;
        public Guid SelectVoucherId
        {
            get { return _SelectVoucherId; }
            set { _SelectVoucherId = value; }
        }
        protected Guid _SelectVoucherId;

        public Voucher this[int index]
        {
            get { return (Voucher)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Voucher Value)
        {
            return this.List.Add(Value);
        }
        public void Read(string recordLocator,
                        string voucherNumber,
                        string voucherID,
                        string status,
                        string recipient,
                        string fOPSubType,
                        string clientProfileId,
                        string currency,
                        string password,
                        bool includeOpenVoucher,
                        bool includeExpiredVoucher,
                        bool includeUsedVoucher,
                        bool includeVoidedVoucher,
                        bool includeRefundable,
                        bool includeFareOnly,
                        bool write,
                        Mappings mapping,
                        Fees fees,
                        BookingHeader bookingHeader,
                        Itinerary itinerary,
                        Quotes quotes,
                        Payments payments)
        {

            string strXml = string.Empty;

            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            strXml = objClient.GetVouchers(recordLocator,
                                            voucherNumber,
                                            voucherID,
                                            status,
                                            recipient,
                                            fOPSubType,
                                            clientProfileId,
                                            currency,
                                            password,
                                            includeOpenVoucher,
                                            includeExpiredVoucher,
                                            includeUsedVoucher,
                                            includeVoidedVoucher,
                                            includeRefundable,
                                            includeFareOnly,
                                            write,
                                            mapping,
                                            fees);


            if (strXml.Length > 0)
            {
                XPathDocument XmlDoc = new XPathDocument(new StringReader(strXml));
                XPathNavigator nv = XmlDoc.CreateNavigator();

                Voucher objVoucher;
                Fees objFees = new Fees();
                objFees.objService = objService;
                foreach (XPathNavigator n in nv.Select("Booking/Voucher"))
                {
                    objVoucher = (Voucher)XmlHelper.Deserialize(n.OuterXml, typeof(Voucher));
                    if (objVoucher != null)
                    {
                        if (objVoucher.voucher_number.Equals(voucherNumber))
                        {
                            // Get Voucher Fee
                            XPathDocument XmlDocFee = new XPathDocument(new StringReader("<Fees>" + objFees.GetFormOfPaymentFeeXML(objVoucher.form_of_payment_rcd,
                                                                                                                        objVoucher.form_of_payment_subtype_rcd,
                                                                                                                        bookingHeader,
                                                                                                                        itinerary,
                                                                                                                        quotes,
                                                                                                                        fees,
                                                                                                                        payments) + "</Fees>"));
                            XPathNavigator nvFee = XmlDocFee.CreateNavigator();                           
                            foreach (XPathNavigator nFee in nvFee.Select("Fees"))
                            {
                                objVoucher.fee_rcd = XmlHelper.XpathValueNullToEmpty(nFee, "fee_rcd");
                                objVoucher.fee_display_name = XmlHelper.XpathValueNullToEmpty(nFee, "display_name");
                                objVoucher.fee_amount_incl = XmlHelper.XpathValueNullToDecimal(nFee, "fee_amount_incl");
                                objVoucher.fee_amount = XmlHelper.XpathValueNullToDecimal(nFee, "fee_amount");
                            }
                            Add(objVoucher);
                        }
                    }
                    objVoucher = null;
                }

            }
        }

        public string ReadVoucher(Guid voucherId, double voucherNumber)
        {
            ServiceClient objClient = new ServiceClient();
            string strXml = string.Empty;

            objClient.objService = objService;

            strXml = objClient.ReadVoucher(voucherId, voucherNumber);

            return strXml;
        }
        public void Fill(string xml)
        {
            if (xml.Length > 0)
            {
                XPathDocument XmlDoc = new XPathDocument(new StringReader(xml));
                XPathNavigator nv = XmlDoc.CreateNavigator();

                Voucher objVoucher;
                foreach (XPathNavigator n in nv.Select("Booking/Voucher"))
                {
                    objVoucher = (Voucher)XmlHelper.Deserialize(n.OuterXml, typeof(Voucher));
                    if (objVoucher != null)
                    {
                        Add(objVoucher);
                    }
                    objVoucher = null;
                }

            }
        }

        public bool Save()
        {
            ServiceClient objClient = new ServiceClient();
            bool bResult = false;

            objClient.objService = objService;

            bResult = objClient.SaveVoucher(this);

            return bResult;
        }
        public string PaymentCreditCard(Payments payments)
        {
            ServiceClient objClient = new ServiceClient();
            string strResult = string.Empty;

            objClient.objService = objService;

            strResult = objClient.VoucherPaymentCreditCard(payments, this);

            return strResult;
        }
        public bool Void(Guid voucherId, Guid userId, DateTime voidDate)
        {
            ServiceClient objClient = new ServiceClient();
            bool bResult = false;

            objClient.objService = objService;

            bResult = objClient.VoidVoucher(voucherId, userId, voidDate);

            return bResult;
        }
    }
}
