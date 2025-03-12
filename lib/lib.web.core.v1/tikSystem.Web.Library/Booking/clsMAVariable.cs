using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class MAVariable
    {
        Int16 _currentStep = 1;
        public Int16 CurrentStep
        {
            get { return _currentStep; }
            set { _currentStep = value; }
        }
        Int16 _Adult;
        public Int16 Adult
        {
            get { return _Adult; }
            set { _Adult = value; }
        }
        Int16 _Child;
        public Int16 Child
        {
            get { return _Child; }
            set { _Child = value; }
        }
        Int16 _Infant;
        public Int16 Infant
        {
            get { return _Infant; }
            set { _Infant = value; }
        }
        Int16 _Other;
        public Int16 Other
        {
            get { return _Other; }
            set { _Other = value; }
        }
        string _OtherPassengerType = string.Empty;
        public string OtherPassengerType
        {
            get { return _OtherPassengerType; }
            set { _OtherPassengerType = value; }
        }
        string _BoardingClass = string.Empty;
        public string BoardingClass
        {
            get { return _BoardingClass; }
            set { _BoardingClass = value; }
        }
        string _SearchCurrencyRcd = string.Empty;
        public string SearchCurrencyRcd
        {
            get { return _SearchCurrencyRcd; }
            set { _SearchCurrencyRcd = value; }
        }
        Guid _UserId = Guid.Empty;
        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        string _formOfPaymentFee;
        public string FormOfPaymentFee
        {
            get { return _formOfPaymentFee; }
            set { _formOfPaymentFee = value; }
        }
        string _OriginRcd = string.Empty;
        public string OriginRcd
        {
            get { return _OriginRcd; }
            set { _OriginRcd = value; }
        }
        string _DestinationRcd = string.Empty;
        public string DestinationRcd
        {
            get { return _DestinationRcd; }
            set { _DestinationRcd = value; }
        }
        string _OriginName = string.Empty;
        public string OriginName
        {
            get { return _OriginName; }
            set { _OriginName = value; }
        }
        string _DestinationName = string.Empty;
        public string DestinationName
        {
            get { return _DestinationName; }
            set { _DestinationName = value; }
        }
        Guid _booking_payment_id = Guid.Empty;
        public Guid booking_payment_id
        {
            get { return _booking_payment_id; }
            set { _booking_payment_id = value; }
        }
        string _DestinationClass = string.Empty;
        public string DestinationClass
        {
            get { return _DestinationClass; }
            set { _DestinationClass = value; }
        }
        string _VendorRcd = string.Empty;
        public string VendorRcd
        {
            get { return _VendorRcd; }
            set { _VendorRcd = value; }
        }
        string _LanguageRcd = string.Empty;
        public string LanguageRcd
        {
            get { return _LanguageRcd; }
            set { _LanguageRcd = value; }
        }
    }
}
