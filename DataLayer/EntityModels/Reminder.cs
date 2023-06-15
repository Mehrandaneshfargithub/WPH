using System;

namespace DataLayer.EntityModels
{
    public partial class Reminder
    {
        public Guid Guid { get; set; }
        public string Explanation { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual User ModifiedUser { get; set; }
    }
}
