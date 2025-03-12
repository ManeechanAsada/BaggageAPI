using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Text.RegularExpressions;

namespace tikSystem.Web.Library
{
    public class Languages : LibraryBase, IDisposable
    {
        public Language this[int Index]
        {
            get { return (Language)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Language Value)
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

                    xtw.WriteStartElement("Languages");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("Language");
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
                            xtw.WriteStartElement("Languages");
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
        private void GetXMLElement(System.Xml.XmlTextWriter xtw, Language language)
        {
            if (string.IsNullOrEmpty(language.language_rcd) == false)
            {
                xtw.WriteStartElement("language_rcd");
                xtw.WriteValue(language.language_rcd);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(language.display_name) == false)
            {
                xtw.WriteStartElement("display_name");
                xtw.WriteValue(language.display_name);
                xtw.WriteEndElement();
            }

            if (string.IsNullOrEmpty(language.character_set) == false)
            {
                xtw.WriteStartElement("character_set");
                xtw.WriteValue(language.character_set);
                xtw.WriteEndElement();
            }
        }
        #endregion
        #region WEMS
        private DataSet _ds = null;
        public bool LoadLanguage(string ApplicationType, string strCulture)
        {
            MultiLanguage.ServiceEngine objLanguage = new MultiLanguage.ServiceEngine();
            _ds = objLanguage.ReadLanguage(ApplicationType, strCulture);
            objLanguage.Dispose();

            if (_ds != null && _ds.Tables.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public StringDictionary GetDictionary()
        {
            string a =string.Empty;
            StringDictionary sd = null;
            if (_ds != null)
            {
                sd = new StringDictionary();
                foreach (DataTable dt in _ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sd.Add(Regex.Replace(dr["name"].ToString(), @"[^\u0000-\u007F]", string.Empty), dr["value"].ToString());
                    }
                }
            }
            return sd;
        }
        public string GetWEMSXml()
        {
            return _ds.GetXml();
        }
        public string GetJson(string strQuery)
        {
            if (_ds.Tables[strQuery] != null)
            {
                StringBuilder stb = new StringBuilder();

                foreach (DataRow rw in _ds.Tables[strQuery].Rows)
                {
                    stb.Append("\"" + rw["name"].ToString() + "\" : \"" + rw["value"].ToString().Replace("'", "\\u0027").Replace("\r\n", "").Replace("\n", "<br/>").Replace("\\n", "<br/>") + "\",");
                }

                stb.Remove(stb.Length - 1, 1);
                return stb.ToString();
            }
            return string.Empty;
        }

        public DataSet ReadLanguageConfig(string applicationType)
        {
            MultiLanguage.ServiceEngine objLanguage = new MultiLanguage.ServiceEngine();
            DataSet ds = objLanguage.ReadConfig(applicationType);
            objLanguage.Dispose();

            return ds;
        }
        public string GetLanguageServiceUrl()
        {
            return tikSystem.Web.Library.Properties.Settings.Default.tikSystem_Web_Library_MultiLanguage_ServiceEngine;
        }
        #endregion
        #region IDisposable Members

        public void Dispose()
        {
            if (_ds != null)
            {
                _ds.Dispose();
            }
        }

        #endregion
    }
}
