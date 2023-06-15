using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using WPH.Helper;
using WPH.Models.Child;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Child
{
    [SessionCheck]
    public class ChildController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ChildController> _logger;

        public ChildController(IStringLocalizer<SharedResource> localizer, IWebHostEnvironment hostingEnvironment, IDIUnit dIUnit, ILogger<ChildController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Children");
                ViewBag.AccessNewChild = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditChild = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteChild = access.Any(p => p.AccessName == "Delete");
                ViewBag.AccessPrintChild = access.Any(p => p.AccessName == "Print");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.child.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Child/wpChild.cshtml");
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " +
                ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(e.Message); }

        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Children");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ChildViewModel des = new ChildViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Child/mdChildNewModal.cshtml", des);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(ChildViewModel Child)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Child.ClinicSectionId = clinicSectionId;
                Child.UserId = userId;

                if (Child.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Children");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.child.CheckRepeatedChildName(clinicSectionId, Child.Name, false, Child.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }


                    return Json(_IDUNIT.child.UpdateChild(Child));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Children");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.child.CheckRepeatedChildName(clinicSectionId, Child.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }


                    return Json(_IDUNIT.child.AddNewChild(Child));
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
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ChildViewModel> AllChild = _IDUNIT.child.GetAllChildren(clinicSectionId);
                return Json(AllChild.ToDataSourceResult(request));
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
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Children");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ChildViewModel hos = _IDUNIT.child.GetChild(Id);
                hos.NameHolder = hos.Name;
                hos.ChildWeight = hos.Weight?.ToString("G", CultureInfo.InvariantCulture) ?? "0";
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Child/mdChildNewModal.cshtml", hos);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return Json(0);
            }

        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Children");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.child.RemoveChild(Id);
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


        public ActionResult ShowReport()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Children");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Child/wpChildReportForm.cshtml");
        }

        private StiReport ChildReport(ChildReportViewModel reportViewModel)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                StiReport report = new StiReport();
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "ChildReport.mrt");
                report.Load(path);

                reportViewModel.FromDate = new DateTime(reportViewModel.FromDate.Year, reportViewModel.FromDate.Month, reportViewModel.FromDate.Day, 0, 0, 0);
                reportViewModel.ToDate = new DateTime(reportViewModel.ToDate.Year, reportViewModel.ToDate.Month, reportViewModel.ToDate.Day, 23, 59, 59);

                reportViewModel.AllClinicSectionGuids = new List<Guid>
            {
                clinicSectionId
            };

                reportViewModel.AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(reportViewModel.AllClinicSectionGuids, UserId));


                ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(clinicSectionId);

                string clinicSectionName = cs.Name;

                ChildReportResultViewModel children = _IDUNIT.child.ChildReport(reportViewModel);


                report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
                report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToShortDateString();
                report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToShortDateString();
                report.Dictionary.Variables["TotalDinar"].Value = children.TotalChildren;
                report.Dictionary.Variables["Total"].Value = _localizer["Total"];

                report.Dictionary.Variables["Name"].Value = _localizer["Name"];
                report.Dictionary.Variables["GenderName"].Value = _localizer["Gender"];
                report.Dictionary.Variables["BirthDate"].Value = _localizer["DateOfBirth"];
                report.Dictionary.Variables["BirthTime"].Value = _localizer["TimeOfBirth"];
                report.Dictionary.Variables["StatusName"].Value = _localizer["ChildStatus"];
                report.Dictionary.Variables["Weight"].Value = _localizer["Weight"];
                report.Dictionary.Variables["VitalActivities"].Value = _localizer["VitalActivities"];
                report.Dictionary.Variables["CongenitalAnomalies"].Value = _localizer["CongenitalAnomalies"];
                report.Dictionary.Variables["OperationOrder"].Value = _localizer["OperationOrder"];
                report.Dictionary.Variables["StatusCount"].Value = _localizer["Number"];


                if (reportViewModel.Detail)
                {
                    report.RegBusinessObject("ChildrenDetail", children.AllChildren);
                }
                else
                {
                    report.RegBusinessObject("Children", children.AllChildren);
                }

                report.RegBusinessObject("ChildrenStatus", children.ChildrenStatus);
                return report;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " +
                    ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); throw e;
            }
            
        }

        public ActionResult PrintChildReport(ChildReportViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Children");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                reportViewModel.FromDate = DateTime.ParseExact(reportViewModel.TxtFromDate, "dd/MM/yyyy", null);
                reportViewModel.ToDate = DateTime.ParseExact(reportViewModel.TxtToDate, "dd/MM/yyyy", null);

                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = ChildReport(reportViewModel);

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

                return Json("0");
            }
        }






        public JsonResult GetAllUnknownChildren()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ChildHospitalPatientViewModel> children = _IDUNIT.child.GetAllUnknownChildren(clinicSectionId);
                return Json(children);
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
        public JsonResult AddToHospitalPatient(ChildHospitalPatientViewModel viewModel)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.UserId = userId;

                return Json(_IDUNIT.child.AddToHospitalPatient(viewModel));
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetAllHospitalPatientChildren([DataSourceRequest] DataSourceRequest request, Guid recptionId)
        {
            try
            {

                IEnumerable<ChildHospitalPatientViewModel> AllChild = _IDUNIT.child.GetAllHospitalPatientChildren(recptionId);
                return Json(AllChild.ToDataSourceResult(request));
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
        public JsonResult RemoveFromHospitalPatient(Guid Id)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(_IDUNIT.child.RemoveFromHospitalPatient(Id, userId));
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
