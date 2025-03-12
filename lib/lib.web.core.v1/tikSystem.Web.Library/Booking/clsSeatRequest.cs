using System;
using System.Collections.Generic;
using System.Text;

namespace tikSystem.Web.Library
{
    //checkin API
    public class SeatRequest
    {
        public Guid BookingSegmentId { get; set; }
        public Guid PassengerId { get; set; }
        public int SeatRow { get; set; }
        public string SeatColumn { get; set; }
        public string SeatFeeRcd { get; set; }
    }
}
