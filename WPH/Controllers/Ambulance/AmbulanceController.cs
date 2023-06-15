using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WPH.Helper;
using WPH.Models.Ambulance;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Ambulance
{
    [SessionCheck]
    public class AmbulanceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AmbulanceController> _logger;
        public AmbulanceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<AmbulanceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Ambulance");
                ViewBag.AccessNewAmbulance = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditAmbulance = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteAmbulance = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.ambulance.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Ambulance/wpAmbulance.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }
        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Ambulance");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }


            AmbulanceViewModel des = new AmbulanceViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Ambulance/mdAmbulanceNewModal.cshtml", des);
        }
        public JsonResult AddOrUpdate(AmbulanceViewModel Ambulance)
        {
            try
            {
                if (Ambulance.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Ambulance");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    if (_IDUNIT.ambulance.CheckRepeatedAmbulanceName(Ambulance.Name, false, Ambulance.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.ambulance.UpdateAmbulance(Ambulance));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Ambulance");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    if (_IDUNIT.ambulance.CheckRepeatedAmbulanceName(Ambulance.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.ambulance.AddNewAmbulance(Ambulance));
                }
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IEnumerable<AmbulanceViewModel> AllAmbulance = _IDUNIT.ambulance.GetAllAmbulances();
                return Json(AllAmbulance.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }


        public ActionResult GetAllAmbulances()
        {
            try
            {
                IEnumerable<AmbulanceViewModel> AllAmbulance = _IDUNIT.ambulance.GetAllAmbulances();
                return Json(AllAmbulance);
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }


        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Ambulance");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                AmbulanceViewModel hos = _IDUNIT.ambulance.GetAmbulance(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Ambulance/mdAmbulanceNewModal.cshtml", hos);
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return PartialView("/Views/Shared/PartialViews/AppWebForms/Ambulance/mdAmbulanceNewModal.cshtml", new AmbulanceViewModel()); }

        }
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Ambulance");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                OperationStatus oStatus = _IDUNIT.ambulance.RemoveAmbulance(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }
    }
}
