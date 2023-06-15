using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class HumanResourceSalaryPayment
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? HumanResourceSalaryId { get; set; }
        public decimal? Amount { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual HumanResourceSalary HumanResourceSalary { get; set; }
        public virtual User ModifiedUser { get; set; }
    }
}
