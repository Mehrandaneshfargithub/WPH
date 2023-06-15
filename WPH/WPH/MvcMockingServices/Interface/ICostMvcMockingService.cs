using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Cost;
using WPH.Models.CustomDataModels.CostReport;

namespace WPH.MvcMockingServices.Interface
{
    public interface ICostMvcMockingService
    {
        IEnumerable<CostViewModel> GetAllCosts(Guid clinicSectionId, Guid costtypeId, int periodId, DateTime DateFrom, DateTime DateTo);
        CostReportViewModel GetAllCostsByDateRange(List<Guid> clinicSectionId, Guid? costTypeId, DateTime fromDate, DateTime toDate, bool Detail);
        void GetModalsViewBags(dynamic viewBag);
        string AddNewCost(CostViewModel cost);
        string UpdateCost(CostViewModel cost);
        string RemoveCost(Guid costId);
        string PurchasInvoiceCostRemove(Guid costId);
        CostViewModel GetWithType(Guid costId);
        IEnumerable<CostViewModel> GetAllPurchasInvoiceCosts(Guid purchaseInvoiceId);
        CostViewModel GetCost(Guid costId);
        string AddPurchaseInvoiceCost(CostViewModel viewModel);
        string AddSaleInvoiceCost(CostViewModel viewModel);
        string UpdatePurchaseInvoiceCost(CostViewModel viewModel);
        string UpdateSaleInvoiceCost(CostViewModel viewModel);
        IEnumerable<CostViewModel> GetAllSaleInvoiceCosts(Guid saleInvoiceId);
    }
}
