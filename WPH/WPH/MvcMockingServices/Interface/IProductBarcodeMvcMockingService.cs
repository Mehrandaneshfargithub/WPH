using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ProductBarcode;

namespace WPH.MvcMockingServices.Interface
{
    public interface IProductBarcodeMvcMockingService
    { 
        IEnumerable<ProductBarcodeViewModel> GetAllProductBarcodeByProductId(Guid productId);
        OperationStatus RemoveProductBarcode(Guid productBarcodeId);
        string AddNewProductBarcode(ProductBarcodeViewModel viewModel);
        string UpdateProductBarcode(ProductBarcodeViewModel viewModel);
        ProductBarcodeViewModel GetProductBarcode(Guid productBarcodeId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
