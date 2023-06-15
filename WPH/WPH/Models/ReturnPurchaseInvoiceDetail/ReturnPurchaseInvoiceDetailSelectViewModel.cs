using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnPurchaseInvoiceDetail
{
    public class ReturnPurchaseInvoiceDetailSelectViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

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

        public string ExpireDateTxt => ExpireDate?.ToString("dd/MM/yyyy", cultures);
        public string InvoiceDateTxt => InvoiceDate?.ToString("dd/MM/yyyy", cultures);
        public string ReturnType => TransferDetailId == null ? "Purchase" : "Transfer";
        public string PurchaseCurrency => $"{Currency} {PurchasePrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public string SellingPriceCurrency => $"{Currency} {SellingPrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public decimal? Total => Num.GetValueOrDefault(0) * PurchasePrice.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => Total - Discount.GetValueOrDefault(0);

    }
}
