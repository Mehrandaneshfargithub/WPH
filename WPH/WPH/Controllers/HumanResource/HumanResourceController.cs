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
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.Cash;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.HumanResource;
using WPH.Models.HumanResourceSalary;
using WPH.MvcMockingServices;

namespace WPH.Controllers.HumanResource
{
    [SessionCheck]
    public class HumanResourceController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<HumanResourceController> _logger;


        public HumanResourceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<HumanResourceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try
            {
                _IDUNIT.user.GetModalsViewBags(ViewBag);
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                ViewBag.PeriodDate = (int)Periods.Allready;
                ViewBag.FromToIdBeginEnd = (int)MonthPeriods.FromDateToDate;
                ViewBag.PeriodDateBeginEnd = (int)MonthPeriods.SeeAllHuman;

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("HumanResourceSalaryPayment", "SubHumanResource", "HumanResourceSalary", "Users", "UserAccess", "SalaryReport", "UserPortion");
                ViewBag.AccessNewSalaryPayment = access.Any(p => p.AccessName == "New" && p.SubSystemName == "HumanResourceSalaryPayment");
                ViewBag.AccessEditSalaryPayment = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "HumanResourceSalaryPayment");
                ViewBag.AccessDeleteSalaryPayment = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "HumanResourceSalaryPayment");

                ViewBag.AccessNewHumanResource = access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubHumanResource");
                ViewBag.AccessEditHumanResource = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SubHumanResource");
                ViewBag.AccessDeleteHumanResource = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SubHumanResource");

                ViewBag.AccessNewHumanResourceSalary = access.Any(p => p.AccessName == "New" && p.SubSystemName == "HumanResourceSalary");
                ViewBag.AccessEditHumanResourceSalary = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "HumanResourceSalary");
                ViewBag.AccessDeleteHumanResourceSalary = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "HumanResourceSalary");

                ViewBag.AccessNewUser = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Users");
                ViewBag.AccessEditUser = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Users");
                ViewBag.AccessDeleteUser = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Users");

                ViewBag.AccessEditUserAccess = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "UserAccess");
                ViewBag.AccessPrintSalaryReport = access.Any(p => p.AccessName == "Print" && p.SubSystemName == "SalaryReport");

                ViewBag.AccessUserPortion = access.Any(p => p.AccessName == "View" && p.SubSystemName == "UserPortion");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/wpHuman.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public async Task<string> Str()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
            ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues.FirstOrDefault().SValue);
            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
            var ss = "";
            try
            {
                ss = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
            }
            catch { ss = "false"; }

            return ss;
        }
        public async Task<ActionResult> AddNewModalAsync()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubHumanResource");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            HumanResourceViewModel human = new HumanResourceViewModel();
            UserInformationViewModel user = new UserInformationViewModel();
            DoctorViewModel doctor = new DoctorViewModel();
            human.Gu = user;
            human.Doctor = doctor;

            Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("DoctorSpeciality");
            ViewBag.SpeciallityId = baseInfoGuid;
            ViewBag.UserAccessType = HttpContext.Session.GetString("UserAccessType");

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
            ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdHuman.cshtml", human);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddorUpdate(HumanResourceViewModel human)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                human.Gu.AccessTypeId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("Normal");
                human.Gu.ClinicSectionId = clinicSectionId;
                if (human.Guid == Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubHumanResource");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    var result = _IDUNIT.humanResource.AddNewHuman(human);
                    return Json(result);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "SubHumanResource");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    var result = _IDUNIT.humanResource.UpdateHuman(human);
                    return Json(result);
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

        public JsonResult GetAllHuman([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<Guid> AllClinicSectionGuids = new();
                AllClinicSectionGuids.Add(clinicSectionId);
                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                IEnumerable<HumanResourceViewModel> AllHuman = _IDUNIT.humanResource.GetAllHuman(AllClinicSectionGuids);
                return Json(AllHuman.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public JsonResult GetAllHumanWithPeriod([DataSourceRequest] DataSourceRequest request, DateTime dateFrom, DateTime dateTo, int periodId, Guid humanId)  //,  
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<Guid> AllClinicSectionGuids = new();
                AllClinicSectionGuids.Add(clinicSectionId);
                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                IEnumerable<HumanResourceViewModel> AllHuman = _IDUNIT.humanResource.GetAllHumanwithPerids(AllClinicSectionGuids, periodId, dateFrom, dateTo, humanId);
                return Json(AllHuman.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public async Task<ActionResult> EditModalAsync(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SubHumanResource");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                HumanResourceViewModel human = _IDUNIT.humanResource.GetHuman(Id);
                human.Gu.UserNameHolder = human.Gu.Name;
                human.RoleTypeIdHolder = human.RoleTypeId;

                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("DoctorSpeciality");
                ViewBag.SpeciallityId = baseInfoGuid;

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdHuman.cshtml", human);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }
        public JsonResult GetFixSalary(string humanName)
        {
            try
            {
                Guid hid = _IDUNIT.humanResource.GetHumanByName(humanName);
                HumanResourceViewModel human = _IDUNIT.humanResource.GetHuman(hid);
                return Json(human.FixSalary);
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
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "SubHumanResource");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.humanResource.RemoveHuman(Id);
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

        public async Task<ActionResult> AddNewModalSalaryAsync(Guid humanResourceId, string humanResourceName)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "HumanResourceSalary");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            HumanResourceSalaryViewModel humanSalary = new()
            {
                HumanResourceId = humanResourceId,
                HumanResourceName = humanResourceName,
                BeginDate = firstDayOfMonth,
                EndDate = lastDayOfMonth
            };

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
            ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdHumanSalary.cshtml", humanSalary);
        }

        public JsonResult AddorNewSalary(HumanResourceSalaryViewModel humanResourceSalaryView)
        {
            try
            {
                string result = "";
                if (!string.IsNullOrWhiteSpace(humanResourceSalaryView.End_Date))
                    humanResourceSalaryView.EndDate = DateTime.ParseExact(humanResourceSalaryView.End_Date, "dd/MM/yyyy", null);
                humanResourceSalaryView.BeginDate = DateTime.ParseExact(humanResourceSalaryView.Begin_Date, "dd/MM/yyyy", null);

                if (humanResourceSalaryView.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HumanResourceSalary");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    humanResourceSalaryView.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    result = _IDUNIT.humanResourceSalary.UpdateHumanSalary(humanResourceSalaryView);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "HumanResourceSalary");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    humanResourceSalaryView.CreateUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    result = _IDUNIT.humanResourceSalary.AddNewHumanSalary(humanResourceSalaryView);
                }
                return Json(result);
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
        public JsonResult UpdateTreatmentStaff(HumanResourceSalaryViewModel humanResourceSalaryView)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HumanResourceSalary");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                humanResourceSalaryView.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var result = _IDUNIT.humanResourceSalary.UpdateTreatmentStaff(humanResourceSalaryView);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }


        public ActionResult GetAllHumanSalary([DataSourceRequest] DataSourceRequest request, Guid guid, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<Guid> AllClinicSectionGuids = new();
                AllClinicSectionGuids.Add(clinicSectionId);
                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                List<HumanResourceSalaryViewModel> AllHumanSalary = _IDUNIT.humanResourceSalary.GetAllHumanSalaryByParameter(AllClinicSectionGuids, guid, periodId, dateFrom, dateTo).ToList();
                var result = AllHumanSalary.ToDataSourceResult(request);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }
        public async Task<ActionResult> EditModalSalaryAsync(Guid Id)
        {

            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HumanResourceSalary");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                HumanResourceSalaryViewModel humanSalary = _IDUNIT.humanResourceSalary.GetHumanSalaryByHumanSalaryId(Id);
                humanSalary.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                humanSalary.ModifiedDate = DateTime.Now;

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval?.SValue == null) ? "false" : sval.SValue.ToLower();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdHumanSalary.cshtml", humanSalary);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public JsonResult RemoveSalary(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "HumanResourceSalary");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.humanResourceSalary.RemoveHumanSalary(Id);
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

        public ActionResult SearchHumanSalary()
        {
            try
            {
                BaseInfoGeneralsAndPeriodsViewModel baseInfoGeneralsAndPeriods = new BaseInfoGeneralsAndPeriodsViewModel();
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                List<BaseInfoGeneralViewModel> bsg = new List<BaseInfoGeneralViewModel>();
                baseInfoGeneralsAndPeriods.periods = periods;
                baseInfoGeneralsAndPeriods.baseInfoGenerals = bsg;
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                ViewBag.PeriodDate = (int)Periods.Allready;
                ViewBag.HumanGuid = Guid.Empty;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdSearchHumanSalary.cshtml", baseInfoGeneralsAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult SearchHumanBeginEnd()
        {
            try
            {
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllMonthPeriods(_localizer);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/mdSearchHuman.cshtml", periods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public JsonResult GetAllTreatmentStaff(DoctorWageViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<Guid> AllClinicSectionGuids = new();
                AllClinicSectionGuids.Add(clinicSectionId);
                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                List<HumanResourceViewModel> AllHuman = _IDUNIT.humanResource.GetAllTreatmentStaff(AllClinicSectionGuids).ToList();
                AllHuman.RemoveAll(s => s.Guid == viewModel.DoctorGuid || s.Guid == viewModel.AnesthesiologistGuid || s.Guid == viewModel.PediatricianGuid || s.Guid == viewModel.ResidentGuid);
                return Json(AllHuman);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
           
        }


        public ActionResult GetAllTreatmentStaffWage([DataSourceRequest] DataSourceRequest request, DoctorWageViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                List<Guid> AllClinicSectionGuids = new();
                AllClinicSectionGuids.Add(clinicSectionId);
                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                List<HumanResourceSalaryViewModel> AllHumanSalary = _IDUNIT.humanResourceSalary.GetAllTreatmentStaffWage(AllClinicSectionGuids, viewModel).ToList();
                return Json(AllHumanSalary.ToDataSourceResult(request));
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
        public JsonResult AddTreatmentStaffWage(HumanResourceSalaryViewModel humanResourceSalaryView)
        {
            try
            {
                string result = "";

                humanResourceSalaryView.CreateUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                result = _IDUNIT.humanResourceSalary.AddHumanWage(humanResourceSalaryView, "TreatmentStaff");

                return Json(result);
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
        public JsonResult AddPrematureCadreWage(HumanResourceSalaryViewModel humanResourceSalaryView)
        {
            try
            {
                string result = "";

                humanResourceSalaryView.CreateUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                result = _IDUNIT.humanResourceSalary.AddHumanWage(humanResourceSalaryView, "PrematureCadre");

                return Json(result);
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
        public JsonResult AddSentinelCadreWage(HumanResourceSalaryViewModel humanResourceSalaryView)
        {
            try
            {
                string result = "";

                humanResourceSalaryView.CreateUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                result = _IDUNIT.humanResourceSalary.AddHumanWage(humanResourceSalaryView, "SentinelCadre");

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult ShowReport()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "SalaryReport");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/HumanResource/wpSalaryReportForm.cshtml");
        }



        private StiReport SalaryReport(SalaryReportViewModel reportViewModel)
        {
            Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "SalaryReport.mrt");
            report.Load(path);

            reportViewModel.FromDate = new DateTime(reportViewModel.FromDate.Year, reportViewModel.FromDate.Month, reportViewModel.FromDate.Day, 0, 0, 0);
            reportViewModel.ToDate = new DateTime(reportViewModel.ToDate.Year, reportViewModel.ToDate.Month, reportViewModel.ToDate.Day, 23, 59, 59);

            reportViewModel.AllClinicSectionGuids = new List<Guid>();

            if (reportViewModel.ClinicSectionId == Guid.Empty)
            {
                reportViewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            }

            reportViewModel.AllClinicSectionGuids.Add(reportViewModel.ClinicSectionId);

            reportViewModel.AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(reportViewModel.AllClinicSectionGuids, UserId));


            ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(reportViewModel.ClinicSectionId);

            string clinicSectionName = cs.Name;
            SalaryReportResultViewModel salary = _IDUNIT.humanResourceSalary.SalaryReport(reportViewModel);


            report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
            report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
            report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
            report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
            report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToShortDateString();
            report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToShortDateString();
            report.Dictionary.Variables["TotalDinar"].Value = salary.AllPay;
            report.Dictionary.Variables["Total"].Value = _localizer["Total"];
            report.Dictionary.Variables["Name"].Value = _localizer["Name"];
            report.Dictionary.Variables["Section"].Value = _localizer["Section"];
            report.Dictionary.Variables["RecieveDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["SalaryType"].Value = _localizer["Type"];
            report.Dictionary.Variables["Amount"].Value = _localizer["Amount"];
            report.Dictionary.Variables["Payment"].Value = _localizer["Pay"];
            report.Dictionary.Variables["Rem"].Value = _localizer["Remained"];
            report.Dictionary.Variables["PaymentStatus"].Value = _localizer["Status"];
            report.Dictionary.Variables["HumanResource"].Value = _localizer["HumanResource"];
            report.Dictionary.Variables["Service"].Value = _localizer["Service"];
            report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
            report.Dictionary.Variables["ReceptionDate"].Value = _localizer["Date"] + " " + _localizer["Reception"];


            if (!reportViewModel.Detail)
            {
                report.RegBusinessObject("SalaryDetail", salary.AllSalary);
                report.RegBusinessObject("HumanDetail", salary.AllHuman);
            }
            else
            {
                report.RegBusinessObject("Salary", salary.AllSalary);
                report.RegBusinessObject("Human", salary.AllHuman);
            }

            report.RegBusinessObject("SalarySection", salary.AllSalarySection);
            return report;
        }


        public ActionResult PrintSalaryReport(SalaryReportViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "SalaryReport");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                reportViewModel.FromDate = DateTime.ParseExact(reportViewModel.TxtFromDate, "dd/MM/yyyy", null);
                reportViewModel.ToDate = DateTime.ParseExact(reportViewModel.TxtToDate, "dd/MM/yyyy", null);

                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = SalaryReport(reportViewModel);

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
