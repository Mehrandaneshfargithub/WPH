using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetailSalePrice;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPurchaseInvoiceDetailSalePriceMvcMockingService
    {
        OperationStatus RemovePurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailSalePriceId);
        string AddNewTransferDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel);
        string AddNewPurchaseInvoiceDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel purchaseInvoiceDetailSalePrice);
        string UpdateTransferDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel viewModel);
        string UpdatePurchaseInvoiceDetailSalePrice(PurchaseInvoiceDetailSalePriceViewModel purchaseInvoiceDetailSalePrice);
        IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> GetAllTransferDetailSalePrices(Guid transferDetailId, IStringLocalizer<SharedResource> localizer);
        IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> GetAllPurchaseInvoiceDetailSalePrices(Guid purchaseInvoiceDetailId, IStringLocalizer<SharedResource> localizer);
        PurchaseInvoiceDetailSalePriceViewModel GetTransferDetailSalePrice(Guid purchaseInvoiceDetailSalePriceId);
        PurchaseInvoiceDetailSalePriceViewModel GetPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailSalePriceId);
        void GetTransferModalsViewBags(dynamic viewBag);
        void GetModalsViewBags(dynamic viewBag);
        PurchaseInvoiceDetailSalePriceViewModel GetTransferParentCurrency(Guid transferDetailId);
        PurchaseInvoiceDetailSalePriceViewModel GetParentCurrency(Guid purchaseInvoiceDetailId);
        string UpdateMainSalePrice(Guid detailId, int typeId, string priceTxt, Guid userId);
    }
}
