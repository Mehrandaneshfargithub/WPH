using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.PatientMedicine;
using WPH.Models.CustomDataModels.PrescrptionDetail;

namespace WPH.Models.CustomDataModels.Medicine
{
    public class MedicineViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public string JoineryName { get; set; }
        public string JoineryNameHolder { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public decimal MedNum { get; set; }
        public string CodeHolder { get; set; }
        public Nullable<Guid> ProducerId { get; set; }
        public Nullable<Guid> MedicineFormId { get; set; }
        public string MedicineFormName { get; set; }
        public string ProducerName { get; set; }
        public List<BaseInfoViewModel> Producers { get; set; }
        public List<BaseInfoViewModel> MedicineForms { get; set; }
        public Nullable<Guid> ClinicSectionid { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Num { get; set; }
        public string Consumption { get; set; }
        public string Explanation { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual ICollection<Medicine_DiseaseViewModel> Medicine_Disease { get; set; }
        public virtual BaseInfoViewModel MedicineForm { get; set; }
        public virtual BaseInfoViewModel Producer { get; set; }
        public virtual ICollection<PrescriptionDetailViewModel> PrescriptionDetails { get; set; }
        public virtual ICollection<PatientMedicineRecordViewModel> PatientMedicineRecords { get; set; }
    }
}