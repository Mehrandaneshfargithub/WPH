using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class TmpVisit
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int UniqueVisitNum { get; set; }
        public int VisitNum { get; set; }
        public Guid ReserveDetailId { get; set; }
        public DateTime? VisitDate { get; set; }
        public TimeSpan? VisitTime { get; set; }
        public int? StatusId { get; set; }
        public string Explanation { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public decimal? BloodPressureSys { get; set; }
        public decimal? BloodPressureDia { get; set; }
        public decimal? BodyTemperature { get; set; }
    }
}
