using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Route
    {
        string _origin_rcd;
        public string origin_rcd
        {
            get { return _origin_rcd; }
            set { _origin_rcd = value; }
        }
        string _destination_rcd;
        public string destination_rcd
        {
            get { return _destination_rcd; }
            set { _destination_rcd = value; }
        }
        string _display_name;
        public string display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
        Guid _create_by;
        public Guid create_by
        {
            get { return _create_by; }
            set { _create_by = value; }
        }
        Guid _update_by;
        public Guid update_by
        {
            get { return _update_by; }
            set { _update_by = value; }
        }
        DateTime _create_date_time;
        public DateTime create_date_time
        {
            get { return _create_date_time; }
            set { _create_date_time = value; }
        }
        DateTime _update_date_time;
        public DateTime update_date_time
        {
            get { return _update_date_time; }
            set { _update_date_time = value; }
        }
        #region Origin Information
        string _country_rcd;
        public string country_rcd
        {
            get { return _country_rcd; }
            set { _country_rcd = value; }
        }
        #endregion
        #region Origin Flag
        string _currency_rcd;
        public string currency_rcd
        {
            get { return _currency_rcd; }
            set { _currency_rcd = value; }
        }
        Int16 _routes_tot;
        public Int16 routes_tot
        {
            get { return _routes_tot; }
            set { _routes_tot = value; }
        }
        Int16 _routes_avl;
        public Int16 routes_avl
        {
            get { return _routes_avl; }
            set { _routes_avl = value; }
        }
        Int16 _routes_b2c;
        public Int16 routes_b2c
        {
            get { return _routes_b2c; }
            set { _routes_b2c = value; }
        }

        Int16 _routes_b2b;
        public Int16 routes_b2b
        {
            get { return _routes_b2b; }
            set { _routes_b2b = value; }
        }
        Int16 _routes_b2s;
        public Int16 routes_b2s
        {
            get { return _routes_b2s; }
            set { _routes_b2s = value; }
        }
        Int16 _routes_api;
        public Int16 routes_api
        {
            get { return _routes_api; }
            set { _routes_api = value; }
        }
        Int16 _routes_b2t;
        public Int16 routes_b2t
        {
            get { return _routes_b2t; }
            set { _routes_b2t = value; }
        }
        #endregion
        #region Destination Flag
        bool _segment_change_fee_flag;
        public bool segment_change_fee_flag
        {
            get { return _segment_change_fee_flag; }
            set { _segment_change_fee_flag = value; }
        }
        bool _transit_flag;
        public bool transit_flag
        {
            get { return _transit_flag; }
            set { _transit_flag = value; }
        }
        bool _direct_flag;
        public bool direct_flag
        {
            get { return _direct_flag; }
            set { _direct_flag = value; }
        }
        bool _avl_flag;
        public bool avl_flag
        {
            get { return _avl_flag; }
            set { _avl_flag = value; }
        }
        bool _b2c_flag;
        public bool b2c_flag
        {
            get { return _b2c_flag; }
            set { _b2c_flag = value; }
        }
        bool _b2b_flag;
        public bool b2b_flag
        {
            get { return _b2b_flag; }
            set { _b2b_flag = value; }
        }
        bool _b2t_flag;
        public bool b2t_flag
        {
            get { return _b2t_flag; }
            set { _b2t_flag = value; }
        }
        Int16 _day_range;
        public Int16 day_range
        {
            get { return _day_range; }
            set { _day_range = value; }
        }
        bool _show_redress_number_flag;
        public bool show_redress_number_flag
        {
            get { return _show_redress_number_flag; }
            set { _show_redress_number_flag = value; }
        }
        bool _require_passenger_title_flag;
        public bool require_passenger_title_flag
        {
            get { return _require_passenger_title_flag; }
            set { _require_passenger_title_flag = value; }
        }
        bool _require_passenger_gender_flag;
        public bool require_passenger_gender_flag
        {
            get { return _require_passenger_gender_flag; }
            set { _require_passenger_gender_flag = value; }
        }
        bool _require_date_of_birth_flag;
        public bool require_date_of_birth_flag
        {
            get { return _require_date_of_birth_flag; }
            set { _require_date_of_birth_flag = value; }
        }
        bool _require_document_details_flag;
        public bool require_document_details_flag
        {
            get { return _require_document_details_flag; }
            set { _require_document_details_flag = value; }
        }
        bool _require_passenger_weight_flag;
        public bool require_passenger_weight_flag
        {
            get { return _require_passenger_weight_flag; }
            set { _require_passenger_weight_flag = value; }
        }
        bool _special_service_fee_flag;
        public bool special_service_fee_flag
        {
            get { return _special_service_fee_flag; }
            set { _special_service_fee_flag = value; }
        }
        bool _show_insurance_on_web_flag;
        public bool show_insurance_on_web_flag
        {
            get { return _show_insurance_on_web_flag; }
            set { _show_insurance_on_web_flag = value; }
        }
        #endregion
    }
}
