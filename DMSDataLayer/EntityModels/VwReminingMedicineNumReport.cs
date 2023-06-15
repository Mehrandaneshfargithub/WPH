using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwReminingMedicineNumReport
    {
        public int Id { get; set; }
        public string JoineryName { get; set; }
        public int? ProducerId { get; set; }
        public int OrderPoint { get; set; }
        public decimal? CurrentNum { get; set; }
        public int? FormId { get; set; }
        public string ProducerName { get; set; }
        public string FormName { get; set; }
        public string SuppliersName { get; set; }
    }
}
