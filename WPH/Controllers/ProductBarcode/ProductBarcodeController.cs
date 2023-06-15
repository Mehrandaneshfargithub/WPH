using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using WPH.Helper;
using WPH.Models.ProductBarcode;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ProductBarcode
{
    [SessionCheck]
    public class ProductBarcodeController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ProductBarcodeController> _logger;


        public ProductBarcodeController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ProductBarcodeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Form(Guid productId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ProductBarcode");
                ViewBag.AccessNewProductBarcode = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditProductBarcode = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteProductBarcode = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.productBarcode.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/ProductBarcode/wpProductBarcodeForm.cshtml", productId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("ERROR_SomeThingWentWrong");
            }

        }

        public ActionResult AddNewModal(Guid productId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "ProductBarcode");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ProductBarcodeViewModel productBarcode = new ProductBarcodeViewModel
            {
                ProductId = productId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/ProductBarcode/mdProductBarcodeNewModal.cshtml", productBarcode);
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ProductBarcode");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ProductBarcodeViewModel productBarcode = _IDUNIT.productBarcode.GetProductBarcode(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ProductBarcode/mdProductBarcodeNewModal.cshtml", productBarcode);
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
        public JsonResult AddOrUpdate(ProductBarcodeViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ProductBarcode");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }


                    viewModel.ModifiedUserId = userId;
                    string result = _IDUNIT.productBarcode.UpdateProductBarcode(viewModel);
                    return Json(result);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ProductBarcode");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.CreateUserId = userId;
                    string result = _IDUNIT.productBarcode.AddNewProductBarcode(viewModel);
                    return Json(result);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid productBarcodeId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ProductBarcode");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.productBarcode.RemoveProductBarcode(productBarcodeId);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("ERROR_SomeThingWentWrong");
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid productId)
        {
            try
            {
                IEnumerable<ProductBarcodeViewModel> AllProductBarcode = _IDUNIT.productBarcode.GetAllProductBarcodeByProductId(productId);
                return Json(AllProductBarcode.ToDataSourceResult(request));
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
