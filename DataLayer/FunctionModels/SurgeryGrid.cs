using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.FunctionModels
{
    public class SurgeryGrid
    {
        public Guid? Guid { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string PatientName { get; set; }
        public string SurgeryOneName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string OperationName { get; set; }
    }
}
