using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class Allocations : CollectionBase
    {
        public Allocation this[int index]
        {
            get { return (Allocation)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(Allocation value)
        {
            return this.List.Add(value);
        }
    }
}
