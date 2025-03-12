using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;

namespace tikSystem.Web.Library
{
    public class Documents : LibraryBase
    {
        public Document this[int Index]
        {
            get { return (Document)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Document Value)
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

                    xtw.WriteStartElement("DocumentTypes");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("DocumentType");
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
                            xtw.WriteStartElement("DocumentType");
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
        private void GetXMLElement(System.Xml.XmlTextWriter xtw, Document document) 
        {
            xtw.WriteStartElement("document_type_rcd");
            xtw.WriteValue(document.document_type_rcd);
            xtw.WriteEndElement();

            xtw.WriteStartElement("display_name");
            xtw.WriteValue(document.display_name);
            xtw.WriteEndElement();
        }
        #endregion
    }
}
