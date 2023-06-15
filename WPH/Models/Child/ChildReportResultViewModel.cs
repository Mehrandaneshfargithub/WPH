using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Child
{
    public class ChildReportResultViewModel
    {
        public List<NewBornBabiesReportViewModel> AllChildren { get; set; }
        public List<NewBornBabiesReportViewModel> ChildrenStatus { get; set; }
         
        public string TotalChildren { get; set; }
    }
}
