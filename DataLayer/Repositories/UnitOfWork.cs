using DataLayer.EntityModels;
using DataLayer.Repositories.Infrastructure;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WASContext _context;
        public UnitOfWork()
            : this(new WASContext())
        {
        }
        public UnitOfWork(WASContext context)
        {

            _context = context;
            Accesses = new AccessRepository(_context);
            Clinics = new ClinicRepository(_context);
            LanguageExps = new LanguageExpRepository(_context);
            Expressions = new ExpressionRepository(_context);
            Medicines = new MedicineRepository(_context);
            Settings = new SettingRepository(_context);
            SubSystems = new SubSystemRepository(_context);
            Users = new UserRepository(_context);
            BaseInfoGenerals = new BaseinfoGeneralRepository(_context);
            BaseInfos = new BaseinfoRepository(_context);
            SoftwareSettings = new SoftwareSettingRepository(_context);
            ClinicSections = new ClinicSectionReposiyory(_context);
            ClinicSection_Users = new ClinicSection_UserRepository(_context);
            Diseases = new DiseaseRepository(_context);
            MedicineDiseases = new MedicineDiseaseRepository(_context);
            Reserves = new ReserveRepository(_context);
            ReserveDetails = new ReserveDetailRepository(_context);
            UserSubSystemAccesses = new UserSubSystemAccessRepository(_context);
            Patients = new PatientRepository(_context);
            Symptoms = new SymptomRepository(_context);
            Disease_Symptoms = new Disease_SymptomRepository(_context);
            PatientDiseaseRecords = new PatientDiseaseRecordRepository(_context);
            PatientMedicineRecords = new PatientMedicineRecordRepository(_context);
            Visits = new VisitRepository(_context);
            PrescriptionDetails = new PrescriptionDetailRepository(_context);
            PrescriptionTests = new PrescriptionTestDetailRepository(_context);
            ClinicSectionSettingValues = new ClinicSectionSettingValueRepository(_context);
            ClinicSectionSettings = new ClinicSectionSettingRepository(_context);
            Costs = new CostRepository(_context);
            DamageDiscounts = new DamageDiscountRepository(_context);
            PurchaseInvoiceDiscounts = new PurchaseInvoiceDiscountRepository(_context);
            ReturnPurchaseInvoiceDiscounts = new ReturnPurchaseInvoiceDiscountRepository(_context);
            ReturnSaleInvoiceDiscounts = new ReturnSaleInvoiceDiscountRepository(_context);
            VisitDiseasePatients = new VisitDiseasePatientRepository(_context);
            Visit_Symptoms = new Visit_SymptomRepository(_context);
            PatientVariablesValue = new PatientVariablesValueRepository(_context);
            PatientVariable = new PatientVariableRepository(_context);
            ClinicSectionChoosenValue = new ClinicSectionChoosenValueRepository(_context);
            PatientImage = new PatientImageRepository(_context);
            Analysis = new AnalysisRepository(_context);
            AnalysisItem = new AnalysisItemRepository(_context);
            AnalysisItemValuesRange = new AnalysisItemValuesRangeRepository(_context);
            AnalysisItemMinMaxValue = new AnalysisItemMinMaxValueRepository(_context);
            GroupAnalysis = new GroupAnalysisRepository(_context);
            GroupAnalysisItem = new GroupAnalysisItemRepository(_context);
            GroupAnalysisAnalyses = new GroupAnalysisAnalysesRepository(_context);
            SubSystemSection = new SubSystemSectionRepository(_context);
            Doctor = new DoctorRepository(_context);
            PatientReception = new PatientReceptionRepository(_context);
            PatientReceptionAnalysis = new PatientReceptionAnalysisRepository(_context);
            MoneyConvert = new MoneyConvertRepository(_context);
            AnalysisAnalysisItem = new AnalysisAnalysisItemRepository(_context);
            AnalysisResult = new AnalysisResultRepository(_context);
            AnalysisResultMaster = new AnalysisResultMasterRepository(_context);
            Hospitals = new HospitalRepository(_context);
            Rooms = new RoomRepository(_context);
            AnalysisResultTemplates = new AnalysisResultTemplateRepository(_context);
            Receptions = new ReceptionRepository(_context);
            Ambulances = new AmbulanceRepository(_context);
            ReceptionAmbulances = new ReceptionAmbulanceRepository(_context);
            ReceptionClinicSections = new ReceptionClinicSectionRepository(_context);
            Emergences = new EmergencyRepository(_context);
            ReceptionRoomBeds = new ReceptionRoomBedRepository(_context);
            RoomItems = new RoomItemRepository(_context);
            Items = new ItemRepository(_context);
            Reminders = new ReminderRepository(_context);
            LicenceKeys = new LicenceKeyRepository(_context);
            Children = new ChildRepository(_context);
            Products = new ProductRepository(_context);
            PurchaseInvoicePays = new PurchaseInvoicePayRepository(_context);
            SaleInvoiceReceives = new SaleInvoiceReceiveRepository(_context);
            ReturnPurchaseInvoicePays = new ReturnPurchaseInvoicePayRepository(_context);
            ReturnSaleInvoiceReceives = new ReturnSaleInvoiceReceiveRepository(_context);
            Surgeries = new SurgeryRepository(_context);
            SurgeryDoctors = new SurgeryDoctorRepository(_context);
            HumanResources = new HumanResourceRepository(_context);
            HumanResourceSalaries = new HumanResourceSalaryRepository(_context);
            HumanResourceSalaryPayments = new HumanResourceSalaryPaymentRepository(_context);
            Services = new ServiceRepository(_context);
            ProductBarcodes = new ProductBarcodeRepository(_context);
            ReceptionServices = new ReceptionServiceRepository(_context);
            ReceptionServiceReceiveds = new ReceptionServiceReceivedRepository(_context);
            RoomBeds = new RoomBedRepository(_context);
            ReceptionDoctor = new ReceptionDoctorRepository(_context);
            ReceptionTemperature = new ReceptionTemperatureRepository(_context);
            Suppliers = new SupplierRepository(_context);
            Customers = new CustomerRepository(_context);
            Damages = new DamageRepository(_context);
            DamageDetails = new DamageDetailRepository(_context);
            PurchaseInvoices = new PurchaseInvoiceRepository(_context);
            PurchaseInvoiceDetails = new PurchaseInvoiceDetailRepository(_context);
            PurchaseInvoiceDetailSalePrice = new PurchaseInvoiceDetailSalePriceRepository(_context);
            ReturnPurchaseInvoices = new ReturnPurchaseInvoiceRepository(_context);
            ReturnPurchaseInvoiceDetails = new ReturnPurchaseInvoiceDetailRepository(_context);
            ReturnSaleInvoices = new ReturnSaleInvoiceRepository(_context);
            ReturnSaleInvoiceDetails = new ReturnSaleInvoiceDetailRepository(_context);
            SaleInvoices = new SaleInvoiceRepository(_context);
            SaleInvoiceDetails = new SaleInvoiceDetailRepository(_context);
            SaleInvoiceDiscounts = new SaleInvoiceDiscountRepository(_context);
            Pays = new PayRepository(_context);
            PayAmounts = new PayAmountRepository(_context);
            Receives = new ReceiveRepository(_context);
            ReceiveAmounts = new ReceiveAmountRepository(_context);
            ReceptionInsurances = new ReceptionInsuranceRepository(_context);
            ReceptionInsuranceReceiveds = new ReceptionInsuranceReceivedRepository(_context);
            Transfers = new TransferRepository(_context);
            TransferDetails = new TransferDetailRepository(_context);
            UserPortions = new UserPortionRepository(_context);
            ReceptionDetailPaies = new ReceptionDetailPayRepository(_context);

        }
        public IAccessRepository Accesses { get; private set; }
        public IClinicRepository Clinics { get; private set; }
        public ILanguageExpRepository LanguageExps { get; private set; }
        public IExpressionRepository Expressions { get; private set; }
        public IMedicineRepository Medicines { get; private set; }
        public ISettingRepository Settings { get; private set; }
        public ISubSystemRepository SubSystems { get; private set; }
        public IUserRepository Users { get; private set; }
        public IBaseinfoGeneralRepository BaseInfoGenerals { get; private set; }
        public IBaseinfoRepository BaseInfos { get; private set; }
        public ISoftwareSettingRepository SoftwareSettings { get; private set; }
        public IClinicSectionRepository ClinicSections { get; private set; }
        public IClinicSection_UserRepository ClinicSection_Users { get; private set; }
        public IDiseaseRepository Diseases { get; private set; }
        public IMedicineDiseaseRepository MedicineDiseases { get; private set; }
        public IReserveRepository Reserves { get; private set; }
        public IReserveDetailRepository ReserveDetails { get; private set; }
        public IUserSubSystemAccessRepository UserSubSystemAccesses { get; private set; }
        public IPatientRepository Patients { get; private set; }
        public ISymptomRepository Symptoms { get; private set; }
        public IDisease_SymptomRepository Disease_Symptoms { get; private set; }
        public IPatientDiseaseRecordRepository PatientDiseaseRecords { get; private set; }
        public IPatientMedicineRecordRepository PatientMedicineRecords { get; private set; }
        public IVisitRepository Visits { get; private set; }
        public IPrescriptionDetailRepository PrescriptionDetails { get; private set; }
        public IPrescriptionTestDetailRepository PrescriptionTests { get; private set; }
        public IClinicSectionSettingValueRepository ClinicSectionSettingValues { get; private set; }
        public IClinicSectionSettingRepository ClinicSectionSettings { get; private set; }
        public ICostRepository Costs { get; private set; }
        public IDamageDiscountRepository DamageDiscounts { get; private set; }
        public IPurchaseInvoiceDiscountRepository PurchaseInvoiceDiscounts { get; private set; }
        public IReturnPurchaseInvoiceDiscountRepository ReturnPurchaseInvoiceDiscounts { get; private set; }
        public IReturnSaleInvoiceDiscountRepository ReturnSaleInvoiceDiscounts { get; private set; }
        public IVisitDiseasePatientRepository VisitDiseasePatients { get; private set; }
        public IVisit_SymptomRepository Visit_Symptoms { get; private set; }
        public IPatientVariablesValueRepository PatientVariablesValue { get; private set; }
        public IPatientVariableRepository PatientVariable { get; private set; }
        public IClinicSectionChoosenValueRepository ClinicSectionChoosenValue { get; private set; }
        public IPatientImageRepository PatientImage { get; private set; }
        public IAnalysisRepository Analysis { get; private set; }
        public IGroupAnalysisRepository GroupAnalysis { get; private set; }
        public IGroupAnalysisItemRepository GroupAnalysisItem { get; private set; }
        public IGroupAnalysisAnalysesRepository GroupAnalysisAnalyses { get; private set; }
        public IAnalysisItemRepository AnalysisItem { get; private set; }
        public IAnalysisItemValuesRangeRepository AnalysisItemValuesRange { get; private set; }
        public IAnalysisItemMinMaxValueRepository AnalysisItemMinMaxValue { get; private set; }
        public ISubSystemSectionRepository SubSystemSection { get; private set; }
        public IDoctorRepository Doctor { get; private set; }
        public IPatientReceptionRepository PatientReception { get; private set; }
        public IPatientReceptionAnalysisRepository PatientReceptionAnalysis { get; private set; }
        public IMoneyConvertRepository MoneyConvert { get; private set; }
        public IAnalysisAnalysisItemRepository AnalysisAnalysisItem { get; private set; }
        public IAnalysisResultRepository AnalysisResult { get; private set; }
        public IAnalysisResultMasterRepository AnalysisResultMaster { get; private set; }
        public IHospitalRepository Hospitals { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IAnalysisResultTemplateRepository AnalysisResultTemplates { get; private set; }
        public IReceptionRepository Receptions { get; private set; }
        public IAmbulanceRepository Ambulances { get; private set; }
        public IReceptionAmbulanceRepository ReceptionAmbulances { get; private set; }
        public IReceptionClinicSectionRepository ReceptionClinicSections { get; private set; }
        public IEmergencyRepository Emergences { get; private set; }
        public IReceptionRoomBedRepository ReceptionRoomBeds { get; private set; }
        public IRoomItemRepository RoomItems { get; private set; }
        public IProductRepository Products { get; private set; }
        public IPurchaseInvoicePayRepository PurchaseInvoicePays { get; private set; }
        public ISaleInvoiceReceiveRepository SaleInvoiceReceives { get; private set; }
        public IReturnPurchaseInvoicePayRepository ReturnPurchaseInvoicePays { get; private set; }
        public IReturnSaleInvoiceReceiveRepository ReturnSaleInvoiceReceives { get; private set; }

        public IItemRepository Items { get; private set; }
        public IReminderRepository Reminders { get; private set; }
        public ILicenceKeyRepository LicenceKeys { get; private set; }
        public IChildRepository Children { get; private set; }
        public ISurgeryRepository Surgeries { get; private set; }
        public ISurgeryDoctorRepository SurgeryDoctors { get; private set; }
        public IHumanResourceRepository HumanResources { get; private set; }
        public IHumanResourceSalaryRepository HumanResourceSalaries { get; private set; }
        public IHumanResourceSalaryPaymentRepository HumanResourceSalaryPayments { get; private set; }
        public IServiceRepository Services { get; private set; }
        public IProductBarcodeRepository ProductBarcodes { get; private set; }
        public IRoomBedRepository RoomBeds { get; private set; }
        public IReceptionDoctorRepository ReceptionDoctor { get; private set; }
        public IReceptionServiceRepository ReceptionServices { get; private set; }
        public IReceptionServiceReceivedRepository ReceptionServiceReceiveds { get; private set; }
        public IReceptionTemperatureRepository ReceptionTemperature { get; private set; }
        public ISupplierRepository Suppliers { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IDamageRepository Damages { get; private set; }
        public IDamageDetailRepository DamageDetails { get; private set; }
        public IPurchaseInvoiceRepository PurchaseInvoices { get; private set; }
        public IPurchaseInvoiceDetailRepository PurchaseInvoiceDetails { get; private set; }
        public IPurchaseInvoiceDetailSalePriceRepository PurchaseInvoiceDetailSalePrice { get; private set; }
        public IReturnPurchaseInvoiceRepository ReturnPurchaseInvoices { get; private set; }
        public IReturnPurchaseInvoiceDetailRepository ReturnPurchaseInvoiceDetails { get; private set; }
        public IReturnSaleInvoiceRepository ReturnSaleInvoices { get; private set; }
        public IReturnSaleInvoiceDetailRepository ReturnSaleInvoiceDetails { get; private set; }
        public ISaleInvoiceRepository SaleInvoices { get; private set; }
        public ISaleInvoiceDetailRepository SaleInvoiceDetails { get; private set; }
        public ISaleInvoiceDiscountRepository SaleInvoiceDiscounts { get; private set; }
        public IPayRepository Pays { get; private set; }
        public IPayAmountRepository PayAmounts { get; private set; }
        public IReceiveRepository Receives { get; private set; }
        public IReceiveAmountRepository ReceiveAmounts { get; private set; }
        public IReceptionInsuranceRepository ReceptionInsurances { get; private set; }
        public IReceptionInsuranceReceivedRepository ReceptionInsuranceReceiveds { get; private set; }
        public ITransferRepository Transfers { get; private set; }
        public ITransferDetailRepository TransferDetails { get; private set; }
        public IUserPortionRepository UserPortions { get; private set; }
        public IReceptionDetailPayRepository ReceptionDetailPaies { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
