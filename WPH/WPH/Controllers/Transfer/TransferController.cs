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
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Transfer;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Transfer
{
    [SessionCheck]
    public class TransferController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<TransferController> _logger;
        protected readonly IWebHostEnvironment _hostingEnvironment;

        public TransferController(IStringLocalizer<SharedResource> localizer, IWebHostEnvironment hostingEnvironment, IDIUnit dIUnit, ILogger<TransferController> logger)
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
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Transfer");
                ViewBag.AccessNewTransfer = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditTransfer = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteTransfer = access.Any(p => p.AccessName == "Delete");
                ViewBag.AccessPrintTransfer = access.Any(p => p.AccessName == "Print");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.transfer.GetModalsViewBags(ViewBag);

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new();

                baseInfosAndPeriods.periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                baseInfosAndPeriods.filters = _IDUNIT.baseInfo.GetAllTransferFilter(_localizer);

                return View("/Views/Shared/PartialViews/AppWebForms/Transfer/wpTransfer.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult AddAndNewModal()
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("TransferDetail", "Transfer", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "CanUseTransferDate");
            ViewBag.AccessNewTransferDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "TransferDetail");
            ViewBag.AccessEditTransferDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TransferDetail");
            ViewBag.AccessDeleteTransferDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "TransferDetail");

            ViewBag.AccessCanUseWholeSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
            ViewBag.AccessCanUseMiddleSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

            var access = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Transfer");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseTransferDate");

            TransferViewModel des = new TransferViewModel
            {
                ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"))
            };
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Transfer/mdTransferNewModal.cshtml", des);
        }

        public JsonResult AddOrUpdate(TransferViewModel Transfer)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (Transfer.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Transfer");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    Transfer.ModifiedUserId = userId;

                    return Json(_IDUNIT.transfer.UpdateTransfer(Transfer));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Transfer");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }
                    Transfer.CreatedUserId = userId;
                    return Json(_IDUNIT.transfer.AddNewTransfer(Transfer/*, clinicSectionId*/));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, TransferFilterViewModel filterViewModel)
        {
            try
            {
                string[] from = filterViewModel.TxtDateFrom.Split('-');
                string[] to = filterViewModel.TxtDateTo.Split('-');

                filterViewModel.DateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                filterViewModel.DateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                List<Guid> clinicSections = new();
                if (filterViewModel.ClinicSectionId == null)
                {
                    clinicSections.AddRange(_IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => p.Guid).ToList());

                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "ParentStoreAccess");
                    if (access)
                    {
                        clinicSections.Add(clinicSectionId);
                    }
                }
                else
                {
                    clinicSections.Add(filterViewModel.ClinicSectionId.Value);
                }

                IEnumerable<TransferViewModel> AllTransfer = _IDUNIT.transfer.GetAllTransfers(clinicSections, filterViewModel);

                return Json(AllTransfer.ToDataSourceResult(request));
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

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("TransferDetail", "Transfer", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "CanUseTransferDate", "TransferDetailSalePrice");
            ViewBag.AccessNewTransferDetail = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "TransferDetail");
            ViewBag.AccessEditTransferDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TransferDetail");
            ViewBag.AccessDeleteTransferDetail = access1.Any(p => p.AccessName == "Delete" && p.SubSystemName == "TransferDetail");

            ViewBag.AccessCanUseWholeSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
            ViewBag.AccessCanUseMiddleSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

            var access = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Transfer");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                ViewBag.AccessEditDate = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseTransferDate");

                TransferViewModel hos = _IDUNIT.transfer.GetTransfer(Id);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                ViewBag.AccessTransferDetailSalePrice = access1.Any(p => p.SubSystemName == "TransferDetailSalePrice") && hos.DestinationClinicSectionId == clinicSectionId;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Transfer/mdTransferNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Transfer");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                var oStatus = _IDUNIT.transfer.RemoveTransfer(Id);
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

        public JsonResult GetSourceClinicSections()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => new { p.Guid, p.Name, Parent = false }).ToList();

                var access = _IDUNIT.subSystem.CheckUserAccess("New", "ParentStoreAccess");
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

        public JsonResult GetDestinationClinicSections(Guid sourceClinicSectionId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => new { p.Guid, p.Name, Parent = false }).ToList();

                clinicSectionAccess.Add(new { Guid = clinicSectionId, Name = clinicSectionName, Parent = false });

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ParentStoreAccess", "ClinicSectionStoreAccess");

                var parent_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ParentStoreAccess");
                if (parent_access && sourceClinicSectionId == clinicSectionId)
                {
                    var clinicSection_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ClinicSectionStoreAccess");
                    if (clinicSection_access)
                    {
                        var main_clinicSections = _IDUNIT.clinicSection.GetAllMainClinicSectionsExceptOne(clinicSectionId)
                            .Select(p => new { p.Guid, p.Name, Parent = true }).ToList();

                        clinicSectionAccess.AddRange(main_clinicSections);
                    }
                }

                return Json(clinicSectionAccess.Where(p => p.Guid != sourceClinicSectionId));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetUserDestinationClinicSections()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => new { p.Guid, p.Name }).ToList();

                clinicSectionAccess.Add(new { Guid = clinicSectionId, Name = clinicSectionName });

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ParentStoreAccess", "ClinicSectionStoreAccess");

                var parent_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ParentStoreAccess");
                if (parent_access)
                {
                    var clinicSection_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ClinicSectionStoreAccess");
                    if (clinicSection_access)
                    {
                        var main_clinicSections = _IDUNIT.clinicSection.GetAllMainClinicSectionsExceptOne(clinicSectionId)
                            .Select(p => new { p.Guid, p.Name }).ToList();

                        clinicSectionAccess.AddRange(main_clinicSections);
                    }
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

        public JsonResult GetReceiversName()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string clinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId)
                    .Select(p => p.Guid).ToList();

                clinicSectionAccess.Add(clinicSectionId);

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ParentStoreAccess", "ClinicSectionStoreAccess");

                var parent_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ParentStoreAccess");
                if (parent_access)
                {
                    var clinicSection_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ClinicSectionStoreAccess");
                    if (clinicSection_access)
                    {
                        var main_clinicSections = _IDUNIT.clinicSection.GetAllMainClinicSectionsExceptOne(clinicSectionId)
                            .Select(p => p.Guid).ToList();

                        clinicSectionAccess.AddRange(main_clinicSections);
                    }
                }

                var result = _IDUNIT.transfer.GetReceiversName(clinicSectionAccess).Select(p => new { Name = p, Value = p });

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

        public ActionResult ShowReport()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Transfer");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Transfer/wpTransferReportForm.cshtml");
        }


        private StiReport TransferReport(TransferReportViewModel reportViewModel)
        {
            Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            StiReport report = new StiReport();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "TransferReport.mrt");
            report.Load(path);

            reportViewModel.FromDate = new DateTime(reportViewModel.FromDate.Year, reportViewModel.FromDate.Month, reportViewModel.FromDate.Day, 0, 0, 0);
            reportViewModel.ToDate = new DateTime(reportViewModel.ToDate.Year, reportViewModel.ToDate.Month, reportViewModel.ToDate.Day, 23, 59, 59);


            reportViewModel.AllClinicSectionGuids = _IDUNIT.clinicSection.GetAllClinicSectionsChildForTransferSource(clinicSectionId, UserId)
            .Select(p => p.Guid).ToList();

            reportViewModel.AllClinicSectionGuids.Add(clinicSectionId);

            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("ParentStoreAccess", "ClinicSectionStoreAccess");

            var parent_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ParentStoreAccess");
            if (parent_access)
            {
                var clinicSection_access = access.Any(p => p.AccessName == "New" && p.SubSystemName == "ClinicSectionStoreAccess");
                if (clinicSection_access)
                {
                    var main_clinicSections = _IDUNIT.clinicSection.GetAllMainClinicSectionsExceptOne(clinicSectionId)
                        .Select(p => p.Guid).ToList();

                    reportViewModel.AllClinicSectionGuids.AddRange(main_clinicSections);
                }
            }

            IEnumerable<TransferReportResultViewModel> transfers = _IDUNIT.transfer.GetTransferReport(reportViewModel);


            report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
            report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
            report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
            report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
            report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToShortDateString();
            report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToShortDateString();

            report.Dictionary.Variables["TransferDate"].Value = _localizer["Date"];
            report.Dictionary.Variables["Receiver"].Value = _localizer["Receiver"];
            report.Dictionary.Variables["Sender"].Value = _localizer["Sender"];
            report.Dictionary.Variables["SourceClinicSectionName"].Value = _localizer["SourceClinicSectionName"];
            report.Dictionary.Variables["DestinationClinicSectionName"].Value = _localizer["DestinationClinicSectionName"];
            report.Dictionary.Variables["SourceProduct"].Value = _localizer["SourceProduct"];
            report.Dictionary.Variables["DestinationProduct"].Value = _localizer["DestinationProduct"];
            report.Dictionary.Variables["Number"].Value = _localizer["Number"];


            report.RegBusinessObject("TransferResult", transfers);
            return report;
        }

        public ActionResult PrintTransferReport(TransferReportViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "Transfer");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                reportViewModel.FromDate = DateTime.ParseExact(reportViewModel.TxtFromDate, "dd/MM/yyyy", null);
                reportViewModel.ToDate = DateTime.ParseExact(reportViewModel.TxtToDate, "dd/MM/yyyy", null);

                string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = TransferReport(reportViewModel);

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
    }
}
