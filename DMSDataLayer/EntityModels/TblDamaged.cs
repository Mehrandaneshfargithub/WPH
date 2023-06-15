using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblDamaged
    {
        public TblDamaged()
        {
            TblDamagedDetails = new HashSet<TblDamagedDetail>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public int? ReasonId { get; set; }
        public int? CostTypeId { get; set; }
        public decimal? Discount { get; set; }
        public bool? Ended { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? WarehouseId { get; set; }
        public bool? PurchasePriceChanged { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual TblBaseInfo Reason { get; set; }
        public virtual TblUser User { get; set; }
        public virtual ICollection<TblDamagedDetail> TblDamagedDetails { get; set; }
    }
}
