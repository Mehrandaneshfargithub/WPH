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
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.Reception;
using WPH.Models.ReceptionAmbulance;
using WPH.Models.ReceptionClinicSection;
using WPH.Models.ReceptionTemperature;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Reception
{
    [SessionCheck]
    public class ReceptionController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ReceptionController> _logger;


        public ReceptionController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostEnvironment, ILogger<ReceptionController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
        public async Task<ActionResult> Form()
        {
            var licenceAccess = _IDUNIT.licenceKey.CheckLicence();
            if (!long.TryParse(licenceAccess, out long remDay))
                return RedirectToAction("Index", "UserHandler");

            var access = _IDUNIT.subSystem.CheckUserAccess("New", "NewNormalReception");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ReceptionViewModel reception = new ReceptionViewModel();
            _IDUNIT.room.GetModalsViewBags(ViewBag);
            ViewBag.Emergency = false;

            return View("/Views/Shared/PartialViews/AppWebForms/Reception/wpReceptionFormNew.cshtml", reception);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddReception(ReceptionViewModel reception)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid clinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                reception.ClinicId = clinicId;
                reception.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                reception.OrginalClinicSectionId = clinicSectionId;
                reception.RootPath = _hostEnvironment.WebRootPath;
                //reception.ReceptionTypeId = _IDUNIT.baseInfo.GetIdByNameAndType("Reception", "ReceptionType");
                if (reception.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reception");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "NewNormalReception");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                }

                return Json(_IDUNIT.reception.AddNewReception(reception));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }


        public async Task<ActionResult> Edit(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reception");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ReceptionViewModel reception = new ReceptionViewModel();
            _IDUNIT.room.GetModalsViewBags(ViewBag);
            ViewBag.Emergency = false;
            ViewBag.ReceptionId = Id;

            return View("/Views/Shared/PartialViews/AppWebForms/Reception/wpReceptionFormNew.cshtml", reception);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Reception");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.reception.RemoveReception(Id, _hostEnvironment.WebRootPath);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }


        public JsonResult GetReception(Guid ReceptionId)
        {
            try
            {
                ReceptionViewModel PatientReception = _IDUNIT.reception.GetReception(ReceptionId);
                PatientReception.ReceptionDateString = PatientReception.ReceptionDate.Value.Date.Day + "/" + PatientReception.ReceptionDate.Value.Date.Month + "/" + PatientReception.ReceptionDate.Value.Date.Year;
                return Json(PatientReception);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public JsonResult GetReceptionAmbulance(Guid ReceptionId)
        {
            try
            {
                ReceptionAmbulanceViewModel Reception = _IDUNIT.receptionAmbulance.GetReceptionAmbulanceByReceptionId(ReceptionId);

                return Json(Reception);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        public JsonResult GetServerVisitNum(Guid visitId)
        {
            try
            {
                string Reception = _IDUNIT.reception.GetServerVisitNum(visitId);

                return Json(Reception);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }
        public JsonResult GetReceptionDoctor(Guid ReceptionId)
        {
            try
            {
                var receptionDoctors = _IDUNIT.reception.GetReceptionDoctor(ReceptionId);

                return Json(receptionDoctors);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }


        public JsonResult UpdateReceptionCleareance(ReceptionViewModel Reception)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HospitalPatient");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                if (!String.IsNullOrWhiteSpace(Reception.ExitDateYear))
                    Reception.ExitDate = new DateTime(Convert.ToInt32(Reception.ExitDateYear), Convert.ToInt32(Reception.ExitDateMonth), Convert.ToInt32(Reception.ExitDateDay));
                _IDUNIT.reception.UpdateReceptionCleareance(Reception);

                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        public JsonResult UpdateReceptionChiefComplaint(ReceptionViewModel Reception)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "HospitalPatient");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                _IDUNIT.reception.UpdateReceptionChiefComplaint(Reception);

                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        public ActionResult SelectModal(Guid RoomId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();

                baseInfosAndPeriods.periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                baseInfosAndPeriods.sections = _IDUNIT.clinicSection.GetAllUserClinicSectionsJustNameAndGuid(userId, "").ToList();

                ViewBag.FromToId = (int)Periods.FromDateToDate;
                var room = _IDUNIT.room.GetRoomWithSection(RoomId);
                ViewBag.SectionId = room.RoomClinicSectionId;
                ViewBag.SectionText = room.SectionName;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Reception/mdSelectReceptionModal.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetReceptionClinicSectionGrid()
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReceptionClinicSection/dgReceptionClinicSectionGridByClinicSectionId.cshtml");
            }
            catch
            (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public async Task<ActionResult> GetReceptionGrid()
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


                ViewBag.NotLab = HttpContext.Session.GetString("SectionTypeName")?.ToLower();
                ViewBag.HospitalPatient = false;

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Reception");
                ViewBag.AccessEditReception = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteReception = access.Any(p => p.AccessName == "Delete");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReception/dgPatientReceptionGrid.cshtml");
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        public ActionResult GetPatientName()
        {
            try
            {
                IEnumerable<ReceptionPatientNameViewModel> patientName = _IDUNIT.reception.GetReceptionPatientName();
                return Json(patientName);
            }
            catch
            (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetAllReceptionTemperature([DataSourceRequest] DataSourceRequest request, Guid ReceptionId)
        {
            try
            {
                IEnumerable<ReceptionTemperatureViewModel> AllRoom;

                AllRoom = _IDUNIT.reception.GetAllReceptionTemperature(ReceptionId);


                return Json(AllRoom.ToDataSourceResult(request));
            }
            catch
            (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
           
        }

        public JsonResult AddReceptionTemperature(ReceptionTemperatureViewModel reception)
        {
            try
            {
                string[] time = reception.InsertedTime.Split(':');
                reception.CreatedDate = DateTime.Now;
                reception.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                reception.InsertedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                _IDUNIT.reception.AddNewReceptionTemperature(reception);
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        public JsonResult AddReceptionClinicSection(ReceptionClinicSectionViewModel reception)
        {
            try
            {
                reception.CreatedDate = DateTime.Now;
                reception.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                _IDUNIT.receptionClinicSection.AddNewReceptionClinicSection(reception);
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public JsonResult GetAllReceptionClinicSectionPatients()
        {
            try
            {
                var patients =_IDUNIT.patient.GetAllReceptionClinicSectionPatients();
                return Json(patients);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public ActionResult GetAllReceptionClinicSectionByReceptionId([DataSourceRequest] DataSourceRequest request, Guid ReceptionId)
        {
            try
            {
                IEnumerable<ReceptionClinicSectionViewModel> AllReceptionClinicSection;

                AllReceptionClinicSection = _IDUNIT.receptionClinicSection.GetAllReceptionClinicSectionByReceptionId(ReceptionId);


                return Json(AllReceptionClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }


        public ActionResult GetAllReceptionClinicSectionByClinicSectionId([DataSourceRequest] DataSourceRequest request, int periodId, string dateFrom, string dateTo, SectionViewModel section, Guid receptionId, int status)
        {
            try
            {
                string[] from = dateFrom.Split('-');
                string[] to = dateTo.Split('-');

                DateTime fromDate = new DateTime(Convert.ToInt32(from[0]), Convert.ToInt32(from[1]), Convert.ToInt32(from[2]), 0, 0, 0);
                DateTime toDate = new DateTime(Convert.ToInt32(to[0]), Convert.ToInt32(to[1]), Convert.ToInt32(to[2]), 23, 59, 59);
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ReceptionClinicSectionViewModel> AllReceptionClinicSection;

                AllReceptionClinicSection = _IDUNIT.receptionClinicSection.GetAllReceptionClinicSectionByClinicSectionId(clinicSectionId, periodId, fromDate, toDate, receptionId, status);


                return Json(AllReceptionClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }

        public JsonResult GetReceptionClinicSectionByDestinationReceptionId(Guid DestinationReceptionId)
        {
            try
            {
                ReceptionClinicSectionViewModel oStatus = _IDUNIT.receptionClinicSection.GetReceptionClinicSectionByDestinationReceptionId(DestinationReceptionId);
                oStatus.Description = oStatus.Description.Replace('\n', ' ');
                return Json(oStatus);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveReceptionTemperature(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.reception.RemoveReceptionTemperature(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearancePatient(Guid receptionId, bool confirm)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Discharge");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var result = _IDUNIT.reception.DischargePatient(receptionId, userId, confirm);
                return Json(result);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        private StiReport ReceptionReport(string PatientName, string MotherName, string AttendantName, string Gender,
            string Age, string Room, string EntranceDate, string OperationDate, string Operation, string Surgery, string DispatcherDoctor, string Address, string Phone, string IdentityNumber)
        {
            try
            {
                StiReport report = new StiReport();
                string path = Path.Combine(this._hostEnvironment.WebRootPath, "Content", "Reports", "HospitalReceptionReport.mrt");
                report.Load(path);

                report.Dictionary.Variables["PatientName"].Value = PatientName;
                report.Dictionary.Variables["MotherName"].Value = MotherName;
                report.Dictionary.Variables["AttendantName"].Value = AttendantName;
                report.Dictionary.Variables["Gender"].Value = Gender;
                report.Dictionary.Variables["Age"].Value = Age;
                report.Dictionary.Variables["Room"].Value = Room;
                report.Dictionary.Variables["EntranceDate"].Value = EntranceDate;
                report.Dictionary.Variables["DispatcherDoctor"].Value = DispatcherDoctor;

                report.Dictionary.Variables["OperationDate"].Value = OperationDate;
                report.Dictionary.Variables["Operation"].Value = Operation;
                report.Dictionary.Variables["SurgeryDoctor"].Value = Surgery;
                report.Dictionary.Variables["Address"].Value = Address;
                report.Dictionary.Variables["Phone"].Value = Phone;
                report.Dictionary.Variables["IdentityNumber"].Value = IdentityNumber;

                report.Dictionary.Variables["vDate"].Value = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;

                return report;
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }


        public ActionResult PrintReceptionReport(string PatientName, string MotherName, string AttendantName, string Gender,
            string Age, string Room, string EntranceDate, string OperationDate, string Operation, string Surgery, string DispatcherDoctor, string Address, string Phone, string IdentityNumber)
        {
            try
            {

                StiReport report = ReceptionReport(PatientName, MotherName, AttendantName, Gender, Age, Room, EntranceDate, OperationDate, Operation, Surgery, DispatcherDoctor, Address, Phone, IdentityNumber);

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
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }
    }
}
