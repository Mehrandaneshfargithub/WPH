using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingSchedule
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public int? PeriodByMinute { get; set; }
        public bool? AppLock { get; set; }
        public bool? AllowReplication { get; set; }
        public int? DestinationId { get; set; }

        public virtual TblChangeTrackingDestination Destination { get; set; }
    }
}
