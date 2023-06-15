using DataLayer.EntityModels;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.ExpireList;
using WPH.Models.MaterialStoreroom;
using WPH.Models.Product;
using WPH.Models.PurchaseInvoiceDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IProductMvcMockingService
    {
        OperationStatus RemoveProduct(Guid Productid);
        string AddNewProductWithOutBarcode(ProductViewModel viewModel);
        string AddNewProduct(ProductViewModel viewModel);
        string UpdateProduct(ProductViewModel viewModel);
        ProductViewModel GetProduct(Guid MasterProductId, Guid ProductId);
        void GetModalsViewBags(dynamic viewBag);
        string GetProductName(Guid ProductId);
        IEnumerable<ProductViewModel> GetAllTotalProductsForFilter(Guid clinicSectionId);
        IEnumerable<ProductViewModel> GetAllMaterialProductsName(Guid clinicSectionId);
        IEnumerable<ProductViewModel> GetAllProductsName(Guid clinicSectionId);
        IEnumerable<ProductWithBarcodeViewModel> GetLimitedProductsWithBarcode(Guid clinicSectionId);
        IEnumerable<ProductWithBarcodeViewModel> GetAllTotalProductsWithBarcode(Guid clinicSectionId);
        IEnumerable<ProductViewModel> GetLimitedProductsName(Guid clinicSectionId, Guid? productId);
        IEnumerable<ProductViewModel> GetLimitedTotalProductsName(Guid clinicSectionId, Guid? productId);
        IEnumerable<ProductViewModel> GetLimitedMaterialProductsName(Guid clinicSectionId, Guid? productId);
        IEnumerable<MaterialStoreroomViewModel> GetAllMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId);
        IEnumerable<ProductStoreroomViewModel> GetAllStoreroomProducts(ProductFilterViewModel filterViewModel);
        IEnumerable<MaterialProductHistoryViewModel> GetAllMaterialProductHistory(Guid originalClinicSectionId, Guid clinicSectionId, Guid productId);
        IEnumerable<ProductViewModel> GetAllProductByMaterialTypeJustName(Guid clinicSectionId, string materialType);
        IEnumerable<UsableProductModel> GetAllUsableProductList(Guid ProductId, Guid ClinicSectionId);
        IEnumerable<ProductWithBarcodeViewModel> GetAllProductsWithBarcode(Guid clinicSectionId);
        PurchaseInvoiceDetailViewModel GetProductDetails(Guid productId, bool latestPrice, string SaleType, int? SellCurrencyId);
        IEnumerable<ProductWithExpireViewModel> GetAllProductExpireList(Guid id);
        PurchaseInvoiceDetailViewModel GetProductDetailsFromExpireList(Guid invoiceId, string invoiceType, string SaleType, int? SellCurrencyId, bool latestPrice, Guid productId);
        PieChartViewModel GetMostSaledProducts(Guid clinicSectionId);
        PieChartViewModel GetProductStocks(Guid clinicSectionId);
        List<int> GetExpiredProducts(Guid clinicSectionId);
        ProductDetailResultViewModel GetProductDetailById(Guid productId, Guid clinicSectionId);
        IEnumerable<ProductPricesViewModel> GetProductLastPricesByProducId(Guid productId, Guid transferId);
        IEnumerable<ProductStoreroomViewModel> GetAllClinicSectionsStoreroomProducts(ProductFilterViewModel filterViewModel);
        IEnumerable<ProductCardexReportResultViewModel> GetProductCardexReport(ProductReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer);
        IEnumerable<ProductReportResultViewModel> GetProductPurchaseReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ProductReportResultViewModel> GetProductSaleReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ProductReportResultViewModel> GetProductReturnPurchaseReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ProductReportResultViewModel> GetProductReturnSaleReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ProductReportResultViewModel> GetProductTransferReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ProductReportResultViewModel> GetProductReceiveReport(ProductReportFilterViewModel viewModel);
        IEnumerable<ExpireListViewModel> GetExpiredList(Guid clinicSectionId, string type);
        IEnumerable<MaterialStoreroomViewModel> GetAllClinicSectionsMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId);
    }
}
