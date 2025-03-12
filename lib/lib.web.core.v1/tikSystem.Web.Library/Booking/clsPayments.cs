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
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;

namespace tikSystem.Web.Library
{
    [Serializable()]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "",
                IsNullable = false)]
    public class Payments : CollectionBase
    {
        public agentservice.TikAeroXMLwebservice objService = null;
        public Payment this[int index]
        {
            get { return (Payment)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(Payment value)
        {
            return this.List.Add(value);
        }

        [XmlIgnore()]
        private Fees _FeesForPayment;
        public Fees FeesForPayment
        {
            get { return _FeesForPayment; }
            set { _FeesForPayment = value; }
        }

        #region Function
        public string PaymentVoucher(ref BookingHeader bookingHeader,
                                    ref Itinerary itinerary,
                                    ref Passengers passengers,
                                    ref Quotes quotes,
                                    ref Fees fees,
                                    ref Mappings mappings,
                                    ref Services services,
                                    ref Remarks remarks,
                                    ref Payments payments,
                                    ref Taxes taxes,
                                    string VoucherId,
                                    string VoucherNumber,
                                    string formOfPayment,
                                    string formOfPaymentsubType,
                                    string strLanguage,
                                    Guid gUserId,
                                    string strIpAddress,
                                    bool createTikets)
        {
            Booking objBooking = new Booking();
            Library objLi = new Library();

            Payment p = null;
            decimal dFeeAmountIncl = 0;
            decimal OutStandingBalance;
            string result = string.Empty;

            if (objBooking != null)
            {
                OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                if (OutStandingBalance > 0)
                {
                    if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true)
                    {
                        //Save Payment
                        ServiceClient objClient = new ServiceClient();
                        objClient.objService = objService;

                        foreach (Mapping mp in mappings)
                        {
                            mp.create_by = gUserId;
                            mp.create_date_time = DateTime.Now;
                            mp.update_by = gUserId;
                            mp.update_date_time = DateTime.Now;
                        }

                        //Fill payment fee.
                        Fees objFees = new Fees();
                        objFees.objService = objService;

                        objFees.GetCreditCardFee(bookingHeader,
                                  passengers,
                                  itinerary,
                                  quotes,
                                  fees,
                                  payments,
                                  formOfPayment,
                                  formOfPaymentsubType,
                                  bookingHeader.currency_rcd,
                                  gUserId,
                                  string.Empty,
                                  OutStandingBalance);

                        //Add Fee Amount include.

                        if (objFees != null && objFees.Count > 0)
                        {
                            for (int i = 0; i < objFees.Count; i++)
                            {
                                dFeeAmountIncl = dFeeAmountIncl + objFees[i].fee_amount_incl;
                            }

                        }

                        p = new Payment();
                        {
                            // Calculate voucher and add voucher information to payment object
                            p.voucher_payment_id = new Guid(VoucherId);
                            p.document_number = VoucherNumber;
                            p.payment_due_date_time = DateTime.MinValue;
                            p.payment_date_time = DateTime.Now;
                            p.booking_payment_id = Guid.NewGuid();
                            p.booking_id = bookingHeader.booking_id;
                            p.form_of_payment_rcd = formOfPayment;
                            p.form_of_payment_subtype_rcd = formOfPaymentsubType;
                            p.document_amount = OutStandingBalance + dFeeAmountIncl;
                            p.payment_amount = OutStandingBalance + dFeeAmountIncl;
                            p.receive_payment_amount = OutStandingBalance + dFeeAmountIncl;
                            p.currency_rcd = bookingHeader.currency_rcd;
                            p.receive_currency_rcd = bookingHeader.currency_rcd;
                            p.agency_code = bookingHeader.agency_code;
                            p.payment_by = gUserId;
                            p.create_by = gUserId;
                            p.update_by = gUserId;
                            p.create_date_time = DateTime.Now;
                            p.client_profile_id = bookingHeader.client_profile_id;
                            p.ip_address = strIpAddress;
                        }
                        this.Add(p);

                        //Save booking information
                        objBooking.objService = objService;
                        result = objClient.SaveBookingPayment(bookingHeader.booking_id.ToString(),
                                                              bookingHeader,
                                                              itinerary,
                                                              passengers,
                                                              remarks,
                                                              this,
                                                              mappings,
                                                              services,
                                                              taxes,
                                                              fees,
                                                              null,
                                                              createTikets,
                                                              false,
                                                              false,
                                                              strLanguage);

                        if (result.Length > 0)
                        {
                            //Get Xml String and return out to generate control
                            objLi.FillBooking(result,
                                            ref bookingHeader,
                                            ref passengers,
                                            ref itinerary,
                                            ref mappings,
                                            ref payments,
                                            ref remarks,
                                            ref taxes,
                                            ref quotes,
                                            ref fees,
                                            ref services);
                            //objBooking.formOfPaymentFee = string.Empty;
                        }
                    }
                }
            }
            return result;
        }
        public string PaymentCreditCard(ref BookingHeader bookingHeader,
                                        ref Itinerary itinerary,
                                        ref Passengers passengers,
                                        ref Quotes quotes,
                                        ref Fees fees,
                                        ref Mappings mappings,
                                        ref Services services,
                                        ref Remarks remarks,
                                        ref Payments payments,
                                        ref Taxes taxes,
                                        string xmlCreditCard,
                                        string strRequestSource,
                                        string strLanguage,
                                        Guid userId,
                                        Guid bookingPaymentId,
                                        string ipAddress,
                                        string formOfPaymentFee,
                                        Int16 adult,
                                        Int16 child,
                                        Int16 infant,
                                        bool createTikets)
        {

            try
            {
                string strPaymentResult = string.Empty;
                
                if (bookingHeader != null)
                {
                    //Validate credit card information and make payment
                    Payment paymentInput = CreateCreditCardPaymentInput(xmlCreditCard);

                    //Make payment.
                    return PaymentCreditCard(ref bookingHeader,
                                            ref itinerary,
                                            ref passengers,
                                            ref quotes,
                                            ref fees,
                                            ref mappings,
                                            ref services,
                                            ref remarks,
                                            ref payments,
                                            ref taxes,
                                            paymentInput,
                                            strRequestSource,
                                            strLanguage,
                                            userId,
                                            bookingPaymentId,
                                            ipAddress,
                                            formOfPaymentFee,
                                            adult,
                                            child,
                                            infant,
                                            createTikets);
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public Payment CreateCreditCardPaymentInput(string xmlCreditCard)
        {
            try
            {
                Payment paymentInput;
                XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlCreditCard));
                XPathNavigator nv = xmlDoc.CreateNavigator();

                foreach (XPathNavigator n in nv.Select("payment"))
                {
                    //Fill credit card payment information.
                    paymentInput = new Payment();
                    paymentInput.name_on_card = XmlHelper.XpathValueNullToEmpty(n, "NameOnCard");
                    paymentInput.document_number = XmlHelper.XpathValueNullToEmpty(n, "CreditCardNumber");
                    paymentInput.form_of_payment_subtype_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_subtype_rcd");
                    paymentInput.form_of_payment_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd");

                    if (XmlHelper.XpathValueNullToByte(n, "display_issue_date_flag") == 1)
                    {
                        paymentInput.issue_month = XmlHelper.XpathValueNullToInt16(n, "IssueMonth");
                        paymentInput.issue_year = XmlHelper.XpathValueNullToInt16(n, "IssueYear");
                    }

                    if (XmlHelper.XpathValueNullToByte(n, "display_issue_number_flag") == 1)
                    {
                        paymentInput.issue_number = XmlHelper.XpathValueNullToEmpty(n, "IssueNumber");
                    }

                    if (XmlHelper.XpathValueNullToByte(n, "display_expiry_date_flag") == 1)
                    {
                        paymentInput.expiry_month = XmlHelper.XpathValueNullToInt16(n, "ExpiryMonth");
                        paymentInput.expiry_year = XmlHelper.XpathValueNullToInt16(n, "ExpiryYear");
                    }

                    if (XmlHelper.XpathValueNullToByte(n, "display_cvv_flag") == 1)
                    {
                        paymentInput.cvv_code = XmlHelper.XpathValueNullToEmpty(n, "CVV");
                    }

                    if (XmlHelper.XpathValueNullToByte(n, "display_address_flag") == 1)
                    {
                        paymentInput.address_line1 = XmlHelper.XpathValueNullToEmpty(n, "Addr1") + " " +
                                          XmlHelper.XpathValueNullToEmpty(n, "Addr2");
                        paymentInput.street = XmlHelper.XpathValueNullToEmpty(n, "Street");
                        paymentInput.state = XmlHelper.XpathValueNullToEmpty(n, "State");
                        paymentInput.city = XmlHelper.XpathValueNullToEmpty(n, "City");
                        paymentInput.zip_code = XmlHelper.XpathValueNullToEmpty(n, "ZipCode");
                        paymentInput.country_rcd = XmlHelper.XpathValueNullToEmpty(n, "Country");
                    }

                    paymentInput.receive_payment_amount = XmlHelper.XpathValueNullToDecimal(n, "exchange_fee_amount");
                    paymentInput.receive_currency_rcd = XmlHelper.XpathValueNullToEmpty(n, "exchange_currency");

                    //ELV Section
                    paymentInput.bank_code = XmlHelper.XpathValueNullToEmpty(n, "bank_code");
                    paymentInput.bank_name = XmlHelper.XpathValueNullToEmpty(n, "bank_name");
                    paymentInput.bank_iban = XmlHelper.XpathValueNullToEmpty(n, "bank_iban");

                    return paymentInput;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public string PaymentCreditCard(ref BookingHeader bookingHeader,
                                        ref Itinerary itinerary,
                                        ref Passengers passengers,
                                        ref Quotes quotes,
                                        ref Fees fees,
                                        ref Mappings mappings,
                                        ref Services services,
                                        ref Remarks remarks,
                                        ref Payments payments,
                                        ref Taxes taxes,
                                        Payment paymentInput,
                                        string strRequestSource,
                                        string strLanguage,
                                        Guid userId,
                                        Guid bookingPaymentId,
                                        string ipAddress,
                                        string formOfPaymentFee,
                                        Int16 adult,
                                        Int16 child,
                                        Int16 infant,
                                        bool createTikets)
        {
            string strPaymentResult = string.Empty;
            try
            {
                if (bookingHeader != null)
                {

                    Library objLi = new Library();
                    decimal dFeeAmountIncl = 0;
                    decimal OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);

                    paymentInput.currency_rcd = bookingHeader.currency_rcd;
                    paymentInput.agency_code = bookingHeader.agency_code;
                    if (ValidPaymentInput(paymentInput) == true)
                    {
                        if (OutStandingBalance >= 0)
                        {
                            //Clear Fee Before add the new one
                            ClearFee(fees,
                                    itinerary[0].booking_segment_id,
                                    passengers[0].passenger_id,
                                    paymentInput.form_of_payment_subtype_rcd);

                            //Fill payment fee.
                            Fees objFees = new Fees();
                            objFees.objService = objService;

                            objFees.GetCreditCardFee(bookingHeader,
                                                    passengers,
                                                    itinerary,
                                                    quotes,
                                                    fees,
                                                    payments,
                                                    string.Empty,
                                                    paymentInput.form_of_payment_subtype_rcd,
                                                    bookingHeader.currency_rcd,
                                                    userId,
                                                    formOfPaymentFee,
                                                    OutStandingBalance);


                            //Add Fee Amount include.

                            if (objFees != null && objFees.Count > 0)
                            {
                                for (int i = 0; i < objFees.Count; i++)
                                {
                                    dFeeAmountIncl = dFeeAmountIncl + objFees[i].fee_amount_incl;
                                }

                            }

                            //Fill payment information.
                            paymentInput.payment_due_date_time = DateTime.MinValue;
                            paymentInput.payment_date_time = DateTime.Now;
                            //Assing booking_payment_id.
                            //  If booking_payment_id in object booking is not null then assign this id
                            //  else booking_payment_id in object booking is null then generate the id and assign to payment object and keep in session as reference
                            if (bookingPaymentId.Equals(Guid.Empty))
                            {
                                bookingPaymentId = Guid.NewGuid();
                            }
                            paymentInput.booking_payment_id = bookingPaymentId;
                            paymentInput.booking_id = bookingHeader.booking_id;
                            if (paymentInput.receive_payment_amount == 0)
                            {
                                paymentInput.receive_payment_amount = OutStandingBalance + dFeeAmountIncl;
                                paymentInput.receive_currency_rcd = bookingHeader.currency_rcd;
                            }
                            paymentInput.payment_amount = OutStandingBalance + dFeeAmountIncl;
                            paymentInput.payment_by = userId;
                            paymentInput.create_by = userId;
                            paymentInput.update_by = userId;
                            paymentInput.create_date_time = DateTime.Now;
                            paymentInput.client_profile_id = bookingHeader.client_profile_id;
                            paymentInput.ip_address = ipAddress;

                            if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true)
                            {
                                //Validate credit card information and make payment
                                ServiceClient objClient = new ServiceClient();

                                //Fill Payment input to object payment
                                this.Add(paymentInput);

                                //Call credit payment.
                                objClient.objService = objService;
                                strPaymentResult = objClient.SaveBookingCreditCard(bookingHeader.booking_id.ToString(),
                                                                                    bookingHeader,
                                                                                    itinerary,
                                                                                    passengers,
                                                                                    remarks,
                                                                                    this,
                                                                                    mappings,
                                                                                    services,
                                                                                    taxes,
                                                                                    fees,
                                                                                    objFees,
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    strRequestSource,
                                                                                    createTikets,
                                                                                    false,
                                                                                    false,
                                                                                    strLanguage);


                                if (string.IsNullOrEmpty(strPaymentResult) == false)
                                {
                                    XPathDocument xmlDoc = new XPathDocument(new StringReader(strPaymentResult));
                                    XPathNavigator nv = xmlDoc.CreateNavigator();

                                    if (nv.Select("Booking/BookingHeader").Count > 0)
                                    {
                                        //Get Xml String and return out to generate control
                                        objLi.FillBooking(strPaymentResult,
                                                          ref bookingHeader,
                                                          ref passengers,
                                                          ref itinerary,
                                                          ref mappings,
                                                          ref payments,
                                                          ref remarks,
                                                          ref taxes,
                                                          ref quotes,
                                                          ref fees,
                                                          ref services);
                                        //objBooking.formOfPaymentFee = string.Empty;
                                    }
                                    return strPaymentResult;
                                }
                            }
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(strPaymentResult) == false)
                {
                    throw new Exception("payment xml Output<br/>" + strPaymentResult.Replace("<", "&lt;").Replace(">", "&gt;") + "<br/>Message" + ex.Message, ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        public string PaymentCreditAgent(ref BookingHeader bookingHeader,
                                         ref Itinerary itinerary,
                                         ref Passengers passengers,
                                         ref Quotes quotes,
                                         ref Fees fees,
                                         ref Mappings mappings,
                                         ref Services services,
                                         ref Remarks remarks,
                                         ref Payments payments,
                                         ref Taxes taxes,
                                         string xmlPayment,
                                         string strLanguage,
                                         Guid userId,
                                         string ipAddress,
                                         Agent agent)
        {
            if (bookingHeader != null)
            {
                Payment paymentInput = CreateCreditAgentInput(xmlPayment);
                if (paymentInput != null)
                {
                    return PaymentCreditAgent(ref bookingHeader,
                                             ref itinerary,
                                             ref passengers,
                                             ref quotes,
                                             ref fees,
                                             ref mappings,
                                             ref services,
                                             ref remarks,
                                             ref payments,
                                             ref taxes,
                                             paymentInput,
                                             strLanguage,
                                             userId,
                                             ipAddress,
                                             agent);
                }
            }
            return string.Empty;
        }
        public string PaymentCreditAgent(ref BookingHeader bookingHeader,
                                         ref Itinerary itinerary,
                                         ref Passengers passengers,
                                         ref Quotes quotes,
                                         ref Fees fees,
                                         ref Mappings mappings,
                                         ref Services services,
                                         ref Remarks remarks,
                                         ref Payments payments,
                                         ref Taxes taxes,
                                         Payment paymentInput,
                                         string strLanguage,
                                         Guid userId,
                                         string ipAddress,
                                         Agent agent)
        {
            string result = string.Empty;
            if (paymentInput != null && 
                string.IsNullOrEmpty(paymentInput.form_of_payment_rcd) == false &&
                (paymentInput.form_of_payment_rcd.ToUpper() == "INV" || paymentInput.form_of_payment_rcd.ToUpper() == "CRAGT"))
            {
                string errorMsg = string.Empty;

                decimal OutStandingBalance;

                decimal dbAgencyAccountBalance = 0;
                bool bGetAgencyAccountBalance = false;

                //yo
                decimal OutStandingBalanceCalculate = 0;
                bool IsSameCurrency = true;
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;

                if (bookingHeader != null)
                {
                    Library objLi = new Library();

                    OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);

                    if (OutStandingBalance > -1)
                    {
                        if (agent.currency_rcd != bookingHeader.currency_rcd)
                        {
                            IsSameCurrency = false;
                            OutStandingBalanceCalculate = Convert.ToDecimal(objClient.CalculateExchange(bookingHeader.currency_rcd,
                                                                                                        agent.currency_rcd,
                                                                                                        Convert.ToDouble(OutStandingBalance),
                                                                                                        string.Empty,
                                                                                                        DateTime.MinValue,
                                                                                                        false));
                        }

                        // If FOP = Credit Agency Account. So, get agency account balance
                        dbAgencyAccountBalance = this.GetCurrentBalance(bookingHeader.agency_code, bookingHeader.create_by.ToString(), ref bGetAgencyAccountBalance);
                        if (bGetAgencyAccountBalance)
                        {
                            if (!IsSameCurrency)
                            {
                                if (OutStandingBalanceCalculate > dbAgencyAccountBalance)
                                {
                                    // Generate Error Code "001" for dbAgencyAccountBalance is not enough payment
                                    result = "<ErrorResponse><Error><Message>001</Message><LastBalance>" + dbAgencyAccountBalance.ToString() + "</LastBalance></Error></ErrorResponse>";
                                }
                            }
                            else
                            {
                                if (OutStandingBalance > dbAgencyAccountBalance)
                                {
                                    // Generate Error Code "001" for dbAgencyAccountBalance is not enough payment
                                    result = "<ErrorResponse><Error><Message>001</Message><LastBalance>" + dbAgencyAccountBalance.ToString() + "</LastBalance></Error></ErrorResponse>";
                                }
                            }

                        }
                        else
                        {
                            // Generate Error Code "002" for cannot get Agency Account Blance
                            result = "<ErrorResponse><Error><Message>002</Message></Error></ErrorResponse>";
                        }

                        // If Does not any error message
                        if (string.IsNullOrEmpty(result))
                        {
                            paymentInput.payment_due_date_time = DateTime.MinValue;
                            paymentInput.payment_date_time = DateTime.Now;
                            paymentInput.booking_payment_id = Guid.NewGuid();
                            paymentInput.booking_id = bookingHeader.booking_id;

                            
                            paymentInput.document_amount = OutStandingBalance;
                            paymentInput.payment_amount = OutStandingBalance;

                            //Assign value for multi-currency payment.
                            if (!IsSameCurrency)
                            {
                                paymentInput.receive_payment_amount = OutStandingBalanceCalculate;
                                paymentInput.receive_currency_rcd = agent.currency_rcd;
                            }
                            else
                            {
                                paymentInput.receive_payment_amount = OutStandingBalance;
                                paymentInput.receive_currency_rcd = bookingHeader.currency_rcd;
                            }

                            paymentInput.currency_rcd = bookingHeader.currency_rcd;
                            paymentInput.agency_code = bookingHeader.agency_code;
                            paymentInput.payment_by = userId;
                            paymentInput.create_by = userId;
                            paymentInput.update_by = userId;
                            paymentInput.create_date_time = DateTime.Now;
                            paymentInput.client_profile_id = bookingHeader.client_profile_id;
                            paymentInput.ip_address = ipAddress;

                            this.Add(paymentInput);
                            
                            if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true)
                            {
                                //Validate credit card information and make payment
                                foreach (Mapping mp in mappings)
                                {
                                    mp.create_by = userId;
                                    mp.create_date_time = DateTime.Now;
                                    mp.update_by = userId;
                                    mp.update_date_time = DateTime.Now;
                                }

                                //Save booking information
                                Booking objBooking = new Booking();
                                objBooking.objService = objService;
                                result = objClient.SaveBookingPayment(bookingHeader.booking_id.ToString(),
                                                                        bookingHeader,
                                                                        itinerary,
                                                                        passengers,
                                                                        remarks,
                                                                        this,
                                                                        mappings,
                                                                        services,
                                                                        taxes,
                                                                        fees,
                                                                        null,
                                                                        true,
                                                                        false,
                                                                        false,
                                                                        strLanguage);

                                if (string.IsNullOrEmpty(result) == false)
                                {
                                    XPathDocument xmlDoc = new XPathDocument(new StringReader(result));
                                    XPathNavigator nv = xmlDoc.CreateNavigator();

                                    if (nv.Select("Booking/BookingHeader").Count > 0)
                                    {
                                        //Get Xml String and return out to generate control
                                        objLi.FillBooking(result,
                                                          ref bookingHeader,
                                                          ref passengers,
                                                          ref itinerary,
                                                          ref mappings,
                                                          ref payments,
                                                          ref remarks,
                                                          ref taxes,
                                                          ref quotes,
                                                          ref fees,
                                                          ref services);
                                    }
                                }
                            }
                            // Start Yai add checking FOP and Agency Accout Balance
                        }
                        // End Yai add checking FOP and Agency Accout Balance
                    }

                }
            }
            return result;
        }

        public Payment CreateCreditAgentInput(string xmlInput)
        {
            Payment objPayment = null;
            
            XPathDocument xmlDoc = new XPathDocument(new StringReader(xmlInput));
            XPathNavigator nv = xmlDoc.CreateNavigator();
            
            foreach (XPathNavigator n in nv.Select("payment"))
            {

                objPayment = new Payment();
                objPayment.form_of_payment_rcd = XmlHelper.XpathValueNullToEmpty(n, "form_of_payment_rcd");
                objPayment.document_number = XmlHelper.XpathValueNullToEmpty(n, "document_number");
            }

            return objPayment;
        }
        public bool ValidateCreditAgencyInput(Payment paymentInput)
        {
            if (paymentInput != null)
            {
                if (string.IsNullOrEmpty(paymentInput.form_of_payment_rcd) == false) 
                {
                    if (paymentInput.form_of_payment_rcd == "CRAGT" || paymentInput.form_of_payment_rcd == "INV")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool FillCreditCardOffline(ref BookingHeader bookingHeader,
                                          ref Itinerary itinerary,
                                          ref Passengers passengers,
                                          ref Quotes quotes,
                                          ref Fees fees,
                                          ref Fees paymentFees,
                                          ref Mappings mappings,
                                          ref Services services,
                                          ref Remarks remarks,
                                          ref Payments payments,
                                          ref Taxes taxes,
                                          string xmlCreditCard,
                                          string formOfPaymentFee,
                                          Guid userId, 
                                          Guid bookingPaymentId,
                                          string ipAddress)
        {
            Fee feeCopy;
            Payment p;

            string errorMsg = string.Empty;
            string CardType = string.Empty;

            bool isCompleted = true;
            decimal OutStandingBalance;
            decimal dFeeAmount = 0;
            decimal dFeeAmountIncl = 0;

            if (bookingHeader != null)
            {
                XPathDocument xmlDoc;
                XPathNavigator nv;
                Library objLi = new Library();

                OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                if (OutStandingBalance > 0)
                {
                    xmlDoc = new XPathDocument(new StringReader(xmlCreditCard));
                    nv = xmlDoc.CreateNavigator();

                    foreach (XPathNavigator n in nv.Select("payment"))
                    {
                        p = new Payment();
                        CardType = objLi.getXPathNodevalue(n, "form_of_payment_subtype_rcd", Library.xmlReturnType.value);

                        dFeeAmount = ConvertToDecimal(objLi.getXPathNodevalue(n, "fee_amount", Library.xmlReturnType.value));
                        dFeeAmountIncl = ConvertToDecimal(objLi.getXPathNodevalue(n, "fee_amount_incl", Library.xmlReturnType.value));

                        p.name_on_card = objLi.getXPathNodevalue(n, "NameOnCard", Library.xmlReturnType.value);
                        if (p.name_on_card.Trim() == "")
                            p.name_on_card = passengers[0].firstname + " " + passengers[0].lastname;
                        p.document_number = objLi.getXPathNodevalue(n, "CreditCardNumber", Library.xmlReturnType.value);
                        p.form_of_payment_subtype_rcd = CardType;
                        p.form_of_payment_rcd = objLi.getXPathNodevalue(n, "form_of_payment_rcd", Library.xmlReturnType.value);

                        if (objLi.getXPathNodevalue(n, "display_issue_date_flag", Library.xmlReturnType.value) == "1")
                        {
                            p.issue_month = Convert.ToInt16(objLi.getXPathNodevalue(n, "IssueMonth", Library.xmlReturnType.value));
                            p.issue_year = Convert.ToInt16(objLi.getXPathNodevalue(n, "IssueYear", Library.xmlReturnType.value));
                        }

                        if (objLi.getXPathNodevalue(n, "display_issue_number_flag", Library.xmlReturnType.value) == "1")
                        { p.issue_number = objLi.getXPathNodevalue(n, "IssueNumber", Library.xmlReturnType.value); }

                        if (objLi.getXPathNodevalue(n, "display_expiry_date_flag", Library.xmlReturnType.value) == "1")
                        {
                            p.expiry_month = Convert.ToInt16(objLi.getXPathNodevalue(n, "ExpiryMonth", Library.xmlReturnType.value));
                            p.expiry_year = Convert.ToInt16(objLi.getXPathNodevalue(n, "ExpiryYear", Library.xmlReturnType.value));
                        }

                        if (objLi.getXPathNodevalue(n, "display_cvv_flag", Library.xmlReturnType.value) == "1")
                        { p.cvv_code = objLi.getXPathNodevalue(n, "CVV", Library.xmlReturnType.value); }
                        if (objLi.getXPathNodevalue(n, "display_address_flag", Library.xmlReturnType.value) == "1")
                        {
                            p.address_line1 = objLi.getXPathNodevalue(n, "Addr1", Library.xmlReturnType.value) + " " +
                                              objLi.getXPathNodevalue(n, "Addr2", Library.xmlReturnType.value);
                            p.street = objLi.getXPathNodevalue(n, "Street", Library.xmlReturnType.value);
                            p.state = objLi.getXPathNodevalue(n, "State", Library.xmlReturnType.value);
                            p.city = objLi.getXPathNodevalue(n, "City", Library.xmlReturnType.value);
                            p.zip_code = objLi.getXPathNodevalue(n, "ZipCode", Library.xmlReturnType.value);
                            p.country_rcd = objLi.getXPathNodevalue(n, "Country", Library.xmlReturnType.value);
                        }
                        p.payment_due_date_time = DateTime.MinValue;
                        p.payment_date_time = DateTime.Now;

                        if (bookingPaymentId.Equals(Guid.Empty))
                            p.booking_payment_id = Guid.NewGuid();
                        else
                            p.booking_payment_id = bookingPaymentId;

                        p.booking_id = bookingHeader.booking_id;
                        if (objLi.getXPathNodevalue(n, "exchange_currency", Library.xmlReturnType.value) != "" & objLi.getXPathNodevalue(n, "exchange_fee_amount", Library.xmlReturnType.value) != "")
                        {
                            p.receive_payment_amount = ConvertToDecimal(objLi.getXPathNodevalue(n, "exchange_fee_amount", Library.xmlReturnType.value));
                            p.receive_currency_rcd = objLi.getXPathNodevalue(n, "exchange_currency", Library.xmlReturnType.value);
                        }
                        else
                        {
                            p.receive_payment_amount = OutStandingBalance + dFeeAmountIncl;
                            p.receive_currency_rcd = bookingHeader.currency_rcd;
                        }
                        p.payment_amount = OutStandingBalance + dFeeAmountIncl;
                        p.currency_rcd = bookingHeader.currency_rcd;
                        p.agency_code = bookingHeader.agency_code;
                        p.payment_by = userId;
                        p.create_by = userId;
                        p.update_by = userId;
                        p.create_date_time = DateTime.Now;
                        p.client_profile_id = bookingHeader.client_profile_id;
                        p.ip_address = ipAddress;

                        //Validate payment value.
                        if (p.name_on_card.Length == 0 ||
                           p.document_number.Length == 0 ||
                           p.form_of_payment_subtype_rcd.Length == 0 ||
                           p.form_of_payment_rcd.Length == 0 ||
                           p.currency_rcd.Length == 0 ||
                           p.agency_code.Length == 0)
                        {
                            isCompleted = false;
                            break;
                        }
                        else
                        {
                            this.Add(p);
                            //tp.Add(p);
                        }
                        p = null;
                    }

                    if (isCompleted == true)
                    {
                        //Reset value to false for reused
                        paymentFees = new Fees();

                        //Fill Credit Card fee
                        xmlDoc = new XPathDocument(new StringReader(formOfPaymentFee));
                        nv = xmlDoc.CreateNavigator();
                        foreach (XPathNavigator n in nv.Select("FormOfPaymentSubtypeFees/Fee[fee_rcd = '" + CardType + "']"))
                        {
                            feeCopy = new Fee();

                            feeCopy.acct_fee_amount = dFeeAmount;
                            feeCopy.acct_fee_amount_incl = dFeeAmountIncl;
                            feeCopy.booking_fee_id = new Guid();
                            feeCopy.booking_id = bookingHeader.booking_id;
                            feeCopy.currency_rcd = bookingHeader.currency_rcd;
                            feeCopy.display_name = objLi.getXPathNodevalue(n, "display_name", Library.xmlReturnType.value);
                            feeCopy.fee_amount = dFeeAmount;
                            feeCopy.fee_amount_incl = dFeeAmountIncl;
                            feeCopy.fee_id = new Guid(objLi.getXPathNodevalue(n, "fee_id", Library.xmlReturnType.value));
                            feeCopy.fee_rcd = objLi.getXPathNodevalue(n, "fee_rcd", Library.xmlReturnType.value);
                            feeCopy.passenger_id = Guid.Empty;
                            feeCopy.booking_segment_id = Guid.Empty;
                            feeCopy.vat_percentage = Convert.ToDecimal(objLi.getXPathNodevalue(n, "vat_percentage", Library.xmlReturnType.value));
                            feeCopy.void_date_time = DateTime.MinValue;
                            feeCopy.create_by = userId;
                            feeCopy.create_date_time = DateTime.Now;
                            feeCopy.update_by = userId;
                            feeCopy.update_date_time = DateTime.Now;
                            paymentFees.Add(feeCopy);
                            feeCopy = null;
                        }

                        xmlDoc = null;
                        nv = null;
                    }
                }
            }
            return isCompleted;
        }

        public string PaymentCreditCardOffline(ref BookingHeader bookingHeader,
                                               ref Itinerary itinerary,
                                               ref Passengers passengers,
                                               ref Quotes quotes,
                                               ref Fees fees,
                                               ref Fees paymentFees,
                                               ref Mappings mappings,
                                               ref Services services,
                                               ref Remarks remarks,
                                               ref Payments payments,
                                               ref Taxes taxes,
                                               string strLanguage)
        {


            string errorMsg = string.Empty;
            string CardType = string.Empty;

            string strPaymentResult = string.Empty;

            Library objLi = new Library();
            if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true)
            {
                //Save booking information
                //Validate credit card information and make payment
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;
                strPaymentResult = objClient.SaveBookingPayment(bookingHeader.booking_id.ToString(),
                                                              bookingHeader,
                                                              itinerary,
                                                              passengers,
                                                              remarks,
                                                              this,
                                                              mappings,
                                                              services,
                                                              taxes,
                                                              fees,
                                                              paymentFees,
                                                              true,
                                                              false,
                                                              false,
                                                              strLanguage);

                if (strPaymentResult.Length > 0)
                {
                    //Get Xml String and return out to generate control
                    objLi.FillBooking(strPaymentResult,
                                    ref bookingHeader,
                                    ref passengers,
                                    ref itinerary,
                                    ref mappings,
                                    ref payments,
                                    ref remarks,
                                    ref taxes,
                                    ref quotes,
                                    ref fees,
                                    ref services);
                    //objBooking.formOfPaymentFee = string.Empty;
                }
            }

            return strPaymentResult;
        }
        public string SavePayment(Booking objBooking,
                                  ref BookingHeader bookingHeader,
                                  ref Itinerary itinerary,
                                  ref Passengers passengers,
                                  ref Quotes quotes,
                                  ref Fees fees,
                                  ref Fees paymentFees,
                                  ref Mappings mappings,
                                  ref Services services,
                                  ref Remarks remarks,
                                  ref Payments payments,
                                  ref Taxes taxes)
        {


            string errorMsg = string.Empty;
            string CardType = string.Empty;

            string strPaymentResult = string.Empty;

            Library objLi = new Library();
            if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true)
            {
                //Save booking information
                //Validate credit card information and make payment
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;
                strPaymentResult = objClient.SavePayment(bookingHeader.booking_id.ToString(), bookingHeader, itinerary, passengers, this, mappings, fees, paymentFees, true);

                if (strPaymentResult.Length > 0)
                {
                    //Get Xml String and return out to generate control
                    objLi.FillBooking(strPaymentResult,
                                    ref bookingHeader,
                                    ref passengers,
                                    ref itinerary,
                                    ref mappings,
                                    ref payments,
                                    ref remarks,
                                    ref taxes,
                                    ref quotes,
                                    ref fees,
                                    ref services);
                    //objBooking.formOfPaymentFee = string.Empty;
                }
            }

            return strPaymentResult;
        }
        public string GetAceInsuranceQuote(BookingHeader objHeader, 
                                            Passengers objPax, 
                                            string strSpName, 
                                            string strUrl, 
                                            string strUserId, 
                                            string strPassword, 
                                            DateTime clientDt, 
                                            string strLanguage,
                                            string strOrg,
                                            string strAppName,
                                            string strCompanyProductCd,
                                            string strGroupDestId,
                                            string strGroupDestDesc,
                                            string strGroupInsureId,
                                            string strGroupInsureDesc,
                                            string strGroupPlanId,
                                            string strPlanDescription,
                                            ref bool bSuccess,
                                            Int16 adult,
                                            Int16 child,
                                            Int16 infant,
                                            string departureDate,
                                            string returnDate,
                                            string originRcd,
                                            string destinationRcd)
        {
            AceInsurance.ACORD objACORD = new AceInsurance.ACORD();
            try
            {
                AceInsurance.ACORDService objAceSvc = new AceInsurance.ACORDService();

                objAceSvc.Url = strUrl;

                //User Info
                objACORD.SignonRq = new AceInsurance.ACORDSignonRq();
                objACORD.SignonRq.SignonPswd = new AceInsurance.ACORDSignonRqSignonPswd();
                objACORD.SignonRq.SignonPswd.CustId = new AceInsurance.ACORDSignonRqSignonPswdCustId();
                objACORD.SignonRq.SignonPswd.CustId.SPName = strSpName;
                objACORD.SignonRq.SignonPswd.CustId.CustLoginId = strUserId;

                //Security Info
                objACORD.SignonRq.SignonPswd.CustPswd = new AceInsurance.ACORDSignonRqSignonPswdCustPswd();
                objACORD.SignonRq.SignonPswd.CustPswd.EncryptionTypeCd = AceInsurance.ACORDSignonRqSignonPswdCustPswdEncryptionTypeCd.None;
                objACORD.SignonRq.SignonPswd.CustPswd.Pswd = strPassword;

                objACORD.SignonRq.ClientDt = clientDt;
                objACORD.SignonRq.CustLangPref = strLanguage;

                objACORD.SignonRq.ClientApp = new AceInsurance.ACORDSignonRqClientApp();
                objACORD.SignonRq.ClientApp.Org = strOrg;
                objACORD.SignonRq.ClientApp.Name = strAppName;
                objACORD.SignonRq.ClientApp.Version = 1.0M;

                //Insurance request detail.
                objACORD.InsuranceSvcRq = new AceInsurance.ACORDInsuranceSvcRq();
                objACORD.InsuranceSvcRq.RqUID = Guid.NewGuid().ToString();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRq();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.TransactionRequestDt = clientDt;

                //Passenger information.
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipal();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfo();

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo[(adult + child + infant) + 1];

                bool bFirstSameContct = false;
                if (objHeader.title_rcd.ToUpper().Equals(objPax[0].title_rcd) & objHeader.firstname.ToUpper().Equals(objPax[0].firstname) & objHeader.lastname.ToUpper().Equals(objPax[0].lastname))
                {
                    bFirstSameContct = true;
                }
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].id = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.Surname = objHeader.lastname;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.GivenName = objHeader.firstname;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.TitlePrefix = objHeader.title_rcd.ToUpper();

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryNameCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryNameCd.Alias;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryName();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.Surname = objHeader.lastname;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.GivenName = objHeader.firstname;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.TitlePrefix = objHeader.title_rcd;
                if (bFirstSameContct == true)
                {
                    if (objPax[0].date_of_birth != DateTime.MinValue)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDtSpecified = true;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDt = objPax[0].date_of_birth;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDt = Convert.ToDateTime("1900-01-01");
                    }
                }

                for (int i = 0; i < objPax.Count; i++)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].id = (i + 2).ToString();
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.Surname = objPax[i].lastname;
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.GivenName = objPax[i].firstname;
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.TitlePrefix = objPax[i].title_rcd.ToUpper();

                    if (objPax[i].date_of_birth != DateTime.MinValue)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDtSpecified = true;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDt = objPax[i].date_of_birth;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDt = Convert.ToDateTime("1900-01-01");
                    }

                    if (objPax[i].gender_type_rcd.Equals("M"))
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.M;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                    }
                    else if (objPax[i].gender_type_rcd.Equals("F"))
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.F;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.U;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                    }
                }


                //Address Information from contact infomation.
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoAddr[1];

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoAddr();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].id = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].NameInfoRef = "1";

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr1 = objHeader.address_line1;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr2 = objHeader.address_line2;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].City = objHeader.city;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].StateProv = objHeader.state;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].PostalCode = objHeader.zip_code.Replace("-", "");
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Country = objHeader.country_rcd;

                //Communication information.
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunications[1];
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunications();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].id = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].NameInfoRef = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo[2];

                for (int i = 0; i < 2; i++)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo();
                    if (i == 0)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Telephone;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_home;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Cell;
                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_mobile;
                    }

                }

                //Email Information.
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo[1];
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0].EmailAddr = objHeader.contact_email;

                //Policy information
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicy();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.CompanyProductCd = strCompanyProductCd;

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyContractTerm();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.EffectiveDt = new DateTime(Convert.ToInt16(departureDate.Substring(0, 4)),
                                                                                                                   Convert.ToInt16(departureDate.Substring(4, 2)),
                                                                                                                   Convert.ToInt16(departureDate.Substring(6, 2)));

                if (string.IsNullOrEmpty(returnDate) == false)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.ExpirationDt = new DateTime(Convert.ToInt16(returnDate.Substring(0, 4)),
                                                                                                                        Convert.ToInt16(returnDate.Substring(4, 2)),
                                                                                                                        Convert.ToInt16(returnDate.Substring(6, 2)));
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.ExpirationDt = new DateTime(Convert.ToInt16(departureDate.Substring(0, 4)),
                                                                                                                        Convert.ToInt16(departureDate.Substring(4, 2)),
                                                                                                                        Convert.ToInt16(departureDate.Substring(6, 2)));
                }

                //Group destination Id
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_Destination();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination.RqUID = strGroupDestId;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination.DestinationDesc = strGroupDestDesc;

                //Group InsuredPackage
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_InsuredPackage();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage.RqUID = strGroupInsureId;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage.InsuredPackageDesc = strGroupInsureDesc;

                //Group plan
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_Plan();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan.RqUID = strGroupPlanId;
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan.PlanDesc = strPlanDescription;

                //Group Data extension
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem[6];
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].key = "DepartureCity";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].value = originRcd;

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].key = "ArrivalCity";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].value = destinationRcd;

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].key = "MobileEmailAddrForPolicyHolder";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].value = objHeader.mobile_email;

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].key = "UseAsFirstPassenger";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].type = "System.String";
                
                if (bFirstSameContct == true)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].value = "Y";
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].value = "N";
                }

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].key = "Total_Insured";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].type = "System.Integer";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].value = (adult + child + infant).ToString();

                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].key = "PaymentTransactionDate";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].value = string.Format("{0:yyyy-MM-dd}", clientDt); // As Asia

                //Submit transaction
                AceInsurance.ACORD1 objACORDResponse = new AceInsurance.ACORD1();
                Library li = new Library();
                //string Test = li.Serialize(objACORD, true); 
                objACORDResponse = objAceSvc.GetTravelQuote(objACORD);
 
                if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.MsgStatus.MsgStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyQuoteInqRsMsgStatusMsgStatusCd.Success)
                {
                    bSuccess = true;
                    return String.Format("{0:0.00}", objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.PersPolicy.QuoteInfo.InsuredFullToBePaidAmt.Amt);
                }
                else
                {
                    bSuccess = false;
                    return objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.MsgStatus.MsgStatusDesc;
                }
                
            }
            catch(Exception ex)
            {
                Library li = new Library();
                bSuccess = false;
                return ex.Message;
            }
        }
        public string ACEPolicyRequest(BookingHeader objHeader,
                                        Passengers objPax,
                                        string strSpName,
                                        string strUrl,
                                        string strUserId,
                                        string strPassword,
                                        DateTime clientDt,
                                        string strLanguage,
                                        string strOrg,
                                        string strAppName,
                                        string strCompanyProductCd,
                                        string strGroupDestId,
                                        string strGroupDestDesc,
                                        string strGroupInsureId,
                                        string strGroupInsureDesc,
                                        string strGroupPlanId,
                                        string strPlanDescription,
                                        decimal dclPremium,
                                        string strDomestic,
                                        string strTravelPurpose,
                                        Int16 adult,
                                        Int16 child,
                                        Int16 infant,
                                        string departureDate,
                                        string returnDate,
                                        string originRcd,
                                        string destinationRcd,
                                        ref string strErrorCode,
                                        ref string strAceMessage)
        {
            
            try
            {
                if (objHeader == null)
                {
                    strErrorCode = "802";
                    strAceMessage = "No booking object found";
                    return string.Empty;
                }
                if (strDomestic == "OTA" && string.IsNullOrEmpty(strTravelPurpose) == true)
                {
                    strErrorCode = "803";
                    strAceMessage = "Purpose of travel is required";
                    return string.Empty;
                }
                else
                {
                    AceInsurance.ACORDService objAceSvc = new AceInsurance.ACORDService();

                    System.Text.StringBuilder strPolicyNumber = new System.Text.StringBuilder();

                    objAceSvc.Url = strUrl;
                    dclPremium = dclPremium / (adult + child + infant);

                    bool bFirstSameContct = FindFirstPax(objHeader, objPax);

                    for (int j = 0; j < objPax.Count; j++)
                    {

                        AceInsurance.ACORD2 objACORD = new AceInsurance.ACORD2();
                        //User Info
                        objACORD.SignonRq = new AceInsurance.ACORDSignonRq1();
                        objACORD.SignonRq.SignonPswd = new AceInsurance.ACORDSignonRqSignonPswd1();
                        objACORD.SignonRq.SignonPswd.CustId = new AceInsurance.ACORDSignonRqSignonPswdCustId1();
                        objACORD.SignonRq.SignonPswd.CustId.SPName = strSpName;
                        objACORD.SignonRq.SignonPswd.CustId.CustLoginId = strUserId;

                        //Security Info
                        objACORD.SignonRq.SignonPswd.CustPswd = new AceInsurance.ACORDSignonRqSignonPswdCustPswd1();
                        objACORD.SignonRq.SignonPswd.CustPswd.EncryptionTypeCd = AceInsurance.ACORDSignonRqSignonPswdCustPswdEncryptionTypeCd1.None;
                        objACORD.SignonRq.SignonPswd.CustPswd.Pswd = strPassword;

                        objACORD.SignonRq.ClientDt = clientDt;
                        objACORD.SignonRq.CustLangPref = strLanguage;

                        objACORD.SignonRq.ClientApp = new AceInsurance.ACORDSignonRqClientApp1();
                        objACORD.SignonRq.ClientApp.Org = strOrg;
                        objACORD.SignonRq.ClientApp.Name = strAppName;
                        objACORD.SignonRq.ClientApp.Version = 1.0M;

                        //Insurance request detail.
                        objACORD.InsuranceSvcRq = new AceInsurance.ACORDInsuranceSvcRq1();
                        objACORD.InsuranceSvcRq.RqUID = Guid.NewGuid().ToString();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRq();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.TransactionRequestDt = clientDt;

                        //Passenger information.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipal();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfo();

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo[2];

                        //Filled Primary Passenger information.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].id = "1";

                        ACEPassengerInfo(objHeader.firstname,
                                         objHeader.lastname,
                                         objHeader.title_rcd,
                                         objPax[0].date_of_birth,
                                         string.Empty,
                                         ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0],
                                         bFirstSameContct);

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfo();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryNameCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryNameCd.Alias;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryName();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.Surname = objHeader.lastname;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.GivenName = objHeader.firstname;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.TitlePrefix = objHeader.title_rcd;

                        //Filled list passenger information.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1].id = (j + 2).ToString();

                        ACEPassengerInfo(objPax[j].firstname,
                                         objPax[j].lastname,
                                         objPax[j].title_rcd,
                                         objPax[j].date_of_birth,
                                         objPax[j].gender_type_rcd,
                                         ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1],
                                         true);

                        //Address Information from contact infomation.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr[1];

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].id = "1";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].NameInfoRef = "1";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr1 = objHeader.address_line1;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr2 = objHeader.address_line2;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].City = objHeader.city;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].StateProv = objHeader.state;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].PostalCode = objHeader.zip_code.Replace("-", "");
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Country = objHeader.country_rcd;

                        //Communication information.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications[1];
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].id = "1";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].NameInfoRef = "1";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo[2];

                        for (int i = 0; i < 2; i++)
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo();
                            if (i == 0)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Telephone;
                                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_home;
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Cell;
                                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_mobile;
                            }

                        }

                        //Email Information.
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo[1];
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0].EmailAddr = objHeader.contact_email;

                        //Policy information
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicy();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.CompanyProductCd = strCompanyProductCd;

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyContractTerm();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.EffectiveDt = new DateTime(Convert.ToInt16(departureDate.Substring(0, 4)),
                                                                                                                        Convert.ToInt16(departureDate.Substring(4, 2)),
                                                                                                                        Convert.ToInt16(departureDate.Substring(6, 2)));

                        if (string.IsNullOrEmpty(returnDate) == false)
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = new DateTime(Convert.ToInt16(returnDate.Substring(0, 4)),
                                                                                                                            Convert.ToInt16(returnDate.Substring(4, 2)),
                                                                                                                            Convert.ToInt16(returnDate.Substring(6, 2)));
                        }
                        else
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = new DateTime(Convert.ToInt16(departureDate.Substring(0, 4)),
                                                                                                                            Convert.ToInt16(departureDate.Substring(4, 2)),
                                                                                                                            Convert.ToInt16(departureDate.Substring(6, 2)));
                        }

                        //Group destination Id
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Destination();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.RqUID = strGroupDestId;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.DestinationDesc = strGroupDestDesc;

                        //Group InsuredPackage
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_InsuredPackage();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.RqUID = strGroupInsureId;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.InsuredPackageDesc = strGroupInsureDesc;

                        //Group plan
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Plan();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.RqUID = strGroupPlanId;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.PlanDesc = strPlanDescription;

                        //Group Data extension
                        if (strDomestic == "OTA")
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[11];
                        }
                        else
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[10];
                        }


                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].key = "DepartureCity";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].value = originRcd;

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].key = "MobileEmailAddrForPolicyHolder";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].value = objHeader.mobile_email;

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].key = "UseAsFirstPassenger";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].type = "System.String";
                        if (bFirstSameContct == true)
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "Y";
                        }
                        else
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "N";
                        }

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].key = "BookingReference";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].value = objHeader.record_locator;

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].key = "BasePremium";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].type = "System.Double";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].value = string.Format("{0:0.00}", dclPremium); // Will be change if required.

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].key = "Total_Insured";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].type = "System.Integer";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].value = (adult + child + infant).ToString();

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].key = "PaymentTransactionDate";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", clientDt); // As Asia

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].key = "PremiumCurrency";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].value = objHeader.currency_rcd; // As Asia

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].key = "ArrivalCity";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].value = destinationRcd; // As Asia

                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].key = "Purpose";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].type = "System.String";

                        if (strDomestic == "DTA")
                        { objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = "2"; }
                        else
                        { objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = strTravelPurpose; }

                        if (strDomestic == "OTA")
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].key = "DestinationGovCode";
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].value = "AS"; // As Asia
                        }

                        //Submit transaction
                        AceInsurance.ACORD3 objACORDResponse = new AceInsurance.ACORD3();

                        Library li = new Library();
                        //string test = li.Serialize(objACORD, true);
                        objACORDResponse = objAceSvc.GetTravelPolicy(objACORD);

                        if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsMsgStatusMsgStatusCd.Success)
                        {
                            if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsPersPolicyPolicyStatusCd.Accepted)
                            {
                                strErrorCode = "000";
                                if (j != 0)
                                {
                                    strPolicyNumber.Append(",");
                                }
                                strPolicyNumber.Append("{");
                                strPolicyNumber.Append("\"PolicyNumber\":\"" + objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyNumber + "\",");
                                strPolicyNumber.Append("\"title_rcd\":\"" + objPax[j].title_rcd + "\",");
                                strPolicyNumber.Append("\"firstname\":\"" + objPax[j].firstname + "\",");
                                strPolicyNumber.Append("\"lastname\":\"" + objPax[j].lastname + "\",");
                                strPolicyNumber.Append("}");
                            }
                            else
                            {
                                strErrorCode = "800";
                                strAceMessage = "Request of policy is reject";
                            }

                        }
                        else
                        {
                            strErrorCode = "801";
                            strAceMessage = objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusDesc;
                        }
                        
                    }

                    if (strPolicyNumber.Length > 0)
                    {
                        return "[" + strPolicyNumber.ToString() + "]";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                
            }
            catch(Exception ex)
            {
                Library li = new Library();
            
                strErrorCode = "400";
                strAceMessage = ex.Message;

                return string.Empty;
            }
        }
        public DataSet GetBinRangeSearch(string strCardType, string strStatusCode, string token)
        {
            ServiceClient objClient = new ServiceClient();
            Booking objBooking = new Booking();

            if (string.IsNullOrEmpty(token))
            {
                objClient.objService = objService;
                return objClient.GetBinRangeSearch(strCardType, strStatusCode);
            }
            else
            {
                return objClient.GetSessionlessBinRangeSearch(strCardType, strStatusCode, token);
            }
            
        }
        
        // Start checking FOP and Agency Accout Balance
        /// <summary>
        /// Return Agency Account Balance
        /// </summary>
        /// <param name="strAgencyCode">String Agency Code</param>
        /// <param name="strUserAccountId">String User Account Id</param>
        /// <param name="bGetAgencyAccountBalance">result boolean can get Agency Account Balance</param>
        /// <returns>Decimal Agency Account Balance</returns>
        public decimal GetCurrentBalance(string strAgencyCode, string strUserAccountId, ref bool bGetAgencyAccountBalance)
        {
            decimal dbAgencyAccountBalance = 0;
            bool isGUID = DataHelper.IsGUID(string.Format("{0}", strUserAccountId));
            if (isGUID)
            {
                Guid user_account_id = new Guid(strUserAccountId);
                if (user_account_id == Guid.Empty) strUserAccountId = "";
            }
            else strUserAccountId = "";

            ServiceClient srvClient = new ServiceClient();
            srvClient.objService = objService;

            Agents objAgent = srvClient.GetAgencySessionProfile(strAgencyCode, strUserAccountId);
            
            if (objAgent == null)
            {
                bGetAgencyAccountBalance = false;
            }
            else
            {
                if (objAgent.Count <= 0)
                {
                    bGetAgencyAccountBalance = false;
                }
                else
                {
                    bGetAgencyAccountBalance = true;
                    dbAgencyAccountBalance = objAgent[0].agency_account - objAgent[0].booking_payment;
                }
            }
            return dbAgencyAccountBalance;
        }
        // End checking FOP and Agency Accout Balance

        public int calCalculateCreditCardFee(ref decimal feeAmount,
                                           ref decimal feeAmountIncl,
                                           Itinerary itinerary,
                                           decimal feePercentage,
                                           string strFeeLevel,
                                           short odFlag,
                                           short minimumFeeFlag,
                                           decimal saleAmount,
                                           Int16 adult,
                                           Int16 child,
                                           Int16 infant)
        {
            int Factor = 0;
            decimal dFee = 0;

            //Calculate fee factor
            int iCountSegment = 0;

            switch (strFeeLevel)
            {
                case "CPN":
                    if (odFlag != 0)
                    {
                        for (int i = 0; i < itinerary.Count; i++)
                        {
                            if (itinerary[i].origin_rcd.Equals(itinerary[i].od_origin_rcd))
                            {
                                iCountSegment = ++iCountSegment;
                            }
                        }

                        Factor = iCountSegment * (adult + child);
                    }
                    else
                    {
                        Factor = itinerary.Count * (adult + child);
                    }
                    break;
                case "PAX":
                    Factor = adult + child;
                    break;
                case "SEG":
                    if (odFlag != 0)
                    {
                        for (int i = 0; i < itinerary.Count; i++)
                        {
                            if (itinerary[i].origin_rcd.Equals(itinerary[i].od_origin_rcd))
                            {
                                iCountSegment = ++iCountSegment;
                            }
                        }

                        Factor = iCountSegment;
                    }
                    else
                    {
                        Factor = itinerary.Count;
                    }
                    break;
                case "PNR":
                    Factor = 1;
                    break;
            }

            //Calculate FormOfPaymentFee
            if (feePercentage != 0)
            {
                dFee = Math.Round((saleAmount / 100) * feePercentage, 2);
                if (feeAmountIncl == 0)
                { }
                else if (minimumFeeFlag == 0)
                {
                    if (dFee > feeAmountIncl)
                    {
                        dFee = feeAmountIncl;
                    }
                }
                else
                {
                    if (dFee < feeAmountIncl)
                    {
                        dFee = feeAmountIncl;
                    }
                }
                feeAmount = dFee;
                feeAmountIncl = dFee;
            }
            else
            {
                feeAmount = feeAmount * Factor;
                feeAmountIncl = feeAmountIncl * Factor;
            }
            return Factor;
        }
        public string PaymentMultipleForm(ref BookingHeader bookingHeader,
                                          ref Itinerary itinerary,
                                          ref Passengers passengers,
                                          ref Quotes quotes,
                                          ref Fees fees,
                                          ref Mappings mappings,
                                          ref Services services,
                                          ref Remarks remarks,
                                          ref Payments payments,
                                          ref Taxes taxes,
                                          Payments paymentsInput,
                                          string strRequestSource,
                                          string strLanguage,
                                          Guid userId,
                                          string ipAddress,
                                          string formOfPaymentFee,
                                          Int16 adult,
                                          Int16 child,
                                          Int16 infant,
                                          Guid bookingPaymentId)
        {

            Fees objFees = new Fees();
            Library objLi = new Library();
 
            XPathDocument xmlDoc;
            XPathNavigator nv;

            string strFeeLevel = string.Empty;
            
            decimal dFeeAmountIncl = 0;
            decimal OutStandingBalance = 0;
            string formOfPayment = string.Empty;
            string formOfPaymentSubtypeRcd = string.Empty;
            string result = string.Empty;
            
            OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
            if (OutStandingBalance > 0 && paymentsInput != null)
            {
                // Find High Fee
                for (int i = 0; i < paymentsInput.Count; i++)
                {
                    if (paymentsInput[i].fee_amount_incl > dFeeAmountIncl)
                    {
                        dFeeAmountIncl = paymentsInput[i].fee_amount_incl;
                        formOfPayment = paymentsInput[i].form_of_payment_rcd;
                        formOfPaymentSubtypeRcd = paymentsInput[i].form_of_payment_subtype_rcd;
                    }
                }

                dFeeAmountIncl = 0;
                //Clear Fee Before add the new one
                ClearFee(fees,
                        itinerary[0].booking_segment_id,
                        passengers[0].passenger_id,
                        formOfPaymentSubtypeRcd);

                //Fill payment fee.
                objFees.objService = objService;

                objFees.GetCreditCardFee(bookingHeader,
                                        passengers,
                                        itinerary,
                                        quotes,
                                        fees,
                                        payments,
                                        formOfPayment,
                                        formOfPaymentSubtypeRcd,
                                        bookingHeader.currency_rcd,
                                        userId,
                                        string.Empty,
                                        OutStandingBalance);


                //Add Fee Amount include.

                if (objFees != null && objFees.Count > 0)
                {
                    for (int j = 0; j < objFees.Count; j++)
                    {
                        dFeeAmountIncl = dFeeAmountIncl + objFees[j].fee_amount_incl;
                    }

                }

                OutStandingBalance = OutStandingBalance + dFeeAmountIncl;

                for (int i = 0; i < paymentsInput.Count; i++)
                {
                    if (paymentsInput[i].form_of_payment_rcd != "CC")
                    {
                        {
                            // Calculate voucher and add voucher information to payment object

                            paymentsInput[i].payment_due_date_time = DateTime.MinValue;
                            paymentsInput[i].payment_date_time = DateTime.Now;
                            paymentsInput[i].booking_payment_id = Guid.NewGuid();
                            paymentsInput[i].booking_id = bookingHeader.booking_id;


                            if (paymentsInput[i].payment_amount >= OutStandingBalance)
                            {
                                paymentsInput[i].document_amount = OutStandingBalance;
                                paymentsInput[i].payment_amount = OutStandingBalance;
                                paymentsInput[i].receive_payment_amount = OutStandingBalance;
                            }
                            else
                            {
                                paymentsInput[i].document_amount = paymentsInput[i].payment_amount;
                                paymentsInput[i].payment_amount = paymentsInput[i].payment_amount;
                                paymentsInput[i].receive_payment_amount = paymentsInput[i].payment_amount;
                            }


                            paymentsInput[i].currency_rcd = bookingHeader.currency_rcd;
                            paymentsInput[i].receive_currency_rcd = bookingHeader.currency_rcd;
                            paymentsInput[i].agency_code = bookingHeader.agency_code;
                            paymentsInput[i].payment_by = userId;
                            paymentsInput[i].create_by = userId;
                            paymentsInput[i].update_by = userId;
                            paymentsInput[i].create_date_time = DateTime.Now;
                            paymentsInput[i].client_profile_id = bookingHeader.client_profile_id;
                            paymentsInput[i].ip_address = ipAddress;

                            OutStandingBalance = OutStandingBalance - paymentsInput[i].payment_amount;
                        }
                        this.Add(paymentsInput[i]);
                    }

                    //Filled credit card payment.
                    if (paymentsInput[i].form_of_payment_rcd == "CC")
                    {

                        paymentsInput[i].payment_due_date_time = DateTime.MinValue;
                        paymentsInput[i].payment_date_time = DateTime.Now;

                        //Assing booking_payment_id.
                        //  If booking_payment_id in object booking is not null then assign this id
                        //  else booking_payment_id in object booking is null then generate the id and assign to payment object and keep in session as reference
                        if (bookingPaymentId.Equals(Guid.Empty))
                        {
                            bookingPaymentId = Guid.NewGuid();
                        }
                        paymentsInput[i].booking_payment_id = bookingPaymentId;
                        paymentsInput[i].booking_id = bookingHeader.booking_id;

                        if (paymentsInput[i].receive_payment_amount == 0)
                        {
                            paymentsInput[i].receive_payment_amount = OutStandingBalance;
                            paymentsInput[i].receive_currency_rcd = bookingHeader.currency_rcd;
                        }

                        paymentsInput[i].payment_amount = OutStandingBalance;
                        OutStandingBalance = OutStandingBalance - paymentsInput[i].payment_amount;

                        paymentsInput[i].currency_rcd = bookingHeader.currency_rcd;
                        paymentsInput[i].agency_code = bookingHeader.agency_code;
                        paymentsInput[i].payment_by = userId;
                        paymentsInput[i].create_by = userId;
                        paymentsInput[i].update_by = userId;
                        paymentsInput[i].create_date_time = DateTime.Now;
                        paymentsInput[i].client_profile_id = bookingHeader.client_profile_id;
                        paymentsInput[i].ip_address = ipAddress;
                      
                        this.Add(paymentsInput[i]);
                    }
                }

                if (objLi.ValidSave(bookingHeader, itinerary, passengers, mappings) == true && OutStandingBalance == 0)
                {
                    //Save Payment
                    ServiceClient objClient = new ServiceClient();
                    objClient.objService = objService;

                    foreach (Mapping mp in mappings)
                    {
                        mp.create_by = userId;
                        mp.create_date_time = DateTime.Now;
                        mp.update_by = userId;
                        mp.update_date_time = DateTime.Now;
                    }

                    //Save booking information
                    Booking objBooking = new Booking();
                    objBooking.objService = objService;
                    result = objClient.SaveBookingMultipleFormOfPayment(bookingHeader.booking_id.ToString(),
                                                                      bookingHeader,
                                                                      itinerary,
                                                                      passengers,
                                                                      remarks,
                                                                      this,
                                                                      mappings,
                                                                      services,
                                                                      taxes,
                                                                      fees,
                                                                      objFees,
                                                                      true,
                                                                      false,
                                                                      false,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      strRequestSource,
                                                                      strLanguage);

                    if (result.Length > 0)
                    {
                        //Get Xml String and return out to generate control
                        xmlDoc = new XPathDocument(new StringReader(result));
                        nv = xmlDoc.CreateNavigator();

                        if (nv.Select("Booking/BookingHeader").Count > 0)
                        {
                            //Get Xml String and return out to generate control
                            objLi.FillBooking(result,
                                              ref bookingHeader,
                                              ref passengers,
                                              ref itinerary,
                                              ref mappings,
                                              ref payments,
                                              ref remarks,
                                              ref taxes,
                                              ref quotes,
                                              ref fees,
                                              ref services);
                            //objBooking.formOfPaymentFee = string.Empty;
                        }
                    }
                }
            }
            return result;
        }

        public string ExternalPaymentAddPayment(string strBookingId,
                                                string strAgencyCode,
                                                string strFormOfPayment,
                                                string strCurrencyCode,
                                                decimal dAmount,
                                                string strFormOfPaymentSubtype,
                                                string strUserId,
                                                string strDocumentNumber,
                                                string strApprovalCode,
                                                string strRemark,
                                                string strLanguage,
                                                DateTime dtPayment,
                                                bool bReturnItinerary,
                                                string strToken)
        {
            if (string.IsNullOrEmpty(strToken) == false)
            {
                ServiceClient objClient = new ServiceClient();
                
                return objClient.SessionlessExternalPaymentAddPayment(strBookingId,
                                                                    strAgencyCode,
                                                                    strFormOfPayment,
                                                                    strCurrencyCode,
                                                                    dAmount,
                                                                    strFormOfPaymentSubtype,
                                                                    strUserId,
                                                                    strDocumentNumber,
                                                                    strApprovalCode,
                                                                    strRemark,
                                                                    strLanguage,
                                                                    dtPayment,
                                                                    bReturnItinerary,
                                                                    strToken);
            }
            return string.Empty;
        }

        public Booking SavePaymentRedirect(Guid bookingId, Payments paymentsInput)
        {
            Library objLi = new Library();
            Booking objBooking = new Booking();
            objBooking.objService = objService;

            BookingHeader bookingHeader = null;
            Itinerary itinerary = null;
            Passengers passengers = null;
            Quotes quotes = null;
            Fees fees = null;
            Fees objFees = null;
            Mappings mappings = null;
            Services services = null;
            Remarks remarks = null;
            Taxes taxes = null;
            Payments payments = new Payments();

            decimal dFeeAmountIncl = 0;
            string strBookingXML = string.Empty;

            if (paymentsInput != null && paymentsInput.Count > 0)
            {
                if (objBooking.BookingRead(bookingId) == true)
                {
                    bookingHeader = objBooking.BookingHeader;
                    itinerary = objBooking.Itinerary;
                    passengers = objBooking.Passengers;
                    quotes = objBooking.Quotes;
                    fees = objBooking.Fees;
                    mappings = objBooking.Mappings;
                    services = objBooking.Services;
                    remarks = objBooking.Remarks;
                    taxes = objBooking.Taxes;

                    if (objFees == null)
                    {
                        objFees = new Fees();
                        objFees.objService = objService;
                    }

                    #region For redirect payment gateway.
                    //Fill payment fee.
                    objFees.GetCreditCardFee(bookingHeader,
                                            passengers,
                                            itinerary,
                                            quotes,
                                            fees,
                                            payments,
                                            paymentsInput[0].form_of_payment_rcd,
                                            paymentsInput[0].form_of_payment_subtype_rcd,
                                            bookingHeader.currency_rcd,
                                            paymentsInput[0].update_by,
                                            "",
                                            0);
                    //Add Fee Amount include.
                    dFeeAmountIncl = 0;
                    if (objFees != null && objFees.Count > 0)
                    {
                        for (int i = 0; i < objFees.Count; i++)
                        {
                            dFeeAmountIncl = dFeeAmountIncl + objFees[i].fee_amount_incl;
                        }
                    }
                    //Fill Payment amount to payment object.
                    decimal OutStandingBalance = objLi.CalOutStandingBalance(quotes, fees, payments);
                    if (OutStandingBalance > 0)
                    {
                        paymentsInput[0].agency_code = bookingHeader.agency_code;
                        paymentsInput[0].receive_payment_amount = OutStandingBalance + dFeeAmountIncl;
                        paymentsInput[0].receive_currency_rcd = bookingHeader.currency_rcd;

                        paymentsInput[0].payment_amount = OutStandingBalance + dFeeAmountIncl;
                        paymentsInput[0].currency_rcd = bookingHeader.currency_rcd;
                    }

                    strBookingXML = SavePayment(objBooking,
                                                ref bookingHeader,
                                                ref itinerary,
                                                ref passengers,
                                                ref quotes,
                                                ref fees,
                                                ref objFees,
                                                ref mappings,
                                                ref services,
                                                ref remarks,
                                                ref paymentsInput,
                                                ref taxes);
                    System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(new StringReader(strBookingXML));
                    System.Xml.XPath.XPathNavigator nv = xmlDoc.CreateNavigator();
                    if (nv.Select("ErrorResponse/Error").Count == 0)
                    {
                        bookingHeader = null;
                        passengers.Clear();
                        itinerary.Clear();
                        mappings.Clear();
                        payments.Clear();
                        remarks.Clear();
                        taxes.Clear();
                        quotes.Clear();
                        fees.Clear();
                        services.Clear();

                        objLi.FillBooking(strBookingXML,
                                           ref bookingHeader,
                                           ref passengers,
                                           ref itinerary,
                                           ref mappings,
                                           ref payments,
                                           ref remarks,
                                           ref taxes,
                                           ref quotes,
                                           ref fees,
                                           ref services);

                        objBooking.Payments = payments;
                    }
                    else
                    {
                        return null;
                    }
                    #endregion
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

            return objBooking;
        }
        public void Sort(String SortBy, GenericComparer.SortOrderEnum SortOrder)
        {
            GenericComparer comparer = new GenericComparer();
            comparer.SortProperty = SortBy;
            comparer.SortOrder = SortOrder;

            InnerList.Sort(comparer);

        }
        #endregion

        #region Helper
        private Decimal ConvertToDecimal(string val)
        {
            Decimal ret = 0;

            if (val != "")
            {
                ret = Convert.ToDecimal(val);
            }
            return ret;
        }
        private Payments CopyPayment(Payments objPayments)
        {
            Payments ps = new Payments();

            foreach (Payment p in objPayments)
            {
                ps.Add(p);
            }

            return ps;
        }
        public void FindFee(XPathNavigator nf,
                            ref string strFeeLevel,
                            ref short sOdFlag,
                            ref short sMinimumFeeFlag,
                            ref decimal dFeePercentage,
                            ref decimal dFeeAmount,
                            ref decimal dFeeAmountIncl)
        {
            if (nf.SelectSingleNode("fee_level") != null)
            {
                strFeeLevel = nf.SelectSingleNode("fee_level").InnerXml;
            }
            else
            {
                strFeeLevel = string.Empty;
            }

            if (nf.SelectSingleNode("od_flag") != null)
            {
                sOdFlag = Convert.ToInt16(nf.SelectSingleNode("od_flag").InnerXml);
            }
            else
            {
                sOdFlag = 0;
            }

            if (nf.SelectSingleNode("minimum_fee_amount_flag") != null)
            {
                sMinimumFeeFlag = Convert.ToInt16(nf.SelectSingleNode("minimum_fee_amount_flag").InnerXml);
            }
            else
            {
                sMinimumFeeFlag = 0;
            }

            if (nf.SelectSingleNode("fee_percentage") != null)
            {
                dFeePercentage = Convert.ToDecimal(nf.SelectSingleNode("fee_percentage").InnerXml);
            }
            else
            {
                dFeePercentage = 0;
            }

            dFeeAmount = dFeeAmount + Convert.ToDecimal(nf.SelectSingleNode("fee_amount").InnerXml);
            dFeeAmountIncl = dFeeAmountIncl + Convert.ToDecimal(nf.SelectSingleNode("fee_amount_incl").InnerXml);
        }

        public void ClearFee(Fees fees, Guid bookingSegmentId, Guid passengerId, string feeRcd)
        {
            for (int i = 0; i < fees.Count; i++)
            {
                if (fees[i].passenger_id.Equals(passengerId) &
                    fees[i].booking_segment_id.Equals(bookingSegmentId) &
                    (string.IsNullOrEmpty(fees[i].fee_rcd) == false && fees[i].fee_rcd.Equals(feeRcd)))
                {
                    fees.RemoveAt(i);
                    break;
                }
            }
        }
        private bool ValidPaymentInput(Payment payment)
        {
            if (payment == null)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(payment.form_of_payment_rcd))
            {
                return false;
            }
            else
            {
                if (payment.form_of_payment_rcd.Equals("CC"))
                {
                    if(string.IsNullOrEmpty(payment.form_of_payment_subtype_rcd))
                    {
                        return false;
                    }
                    else if(string.IsNullOrEmpty(payment.name_on_card))
                    {
                        return false;
                    }
                    else if(string.IsNullOrEmpty(payment.document_number))
                    {
                        return false;
                    }
                    else if(string.IsNullOrEmpty(payment.currency_rcd))
                    {
                        return false;
                    }
                    else if (string.IsNullOrEmpty(payment.agency_code))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
        #region ACE Helper
        private void ACEPassengerInfo(string firstName,
                                      string lastName,
                                      string strTitle,
                                      DateTime dtDateOfBirth,
                                      string strGender,
                                      ref AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo objACEPax,
                                      bool bFirstSameContct)
        {
            objACEPax.PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
            objACEPax.PersonName.Surname = lastName;
            objACEPax.PersonName.GivenName = firstName;
            objACEPax.PersonName.TitlePrefix = strTitle.ToUpper();

            if (bFirstSameContct == true)
            {
                if (dtDateOfBirth != DateTime.MinValue)
                {
                    objACEPax.BirthDt = dtDateOfBirth;
                }
                else
                {
                    objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
                }
            }
            else
            {
                objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
            }
            

            if (string.IsNullOrEmpty(strGender) == false)
            {
                if (strGender.Equals("M"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.M;
                    objACEPax.GenderSpecified = true;
                }
                else if (strGender.Equals("F"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.F;
                    objACEPax.GenderSpecified = true;
                }
                else
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.U;
                    objACEPax.GenderSpecified = true;
                }
            }
            
        }
        private bool FindFirstPax(BookingHeader header, Passengers paxs)
        {
            if (header.title_rcd.ToUpper().Equals(paxs[0].title_rcd) & header.firstname.ToUpper().Equals(paxs[0].firstname) & header.lastname.ToUpper().Equals(paxs[0].lastname))
            {
                return true;
            }

            return false;
        }
        #endregion
    }
    
    public class GenericComparer : IComparer 
    {

        public enum SortOrderEnum
        {
            Ascending,
            Descending
        }   
        private string _Property = null;
        public string SortProperty
        {
            get { return _Property; }
            set { _Property = value; }
        }

        private SortOrderEnum _SortOrder = SortOrderEnum.Ascending;
        public SortOrderEnum SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }

        public int Compare(object x, object y)
        {
            Payment pi1;
            Payment pi2;

            if (x is Payment)
                pi1 = (Payment)x;
            else
                throw new ArgumentException("Object is not type PaymentInput.");

            if (y is Payment)
                pi2 = (Payment)y;
            else
                throw new ArgumentException("Object is not type PaymentInput.");

            if (this.SortOrder.Equals(SortOrderEnum.Ascending))
                return pi1.CompareTo(pi2, this.SortProperty);
            else
                return pi2.CompareTo(pi1, this.SortProperty);

        }
        
        #region IComparable Members

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}