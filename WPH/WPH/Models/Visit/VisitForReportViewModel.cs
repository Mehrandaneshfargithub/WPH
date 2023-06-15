using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Visit
{
    public class VisitForReportViewModel
    {
        public Guid Guid{ get; set; }
        public string DoctorName { get; set; }
        public string Explanation { get; set; }
        public string LogoAddress { get; set; }
        public string PatientName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? VisitDate { get; set; }
        public string UniqueVisitNum { get; set; }
    }
}
