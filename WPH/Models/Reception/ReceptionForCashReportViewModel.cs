using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Reception
{
    public class ReceptionForCashReportViewModel
    {
        public Guid Guid { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceNum { get; set; }
        public string PatientName { get; set; }
        public string Operation { get; set; }
        public Guid DoctorId { get; set; }
        public string SurgeryName { get; set; }
        public string SurgeryWage => TempSurgeryWage.ToString("N0");
        public string AnestheticName { get; set; }
        public string AnestheticWage => TempAnestheticWage.ToString("N0");
        public string ChilderenDoctorWage => TempChilderenDoctorWage.ToString("N0");
        public string ResidentWage => TempResidentWage.ToString("N0");
        public string OperationPrice => (TempOperationPrice + TempSurgeryWage).ToString("N0");
        public string TreatmentStaffWage => TempTreatmentStaffWage.ToString("N0");
        public string PrematureCadres => TempPrematureCadres.ToString("N0");
        public string SentinelCadre => TempSentinelCadre.ToString("N0");
        public string ServicePrice => TempService.ToString("N0");
        public string HospitalRemaining => (TempServicePrice - TempTreatmentStaffWage - TempSentinelCadre - TempPrematureCadres - TempAnestheticWage - TempResidentWage - TempChilderenDoctorWage).ToString("N0");
        public int? SurgeryId { get; set; }
        public string TotalOpertion { get; set; }
        public string CountOpertion { get; set; }
        public string Description { get; set; }


        public decimal TempSurgeryWage { get; set; }
        public decimal TempChilderenDoctorWage { get; set; }
        public decimal TempResidentWage { get; set; }
        public decimal TempAnestheticWage { get; set; }
        public decimal TempTreatmentStaffWage { get; set; }
        public decimal TempSentinelCadre { get; set; }
        public decimal TempPrematureCadres { get; set; }
        public decimal TempOperationPrice { get; set; }
        public decimal TempServicePrice => TempOperationPrice + TempService;
        public decimal TempService { get; set; }
    }
}
