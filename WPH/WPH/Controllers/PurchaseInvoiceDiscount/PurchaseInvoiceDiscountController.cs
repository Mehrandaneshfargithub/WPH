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
using WPH.Models.PurchaseInvoiceDiscount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PurchaseInvoiceDiscount
{
    [SessionCheck]
    public class PurchaseInvoiceDiscountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PurchaseInvoiceDiscountController> _logger;


        public PurchaseInvoiceDiscountController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PurchaseInvoiceDiscountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form(Guid purchaseInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDiscount/wpPurchaseInvoiceDiscount.cshtml", purchaseInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddNewModal(Guid purchaseInvoiceId)
        {
            PurchaseInvoiceDiscountViewModel purchaseInvoiceDiscount = new PurchaseInvoiceDiscountViewModel
            {
                PurchaseInvoiceId = purchaseInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDiscount/mdPurchaseInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                PurchaseInvoiceDiscountViewModel purchaseInvoiceDiscount = _IDUNIT.purchaseInvoiceDiscount.GetPurchaseInvoiceDiscount(Id);


                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDiscount/mdPurchaseInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
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
        public JsonResult Remove(Guid purchaseInvoiceDiscountId)
        {
            try
            {
                var oStatus = _IDUNIT.purchaseInvoiceDiscount.RemovePurchaseInvoiceDiscount(purchaseInvoiceDiscountId);
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
        public JsonResult AddOrUpdate(PurchaseInvoiceDiscountViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.purchaseInvoiceDiscount.UpdatePurchaseInvoiceDiscount(viewModel));
                }
                else
                {
                    viewModel.CreateUserId = userId;
                    viewModel.CreateDate = DateTime.Now;

                    return Json(_IDUNIT.purchaseInvoiceDiscount.AddNewPurchaseInvoiceDiscount(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid purchaseInvoiceId)
        {
            try
            {
                IEnumerable<PurchaseInvoiceDiscountViewModel> purchaseInvoiceDiscounts = _IDUNIT.purchaseInvoiceDiscount.GetAllPurchaseInvoiceDiscounts(purchaseInvoiceId);
                return Json(purchaseInvoiceDiscounts.ToDataSourceResult(request));
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