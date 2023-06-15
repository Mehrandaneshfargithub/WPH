using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class HumanResourceSalaryPaymentRepository : Repository<HumanResourceSalaryPayment>, IHumanResourceSalaryPaymentRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public HumanResourceSalaryPaymentRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<HumanResourceSalaryPayment> GetHumanResourceSalaryPaymentsByHumanSalaryId(Guid humanSalaryId)
        {
            return Context.HumanResourceSalaryPayments.AsNoTracking()
                .Where(p => p.HumanResourceSalaryId == humanSalaryId)
                .Select(p => new HumanResourceSalaryPayment
                {
                    Guid = p.Guid,
                    Amount = p.Amount,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate
                });
        }

        public decimal? GetSalaryPaymentByHumanSalaryId(Guid? humanSalaryId)
        {
            return Context.HumanResourceSalaryPayments.AsNoTracking()
                .Where(p => p.HumanResourceSalaryId == humanSalaryId)
                .Sum(p => p.Amount.GetValueOrDefault(0));
        }

        public IEnumerable<HumanResourceSalaryPayment> GetSalaryForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        {
            return Context.HumanResourceSalaryPayments.AsNoTracking()
                .Include(p => p.HumanResourceSalary.HumanResource.Gu.ClinicSection)
                .Include(p => p.HumanResourceSalary.SalaryType)
                .Where(x => clinicSectionIds.Contains(x.HumanResourceSalary.HumanResource.Gu.ClinicSectionId ?? Guid.Empty) && x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo)
                .Select(x => new HumanResourceSalaryPayment
                {
                    Amount = x.Amount,
                    CreatedDate = x.CreatedDate,
                    HumanResourceSalary = new HumanResourceSalary
                    {
                        SalaryType = new BaseInfoGeneral
                        {
                            Name = x.HumanResourceSalary.SalaryType.Name
                        },
                        HumanResource = new HumanResource
                        {
                            Gu = new User
                            {
                                ClinicSection = new ClinicSection
                                {
                                    Name = x.HumanResourceSalary.HumanResource.Gu.ClinicSection.Name
                                }
                            }
                        }
                    }
                });
        }


        

    }
}
