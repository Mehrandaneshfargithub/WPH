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
using WPH.Models.ReturnPurchaseInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnPurchaseInvoiceDetail
{
    [SessionCheck]
    public class ReturnPurchaseInvoiceDetailController : Controller
    {
        //private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnPurchaseInvoiceDetailController> _logger;

        public ReturnPurchaseInvoiceDetailController(IDIUnit dIUnit, ILogger<ReturnPurchaseInvoiceDetailController> logger)
        {
            //_localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public JsonResult AddOrUpdate(ReturnPurchaseInvoiceDetailViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ReturnPurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModifiedUserId = userId;
                    string a = _IDUNIT.returnPurchaseInvoiceDetail.UpdateReturnPurchaseInvoiceDetail(viewModel, clinicSectionId);
                    return Json(a);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnPurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.CreatedUserId = userId;

                    return Json(_IDUNIT.returnPurchaseInvoiceDetail.AddNewReturnPurchaseInvoiceDetail(viewModel, clinicSectionId));
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
        public JsonResult AddList(List<ReturnPurchaseInvoiceDetailViewModel> viewModels)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnPurchaseInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                return Json(_IDUNIT.returnPurchaseInvoiceDetail.AddNewReturnPurchaseInvoiceDetailList(viewModels, clinicSectionId, userId));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid returnPurchaseInvoiceId)
        {
            try
            {
                IEnumerable<ReturnPurchaseInvoiceDetailViewModel> result = _IDUNIT.returnPurchaseInvoiceDetail.GetAllReturnPurchaseInvoiceDetails(returnPurchaseInvoiceId);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetAllChildren([DataSourceRequest] DataSourceRequest request, string children)
        {
            try
            {
                IEnumerable<SubReturnPurchaseInvoiceDetailViewModel> result = _IDUNIT.returnPurchaseInvoiceDetail.GetAllReturnPurchaseInvoiceDetailChildren(children);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult AddModal()
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnPurchaseInvoiceDetails");
            var access = _access.Any(p => p.AccessName == "New");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                ReturnPurchaseInvoiceDetailViewModel hos = new ReturnPurchaseInvoiceDetailViewModel();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDetail/mdReturnPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public ActionResult EditModal(Guid Id)
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnPurchaseInvoiceDetails");
            var access = _access.Any(p => p.AccessName == "New");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ReturnPurchaseInvoiceDetailViewModel hos = _IDUNIT.returnPurchaseInvoiceDetail.GetReturnPurchaseInvoiceDetailForEdit(Id);

                ViewBag.ReturnPurchasePaid = _IDUNIT.returnPurchaseInvoice.CheckReturnPurchaseInvoicePaid(hos.MasterId.Value);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDetail/mdReturnPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ReturnPurchaseInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.returnPurchaseInvoiceDetail.RemoveReturnPurchaseInvoiceDetail(Id);
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

        public IActionResult SelectModal()
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDetail/dgPurchaseInvoiceDetailSelectGrid.cshtml");
        }

    }
}
