using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Product
{
    public class ProductDetailResultViewModel
    {
        public string ProductName { get; set; }
        public string PurchasePrice { get; set; }
        public string SellingPrice { get; set; }
        public string MiddleSellPrice { get; set; }
        public string WholeSellPrice { get; set; }
        public string Profit { get; set; }
        public string Consideration { get; set; }
        public IEnumerable<ProductDetailViewModel> ProductExp { get; set; }
        public IEnumerable<ProductDetailViewModel> ProductBuj { get; set; }
        public IEnumerable<ProductDetailViewModel> Suppliers { get; set; }
    }
}
