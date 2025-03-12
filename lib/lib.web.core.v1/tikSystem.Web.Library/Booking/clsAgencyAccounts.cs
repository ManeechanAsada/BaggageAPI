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
using tikSystem.Web.Library;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class AgencyAccounts : CollectionBase
    {
        public AgencyAccount this[int index]
        {
            get { return (AgencyAccount)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(AgencyAccount value)
        {
            return this.List.Add(value);
        }        
    }
}
