using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCombinedMedicine
    {
        public int Id { get; set; }
        public int MasterMedicineId { get; set; }
        public int MedicineId { get; set; }
    }
}
