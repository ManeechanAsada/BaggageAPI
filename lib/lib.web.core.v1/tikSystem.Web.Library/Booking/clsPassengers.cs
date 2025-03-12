using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Passengers : LibraryBase
    {
        public Passenger this[int index]
        {
            get { return (Passenger)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Passenger value)
        {
            return this.List.Add(value);
        }
        #region Method
        public bool Valid(bool groupBooking)
        {
            if (this.Count > 0)
            {
                if (groupBooking == false)
                {
                    foreach (Passenger p in this)
                    {
                        if (string.IsNullOrEmpty(p.lastname))
                        {
                            return false;
                        }
                        else if (!DataHelper.ValidateEnglishCharacter(p.lastname))
                        {
                            return false;
                        }
                        else if (!DataHelper.ValidateEnglishCharacter(p.firstname))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    foreach (Passenger p in this)
                    {
                        if (!string.IsNullOrEmpty(p.lastname))
                        {
                            if (!DataHelper.ValidateEnglishCharacter(p.lastname))
                            {
                                return false;
                            }
                        }
                        else if (!string.IsNullOrEmpty(p.firstname))
                        {
                            if (!DataHelper.ValidateEnglishCharacter(p.firstname))
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
                foreach (Passenger p in this)
                {
                    if (p.date_of_birth != null && p.date_of_birth != DateTime.MinValue)
                    {
                        if (!DataHelper.ValidateDOB(departureDate, p.date_of_birth, p.passenger_type_rcd))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool IsInfant(Guid passengerId)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].passenger_id.Equals(passengerId) && this[i].passenger_type_rcd == "INF")
                {
                    return true;
                }
            }
            return false;
        }
        public Passengers GetAdultPassenger()
        {
            Passengers objAdult = new Passengers();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].passenger_type_rcd == "ADULT")
                {
                    objAdult.Add(this[i]);
                }
            }
            return objAdult;
        }
        public Passengers GetChildPassenger()
        {
            Passengers objAdult = new Passengers();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].passenger_type_rcd == "CHD")
                {
                    objAdult.Add(this[i]);
                }
            }
            return objAdult;
        }
        public Passengers GetInfantPassenger()
        {
            Passengers objAdult = new Passengers();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].passenger_type_rcd == "INF")
                {
                    objAdult.Add(this[i]);
                }
            }
            return objAdult;
        }

        public Passenger GetPassengerById(Guid passenger_id)
        {
            return this.OfType<Passenger>().Where(p => p.passenger_id == passenger_id).Single<Passenger>();
        }

        public void Clean()
        {
            for (int i = 0; i < this.Count; i++)
            {
                Passenger p = this[i];
                p.firstname = string.Empty;
                p.middlename = string.Empty;
                p.lastname = string.Empty;
                p.title_rcd = string.Empty;
                p.document_type_rcd = string.Empty;
                p.nationality_rcd = string.Empty;
                p.passport_number = string.Empty;
                p.passport_issue_place = string.Empty;
                p.passport_issue_date = DateTime.MinValue;
                p.passport_expiry_date = DateTime.MinValue;
                p.date_of_birth = DateTime.MinValue;
                p.member_number = string.Empty;
            }
        }
        #endregion
     
        public void Sort(String SortBy, PassengerComparer.PassengerSortOrderEnum SortOrder)
        {
            PassengerComparer comparer = new PassengerComparer();
            comparer.SortProperty = SortBy;
            comparer.SortOrder = SortOrder;

            InnerList.Sort(comparer);

        }
    }

    public class PassengerComparer : IComparer
    {

        public enum PassengerSortOrderEnum
        {
            Ascending,
            Descending
        }
        private string _Property = null;
        public string SortProperty
        {
            get { return _Property; }
            set { _Property = value; }
        }

        private PassengerSortOrderEnum _SortOrder = PassengerSortOrderEnum.Ascending;
        public PassengerSortOrderEnum SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }

        public int Compare(object x, object y)
        {
            Passenger pax1;
            Passenger pax2;

            if (x is Passenger)
                pax1 = (Passenger)x;
            else
                throw new ArgumentException("Object is not type Passenger.");

            if (y is Passenger)
                pax2 = (Passenger)y;
            else
                throw new ArgumentException("Object is not type Passenger.");

            if (this.SortOrder.Equals(PassengerSortOrderEnum.Ascending))
                return pax1.CompareTo(pax2, this.SortProperty);
            else
                return pax2.CompareTo(pax1, this.SortProperty);

        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
