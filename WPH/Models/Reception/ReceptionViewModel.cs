using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Emergency;
using WPH.Models.ReceptionAmbulance;
using WPH.Models.ReceptionClinicSection;
using WPH.Models.ReceptionRoomBed;
using WPH.Models.ReceptionService;
using WPH.Models.Surgery;
using WPH.Models.CustomDataModels.AnalysisResultMaster;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.PatientReceptionReceived;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.MoneyConvert;
using WPH.Models.ReceptionDoctor;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.Visit;
using Microsoft.AspNetCore.Http;
using WPH.Models.PatientImage;

namespace WPH.Models.Reception
{
    public class ReceptionViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string ReceptionNum { get; set; }
        public string ReceptionDateString { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? EntranceDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string ExitDateDay { get; set; }
        public string ExitDateMonth { get; set; }
        public string ExitDateYear { get; set; }
        public Guid? PatientId { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PatientAttendanceName { get; set; }
        //public string PoliceReport { get; set; }
        public string AddressName { get; set; }
        public string PurposeName { get; set; }
        public string ClinicSectionName { get; set; }
        public string ClinicSectionTypeName { get; set; }
        public int? ReceptionTypeId { get; set; }
        public int? ClearanceTypeId { get; set; }
        public bool? Active { get; set; }
        public int? PaymentStatusId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid OrginalClinicSectionId { get; set; }
        public int? ArrivalId { get; set; }
        public int? CriticallyId { get; set; }
        public int? ServiceNumber { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid ClinicId { get; set; }
        public Guid UserId { get; set; }
        public Guid DoctorId { get; set; }
        public string LastInvoiceNum { get; set; }
        public string FirstInvoiceNum { get; set; }
        public string Barcode { get; set; }
        public string DoctorUserName { get; set; }
        public int? BaseCurrencyId { get; set; }
        public decimal? TotalReceived { get; set; }
        public decimal? Remained { get; set; }
        public string BaseCurrencyName { get; set; }
        public string AllAnalysisName { get; set; }
        public Guid? RoomBedId { get; set; }
        public string RoomBedName { get; set; }
        public string AgeInterval { get; set; }
        public List<IFormFile> MainAttachments { get; set; }
        public List<IFormFile> OtherAttachments { get; set; }
        public List<IFormFile> PoliceReport { get; set; }
        public string RootPath { get; set; }
        public Guid? SurgeryOne { get; set; }
        public string SurgeryOneName { get; set; }
        public Guid? DispatcherDoctor { get; set; }
        public string DispatcherDoctorName { get; set; }
        public DateTime? SurgeryTime { get; set; }
        public string SurgeryTimeYear { get; set; }
        public string SurgeryTimeMonth { get; set; }
        public string SurgeryTimeDay { get; set; }
        public string SurgeryTimeTime { get; set; }
        public int? PurposeId { get; set; }
        public string ChiefComplaint { get; set; }
        public string Examination { get; set; }
        public bool? Discharge { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public bool? HospitalReception { get; set; }
        public Guid? ReceptionClinicSectionId { get; set; }
        public decimal? PriceVisit { get; set; }
        public string RadiologyDoctorName { get; set; }
        public Guid RadiologyDoctorId { get; set; }
        public bool AutoPay { get; set; }

        public virtual BaseInfoGeneralViewModel ClearanceType { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual BaseInfoGeneralViewModel DiscountCurrency { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public virtual BaseInfoGeneralViewModel ReceptionType { get; set; }
        public virtual BaseInfoGeneralViewModel PaymentStatus { get; set; }
        public virtual BaseInfoGeneralViewModel Purpose { get; set; }
        public virtual EmergencyViewModel Emergency { get; set; }
        public virtual ICollection<AnalysisResultMasterViewModel> AnalysisResultMasters { get; set; }
        public virtual ICollection<PatientReceptionAnalysisViewModel> PatientReceptionAnalyses { get; set; }
        public virtual ICollection<PatientReceptionReceivedViewModel> PatientReceptionReceiveds { get; set; }
        public virtual ICollection<ReceptionAmbulanceViewModel> ReceptionAmbulances { get; set; }
        public virtual ReceptionAmbulanceViewModel ReceptionAmbulance { get; set; }
        public virtual ICollection<ReceptionClinicSectionViewModel> ReceptionClinicSections { get; set; }
        public virtual ICollection<ReceptionClinicSectionViewModel> ReceptionClinicSectionDestinations { get; set; }
        public virtual ICollection<ReceptionRoomBedViewModel> ReceptionRoomBeds { get; set; }
        public virtual ICollection<ReceptionServiceViewModel> ReceptionServices { get; set; }
        public virtual ICollection<SurgeryViewModel> Surgeries { get; set; }
        public virtual ICollection<ReceptionDoctorViewModel> ReceptionDoctors { get; set; }
        public virtual ICollection<VisitViewModel> Visits { get; set; }
        public virtual DoctorViewModel Doctor { get; set; }
        public List<AnalysisWithAnalysisItemViewModel> AllAnalysis { get; set; }
        public List<AnalysisItemJustNameAndGuidViewModel> AllAnalysisItems { get; set; }
        public List<GroupAnalysisJustNameAndGuid> AllGroupAnalysis { get; set; }
        public List<MoneyConvertViewModel> AllMoneyConverts { get; set; }
        public List<ClinicSectionSettingValueViewModel> AllDecimalAmount { get; set; }
        public virtual ICollection<PatientImageViewModel> PatientImages { get; set; }
    }
}
