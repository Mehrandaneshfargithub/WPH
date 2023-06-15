using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoice
    {
        public TblSaleInvoice()
        {
            TblSaleInvoiceDetails = new HashSet<TblSaleInvoiceDetail>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public int? CreatedUserId { get; set; }
        public bool? IsDollar { get; set; }
        public int ModifiedUserId { get; set; }
        public bool? Enabled { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? InvoiceProfit { get; set; }
        public bool? IsWholesale { get; set; }
        public bool? OldFactor { get; set; }
        public int? PrintedNum { get; set; }
        public int? DaysWaiteForPayMoney { get; set; }
        public bool? FromSub { get; set; }
        public int? MandoobId { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int? SaleType { get; set; }
        public decimal? WorkerFare { get; set; }
        public decimal? TaxiFare { get; set; }
        public string VisitNum { get; set; }
        public bool? Opened { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblCustomer Customer { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual ICollection<TblSaleInvoiceDetail> TblSaleInvoiceDetails { get; set; }
    }
}
