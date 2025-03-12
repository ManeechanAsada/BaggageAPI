using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Mappings : LibraryBase
    {
        public Mapping this[int index]
        {
            get { return (Mapping)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Mapping Value)
        {
            return this.List.Add(Value);
        }

        #region Method
        public bool CopyPassenger(Passengers passengers)
        {
            try
            {
                for (int i = 0; i < passengers.Count; i++)
                {
                    foreach (Mapping mp in this)
                    {
                        if (mp.passenger_id.Equals(passengers[i].passenger_id))
                        {
                            mp.firstname = passengers[i].firstname;
                            mp.lastname = passengers[i].lastname;
                            mp.middlename = passengers[i].middlename;
                            mp.title_rcd = passengers[i].title_rcd;
                            mp.date_of_birth = passengers[i].date_of_birth;
                            mp.gender_type_rcd = passengers[i].gender_type_rcd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool Valid(bool groupBooking, string currencyRcd)
        {
            //Check mapping name and lastname
            if (this.Count > 0)
            {
                if (groupBooking == false)
                {
                    foreach (Mapping m in this)
                    {
                        if (string.IsNullOrEmpty(m.lastname))
                        {
                            return false;
                        }
                        else if (string.IsNullOrEmpty(m.currency_rcd) || m.currency_rcd.Equals(currencyRcd) == false || ((m.passenger_status_rcd == "PO") ? false : string.IsNullOrEmpty(m.fare_code)))
                        {
                            return false;
                        }
                        else if (!DataHelper.ValidateEnglishCharacter(m.lastname))
                        {
                            return false;
                        }
                        else if (!DataHelper.ValidateEnglishCharacter(m.firstname))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    foreach (Mapping m in this)
                    {
                        if (!string.IsNullOrEmpty(m.lastname))
                        {
                            if (!DataHelper.ValidateEnglishCharacter(m.lastname))
                            {
                                return false;
                            }
                        }
                        else if (!string.IsNullOrEmpty(m.firstname))
                        {
                            if (!DataHelper.ValidateEnglishCharacter(m.firstname))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        public bool ValidDateOfBirth(DateTime departureDate)
        {
            if (this.Count > 0)
            {
                foreach (Mapping m in this)
                {
                    if (m.date_of_birth != null && m.date_of_birth != DateTime.MinValue)
                    {
                        if (!DataHelper.ValidateDOB(departureDate, m.date_of_birth, m.passenger_type_rcd))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public short FindPieceAllowance(Guid gBookingSegmentId, Guid gPassengerId)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].booking_segment_id.Equals(gBookingSegmentId) & this[i].passenger_id.Equals(gPassengerId) & this[i].piece_allowance > 0)
                {
                    return this[i].piece_allowance;
                }
            }
            return 0;
        }

        public List<Mapping> GetMappingByPassengerId(Guid passenger_id)
        {
            return this.OfType<Mapping>().Where(m => m.passenger_id == passenger_id).ToList<Mapping>();
        }

        public List<Mapping> GetPassengerBySegmentId(Guid booking_segment_id)
        {
            return this.OfType<Mapping>().Where(m => m.booking_segment_id == booking_segment_id).ToList<Mapping>();
        }

        public void Clean()
        {
            for (int i = 0; i < this.Count; i++)
            {
                Mapping mp = this[i];
                mp.firstname = string.Empty;
                mp.middlename = string.Empty;
                mp.lastname = string.Empty;
                mp.title_rcd = string.Empty;
                mp.date_of_birth = DateTime.MinValue;
                mp.seat_row = 0;
                mp.seat_column = string.Empty;
                mp.seat_number = string.Empty;
            }
        }

        #endregion
        #region COM Call

        public bool PointValidatePass(Client objClient, Mappings objMappings)
        {
            try
            {
                if (objMappings[0].fare_type_rcd == "POINT")
                {
                    if (objClient != null)
                    {
                        double TotalRedeem = 0;
                        foreach (Mapping m in objMappings)
                        {
                            TotalRedeem = TotalRedeem + m.redemption_points;
                        }
                        if (objClient.ffp_balance >= TotalRedeem)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
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
