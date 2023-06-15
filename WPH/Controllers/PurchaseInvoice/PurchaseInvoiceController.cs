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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.PurchaseInvoice;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PurchaseInvoice
{
    [SessionCheck]
    public class PurchaseInvoiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PurchaseInvoiceController> _logger;
        protected readonly IWebHostEnvironment _hostingEnvironment;

        public PurchaseInvoiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PurchaseInvoiceController> logger, IWebHostEnvironment hostingEnvironment)
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
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoice");
                ViewBag.AccessNewPurchaseInvoice = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditPurchaseInvoice = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeletePurchaseInvoice = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.purchaseInvoice.GetModalsViewBags(ViewBag);

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel
                {
                    periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer),
                    filters = _IDUNIT.baseInfo.GetAllPurchaseFilter(_localizer)
                };

                return View("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoice/wpPurchaseInvoice.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddAndNewModal(string PurchaseType)
        {
            if (string.IsNullOrWhiteSpace(PurchaseType))
                return Json("0");

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "PurchaseInvoice", "PurchaseInvoiceDetailSalePrice", "CanUsePurchaseInvoiceDate");
            ViewBag.AccessNewPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            ViewBag.AccessEditPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PurchaseInvoiceDetails");
            ViewBag.AccessDeletePurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PurchaseInvoiceDetails");

            ViewBag.AccessPurchaseInvoiceDetailSalePrice = access1.Any(p => p.SubSystemName == "PurchaseInvoiceDetailSalePrice");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePurchaseInvoiceDate");
            ViewBag.PurchaseType = PurchaseType;

            PurchaseInvoiceViewModel des = new PurchaseInvoiceViewModel
            {
                CanChange = true
            };
            return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoice/mdPurchaseInvoiceNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdateMaterial(PurchaseInvoiceViewModel purchaseInvoice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                purchaseInvoice.PurchaseType = "Material";
                purchaseInvoice.ClinicSectionId = clinicSectionId;

                if (purchaseInvoice.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    purchaseInvoice.ModifiedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoice.UpdatePurchaseInvoice(purchaseInvoice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    purchaseInvoice.CreatedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoice.AddNewPurchaseInvoice(purchaseInvoice));
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

        public JsonResult AddOrUpdate(PurchaseInvoiceViewModel purchaseInvoice)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                purchaseInvoice.PurchaseType = "Medicine";
                purchaseInvoice.ClinicSectionId = clinicSectionId;

                if (purchaseInvoice.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    purchaseInvoice.ModifiedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoice.UpdatePurchaseInvoice(purchaseInvoice));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoice");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    purchaseInvoice.CreatedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoice.AddNewPurchaseInvoice(purchaseInvoice));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, PurchaseInvoiceFilterViewModel filterViewModel)
        {
            try
            {
                string[] from = filterViewModel.TxtDateFrom.Split('-');
                string[] to = filterViewModel.TxtDateTo.Split('-');

                filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<PurchaseInvoiceViewModel> AllPurchaseInvoice = _IDUNIT.purchaseInvoice.GetAllPurchaseInvoices(clinicSectionId, filterViewModel, _localizer);
                return Json(AllPurchaseInvoice.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult EditModal(Guid Id, bool goBack = false)
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "PurchaseInvoice", "PurchaseInvoiceDetailSalePrice", "CanUsePurchaseInvoiceDate");
            ViewBag.AccessNewPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            ViewBag.AccessEditPurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PurchaseInvoiceDetails");
            ViewBag.AccessDeletePurchaseInvoiceDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PurchaseInvoiceDetails");

            ViewBag.AccessPurchaseInvoiceDetailSalePrice = access1.Any(p => p.SubSystemName == "PurchaseInvoiceDetailSalePrice");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PurchaseInvoice");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUsePurchaseInvoiceDate");
                ViewBag.BackToAccount = goBack;
                ViewBag.PurchasePaid = _IDUNIT.purchaseInvoice.CheckPurchaseInvoicePaid(Id);
                PurchaseInvoiceViewModel hos = _IDUNIT.purchaseInvoice.GetPurchaseInvoice(Id);
                ViewBag.PurchaseType = hos.PurchaseType;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoice/mdPurchaseInvoiceNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id, string pass)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "PurchaseInvoice");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                string password = Crypto.Hash(pass, "MD5");


                var oStatus = _IDUNIT.purchaseInvoice.RemovePurchaseInvoice(Id, userId, password);
                return Json(oStatus);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult TotalPriceModal(Guid Id)
        {
            try
            {

                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoice/dgPurchaseInvoiceTotalPriceGrid.cshtml", Id);
            }
            catch { return Json(0); }
        }

        public ActionResult GetAllTotalPrice([DataSourceRequest] DataSourceRequest request, Guid purchaseInvoiceId)
        {
            try
            {
                var total = _IDUNIT.purchaseInvoice.GetAllTotalPrice(purchaseInvoiceId);
                return Json(total.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetPayPurchaseInvoice(Guid? payId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetPayPurchaseInvoice(payId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetNotPayPurchaseInvoice(Guid? supplierId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetNotPayPurchaseInvoice(supplierId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetPartialPayPurchaseInvoice(payId, currencyId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId)
        {
            try
            {
                var result = _IDUNIT.purchaseInvoice.GetNotPartialPayPurchaseInvoice(supplierId, currencyId, payId);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        private StiReport PurchaseReport(Guid purchaseInvoiceId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "PurchaseReport.mrt");
            report.Load(path);

            PrintPurchaseReportViewModel result = _IDUNIT.purchaseInvoice.GetPurchaseForReport(purchaseInvoiceId, _localizer);

            CultureInfo cultures = new CultureInfo("en-US");
            report.Dictionary.Variables["vTitle"].Value = _localizer["PurchaseInvoice"];
            report.Dictionary.Variables["VisitDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["vVisitDate"].Value = DateTime.Now.ToString("dd/MM/yyyy", cultures);

            report.Dictionary.Variables["Supplier"].Value = _localizer["Supplier"];
            report.Dictionary.Variables["vSupplier"].Value = result.Supplier;

            report.Dictionary.Variables["InvoiceDate"].Value = _localizer["InvoiceDate"];
            report.Dictionary.Variables["vInvoiceDate"].Value = result.InvoiceDate;

            report.Dictionary.Variables["InvoiceNum"].Value = _localizer["InvoiceNum"];
            report.Dictionary.Variables["vInvoiceNum"].Value = result.InvoiceNum;

            report.Dictionary.Variables["MainInvoiceNum"].Value = _localizer["MainInvoiceNum"];
            report.Dictionary.Variables["vMainInvoiceNum"].Value = result.MainInvoiceNum;

            report.Dictionary.Variables["Description"].Value = _localizer["Description"];
            report.Dictionary.Variables["vDescription"].Value = result.Description;

            report.Dictionary.Variables["ProductName"].Value = _localizer["ProductName"];
            report.Dictionary.Variables["ProductType"].Value = _localizer["ProductType"];
            report.Dictionary.Variables["ProducerName"].Value = _localizer["ProducerName"];
            report.Dictionary.Variables["ExpiryDate"].Value = _localizer["ExpiryDate"];
            report.Dictionary.Variables["Num"].Value = _localizer["Num"];
            report.Dictionary.Variables["FreeNum"].Value = _localizer["FreeNum"];
            report.Dictionary.Variables["PurchasePrice"].Value = _localizer["PurchasePrice"];
            report.Dictionary.Variables["TotalPrice"].Value = _localizer["TotalPrice"];
            report.Dictionary.Variables["Discount"].Value = _localizer["Discount"];
            report.Dictionary.Variables["vTotal"].Value = _localizer["TotalAmount"];

            var image_path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportHeaderBanner");
            string rootPath = _hostingEnvironment.WebRootPath;

            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + image_path));
                report.Dictionary.Variables["banner"].ValueObject = (Image)banner;
            }
            catch { }

            report.RegBusinessObject("Details", result.Details);
            report.RegBusinessObject("TotalResult", result.Totals);

            return report;
        }

        public IActionResult PrintPurchaseReport(Guid purchaseInvoiceId)
        {
            try
            {
                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = PurchaseReport(purchaseInvoiceId);
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

        public IActionResult GetPurchaseTypes()
        {
            var result = new[]
            {
                new
                {
                    Name = $"{_localizer["MedicineStoreroom"]}",
                    Value = "Medicine"
                },
                new
                {
                    Name = $"{_localizer["MaterialStoreroom"]}",
                    Value = "Material"
                }
            }.ToList();

            return Json(result);
        }
    }
}
