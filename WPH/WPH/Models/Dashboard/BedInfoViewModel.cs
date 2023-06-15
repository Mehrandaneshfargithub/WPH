using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Views.Shared.PartialViews.AppWebForms.Home
{
    public class BedInfoViewModel
    {
        public string BedName { get; set; }
        public string BedStatus { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string ReceptionDate { get; set; }
        public string SurgeryDate { get; set; }
        public string Surgery { get; set; }
        public string Doctor { get; set; }
        public Guid? ReceptionId { get; set; }
    }
}
