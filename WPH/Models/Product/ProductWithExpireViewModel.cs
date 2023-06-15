using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductWithExpireViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal Stock { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string SaleType { get; set; }
        //public decimal? SalePrice { get; set; }
        //public decimal? MiddlePrice { get; set; }
        //public decimal? WholePrice { get; set; }
        //public int SaleCurrencyId { get; set; }
    }
}
