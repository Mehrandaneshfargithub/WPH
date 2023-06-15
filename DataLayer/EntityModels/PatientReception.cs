using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientReception
    {
        public PatientReception()
        {
            
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid PatientId { get; set; }
        public Guid? DoctorId { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public string Barcode { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? BaseCurrencyId { get; set; }

        public virtual BaseInfoGeneral BaseCurrency { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral DiscountCurrency { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Patient Patient { get; set; }
        
    }
}
