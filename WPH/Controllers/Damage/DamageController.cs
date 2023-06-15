using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Damage;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Damage
{
    [SessionCheck]
    public class DamageController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DamageController> _logger;

        public DamageController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<DamageController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Damage");
                ViewBag.AccessNewDamage = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditDamage = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteDamage = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.damage.GetModalsViewBags(ViewBag);

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel
                {
                    periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer)
                };

                return View("/Views/Shared/PartialViews/AppWebForms/Damage/wpDamage.cshtml", baseInfosAndPeriods);
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
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("DamageDetails", "Damage", "CanUseDamageDate");
            ViewBag.AccessNewDamageDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "DamageDetails");
            ViewBag.AccessEditDamageDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "DamageDetails");
            ViewBag.AccessDeleteDamageDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "DamageDetails");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Damage");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseDamageDate");

            DamageViewModel des = new DamageViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Damage/mdDamageNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate(DamageViewModel viewModel)
        {
            try
            {
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Damage");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModifiedUserId = userId;

                    return Json(_IDUNIT.damage.UpdateDamage(viewModel));
                    return Json(0);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Damage");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.CreatedUserId = userId;

                    return Json(_IDUNIT.damage.AddNewDamage(viewModel));
                    return Json(0);
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, DamageFilterViewModel filterViewModel)
        {
            try
            {
                string[] from = filterViewModel.TxtDateFrom.Split('-');
                string[] to = filterViewModel.TxtDateTo.Split('-');

                filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DamageViewModel> AllDamage = _IDUNIT.damage.GetAllDamages(clinicSectionId, filterViewModel);
                return Json(AllDamage.ToDataSourceResult(request));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult EditModal(Guid Id)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("DamageDetails", "Damage", "CanUseDamageDate");
            ViewBag.AccessNewDamageDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "DamageDetails");
            ViewBag.AccessEditDamageDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "DamageDetails");
            ViewBag.AccessDeleteDamageDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "DamageDetails");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Damage");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseDamageDate");

                DamageViewModel hos = _IDUNIT.damage.GetDamage(Id);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Damage/mdDamageNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Damage");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");


                var oStatus = _IDUNIT.damage.RemoveDamage(Id, userId, password);
                return Json(oStatus);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult TotalPriceModal(Guid Id)
        {
            try
            {

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Damage/dgDamageTotalPriceGrid.cshtml", Id);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAllTotalPrice([DataSourceRequest] DataSourceRequest request, Guid damageId)
        {
            try
            {
                var total = _IDUNIT.damage.GetAllTotalPrice(damageId);
                return Json(total.ToDataSourceResult(request));
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
