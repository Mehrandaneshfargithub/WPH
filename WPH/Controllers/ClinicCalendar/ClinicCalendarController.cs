using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ClinicCalendar;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Reserve;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ClinicCalendar
{
    public class ClinicCalendarController : Controller
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;

        public ClinicCalendarController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
        }

        public async Task<IActionResult> Form()
        {
            List<PeriodsViewModel> pre = _IDUNIT.baseInfo.GetAllPeriods(_localizer).ToList();
            pre.RemoveAll(x => x.Id == (int)Periods.Allready);
            pre.RemoveAll(x => x.Id == (int)Periods.Year);
            ViewBag.periods = pre;
            ViewBag.FromToId = (int)Periods.FromDateToDate;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicCalendar/wpClinicCalendarForm.cshtml");
        }


        public async Task<JsonResult> GetDays(int PeriodId, DateTime DateFrom, DateTime DateTo)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                DateTime fromDate = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
                DateTime toDate = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);

                IEnumerable<ReserveViewModel> eventList = _IDUNIT.clinicCalendar.GetClinicCalendarDaysByClinicSection(ClinicSectionId, PeriodId, fromDate, toDate).OrderBy(x=>x.Date);

                return Json(eventList);
            }
            catch { return Json(0); }
        }

        public async Task<JsonResult> SaveDays(IEnumerable<ClinicCalendarViewModel> AllDays, int PeriodId, DateTime DateFrom, DateTime DateTo)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                List<ReserveViewModel> Reserves = new List<ReserveViewModel>();

                foreach(var day in AllDays)
                {
                    ReserveViewModel re = new ReserveViewModel();
                    re.StartTime = new TimeSpan(day.StartTime, 0, 0);
                    re.EndTime = new TimeSpan(day.EndTime, 0, 0);
                    re.RoundTime = day.Interval;
                    re.ClinicSectionId = clinicSectionId;
                    re.Guid = day.Guid ?? Guid.Empty;
                    string[] allDay = day.Date.Split(':');
                    re.Date = new DateTime(Convert.ToInt32(allDay[0]), Convert.ToInt32(allDay[1]), Convert.ToInt32(allDay[2]));
                    Reserves.Add(re);
                }
                DateTime fromDate = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
                DateTime toDate = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);
                await _IDUNIT.clinicCalendar.SaveClinicCalendar(Reserves, clinicSectionId, PeriodId, fromDate, toDate);

                return Json(1);
            }
            catch { return Json(0); }
        }
    }
}
