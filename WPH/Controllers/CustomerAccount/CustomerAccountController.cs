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
using WPH.Models.CustomerAccount;
using WPH.MvcMockingServices;

namespace WPH.Controllers.CustomerAccount
{
    public class CustomerAccountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<CustomerAccountController> _logger;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDIUnit _IDUNIT;

        public CustomerAccountController(IStringLocalizer<SharedResource> localizer, IWebHostEnvironment hostingEnvironment, IDIUnit dIUnit, ILogger<CustomerAccountController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }


        public ActionResult Form(CustomerAccountFilterViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Receive");
                ViewBag.AccessNewReceive = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditReceive = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteReceive = access.Any(p => p.AccessName == "Delete");
                ViewBag.AccessPrintReceive = access.Any(p => p.AccessName == "Print");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.receive.GetModalsViewBags(ViewBag);

                CultureInfo cultures = new CultureInfo("en-US");
                var now = DateTime.Now;
                viewModel.DateFromTxt ??= now.ToString("dd/MM/yyyy", cultures);
                viewModel.DateToTxt ??= now.ToString("dd/MM/yyyy", cultures);
                viewModel.Year = viewModel.Year == 0 ? now.Year : viewModel.Year;

                return View("/Views/Shared/PartialViews/AppWebForms/CustomerAccount/wpCustomerAccount.cshtml", viewModel);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllCustomerAccount([DataSourceRequest] DataSourceRequest request, CustomerAccountFilterViewModel viewModel)
        {
            try
            {
                IEnumerable<CustomerAccountViewModel> AllCustomer = _IDUNIT.customer.GetAllCustomerAccount(viewModel);
                return Json(AllCustomer.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllReceiveReportFilter()
        {
            try
            {
                var filters = _IDUNIT.baseInfo.GetAllReceiveReportFilter(_localizer);
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
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Receive");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Receive/wpReceiveReportForm.cshtml");
        }

        private StiReport ReceiveReport(CustomerAccountReportFilterViewModel reportViewModel)
        {
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "KashfKeryarReport.mrt");
            report.Load(path);

            reportViewModel.FromDate = new DateTime(reportViewModel.FromDate.Year, reportViewModel.FromDate.Month, reportViewModel.FromDate.Day, 0, 0, 0);
            reportViewModel.ToDate = new DateTime(reportViewModel.ToDate.Year, reportViewModel.ToDate.Month, reportViewModel.ToDate.Day, 23, 59, 59);


            CustomerAccountReportResultViewModel result = _IDUNIT.customer.GetCustomerAccountReport(reportViewModel, _localizer);


            CultureInfo cultures = new CultureInfo("en-US");
            report.Dictionary.Variables["vTitle"].Value = $"{_localizer["Customer"]} : {result.CustomerName}";
            report.Dictionary.Variables["ReportDate"].Value = $"{_localizer["Date"]} {_localizer["Report"]}";
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
            report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
            report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToString("dd/MM/yyyy", cultures);

            report.Dictionary.Variables["vDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["InvoiceNum"].Value = _localizer["InvoiceNum"];
            report.Dictionary.Variables["Description"].Value = _localizer["Description"];
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
                report.Dictionary.Variables["SalePrice"].Value = _localizer["SalePrice"];
                report.Dictionary.Variables["vTotal"].Value = _localizer["Total"];

                report.RegBusinessObject("AllDetailSale", result.AllDetailSale);
            }
            else
            {
                report.Dictionary.Variables["InvoiceType"].Value = _localizer["InvoiceType"];
                report.Dictionary.Variables["TotalPrice"].Value = _localizer["TotalPrice"];
                report.Dictionary.Variables["Rem"].Value = _localizer["Rem"];

                report.RegBusinessObject("AllSale", result.AllSale);
            }

            report.RegBusinessObject("Results", result.Results);
            return report;
        }


        public ActionResult PrintReceiveReport(CustomerAccountReportFilterViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Receive");
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
                StiReport report = ReceiveReport(reportViewModel);

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
