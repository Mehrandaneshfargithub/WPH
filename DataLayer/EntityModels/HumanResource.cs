using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class HumanResource
    {
        public HumanResource()
        {
            HumanResourceSalaries = new HashSet<HumanResourceSalary>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public decimal? FixSalary { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public int? RoleTypeId { get; set; }
        public string Duty { get; set; }
        public decimal? ExtraSalaryPh { get; set; }
        public decimal? MinWorkTime { get; set; }

        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual User Gu { get; set; }
        public virtual BaseInfoGeneral RoleType { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaries { get; set; }
    }
}
