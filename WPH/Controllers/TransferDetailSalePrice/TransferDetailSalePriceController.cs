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

namespace WPH.Controllers.TransferDetailSalePrice
{
    [SessionCheck]
    public class TransferDetailSalePriceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<TransferDetailSalePriceController> _logger;

        public TransferDetailSalePriceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<TransferDetailSalePriceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid transferDetailId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("TransferDetailSalePrice");
                ViewBag.AccessNewTransferDetailSalePrice = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditTransferDetailSalePrice = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteTransferDetailSalePrice = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.purchaseInvoiceDetailSalePrice.GetTransferModalsViewBags(ViewBag);

                var purchase = _IDUNIT.transferDetail.GetForNewSalePrice(transferDetailId);

                return View("/Views/Shared/PartialViews/AppWebForms/TransferDetailSalePrice/wpTransferDetailSalePrice.cshtml", purchase);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public IActionResult AddNewModal(Guid transferDetailId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "TransferDetailSalePrice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            var des = _IDUNIT.purchaseInvoiceDetailSalePrice.GetTransferParentCurrency(transferDetailId);

            return PartialView("/Views/Shared/PartialViews/AppWebForms/TransferDetailSalePrice/mdTransferDetailSalePriceNewModal.cshtml", des);
        }

        public IActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "TransferDetailSalePrice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }
            try
            {
                PurchaseInvoiceDetailSalePriceViewModel hos = _IDUNIT.purchaseInvoiceDetailSalePrice.GetTransferDetailSalePrice(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/TransferDetailSalePrice/mdTransferDetailSalePriceNewModal.cshtml", hos);
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
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "TransferDetailSalePrice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    return Json(_IDUNIT.purchaseInvoiceDetailSalePrice.UpdateTransferDetailSalePrice(purchaseInvoiceDetailSalePrice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "TransferDetailSalePrice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    return Json(_IDUNIT.purchaseInvoiceDetailSalePrice.AddNewTransferDetailSalePrice(purchaseInvoiceDetailSalePrice));
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
        public IActionResult UpdateMainSalePrice(Guid detailId, int typeId, string price)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "TransferDetailSalePrice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                return Json(_IDUNIT.purchaseInvoiceDetailSalePrice.UpdateMainSalePrice(detailId, typeId, price, userId));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid transferDetailId)
        {
            try
            {
                IEnumerable<PurchaseInvoiceDetailSalePriceViewModel> AllPurchaseInvoiceDetailSalePrice = _IDUNIT.purchaseInvoiceDetailSalePrice.GetAllTransferDetailSalePrices(transferDetailId, _localizer);
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
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "TransferDetailSalePrice");
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
