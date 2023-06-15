using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoice
    {
        public SaleInvoice()
        {
            Costs = new HashSet<Cost>();
            SaleInvoiceCosts = new HashSet<SaleInvoiceCost>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
            SaleInvoiceDiscounts = new HashSet<SaleInvoiceDiscount>();
            SaleInvoiceReceives = new HashSet<SaleInvoiceReceive>();
            Receives = new HashSet<Receive>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? SalePriceTypeId { get; set; }
        public string TotalPrice { get; set; }
        public bool? OldFactor { get; set; }
        

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual BaseInfoGeneral SalePriceType { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<SaleInvoiceCost> SaleInvoiceCosts { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public virtual ICollection<SaleInvoiceDiscount> SaleInvoiceDiscounts { get; set; }
        public virtual ICollection<SaleInvoiceReceive> SaleInvoiceReceives { get; set; }
        public virtual ICollection<Receive> Receives { get; set; }
    }
}
