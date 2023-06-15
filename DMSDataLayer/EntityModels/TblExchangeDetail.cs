using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblExchangeDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int SupplierId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public string Consideration { get; set; }

        public virtual TblExchangeInvoice Master { get; set; }
        public virtual TblSupplier Supplier { get; set; }
    }
}
