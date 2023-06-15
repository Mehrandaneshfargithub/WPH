﻿using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices
{
    public interface IDIUnit
    {
        IAnalysisItemMinMaxValueMvcMockingService analysisItemMinMaxValue { get; }
        IAnalysisItemMvcMockingService analysisItem { get; }
        IAnalysisItemValuesRangeMvcMockingService analysisItemValuesRange { get; }
        IAnalysisMvcMockingService analysis { get; }
        IAnalysisResultMasterMvcMockingService analysisResultMaster { get; }
        IAnalysisResultMvcMockingService analysisResult { get; }
        IBaseInfoMvcMockingService baseInfo { get; }
        IClinicMvcMockingService clinic { get; }
        IClinicCalendarMvcMockingService clinicCalendar { get; }
        IClinicSectionChoosenValueMvcMockingService clinicSectionChoosenValue { get; }
        IClinicSectionSettingMvcMockingService clinicSectionSetting{ get; }
        IClinicSectionMvcMockingService clinicSection { get; }
        ICostMvcMockingService cost { get; }
        IDamageDiscountMvcMockingService damageDiscount { get; }
        IPurchaseInvoiceDiscountMvcMockingService purchaseInvoiceDiscount { get; }
        ISaleInvoiceDiscountMvcMockingService saleInvoiceDiscount { get; }
        IReturnPurchaseInvoiceDiscountMvcMockingService returnPurchaseInvoiceDiscount { get; }
        IReturnSaleInvoiceDiscountMvcMockingService returnSaleInvoiceDiscount { get; }
        IDiseaseMvcMockingService disease { get; }
        IDoctorMvcMockingService doctor { get; }
        IGroupAnalysis_AnalysisMvcMockingService groupAnalysis_Analysis { get; }
        IGroupAnalysisItemMvcMokingService groupAnalysisItem { get; }
        IGroupAnalysisMvcMockingService groupAnalysis { get; }
        ILanguageMvcMockingService language { get; }
        IMedicineMvcMockingService medicine { get; }
        IMoneyConvertMvcMockingService moneyConvert { get; }
        IPatientDiseaseMvcMockingService patientDisease { get; }
        IPatientImageMvcMockingService patientImage { get; }
        IPatientMedicineMvcMockingService patientMedicine { get; }
        IPatientMvcMockingService patient { get; }
        IPatientReceptionAnalysisMvcMockingService patientReceptionAnalysis { get; }
        IPatientReceptionMvcMockingService patientReception { get; }
        IPatientReceptionReceivedMvcMockingService patientReceptionReceived { get; }
        IPatientVariableMvcMockingService patientVariable { get; }
        IPatientVariablesValuesMvcMockingService patientVariablesValue { get; }
        IPrescriptionMvcMockingService prescription { get; }
        IReserveDetailMvcMockingService reserveDetail { get; }
        IReserveMvcMockingService reserve { get; }
        ISettingMvcMockingService setting { get; }
        ISubSystemMvcMockingService subSystem { get; }
        ISymptomMvcMockingService symptom { get; }
        ITotalReservesMvcMockingService totalReserve { get; }
        IUserMvcMockingService user { get; }
        IVisitMvcMockingService visit { get; }
        IFundMvcMockingService fund { get; }
        IHospitalMvcMockingService hospital { get; }
        IRoomMvcMockingService room { get; }
        IAnalysisResultTemplateMvcMockingService analysisResultTemplate { get; }
        IAmbulanceMvcMockingService ambulance { get; }
        IEmergencyMvcMockingService emergency { get; }
        IReceptionAmbulanceMvcMockingService receptionAmbulance { get; }
        IReceptionClinicSectionMvcMockingService receptionClinicSection { get; }
        IReceptionMvcMockingService reception { get; }
        IReceptionRoomBedMvcMockingService receptionRoomBed { get; }
        IRoomItemMvcMockingService roomItem { get; }
        IItemMvcMockingService item { get; }
        IReminderMvcMockingService reminder { get; }
        IChildMvcMockingService child { get; }
        ISurgeryMvcMockingService surgery { get; }
        IHumanResourceMvcMockingService humanResource { get; }
        IHumanResourceSalaryMvcMockingService humanResourceSalary { get; }
        IHumanResourceSalaryPaymentMvcMockingService humanResourceSalaryPayment { get; }
        IServiceMvcMockingService service { get; }
        IProductBarcodeMvcMockingService productBarcode { get; }
        IReceptionServiceMvcMockingService receptionService { get; }
        IReceptionServiceReceivedMvcMockingService receptionServiceReceived { get; }
        IRoomBedMvcMockingService roomBed { get; }
        IProductMvcMockingService product { get; }
        IDMSMedicineMvcMockingService dmsMedicine { get; }
        IDMSSaleInvoiceDetailMvcMockingService dmsSaleInvoiceDetail { get; }
        IDMSSaleInvoiceMvcMockingService dmsSaleInvoice { get; }
        ILicenceKeyMvcMockingService licenceKey { get; }
        ISectionManagementMvcMockingService sectionManagement { get; }
        IBaseInfoTypeMvcMockingService baseInfoType { get; }
        ISupplierMvcMockingService supplier { get; }
        ICustomerMvcMockingService customer { get; }
        IDamageMvcMockingService damage { get; }
        IDamageDetailMvcMockingService damageDetail { get; }
        IPurchaseInvoiceMvcMockingService purchaseInvoice { get; }
        IPurchaseInvoiceDetailMvcMockingService purchaseInvoiceDetail { get; }
        IPurchaseInvoiceDetailSalePriceMvcMockingService purchaseInvoiceDetailSalePrice { get; }
        IReturnPurchaseInvoiceMvcMockingService returnPurchaseInvoice { get; }
        IReturnPurchaseInvoiceDetailMvcMockingService returnPurchaseInvoiceDetail { get; }
        IReturnSaleInvoiceMvcMockingService returnSaleInvoice { get; }
        IReturnSaleInvoiceDetailMvcMockingService returnSaleInvoiceDetail { get; }
        ISaleInvoiceMvcMockingService saleInvoice { get; }
        ISaleInvoiceDetailMvcMockingService saleInvoiceDetail { get; }
        IPayMvcMockingService pay { get; }
        IReceiveMvcMockingService receive { get; }
        IReceptionInsuranceMvcMockingService receptionInsurance { get; }
        IReceptionInsuranceReceivedMvcMockingService receptionInsuranceReceived { get; }
        ITransferMvcMockingService transfer { get; }
        ITransferDetailMvcMockingService transferDetail { get; }
        IUserPortionMvcMockingService userPortion { get; }
    }
}
