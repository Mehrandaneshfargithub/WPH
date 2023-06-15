using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.AdminClinicSectionSetting;
using WPH.Helper;
using WPH.MvcMockingServices;

namespace WPH.Areas.Admin.Controllers.AdminClinicSectionSetting
{
    [Area("Admin")]
    [AdminLoginCheck]
    public class AdminClinicSectionSettingController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AdminClinicSectionSettingController> _logger;

        public AdminClinicSectionSettingController(IDIUnit dIUnit, ILogger<AdminClinicSectionSettingController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {

            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/wpAdminClinicSectionSettingForm.cshtml");
        }

        public ActionResult AddNewModal()
        {
            AdminClinicSectionSettingViewModel baseInfo = new AdminClinicSectionSettingViewModel();

            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/mdAdminClinicSectionSettingNewModal.cshtml", baseInfo);
        }


        public ActionResult EditModal(int Id)
        {
            try
            {
                AdminClinicSectionSettingViewModel adminClinicSectionSetting = _IDUNIT.clinicSectionSetting.GetClinicSectionSetting(Id);

                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/mdAdminClinicSectionSettingNewModal.cshtml", adminClinicSectionSetting);
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(AdminClinicSectionSettingViewModel viewModel)
        {
            try
            {
                if (viewModel.Id != 0)
                {

                    return Json(_IDUNIT.clinicSectionSetting.UpdateClinicSectionSetting(viewModel));
                }
                else
                {

                    return Json(_IDUNIT.clinicSectionSetting.AddNewClinicSetionSetting(viewModel));
                }
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<AdminClinicSectionSettingViewModel> allAdminClinicSectionSettings = _IDUNIT.clinicSectionSetting.GetAllClinicSectionSettings();

                return Json(allAdminClinicSectionSettings.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(int Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.clinicSectionSetting.RemoveClinicSetionSetting(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation(e.Message); return Json(0); }
        }
    }
}
