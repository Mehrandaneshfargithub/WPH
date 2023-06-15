using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class User
    {
        public User()
        {
            AnalysisCreateUsers = new HashSet<Analysis>();
            AnalysisItemCreatedUsers = new HashSet<AnalysisItem>();
            AnalysisItemModifiedUsers = new HashSet<AnalysisItem>();
            AnalysisModifiedUsers = new HashSet<Analysis>();
            AnalysisResultCreatedUsers = new HashSet<AnalysisResult>();
            AnalysisResultMasterCreatedUsers = new HashSet<AnalysisResultMaster>();
            AnalysisResultMasterModifiedUsers = new HashSet<AnalysisResultMaster>();
            AnalysisResultModifiedUsers = new HashSet<AnalysisResult>();
            ChildCreateUsers = new HashSet<Child>();
            ChildModifiedUsers = new HashSet<Child>();
            ClinicSectionUsers = new HashSet<ClinicSectionUser>();
            Costs = new HashSet<Cost>();
            CustomerCreateUsers = new HashSet<Customer>();
            CustomerModidiedUsers = new HashSet<Customer>();
            DamageCreatedUsers = new HashSet<Damage>();
            DamageDetailCreatedUsers = new HashSet<DamageDetail>();
            DamageDetailModifiedUsers = new HashSet<DamageDetail>();
            DamageDiscountCreateUsers = new HashSet<DamageDiscount>();
            DamageDiscountModifiedUsers = new HashSet<DamageDiscount>();
            DamageModifiedUsers = new HashSet<Damage>();
            GroupAnalysisCreatedUsers = new HashSet<GroupAnalysis>();
            GroupAnalysisModifiedUsers = new HashSet<GroupAnalysis>();
            HumanResourceSalaryCreateUsers = new HashSet<HumanResourceSalary>();
            HumanResourceSalaryModifiedUsers = new HashSet<HumanResourceSalary>();
            HumanResourceSalaryPaymentCreatedUsers = new HashSet<HumanResourceSalaryPayment>();
            HumanResourceSalaryPaymentModifiedUsers = new HashSet<HumanResourceSalaryPayment>();
            PatientReceptionAnalysisCreatedUsers = new HashSet<PatientReceptionAnalysis>();
            PatientReceptionAnalysisModifiedUsers = new HashSet<PatientReceptionAnalysis>();
            PatientReceptionCreatedUsers = new HashSet<PatientReception>();
            PatientReceptionModifiedUsers = new HashSet<PatientReception>();
            PayCreatedUsers = new HashSet<Pay>();
            PayModifiedUsers = new HashSet<Pay>();
            PrescriptionDetailCreatedUsers = new HashSet<PrescriptionDetail>();
            PrescriptionDetailModifiedUsers = new HashSet<PrescriptionDetail>();
            PrescriptionTestDetailCreatedUsers = new HashSet<PrescriptionTestDetail>();
            PrescriptionTestDetailModifiedUsers = new HashSet<PrescriptionTestDetail>();
            ProductBarcodeCreateUsers = new HashSet<ProductBarcode>();
            ProductBarcodeModifiedUsers = new HashSet<ProductBarcode>();
            ProductCreateUsers = new HashSet<Product>();
            ProductModifiedUsers = new HashSet<Product>();
            PurchaseInvoiceCreatedUsers = new HashSet<PurchaseInvoice>();
            PurchaseInvoiceDetailCreatedUsers = new HashSet<PurchaseInvoiceDetail>();
            PurchaseInvoiceDetailModifiedUsers = new HashSet<PurchaseInvoiceDetail>();
            PurchaseInvoiceDetailSalePriceCreateUsers = new HashSet<PurchaseInvoiceDetailSalePrice>();
            PurchaseInvoiceDetailSalePriceModifiedUsers = new HashSet<PurchaseInvoiceDetailSalePrice>();
            PurchaseInvoiceDiscountCreateUsers = new HashSet<PurchaseInvoiceDiscount>();
            PurchaseInvoiceDiscountModifiedUsers = new HashSet<PurchaseInvoiceDiscount>();
            PurchaseInvoiceModifiedUsers = new HashSet<PurchaseInvoice>();
            ReceiveCreatedUsers = new HashSet<Receive>();
            ReceiveModifiedUsers = new HashSet<Receive>();
            ReceptionClinicSections = new HashSet<ReceptionClinicSection>();
            ReceptionCreatedUsers = new HashSet<Reception>();
            ReceptionInsuranceReceiveds = new HashSet<ReceptionInsuranceReceived>();
            ReceptionInsurances = new HashSet<ReceptionInsurance>();
            ReceptionModifiedUsers = new HashSet<Reception>();
            ReceptionRoomBedCreatedUsers = new HashSet<ReceptionRoomBed>();
            ReceptionRoomBedModifiedUsers = new HashSet<ReceptionRoomBed>();
            ReceptionTemperatures = new HashSet<ReceptionTemperature>();
            ReceptionServiceReceiveds = new HashSet<ReceptionServiceReceived>();
            ReceptionServices = new HashSet<ReceptionService>();
            ReminderCreateUsers = new HashSet<Reminder>();
            ReminderModifiedUsers = new HashSet<Reminder>();
            SaleInvoiceCosts = new HashSet<SaleInvoiceCost>();
            SaleInvoiceCreatedUsers = new HashSet<SaleInvoice>();
            SaleInvoiceDetailCreatedUsers = new HashSet<SaleInvoiceDetail>();
            SaleInvoiceDetailModifiedUsers = new HashSet<SaleInvoiceDetail>();
            SaleInvoiceDiscountCreateUsers = new HashSet<SaleInvoiceDiscount>();
            SaleInvoiceDiscountModifiedUsers = new HashSet<SaleInvoiceDiscount>();
            SaleInvoiceModifiedUsers = new HashSet<SaleInvoice>();
            SupplierCreatedUsers = new HashSet<Supplier>();
            SupplierModifiedUsers = new HashSet<Supplier>();
            SurgeryCreatedUsers = new HashSet<Surgery>();
            SurgeryModifiedUsers = new HashSet<Surgery>();
            TransferCreatedUsers = new HashSet<Transfer>();
            TransferDetailCreatedUsers = new HashSet<TransferDetail>();
            TransferDetailModifiedUsers = new HashSet<TransferDetail>();
            TransferModifiedUsers = new HashSet<Transfer>();
            TransferReceiverUsers = new HashSet<Transfer>();
            UserProfiles = new HashSet<UserProfile>();
            UserSubSystemAccesses = new HashSet<UserSubSystemAccess>();
            ReturnPurchaseInvoiceCreatedUsers = new HashSet<ReturnPurchaseInvoice>();
            ReturnPurchaseInvoiceDetailCreatedUsers = new HashSet<ReturnPurchaseInvoiceDetail>();
            ReturnPurchaseInvoiceDetailModifiedUsers = new HashSet<ReturnPurchaseInvoiceDetail>();
            ReturnPurchaseInvoiceDiscountCreateUsers = new HashSet<ReturnPurchaseInvoiceDiscount>();
            ReturnPurchaseInvoiceDiscountModifiedUsers = new HashSet<ReturnPurchaseInvoiceDiscount>();
            ReturnPurchaseInvoiceModifiedUsers = new HashSet<ReturnPurchaseInvoice>();
            ReturnSaleInvoiceCreatedUsers = new HashSet<ReturnSaleInvoice>();
            ReturnSaleInvoiceDetailCreatedUsers = new HashSet<ReturnSaleInvoiceDetail>();
            ReturnSaleInvoiceDetailModifiedUsers = new HashSet<ReturnSaleInvoiceDetail>();
            ReturnSaleInvoiceDiscountCreateUsers = new HashSet<ReturnSaleInvoiceDiscount>();
            ReturnSaleInvoiceDiscountModifiedUsers = new HashSet<ReturnSaleInvoiceDiscount>();
            ReturnSaleInvoiceModifiedUsers = new HashSet<ReturnSaleInvoice>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public string Pass4 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Mobile { get; set; }
        public int? GenderId { get; set; }
        public int? UserTypeId { get; set; }
        public bool? Active { get; set; }
        public int? AccessTypeId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public bool? IsUser { get; set; }

        public virtual BaseInfoGeneral AccessType { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral Gender { get; set; }
        public virtual BaseInfoGeneral UserType { get; set; }
        public virtual Child Child { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual HumanResource HumanResource { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual UserPortion UserPortion { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Secretary Secretary { get; set; }
        public virtual ICollection<Analysis> AnalysisCreateUsers { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItemCreatedUsers { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItemModifiedUsers { get; set; }
        public virtual ICollection<Analysis> AnalysisModifiedUsers { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResultCreatedUsers { get; set; }
        public virtual ICollection<AnalysisResultMaster> AnalysisResultMasterCreatedUsers { get; set; }
        public virtual ICollection<AnalysisResultMaster> AnalysisResultMasterModifiedUsers { get; set; }
        public virtual ICollection<AnalysisResult> AnalysisResultModifiedUsers { get; set; }
        public virtual ICollection<Child> ChildCreateUsers { get; set; }
        public virtual ICollection<Child> ChildModifiedUsers { get; set; }
        public virtual ICollection<ClinicSectionUser> ClinicSectionUsers { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Customer> CustomerCreateUsers { get; set; }
        public virtual ICollection<Customer> CustomerModidiedUsers { get; set; }
        public virtual ICollection<Damage> DamageCreatedUsers { get; set; }
        public virtual ICollection<DamageDetail> DamageDetailCreatedUsers { get; set; }
        public virtual ICollection<DamageDetail> DamageDetailModifiedUsers { get; set; }
        public virtual ICollection<DamageDiscount> DamageDiscountCreateUsers { get; set; }
        public virtual ICollection<DamageDiscount> DamageDiscountModifiedUsers { get; set; }
        public virtual ICollection<Damage> DamageModifiedUsers { get; set; }
        public virtual ICollection<GroupAnalysis> GroupAnalysisCreatedUsers { get; set; }
        public virtual ICollection<GroupAnalysis> GroupAnalysisModifiedUsers { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaryCreateUsers { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaryModifiedUsers { get; set; }
        public virtual ICollection<HumanResourceSalaryPayment> HumanResourceSalaryPaymentCreatedUsers { get; set; }
        public virtual ICollection<HumanResourceSalaryPayment> HumanResourceSalaryPaymentModifiedUsers { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalysisCreatedUsers { get; set; }
        public virtual ICollection<PatientReceptionAnalysis> PatientReceptionAnalysisModifiedUsers { get; set; }
        public virtual ICollection<PatientReception> PatientReceptionCreatedUsers { get; set; }
        public virtual ICollection<PatientReception> PatientReceptionModifiedUsers { get; set; }
        public virtual ICollection<Pay> PayCreatedUsers { get; set; }
        public virtual ICollection<Pay> PayModifiedUsers { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetailCreatedUsers { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetailModifiedUsers { get; set; }
        public virtual ICollection<PrescriptionTestDetail> PrescriptionTestDetailCreatedUsers { get; set; }
        public virtual ICollection<PrescriptionTestDetail> PrescriptionTestDetailModifiedUsers { get; set; }
        public virtual ICollection<ProductBarcode> ProductBarcodeCreateUsers { get; set; }
        public virtual ICollection<ProductBarcode> ProductBarcodeModifiedUsers { get; set; }
        public virtual ICollection<Product> ProductCreateUsers { get; set; }
        public virtual ICollection<Product> ProductModifiedUsers { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoiceCreatedUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDiscount> PurchaseInvoiceDiscountCreateUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDiscount> PurchaseInvoiceDiscountModifiedUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetailCreatedUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetailModifiedUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePriceCreateUsers { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePriceModifiedUsers { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoiceModifiedUsers { get; set; }
        public virtual ICollection<Receive> ReceiveCreatedUsers { get; set; }
        public virtual ICollection<Receive> ReceiveModifiedUsers { get; set; }
        public virtual ICollection<ReceptionClinicSection> ReceptionClinicSections { get; set; }
        public virtual ICollection<Reception> ReceptionCreatedUsers { get; set; }
        public virtual ICollection<ReceptionInsuranceReceived> ReceptionInsuranceReceiveds { get; set; }
        public virtual ICollection<ReceptionInsurance> ReceptionInsurances { get; set; }
        public virtual ICollection<Reception> ReceptionModifiedUsers { get; set; }
        public virtual ICollection<ReceptionRoomBed> ReceptionRoomBedCreatedUsers { get; set; }
        public virtual ICollection<ReceptionRoomBed> ReceptionRoomBedModifiedUsers { get; set; }
        public virtual ICollection<ReceptionTemperature> ReceptionTemperatures { get; set; }
        public virtual ICollection<ReceptionServiceReceived> ReceptionServiceReceiveds { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
        public virtual ICollection<Reminder> ReminderCreateUsers { get; set; }
        public virtual ICollection<Reminder> ReminderModifiedUsers { get; set; }
        public virtual ICollection<SaleInvoiceCost> SaleInvoiceCosts { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoiceCreatedUsers { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetailCreatedUsers { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetailModifiedUsers { get; set; }
        public virtual ICollection<SaleInvoiceDiscount> SaleInvoiceDiscountCreateUsers { get; set; }
        public virtual ICollection<SaleInvoiceDiscount> SaleInvoiceDiscountModifiedUsers { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoiceModifiedUsers { get; set; }
        public virtual ICollection<Supplier> SupplierCreatedUsers { get; set; }
        public virtual ICollection<Supplier> SupplierModifiedUsers { get; set; }
        public virtual ICollection<Surgery> SurgeryCreatedUsers { get; set; }
        public virtual ICollection<Surgery> SurgeryModifiedUsers { get; set; }
        public virtual ICollection<Transfer> TransferCreatedUsers { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailCreatedUsers { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailModifiedUsers { get; set; }
        public virtual ICollection<Transfer> TransferModifiedUsers { get; set; }
        public virtual ICollection<Transfer> TransferReceiverUsers { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<UserSubSystemAccess> UserSubSystemAccesses { get; set; }
        public virtual ICollection<ReturnPurchaseInvoice> ReturnPurchaseInvoiceCreatedUsers { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetailCreatedUsers { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetailModifiedUsers { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDiscount> ReturnPurchaseInvoiceDiscountCreateUsers { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDiscount> ReturnPurchaseInvoiceDiscountModifiedUsers { get; set; }
        public virtual ICollection<ReturnPurchaseInvoice> ReturnPurchaseInvoiceModifiedUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoice> ReturnSaleInvoiceCreatedUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetailCreatedUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetailModifiedUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDiscount> ReturnSaleInvoiceDiscountCreateUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDiscount> ReturnSaleInvoiceDiscountModifiedUsers { get; set; }
        public virtual ICollection<ReturnSaleInvoice> ReturnSaleInvoiceModifiedUsers { get; set; }
    }
}
