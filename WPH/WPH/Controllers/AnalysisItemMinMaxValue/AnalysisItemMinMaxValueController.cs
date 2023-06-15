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
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.MvcMockingServices;

namespace WPH.Controllers.AnalysisItemMinMaxValue
{
    public class AnalysisItemMinMaxValueController : Controller
    {

        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AnalysisItemMinMaxValueController> _logger;

        public AnalysisItemMinMaxValueController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<AnalysisItemMinMaxValueController> logger)
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
                _IDUNIT.analysisItemMinMaxValue.GetModalsViewBags(ViewBag);
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
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemMinMaxValue/wpAnalysisItemMinMaxValue.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisItemMinMaxValue.GetModalsViewBags(ViewBag);
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemMinMaxValue/wpAnalysisItemMinMaxValue.cshtml", baseInfosAndPeriods);
            }
        }
        public async Task<IActionResult> AddNewModal()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue = new AnalysisItemMinMaxValueViewModel();

            ClinicSectionSettingValueViewModel sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault();
            try
            {
                ViewBag.CurrencyTypeId = Convert.ToDecimal(sval.SValue);
            }
            catch { ViewBag.CurrencyTypeId = 11; }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemMinMaxValue/mdAnalysisItemMinMaxValueModal.cshtml", AnalysisItemMinMaxValue);
        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue = _IDUNIT.analysisItemMinMaxValue.GetAnalysisItemMinMaxValue(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItemMinMaxValue/mdAnalysisItemMinMaxValueModal.cshtml", AnalysisItemMinMaxValue);
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
                IEnumerable<AnalysisItemMinMaxValueViewModel> AnalysisItemMinMaxValue = _IDUNIT.analysisItemMinMaxValue.GetAllAnalysisItemMinMaxValue(AnalysisItemId);
                return Json(AnalysisItemMinMaxValue.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }
        public JsonResult GetAllAnalysisItemMinMaxValue(Guid AnalysisItemId)
        {
            try
            {
                IEnumerable<AnalysisItemMinMaxValueViewModel> AnalysisItemMinMaxValue = _IDUNIT.analysisItemMinMaxValue.GetAllAnalysisItemMinMaxValue(AnalysisItemId);
                return Json(AnalysisItemMinMaxValue);
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
                OperationStatus oStatus = _IDUNIT.analysisItemMinMaxValue.RemoveAnalysisItemMinMaxValue(Id);
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

        public OperationStatus RemoveAnalysisItemMinMaxValueWithAnalysisItemId(Guid AnalysisItemId)
        {
            try
            {
                _IDUNIT.analysisItemMinMaxValue.RemoveAllWithAnalysisItemId(AnalysisItemId);
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + ex.Message);
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
        public JsonResult AddOrUpdate(Guid AnalysisItemId, string M_maxValue, string M_minValue, string F_maxValue,
            string F_minValue, string B_maxValue, string B_minValue, string C_maxValue, string C_minValue)
        {
            try
            {
                AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue = new AnalysisItemMinMaxValueViewModel
                {
                    AnalysisItemId = AnalysisItemId,
                    MMaxValue = decimal.Parse(M_maxValue == "" ? "0" : M_maxValue),
                    MMinValue = decimal.Parse(M_minValue == "" ? "0" : M_minValue),
                    FMaxValue = decimal.Parse(F_maxValue == "" ? "0" : F_maxValue),
                    FMinValue = decimal.Parse(F_minValue == "" ? "0" : F_minValue),
                    CMaxValue = decimal.Parse(C_maxValue == "" ? "0" : C_maxValue),
                    CMinValue = decimal.Parse(C_minValue == "" ? "0" : C_minValue),
                    BMaxValue = decimal.Parse(B_maxValue == "" ? "0" : B_maxValue),
                    BMinValue = decimal.Parse(B_minValue == "" ? "0" : B_minValue)
                };
                AnalysisItemMinMaxValue.AnalysisItemId = AnalysisItemId;
                Guid AnalysisItemMinMaxValueId = _IDUNIT.analysisItemMinMaxValue.AddNewAnalysisItemMinMaxValue(AnalysisItemMinMaxValue);
                return Json(AnalysisItemMinMaxValueId);
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