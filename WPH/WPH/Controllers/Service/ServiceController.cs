using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Mvc;
using WPH.Helper;
using WPH.Models.Service;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Service
{
    [SessionCheck]
    public class ServiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ServiceController> _logger;


        public ServiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ServiceController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Service");
                ViewBag.AccessNewService = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditService = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteService = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.service.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Service/weService.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("ERROR_SomeThingWentWrong");
            }

        }

        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Service");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ServiceViewModel service = new ServiceViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Service/mdServiceNewModal.cshtml", service);
        }

        public JsonResult AddOrUpdate(ServiceViewModel Service)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Service.ClinicSectionId = clinicSectionId;
            try
            {
                if (Service.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Service");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    string result = _IDUNIT.service.UpdateService(Service);
                    return Json(result);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Service");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    string result = _IDUNIT.service.AddNewService(Service);
                    return Json(result);
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

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Service");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ServiceViewModel service = _IDUNIT.service.GetService(Id);
                service.NameHolder = service.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Service/mdServiceNewModal.cshtml", service);
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
        public JsonResult Remove(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Service");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                OperationStatus oStatus = _IDUNIT.service.RemoveService(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("ERROR_SomeThingWentWrong");
            }
        }

        //public JsonResult GetAllServices()
        //{
        //    try
        //    {
        //        Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

        //        IEnumerable<ServiceViewModel> AllServices = _IDUNIT.service.GetAllService(clinicSectionId);
        //        return Json(AllServices);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                               "\t Message: " + e.Message);
        //        return Json(0);
        //    }
        //}

        public JsonResult GetAllServicesExceptOperation()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ServiceViewModel> AllServices = _IDUNIT.service.GetAllServicesExceptOperation(clinicSectionId);

                return Json(AllServices);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        public JsonResult GetAllOperationServices()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ServiceViewModel> AllServices = _IDUNIT.service.GetAllSpeceficServices("Operation", clinicSectionId);

                return Json(AllServices);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        public JsonResult GetAllSpeceficServices(string ServiceName)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ServiceViewModel> AllServices = _IDUNIT.service.GetAllSpeceficServices(ServiceName, clinicSectionId);

                return Json(AllServices);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ServiceViewModel> AllService = _IDUNIT.service.GetAllService(clinicSectionId);
                return Json(AllService.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult SingleNewModal(string typeName)
        {
            try
            {
                ServiceViewModel service = _IDUNIT.service.GetServiceType(typeName);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Service/mdSingleServiceNewModal.cshtml", service);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult CustomNewModal()
        {
            try
            {
                ServiceViewModel service = new ServiceViewModel();
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Service/mdCustomServiceNewModal.cshtml", service);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        private StiReport ServiceReport()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "ServiceReport.mrt");
            report.Load(path);

            List<ServiceReportViewModel> result = _IDUNIT.service.GetAllOperationsForReport(clinicSectionId);

            report.Dictionary.Variables["ServiceName"].Value = _localizer["ServiceName"];
            report.Dictionary.Variables["TypeName"].Value = _localizer["ServiceType"];
            report.Dictionary.Variables["Price"].Value = _localizer["ServicePrice"];

            var image_path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportHeaderBanner");
            string rootPath = _hostingEnvironment.WebRootPath;

            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + image_path));
                report.Dictionary.Variables["banner"].ValueObject = (Image)banner;
            }
            catch { }

            report.RegBusinessObject("ServiceReport", result);

            return report;
        }

        public ActionResult PrintServiceReport()
        {
            try
            {
                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = ServiceReport();
                report.Render();

                List<byte[]> allb = new List<byte[]>();

                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        //CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });
                    allb.Add(stream.ToArray());
                }

                return Json(new { allb });
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Service/GetReport")]
        [Route("GetReport/Service")]
        public IActionResult GetReport()
        {
            var report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "TestServiceReport.mrt");
            report.Load(path);

            return StiNetCoreDesigner.GetReportResult(this, report);
        }

        [Route("Service/DesignerEvent")]
        [Route("DesignerEvent/Service")]
        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

        [Route("Service/SaveReport")]
        [Route("SaveReport/Service")]
        public IActionResult SaveReport()
        {
            var report = StiNetCoreDesigner.GetReportObject(this);

            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "ServiceReport.mrt");
            report.Save(path);
            // Save the report template, for example to JSON string

            return StiNetCoreDesigner.SaveReportResult(this);
        }
    }
}
