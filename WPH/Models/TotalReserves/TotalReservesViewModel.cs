using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.TotalReserves
{
    public class TotalReservesViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int VisitNum { get; set; }
        public Guid ReserveDetailId { get; set; }
        public Nullable<DateTime> VisitDate { get; set; }
        public Nullable<TimeSpan> VisitTime { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Explanation { get; set; }
        public Nullable<Guid> PatientId { get; set; }
        public Nullable<Guid> SecretaryId { get; set; }
        public Nullable<Guid> DoctorId { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
    }
}