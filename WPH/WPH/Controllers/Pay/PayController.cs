using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.Pay;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Pay
{
    [SessionCheck]
    public class PayController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PayController> _logger;

        public PayController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PayController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult PayNewModal(Guid? supplierId, int? currencyId)
        {
            try
            {
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Pay", "CanUsePayDate");

                var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Pay");

                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var supplier = _IDUNIT.supplier.GetSupplierName(supplierId.Value);
                PayViewModel des = new PayViewModel
                {
                    SupplierId = supplier.Guid,
                    SupplierName = supplier.Name,
                    PayAmounts = new List<PayAmountViewModel>
                    {
                        new PayAmountViewModel
                        {
                            BaseCurrencyId = currencyId
                        }
                    }
                };

                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePayDate");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPayNewModal.cshtml", des);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public ActionResult PartialPayNewModal(Guid? supplierId)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Pay", "CanUsePayDate");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Pay");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            var supplier = _IDUNIT.supplier.GetSupplierName(supplierId.Value);
            PayViewModel des = new PayViewModel
            {
                SupplierId = supplier.Guid,
                SupplierName = supplier.Name,
                //BaseCurrencyId = currencyId
            };

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePayDate");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPartialPayNewModal.cshtml", des);
        }

        public ActionResult PayInvoiceNewModal(Guid? supplierId)
        {

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Pay", "CanUsePayDate");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Pay");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            var supplier = _IDUNIT.supplier.GetSupplierName(supplierId.Value);
            PayViewModel des = new PayViewModel
            {
                SupplierId = supplier.Guid,
                SupplierName = supplier.Name,
                //BaseCurrencyId = currencyId
            };

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePayDate");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPayInvoiceNewModal.cshtml", des);
        }

        public ActionResult PayEditModal(Guid Id)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Pay", "CanUsePayDate");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Pay");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                PayViewModel hos = _IDUNIT.pay.GetPay(Id);

                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePayDate");

                if (hos.PayType == "Total")
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPayNewModal.cshtml", hos);

                if (hos.PayType == "Partial")
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPartialPayNewModal.cshtml", hos);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/mdPayInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddOrUpdatePartialPay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    return Json(_IDUNIT.pay.UpdatePartialPay(viewModel, invoiceIds, returnIds));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.pay.AddNewPartialPay(viewModel, invoiceIds, returnIds));
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
        public JsonResult AddOrUpdateInvoice(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    return Json(_IDUNIT.pay.UpdateInvoicePay(viewModel, invoiceIds, returnIds));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.pay.AddNewInvoicePay(viewModel, invoiceIds, returnIds));
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
        public JsonResult AddOrUpdate(PayViewModel viewModel)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                viewModel.ClinicSectionId = clinicSectionId;

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.pay.UpdatePay(viewModel));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Pay");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.pay.AddNewPay(viewModel));
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

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Pay");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");

                var oStatus = _IDUNIT.pay.RemovePay(Id, userId, password);
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
        public JsonResult GetPartialPayHistory(IEnumerable<string> payIds)
        {
            try
            {
                var result = _IDUNIT.pay.GetPartialPayHistory(payIds);

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
