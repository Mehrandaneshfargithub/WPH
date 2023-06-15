using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Dashboard;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ClinicSection
{
    [SessionCheck]
    public class ClinicSectionController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;

        public ClinicSectionController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
        }


        public ActionResult Form()
        {
            string userName = "";
            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            _IDUNIT.clinicSection.GetModalsViewBags(ViewBag);
            return View("/Views/Shared/PartialViews/AppWebForms/ClinicSection/wpClinicSectionForm.cshtml");
        }

        public ActionResult GetAllLabAndXRayClinicSectionsForUser()
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            string username = HttpContext.Session.GetString("UserName");
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            List<ClinicSectionViewModel> csvm = new List<ClinicSectionViewModel>();
            if (username == "developer")
                csvm = _IDUNIT.clinicSection.GetAllClinicSectionsBasedOnClinicSectionId(ClinicSectionId);
            else
                csvm = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "LabratoryRadiology", ClinicSectionId);
            return Json(csvm);

        }

        public ActionResult GetAllNormalClinicSectionsForUser()
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            string username = HttpContext.Session.GetString("UserName");
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            List<ClinicSectionViewModel> csvm = new List<ClinicSectionViewModel>();
            if (username == "developer")
                csvm = _IDUNIT.clinicSection.GetAllClinicSectionsBasedOnClinicSectionId(ClinicSectionId);
            else
                csvm = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "NotLab", ClinicSectionId);
            return Json(csvm);

        }

        public ActionResult Dashboard()
        {

            if (HttpContext.Session.GetString("SectionTypeName") == "Hospital")
            {
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("NewNormalReception", "AllSurgeries", "Room", "HospitalPatient", "Children", "Service", "AllReception");
                ViewBag.AccessNewNormalReception = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "NewNormalReception");
                ViewBag.AccessAllSurgeries = access1.Any(p => p.AccessName == "View" && p.SubSystemName == "AllSurgeries");
                ViewBag.AccessRoom = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Room");
                ViewBag.AccessHospitalPatient = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "HospitalPatient");
                ViewBag.AccessChildren = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Children");
                ViewBag.AccessService = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Service");
                ViewBag.AccessAllReception = access1.Any(p => p.AccessName == "View" && p.SubSystemName == "AllReception");
                return PartialView("~/Views/Shared/PartialViews/AppWebForms/Home/HospitalDashboard.cshtml");
        }
            else if (HttpContext.Session.GetString("SectionTypeName") == "Store")
            {
                return PartialView("~/Views/Shared/PartialViews/AppWebForms/Home/StoreDashboard.cshtml");
            }
            else if (HttpContext.Session.GetString("SectionTypeName") == "Clinic")
            {
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Reserve", "TotalReserves", "Visit", "TotalVisits", "Patient");
                ViewBag.AccessReserve = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Reserve");
                ViewBag.AccessTotalReserves = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TotalReserves");
                ViewBag.AccessVisit = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Visit");
                ViewBag.AccessTotalVisits = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TotalVisits");
                ViewBag.AccessPatient = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Patient");
                return PartialView("~/Views/Shared/PartialViews/AppWebForms/Home/ClinicDashboard.cshtml");
            }
            else
            {
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("NewAnalysisReception", "AllReception", "Patient");
                ViewBag.AccessNewReception = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "NewAnalysisReception");
                ViewBag.AccessAllReception = access1.Any(p => p.AccessName == "View" && p.SubSystemName == "AllReception");
                ViewBag.AccessPatient = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Patient");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Home/LabDashboard.cshtml");
            }
            
        }


        public JsonResult GetAllClinicSections()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetAllClinicSectionsBasedOnClinicId(ClinicSectionId);
                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllClinicSectionWithType([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                List<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetAllClinicSectionWithType(ClinicSectionId);

                return Json(AllClinicSections.ToDataSourceResult(request));
            }
            catch { return Json(0); }
        }


        public JsonResult GetAllClinicSectionsForUserWithParent(Guid UserId)
        {
            try
            {
                Guid ParentId = Guid.Parse(HttpContext.Session.GetString("ParentId"));

                if (UserId == Guid.Empty)
                {
                    UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                }
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string parentName = HttpContext.Session.GetString("ClinicSectionName");

                List<ClinicSectionViewModel> AllClinicSections = new List<ClinicSectionViewModel>();

                if (ParentId == Guid.Empty)
                {
                    AllClinicSections = _IDUNIT.clinicSection.GetClinicSectionsForUser(UserId, "", ClinicSectionId);
                }

                AllClinicSections.Add(new ClinicSectionViewModel
                {
                    Guid = ClinicSectionId,
                    Name = parentName
                });


                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllClinicSectionsForUserByAccess()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                List<ClinicSectionViewModel> AllClinicSections = new List<ClinicSectionViewModel>();

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ShowClinicSectionChild", "ShowAllOrAccessedClinicSections");
                var showChild = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ShowClinicSectionChild");
                var showAllClinicSections = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ShowAllOrAccessedClinicSections");
                
                if (showAllClinicSections)
                {
                    AllClinicSections = _IDUNIT.clinicSection.GetAllClinicSectionsWithChilds(ClinicId, showChild);
                }
                else
                {
                    AllClinicSections = _IDUNIT.clinicSection.GetAllAccessedUserClinicSectionWithChilds(UserId, showChild);
                }


                //AllClinicSections.Add(new ClinicSectionViewModel
                //{
                //    Guid = ClinicSectionId,
                //    Name = parentName
                //});


                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllClinicSectionParentsForUser()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (UserId == Guid.Empty)
                {
                    UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                }
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                string parentName = HttpContext.Session.GetString("ClinicSectionName");

                List<ClinicSectionViewModel> AllClinicSections = new List<ClinicSectionViewModel>();

                
                    AllClinicSections = _IDUNIT.clinicSection.GetAllClinicSectionParentsForUser(UserId, ClinicId);

                AllClinicSections.Remove(AllClinicSections.FirstOrDefault(a => a.Name == "Main"));
                AllClinicSections.Remove(AllClinicSections.FirstOrDefault(a => a.Name == parentName));
                //AllClinicSections.Add(new ClinicSectionViewModel
                //{
                //    Guid = ClinicSectionId,
                //    Name = parentName
                //});


                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllClinicSectionsForUser(Guid UserId)
        {
            try
            {
                if (UserId == Guid.Empty)
                {
                    UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                }
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetClinicSectionsForUser(UserId, "", ClinicSectionId);
                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllUserClinicSections()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetAllUserClinicSections(UserId, ClinicSectionId);
                return Json(AllClinicSections);
            }
            catch { return Json(0); }
        }

        //public JsonResult GetAllUserClinicSection(Guid UserId)
        //{
        //    try
        //    {
        //        if (UserId == Guid.Empty)
        //        {
        //            UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        //        }
        //        Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

        //        IEnumerable<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetClinicSectionsForUser(UserId, "", ClinicSectionId);
        //        return Json(AllClinicSections);
        //    }
        //    catch { return Json(0); }
        //}


        public JsonResult ChangeSettingValue(string value, string Name)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                _IDUNIT.clinicSection.SaveClinicSectionSettingValue(ClinicSectionId, value, Name);
                return Json(1);
            }
            catch { return Json(0); }
        }


        public async Task<JsonResult> GetClinicSectionSettingValueBySettingName(string settingName)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                ClinicSectionSettingValueViewModel value = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, settingName).FirstOrDefault();
                if (value != null)
                    if (value.SValue.Equals(typeof(string)))
                        return Json(value.SValue.ToLower());
                    else
                        return Json(value.SValue);
                else
                    return Json("");
            }
            catch { return Json(0); }
        }



        public ActionResult AddNewModal()
        {
            try
            {
                ClinicSectionViewModel des = new ClinicSectionViewModel();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicSection/mdClinicSectionModal.cshtml", des);
            }
            catch (Exception e) { throw e; }
        }
        public ActionResult EditModal(Guid Id)
        {
            ClinicSectionViewModel des = _IDUNIT.clinicSection.GetClinicSectionById(Id);
            des.NameHolder = des.Name;
            des.SystemCodeHolder = des.SystemCode;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicSection/mdClinicSectionModal.cshtml", des);
        }


        public JsonResult AddOrUpdate(ClinicSectionViewModel Entity)
        {
            try
            {
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                Entity.ClinicId = ClinicId;
                Entity.ParentId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                if (Entity.Guid != Guid.Empty)
                {
                    if (_IDUNIT.clinicSection.CheckRepeatedClinicNameAndCode(Entity.Name, false, Entity.SystemCode, Entity.NameHolder, Entity.SystemCodeHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid diseaseId = _IDUNIT.clinicSection.UpdateClinicSection(Entity);
                    return Json(diseaseId);
                }
                else
                {
                    if (_IDUNIT.clinicSection.CheckRepeatedClinicNameAndCode(Entity.Name, true, Entity.SystemCode))
                    {
                        return Json("ValueIsRepeated");
                    }

                    //Disease.Guid = Guid.NewGuid();
                    Guid clinicId = _IDUNIT.clinicSection.AddNewClinicSection(Entity);
                    return Json(clinicId);
                }
            }
            catch { return Json(0); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.clinicSection.RemoveClinicSection(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }

        }

        public JsonResult GetAllDashboardDatas()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DashboardViewModel> allVisits = _IDUNIT.clinicSection.GetAllDashboardDatas(ClinicSectionId).OrderByDescending(x => x.Date);
                return Json(allVisits);
            }
            catch { return Json(0); }
        }

        public JsonResult GetClinicSectionIdByName(string ClinicSectionName)
        {
            try
            {
                Guid ClinicSectionId = _IDUNIT.clinicSection.GetClinicSectionIdByName(ClinicSectionName);
                return Json(ClinicSectionId);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAllClinicSectionsChild(Guid clinicSectionId)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            if (clinicSectionId == Guid.Empty)
                clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            List<ClinicSectionViewModel> csvm = _IDUNIT.clinicSection.GetAllClinicSectionsChild(clinicSectionId, userId).ToList();

            return Json(csvm);

        }

        public JsonResult GetAllParentClinicSections()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            List<ClinicSectionViewModel> csvm = _IDUNIT.clinicSection.GetAllParentClinicSections().ToList();
            csvm.Remove(csvm.SingleOrDefault(x => x.Guid == clinicSectionId));
            return Json(csvm);

        }
    }
}