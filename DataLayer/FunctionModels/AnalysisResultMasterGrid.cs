using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class AnalysisResultMasterGrid
    {
        public Guid? Guid { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? PrintedNum { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ClinicSectionTypeName { get; set; }
        public int? ServerNumber { get; set; }
        public DateTime? UploadDate { get; set; }

    }
}
