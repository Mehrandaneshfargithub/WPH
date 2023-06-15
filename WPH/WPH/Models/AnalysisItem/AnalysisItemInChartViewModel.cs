using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.AnalysisItem
{
    public class AnalysisItemInChartViewModel
    {
        public string AnalysisName { get; set; }
        public string AnalysisArgument { get; set; }
        public float AnalysisValue { get; set; }

        public IEnumerable<AnalysisItemInChartViewModel> History { get; set; }
    }
}
