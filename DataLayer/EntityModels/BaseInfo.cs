using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class BaseInfo
    {
        public BaseInfo()
        {
            AnalysisItems = new HashSet<AnalysisItem>();
            ClinicSections = new HashSet<ClinicSection>();
            Costs = new HashSet<Cost>();
            CustomerCities = new HashSet<Customer>();
            CustomerTypes = new HashSet<Customer>();
            DamageCostTypes = new HashSet<Damage>();
            DamageReasons = new HashSet<Damage>();
            Doctors = new HashSet<Doctor>();
            Items = new HashSet<Item>();
            ItemSections = new HashSet<Item>();
            MedicineMedicineForms = new HashSet<Medicine>();
            MedicineProducers = new HashSet<Medicine>();
            PatientAddresses = new HashSet<Patient>();
            PatientFatherJobs = new HashSet<Patient>();
            PatientMotherJobs = new HashSet<Patient>();
            PrescriptionTestDetails = new HashSet<PrescriptionTestDetail>();
            ProductProducers = new HashSet<Product>();
            ProductProductTypes = new HashSet<Product>();
            ProductUnits = new HashSet<Product>();
            Services = new HashSet<Service>();
            SupplierCities = new HashSet<Supplier>();
            SupplierCountries = new HashSet<Supplier>();
            SupplierSupplierTypes = new HashSet<Supplier>();
            ReturnPurchaseInvoiceDetails = new HashSet<ReturnPurchaseInvoiceDetail>();
            ReturnSaleInvoiceDetails = new HashSet<ReturnSaleInvoiceDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public string Description { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoType Type { get; set; }
        public virtual ICollection<AnalysisItem> AnalysisItems { get; set; }
        public virtual ICollection<ClinicSection> ClinicSections { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Customer> CustomerCities { get; set; }
        public virtual ICollection<Customer> CustomerTypes { get; set; }
        public virtual ICollection<Damage> DamageCostTypes { get; set; }
        public virtual ICollection<Damage> DamageReasons { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Item> ItemSections { get; set; }
        public virtual ICollection<Medicine> MedicineMedicineForms { get; set; }
        public virtual ICollection<Medicine> MedicineProducers { get; set; }
        public virtual ICollection<Patient> PatientAddresses { get; set; }
        public virtual ICollection<Patient> PatientFatherJobs { get; set; }
        public virtual ICollection<Patient> PatientMotherJobs { get; set; }
        public virtual ICollection<PrescriptionTestDetail> PrescriptionTestDetails { get; set; }
        public virtual ICollection<Product> ProductProducers { get; set; }
        public virtual ICollection<Product> ProductProductTypes { get; set; }
        public virtual ICollection<Product> ProductUnits { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Supplier> SupplierCities { get; set; }
        public virtual ICollection<Supplier> SupplierCountries { get; set; }
        public virtual ICollection<Supplier> SupplierSupplierTypes { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetails { get; set; }
    }
}
