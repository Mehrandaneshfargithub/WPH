using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.ClinicCalendar
{
    public class ClinicCalendarViewModel
    {
        public Nullable<Guid> Guid { get; set; }
        public string Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Interval { get; set; }
    }
}
