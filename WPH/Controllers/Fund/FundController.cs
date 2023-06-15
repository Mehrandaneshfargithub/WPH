using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using WPH.MvcMockingServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WPH.Models.Fund;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Helper;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WPH.Controllers.Fund
{
    [SessionCheck]
    public class FundController : Controller
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<FundController> _logger;

        public FundController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<FundController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public ActionResult Form()
        {
            ViewBag.AccessPrintFundReport = _IDUNIT.subSystem.CheckUserAccess("Print", "FundReport");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Fund/wpFundPage.cshtml");
        }

        private StiReport FundReport(DateTime fromDate, DateTime toDate, Guid ClinicSectionId, bool Detail)
        {
            Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            StiReport report = new StiReport();
            string path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "FundReport.mrt");
            report.Load(path);

            List<Guid> AllClinicSectionGuids = new List<Guid>
            {
                ClinicSectionId
            };

            AllClinicSectionGuids.AddRange(_IDUNIT.clinicSection.GetClinicSectionChilds(AllClinicSectionGuids, UserId));

            ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(ClinicSectionId);
            string clinicSectionName = cs.Name;

            FundReportViewModel funds = _IDUNIT.fund.GetAllReceivesForHospital(AllClinicSectionGuids, fromDate, toDate, Detail);

            report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
            report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
            report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
            report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
            report.Dictionary.Variables["vDateFrom"].Value = fromDate.ToString("dd/MM/yyyy HH:mm");
            report.Dictionary.Variables["vDateTo"].Value = toDate.ToString("dd/MM/yyyy HH:mm");
            report.Dictionary.Variables["RecieveDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["RecieveAmount"].Value = _localizer["Amount"];
            report.Dictionary.Variables["Section"].Value = _localizer["Section"];
            report.Dictionary.Variables["RadiologyDoctor"].Value = _localizer["RadiologyDoctor"];
            report.Dictionary.Variables["Total"].Value = _localizer["Total"];
            report.Dictionary.Variables["Number"].Value = _localizer["Number"];
            report.Dictionary.Variables["ReceptionNum"].Value = _localizer["InvoiceNum"];
            report.Dictionary.Variables["TotalDinar"].Value = funds.Total;

            report.Dictionary.Variables["InvoiceDate"].Value = _localizer["InvoiceDate"];
            report.Dictionary.Variables["Supplier"].Value = _localizer["Supplier"];
            report.Dictionary.Variables["Discount"].Value = _localizer["Discount"];
            report.Dictionary.Variables["WholePurchasePrice"].Value = _localizer["WholePurchasePrice"];
            report.Dictionary.Variables["Currency"].Value = _localizer["Currency"];
            report.Dictionary.Variables["Product"].Value = _localizer["Product"];
            report.Dictionary.Variables["PurchasePrice"].Value = _localizer["PurchasePrice"];

            if (Detail)
            {
                report.RegBusinessObject("fundDetail", funds.AllFund.OrderBy(a=>a.Date));
                report.RegBusinessObject("PurchaseFundDetail", funds.PurchaseFundDetail.OrderBy(a=>a.InvoiceDate));
            }
            else
            {
                report.RegBusinessObject("fund", funds.AllFund.OrderBy(a => a.Date));
                report.RegBusinessObject("PurchaseFund", funds.PurchaseFund.OrderBy(a=>a.InvoiceDate));
            }

            report.RegBusinessObject("DoctorFund", funds.DoctorFund);
            report.RegBusinessObject("SectionFund", funds.AllSectionsTotal);
            report.RegBusinessObject("TotalCurrency", funds.TotalCurrency);
            return report;
        }

        public JsonResult GetSourceClinicSections()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => new { p.Guid, p.Name, Parent = false }).ToList();

                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AccessParentFund");
                if (access)
                {
                    clinicSectionAccess.Insert(0, new { Guid = clinicSectionId, Name = clinicSectionName, Parent = true });
                }

                return Json(clinicSectionAccess);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public ActionResult PrintFundReport(string fromDate, string toDate, Guid ClinicSectionId, bool Detail)
        {
            try
            {
                //var access = _IDUNIT.subSystem.CheckUserAccess("Print", "FundReport");
                //if (!access) 
                //{
                //    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                //                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                //                           "\t Message: AccessDenied");
                //    return Json("");
                //}

                string[] from = fromDate.Split(':');
                DateTime dateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), Convert.ToInt32(from[3]), Convert.ToInt32(from[4]), 0);
                string[] to = toDate.Split(':');
                DateTime dateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), Convert.ToInt32(to[3]), Convert.ToInt32(to[4]), 0);
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = FundReport(dateFrom, dateTo, ClinicSectionId, Detail);

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
                    //Image x = Image.FromStream(stream);
                    //height = x.Height;
                    //width = x.Width;
                    allb.Add(stream.ToArray());
                }
                //MemoryStream stream = new MemoryStream();
                //StiImageExportService service = new StiImageExportService();
                //PrinterSettings r = new PrinterSettings() { };


                //service.ExportImage(report, stream, set);

                //Image x = Image.FromStream(stream);
                //byte[] all = stream.ToArray();
                //int height = x.Height;
                //int width = x.Width;

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