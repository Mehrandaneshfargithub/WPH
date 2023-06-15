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
using WPH.Models.ReturnSaleInvoiceDiscount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReturnSaleInvoiceDiscount
{
    [SessionCheck]
    public class ReturnSaleInvoiceDiscountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReturnSaleInvoiceDiscountController> _logger;


        public ReturnSaleInvoiceDiscountController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReturnSaleInvoiceDiscountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form(Guid returnSaleInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDiscount/wpReturnSaleInvoiceDiscount.cshtml", returnSaleInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddNewModal(Guid returnSaleInvoiceId)
        {
            ReturnSaleInvoiceDiscountViewModel purchaseInvoiceDiscount = new ReturnSaleInvoiceDiscountViewModel
            {
                ReturnSaleInvoiceId = returnSaleInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDiscount/mdReturnSaleInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                ReturnSaleInvoiceDiscountViewModel purchaseInvoiceDiscount = _IDUNIT.returnSaleInvoiceDiscount.GetReturnSaleInvoiceDiscount(Id);


                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReturnSaleInvoiceDiscount/mdReturnSaleInvoiceDiscountModal.cshtml", purchaseInvoiceDiscount);
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
        public JsonResult Remove(Guid returnSaleInvoiceDiscountId)
        {
            try
            {
                var oStatus = _IDUNIT.returnSaleInvoiceDiscount.RemoveReturnSaleInvoiceDiscount(returnSaleInvoiceDiscountId);
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
        public JsonResult AddOrUpdate(ReturnSaleInvoiceDiscountViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.returnSaleInvoiceDiscount.UpdateReturnSaleInvoiceDiscount(viewModel));
                }
                else
                {
                    viewModel.CreateUserId = userId;
                    viewModel.CreateDate = DateTime.Now;

                    return Json(_IDUNIT.returnSaleInvoiceDiscount.AddNewReturnSaleInvoiceDiscount(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid returnSaleInvoiceId)
        {
            try
            {
                IEnumerable<ReturnSaleInvoiceDiscountViewModel> purchaseInvoiceDiscounts = _IDUNIT.returnSaleInvoiceDiscount.GetAllReturnSaleInvoiceDiscounts(returnSaleInvoiceId);
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