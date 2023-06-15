using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Common.Enums;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        bool CheckSupplierExistBaseOnName(Guid clinicSectionId, string name, Expression<Func<Supplier, bool>> predicate = null);
        IEnumerable<Supplier> GetAllSupplier(Guid clinicSectionId);
        Supplier GetSupplier(Guid supplierId);
        IEnumerable<Supplier> GetAllSupplierName(Guid clinicSectionId);
        IEnumerable<SupplierAccountModel> GetAllSupplierAccount(Guid supplierId, int? currencyId, SupplierFilter filer, DateTime fromDate, DateTime toDate);
        Supplier GetSupplierName(Guid supplierId);
        Supplier GetSupplierAccountReport(Guid supplierId, bool? paid, bool? purchase, string currencyName, DateTime fromDate, DateTime toDate);
        Supplier GetSupplierAccountDetailReport(Guid supplierId, bool? paid, bool? purchase, string currencyName, int? currencyId, DateTime fromDate, DateTime toDate);
    }
}
