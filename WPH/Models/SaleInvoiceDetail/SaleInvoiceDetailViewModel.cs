using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Product;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.TransferDetail;

namespace WPH.Models.SaleInvoiceDetail
{
    public class SaleInvoiceDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public decimal? Num { get; set; }
        public decimal? SalePrice { get; set; }
        public string OldSalePrice { get; set; }
        public decimal? FirstPrice { get; set; }
        public string SalePriceTxt { get; set; }
        public string PurchasePrice { get; set; }
        public decimal PurchasePriceNumeric { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public string InvoiceType { get; set; }
        public string PurchasePriceCurrency { get; set; }
        public string PurchasePriceCurrencyId { get; set; }
        public string Consideration { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ExpireDate { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? Discount { get; set; }
        public string DiscountTxt { get; set; }
        public decimal WholesalePrice => (Num.GetValueOrDefault(0) * SalePrice.GetValueOrDefault(0));
        public decimal? Profit { get; set; }
        public Guid? TransferDetailId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int? CurrencyNameId { get; set; }
        public int? FirstCurrencyId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string BujNumber { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public string MoneyConvertTxtId { get; set; }
        public Guid? FirstMoneyConvertId { get; set; }
        public string MoneyConvertTxt { get; set; }
        public bool ChangeNum { get; set; }
        public bool CurrentStock { get; set; }
        public bool NearestExpire { get; set; }
        public int ChildrenCount { get; set; }
        public string ChildrenGuids { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public decimal? RemainingNum { get; set; }

        public virtual BaseInfoGeneralViewModel Currency { get; set; }
        public virtual ProductViewModel Product { get; set; }
        public virtual PurchaseInvoiceDetailViewModel PurchaseInvoiceDetail { get; set; }
        public virtual TransferDetailViewModel TransferDetail { get; set; }
    }
}
