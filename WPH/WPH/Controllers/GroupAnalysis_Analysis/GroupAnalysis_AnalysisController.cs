using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.MvcMockingServices;

namespace WPH.Controllers.GroupAnalysis_Analysis
{
    public class GroupAnalysis_AnalysisController : Controller
    {

        string userName = string.Empty;
        
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;

        public GroupAnalysis_AnalysisController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
        }

        public ActionResult Form()
        {
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.groupAnalysis_Analysis.GetModalsViewBags(ViewBag);
                //_IDUNIT.groupAnalysis_Analysis.DoFormLanguage(ViewBag, HttpContext);
                //_IDUNIT.groupAnalysis_Analysis.DoModalLanguage(ViewBag, HttpContext);
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis_Analysis/wpGroupAnalysis_Analysis.cshtml", baseInfosAndPeriods);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult AddNewModal()
        {
            GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis = new GroupAnalysis_AnalysisViewModel();
            //_IDUNIT.groupAnalysis_Analysis.DoFormLanguage(ViewBag, HttpContext);
            return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis_Analysis/mdGroupAnalysis_AnalysisModal.cshtml", groupAnalysis_Analysis);
        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis = _IDUNIT.groupAnalysis_Analysis.GetGroupAnalysis_Analysis(Id);
                //_IDUNIT.groupAnalysis_Analysis.DoFormLanguage(ViewBag, HttpContext);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysis_Analysis/mdGroupAnalysis_AnalysisModal.cshtml", groupAnalysis_Analysis);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request,Guid groupAnalysisId)
        {
            IEnumerable<GroupAnalysis_AnalysisViewModel> groupAnalysis_Analysis = _IDUNIT.groupAnalysis_Analysis.GetAllGroupAnalysis_Analysis( groupAnalysisId);
            return Json(groupAnalysis_Analysis.ToDataSourceResult(request));
        }
        public JsonResult GetAllGroupAnalysis_Analysiss()
        {
            try
            {
                IEnumerable<GroupAnalysis_AnalysisViewModel> Analysis_Analysises = _IDUNIT.groupAnalysis_Analysis.GetAllGroupAnalysis_Analysis();
                return Json(Analysis_Analysises);
            }
            catch (Exception ex) { return Json(0); }
        }
        [HttpPost]
        
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.groupAnalysis_Analysis.Remove(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        public JsonResult DeleteAllAnalysis_Analysis(Guid GroupAnalysisId)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.groupAnalysis_Analysis.RemoveGroupAnalysis_AnalysisWithGroupAnalysisId(GroupAnalysisId);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddOrUpdate(GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis,Guid groupAnalysisId)
        {
            try
            {
                if (groupAnalysis_Analysis.Guid != Guid.Empty)
                {
                    Guid groupAnalysis_AnalysisId = _IDUNIT.groupAnalysis_Analysis.UpdateGroupAnalysis_Analysis(groupAnalysis_Analysis);
                    return Json(groupAnalysis_AnalysisId);
                }
                else
                {
                    Guid groupAnalysis_AnalysisId = _IDUNIT.groupAnalysis_Analysis.AddNewGroupAnalysis_Analysis(groupAnalysis_Analysis, groupAnalysisId);
                    return Json(groupAnalysis_AnalysisId);
                }
            }
            catch (Exception ex) { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddAllAnalysisItemToGroup(Guid GroupAnalysisId, Guid AnalysisId)
        {
            try
            {
                List<GroupAnalysis_AnalysisViewModel> GroupAnalysis_Analysis = _IDUNIT.groupAnalysis_Analysis.GetAllGroupAnalysis_Analysis(GroupAnalysisId).ToList();

                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItem(AnalysisId);
                Guid groupAnalysis_AnalysisId = new Guid();
                GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis = new GroupAnalysis_AnalysisViewModel();
                foreach (var item in AnalysisItem)
                {
                    if (!GroupAnalysis_Analysis.Exists(x => x.AnalysisId == item.Guid))
                    {
                        Guid AnalysisItemId = item.Guid;
                        groupAnalysis_Analysis.GroupAnalysisId = GroupAnalysisId;
                        groupAnalysis_Analysis.AnalysisId = AnalysisItemId;
                        groupAnalysis_AnalysisId = _IDUNIT.groupAnalysis_Analysis.AddNewGroupAnalysis_Analysis(groupAnalysis_Analysis, GroupAnalysisId);
                    }
                }
                return Json(groupAnalysis_AnalysisId);
            }
            catch (Exception ex) { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddAnalysisToGroup(Guid GroupAnalysisId, Guid AnalysisId)
        {
            try
            {
                
                    Guid groupAnalysis_AnalysisId = new Guid();
                    GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis = new GroupAnalysis_AnalysisViewModel();
                    groupAnalysis_Analysis.GroupAnalysisId = GroupAnalysisId;
                    groupAnalysis_Analysis.AnalysisId = AnalysisId;
                    groupAnalysis_AnalysisId = _IDUNIT.groupAnalysis_Analysis.AddNewGroupAnalysis_Analysis(groupAnalysis_Analysis, GroupAnalysisId);
                    return Json(groupAnalysis_AnalysisId);
                
            }
            catch (Exception ex) { return Json(0); }
        }

        public JsonResult GroupAnalysis_AnalysisPriorityEdit(Guid id,Guid groupAnalysisId, string type)
        {
            _IDUNIT.groupAnalysis_Analysis.SwapPriority(id, groupAnalysisId, type);
            return Json(1);
        }

    }
}