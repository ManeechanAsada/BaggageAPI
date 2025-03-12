using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.Caching;
using System.Xml.Xsl;

namespace tikSystem.Web.Library
{
    public class MailManagement : LibraryBase
    {
        #region Properties
        string _mailForm = string.Empty;
        public string MailForm
        {
            get { return _mailForm; }
            set { _mailForm = value; }
        }

        string _mailTo = string.Empty;
        public string MailTo
        {
            get { return _mailTo; }
            set { _mailTo = value; }
        }

        string _mailCC = string.Empty;
        public string MailCC
        {
            get { return _mailCC; }
            set { _mailCC = value; }
        }

        string _mailBcc = string.Empty;
        public string MailBcc
        {
            get { return _mailBcc; }
            set { _mailBcc = value; }
        }

        string _mailSubject = string.Empty;
        public string MailSubject
        {
            get { return _mailSubject; }
            set { _mailSubject = value; }
        }

        string _bookingID = string.Empty;
        public string BookingID
        {
            get { return _bookingID; }
            set { _bookingID = value; }
        }

        string _userAccountID = string.Empty;
        public string UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        string _ibeEngine = string.Empty;
        public string IBEEngine
        {
            get { return _ibeEngine; }
            set { _ibeEngine = value; }
        }

        string _languageRCD = string.Empty;
        public string LanguageRCD
        {
            get { return _languageRCD; }
            set { _languageRCD = value; }
        }

        bool _isUsePDF;
        public bool IsUsePDF
        {
            get { return _isUsePDF; }
            set { _isUsePDF = value; }
        }

        string _xslEmailPath = string.Empty;
        public string XslEmailPath
        {
            get { return _xslEmailPath; }
            set { _xslEmailPath = value; }
        }

        #endregion

        #region Methods

        public bool SendMailSmtp(string mailSMTPBody, string SMTPServer)
        {
            bool bResult = false;
            if (SMTPServer.Length > 0)
            {
                MailMessage message = null;
                try
                {
                    // Command line argument must the the SMTP host.
                    SmtpClient client = new SmtpClient(SMTPServer);
                    MailAddress from = new MailAddress(MailForm,
                                                       MailForm,
                                                       System.Text.Encoding.UTF8);

                    MailAddress to = new MailAddress(MailTo);
                    message = new MailMessage(from, to);

                    if (MailBcc.Length > 0)
                    { message.Bcc.Add(MailBcc); }

                    if (MailCC.Length > 0)
                    { message.CC.Add(MailCC); }

                    message.IsBodyHtml = true;
                    message.Body = mailSMTPBody;

                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = MailSubject;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    client.Send(message);
                    bResult = true;
                }
                catch
                {
                    bResult = false;
                }
                finally
                {
                    if (message != null)
                    {
                        // Clean up.
                        message.Dispose();
                    }
                }
            }
            else
            { bResult = false; }

            return bResult;
        }

        private bool SendQueueMail(string strBody, string attachBody, bool IsHTMLBody, bool isUsePDF)
        {
            try
            {
                bool sendResult = false;

                tikSystem.Web.Library.ServiceClient objClient = new tikSystem.Web.Library.ServiceClient();
                objClient.objService = objService;

                if (strBody.Length > 0 && MailForm.Length > 0 && MailTo.Length > 0 && MailSubject.Length > 0)
                {
                    sendResult = objClient.QueueMail(MailForm,
                                       MailForm,
                                       MailTo,
                                       MailCC,
                                       MailBcc, string.Empty,
                                       MailSubject,
                                       strBody, "ITIN", attachBody, string.Empty, string.Empty, string.Empty,
                                       IsHTMLBody,
                                       isUsePDF,
                                       false,
                                       UserAccountID,
                                       BookingID,
                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                objClient.objService = null;
                objClient = null;
                return sendResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendItineraryMail()
        {
            try
            {
                bool result = false;
                string xmlBooking = string.Empty;
                string bookingLanguage = string.Empty;
                string xslMailFile = string.Empty;
                string xslAttachMailFile = string.Empty;
                string xslMobileFile = string.Empty;
                string mobileEmail = string.Empty;
                XmlDocument xmlDoc;
                XPathNavigator nv;
                string strBody = string.Empty;
                string strAttachBody = string.Empty;
                string strMobileBody = string.Empty;
                string recordLocator = string.Empty;
                Library li = new Library();
                XslTransform objTransform = null;
                XslTransform objTransformAttach = null;
                XslTransform objTransformMobile = null;

                tikSystem.Web.Library.ServiceClient objClient = new tikSystem.Web.Library.ServiceClient();
                objClient.objService = objService;

                xmlBooking = objClient.GetItinerary(BookingID, LanguageRCD, string.Empty, string.Empty);
                if (!string.IsNullOrEmpty(xmlBooking))
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlBooking);
                    nv = xmlDoc.CreateNavigator();
                    bookingLanguage = LanguageRCD; //nv.SelectSingleNode("Booking/Header/BookingHeader/language_rcd").InnerXml;

                    if (MailTo == string.Empty && nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email") != null)
                        MailTo = nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email").InnerXml;

                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email_cc") != null)
                        MailCC = nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email_cc").InnerXml;

                    //Get Mobile Email
                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/mobile_email") != null)
                        mobileEmail = nv.SelectSingleNode("Booking/Header/BookingHeader/mobile_email").InnerXml;

                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/record_locator") != null)
                        recordLocator = nv.SelectSingleNode("Booking/Header/BookingHeader/record_locator").InnerXml;

                    MailSubject = MailSubject.Replace("{PNR}", recordLocator);


                    xslMailFile = XslEmailPath + bookingLanguage + "_email_body.xsl";
                    
                    if (!System.IO.File.Exists(xslMailFile))
                    {
                        xslMailFile = XslEmailPath + bookingLanguage + "_email_" + IBEEngine + ".xsl";
                        if (!System.IO.File.Exists(xslMailFile)) xslMailFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";
                    }

                    objTransform = GetXSLDocument(xslMailFile);
                    
                    strBody = li.RenderHtml(objTransform, null, xmlBooking);
                    strAttachBody = li.RenderHtml(objTransformAttach, null, xmlBooking);
                    
                    if (IsUsePDF)
                    {
                        xslAttachMailFile = XslEmailPath + bookingLanguage + "_email_" + IBEEngine + ".xsl";
                        if (!System.IO.File.Exists(xslAttachMailFile)) xslAttachMailFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";
                        objTransformAttach = GetXSLDocument(xslAttachMailFile);
                        strAttachBody = li.RenderHtml(objTransformAttach, null, xmlBooking);
                    }

                    result = SendQueueMail(strBody, strAttachBody, true, IsUsePDF);

                    //Mobile Email
                    if (mobileEmail != string.Empty)
                    {
                        MailTo = mobileEmail;
                        xslMobileFile = XslEmailPath + bookingLanguage + "_email_b2m.xsl";
                        if (!System.IO.File.Exists(xslMobileFile)) xslMobileFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";

                        objTransformMobile = GetXSLDocument(xslMobileFile);
                        strMobileBody = li.RenderHtml(objTransformMobile, null, xmlBooking);
                        result = SendQueueMail(strMobileBody, string.Empty, false, false);

                    }
                }

                //Dispose Object
                xmlBooking = string.Empty;
                bookingLanguage = string.Empty;
                xslMailFile = string.Empty;
                xslAttachMailFile = string.Empty;
                xslMobileFile = string.Empty;
                mobileEmail = string.Empty;
                strBody = string.Empty;
                strAttachBody = string.Empty;
                strMobileBody = string.Empty;
                recordLocator = string.Empty;
                li = null;
                objTransform = null;
                objTransformAttach = null;
                objTransformMobile = null;
                xmlDoc = null;
                nv = null;
                objClient = null;

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool SendMaintainMail(string strHMTLBody)
        {
            try
            {
                bool result = false;
                tikSystem.Web.Library.ServiceClient objClient = new tikSystem.Web.Library.ServiceClient();
                objClient.objService = objService;

                result = objClient.QueueMail(MailForm,
                                        MailForm,
                                        MailTo,
                                        MailCC,
                                        MailBcc, string.Empty,
                                        MailSubject,
                                        strHMTLBody, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                        true,
                                        false,
                                        false,
                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public XslTransform GetXSLDocument(string filePath)
        {
            try
            {
                XslTransform xslTransform = HttpRuntime.Cache[filePath] as XslTransform;
                if (xslTransform == null)
                {
                    xslTransform = new XslTransform();
                    xslTransform.Load(filePath);
                    HttpRuntime.Cache.Insert(filePath, xslTransform, new CacheDependency(filePath));
                }

                return xslTransform;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendItineraryMailWithData(string showLogo)
        {
            try
            {
                bool result = false;
                string xmlBooking = string.Empty;
                string bookingLanguage = string.Empty;
                string xslMailFile = string.Empty;
                string xslAttachMailFile = string.Empty;
                string xslMobileFile = string.Empty;
                string mobileEmail = string.Empty;
                XmlDocument xmlDoc;
                XPathNavigator nv;
                string strBody = string.Empty;
                string strAttachBody = string.Empty;
                string strMobileBody = string.Empty;
                string recordLocator = string.Empty;
                Library li = new Library();
                XslTransform objTransform = null;
                XslTransform objTransformAttach = null;
                XslTransform objTransformMobile = null;

                tikSystem.Web.Library.ServiceClient objClient = new tikSystem.Web.Library.ServiceClient();
                objClient.objService = objService;

                xmlBooking = objClient.GetItinerary(BookingID, LanguageRCD, string.Empty, string.Empty);
                if (!string.IsNullOrEmpty(xmlBooking))
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlBooking);
                    nv = xmlDoc.CreateNavigator();
                    bookingLanguage = nv.SelectSingleNode("Booking/Header/BookingHeader/language_rcd").InnerXml;

                    if (MailTo == string.Empty && nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email") != null)
                        MailTo = nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email").InnerXml;

                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email_cc") != null)
                        MailCC = nv.SelectSingleNode("Booking/Header/BookingHeader/contact_email_cc").InnerXml;

                    //Get Mobile Email
                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/mobile_email") != null)
                        mobileEmail = nv.SelectSingleNode("Booking/Header/BookingHeader/mobile_email").InnerXml;

                    if (nv.SelectSingleNode("Booking/Header/BookingHeader/record_locator") != null)
                        recordLocator = nv.SelectSingleNode("Booking/Header/BookingHeader/record_locator").InnerXml;

                    MailSubject = MailSubject.Replace("{PNR}", recordLocator);


                    xslMailFile = XslEmailPath + bookingLanguage + "_email_body.xsl";

                    if (!System.IO.File.Exists(xslMailFile))
                    {
                        xslMailFile = XslEmailPath + bookingLanguage + "_email_" + IBEEngine + ".xsl";
                        if (!System.IO.File.Exists(xslMailFile)) xslMailFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";
                    }

                    objTransform = GetXSLDocument(xslMailFile);

                    //showLogo
                    xmlBooking = xmlBooking.Replace("</Booking>", "<Logo><showLogo>" + showLogo + "</showLogo></Logo></Booking>");

                    strBody = li.RenderHtml(objTransform, null, xmlBooking);
                    strAttachBody = li.RenderHtml(objTransformAttach, null, xmlBooking);

                    if (IsUsePDF)
                    {
                        xslAttachMailFile = XslEmailPath + bookingLanguage + "_email_" + IBEEngine + ".xsl";
                        if (!System.IO.File.Exists(xslAttachMailFile)) xslAttachMailFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";
                        objTransformAttach = GetXSLDocument(xslAttachMailFile);
                        strAttachBody = li.RenderHtml(objTransformAttach, null, xmlBooking);
                    }

                    result = SendQueueMail(strBody, strAttachBody, true, IsUsePDF);

                    //Mobile Email
                    if (mobileEmail != string.Empty)
                    {
                        MailTo = mobileEmail;
                        xslMobileFile = XslEmailPath + bookingLanguage + "_email_b2m.xsl";
                        if (!System.IO.File.Exists(xslMobileFile)) xslMobileFile = XslEmailPath + "en_email_" + IBEEngine + ".xsl";

                        objTransformMobile = GetXSLDocument(xslMobileFile);
                        strMobileBody = li.RenderHtml(objTransformMobile, null, xmlBooking);
                        result = SendQueueMail(strMobileBody, string.Empty, false, false);

                    }
                }

                //Dispose Object
                xmlBooking = string.Empty;
                bookingLanguage = string.Empty;
                xslMailFile = string.Empty;
                xslAttachMailFile = string.Empty;
                xslMobileFile = string.Empty;
                mobileEmail = string.Empty;
                strBody = string.Empty;
                strAttachBody = string.Empty;
                strMobileBody = string.Empty;
                recordLocator = string.Empty;
                li = null;
                objTransform = null;
                objTransformAttach = null;
                objTransformMobile = null;
                xmlDoc = null;
                nv = null;
                objClient = null;

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}
