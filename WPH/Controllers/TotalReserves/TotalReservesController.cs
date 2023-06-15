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
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.TotalReserves
{
    [SessionCheck]
    public class TotalReservesController : Controller
    {
        string userName = string.Empty;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<TotalReservesController> _logger;
        private readonly IDIUnit _IDUNIT;


        public TotalReservesController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<TotalReservesController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public async Task<IActionResult> Form()
        {
            try
            {
                ViewBag.AccessDeleteTotalReserves = _IDUNIT.subSystem.CheckUserAccess("Delete", "TotalReserves");

                ViewBag.periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                ViewBag.FirstPeriodId = (int)Periods.Day;

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseFormNumber").FirstOrDefault();

                ViewBag.useform = bool.Parse(sval?.SValue ?? "false");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/TotalReserves/wpTotalReservesForm.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            

        }

        public ActionResult GetAllReserves([DataSourceRequest] DataSourceRequest request, int periodId, DateTime dateFrom, DateTime dateTo, Guid doctorId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var user = _IDUNIT.user.GetUserWithRole(userId);

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                DateTime fromDate = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                DateTime toDate = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

                if (user.UserTypeName.ToLower() == "doctor")
                {

                    List<ReserveDetailViewModel> eventList = _IDUNIT.totalReserve.GetAllReserveByDoctorIdForSpecificDate(userId, periodId, fromDate, toDate);

                    return Json(eventList.ToDataSourceResult(request));
                }
                else
                {
                    List<Guid> doctors = new List<Guid>();

                    if (doctorId == Guid.Empty)
                    {

                        Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                        var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChild(clinicSectionId, userId);

                        var doctorList = _IDUNIT.doctor.GetDoctorsBasedOnUserSection(clinicSectionAccess.Select(p => p.Guid).ToList());
                        doctors.AddRange(doctorList.Select(p => p.Guid).ToList());
                    }
                    else
                    {
                        doctors.Add(doctorId);
                    }

                    List<ReserveDetailViewModel> eventList = _IDUNIT.totalReserve.GetAllReserveForSpecificDateBasedOnUserAccess(doctors, periodId, fromDate, toDate);

                    return Json(eventList.ToDataSourceResult(request));
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
        public JsonResult Remove(Guid id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "TotalReserves");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.reserveDetail.RemoveReserveDetail(id);

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

        public async Task<IActionResult> ConvertToVisit(Guid id)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var result = await _IDUNIT.totalReserve.ConvertToVisit(id, clinicSectionId, userId);
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