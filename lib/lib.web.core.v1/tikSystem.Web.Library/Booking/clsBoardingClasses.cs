using System;
using System.Collections;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml.Linq;

namespace tikSystem.Web.Library
{
    public class BoardingClasses : LibraryBase
    {
        string _token = string.Empty;
        public BoardingClasses(string strToken)
        {
            _token = strToken;
        }
        public BoardingClass this[int Index]
        {
            get { return (BoardingClass)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(BoardingClass Value)
        {
            return this.List.Add(Value);
        }

        #region Boarding Class
        public void GetBoardingClass(string boardingClassCode, string boardingClass, string sortSeq, string status, bool bWrite)
        {
            try
            {
                ServiceClient objClient = new ServiceClient();
                string xml = objClient.BoardingClassRead(boardingClassCode, boardingClass, sortSeq, status, bWrite);
                if (string.IsNullOrEmpty(xml) == false)
                {
                    //using (StreamReader str = new StreamReader(strBoardingClassXML))
                    //{
                    //    XPathDocument xmlDoc = new XPathDocument(str);
                    //    XPathNavigator nv = xmlDoc.CreateNavigator();

                    //    BoardingClass b;
                    //    foreach (XPathNavigator n in nv.Select("BoardingClasses/BoardingClass"))
                    //    {
                    //        b = new BoardingClass();

                    //        b.boarding_class_rcd = XmlHelper.XpathValueNullToEmpty(n, "boarding_class_rcd");
                    //        b.display_name = XmlHelper.XpathValueNullToEmpty(n, "display_name");
                    //        b.status_code = XmlHelper.XpathValueNullToEmpty(n, "status_code");
                    //        b.create_date_time = XmlHelper.XpathValueNullToDateTime(n, "create_date_time");
                    //        b.create_by = XmlHelper.XpathValueNullToGUID(n, "create_by");
                    //        b.update_date_time = XmlHelper.XpathValueNullToDateTime(n, "update_date_time");
                    //        b.update_by = XmlHelper.XpathValueNullToGUID(n, "update_by");
                    //        b.create_name = XmlHelper.XpathValueNullToEmpty(n, "create_name");
                    //        b.update_name = XmlHelper.XpathValueNullToEmpty(n, "update_name");

                    //        Add(b);
                    //        b = null;
                    //    }

                    BoardingClass b;

                    XElement elements = XElement.Load(xml);
                    foreach (XElement x in elements.Elements("BoardingClasses/BoardingClass"))
                    {
                        DateTime create_date_time = DateTime.MinValue;
                        DateTime update_date_time = DateTime.MinValue;
                        Guid create_by = Guid.Empty;
                        Guid update_by = Guid.Empty;

                        DateTime.TryParse(x.Element("create_date_time").Value, out create_date_time);
                        DateTime.TryParse(x.Element("update_date_time").Value, out update_date_time);

                        if (DataHelper.IsGUID(x.Element("create_by").Value)) create_by = new Guid(x.Element("create_by").Value);
                        if (DataHelper.IsGUID(x.Element("update_by").Value)) update_by = new Guid(x.Element("update_by").Value);

                        b = new BoardingClass()
                        {
                            boarding_class_rcd = x.Element("boarding_class_rcd").Value,
                            display_name = x.Element("display_name").Value,
                            status_code = x.Element("status_code").Value,
                            create_date_time = create_date_time,
                            create_by = create_by,
                            update_date_time = update_date_time,
                            update_by = update_by,
                            create_name = x.Element("create_name").Value,
                            update_name = x.Element("update_name").Value
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
