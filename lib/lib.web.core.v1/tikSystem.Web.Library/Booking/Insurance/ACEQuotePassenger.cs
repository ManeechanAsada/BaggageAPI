using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    public class ACEQuotePassenger : People
    {
        #region Property
       
        DateTime _EffectiveDt;
        public DateTime EffectiveDt
        {
            get { return _EffectiveDt; }
            set { _EffectiveDt = value; }
        }
        DateTime _ExpirationDt;
        public DateTime ExpirationDt
        {
            get { return _ExpirationDt; }
            set { _ExpirationDt = value; }
        }

        #endregion
    }
}
