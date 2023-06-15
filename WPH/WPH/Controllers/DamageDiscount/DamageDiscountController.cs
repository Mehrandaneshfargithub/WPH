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
using WPH.Models.DamageDiscount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.DamageDiscount
{
    [SessionCheck]
    public class DamageDiscountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DamageDiscountController> _logger;


        public DamageDiscountController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<DamageDiscountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form(Guid damageId)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDiscount/wpDamageDiscount.cshtml", damageId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddNewModal(Guid damageId)
        {
            DamageDiscountViewModel damageDiscount = new DamageDiscountViewModel
            {
                DamageId = damageId
            };

            return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDiscount/mdDamageDiscountModal.cshtml", damageDiscount);
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                DamageDiscountViewModel damageDiscount = _IDUNIT.damageDiscount.GetDamageDiscount(Id);


                return PartialView("/Views/Shared/PartialViews/AppWebForms/DamageDiscount/mdDamageDiscountModal.cshtml", damageDiscount);
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
        public JsonResult Remove(Guid damageDiscountId)
        {
            try
            {
                var oStatus = _IDUNIT.damageDiscount.RemoveDamageDiscount(damageDiscountId);
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
        public JsonResult AddOrUpdate(DamageDiscountViewModel viewModel)
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                if (viewModel.Guid != Guid.Empty)
                {
                    viewModel.ModifiedUserId = userId;

                    return Json(_IDUNIT.damageDiscount.UpdateDamageDiscount(viewModel));
                }
                else
                {
                    viewModel.CreateUserId = userId;

                    return Json(_IDUNIT.damageDiscount.AddNewDamageDiscount(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid damageId)
        {
            try
            {
                IEnumerable<DamageDiscountViewModel> damageDiscounts = _IDUNIT.damageDiscount.GetAllDamageDiscounts(damageId);
                return Json(damageDiscounts.ToDataSourceResult(request));
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