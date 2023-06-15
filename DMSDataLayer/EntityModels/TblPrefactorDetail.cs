using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPrefactorDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int? Num { get; set; }
        public decimal? SalePrice { get; set; }
        public string Consideration { get; set; }
        public int UserId { get; set; }
        public int? MedicineId { get; set; }
        public string JoineryName { get; set; }

        public virtual TblUser User { get; set; }
    }
}
