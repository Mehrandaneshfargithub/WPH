using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Damage
    {
        public Damage()
        {
            DamageDetails = new HashSet<DamageDetail>();
            DamageDiscounts = new HashSet<DamageDiscount>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Description { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public Guid? CostTypeId { get; set; }
        public Guid? ReasonId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfo CostType { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual BaseInfo Reason { get; set; }
        public virtual ICollection<DamageDetail> DamageDetails { get; set; }
        public virtual ICollection<DamageDiscount> DamageDiscounts { get; set; }
    }
}
