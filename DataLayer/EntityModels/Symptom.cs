using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Symptom
    {
        public Symptom()
        {
            DiseaseSymptoms = new HashSet<DiseaseSymptom>();
            VisitSymptoms = new HashSet<VisitSymptom>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual ICollection<DiseaseSymptom> DiseaseSymptoms { get; set; }
        public virtual ICollection<VisitSymptom> VisitSymptoms { get; set; }
    }
}
