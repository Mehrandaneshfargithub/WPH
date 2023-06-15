using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Supplier;
using WPH.Models.SupplierAccount;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISupplierMvcMockingService
    {
        OperationStatus RemoveSupplier(Guid Supplierid);
        string AddNewSupplier(SupplierViewModel Supplier);
        string UpdateSupplier(SupplierViewModel Supplier);
        IEnumerable<SupplierViewModel> GetAllSuppliers(Guid clinicSectionId);
        IEnumerable<SupplierViewModel> GetAllSuppliersName(Guid clinicSectionId);
        IEnumerable<SupplierViewModel> GetAllSuppliersByClinicSectionId(Guid clinicSectionId);
        SupplierViewModel GetSupplier(Guid SupplierId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<SupplierAccountViewModel> GetAllSupplierAccount(SupplierAccountFilterViewModel viewModel);
        SupplierViewModel GetSupplierName(Guid supplierId);
        SupplierAccountReportResultViewModel GetSupplierAccountReport(SupplierAccountReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer);
    }
}
