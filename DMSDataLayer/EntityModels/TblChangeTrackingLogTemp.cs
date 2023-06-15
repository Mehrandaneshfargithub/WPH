using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingLogTemp
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Time { get; set; }
        public string Query { get; set; }
    }
}
