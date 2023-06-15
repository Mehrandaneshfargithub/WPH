using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Surgery
{
    public class SurgeryGridViewModel : IndexViewModel
    {
        public Guid? Guid { get; set; }
        public int Index { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string PatientName { get; set; }
        public string SurgeryOneName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string OperationName { get; set; }
        public int? Age { get; set; }
    }
}
