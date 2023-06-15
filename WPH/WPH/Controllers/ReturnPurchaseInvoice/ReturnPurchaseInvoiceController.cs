using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.ReturnPurchaseInvoice;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnPurchaseInvoice
{
    [SessionCheck]
    public class ReturnPurchaseInvoiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnPurchaseInvoiceController> _logger;

        public ReturnPurchaseInvoiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReturnPurchaseInvoiceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnPurchaseInvoice");
                ViewBag.AccessNewReturnPurchaseInvoice = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditReturnPurchaseInvoice = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteReturnPurchaseInvoice = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.returnPurchaseInvoice.GetModalsViewBags(ViewBag);

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel
                {
                    periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer),
                    filters = _IDUNIT.baseInfo.GetAllReturnPurchaseFilter(_localizer)
                };

                return View("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoice/wpReturnPurchaseInvoice.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult AddAndNewModal()
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnPurchaseInvoiceDetails", "ReturnPurchaseInvoice", "CanUseReturnPurchaseInvoiceDate");
            ViewBag.AccessNewReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");
            ViewBag.AccessEditReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");
            ViewBag.AccessDeleteReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnPurchaseInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }
            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseReturnPurchaseInvoiceDate");


            ReturnPurchaseInvoiceViewModel des = new ReturnPurchaseInvoiceViewModel
            {
                CanChange = true
            };
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoice/mdReturnPurchaseInvoiceNewModal.cshtml", des);
        }

        [HttpPost]
        public JsonResult AddOrUpdate(ReturnPurchaseInvoiceViewModel ReturnPurchaseInvoice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                ReturnPurchaseInvoice.ClinicSectionId = clinicSectionId;

                if (ReturnPurchaseInvoice.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ReturnPurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    ReturnPurchaseInvoice.ModifiedUserId = userId;

                    return Json(_IDUNIT.returnPurchaseInvoice.UpdateReturnPurchaseInvoice(ReturnPurchaseInvoice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnPurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    ReturnPurchaseInvoice.CreatedUserId = userId;

                    return Json(_IDUNIT.returnPurchaseInvoice.AddNewReturnPurchaseInvoice(ReturnPurchaseInvoice));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, ReturnPurchaseInvoiceFilterViewModel filterViewModel)
        {
            try
            {
                string[] from = filterViewModel.TxtDateFrom.Split('-');
                string[] to = filterViewModel.TxtDateTo.Split('-');

                filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ReturnPurchaseInvoiceViewModel> AllReturnPurchaseInvoice = _IDUNIT.returnPurchaseInvoice.GetAllReturnPurchaseInvoices(clinicSectionId, filterViewModel);
                return Json(AllReturnPurchaseInvoice.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult EditModal(Guid Id, bool goBack = false)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnPurchaseInvoiceDetails", "ReturnPurchaseInvoice", "CanUseReturnPurchaseInvoiceDate");
            ViewBag.AccessNewReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");
            ViewBag.AccessEditReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");
            ViewBag.AccessDeleteReturnPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnPurchaseInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseReturnPurchaseInvoiceDate");
                ViewBag.BackToAccount = goBack;
                ViewBag.ReturnPurchasePaid = _IDUNIT.returnPurchaseInvoice.CheckReturnPurchaseInvoicePaid(Id);
                ReturnPurchaseInvoiceViewModel hos = _IDUNIT.returnPurchaseInvoice.GetReturnPurchaseInvoice(Id);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoice/mdReturnPurchaseInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ReturnPurchaseInvoice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");

                var oStatus = _IDUNIT.returnPurchaseInvoice.RemoveReturnPurchaseInvoice(Id, userId, password);
                return Json(oStatus);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult TotalPriceModal(Guid Id)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoice/dgReturnPurchaseInvoiceTotalPriceGrid.cshtml", Id);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAllTotalPrice([DataSourceRequest] DataSourceRequest request, Guid returnPurchaseInvoiceId)
        {
            try
            {
                var total = _IDUNIT.returnPurchaseInvoice.GetAllTotalPrice(returnPurchaseInvoiceId);
                return Json(total.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }


        public ActionResult GetPayReturnPurchaseInvoice(Guid? payId)
        {
            try
            {
                var result = _IDUNIT.returnPurchaseInvoice.GetPayReturnPurchaseInvoice(payId);
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

        public ActionResult GetNotPayReturnPurchaseInvoice(Guid? supplierId)
        {
            try
            {
                var result = _IDUNIT.returnPurchaseInvoice.GetNotPayReturnPurchaseInvoice(supplierId);
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

        public ActionResult GetPartialPayReturnPurchaseInvoice(Guid? payId)
        {
            try
            {
                var result = _IDUNIT.returnPurchaseInvoice.GetPartialPayReturnPurchaseInvoice(payId);
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

        public ActionResult GetNotPartialPayReturnPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            try
            {
                var result = _IDUNIT.returnPurchaseInvoice.GetNotPartialPayReturnPurchaseInvoice(supplierId, currencyId, payId);
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
    }
}
