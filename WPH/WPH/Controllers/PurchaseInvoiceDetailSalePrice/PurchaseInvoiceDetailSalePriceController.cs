using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PurchaseInvoiceDetailSalePrice
{
    [SessionCheck]
    public class PurchaseInvoiceDetailSalePriceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PurchaseInvoiceDetailSalePriceController> _logger;

        public PurchaseInvoiceDetailSalePriceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PurchaseInvoiceDetailSalePriceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid purchaseInvoiceDetailId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetailSalePrice");
                ViewBag.AccessNewPurchaseInvoiceDetailSalePrice = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditPurchaseInvoiceDetailSalePrice = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeletePurchaseInvoiceDetailSalePrice = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.purchaseInvoiceDetailSalePrice.GetModalsViewBags(ViewBag);

                var purchase = _IDUNIT.purchaseInvoiceDetail.GetForNewSalePrice(purchaseInvoiceDetailId);

                return View("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetailSalePrice/wpPurchaseInvoiceDetailSalePrice.cshtml", purchase);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public IActionResult AddNewModal(Guid purchaseInvoiceDetailId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoiceDetailSalePrice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            var des = _IDUNIT.purchaseInvoiceDetailSalePrice.GetParentCurrency(purchaseInvoiceDetailId);

            return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetailSalePrice/mdPurchaseInvoiceDetailSalePriceNewModal.cshtml", des);
        }

        public IActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoiceDetailSalePrice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }
            try
            {
                PurchaseInvoiceDetailSalePriceViewModel hos = _IDUNIT.purchaseInvoiceDetailSalePrice.GetPurchaseInvoiceDetailSalePrice(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetailSalePrice/mdPurchaseInvoiceDetailSalePriceNewModal.cshtml", hos);
            }
            catch { return Json("Error"); }
        }

        public JsonResult AddOrUpdate(PurchaseInvoiceDetailSalePriceViewModel purchaseInvoiceDetailSalePrice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                purchaseInvoiceDetailSalePrice.ClinicSectionId = clinicSectionId;
                purchaseInvoiceDetailSalePrice.UserId = userId;

                if (purchaseInvoiceDetailSalePrice.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoiceDetailSalePrice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    return Json(_IDUNIT.purchaseInvoiceDetailSalePrice.UpdatePurchaseInvoiceDetailSalePrice(purchaseInvoiceDetailSalePrice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoiceDetailSalePrice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    return Json(_IDUNIT.purchaseInvoiceDetailSalePrice.AddNewPurchaseInvoiceDetailSalePrice(purchaseInvoiceDetailSalePrice));
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

        public IActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid purchaseInvoiceDetailId)
        {
            try
            {
                IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> AllPurchaseInvoiceDetailSalePrice = _IDUNIT.purchaseInvoiceDetailSalePrice.GetAllPurchaseInvoiceDetailSalePrices(purchaseInvoiceDetailId, _localizer);
                return Json(AllPurchaseInvoiceDetailSalePrice.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult Remove(Guid salePriceId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "PurchaseInvoiceDetailSalePrice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                OperationStatus oStatus = _IDUNIT.purchaseInvoiceDetailSalePrice.RemovePurchaseInvoiceDetailSalePrice(salePriceId);
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

    }
}
