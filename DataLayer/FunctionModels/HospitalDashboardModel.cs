using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.FunctionModels
{
    public class HospitalDashboardModel
    {
        public string RoomName { get; set; }
        public string BedName { get; set; }
        public string BedStatus { get; set; }
        public string PatientName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public string Surgery { get; set; }
        public string Doctor { get; set; }
    }
}
