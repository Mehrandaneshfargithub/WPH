using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblExchangeInvoice
    {
        public TblExchangeInvoice()
        {
            TblExchangeDetails = new HashSet<TblExchangeDetail>();
            TblExchangeInvoicePays = new HashSet<TblExchangeInvoicePay>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int MaktabId { get; set; }
        public decimal? CostDollar { get; set; }
        public int? CreatedUserId { get; set; }
        public bool? IsDollar { get; set; }
        public int ModifiedUserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Desc { get; set; }
        public decimal? CostDinar { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblMaktab Maktab { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual ICollection<TblExchangeDetail> TblExchangeDetails { get; set; }
        public virtual ICollection<TblExchangeInvoicePay> TblExchangeInvoicePays { get; set; }
    }
}
