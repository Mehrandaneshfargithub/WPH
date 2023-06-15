using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.Visit;
using WPH.Models.ReceptionService;
using WPH.Models.Reserve;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Visit
{
    [SessionCheck]
    public class ReservesController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReservesController> _logger;

        public ReservesController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReservesController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        // GET: Reserve
        public async Task<IActionResult> Form()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                ViewBag.AccessPrescription = _IDUNIT.subSystem.CheckUserAccess("Edit", "SecretorCanWritePrescription");
                _IDUNIT.reserve.GetModalsViewBags(ViewBag);

                IEnumerable<ClinicSectionSettingValueViewModel> sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "VisitPrice", "CanAddReceptionToPreviousDays");
                ViewBag.VisitPriceInReserve = Convert.ToDecimal(sval.FirstOrDefault(a=>a.ShowSName == "VisitPrice")?.SValue ?? "0");
                ViewBag.CanAddReceptionToPreviousDays = Convert.ToBoolean(sval.FirstOrDefault(a=>a.ShowSName == "CanAddReceptionToPreviousDays")?.SValue);

                if (ViewBag.CanAddReceptionToPreviousDays == null)
                    ViewBag.CanAddReceptionToPreviousDays = false;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/wpReserveForm.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public IActionResult AddReserve(Guid clinicSectionId, DateTime? day)
        {
            try
            {

                return Json(_IDUNIT.reserve.CheckAndAddReserve(clinicSectionId, day));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public async Task<JsonResult> ReserveDay(ReserveDayViewModel viewModel)
        {
            ReserveViewModel res = new ReserveViewModel();

            string[] Date = viewModel.CalendarDate.Split('/');
            DateTime reserveDate = new DateTime(Convert.ToInt32(Date[2]), Convert.ToInt32(Date[1]), Convert.ToInt32(Date[0]), 0, 0, 0);
            if (viewModel.Direct == "next")
            {
                reserveDate = reserveDate.AddDays(1);
            }
            else if (viewModel.Direct == "prev")
            {
                reserveDate = reserveDate.AddDays(-1);
            }

            ReserveViewModel dateExisit = _IDUNIT.reserve.GetDate(reserveDate, viewModel.ClinicSectionId);
            _IDUNIT.reserve.GetModalsViewBags(ViewBag);
            if (dateExisit != null)
            {
                if (viewModel.Pasand)
                {
                    int startTimeHour = Convert.ToInt32(viewModel.StartTime);
                    int endTimeHour = Convert.ToInt32(viewModel.EndTime);
                    int duration = Convert.ToInt32(viewModel.Dur);
                    res.StartTime = new TimeSpan(startTimeHour, 0, 0);
                    res.EndTime = new TimeSpan(endTimeHour, 0, 0);
                    res.Explanation = viewModel.Explanition;
                    res.RoundTime = duration;
                    res.Guid = dateExisit.Guid;
                    res.ClinicSectionId = dateExisit.ClinicSectionId;
                    res.Date = dateExisit.Date;
                    _IDUNIT.reserve.UpdateReserve(res);
                    return Json(res);
                }

                return Json(dateExisit);
            }
            else
            {
                res = _IDUNIT.reserve.CheckAndAddReserve(viewModel.ClinicSectionId, reserveDate);
                return Json(res);
            }

        }

        public async Task<IActionResult> GoToDate(ReserveDayViewModel viewModel)
        {
            ReserveViewModel res = new ReserveViewModel();
            string[] Date = viewModel.CalendarDate.Split('/');
            DateTime reserveDate = new DateTime(Convert.ToInt32(Date[2]), Convert.ToInt32(Date[1]), Convert.ToInt32(Date[0]), 0, 0, 0);
            ReserveViewModel dateExisit = _IDUNIT.reserve.GetDate(reserveDate, viewModel.ClinicSectionId);
            _IDUNIT.reserve.GetModalsViewBags(ViewBag);
            if (dateExisit != null)
            {
                if (viewModel.Pasand)
                {
                    int startTimeHour = Convert.ToInt32(viewModel.StartTime);
                    int endTimeHour = Convert.ToInt32(viewModel.EndTime);
                    string[] dura = viewModel.Dur.Split(':');
                    int duration = Convert.ToInt32(dura[1]);
                    res.StartTime = new TimeSpan(startTimeHour, 0, 0);
                    res.EndTime = new TimeSpan(endTimeHour, 0, 0);
                    res.Explanation = viewModel.Explanition;
                    res.RoundTime = duration;
                    res.Guid = dateExisit.Guid;
                    res.ClinicSectionId = dateExisit.ClinicSectionId;
                    res.Date = dateExisit.Date;
                    _IDUNIT.reserve.UpdateReserve(res);
                    return Json(res);
                }
                return Json(dateExisit);
            }
            else
            {
                res = _IDUNIT.reserve.CheckAndAddReserve(viewModel.ClinicSectionId, reserveDate);
                return Json(res);
            }

        }

        public async Task<IActionResult> AddNewModal(string start, string end, Guid? clinicSectionId, Guid? doctorId)
        {
            if (clinicSectionId == null || doctorId == null)
                return Json(_localizer["SelectSectionOrDoctor"]);

            Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
            string[] startTime = start.Split(' ');
            string[] endTime = end.Split(' ');
            ReserveDetailViewModel resAllDetail = new ReserveDetailViewModel();
            PatientViewModel patient = new PatientViewModel();

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(originalClinicSectionId, "UseFormNumber", "AutoCompleteFormNum", "AutoCompletePhoneNumber");
            string useform = sval?.FirstOrDefault(p => p.ShowSName == "UseFormNumber")?.SValue ?? "";
            bool useFormNum = false;
            if (useform == "true")
                useFormNum = true;
            patient.FileNumChoose = useFormNum;

            string useAutoCompleteform = sval?.FirstOrDefault(p => p.ShowSName == "AutoCompleteFormNum")?.SValue ?? "";
            bool useAutoCompleteFormNum = false;
            if (useAutoCompleteform == "true")
                useAutoCompleteFormNum = true;

            string useAutoCompletePhoneNumber = sval?.FirstOrDefault(p => p.ShowSName == "AutoCompletePhoneNumber")?.SValue ?? "false";
            ViewBag.AutoCompletePhoneNumber = bool.Parse(useAutoCompletePhoneNumber);

            resAllDetail.UseAutoCompleteFormNum = useAutoCompleteFormNum;
            resAllDetail.Patient = patient;
            resAllDetail.ReserveStartTime = startTime[0];
            resAllDetail.ReserveEndTime = endTime[0];
            resAllDetail.ClinicSectionId = clinicSectionId.Value;

            IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
            ViewBag.JobId = test.FirstOrDefault(x => x.EName == "Job").Guid;
            ViewBag.addressId = test.FirstOrDefault(x => x.EName == "Address").Guid;

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/mdReserveNewModal.cshtml", resAllDetail);

        }

        public async Task<IActionResult> EditModal(Guid Id, Guid? clinicSectionId, Guid? doctorId)
        {
            if (clinicSectionId == null || doctorId == null)
                return Json(_localizer["SelectSectionOrDoctor"]);

            Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                ReserveDetailViewModel resAllD = _IDUNIT.reserveDetail.GetReserveAllDetail(Id);
                resAllD.Patient.Age = resAllD.Patient.DateOfBirth.GetAge();
                resAllD.Patient.NameHolder = resAllD.Patient.Name;
                resAllD.Patient.PhoneNumberHolder = resAllD.Patient.PhoneNumber;

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(originalClinicSectionId, "UseFormNumber", "AutoCompleteFormNum", "AutoCompletePhoneNumber");
                string useform = sval?.FirstOrDefault(p => p.ShowSName == "UseFormNumber")?.SValue ?? "";
                bool useFormNum = false;
                if (useform == "true")
                    useFormNum = true;

                string useAutoCompleteform = sval?.FirstOrDefault(p => p.ShowSName == "AutoCompleteFormNum")?.SValue ?? "";

                bool useAutoCompleteFormNum = false;
                if (useAutoCompleteform == "true")
                    useAutoCompleteFormNum = true;

                resAllD.UseAutoCompleteFormNum = useAutoCompleteFormNum;
                resAllD.Patient.FileNumChoose = useFormNum;
                resAllD.ClinicSectionId = clinicSectionId.Value;

                string useAutoCompletePhoneNumber = sval?.FirstOrDefault(p => p.ShowSName == "AutoCompletePhoneNumber")?.SValue ?? "false";
                ViewBag.AutoCompletePhoneNumber = bool.Parse(useAutoCompletePhoneNumber);

                IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
                ViewBag.JobId = test.FirstOrDefault(x => x.EName == "Job").Guid;
                ViewBag.addressId = test.FirstOrDefault(x => x.EName == "Address").Guid;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/mdReserveNewModal.cshtml", resAllD);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/mdReserveNewModal.cshtml", new EventViewModel()); }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddOrUpdate(ReserveDetailViewModel reserveDetailViewModel)
        {
            if (reserveDetailViewModel.DoctorId.GetValueOrDefault(Guid.Empty) == Guid.Empty)
                return Json("InsertDoctor");

            Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {

                if (DateTime.TryParseExact(reserveDetailViewModel.Patient.DateOfBirthTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime birthday))
                    reserveDetailViewModel.Patient.DateOfBirth = birthday;
                else
                    reserveDetailViewModel.Patient.DateOfBirth = DateTime.Now;
                int statu = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("NotVisited");

                if (reserveDetailViewModel.Guid != Guid.Empty)
                {
                    if (reserveDetailViewModel.Patient.Name == reserveDetailViewModel.Patient.NameHolder && reserveDetailViewModel.Patient.PhoneNumber == reserveDetailViewModel.Patient.PhoneNumberHolder)
                    {
                        reserveDetailViewModel.Patient.Pass1 = "123";
                        var res = _IDUNIT.reserveDetail.UpdateReserveDetail(reserveDetailViewModel, false, originalClinicSectionId);
                        return Json(res);
                    }

                    if (_IDUNIT.patient.CheckRepeatedNameAndNumber(reserveDetailViewModel.Patient.Name, reserveDetailViewModel.Patient.PhoneNumber, originalClinicSectionId, false))
                    {
                        reserveDetailViewModel.Patient.Guid = reserveDetailViewModel.PatientId ?? Guid.Empty;
                        var res = _IDUNIT.reserveDetail.UpdateReserveDetail(reserveDetailViewModel, false, originalClinicSectionId);
                        return Json(res);
                    }
                    reserveDetailViewModel.Patient.UserName = _IDUNIT.patient.RandomString(10);
                    reserveDetailViewModel.Patient.Pass1 = "123";
                    reserveDetailViewModel.Patient.ClinicSectionId = originalClinicSectionId;
                    reserveDetailViewModel.PatientId = reserveDetailViewModel.Patient.Guid = Guid.NewGuid();

                    var result = _IDUNIT.reserveDetail.UpdateReserveDetail(reserveDetailViewModel, true, originalClinicSectionId);

                    return Json(result);
                }
                else
                {
                    if (_IDUNIT.patient.CheckRepeatedNameAndNumber(reserveDetailViewModel.Patient.Name, reserveDetailViewModel.Patient.PhoneNumber, originalClinicSectionId, true))
                    {

                        reserveDetailViewModel.Guid = Guid.NewGuid();
                        string[] Date1 = reserveDetailViewModel.ReserveStartTime.Split('T');
                        string[] Date2 = Date1[0].Split('-');
                        DateTime reserveDate = new DateTime(Convert.ToInt32(Date2[0]), Convert.ToInt32(Date2[1]), Convert.ToInt32(Date2[2]), 0, 0, 0);
                        ReserveViewModel dateExisit = _IDUNIT.reserve.CheckAndAddReserve(reserveDetailViewModel.ClinicSectionId, reserveDate);
                        reserveDetailViewModel.MasterId = dateExisit.Guid;
                        reserveDetailViewModel.StatusId = statu;
                        reserveDetailViewModel.Patient.UserName = _IDUNIT.patient.RandomString(10);
                        reserveDetailViewModel.Patient.Guid = reserveDetailViewModel.PatientId ?? Guid.Empty;
                        reserveDetailViewModel.ReserveDate = reserveDate;
                        _IDUNIT.reserveDetail.AddNewReserveDetail(reserveDetailViewModel, false, originalClinicSectionId);

                        return Json(1);
                    }
                    else
                    {
                        reserveDetailViewModel.StatusId = statu;

                        reserveDetailViewModel.Patient.UserName = _IDUNIT.patient.RandomString(10);
                        reserveDetailViewModel.Patient.ClinicSectionId = originalClinicSectionId;
                        reserveDetailViewModel.Guid = Guid.NewGuid();

                        string[] startDate = reserveDetailViewModel.ReserveStartTime.Split('T');
                        string[] Date = startDate[0].Split('-');
                        DateTime date = new DateTime(Convert.ToInt32(Date[0]), Convert.ToInt32(Date[1]), Convert.ToInt32(Date[2]));
                        ReserveViewModel dateExisit = _IDUNIT.reserve.GetDate(date, reserveDetailViewModel.ClinicSectionId);
                        reserveDetailViewModel.MasterId = dateExisit.Guid;
                        reserveDetailViewModel.ReserveDate = date;
                        reserveDetailViewModel.Patient.Guid = Guid.NewGuid();

                        _IDUNIT.reserveDetail.AddNewReserveDetail(reserveDetailViewModel, true, originalClinicSectionId);

                        return Json(1);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public async Task<IActionResult> QeueModal(Guid Id, Guid clinicSectionId)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                VisitViewModel visitDetailList = _IDUNIT.visit.GetVisitBasedOnReserveDetailId(Id);

                if (visitDetailList != null)
                {
                    //visitDetailList.AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllFillBySecretaryClinicSectionChoosenValues(originalClinicSectionId).OrderBy(x => x.Priority).ToList();
                    //IEnumerable<PatientVariablesValueViewModel> PatientConstantVariablesValue = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(visitDetailList.ReserveDetail.PatientId);
                    //IEnumerable<PatientVariablesValueViewModel> PatientVariableValue = PatientConstantVariablesValue.Where(x => x.VisitId == visitDetailList.Guid);

                    //foreach (var PatientVal in PatientVariableValue)
                    //{
                    //    ClinicSectionChoosenValueViewModel cli = visitDetailList.AllClinicSectionChoosenValues.SingleOrDefault(x => x.PatientVariableId == PatientVal.PatientVariableId);
                    //    if (cli != null)
                    //    {
                    //        cli.Value = PatientVal.Value;
                    //        cli.PatientVariableValueGuid = PatientVal.Guid;
                    //    }
                    //}

                    //foreach (var PatientVal in PatientConstantVariablesValue)
                    //{
                    //    ClinicSectionChoosenValueViewModel cli = visitDetailList.AllClinicSectionChoosenValues.SingleOrDefault(x => x.PatientVariableId == PatientVal.PatientVariableId);
                    //    if (cli != null)
                    //    {
                    //        if (cli.VariableStatusName == "VariableValueIsConstant" || PatientVal.VariableInsertedDate == visitDetailList.VisitDate)
                    //        {
                    //            cli.Value = PatientVal.Value;
                    //            cli.PatientVariableValueGuid = PatientVal.Guid;
                    //        }
                    //    }
                    //}

                    //foreach (var clinicVal in visitDetailList.AllClinicSectionChoosenValues)
                    //{
                    //    string name = clinicVal.PatientVariableVariableName;
                    //    string[] all = name.Split(' ');
                    //    string total = "";
                    //    foreach (var s in all)
                    //    {
                    //        total += s;
                    //    }
                    //    clinicVal.VariableNameForView = total;
                    //}

                    return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/newQueuModal.cshtml", visitDetailList);

                }

                ReserveDetailViewModel resAllD = new ReserveDetailViewModel();

                VisitViewModel visit = new VisitViewModel
                {
                    ReserveDetailId = Id,
                    ReserveDetail = resAllD,
                    Guid = Guid.NewGuid(),
                    ClinicSectionId = clinicSectionId
                };

                DateTime Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                resAllD.Patient = _IDUNIT.reserveDetail.GetPatientIdAndNameFromReserveDetailId(Id);
                resAllD.PatientId = resAllD.Patient.Guid;
                //visit.AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllFillBySecretaryClinicSectionChoosenValues(originalClinicSectionId).OrderBy(x => x.Priority).ToList();
                //IEnumerable<PatientVariablesValueViewModel> PatientConstantVariablesValueForNewVisit = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(resAllD.PatientId);

                //foreach (var PatientVal in PatientConstantVariablesValueForNewVisit)
                //{
                //    ClinicSectionChoosenValueViewModel cli = visit.AllClinicSectionChoosenValues.SingleOrDefault(x => x.PatientVariableId == PatientVal.PatientVariableId);
                //    if (cli != null)
                //    {
                //        if (cli.VariableStatusName == "VariableValueIsConstant" || PatientVal.VariableInsertedDate == Today)
                //        {
                //            cli.Value = PatientVal.Value;
                //            cli.PatientVariableValueGuid = PatientVal.Guid;
                //        }
                //    }
                //}

                //foreach (var clinicVal in visit.AllClinicSectionChoosenValues)
                //{
                //    string name = clinicVal.PatientVariableVariableName;
                //    string[] all = name.Split(' ');
                //    string total = "";
                //    foreach (var s in all)
                //    {
                //        total += s;
                //    }
                //    clinicVal.VariableNameForView = total;
                //}

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/newQueuModal.cshtml", visit);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }

        }


        public async Task<JsonResult> AddToQeue(VisitViewModel visit, IEnumerable<PatientVariablesValueViewModel> Variables)
        {
            Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                List<PatientVariablesValueViewModel> addVariables = new List<PatientVariablesValueViewModel>();
                List<PatientVariablesValueViewModel> updatedVariables = new List<PatientVariablesValueViewModel>();
                if (Variables != null)
                {
                    foreach (var variable in Variables)
                    {
                        if (!string.IsNullOrWhiteSpace(variable.Value))
                        {

                            variable.ReceptionId = variable.Status.ToLower() == "constant" ? null : variable.ReceptionId;
                            variable.VariableInsertedDate = visit.VisitDate;
                            addVariables.Add(variable);
                        }
                        //else
                        //{
                        //    updatedVariables.Add(new PatientVariablesValueViewModel() { Guid = variable.Guid, Value = variable.Value });
                        //}
                    }
                }

                int statu = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("InQueue");

                visit.StatusId = statu;
                visit.VisitDate = DateTime.Now;
                visit.OriginalClinicSectionId = originalClinicSectionId;
                visit.CreateUserId = userId;
                visit.AddVariables = addVariables;
                visit.UpdatedVariables = updatedVariables;

                Guid visitId;
                if (visit.UniqueVisitNum == null)
                {
                    await _IDUNIT.visit.AddNewVisit(visit);
                }
                else
                {
                    _IDUNIT.visit.UpdateVisit(visit);
                    visitId = visit.Guid;
                }

                var doctorId = _IDUNIT.reserveDetail.GetReserveDetailDoctorId(visit.ReserveDetailId ?? Guid.Empty);
                _IDUNIT.visit.UpdateReceptionNums(doctorId);


                return Json(visit.Guid);

            }

            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public async Task<JsonResult> GetPatient()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                IEnumerable<PatientViewModel> eventList = await _IDUNIT.patient.GetAllPatientsWithCombinedNameAndFileNumForReserve(false, clinicSectionId, ClinicId, SectionTypeId);

                return Json(eventList);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public async Task<JsonResult> GetEvents(string date, Guid doctorId, Guid clinicSectionId, string calendarStatus)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                string[] D = date.Split('T');
                string[] Da = D[0].Split('-');
                DateTime CalDate = new DateTime(Convert.ToInt32(Da[0]), Convert.ToInt32(Da[1]), Convert.ToInt32(Da[2]), 0, 0, 0);
                DateTime today = DateTime.Now;

                List<EventViewModel> eventList = new List<EventViewModel>();

                DateTime FromDate = new DateTime(CalDate.Year, CalDate.Month, 1, 0, 0, 0);
                DateTime ToDate = FromDate.AddMonths(2);
                if (calendarStatus == "d")
                {
                    FromDate = CalDate;
                    ToDate = FromDate.AddDays(1);
                }
                else if (calendarStatus == "w")
                {
                    FromDate = CalDate.AddDays(-(int)DateTime.Today.DayOfWeek);
                    ToDate = FromDate.AddDays(7);
                }
                else
                {
                    FromDate = new DateTime(CalDate.Year, CalDate.Month, 1, 0, 0, 0);
                    ToDate = FromDate.AddMonths(2);
                }

                eventList = _IDUNIT.reserveDetail.GetAllReservesBetweenTwoDate(originalClinicSectionId, clinicSectionId, FromDate, ToDate, CalDate, today, doctorId);
                eventList.ForEach(x => x.Remain = x.Remain.Replace('٫', '.'));

                return Json(eventList);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }


        public async Task<JsonResult> GetReserveQueueCondition()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                ClinicSectionSettingValueViewModel sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "ReserveHasQueue").FirstOrDefault();
                string reserveQueue = sval?.SValue?.ToLower();

                if (reserveQueue == "false")
                    return Json(false);
                else
                    return Json(true);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }



        public async Task<JsonResult> EventChangePosition(Guid id, string start, string end, bool today)
        {
            try
            {
                VisitViewModel visit = _IDUNIT.visit.GetVisitBasedOnReserveDetailId(id);
                if (visit != null)
                {

                    if (_IDUNIT.prescription.VisitHasPrescription(visit.Guid))
                    {
                        return Json(0);
                    }

                    if (!today)
                        _IDUNIT.visit.RemoveVisit(visit.Guid);
                }

                string[] startTime = start.Split('+');
                string[] endTime = end.Split('+');

                _IDUNIT.reserveDetail.UpdateReserveDetailTime(id, startTime[0], endTime[0]);
                if (visit != null)
                {
                    var doctorId = _IDUNIT.reserveDetail.GetReserveDetailDoctorId(visit.ReserveDetailId ?? Guid.Empty);

                    _IDUNIT.visit.UpdateReceptionNums(doctorId);
                }

                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }

        public async Task<JsonResult> EventResized(Guid id, string start, string end)
        {
            try
            {
                string[] startTime = start.Split('+');
                string[] endTime = end.Split('+');
                _IDUNIT.reserveDetail.UpdateReserveDetailTime(id, startTime[0], endTime[0]);
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }



        [HttpPost]
        public async Task<JsonResult> EventChangeStatus(Guid id, string status, string correntReserveStatus, Guid clinicSectionId)
        {
            try
            {
                var originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                int statusId = _IDUNIT.baseInfo.GetBaseInfoGeneralByName(status);
                ReserveDetailViewModel resAllD = new ReserveDetailViewModel()
                {
                    Guid = id,
                    StatusId = statusId,
                    UserId = userId,
                    ClinicSectionId = clinicSectionId,
                    OriginalClinicSectionId = originalClinicSectionId
                };

                VisitViewModel visit = _IDUNIT.visit.GetVisitBasedOnReserveDetailId(id);
                if (visit != null)
                    visit.OriginalClinicSectionId = originalClinicSectionId;

                if (correntReserveStatus == "InQueue")
                {
                    if (status == "NotVisited")
                    {
                        if (visit != null)
                            _IDUNIT.visit.RemoveVisit(visit.Guid);

                        _IDUNIT.reserveDetail.UpdateReserveDetailStatus(resAllD);

                        var doctorId = _IDUNIT.reserveDetail.GetReserveDetailDoctorId(id);
                        _IDUNIT.visit.UpdateReceptionNums(doctorId);
                        return Json(1);
                    }
                }

                if (correntReserveStatus == "NotVisited")
                {
                    if (status == "Visiting")
                    {
                        await _IDUNIT.reception.AddReceptionForReserve(resAllD);
                        return Json(1);
                    }
                }
                else if (correntReserveStatus == "Visited")
                {
                    if (status == "Visiting")
                    {
                        if (visit != null)
                        {
                            if (_IDUNIT.prescription.VisitHasPrescription(visit.Guid))
                            {
                                return Json(0);
                            }
                        }
                    }
                }

                _IDUNIT.reserveDetail.UpdateReserveDetailStatus(resAllD);

                if (visit != null)
                {
                    visit.StatusId = statusId;
                    visit.VisitDate = DateTime.Now;
                    visit.ModifiedUserId = userId;
                    await _IDUNIT.visit.UpdateVisitStatus(visit);
                    return Json(1);
                }
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Remove(Guid id)
        {
            try
            {
                var oStatus = _IDUNIT.reserveDetail.RemoveReserveDetail(id);

                var doctorId = _IDUNIT.reserveDetail.GetReserveDetailDoctorId(id);
                _IDUNIT.visit.UpdateReceptionNums(doctorId);
                return Json(oStatus);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

        }


        public async Task<JsonResult> GetPatientByReserveId(Guid id)
        {
            try
            {
                ReserveDetailViewModel resAllD = _IDUNIT.reserveDetail.GetReserveAllDetail(id);
                return Json(resAllD.Patient);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public async Task<JsonResult> GetLastPatientVisit(Guid patientId)
        {
            try
            {
                bool result = false;
                DateTime? lastTime = _IDUNIT.reserveDetail.GetLastPatientVisitDate(patientId, false);
                if (lastTime != null)
                {
                    if ((DateTime.Now - lastTime).Value.TotalDays <= 6)
                    {
                        result = true;
                    }
                }
                CultureInfo cultures = new CultureInfo("en-US");

                string time = lastTime.Value.ToString("dd/MM/yyyy", cultures);
                return Json(new { time, result });
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }


        public async Task<JsonResult> GetReceptionRemByReceptionId(Guid reserveDetailId)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var result = await _IDUNIT.reception.GetReceptionRemByReserveDetailId(reserveDetailId, ClinicSectionId);
                return Json(result);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        [HttpPost]
        public ActionResult AddReceptionService(ReceptionServiceViewModel viewModel)
        {
            try
            {
                var today = DateTime.Now;
                viewModel.ServiceDate = today;
                viewModel.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.CreatedDate = today;
                _IDUNIT.receptionService.AddReceptionService(viewModel);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);

                if (e.Message == "NotEnoughProductCount")
                    return Json("NotEnoughProductCount");

                return Json(0);
            }
        }

        public async Task<IActionResult> CheckLastPatientVisit(Guid reserveDetailId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var result = await _IDUNIT.reserve.CheckLastPatientVisit(reserveDetailId, clinicSectionId);
                return Json(result);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

        public async Task<IActionResult> RemoveReserveVisitPrice(Guid reserveDetailId)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                _IDUNIT.reserve.RemoveReserveVisitPrice(reserveDetailId);

                var result = await _IDUNIT.reception.GetReceptionRemByReserveDetailId(reserveDetailId, ClinicSectionId);
                return Json(result);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }
        }

    }
}