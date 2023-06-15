using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReplicationLog
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SourceServerName { get; set; }
        public string DestinationServerName { get; set; }
        public string SourceDbname { get; set; }
        public string DestinationDbname { get; set; }
        public string ErrorMessage { get; set; }
        public bool? Completed { get; set; }
    }
}
