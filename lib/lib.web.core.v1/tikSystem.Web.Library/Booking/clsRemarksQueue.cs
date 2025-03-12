using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml.XPath;

namespace tikSystem.Web.Library
{
    public class RemarksQueue : LibraryBase
    {
        string _token = string.Empty;
        public RemarksQueue()
        {
           
        }
        public RemarksQueue(string strToken)
        {
            _token = strToken;
        }
        public RemarkQueue this[int Index]
        {
            get { return (RemarkQueue)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(RemarkQueue Value)
        {
            return this.List.Add(Value);
        }

        #region Method
        public string GetQueueCount(string agencyCode, bool unassigned)
        {
            ServiceClient obj = new ServiceClient();
            string strQueueXML = string.Empty;

            obj.objService = objService;

            strQueueXML = obj.GetQueueCount(agencyCode, unassigned);

            obj.objService = null;

            return strQueueXML;
        }

        public void Fill(string strQueueXML)
        {
            if (string.IsNullOrEmpty(strQueueXML) == false)
            {
                using (StreamReader str = new StreamReader(strQueueXML))
                {
                    XPathDocument xmlDoc = new XPathDocument(str);
                    XPathNavigator nv = xmlDoc.CreateNavigator();

                    RemarkQueue b;
                    this.Clear();
                    foreach (XPathNavigator n in nv.Select("RemarksQueue/RemarkQueue"))
                    {
                        b = new RemarkQueue();

                        b.sort_sequence = XmlHelper.XpathValueNullToInt(n, "sort_sequence");
                        b.is_event = XmlHelper.XpathValueNullToByte(n, "is_event");
                        b.within_48 = XmlHelper.XpathValueNullToByte(n, "within_48");
                        b.remark_type_rcd = XmlHelper.XpathValueNullToEmpty(n, "remark_type_rcd");
                        b.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                        b.queue_count = XmlHelper.XpathValueNullToInt(n, "queue_count");

                        Add(b);
                        b = null;
                    }
                }
            }
        }
        #endregion
    }
}
