using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Reserve
    {
        public Reserve()
        {
            ReserveDetails = new HashSet<ReserveDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? RoundTime { get; set; }
        public string Explanation { get; set; }
        public Guid ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual ICollection<ReserveDetail> ReserveDetails { get; set; }
    }
}
