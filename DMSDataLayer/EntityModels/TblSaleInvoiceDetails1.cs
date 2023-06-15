using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoiceDetails1
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int? Num { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal Discount { get; set; }
        public string Consideration { get; set; }
        public int? PurchaseInvoiceDetailId { get; set; }
        public int? NumInPaakat { get; set; }
        public int? FreeNum { get; set; }
        public int? Paakat { get; set; }
        public int UserId { get; set; }
        public DateTime DetailDate { get; set; }
        public int? MedicineId { get; set; }
    }
}
