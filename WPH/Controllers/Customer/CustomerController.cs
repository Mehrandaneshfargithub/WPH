using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Customer;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Customer
{
    [SessionCheck]
    public class CustomerController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<CustomerController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Customer");
                ViewBag.AccessNewCustomer = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditCustomer = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteCustomer = access.Any(p => p.AccessName == "Delete");

                _IDUNIT.customer.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Customer/wpCustomer.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Customer");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            CustomerViewModel des = new CustomerViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Customer/mdCustomerNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate(CustomerViewModel viewModel)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                if (viewModel.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Customer");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.ModidiedUserId = userId;
                    return Json(_IDUNIT.customer.UpdateCustomer(viewModel));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Customer");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    viewModel.CreateUserId = userId;
                    return Json(_IDUNIT.customer.AddNewCustomer(viewModel));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<CustomerViewModel> AllCustomer = _IDUNIT.customer.GetAllCustomers(clinicSectionId);
                return Json(AllCustomer.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllCustomers()
        {
            try
            {
                var clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<CustomerViewModel> AllCustomer = _IDUNIT.customer.GetAllCustomersName(clinicSectionId);
                return Json(AllCustomer);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Customer");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                CustomerViewModel hos = _IDUNIT.customer.GetCustomer(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Customer/mdCustomerNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Customer/mdCustomerNewModal.cshtml", new CustomerViewModel()); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Customer");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.customer.RemoveCustomer(Id);
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
    }
}
