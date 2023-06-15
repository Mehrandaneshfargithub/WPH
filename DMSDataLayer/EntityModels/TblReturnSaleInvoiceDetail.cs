using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReturnSaleInvoiceDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int SalePurchaseDetailId { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? SalesPrice { get; set; }
        public int? MedicineId { get; set; }
        public string Consideration { get; set; }
        public DateTime DetailDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NumInPaakat { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblReturnSaleInvoice Master { get; set; }
        public virtual TblSalePurchaseDetail SalePurchaseDetail { get; set; }
    }
}
