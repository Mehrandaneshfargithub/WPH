using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.Hospital;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Hospital
{
    [SessionCheck]
    public class HospitalController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<HospitalController> _logger;


        public HospitalController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<HospitalController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Hospital");
                ViewBag.AccessNewHospital = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditHospital = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteHospital = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.hospital.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Hospital/wpHospital.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Hospital");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            HospitalViewModel des = new HospitalViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Hospital/mdHospitalNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate( HospitalViewModel Hospital)
        {
            try
            {
                if (Hospital.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Hospital");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.hospital.CheckRepeatedHospitalName(Hospital.Name, false, Hospital.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid hospitalid = _IDUNIT.hospital.UpdateHospital(Hospital);
                    return Json(hospitalid);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Hospital");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.hospital.CheckRepeatedHospitalName(Hospital.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    Guid hospitalid = _IDUNIT.hospital.AddNewHospital(Hospital);
                    return Json(hospitalid);
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IEnumerable<HospitalViewModel> AllHospital = _IDUNIT.hospital.GetAllHospitals();
                return Json(AllHospital.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult GetAllHospitals()
        {
            try
            {
                IEnumerable<HospitalViewModel> AllHospital = _IDUNIT.hospital.GetAllHospitals();
                return Json(AllHospital);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }
        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Hospital");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                HospitalViewModel hos = _IDUNIT.hospital.GetHospital(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Hospital/mdHospitalNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Hospital/mdHospitalNewModal.cshtml", new HospitalViewModel()); }

        }
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Hospital");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.hospital.RemoveHospital(Id);
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
