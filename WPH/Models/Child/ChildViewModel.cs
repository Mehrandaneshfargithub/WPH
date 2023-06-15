using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Child
{
    public class ChildViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public int? GenderId { get; set; }
        public string GenderName { get; set; }
        public TimeSpan? TimeOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateBirth { get; set; }
        public int? ChildStatus { get; set; }
        public string StatusName { get; set; }
        public string VitalActivities { get; set; }
        public string CongenitalAnomalies { get; set; }
        public decimal? Weight { get; set; }
        public string ChildWeight { get; set; }
        public bool? NeedOperation { get; set; }
        public string OperationOrder { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid UserId { get; set; }
    }
}