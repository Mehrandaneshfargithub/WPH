using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Customer
    {
        public Customer()
        {
            Receives = new HashSet<Receive>();
            ReturnSaleInvoices = new HashSet<ReturnSaleInvoice>();
            SaleInvoices = new HashSet<SaleInvoice>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? CustomerTypeId { get; set; }
        public string Address { get; set; }
        public Guid? CityId { get; set; }
        public string Description { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModidiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual BaseInfo City { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User User { get; set; }
        public virtual BaseInfo CustomerType { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual User ModidiedUser { get; set; }
        public virtual ICollection<Receive> Receives { get; set; }
        public virtual ICollection<ReturnSaleInvoice> ReturnSaleInvoices { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoices { get; set; }
    }
}
