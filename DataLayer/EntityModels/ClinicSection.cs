using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ClinicSection
    {
        public ClinicSection()
        {
            AnalysisItems = new HashSet<AnalysisItem>();
            AnalysisResultTemplates = new HashSet<AnalysisResultTemplate>();
            BaseInfos = new HashSet<BaseInfo>();
            ClinicSectionChoosenValues = new HashSet<ClinicSectionChoosenValue>();
            ClinicSectionSettingValues = new HashSet<ClinicSectionSettingValue>();
            ClinicSectionUsers = new HashSet<ClinicSectionUser>();
            Costs = new HashSet<Cost>();
            Customers = new HashSet<Customer>();
            Damages = new HashSet<Damage>();
            Diseases = new HashSet<Disease>();
            Doctors = new HashSet<Doctor>();
            Items = new HashSet<Item>();
            Medicines = new HashSet<Medicine>();
            MoneyConverts = new HashSet<MoneyConvert>();
            PatientReceptions = new HashSet<PatientReception>();
            PatientVariablesValues = new HashSet<PatientVariablesValue>();
            Pays = new HashSet<Pay>();
            PrescriptionDetails = new HashSet<PrescriptionDetail>();
            PrescriptionTestDetails = new HashSet<PrescriptionTestDetail>();
            Products = new HashSet<Product>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
            Receives = new HashSet<Receive>();
            ReceptionClinicSections = new HashSet<ReceptionClinicSection>();
            Receptions = new HashSet<Reception>();
            Reminders = new HashSet<Reminder>();
            Reserves = new HashSet<Reserve>();
            ReturnPurchaseInvoices = new HashSet<ReturnPurchaseInvoice>();
            ReturnSaleInvoices = new HashSet<ReturnSaleInvoice>();
            Rooms = new HashSet<Room>();
            SaleInvoiceCosts = new HashSet<SaleInvoiceCost>();
            SaleInvoices = new HashSet<SaleInvoice>();
            Services = new HashSet<Service>();
            Suppliers = new HashSet<Supplier>();
            Surgeries = new HashSet<Surgery>();
            Symptoms = new HashSet<Symptom>();
            TransferDestinationClinicSections = new HashSet<Transfer>();
            TransferSourceClinicSections = new HashSet<Transfer>();
            UserSubSystemAccesses = new HashSet<UserSubSystemAccess>();
            Users = new HashSet<User>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string Explanation { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string SystemCode { get; set; }
        public string LicenseNumber { get; set; }
        public int? SectionTypeId { get; set; }
        public Guid? SpecialityId { get; set; }
        public int? ClinicSectionTypeId { get; set; }
        public Guid? ParentId { get; set; }
        public int? Priority { get; set; }
        public int? ClinicSectionShowTypeId { get; set; }

        public virtual Clinic Clinic { get; set; }
        public virtual BaseInfoGeneral ClinicSectionShowType { get; set; }
        public virtual ClinicSection Parent { get; set; }
        public virtual BaseInfoGeneral ClinicSectionType { get; set; }
        public virtual BaseInfoGeneral SectionType { get; set; }
        public virtual BaseInfo Speciality { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItems { get; set; }
        public virtual ICollection<AnalysisResultTemplate> AnalysisResultTemplates { get; set; }
        public virtual ICollection<BaseInfo> BaseInfos { get; set; }
        public virtual ICollection<ClinicSection> Children { get; set; }
        public virtual ICollection<ClinicSectionChoosenValue> ClinicSectionChoosenValues { get; set; }
        public virtual ICollection<ClinicSectionSettingValue> ClinicSectionSettingValues { get; set; }
        public virtual ICollection<ClinicSectionUser> ClinicSectionUsers { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Damage> Damages { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
        public virtual ICollection<MoneyConvert> MoneyConverts { get; set; }
        public virtual ICollection<PatientReception> PatientReceptions { get; set; }
        public virtual ICollection<PatientVariablesValue> PatientVariablesValues { get; set; }
        public virtual ICollection<Pay> Pays { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
        public virtual ICollection<PrescriptionTestDetail> PrescriptionTestDetails { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual ICollection<Receive> Receives { get; set; }
        public virtual ICollection<ReceptionClinicSection> ReceptionClinicSections { get; set; }
        public virtual ICollection<Reception> Receptions { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
        public virtual ICollection<Reserve> Reserves { get; set; }
        public virtual ICollection<ReturnSaleInvoice> ReturnSaleInvoices { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<SaleInvoiceCost> SaleInvoiceCosts { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoices { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
        public virtual ICollection<Surgery> Surgeries { get; set; }
        public virtual ICollection<Symptom> Symptoms { get; set; }
        public virtual ICollection<Transfer> TransferDestinationClinicSections { get; set; }
        public virtual ICollection<Transfer> TransferSourceClinicSections { get; set; }
        public virtual ICollection<UserSubSystemAccess> UserSubSystemAccesses { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ReturnPurchaseInvoice> ReturnPurchaseInvoices { get; set; }
    }
}
