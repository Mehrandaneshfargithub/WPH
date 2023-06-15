using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Common.Enums;

namespace DataLayer.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        bool CheckCustomerExistBaseOnName(Guid clinicSectionId, string name, Expression<Func<Customer, bool>> predicate = null);
        IEnumerable<Customer> GetAllCustomer(Guid clinicSectionId);
        Customer GetCustomer(Guid customerId);
        Customer GetCustomerWithUser(Guid customerId);
        IEnumerable<Customer> GetAllCustomerName(Guid clinicSectionId);
        IEnumerable<CustomerAccountModel> GetAllCustomerAccount(Guid CustomerId, int? currencyId, SupplierFilter filer, DateTime fromDate, DateTime toDate);
        Customer GetCustomerName(Guid customerId);
        public Customer GetCustomerAccountDetailReport(Guid customerId, bool? paid, bool? purchase, string currencyName, int? currencyId, DateTime fromDate, DateTime toDate);
        public Customer GetCustomerAccountReport(Guid customerId, bool? paid, bool? purchase, string currencyName, DateTime fromDate, DateTime toDate);
    }
}
