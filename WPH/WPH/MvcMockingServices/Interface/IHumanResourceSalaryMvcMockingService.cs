using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.HumanResourceSalary;

namespace WPH.MvcMockingServices.Interface
{
    public interface IHumanResourceSalaryMvcMockingService
    {
        string AddNewHumanSalary(HumanResourceSalaryViewModel newHumanSalary);
        OperationStatus RemoveHumanSalary(Guid HumanSalaryId);
        string UpdateHumanSalary(HumanResourceSalaryViewModel upHumanSalary);
        string UpdateTreatmentStaff(HumanResourceSalaryViewModel upHumanSalary);
        HumanResourceSalaryViewModel GetHumanSalaryByHumanSalaryId(Guid humanSalaryid);
        IEnumerable<HumanResourceSalaryViewModel> GetAllHumanSalaryByParameter(List<Guid> sections, Guid guid, int periodId, DateTime dateFrom, DateTime dateTo);
        IEnumerable<HumanResourceSalaryViewModel> GetAllTreatmentStaffWage(List<Guid> sections, DoctorWageViewModel viewModel);
        decimal? GetHumanSalaryRem(Guid humanSalaryId);
        string AddHumanWage(HumanResourceSalaryViewModel viewModel, string cadreType);
        SalaryReportResultViewModel SalaryReport(SalaryReportViewModel viewModel);
    }
}
