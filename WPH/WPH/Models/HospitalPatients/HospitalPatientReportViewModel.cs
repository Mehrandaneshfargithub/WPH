using System;
using WPH.Models.BaseInfo;

namespace WPH.Models.HospitalPatients
{
    public class HospitalPatientReportViewModel
    {
        public int periodId { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public SectionViewModel section { get; set; }
        public int status { get; set; }


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
