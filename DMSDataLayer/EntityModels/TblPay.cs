using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPay
    {
        public int Id { get; set; }
        public int? SuppliersId { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Amount { get; set; }
        public string Desc { get; set; }
        public bool? IsPaid { get; set; }
        public int? InvoiceId { get; set; }
        public string QabzNum { get; set; }
        public bool? IsDollar { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool? FullPay { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual TblSupplier Suppliers { get; set; }
    }
}
