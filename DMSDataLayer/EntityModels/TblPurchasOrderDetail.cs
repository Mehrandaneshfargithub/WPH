using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPurchasOrderDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public decimal? Num { get; set; }
        public string Consideration { get; set; }
        public int UserId { get; set; }
        public int? MedicineId { get; set; }
        public string JoineryName { get; set; }

        public virtual TblUser User { get; set; }
    }
}
