using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.HospitalPatients
{
    public class ShowPatientToAnotherSectionReportResultViewModel
    {
        public List<PatientToAnotherSectionReportViewModel> Human { get; set; }
        public List<PatientToAnotherSectionReportViewModel> Section { get; set; }

        public string Total { get; set; }
    }
}
