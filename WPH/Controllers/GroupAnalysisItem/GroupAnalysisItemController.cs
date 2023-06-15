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
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.MvcMockingServices;

namespace WPH.Controllers.GroupAnalysisItem
{
    [SessionCheck]
    public class GroupAnalysisItemController : Controller
    {

        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;

        public GroupAnalysisItemController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
        }

        public ActionResult Form()
        {
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.groupAnalysisItem.GetModalsViewBags(ViewBag);
                //_IDUNIT.groupAnalysisItem.DoFormLanguage(ViewBag, HttpContext);
                //_IDUNIT.groupAnalysisItem.DoModalLanguage(ViewBag, HttpContext);
                IEnumerable<GroupAnalysisViewModel> GroupAnalysis = _IDUNIT.groupAnalysis.GetAllGroupAnalysis();
                if (GroupAnalysis.Count() != 0)
                    ViewBag.GroupAnalysisFirstId = GroupAnalysis.FirstOrDefault().Guid;
                //IEnumerable<AnalysisViewModel> Analyses = _analysisMvcService.GetAllAnalysisWithoutInGroupAnalysis_Analysis(clinicSectionId);
                //if (Analyses.Count()!=0)
                //    ViewBag.AnalysisFirstItem = Analyses.FirstOrDefault().Guid;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysisItem/wpGroupAnalysisItem.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ActionResult AddNewModal()
        {
            GroupAnalysisItemViewModel groupAnalysisItem = new GroupAnalysisItemViewModel();
            //_IDUNIT.groupAnalysisItem.DoFormLanguage(ViewBag, HttpContext);
            return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysisItem/mdGroupAnalysisItemModal.cshtml", groupAnalysisItem);
        }
        public ActionResult EditModal(Guid Id)
        {
            try
            {
                GroupAnalysisItemViewModel groupAnalysisItem = _IDUNIT.groupAnalysisItem.GetGroupAnalysisItem(Id);
                //_IDUNIT.groupAnalysisItem.DoFormLanguage(ViewBag, HttpContext);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/GroupAnalysisItem/mdGroupAnalysisItemModal.cshtml", groupAnalysisItem);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request,Guid groupAnalysisId)
        {
            try
            {
                IEnumerable<GroupAnalysisItemViewModel> groupAnalysisItem = _IDUNIT.groupAnalysisItem.GetAllGroupAnalysisItem(groupAnalysisId);
                return Json(groupAnalysisItem.ToDataSourceResult(request));
            }
            catch(Exception e) { throw e; }
            
        }
        public JsonResult GetAllGroupAnalysisItems()
        {
            try
            {
                IEnumerable<GroupAnalysisItemViewModel> AnalysisItems = _IDUNIT.groupAnalysisItem.GetAllGroupAnalysisItem();
                return Json(AnalysisItems);
            }
            catch (Exception ex) { return Json(0); }
        }
        [HttpPost]
        
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.groupAnalysisItem.Remove(Id);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        public JsonResult DeleteAllAnalysisItem(Guid GroupAnalysisId)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.groupAnalysisItem.RemoveGroupAnalysisItemWithGroupAnalysisId(GroupAnalysisId);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddOrUpdate(GroupAnalysisItemViewModel groupAnalysisItem,Guid groupAnalysisId)
        {
            try
            {
                if (groupAnalysisItem.Guid != Guid.Empty)
                {
                    Guid groupAnalysisItemId = _IDUNIT.groupAnalysisItem.UpdateGroupAnalysisItem(groupAnalysisItem);
                    return Json(groupAnalysisItemId);
                }
                else
                {
                    Guid groupAnalysisItemId = _IDUNIT.groupAnalysisItem.AddNewGroupAnalysisItem(groupAnalysisItem, groupAnalysisId);
                    return Json(groupAnalysisItemId);
                }
            }
            catch (Exception ex) { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddAllAnalysisItemToGroup(Guid GroupAnalysisId, Guid AnalysisId)
        {
            try
            {
                List<GroupAnalysisItemViewModel> GroupAnalysisItem = _IDUNIT.groupAnalysisItem.GetAllGroupAnalysisItem(GroupAnalysisId).ToList();

                IEnumerable<AnalysisItemViewModel> AnalysisItem = _IDUNIT.analysisItem.GetAllAnalysisItem(AnalysisId);
                Guid groupAnalysisItemId = new Guid();
                GroupAnalysisItemViewModel groupAnalysisItem = new GroupAnalysisItemViewModel();
                foreach (var item in AnalysisItem)
                {
                    if (!GroupAnalysisItem.Exists(x => x.AnalysisItemId == item.Guid))
                    {
                        Guid AnalysisItemId = item.Guid;
                        groupAnalysisItem.GroupAnalysisId = GroupAnalysisId;
                        groupAnalysisItem.AnalysisItemId = AnalysisItemId;
                        groupAnalysisItemId = _IDUNIT.groupAnalysisItem.AddNewGroupAnalysisItem(groupAnalysisItem, GroupAnalysisId);
                    }
                }
                return Json(groupAnalysisItemId);
            }
            catch (Exception ex) { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddAnalysisItemToGroup(Guid GroupAnalysisId, Guid AnalysisItemId)
        {
            try
            {
                
                    Guid groupAnalysisItemId = new Guid();
                    GroupAnalysisItemViewModel groupAnalysisItem = new GroupAnalysisItemViewModel();
                    groupAnalysisItem.GroupAnalysisId = GroupAnalysisId;
                    groupAnalysisItem.AnalysisItemId = AnalysisItemId;
                    groupAnalysisItemId = _IDUNIT.groupAnalysisItem.AddNewGroupAnalysisItem(groupAnalysisItem, GroupAnalysisId);
                    return Json(groupAnalysisItemId);
                
            }
            catch (Exception ex) { return Json(0); }
        }

        public JsonResult GroupAnalysisItemPriorityEdit(Guid id,Guid groupAnalysisId, string type)
        {
            _IDUNIT.groupAnalysisItem.SwapPriority(id, groupAnalysisId, type);
            return Json(1);
        }

    }
}