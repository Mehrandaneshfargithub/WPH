using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class HumanResourceSalary
    {
        public HumanResourceSalary()
        {
            HumanResourceSalaryPayments = new HashSet<HumanResourceSalaryPayment>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? HumanResourceId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? WorkTime { get; set; }
        public decimal? ExtraSalary { get; set; }
        public decimal? Salary { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? SalaryTypeId { get; set; }
        public Guid? SurgeryId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? CadreTypeId { get; set; }

        public virtual BaseInfoGeneral CadreType { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual HumanResource HumanResource { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual BaseInfoGeneral PaymentStatus { get; set; }
        public virtual BaseInfoGeneral SalaryType { get; set; }
        public virtual Surgery Surgery { get; set; }
        public virtual ICollection<HumanResourceSalaryPayment> HumanResourceSalaryPayments { get; set; }
    }
}
