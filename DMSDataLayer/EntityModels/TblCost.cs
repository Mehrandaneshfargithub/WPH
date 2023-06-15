using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCost
    {
        public int Id { get; set; }
        public int? CostTypeId { get; set; }
        public DateTime? Data { get; set; }
        public decimal? Price { get; set; }
        public string Desc { get; set; }
        public int? CostType { get; set; }
        public bool? IsDollar { get; set; }
        public int? SourceId { get; set; }
        public string QabzNum { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? ProducerId { get; set; }
        public string StatusC { get; set; }
        public int? PurchaseInvoiceId { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
    }
}
