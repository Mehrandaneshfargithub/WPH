using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.Receive;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Receive
{
    [SessionCheck]
    public class ReceiveController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReceiveController> _logger;

        public ReceiveController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReceiveController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult ReceiveNewModal(Guid? CustomerId, int? currencyId)
        {
            try
            {
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Receive", "CanUseRecevieDate");

                var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Receive");

                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var customer = _IDUNIT.customer.GetCustomerName(CustomerId.Value);
                ReceiveViewModel des = new ReceiveViewModel
                {
                    CustomerId = customer.Guid,
                    CustomerName = customer.Name,
                    ReceiveAmounts = new List<ReceiveAmountViewModel>
                    {
                        new ReceiveAmountViewModel
                        {
                            BaseCurrencyId = currencyId
                        }
                    }
                };

                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseRecevieDate");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveNewModal.cshtml", des);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public ActionResult PartialReceiveNewModal(Guid? CustomerId)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Receive", "CanUseRecevieDate");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Receive");

            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            var Customer = _IDUNIT.customer.GetCustomerName(CustomerId.Value);
            ReceiveViewModel des = new ReceiveViewModel
            {
                CustomerId = Customer.Guid,
                CustomerName = Customer.Name,
                //BaseCurrencyId = currencyId
            };

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseRecevieDate");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdPartialReceiveNewModal.cshtml", des);
        }

        public ActionResult ReceiveInvoiceNewModal(Guid? CustomerId)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Receive", "CanUseRecevieDate");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Receive");

            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            var Customer = _IDUNIT.customer.GetCustomerName(CustomerId.Value);
            ReceiveViewModel des = new ReceiveViewModel
            {
                CustomerId = Customer.Guid,
                CustomerName = Customer.Name,
                //BaseCurrencyId = currencyId
            };

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseRecevieDate");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveInvoiceNewModal.cshtml", des);
        }

        public ActionResult ReceiveEditModal(Guid Id)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Receive", "CanUseRecevieDate");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Receive");

            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ReceiveViewModel hos = _IDUNIT.receive.GetReceive(Id);

                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseRecevieDate");

                if (hos.ReceiveType == "Total")
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveNewModal.cshtml", hos);

                if (hos.ReceiveType == "Partial")
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdPartialReceiveNewModal.cshtml", hos);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        [HttpPost]
        public JsonResult AddOrUpdatePartialReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    return Json(_IDUNIT.receive.UpdatePartialReceive(viewModel, invoiceIds, returnIds));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.receive.AddNewPartialReceive(viewModel, invoiceIds, returnIds));
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
        public JsonResult AddOrUpdateInvoice(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    return Json(_IDUNIT.receive.UpdateInvoiceReceive(viewModel, invoiceIds, returnIds));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.receive.AddNewInvoiceReceive(viewModel, invoiceIds, returnIds));
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
        public JsonResult AddOrUpdate(ReceiveViewModel viewModel)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                viewModel.ClinicSectionId = clinicSectionId;

                if(string.IsNullOrWhiteSpace(viewModel.ReceiveDateTxt))
                {
                    viewModel.ReceiveDateTxt = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.receive.UpdateReceive(viewModel));
                }
                else
                {
                    viewModel.CreatedUserId = userId;
                    viewModel.CreatedDate = DateTime.Now;
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Receive");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.receive.AddNewReceive(viewModel));
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
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Receive");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");

                var oStatus = _IDUNIT.receive.RemoveReceive(Id, userId, password);
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


        public JsonResult RemoveReceiveAmount(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Receive");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                
                var oStatus = _IDUNIT.receive.RemoveReceiveAmount(Id);
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

        public JsonResult GetSaleInvoiceReceives(Guid SaleInvoiceId, int CurrencyId)
        {
            try
            {
                decimal SaleInvoiceReceive = _IDUNIT.receive.GetSaleInvoiceReceives(SaleInvoiceId, CurrencyId);
                return Json(SaleInvoiceReceive);
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
        public JsonResult GetPartialReceiveHistory(IEnumerable<string> receiveIds)
        {
            try
            {
                var result = _IDUNIT.receive.GetPartialReceiveHistory(receiveIds);

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

        public ActionResult ReceiveAmountHistoryModal(int CurrencyId)
        {
            try
            {

                ViewBag.AccessDeleteReceiveAmount = true;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDetail/dgSaleInvoiceDetailReceiveHistoryGrid.cshtml", CurrencyId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllReceiveAmount([DataSourceRequest] DataSourceRequest request, Guid SaleInvoiceId, int CurrencyId)
        {
            try
            {
                IEnumerable<ReceiveAmountViewModel> AllReceiveAmount = _IDUNIT.receive.GetAllReceiveAmount(SaleInvoiceId, CurrencyId);
                return Json(AllReceiveAmount.ToDataSourceResult(request));
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
