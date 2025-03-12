using System;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;

namespace tikSystem.Web.Library
{
    public class ACEInsurance :IInsurance
    {
        NameValueCollection _Setting;
        public ACEInsurance()
        {
            _Setting = (NameValueCollection)ConfigurationManager.GetSection("InsuranceACESetting");
        }
        public ACEInsurance(string InsuranceACESetting)
        {
            _Setting = (NameValueCollection)ConfigurationManager.GetSection(InsuranceACESetting);
        }
        //Delegate function used for multithreading.
        public delegate Insurances RequestPolicyDelegate(BookingHeader objHeader,
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
        #region IInsurance Members

        public Insurance RequestQuote(BookingHeader objHeader,
                                        Passengers objPax,
                                        DateTime clientDt,
                                        DateTime departureDate,
                                        DateTime returnDate,
                                        string originRcd,
                                        string destinationRcd,
                                        string strLanguage,
                                        bool international,
                                        int agencyTimeZone,
                                        int systemSettingTimeZone)
        {
                //Log writer declaration.
                StringBuilder strLog = new StringBuilder();
                Library objLi = new Library();

                AceInsurance.ACORD objACORD = new AceInsurance.ACORD();
                Insurance insuResult = new Insurance();
                insuResult.Fee = new Fee();
                string strLogLocation = string.Empty;
                string strConfigGroupPlanId = string.Empty;
                try
                {
                    if (string.IsNullOrEmpty(_Setting["LogFilePath"]) == false)
                    {
                        strLogLocation = _Setting["LogFilePath"] + @"\" + "QUOTE_INSU_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + Guid.NewGuid() + ".log"; ;
                    }

                    if ((_Setting["AllowedWithReturnFlight"] == null || Convert.ToInt16(_Setting["AllowedWithReturnFlight"]) == 1) && returnDate.Equals(DateTime.MinValue))
                    {
                        insuResult.error_code = "402";
                        insuResult.error_message = "Insurance Allowed only with return flight.";
                    }
                    else if (string.IsNullOrEmpty(_Setting["SellLanguage"]) == false && _Setting["SellLanguage"].ToUpper().Contains(strLanguage.ToUpper()) == false)
                    {
                        insuResult.error_code = "403";
                        insuResult.error_message = "Selected country is not allowed to buy insurance";
                    }
                    else
                    {
                        if (international == true)
                        {
                            insuResult.flight_Type = "OTA";
                        }
                        else
                        {
                            insuResult.flight_Type = "DTA";
                        }

                        // check oneway or roundtrip or  undefined
                        if ((_Setting["AllowedWithReturnFlight"] != null && Convert.ToInt16(_Setting["AllowedWithReturnFlight"]) == 2))
                        {
                            if (returnDate == DateTime.MinValue)
                            {
                                insuResult.flight_trip = "OneWay";
                            }
                            else
                            {
                                insuResult.flight_trip = "RoundTrip";
                            }
                        }
                        else
                        {
                            insuResult.flight_trip = "";
                        }

                        strConfigGroupPlanId = insuResult.flight_Type + insuResult.flight_trip;


                        if (ValidConfiguration(insuResult.flight_Type , insuResult.flight_trip) == true)
                        {
                            AceInsurance.ACORDService objAceSvc = new AceInsurance.ACORDService();

                            objAceSvc.Url = _Setting["SOAPUrl"];

                            if (_Setting["SetACETimeout"] != null && Int32.Parse(_Setting["SetACETimeout"]) > 0)
                            {
                                objAceSvc.Timeout = Int32.Parse(_Setting["SetACETimeout"]);
                            }

                            //User Info
                            objACORD.SignonRq = new AceInsurance.ACORDSignonRq();
                            objACORD.SignonRq.SignonPswd = new AceInsurance.ACORDSignonRqSignonPswd();
                            objACORD.SignonRq.SignonPswd.CustId = new AceInsurance.ACORDSignonRqSignonPswdCustId();
                            objACORD.SignonRq.SignonPswd.CustId.SPName = _Setting["SpName"];
                            objACORD.SignonRq.SignonPswd.CustId.CustLoginId = _Setting["UserId"];

                            //Security Info
                            objACORD.SignonRq.SignonPswd.CustPswd = new AceInsurance.ACORDSignonRqSignonPswdCustPswd();
                            objACORD.SignonRq.SignonPswd.CustPswd.EncryptionTypeCd = AceInsurance.ACORDSignonRqSignonPswdCustPswdEncryptionTypeCd.None;
                            objACORD.SignonRq.SignonPswd.CustPswd.Pswd = _Setting["Password"];

                            objACORD.SignonRq.ClientDt = clientDt;
                            objACORD.SignonRq.CustLangPref = strLanguage;

                            objACORD.SignonRq.ClientApp = new AceInsurance.ACORDSignonRqClientApp();
                            objACORD.SignonRq.ClientApp.Org = _Setting["OrganizationName"];
                            objACORD.SignonRq.ClientApp.Name = _Setting["ApplicationName"];
                            objACORD.SignonRq.ClientApp.Version = 1.0M;

                            //Insurance request detail.
                            objACORD.InsuranceSvcRq = new AceInsurance.ACORDInsuranceSvcRq();
                            objACORD.InsuranceSvcRq.RqUID = Guid.NewGuid().ToString();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRq();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.TransactionRequestDt = DateTime.Now;

                            //Passenger information.
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipal();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfo();

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo[(objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants) + 1];

                            bool bFirstSameContct = FindFirstPax(objHeader, objPax);
                            
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].id = "1";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.Surname = objHeader.lastname;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.GivenName = objHeader.firstname;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].PersonName.TitlePrefix = objHeader.title_rcd.ToUpper();

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfo();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryNameCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryNameCd.Alias;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryName();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.Surname = objHeader.lastname;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.GivenName = objHeader.firstname;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.TitlePrefix = objHeader.title_rcd;

                            if (objPax[0].passenger_type_rcd.Equals("ADULT"))
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].TitleRelationshipDesc = "Adult";
                            else
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].TitleRelationshipDesc = "Child";

                            
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDtSpecified = true;
                            if (bFirstSameContct == true)
                            {
                                if (objPax[0].date_of_birth != DateTime.MinValue)
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDt = objPax[0].date_of_birth;
                                    if (objPax[0].passport_number.Length > 0)
                                    {
                                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].IdNumberTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoIdNumberTypeCd.IDNumber;
                                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].IdNumber = objPax[0].passport_number;
                                    }
                                    else
                                    {
                                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].IdNumberTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoIdNumberTypeCd.IDNumber;
                                        objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].IdNumber = "";
                                    }

                                }
                                else
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDt = Convert.ToDateTime("1900-01-01");
                                }
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].BirthDt = Convert.ToDateTime("1900-01-01");
                            }

                            for (int i = 0; i < objPax.Count; i++)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].id = (i + 2).ToString();
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.Surname = objPax[i].lastname;
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.GivenName = objPax[i].firstname;
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].PersonName.TitlePrefix = objPax[i].title_rcd.ToUpper();

                                //if (objPax[i].passenger_type_rcd.Equals("ADULT"))
                                //    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].TitleRelationshipDesc = "Adult";
                                //else if (objPax[i].passenger_type_rcd.Equals("CHD"))
                                //    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].TitleRelationshipDesc = "Child";
                                //else
                                //    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].TitleRelationshipDesc = "Infant";

                                if (i == 0)
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].TitleRelationshipDesc = "Self"; 
                                else
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].TitleRelationshipDesc = "Travel Companion";


                                if (objPax[i].date_of_birth != DateTime.MinValue)
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDtSpecified = true;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDt = objPax[i].date_of_birth;
                                }
                                else
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].BirthDt = Convert.ToDateTime("1900-01-01");
                                }

                                if (objPax[i].gender_type_rcd.Equals("M"))
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.M;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                                }
                                else if (objPax[i].gender_type_rcd.Equals("F"))
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.F;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                                }
                                else
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.U;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].GenderSpecified = true;
                                }

                                if (objPax[i].passport_number.Length > 0)
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].IdNumberTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoIdNumberTypeCd.IDNumber;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].IdNumber = objPax[i].passport_number;
                                }
                                else
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].IdNumberTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoNameInfoIdNumberTypeCd.IDNumber;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i + 1].IdNumber = "";
                                }
                            }

                            //Address Information from contact infomation.
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoAddr[1];

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoAddr();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].id = "1";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].NameInfoRef = "1";

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr1 = objHeader.address_line1;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr2 = objHeader.address_line2;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].City = objHeader.city;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].StateProv = objHeader.state;
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].PostalCode = objHeader.zip_code.Replace("-", "");
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Country = objHeader.country_rcd;

                            //Communication information.
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunications[1];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunications();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].id = "1";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].NameInfoRef = "1";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo[2];

                            for (int i = 0; i < 2; i++)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo();
                                if (i == 0)
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Telephone;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_home;
                                }
                                else
                                {
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Cell;
                                    objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_mobile;
                                }

                            }

                            //Email Information.
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo[1];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0].EmailAddr = objHeader.contact_email;

                            //Policy information
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicy();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.CompanyProductCd = _Setting["ProductId" + insuResult.flight_Type];

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyContractTerm();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.EffectiveDt = departureDate;

                            if (returnDate.Equals(DateTime.MinValue) == false)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.ExpirationDt = returnDate;
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.ContractTerm.ExpirationDt = departureDate;
                            }

                            //Group destination Id
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_Destination();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination.RqUID = _Setting["ComGroupDestId" + insuResult.flight_Type];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Destination.DestinationDesc = _Setting["ComGroupDestDesc" + insuResult.flight_Type];

                            //Group InsuredPackage
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_InsuredPackage();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage.RqUID = _Setting["ComGroupInsuretId"];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_InsuredPackage.InsuredPackageDesc = _Setting["ComGroupInsuretDesc"];

                            //Group plan
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqPersPolicyComacegroup_Plan();

                            //add flight_trip
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan.RqUID = _Setting["GroupPlanId" + strConfigGroupPlanId];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.PersPolicy.comacegroup_Plan.PlanDesc = _Setting["GroupPlanDesc" + insuResult.flight_Type];

                            //Group Data extension
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem[9];
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].key = "DepartureCity";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[0].value = originRcd;

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].key = "ArrivalCity";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[1].value = destinationRcd;

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].key = "MobileEmailAddrForPolicyHolder";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[2].value = objHeader.mobile_email;

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].key = "UseAsFirstPassenger";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].type = "System.String";

                            if (bFirstSameContct == true)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].value = "Y";
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[3].value = "N";
                            }

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].key = "Total_Insured";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].type = "System.Integer";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[4].value = (objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants).ToString();

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].key = "PaymentTransactionDate";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].type = "System.String";

                            if (agencyTimeZone > 0)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(agencyTimeZone)); // As Asia
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[5].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(systemSettingTimeZone)); // As Asia
                            }

                            //add more items
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[6] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[6].key = "ArrivalDate";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[6].type = "System.DateTime";


                            if (returnDate == DateTime.MinValue)
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", departureDate);
                            }
                            else
                            {
                                objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", returnDate);
                            }

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[7] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[7].key = "BookingReference";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[7].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[7].value = "BookingReference";

                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[8] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyQuoteInqRqDataItemDataItem();
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[8].key = "PremiumCurrency";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[8].type = "System.String";
                            objACORD.InsuranceSvcRq.PersPkgPolicyQuoteInqRq.comacegroup_DataExtensions[8].value = objHeader.currency_rcd;

                            //Submit transaction
                            AceInsurance.ACORD1 objACORDResponse = new AceInsurance.ACORD1();

                            if (string.IsNullOrEmpty(strLogLocation) == false)
                            {
                                if (objACORD != null)
                                {
                                    //Write Log Request
                                    strLog.Append("------------------- Request" + Environment.NewLine);
                                    strLog.Append(XmlHelper.Serialize(objACORD, false) + Environment.NewLine);
                                }
                            }

                            objACORDResponse = objAceSvc.GetTravelQuote(objACORD);

                            if (string.IsNullOrEmpty(strLogLocation) == false)
                            {
                                if (objACORDResponse != null)
                                {
                                    //Write Log Response
                                    strLog.Append("------------------- Response" + Environment.NewLine);
                                    strLog.Append(XmlHelper.Serialize(objACORDResponse, false) + Environment.NewLine);
                                }
                            }
                            if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.MsgStatus.MsgStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyQuoteInqRsMsgStatusMsgStatusCd.Success)
                            {
                                insuResult.error_code = "000";
                                insuResult.error_message = "Success Request";
                                //   objFee = new Fee();
                                if (_Setting["InsuranceFeeRcd"] != null)
                                {
                                    insuResult.Fee.fee_rcd = _Setting["InsuranceFeeRcd"];
                                }
                                else
                                {
                                    insuResult.Fee.fee_rcd = "INSU";
                                }
                                insuResult.Fee.currency_rcd = objHeader.currency_rcd;
                                insuResult.Fee.fee_amount = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.PersPolicy.QuoteInfo.InsuredFullToBePaidAmt.Amt;
                                insuResult.Fee.agency_code = objHeader.agency_code;

                                // keep ACE Quote passengers
                                if (_Setting["ACEQuotePassenger"] != null)
                                {
                                    int countOfInsPassenger = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo.Length;
                                    ACEQuotePassenger[] ACEQuotePassenger = new ACEQuotePassenger[countOfInsPassenger - 1];
                                    insuResult.ACEQuotePassengers = new ACEQuotePassengers();

                                    for (int i = 1; i < countOfInsPassenger; i++)
                                    {
                                        int j = 0;
                                        ACEQuotePassenger[j] = new ACEQuotePassenger();
                                        ACEQuotePassenger[j].title_rcd = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].PersonName.TitlePrefix;
                                        ACEQuotePassenger[j].lastname = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].PersonName.Surname;
                                        ACEQuotePassenger[j].firstname = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].PersonName.GivenName;
                                        ACEQuotePassenger[j].gender_type_rcd = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].Gender.ToString();
                                        ACEQuotePassenger[j].passport_number = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].IdNumber.ToString();
                                        ACEQuotePassenger[j].passenger_type_rcd = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].TitleRelationshipDesc.ToString();
                                        ACEQuotePassenger[j].date_of_birth = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[i].BirthDt;
                                        ACEQuotePassenger[j].EffectiveDt = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.PersPolicy.ContractTerm.EffectiveDt;
                                        ACEQuotePassenger[j].ExpirationDt = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.PersPolicy.ContractTerm.ExpirationDt;

                                        insuResult.ACEQuotePassengers.Add(ACEQuotePassenger[j]);

                                        j++;

                                    }
                                }
                            }
                            else
                            {
                                insuResult.error_code = "401";
                                insuResult.error_message = objACORDResponse.InsuranceSvcRs.PersPkgPolicyQuoteInqRs.MsgStatus.MsgStatusDesc;
                            }
                        }
                        else
                        {
                            insuResult.error_code = "400";
                            insuResult.error_message = "Configuration incorrect";
                        }
                    }
                }
                catch (Exception ex)
                {
                    insuResult.error_code = "004";
                    insuResult.error_message = ex.Message;
                    strLog.Append("Error : " + ex.Message);
                }
                finally
                {
                    if (string.IsNullOrEmpty(strLogLocation) == false)
                    {
                        //write Log File.
                        objLi.WriteLog(strLogLocation, strLog.ToString());
                    }
                }
                return insuResult;
        }

        

        public Insurances RequestPolicy(BookingHeader objHeader,
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
                                       int systemSettingTimeZone)
        {
            if (_Setting != null)
            {
                if (string.IsNullOrEmpty(_Setting["AsyncMode"]) || Convert.ToBoolean(_Setting["AsyncMode"]) == false)
                {
                    //Sync Chronous Mode.
                    return SubmitPolicy(objHeader,
                                        objPax,
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
                }
                else
                {
                    //Async Chronous Mode. for Ja-Jp route
                    StartPolicyRequestAsync(objHeader,
                                            objPax,
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
                }
            }
            return null;
        }
        public bool InsuFeeFound(Fees fees)
        {
            string strFeeCode;
            if (_Setting["InsuranceFeeRcd"] != null)
            {
                strFeeCode = _Setting["InsuranceFeeRcd"];
            }
            else
            {
                strFeeCode = "INSU";
            }

            if (fees != null && fees.Count > 0)
            {
                for (int i = 0; i < fees.Count; i++)
                {
                    if (fees[i].fee_rcd.ToUpper().Equals(strFeeCode.ToUpper()))
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        #endregion
        #region Helper
        private void ACEPassengerInfo(string firstName,
                                      string lastName,
                                      string strTitle,
                                      DateTime dtDateOfBirth,
                                      string strGender,
                                      ref AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo objACEPax,
                                      bool bFirstSameContct)
        {
            objACEPax.PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
            objACEPax.PersonName.Surname = lastName;
            objACEPax.PersonName.GivenName = firstName;
            objACEPax.PersonName.TitlePrefix = strTitle.ToUpper();

            if (bFirstSameContct == true)
            {
                if (dtDateOfBirth != DateTime.MinValue)
                {
                    objACEPax.BirthDt = dtDateOfBirth;
                }
                else
                {
                    objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
                }
            }
            else
            {
                objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
            }


            if (string.IsNullOrEmpty(strGender) == false)
            {
                if (strGender.Equals("M"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.M;
                    objACEPax.GenderSpecified = true;
                }
                else if (strGender.Equals("F"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.F;
                    objACEPax.GenderSpecified = true;
                }
                else
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.U;
                    objACEPax.GenderSpecified = true;
                }
            }

        }

        private void ACEPassengerInfo(string firstName,
                                      string lastName,
                                      string strTitle,
                                      DateTime dtDateOfBirth,
                                      string strGender,
                                      string passenger_type_rcd,
                                      string passport_number,
                                      ref AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo objACEPax,
                                      bool bFirstSameContct)
        {
            objACEPax.PersonName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoPersonName();
            objACEPax.PersonName.Surname = lastName;
            objACEPax.PersonName.GivenName = firstName;
            objACEPax.PersonName.TitlePrefix = strTitle.ToUpper();

            if (passenger_type_rcd.Equals("ADULT"))
            {
                objACEPax.TitleRelationshipDesc = "Self";
            }
            else
            {
                objACEPax.TitleRelationshipDesc = "Travel Companion";
            }

            objACEPax.IdNumberTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoIdNumberTypeCd.IDNumber;
            objACEPax.IdNumber = passport_number;

            if (bFirstSameContct == true)
            {
                if (dtDateOfBirth != DateTime.MinValue)
                {
                    objACEPax.BirthDt = dtDateOfBirth;
                }
                else
                {
                    objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
                }
            }
            else
            {
                objACEPax.BirthDt = Convert.ToDateTime("1900-01-01");
            }


            if (string.IsNullOrEmpty(strGender) == false)
            {
                if (strGender.Equals("M"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.M;
                    objACEPax.GenderSpecified = true;
                }
                else if (strGender.Equals("F"))
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.F;
                    objACEPax.GenderSpecified = true;
                }
                else
                {
                    objACEPax.Gender = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoGender.U;
                    objACEPax.GenderSpecified = true;
                }
            }

        }

        private bool FindFirstPax(BookingHeader header, Passengers paxs)
        {
            if (paxs != null && paxs.Count > 0)
            {
                if (header.title_rcd.ToUpper().Equals(paxs[0].title_rcd) & header.firstname.ToUpper().Equals(paxs[0].firstname) & header.lastname.ToUpper().Equals(paxs[0].lastname))
                {
                    return true;
                }
            }
            
            return false;
        }
        private bool ValidConfiguration(string strConfigSubfix, string strConfigSubfixFlightTrip)
        {
            if ((_Setting == null) ||
                (_Setting["SpName"] == null || _Setting["SpName"].Length == 0) ||
                (_Setting["SOAPUrl"] == null || _Setting["SOAPUrl"].Length == 0) ||
                (_Setting["UserId"] == null || _Setting["UserId"].Length == 0) ||
                (_Setting["Password"] == null || _Setting["Password"].Length == 0) ||
                (_Setting["OrganizationName"] == null || _Setting["OrganizationName"].Length == 0) ||
                (_Setting["ApplicationName"] == null || _Setting["ApplicationName"].Length == 0) ||
                (_Setting["ProductId" + strConfigSubfix] == null || _Setting["ProductId" + strConfigSubfix].Length == 0) ||
                (_Setting["ComGroupDestId" + strConfigSubfix] == null || _Setting["ComGroupDestId" + strConfigSubfix].Length == 0) ||
                (_Setting["ComGroupDestDesc" + strConfigSubfix] == null || _Setting["ComGroupDestDesc" + strConfigSubfix].Length == 0) ||
                (_Setting["ComGroupInsuretId"] == null || _Setting["ComGroupInsuretId"].Length == 0) ||
                (_Setting["ComGroupInsuretDesc"] == null || _Setting["ComGroupInsuretDesc"].Length == 0) ||
                (_Setting["GroupPlanId" + strConfigSubfix + strConfigSubfixFlightTrip] == null || _Setting["GroupPlanId" + strConfigSubfix + strConfigSubfixFlightTrip].Length == 0) ||
                (_Setting["GroupPlanDesc" + strConfigSubfix] == null || _Setting["GroupPlanDesc" + strConfigSubfix].Length == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Insurance RequestSinglePolicy(BookingHeader objHeader,
                                                Passengers objPax,
                                                DateTime clientDt,
                                                DateTime departureDate,
                                                DateTime returnDate,
                                                string originRcd,
                                                string destinationRcd,
                                                string strLanguage,
                                                string strTravelPurpose,
                                                decimal dclPremium,
                                                string strUrl,
                                                string strSpName,
                                                string strUserId,
                                                string strPassword,
                                                string strOrganizationName,
                                                string strApplicationName,
                                                string strProductId,
                                                string strComGroupDestId,
                                                string strComGroupDestDesc,
                                                string strComGroupInsuretId,
                                                string strComGroupInsuretDesc,
                                                string strGroupPlanId,
                                                string strGroupPlanDesc,
                                                string strFlightType,
                                                int agencyTimeZone,
                                                int systemSettingTimeZone)
        {
            bool bFirstSameContct = FindFirstPax(objHeader, objPax);
            Insurance insu = new Insurance();
            //Log writer declaration.
            StringBuilder strLog = new StringBuilder();
            Library objLi = new Library();
            string strLogLocation = "";
            if (string.IsNullOrEmpty(_Setting["LogFilePath"]) == false)
            {
                strLogLocation = _Setting["LogFilePath"] + @"\" + "POLICY_INSU_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + Guid.NewGuid() + ".log"; ;
            }

            try
            {

                #region Webservice setting
                AceInsurance.ACORDService objAceSvc = new AceInsurance.ACORDService();
                objAceSvc.Url = strUrl;
                dclPremium = dclPremium / (objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants);

                AceInsurance.ACORD2 objACORD = new AceInsurance.ACORD2();
                //User Info
                objACORD.SignonRq = new AceInsurance.ACORDSignonRq1();
                objACORD.SignonRq.SignonPswd = new AceInsurance.ACORDSignonRqSignonPswd1();
                objACORD.SignonRq.SignonPswd.CustId = new AceInsurance.ACORDSignonRqSignonPswdCustId1();
                objACORD.SignonRq.SignonPswd.CustId.SPName = strSpName;
                objACORD.SignonRq.SignonPswd.CustId.CustLoginId = strUserId;

                //Security Info
                objACORD.SignonRq.SignonPswd.CustPswd = new AceInsurance.ACORDSignonRqSignonPswdCustPswd1();
                objACORD.SignonRq.SignonPswd.CustPswd.EncryptionTypeCd = AceInsurance.ACORDSignonRqSignonPswdCustPswdEncryptionTypeCd1.None;
                objACORD.SignonRq.SignonPswd.CustPswd.Pswd = strPassword;

                objACORD.SignonRq.ClientDt = clientDt;
                objACORD.SignonRq.CustLangPref = strLanguage;

                objACORD.SignonRq.ClientApp = new AceInsurance.ACORDSignonRqClientApp1();
                objACORD.SignonRq.ClientApp.Org = strOrganizationName;
                objACORD.SignonRq.ClientApp.Name = strApplicationName;
                objACORD.SignonRq.ClientApp.Version = 1.0M;

                //Insurance request detail.
                objACORD.InsuranceSvcRq = new AceInsurance.ACORDInsuranceSvcRq1();
                objACORD.InsuranceSvcRq.RqUID = Guid.NewGuid().ToString();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRq();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.TransactionRequestDt = DateTime.Now;
                #endregion

                #region Passenger Detail
                //Passenger information.

                int count = objPax.Count;

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipal();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo[count + 1];


                //Filled Primary Passenger information.
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].id = "1";


                ACEPassengerInfo(objHeader.firstname,
                                 objHeader.lastname,
                                 objHeader.title_rcd,
                                 objPax[0].date_of_birth,
                                 string.Empty,
                                 objPax[0].passenger_type_rcd,
                                 objPax[0].passport_number,
                                 ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0],
                                 bFirstSameContct);

                //Address Information from contact infomation.
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr[1];

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].id = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].NameInfoRef = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr1 = objHeader.address_line1;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr2 = objHeader.address_line2;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].City = objHeader.city;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].StateProv = objHeader.state;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].PostalCode = objHeader.zip_code.Replace("-", "");
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Country = objHeader.country_rcd;

                //Communication information.
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications[1];
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].id = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].NameInfoRef = "1";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo[2];

                for (int i = 0; i < 2; i++)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo();
                    if (i == 0)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Telephone;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_home;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Cell;
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_mobile;
                    }
                }

                //Email Information.
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo[1];
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0].EmailAddr = objHeader.contact_email;


                int IndexCount = 0;
                for (int j = 0; j < objPax.Count; j++)
                {
                    IndexCount = j + 1;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[IndexCount] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[IndexCount].id = (j + 2).ToString();

                    ACEPassengerInfo(objPax[j].firstname,
                                     objPax[j].lastname,
                                     objPax[j].title_rcd,
                                     objPax[j].date_of_birth,
                                     objPax[j].gender_type_rcd,//string.Empty,
                                     objPax[j].passenger_type_rcd,
                                     objPax[j].passport_number,
                                     ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[IndexCount],
                                     true);
                }

                #endregion

                #region Policy Information
                //Policy information
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicy();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.CompanyProductCd = strProductId;

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyContractTerm();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.EffectiveDt = departureDate;

                if (returnDate.Equals(DateTime.MinValue) == false)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = returnDate;
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = departureDate;
                }

                //Group destination Id
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Destination();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.RqUID = strComGroupDestId;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.DestinationDesc = strComGroupDestDesc;

                //Group InsuredPackage
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_InsuredPackage();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.RqUID = strComGroupInsuretId;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.InsuredPackageDesc = strComGroupInsuretDesc;

                //Group plan
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Plan();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.RqUID = strGroupPlanId;
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.PlanDesc = strGroupPlanDesc;

                //Group Data extension
                if (strFlightType == "OTA")
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[11];
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[10];
                }


                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].key = "DepartureCity";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].value = originRcd;

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].key = "MobileEmailAddrForPolicyHolder";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].value = objHeader.mobile_email;

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].key = "UseAsFirstPassenger";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].type = "System.String";
                if (bFirstSameContct == true)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "Y";
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "N";
                }

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].key = "BookingReference";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].value = objHeader.record_locator;

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].key = "BasePremium";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].type = "System.Double";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].value = string.Format("{0:0.00}", dclPremium); // Will be change if required.

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].key = "Total_Insured";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].type = "System.Integer";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].value = (objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants).ToString();

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].key = "PaymentTransactionDate";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].type = "System.String";

                if (agencyTimeZone > 0)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(agencyTimeZone)); // As Asia
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(systemSettingTimeZone)); // As Asia
                }

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].key = "PremiumCurrency";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].value = objHeader.currency_rcd; // As Asia

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].key = "ArrivalCity";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].type = "System.String";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].value = destinationRcd; // As Asia

                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].key = "ArrivalDate";
                objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].type = "System.DateTime";

                // check oneway or roundtrip
                if (returnDate == DateTime.MinValue)
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = string.Format("{0:yyyy-MM-dd}", departureDate);
                }
                else
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = string.Format("{0:yyyy-MM-dd}", returnDate);
                }


                if (strFlightType == "OTA")
                {
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].key = "DestinationGovCode";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].value = "AS"; // As Asia
                }

                #endregion

                #region Webservice Request

                if (string.IsNullOrEmpty(strLogLocation) == false)
                {
                    if (objACORD != null)
                    {
                        //Write Log Request
                        strLog.Append("------------------- RequestPolicy" + Environment.NewLine);
                        strLog.Append(XmlHelper.Serialize(objACORD, false) + Environment.NewLine);
                    }
                }

                //Submit transaction
                AceInsurance.ACORD3 objACORDResponse;
                objACORDResponse = objAceSvc.GetTravelPolicy(objACORD);

                if (objACORDResponse != null)
                {
                    //Write Log Request
                    strLog.Append("------------------- ResponsePolicy" + Environment.NewLine);
                    strLog.Append(XmlHelper.Serialize(objACORDResponse, false) + Environment.NewLine);
                }

                insu.flight_Type = strFlightType;
                if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsMsgStatusMsgStatusCd.Success)
                {
                    if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsPersPolicyPolicyStatusCd.Accepted)
                    {
                        insu.error_code = "000";
                        insu.error_message = "SUCCESS";
                        insu.policy_number = objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyNumber;
                        insu.title_rcd = objHeader.title_rcd;
                        insu.lastname = objHeader.lastname;
                        insu.firstname = objHeader.firstname;
                    }
                    else
                    {
                        insu.error_code = "800";
                        insu.error_message = "Request of policy is reject";
                    }
                }
                else
                {
                    insu.error_code = "801";
                    insu.error_message = objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusDesc;
                }
                #endregion
            }
            catch (Exception ex)
            {
                insu.error_code = "400";
                insu.error_message = ex.Message;
            }
            finally
            {
                if (string.IsNullOrEmpty(strLogLocation) == false)
                {
                    //write Log File.
                    objLi.WriteLog(strLogLocation, strLog.ToString());
                }
            }

            return insu;
        }

        private Insurances RequestMultiplePolicy(BookingHeader objHeader,
                                                Passengers objPax,
                                                DateTime clientDt,
                                                DateTime departureDate,
                                                DateTime returnDate,
                                                string originRcd,
                                                string destinationRcd,
                                                string strLanguage,
                                                string strTravelPurpose,
                                                decimal dclPremium,
                                                string strUrl,
                                                string strSpName,
                                                string strUserId,
                                                string strPassword,
                                                string strOrganizationName,
                                                string strApplicationName,
                                                string strProductId,
                                                string strComGroupDestId,
                                                string strComGroupDestDesc,
                                                string strComGroupInsuretId,
                                                string strComGroupInsuretDesc,
                                                string strGroupPlanId,
                                                string strGroupPlanDesc,
                                                string strFlightType,
                                                string strAppPath, 
                                                int agencyTimeZone, 
                                                int systemSettingTimeZone)
        {
            //Log declaration.
            string strPath = string.Empty;
            StringBuilder strLog = new StringBuilder();
            Library objLi = new Library();

            Insurances insuResults = null;
            bool bFirstSameContct = FindFirstPax(objHeader, objPax);
            
            AceInsurance.ACORDService objAceSvc = new AceInsurance.ACORDService();
            objAceSvc.Url = strUrl;
            dclPremium = dclPremium / (objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants);

            if (string.IsNullOrEmpty(strAppPath) == false)
            {
                strPath = strAppPath + @"\" + "POLICY_INSU_" + objHeader.record_locator + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + Guid.NewGuid() + ".log";
                strLog.Append("------------------- Start Write Log (" + objHeader.record_locator + ")" + Environment.NewLine);
            }
            
            try
            {
                for (int j = 0; j < objPax.Count; j++)
                {
                    AceInsurance.ACORD2 objACORD = new AceInsurance.ACORD2();
                    //User Info
                    objACORD.SignonRq = new AceInsurance.ACORDSignonRq1();
                    objACORD.SignonRq.SignonPswd = new AceInsurance.ACORDSignonRqSignonPswd1();
                    objACORD.SignonRq.SignonPswd.CustId = new AceInsurance.ACORDSignonRqSignonPswdCustId1();
                    objACORD.SignonRq.SignonPswd.CustId.SPName = strSpName;
                    objACORD.SignonRq.SignonPswd.CustId.CustLoginId = strUserId;

                    //Security Info
                    objACORD.SignonRq.SignonPswd.CustPswd = new AceInsurance.ACORDSignonRqSignonPswdCustPswd1();
                    objACORD.SignonRq.SignonPswd.CustPswd.EncryptionTypeCd = AceInsurance.ACORDSignonRqSignonPswdCustPswdEncryptionTypeCd1.None;
                    objACORD.SignonRq.SignonPswd.CustPswd.Pswd = strPassword;

                    objACORD.SignonRq.ClientDt = clientDt;
                    objACORD.SignonRq.CustLangPref = strLanguage;

                    objACORD.SignonRq.ClientApp = new AceInsurance.ACORDSignonRqClientApp1();
                    objACORD.SignonRq.ClientApp.Org = strOrganizationName;
                    objACORD.SignonRq.ClientApp.Name = strApplicationName;
                    objACORD.SignonRq.ClientApp.Version = 1.0M;

                    //Insurance request detail.
                    objACORD.InsuranceSvcRq = new AceInsurance.ACORDInsuranceSvcRq1();
                    objACORD.InsuranceSvcRq.RqUID = Guid.NewGuid().ToString();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRq();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.TransactionRequestDt = DateTime.Now;
                    
                    //Passenger information.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipal();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfo();

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo[2];

                    //Filled Primary Passenger information.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].id = "1";

                    ACEPassengerInfo(objHeader.firstname,
                                     objHeader.lastname,
                                     objHeader.title_rcd,
                                     objPax[0].date_of_birth,
                                     string.Empty,
                                     ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0],
                                     bFirstSameContct);

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryNameCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryNameCd.Alias;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfoSupplementaryNameInfoSupplementaryName();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.Surname = objHeader.lastname;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.GivenName = objHeader.firstname;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[0].SupplementaryNameInfo.SupplementaryName.TitlePrefix = objHeader.title_rcd;

                    //Filled list passenger information.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoNameInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1].id = (j + 2).ToString();

                    ACEPassengerInfo(objPax[j].firstname,
                                     objPax[j].lastname,
                                     objPax[j].title_rcd,
                                     objPax[j].date_of_birth,
                                     objPax[j].gender_type_rcd,
                                     ref objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.NameInfo[1],
                                     true);

                    //Address Information from contact infomation.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr[1];

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoAddr();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].id = "1";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].NameInfoRef = "1";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr1 = objHeader.address_line1;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Addr2 = objHeader.address_line2;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].City = objHeader.city;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].StateProv = objHeader.state;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].PostalCode = objHeader.zip_code.Replace("-", "");
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Addr[0].Country = objHeader.country_rcd;

                    //Communication information.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications[1];
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunications();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].id = "1";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].NameInfoRef = "1";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo[2];

                    for (int i = 0; i < 2; i++)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfo();
                        if (i == 0)
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Telephone;
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_home;
                        }
                        else
                        {
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneTypeCd = AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsPhoneInfoPhoneTypeCd.Cell;
                            objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].PhoneInfo[i].PhoneNumber = objHeader.phone_mobile;
                        }

                    }

                    //Email Information.
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo[1];
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0] = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqInsuredOrPrincipalGeneralPartyInfoCommunicationsEmailInfo();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.InsuredOrPrincipal.GeneralPartyInfo.Communications[0].EmailInfo[0].EmailAddr = objHeader.contact_email;

                    //Policy information
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicy();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.CompanyProductCd = strProductId;

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyContractTerm();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.EffectiveDt = departureDate;

                    if (returnDate.Equals(DateTime.MinValue) == false)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = returnDate;
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.ContractTerm.ExpirationDt = departureDate;
                    }

                    //Group destination Id
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Destination();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.RqUID = strComGroupDestId;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Destination.DestinationDesc = strComGroupDestDesc;

                    //Group InsuredPackage
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_InsuredPackage();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.RqUID = strComGroupInsuretId;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_InsuredPackage.InsuredPackageDesc = strComGroupInsuretDesc;

                    //Group plan
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan = new AceInsurance.ACORDInsuranceSvcRqPersPkgPolicyAddRqPersPolicyComacegroup_Plan();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.RqUID = strGroupPlanId;
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.PersPolicy.comacegroup_Plan.PlanDesc = strGroupPlanDesc;

                    //Group Data extension
                    if (strFlightType == "OTA")
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[11];
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem[10];
                    }


                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].key = "DepartureCity";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[0].value = originRcd;

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].key = "MobileEmailAddrForPolicyHolder";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[1].value = objHeader.mobile_email;

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].key = "UseAsFirstPassenger";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].type = "System.String";
                    if (bFirstSameContct == true)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "Y";
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[2].value = "N";
                    }

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].key = "BookingReference";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[3].value = objHeader.record_locator;

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].key = "BasePremium";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].type = "System.Double";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[4].value = string.Format("{0:0.00}", dclPremium); // Will be change if required.

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].key = "Total_Insured";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].type = "System.Integer";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[5].value = (objHeader.number_of_adults + objHeader.number_of_children + objHeader.number_of_infants).ToString();

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].key = "PaymentTransactionDate";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].type = "System.String";
                    if (agencyTimeZone > 0)
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(agencyTimeZone)); // As Asia
                    }
                    else
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[6].value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMinutes(systemSettingTimeZone)); // As Asia
                    }
                    
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].key = "PremiumCurrency";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[7].value = objHeader.currency_rcd; // As Asia

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].key = "ArrivalCity";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].type = "System.String";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[8].value = destinationRcd; // As Asia

                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].key = "Purpose";
                    objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].type = "System.String";

                    if (strFlightType == "DTA")
                    { objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = "2"; }
                    else
                    { objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[9].value = strTravelPurpose; }

                    if (strFlightType == "OTA")
                    {
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10] = new AceInsurance.ArrayOfACORDInsuranceSvcRqPersPkgPolicyAddRqDataItemDataItem();
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].key = "DestinationGovCode";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].type = "System.String";
                        objACORD.InsuranceSvcRq.PersPkgPolicyAddRq.comacegroup_DataExtensions[10].value = "AS"; // As Asia
                    }

                    #region Webservice Request
                    try
                    {
                        //Submit transaction
                        AceInsurance.ACORD3 objACORDResponse;

                        if (string.IsNullOrEmpty(strAppPath) == false)
                        {
                            if (objACORD != null)
                            {
                                //Write Log Request
                                strLog.Append("------------------- Request" + Environment.NewLine);
                                strLog.Append(XmlHelper.Serialize(objACORD, false) + Environment.NewLine);
                            }
                        }

                        objACORDResponse = objAceSvc.GetTravelPolicy(objACORD);
                        insuResults = new Insurances();
                        Insurance insu = new Insurance();
                        insu.flight_Type = strFlightType;

                        if (string.IsNullOrEmpty(strAppPath) == false)
                        {
                            if (objACORDResponse != null)
                            {
                                //Write Log Response
                                strLog.Append("------------------- Response" + Environment.NewLine);
                                strLog.Append(XmlHelper.Serialize(objACORDResponse, false) + Environment.NewLine);
                            }
                        }


                        if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsMsgStatusMsgStatusCd.Success)
                        {
                            if (objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyStatusCd == AceInsurance.ACORDInsuranceSvcRsPersPkgPolicyAddRsPersPolicyPolicyStatusCd.Accepted)
                            {
                                insu.error_code = "000";
                                insu.error_message = "SUCCESS";
                                insu.policy_number = objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.PersPolicy.PolicyNumber;
                                insu.title_rcd = objPax[j].title_rcd;
                                insu.lastname = objPax[j].lastname;
                                insu.firstname = objPax[j].firstname;
                            }
                            else
                            {
                                insu.error_code = "800";
                                insu.error_message = "Request of policy is reject";
                            }
                        }
                        else
                        {
                            insu.error_code = "801";
                            insu.error_message = objACORDResponse.InsuranceSvcRs.PersPkgPolicyAddRs.MsgStatus.MsgStatusDesc;
                        }
                        insuResults.Add(insu);
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(strAppPath) == false)
                        {
                            strLog.Append("Error : " + ex.Message);
                            objLi.WriteLog(strPath, strLog.ToString());
                        }
                    }
                    #endregion
                }

                if (string.IsNullOrEmpty(strAppPath) == false)
                {
                    objLi.WriteLog(strPath, strLog.ToString());
                }
            }
            catch(Exception ex)
            {
                if (string.IsNullOrEmpty(strAppPath) == false)
                {
                    strLog.Append("Error : " + ex.Message);
                    objLi.WriteLog(strPath, strLog.ToString());
                }
                throw ex;
            }
            return insuResults;
        }
        private Insurances SubmitPolicy(BookingHeader objHeader,
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
                                        int systemSettingTimeZone)
        {
            Insurances insuResults;
            Insurance insu;

            if (originRcd.Equals("HKG"))
                _Setting = (NameValueCollection)ConfigurationManager.GetSection("InsuranceACEHKGSetting");

            try
            {
                if (objHeader == null)
                {
                    insuResults = new Insurances();
                    insu = new Insurance();
                    insu.error_code = "802";
                    insu.error_message = "No booking object found";
                    insuResults.Add(insu);
                }
                if (international == true && string.IsNullOrEmpty(strTravelPurpose) == true)
                {
                    insuResults = new Insurances();
                    insu = new Insurance();
                    insu.error_code = "803";
                    insu.error_message = "Purpose of travel is required";
                    insuResults.Add(insu);
                }
                else
                {
                    string strFlightType;

                    if (international == true)
                    {
                        strFlightType = "OTA";
                    }
                    else
                    {
                        strFlightType = "DTA";
                    }

                    if (string.IsNullOrEmpty(_Setting["ApplyMultiplePolicyNumber"]) == true || Convert.ToInt16(_Setting["ApplyMultiplePolicyNumber"]) == 1)
                    {
                        //Request For Multiple policy number
                        insuResults = RequestMultiplePolicy(objHeader,
                                                             objPax,
                                                             clientDt,
                                                             departureDate,
                                                             returnDate,
                                                             originRcd,
                                                             destinationRcd,
                                                             strLanguage,
                                                             strTravelPurpose,
                                                             dclPremium,
                                                             _Setting["SOAPUrl"],
                                                             _Setting["SpName"],
                                                             _Setting["UserId"],
                                                             _Setting["Password"],
                                                             _Setting["OrganizationName"],
                                                             _Setting["ApplicationName"],
                                                             _Setting["ProductId" + strFlightType],
                                                             _Setting["ComGroupDestId" + strFlightType],
                                                             _Setting["ComGroupDestDesc" + strFlightType],
                                                             _Setting["ComGroupInsuretId"],
                                                             _Setting["ComGroupInsuretDesc"],
                                                             _Setting["GroupPlanId" + strFlightType],
                                                             _Setting["GroupPlanDesc" + strFlightType],
                                                             strFlightType,
                                                             _Setting["LogFilePath"],
                                                             agencyTimeZone,
                                                             systemSettingTimeZone);
                    }
                    else
                    {
                        // check flight trip
                        string GroupPlanId = string.Empty;

                        if (returnDate == DateTime.MinValue)
                        {
                            GroupPlanId = strFlightType + "OneWay";
                        }
                        else
                        {
                            GroupPlanId = strFlightType + "RoundTrip";
                        }

                        insuResults = new Insurances();

                        insu = RequestSinglePolicy(objHeader,
                                                    objPax,
                                                    clientDt,
                                                    departureDate,
                                                    returnDate,
                                                    originRcd,
                                                    destinationRcd,
                                                    strLanguage,
                                                    strTravelPurpose,
                                                    dclPremium,
                                                    _Setting["SOAPUrl"],
                                                    _Setting["SpName"],
                                                    _Setting["UserId"],
                                                    _Setting["Password"],
                                                    _Setting["OrganizationName"],
                                                    _Setting["ApplicationName"],
                                                    _Setting["ProductId" + strFlightType],
                                                    _Setting["ComGroupDestId" + strFlightType],
                                                    _Setting["ComGroupDestDesc" + strFlightType],
                                                    _Setting["ComGroupInsuretId"],
                                                    _Setting["ComGroupInsuretDesc"],
                                                    _Setting["GroupPlanId" + GroupPlanId],
                                                    _Setting["GroupPlanDesc" + strFlightType],
                                                    strFlightType,
                                                    agencyTimeZone,
                                                    systemSettingTimeZone);
                        insuResults.Add(insu);
                    }

                }

            }
            catch (Exception ex)
            {
                insuResults = new Insurances();
                insu = new Insurance();
                insu.error_code = "400";
                insu.error_message = ex.Message;
                insuResults.Add(insu);
            }
            return insuResults;
        }
        private bool SendErrorMail(string strMailFrom, string strMailTo, string strSubject, string strBody, string strUserId, string strBookingId)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = "";
            return objClient.QueueMail(strMailFrom,
                                        strMailFrom,
                                        strMailTo,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        strSubject,
                                        strBody,
                                        "ACEINSU",
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        true,
                                        false,
                                        true,
                                        strUserId,
                                        strBookingId,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty);
        }
        #endregion
        #region Async Function
        public void StartPolicyRequestAsync(BookingHeader objHeader,
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
                                            int systemSettingTimeZone)
        {
            RequestPolicyDelegate objDelegate = new RequestPolicyDelegate(SubmitPolicy);
            IAsyncResult ar = objDelegate.BeginInvoke(objHeader, 
                                                        objPax, 
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
                                                        systemSettingTimeZone,
                                                        new AsyncCallback(EndPolicyRequestAsync), 
                                                        null);
   
        }
        public void EndPolicyRequestAsync(IAsyncResult ar)
        {
            if (ar != null)
            {
                RequestPolicyDelegate objDelegate = (RequestPolicyDelegate)((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate;
                objDelegate.EndInvoke(ar);
            }
            
        }
        #endregion
    }
}
