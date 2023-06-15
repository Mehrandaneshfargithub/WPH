using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Cost;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Cost
{
    [SessionCheck]
    public class CostController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<CostController> _logger;


        public CostController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<CostController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public async Task<IActionResult> Form()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                string userName = string.Empty;
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.cost.GetModalsViewBags(ViewBag);
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                baseInfosAndPeriods.periods = periods;
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                ClinicSectionSettingValueViewModel value = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId").FirstOrDefault();
                if (value != null)
                    ViewBag.CurrencyId = Convert.ToInt32(value.Id);

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);

                baseInfosAndPeriods.sections = new();

                baseInfosAndPeriods.sections.Add(new SectionViewModel { Id = clinicSectionId, Name = clinicSectionName });

                baseInfosAndPeriods.sections.AddRange(clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList());

                ViewBag.BaseInfoTypeId = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName("CostType");

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Costs", "BaseInfoSub");
                ViewBag.AccessNewCosts = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Costs");
                ViewBag.AccessEditCosts = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Costs");
                ViewBag.AccessDeleteCosts = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Costs");

                ViewBag.AccessNewCostType = access.Any(p => p.AccessName == "New" && p.SubSystemName == "BaseInfoSub");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/wpCost.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public async Task<ActionResult> AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Costs");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            CostViewModel cost = new();
            cost.CostDate = DateTime.Now;

            try
            {
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
            }
            catch
            {
                ViewBag.useDollar = "false";
            }
            cost.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();

            if (ViewBag.useDollar != "false")
            {
                var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
                List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
                ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues.FirstOrDefault().SValue);
                foreach (var dec in css)
                {
                    if (dec.SName == "DinarDecimalAmount")
                        cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "DollarDecimalAmount")
                        cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "EuroDecimalAmount")
                        cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    else if (dec.SName == "PondDecimalAmount")
                        cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                }
            }


            return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdCostModal.cshtml", cost);
        }

        public async Task<ActionResult> EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Costs");
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
                CostViewModel cost = _IDUNIT.cost.GetWithType(Id);

                try
                {
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                    ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                }
                catch
                {
                    ViewBag.useDollar = "false";
                }

                cost.AllDecimalAmount = new List<ClinicSectionSettingValueViewModel>();

                if (ViewBag.useDollar != "false")
                {
                    var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
                    List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
                    ViewBag.CurrencyTypeId = Convert.ToInt32(css.SingleOrDefault(x => x.SName == "CurrencyTypeId").ClinicSectionSettingValues.FirstOrDefault().SValue);
                    foreach (var dec in css)
                    {
                        if (dec.SName == "DinarDecimalAmount")
                            cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "DollarDecimalAmount")
                            cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "EuroDecimalAmount")
                            cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                        else if (dec.SName == "PondDecimalAmount")
                            cost.AllDecimalAmount.Add(dec.ClinicSectionSettingValues.FirstOrDefault());
                    }
                }

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdCostModal.cshtml", cost);
            }
            catch { return Json(0); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Costs");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.cost.RemoveCost(Id);
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
        public JsonResult AddOrUpdate(CostViewModel cost/*, DateTime costdate*/)
        {
            cost.CostDate = new DateTime(Convert.ToInt32(cost.CostDateYear), Convert.ToInt32(cost.CostDateMonth), Convert.ToInt32(cost.CostDateDay));
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            cost.OriginalClinicSectionId = clinicSectionId;
            try
            {
                if (cost.Price == null)
                    cost.Price = 0;

                Guid userid = Guid.Parse(HttpContext.Session.GetString("UserId"));
                cost.UserId = userid;

                if (cost.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Costs");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.cost.UpdateCost(cost));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Costs");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.cost.AddNewCost(cost));
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


        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid costTypeId, int periodId, string dateFrom, string dateTo, Guid clinicSectionId)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);
                IEnumerable<CostViewModel> costs = _IDUNIT.cost.GetAllCosts(clinicSectionId, costTypeId, periodId, fromDate, toDate);
                return Json(costs.ToDataSourceResult(request));
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public IActionResult PurchasInvoiceCostForm(Guid purchaseInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/wpPurchasInvoiceCost.cshtml", purchaseInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public IActionResult SaleInvoiceCostForm(Guid saleInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/wpSaleInvoiceCost.cshtml", saleInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult PurchasInvoiceCostAddNewModal(Guid purchaseInvoiceId)
        {
            CostViewModel cost = new CostViewModel
            {
                PurchaseInvoiceId = purchaseInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdPurchasInvoiceCostModal.cshtml", cost);
        }

        public ActionResult SaleInvoiceCostAddNewModal(Guid saleInvoiceId)
        {
            CostViewModel cost = new CostViewModel
            {
                SaleInvoiceId = saleInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdSaleInvoiceCostModal.cshtml", cost);
        }

        public ActionResult PurchasInvoiceCostEditModal(Guid Id)
        {
            try
            {
                var cost = _IDUNIT.cost.GetCost(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdPurchasInvoiceCostModal.cshtml", cost);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult SaleInvoiceCostEditModal(Guid Id)
        {
            try
            {
                var cost = _IDUNIT.cost.GetWithType(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cost/mdSaleInvoiceCostModal.cshtml", cost);
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
        public JsonResult PurchasInvoiceCostRemove(Guid costId)
        {
            try
            {

                var oStatus = _IDUNIT.cost.PurchasInvoiceCostRemove(costId);
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
        [ValidateAntiForgeryToken]
        public JsonResult SaleInvoiceCostRemove(Guid costId)
        {
            try
            {

                var oStatus = _IDUNIT.cost.RemoveCost(costId);
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
        [ValidateAntiForgeryToken]
        public JsonResult PurchasInvoiceCostAddOrUpdate(CostViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    return Json(_IDUNIT.cost.UpdatePurchaseInvoiceCost(viewModel));
                }
                else
                {
                    viewModel.ClinicSectionId = clinicSectionId;
                    viewModel.UserId = userId;
                    viewModel.CostDate = DateTime.Now;

                    return Json(_IDUNIT.cost.AddPurchaseInvoiceCost(viewModel));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaleInvoiceCostAddOrUpdate(CostViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    return Json(_IDUNIT.cost.UpdateSaleInvoiceCost(viewModel));
                }
                else
                {
                    viewModel.ClinicSectionId = clinicSectionId;
                    viewModel.UserId = userId;
                    viewModel.CostDate = DateTime.Now;
                    viewModel.InvoiceNum = _localizer["SaleInvoice"] + " - " + _localizer["InvoiceNum"] + ": " + viewModel.InvoiceNum;
                    return Json(_IDUNIT.cost.AddSaleInvoiceCost(viewModel));
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

        public ActionResult GetAllPurchasInvoiceCosts([DataSourceRequest] DataSourceRequest request, Guid purchaseInvoiceId)
        {
            try
            {
                IEnumerable<CostViewModel> costs = _IDUNIT.cost.GetAllPurchasInvoiceCosts(purchaseInvoiceId);
                return Json(costs.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllSaleInvoiceCosts([DataSourceRequest] DataSourceRequest request, Guid saleInvoiceId)
        {
            try
            {
                IEnumerable<CostViewModel> costs = _IDUNIT.cost.GetAllSaleInvoiceCosts(saleInvoiceId);
                return Json(costs.ToDataSourceResult(request));
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