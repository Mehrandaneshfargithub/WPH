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
using WPH.Models.HumanResourceSalaryPayment;
using WPH.MvcMockingServices;

namespace WPH.Controllers.HumanResourceSalaryPayment
{
    [SessionCheck]
    public class HumanResourceSalaryPaymentController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<HumanResourceSalaryPaymentController> _logger;


        public HumanResourceSalaryPaymentController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<HumanResourceSalaryPaymentController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult GetHistoryPaymentsModal(Guid humanSalaryId)
        {
            try
            {
                var pay_access = _IDUNIT.subSystem.GetUserSubSystemAccess("HumanResourceSalaryPayment");
                ViewBag.AccessNewSalaryPayment = pay_access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditSalaryPayment = pay_access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteSalaryPayment = pay_access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResourceSalaryPayment/dgHumanSalaryPaymentGrid.cshtml", humanSalaryId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public IActionResult GetAllPaymentSalary([DataSourceRequest] DataSourceRequest request, Guid humanSalaryId)
        {
            try
            {
                List<HumanResourceSalaryPaymentViewModel> AllHumanSalary = _IDUNIT.humanResourceSalaryPayment.GetAllPaymentSalary(humanSalaryId).ToList();
                var result = AllHumanSalary.ToDataSourceResult(request);
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

        public async Task<IActionResult> PaySalaryModalAsync(Guid humanSalaryId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "HumanResourceSalaryPayment");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                HumanResourceSalaryPaymentViewModel viewModel = new()
                {
                    HumanResourceSalaryId = humanSalaryId,
                    Amount = _IDUNIT.humanResourceSalary.GetHumanSalaryRem(humanSalaryId)
                };
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResourceSalaryPayment/mdHumanSalaryPaymentNewModal.cshtml", viewModel);

            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResourceSalaryPayment/mdHumanSalaryPaymentNewModal.cshtml", new HumanResourceSalaryPaymentViewModel()); }
        }


        public async Task<IActionResult> PaySalaryEditModalAsync(Guid paySalaryId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HumanResourceSalaryPayment");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var viewModel = _IDUNIT.humanResourceSalaryPayment.GetPayment(paySalaryId);
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResourceSalaryPayment/mdHumanSalaryPaymentNewModal.cshtml", viewModel);

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
        public IActionResult PaySalary(HumanResourceSalaryPaymentViewModel viewModel)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (viewModel.Guid == Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "HumanResourceSalaryPayment");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.CreatedUserId = userId;
                    var result = _IDUNIT.humanResourceSalaryPayment.PaySalary(viewModel);
                    return Json(result);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HumanResourceSalaryPayment");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ModifiedUserId = userId;
                    var result = _IDUNIT.humanResourceSalaryPayment.UpdateSalary(viewModel);
                    return Json(result);
                }
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
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "HumanResourceSalaryPayment");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.humanResourceSalaryPayment.RemovePayment(Id);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayAllSalaries(Guid humanResourceId, string description)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "HumanResourceSalaryPayment");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var result = _IDUNIT.humanResourceSalaryPayment.PayAllSalaries(humanResourceId, UserId, description);
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
    }
}
