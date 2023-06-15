using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using WPH.Helper;
using WPH.Models.CustomDataModels.Disease;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.CustomDataModels.Symptom;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WPH.Controllers.Disease
{
    [SessionCheck]
    public class DiseaseController : Controller
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DiseaseController> _logger;


        public DiseaseController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<DiseaseController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Form()
        {
            string userName = "";
            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            _IDUNIT.disease.GetModalsViewBags(ViewBag);

            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Disease");
            ViewBag.AccessNewDisease = access.Any(p => p.AccessName == "New");
            ViewBag.AccessEditDisease = access.Any(p => p.AccessName == "Edit");
            ViewBag.AccessDeleteDisease = access.Any(p => p.AccessName == "Delete");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Disease/wpDisease.cshtml");
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DiseaseViewModel> AllDisease = _IDUNIT.disease.GetAllDiseases(clinicSectionId);
                return Json(AllDisease.ToDataSourceResult(request));
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public JsonResult GetAllDisease()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DiseaseViewModel> AllDisease = _IDUNIT.disease.GetAllDiseases(clinicSectionId);
                return Json(AllDisease);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public JsonResult GetAllDiseaseForListBox(Guid Id)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                List<DiseaseViewModel> disease = _IDUNIT.disease.GetAllDiseaseForListBox(clinicSectionId, Id).ToList();
                List<SelectListItem> items = disease.Select(p => new SelectListItem { Text = p.Name, Value = p.Guid.ToString() }).ToList();

                return Json(items);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public ActionResult AddNewModal()
        {

            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("Disease", "Medicine");
            var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "Disease");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.AccessNewMedicine = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "Medicine");

            DiseaseViewModel des = new DiseaseViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Disease/mdDiseaseNewModal.cshtml", des);
        }

        public ActionResult EditModal(Guid Id)
        {

            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("Disease", "Medicine");
            var access = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Disease");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ViewBag.AccessNewMedicine = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "Medicine");

                DiseaseViewModel des = _IDUNIT.disease.GetDisease(Id);
                des.NameHolder = des.Name;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Disease/mdDiseaseNewModal.cshtml", des);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }

        }

        public JsonResult GetAllMedicinesForDisease(Guid diseaseId, bool All)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                IEnumerable<MedicineForVisitViewModel> medicines = _IDUNIT.medicine.GetAllMedicinesForDisease(clinicSectionId, diseaseId, All);
                var jsonResult = Json(medicines);


                return jsonResult;

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }
        public JsonResult GetAllSymptomsForDisease(Guid diseaseId, bool All)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                IEnumerable<SymptomViewModel> symptoms = _IDUNIT.symptom.GetAllSymptomForDisease(clinicSectionId, diseaseId, All);
                return Json(symptoms);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult MedicineForDisease(string itemList, Guid DiseaseId)
        {
            try
            {
                _IDUNIT.disease.AddAllMedicineForDisease(itemList, DiseaseId);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult SymptomForDisease(string itemList, Guid DiseaseId)
        {
            try
            {
                _IDUNIT.disease.AddAllSymptomForDisease(itemList, DiseaseId);
                return Json(1);
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
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Disease");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.disease.RemoveDisease(Id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(DiseaseViewModel Disease)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                Disease.ClinicSectionId = clinicSectionId;
                if (Disease.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Disease");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.disease.CheckRepeatedDiseaseName(Disease.Name, clinicSectionId, false, Disease.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.disease.UpdateDisease(Disease));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Disease");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.disease.CheckRepeatedDiseaseName(Disease.Name, clinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.disease.AddNewDisease(Disease));
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

        public JsonResult AddDiseaseByName(string Name)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                DiseaseViewModel Disease = new();
                Disease.Name = Name;
                Disease.ClinicSectionId = clinicSectionId;
                Disease.DiseaseTypeId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("NormalDisease");

                return Json(_IDUNIT.disease.AddNewDisease(Disease));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public JsonResult GetAllDiseasesJustNameAndGuid()
        {

            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DiseaseViewModel> allDisease = _IDUNIT.disease.GetAllDiseasesJustNameAndGuid(clinicSectionId);
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