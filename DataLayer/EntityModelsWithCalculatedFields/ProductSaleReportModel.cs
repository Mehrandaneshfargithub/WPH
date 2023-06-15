using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class ProductSaleReportModel
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
        public string PurchaseCurrencyName { get; set; }
        public string SaleCurrencyName { get; set; }
        public decimal SalePrice { get; set; }
        public int CurrencyId { get; set; }
        public MoneyConvert MoneyConvert { get; set; }
    }
}
