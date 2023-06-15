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
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices;

namespace WPH.Controllers.AnalysisItemValuesRange
{
    public class AnalysisItemValuesRangeController : Controller
    {

        string userName = string.Empty;
        
        public ActionResult Index()
        {
            return View();
        }

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AnalysisItemValuesRangeController> _logger;

        public AnalysisItemValuesRangeController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<AnalysisItemValuesRangeController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisItemValuesRange.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                int sectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                try
                {
                    ViewBag.CurrencyId = Convert.ToInt32(_IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault().Id);
                }
                catch
                {
                    ViewBag.CurrencyId = 11;
                }
                IEnumerable<AnalysisViewModel> Analyses = _IDUNIT.analysis.GetAllAnalysis();
                ViewBag.AnalysisFirstId = Analyses.FirstOrDefault().Guid;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemValuesRange/wpAnalysisItemValuesRange.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisItemValuesRange.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemValuesRange/wpAnalysisItemValuesRange.cshtml", baseInfosAndPeriods);
            }
        }

        public async Task<IActionResult> AddNewModal()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            AnalysisItemValuesRangeViewModel AnalysisItemValuesRange = new AnalysisItemValuesRangeViewModel();
            //_IDUNIT.analysisItemValuesRange.DoFormLanguage(ViewBag, HttpContext);
            int sectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));

            try
            {
                var sval =  _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault();

                ViewBag.CurrencyTypeId = Convert.ToInt32(sval.SValue);
            }
            catch
            {
                ViewBag.CurrencyTypeId = 11;
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemValuesRange/mdAnalysisItemValuesRangeModal.cshtml", AnalysisItemValuesRange);
        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                AnalysisItemValuesRangeViewModel AnalysisItemValuesRange = _IDUNIT.analysisItemValuesRange.GetAnalysisItemValuesRange(Id);
                //_AnalysisItemValuesRangeMvcService.DoFormLanguage(ViewBag, HttpContext);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemValuesRange/mdAnalysisItemValuesRangeModal.cshtml", AnalysisItemValuesRange);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid AnalysisItemId)
        {
            try
            {
                IEnumerable<AnalysisItemValuesRangeViewModel> AnalysisItemValuesRange = _IDUNIT.analysisItemValuesRange.GetAllAnalysisItemValuesRange(AnalysisItemId);
                return Json(AnalysisItemValuesRange.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
            
        }
        public JsonResult GetAllAnalysisItemValuesRange(Guid AnalysisItemId)
        {
            try
            {
                IEnumerable<AnalysisItemValuesRangeViewModel> AnalysisItemValuesRange = _IDUNIT.analysisItemValuesRange.GetAllAnalysisItemValuesRange(AnalysisItemId);
                return Json(AnalysisItemValuesRange);
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
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.analysisItemValuesRange.RemoveAnalysisItemValuesRange(Id);
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

        public OperationStatus RemoveAnalysisItemValuesRangeWithAnalysisItemId(Guid AnalysisItemId)
        {
            try
            {
                _IDUNIT.analysisItemValuesRange.RemoveAllWithAnalysisItemId(AnalysisItemId);
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }


        [HttpPost]
        public JsonResult AddOrUpdate(string Values, Guid AnalysisItemId)
        {
            try
            {
                Guid AnalysisItemValuesRangeId = new Guid();
                string[] valuesSplit = Values.Split(',');
                for (int i = 0; i < valuesSplit.Length; i++)
                {
                    string[] CurrentValue = valuesSplit[i].Split('_');
                    string Value = CurrentValue[0];
                    string Default = CurrentValue[1];
                    AnalysisItemValuesRangeViewModel AnalysisItemValuesRange = new AnalysisItemValuesRangeViewModel();
                    AnalysisItemValuesRange.AnalysisItemId = AnalysisItemId;
                    AnalysisItemValuesRange.Default = Default=="1"?true:false;
                    AnalysisItemValuesRange.Value = Value.Trim();
                    AnalysisItemValuesRangeId = _IDUNIT.analysisItemValuesRange.AddNewAnalysisItemValuesRange(AnalysisItemValuesRange);
                }

                return Json(AnalysisItemValuesRangeId);
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