using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.SaleInvoiceDiscount;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISaleInvoiceDiscountMvcMockingService
    {
        IEnumerable<SaleInvoiceDiscountViewModel> GetAllSaleInvoiceDiscounts(Guid SaleInvoiceId);

        string AddNewSaleInvoiceDiscount(SaleInvoiceDiscountViewModel viewModel);
        string UpdateSaleInvoiceDiscount(SaleInvoiceDiscountViewModel viewModel);
        string RemoveSaleInvoiceDiscount(Guid SaleInvoiceDiscountId);
        SaleInvoiceDiscountViewModel GetSaleInvoiceDiscount(Guid SaleInvoiceDiscountId);
        SaleInvoiceDiscountViewModel GetSaleInvoiceDiscountByCurrencyId(Guid saleInvoiceId, int currencyId);
    }
}
