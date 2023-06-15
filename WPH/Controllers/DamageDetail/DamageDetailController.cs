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
using WPH.Models.DamageDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.DamageDetail
{
    [SessionCheck]
    public class DamageDetailController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DamageDetailController> _logger;

        public DamageDetailController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<DamageDetailController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }


        public JsonResult AddOrUpdate(DamageDetailViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "DamageDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModifiedUserId = userId;
                    string a = _IDUNIT.damageDetail.UpdateDamageDetail(viewModel, clinicSectionId);

                    return Json(a);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "DamageDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.CreatedUserId = userId;

                    return Json(_IDUNIT.damageDetail.AddNewDamageDetail(viewModel, clinicSectionId));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid damageId)
        {
            try
            {
                IEnumerable<DamageDetailViewModel> AllDamageDetail = _IDUNIT.damageDetail.GetAllDamageDetails(damageId);
                return Json(AllDamageDetail.ToDataSourceResult(request));
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
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("DamageDetails");
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
                DamageDetailViewModel hos = new DamageDetailViewModel
                {
                    DiscountTxt = "0",
                    FreeNumTxt = "0",
                    PriceTxt = "0",
                    NumTxt = "0",
                };

                return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDetail/mdDamageDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public ActionResult EditModal(Guid Id)
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("DamageDetails");
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
                var hos = _IDUNIT.damageDetail.GetDamageDetailForEdit(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDetail/mdDamageDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "DamageDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.damageDetail.RemoveDamageDetail(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public IActionResult SelectModal()
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDetail/dgDamageDetailSelectGrid.cshtml");
        }

        public ActionResult GetDetailsForDamage([DataSourceRequest] DataSourceRequest request, Guid productId)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var result = _IDUNIT.purchaseInvoiceDetail.GetDetailsForReturn(Guid.Empty, productId, originalClinicSectionId, false);
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
    }
}
