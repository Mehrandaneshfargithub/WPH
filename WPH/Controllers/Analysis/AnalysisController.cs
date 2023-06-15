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
using WPH;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Analysis
{
    [SessionCheck]
    public class AnalysisController : Controller
    {

        string userName = string.Empty;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AnalysisController> _logger;


        public AnalysisController(IDIUnit dIUnit, ILogger<AnalysisController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                _IDUNIT.analysis.GetModalsViewBags(ViewBag);
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);

                baseInfosAndPeriods.sections = clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Analysis");
                ViewBag.AccessNewAnalysis = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditAnalysis = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteAnalysis = access.Any(p => p.AccessName == "Delete");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Analysis/wpAnalysis.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }


        }

        public async Task<ActionResult> AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Analysis");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            AnalysisViewModel analysis = new();
            analysis.CreateDate = DateTime.Now;
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("Unit");
            ViewBag.UnitId = baseInfoGuid;

            try
            {
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = sval?.SValue?.ToLower() ?? "false";
            }
            catch { }

            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
            ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues?.FirstOrDefault()?.SValue);

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Analysis/mdAnalysisModal.cshtml", analysis);
        }

        public async Task<ActionResult> EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Analysis");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                AnalysisViewModel analysis = _IDUNIT.analysis.GetAnalysis(Id);
                analysis.NameHolder = analysis.Name;
                analysis.CodeHolder = analysis.Code;

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));

                try
                {
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                    ViewBag.useDollar = sval?.SValue?.ToLower() ?? "false";
                }
                catch { ViewBag.useDollar = "false"; }

                List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Analysis/mdAnalysisModal.cshtml", analysis);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid ClinicSectionId)
        {

            try
            {
                List<AnalysisViewModel> analysis = _IDUNIT.analysis.GetAllAnalysisByClinicSectionId(ClinicSectionId);

                return Json(analysis.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);

                return Json(0);
            }
        }

        public JsonResult GetAllAnalyses()
        {
            try
            {
                IEnumerable<AnalysisViewModel> Analyses = _IDUNIT.analysis.GetAllAnalysis();
                return Json(Analyses);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllAnalysesWithoutInGroupAnalysis(Guid GroupId)
        {
            try
            {
                IEnumerable<AnalysisViewModel> Analyses = _IDUNIT.analysis.GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(GroupId);
                return Json(Analyses);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllAnalysesByClinicSectionId(Guid ClinicSectionId)
        {
            try
            {
                IEnumerable<AnalysisWithAnalysisItemViewModel> all = _IDUNIT.analysis.GetAllAnalysisWithAnalysisItems(ClinicSectionId, 11);

                return Json(all);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllAnalysisAndAnalysisItem(Guid ClinicSectionId)
        {
            try
            {
                IEnumerable<AnalysisViewModel> all = _IDUNIT.analysis.GetAllAnalysisAndAnalysisItem(ClinicSectionId);

                return Json(all);
            }
            catch (Exception e)
            {
               
                return Json(0);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Analysis");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.analysis.RemoveAnalysis(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult AddOrUpdate(AnalysisViewModel analysis)
        {
            try
            {
                if (analysis.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Analysis");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    analysis.ModifiedDate = DateTime.Now;
                    analysis.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    Guid analysisId = _IDUNIT.analysis.UpdateAnalysis(analysis);
                    return Json(analysisId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Analysis");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    analysis.CreateDate = DateTime.Now;
                    analysis.CreateUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    Guid serviceId = _IDUNIT.analysis.AddNewAnalysis(analysis);
                    return Json(serviceId);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult AddAnalysisItemToAnalysis(Analysis_AnalysisItemViewModel analysis)
        {
            try
            {
                _IDUNIT.analysis.AddNewAnalysisItemToAnalysis(analysis);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult RemoveAnalysisItemFromAnalysis(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.analysis.RemoveAnalysisItemFromAnalysis(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }


        public ActionResult Analysis_AnalysisItems()
        {
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("Unit");
                ViewBag.UnitId = baseInfoGuid;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Analysis/mdAnalysisAnalysisItemModal.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }

        }


        public ActionResult GetAllAnalysisAnalysisItem([DataSourceRequest] DataSourceRequest request, Guid AnalysisId)
        {
            try
            {
                IEnumerable<Analysis_AnalysisItemViewModel> analysis = _IDUNIT.analysis.GetAllAnalysisAnalysisItem(AnalysisId);
                return Json(analysis.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }

        }

        public JsonResult AnalysisPriorityEdit(Guid id, string type)
        {
            try
            {
                _IDUNIT.analysis.SwapPriority(id, type);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }

        }


        [HttpPost]
        public JsonResult ActiveDeactiveAnalysis(Guid AnalysisId)
        {
            try
            {
                _IDUNIT.analysis.ActiveDeactiveAnalysis(AnalysisId);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult UpdateAnalysisButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisWithAnalysisItemViewModel> allAnalysis)
        {
            try
            {
                _IDUNIT.analysis.UpdateAnalysisButtonAndPriority(clinicSectionId, allAnalysis);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                             "\t Action: " + ControllerContext.RouteData.Values["action"] +
                             "\t Message:" + e.Message);
                return Json(0);
            }
        }
    }
}