using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingSetting
    {
        public int Id { get; set; }
        public int? DatabaseType { get; set; }
        public int? ChangeTrackingsCount { get; set; }
        public int? ProcedurePeriod { get; set; }
        public bool? RunOnHost { get; set; }
        public bool? SaveOnHost { get; set; }
        public bool? MobileApp { get; set; }
        public int? DestCount { get; set; }
        public int? DestNumber { get; set; }
        public bool? FullReplication { get; set; }
        public bool? OnLocal { get; set; }
    }
}
