using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IHumanResourceSalaryRepository : IRepository<HumanResourceSalary>
    {
        IEnumerable<HumanResourceSalary> GetAllHumanSalary();
        IEnumerable<HumanResourceSalary> GetAllHumanSalaryName(List<Guid> sections);
        IEnumerable<HumanResourceSalary> GetHumanSalaryByHumanId(Guid gd);
        IEnumerable<HumanResourceSalary> GetAllHumanSalaryByForSpecificDate(DateTime dateFrom, DateTime dateTo);
        HumanResourceSalary GetHumanSalaryByHumanSalaryId(Guid gd);
        IEnumerable<HumanResourceSalary> GetAllHumanSalaryWithDate(List<Guid> sections, DateTime dateFrom, DateTime dateTo, Expression<Func<HumanResourceSalary, bool>> predicate = null);
        IEnumerable<HumanResourceSalary> GetAllTreatmentStaffWage(List<Guid> sections, Guid surgeryId, int? cadreType);
        decimal? GetHumanSalaryRem(Guid humanSalaryId);
        HumanResourceSalary GetFirstOrDefault(Expression<Func<HumanResourceSalary, bool>> predicate = null);
        //IEnumerable<HumanResourceSalary> GetDetailSalaryReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<HumanResourceSalary, bool>> predicate = null);
        IEnumerable<HumanResourceSalary> GetUnpaidHumanSalariesByHumanId(Guid humanResourceId);
        HumanResourceSalary GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(Guid? surgeryId, int? cadreTypeId, int? salaryTypeId);
        IEnumerable<HumanResourceSalary> GetDetailSalaryReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<HumanResourceSalary, bool>> predicate = null);
    }
}
