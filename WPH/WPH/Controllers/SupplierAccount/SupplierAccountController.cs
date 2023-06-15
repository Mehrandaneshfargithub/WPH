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
using System.Threading.Tasks;
using WPH.Models.SupplierAccount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.SupplierAccount
{
    public class SupplierAccountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<SupplierAccountController> _logger;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDIUnit _IDUNIT;

        public SupplierAccountController(IStringLocalizer<SharedResource> localizer, IWebHostEnvironment hostingEnvironment, IDIUnit dIUnit, ILogger<SupplierAccountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }


        public ActionResult Form(SupplierAccountFilterViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Pay");
                ViewBag.AccessNewPay = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditPay = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeletePay = access.Any(p => p.AccessName == "Delete");
                ViewBag.AccessPrintPay = access.Any(p => p.AccessName == "Print");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.pay.GetModalsViewBags(ViewBag);

                CultureInfo cultures = new CultureInfo("en-US");
                var now = DateTime.Now;
                viewModel.DateFromTxt ??= now.ToString("dd/MM/yyyy", cultures);
                viewModel.DateToTxt ??= now.ToString("dd/MM/yyyy", cultures);
                viewModel.Year = viewModel.Year == 0 ? now.Year : viewModel.Year;

                return View("/Views/Shared/PartialViews/AppWebForms/SupplierAccount/wpSupplierAccount.cshtml", viewModel);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllSupplierAccount([DataSourceRequest] DataSourceRequest request, SupplierAccountFilterViewModel viewModel)
        {
            try
            {
                IEnumerable<SupplierAccountViewModel> AllSupplier = _IDUNIT.supplier.GetAllSupplierAccount(viewModel);
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

        public ActionResult GetAllPayReportFilter()
        {
            try
            {
                var filters = _IDUNIT.baseInfo.GetAllPayReportFilter(_localizer);
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

        public ActionResult ShowReport()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Pay");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Pay/wpPayReportForm.cshtml");
        }

        private StiReport PayReport(SupplierAccountReportFilterViewModel reportViewModel)
        {
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "KashfDabinKarReport.mrt");
            report.Load(path);

            reportViewModel.FromDate = new DateTime(reportViewModel.FromDate.Year, reportViewModel.FromDate.Month, reportViewModel.FromDate.Day, 0, 0, 0);
            reportViewModel.ToDate = new DateTime(reportViewModel.ToDate.Year, reportViewModel.ToDate.Month, reportViewModel.ToDate.Day, 23, 59, 59);


            SupplierAccountReportResultViewModel result = _IDUNIT.supplier.GetSupplierAccountReport(reportViewModel, _localizer);

            CultureInfo cultures = new CultureInfo("en-US");
            report.Dictionary.Variables["vTitle"].Value = $"{_localizer["Supplier"]} : {result.SupplierName}";
            report.Dictionary.Variables["ReportDate"].Value = $"{_localizer["Date"]} {_localizer["Report"]}";
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
            report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
            report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToString("dd/MM/yyyy", cultures);

            report.Dictionary.Variables["vDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["InvoiceNum"].Value = _localizer["InvoiceNum"];
            report.Dictionary.Variables["MainInvoiceNum"].Value = _localizer["MainInvoiceNum"];
            report.Dictionary.Variables["Discount"].Value = _localizer["Discount"];
            report.Dictionary.Variables["TotalAfterDiscount"].Value = _localizer["TotalAfterDiscount"];


            if (reportViewModel.Detail)
            {
                report.Dictionary.Variables["ProductName"].Value = _localizer["ProductName"];
                report.Dictionary.Variables["ProductType"].Value = _localizer["ProductType"];
                report.Dictionary.Variables["ProducerName"].Value = _localizer["ProducerName"];
                report.Dictionary.Variables["ExpiryDate"].Value = _localizer["ExpiryDate"];
                report.Dictionary.Variables["Num"].Value = _localizer["Num"];
                report.Dictionary.Variables["FreeNum"].Value = _localizer["FreeNum"];
                report.Dictionary.Variables["PurchasePrice"].Value = _localizer["PurchasePrice"];
                report.Dictionary.Variables["vTotal"].Value = _localizer["Total"];

                report.RegBusinessObject("AllDetailPurchase", result.AllDetailPurchase);
            }
            else
            {
                report.Dictionary.Variables["InvoiceType"].Value = _localizer["InvoiceType"];
                report.Dictionary.Variables["TotalPrice"].Value = _localizer["TotalPrice"];
                report.Dictionary.Variables["Rem"].Value = _localizer["Rem"];

                report.RegBusinessObject("AllPurchase", result.AllPurchase);
            }

            report.RegBusinessObject("Results", result.Results);
            return report;
        }


        public ActionResult PrintPayReport(SupplierAccountReportFilterViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Pay");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                CultureInfo cultures = new CultureInfo("en-US");
                reportViewModel.FromDate = DateTime.ParseExact(reportViewModel.TxtFromDate, "dd/MM/yyyy", cultures);
                reportViewModel.ToDate = DateTime.ParseExact(reportViewModel.TxtToDate, "dd/MM/yyyy", cultures);

                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = PayReport(reportViewModel);

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
