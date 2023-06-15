using System;

namespace DataLayer.EntityModels
{
    public class ReturnProductPurchaseReportModel
    {
        public Guid Guid { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Name { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Discount { get; set; }
        public string CurrencyName { get; set; }
        public string Reason { get; set; }
    }
}
