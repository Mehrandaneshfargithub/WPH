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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.Service;
using WPH.Models.Surgery;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Surgery
{
    [SessionCheck]
    public class SurgeryController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<SurgeryController> _logger;


        public SurgeryController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<SurgeryController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<ActionResult> Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("AllSurgeries");
                ViewBag.AccessEditSurgery = access.Any(p => p.AccessName == "Edit" );
                ViewBag.AccessDeleteSurgery = access.Any(p => p.AccessName == "Delete" );

                var _access = access.Any(p => p.AccessName == "View" );
                if (!_access)
                    return Json("");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.room.GetModalsViewBags(ViewBag);
                ViewBag.Emergency = false;
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                try
                {
                    ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                }
                catch { ViewBag.useDollar = "false"; }

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();

                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                IEnumerable<ClinicSectionViewModel> clisections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "NotLab", clinicSectionId);
                
                baseInfosAndPeriods.periods = periods;
                baseInfosAndPeriods.sections = clisections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                return View("/Views/Shared/PartialViews/AppWebForms/Surgery/wpSurgeryForm.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, Guid? doctorId, Guid? operationId)
        {
            try
            {

                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                IEnumerable<SurgeryGridViewModel> AllPatientReception = _IDUNIT.surgery.GetAllSurgeryByClinicSectionId(section.Id, periodId, fromDate, toDate, doctorId, operationId);
                return Json(AllPatientReception.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json("");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSurgery(SurgeryViewModel Surgery)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AllSurgeries");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Surgery.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(_IDUNIT.surgery.UpdateSurgery(Surgery, clinicSectionId));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return Json(0);
            }

        }


        public async Task<ActionResult> Edit(Guid Id)
        {
            try
            {

                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AllSurgeries");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string userName = "";
                SurgeryViewModel Surgery = _IDUNIT.surgery.GetSurgery(Id);
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.room.GetModalsViewBags(ViewBag);

                DateTime dat = Surgery.Reception.ReceptionDate.GetValueOrDefault();
                Surgery.Reception.ReceptionDateString = dat.Day + "/" + dat.Month + "/" + dat.Year + " " + dat.Hour + ":" + dat.Minute;
                return View("/Views/Shared/PartialViews/AppWebForms/Surgery/wpSurgeryFormDetail.cshtml", Surgery);
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
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "AllSurgeries");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.surgery.RemoveSurgery(Id);
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

        public JsonResult GetReceptionSurgery(Guid ReceptionId)
        {
            try
            {
                SurgeryViewModel Surgery = _IDUNIT.surgery.GetSurgeryByReceptionId(ReceptionId);
                return Json(Surgery);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetReceptionOperation(Guid ReceptionId)
        {
            try
            {
                ServiceViewModel Surgery = _IDUNIT.surgery.GetReceptionOperation(ReceptionId);
                return Json(Surgery);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }



        private async Task<StiReport> SurgeryReport(Guid SurgeryId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                StiReport report = new StiReport();

                report.Load(Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "VatanVaslSarfOjurAmaliatReport.mrt"));

                SurgeryViewModel master = _IDUNIT.surgery.GetSurgeryReportForPrint(SurgeryId);


                report.Dictionary.Variables["vPatientName"].Value = master.Reception.Patient.User.Name ?? "";
                report.Dictionary.Variables["vReceptionNum"].Value = master.Reception.ReceptionNum.ToString();

                report.Dictionary.Variables["vSurgerer1"].Value = master.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery1").Doctor.UserName;


                report.Dictionary.Variables["vSurgererSalary2"].Value = master.SurgeryDoctors.SingleOrDefault(x => x.DoctorRole.Name == "Surgery2")?.Doctor.UserName;
                report.Dictionary.Variables["vUserName"].Value = master.ModifiedUser.Name;


                return report;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return new StiReport();
            }

        }


        public async Task<ActionResult> PrintSurgeryReport(Guid SurgeryId)
        {
            try
            {
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = await SurgeryReport(SurgeryId);


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
                return Json("");
            }
        }


    }
}
