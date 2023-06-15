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
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PatientVariable
{
    public class PatientVariableController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PatientVariableController> _logger;

        public PatientVariableController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PatientVariableController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Variables");
                ViewBag.AccessNewPatientVariable = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditPatientVariable = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeletePatientVariable = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.patientVariable.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/PatientVariable/wpPatientVariablesForm.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Variables");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            PatientVariableViewModel des = new PatientVariableViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientVariable/mdPatientVariableNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate(PatientVariableViewModel PatientVariable)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (PatientVariable.Id != 0)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Variables");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.patientVariable.CheckRepeatedPatientVariableName(UserId, PatientVariable.VariableName, false, PatientVariable.VariableNameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    PatientVariable.DoctorId = UserId;

                    return Json(_IDUNIT.patientVariable.UpdatePatientVariable(PatientVariable));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Variables");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.patientVariable.CheckRepeatedPatientVariableName(UserId, PatientVariable.VariableName, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    PatientVariable.DoctorId = UserId;

                    return Json(_IDUNIT.patientVariable.AddPatientVariable(PatientVariable));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid DoctorId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<PatientVariableViewModel> AllPatientVariable = _IDUNIT.patientVariable.GetAllPatientVariables(DoctorId);
                return Json(AllPatientVariable.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult EditModal(int Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Variables");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                PatientVariableViewModel hos = _IDUNIT.patientVariable.GetPatientVariableById(Id);
                hos.VariableNameHolder = hos.VariableName;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientVariable/mdPatientVariableNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientVariable/mdPatientVariableNewModal.cshtml", new PatientVariableViewModel()); }

        }
        public JsonResult Remove(int Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Variables");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.patientVariable.RemovePatientVariable(Id);
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

        public JsonResult RemoveVariableValue(Guid Id)
        {
            try
            {
                
                OperationStatus oStatus = _IDUNIT.patientVariablesValue.Remove(Id);
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

        public JsonResult GetAllVariablesForPatient(Guid PatientId, Guid ReceptionId, string DisplayType)
        {
            try
            {
                Guid DoctorId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<PatientVariableViewModel> PatientVariables = _IDUNIT.patientVariable.GetAllVariablesForPatient(DoctorId,PatientId, ReceptionId, DisplayType);
                return Json(PatientVariables);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public JsonResult UpdatePatientVariableValue(Guid VariableValueId, string Value)
        {
            try
            {
                _IDUNIT.patientVariablesValue.UpdatePatientVariablesValueBasedOnGuid(VariableValueId, Value);
                return Json(1);
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
