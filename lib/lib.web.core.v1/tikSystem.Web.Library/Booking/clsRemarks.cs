using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Remarks : LibraryBase
    {
        public Remark this[int index]
        {
            get { return (Remark)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Remark Value)
        {
            return this.List.Add(Value);
        }

        #region Method
        public bool HasRemark(Remark remark, bool updateChange)
        {
            bool hasRemark = false;
            foreach (Remark r in this)
            {
                if (r.remark_type_rcd.ToUpper().Equals(remark.remark_type_rcd.ToUpper()))
                {
                    if (updateChange == true)
                    {
                        r.remark_text = remark.remark_text;
                    }
                    hasRemark = true;
                    break;
                }
            }
            return hasRemark;
        }
        public string RemarkAdd(Remark remark, Guid userId)
        {
            try
            {
                string strResult = string.Empty;
                ServiceClient objSClient = new ServiceClient();
                objSClient.objService = objService;

                strResult = objSClient.RemarkAdd(remark.remark_type_rcd,
                                                remark.booking_remark_id.ToString(),
                                                remark.booking_id.ToString(),
                                                remark.client_profile_id.ToString(),
                                                remark.nickname,
                                                remark.remark_text,
                                                remark.agency_code,
                                                remark.added_by,
                                                userId.ToString(),
                                                Convert.ToBoolean(remark.protected_flag),
                                                Convert.ToBoolean(remark.warning_flag),
                                                Convert.ToBoolean(remark.process_message_flag),
                                                Convert.ToBoolean(remark.system_flag),
                                                remark.timelimit_date_time,
                                                remark.utc_timelimit_date_time);



                objSClient.objService = null;

                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool RemarkDelete(Guid bookingRemarkId)
        {
            try
            {
                bool bResult = false;

                ServiceClient objSClient = new ServiceClient();
                objSClient.objService = objService;

                bResult = objSClient.RemarkDelete(bookingRemarkId);

                objSClient.objService = null;

                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool RemarkComplete(Guid bookingRemarkId, Guid userId)
        {
            try
            {
                bool bResult = false;

                ServiceClient objSClient = new ServiceClient();
                objSClient.objService = objService;

                bResult = objSClient.RemarkComplete(bookingRemarkId, userId);

                objSClient.objService = null;

                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RemarkRead(string remarkId, 
                                string bookingId,
                                string bookingReference,
                                double bookingNumber,
                                bool readOnly)
        {
            try
            {
                string strResult = string.Empty;

                ServiceClient objSClient = new ServiceClient();
                objSClient.objService = objService;

                strResult = objSClient.RemarkRead(remarkId, bookingId, bookingReference, bookingNumber, readOnly);

                objSClient.objService = null;

                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemarkSave()
        {
            try
            {
                bool bResult = false;
                if (this.Count > 0)
                {
                    ServiceClient objSClient = new ServiceClient();
                    objSClient.objService = objService;

                    bResult = objSClient.RemarkSave(this);

                    objSClient.objService = null;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Fill(string xml)
        {
            if (string.IsNullOrEmpty(xml) == false)
            {
                using (System.IO.StreamReader str = new System.IO.StreamReader(xml))
                {
                    System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(str);
                    System.Xml.XPath.XPathNavigator nv = xmlDoc.CreateNavigator();

                    Remark rk;
                    this.Clear();
                    foreach (System.Xml.XPath.XPathNavigator n in nv.Select("RemarksQueue/RemarkQueue"))
                    {
                        rk = new Remark();

                        rk.booking_remark_id = XmlHelper.XpathValueNullToGUID(n, "booking_remark_id");
                        rk.remark_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "remark_type_rcd ");
                        rk.timelimit_date_time = XmlHelper.XpathValueNullToDateTime(n, "timelimit_date_time");
                        rk.remark_text = XmlHelper.XpathValueNullToEmpty(n, "remark_text");
                        rk.agency_code = XmlHelper.XpathValueNullToEmpty(n, "agency_code");
                        rk.booking_id = XmlHelper.XpathValueNullToGUID(n, "booking_id");
                        rk.added_by = XmlHelper.XpathValueNullToEmpty(n, "added_by");
                        rk.system_flag = XmlHelper.XpathValueNullToByte(n, "system_flag");
                        rk.utc_timelimit_date_time = XmlHelper.XpathValueNullToDateTime(n, "utc_timelimit_date_time");

                        Add(rk);
                        rk = null;
                    }
                }
            }
        }

        #endregion
    }


}
