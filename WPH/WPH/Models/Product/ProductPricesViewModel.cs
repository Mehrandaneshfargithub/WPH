using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductPricesViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDate { get; set; }
        public string ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public string RemainingNum { get; set; }
        public string PurchasePrice { get; set; }
        public string SellingPrice { get; set; }
        public string WholeSellPrice { get; set; }
        public string MiddleSellPrice { get; set; }
        public string Consideration { get; set; }
    }
}
