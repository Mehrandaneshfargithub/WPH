using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.MaterialStoreroom;
using WPH.Models.Product;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Product
{
    [SessionCheck]
    public class ProductController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ProductController> _logger;


        public ProductController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ProductController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public async Task<ActionResult> Form()
        {
            try
            {
                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.product.GetModalsViewBags(ViewBag);

                ViewBag.ProducerId = _IDUNIT.baseInfo.GetBaseInfoTypeGuidByName("Producer");
                ViewBag.ProductId = _IDUNIT.baseInfo.GetBaseInfoTypeGuidByName("ProductType");
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                ViewBag.DMS = "false";

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("SubStoreroom", "CanUseMiddleSellPrice", "CanUseWholeSellPrice", "MaterialStoreroom");
                ViewBag.AccessNewStoreroom = access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");
                ViewBag.AccessEditStoreroom = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SubStoreroom");
                ViewBag.AccessDeleteStoreroom = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SubStoreroom");

                ViewBag.AccessNewMaterialStoreroom = access.Any(p => p.AccessName == "New" && p.SubSystemName == "MaterialStoreroom");
                ViewBag.AccessEditMaterialStoreroom = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "MaterialStoreroom");
                ViewBag.AccessDeleteMaterialStoreroom = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "MaterialStoreroom");

                ViewBag.CanUseWholeSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
                ViewBag.CanUseMiddleSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

                var filters = _IDUNIT.baseInfo.GetMedicineProductFilter(_localizer);

                return View("/Views/Shared/PartialViews/AppWebForms/Product/wpProduct.cshtml", filters);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubStoreroom");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");

                return Json("AccessDenied");
            }

            ProductViewModel product = new ProductViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Product/mdProductNewModal.cshtml", product);
        }


        //public async Task<ActionResult> ShowGrid(Guid clinicSectionId)
        //{
        //    try
        //    {
        //        Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
        //        if (clinicSectionId == Guid.Empty)
        //            clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

        //        ViewBag.DMS = "false";

        //        Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        //        var child = _IDUNIT.clinicSection.ClinicSectionHasChild(clinicSectionId, userId);

        //        var access = _IDUNIT.subSystem.GetUserSubSystemAccess("SubStoreroom", "InvoiceDetails");

        //        ViewBag.AccessEditStoreroom = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SubStoreroom");
        //        ViewBag.AccessDeleteStoreroom = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SubStoreroom");

        //        ViewBag.AccessNewInvoiceDetails = access.Any(p => p.AccessName == "New" && p.SubSystemName == "InvoiceDetails");
        //        ViewBag.AccessEditInvoiceDetails = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "InvoiceDetails");
        //        ViewBag.AccessDeleteInvoiceDetails = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "InvoiceDetails");

        //        return PartialView("/Views/Shared/PartialViews/AppWebForms/Product/dgProductGrid.cshtml", child);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                               "\t Message: " + e.Message);
        //        throw e;
        //    }

        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(ProductViewModel product)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                product.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                product.MaterialTypeId = _IDUNIT.baseInfo.GetIdByNameAndType("Medicine", "MaterialType");
                List<ProductViewModel> allProduct = new List<ProductViewModel>();
                if (product.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubStoreroom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");

                        return Json("AccessDenied");
                    }

                    product.ModifiedUserId = userId;
                    string productid = _IDUNIT.product.UpdateProduct(product);
                    return Json(productid);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubStoreroom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    product.CreateUserId = userId;

                    //product.ClinicSectionss = product.ClinicSections.Split(',');
                    //List<ProductViewModel> all = new List<ProductViewModel>()
                    //{
                    //    product
                    //};

                    //var result = all.SelectMany(a => a.ClinicSectionss , (x, c) => { x.ClinicSectionId = Guid.Parse(c); return x; }).ToList();


                    //string[] ids = product.ClinicSections.Split(',');

                    //if (ids.Length > 0)
                    //{
                    //    for(int i =0; i < ids.Length; i++)
                    //    {

                    //        ProductViewModel p = Common.ConvertModels<ProductViewModel, ProductViewModel>.convertModels(product);

                    //        p.ClinicSectionId = Guid.Parse(ids[i]);

                    //        allProduct.Add(p);

                    //    }

                    //}
                    //allProduct.Add(product);


                    string productid = _IDUNIT.product.AddNewProduct(product);
                    return Json(productid);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, ProductFilterViewModel filterViewModel)
        {
            try
            {
                //filterViewModel.OriginalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ProductStoreroomViewModel> products;

                if (filterViewModel.ClinicSectionName == "Main")
                {
                    products = _IDUNIT.product.GetAllClinicSectionsStoreroomProducts(filterViewModel);
                }
                else
                {
                    products = _IDUNIT.product.GetAllStoreroomProducts(filterViewModel);
                }

                return Json(products.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAllProductByMaterialType(string MaterialType)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ProductViewModel> AllProduct;

                AllProduct = _IDUNIT.product.GetAllProductByMaterialTypeJustName(clinicSectionId, MaterialType);

                return Json(AllProduct);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public IActionResult GetLimitedTotalProductsName(Guid? productId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetLimitedTotalProductsName(clinicSectionId, productId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetLimitedMaterialProductsName(Guid? productId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetLimitedMaterialProductsName(clinicSectionId, productId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetLimitedProductsWithBarcode()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetLimitedProductsWithBarcode(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetAllTotalProductsWithBarcode()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetAllTotalProductsWithBarcode(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public IActionResult GetLimitedProductsName(Guid? productId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetLimitedProductsName(clinicSectionId, productId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetAllTotalProductsForFilter()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetAllTotalProductsForFilter(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetAllMaterialProductsForFilter()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetAllMaterialProductsName(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetAllProductsForFilter()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetAllProductsName(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }



        public IActionResult GetAllProductsWithBarcodeForFilter()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var products = _IDUNIT.product.GetAllProductsWithBarcode(clinicSectionId);

                return Json(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult EditModal(Guid MasterId, Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubStoreroom");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                ProductViewModel product = _IDUNIT.product.GetProduct(MasterId, Id);
                product.NameHolder = product.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Product/mdProductNewModal.cshtml", product);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "SubStoreroom");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                OperationStatus oStatus = _IDUNIT.product.RemoveProduct(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetProductDetails(Guid ProductId, bool LatestPrice, string SaleType)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId");
                string currencyId = sval?.FirstOrDefault(p => p.ShowSName == "CurrencyTypeId")?.SValue ?? null;

                PurchaseInvoiceDetailViewModel product = _IDUNIT.product.GetProductDetails(ProductId, LatestPrice, SaleType, Convert.ToInt32(currencyId));
                if (product != null)
                {
                    product.ExpireDateTxt = product.ExpireDate.GetValueOrDefault().ToString("dd/MM/yyyy", new CultureInfo("en-us"));
                    return Json(product);
                }
                else
                    return Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetProductDetailsFromExpireList(Guid InvoiceId, string InvoiceType, string SaleType, Guid ProductId, bool LatestPrice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId");
                string currencyId = sval?.FirstOrDefault(p => p.ShowSName == "CurrencyTypeId")?.SValue ?? null;

                PurchaseInvoiceDetailViewModel product = _IDUNIT.product.GetProductDetailsFromExpireList(InvoiceId, InvoiceType, SaleType, Convert.ToInt32(currencyId), LatestPrice, ProductId);
                if (product != null)
                {
                    product.ExpireDateTxt = product.ExpireDate.GetValueOrDefault().Day + "/" + product.ExpireDate.GetValueOrDefault().Month + "/" + product.ExpireDate.GetValueOrDefault().Year;
                    return Json(product);
                }
                else
                    return Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllProductExpireList([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {

                IEnumerable<ProductWithExpireViewModel> product = _IDUNIT.product.GetAllProductExpireList(Id);
                return Json(product.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetExpiredProductByTime([DataSourceRequest] DataSourceRequest request, string Type)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var product = _IDUNIT.product.GetExpiredList(clinicSectionId, Type);
                //return Json(product);
                //IEnumerable<ProductWithExpireViewModel> product = _IDUNIT.product.GetAllProductExpireList(Id);
                return Json(product.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAllProductExpireListModal(Guid id)
        {
            try
            {

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ProductExpire/dgProductExpireModal.cshtml", id);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }



        public JsonResult GetProductDetailById(Guid productId, Guid clinicSectionId)
        {
            try
            {
                var result = _IDUNIT.product.GetProductDetailById(productId, clinicSectionId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAllProductLastPricesById([DataSourceRequest] DataSourceRequest request, Guid productId, Guid transferId)
        {
            try
            {
                var result = _IDUNIT.product.GetProductLastPricesByProducId(productId, transferId);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult ProductReportsModal(Guid productId)
        {
            try
            {
                ViewBag.ProductName = _IDUNIT.product.GetProductName(productId);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Product/mdProductReportsModal.cshtml", productId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAllProductReportFilter()
        {
            try
            {
                var filters = _IDUNIT.baseInfo.GetAllProductReportFilter(_localizer);
                return Json(filters);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetProductCardexReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductCardexReport(viewModel, _localizer);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductPurchaseReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductPurchaseReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductSaleReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductSaleReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductReturnPurchaseReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductReturnPurchaseReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductReturnSaleReport( ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductReturnSaleReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductTransferReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductTransferReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetProductReceiveReport(ProductReportFilterViewModel viewModel)
        {
            try
            {
                var result = _IDUNIT.product.GetProductReceiveReport(viewModel);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


    }
}
