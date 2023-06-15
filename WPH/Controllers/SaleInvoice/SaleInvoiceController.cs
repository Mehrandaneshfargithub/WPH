using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.SaleInvoice;
using WPH.Models.SaleInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.SaleInvoice
{
    [SessionCheck]
    public class SaleInvoiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<SaleInvoiceController> _logger;

        public SaleInvoiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<SaleInvoiceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("SaleInvoice");
            ViewBag.AccessNewSaleInvoice = access.Any(p => p.AccessName == "New");
            ViewBag.AccessEditSaleInvoice = access.Any(p => p.AccessName == "Edit");
            ViewBag.AccessDeleteSaleInvoice = access.Any(p => p.AccessName == "Delete");

            string userName = "";
            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            _IDUNIT.saleInvoice.GetModalsViewBags(ViewBag);

            ViewBag.FromToId = (int)Periods.FromDateToDate;
            BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel
            {
                periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer),
                filters = _IDUNIT.baseInfo.GetAllSaleFilter(_localizer)
            };

            return View("/Views/Shared/PartialViews/AppWebForms/SaleInvoice/wpSaleInvoice.cshtml", baseInfosAndPeriods);
        }

        public ActionResult AddAndNewModal()
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("SaleInvoiceDetails", "SaleInvoice");

            ViewBag.AccessNewSaleInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessEditSaleInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessDeleteSaleInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SaleInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "SaleInvoice");

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "StockSelection", "NearestExpire", "ThisExpireStock", "LatestSellingPrice", "RapidSelling");

            ViewBag.stockSelection = _IDUNIT.baseInfo.GetBaseInfoGeneral(Convert.ToInt32(sval?.FirstOrDefault(p => p.ShowSName == "StockSelection")?.SValue))?.Name ?? "Stock";
            ViewBag.NearestExpire = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "NearestExpire")?.SValue);
            ViewBag.ThisExpireStock = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "ThisExpireStock")?.SValue);
            ViewBag.LatestSellingPrice = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "LatestSellingPrice")?.SValue);
            ViewBag.RapidSelling = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "RapidSelling")?.SValue);

            //bool stockSelection = sval?.FirstOrDefault(p => p.ShowSName == "StockSelection")?.SValue;

            //ViewBag.stockSelection = stockSelection;

            //var access = _IDUNIT.subSystem.CheckUserAccess("New", "SaleInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            SaleInvoiceViewModel des = new SaleInvoiceViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoice/mdSaleInvoiceNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate(SaleInvoiceViewModel SaleInvoice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                SaleInvoice.ClinicSectionId = clinicSectionId;

                if (SaleInvoice.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SaleInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    SaleInvoice.ModifiedUserId = userId;

                    return Json(_IDUNIT.saleInvoice.UpdateSaleInvoice(SaleInvoice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "SaleInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    SaleInvoice.CreatedUserId = userId;

                    return Json(_IDUNIT.saleInvoice.AddNewSaleInvoice(SaleInvoice));
                }
            }
            catch (Exception e) { return Json(0); }
        }




        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, SaleInvoiceFilterViewModel filterViewModel)
        {

            string[] from = filterViewModel.TxtDateFrom.Split('-');
            string[] to = filterViewModel.TxtDateTo.Split('-');

            filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
            filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            IEnumerable<SaleInvoiceViewModel> AllSaleInvoice = _IDUNIT.saleInvoice.GetAllSaleInvoices(clinicSectionId, filterViewModel, _localizer);
            return Json(AllSaleInvoice.ToDataSourceResult(request));
        }

        public ActionResult EditModalByReceiveId(Guid receiveId)
        {

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("SaleInvoiceDetails", "SaleInvoice");

            ViewBag.AccessNewSaleInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessEditSaleInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessDeleteSaleInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SaleInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoice");

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "StockSelection");

            ViewBag.stockSelection = _IDUNIT.baseInfo.GetBaseInfoGeneral(Convert.ToInt32(sval?.FirstOrDefault(p => p.ShowSName == "StockSelection")?.SValue))?.Name ?? "Stock";


            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                SaleInvoiceViewModel hos = _IDUNIT.saleInvoice.GetSaleInvoiceByReceiveId(receiveId);

                ViewBag.Lock = _IDUNIT.saleInvoice.CheckSaleInvoiceRecieve(hos.Guid);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoice/mdSaleInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public ActionResult EditModal(Guid Id)
        {

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("SaleInvoiceDetails", "SaleInvoice");

            ViewBag.AccessNewSaleInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessEditSaleInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoiceDetails");
            ViewBag.AccessDeleteSaleInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "SaleInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoice");

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "StockSelection", "NearestExpire", "ThisExpireStock", "LatestSellingPrice", "RapidSelling");

            ViewBag.stockSelection = _IDUNIT.baseInfo.GetBaseInfoGeneral(Convert.ToInt32(sval?.FirstOrDefault(p => p.ShowSName == "StockSelection")?.SValue))?.Name ?? "Stock";
            ViewBag.NearestExpire = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "NearestExpire")?.SValue);
            ViewBag.ThisExpireStock = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "ThisExpireStock")?.SValue);
            ViewBag.LatestSellingPrice = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "LatestSellingPrice")?.SValue);
            ViewBag.RapidSelling = Convert.ToBoolean(sval?.FirstOrDefault(p => p.ShowSName == "RapidSelling")?.SValue);


            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                SaleInvoiceViewModel hos = _IDUNIT.saleInvoice.GetSaleInvoice(Id);

                ViewBag.Lock = _IDUNIT.saleInvoice.CheckSaleInvoiceRecieve(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoice/mdSaleInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "SaleInvoice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");
                string oStatus = _IDUNIT.saleInvoice.RemoveSaleInvoice(Id, userId, password);
                return Json(oStatus);
            }
            catch { return Json(0); }
        }

        public JsonResult GetAllSaleInvoices()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<SaleInvoiceViewModel> saleInvoices = _IDUNIT.saleInvoice.GetAllSaleInvoices(clinicSectionId, null, _localizer);
                return Json(saleInvoices);
            }
            catch (Exception) { return Json(0); }
        }

        public ActionResult GetReceiveSaleInvoice(Guid? ReceiveId)
        {
            try
            {
                var result = _IDUNIT.saleInvoice.GetReceiveSaleInvoice(ReceiveId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetNotReceiveSaleInvoice(Guid? customerId)
        {
            try
            {
                var result = _IDUNIT.saleInvoice.GetNotReceiveSaleInvoice(customerId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public ActionResult GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetPartialPayPurchaseInvoice(payId, currencyId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetNotPartialPayPurchaseInvoice(supplierId, currencyId, payId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetTotalPrice(Guid SaleInvoiceId)
        {
            try
            {
                string SaleInvoicegu = _IDUNIT.saleInvoice.GetTotalPrice(SaleInvoiceId);

                return Json(Convert.ToString(SaleInvoicegu, CultureInfo.InvariantCulture));
            }
            catch { return null; }
        }


        public JsonResult GetAllIncomes()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var SaleInvoicegu = _IDUNIT.saleInvoice.GetAllIncomes(clinicSectionId);

                return Json(SaleInvoicegu);
            }
            catch { return null; }
        }

    }
}
