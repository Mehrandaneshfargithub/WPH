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
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Analysis_AnalysisItem
{
    public class Analysis_AnalysisItemController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<Analysis_AnalysisItemController> _logger;

        public Analysis_AnalysisItemController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<Analysis_AnalysisItemController> logger)
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
                _IDUNIT.analysisItem.GetModalsViewBags(ViewBag);

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
                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("UnitForAnalysisItem");
                ViewBag.UnitId = baseInfoGuid;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/wpAnalysisItem.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message:" + e.Message);
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisItem.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                try
                {
                    ViewBag.CurrencyId = Convert.ToInt32(_IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault().Id);
                }
                catch
                {
                    ViewBag.CurrencyId = 11;
                }
                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("UnitForAnalysisItem");
                ViewBag.UnitId = baseInfoGuid;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/wpAnalysisItem.cshtml", baseInfosAndPeriods);
            }
        }
        public async Task<IActionResult> AddNewModal()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            AnalysisItemViewModel analysisItem = new AnalysisItemViewModel
            {
                CreatedDate = DateTime.Now
            };

            try
            {
                ViewBag.CurrencyId = Convert.ToInt32(_IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault().Id);
            }
            catch
            {
                ViewBag.CurrencyId = 11;
            }
            return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/mdAnalysisItemModal.cshtml", analysisItem);
        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                AnalysisItemViewModel analysisItem = _IDUNIT.analysisItem.GetAnalysisItem(Id);
                //_analysisItemMvcService.DoFormLanguage(ViewBag, HttpContext);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/mdAnalysisItemModal.cshtml", analysisItem);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid AnalysisId)
        {
            try
            {
                IEnumerable<AnalysisItemViewModel> analysisItem = _IDUNIT.analysisItem.GetAllAnalysisItem(AnalysisId);
                return Json(analysisItem.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }
        public JsonResult GetAllAnalysisItem(Guid AnalysisId)
        {
            try
            {
                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItem(AnalysisId);
                return Json(AnalysisItem);
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
                OperationStatus oStatus = _IDUNIT.analysisItem.RemoveAnalysisItem(Id);
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
        public async Task<JsonResult> AddOrUpdate(AnalysisItemViewModel analysisItem)
        {
            try
            {
                if (analysisItem.Guid != Guid.Empty)
                {
                    Guid userid = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    analysisItem.ModifiedDate = DateTime.Now;
                    analysisItem.ModifiedUserId = userid;
                    Guid analysisItemId = _IDUNIT.analysisItem.UpdateAnalysisItem(analysisItem);
                    _IDUNIT.analysisItemValuesRange.RemoveAllWithAnalysisItemId(analysisItemId);
                    _IDUNIT.analysisItemMinMaxValue.RemoveAllWithAnalysisItemId(analysisItemId);
                    return Json(analysisItemId);
                }
                else
                {
                    Guid userid = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    analysisItem.CreatedDate = DateTime.Now;
                    analysisItem.CreatedUserId = userid;
                    Guid AnalysisId = (Guid)analysisItem.AnalysisId;
                    Guid analysisItemId =  _IDUNIT.analysisItem.AddNewAnalysisItem(analysisItem);
                    return Json(analysisItemId);
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
        public JsonResult AddAnalysis(Guid AnalysisItemId, Guid AnalysisId)
        {
            try
            {
                _IDUNIT.analysisItem.AddAnalysisOfAnalysisItem(AnalysisItemId, AnalysisId);
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
        public JsonResult AnalysisItemPriorityEdit(Guid id, string type)
        {
            try
            {
                _IDUNIT.analysisItem.AnalysisAnalysisItemSwapPriority(id, type);
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