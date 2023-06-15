using DataLayer.Repositories;
using DMSDataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.MvcMockingServices.InfraStructure;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices
{
    public class DIUnit : IDIUnit
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDMSUnitOfWork _dmsUnitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly AnalysisItemWorker _worker;
        private readonly IHttpContextAccessor _contextAccessor;
        //public DIUnit(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        //: this(new UnitOfWork())
        //{
        //    _unitOfWork = unitOfWork ?? new UnitOfWork();
        //    _memoryCache = memoryCache;
        //}
        public DIUnit(IUnitOfWork unitOfWork, IMemoryCache memoryCache, AnalysisItemWorker worker, IDMSUnitOfWork dmsUnitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _dmsUnitOfWork = dmsUnitOfWork ?? new DMSUnitOfWork();
            _memoryCache = memoryCache;
            _worker = worker;
            _contextAccessor = contextAccessor;


            analysisItemMinMaxValue = new AnalysisItemMinMaxValueMvcMockingService(_unitOfWork, _worker);
            analysisItem = new AnalysisItemMvcMockingService(_unitOfWork, _memoryCache, _worker);
            analysisItemValuesRange = new AnalysisItemValuesRangeMvcMockingService(_unitOfWork, _worker);
            analysis = new AnalysisMvcMockingService(_unitOfWork, _memoryCache, _worker);
            analysisResultMaster = new AnalysisResultMasterMvcMockingService(_unitOfWork, this);
            analysisResult = new AnalysisResultMvcMockingService(_unitOfWork);
            baseInfo = new BaseInfoMvcMockingService(_unitOfWork);
            clinic = new ClinicMvcMockingService(_unitOfWork);
            clinicCalendar = new ClinicCalendarMvcMockingService(_unitOfWork);
            clinicSectionChoosenValue = new ClinicSectionChoosenValueMvcMockingService(_unitOfWork);
            clinicSectionSetting = new ClinicSectionSettingMvcMockingService(_unitOfWork);
            clinicSection = new ClinicSectionMvcMockingService(_unitOfWork);
            cost = new CostMvcMockingService(_unitOfWork, this);
            purchaseInvoiceDiscount = new PurchaseInvoiceDiscountMvcMockingService(_unitOfWork, this);
            saleInvoiceDiscount = new SaleInvoiceDiscountMvcMockingService(_unitOfWork, this);
            returnPurchaseInvoiceDiscount = new ReturnPurchaseInvoiceDiscountMvcMockingService(_unitOfWork, this);
            returnSaleInvoiceDiscount = new ReturnSaleInvoiceDiscountMvcMockingService(_unitOfWork, this);
            disease = new DiseaseMvcMockingService(_unitOfWork);
            doctor = new DoctorMvcMockingService(_unitOfWork);
            groupAnalysis_Analysis = new GroupAnalysis_AnalysisMvcMockingService(_unitOfWork);
            groupAnalysisItem = new GroupAnalysisItemMvcMokingService(_unitOfWork);
            groupAnalysis = new GroupAnalysisMvcMockingService(_unitOfWork, _memoryCache, _worker);
            language = new LanguageMvcMockingService(_unitOfWork, this);
            medicine = new MedicineMvcMockingService(_unitOfWork);
            moneyConvert = new MoneyConvertMvcMockingService(_unitOfWork);
            patientDisease = new PatientDiseaseMvcMockingService(_unitOfWork);
            patientImage = new PatientImageMvcMockingService(_unitOfWork);
            patientMedicine = new PatientMedicineMvcMockingService(_unitOfWork);
            patient = new PatientMvcMockingService(_unitOfWork, this);
            patientReceptionAnalysis = new PatientReceptionAnalysisMvcMockingService(_unitOfWork, _memoryCache);
            patientReception = new PatientReceptionMvcMockingService(_unitOfWork, this);
            patientReceptionReceived = new PatientReceptionReceivedMvcMockingService(_unitOfWork);
            patientVariable = new PatientVariableMvcMockingService(_unitOfWork);
            patientVariablesValue = new PatientVariablesValuesMvcMockingService(_unitOfWork);
            prescription = new PrescriptionMvcMockingService(_unitOfWork);
            reserveDetail = new ReserveDetailMvcMockingService(_unitOfWork, this);
            reserve = new ReserveMvcMockingService(_unitOfWork, this);
            setting = new SettingMvcMockingService(_unitOfWork, this);
            subSystem = new SubSystemMvcMockingService(_unitOfWork, _contextAccessor);
            symptom = new SymptomMvcMockingService(_unitOfWork);
            totalReserve = new TotalReservesMvcMockingService(_unitOfWork, this);
            user = new UserMvcMockingService(_unitOfWork, this);
            visit = new VisitMvcMockingService(_unitOfWork, this);
            fund = new FundMvcMockingService(_unitOfWork, this, _memoryCache);
            hospital = new HospitalMvcMockingService(_unitOfWork);
            room = new RoomMvcMockingService(_unitOfWork);
            analysisResultTemplate = new AnalysisResultTemplateMvcMockingService(_unitOfWork);
            ambulance = new AmbulanceMvcMockingService(_unitOfWork);
            emergency = new EmergencyMvcMockingService(_unitOfWork);
            receptionAmbulance = new ReceptionAmbulanceMvcMockingService(_unitOfWork);
            receptionClinicSection = new ReceptionClinicSectionMvcMockingService(_unitOfWork);
            reception = new ReceptionMvcMockingService(_unitOfWork, this);
            receptionRoomBed = new ReceptionRoomBedMvcMockingService(_unitOfWork);
            roomItem = new RoomItemMvcMockingService(_unitOfWork);
            item = new ItemMvcMockingService(_unitOfWork);
            reminder = new ReminderMvcMockingService(_unitOfWork);
            child = new ChildMvcMockingService(_unitOfWork);
            surgery = new SurgeryMvcMockingService(_unitOfWork);
            humanResource = new HumanResourceMvcMockingService(_unitOfWork);
            humanResourceSalary = new HumanResourceSalaryMvcMockingService(_unitOfWork, this);
            humanResourceSalaryPayment = new HumanResourceSalaryPaymentMvcMockingService(_unitOfWork);
            service = new ServiceMvcMockingService(_unitOfWork);
            productBarcode = new ProductBarcodeMvcMockingService(_unitOfWork);
            receptionService = new ReceptionServiceMvcMockingService(_unitOfWork, _dmsUnitOfWork, this);
            receptionServiceReceived = new ReceptionServiceReceivedMvcMockingService(_unitOfWork);
            roomBed = new RoomBedMvcMockingService(_unitOfWork);
            product = new ProductMvcMockingService(_unitOfWork, this);
            dmsMedicine = new DMSMedicineMvcMockingService(_dmsUnitOfWork);
            dmsSaleInvoiceDetail = new DMSSaleInvoiceDetailMvcMockingService(_dmsUnitOfWork);
            dmsSaleInvoice = new DMSSaleInvoiceMvcMockingService(_dmsUnitOfWork, _unitOfWork);
            licenceKey = new LicenceKeyMvcMockingService(_unitOfWork);
            sectionManagement = new SectionManagementMvcMockingService(_unitOfWork);
            baseInfoType = new BaseInfoTypeMvcMockingService(_unitOfWork);
            supplier = new SupplierMvcMockingService(_unitOfWork);
            customer = new CustomerMvcMockingService(_unitOfWork);
            purchaseInvoice = new PurchaseInvoiceMvcMockingService(_unitOfWork, this);
            purchaseInvoiceDetail = new PurchaseInvoiceDetailMvcMockingService(_unitOfWork, this);
            purchaseInvoiceDetailSalePrice = new PurchaseInvoiceDetailSalePriceMvcMockingService(_unitOfWork, this);
            returnPurchaseInvoice = new ReturnPurchaseInvoiceMvcMockingService(_unitOfWork, this);
            returnPurchaseInvoiceDetail = new ReturnPurchaseInvoiceDetailMvcMockingService(_unitOfWork, this);
            returnSaleInvoice = new ReturnSaleInvoiceMvcMockingService(_unitOfWork, this);
            returnSaleInvoiceDetail = new ReturnSaleInvoiceDetailMvcMockingService(_unitOfWork, this);
            saleInvoice = new SaleInvoiceMvcMockingService(_unitOfWork);
            saleInvoiceDetail = new SaleInvoiceDetailMvcMockingService(_unitOfWork, this);
            pay = new PayMvcMockingService(_unitOfWork, this);
            receive = new ReceiveMvcMockingService(_unitOfWork, this);
            receptionInsurance = new ReceptionInsuranceMvcMockingService(_unitOfWork);
            receptionInsuranceReceived = new ReceptionInsuranceReceivedMvcMockingService(_unitOfWork);
            transfer = new TransferMvcMockingService(_unitOfWork, this);
            transferDetail = new TransferDetailMvcMockingService(_unitOfWork, this);
            damage = new DamageMvcMockingService(_unitOfWork, this);
            damageDetail = new DamageDetailMvcMockingService(_unitOfWork, this);
            damageDiscount = new DamageDiscountMvcMockingService(_unitOfWork, this);
            userPortion = new UserPortionMvcMockingService(_unitOfWork);

        }

        public IAnalysisItemMinMaxValueMvcMockingService analysisItemMinMaxValue { get; private set; }
        public IAnalysisItemMvcMockingService analysisItem { get; private set; }
        public IAnalysisItemValuesRangeMvcMockingService analysisItemValuesRange { get; private set; }
        public IAnalysisMvcMockingService analysis { get; private set; }
        public IAnalysisResultMasterMvcMockingService analysisResultMaster { get; private set; }
        public IAnalysisResultMvcMockingService analysisResult { get; private set; }
        public IBaseInfoMvcMockingService baseInfo { get; private set; }
        public IClinicMvcMockingService clinic { get; private set; }
        public IClinicCalendarMvcMockingService clinicCalendar { get; private set; }
        public IClinicSectionChoosenValueMvcMockingService clinicSectionChoosenValue { get; private set; }
        public IClinicSectionSettingMvcMockingService clinicSectionSetting { get; private set; }
        public IClinicSectionMvcMockingService clinicSection { get; private set; }
        public ICostMvcMockingService cost { get; private set; }
        public IPurchaseInvoiceDiscountMvcMockingService purchaseInvoiceDiscount { get; private set; }
        public ISaleInvoiceDiscountMvcMockingService saleInvoiceDiscount { get; private set; }
        public IReturnPurchaseInvoiceDiscountMvcMockingService returnPurchaseInvoiceDiscount { get; private set; }
        public IReturnSaleInvoiceDiscountMvcMockingService returnSaleInvoiceDiscount { get; private set; }
        public IDiseaseMvcMockingService disease { get; private set; }
        public IDoctorMvcMockingService doctor { get; private set; }
        public IGroupAnalysis_AnalysisMvcMockingService groupAnalysis_Analysis { get; private set; }
        public IGroupAnalysisItemMvcMokingService groupAnalysisItem { get; private set; }
        public IGroupAnalysisMvcMockingService groupAnalysis { get; private set; }
        public ILanguageMvcMockingService language { get; private set; }
        public IMedicineMvcMockingService medicine { get; private set; }
        public IMoneyConvertMvcMockingService moneyConvert { get; private set; }
        public IPatientDiseaseMvcMockingService patientDisease { get; private set; }
        public IPatientMedicineMvcMockingService patientMedicine { get; private set; }
        public IPatientImageMvcMockingService patientImage { get; private set; }
        public IPatientMvcMockingService patient { get; private set; }
        public IPatientReceptionAnalysisMvcMockingService patientReceptionAnalysis { get; private set; }
        public IPatientReceptionMvcMockingService patientReception { get; private set; }
        public IPatientReceptionReceivedMvcMockingService patientReceptionReceived { get; private set; }
        public IPatientVariableMvcMockingService patientVariable { get; private set; }
        public IPatientVariablesValuesMvcMockingService patientVariablesValue { get; private set; }
        public IPrescriptionMvcMockingService prescription { get; private set; }
        public IReserveDetailMvcMockingService reserveDetail { get; private set; }
        public IReserveMvcMockingService reserve { get; private set; }
        public ISettingMvcMockingService setting { get; private set; }
        public ISubSystemMvcMockingService subSystem { get; private set; }
        public ISymptomMvcMockingService symptom { get; private set; }
        public ITotalReservesMvcMockingService totalReserve { get; private set; }
        public IUserMvcMockingService user { get; private set; }
        public IVisitMvcMockingService visit { get; private set; }
        public IFundMvcMockingService fund { get; private set; }
        public IHospitalMvcMockingService hospital { get; private set; }
        public IRoomMvcMockingService room { get; private set; }
        public IAnalysisResultTemplateMvcMockingService analysisResultTemplate { get; private set; }
        public IAmbulanceMvcMockingService ambulance { get; private set; }
        public IEmergencyMvcMockingService emergency { get; private set; }
        public IReceptionAmbulanceMvcMockingService receptionAmbulance { get; private set; }
        public IReceptionClinicSectionMvcMockingService receptionClinicSection { get; private set; }
        public IReceptionMvcMockingService reception { get; private set; }
        public IReceptionRoomBedMvcMockingService receptionRoomBed { get; private set; }
        public IRoomItemMvcMockingService roomItem { get; private set; }
        public IItemMvcMockingService item { get; private set; }
        public IReminderMvcMockingService reminder { get; private set; }
        public IChildMvcMockingService child { get; private set; }
        public ISurgeryMvcMockingService surgery { get; private set; }
        public IHumanResourceMvcMockingService humanResource { get; private set; }
        public IHumanResourceSalaryMvcMockingService humanResourceSalary { get; private set; }
        public IHumanResourceSalaryPaymentMvcMockingService humanResourceSalaryPayment { get; private set; }
        public IServiceMvcMockingService service { get; private set; }
        public IProductBarcodeMvcMockingService productBarcode { get; private set; }
        public IRoomBedMvcMockingService roomBed { get; private set; }
        public IProductMvcMockingService product { get; private set; }
        public IReceptionServiceMvcMockingService receptionService { get; private set; }
        public IReceptionServiceReceivedMvcMockingService receptionServiceReceived { get; private set; }
        public IDMSMedicineMvcMockingService dmsMedicine { get; private set; }
        public IDMSSaleInvoiceDetailMvcMockingService dmsSaleInvoiceDetail { get; private set; }
        public IDMSSaleInvoiceMvcMockingService dmsSaleInvoice { get; private set; }
        public ILicenceKeyMvcMockingService licenceKey { get; private set; }
        public ISectionManagementMvcMockingService sectionManagement { get; private set; }
        public IBaseInfoTypeMvcMockingService baseInfoType { get; private set; }
        public ISupplierMvcMockingService supplier { get; private set; }
        public ICustomerMvcMockingService customer { get; private set; }
        public IPurchaseInvoiceMvcMockingService purchaseInvoice { get; private set; }
        public IPurchaseInvoiceDetailMvcMockingService purchaseInvoiceDetail { get; private set; }
        public IPurchaseInvoiceDetailSalePriceMvcMockingService purchaseInvoiceDetailSalePrice { get; private set; }
        public IReturnPurchaseInvoiceMvcMockingService returnPurchaseInvoice { get; private set; }
        public IReturnPurchaseInvoiceDetailMvcMockingService returnPurchaseInvoiceDetail { get; private set; }
        public IReturnSaleInvoiceMvcMockingService returnSaleInvoice { get; private set; }
        public IReturnSaleInvoiceDetailMvcMockingService returnSaleInvoiceDetail { get; private set; }
        public ISaleInvoiceMvcMockingService saleInvoice { get; private set; }
        public ISaleInvoiceDetailMvcMockingService saleInvoiceDetail { get; private set; }
        public IPayMvcMockingService pay { get; private set; }
        public IReceiveMvcMockingService receive { get; private set; }
        public IReceptionInsuranceMvcMockingService receptionInsurance { get; private set; }
        public IReceptionInsuranceReceivedMvcMockingService receptionInsuranceReceived { get; private set; }
        public ITransferMvcMockingService transfer { get; private set; }
        public ITransferDetailMvcMockingService transferDetail { get; private set; }
        public IDamageMvcMockingService damage { get; private set; }
        public IDamageDetailMvcMockingService damageDetail { get; private set; }
        public IDamageDiscountMvcMockingService damageDiscount { get; private set; }
        public IUserPortionMvcMockingService userPortion { get; private set; }
    }
}
