using System;

namespace WPH.Models.PurchaseInvoiceDetail
{
    public class EditPurchaseInvoiceDetailViewModel
    {
        public Guid Guid { get; set; }
        public Guid? MasterId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string ExpireDateTxt { get; set; }
        public decimal? Num { get; set; }
        public string NumTxt { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string PurchasePriceTxt { get; set; }
        public decimal? SellingPrice { get; set; }
        public string SellingPriceTxt { get; set; }
        public string Consideration { get; set; }
        public decimal? Discount { get; set; }
        public string DiscountTxt { get; set; }
        public decimal? FreeNum { get; set; }
        public string FreeNumTxt { get; set; }
        public decimal? WholePurchasePrice { get; set; }
        public string WholePurchasePriceTxt { get; set; }


        public Guid? ModifiedUserId { get; set; }

        public int? CurrencyId { get; set; }
        public decimal? WholeSellPrice { get; set; }
        public string WholeSellPriceTxt { get; set; }
        public decimal? MiddleSellPrice { get; set; }
        public string MiddleSellPriceTxt { get; set; }
        public string BujNumber { get; set; }

    }
}
