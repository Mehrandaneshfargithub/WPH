using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Helper;
using WPH.Models.Reception;
using WPH.Models.CustomDataModels.PatientReception;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;
using WPH.Models.CustomDataModels.AnalysisResultMaster;
using Microsoft.Extensions.Logging;

namespace WPH.Controllers.PatientReception
{
    [SessionCheck]
    public class PatientReceptionController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PatientReceptionController> _logger;

        public PatientReceptionController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostEnvironment, IConfiguration configuration, ILogger<PatientReceptionController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _logger = logger;
        }



        public async Task<ActionResult> Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Reception", "AllReception");
                ViewBag.AccessEditReception = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Reception");
                ViewBag.AccessDeleteReception = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Reception");

                var _access = access.Any(p => p.AccessName == "View" && p.SubSystemName == "AllReception");
                if (!_access)
                    return Json("");

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);

                _IDUNIT.patientReception.GetModalsViewBags(ViewBag);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();

                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                IEnumerable<PeriodsViewModel> clearance = _IDUNIT.baseInfo.GetAllClearanceType(_localizer);

                IEnumerable<ClinicSectionViewModel> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", clinicSectionId);


                try
                {
                    var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar").FirstOrDefault();

                    ViewBag.useDollar = (sval.SValue == null) ? "false" : sval.SValue.ToLower();
                }
                catch { ViewBag.useDollar = "false"; }


                baseInfosAndPeriods.periods = periods;
                baseInfosAndPeriods.sections = clinicsections.Select(section => new SectionViewModel { Id = section.Guid, Name = section.Name }).ToList();
                baseInfosAndPeriods.clearances = clearance;

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                ViewBag.NotLab = HttpContext.Session.GetString("SectionTypeName")?.ToLower();
                ViewBag.HospitalPatient = false;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReception/wpPatientReceptionForm.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddOrUpdate(ReceptionViewModel patientReception)
        {

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Guid ReceptionId = Guid.Empty;
            try
            {
                var sval =  _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseOnlineResult", "AutoPay");
                patientReception.AutoPay = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "AutoPay")?.SValue ?? "false");
                bool UseOnlineResult = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseOnlineResult")?.SValue ?? "false"); //   sval.SValue != null && bool.Parse(sval.SValue.ToLower());
                if (UseOnlineResult)
                {

                    Ping myPing = new Ping();
                    String host = "google.com";
                    byte[] buffer = new byte[32];
                    int timeout = 3000;
                    PingOptions pingOptions = new PingOptions();

                    PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);

                    if (reply.Status == IPStatus.Success)
                    {
                        try
                        {
                            patientReception.Patient.DateOfBirth = new DateTime(Convert.ToInt32(patientReception.Patient.DateOfBirthYear), Convert.ToInt32(patientReception.Patient.DateOfBirthMonth),
                            Convert.ToInt32(patientReception.Patient.DateOfBirthDay), 0, 0, 0);

                            Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                            Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));

                            patientReception.ClinicId = ClinicId;
                            patientReception.UserId = UserId;
                            patientReception.OrginalClinicSectionId = clinicSectionId;
                            patientReception.ClinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                            ReceptionId = _IDUNIT.patientReception.AddOrUpdate(patientReception);
                        }
                        catch (Exception e)
                        {
                            var message = "InternalError";
                            var receptionId = ReceptionId;
                            return Json(new { message }); // 500 is generic server error
                        }

                        using var client = new HttpClient();
                        try
                        {

                            MultipartFormDataContent multiContent = new MultipartFormDataContent();

                            ReceptionViewModel recep = _IDUNIT.reception.GetReception(ReceptionId);

                            multiContent.Add(new StringContent(recep.Patient.UserName), "PatientName");
                            multiContent.Add(new StringContent(ReceptionId.ToString()), "ReceptionId");
                            multiContent.Add(new StringContent(recep.PatientId.ToString()), "PatientId");
                            multiContent.Add(new StringContent(recep.Patient.User.PhoneNumber), "PhoneNumber");
                            string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                            string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);
                            var result = await client.PostAsync(baseurl + "Result/SetResult", multiContent);

                            result.EnsureSuccessStatusCode();
                            var responseContent = await result.Content.ReadAsStringAsync();
                            _IDUNIT.analysisResultMaster.UpdateAnalysisResultMasterForServerNumByReceptionId(ReceptionId, int.Parse(responseContent),null);
                            return Json(ReceptionId); //201 Created the request has been fulfilled, resulting in the creation of a new resource.

                        }
                        catch (Exception e)
                        {
                            var message = "ServerError";
                            var receptionId = ReceptionId;
                            return Json(new { message, receptionId }); // 500 is generic server error
                        }
                    }
                    else
                    {
                        return Json("NoInernetConnection");
                    }


                }
                else
                {
                    patientReception.Patient.DateOfBirth = new DateTime(Convert.ToInt32(patientReception.Patient.DateOfBirthYear), Convert.ToInt32(patientReception.Patient.DateOfBirthMonth),
                    Convert.ToInt32(patientReception.Patient.DateOfBirthDay), 0, 0, 0);

                    Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));

                    patientReception.ClinicId = ClinicId;
                    patientReception.UserId = UserId;
                    patientReception.OrginalClinicSectionId = clinicSectionId;
                    patientReception.ClinicSectionName = HttpContext.Session.GetString("ClinicSectionName");
                    ReceptionId = _IDUNIT.patientReception.AddOrUpdate(patientReception);
                    return Json(ReceptionId);
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, Guid receptionId, int status)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                IEnumerable<PatientReceptionViewModel> AllPatientReception = _IDUNIT.reception.GetAllReceptionsByClinicSection(section.Id, periodId, fromDate, toDate, receptionId, status);
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

        public ActionResult GetAllForRoomBed([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, Guid receptionId, int status)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                IEnumerable<PatientReceptionViewModel> AllPatientReception = _IDUNIT.reception.GetAllReceptionsForSelectRoomBed(section.Id, periodId, fromDate, toDate, receptionId, status);
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult Remove(Guid Id)
        //{
        //    try
        //    {
        //        var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Reception");
        //        if (!access) 
        //{
        //    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                           "\t Message: AccessDenied");
        //    return Json("");
        //}

        //        OperationStatus oStatus = _IDUNIT.patientReception.RemovePatientReception(Id);
        //        return Json(oStatus.ToString());
        //    }
        //    catch { return Json(0); }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveWithReceives(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.patientReception.RemovePatientReceptionWithReceives(Id, _hostEnvironment.WebRootPath);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllPatientReceptionInvoiceNums()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ReceptionViewModel> AllInvoiceNums = _IDUNIT.patientReception.GetAllPatientReceptionInvoiceNums(clinicSectionId);
                return Json(AllInvoiceNums);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetLatestReceptionNum()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var num = _IDUNIT.patientReception.GetLatestReceptionInvoiceNum(clinicSectionId);
                return Json(num);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllPatientReceptionPatients()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ReceptionViewModel> AllPatients = _IDUNIT.patientReception.GetAllPatientReceptionPatients(clinicSectionId);
                return Json(AllPatients);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetPatientReception(Guid PatientReceptionId)
        {
            try
            {
                ReceptionViewModel PatientReception = _IDUNIT.patientReception.GetPatientReceptionByIdWithDoctor(PatientReceptionId);
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

        public ActionResult GetAllForCash([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, Guid receptionId, int paymentStatus)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);

                IEnumerable<PatientReceptionViewModel> AllPatientReception = _IDUNIT.reception.GetReceptionsByClinicSectionForCash(section.Id, periodId, fromDate, toDate, receptionId, paymentStatus);
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

    }
}