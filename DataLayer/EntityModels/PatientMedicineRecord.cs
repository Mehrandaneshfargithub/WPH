using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientMedicineRecord
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicineId { get; set; }

        public virtual Medicine Medicine { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
