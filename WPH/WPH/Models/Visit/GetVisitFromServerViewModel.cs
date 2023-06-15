using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Analysis;
using WPH.Models.Medicine;

namespace WPH.Models.Visit
{
    public class GetVisitFromServerViewModel
    {
        public long MasterId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public List<GetMedicineFromServerViewModel> Medicines { get; set; }
        public List<GetAnalysisFromServerViewModel> Analysis { get; set; }
    }
}
