using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.SectionManagement;
using WPH.Helper;
using WPH.MvcMockingServices;

namespace WPH.Areas.Admin.Controllers.SectionManagement
{
    [Area("Admin")]
    [AdminLoginCheck]
    public class SectionManagementController : Controller
    {
        private readonly IDIUnit _IDUNIT;

        public SectionManagementController(IDIUnit dIUnit)
        {
            _IDUNIT = dIUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowSections()
        {
            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/wpShowSections.cshtml");
        }

        public IActionResult ShowSubsystems()
        {
            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/wpShowSubsystems.cshtml");
        }

        public IActionResult GetAllSectionsGrid([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var sections = _IDUNIT.sectionManagement.GetAllSections();
                return Json(sections.ToDataSourceResult(request));
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult GetAllClinicSectionsGrid([DataSourceRequest] DataSourceRequest request, int sectionTypeId)
        {
            try
            {
                var clinicSections = _IDUNIT.sectionManagement.GetAllClinicSectionsBySectionTypeId(sectionTypeId);
                return Json(clinicSections.ToDataSourceResult(request));
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult GetAllSubSystemsGrid([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var subsystems = _IDUNIT.subSystem.GetAllSubsystemsWithParent();
                return Json(subsystems.ToDataSourceResult(request));
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult GetAllSectionNames()
        {
            try
            {
                var result = _IDUNIT.sectionManagement.GetAllSections();
                return Json(result);
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult GetClinicSectionType()
        {
            try
            {
                var result = _IDUNIT.sectionManagement.GetAllClinicSections();
                return Json(result);
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult GetClinicSectionParents()
        {
            try
            {
                var result = _IDUNIT.sectionManagement.GetClinicSectionParents();
                return Json(result);
            }
            catch (Exception) { return Json(0); }
        }


        public IActionResult GetSubsystemParents()
        {
            try
            {
                var result = _IDUNIT.sectionManagement.GetSubsystemParents();
                return Json(result);
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult AddNewModalSection()
        {
            SectionsNameViewModel section = new SectionsNameViewModel();
            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdSectionNewModal.cshtml", section);
        }

        public IActionResult EditModalSection(int Id)
        {
            try
            {
                var section = _IDUNIT.sectionManagement.GetSection(Id);
                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdSectionNewModal.cshtml", section);
            }
            catch (Exception) { return Json(0); }
        }

        public IActionResult AddNewModalClinicSection()
        {
            ClinicSectionNamesViewModel clinicSection = new ClinicSectionNamesViewModel();
            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdClinicSectionNewModal.cshtml", clinicSection);
        }

        public IActionResult EditModalClinicSection(Guid Id)
        {
            try
            {
                ClinicSectionNamesViewModel clinicSection = _IDUNIT.sectionManagement.GetClinicSection(Id);
                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdClinicSectionNewModal.cshtml", clinicSection);
            }
            catch (Exception) { return Json(0); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdateSection(SectionsNameViewModel section)
        {
            try
            {
                if (section.Id == 0)
                {
                    var result = _IDUNIT.sectionManagement.AddNewSection(section);
                    return Json(result);
                }
                else
                {
                    var result = _IDUNIT.sectionManagement.UpdateSection(section);
                    return Json(result);
                }
            }
            catch { return Json("SomeThingWentWrong"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdateClinicSection(ClinicSectionNamesViewModel clinicSection)
        {
            try
            {
                clinicSection.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (clinicSection.Guid == Guid.Empty)
                {
                    var result = _IDUNIT.sectionManagement.AddNewClinicSection(clinicSection);
                    return Json(result);
                }
                else
                {
                    var result = _IDUNIT.sectionManagement.UpdateClinicSection(clinicSection);
                    return Json(result);
                }
            }
            catch (Exception) { return Json("SomeThingWentWrong"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdateSubsystem(SubsystemViewModel subsystem)
        {
            try
            {
                if (subsystem.Id == 0)
                {
                    var result = _IDUNIT.subSystem.AddNewSubsystem(subsystem);
                    return Json(result);
                }
                else
                {
                    var result = _IDUNIT.subSystem.UpdateSubsystem(subsystem);
                    return Json(result);
                }
            }
            catch (Exception) { return Json("SomeThingWentWrong"); }
        }

        public JsonResult RemoveSection(int Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.sectionManagement.RemoveSection(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception) { return Json("ERROR_SomeThingWentWrong"); }
        }

        public JsonResult RemoveClinicSection(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.sectionManagement.RemoveClinicSection(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception) { return Json("ERROR_SomeThingWentWrong"); }
        }

        public JsonResult RemoveSubsystem(int Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.subSystem.RemoveSubsystem(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception) { return Json("ERROR_SomeThingWentWrong"); }
        }

        public IActionResult AddNewModalSubsystem()
        {
            SubsystemViewModel subsystem = new SubsystemViewModel();
            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdSubsystemNewModal.cshtml", subsystem);
        }

        public IActionResult EditModalSubsystem(int Id)
        {
            try
            {
                var subsystem = _IDUNIT.sectionManagement.GetSubsystem(Id);
                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdSubsystemNewModal.cshtml", subsystem);
            }
            catch (Exception) { return Json(0); }
        }

        public JsonResult ChangeSubsystemActivation(int id)
        {
            try
            {
                _IDUNIT.subSystem.ChangeSubsystemActivation(id);
                return Json(1);
            }
            catch { return Json(0); }
        }

        public ActionResult SubsystemAccessModal(int Id)
        {
            try
            {

                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/SectionManagement/mdSubsystemsAccessModal.cshtml", Id);
            }
            catch { return Json(0); }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetSubsystemAccess(int subSystemId, List<SubsystemAccessViewModel> accessList, List<SubsystemAccessViewModel> sectionTypes)
        {
            try
            {
                _IDUNIT.sectionManagement.AddSubsystemAccess(subSystemId, accessList, sectionTypes);
                return Json(1);

            }
            catch (Exception e)
            {
                if ((e.InnerException?.Message).Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return Json("ERROR_ThisRecordHasDependencyOnItInAnotherEntity");
                }
                else
                {
                    return Json(0);
                }
            }
        }

        public JsonResult GetSubsystemAccess(int subSystemId)
        {
            try
            {
                var result = _IDUNIT.sectionManagement.GetSubsystemAccess(subSystemId);
                return Json(result);
            }
            catch (Exception) { return Json(0); }
        }

        public ActionResult AccessModal()
        {
            ViewBag.userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

            return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdUserManagmentAccessModal.cshtml");
        }

        public JsonResult SaveCheckedNodes(List<string> CheckedIds, Guid UserId, Guid? ClinicSectionId)
        {
            Guid clinicSectionId = ClinicSectionId ?? Guid.Empty;
            try
            {
                if (CheckedIds != null)
                {
                    _IDUNIT.user.SaveUserAccess(UserId, CheckedIds, clinicSectionId);
                }
                else
                {
                    _IDUNIT.user.DeleteAllUserAccess(UserId, clinicSectionId);
                }

                return Json(1);
            }
            catch { return Json(0); }
        }
    }
}
