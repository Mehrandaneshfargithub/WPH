using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Product
{
    public class ProductWithBarcodeViewModel
    {
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
    }
}
