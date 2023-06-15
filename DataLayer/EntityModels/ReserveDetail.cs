using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ReserveDetail
    {
        public ReserveDetail()
        {
            Receptions = new HashSet<Reception>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid MasterId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? SecretaryId { get; set; }
        public Guid? DoctorId { get; set; }
        public int? StatusId { get; set; }
        public TimeSpan? ReservedTime { get; set; }
        public string Explanation { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
        public bool? LastVisit { get; set; }
        public DateTime? ReserveDate { get; set; }
        public bool? OldVisit { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Reserve Master { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Secretary Secretary { get; set; }
        public virtual BaseInfoGeneral Status { get; set; }
        public virtual ICollection<Reception> Receptions { get; set; }
    }
}
