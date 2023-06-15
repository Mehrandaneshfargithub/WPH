using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System;
using WPH.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.Medicine;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using WPH.Models.CustomDataModels.PatientMedicine;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WPH.Controllers.Medicine
{
    [SessionCheck]
    public class MedicineController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<MedicineController> logger)
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
            try
            {
                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.medicine.GetModalsViewBags(ViewBag);

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Medicine");
                ViewBag.AccessNewMedicine = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditMedicine = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteMedicine = access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Medicine/wpMedicine.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(MedicineViewModel med)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (med.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Medicine");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.medicine.CheckRepeatedMedicineName(med.JoineryName, clinicSectionId, false, med.JoineryNameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid medicineId = _IDUNIT.medicine.UpdateMedicine(med);
                    return Json(medicineId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Medicine");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.medicine.CheckRepeatedMedicineName(med.JoineryName, clinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid serviceId = _IDUNIT.medicine.AddNewMedicine(med, clinicSectionId);
                    return Json(serviceId);
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
        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Medicine");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            MedicineViewModel med = new MedicineViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Medicine/mdMedicineNewModal.cshtml", med);
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Medicine");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                MedicineViewModel med = _IDUNIT.medicine.GetMedicine(Id);
                med.JoineryNameHolder = med.JoineryName;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Medicine/mdMedicineNewModal.cshtml", med);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Medicine/mdMedicineNewModal.cshtml", new MedicineViewModel()); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Medicine");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.medicine.RemoveMedicine(Id);
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
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<MedicineViewModel> AllMeds = _IDUNIT.medicine.GetAllMedicines(clinicSectionId);

                return Json(AllMeds.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public JsonResult GetAllMedicine()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<MedicineViewModel> AllMeds = _IDUNIT.medicine.GetAllMedicines(clinicSectionId);


                return Json(AllMeds);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public JsonResult MedicinePriorityEdit(Guid id, string type)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                _IDUNIT.medicine.SwapPriority(id, clinicSectionId, type);
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

        public JsonResult GetAllExpiredMedicines()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<MedicineViewModel> allMed = _IDUNIT.medicine.GetAllExpiredMedicines(clinicSectionId);
                return Json(allMed);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public JsonResult GetAllMedicineForListBox(Guid Id)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                List<MedicineViewModel> disease = _IDUNIT.medicine.GetAllMedicineForListBox(clinicSectionId, Id).ToList();

                List<SelectListItem> items = disease.Select(p => new SelectListItem { Text = p.JoineryName, Value = p.Guid.ToString() }).ToList();

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

    }
}