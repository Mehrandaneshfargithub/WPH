using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Cost
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? CostTypeId { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? Price { get; set; }
        public string Explanation { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? UserId { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? PurchaseInvoiceId { get; set; }
        public Guid? SaleInvoiceId { get; set; }


        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfo CostType { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
        public virtual User User { get; set; }
    }
}
