using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Customer;
using WPH.Models.CustomerAccount;

namespace WPH.MvcMockingServices.Interface
{
    public interface ICustomerMvcMockingService
    {
        OperationStatus RemoveCustomer(Guid Customerid);
        string AddNewCustomer(CustomerViewModel Customer);
        string UpdateCustomer(CustomerViewModel Customer);
        IEnumerable<CustomerViewModel> GetAllCustomers(Guid clinicSectionId);
        IEnumerable<CustomerViewModel> GetAllCustomersName(Guid clinicSectionId);
        CustomerViewModel GetCustomer(Guid CustomerId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<CustomerAccountViewModel> GetAllCustomerAccount(CustomerAccountFilterViewModel viewModel);
        CustomerAccountReportResultViewModel GetCustomerAccountReport(CustomerAccountReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer);
        CustomerViewModel GetCustomerName(Guid CustomerId);
    }
}
