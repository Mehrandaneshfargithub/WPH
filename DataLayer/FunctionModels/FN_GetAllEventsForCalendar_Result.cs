using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class FN_GetAllEventsForCalendar_Result
    {
        public Nullable<System.Guid> GUID { get; set; }
        public string Name { get; set; }
        public string FileNum { get; set; }
        public string FormNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
        public string StatusName { get; set; }
        public bool? OldVisit { get; set; }
        public bool? LastVisit { get; set; }
        public decimal? Amount { get; set; }
        public string Explanation { get; set; }
        public decimal? Remain { get; set; }
        public bool? UseFormNum { get; set; }
    }
}
