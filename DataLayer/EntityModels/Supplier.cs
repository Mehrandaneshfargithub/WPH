using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class Supplier
    {
        public Supplier()
        {
            Pays = new HashSet<Pay>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
            ReturnPurchaseInvoices = new HashSet<ReturnPurchaseInvoice>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SupplierTypeId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual BaseInfo City { get; set; }
        public virtual BaseInfo Country { get; set; }
        public virtual ICollection<Pay> Pays { get; set; }
        public virtual BaseInfo SupplierType { get; set; }
        public virtual User User { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual ICollection<ReturnPurchaseInvoice> ReturnPurchaseInvoices { get; set; }
    }
}
