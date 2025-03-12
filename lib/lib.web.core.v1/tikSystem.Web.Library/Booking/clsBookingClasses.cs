using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace tikSystem.Web.Library
{
    public class BookingClasses : LibraryBase
    {
        public BookingClass this[int Index]
        {
            get { return (BookingClass)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(BookingClass Value)
        {
            return this.List.Add(Value);
        }

        #region Method
        public void GetBookingClass(string strBoardingclass, string language)
        {
            this.Clear();

            int iConfigValue = 0;
            if (ConfigurationManager.AppSettings["Service"] != null)
            {
                iConfigValue = Convert.ToInt16(ConfigurationManager.AppSettings["Service"]);
            }

            switch (iConfigValue)
            {
                case 1:
                    //Old Web service
                    GetBookingClasses(strBoardingclass, language);
                    break;
                default:
                    //new Web service
                    GetBookingClassesWs(strBoardingclass, language);
                    break;
            }
        }

        public string GetXml()
        {
            if (this.Count > 0)
            {
                StringBuilder stb = new StringBuilder();
                using (System.IO.StringWriter stw = new System.IO.StringWriter(stb))
                {
                    System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(stw);

                    xtw.WriteStartElement("BookingClass");
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            xtw.WriteStartElement("BookingClasses");
                            {
                                if (string.IsNullOrEmpty(this[i].booking_class_rcd) == false)
                                {
                                    xtw.WriteStartElement("booking_class_rcd");
                                    xtw.WriteValue(this[i].booking_class_rcd);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].display_name) == false)
                                {
                                    xtw.WriteStartElement("display_name");
                                    xtw.WriteValue(this[i].display_name);
                                    xtw.WriteEndElement();
                                }

                                if (string.IsNullOrEmpty(this[i].boarding_class_rcd) == false)
                                {
                                    xtw.WriteStartElement("boarding_class_rcd");
                                    xtw.WriteValue(this[i].boarding_class_rcd);
                                    xtw.WriteEndElement();
                                }

                                xtw.WriteStartElement("sort_sequence");
                                xtw.WriteValue(this[i].sort_sequence);
                                xtw.WriteEndElement();

                                if (string.IsNullOrEmpty(this[i].nesting_string) == false)
                                {
                                    xtw.WriteStartElement("nesting_string");
                                    xtw.WriteValue(this[i].nesting_string);
                                    xtw.WriteEndElement();
                                }


                                xtw.WriteStartElement("sales_notification_percentage");
                                xtw.WriteValue(this[i].sales_notification_percentage);
                                xtw.WriteEndElement();


                                xtw.WriteStartElement("cancel_notification_percentage");
                                xtw.WriteValue(this[i].cancel_notification_percentage);
                                xtw.WriteEndElement();



                                xtw.WriteStartElement("controlling_class_flag");
                                xtw.WriteValue(this[i].controlling_class_flag);
                                xtw.WriteEndElement();


                                xtw.WriteStartElement("length_nesting_string");
                                xtw.WriteValue(this[i].length_nesting_string);
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

        #region Old Web Service (This section will be remove when the new one is stable)
        private void GetBookingClasses(string strBoardingclass, string language)
        {
            try
            {
                if (objService != null)
                {
                    System.Data.DataSet ds = objService.GetBookingClasses(strBoardingclass, language);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        BookingClass b;
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            b = new BookingClass();

                            b.booking_class_rcd = DataHelper.DBToString(dr, "booking_class_rcd");
                            b.display_name = DataHelper.DBToString(dr, "display_name");
                            b.boarding_class_rcd = DataHelper.DBToString(dr, "boarding_class_rcd");
                            b.sort_sequence = DataHelper.DBToInt16(dr, "sort_sequence");
                            b.nesting_string = DataHelper.DBToString(dr, "nesting_string");
                            b.sales_notification_percentage = DataHelper.DBToDecimal(dr, "sales_notification_percentage");
                            b.cancel_notification_percentage = DataHelper.DBToDecimal(dr, "cancel_notification_percentage");
                            b.controlling_class_flag = DataHelper.DBToByte(dr, "controlling_class_flag");
                            b.length_nesting_string = DataHelper.DBToInt16(dr, "length_nesting_string");

                            Add(b);
                            b = null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region New Webservice call
        private void GetBookingClassesWs(string strBoardingclass, string language)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
