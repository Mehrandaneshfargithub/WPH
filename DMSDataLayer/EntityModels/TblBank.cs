using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblBank
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InvoiceId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime Date { get; set; }
        public int FromType { get; set; }
        public decimal Amount { get; set; }
        public decimal? InvoiceCost { get; set; }
        public bool PaidRemoval { get; set; }
        public bool IsDollar { get; set; }
        public string Desc { get; set; }
        public string QabzNum { get; set; }
        public int? CostTypeId { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public string StatusC { get; set; }
        public int? CostFromType { get; set; }
        public int? PayId { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
    }
}
