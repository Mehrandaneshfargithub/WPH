using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using WPH.Models.CustomDataModels.CostReport;
using WPH.MvcMockingServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WPH.Helper;

namespace WPH.Controllers.CostReport
{
    [SessionCheck]
    public class CostReportController : Controller
    {

        string userName = string.Empty;
        
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<CostReportController> _logger;


        public CostReportController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<CostReportController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
        }


        public ActionResult Form()
        {
            try
            {
                ViewBag.AccessPrintCostReport = _IDUNIT.subSystem.CheckUserAccess("Print", "CostReport");

                CostReportViewModel costReport = new();
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);

                costReport.FromDate = DateTime.Now;
                costReport.ToDate = DateTime.Now;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/CostReport/wpCostReportForm.cshtml", costReport);
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }


        private StiReport CostReport(DateTime fromDate, DateTime toDate, Guid clinicSectionId, bool Detail)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                StiReport report = new StiReport();
                string path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "CostsReport.mrt");
                report.Load(path);
                DateTime FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
                DateTime ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                List<Guid> AllClinicSectionGuids = new List<Guid>();

                if (clinicSectionId == Guid.Empty)
                {
                    clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                }

                AllClinicSectionGuids.Add(clinicSectionId);

                AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

                if (AllClinicSectionGuids.Count > 1)
                {
                    AllClinicSectionGuids.Remove(clinicSectionId);
                }

                CostReportViewModel AllCosts = _IDUNIT.cost.GetAllCostsByDateRange(AllClinicSectionGuids, null, FromDate, ToDate, Detail);

                string clinicSectionName = _IDUNIT.clinicSection.GetClinicSectionNameById(clinicSectionId);
                report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
                //string now = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["vDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                //string convertedfromDate = fromDate.ToShortDateString();
                report.Dictionary.Variables["vDateFrom"].Value = fromDate.ToShortDateString();

                report.Dictionary.Variables["vDateTo"].Value = toDate.ToShortDateString();

                report.Dictionary.Variables["vSection"].Value = _localizer["Section"];
                report.Dictionary.Variables["vType"].Value = _localizer["Type"];
                report.Dictionary.Variables["vPrice"].Value = _localizer["Price"];
                report.Dictionary.Variables["vExplanation"].Value = _localizer["Explanation"];


                report.Dictionary.Variables["TotalDinar"].Value = _localizer["Total"];
                report.Dictionary.Variables["vTotal"].Value = AllCosts.Total;
                //report.Dictionary.Variables["TotalDollar"].Value = totalDollar.ToString("N2");
                //report.Dictionary.Variables["TotalPond"].Value = totalPond.ToString("N2");
                //report.Dictionary.Variables["TotalEuro"].Value = totalEuro.ToString("N2");


                report.RegBusinessObject("Cost", AllCosts.AllCost);
                report.RegBusinessObject("SectionType", AllCosts.AllClinicSectionTypeCostTotal);
                report.RegBusinessObject("Type", AllCosts.AllTypeTotal);
                report.RegBusinessObject("Section", AllCosts.AllSectionsTotal);
                return report;
            }
            catch (Exception e)
            {

                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public ActionResult PrintCostReport(string fromDate, string toDate, Guid clinicSectionId, bool Detail)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "CostReport");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string[] from = fromDate.Split(':');
                DateTime dateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]));
                string[] to = toDate.Split(':');
                DateTime dateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]));
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = CostReport(dateFrom, dateTo, clinicSectionId, Detail);

                //StiImageExportSettings set = new StiImageExportSettings() { ImageFormat = StiImageFormat.Color, CutEdges = true, ImageResolution = 200, ImageType = StiImageType.Jpeg };
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
                throw e;
            }
        }



    }
}