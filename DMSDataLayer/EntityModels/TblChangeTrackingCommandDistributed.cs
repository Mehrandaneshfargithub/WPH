using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingCommandDistributed
    {
        public int Id { get; set; }
        public int? Ctversion { get; set; }
        public int? CtcreationVersion { get; set; }
        public string Ctsqlresult { get; set; }
        public string TableName { get; set; }
        public int? IdInTable { get; set; }
        public string StatusC { get; set; }
        public int? TransferredFromSourceToSite { get; set; }
        public int? TransferredFromSiteToDest { get; set; }
        public bool? ExecutedOnHost { get; set; }
        public bool? ExecutedOnDest { get; set; }
    }
}
