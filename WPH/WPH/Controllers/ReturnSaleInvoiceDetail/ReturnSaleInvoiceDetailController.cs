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
using WPH.Models.ReturnSaleInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnSaleInvoiceDetail
{
    [SessionCheck]
    public class ReturnSaleInvoiceDetailController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnSaleInvoiceDetailController> _logger;

        public ReturnSaleInvoiceDetailController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReturnSaleInvoiceDetailController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public JsonResult AddOrUpdate(ReturnSaleInvoiceDetailViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ReturnSaleInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModifiedUserId = userId;
                    string a = _IDUNIT.returnSaleInvoiceDetail.UpdateReturnSaleInvoiceDetail(viewModel, clinicSectionId);
                    return Json(a);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnSaleInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.CreatedUserId = userId;

                    return Json(_IDUNIT.returnSaleInvoiceDetail.AddNewReturnSaleInvoiceDetail(viewModel, clinicSectionId));
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
        public JsonResult AddList(List<ReturnSaleInvoiceDetailViewModel> viewModels)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnSaleInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                return Json(_IDUNIT.returnSaleInvoiceDetail.AddNewReturnSaleInvoiceDetailList(viewModels, clinicSectionId, userId));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid returnSaleInvoiceId)
        {
            try
            {
                IEnumerable<ReturnSaleInvoiceDetailViewModel> result = _IDUNIT.returnSaleInvoiceDetail.GetAllReturnSaleInvoiceDetails(returnSaleInvoiceId);
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
                IEnumerable<SubReturnSaleInvoiceDetailViewModel> result = _IDUNIT.returnSaleInvoiceDetail.GetAllReturnSaleInvoiceDetailChildren(children);
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
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnSaleInvoiceDetails");
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
                ReturnSaleInvoiceDetailViewModel hos = new ReturnSaleInvoiceDetailViewModel
                {
                    DiscountTxt = "0",
                    NumTxt = "0",
                    FreeNumTxt = "0",
                    PriceTxt = "0",
                };

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDetail/mdReturnSaleInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public ActionResult EditModal(Guid Id)
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnSaleInvoiceDetails");
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
                ReturnSaleInvoiceDetailViewModel hos = _IDUNIT.returnSaleInvoiceDetail.GetReturnSaleInvoiceDetailForEdit(Id);

                ViewBag.ReturnSaleReceived = _IDUNIT.returnSaleInvoice.CheckReturnSaleInvoiceReceived(hos.MasterId.Value);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDetail/mdReturnSaleInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ReturnSaleInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.returnSaleInvoiceDetail.RemoveReturnSaleInvoiceDetail(Id);
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
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDetail/dgSaleInvoiceDetailSelectGrid.cshtml");
        }

    }
}
