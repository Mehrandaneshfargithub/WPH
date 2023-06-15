using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSale1
    {
        public string Name { get; set; }
        public double? SalePriceDollar { get; set; }
        public string Barcode { get; set; }
        public string SourceOfDrug { get; set; }
        public string Type { get; set; }
        public string Text74 { get; set; }
        public double? DdPrice { get; set; }
        public string Text80 { get; set; }
    }
}
