using System;

namespace DataLayer.EntityModels
{
    public class ProductPurchaseReportModel
    {
        public Guid Guid { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string MainInvoiceNum { get; set; }
        public string Name { get; set; }
        public DateTime ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Discount { get; set; }
        public string Consideration { get; set; }
        public string CurrencyName { get; set; }
    }
}
