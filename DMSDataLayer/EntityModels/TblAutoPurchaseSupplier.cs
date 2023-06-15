using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblAutoPurchaseSupplier
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public int? RemoteCustomerId { get; set; }
        public string HostDataSource { get; set; }
        public string HostDbname { get; set; }
        public string HostUserName { get; set; }
        public string HostPass { get; set; }
        public string TablePrefix { get; set; }

        public virtual TblSupplier Supplier { get; set; }
    }
}
