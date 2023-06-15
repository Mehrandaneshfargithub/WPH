using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWarehouseTransferringDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int? MedicineId { get; set; }
        public decimal? Paakat { get; set; }
        public decimal? NumInPaakat { get; set; }
        public decimal? Num { get; set; }
        public string Consideration { get; set; }
        public DateTime DetailDate { get; set; }
        public int UserId { get; set; }
        public int? PurchaseId { get; set; }
        public decimal? FreeNum { get; set; }

        public virtual TblWarehouseTransferring Master { get; set; }
        public virtual TblPurchaseInvoiceDetail Purchase { get; set; }
    }
}
