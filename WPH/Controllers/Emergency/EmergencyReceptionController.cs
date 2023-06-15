using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Emergency;
using WPH.Models.Reception;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Emergency
{
    [SessionCheck]
    public class EmergencyReceptionController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<EmergencyReceptionController> _logger;


        public EmergencyReceptionController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostEnvironment, ILogger<EmergencyReceptionController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
        public async Task<ActionResult> Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "NewNormalReception");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                ReceptionViewModel reception = new ReceptionViewModel();
                _IDUNIT.room.GetModalsViewBags(ViewBag);
                ViewBag.Emergency = true;

                return View("/Views/Shared/PartialViews/AppWebForms/Reception/wpReceptionFormNew.cshtml", reception);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
           
        }

        [ValidateAntiForgeryToken]
        public JsonResult AddReception(ReceptionViewModel reception)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid clinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                reception.ClinicId = clinicId;
                reception.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                reception.OrginalClinicSectionId = clinicSectionId;
                reception.RootPath = _hostEnvironment.WebRootPath;

                if (reception.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reception");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "NewNormalReception");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                }

                return Json(_IDUNIT.reception.AddNewReception(reception));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reception");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            _IDUNIT.room.GetModalsViewBags(ViewBag);
            ViewBag.Emergency = true;
            ViewBag.ReceptionId = id;
            ReceptionViewModel reception = new ReceptionViewModel();

            return View("/Views/Shared/PartialViews/AppWebForms/Reception/wpReceptionFormNew.cshtml", reception);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Reception");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.emergency.RemoveEmergency(Id, _hostEnvironment.WebRootPath);
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

        public JsonResult GetEmergency(Guid EmergencyId)
        {
            try
            {
                EmergencyViewModel emergency = _IDUNIT.emergency.GetEmergencyById(EmergencyId);
                return Json(emergency);
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
