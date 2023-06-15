using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoiceDiscount;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnPurchaseInvoiceDiscountMvcMockingService
    {
        IEnumerable<ReturnPurchaseInvoiceDiscountViewModel> GetAllReturnPurchaseInvoiceDiscounts(Guid purchaseInvoiceId);

        string AddNewReturnPurchaseInvoiceDiscount(ReturnPurchaseInvoiceDiscountViewModel viewModel);
        string UpdateReturnPurchaseInvoiceDiscount(ReturnPurchaseInvoiceDiscountViewModel viewModel);
        string RemoveReturnPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId);
        ReturnPurchaseInvoiceDiscountViewModel GetReturnPurchaseInvoiceDiscount(Guid purchaseInvoiceDiscountId);

    }
}
