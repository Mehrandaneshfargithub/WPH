using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwNumMidicine
    {
        public int Id { get; set; }
        public int MidicenName { get; set; }
        public decimal? Num { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Expr1 { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Discount { get; set; }
    }
}
