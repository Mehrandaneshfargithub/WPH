using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.LicenceKeyManagement;
using WPH.Helper;
using WPH.MvcMockingServices;

namespace WPH.Areas.Admin.Controllers.LicenceKeyManagement
{
    [Area("Admin")]
    [AdminLoginCheck]
    public class LicenceKeyManagementController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<LicenceKeyManagementController> _logger;

        public LicenceKeyManagementController(IDIUnit dIUnit, ILogger<LicenceKeyManagementController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public IActionResult Form()
        {

            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/LicenceKeyManagement/wpLicenceKeyManagementForm.cshtml");
        }

        public IActionResult AddNewModal()
        {
            LicenceKeyManagementViewModel licenceKey = new LicenceKeyManagementViewModel();


            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/LicenceKeyManagement/mdLicenceKeyManagementNewModal.cshtml", licenceKey);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(LicenceKeyManagementViewModel viewModel)
        {
            try
            {
                return Json(_IDUNIT.licenceKey.AddNewLicenceKey(viewModel.SerialKey));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public IActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var result = _IDUNIT.licenceKey.GetAll();

            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(int Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.licenceKey.RemoveLicenceKey(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
    }
}
