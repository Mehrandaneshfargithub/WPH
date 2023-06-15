using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblAlarmSetting
    {
        public int Id { get; set; }
        public bool? NumLessEqualOrderPoint { get; set; }
        public bool? ExpireDateLessThan { get; set; }
        public DateTime? ExpireDateLessThanIs { get; set; }
        public bool? ExpireDateNextYear { get; set; }
        public bool? ExpireDateNextMonth { get; set; }
        public bool? ExpireDateNextWeek { get; set; }
        public bool? ExpireDateToday { get; set; }
        public bool? ExpireDatePassed { get; set; }
        public bool? DaySale { get; set; }
        public bool? WeekSale { get; set; }
        public bool? MonthSale { get; set; }
        public bool? YearSale { get; set; }
    }
}
