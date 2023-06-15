using System;

namespace WPH.Models.CustomDataModels.Visit_Patient_Disease
{
    public class Visit_Patient_DiseaseViewModel
    {
        public Guid Guid { get; set; }
        
        public Guid VisitId { get; set; }
        public Guid DiseaseId { get; set; }
        
    }
}