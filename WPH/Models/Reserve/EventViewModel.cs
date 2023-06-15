using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Reserve
{
    public class EventViewModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string FileNum { get; set; }
        public string FormNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
        public string StatusName { get; set; }
        public bool? OldVisit { get; set; }
        public bool? LastVisit { get; set; }
        public decimal Amount { get; set; }
        public string Explanation { get; set; }
        public string Remain { get; set; }
        public bool? UseFormNum { get; set; }
        public DateTime? ReserveDate { get; set; }
    }
}