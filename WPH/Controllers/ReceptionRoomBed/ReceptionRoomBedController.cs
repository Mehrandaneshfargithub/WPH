using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.ReceptionRoomBed;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReceptionRoomBed
{
    [SessionCheck]
    public class ReceptionRoomBedController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReceptionRoomBedController> _logger;
        public ReceptionRoomBedController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReceptionRoomBedController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, Guid roomId, Guid roomBedId, Guid patientId)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');
                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                string username = HttpContext.Session.GetString("UserName");
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                IEnumerable<ReceptionRoomBedViewModel> receptionRoomBeds;
                if (username == "developer")
                {
                    receptionRoomBeds = _IDUNIT.receptionRoomBed.FilterReceptionByRoomAndBedAndPatient(roomId, roomBedId, patientId, periodId, fromDate, toDate);
                }
                else
                {
                    receptionRoomBeds = _IDUNIT.receptionRoomBed.FilterReceptionByRoomAndBedAndPatientAndUser(userId, roomId, roomBedId, patientId, periodId, fromDate, toDate);
                }
                return Json(receptionRoomBeds.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }

        [HttpPost]
        public ActionResult CloseOldReceptionRoomBed(Guid receptionId)
        {
            try
            {
                _IDUNIT.receptionRoomBed.CloseOldReceptionRoomBed(receptionId);
                return Json(0);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        public JsonResult GetReceptionRoomBedName(Guid ReceptionId)
        {
            try
            {
                string hos = _IDUNIT.roomBed.GetReceptionRoomBedName(ReceptionId);
                return Json(hos);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }


        public JsonResult GetReceptionRoomBedId(Guid ReceptionId)
        {
            try
            {
                var hos = _IDUNIT.roomBed.GetReceptionRoomBedId(ReceptionId);
                return Json(hos);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }


        }
    }
}
