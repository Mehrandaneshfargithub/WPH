using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Helper;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using WPH.Models.Doctor;
using Microsoft.AspNetCore.Hosting;

namespace WPH.Controllers.Doctor
{
    [SessionCheck]
    public class DoctorController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DoctorController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DoctorController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<DoctorController> logger, IWebHostEnvironment hostEnvironment)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }


        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("SubDoctor", "Users");
                ViewBag.AccessNewSubDoctor = access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubDoctor");
                ViewBag.AccessEditSubDoctor = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SubDoctor");
                ViewBag.AccessDeleteSubDoctor = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SubDoctor");

                ViewBag.AccessEditUser = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Users");

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.doctor.GetModalsViewBags(ViewBag);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Doctor/wpDoctorForm.cshtml");
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
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubDoctor");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            DoctorViewModel Doctor = new DoctorViewModel();
            UserInformationViewModel user = new UserInformationViewModel();
            Doctor.User = user;
            Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("DoctorSpeciality");
            ViewBag.SpeciallityId = baseInfoGuid;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Doctor/mdDoctorNewModal.cshtml", Doctor);
        }


        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubDoctor");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                DoctorViewModel Doctor = _IDUNIT.doctor.GetDoctor(Id);
                Doctor.NameHolder = Doctor.User.Name;
                Doctor.PhoneNumberHolder = Doctor.User.PhoneNumber;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Doctor/mdDoctorNewModal.cshtml", Doctor);
            }
            catch { return Json(0); }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(DoctorViewModel Doctor)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (Doctor.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubDoctor");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.doctor.CheckRepeatedNameAndSpeciallity(Doctor.User.Name, null, clinicSectionId, false, Doctor.NameHolder, Doctor.SpecialityId))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Doctor.ClinicSectionId = clinicSectionId;
                    return Json(_IDUNIT.doctor.UpdateDoctor(Doctor));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubDoctor");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.doctor.CheckRepeatedNameAndSpeciallity(Doctor.User.Name, null, clinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    Doctor.ClinicSectionId = clinicSectionId;
                    Doctor.User.ClinicSectionId = clinicSectionId;
                    Doctor.User.Pass1 = "123";
                    Doctor.User.UserName = "fgh";

                    return Json(_IDUNIT.doctor.AddNewDoctor(Doctor));
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
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid ParentId = Guid.Parse(HttpContext.Session.GetString("ParentId"));
                List<DoctorViewModel> AllDoctor;
                if (ParentId == Guid.Empty)
                {
                    AllDoctor = _IDUNIT.doctor.GetAllDoctor(true);
                }
                else
                {
                    AllDoctor = _IDUNIT.doctor.GetAllDoctor(true, clinicSectionId);
                }

                return Json(AllDoctor.ToDataSourceResult(request));
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult GetAllDoctors()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DoctorViewModel> AllDoctor = _IDUNIT.doctor.GetAllDoctor(false, clinicSectionId);
                return Json(AllDoctor);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult GetAllDoctorsForFilter()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DoctorFilterViewModel> AllDoctor = _IDUNIT.doctor.GetAllDoctorsForFilter(clinicSectionId);
                return Json(AllDoctor);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public JsonResult GetDoctorById(Guid DoctorId)
        {
            try
            {
                DoctorViewModel Doctor = _IDUNIT.doctor.GetDoctor(DoctorId);
                return Json(Doctor);
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
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "SubDoctor");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.doctor.RemoveDoctor(Id);
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

        public JsonResult ConvertDoctorToUser(Guid id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Users");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                var result = _IDUNIT.doctor.ConvertDoctorToUser(id);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public JsonResult GetDoctorsBaseUserAccess()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var user = _IDUNIT.user.GetUserWithRole(userId);

                if (user.UserTypeName.ToLower() == "doctor")
                {
                    user.UserName = user.Name;

                    return Json(new List<DoctorViewModel>
                    {
                        new DoctorViewModel {
                            Guid = user.Guid,
                            UserName = user.Name
                        }
                    });
                }
                else
                {
                    Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                    var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChild(clinicSectionId, userId);

                    var doctors = _IDUNIT.doctor.GetDoctorsBasedOnUserSection(clinicSectionAccess.Select(p => p.Guid).ToList());
                    return Json(doctors);
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

        public JsonResult GetDoctorsBaseClinicSectionAccess(Guid clinicSectionId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var user = _IDUNIT.user.GetUserWithRole(userId);

                if (user.UserTypeName.ToLower() == "doctor")
                {
                    user.UserName = user.Name;

                    return Json(new List<DoctorViewModel>
                    {
                        new DoctorViewModel {
                            Guid = user.Guid,
                            UserName = user.Name
                        }
                    });
                }
                else
                {
                    var clinicSectionAccess = new List<Guid>
                    {
                        clinicSectionId
                    };

                    var doctors = _IDUNIT.doctor.GetDoctorsBasedOnUserSection(clinicSectionAccess);
                    return Json(doctors);
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

        public ActionResult DoctorReportLogoModal(Guid Id)
        {
            DoctorViewModel doctor = _IDUNIT.doctor.GetDoctorLogoAddress(Id);

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Doctor/mdDoctorLogoModal.cshtml", doctor);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDoctorReportLogo(Guid doctorId, IFormFile reportLogo)
        {
            try
            {
                string rootPath = _hostEnvironment.WebRootPath;

                return Json(_IDUNIT.doctor.SaveDoctorReportLogo(doctorId, reportLogo, rootPath));
            }
            catch (Exception e) { return Json(""); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveDoctorReportLogo(Guid doctorId)
        {
            try
            {
                string rootPath = _hostEnvironment.WebRootPath;

                _IDUNIT.doctor.RemoveDoctorReportLogo(doctorId, rootPath);
                return Json("");
            }
            catch(Exception e) { return Json(0); }
        }
    }
}