using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class PurchaseOrTransferProductDetail
    {
        public Guid Guid { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal TotalStock { get; set; }
        public decimal Stock { get; set; }
        public int? SellingCurrencyId { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? FirstPrice { get; set; }
        public string SellingCurrencyName { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string PurchaseCurrencyName { get; set; }
        public int? CurrencyId { get; set; }
        public string SaleType { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public string Consideration { get; set; }
        public string BujNumber { get; set; }
    }
}

