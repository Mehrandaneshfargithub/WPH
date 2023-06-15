using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDiscount;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPurchaseInvoiceDiscountMvcMockingService
    {
        IEnumerable<PurchaseInvoiceDiscountViewModel> GetAllPurchaseInvoiceDiscounts(Guid purchaseInvoiceId);

        string AddNewPurchaseInvoiceDiscount(PurchaseInvoiceDiscountViewModel viewModel);
        string UpdatePurchaseInvoiceDiscount(PurchaseInvoiceDiscountViewModel viewModel);
        string RemovePurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId);
        PurchaseInvoiceDiscountViewModel GetPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId);

    }
}
