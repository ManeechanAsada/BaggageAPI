using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class ACEQuotePassengers : LibraryBase
    {
        public ACEQuotePassenger this[int index]
        {
            get { return (ACEQuotePassenger)this.List[index]; }
        }
        public int Add(ACEQuotePassenger value)
        {
            return this.List.Add(value);
        }

    }
}
