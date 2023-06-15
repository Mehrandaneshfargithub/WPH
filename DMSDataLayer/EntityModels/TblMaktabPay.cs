using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMaktabPay
    {
        public TblMaktabPay()
        {
            TblExchangeInvoicePays = new HashSet<TblExchangeInvoicePay>();
        }

        public int Id { get; set; }
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsDollar { get; set; }
        public string Desc { get; set; }
        public int MaktabId { get; set; }
        public bool IsPaid { get; set; }
        public int CreatedUserId { get; set; }
        public int ModifiedUserId { get; set; }
        public int? InvoiceId { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblMaktab Maktab { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual ICollection<TblExchangeInvoicePay> TblExchangeInvoicePays { get; set; }
    }
}
