using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingGeneratorLog
    {
        public int Id { get; set; }
        public int? StartVersion { get; set; }
        public int? EndVersion { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
