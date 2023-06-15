using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Helper;
using Microsoft.AspNetCore.Http;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.Cash;
using Stimulsoft.Report;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Report.Export;
using WPH.Models.Reception;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WPH.Controllers.Cash
{
    [SessionCheck]
    public class CashController : Controller
    {
        string userName = string.Empty;


        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<CashController> _logger;


        public CashController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<CashController> logger)
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
                var licenceAccess = _IDUNIT.licenceKey.CheckLicence();
                if (!long.TryParse(licenceAccess, out long remDay))
                    return RedirectToAction("Index", "UserHandler");

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.patientReception.GetModalsViewBags(ViewBag);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();

                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                IEnumerable<PeriodsViewModel> payments = _IDUNIT.baseInfo.GetAllPaymentStatus(_localizer);

                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);

                baseInfosAndPeriods.sections = clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();

                baseInfosAndPeriods.periods = periods;
                baseInfosAndPeriods.payments = payments;

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Cash");
                ViewBag.AccessNewCash = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Cash");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cash/wpCashForm.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

            
        }

        public ActionResult PayAllModal(Guid receptionId)
        {
            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Cash", "UserPortion", "ReceptionDetailPay");
            var NewCash = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Cash");
            ViewBag.AccessUserPortion = access.Any(p => p.AccessName == "View" && p.SubSystemName == "UserPortion");
            ViewBag.AccessDeleteReceptionDetailPay = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "ReceptionDetailPay");
            if (!NewCash)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            try
            {
                PayAllServiceViewModel pay = new PayAllServiceViewModel
                {
                    ReceptionId = receptionId,
                    ReceptionInvoiceNum = _IDUNIT.reception.GetReceptionOnly(receptionId).ReceptionInvoiceNum,
                    PayerName = ""
                };
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId,  "InvoiceNumAndPayerNameRequired").FirstOrDefault();
                ViewBag.InvoiceNumAndPayerNameRequired = (sval?.SValue == null) ? bool.Parse("false") : bool.Parse(sval.SValue.ToLower());
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cash/mdPayAllModal.cshtml", pay);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }


        }


        public ActionResult DoctorWageModal(Guid receptionId)
        {
            try
            {
                var salary_access = _IDUNIT.subSystem.GetUserSubSystemAccess("HumanResourceSalary");
                ViewBag.AccessEditHumanResourceSalary = salary_access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteHumanResourceSalary = salary_access.Any(p => p.AccessName == "Delete");

                DoctorWageViewModel wage = _IDUNIT.receptionService.GetReceptionOperationAndDoctor(receptionId);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Cash/mdDoctorWageModal.cshtml", wage);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }


        }

        private StiReport CashReport(Guid clinicSectionId, int periodId, DateTime dateFrom, DateTime dateTo, string status)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                StiReport report = new StiReport();
                string path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "AllOperationFundReport.mrt");
                report.Load(path);

                string clinicSectionName = _IDUNIT.clinicSection.GetClinicSectionNameById(clinicSectionId);

                IEnumerable<ReceptionForCashReportViewModel> AllRecep = _IDUNIT.reception.GetAllReceptionsByClinicSectionForCashReport(clinicSectionId, periodId, dateFrom, dateTo, status);
                if (periodId != (int)Periods.FromDateToDate)
                {
                    dateFrom = DateTime.Now;
                    dateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
                }
                //report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                report.Dictionary.Variables["vDateFrom"].Value = dateFrom.ToShortDateString();
                report.Dictionary.Variables["vDateTo"].Value = dateTo.ToShortDateString();


                report.Dictionary.Variables["Operation"].Value = _localizer["Operation"];
                report.Dictionary.Variables["ReceptionNum"].Value = _localizer["PayInvoiceNum"];
                report.Dictionary.Variables["HospitalRemaining"].Value = _localizer["HospitalRemaining"];
                report.Dictionary.Variables["RadiologyWage"].Value = _localizer["RadiologyWage"];
                report.Dictionary.Variables["AndoscopyWage"].Value = _localizer["AndoscopyWage"];
                report.Dictionary.Variables["MedicalStaff"].Value = _localizer["MedicalStaff"];
                report.Dictionary.Variables["ChilderenDoctorWage"].Value = _localizer["ChilderenDoctorWage"];
                report.Dictionary.Variables["TreatmentStaffWage"].Value = _localizer["TreatmentStaffWage"];
                report.Dictionary.Variables["PrematureCadres"].Value = _localizer["PrematureCadreWage"];
                report.Dictionary.Variables["SentinelCadre"].Value = _localizer["SentinelCadreWage"];
                report.Dictionary.Variables["AnestheticWage"].Value = _localizer["AnestheticWage"];
                report.Dictionary.Variables["SurgeryWage"].Value = _localizer["SurgeryWage"];
                report.Dictionary.Variables["SurgeryName"].Value = _localizer["SurgeryDoctor"];
                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["InvoiceNum"].Value = _localizer["InvoiceOperationNum"];
                report.Dictionary.Variables["InvoiceDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["Services"].Value = _localizer["Services"];

                report.Dictionary.Variables["OperationPrice"].Value = _localizer["OperationPrice"];
                report.Dictionary.Variables["AnestheticName"].Value = _localizer["AnesthesiologistName"];
                report.Dictionary.Variables["ResidentWage"].Value = _localizer["ResidentWage"];
                report.Dictionary.Variables["Notices"].Value = _localizer["Notices"];
                report.Dictionary.Variables["Number"].Value = _localizer["Num"];


                report.Dictionary.Variables["vTotal"].Value = AllRecep.Sum(x => Convert.ToDecimal(x.HospitalRemaining)).ToString("N0");
                report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                report.Dictionary.Variables["TotalHospitalRemaining"].Value = _localizer["Total"] + " " + _localizer["HospitalRemaining"];
                report.Dictionary.Variables["NumberOfOperations"].Value = _localizer["Num"] + " " + _localizer["Operation"];

                IEnumerable<DateTime> allDate = AllRecep.Select(x => x.Date.GetValueOrDefault().Date).Distinct();
                List<ReceptionForCashReportViewModel> total = new List<ReceptionForCashReportViewModel>();
                //foreach (var sur in allDate)
                {
                    //total.Add( new ReceptionForCashReportViewModel()
                    total.AddRange(allDate.Select(sur => new ReceptionForCashReportViewModel()
                    {
                        Date = sur,
                        TotalOpertion = AllRecep.Where(x => x.Date.GetValueOrDefault().Date == sur).Sum(a => Convert.ToDecimal(a.HospitalRemaining)).ToString("N0"),
                        CountOpertion = AllRecep.Where(x => x.Date.GetValueOrDefault().Date == sur).Count().ToString("N0")
                    }));
                }

                report.RegBusinessObject("fundDetail", AllRecep);
                report.RegBusinessObject("TotalOperations", total);

                return report;
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); throw e; }

            
        }


        public ActionResult PrintCashReport(Guid clinicSectionId, int periodId, string fromDate, string toDate, string status)
        {
            try
            {
                string[] from = fromDate.Split(':');
                DateTime dateFrom = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]));
                string[] to = toDate.Split(':');
                DateTime dateTo = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]));
                StiReport report = CashReport(clinicSectionId, periodId, dateFrom, dateTo, status);

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
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); throw e; }

        }



        

    }
}