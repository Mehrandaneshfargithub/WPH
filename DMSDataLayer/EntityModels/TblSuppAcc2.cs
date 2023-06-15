using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSuppAcc2
    {
        public int? Id { get; set; }
        public int? SuppliersId { get; set; }
        public string SupplierName { get; set; }
        public string QabzNum { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? RecievedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Desc { get; set; }
        public decimal? TotalSupplierRecieved { get; set; }
        public decimal? TotalSupplierPaid { get; set; }
        public decimal? TotalSupplierPurchase { get; set; }
        public decimal? TotalSupplierReturnPurchase { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? TotalRecieved { get; set; }
        public decimal? TotalPurchase { get; set; }
        public decimal? TotalReturnPurchase { get; set; }
        public int? Type { get; set; }
        public decimal? RemainAmount { get; set; }
        public decimal? EndRemain { get; set; }
        public string MainInvoiceNum { get; set; }
        public DateTime? MyDate { get; set; }
        public bool? IsDollar { get; set; }
        public string CreatedUsername { get; set; }
        public string ModifiedUsername { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
