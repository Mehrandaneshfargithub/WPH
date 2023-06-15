using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReciever
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? RecieveDate { get; set; }
        public decimal? Amount { get; set; }
        public string Desc { get; set; }
        public bool IsRecieved { get; set; }
        public int? InvoiceId { get; set; }
        public string QabzNum { get; set; }
        public bool? IsDollar { get; set; }
        public decimal? RebhDamaged { get; set; }
        public DateTime? UnderAccount { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool? ReverseRecieved { get; set; }
        public decimal? DinarPerDollar { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblCustomer Customer { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
    }
}
