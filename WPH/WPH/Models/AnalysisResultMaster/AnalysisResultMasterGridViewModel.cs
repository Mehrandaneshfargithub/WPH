using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.AnalysisResultMaster
{
    public class AnalysisResultMasterGridViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid? Guid { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? PrintedNum { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ClinicSectionTypeName { get; set; }
        public string PatientReceptionRemained { get; set; }
        public int? Age { get; set; }
        public int? ServerNumber { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
