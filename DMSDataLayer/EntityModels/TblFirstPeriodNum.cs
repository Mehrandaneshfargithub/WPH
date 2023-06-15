using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblFirstPeriodNum
    {
        public int Id { get; set; }
        public int? PeriodId { get; set; }
        public int? MedicineId { get; set; }
        public int? Num { get; set; }
    }
}
