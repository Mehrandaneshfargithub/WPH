using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.HospitalPatients
{
    public class HospitalPatientReportResultViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Kind { get; set; }
        public string DoctorName { get; set; }
        public string RoomId { get; set; }
        public string DateSurgery { get; set; }
        public string DateExit { get; set; }
        public string DateReception { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
    }
}
