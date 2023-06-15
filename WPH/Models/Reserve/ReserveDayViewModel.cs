using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Reserve
{
    public class ReserveDayViewModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Explanition { get; set; }
        public string Dur { get; set; }
        public string CalendarDate { get; set; }
        public bool Pasand { get; set; }
        public string Direct { get; set; }
        public Guid ClinicSectionId { get; set; }
    }
}
