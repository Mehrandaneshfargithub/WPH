using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Symptom;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.Visit_Symptom
{
    public class Visit_SymptomViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public System.Guid VisitId { get; set; }
        public System.Guid SymptomId { get; set; }
        public string Explanation { get; set; }

        public virtual SymptomViewModel Symptom { get; set; }
        public virtual VisitViewModel Visit { get; set; }
    }
}