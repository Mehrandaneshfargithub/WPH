using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingLog
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsSource { get; set; }
        public int? DestinationId { get; set; }

        public virtual TblChangeTrackingDestination Destination { get; set; }
    }
}
