using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class ProductCardexReportModel
    {
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Name { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? InNum { get; set; }
        public decimal? InFree { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? OutNum { get; set; }
        public decimal? OutFreeNum { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public string CurrencyName { get; set; }
        public decimal? RemainingNum { get; set; }
    }
}
