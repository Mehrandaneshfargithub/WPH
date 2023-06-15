using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.CustomDataModels.PrescriptionTest;
using WPH.Models.CustomDataModels.PrescrptionDetail;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.Visit_Patient_Disease;
using WPH.Models.CustomDataModels.Visit_Symptom;
using WPH.Models.Reception;

namespace WPH.Models.CustomDataModels.Visit
{
    public class VisitViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int VisitNum { get; set; }
        public Guid? ReserveDetailId { get; set; }
        public DateTime? VisitDate { get; set; }
        public TimeSpan? VisitTime { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string Explanation { get; set; }
        public Guid ClinicSectionId { get; set; } 
        public Guid OriginalClinicSectionId { get; set; } 
        public string UniqueVisitNum { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? PatientId { get; set; }
        public string ReserveStartTime { get; set; }
        public string FileNum { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDateOfBirth { get; set; }
        public int? PatientAge { get; set; }
        public int? GenderId { get; set; }
        public long? ServerVisitNum { get; set; }
        public long? AnalysisServerVisitNum { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }


        public string AllPrescriptionDetail { get; set; }
        public string AllPrescriptionTestDetail { get; set; }
        public List<ClinicSectionChoosenValueViewModel> AllClinicSectionChoosenValues { get; set; }
        public List<List<PatientVariablesValueViewModel>> AllPreviousValues { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        public virtual BaseInfoGeneralViewModel Status { get; set; }
        public virtual ICollection<PrescriptionDetailViewModel> PrescriptionDetails { get; set; }
        public virtual ICollection<PrescriptionTestDetailViewModel> PrescriptionTestDetails { get; set; }
        public virtual ReserveDetailViewModel ReserveDetail { get; set; }

        public virtual ICollection<Visit_Patient_DiseaseViewModel> Visit_Patient_Disease { get; set; }
        public virtual ICollection<Visit_SymptomViewModel> Visit_Symptom { get; set; }
        public List<PatientVariablesValueViewModel> AddVariables { get; set; }
        public List<PatientVariablesValueViewModel> UpdatedVariables { get; set; }
        public List<Guid> AllDiseaseForVisitList { get; set; }
        public List<Guid> AllSymptomForVisitList { get; set; }
    }
}