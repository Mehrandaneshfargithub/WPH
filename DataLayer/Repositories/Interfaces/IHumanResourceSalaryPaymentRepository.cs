using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IHumanResourceSalaryPaymentRepository : IRepository<HumanResourceSalaryPayment>
    {
        IEnumerable<HumanResourceSalaryPayment> GetHumanResourceSalaryPaymentsByHumanSalaryId(Guid humanSalaryId);
        decimal? GetSalaryPaymentByHumanSalaryId(Guid? humanSalaryId);
        IEnumerable<HumanResourceSalaryPayment> GetSalaryForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo);
        
    }
}
