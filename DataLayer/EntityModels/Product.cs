using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class Product
    {
        public Product()
        {
            DamageDetails = new HashSet<DamageDetail>();
            ProductBarcodes = new HashSet<ProductBarcode>();
            PurchaseInvoiceDetails = new HashSet<PurchaseInvoiceDetail>();
            ReceptionServices = new HashSet<ReceptionService>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
            TransferDetailDestinationProducts = new HashSet<TransferDetail>();
            TransferDetailProducts = new HashSet<TransferDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ProducerId { get; set; }
        public string Barcode { get; set; }
        public Guid? ProductTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreateUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? UnitId { get; set; }
        public string Code { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public int? MaterialTypeId { get; set; }
        public int? OrderPoint { get; set; }
        public string ProductLocation { get; set; }
        public Guid? ProductMasterId { get; set; }
        [NotMapped]
        public Guid[] ClinicSectionIds { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual BaseInfoGeneral MaterialType { get; set; }
        public virtual BaseInfo Producer { get; set; }
        public virtual BaseInfo ProductType { get; set; }
        public virtual BaseInfo Unit { get; set; }
        public virtual ICollection<DamageDetail> DamageDetails { get; set; }
        public virtual ICollection<ProductBarcode> ProductBarcodes { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailDestinationProducts { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailProducts { get; set; }
    }
}
