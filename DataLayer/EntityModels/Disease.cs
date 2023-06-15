using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Disease
    {
        public Disease()
        {
            DiseaseSymptoms = new HashSet<DiseaseSymptom>();
            MedicineDiseases = new HashSet<MedicineDisease>();
            PatientDiseaseRecords = new HashSet<PatientDiseaseRecord>();
            VisitPatientDiseases = new HashSet<VisitPatientDisease>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? DiseaseTypeId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral DiseaseType { get; set; }
        public virtual ICollection<DiseaseSymptom> DiseaseSymptoms { get; set; }
        public virtual ICollection<MedicineDisease> MedicineDiseases { get; set; }
        public virtual ICollection<PatientDiseaseRecord> PatientDiseaseRecords { get; set; }
        public virtual ICollection<VisitPatientDisease> VisitPatientDiseases { get; set; }
    }
}
