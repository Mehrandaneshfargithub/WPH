using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Child
{
    public class NewBornBabiesReportViewModel
    {
        public string Name { get; set; }
        public string GenderName { get; set; }
        public string BirthDate { get; set; } 
        public string BirthTime { get; set; }
        public string StatusName { get; set; }
        public string Weight { get; set; }
        public string VitalActivities { get; set; }
        public string CongenitalAnomalies { get; set; }
        public string OperationOrder { get; set; }
        public string BirthDateTime => $"{BirthDate} {BirthTime}";
        public string Count { get; set; }
        public string ReceptionDoctor { get; set; }
        public string RecivedDate { get; set; }
    }
}
