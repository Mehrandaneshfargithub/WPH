using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.PatientImage;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PatientImage
{
    public class PatientImageController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<PatientImageController> _logger;
        public PatientImageController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostEnvironment, ILogger<PatientImageController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public ActionResult GetMainPatientImageByPatientId(Guid patientId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> AllRoomItem = _IDUNIT.patientImage.GetMainAttachmentsByPatientId(patientId);
                return AllRoomItem.Any() ? Json(AllRoomItem) : Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetPolicReportPatientImageByPatientId(Guid patientId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> AllRoomItem = _IDUNIT.patientImage.GetPoliceReportAttachmentsByPatientId(patientId);
                return AllRoomItem.Any() ? Json(AllRoomItem) : Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetMainPatientImageByRecptionId(Guid receptionId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> AllRoomItem = _IDUNIT.patientImage.GetMainAttachmentsByReceptionId(receptionId);
                return AllRoomItem.Any() ? Json(AllRoomItem) : Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetOtherPatientImageByRecptionId(Guid receptionId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> AllRoomItem = _IDUNIT.patientImage.GetOtherAttachmentsByReceptionId(receptionId);
                return AllRoomItem.Any() ? Json(AllRoomItem) : Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetPolicReportPatientImageByReceptionId(Guid receptionId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> AllRoomItem = _IDUNIT.patientImage.GetPoliceReportAttachmentsByReceptionId(receptionId);
                return AllRoomItem.Any() ? Json(AllRoomItem) : Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.patientImage.RemovePatientImage(Id, _hostEnvironment.WebRootPath);
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
    }
}
