using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APISeatMap
    {
        #region Property
        Guid _flight_id = Guid.Empty;
        public Guid flight_id
        {
            get { return _flight_id; }
            set { _flight_id = value; }
        }

        int _free_seating_flag;
        public int free_seating_flag
        {
            get { return _free_seating_flag; }
            set { _free_seating_flag = value; }
        }

        string _flight_check_in_status_rcd;
        public string flight_check_in_status_rcd
        {
            get { return _flight_check_in_status_rcd; }
            set { _flight_check_in_status_rcd = value; }
        }

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

        string _aircraft_configuration_code;
        public string aircraft_configuration_code
        {
            get { return _aircraft_configuration_code; }
            set { _aircraft_configuration_code = value; }
        }

        int _number_of_bays;
        public int number_of_bays
        {
            get { return _number_of_bays; }
            set { _number_of_bays = value; }
        }

        string _boarding_class_rcd;
        public string boarding_class_rcd
        {
            get { return _boarding_class_rcd; }
            set { _boarding_class_rcd = value; }
        }

        int _number_of_rows;
        public int number_of_rows
        {
            get { return _number_of_rows; }
            set { _number_of_rows = value; }
        }

        int _number_of_columns;
        public int number_of_columns
        {
            get { return _number_of_columns; }
            set { _number_of_columns = value; }
        }

        int _layout_row;
        public int layout_row
        {
            get { return _layout_row; }
            set { _layout_row = value; }
        }

        int _layout_column;
        public int layout_column
        {
            get { return _layout_column; }
            set { _layout_column = value; }
        }

        string _location_type_rcd;
        public string location_type_rcd
        {
            get { return _location_type_rcd; }
            set { _location_type_rcd = value; }
        }

        string _seat_column;
        public string seat_column
        {
            get { return _seat_column; }
            set { _seat_column = value; }
        }
        string _seat_row;
        public string seat_row
        {
            get { return _seat_row; }
            set { _seat_row = value; }
        }


        int _stretcher_flag;
        public int stretcher_flag
        {
            get { return _stretcher_flag; }
            set { _stretcher_flag = value; }
        }

        int _handicapped_flag;
        public int handicapped_flag
        {
            get { return _handicapped_flag; }
            set { _handicapped_flag = value; }
        }

        int _no_child_flag;
        public int no_child_flag
        {
            get { return _no_child_flag; }
            set { _no_child_flag = value; }
        }

        int _bassinet_flag;
        public int bassinet_flag
        {
            get { return _bassinet_flag; }
            set { _bassinet_flag = value; }
        }

        int _no_infant_flag;
        public int no_infant_flag
        {
            get { return _no_infant_flag; }
            set { _no_infant_flag = value; }
        }

        int _infant_flag;
        public int infant_flag
        {
            get { return _infant_flag; }
            set { _infant_flag = value; }
        }

        int _emergency_exit_flag;
        public int emergency_exit_flag
        {
            get { return _emergency_exit_flag; }
            set { _emergency_exit_flag = value; }
        }

        int _unaccompanied_minors_flag;
        public int unaccompanied_minors_flag
        {
            get { return _unaccompanied_minors_flag; }
            set { _unaccompanied_minors_flag = value; }
        }

        int _window_flag;
        public int window_flag
        {
            get { return _window_flag; }
            set { _window_flag = value; }
        }

        int _aisle_flag;
        public int aisle_flag
        {
            get { return _aisle_flag; }
            set { _aisle_flag = value; }
        }

        int _block_b2c_flag;
        public int block_b2c_flag
        {
            get { return _block_b2c_flag; }
            set { _block_b2c_flag = value; }
        }

        int _block_b2b_flag;
        public int block_b2b_flag
        {
            get { return _block_b2b_flag; }
            set { _block_b2b_flag = value; }
        }

        int _blocked_flag;
        public int blocked_flag
        {
            get { return _blocked_flag; }
            set { _blocked_flag = value; }
        }

        int _low_comfort_flag;
        public int low_comfort_flag
        {
            get { return _low_comfort_flag; }
            set { _low_comfort_flag = value; }
        }

        int _passenger_count;
        public int passenger_count
        {
            get { return _passenger_count; }
            set { _passenger_count = value; }
        }
        #endregion
    }
}
