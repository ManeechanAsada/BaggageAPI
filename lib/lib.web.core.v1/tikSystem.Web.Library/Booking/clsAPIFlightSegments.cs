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
    public class APIFlightSegments : CollectionBase
    {
        public APIFlightSegments() { }

        public APIFlightSegment this[int index]
        {
            get { return (APIFlightSegment)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(APIFlightSegment Value)
        {
            return this.List.Add(Value);
        }
    }
}
