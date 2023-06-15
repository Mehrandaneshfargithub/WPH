using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWarehouseTransferring
    {
        public TblWarehouseTransferring()
        {
            TblWarehouseTransferringDetails = new HashSet<TblWarehouseTransferringDetail>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
        public string Description { get; set; }
        public int? CreatedUserId { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual ICollection<TblWarehouseTransferringDetail> TblWarehouseTransferringDetails { get; set; }
    }
}
