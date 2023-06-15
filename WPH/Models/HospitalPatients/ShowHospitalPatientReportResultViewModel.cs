using System;
using System.Collections.Generic;

namespace WPH.Models.HospitalPatients
{
    public class ShowHospitalPatientReportResultViewModel
    {
        public IEnumerable<HospitalPatientReportResultViewModel> CustomPatients { get; set; }
        public IEnumerable<HospitalPatientReportResultViewModel> RemPatients { get; set; }
        public IEnumerable<HospitalPatientCountReportViewModel> SurgeryTypeCount { get; set; }

        public string TotalSurgery { get; set; }
        public string TotalDischarge { get; set; }
        public string TotalNotDischarge { get; set; }
    }
}
