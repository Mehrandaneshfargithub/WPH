using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.MoneyConvert;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.Reception;
using WPH.MvcMockingServices;
using Microsoft.Extensions.Logging;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Helper;
using System.Drawing;
using WPH.Models.CustomDataModels.AnalysisResultMaster;
using System.Globalization;
using QRCoder;

namespace WPH.Controllers.PatientReceptionAnalysis
{
    [SessionCheck]
    public class PatientReceptionAnalysisController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<PatientReceptionAnalysisController> _logger;

        public PatientReceptionAnalysisController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<PatientReceptionAnalysisController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
        }


        public async Task<IActionResult> Form()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("Service", "NewAnalysisReception");

                var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "NewAnalysisReception");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CanConsumeProduct", "UseOnlinePrescriptionTest", "UseOnlineResult");

                try
                {
                    ViewBag.UseOnlinePrescriptionTest = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseOnlinePrescriptionTest")?.SValue ?? "false");
                }
                catch { ViewBag.UseOnlinePrescriptionTest = "false"; }


                try
                {
                    ViewBag.UseOnlineResult = sval?.FirstOrDefault(p => p.ShowSName == "UseOnlineResult")?.SValue?.ToLower() ?? "false";
                }
                catch { ViewBag.UseOnlineResult = "false"; }


                ViewBag.CanConsumeProduct = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "CanConsumeProduct")?.SValue ?? "false");

                ViewBag.AccessNewService = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "Service");

                ViewBag.ReceptionId = Guid.Empty;
                ViewBag.PatientId = "";
                ViewBag.DoctorId = "";
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReceptionAnalysis/wpPatientReceptionAnalysisFormNew.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            

        }

        public async Task<IActionResult> EditModal(Guid Id, Guid PatientId, Guid DoctorId, Guid ReceptionClinicSectionId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reception");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }
            string des = "";
            if (ReceptionClinicSectionId != Guid.Empty)
                des = _IDUNIT.receptionClinicSection.GetReceptionClinicSection(ReceptionClinicSectionId).Description;
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                var sval1 = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseOnlineResult").FirstOrDefault();

                ViewBag.UseOnlineResult = sval1?.SValue?.ToLower() ?? "false";
            }
            catch { ViewBag.UseOnlineResult = "false"; }
            ViewBag.ReceptionClinicSectionDescription = des;
            ViewBag.ReceptionId = "";
            ViewBag.PatientId = "";
            ViewBag.DoctorId = "";
            ViewBag.ReceptionClinicSectionId = "";


            if (PatientId != Guid.Empty)
            {
                ViewBag.PatientId = PatientId;
            }
            if (DoctorId != Guid.Empty)
            {
                ViewBag.DoctorId = DoctorId;
            }
            if (Id != Guid.Empty)
            {
                ViewBag.ReceptionId = Id;
            }
            if (ReceptionClinicSectionId != Guid.Empty)
            {
                ViewBag.ReceptionClinicSectionId = ReceptionClinicSectionId;
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReceptionAnalysis/wpPatientReceptionAnalysisFormNew.cshtml");
        }

        public JsonResult GetReception(Guid ReceptionId)
        {
            try
            {
                ReceptionViewModel PatientReception = _IDUNIT.patientReception.GetPatientReceptionById(ReceptionId);

                return Json(PatientReception);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetPatientReceptionAnalysisByReceptionId(Guid ReceptionId)
        {
            try
            {
                List<PatientReceptionAnalysisViewModel> PatientReception = _IDUNIT.patientReceptionAnalysis.GetPatientReceptionAnalysisByReceptionId(ReceptionId);

                return Json(PatientReception);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetPatient()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<PatientViewModel> eventList = _IDUNIT.patient.GetAllPatientsWithCombinedNameAndPhoneNumber(false, ClinicSectionId);

                return Json(eventList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetDoctor()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<DoctorViewModel> doctors = _IDUNIT.doctor.GetAllDoctorsWithCombinedNameAndSpeciallity(false, clinicSectionId);

                return Json(doctors);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        private async Task<StiReport> PatientReceptionAnalysisReport(Guid PatientReceptionId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                StiReport report = new StiReport();

                var val = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseRadiologyReportNewDesign_8cm", "LabPhoneNumber", "LabratoryReportHeader_8cm", "RadiologyReportHeader_8cm", "DefaultTextForPrint", "UseOnlineResult");
                bool useNewDesign = bool.Parse(val?.FirstOrDefault(p => p.ShowSName == "UseRadiologyReportNewDesign_8cm")?.SValue ?? "false");

                ReceptionViewModel PatientReception = _IDUNIT.patientReception.GetPatientReceptionByIdForReport(PatientReceptionId);

                string path;
                if (PatientReception.ClinicSectionTypeName == "Labratory")
                {
                    path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "VatanReceptionLabratoryReport8cm.mrt");
                }
                else
                {
                    if (useNewDesign)
                    {
                        path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "VatanReceptionLabratoryReport8cm.mrt");
                    }
                    else
                    {
                        path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "VatanReceptionRadiologyReport8cm.mrt");
                    }
                }

                CultureInfo cultures = new CultureInfo("en-US");
                report.Load(path);
                report.Dictionary.Variables["Date"].Value = _localizer["Date"];
                report.Dictionary.Variables["ReceptionDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["vDate"].Value = PatientReception.ReceptionDate.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy", cultures);
                report.Dictionary.Variables["DoctorName"].Value = _localizer["DoctorSentName"];
                report.Dictionary.Variables["vDoctorName"].Value = PatientReception.DoctorUserName;
                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["vPatientName"].Value = PatientReception.Patient.User.Name;
                report.Dictionary.Variables["Age"].Value = _localizer["Age"];
                report.Dictionary.Variables["vAge"].Value = PatientReception.Patient.Age.GetValueOrDefault().ToString();
                report.Dictionary.Variables["Money"].Value = _localizer["ReceivedAmount"];
                
                try
                {
                    report.Dictionary.Variables["Discount"].Value = _localizer["Discount"];
                    report.Dictionary.Variables["vDiscount"].Value = PatientReception.Discount.GetValueOrDefault().ToString("N0");
                }
                catch { }
                report.Dictionary.Variables["InvoiceNum"].Value = PatientReception.ReceptionNum;

                report.Dictionary.Variables["LabPhoneNumber"].Value = val?.FirstOrDefault(p => p.ShowSName == "LabPhoneNumber")?.SValue ?? "";


                if (PatientReception.ClinicSectionTypeName == "Labratory")
                {
                    var res = _IDUNIT.patientReceptionAnalysis.GetPatientAnalysisReportByReceptionId(PatientReceptionId);

                    report.Dictionary.Variables["AnalysisName"].Value = _localizer["AnalysisName"];
                    report.Dictionary.Variables["Price"].Value = _localizer["Price"];
                    report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                    report.Dictionary.Variables["VTotal"].Value = res.Total;
                    report.Dictionary.Variables["ReportHeader"].Value = val?.FirstOrDefault(p => p.ShowSName == "LabratoryReportHeader_8cm")?.SValue ?? "";
                    report.Dictionary.Variables["SectionName"].Value = PatientReception?.ClinicSectionName ?? "";
                    report.Dictionary.Variables["vMoney"].Value = ( Convert.ToDecimal(res.Total) - PatientReception.Discount.GetValueOrDefault()).ToString("N0");

                    report.RegBusinessObject("SelectedAnalysis", res.Items);
                }
                else
                {
                    if (useNewDesign)
                    {
                        var res = _IDUNIT.patientReceptionAnalysis.GetPatientAnalysisReportByReceptionId(PatientReceptionId);

                        report.Dictionary.Variables["AnalysisName"].Value = _localizer["AnalysisName"];
                        report.Dictionary.Variables["Price"].Value = _localizer["Price"];
                        report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                        report.Dictionary.Variables["VTotal"].Value = res.Total;
                        report.Dictionary.Variables["SectionName"].Value = PatientReception?.ClinicSectionName ?? "";
                        report.Dictionary.Variables["vMoney"].Value = (Convert.ToDecimal(res.Total) - PatientReception.Discount.GetValueOrDefault()).ToString("N0");

                        report.RegBusinessObject("SelectedAnalysis", res.Items);
                    }
                    else
                    {
                        string allAnalysis = "";

                        foreach (var analysis in PatientReception.PatientReceptionAnalyses)
                        {
                            if (analysis.Analysis != null)
                            {
                                allAnalysis += " & " + analysis.Analysis.Name;
                            }
                            if (analysis.AnalysisItem != null)
                            {
                                allAnalysis += " & " + analysis.AnalysisItem.Name;
                            }
                            if (analysis.GroupAnalysis != null)
                            {
                                allAnalysis += " & " + analysis.GroupAnalysis.Name;
                            }
                        }

                        string newallAnalysis;
                        try
                        {
                            newallAnalysis = allAnalysis.Remove(0, 2);
                        }
                        catch
                        {
                            newallAnalysis = "";
                        }


                        if (PatientReception.ClinicSectionName.ToLower() == "ctscan")
                        {
                            report.Dictionary.Variables["vCTScan"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "ultrasound")
                        {
                            report.Dictionary.Variables["vUltraSound"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "mri")
                        {
                            report.Dictionary.Variables["vMRI"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "xray")
                        {
                            report.Dictionary.Variables["vXray"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "mammography")
                        {
                            report.Dictionary.Variables["Mammography"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "nephrolithotripsy unit")
                        {
                            report.Dictionary.Variables["Nephrolithotripsy_Unit"].Value = newallAnalysis;
                        }
                        if (PatientReception.ClinicSectionName.ToLower() == "dexa scan")
                        {
                            report.Dictionary.Variables["Dexa_Scan"].Value = newallAnalysis;
                        }

                    }

                    report.Dictionary.Variables["ReportHeader"].Value = val?.FirstOrDefault(p => p.ShowSName == "RadiologyReportHeader_8cm")?.SValue ?? "";
                }


                string rootPath = HostingEnvironment.WebRootPath;
                var logo = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportLogo_8cm");
                try
                {
                    Bitmap banner = new Bitmap(Path.Combine(rootPath + logo));
                    report.Dictionary.Variables["Logo"].ValueObject = (Image)banner;
                }
                catch { }


                try
                {
                    report.Dictionary.Variables["Description"].Value = val?.FirstOrDefault(p => p.ShowSName == "DefaultTextForPrint")?.SValue;
                }
                catch { }

                try
                {
                    bool useOnlineResult = bool.Parse(val?.FirstOrDefault(p => p.ShowSName == "UseOnlineResult")?.SValue ?? "false");
                    if (useOnlineResult)
                    {
                        int serverNumber = _IDUNIT.analysisResultMaster.GetReceptionServerNumber(PatientReceptionId);
                        //report.Dictionary.Variables["OnlineResultText"].Value = _localizer["YouCanVisit"] + " Masnaw.com " + _localizer["ForSeeResult"] + $" Code={serverNumber}, PhoneNumber={PatientReception.Patient.User.PhoneNumber} ";
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        string link = $"http://159.69.236.101:402/api/Result/GetResult?Code={serverNumber}&PhoneNumber={PatientReception.Patient.User.PhoneNumber}";
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                        QRCoder.QRCode QrCode = new QRCoder.QRCode(QrCodeInfo);
                        Bitmap QrBitmap = QrCode.GetGraphic(5);
                        Image im = (Image)QrBitmap;
                        report.Dictionary.Variables["Result"].ValueObject = im;
                    }
                }
                catch { }



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


        public async Task<IActionResult> PrintPatientReceptionAnalysisReport(Guid PatientReceptionId)
        {
            try
            {
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = await PatientReceptionAnalysisReport(PatientReceptionId);

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