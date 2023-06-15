using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Reminder
{
    public class ReminderViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string Explanation { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string TxtReminderDate { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
    }
}