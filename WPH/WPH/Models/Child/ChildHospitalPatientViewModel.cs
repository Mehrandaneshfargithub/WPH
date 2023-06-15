using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Child
{
    public class ChildHospitalPatientViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string TxtReceptionDate { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid UserId { get; set; }



        public string Name { get; set; }
        public string GenderName { get; set; }
        public string DateOfBirth { get; set; }
        public string DoctorName { get; set; }
        public string RoomName { get; set; }
        public string Weight { get; set; }
        public string DateOfReception { get; set; }
    }
}
