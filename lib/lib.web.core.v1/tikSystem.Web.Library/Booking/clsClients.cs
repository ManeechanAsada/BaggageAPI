using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using System.Collections;
using System.IO;
using tikSystem.Web.Library;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Clients : LibraryBase
    {
        public agentservice.TikAeroXMLwebservice objService = null;
        public Client this[int Index]
        {
            get { return (Client)this.List[Index]; }
            set { this.List[Index] = value; }
        }
        public int Add(Client Value)
        {
            return this.List.Add(Value);
        }
        public string GetClient(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            return ReadClient(clientId, clientNumber, passengerId, bShowRemark);
        }
        public void ClientRead(Guid client_profile_id)
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
                    GetClientSessionProfile(client_profile_id);
                    break;
                default:
                    //new Web service
                    GetClientSessionProfileWs(client_profile_id);
                    break;
            }
        }
        public void Read(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            string strXml = ReadClient(clientId, clientNumber, passengerId, bShowRemark);
            if (strXml.Length > 0)
            {
                XPathDocument XmlDoc = new XPathDocument(new StringReader(strXml));
                XPathNavigator nv = XmlDoc.CreateNavigator();

                Client objClient;
                foreach (XPathNavigator n in nv.Select("Booking"))
                {
                    objClient = (Client)XmlHelper.Deserialize(n.InnerXml, typeof(Client));
                    if (objClient != null)
                    {
                        Add(objClient);
                        objClient = null;
                    }
                }
            }
        }
        public void ReadClientPassenger(string bookingId, string clientProfileId, string clientNumber)
        {
            DataSet ds = GetClientPassenger(bookingId, clientProfileId, clientNumber);

            if (ds != null && ds.Tables.Count > 0)
            {

                XPathDocument XmlDoc = new XPathDocument(new StringReader(ds.GetXml()));
                XPathNavigator nv = XmlDoc.CreateNavigator();

                Client objClient;
                foreach (XPathNavigator n in nv.Select("TikAero/Passenger"))
                {
                    objClient = (Client)XmlHelper.Deserialize("<Client>" + n.InnerXml + "</Client>", typeof(Client));
                    if (objClient != null)
                    {
                        Add(objClient);
                        objClient = null;
                    }
                }
            }

            if (ds != null)
            {
                ds.Dispose();
            }
        }
        public DataSet GetClientPassenger(string bookingId, string clientProfileId, string clientNumber)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            return objClient.GetClientPassenger(bookingId, clientProfileId, clientNumber);
        }
        public DataSet GetTransaction(string strOrigin,
                                         string strDestination,
                                         string strAirline,
                                         string strFlight,
                                         string strSegmentType,
                                         string strClientProfileId,
                                         string strPassengerProfileId,
                                         string strVendor,
                                         string strCreditDebit,
                                         DateTime dtFlightFrom,
                                         DateTime dtFlightTo,
                                         DateTime dtTransactionFrom,
                                         DateTime dtTransactionTo,
                                         DateTime dtExpiryFrom,
                                         DateTime dtExpiryTo,
                                         DateTime dtVoidFrom,
                                         DateTime dtVoidTo,
                                         int iBatch,
                                         bool bAllVoid,
                                         bool bAllExpired,
                                         bool bAuto,
                                         bool bManual,
                                         bool bAllPoint)
        {
            DataSet ds = null;
            string result = string.Empty;
            ServiceClient objClient = new ServiceClient();

            try
            {
                objClient.objService = objService;

                result = objClient.GetTransaction(strOrigin, strDestination, strAirline, strFlight, strSegmentType, strClientProfileId, strPassengerProfileId, strVendor, strCreditDebit, dtFlightFrom, dtFlightTo, dtTransactionFrom, dtTransactionTo, dtExpiryFrom, dtExpiryTo, dtVoidFrom, dtVoidTo, iBatch, bAllVoid, bAllExpired, bAuto, bManual, bAllPoint);

                if (!string.IsNullOrEmpty(result.Trim()))
                {
                    using (StringReader r = new StringReader(result))
                    {
                        using (XmlReader reader = XmlReader.Create(r))
                        {
                            ds = new DataSet();
                            ds.ReadXml(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;

        }
        public DataSet AccuralQuote(Passengers Passengers, Mappings Mappings, string strClientProfileId)
        {
            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;

            return objClient.AccuralQuote(XmlHelper.Serialize(Passengers, false),
                                          XmlHelper.Serialize(Mappings, false),
                                          strClientProfileId);
        }
        public Client ClientLogon(string userName, string password)
        {
            ServiceClient objCient = new ServiceClient();
            objCient.objService = objService;
            Client client = null;

            try
            {
                DataSet ds = objCient.ClientLogon(userName, password);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    client = new Client();
                    PropertyInfo[] props = client.GetType().GetProperties();
                    for (int i = 0; i < props.Length; i++)
                    {
                        PropertyInfo prop = client.GetType().GetProperty(props[i].Name);
                        if (row.Table.Columns.Contains(prop.Name))
                        {
                            object value = row[prop.Name];
                            if (value != null)
                            {
                                object t = (object)TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(value.ToString());
                                prop.SetValue(client, Convert.ChangeType(t, prop.PropertyType), null);
                            }
                        }
                    }

                    return client;
                }
                else
                {
                    return client;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool FillClientInformation(Guid bookingId)
        {
            if (this.Count > 0)
            {
                //Sort object.
                this.Sort("client_number", ClientComparer.ClientSortOrderEnum.Ascending);

                int iFillCount = 0;
                long lClientNumber = 0;
                ServiceClient objClient = new ServiceClient();
                objClient.objService = objService;
                DataSet ds = null;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].client_number != lClientNumber)
                    {
                        //If dataset is not null clear and read data again.
                        if (ds != null)
                        {
                            ds.Dispose();
                            ds = null;
                        }

                        lClientNumber = this[i].client_number;
                        ds = objClient.GetClientPassenger(bookingId.ToString(), string.Empty, lClientNumber.ToString());
                    }

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["client_number"].ToString().Equals(lClientNumber.ToString()) == true &
                                dr["title_rcd"].ToString().ToUpper().Equals(this[i].title_rcd.ToUpper()) == true &
                                dr["firstname"].ToString().ToUpper().Equals(this[i].firstname.ToUpper()) == true &
                                dr["lastname"].ToString().ToUpper().Equals(this[i].lastname.ToUpper()) == true)
                            {
                                this[i].client_profile_id = new Guid(dr["client_profile_id"].ToString());
                                this[i].passenger_profile_id = new Guid(dr["passenger_profile_id"].ToString());

                                //------------------------------------------------------------------------------------//
                                this[i].firstname = dr["firstname"].ToString();
                                this[i].lastname = dr["lastname"].ToString();
                                this[i].title_rcd = dr["title_rcd"].ToString();
                                this[i].gender_type_rcd = dr["gender_type_rcd"].ToString();
                                this[i].document_type_rcd = dr["document_type_rcd"].ToString();
                                this[i].nationality_rcd = dr["nationality_rcd"].ToString();


                                this[i].passport_number = dr["passport_number"].ToString();
                                this[i].passport_issue_place = dr["passport_issue_place"].ToString();
                                this[i].vip_flag = Convert.ToByte((dr["vip_flag"].ToString().Length > 0) ? dr["vip_flag"] : 0);
                                this[i].passport_issue_date = (dr["passport_issue_date"].ToString().Length > 0) ? Convert.ToDateTime(dr["passport_issue_date"]) : DateTime.MinValue;
                                this[i].passport_expiry_date = (dr["passport_expiry_date"].ToString().Length > 0) ? Convert.ToDateTime(dr["passport_expiry_date"]) : DateTime.MinValue;
                                this[i].date_of_birth = (dr["date_of_birth"].ToString().Length > 0) ? Convert.ToDateTime(dr["date_of_birth"]) : DateTime.MinValue;
                                this[i].passenger_weight = (dr["passenger_weight"].ToString().Length > 0) ? Convert.ToDecimal(dr["passenger_weight"]) : 0;
                                this[i].passport_birth_place = dr["passport_birth_place"].ToString();
                                this[i].member_level_rcd = dr["member_level_rcd"].ToString();

                                //increment number of successful data fill.
                                iFillCount++;
                                break;
                            }
                        }
                    }
                }

                //Free dataset object.
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                //Check is all profile are added.
                if (this.Count == iFillCount)
                {
                    return true;
                }
            }
            return false;
        }
        public void Sort(String SortBy, ClientComparer.ClientSortOrderEnum SortOrder)
        {
            ClientComparer comparer = new ClientComparer();
            comparer.SortProperty = SortBy;
            comparer.SortOrder = SortOrder;

            InnerList.Sort(comparer);

        }
        #region Generate XML
        public string GetPassengerProfileXml(Client mySelfProfile)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-16", "yes"));
            XElement profile = new XElement("ClientProfile");
            XElement client = new XElement("MainProfile");
            PropertyInfo[] cProps = mySelfProfile.GetType().GetProperties();
            for (int i = 0; i < cProps.Length; i++)
            {
                PropertyInfo cProp = cProps[i];
                XElement c = new XElement(cProp.Name, cProp.GetValue(mySelfProfile, null));
                client.Add(c);
            }
            profile.Add(client);

            for (int i = 0; i < this.Count; i++)
            {
                Client passenger = this[i];
                XElement pax = new XElement(passenger.GetType().Name);
                PropertyInfo[] pProps = mySelfProfile.GetType().GetProperties();
                for (int j = 0; j < pProps.Length; j++)
                {
                    PropertyInfo pProp = pProps[j];
                    XElement c = new XElement(pProp.Name, pProp.GetValue(passenger, null));
                    pax.Add(c);
                }
                profile.Add(pax);
            }

            doc.Add(profile);
            return doc.ToString();
        }
        #endregion

        #region Helper
        private void GetClientSessionProfile(Guid client_profile_id)
        {
            try
            {
                if (objService != null)
                {
                    Library li = new Library();
                    DataSet ds = objService.GetClientSessionProfile(string.Format("{0}", client_profile_id));
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            Client client;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                client = new Client();
                                DataRow row = ds.Tables[0].Rows[i];
                                client = li.ObjectBinding(row, client) as Client;
                                if (client != null) Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetClientSessionProfileWs(Guid client_profile_id)
        {
            try
            {
                using (tikAeroWebService.tikAeroWebService objService = new tikSystem.Web.Library.tikAeroWebService.tikAeroWebService())
                {
                    Library li = new Library();
                    tikAeroWebService.Client[] clients = objService.GetClientSessionProfile(client_profile_id, base.CreateToken());
                    if (clients != null && clients.Length > 0)
                    {
                        Clear();
                        Client client;
                        for (int i = 0; i < clients.Length; i++)
                        {
                            client = new Client();
                            li.ObjectBinding(clients[i], client);
                            if (client != null) Add(client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string ReadClient(string clientId, string clientNumber, string passengerId, bool bShowRemark)
        {
            string result = string.Empty;

            ServiceClient objClient = new ServiceClient();
            objClient.objService = objService;
            return objClient.GetClient(clientId, clientNumber, passengerId, bShowRemark);
        }
        #endregion
    }
    public class ClientComparer : IComparer
    {

        public enum ClientSortOrderEnum
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

        private ClientSortOrderEnum _SortOrder = ClientSortOrderEnum.Ascending;
        public ClientSortOrderEnum SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }

        public int Compare(object x, object y)
        {
            Client client1;
            Client client2;

            if (x is Client)
                client1 = (Client)x;
            else
                throw new ArgumentException("Object is not type Client.");

            if (y is Client)
                client2 = (Client)y;
            else
                throw new ArgumentException("Object is not type Client.");

            if (this.SortOrder.Equals(ClientSortOrderEnum.Ascending))
                return client1.CompareTo(client2, this.SortProperty);
            else
                return client2.CompareTo(client1, this.SortProperty);

        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
