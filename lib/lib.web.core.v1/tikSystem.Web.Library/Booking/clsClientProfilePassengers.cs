using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace tikSystem.Web.Library
{
    public class ClientProfilePassengers : CollectionBase  
    {
        public ClientProfilePassenger this[int index]
        {
            get { return (ClientProfilePassenger)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(ClientProfilePassenger Value)
        {
            return this.List.Add(Value);
        }
    }


}
