using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.Extensions;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisResult;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.Clinic;
using Microsoft.AspNetCore.Http;
using WPH.Models.Reception;
using Microsoft.Extensions.Hosting;
using System.Threading;
using WPH.WorkerServices;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
//using Stimulsoft.Report;

namespace WPH.Controllers.AnalysisResult
{
    [SessionCheck]
    public class AnalysisResultController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<AnalysisResultController> _logger;
        private readonly IConfiguration _configuration;

        public AnalysisResultController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<AnalysisResultController> logger, IConfiguration configuration)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ActionResult> Form()
        {
            try
            {
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.analysisResult.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                baseInfosAndPeriods.periods = periods;
                ViewBag.FromToId = (int)Periods.FromDateToDate;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultForm.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
            
        }

        public async Task<IActionResult> GetResult(string Code, string PhoneNumber)
        {
            HttpClient client = new HttpClient();
            string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");

            try
            {
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);
                var response = client.GetAsync(baseurl + "Result/GetResult?Code=" + Code + "&PhoneNumber=" + PhoneNumber);
                
                if (response.Result.IsSuccessStatusCode)
                {
                    var respond = response.Result.Content.ReadAsStringAsync();
                    if (respond.Result == "NotExist")
                    {
                        return Ok("NotExist");
                    }



                    return File(response.Result.Content.ReadAsStream(), /*response.Result.Content.Headers.ContentType.ToString()*/"APPLICATION/pdf", response.Result.Content.Headers.ContentDisposition.FileName.ToString());
                }
                else
                {
                    return Ok("Error");
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        public async Task<ActionResult> OnlineAnalysisResult()
        {
            try
            {
                
                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResultMaster/wpOnlineAnalysisResult.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }

        }


        public ActionResult AddNewModal()
        {
            try
            {
                AnalysisResultViewModel AnalysisResult = new AnalysisResultViewModel();

                return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/mdAnalysisResultNewModal.cshtml", AnalysisResult);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
            
        }


        public async Task<ActionResult> EditModal(Guid Id)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                ReceptionViewModel PatientReception = _IDUNIT.patientReception.GetPatientReceptionByIdForAnalysisResult(Id);


                _IDUNIT.analysisResult.GetModalsViewBags(ViewBag);

                foreach (var ana in PatientReception.PatientReceptionAnalyses)
                {
                    if (ana.GroupAnalysis != null)
                    {
                        PatientReception.AllAnalysisName = PatientReception + " - " + ana.GroupAnalysis.Name;
                    }
                    if (ana.Analysis != null)
                    {
                        PatientReception.AllAnalysisName = PatientReception + " - " + ana.Analysis.Name;
                    }
                    if (ana.AnalysisItem != null)
                    {
                        PatientReception.AllAnalysisName = PatientReception + " - " + ana.AnalysisItem.Name;
                    }

                }

                var svalT = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseTemplate").FirstOrDefault();
                try
                {
                    ViewBag.useTemplate = svalT?.SValue?.ToLower() ?? "false";
                }
                catch { ViewBag.useTemplate = "false"; }


                string sectionName = HttpContext.Session.GetString("SectionTypeName");

                if (PatientReception.ClinicSection.ClinicSectionTypeName == "Radiology")
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailFormForRadiology.cshtml", PatientReception);
                }
                else
                {
                    return PartialView("/Views/Shared/PartialViews/AppWebForms/AnalysisResult/wpAnalysisResultDetailForm.cshtml", PatientReception);
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }


        public ActionResult GetAllAnalysisResult(Guid AnalysisResultMasterId)
        {
            try
            {
                IEnumerable<AnalysisResultViewModel> AllAnalysisResultAnalysis = _IDUNIT.analysisResult.GetAllAnalysisResult(AnalysisResultMasterId);
                return Json(AllAnalysisResultAnalysis);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(IEnumerable<AnalysisResultViewModel> AnalysisResults)
        {
            try
            {
                if (AnalysisResults.Any())
                {
                    foreach (var result in AnalysisResults)
                    {
                        result.CreatedDate = DateTime.Now;
                        result.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                        result.Guid = Guid.NewGuid();
                    }
                    _IDUNIT.analysisResult.AddAnalysisResultToPatientReception(AnalysisResults);
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.analysisResult.RemoveAnalysisResult(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                            "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                            "\t Message:" + e.Message);
                return Json(0);
            }
        }
    }
}