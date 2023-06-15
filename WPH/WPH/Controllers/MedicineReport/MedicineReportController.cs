using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using WPH.Helper;
using WPH.Models.MedicineReport;
using WPH.MvcMockingServices;

namespace WPH.Controllers.MedicineReport
{
    [SessionCheck]
    public class MedicineReportController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;

        public MedicineReportController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
        }


        public ActionResult Form()
        {

            return PartialView("/Views/Shared/PartialViews/AppWebForms/MedicineReport/wpMedicineReportForm.cshtml");
        }


        private StiReport MedicineReport(DateTime fromDate, DateTime toDate, Guid MedicineId, Guid ProducerId)
        {

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            StiReport report = new StiReport();
            string path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "MedicineReport.mrt");
            report.Load(path);
            DateTime FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            DateTime ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

            List<MedicineReportViewModel> AllCosts = _IDUNIT.medicine.GetMedicineReport(clinicSectionId, FromDate, ToDate, MedicineId, ProducerId);

            report.Dictionary.Variables["vTitle"].Value = _localizer["MedicineReport"];
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
            report.Dictionary.Variables["DateFrom"].Value = fromDate.ToShortDateString();
            report.Dictionary.Variables["DateTo"].Value = toDate.ToShortDateString();

            report.RegBusinessObject("Medicine", AllCosts);
            return report;
        }


        public ActionResult PrintMedicineReport(DateTime fromDate, DateTime toDate, Guid MedicineId, Guid ProducerId)
        {
            try
            {

                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = MedicineReport(fromDate, toDate, MedicineId, ProducerId);

                report.Render();
                List<byte[]> allb = new List<byte[]>();


                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });
                    
                    allb.Add(stream.ToArray());
                }
                

                return Json(new { allb });
            }
            catch (Exception e) { throw e; }
        }
    }
}
