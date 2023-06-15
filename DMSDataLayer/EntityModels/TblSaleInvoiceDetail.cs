using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoiceDetail
    {
        public TblSaleInvoiceDetail()
        {
            TblMandoobHistoryMainSaleInvoiceDetails = new HashSet<TblMandoobHistory>();
            TblMandoobHistoryMandoobSaleInvoiceDetails = new HashSet<TblMandoobHistory>();
            TblMandoobReturnHistoryMainSaleInvoiceDetails = new HashSet<TblMandoobReturnHistory>();
            TblMandoobReturnHistoryMandoobSaleInvoiceDetails = new HashSet<TblMandoobReturnHistory>();
        }

        public int Id { get; set; }
        public int MasterId { get; set; }
        public decimal? Num { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Discount { get; set; }
        public string Consideration { get; set; }
        public int? PurchaseInvoiceDetailId { get; set; }
        public decimal? NumInPaakat { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? Paakat { get; set; }
        public int UserId { get; set; }
        public DateTime DetailDate { get; set; }
        public int? MedicineId { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblSaleInvoice Master { get; set; }
        public virtual TblUser User { get; set; }
        public virtual ICollection<TblMandoobHistory> TblMandoobHistoryMainSaleInvoiceDetails { get; set; }
        public virtual ICollection<TblMandoobHistory> TblMandoobHistoryMandoobSaleInvoiceDetails { get; set; }
        public virtual ICollection<TblMandoobReturnHistory> TblMandoobReturnHistoryMainSaleInvoiceDetails { get; set; }
        public virtual ICollection<TblMandoobReturnHistory> TblMandoobReturnHistoryMandoobSaleInvoiceDetails { get; set; }
    }
}
