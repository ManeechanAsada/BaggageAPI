using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Runtime.InteropServices;

namespace tikSystem.Web.Library
{
    public class Titles : LibraryBase
    {
        public Title this[int Index]
        {
            get { return (Title)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Title Value)
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

                    xtw.WriteStartElement("NewDataSet");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("PassengerTitels");
                            {
                                xtw.WriteStartElement("title_rcd");
                                xtw.WriteValue(this[i].title_rcd);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("display_name");
                                xtw.WriteValue(this[i].display_name);
                                xtw.WriteEndElement();

                                xtw.WriteStartElement("gender_type_rcd");
                                xtw.WriteValue(this[i].gender_type_rcd);
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
        #endregion
    }
}
