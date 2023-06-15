using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.Patient;

namespace WPH.Models.CustomDataModels.PatientDisease
{
    public class PatientDiseaseRecordViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? Patientid { get; set; }
        public Guid? DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public virtual DiseaseViewModel Disease { get; set; }
        public virtual PatientViewModel Patient { get; set; }
    }
}