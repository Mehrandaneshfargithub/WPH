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
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.MvcMockingServices;

namespace WPH.Controllers.GroupAnalysis
{
    [SessionCheck]
    public class GroupAnalysisController : Controller
    {

        string userName = string.Empty;

        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<GroupAnalysisController> _logger;


        public GroupAnalysisController(IDIUnit dIUnit, ILogger<GroupAnalysisController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Form()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.groupAnalysis.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                ClinicSectionSettingValueViewModel value = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault();
                if (value != null)
                    ViewBag.CurrencyId = Convert.ToInt32(value.Id);

                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);
                baseInfosAndPeriods.sections = clinicsections.Select(p => new SectionViewModel { Id = p.Guid, Name = p.Name }).ToList();

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("GroupAnalysis");
                ViewBag.AccessNewGroupAnalysis = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditGroupAnalysis = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteGroupAnalysis = access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/wpGroupAnalysis.cshtml", baseInfosAndPeriods);
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

            var access = _IDUNIT.subSystem.CheckUserAccess("New", "GroupAnalysis");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            GroupAnalysisViewModel groupAnalysis = new();
            groupAnalysis.CreatedDate = DateTime.Now;

            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
            try
            {
                ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues.FirstOrDefault().SValue);
            }
            catch
            {
                ViewBag.CurrencyTypeId = 11;
            }

            groupAnalysis.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();
            foreach (var dec in css)
            {
                if (dec.SName == "DinarDecimalAmount")
                    groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                else if (dec.SName == "DollarDecimalAmount")
                    groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                else if (dec.SName == "EuroDecimalAmount")
                    groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                else if (dec.SName == "PondDecimalAmount")
                    groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
            }
            return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/mdGroupAnalysisModal.cshtml", groupAnalysis);
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "GroupAnalysis");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            try
            {
                GroupAnalysisViewModel groupAnalysis = _IDUNIT.groupAnalysis.GetGroupAnalysis(Id);

                List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
                groupAnalysis.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();
                foreach (var dec in css)
                {
                    if (dec.SName == "DinarDecimalAmount")
                        groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "DollarDecimalAmount")
                        groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "EuroDecimalAmount")
                        groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "PondDecimalAmount")
                        groupAnalysis.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                }
                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/mdGroupAnalysisModal.cshtml", groupAnalysis);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid clinicSectionId)
        {
            try
            {
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;
                IEnumerable<GroupAnalysisViewModel> groupAnalysis = _IDUNIT.groupAnalysis.GetAllGroupAnalysis(clinicSectionId, fromDate, toDate);
                return Json(groupAnalysis.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }
        public JsonResult GetAllGroupAnalysis()
        {
            try
            {
                IEnumerable<GroupAnalysisViewModel> GroupAnalysis = _IDUNIT.groupAnalysis.GetAllGroupAnalysis();
                return Json(GroupAnalysis);
            }
            catch (Exception) { return Json(0); }
        }


        public JsonResult GetAllGroupAnalysisByClinicSectionId(Guid ClinicSectionId)
        {
            try
            {
                List<GroupAnalysisJustNameAndGuid> all = _IDUNIT.groupAnalysis.GetAllGroupAnalysisWithNameAndGuidOnly(ClinicSectionId, 11);

                return Json(all);
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
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "GroupAnalysis");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.groupAnalysis.RemoveGroupAnalysis(Id);
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
        public JsonResult AddOrUpdate(GroupAnalysisViewModel groupAnalysis)
        {
            try
            {
                Guid userid = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (groupAnalysis.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "GroupAnalysis");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    groupAnalysis.ModifiedDate = DateTime.Now;
                    groupAnalysis.ModifiedUserId = userid;
                    Guid groupAnalysisId = _IDUNIT.groupAnalysis.UpdateGroupAnalysis(groupAnalysis);
                    return Json(groupAnalysisId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "GroupAnalysis");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    groupAnalysis.CreatedDate = DateTime.Now;
                    groupAnalysis.CreatedUserId = userid;
                    Guid serviceId = _IDUNIT.groupAnalysis.AddNewGroupAnalysis(groupAnalysis);
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
        public JsonResult GroupAnalysisPriorityEdit(Guid id, string type)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                _IDUNIT.groupAnalysis.SwapPriority(id, clinicSectionId, type);
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


        public ActionResult AddItemToGroupAnalysis()
        {
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/mdGroupAnalysisAndAnalysisItems.cshtml");
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
        public JsonResult ActiveDeactiveAnalysis(Guid AnalysisId)
        {
            try
            {

                _IDUNIT.groupAnalysis.ActiveDeactiveAnalysis(AnalysisId);
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
        public JsonResult UpdateGroupAnalysisButtonAndPriority(Guid clinicSectionId, IEnumerable<GroupAnalysisJustNameAndGuid> allGroup)
        {
            try
            {

                _IDUNIT.groupAnalysis.UpdateGroupAnalysisButtonAndPriority(clinicSectionId, allGroup);
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

    }
}