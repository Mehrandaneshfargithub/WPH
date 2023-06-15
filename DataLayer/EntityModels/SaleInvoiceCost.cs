using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoiceCost
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? Price { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public Guid? UserId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
        public virtual User User { get; set; }
    }
}
