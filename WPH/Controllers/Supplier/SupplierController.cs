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
using WPH.Models.Supplier;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Supplier
{
    [SessionCheck]
    public class SupplierController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<SupplierController> _logger;
        public SupplierController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<SupplierController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Supplier");
                ViewBag.AccessNewSupplier = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditSupplier = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteSupplier = access.Any(p => p.AccessName == "Delete");

                _IDUNIT.supplier.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Supplier/wpSupplier.cshtml");
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
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Supplier");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }


            SupplierViewModel des = new SupplierViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Supplier/mdSupplierNewModal.cshtml", des);
        }
        public JsonResult AddOrUpdate(SupplierViewModel Supplier)
        {
            try
            {
                var clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Supplier.ClinicSectionId = clinicSectionId;

                if (Supplier.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Supplier");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.supplier.UpdateSupplier(Supplier));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Supplier");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    return Json(_IDUNIT.supplier.AddNewSupplier(Supplier));
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
                IEnumerable<SupplierViewModel> AllSupplier = _IDUNIT.supplier.GetAllSuppliers(clinicSectionId);
                return Json(AllSupplier.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllSuppliers()
        {
            try
            {
                var clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<SupplierViewModel> AllSupplier = _IDUNIT.supplier.GetAllSuppliersName(clinicSectionId);
                return Json(AllSupplier);
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
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Supplier");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                SupplierViewModel hos = _IDUNIT.supplier.GetSupplier(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Supplier/mdSupplierNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Supplier/mdSupplierNewModal.cshtml", new SupplierViewModel()); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Supplier");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.supplier.RemoveSupplier(Id);
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

        public ActionResult GetAllSupplierFilter()
        {
            try
            {
                var filters = _IDUNIT.baseInfo.GetAllSupplierFilter(_localizer);
                return Json(filters);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllSupplierYearFilter()
        {
            try
            {
                var clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "YearStartFrom");

                string year = "";
                try { year = sval?.FirstOrDefault()?.SValue; } catch { }

                var filters = _IDUNIT.baseInfo.GetAllTransferYearFilter(_localizer, year);
                return Json(filters);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public IActionResult GetAllSuppliersByClinicSectionId(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<SupplierViewModel> result = _IDUNIT.supplier.GetAllSuppliersByClinicSectionId(clinicSectionId);
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
