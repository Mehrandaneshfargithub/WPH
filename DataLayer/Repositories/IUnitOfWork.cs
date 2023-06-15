﻿using DataLayer.Repositories.Infrastructure;
using DataLayer.Repositories.Interfaces;
using System;

namespace DataLayer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccessRepository Accesses { get; }
        IClinicRepository Clinics { get; }
        ILanguageExpRepository LanguageExps { get; }
        IExpressionRepository Expressions { get; }
        IMedicineRepository Medicines { get; }
        ISettingRepository Settings { get; }
        ISubSystemRepository SubSystems { get; }
        IUserRepository Users { get; }
        IBaseinfoGeneralRepository BaseInfoGenerals { get; }
        IBaseinfoRepository BaseInfos { get; }
        ISoftwareSettingRepository SoftwareSettings { get; }
        IClinicSectionRepository ClinicSections { get; }
        IClinicSection_UserRepository ClinicSection_Users { get; }
        IDiseaseRepository Diseases { get; }
        IMedicineDiseaseRepository MedicineDiseases { get; }
        IReserveRepository Reserves { get; }
        IReserveDetailRepository ReserveDetails { get; }
        IUserSubSystemAccessRepository UserSubSystemAccesses { get; }
        IPatientRepository Patients { get; }
        ISymptomRepository Symptoms { get; }
        IDisease_SymptomRepository Disease_Symptoms { get; }
        IPatientDiseaseRecordRepository PatientDiseaseRecords { get; }
        IPatientMedicineRecordRepository PatientMedicineRecords { get; }
        IVisitRepository Visits { get; }
        IPrescriptionDetailRepository PrescriptionDetails { get; }
        IPrescriptionTestDetailRepository PrescriptionTests { get; }
        IClinicSectionSettingRepository ClinicSectionSettings { get; }
        IClinicSectionSettingValueRepository ClinicSectionSettingValues { get; }
        ICostRepository Costs { get; }
        IDamageDiscountRepository DamageDiscounts { get; }
        IPurchaseInvoiceDiscountRepository PurchaseInvoiceDiscounts { get; }
        IReturnPurchaseInvoiceDiscountRepository ReturnPurchaseInvoiceDiscounts { get; }
        IReturnSaleInvoiceDiscountRepository ReturnSaleInvoiceDiscounts { get; }
        IVisitDiseasePatientRepository VisitDiseasePatients { get; }
        IVisit_SymptomRepository Visit_Symptoms { get; }
        IPatientVariablesValueRepository PatientVariablesValue { get; }
        IPatientVariableRepository PatientVariable { get; }
        IClinicSectionChoosenValueRepository ClinicSectionChoosenValue { get; }
        IPatientImageRepository PatientImage { get; }
        IAnalysisRepository Analysis { get; }
        IAnalysisItemRepository AnalysisItem { get; }
        IAnalysisItemValuesRangeRepository AnalysisItemValuesRange { get; }
        IAnalysisItemMinMaxValueRepository AnalysisItemMinMaxValue { get; }
        IGroupAnalysisRepository GroupAnalysis { get; }
        IGroupAnalysisItemRepository GroupAnalysisItem { get; }
        IGroupAnalysisAnalysesRepository GroupAnalysisAnalyses { get; }
        ISubSystemSectionRepository SubSystemSection { get; }
        IDoctorRepository Doctor { get; }
        IPatientReceptionRepository PatientReception { get; }
        IPatientReceptionAnalysisRepository PatientReceptionAnalysis { get; }
        IMoneyConvertRepository MoneyConvert { get; }
        IAnalysisAnalysisItemRepository AnalysisAnalysisItem { get; }
        IAnalysisResultRepository AnalysisResult { get; }
        IAnalysisResultMasterRepository AnalysisResultMaster { get; }
        IHospitalRepository Hospitals { get; }
        IRoomRepository Rooms { get; }
        IAnalysisResultTemplateRepository AnalysisResultTemplates { get; }
        IReceptionRepository Receptions { get; }
        IAmbulanceRepository Ambulances { get; }
        IReceptionAmbulanceRepository ReceptionAmbulances { get; }
        IReceptionClinicSectionRepository ReceptionClinicSections { get; }
        IEmergencyRepository Emergences { get; }
        IReceptionRoomBedRepository ReceptionRoomBeds { get; }
        IRoomItemRepository RoomItems { get; }
        IItemRepository Items { get; }
        IReminderRepository Reminders { get; }
        ILicenceKeyRepository LicenceKeys { get; }
        IChildRepository Children { get; }
        ISurgeryRepository Surgeries { get; }
        ISurgeryDoctorRepository SurgeryDoctors { get; }
        IHumanResourceRepository HumanResources { get; }
        IHumanResourceSalaryRepository HumanResourceSalaries { get; }
        IHumanResourceSalaryPaymentRepository HumanResourceSalaryPayments { get; }
        IServiceRepository Services { get; }
        IProductBarcodeRepository ProductBarcodes { get; }
        IReceptionServiceRepository ReceptionServices { get; }
        IReceptionServiceReceivedRepository ReceptionServiceReceiveds { get; } 
        IRoomBedRepository RoomBeds { get; }
        IReceptionDoctorRepository ReceptionDoctor { get; }
        IReceptionTemperatureRepository ReceptionTemperature { get; }
        IProductRepository Products { get; }
        IPurchaseInvoicePayRepository PurchaseInvoicePays { get; }
        ISaleInvoiceReceiveRepository SaleInvoiceReceives { get; }
        IReturnPurchaseInvoicePayRepository ReturnPurchaseInvoicePays { get; }
        IReturnSaleInvoiceReceiveRepository ReturnSaleInvoiceReceives { get; }
        ISupplierRepository Suppliers { get; }
        ICustomerRepository Customers { get; }
        IReceptionInsuranceRepository ReceptionInsurances { get; }
        IReceptionInsuranceReceivedRepository ReceptionInsuranceReceiveds { get; }
        IDamageRepository Damages { get; }
        IDamageDetailRepository DamageDetails { get; }
        IPurchaseInvoiceRepository PurchaseInvoices { get; }
        IPurchaseInvoiceDetailRepository PurchaseInvoiceDetails { get; }
        IPurchaseInvoiceDetailSalePriceRepository PurchaseInvoiceDetailSalePrice { get; }
        IReturnPurchaseInvoiceRepository ReturnPurchaseInvoices { get; }
        IReturnPurchaseInvoiceDetailRepository ReturnPurchaseInvoiceDetails { get; }
        IReturnSaleInvoiceRepository ReturnSaleInvoices { get; }
        IReturnSaleInvoiceDetailRepository ReturnSaleInvoiceDetails { get; }
        ISaleInvoiceRepository SaleInvoices { get; }
        ISaleInvoiceDetailRepository SaleInvoiceDetails { get; }
        ISaleInvoiceDiscountRepository SaleInvoiceDiscounts { get; }
        IPayRepository Pays { get; }
        IPayAmountRepository PayAmounts { get; }
        IReceiveRepository Receives { get; }
        IReceiveAmountRepository ReceiveAmounts { get; }
        ITransferRepository Transfers { get; }
        ITransferDetailRepository TransferDetails { get; }
        IUserPortionRepository UserPortions { get; }
        IReceptionDetailPayRepository ReceptionDetailPaies { get; }
        
        int Complete();
    }
}
