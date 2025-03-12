using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Serialization;

namespace tikSystem.Web.Library
{
    //checkin API
    public class APIPassengerFees : CollectionBase
    {
        public APIPassengerFees() { }

        public APIPassengerFee this[int index]
        {
            get { return (APIPassengerFee)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIPassengerFee Value)
        {
            return this.List.Add(Value);
        }
    }


    //checkin API
    public class APIPassengerAddresses : CollectionBase
    {
        public APIPassengerAddresses() { }

        public APIPassengerAddress this[int index]
        {
            get { return (APIPassengerAddress)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIPassengerAddress Value)
        {
            return this.List.Add(Value);
        }
    }

    public class APIPassengerDocuments : CollectionBase
    {
        public APIPassengerDocuments() { }

        public APIPassengerDocument this[int index]
        {
            get { return (APIPassengerDocument)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIPassengerDocument Value)
        {
            return this.List.Add(Value);
        }
    }

    //baggage
    public class APIPassengerBaggages : CollectionBase
    {
        public APIPassengerBaggages() { }

        public APIPassengerBaggage this[int index]
        {
            get { return (APIPassengerBaggage)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIPassengerBaggage Value)
        {
            return this.List.Add(Value);
        }
    }

}
