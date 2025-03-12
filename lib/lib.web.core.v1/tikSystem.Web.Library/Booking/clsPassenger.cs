using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
namespace tikSystem.Web.Library
{
    public class Passenger : People
    {
        #region Properties
        Guid _passenger_id;
        public Guid passenger_id
        {
            get { return _passenger_id; }
            set { _passenger_id = value; }
        }
        Guid _booking_id;
        public Guid booking_id
        {
            get { return _booking_id; }
            set { _booking_id = value; }
        }
        Guid _passenger_profile_id;
        public Guid passenger_profile_id
        {
            get { return _passenger_profile_id; }
            set { _passenger_profile_id = value; }
        }
        string _passenger_status_rcd;
        public string passenger_status_rcd
        {
            get { return _passenger_status_rcd; }
            set { _passenger_status_rcd = value; }
        }
        string _employee_number = string.Empty;
        public string employee_number
        {
            get { return _employee_number; }
            set { _employee_number = value; }
        }

        byte _wheelchair_flag;
        public byte wheelchair_flag
        {
            get { return _wheelchair_flag; }
            set { _wheelchair_flag = value; }
        }
        byte _vip_flag;
        public byte vip_flag
        {
            get { return _vip_flag; }
            set { _vip_flag = value; }
        }
       
        string _passenger_role_rcd;
        public string passenger_role_rcd
        {
            get { return _passenger_role_rcd; }
            set { _passenger_role_rcd = value; }
        }
        string _member_level_rcd = string.Empty;
        public string member_level_rcd
        {
            get { return _member_level_rcd; }
            set { _member_level_rcd = value; }
        }
        string _member_number = string.Empty;
        public string member_number
        {
            get { return _member_number; }
            set { _member_number = value; }
        }
        byte _window_seat_flag;
        public byte window_seat_flag
        {
            get { return _window_seat_flag; }
            set { _window_seat_flag = value; }
        }
        string _company_phone_home;
        public string company_phone_home
        {
            get { return _company_phone_home; }
            set { _company_phone_home = value; }
        }
        string _company_phone_business;
        public string company_phone_business
        {
            get { return _company_phone_business; }
            set { _company_phone_business = value; }
        }
        string _company_phone_mobile;
        public string company_phone_mobile
        {
            get { return _company_phone_mobile; }
            set { _company_phone_mobile = value; }
        }
        string _redress_number = string.Empty;
        public string redress_number
        {
            get { return _redress_number; }
            set { _redress_number = value; }
        }
        string _pnr_name;
        public string pnr_name
        {
            get { return _pnr_name; }
            set { _pnr_name = value; }
        }

        string _nationality_display_name;
        public string nationality_display_name
        {
            get { return _nationality_display_name; }
            set { _nationality_display_name = value; }
        }

        string _passport_issue_country_display_name;
        public string passport_issue_country_display_name
        {
            get { return _passport_issue_country_display_name; }
            set { _passport_issue_country_display_name = value; }
        }
        string _known_traveler_number;
        public string known_traveler_number
        {
            get { return _known_traveler_number; }
            set { _known_traveler_number = value; }
        }
        
        #endregion

        #region Sort Function
        public int CompareTo(object obj, string Property)
        {
            try
            {
                Type type = this.GetType();
                System.Reflection.PropertyInfo propertie = type.GetProperty(Property);


                Type type2 = obj.GetType();
                System.Reflection.PropertyInfo propertie2 = type2.GetProperty(Property);

                object[] index = null;

                object Obj1 = propertie.GetValue(this, index);
                object Obj2 = propertie2.GetValue(obj, index);

                IComparable Ic1 = (IComparable)Obj1;
                IComparable Ic2 = (IComparable)Obj2;

                int returnValue = Ic1.CompareTo(Ic2);

                return returnValue;

            }
            catch
            {
                throw new ArgumentException("CompareTo is not possible !");
            }
        }
        #endregion
    }
}
