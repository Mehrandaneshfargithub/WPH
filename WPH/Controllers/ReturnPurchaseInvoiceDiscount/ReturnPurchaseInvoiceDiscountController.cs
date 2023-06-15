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
using WPH.Models.ReturnPurchaseInvoiceDiscount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnPurchaseInvoiceDiscount
{
    [SessionCheck]
    public class ReturnPurchaseInvoiceDiscountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnPurchaseInvoiceDiscountController> _logger;


        public ReturnPurchaseInvoiceDiscountController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReturnPurchaseInvoiceDiscountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form(Guid returnPurchaseInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDiscount/wpReturnPurchaseInvoiceDiscount.cshtml", returnPurchaseInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddNewModal(Guid returnPurchaseInvoiceId)
        {
            ReturnPurchaseInvoiceDiscountViewModel purchaseInvoiceDiscount = new ReturnPurchaseInvoiceDiscountViewModel
            {
                ReturnPurchaseInvoiceId = returnPurchaseInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDiscount/mdReturnPurchaseInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                ReturnPurchaseInvoiceDiscountViewModel purchaseInvoiceDiscount = _IDUNIT.returnPurchaseInvoiceDiscount.GetReturnPurchaseInvoiceDiscount(Id);


                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnPurchaseInvoiceDiscount/mdReturnPurchaseInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
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
        public JsonResult Remove(Guid returnPurchaseInvoiceDiscountId)
        {
            try
            {
                var oStatus = _IDUNIT.returnPurchaseInvoiceDiscount.RemoveReturnPurchaseInvoiceDiscount(returnPurchaseInvoiceDiscountId);
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
        public JsonResult AddOrUpdate(ReturnPurchaseInvoiceDiscountViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.returnPurchaseInvoiceDiscount.UpdateReturnPurchaseInvoiceDiscount(viewModel));
                }
                else
                {
                    viewModel.CreateUserId = userId;
                    viewModel.CreateDate = DateTime.Now;

                    return Json(_IDUNIT.returnPurchaseInvoiceDiscount.AddNewReturnPurchaseInvoiceDiscount(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid returnPurchaseInvoiceId)
        {
            try
            {
                IEnumerable<ReturnPurchaseInvoiceDiscountViewModel> purchaseInvoiceDiscounts = _IDUNIT.returnPurchaseInvoiceDiscount.GetAllReturnPurchaseInvoiceDiscounts(returnPurchaseInvoiceId);
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