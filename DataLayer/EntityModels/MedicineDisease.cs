using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class MedicineDisease
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid DiseaseId { get; set; }
        public string Explanation { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
