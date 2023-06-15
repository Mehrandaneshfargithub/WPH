using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCombinedInvoice
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public string Description { get; set; }
        public int? SaleInvoiceId { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public decimal? HandPrice { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool? PurchaseDollar { get; set; }
        public bool? SaleDollar { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
    }
}
