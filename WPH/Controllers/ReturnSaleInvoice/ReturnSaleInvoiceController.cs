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
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.ReturnSaleInvoice;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnSaleInvoice
{
    [SessionCheck]
    public class ReturnSaleInvoiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnSaleInvoiceController> _logger;

        public ReturnSaleInvoiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReturnSaleInvoiceController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnSaleInvoice");
                ViewBag.AccessNewReturnSaleInvoice = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditReturnSaleInvoice = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteReturnSaleInvoice = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.returnSaleInvoice.GetModalsViewBags(ViewBag);

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel
                {
                    periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer),
                    filters = _IDUNIT.baseInfo.GetAllReturnSaleFilter(_localizer)
                };

                return View("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoice/wpReturnSaleInvoice.cshtml", baseInfosAndPeriods);
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
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnSaleInvoiceDetails", "ReturnSaleInvoice", "CanUseReturnSaleInvoiceDate");
            ViewBag.AccessNewReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnSaleInvoiceDetails");
            ViewBag.AccessEditReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnSaleInvoiceDetails");
            ViewBag.AccessDeleteReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "ReturnSaleInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnSaleInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseReturnSaleInvoiceDate");

            ReturnSaleInvoiceViewModel des = new ReturnSaleInvoiceViewModel
            {
                CanChange = true
            };
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoice/mdReturnSaleInvoiceNewModal.cshtml", des);
        }

        [HttpPost]
        public JsonResult AddOrUpdate(ReturnSaleInvoiceViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                viewModel.ClinicSectionId = clinicSectionId;

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "ReturnSaleInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModifiedUserId = userId;

                    return Json(_IDUNIT.returnSaleInvoice.UpdateReturnSaleInvoice(viewModel));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ReturnSaleInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.CreatedUserId = userId;

                    return Json(_IDUNIT.returnSaleInvoice.AddNewReturnSaleInvoice(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, ReturnSaleInvoiceFilterViewModel filterViewModel)
        {
            try
            {
                string[] from = filterViewModel.TxtDateFrom.Split('-');
                string[] to = filterViewModel.TxtDateTo.Split('-');

                filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ReturnSaleInvoiceViewModel> AllReturnSaleInvoice = _IDUNIT.returnSaleInvoice.GetAllReturnSaleInvoices(clinicSectionId, filterViewModel);
                return Json(AllReturnSaleInvoice.ToDataSourceResult(request));
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
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("ReturnSaleInvoiceDetails", "ReturnSaleInvoice", "CanUseReturnSaleInvoiceDate");
            ViewBag.AccessNewReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "ReturnSaleInvoiceDetails");
            ViewBag.AccessEditReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnSaleInvoiceDetails");
            ViewBag.AccessDeleteReturnSaleInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "ReturnSaleInvoiceDetails");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnSaleInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseReturnSaleInvoiceDate");
                ViewBag.BackToAccount = goBack;
                ViewBag.ReturnSaleReceived = _IDUNIT.returnSaleInvoice.CheckReturnSaleInvoiceReceived(Id);
                ReturnSaleInvoiceViewModel hos = _IDUNIT.returnSaleInvoice.GetReturnSaleInvoice(Id);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoice/mdReturnSaleInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "ReturnSaleInvoice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");

                var oStatus = _IDUNIT.returnSaleInvoice.RemoveReturnSaleInvoice(Id, userId, password);
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
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoice/dgReturnSaleInvoiceTotalPriceGrid.cshtml", Id);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAllTotalPrice([DataSourceRequest] DataSourceRequest request, Guid returnSaleInvoiceId)
        {
            try
            {
                var total = _IDUNIT.returnSaleInvoice.GetAllTotalPrice(returnSaleInvoiceId);
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


        public ActionResult GetReceiveReturnSaleInvoice(Guid? receiveId)
        {
            try
            {
                var result = _IDUNIT.returnSaleInvoice.GetReceiveReturnSaleInvoice(receiveId);
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

        public ActionResult GetNotReceiveReturnSaleInvoice(Guid? customerId)
        {
            try
            {
                var result = _IDUNIT.returnSaleInvoice.GetNotReceiveReturnSaleInvoice(customerId);
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



        //public ActionResult GetPartialReceiveReturnSaleInvoice(Guid? receiveId)
        //{
        //    try
        //    {
        //        var result = _IDUNIT.returnSaleInvoice.GetPartialReceiveReturnSaleInvoice(receiveId);
        //        return Json(result);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                               "\t Message: " + e.Message);
        //        return Json(0);
        //    }
        //}

        //public ActionResult GetNotPartialReceiveReturnSaleInvoice(Guid? supplierId, int? currencyId, Guid? receiveId)
        //{
        //    try
        //    {
        //        var result = _IDUNIT.returnSaleInvoice.GetNotPartialReceiveReturnSaleInvoice(supplierId, currencyId, receiveId);
        //        return Json(result);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                               "\t Message: " + e.Message);
        //        return Json(0);
        //    }
        //}
    }
}
