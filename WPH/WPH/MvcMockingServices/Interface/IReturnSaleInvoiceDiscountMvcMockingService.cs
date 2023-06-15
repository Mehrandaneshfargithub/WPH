using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnSaleInvoiceDiscount;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnSaleInvoiceDiscountMvcMockingService
    {
        IEnumerable<ReturnSaleInvoiceDiscountViewModel> GetAllReturnSaleInvoiceDiscounts(Guid saleInvoiceId);

        string AddNewReturnSaleInvoiceDiscount(ReturnSaleInvoiceDiscountViewModel viewModel);
        string UpdateReturnSaleInvoiceDiscount(ReturnSaleInvoiceDiscountViewModel viewModel);
        string RemoveReturnSaleInvoiceDiscount(Guid saleInvoiceDiscountId);
        ReturnSaleInvoiceDiscountViewModel GetReturnSaleInvoiceDiscount(Guid saleInvoiceDiscountId);

    }
}
