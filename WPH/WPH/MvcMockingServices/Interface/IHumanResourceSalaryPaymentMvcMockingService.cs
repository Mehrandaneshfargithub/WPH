using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.HumanResourceSalaryPayment;

namespace WPH.MvcMockingServices.Interface
{
    public interface IHumanResourceSalaryPaymentMvcMockingService
    {
        IEnumerable<HumanResourceSalaryPaymentViewModel> GetAllPaymentSalary(Guid humanSalaryId);
        string PaySalary(HumanResourceSalaryPaymentViewModel viewModel);
        string UpdateSalary(HumanResourceSalaryPaymentViewModel viewModel);
        HumanResourceSalaryPaymentViewModel GetPayment(Guid paymentId);
        OperationStatus RemovePayment(Guid paymentId);
        string PayAllSalaries(Guid humanResourceId, Guid userId, string description);
    }
}
