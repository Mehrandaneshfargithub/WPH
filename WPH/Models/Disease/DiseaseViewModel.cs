using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.Models.CustomDataModels.Visit_Patient_Disease;

namespace WPH.Models.CustomDataModels.Disease
{
    public class DiseaseViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string Explanation { get; set; }
        public int? DiseaseTypeId { get; set; }
        public string DiseaseTypeName { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public IEnumerable<Guid> AllMedsForDisease { get; set; }
        public IEnumerable<Guid> AllSymptomsForDisease { get; set; }
        
        public virtual BaseInfoGeneralViewModel DiseaseType { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual ICollection<Disease_SymptomViewModel> Disease_Symptom { get; set; }
        public virtual ICollection<Medicine_DiseaseViewModel> Medicine_Disease { get; set; }
        public virtual ICollection<PatientDiseaseRecordViewModel> PatientDiseaseRecords { get; set; }
        public virtual ICollection<Visit_Patient_DiseaseViewModel> Visit_Patient_Disease { get; set; }
    }
}