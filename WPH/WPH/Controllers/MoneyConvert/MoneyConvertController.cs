using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.MoneyConvert;
using WPH.MvcMockingServices;

namespace WPH.Controllers.MoneyConvert
{
    [SessionCheck]
    public class MoneyConvertController : Controller
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<MoneyConvertController> _logger;

        public MoneyConvertController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<MoneyConvertController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("MoneyConvert");
                ViewBag.AccessNewMoneyConvert = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditMoneyConvert = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteMoneyConvert = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.moneyConvert.GetModalsViewBags(ViewBag);
                MoneyConvertViewModel money = new MoneyConvertViewModel();
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                money.periods = periods;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/MoneyConvert/wpMoneyConvertForm.cshtml", money);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "MoneyConvert");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }


            MoneyConvertViewModel MoneyConvert = new MoneyConvertViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/MoneyConvert/mdMoneyConvertModal.cshtml", MoneyConvert);
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "MoneyConvert");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                MoneyConvertViewModel MoneyConvert = _IDUNIT.moneyConvert.GetMoneyConvertById(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/MoneyConvert/mdMoneyConvertModal.cshtml", MoneyConvert);
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
        public JsonResult AddOrUpdate(MoneyConvertViewModel viewModel)
        {
            try
            {
                if (viewModel.Guid == Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "MoneyConvert");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                    return Json(_IDUNIT.moneyConvert.AddNewMoneyConvert(viewModel));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "MoneyConvert");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.moneyConvert.UpdateMoneyConvert(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            string[] from = dateFrom.Split('-');
            string[] to = dateTo.Split('-');

            DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
            DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);
            IEnumerable<MoneyConvertViewModel> AllMoneyConvert = _IDUNIT.moneyConvert.GetAllMoneyConvertByDate(clinicSectionId, periodId, fromDate, toDate);

            return Json(AllMoneyConvert.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid moneyConvertId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "MoneyConvert");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.moneyConvert.RemoveMoneyConvert(moneyConvertId);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        [HttpPost]
        public JsonResult GetConvertAmount(int BaseCurrencyId, int DestCurrencyId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                decimal? amount = _IDUNIT.moneyConvert.GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(clinicSectionId, BaseCurrencyId, DestCurrencyId);
                return Json(amount);
            }
            catch { return Json(0); }
        }

        public IActionResult GetMoneyConvertBaseCurrencyName(string baseCurrencyName, string destCurrencyName)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                var result = _IDUNIT.moneyConvert.GetMoneyConvertBaseCurrencyName(clinicSectionId, baseCurrencyName, destCurrencyName);
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
        
        public IActionResult GetMoneyConvertBaseCurrencies(int baseCurrencyId, int destCurrencyId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                var result = _IDUNIT.moneyConvert.GetMoneyConvertBaseCurrencies(clinicSectionId, baseCurrencyId, destCurrencyId);
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

        public IActionResult GetLatestMoneyConverts(int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                var result = _IDUNIT.moneyConvert.GetLatestMoneyConverts(clinicSectionId, baseCurrencyId, destCurrencyId, moneyConvertId);
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

        public IActionResult GetLatestMoneyConvertsWithIsMain(int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                var result = _IDUNIT.moneyConvert.GetLatestMoneyConvertsWithIsMain(clinicSectionId, baseCurrencyId, destCurrencyId, moneyConvertId);
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