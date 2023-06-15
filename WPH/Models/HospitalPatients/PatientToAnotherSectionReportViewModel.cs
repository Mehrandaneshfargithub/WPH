using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.HospitalPatients
{
    public class PatientToAnotherSectionReportViewModel
    {
        public string PatientName { get; set; }
        public string Date { get; set; }
        public string Section { get; set; }
        public string Analysis { get; set; }
        public string Amount => TempAmount.GetValueOrDefault(0).ToString("N0");
        public string AnalysisCount { get; set; }

        public decimal? TempAmount { get; set; }
    }
}
