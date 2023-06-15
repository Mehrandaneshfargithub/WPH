using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Analysis;
using WPH.Models.Medicine;

namespace WPH.Models.Visit
{
    public class SendVisitToServerViewModel
    {
        public long MasterId { get; set; }
        public Guid? ClinicId { get; set; }
        public Guid? DoctorClinicId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string Mobile { get; set; }

        public List<SendMedicineToServerViewModel> AddMedicines { get; set; }
        public List<SendAnalysisToServerViewModel> AddAnalysis { get; set; }
    }
}
