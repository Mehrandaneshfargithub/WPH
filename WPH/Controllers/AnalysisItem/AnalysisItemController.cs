using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.MvcMockingServices;
using WPH.WorkerServices;

namespace WPH.Controllers.AnalysisItem
{
    [SessionCheck]
    public class AnalysisItemController : Controller
    {

        string userName = string.Empty;

        private readonly IDIUnit _IDUNIT;
        private readonly IMemoryCache _memoryCache;
        private readonly AnalysisItemWorker _worker;
        private readonly ILogger<AnalysisItemController> _logger;


        public AnalysisItemController(IDIUnit dIUnit, IMemoryCache memoryCache, AnalysisItemWorker worker, ILogger<AnalysisItemController> logger)
        {
            _IDUNIT = dIUnit;
            _memoryCache = memoryCache;
            _worker = worker;
            _logger = logger;
        }

        public IActionResult Form()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {

                _IDUNIT.analysisItem.GetModalsViewBags(ViewBag);
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                int sectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("Unit");
                ViewBag.UnitId = baseInfoGuid;
                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);

                baseInfosAndPeriods.sections = clinicsections.Select(p => new SectionViewModel { Id = p.Guid, Name = p.Name }).ToList();

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("AnalysisItem");
                ViewBag.AccessNewAnalysisItem = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditAnalysisItem = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteAnalysisItem = access.Any(p => p.AccessName == "Delete");

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
                Guid baseInfoGuid = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("Unit");
                ViewBag.UnitId = baseInfoGuid;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/wpAnalysisItem.cshtml", baseInfosAndPeriods);
            }
        }
        public async Task<ActionResult> AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "AnalysisItem");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            AnalysisItemViewModel analysisItem = new AnalysisItemViewModel();
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);

            try
            {
                ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues.FirstOrDefault().SValue);
            }
            catch
            {
                ViewBag.CurrencyTypeId = 11;
            }
            try
            {
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();

            }
            catch
            {
                ViewBag.useDollar = "false";
            }

            if (ViewBag.useDollar == "true")
            {
                analysisItem.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();
                foreach (var dec in css)
                {
                    if (dec.SName == "DinarDecimalAmount")
                        analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "DollarDecimalAmount")
                        analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "EuroDecimalAmount")
                        analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "PondDecimalAmount")
                        analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                }
            }


            return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/mdAnalysisItemModal.cshtml", analysisItem);
        }
        public async Task<ActionResult> EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisItem");
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
                AnalysisItemViewModel analysisItem = _IDUNIT.analysisItem.GetAnalysisItem(Id);
                var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
                List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
                try
                {
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                    ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                }
                catch
                {
                    ViewBag.useDollar = "false";
                }

                if (ViewBag.useDollar == "true")
                {
                    analysisItem.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();
                    foreach (var dec in css)
                    {
                        if (dec.SName == "DinarDecimalAmount")
                            analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "DollarDecimalAmount")
                            analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "EuroDecimalAmount")
                            analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "PondDecimalAmount")
                            analysisItem.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    }
                }
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisItem/mdAnalysisItemModal.cshtml", analysisItem);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid ClinicSectionId)
        {
            try
            {
                IEnumerable<AnalysisItemViewModel> analysisItem = _IDUNIT.analysisItem.GetAllAnalysisItemByClinicSectionId(ClinicSectionId);
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

        public JsonResult GetAllAnalysisItemForClinic()
        {
            try
            {
                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItemName();
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


        public JsonResult GetAllAnalysisItemWithoutInAnalysis([DataSourceRequest] DataSourceRequest request, Guid AnalysisId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItemWithoutInAnalysisByUserId(AnalysisId, userId);
                return Json(AnalysisItem.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllAnalysisItemByClinicSectionId(Guid ClinicSectionId)
        {
            try
            {
                List<AnalysisItemJustNameAndGuidViewModel> all = _IDUNIT.analysisItem.GetAllAnalysisItemsWithNameAndGuidOnly(ClinicSectionId, 11);

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

        public JsonResult GetAllAnalysisItemWithoutInGroupAnalysisItem(Guid GroupId)
        {
            try
            {
                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(GroupId);
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
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "AnalysisItem");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

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
                if (analysisItem.BaseInfoGeneralName == "Text")
                {
                    analysisItem.AnalysisItemMinMaxValues = null;
                    analysisItem.AnalysisItemValuesRanges = null;
                }
                else if (analysisItem.BaseInfoGeneralName == "Numerical")
                {
                    analysisItem.AnalysisItemValuesRanges = null;
                }
                else if (analysisItem.BaseInfoGeneralName == "Optional")
                {
                    analysisItem.AnalysisItemMinMaxValues = null;
                    analysisItem.AnalysisItemValuesRanges = new List<AnalysisItemValuesRangeViewModel>();

                    string[] valuesSplit = analysisItem.AnalysisItemValuesRanges_Value.Split(',');
                    for (int i = 0; i < valuesSplit.Length - 1; i++)
                    {
                        string[] CurrentValue = valuesSplit[i].Split('_');
                        string Value = CurrentValue[0];
                        string Default = CurrentValue[1];
                        AnalysisItemValuesRangeViewModel AnalysisItemValuesRange = new();
                        AnalysisItemValuesRange.Guid = Guid.NewGuid();
                        AnalysisItemValuesRange.Default = Default == "1";
                        AnalysisItemValuesRange.Value = Value.Trim();
                        analysisItem.AnalysisItemValuesRanges.Add(AnalysisItemValuesRange);
                    }
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (analysisItem.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisItem");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    analysisItem.ModifiedDate = DateTime.Now;
                    analysisItem.ModifiedUserId = userId;
                    Guid analysisItemId = _IDUNIT.analysisItem.UpdateAnalysisItem(analysisItem);
                    return Json(analysisItemId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "AnalysisItem");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    analysisItem.CreatedDate = DateTime.Now;
                    analysisItem.CreatedUserId = userId;
                    Guid analysisItemId = _IDUNIT.analysisItem.AddNewAnalysisItem(analysisItem);
                    return Json(analysisItemId);
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
                _IDUNIT.analysisItem.SwapPriority(id, type);
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
        public JsonResult UpdateAnalysisItemButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisItemViewModel> allAnalysisItem)
        {
            try
            {

                _IDUNIT.analysisItem.UpdateAnalysisItemButtonAndPriority(clinicSectionId, allAnalysisItem);
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

        public IActionResult CreateChartInResult(Guid id)
        {
            try
            {
                var result = _IDUNIT.analysisItem.CreateChartInResult(id);
                return Json(result);
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