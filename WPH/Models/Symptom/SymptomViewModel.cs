using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.Visit_Symptom;

namespace WPH.Models.CustomDataModels.Symptom
{
    public class SymptomViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public Nullable<System.Guid> ClinicSectionId { get; set; }
        public virtual ICollection<Disease_SymptomViewModel> Disease_Symptom { get; set; }
        public virtual ICollection<Visit_SymptomViewModel> Visit_Symptom { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
    }
}