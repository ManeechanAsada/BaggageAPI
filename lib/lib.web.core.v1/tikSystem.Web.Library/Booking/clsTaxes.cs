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

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Taxes : CollectionBase
    {
        public Tax this[int index]
        {
            get { return (Tax)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(Tax value)
        {
            return this.List.Add(value);
        }
    }
}
