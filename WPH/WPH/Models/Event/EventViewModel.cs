using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Event
{
    public class EventViewModel
    {
        public Nullable<System.Guid> Guid { get; set; }
        public string Name { get; set; }
        public string FileNum { get; set; }
        public string FormNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
        public string StatusName { get; set; }
        public Nullable<bool> OldVisit { get; set; }
        public Nullable<bool> LastVisit { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Remain { get; set; }
        public Nullable<bool> UseFormNum { get; set; }
    }
}