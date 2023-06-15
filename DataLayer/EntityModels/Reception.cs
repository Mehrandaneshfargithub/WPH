using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Reception
    {
        public Reception()
        {
            AnalysisResultMasters = new HashSet<AnalysisResultMaster>();
            Children = new HashSet<Child>();
            PatientImages = new HashSet<PatientImage>();
            PatientReceptionAnalyses = new HashSet<PatientReceptionAnalysis>();
            PatientVariablesValues = new HashSet<PatientVariablesValue>();
            PrescriptionDetails = new HashSet<PrescriptionDetail>();
            PrescriptionTestDetails = new HashSet<PrescriptionTestDetail>();
            ReceptionAmbulances = new HashSet<ReceptionAmbulance>();
            ReceptionClinicSectionDestinations = new HashSet<ReceptionClinicSection>();
            ReceptionClinicSections = new HashSet<ReceptionClinicSection>();
            ReceptionDetailPays = new HashSet<ReceptionDetailPay>();
            ReceptionDoctors = new HashSet<ReceptionDoctor>();
            ReceptionInsurances = new HashSet<ReceptionInsurance>();
            ReceptionRoomBeds = new HashSet<ReceptionRoomBed>();
            ReceptionServices = new HashSet<ReceptionService>();
            ReceptionTemperatures = new HashSet<ReceptionTemperature>();
            Surgeries = new HashSet<Surgery>();
            VisitPatientDiseases = new HashSet<VisitPatientDisease>();
            VisitSymptoms = new HashSet<VisitSymptom>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string ReceptionNum { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? EntranceDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public int? ReceptionTypeId { get; set; }
        public Guid? PatientId { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public string PatientAttendanceName { get; set; }
        public string PoliceReport { get; set; }
        public int? ClearanceTypeId { get; set; }
        public bool? Active { get; set; }
        public int? PaymentStatusId { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? PurposeId { get; set; }
        public string ChiefComplaint { get; set; }
        public string Examination { get; set; }
        public bool? Discharge { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public bool? HospitalReception { get; set; }
        public int? VisitNum { get; set; }
        public Guid? ReserveDetailId { get; set; }
        public int? StatusId { get; set; }
        public long? ServerVisitNum { get; set; }
        public long? AnalysisServerVisitNum { get; set; }

        public virtual BaseInfoGeneral ClearanceType { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral DiscountCurrency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual BaseInfoGeneral PaymentStatus { get; set; }
        public virtual BaseInfoGeneral Purpose { get; set; }
        public virtual BaseInfoGeneral ReceptionType { get; set; }
        public virtual ReserveDetail ReserveDetail { get; set; }
        public virtual BaseInfoGeneral Status { get; set; }
        public virtual Emergency Emergency { get; set; }
        public virtual ICollection<AnalysisResultMaster> AnalysisResultMasters { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ICollection<PatientImage> PatientImages { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
        public virtual ICollection<PatientVariablesValue> PatientVariablesValues { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
        public virtual ICollection<PrescriptionTestDetail> PrescriptionTestDetails { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulances { get; set; }
        public virtual ICollection<ReceptionClinicSection> ReceptionClinicSectionDestinations { get; set; }
        public virtual ICollection<ReceptionClinicSection> ReceptionClinicSections { get; set; }
        public virtual ICollection<ReceptionDetailPay> ReceptionDetailPays { get; set; }

        public virtual ICollection<ReceptionDoctor> ReceptionDoctors { get; set; }
        public virtual ICollection<ReceptionInsurance> ReceptionInsurances { get; set; }
        public virtual ICollection<ReceptionRoomBed> ReceptionRoomBeds { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
        public virtual ICollection<ReceptionTemperature> ReceptionTemperatures { get; set; }
        public virtual ICollection<Surgery> Surgeries { get; set; }
        public virtual ICollection<VisitPatientDisease> VisitPatientDiseases { get; set; }
        public virtual ICollection<VisitSymptom> VisitSymptoms { get; set; }
    }
}
