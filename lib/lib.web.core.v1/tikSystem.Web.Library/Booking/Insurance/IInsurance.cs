using System;
using System.Collections.Specialized;
using System.Text;

namespace tikSystem.Web.Library
{
    public interface IInsurance
    {
        Insurance RequestQuote(BookingHeader objHeader, 
                                Passengers objPax, 
                                DateTime clientDt, 
                                DateTime departureDate, 
                                DateTime returnDate, 
                                string originRcd, 
                                string destinationRcd,
                                string strLanguage,
                                bool international,
                                int agencyTimeZone,
                                int systemSettingTimeZone);

        Insurances RequestPolicy(BookingHeader objHeader,
                                Passengers objPax,
                                DateTime clientDt,
                                DateTime departureDate,
                                DateTime returnDate,
                                string originRcd,
                                string destinationRcd,
                                string strLanguage,
                                string strTravelPurpose,
                                decimal dclPremium,
                                bool international,
                                int agencyTimeZone,
                                int systemSettingTimeZone);
        bool InsuFeeFound(Fees fees);
    }
}
