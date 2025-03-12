using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace tikSystem.Web.Library
{
    public class APIPassengerUpdateRequests : CollectionBase
    {
        public APIPassengerUpdateRequest this[int index]
        {
            get { return (APIPassengerUpdateRequest)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(APIPassengerUpdateRequest value)
        {
            return this.List.Add(value);
        }
    }
}
