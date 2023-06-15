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
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.Child;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.HospitalPatients;
using WPH.Models.Reception;
using WPH.Models.ReceptionService;
using WPH.Models.ReceptionTemperature;
using WPH.Models.Surgery;
using WPH.MvcMockingServices;

namespace WPH.Controllers.HospitalPatients
{
    [SessionCheck]
    public class HospitalPatientController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<HospitalPatientController> _logger;



        public HospitalPatientController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<HospitalPatientController> logger)
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
                _IDUNIT.patientReception.GetModalsViewBags(ViewBag);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();

                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                IEnumerable<PeriodsViewModel> clearance = _IDUNIT.baseInfo.GetAllClearanceType(_localizer);


                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);
                baseInfosAndPeriods.sections = clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();

                try
                {
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                    ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                }
                catch { ViewBag.useDollar = "false"; }

                baseInfosAndPeriods.periods = periods;
                baseInfosAndPeriods.clearances = clearance;

                ViewBag.NotLab = HttpContext.Session.GetString("SectionTypeName")?.ToLower();
                ViewBag.HospitalPatient = true;
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("HospitalPatient");
                ViewBag.AccessEditHospitalPatient = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessPrintHospitalPatient = access.Any(p => p.AccessName == "Print");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HospitalPatients/wpHospitalPatients.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, int status)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                IEnumerable<HospitalPatientReportResultViewModel> AllPatientReception = _IDUNIT.reception.GetAllReceptionsForHospitalPatients(section.Id, periodId, fromDate, toDate, status);
                return Json(AllPatientReception.ToDataSourceResult(request));
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

            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("Service", "SubStoreroom", "Discharge", "HospitalPatient");
            ViewBag.AccessNewService = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "Service");

            ViewBag.AccessNewMedicineProduct = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");

            ViewBag.AccessEditDischarge = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Discharge");

            var access = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "HospitalPatient");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ViewBag.ReceptionId = Id;
            ViewBag.ShowHistory = false;
            ViewBag.OtherAnalysis = false;
            ViewBag.UseAnalysis = false;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/HospitalPatients/wpAllPatientInformation.cshtml");
        }

        private async Task<StiReport> PatientReceptionAnalysisReport(Guid ReceptionId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                StiReport report = new StiReport();



                ReceptionViewModel Reception = _IDUNIT.reception.GetReception(ReceptionId);
                string path = "";
                path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "AllPatientInformationReport.mrt");

                CultureInfo cultures = new CultureInfo("en-US");
                report.Load(path);
                report.Dictionary.Variables["PatientId"].Value = Reception.PatientId.ToString();


                report.Dictionary.Variables["Examination"].Value = Reception.Examination;

                report.Dictionary.Variables["PatientName"].Value = Reception.Patient.UserName.ToString();
                report.Dictionary.Variables["Age"].Value = Reception.Patient.Age.ToString();
                report.Dictionary.Variables["Gender"].Value = Reception.Patient.UserGenderName;
                report.Dictionary.Variables["EntranceDate"].Value = Reception.EntranceDate.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy", cultures);
                try
                {
                    report.Dictionary.Variables["ExitDate"].Value = Reception.ExitDate.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy", cultures);
                }
                catch { }
                try
                {
                    report.Dictionary.Variables["Cleareance"].Value = Reception.ClearanceType.Name;
                }
                catch { }


                report.Dictionary.Variables["UserName"].Value = HttpContext.Session.GetString("UserName");
                report.Dictionary.Variables["Chief"].Value = Reception.ChiefComplaint;

                SurgeryViewModel surgery = _IDUNIT.surgery.GetSurgeryByReceptionId(ReceptionId);
                report.Dictionary.Variables["SurgererName"].Value = surgery.SurgeryOne.UserName;

                try
                {
                    report.Dictionary.Variables["AntheType"].Value = surgery.AnesthesiologistionType.Name;
                }
                catch { }

                report.Dictionary.Variables["AntheMedicine"].Value = surgery.AnesthesiologistionMedicine;
                try
                {
                    report.Dictionary.Variables["AntheDoctor"].Value = surgery.Anesthesiologist.UserName;
                }
                catch { }

                try
                {
                    report.Dictionary.Variables["SurgeryType"].Value = surgery.Classification.Name;
                }
                catch { }

                report.Dictionary.Variables["SurgeryDetail"].Value = surgery.SurgeryDetail;
                report.Dictionary.Variables["AfterSurgery"].Value = surgery.PostOperativeTreatment;

                ReceptionServiceViewModel PV2 = _IDUNIT.receptionService.GetReceptionOperation(ReceptionId);
                report.Dictionary.Variables["OperationPrice"].Value = PV2.ServiceName;

                IEnumerable<ReceptionTemperatureViewModel> ReceptionTemperatures = _IDUNIT.reception.GetAllReceptionTemperature(ReceptionId);
                report.RegBusinessObject("Temerature", ReceptionTemperatures);

                IEnumerable<ReceptionServiceViewModel> Serums = _IDUNIT.receptionService.GetAllReceptionProducts(ReceptionId, "");
                report.RegBusinessObject("Product", Serums);

                IEnumerable<ReceptionServiceViewModel> Stitch = _IDUNIT.receptionService.GetReceptionSpecificServicesByReceptionId(ReceptionId, "Stitch");
                report.RegBusinessObject("Stitch", Stitch);

                IEnumerable<ReceptionServiceViewModel> Other = _IDUNIT.receptionService.GetReceptionSpecificServicesByReceptionId(ReceptionId, "Other");
                report.RegBusinessObject("Other", Other);

                IEnumerable<ReceptionServiceViewModel> Transfusion = _IDUNIT.receptionService.GetReceptionSpecificServicesByReceptionId(ReceptionId, "Transfusion");
                report.RegBusinessObject("Transfiusion", Transfusion);

                IEnumerable<PatientVariablesValueViewModel> PV = _IDUNIT.patientVariablesValue.GetAllPatientSpeceficVariable(ReceptionId, "Patient Progress");
                report.RegBusinessObject("FollowUp", PV);

                report.Dictionary.Variables["BabyReception"].Value = _localizer["BabyReception"];
                report.Dictionary.Variables["BabyName"].Value = _localizer["BabyName"];
                report.Dictionary.Variables["ReceptionDoctor"].Value = _localizer["ReceptionDoctor"];
                report.Dictionary.Variables["ReceivedDate"].Value = _localizer["ReceivedDate"];
                report.Dictionary.Variables["Weight"].Value = _localizer["Weight"];
                report.Dictionary.Variables["GenderName"].Value = _localizer["Gender"];
                report.Dictionary.Variables["DateOfBirth"].Value = _localizer["DateOfBirth"];

                IEnumerable<NewBornBabiesReportViewModel> children = _IDUNIT.child.GetAllHospitalPatientChildrenReport(ReceptionId);
                report.RegBusinessObject("Children", children);

                IEnumerable<PatientVariablesValueViewModel> all = _IDUNIT.patientVariablesValue.GetAllReceptionVariable(ReceptionId);
                try
                {
                    report.Dictionary.Variables["DrugHistory"].Value = all.FirstOrDefault(x => x.PatientVariableVariableName == "Drug History").Value;
                }
                catch { }

                try
                {
                    report.Dictionary.Variables["PastMedical"].Value = all.FirstOrDefault(x => x.PatientVariableVariableName == "Past Medical And Surgical History").Value;
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


        public async Task<IActionResult> PrintHospitalPatientReport(Guid ReceptionId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "HospitalPatient");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                StiReport report = await PatientReceptionAnalysisReport(ReceptionId);

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


        private StiReport AllHospitalPatientReport(HospitalPatientReportViewModel reportViewModel)
        {
            try
            {
                StiReport report = new StiReport();
                string path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "AllHospitalPatientReport.mrt");
                report.Load(path);

                ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(reportViewModel.section.Id);

                string clinicSectionName = cs.Name;

                if (reportViewModel.periodId != (int)Periods.FromDateToDate)
                {
                    var FromDate = DateTime.Now;
                    var ToDate = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref FromDate, ref ToDate, reportViewModel.periodId);

                    reportViewModel.FromDate = FromDate;
                    reportViewModel.ToDate = ToDate;
                }

                ShowHospitalPatientReportResultViewModel reportResult = _IDUNIT.reception.AllHospitalPatientReport(reportViewModel);


                report.Dictionary.Variables["vTitle"].Value = clinicSectionName;
                report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToShortDateString();
                report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToShortDateString();
                report.Dictionary.Variables["Title1"].Value = _localizer["Title1"];
                report.Dictionary.Variables["Title2"].Value = _localizer["Title2"];

                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["Age"].Value = _localizer["Age"];
                report.Dictionary.Variables["Kind"].Value = _localizer["OperationType"];
                report.Dictionary.Variables["DoctorName"].Value = _localizer["SurgeryDoctorName"];
                report.Dictionary.Variables["RoomId"].Value = _localizer["RoomId"];
                report.Dictionary.Variables["SurgeryDate"].Value = _localizer["SurgeryDate"];
                report.Dictionary.Variables["ReceptionDate"].Value = _localizer["ReceptionDate"];
                report.Dictionary.Variables["ExitDate"].Value = _localizer["ExitDate"];
                report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                report.Dictionary.Variables["TotalSurgery"].Value = _localizer["TotalSurgery"];
                report.Dictionary.Variables["TotalDischarge"].Value = _localizer["TotalDischarge"];
                report.Dictionary.Variables["TotalNotDischarge"].Value = _localizer["TotalNotDischarge"];
                report.Dictionary.Variables["VTotalSurgery"].Value = reportResult.TotalSurgery;
                report.Dictionary.Variables["VTotalDischarge"].Value = reportResult.TotalDischarge;
                report.Dictionary.Variables["VTotalNotDischarge"].Value = reportResult.TotalNotDischarge;


                report.RegBusinessObject("Receptions", reportResult.CustomPatients);
                report.RegBusinessObject("RemReceptions", reportResult.RemPatients);
                report.RegBusinessObject("SurgeryCount", reportResult.SurgeryTypeCount);
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


        public ActionResult PrintAllHospitalPatientReport(HospitalPatientReportViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "HospitalPatient");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string[] from = reportViewModel.dateFrom.Split('-');
                string[] to = reportViewModel.dateTo.Split('-');

                reportViewModel.FromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                reportViewModel.ToDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);


                string font1 = Path.Combine(HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = AllHospitalPatientReport(reportViewModel);

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

        public ActionResult ShowReport()
        {
            try
            {
                ViewBag.AccessPrintFundReportFromSections = _IDUNIT.subSystem.CheckUserAccess("Print", "FundReportFromSections");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/HospitalPatients/wpPatientsLabReportForm.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        private StiReport PatientToAnotherSectionReport(HospitalPatientReportViewModel reportViewModel)
        {
            try
            {
                StiReport report = new StiReport();
                string path = Path.Combine(HostingEnvironment.WebRootPath, "Content", "Reports", "PatientToAnotherSectionReport.mrt");
                report.Load(path);

                ShowPatientToAnotherSectionReportResultViewModel reportResult = _IDUNIT.receptionClinicSection.GetPatientToAnotherSectionReport(reportViewModel.section.Id, reportViewModel.FromDate, reportViewModel.ToDate);

                report.Dictionary.Variables["vTitle"].Value = _localizer["PatientToAnotherSectionReport"];
                report.Dictionary.Variables["ReportDate"].Value = _localizer["Date"] + " " + _localizer["Report"];
                report.Dictionary.Variables["vReportDate"].Value = DateTime.Now.ToShortDateString();
                report.Dictionary.Variables["DateFrom"].Value = _localizer["DateFrom"];
                report.Dictionary.Variables["DateTo"].Value = _localizer["DateTo"];
                report.Dictionary.Variables["vDateFrom"].Value = reportViewModel.FromDate.ToShortDateString();
                report.Dictionary.Variables["vDateTo"].Value = reportViewModel.ToDate.ToShortDateString();

                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["RceptionDate"].Value = _localizer["RceptionDate"];
                report.Dictionary.Variables["Section"].Value = _localizer["Section"];
                report.Dictionary.Variables["Amount"].Value = _localizer["Amount"];
                report.Dictionary.Variables["Total"].Value = _localizer["Total"];
                report.Dictionary.Variables["Analysis"].Value = _localizer["Analysis"];
                report.Dictionary.Variables["AnalysisCount"].Value = _localizer["Number"];

                report.Dictionary.Variables["TotalDinar"].Value = reportResult.Total;
                report.RegBusinessObject("Human", reportResult.Human);
                report.RegBusinessObject("Sections", reportResult.Section);

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


        public ActionResult PrintPatientToAnotherSectionReport(HospitalPatientReportViewModel reportViewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Print", "FundReportFromSections");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string[] from = reportViewModel.dateFrom.Split('-');
                string[] to = reportViewModel.dateTo.Split('-');

                reportViewModel.FromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                reportViewModel.ToDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);


                string font1 = Path.Combine(HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = PatientToAnotherSectionReport(reportViewModel);

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
