using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.UserPortion
{
    public class UserPortionReportViewModel
    {
        public DateTime ReceptionDate { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public decimal ServiceAmount { get; set; }
    }
}
