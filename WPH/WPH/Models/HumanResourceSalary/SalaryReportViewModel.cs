using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.HumanResourceSalary
{
    public class SalaryReportViewModel
    {
        public Guid? HumanResource { get; set; }
        public int? PaymentStatus { get; set; }
        public int? SalaryType { get; set; }
        public DateTime FromDate { get; set; }
        public string TxtFromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TxtToDate { get; set; }
        public Guid ClinicSectionId { get; set; }
        public bool Detail { get; set; }
        public int OrderBy { get; set; }

        public List<Guid> AllClinicSectionGuids { get; set; }
    }
}
