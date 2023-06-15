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
using WPH.Models.SaleInvoiceDiscount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.SaleInvoiceDiscount
{
    [SessionCheck]
    public class SaleInvoiceDiscountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<SaleInvoiceDiscountController> _logger;


        public SaleInvoiceDiscountController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<SaleInvoiceDiscountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form(Guid SaleInvoiceId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDiscount/wpSaleInvoiceDiscount.cshtml", SaleInvoiceId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddNewModal(Guid SaleInvoiceId)
        {
            SaleInvoiceDiscountViewModel SaleInvoiceDiscount = new SaleInvoiceDiscountViewModel
            {
                SaleInvoiceId = SaleInvoiceId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDiscount/mdSaleInvoiceDiscountModal.cshtml", SaleInvoiceDiscount);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                SaleInvoiceDiscountViewModel SaleInvoiceDiscount = _IDUNIT.saleInvoiceDiscount.GetSaleInvoiceDiscount(Id);


                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDiscount/mdSaleInvoiceDiscountModal.cshtml", SaleInvoiceDiscount);
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
        public JsonResult Remove(Guid SaleInvoiceDiscountId)
        {
            try
            {
                var oStatus = _IDUNIT.saleInvoiceDiscount.RemoveSaleInvoiceDiscount(SaleInvoiceDiscountId);
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
        public JsonResult AddOrUpdate(SaleInvoiceDiscountViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.saleInvoiceDiscount.UpdateSaleInvoiceDiscount(viewModel));
                }
                else
                {
                    viewModel.CreateUserId = userId;
                    viewModel.CreateDate = DateTime.Now;
                    viewModel.ModifiedUserId = userId;
                    viewModel.ModifiedDate = DateTime.Now;
                    return Json(_IDUNIT.saleInvoiceDiscount.AddNewSaleInvoiceDiscount(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid SaleInvoiceId)
        {
            try
            {
                IEnumerable<SaleInvoiceDiscountViewModel> SaleInvoiceDiscounts = _IDUNIT.saleInvoiceDiscount.GetAllSaleInvoiceDiscounts(SaleInvoiceId);
                return Json(SaleInvoiceDiscounts.ToDataSourceResult(request));
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetSaleInvoiceDiscount(Guid SaleInvoiceId, int CurrencyId)
        {
            try
            {
                SaleInvoiceDiscountViewModel SaleInvoiceDiscount = _IDUNIT.saleInvoiceDiscount.GetSaleInvoiceDiscountByCurrencyId(SaleInvoiceId, CurrencyId);


                return Json(SaleInvoiceDiscount);
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
