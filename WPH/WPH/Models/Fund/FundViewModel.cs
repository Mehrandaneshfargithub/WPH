using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Fund
{
    public class FundViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public string Amount { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime Date { get; set; }
        public string ClinicSectionName { get; set; }
        public string AnalysisNumber { get; set; }
        public string RadiologyDoctor { get; set; }

        public decimal? Temp_Amount { get; set; }
    }
}
