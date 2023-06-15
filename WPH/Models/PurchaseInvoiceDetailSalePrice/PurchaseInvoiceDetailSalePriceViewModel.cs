using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.PurchaseInvoiceDetailSalePrice
{
    public class PurchaseInvoiceDetailSalePriceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? TransferDetailId { get; set; }
        public int? BaseCurrencyId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public string MoneyConvertName { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public Guid UserId { get; set; }
                    

        public string SellingPrice { get; set; }
        public string MiddleSellPrice { get; set; }
        public string WholeSellPrice { get; set; }

        public Guid ClinicSectionId { get; set; }
        public decimal? BaseAmount { get; set; }
        public string BaseAmountTxt { get; set; }
        public decimal? DestAmount { get; set; }
        public string DestAmountTxt { get; set; }
        public string AmountTxt { get; set; }
    }
}
