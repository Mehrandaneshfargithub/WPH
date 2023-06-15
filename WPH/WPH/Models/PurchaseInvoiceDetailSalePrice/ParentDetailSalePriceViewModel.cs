using System;

namespace WPH.Models.PurchaseInvoiceDetailSalePrice
{
    public class ParentDetailSalePriceViewModel
    {
        public Guid PurchaseInvoiceDetailId { get; set; }
        public Guid TransferDetailId { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }
    }
}
