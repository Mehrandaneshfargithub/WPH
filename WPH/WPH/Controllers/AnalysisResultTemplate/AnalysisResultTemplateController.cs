using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.MvcMockingServices;
using WPH.Models.AnalysisResultTemplate;
using Microsoft.Extensions.Logging;

namespace WPH.Controllers.AnalysisResultTemplate
{
    public class AnalysisResultTemplateController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AnalysisResultTemplateController> _logger;


        public AnalysisResultTemplateController( IDIUnit dIUnit, ILogger<AnalysisResultTemplateController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                string userName = string.Empty;
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisResultTemplate.GetModalsViewBags(ViewBag);

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("AnalysisResultTemplate");
                ViewBag.AccessNewTemplate = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditTemplate = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteTemplate = access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultTemplate/wpAnalysisResultTemplate.cshtml");
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

            
        }

        public ActionResult AddNewModal()
        {

            var access = _IDUNIT.subSystem.CheckUserAccess("New", "AnalysisResultTemplate");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            AnalysisResultTemplateViewModel symptom = new AnalysisResultTemplateViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultTemplate/mdAnalysisResultTemplateModal.cshtml", symptom);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisResultTemplate");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                AnalysisResultTemplateViewModel symptom = _IDUNIT.analysisResultTemplate.GetAnalysisResultTemplate(Id);
                symptom.NameHolder = symptom.Name;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultTemplate/mdAnalysisResultTemplateModal.cshtml", symptom);
            }
            catch (Exception ) { return Json(0); }
        }

        public JsonResult GetTemplate(Guid Id)
        {
            try
            {
                AnalysisResultTemplateViewModel symptom = _IDUNIT.analysisResultTemplate.GetAnalysisResultTemplate(Id);
                return Json(symptom);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(AnalysisResultTemplateViewModel symptom)
        {
            try
            {
                if (symptom.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisResultTemplate");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.symptom.CheckRepeatedSymptomName(symptom.Name, symptom.ClinicSectionId ?? Guid.Empty, false, symptom.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid symptomId = _IDUNIT.analysisResultTemplate.UpdateAnalysisResultTemplate(symptom);
                    return Json(symptomId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "AnalysisResultTemplate");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.symptom.CheckRepeatedSymptomName(symptom.Name, symptom.ClinicSectionId ?? Guid.Empty, true))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid serviceId = _IDUNIT.analysisResultTemplate.AddNewAnalysisResultTemplate(symptom);
                    return Json(serviceId);
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }



        }


        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<AnalysisResultTemplateViewModel> Allsymptom = _IDUNIT.analysisResultTemplate.GetAllAnalysisResultTemplateByUserId(userId);
                return Json(Allsymptom.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

            
        }

        public ActionResult GetAllTemplate()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<AnalysisResultTemplateViewModel> Allsymptom = _IDUNIT.analysisResultTemplate.GetAllAnalysisResultTemplateByUserId(userId);
                return Json(Allsymptom);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "AnalysisResultTemplate");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.analysisResultTemplate.RemoveAnalysisResultTemplate(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
    }
}
