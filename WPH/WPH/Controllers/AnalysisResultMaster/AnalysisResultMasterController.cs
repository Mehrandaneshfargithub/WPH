using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report;
using System.IO;
using Stimulsoft.Report.Export;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisResultMaster;
using WPH.Models.CustomDataModels.Clinic;
using Microsoft.AspNetCore.Http;
using WPH.Models.CustomDataModels.AnalysisResult;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using QRCoder;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.PatientReceptionAnalysis;
using Microsoft.Extensions.Logging;
using WPH.Models.AnalysisResultMaster;
using System.Xml.Linq;
using Stimulsoft.Report.Dictionary;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Globalization;
//using QRCoder;

namespace WPH.Controllers.AnalysisResultMaster
{
    public class AnalysisResultMasterController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<AnalysisResultMasterController> _logger;
        private readonly IConfiguration _configuration;

        public AnalysisResultMasterController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<AnalysisResultMasterController> logger, IConfiguration configuration)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            HostingEnvironment = hostingEnvironment;
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<ActionResult> Form()
        {
            try
            {
                try
                {
                    Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                    Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                    _IDUNIT.analysisResultMaster.GetModalsViewBags(ViewBag);
                    BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                    IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                    baseInfosAndPeriods.periods = periods;
                    ViewBag.FromToId = (int)Periods.FromDateToDate;
                    IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);
                    baseInfosAndPeriods.sections = clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar", "UseOnlineResult");
                    try
                    {
                        ViewBag.useDollar = sval?.FirstOrDefault(p => p.ShowSName == "UseDollar")?.SValue?.ToLower() ?? "false";
                    }
                    catch { ViewBag.useDollar = "false"; }
                    try
                    {
                        ViewBag.UseOnlineResult = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseOnlineResult")?.SValue ?? "false");
                    }
                    catch { ViewBag.UseOnlineResult = false; }
                    var access = _IDUNIT.subSystem.GetUserSubSystemAccess("AnalysisResult");
                    ViewBag.AccessEditAnalysisResult = access.Any(p => p.AccessName == "Edit");
                    ViewBag.AccessPrintAnalysisResult = access.Any(p => p.AccessName == "Print");
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultMaster/wpAnalysisResultMasterForm.cshtml", baseInfosAndPeriods);
                }
                catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public async Task<ActionResult> MasterGrid()
        {
            try
            {
                try
                {
                    Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                    try
                    {
                        var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();
                        ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                    }
                    catch { ViewBag.useDollar = "false"; }
                    var access = _IDUNIT.subSystem.GetUserSubSystemAccess("AnalysisResult");
                    ViewBag.AccessEditAnalysisResult = access.Any(p => p.AccessName == "Edit");
                    ViewBag.AccessPrintAnalysisResult = access.Any(p => p.AccessName == "Print");
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultMaster/dgAnalysisResultMasterGrid.cshtml");
                }
                catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        public async Task<ActionResult> EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisResult");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                AnalysisResultMasterViewModel AnalysisResultMaster = _IDUNIT.analysisResultMaster.GetAnalysisResultMasterByIdForAnalysisResult(Id);


                string sectionName = HttpContext.Session.GetString("ClinicSectionName");

                try
                {
                    var svalT = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseTemplate").FirstOrDefault();
                    ViewBag.useTemplate = (svalT.SValue == null) ? "false" : svalT.SValue.ToLower();
                }
                catch { ViewBag.useTemplate = "false"; }


                if (AnalysisResultMaster.PatientReception.ClinicSection.ClinicSectionTypeName == "Radiology")
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailFormForRadiology.cshtml", AnalysisResultMaster);
                }
                else
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailFormNew.cshtml", AnalysisResultMaster);
                }

            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        public ActionResult GetAnalysisResult(string InvoiceNum)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sectionTypeId = int.Parse(HttpContext.Session.GetString("SectionTypeId"));
            try
            {
                AnalysisResultMasterViewModel AnalysisResultMaster = _IDUNIT.analysisResultMaster.GetAnalysisResultMasterByInvoiceNum(clinicSectionId, InvoiceNum);
                List<ClinicSectionSettingViewModel> css = _IDUNIT.clinicSection.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
                AnalysisResultMaster.LastInvoiceNum = (Convert.ToInt32(_IDUNIT.patientReception.GetLatestReceptionInvoiceNum(clinicSectionId)) - 1).ToString();
                AnalysisResultMaster.FirstInvoiceNum = _IDUNIT.patientReception.GetTodaysFirstReceptionInvoiceNum(clinicSectionId, DateTime.Now);
                if (AnalysisResultMaster.FirstInvoiceNum == "")
                {
                    AnalysisResultMaster.FirstInvoiceNum = AnalysisResultMaster.PatientReception.ReceptionNum;
                }

                _IDUNIT.analysisResult.GetModalsViewBags(ViewBag);

                foreach (var ana in AnalysisResultMaster.PatientReception.PatientReceptionAnalyses)
                {
                    if (ana.GroupAnalysis != null)
                    {
                        AnalysisResultMaster.AllAnalysisName = AnalysisResultMaster.AllAnalysisName + " - " + ana.GroupAnalysis.Name;
                    }
                    if (ana.Analysis != null)
                    {
                        AnalysisResultMaster.AllAnalysisName = AnalysisResultMaster.AllAnalysisName + " - " + ana.Analysis.Name;
                    }
                    if (ana.AnalysisItem != null)
                    {
                        AnalysisResultMaster.AllAnalysisName = AnalysisResultMaster.AllAnalysisName + " - " + ana.AnalysisItem.Name;
                    }

                }

                string sectionName = HttpContext.Session.GetString("ClinicSectionName");

                if (sectionName == "Radiology")
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailFormForRadiology.cshtml", AnalysisResultMaster);
                }
                else
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailForm.cshtml", AnalysisResultMaster);
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }



        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');
                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);
                IEnumerable<AnalysisResultMasterGridViewModel> AllAnalysisResultMaster = _IDUNIT.analysisResultMaster.GetAllAnalysisResultMasterByUserId(section.Id, periodId, fromDate, toDate);
                return Json(AllAnalysisResultMaster.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        public ActionResult GetAllPatientAnalysisResult([DataSourceRequest] DataSourceRequest request, Guid PatientId)
        {
            try
            {
                IEnumerable<AnalysisResultMasterGridViewModel> AllAnalysisResultMaster = _IDUNIT.analysisResultMaster.GetAnalysisResultByPatientId(PatientId);
                return Json(AllAnalysisResultMaster.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        public ActionResult GetAllPatientReceptionAnalysis(Guid ReceptionId)
        {
            try
            {
                IEnumerable<PatientReceptionAnalysisViewModel> AllAnalysisResultAnalysis = _IDUNIT.patientReceptionAnalysis.GetAllPatientReceptionAnalysis(ReceptionId);
                return Json(AllAnalysisResultAnalysis);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public ActionResult GetAnalysisResultByPatientId(Guid PatientId)
        {
            try
            {
                IEnumerable<AnalysisResultMasterGridViewModel> AllAnalysisResultMaster = _IDUNIT.analysisResultMaster.GetAnalysisResultByPatientId(PatientId);
                return Json(AllAnalysisResultMaster);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(AnalysisResultMasterViewModel AnalysisResultMaster)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "AnalysisResult");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                if (AnalysisResultMaster.AnalysisResults != null)
                {
                    foreach (var result in AnalysisResultMaster.AnalysisResults)
                    {
                        result.CreatedDate = DateTime.Now;
                        result.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                        result.Guid = Guid.NewGuid();
                    }
                    AnalysisResultMaster.ModifiedDate = DateTime.Now;
                    AnalysisResultMaster.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                    _IDUNIT.analysisResultMaster.UpdateAnalysisResultMaster(AnalysisResultMaster);
                    return Json(1);
                }
                else
                {
                    AnalysisResultMaster.ModifiedDate = DateTime.Now;
                    AnalysisResultMaster.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                    _IDUNIT.analysisResultMaster.UpdateAnalysisResultMaster(AnalysisResultMaster);
                    return Json(1);
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }



        }

        public JsonResult IncreasePrintNumber(Guid AnalysisResultMasterId)
        {
            try
            {
                _IDUNIT.analysisResultMaster.IncreasePrintNumber(AnalysisResultMasterId);
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }


        }

        bool GUEAnalysis = false;
        bool GSEAnalysis = false;
        private async Task<StiReport> AnalysisResultReport(Guid AnalysisResultMasterId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            StiReport report = new StiReport();

            report.Load(Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "AnalysisResultReportNew.mrt"));

            AnalysisResultMasterViewModel master = _IDUNIT.analysisResultMaster.GetAnalysisResultMasterForAnalysisResultReport(AnalysisResultMasterId);

            master.AnalysisResults = master.AnalysisResults.OrderBy(a => a.AnalysisId).ThenBy(a => a.AnalysisItem.Priority).ToList();



            report.Dictionary.Variables["PatientId"].Value = master.PatientReception.PatientId.GetValueOrDefault().ToString();
            report.Dictionary.Variables["ReceptionId"].Value = master.ReceptionId.ToString();
            report.Dictionary.Variables["PhoneNumber"].Value = master.PatientReception.Patient.User.PhoneNumber;


            report.Dictionary.Variables["vReceptionDate"].Value = master.PatientReception.ReceptionDate == null ? "" : master.PatientReception.ReceptionDate.Value.ToString("dd/MM/yyyy HH:mm");
            report.Dictionary.Variables["ReceptionDate"].Value = _localizer["Date"] + " " + _localizer["Reception"];

            report.Dictionary.Variables["vResultDate"].Value = master.ModifiedDate == null ? "" : master.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
            report.Dictionary.Variables["ResultDate"].Value = _localizer["Date"] + " " + _localizer["Result"];
            try
            {
                report.Dictionary.Variables["vDoctorName"].Value = master.PatientReception.Doctor.UserName ?? "";
            }
            catch
            {
                report.Dictionary.Variables["vDoctorName"].Value = "";
            }
            report.Dictionary.Variables["DoctorName"].Value = _localizer["Doctor"];

            report.Dictionary.Variables["vPatientName"].Value = master.PatientReception.Patient.User.Name ?? "";
            report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];

            report.Dictionary.Variables["vAge"].Value = master.PatientReception.Patient.Age.Value.ToString() ?? "";
            report.Dictionary.Variables["Age"].Value = _localizer["Age"];

            report.Dictionary.Variables["vGender"].Value = master.PatientReception.Patient.UserGenderName.ToString() ?? "";
            report.Dictionary.Variables["Gender"].Value = _localizer["Gender"];

            var path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportHeaderBanner");
            string rootPath = HostingEnvironment.WebRootPath;

            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + path));
                report.Dictionary.Variables["banner"].ValueObject = (Image)banner;
            }
            catch { }

            var footer_path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportFooterBanner");
            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + footer_path));
                report.Dictionary.Variables["FooterBanner"].ValueObject = (Image)banner;
            }
            catch { }

            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(master.PatientReception.PatientId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode QrCode = new QRCoder.QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(5);
            Image im = (Image)QrBitmap;
            report.Dictionary.Variables["Barcode"].ValueObject = im;

            List<AnalysisResultViewModel> allAnalysisResult = new List<AnalysisResultViewModel>();
            List<AnalysisResultViewModel> GUE = new List<AnalysisResultViewModel>();
            List<AnalysisResultViewModel> GSE = new List<AnalysisResultViewModel>();
            int i = 1;
            var analysisname = "";
            var allAnalysisName = "";


            foreach (var result in master.AnalysisResults)
            {
                if (result.Analysis != null)
                {
                    if (analysisname != result.Analysis.Name)
                    {
                        if (result.Analysis.Name == "GUE")
                        {
                            GUEAnalysis = true;
                            if (result.AnalysisItem.Unit != null)
                            {
                                GUE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                GUE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                        else if (result.Analysis.Name == "GSE")
                        {
                            GSEAnalysis = true;
                            if (result.AnalysisItem.Unit != null)
                            {
                                GSE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                GSE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                        else
                        {
                            allAnalysisName += analysisname + " & ";
                            if (result.AnalysisItem.Unit != null)
                            {
                                allAnalysisResult.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                allAnalysisResult.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                    }
                    else
                    {
                        if (result.Analysis.Name == "GUE")
                        {
                            GUEAnalysis = true;
                            if (result.AnalysisItem.Unit != null)
                            {
                                GUE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                GUE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                        else if (result.Analysis.Name == "GSE")
                        {
                            GSEAnalysis = true;
                            if (result.AnalysisItem.Unit != null)
                            {
                                GSE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                GSE.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                        else
                        {
                            if (result.AnalysisItem.Unit != null)
                            {
                                allAnalysisResult.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Unit = result.AnalysisItem.Unit.Name,
                                    Value = result.Value,
                                    Category = result.Analysis.Name
                                });
                            }
                            else
                            {
                                allAnalysisResult.Add(new AnalysisResultViewModel()
                                {
                                    Id = i,
                                    Name = result.AnalysisItem.Name,
                                    NormalValue = result.AnalysisItem.NormalValues,
                                    Value = result.Value,
                                    Unit = "",
                                    Category = result.Analysis.Name
                                });
                            }
                        }
                    }
                }
                else
                {
                    if (result.AnalysisItem.Unit != null)
                    {
                        allAnalysisResult.Add(new AnalysisResultViewModel()
                        {
                            Id = i,
                            Name = result.AnalysisItem.Name,
                            NormalValue = result.AnalysisItem.NormalValues,
                            Unit = result.AnalysisItem.Unit.Name,
                            Value = result.Value,
                            Category = "Other Analysis"
                        });
                    }
                    else
                    {
                        allAnalysisResult.Add(new AnalysisResultViewModel()
                        {
                            Id = i,
                            Name = result.AnalysisItem.Name,
                            NormalValue = result.AnalysisItem.NormalValues,
                            Value = result.Value,
                            Unit = "",
                            Category = "Other Analysis"
                        });
                    }
                }

                i++;

            }

            try
            {
                var newAllAnalysisName = allAnalysisName.Remove(allAnalysisName.Length - 2, 2);
                report.Dictionary.Variables["vAllAnalysis"].Value = newAllAnalysisName;
            }
            catch
            {
                report.Dictionary.Variables["vAllAnalysis"].Value = "";
            }

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "LabAddress", "LabName", "LabDesc", "UseTemplate");
            try
            {
                report.Dictionary.Variables["Address"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabAddress")?.SValue;
            }
            catch { }

            try
            {
                report.Dictionary.Variables["LabName"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabName")?.SValue;
            }
            catch { }

            try
            {
                report.Dictionary.Variables["LabDesc"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabDesc")?.SValue;
            }
            catch { }

            if (allAnalysisResult.Any())
            {
                report.RegBusinessObject("AnalysisResult", allAnalysisResult);
                report.RegBusinessObject("GUE", GUE);
                report.RegBusinessObject("GSE", GSE);
            }
            else if (GUE.Any())
            {
                report.RegBusinessObject("AnalysisResult", GUE);
                GUEAnalysis = false;
                report.RegBusinessObject("GSE", GSE);
            }
            else if (GSE.Any())
            {
                report.RegBusinessObject("AnalysisResult", GSE);
                GSEAnalysis = false;
            }


            string useTem = "false";
            try
            {
                useTem = sval?.FirstOrDefault(p => p.ShowSName == "UseTemplate")?.SValue;
            }
            catch { }

            AnalysisResultViewModel allAnalysisResult2 = new AnalysisResultViewModel();


            if (useTem?.ToUpper() == "TRUE" && master.Description != null)
            {
                string notSupportedValue = master.Description;

                notSupportedValue = notSupportedValue.Replace("xx-small", "8");
                notSupportedValue = notSupportedValue.Replace("x-small", "10");
                notSupportedValue = notSupportedValue.Replace("small", "12");
                notSupportedValue = notSupportedValue.Replace("medium", "14");
                notSupportedValue = notSupportedValue.Replace("xx-large", "36");
                notSupportedValue = notSupportedValue.Replace("x-large", "24");
                notSupportedValue = notSupportedValue.Replace("large", "18");

                string[] allP = notSupportedValue.Split("<p>");

                allP = allP.Where(val => val != "").ToArray();
                string newAllP = "";
                foreach (var p in allP)
                {
                    string newP = "<p>";
                    if (p.Contains("<span"))
                    {
                        string[] allSpan = p.Split("<span");
                        allSpan = allSpan.Where(val => val != "").ToArray();

                        foreach (var span in allSpan)
                        {
                            if (span.Contains("font-size"))
                            {
                                string newspan = span.Replace("font-size:", "font-size=");
                                int index = newspan.IndexOf("font-size=");
                                string mande = newspan.Substring(index);
                                int indexsimicolon = mande.IndexOf(";");
                                string fontSize = mande.Substring(0, indexsimicolon);
                                string newTag = "<" + fontSize + ">";
                                newspan = newspan.Replace(fontSize + ";", "");
                                newspan = newTag + "<span" + newspan + "</font-size>";
                                newP += newspan;
                            }
                            else
                            {
                                newP = newP + "<span" + span;
                            }

                        }
                        if (!newP.Contains("</p>"))
                        {
                            newP += "</p>";
                        }
                    }
                    else
                    {
                        newP += p;
                    }
                    newAllP += newP;
                }


                while (newAllP.Contains("text-decoration"))
                {
                    int indexdecor = newAllP.IndexOf("text-decoration");
                    string mande = newAllP.Substring(indexdecor);
                    int indexsimicolon = mande.IndexOf(";");
                    int indexaval = mande.IndexOf(">");
                    int indexakhr = mande.IndexOf("<");
                    int aval = indexdecor + indexaval + 1;


                    int akhar = indexdecor + indexakhr;
                    newAllP = newAllP.Substring(0, akhar) + "</u>" + newAllP.Substring(akhar);
                    newAllP = newAllP.Substring(0, aval) + "<u>" + newAllP.Substring(aval);


                    newAllP = newAllP.Remove(indexdecor, indexsimicolon + 1);
                }

                report.Dictionary.Variables["vResult"].Value = newAllP;
                allAnalysisResult2.Value = newAllP;
            }
            else
            {
                report.Dictionary.Variables["vResult"].Value = master.Description;
            }

            try
            {
                string save_path = Path.Combine(rootPath + "\\chart\\");

                if (!Directory.Exists(save_path))
                    Directory.CreateDirectory(save_path);

                save_path += $"{userId}_{clinicSectionId}.xml";

                var chart = _IDUNIT.analysisResultMaster.GetAnalysisItemForChart(AnalysisResultMasterId);
                var analysis_name = chart.Select(p =>
                                    new XElement("AnalysisName",
                                                  new XElement("Name", p.AnalysisName)));
                var analysis_item = chart.SelectMany(x => x.History).Select(p =>
                                                    new XElement("AnalysisItem",
                                                                  new XElement("Name", p.AnalysisName),
                                                                  new XElement("AnalysisArgument", p.AnalysisArgument),
                                                                  new XElement("AnalysisValue", p.AnalysisValue)
                                                    ));

                var xmlSave = new XElement("History", analysis_item, analysis_name);
                xmlSave.Save(save_path);

                var dbdemo = (StiXmlDatabase)report.Dictionary.Databases["demo"];
                dbdemo.PathSchema = "";
                dbdemo.PathData = save_path;
            }
            catch (Exception) { }

            allAnalysisResult2.Value = master.Description;
            report.RegBusinessObject("RadiologyResult", allAnalysisResult2);

            return report;
        }


        public async Task<ActionResult> PrintAnalysisResultReport(Guid AnalysisResultMasterId)
        {
            //var access = _IDUNIT.subSystem.CheckUserAccess("Print", "AnalysisResult");
            //var accessHospitalAnalysisResult = _IDUNIT.subSystem.CheckUserAccess("Print", "HospitalAnalysisResult"); 
            //var accessHospitalAnalysisResult1 = _IDUNIT.subSystem.CheckUserAccess("Edit", "HospitalAnalysisResult"); 
            //if (!access)
            //{
            //    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
            //                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
            //                           "\t Message: AccessDenied");
            //    return Json("");
            //}

            try
            {
                try
                {
                    string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                    string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                    Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                    Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                    StiReport report = await AnalysisResultReport(AnalysisResultMasterId);
                    if (!GSEAnalysis)
                    {
                        report.Pages.Remove(report.Pages["Page3"]);
                    }
                    if (!GUEAnalysis)
                    {
                        report.Pages.Remove(report.Pages["Page2"]);
                    }
                    report.Render();
                    List<byte[]> allb = new List<byte[]>();
                    try
                    {
                        for (int i = 0; i < report.RenderedPages.Count; i++)
                        {
                            MemoryStream stream = new MemoryStream();
                            report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings() 
                            { 
                                PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                                MultipleFiles = true,
                                ImageResolution = 200,
                                ImageFormat = StiImageFormat.Color
                            });
                            allb.Add(stream.ToArray());
                        }
                    }
                    catch (Exception e) { }
                    return Json(new
                    {
                        allb
                    });
                }
                catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
            }
            catch (Exception)
            {
                return Json("");
            }
        }


        private async Task<StiReport> AnalysisResultReportRadiology(Guid AnalysisResultMasterId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            StiReport report = new StiReport();

            report.Load(Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "AnalysisResultReportForRadiology.mrt"));

            AnalysisResultMasterViewModel master = _IDUNIT.analysisResultMaster.GetAnalysisResultMasterForAnalysisResultReport(AnalysisResultMasterId);

            CultureInfo cultures = new CultureInfo("en-US");
            report.Dictionary.Variables["vReceptionDate"].Value = master.PatientReception.ReceptionDate.GetValueOrDefault().ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["ReceptionDate"].Value = _localizer["Date"] + " " + _localizer["Reception"];

            report.Dictionary.Variables["vResultDate"].Value = master.ModifiedDate.GetValueOrDefault().ToString("dd/MM/yyyy", cultures);
            report.Dictionary.Variables["ResultDate"].Value = _localizer["Date"] + " " + _localizer["Result"];

            report.Dictionary.Variables["PatientId"].Value = master.PatientReception.PatientId.GetValueOrDefault().ToString();
            report.Dictionary.Variables["ReceptionId"].Value = master.ReceptionId.ToString();
            report.Dictionary.Variables["PhoneNumber"].Value = master.PatientReception.Patient.User.PhoneNumber;

            try
            {
                report.Dictionary.Variables["vDoctorName"].Value = master.PatientReception.Doctor.UserName ?? "";
            }
            catch
            {
                report.Dictionary.Variables["vDoctorName"].Value = "";
            }
            report.Dictionary.Variables["DoctorName"].Value = _localizer["Doctor"];

            report.Dictionary.Variables["vPatientName"].Value = master.PatientReception.Patient.User.Name ?? "";
            report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];

            report.Dictionary.Variables["vAge"].Value = master.PatientReception.Patient.Age.Value.ToString() ?? "";
            report.Dictionary.Variables["Age"].Value = _localizer["Age"];

            report.Dictionary.Variables["vGender"].Value = master.PatientReception.Patient.UserGenderName.ToString() ?? "";
            report.Dictionary.Variables["Gender"].Value = _localizer["Gender"];


            var path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportHeaderBanner");
            string rootPath = HostingEnvironment.WebRootPath;

            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + path));
                report.Dictionary.Variables["banner"].ValueObject = (Image)banner;
            }
            catch { }

            var footer_path = _IDUNIT.clinicSection.GetBanner(clinicSectionId, "ReportFooterBanner");
            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + footer_path));
                report.Dictionary.Variables["FooterBanner"].ValueObject = (Image)banner;
            }
            catch { }

            string notSupportedValue = master.AnalysisResults.FirstOrDefault()?.Value ?? "";

            notSupportedValue = notSupportedValue.Replace("xx-small", "8");
            notSupportedValue = notSupportedValue.Replace("x-small", "10");
            notSupportedValue = notSupportedValue.Replace("small", "12");
            notSupportedValue = notSupportedValue.Replace("medium", "14");
            notSupportedValue = notSupportedValue.Replace("xx-large", "36");
            notSupportedValue = notSupportedValue.Replace("x-large", "24");
            notSupportedValue = notSupportedValue.Replace("large", "18");




            string[] allP = notSupportedValue.Split("<p>");

            allP = allP.Where(val => val != "").ToArray();
            string newAllP = "";
            foreach (var p in allP)
            {
                string newP = "<p>";
                if (p.Contains("<span"))
                {


                    string[] allSpan = p.Split("<span");
                    allSpan = allSpan.Where(val => val != "").ToArray();

                    foreach (var span in allSpan)
                    {

                        if (span.Contains("font-size"))
                        {
                            string newspan = span.Replace("font-size:", "font-size=");
                            int index = newspan.IndexOf("font-size=");
                            string mande = newspan.Substring(index);
                            int indexsimicolon = mande.IndexOf(";");
                            string fontSize = mande.Substring(0, indexsimicolon);
                            string newTag = "<" + fontSize + ">";
                            newspan = newspan.Replace(fontSize + ";", "");
                            newspan = newTag + "<span" + newspan + "</font-size>";

                            newP += newspan;
                        }
                        else
                        {
                            newP = newP + "<span" + span;
                        }

                    }
                    if (!newP.Contains("</p>"))
                    {
                        newP += "</p>";
                    }
                }
                else
                {
                    newP += p;
                }
                newAllP += newP;
            }


            while (newAllP.Contains("text-decoration"))
            {
                int indexdecor = newAllP.IndexOf("text-decoration");
                string mande = newAllP.Substring(indexdecor);
                int indexsimicolon = mande.IndexOf(";");
                int indexaval = mande.IndexOf(">");
                int indexakhr = mande.IndexOf("<");
                int aval = indexdecor + indexaval + 1;


                int akhar = indexdecor + indexakhr;
                newAllP = newAllP.Substring(0, akhar) + "</u>" + newAllP.Substring(akhar);
                newAllP = newAllP.Substring(0, aval) + "<u>" + newAllP.Substring(aval);


                newAllP = newAllP.Remove(indexdecor, indexsimicolon + 1);
            }


            report.Dictionary.Variables["vResult"].Value = newAllP;
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(master.PatientReception.PatientId.GetValueOrDefault().ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode QrCode = new QRCoder.QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(5);
            Image im = (Image)QrBitmap;
            report.Dictionary.Variables["Barcode"].ValueObject = im;

            AnalysisResultViewModel allAnalysisResult = new AnalysisResultViewModel();


            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "LabAddress", "LabName", "LabDesc");
            try
            {
                report.Dictionary.Variables["Address"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabAddress")?.SValue;
            }
            catch { }

            try
            {
                report.Dictionary.Variables["LabName"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabName")?.SValue;
            }
            catch { }

            try
            {
                report.Dictionary.Variables["LabDesc"].Value = sval?.FirstOrDefault(p => p.ShowSName == "LabDesc")?.SValue;
            }
            catch { }



            allAnalysisResult.Value = master.AnalysisResults.FirstOrDefault()?.Value;
            report.RegBusinessObject("RadiologyResult", allAnalysisResult);

            return report;
        }


        public async Task<ActionResult> PrintAnalysisResultReportRadiology(Guid AnalysisResultMasterId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "AnalysisResult");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }
            try
            {
                try
                {
                    string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                    string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                    Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                    Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                    StiReport report = await AnalysisResultReportRadiology(AnalysisResultMasterId);
                    report.Render();
                    List<byte[]> allb = new List<byte[]>();
                    for (int i = 0; i < report.RenderedPages.Count; i++)
                    {
                        MemoryStream stream = new MemoryStream();
                        report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1), MultipleFiles = true, ImageResolution = 200, ImageFormat = StiImageFormat.Color });
                        allb.Add(stream.ToArray());
                    }
                    return Json(new
                    {
                        allb
                    });
                }
                catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
            }
            catch (Exception)
            {
                return Json("0");
            }
        }


        public async Task<ActionResult> SendResultOnline(Guid AnalysisResultMasterId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "AnalysisResult");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = await AnalysisResultReport(AnalysisResultMasterId);

                if (!GSEAnalysis)
                {
                    report.Pages.Remove(report.Pages["Page3"]);
                }
                if (!GUEAnalysis)
                {
                    report.Pages.Remove(report.Pages["Page2"]);
                }

                report.Render();

                MemoryStream stream = new MemoryStream();

                report.ExportDocument(StiExportFormat.Pdf, stream, new StiPdfExportSettings()
                {
                    ImageResolution = 200,
                    ImageFormat = StiImageFormat.Color
                });

                var inputAsString = Convert.ToBase64String(stream.ToArray());

                if (report.Dictionary.Variables["vPatientName"].Value == null)
                    return Json(_localizer["EmptyPatientName"].Value);
                if (report.Dictionary.Variables["ReceptionId"].Value == null)
                    return Json(_localizer["ReceptionNotSelected"].Value);
                if (report.Dictionary.Variables["PatientId"].Value == null)
                    return Json(_localizer["PatientNotSelected"].Value);
                if (report.Dictionary.Variables["PhoneNumber"].Value == null)
                    return Json(_localizer["ForOnlineResultPhoneNumberIsRequired"].Value);

                using (var client = new HttpClient())
                {
                    try
                    {
                        try
                        {
                            MultipartFormDataContent multiContent = new MultipartFormDataContent
                            {
                                { new StringContent(inputAsString), "formFile" },
                                { new StringContent(report.Dictionary.Variables["vPatientName"].Value), "PatientName" },
                                { new StringContent(report.Dictionary.Variables["ReceptionId"].Value), "ReceptionId" },
                                { new StringContent(report.Dictionary.Variables["PatientId"].Value), "PatientId" },
                                { new StringContent(report.Dictionary.Variables["PhoneNumber"].Value), "PhoneNumber" },
                                { new StringContent(report.Dictionary.Variables["vPatientName"].Value.ToString() + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf"), "FileName" }
                            };
                            string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                            string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);
                            var result = await client.PostAsync(baseurl + "Result/SaveResult", multiContent);
                            result.EnsureSuccessStatusCode();
                            var responseContent = await result.Content.ReadAsStringAsync();
                            _IDUNIT.analysisResultMaster.UpdateAnalysisResultMasterForServerNumByReceptionId(Guid.Parse(report.Dictionary.Variables["ReceptionId"].Value), int.Parse(responseContent), DateTime.Now);
                            return StatusCode((int)result.StatusCode);
                        }
                        catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(_localizer["ERROR_InsertWrong"].Value); }
                    }
                    catch (Exception)
                    {
                        return Json(_localizer["ServerError"].Value); // 500 is generic server error
                    }
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(_localizer["ERROR_InsertWrong"].Value); }

        }


        public async Task<ActionResult> SendRadiologyResultOnline(Guid AnalysisResultMasterId)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Print", "AnalysisResult");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }
            try
            {
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = await AnalysisResultReportRadiology(AnalysisResultMasterId);

                report.Render();

                MemoryStream stream = new MemoryStream();

                report.ExportDocument(StiExportFormat.Pdf, stream, new StiPdfExportSettings()
                {

                    ImageResolution = 200,
                    ImageFormat = StiImageFormat.Color
                });

                var inputAsString = Convert.ToBase64String(stream.ToArray());

                if (report.Dictionary.Variables["vPatientName"].Value == null)
                    return Json(_localizer["EmptyPatientName"].Value);
                if (report.Dictionary.Variables["ReceptionId"].Value == null)
                    return Json(_localizer["ReceptionNotSelected"].Value);
                if (report.Dictionary.Variables["PatientId"].Value == null)
                    return Json(_localizer["PatientNotSelected"].Value);
                if (report.Dictionary.Variables["PhoneNumber"].Value == null)
                    return Json(_localizer["ForOnlineResultPhoneNumberIsRequired"].Value);

                using (var client = new HttpClient())
                {
                    try
                    {
                        try
                        {
                            MultipartFormDataContent multiContent = new MultipartFormDataContent
                            {
                                { new StringContent(inputAsString), "formFile" },
                                { new StringContent(report.Dictionary.Variables["vPatientName"].Value), "PatientName" },
                                { new StringContent(report.Dictionary.Variables["ReceptionId"].Value), "ReceptionId" },
                                { new StringContent(report.Dictionary.Variables["PatientId"].Value), "PatientId" },
                                { new StringContent(report.Dictionary.Variables["PhoneNumber"].Value), "PhoneNumber" },
                                { new StringContent(report.Dictionary.Variables["vPatientName"].Value.ToString() + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf"), "FileName" }
                            };
                            string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                            string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);
                            var result = await client.PostAsync(baseurl + "Result/SaveResult", multiContent);
                            result.EnsureSuccessStatusCode();
                            var responseContent = await result.Content.ReadAsStringAsync();
                            _IDUNIT.analysisResultMaster.UpdateAnalysisResultMasterForServerNumByReceptionId(Guid.Parse(report.Dictionary.Variables["ReceptionId"].Value), int.Parse(responseContent), DateTime.Now);
                            return StatusCode((int)result.StatusCode);
                        }
                        catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(_localizer["ERROR_InsertWrong"].Value); }
                    }
                    catch (Exception)
                    {
                        return Json(_localizer["ServerError"]); // 500 is generic server error
                    }
                }

            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(_localizer["ERROR_InsertWrong"].Value); }
        }
    }
}