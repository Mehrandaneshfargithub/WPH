using System;

namespace DataLayer.EntityModels
{
    public partial class Child
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ChildStatus { get; set; }
        public string VitalActivities { get; set; }
        public string CongenitalAnomalies { get; set; }
        public decimal? Weight { get; set; }
        public bool? NeedOperation { get; set; }
        public string OperationOrder { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? RoomId { get; set; }


        public virtual BaseInfoGeneral Status { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual User User { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual Room Room { get; set; }
    }
}
