using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Symptom;

namespace WPH.Models.CustomDataModels.Disease
{
    public class Disease_SymptomViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public System.Guid DiseaseId { get; set; }
        public System.Guid SymptomId { get; set; }
        public string Explanation { get; set; }

        public virtual SymptomViewModel Symptom { get; set; }
        public virtual DiseaseViewModel Disease { get; set; }
    }
}