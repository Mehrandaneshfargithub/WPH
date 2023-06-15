using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetWithProducerAndProduct(Guid MasterProductId, Guid productId);
        IEnumerable<Product> GetAllProductsName(Guid clinicSectionId, int count, Expression<Func<Product, bool>> predicate = null);
        IEnumerable<Product> GetProductsWithBarcode(Guid clinicSectionId, int count, Expression<Func<Product, bool>> predicate = null);
        Product GetProductCount(Guid productId, Guid clinicSectionId);
        Product GetProductName(Guid productId, Expression<Func<Product, bool>> predicate = null);
        IEnumerable<Product> GetAllStoreroomProducts(Guid originalClinicSectionId, Guid clinicSectionId, string productBarcode, int? FromOrderPoint, int? toOrderPoint, Guid? supplierId);
        IEnumerable<Product> GetAllMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId);
        Product GetProductHistory(Guid productId, Guid clinicSectionId, Guid originalClinicSectionId);
        Product GetWithChild(Guid productId);
        Product GetProductByNameAndProducerAndType(Guid clinicSectionId, string productName, string producerName, int? materialTypeId, string productType);
        IEnumerable<Product> GetAllProductByMaterialTypeJustName(Guid clinicSectionId, string materialType);
        bool CheckRepeatedProductBarcode(string productName, string barcode);
        IEnumerable<ProductWithBarcodeModel> GetAllProductsWithBarcode(Guid clinicSectionId);
        IEnumerable<ExpiredProductModel> GetExpiredProducts(Guid clinicSectionId);
        Product GetProductDetailById(Guid productId, Guid clinicSectionId);
        IEnumerable<Product> GetAllClinicSectionsStoreroomProducts(Guid originalClinicSectionId, Guid clinicSectionId, string productBarcode, int? fromOrderPoint, int? toOrderPoint, Guid? supplierId);
        IEnumerable<ProductCardexReportModel> GetProductCardexReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ProductPurchaseReportModel> GetProductPurchaseReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ProductSaleReportModel> GetProductSaleReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ReturnProductPurchaseReportModel> GetProductReturnPurchaseReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ReturnProductPurchaseReportModel> GetProductReturnSaleReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ProductTransferReportModel> GetProductTransferReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<ProductTransferReportModel> GetProductReceiveReport(Guid productId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, int? currencyId);
        IEnumerable<Product> GetAllClinicSectionsMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId);
    }
}
