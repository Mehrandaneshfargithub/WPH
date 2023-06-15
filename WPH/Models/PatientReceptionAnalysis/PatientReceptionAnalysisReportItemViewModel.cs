using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.PatientReceptionAnalysis
{
    public class PatientReceptionAnalysisReportItemViewModel
    {
        public string Name { get; set; }
        public string Price => Temp_Price.ToString("N0");
        public decimal Temp_Price { get; set; }
    }
}
