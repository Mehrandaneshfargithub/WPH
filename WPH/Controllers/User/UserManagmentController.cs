using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.CostReport;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.UserPortion;
using WPH.MvcMockingServices;

namespace WPH.Controllers.User
{
    [SessionCheck]
    public class UserManagmentController : Controller
    {
        string userName = string.Empty;


        // GET: UserManagment
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<UserManagmentController> _logger;
        protected readonly IWebHostEnvironment HostingEnvironment;


        public UserManagmentController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<UserManagmentController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            HostingEnvironment = hostingEnvironment;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Users", "UserAccess", "UserPortion");
                ViewBag.AccessNewUser = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Users");
                ViewBag.AccessEditUser = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Users");
                ViewBag.AccessDeleteUser = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Users");

                ViewBag.AccessEditUserAccess = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "UserAccess");

                ViewBag.AccessUserPortion = access.Any(p => p.AccessName == "View" && p.SubSystemName == "UserPortion");

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.user.GetModalsViewBags(ViewBag);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/wpUserManagmentForm.cshtml", false);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Users");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.UserAccessType = HttpContext.Session.GetString("UserAccessType");
            UserInformationViewModel med = new UserInformationViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdUserManagmentModal.cshtml", med);

        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Users");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                UserInformationViewModel user = _IDUNIT.user.GetUser(Id);
                user.UserNameHolder = user.UserName;
                ViewBag.UserAccessType = HttpContext.Session.GetString("UserAccessType");
                user.Pass3 = null;
                user.Pass4 = null;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdUserManagmentModal.cshtml", user);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdUserManagmentModal.cshtml", new UserInformationViewModel()); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Users");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.user.RemoveUser(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(UserInformationViewModel user)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(user.MultiSelectGuids))
                {
                    user.ClinicSectionUsers = new List<ClinicSectionUserViewModel>();
                    string[] ClinicSectionsIds = user.MultiSelectGuids.Split(',');

                    List<Guid> AllClinicSectionGuids = new List<Guid>();

                    foreach (var id in ClinicSectionsIds)
                    {
                        AllClinicSectionGuids.Add(new Guid(id));
                    }

                    AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids));

                    List<ClinicSectionUserViewModel> clinicSectionUsers = new List<ClinicSectionUserViewModel>();

                    foreach (var id in AllClinicSectionGuids)
                    {
                        ClinicSectionUserViewModel clinicSectionUser = new ClinicSectionUserViewModel();
                        if (user.Guid == Guid.Empty)
                        {
                            clinicSectionUser.UserId = user.Guid;
                        }
                        //clinicSectionUser.UserId = UserId;
                        clinicSectionUser.ClinicSectionId = id;
                        clinicSectionUser.Guid = Guid.NewGuid();
                        user.ClinicSectionUsers.Add(clinicSectionUser);
                    }


                }
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid ParentId = Guid.Parse(HttpContext.Session.GetString("ParentId"));


                user.ClinicSectionId = ClinicSectionId;
                if (ParentId == Guid.Empty)
                {
                    user.AccessTypeId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("ClinicAdmin");
                }
                else
                {
                    user.AccessTypeId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("Normal");
                }
                if (user.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Users");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.user.CheckRepeatedUserName(user.UserName, ClinicSectionId, false, user.UserNameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.user.UpdateUser(user));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Users");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.user.CheckRepeatedUserName(user.UserName, ClinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.user.AddNewUser(user));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid humanId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid ClinicId = Guid.Empty;
                string UserAccessType = HttpContext.Session.GetString("UserAccessType");
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<UserInformationViewModel> AllUsers = _IDUNIT.user.GetAllUsers(clinicSectionId, humanId/*, UserAccessType*/).ToList();
                AllUsers.RemoveAll(x => x.Guid == UserId || x.UserName == "developer");
                return Json(AllUsers.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult GetAllClinicSection([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                IEnumerable<ClinicSectionViewModel> AllClinicSection = _IDUNIT.clinicSection.GetAllClinicSectionsBasedOnClinicId(ClinicId);
                return Json(AllClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult AccessModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "UserAccess");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.userId = Id;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdUserManagmentAccessModal.cshtml");
        }

        public JsonResult Get(Guid UserId, Guid? ClinicSectionId)
        {
            try
            {
                Guid clinicSectionId = ClinicSectionId ?? Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid parentUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(clinicSectionId);
                List<HierarchyViewModel> records;
                List<SubSystemsWithAccessViewModel> subsystems = _IDUNIT.user.GetAllSubSystemsWithAccessNames(UserId, cs.SectionTypeId ?? 39, clinicSectionId, parentUserId);
                
                
                //foreach (var menu in subsystems)
                //{
                //    menu.Name = menu.ShowName;
                //    menu.ShowName = _localizer[menu.ShowName];
                //}

                records = subsystems.Where(l => l.ParentId == 0).OrderBy(l => l.ParentId)
                    .Select(l => new HierarchyViewModel
                    {
                        id = $"{l.Id}",
                        text = _localizer[l.ShowName],
                        @checked = l.Checked,
                        children = GetChildren(subsystems, l.Id)
                    }).ToList();

                records.RemoveAll(a => a.children.Count == 0);

                return this.Json(records);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        private List<HierarchyViewModel> GetChildren(List<SubSystemsWithAccessViewModel> context, int parentId)
        {
            //var all = context.Where(l => l.ParentId == parentId).OrderBy(l => l.ParentId)
            //    .Select(l => new HierarchyViewModel
            //    {
            //        id = l.Id,
            //        text = l.ShowName,
            //        @checked = l.Checked,
            //        children = GetChildren(context, l.Id)
            //    }).ToList();

            //if(all.Count)


            return context.Where(l => l.ParentId == parentId).OrderBy(l => l.ParentId)
                .Select(l => new HierarchyViewModel
                {
                    id = $"{l.ShowName}_{l.Id}",
                    text = _localizer[l.ShowName],
                    @checked = l.Checked,
                    children = GetChildren(context, l.Id)
                }).ToList();
        }

        public JsonResult SaveCheckedNodes(List<string> CheckedIds, Guid UserId, Guid? ClinicSectionId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "UserAccess");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

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

        public JsonResult UserClinicSections(string itemList, Guid userId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Users");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (itemList == null)
                {
                    itemList = "";
                }
                Guid ClinicSectionId = Guid.Empty;
                if (itemList == "")
                    ClinicSectionId = clinicSectionId;

                _IDUNIT.user.AddUserClinicSections(itemList, userId, ClinicSectionId);
                return Json(1);
            }
            catch (Exception) { return Json(0); }
        }

        public JsonResult ActiveDeActiveUser(Guid id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "UserAccess");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            _IDUNIT.user.UserActivationStatus(id);
            return Json(1);
        }




        public ActionResult AddUserToClinicModal()
        {
            try
            {
                UserInformationViewModel user = new UserInformationViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdAddUserToClinicModal.cshtml", user);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdAddUserToClinicModal.cshtml", new UserInformationViewModel()); }

        }

        public ActionResult EditUserToClinicModal(Guid Id)
        {
            try
            {
                UserInformationViewModel user = _IDUNIT.user.GetUser(Id);
                user.UserNameHolder = user.UserName;
                user.Pass3 = null;
                user.Pass4 = null;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdAddUserToClinicModal.cshtml", user);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdAddUserToClinicModal.cshtml", new UserInformationViewModel()); }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdateUserForClinic(UserInformationViewModel user)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                if (user.Guid != Guid.Empty)
                {
                    if (_IDUNIT.user.CheckRepeatedUserName(user.UserName, ClinicSectionId, false, user.UserNameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.user.UpdateUser(user));
                }
                else
                {
                    if (_IDUNIT.user.CheckRepeatedUserName(user.UserName, ClinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    user.Active = true;
                    _IDUNIT.user.AddNewUserToClinic(user);
                    return Json(1);
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

        public ActionResult GetCurrentUser()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                UserInformationViewModel user = _IDUNIT.user.GetUser(userId);

                user.UserNameHolder = user.UserName;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdEditCurrentUserModal.cshtml", user);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdEditCurrentUserModal.cshtml", new UserInformationViewModel()); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditCurrentUser(UserInformationViewModel user)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                if (_IDUNIT.user.CheckRepeatedUserName(user.UserName, ClinicSectionId, false, user.UserNameHolder))
                {
                    return Json("ValueIsRepeated");
                }
                if (user.Pass3 != user.Pass4)
                {
                    return Json("NotEqual");
                }
                if (!_IDUNIT.user.PastPassIsCorrect(user))
                {
                    return Json("OldPassError");
                }


                _IDUNIT.user.EditCurrentUser(user);
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

        public IActionResult ShowAdminLogin()
        {
            var accessTypeName = HttpContext.Session.GetString("AccessTypeName");
            if (accessTypeName != "ClinicAdmin")
                return Json(0);

            return PartialView("/Views/Shared/PartialViews/AppWebForms/UserManagment/mdAdminLoginModal.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AdminLogin(LoginViewModel model)
        {
            try
            {
                var accessTypeName = HttpContext.Session.GetString("AccessTypeName");
                if (accessTypeName != "ClinicAdmin")
                    return Json("AccessDenied");

                bool result = false;

                if (ModelState.IsValid)
                {
                    string password = Crypto.Hash(model.Pass3, "MD5");

                    UserInformationViewModel userExist = _IDUNIT.user.CheckAdminExist(model.UserName, password);
                    if (userExist != null)
                    {
                        HttpContext.Session.SetString("AdminPass", password);

                        result = true;
                    }
                }


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

        public ActionResult UserPortionForm()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("UserPortion");
                ViewBag.AccessEditUserPortion = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "UserPortion");
                ViewBag.AccessDeleteUserPortion = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "UserPortion");
                ViewBag.AccessNewUserPortion = access.Any(p => p.AccessName == "New" && p.SubSystemName == "UserPortion");
                //ViewBag.AccessDeleteReceptionDetailPay = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ReceptionDetailPay");
                
                
                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserPortion/wpUserPortionForm.cshtml", false);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult AddOrUpdateUserPortion(UserPortionViewModel user)
        {
            try
            {
                
                
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "UserPortion");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.userPortion.AddNewUserPortion(user));
                
                
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllUserPortion([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<UserPortionViewModel> AllClinicSection = _IDUNIT.userPortion.GetAllUserPortionsByClinicSection(ClinicSectionId);
                return Json(AllClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllUserPortionForDropDown()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<UserPortionViewModel> AllClinicSection = _IDUNIT.userPortion.GetAllUserPortionsByClinicSection(ClinicSectionId);
                return Json(AllClinicSection);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetAllUsersExeptUserPortions()
        {
            try
            {

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<UserInformationViewModel> AllClinicSection = _IDUNIT.user.GetAllUsersExeptUserPortions(ClinicSectionId);
                return Json(AllClinicSection);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult RemoveUserPortion(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "UserPortion");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.userPortion.RemoveUserPortion(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }

        }

        public JsonResult GetAllUserPortionsBySpecification(bool Specification, Guid ReceptionId)
        {
            try
            {

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<UserPortionViewModel> AllClinicSection = _IDUNIT.userPortion.GetAllUserPortionsBySpecification(ClinicSectionId, Specification, ReceptionId);
                return Json(AllClinicSection);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetAllReceptionDetailPayBySpecification([DataSourceRequest] DataSourceRequest request,Guid ReceptionId,bool Specification)
        {
            try
            {

                IEnumerable<ReceptionDetailPayViewModel> AllClinicSection = _IDUNIT.userPortion.GetAllReceptionDetailPayBySpecification(ReceptionId, Specification);
                return Json(AllClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult AddReceptionDetailPay(ReceptionDetailPayViewModel portion)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReceptionDetailPay");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                _IDUNIT.userPortion.AddReceptionDetailPay(portion);
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

        public JsonResult RemoveReceptionDetailPay(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ReceptionDetailPay");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.userPortion.RemoveReceptionDetailPay(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }

        }


        public ActionResult PortionReport()
        {
            try
            {
                ViewBag.AccessPrintPortionReport = _IDUNIT.subSystem.CheckUserAccess("Print", "PortionReport");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/UserPortion/wpPortionReportForm.cshtml");
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        private StiReport PortionReport(DateTime fromDate, DateTime toDate, Guid userId, bool Detail)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                StiReport report = new StiReport();
                string path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "UserPortionReport.mrt");
                report.Load(path);
                DateTime FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
                DateTime ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

                var AllCosts = _IDUNIT.userPortion.GetAllUserPortionForReport(userId, FromDate, ToDate, Detail).OrderBy(a=>a.ReceptionDate);

                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["ReceptionDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                report.Dictionary.Variables["DoctorName"].Value = _localizer["Name"];
                report.Dictionary.Variables["UserPortion"].Value = _localizer["Amount"];
                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["ServiceName"].Value = _localizer["Service"];
                report.Dictionary.Variables["ServiceAmount"].Value = _localizer["Amount"];
                report.Dictionary.Variables["UserPortion"].Value = _localizer["Portion"];
                report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                report.Dictionary.Variables["vDateFrom"].Value = fromDate.ToShortDateString();

                report.Dictionary.Variables["vDateTo"].Value = toDate.ToShortDateString();

                if (Detail)
                {
                    report.RegBusinessObject("RemReceptions", AllCosts);
                }
                else
                {
                    report.RegBusinessObject("Receptions", AllCosts);
                }

                var total = AllCosts.GroupBy(a => a.DoctorName).Select(a => new
                {
                    DoctorName = a.Key,
                    TotalAmount = a.Sum(a => a.Amount)
                });

                report.RegBusinessObject("Total", total);
                //report.Dictionary.Variables["vSection"].Value = _localizer["Section"];
                //report.Dictionary.Variables["vType"].Value = _localizer["Type"];
                //report.Dictionary.Variables["vPrice"].Value = _localizer["Price"];
                //report.Dictionary.Variables["vExplanation"].Value = _localizer["Explanation"];

                //report.Dictionary.Variables["TotalDinar"].Value = _localizer["Total"];
                //report.Dictionary.Variables["vTotal"].Value = AllCosts.Total;

                //report.RegBusinessObject("Cost", AllCosts.AllCost);
                //report.RegBusinessObject("SectionType", AllCosts.AllClinicSectionTypeCostTotal);
                //report.RegBusinessObject("Type", AllCosts.AllTypeTotal);
                //report.RegBusinessObject("Section", AllCosts.AllSectionsTotal);
                return report;
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }

        }


        public ActionResult PrintPortionReport(string fromDate, string toDate, Guid clinicSectionId, bool Detail)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "CostReport");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string[] from = fromDate.Split(':');
                DateTime dateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]));
                string[] to = toDate.Split(':');
                DateTime dateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]));
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = PortionReport(dateFrom, dateTo, clinicSectionId, Detail);

                //StiImageExportSettings set = new StiImageExportSettings() { ImageFormat = StiImageFormat.Color, CutEdges = true, ImageResolution = 200, ImageType = StiImageType.Jpeg };
                report.Render();
                List<byte[]> allb = new List<byte[]>();

                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        //CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });

                    allb.Add(stream.ToArray());
                }

                return Json(new { allb });
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