using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.PatientReceptionAnalysis
{
    public class PatientReceptionAnalysisReportViewModel
    {
        public List<PatientReceptionAnalysisReportItemViewModel> Items { get; set; }

        public string Total { get; set; }
    }
}
