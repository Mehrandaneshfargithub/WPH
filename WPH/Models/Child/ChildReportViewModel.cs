using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Child
{
    public class ChildReportViewModel
    {
        public DateTime FromDate { get; set; }
        public string TxtFromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TxtToDate { get; set; } 
        public int? GenderId { get; set; }
        public int? ChildStatus { get; set; }
        public bool Detail { get; set; }

        public List<Guid> AllClinicSectionGuids { get; set; }
    }
}
