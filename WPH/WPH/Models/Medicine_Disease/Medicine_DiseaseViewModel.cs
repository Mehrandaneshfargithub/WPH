using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Medicine;

namespace WPH.Models.CustomDataModels.Disease
{
    public class Medicine_DiseaseViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public System.Guid MedicineId { get; set; }
        public System.Guid DiseaseId { get; set; }
        public string Explanation { get; set; }
        public string MedicineJoineryName { get; set; }
        public virtual MedicineViewModel Medicine { get; set; }
        public virtual DiseaseViewModel Disease { get; set; }
    }
}