using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.TransferDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ProductRecive
{
    [SessionCheck]
    public class ProductReciveController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ProductReciveController> _logger;

        public ProductReciveController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ProductReciveController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            ViewBag.AccessDeleteTransfer = _IDUNIT.subSystem.CheckUserAccess("Delete", "Transfer");

            return View("/Views/Shared/PartialViews/AppWebForms/ProductRecive/wpProductRecive.cshtml");
        }

        public IActionResult ConfirmProductReciveModal(Guid id)
        {
            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Transfer", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "ProductRecive");

            ViewBag.AccessDeleteTransferDetail = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Transfer");
            ViewBag.AccessNewProductRecive = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ProductRecive");

            ViewBag.AccessCanUseWholeSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
            ViewBag.AccessCanUseMiddleSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

            return View("/Views/Shared/PartialViews/AppWebForms/ProductRecive/dgProductReciveDetailGrid.cshtml", id);
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var productRecive = _IDUNIT.transfer.GetAllProductRecive(clinicSectionId);

                return Json(productRecive.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public IActionResult ResetProductReceive(Guid id)
        {
            try
            {
                _IDUNIT.transferDetail.ResetProductReceive(id);
            }
            catch (Exception e) { }

            return Json(1);
        }

        public IActionResult ConfirmAllProductRecive(Guid id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ProductRecive");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var result = _IDUNIT.transferDetail.ConfirmAllProductRecive(id, userId);

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public IActionResult ConfirmModal(Guid id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ProductRecive");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }


                var result = _IDUNIT.transferDetail.GetSourceProducName(id);


                return View("/Views/Shared/PartialViews/AppWebForms/ProductRecive/mdProductReciveNewModal.cshtml", result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddProductReceive(TransferDetailViewModel transferDetail)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ProductRecive");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                transferDetail.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                return Json(_IDUNIT.transferDetail.AddProductReceive(transferDetail, clinicSectionId));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }
    }
}
