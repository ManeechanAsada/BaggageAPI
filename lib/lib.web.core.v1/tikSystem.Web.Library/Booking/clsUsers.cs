using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.XPath;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class Users : LibraryBase
    {
        public User this[int index]
        {
            get { return (User)this.List[index]; }
            set { this.List[index] = value; }
        }
        public int Add(User value)
        {
            return this.List.Add(value);
        }

    }
}
