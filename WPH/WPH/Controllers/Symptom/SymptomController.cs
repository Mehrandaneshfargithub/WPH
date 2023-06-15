using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WPH.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.Symptom;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WPH.Controllers.Symptom
{
    [SessionCheck]
    public class SymptomController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<SymptomController> _logger;


        public SymptomController(IDIUnit dIUnit, ILogger<SymptomController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }


        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Symptom");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            SymptomViewModel symptom = new SymptomViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Symptom/mdSymptomNewModal.cshtml", symptom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(SymptomViewModel symptom)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                symptom.ClinicSectionId = clinicSectionId;

                if (symptom.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Symptom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.symptom.CheckRepeatedSymptomName(symptom.Name, clinicSectionId, false, symptom.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid symptomId = _IDUNIT.symptom.UpdateSymptom(symptom);
                    return Json(symptomId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Symptom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.symptom.CheckRepeatedSymptomName(symptom.Name, clinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid serviceId = _IDUNIT.symptom.AddNewSymptom(symptom);
                    return Json(serviceId);
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

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Symptom");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                SymptomViewModel symptom = _IDUNIT.symptom.GetSymptom(Id);
                symptom.NameHolder = symptom.Name;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Symptom/mdSymptomNewModal.cshtml", symptom);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult Form()
        {
            try
            {
                string userName = string.Empty;
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.symptom.GetModalsViewBags(ViewBag);

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Symptom");
                ViewBag.AccessNewSymptom = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditSymptom = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteSymptom = access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Symptom/wpSymptom.cshtml");
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

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<SymptomViewModel> Allsymptom = _IDUNIT.symptom.GetAllSymptom(clinicSectionId);
                return Json(Allsymptom.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Symptom");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.symptom.RemoveSymptom(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json("ERROR_SomeThingWentWrong");
            }
        }

        public JsonResult GetAllSymptomJustNameAndGuid()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<SymptomViewModel> allDisease = _IDUNIT.symptom.GetAllSymptomJustNameAndGuid(clinicSectionId);
                return Json(allDisease);
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