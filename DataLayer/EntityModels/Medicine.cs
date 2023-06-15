using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Medicine
    {
        public Medicine()
        {
            MedicineDiseases = new HashSet<MedicineDisease>();
            PatientMedicineRecords = new HashSet<PatientMedicineRecord>();
            PrescriptionDetails = new HashSet<PrescriptionDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string JoineryName { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public Guid? ProducerId { get; set; }
        public Guid? MedicineFormId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? Priority { get; set; }
        public decimal? MedNum { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfo MedicineForm { get; set; }
        public virtual BaseInfo Producer { get; set; }
        public virtual ICollection<MedicineDisease> MedicineDiseases { get; set; }
        public virtual ICollection<PatientMedicineRecord> PatientMedicineRecords { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
    }
}
