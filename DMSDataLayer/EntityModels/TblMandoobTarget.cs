using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMandoobTarget
    {
        public int Id { get; set; }
        public int? MandoobId { get; set; }
        public int? MedicineId { get; set; }
        public DateTime? Datefrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? Num { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
    }
}
