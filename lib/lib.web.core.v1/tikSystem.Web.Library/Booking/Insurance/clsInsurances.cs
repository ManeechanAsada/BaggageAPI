using System;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Configuration;

namespace tikSystem.Web.Library
{
    public class Insurances : LibraryBase
    {
        public enum InsuranceType
        {
            ACEJapan = 0,
            ACENonJapan = 1
        }
     //   private InsuranceType _type;
        IInsurance objInsu = null;

        public Insurances(InsuranceType type)
        {
            if (type.Equals(InsuranceType.ACENonJapan))
            {
                objInsu = new ACEInsurance("InsuranceACEHKGSetting");
            }
            else
            {
                objInsu = new ACEInsurance();
            }

           // _type = type;
        }
        public Insurances()
        {
            objInsu = new ACEInsurance();
        }
        public Insurance this[int index]
        {
            get { return (Insurance)this.List[index]; }
        }
        public int Add(Insurance value)
        {
            return this.List.Add(value);
        }

        #region Field
        string _agency_code = string.Empty;
        string _language_rcd = string.Empty;

        Guid _booking_segment_id = Guid.Empty;
        Guid _passenger_id = Guid.Empty;

        decimal _passenger_count = 0;
        #endregion

        #region Method
        public bool RequestQuote(BookingHeader header,
                                Passengers passengers,
                                Itinerary itinerary,
                                Fees fees,
                                DateTime clientDt,
                                DateTime departureDate,
                                DateTime returnDate,
                                Routes routes,
                                string originRcd,
                                string destinationRcd,
                                string strLanguage,
                                int agencyTimeZone,
                                int systemSettingTimeZone)
        {
            Insurance insu;
            if ((passengers != null && passengers.Count > 0) && (itinerary != null && itinerary.Count > 0))
            {
                bool international = false;

                if (itinerary[0] != null)
                {
                    international = itinerary.IsInternationalFlight;
                }
                else
                {
                    for (int i = 0; i <= routes.Count; i++)
                    {
                        if (routes[i].origin_rcd == originRcd & routes[i].destination_rcd == destinationRcd)
                        {
                            if (routes[i].require_document_details_flag == true)
                            {
                                international = true;
                            }
                            else
                            {
                                international = false;
                            }
                            break;
                        }
                    }
                }

                //Request Quote
                bool bFeeFound = objInsu.InsuFeeFound(fees);
                insu = objInsu.RequestQuote(header, 
                                            passengers, 
                                            clientDt, 
                                            departureDate, 
                                            returnDate, 
                                            originRcd, 
                                            destinationRcd, 
                                            strLanguage, 
                                            international,
                                            agencyTimeZone,
                                            systemSettingTimeZone);
                if (insu != null)
                {
                    _agency_code = header.agency_code;
                    _language_rcd = strLanguage;

                    _booking_segment_id = itinerary[0].booking_segment_id;
                    _passenger_id = passengers[0].passenger_id;

                    _passenger_count = passengers.Count;
                    if (insu != null)
                    {
                        //Check if there any insu fee in the session
                        insu.fee_found = bFeeFound;
                    }
                    Add(insu);
                    return true;
                }
                else
                {
                    insu = new Insurance();
                    insu.error_code = "405";
                    insu.error_message = "Insurance request failed.";
                    insu.fee_found = bFeeFound;
                    this.Add(insu);
                    return false;
                }
            }
            else
            {
                insu = new Insurance();
                insu.error_code = "403";
                insu.error_message = "Some object is null or no value";
                insu.fee_found = false;
                this.Add(insu);

                return false;
            }
        }

        public bool RequestPolicy(BookingHeader header,
                                  Passengers passengers,
                                  Itinerary itinerary,
                                  DateTime clientDt,
                                  DateTime departureDate,
                                  DateTime returnDate,
                                  Fees fees,
                                  string originRcd,
                                  string destinationRcd,
                                  string strLanguage,
                                  string strTravelPurpose,
                                  decimal dclPremium,
                                  int agencyTimeZone, 
                                  int systemSettingTimeZone)
        {

            if ((header != null && string.IsNullOrEmpty(header.record_locator) == false) &&
                (passengers != null && passengers.Count > 0) && 
                (itinerary != null && itinerary.Count > 0))
            {
                bool international = false;

                international = itinerary.IsInternationalFlight;
                //Request Policy
                if (objInsu != null)
                {
                    if (objInsu.InsuFeeFound(fees) == true)
                    {
                        Insurances insus = objInsu.RequestPolicy(header,
                                                                passengers,
                                                                clientDt,
                                                                departureDate,
                                                                returnDate,
                                                                originRcd,
                                                                destinationRcd,
                                                                strLanguage,
                                                                strTravelPurpose,
                                                                dclPremium,
                                                                international,
                                                                agencyTimeZone,
                                                                systemSettingTimeZone);
                        if (insus != null && insus.Count > 0)
                        {
                            for (int i = 0; i < insus.Count; i++)
                            {
                                Add(insus[i]);
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Insurance insu = new Insurance();
                        insu.error_code = "404";
                        insu.error_message = "No Fee Found";
                        this.Add(insu);

                        return false;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                Insurance insu = new Insurance();
                insu.error_code = "403";
                insu.error_message = "Some object is null or no value";
                this.Add(insu);

                return false;
            }
        }
        public bool FillPremiumToFee(Fee fee)
        {
            try
            {
                if (this.Count > 0 && string.IsNullOrEmpty(this[0].error_code) == false  && this[0].error_code == "000")
                {
                    if (fee != null && string.IsNullOrEmpty(fee.fee_rcd) == false)
                    {
                        //Fill Fee information.
                        fee.booking_fee_id = Guid.NewGuid();
                        fee.agency_code = _agency_code;
                        fee.fee_amount = this[0].Fee.fee_amount;
                        fee.fee_amount_incl = fee.fee_amount;
                        fee.booking_segment_id = _booking_segment_id;
                        fee.passenger_id = _passenger_id;
                        fee.number_of_units = _passenger_count;

                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FillPolicyNumberToFee(Fee fee, string policy_number)
        {
            try
            {
                if (this.Count > 0 && string.IsNullOrEmpty(this[0].error_code) == false && this[0].error_code == "000")
                {
                    if (fee != null && string.IsNullOrEmpty(fee.fee_rcd) == false)
                    {
                        fee.comment = policy_number;

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Fee ReadFee(bool bNoVat)
        {
            if (this.Count > 0 && string.IsNullOrEmpty(this[0].error_code) == false && this[0].error_code == "000")
            {
                Fees objFees = new Fees();
                objFees.objService = objService;
                objFees.GetFeeDefinition(this[0].Fee.fee_rcd,
                                        this[0].Fee.currency_rcd,
                                        _agency_code,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        DateTime.MinValue,
                                        _language_rcd,
                                        bNoVat);

                for (int i = 0; i < objFees.Count; i++)
                {
                    if (objFees[i].fee_rcd.ToUpper().Equals(this[0].Fee.fee_rcd.ToUpper()) == true)
                    {
                        return objFees[i];
                    }
                }
            }
            return null;
        }
        public bool InsuranceFeeExist(Fees fees)
        {
            return objInsu.InsuFeeFound(fees);
        }
        #endregion

        
    }
}
