using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.ReserveDetail;

namespace WPH.Models.CustomDataModels.Reserve
{
    public class ReserveViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoundTime { get; set; }
        public string Explanation { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public int StartTimeHours => StartTime.Hours;
        public int EndTimeHours => EndTime.Hours;
        public IEnumerable<ReserveDetailViewModel> ReserveDetails { get; set; }

    }
}