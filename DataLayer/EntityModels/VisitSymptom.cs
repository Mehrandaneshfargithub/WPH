using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class VisitSymptom
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid SymptomId { get; set; }
        public string Explanation { get; set; }
        public Guid? ReceptionId { get; set; }

        public virtual Reception Reception { get; set; }
        public virtual Symptom Symptom { get; set; }
    }
}
