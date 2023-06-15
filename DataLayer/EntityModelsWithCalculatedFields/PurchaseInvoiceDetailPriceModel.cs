using System;

namespace DataLayer.EntityModels
{
    public class PurchaseInvoiceDetailPriceModel
    {
        public Guid Guid { get; set; }
        public string InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public decimal RemainingNum { get; set; }
        public string CurrencyName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal WholeSellPrice { get; set; }
        public decimal MiddleSellPrice { get; set; }
        public string Consideration { get; set; }
    }
}
