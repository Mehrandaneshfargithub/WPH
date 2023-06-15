using System.Collections.Generic;

namespace WPH.Models.HumanResourceSalary
{
    public class SalaryReportResultViewModel
    {
        public List<HumanResourceSalaryReportViewModel> AllSalary { get; set; }
        public List<HumanResourceSalaryReportViewModel> AllSalarySection { get; set; }
        public List<HumanResourceSalaryReportViewModel> AllHuman { get; set; }

        public string AllPay { get; set; }
    }
}
