using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingDestination
    {
        public TblChangeTrackingDestination()
        {
            TblChangeTrackingLogs = new HashSet<TblChangeTrackingLog>();
            TblChangeTrackingSchedules = new HashSet<TblChangeTrackingSchedule>();
        }

        public int Id { get; set; }
        public string Destination { get; set; }
        public bool? IsTempDest { get; set; }

        public virtual ICollection<TblChangeTrackingLog> TblChangeTrackingLogs { get; set; }
        public virtual ICollection<TblChangeTrackingSchedule> TblChangeTrackingSchedules { get; set; }
    }
}
