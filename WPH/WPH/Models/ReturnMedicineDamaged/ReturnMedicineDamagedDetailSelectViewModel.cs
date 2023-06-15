using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnMedicineDamaged
{
    public class ReturnMedicineDamagedDetailSelectViewModel : IndexViewModel
    {
        CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? TransferDetailId { get; set; }
        public decimal? RemainingNum { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Consideration { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Currency { get; set; }
        public string SellingCurrency { get; set; }

        public string InvoiceDateTxt => InvoiceDate?.ToString("dd/MM/yyyy", cultures);
        public string ReturnType => TransferDetailId == null ? "Purchase" : "Transfer";
        public string PurchaseCurrency => $"{Currency} {PurchasePrice.GetValueOrDefault(0):#,##}";
        public string SellingPriceCurrency => $"{SellingCurrency} {SellingPrice.GetValueOrDefault(0):#,##}";
        public decimal? Total => Num.GetValueOrDefault(0) * PurchasePrice.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => (Num.GetValueOrDefault(0) * PurchasePrice.GetValueOrDefault(0)) - Discount.GetValueOrDefault(0);
    }
}
