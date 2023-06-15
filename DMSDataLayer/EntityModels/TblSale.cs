using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSale
    {
        public double? SalePriceDollar { get; set; }
        public double? SalePriceDinar { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
