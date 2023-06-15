using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class BaseInfoGeneral
    {
        public BaseInfoGeneral()
        {
            Analyses = new HashSet<Analysis>();
            AnalysisItemAmountCurrencies = new HashSet<AnalysisItem>();
            AnalysisItemValueTypes = new HashSet<AnalysisItem>();
            BaseInfoSectionTypes = new HashSet<BaseInfoSectionType>();
            Children = new HashSet<Child>();
            ClinicSectionChoosenValueVariableDisplays = new HashSet<ClinicSectionChoosenValue>();
            ClinicSectionChoosenValueVariableStatuses = new HashSet<ClinicSectionChoosenValue>();
            ClinicSectionClinicSectionShowTypes = new HashSet<ClinicSection>();
            ClinicSectionClinicSectionTypes = new HashSet<ClinicSection>();
            ClinicSectionSectionTypes = new HashSet<ClinicSection>();
            ClinicSectionSettings = new HashSet<ClinicSectionSetting>();
            Costs = new HashSet<Cost>();
            DamageDetails = new HashSet<DamageDetail>();
            DamageDiscounts = new HashSet<DamageDiscount>();
            Diseases = new HashSet<Disease>();
            EmergencyArrivals = new HashSet<Emergency>();
            EmergencyCriticallies = new HashSet<Emergency>();
            GroupAnalyses = new HashSet<GroupAnalysis>();
            HumanResourceCurrencies = new HashSet<HumanResource>();
            HumanResourceRoleTypes = new HashSet<HumanResource>();
            HumanResourceSalaryCadreTypes = new HashSet<HumanResourceSalary>();
            HumanResourceSalaryCurrencies = new HashSet<HumanResourceSalary>();
            HumanResourceSalaryPaymentStatuses = new HashSet<HumanResourceSalary>();
            HumanResourceSalaryPayments = new HashSet<HumanResourceSalaryPayment>();
            HumanResourceSalarySalaryTypes = new HashSet<HumanResourceSalary>();
            MoneyConvertBaseCurrencies = new HashSet<MoneyConvert>();
            MoneyConvertDestCurrencies = new HashSet<MoneyConvert>();
            PatientImages = new HashSet<PatientImage>();
            PatientReceptionAnalyses = new HashSet<PatientReceptionAnalysis>();
            PatientReceptionBaseCurrencies = new HashSet<PatientReception>();
            PatientReceptionDiscountCurrencies = new HashSet<PatientReception>();
            PatientVariables = new HashSet<PatientVariable>();
            PatientVariableStatuses = new HashSet<PatientVariable>();
            PatientVariableTypes = new HashSet<PatientVariable>();
            Patients = new HashSet<Patient>();
            PayAmountCurrencies = new HashSet<PayAmount>();
            PayAmountBaseCurrencies = new HashSet<PayAmount>();
            Products = new HashSet<Product>();
            PurchaseInvoiceDetailSalePriceCurrencies = new HashSet<PurchaseInvoiceDetailSalePrice>();
            PurchaseInvoiceDetailSalePriceTypes = new HashSet<PurchaseInvoiceDetailSalePrice>();
            PurchaseInvoiceDetails = new HashSet<PurchaseInvoiceDetail>();
            PurchaseInvoiceDiscounts = new HashSet<PurchaseInvoiceDiscount>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
            ReceiveAmountBaseCurrencies = new HashSet<ReceiveAmount>();
            ReceiveAmountCurrencies = new HashSet<ReceiveAmount>();
            ReceptionAmbulanceCostCurrencies = new HashSet<ReceptionAmbulance>();
            ReceptionAmbulancePatientHealths = new HashSet<ReceptionAmbulance>();
            ReceptionClearanceTypes = new HashSet<Reception>();
            ReceptionDiscountCurrencies = new HashSet<Reception>();
            ReceptionInsuranceReceiveds = new HashSet<ReceptionInsuranceReceived>();
            ReceptionPaymentStatuses = new HashSet<Reception>();
            ReceptionPurposes = new HashSet<Reception>();
            ReceptionReceptionTypes = new HashSet<Reception>();
            ReceptionServiceDiscountCurrencies = new HashSet<ReceptionService>();
            ReceptionServiceReceiveds = new HashSet<ReceptionServiceReceived>();
            ReceptionServiceStatuses = new HashSet<ReceptionService>();
            ReserveDetails = new HashSet<ReserveDetail>();
            RoomBeds = new HashSet<RoomBed>();
            RoomStatuses = new HashSet<Room>();
            RoomTypes = new HashSet<Room>();
            SaleInvoiceCosts = new HashSet<SaleInvoiceCost>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
            SaleInvoiceDiscounts = new HashSet<SaleInvoiceDiscount>();
            SaleInvoices = new HashSet<SaleInvoice>();
            Secretaries = new HashSet<Secretary>();
            ServiceCurrencies = new HashSet<Service>();
            ServiceTypes = new HashSet<Service>();
            SubSystemSections = new HashSet<SubSystemSection>();
            SurgeryAnesthesiologistionTypes = new HashSet<Surgery>();
            SurgeryClassifications = new HashSet<Surgery>();
            SurgeryDoctors = new HashSet<SurgeryDoctor>();
            TransferDetailPurchaseCurrencies = new HashSet<TransferDetail>();
            UserAccessTypes = new HashSet<User>();
            UserGenders = new HashSet<User>();
            UserUserTypes = new HashSet<User>();
            ReceptionDoctors = new HashSet<ReceptionDoctor>();
            ReceptionStatuses = new HashSet<Reception>();
            ReturnPurchaseInvoiceDetails = new HashSet<ReturnPurchaseInvoiceDetail>();
            ReturnPurchaseInvoiceDiscounts = new HashSet<ReturnPurchaseInvoiceDiscount>();
            ReturnSaleInvoiceDetails = new HashSet<ReturnSaleInvoiceDetail>();
            ReturnSaleInvoiceDiscounts = new HashSet<ReturnSaleInvoiceDiscount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }

        public virtual BaseInfoGeneralType Type { get; set; }
        public virtual ICollection<Analysis> Analyses { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItemAmountCurrencies { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItemValueTypes { get; set; }
        public virtual ICollection<BaseInfoSectionType> BaseInfoSectionTypes { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ICollection<ClinicSectionChoosenValue> ClinicSectionChoosenValueVariableDisplays { get; set; }
        public virtual ICollection<ClinicSectionChoosenValue> ClinicSectionChoosenValueVariableStatuses { get; set; }
        public virtual ICollection<ClinicSection> ClinicSectionClinicSectionShowTypes { get; set; }
        public virtual ICollection<ClinicSection> ClinicSectionClinicSectionTypes { get; set; }
        public virtual ICollection<ClinicSection> ClinicSectionSectionTypes { get; set; }
        public virtual ICollection<ClinicSectionSetting> ClinicSectionSettings { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<DamageDetail> DamageDetails { get; set; }
        public virtual ICollection<DamageDiscount> DamageDiscounts { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Emergency> EmergencyArrivals { get; set; }
        public virtual ICollection<Emergency> EmergencyCriticallies { get; set; }
        public virtual ICollection<GroupAnalysis> GroupAnalyses { get; set; }
        public virtual ICollection<HumanResource> HumanResourceCurrencies { get; set; }
        public virtual ICollection<HumanResource> HumanResourceRoleTypes { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaryCadreTypes { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaryCurrencies { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaryPaymentStatuses { get; set; }
        public virtual ICollection<HumanResourceSalaryPayment> HumanResourceSalaryPayments { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalarySalaryTypes { get; set; }
        public virtual ICollection<MoneyConvert> MoneyConvertBaseCurrencies { get; set; }
        public virtual ICollection<MoneyConvert> MoneyConvertDestCurrencies { get; set; }
        public virtual ICollection<PatientImage> PatientImages { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
        public virtual ICollection<PatientReception> PatientReceptionBaseCurrencies { get; set; }
        public virtual ICollection<PatientReception> PatientReceptionDiscountCurrencies { get; set; }
        public virtual ICollection<PatientVariable> PatientVariables { get; set; }
        public virtual ICollection<PatientVariable> PatientVariableStatuses { get; set; }
        public virtual ICollection<PatientVariable> PatientVariableTypes { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<PayAmount> PayAmountCurrencies { get; set; }
        public virtual ICollection<PayAmount> PayAmountBaseCurrencies { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePriceCurrencies { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePriceTypes { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public virtual ICollection<PurchaseInvoiceDiscount> PurchaseInvoiceDiscounts { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual ICollection<ReceiveAmount> ReceiveAmountBaseCurrencies { get; set; }
        public virtual ICollection<ReceiveAmount> ReceiveAmountCurrencies { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulanceCostCurrencies { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulancePatientHealths { get; set; }
        public virtual ICollection<Reception> ReceptionClearanceTypes { get; set; }
        public virtual ICollection<Reception> ReceptionDiscountCurrencies { get; set; }
        public virtual ICollection<ReceptionInsuranceReceived> ReceptionInsuranceReceiveds { get; set; }
        public virtual ICollection<Reception> ReceptionPaymentStatuses { get; set; }
        public virtual ICollection<Reception> ReceptionPurposes { get; set; }
        public virtual ICollection<Reception> ReceptionReceptionTypes { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServiceDiscountCurrencies { get; set; }
        public virtual ICollection<ReceptionServiceReceived> ReceptionServiceReceiveds { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServiceStatuses { get; set; }
        public virtual ICollection<ReserveDetail> ReserveDetails { get; set; }
        public virtual ICollection<RoomBed> RoomBeds { get; set; }
        public virtual ICollection<Room> RoomStatuses { get; set; }
        public virtual ICollection<Room> RoomTypes { get; set; }
        public virtual ICollection<SaleInvoiceCost> SaleInvoiceCosts { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public virtual ICollection<SaleInvoiceDiscount> SaleInvoiceDiscounts { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoices { get; set; }
        public virtual ICollection<Secretary> Secretaries { get; set; }
        public virtual ICollection<Service> ServiceCurrencies { get; set; }
        public virtual ICollection<Service> ServiceTypes { get; set; }
        public virtual ICollection<SubSystemSection> SubSystemSections { get; set; }
        public virtual ICollection<Surgery> SurgeryAnesthesiologistionTypes { get; set; }
        public virtual ICollection<Surgery> SurgeryClassifications { get; set; }
        public virtual ICollection<SurgeryDoctor> SurgeryDoctors { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailPurchaseCurrencies { get; set; }
        public virtual ICollection<User> UserAccessTypes { get; set; }
        public virtual ICollection<User> UserGenders { get; set; }
        public virtual ICollection<User> UserUserTypes { get; set; }
        public virtual ICollection<ReceptionDoctor> ReceptionDoctors { get; set; }
        public virtual ICollection<Reception> ReceptionStatuses { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDiscount> ReturnPurchaseInvoiceDiscounts { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetails { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDiscount> ReturnSaleInvoiceDiscounts { get; set; }
    }
}
