using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class DiseaseSymptom
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid DiseaseId { get; set; }
        public Guid SymptomId { get; set; }
        public string Explanation { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Symptom Symptom { get; set; }
    }
}
