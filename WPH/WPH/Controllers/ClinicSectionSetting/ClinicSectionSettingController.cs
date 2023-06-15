using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH;
using WPH.Helper;
using WPH.Models.Clinic;
using WPH.Models.CustomDataModels.Clinic;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Clinic
{
    [SessionCheck]
    public class ClinicSectionSettingController : Controller
    {

        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ClinicSectionSettingController> _logger;


        public ClinicSectionSettingController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostEnvironment, ILogger<ClinicSectionSettingController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(ClinicSectionId, sectionTypeId);

            ViewBag.ClinicSection = ClinicSectionId;

            ViewBag.AccessEditLabSetting = _IDUNIT.subSystem.CheckUserAccess("Edit", "LabSetting");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/LabSetting/wpLabSetting.cshtml", css);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveClinicSectionSettings(List<ClinicSectionSettingValueViewModel> cssvmList, List<ClinicSectionSettingBannerViewModel> bannerList)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "LabSetting");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                //var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
                string rootPath = _hostEnvironment.WebRootPath;

                var clinicSectionSettings = cssvmList.Where(p => !string.IsNullOrWhiteSpace(p.SValue) && !string.IsNullOrWhiteSpace(p.SValue))
                    .Select(p =>
                    {
                        p.Guid = Guid.NewGuid();
                        p.ClinicSectionId = clinicSectionId;
                        return p;
                    }).ToList();

                _IDUNIT.clinicSection.SaveClinicSectionSettings(clinicSectionSettings, rootPath, bannerList, clinicSectionId);

                return Json("");
            }
            catch (Exception e) { return Json(""); }
        }


        public async Task<JsonResult> GetSpecificSettingByName(string SettingName)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var svalT = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, SettingName).FirstOrDefault();
            string value;
            try
            {
                value = svalT.SValue ?? "";
            }
            catch { value = ""; }

            return Json(value);
        }


        public JsonResult GetAllDecimalValues([DataSourceRequest] DataSourceRequest request)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(ClinicSectionId, sectionTypeId);
            return Json(css.ToDataSourceResult(request));
        }


        public JsonResult GetClinicSectionName()
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            string clinicSectionName = _IDUNIT.clinicSection.GetClinicSectionById(ClinicSectionId).Name;
            return Json(clinicSectionName);
        }

        public JsonResult UpdateClinicSectionName(string Name)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "LabSetting");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            _IDUNIT.clinicSection.UpdateClinicSectionName(ClinicSectionId, Name);
            return Json(1);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult AddReportHeaderBanner(IFormFile banner)
        //{
        //    Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
        //    string rootPath = _hostEnvironment.WebRootPath;
        //    return Json(_IDUNIT.clinicSection.SaveBanner(clinicSectionId, rootPath, banner, "ReportHeaderBanner"));
        //}
        //public JsonResult GetReportHeaderBanner()
        //{
        //    Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
        //    var result = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportHeaderBanner");
        //    return Json(result);
        //}

        public void UpdateClinicSectionSetting(string SettingName, string SrttingVal)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            _IDUNIT.clinicSection.SaveClinicSectionSettingValue(ClinicSectionId, SrttingVal, SettingName);

        }
    }
}