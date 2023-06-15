using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSuppAcc4
    {
        public int? Id { get; set; }
        public int? SuppliersId { get; set; }
        public string SuppliersName { get; set; }
        public string QabzNum { get; set; }
        public DateTime? MyDate { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? RecievedAmount { get; set; }
        public decimal? RecievedAmountSecond { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PaidAmountSecond { get; set; }
        public string Desc { get; set; }
        public decimal? TotalSupplierRecieved { get; set; }
        public decimal? TotalSupplierRecievedSecond { get; set; }
        public decimal? TotalSupplierPaid { get; set; }
        public decimal? TotalSupplierPaidSecond { get; set; }
        public decimal? TotalSupplierPurchase { get; set; }
        public decimal? TotalSupplierPurchaseSecond { get; set; }
        public decimal? TotalSupplierReturnPurchase { get; set; }
        public decimal? TotalSupplierReturnPurchaseSecond { get; set; }
        public int? Type { get; set; }
        public decimal? RemainAmount { get; set; }
        public decimal? RemainAmountSecond { get; set; }
        public decimal? EndRemain { get; set; }
        public decimal? EndRemainSecond { get; set; }
        public int? PaidFactor { get; set; }
        public string MainInvoiceNum { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
