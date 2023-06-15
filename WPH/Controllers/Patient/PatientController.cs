using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using WPH.Helper;
using Microsoft.AspNetCore.Mvc;
using WPH.MvcMockingServices;
using Microsoft.Extensions.Localization;
using WPH.Models.CustomDataModels.Patient;
using Microsoft.AspNetCore.Http;
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.CustomDataModels.Visit_PatientVariables;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.Models.PatientImage;
using WPH.Models.CustomDataModels.Chart;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.Clinic;
using Microsoft.Extensions.Logging;
using WPH.Models.Patient;
using WPH.Models.CustomDataModels.PatientMedicine;

namespace WPH.Controllers.Patient
{
    [SessionCheck]
    public class PatientController : Controller
    {
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<PatientController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Patient");
                ViewBag.AccessNewPatient = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditPatient = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeletePatient = access.Any(p => p.AccessName == "Delete");

                string userName = string.Empty;
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.patient.GetModalsViewBags(ViewBag);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/wpPatient.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
           
        }

        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid ParentId = Guid.Parse(HttpContext.Session.GetString("ParentId"));

                List<PatientViewModel> Allpatient = new List<PatientViewModel>();
                if (ParentId == Guid.Empty)
                {
                    Allpatient = _IDUNIT.patient.GetAllPatients(true, null).ToList();
                }
                else
                {
                    Allpatient = _IDUNIT.patient.GetAllPatients(true, clinicSectionId).ToList();
                }
                foreach (var baseInf in Allpatient)
                {
                    if (baseInf.UserGenderName != null)
                        baseInf.UserGenderName = _localizer[baseInf.UserGenderName];
                }
                string SectionTypeName = HttpContext.Session.GetString("SectionTypeName");

                //if (SectionTypeName == "Clinic")
                //{
                //    int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));

                //    ClinicSectionSettingValueViewModel sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();
                //    string useform = "";
                //    try
                //    {
                //        useform = sval.SValue;
                //    }
                //    catch
                //    {
                //        useform = "false";
                //    }
                //    bool useFormNum = false;
                //    if (useform == "true")
                //        useFormNum = true;
                //    Allpatient.ForEach(a => { a.FileNumChoose = useFormNum; });

                //}


                return Json(Allpatient.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult PatientVariableModal(Guid patientId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                PatientVariableViewModel AllVariables = new();
                IEnumerable<ClinicSectionChoosenValueViewModel> allPatientChoosenVariables = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(clinicSectionId);
                IEnumerable<ClinicSectionChoosenValueViewModel> allConstant = allPatientChoosenVariables.Where(x => x.VariableStatusName == "VariableValueIsConstant");

                List<PatientVariablesValueViewModel> allVariablesValue = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(patientId).ToList();

                foreach (var co in allConstant)
                {
                    allVariablesValue.RemoveAll(x => x.PatientVariableId == co.PatientVariableId);
                }

                AllVariables.ClinicSectionChoosenValues = allPatientChoosenVariables.Where(x => x.VariableStatusName == "VariableValueIsVariable" && (x.VariableDisplayName == "ShowInPatient" || x.VariableDisplayName == "ShowInVisitAndPatient")).OrderBy(x => x.PatientVariableVariableName).ToList();
                AllVariables.PatientVariablesValues = allVariablesValue.OrderByDescending(x => x.VariableInsertedDate).ThenBy(x => x.PatientVariableVariableName).ToList();

                List<DateTime?> AllDates = AllVariables.PatientVariablesValues.Select(x => x.VariableInsertedDate).Distinct().ToList();

                List<List<PatientVariablesValueViewModel>> AllVariablewithDate = new List<List<PatientVariablesValueViewModel>>();

                foreach (var date in AllDates)
                {
                    List<PatientVariablesValueViewModel> PatientVariablesinnerList = new List<PatientVariablesValueViewModel>();
                    foreach (var choosenValue in AllVariables.ClinicSectionChoosenValues)
                    {
                        PatientVariablesValueViewModel newpValue = new();
                        newpValue = AllVariables.PatientVariablesValues.Where(x => x.VariableInsertedDate == date && x.PatientVariableId == choosenValue.PatientVariableId).FirstOrDefault();
                        if (newpValue != null)
                        {
                            PatientVariablesinnerList.Add(newpValue);
                        }
                        else
                        {
                            PatientVariablesinnerList.Add(new PatientVariablesValueViewModel { PatientVariableId = choosenValue.PatientVariableId, VariableInsertedDate = date, Value = "" });
                        }
                    }
                    AllVariablewithDate.Add(PatientVariablesinnerList);
                }

                AllVariables.AllVariableValues = AllVariablewithDate;
                AllVariables.PatientId = patientId;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/dgPatientVariablesGrid.cshtml", AllVariables);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetPatientJustNameAndGuid([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<PatientViewModel> eventList = _IDUNIT.patient.GetPatientJustNameAndGuid(clinicSectionId);

                return Json(eventList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public ActionResult GetAllPatientSpeceficVariable([DataSourceRequest] DataSourceRequest request, Guid ReceptionId, string VariableName)
        {
            try
            {
                IEnumerable<PatientVariablesValueViewModel> AllpatientVariablse = _IDUNIT.patientVariablesValue.GetAllPatientSpeceficVariable(ReceptionId, VariableName);

                return Json(AllpatientVariablse.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult GetAllReceptionVariable(Guid ReceptionId)
        {
            try
            {
                IEnumerable<PatientVariablesValueViewModel> AllpatientVariablse = _IDUNIT.patientVariablesValue.GetAllReceptionVariable(ReceptionId);

                return Json(AllpatientVariablse);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(PatientViewModel Patient)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                DateTime date = DateTime.Now;
                if (Patient.DateOfBirthYear != null)
                    date = new DateTime(Convert.ToInt32(Patient.DateOfBirthYear), Convert.ToInt32(Patient.DateOfBirthMonth), Convert.ToInt32(Patient.DateOfBirthDay));
                Patient.DateOfBirth = date;
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                if (Patient.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Patient");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.patient.CheckRepeatedNameAndNumber(Patient.Name, Patient.PhoneNumber, clinicSectionId, false, Patient.NameHolder, Patient.PhoneNumberHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Patient.UserName = _IDUNIT.patient.RandomString(10);
                    Patient.Pass1 = _IDUNIT.patient.RandomString(10);

                    _IDUNIT.patient.UpdatePatient(Patient);

                    return Json(1);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Patient");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.patient.CheckRepeatedNameAndNumber(Patient.Name, Patient.PhoneNumber, clinicSectionId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    Patient.UserName = _IDUNIT.patient.RandomString(10);
                    Patient.Pass1 = _IDUNIT.patient.RandomString(10);
                    Patient.ClinicSectionId = clinicSectionId;

                    return Json(_IDUNIT.patient.AddPatient(Patient, clinicSectionId));
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

        public async Task<IActionResult> AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Patient");
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
                bool useFormNum = false;

                string SectionTypeName = HttpContext.Session.GetString("SectionTypeName");
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));


                if (SectionTypeName == "Clinic")
                {
                    ClinicSectionSettingValueViewModel sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();

                    string useform = sval?.SValue ?? "";
                    useFormNum = false;
                    if (useform == "true")
                        useFormNum = true;
                }


                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                PatientViewModel patient = new();
                patient.DateOfBirth = DateTime.Now;
                patient.FileNum = _IDUNIT.patient.getLastPatientFileNumber(clinicSectionId, ClinicId);
                patient.FileNumChoose = useFormNum;
                IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
                ViewBag.addressId = test.FirstOrDefault(x => x.EName == "Address").Guid;
                ViewBag.jobId = test.FirstOrDefault(x => x.EName == "Job").Guid;
                ViewBag.SectionTypeName = SectionTypeName;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdPatientNewModal.cshtml", patient);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdPatientNewModal.cshtml", new PatientViewModel());
            }
        }

        public async Task<IActionResult> EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Patient");
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

                bool useFormNum = false;

                string SectionTypeName = HttpContext.Session.GetString("SectionTypeName");
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                if (SectionTypeName == "Clinic")
                {
                    ClinicSectionSettingValueViewModel sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseFormNumber").FirstOrDefault();

                    string useform = sval?.SValue ?? "";
                    useFormNum = false;
                    if (useform == "true")
                        useFormNum = true;
                }


                PatientViewModel Patient = _IDUNIT.patient.GetPatient(Id);
                Patient.NameHolder = Patient.Name;
                Patient.PhoneNumberHolder = Patient.PhoneNumber;
                Patient.FileNumChoose = useFormNum;
                IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
                ViewBag.addressId = test.FirstOrDefault(x => x.EName == "Address").Guid;
                ViewBag.jobId = test.FirstOrDefault(x => x.EName == "Job").Guid;
                ViewBag.SectionTypeName = SectionTypeName;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdPatientNewModal.cshtml", Patient);
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
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Patient");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.patient.RemovePatient(Id);
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

        public ActionResult PatientRecordForm(Guid patientid)
        {
            try
            {
                ViewBag.AccessNewDisease = _IDUNIT.subSystem.CheckUserAccess("New", "Disease");
                PatientViewModel Patient = _IDUNIT.patient.GetPatientWithCombinedNameAndPhone(patientid);
                PatientDiseaseRecordViewModel allDiseases = new();
                allDiseases.Patient = Patient;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/PatientRecordForm.cshtml", allDiseases);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/PatientRecordForm.cshtml", new PatientDiseaseRecordViewModel());
            }
        }

        public JsonResult GetPatientById(Guid PatientId)
        {
            try
            {
                PatientViewModel Patient = _IDUNIT.patient.GetPatient(PatientId);
                return Json(Patient);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllDiseaseForPatient(Guid Id)
        {
            try
            {
                List<PatientDiseaseRecordViewModel> patientDiseaseRecord = _IDUNIT.patientDisease.GetAllDiseaseForPatient(Id).ToList();

                List<SelectListItem> allPatientDisease = patientDiseaseRecord.Select(x => new SelectListItem { Text = x.DiseaseName, Value = x.DiseaseId.ToString() }).ToList();
                return Json(allPatientDisease);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }



        public JsonResult AddDiseaseForPatient(Guid[] DiseaseId, Guid PatientId)
        {
            try
            {
                List<PatientDiseaseRecordViewModel> diseases = new List<PatientDiseaseRecordViewModel>();
                if (DiseaseId != null)
                {
                    diseases = DiseaseId.Select(p => new PatientDiseaseRecordViewModel
                    {
                        Patientid = PatientId,
                        DiseaseId = p
                    }).ToList();

                    _IDUNIT.patientDisease.AddNewDiseasesForPatient(diseases);
                }


                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult RemoveDiseaseFromPatient(Guid[] DiseaseId, Guid PatientId)
        {
            try
            {
                _IDUNIT.patientDisease.RemoveDiseasesFromPatient(DiseaseId, PatientId);

                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult PatientRecordMedicineForm(Guid patientid)
        {
            try
            {
                ViewBag.AccessNewMedicine = _IDUNIT.subSystem.CheckUserAccess("New", "Medicine");
                PatientViewModel Patient = _IDUNIT.patient.GetPatientWithCombinedNameAndPhone(patientid);
                PatientMedicineRecordViewModel allDiseases = new();
                allDiseases.Patient = Patient;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/PatientMedicineRecordForm.cshtml", allDiseases);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/PatientMedicineRecordForm.cshtml", new PatientDiseaseRecordViewModel());
            }
        }


        public JsonResult GetAllMedicineForPatient(Guid Id)
        {
            try
            {
                List<PatientMedicineRecordViewModel> patientDiseaseRecord = _IDUNIT.patientMedicine.GetAllMedicineRecordForPatient(Id);

                List<SelectListItem> allPatientDisease = patientDiseaseRecord.Select(x => new SelectListItem { Text = x.MedicineName, Value = x.MedicineId.ToString() }).ToList();
                return Json(allPatientDisease);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }



        public JsonResult AddMedicineToPatient(Guid[] MedicineId, Guid PatientId)
        {
            try
            {
                List<PatientMedicineRecordViewModel> medicines = new List<PatientMedicineRecordViewModel>();
                if (MedicineId != null)
                {
                    medicines = MedicineId.Select(p => new PatientMedicineRecordViewModel
                    {
                        Patientid = PatientId,
                        MedicineId = p
                    }).ToList();

                    _IDUNIT.patientMedicine.AddMedicineToPatient(medicines);
                }


                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult RemoveMedicineFromPatient(Guid[] MedicineId, Guid PatientId)
        {
            try
            {
                _IDUNIT.patientMedicine.RemoveMedicineFromPatient(MedicineId, PatientId);

                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public async Task<JsonResult> GetAllPatient()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                Guid ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId"));
                int SectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                IEnumerable<PatientViewModel> patients = await _IDUNIT.patient.GetAllPatientsWithCombinedNameAndFileNum(false, clinicSectionId, ClinicId, SectionTypeId);
                return Json(patients);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public async Task<JsonResult> GetAllPatientForFilter()
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                IEnumerable<PatientFilterViewModel> patients = await _IDUNIT.patient.GetAllPatientForFilter(clinicSectionId);
                return Json(patients);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public ActionResult NewPatientVariableModal(Guid PatientId)
        {

            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            ViewBag.AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(ClinicSectionId).Where(x => x.VariableStatusName == "VariableValueIsVariable" && (x.VariableDisplayName == "ShowInPatient" || x.VariableDisplayName == "ShowInVisitAndPatient")).OrderBy(x => x.PatientVariableVariableName)/*.OrderBy(x => x.Priority).ToList()*/;
            foreach (var clinicVal in ViewBag.AllClinicSectionChoosenValues)
            {
                string name = clinicVal.PatientVariableVariableName;
                string[] all = name.Split(' ');
                string total = "";
                foreach (var s in all)
                {
                    total += s;
                }
                clinicVal.VariableNameForView = total + "New";
            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdNewPatientVariableModal.cshtml");
        }

        public ActionResult EditPatientVariableModal(string PatientId)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            ViewBag.AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(ClinicSectionId).Where(x => x.VariableStatusName == "VariableValueIsVariable" && (x.VariableDisplayName == "ShowInPatient" || x.VariableDisplayName == "ShowInVisitAndPatient")).OrderBy(x => x.PatientVariableVariableName);
            List<PatientVariablesValueViewModel> allPatientVariable = new List<PatientVariablesValueViewModel>();
            string[] PatientIds = PatientId.Split(',');
            foreach (var id in PatientIds)
            {
                Guid Id = new Guid(id);
                if (Id != Guid.Empty)
                    allPatientVariable.Add(_IDUNIT.patientVariablesValue.GetPatientVariablesValueBasedOnGuid(Id));
            }

            foreach (var clinicVal in ViewBag.AllClinicSectionChoosenValues)
            {
                string name = clinicVal.PatientVariableVariableName;
                string[] all = name.Split(' ');
                string total = "";
                foreach (var s in all)
                {
                    total += s;
                }
                clinicVal.VariableNameForView = total;
                if (allPatientVariable.SingleOrDefault(x => x.PatientVariableId == clinicVal.PatientVariableId) != null)
                {
                    clinicVal.Value = allPatientVariable.SingleOrDefault(x => x.PatientVariableId == clinicVal.PatientVariableId).Value;
                    clinicVal.PatientVariableValueGuid = allPatientVariable.SingleOrDefault(x => x.PatientVariableId == clinicVal.PatientVariableId).Guid;
                    clinicVal.VariableDate = allPatientVariable.SingleOrDefault(x => x.PatientVariableId == clinicVal.PatientVariableId).VariableInsertedDate;
                }

            }

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdNewPatientVariableModal.cshtml");
        }



        public JsonResult AddorUpdatePatentVariables(IEnumerable<PatientVariablesValueViewModel> VariablesValue, Guid PatientId)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            foreach (var variable in VariablesValue)
            {

                if (variable.Guid == Guid.Empty)
                {
                    variable.Guid = Guid.NewGuid();
                    variable.PatientId = PatientId;
                    variable.ClinicSectionId = ClinicSectionId;
                    _IDUNIT.patientVariablesValue.AddPatientVariablesValue(variable);
                }
                else
                {
                    PatientVariablesValueViewModel previouseValue = _IDUNIT.patientVariablesValue.GetPatientVariablesValueBasedOnGuid(variable.Guid);


                    previouseValue.Value = variable.Value;
                    _IDUNIT.patientVariablesValue.UpdatePatientVariablesValue(previouseValue);

                }
            }

            return Json(1);
        }


        public JsonResult AddorUpdatePatentVariablesForReception(IEnumerable<PatientVariablesValueViewModel> VariablesValue, Guid ReceptionId)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            List<PatientVariablesValueViewModel> allValues = new List<PatientVariablesValueViewModel>();
            foreach (var variable in VariablesValue)
            {

                if (variable.Guid == Guid.Empty)
                {
                    if (!String.IsNullOrWhiteSpace(variable.VariableInsertedDateYear))
                        variable.VariableInsertedDate = new DateTime(Convert.ToInt32(variable.VariableInsertedDateYear), Convert.ToInt32(variable.VariableInsertedDateMonth), Convert.ToInt32(variable.VariableInsertedDateDay), Convert.ToInt32(variable.VariableInsertedDateHour), Convert.ToInt32(variable.VariableInsertedDateMin), 0);
                    else
                        variable.VariableInsertedDate = DateTime.Now;
                    variable.PatientVariableId = _IDUNIT.patientVariable.GetPatientVariableIdByName(variable.PatientVariableVariableName);
                    variable.ClinicSectionId = ClinicSectionId;
                    variable.ReceptionId = ReceptionId;
                    allValues.Add(variable);

                }
            }
            _IDUNIT.patientVariablesValue.AddPatientVariablesValueRange(allValues);


            return Json(1);
        }


        public JsonResult DeletePatientVariableModal(string[] PatientId)
        {
            List<PatientVariablesValueViewModel> allPatientVariable = new List<PatientVariablesValueViewModel>();

            foreach (var id in PatientId)
            {
                Guid Id = new Guid(id);
                if (Id != Guid.Empty)
                    allPatientVariable.Add(_IDUNIT.patientVariablesValue.GetPatientVariablesValueBasedOnGuid(Id));
            }

            _IDUNIT.patientVariablesValue.RemoveRange(allPatientVariable);

            return Json(1);
        }

        public ActionResult Async_Save(string file, string fileName, Guid patientId, Guid? visitId)
        {

            PatientImageViewModel patientImageViewModel = null;
            FileAttachments fileAttachments = new FileAttachments();
            try
            {
                var fileId = Guid.NewGuid();
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = fileId + ".png";
                }

                var date = DateTime.Now;
                var url = $"\\Uploads\\main\\{date.Year}\\{date.Month}\\{date.Day}\\";
                var basePath = Path.Combine(_hostingEnvironment.WebRootPath + url);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var extension = Path.GetExtension(fileName);
                var fullFileName = fileId + extension;

                var filePath = Path.Combine(basePath, fullFileName);

                string[] base64 = file.Split(',');
                byte[] fileByte = Convert.FromBase64String(base64[1]);
                MemoryStream ms = new MemoryStream(fileByte);

                IFormFile mainFile = new FormFile(ms, 0, fileByte.Length, fullFileName, fullFileName);
                var thumPath = fileAttachments.CreateThumbnail(mainFile, _hostingEnvironment.WebRootPath, fullFileName);

                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        stream.Position = 0;
                        mainFile.CopyTo(stream);
                    }

                    patientImageViewModel = new PatientImageViewModel
                    {
                        Guid = fileId,
                        PatientId = patientId,
                        VisitId = visitId,
                        ThumbNailAddress = thumPath,
                        FileName = fullFileName,
                        ImageAddress = url + fullFileName,
                        ImageDateTime = date,
                        AttachmentTypeId = _IDUNIT.baseInfo.GetIdByNameAndType("OtherAttachment", "AttachmentType")
                    };

                    _IDUNIT.patientImage.AddPatientImage(patientImageViewModel);
                }

                return Json(1);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                if (patientImageViewModel != null)
                    fileAttachments.DeleteFile(patientImageViewModel);

                return Json(0);
            }

        }

        public JsonResult GetAllPatientImages(Guid patientId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> patientImages = _IDUNIT.patientImage.GetAllPatientImages(patientId).OrderByDescending(x => x.Id);
                return Json(patientImages);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllPatients()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<PatientViewModel> allPatient = _IDUNIT.patient.GetAllClinicPatients(ClinicSectionId);
                return Json(allPatient);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult RemovePatientImage(Guid patientImageId)
        {
            try
            {

                _IDUNIT.patientImage.RemovePatientImage(patientImageId, _hostingEnvironment.WebRootPath);

                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public ActionResult ChartModal(Guid patientId)
        {

            ChartViewModel Chart = new();
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            IEnumerable<ClinicSectionChoosenValueViewModel> AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllNumericClinicSectionChoosenValues(ClinicSectionId);


            Chart.AllClinicSectionChoosenValues = AllClinicSectionChoosenValues;

            foreach (var clinicVal in Chart.AllClinicSectionChoosenValues)
            {
                string name = clinicVal.PatientVariableVariableName;
                string[] all = name.Split(' ');
                string total = "";
                foreach (var s in all)
                {
                    total += s;
                }
                clinicVal.VariableNameForView = total;
            }


            IEnumerable<PatientVariablesValueViewModel> patientConstantVariablesValue = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(patientId);

            PatientViewModel patient = _IDUNIT.patient.GetPatient(patientId);

            List<DateTime?> AllDates = patientConstantVariablesValue.Select(x => x.VariableInsertedDate).Distinct().OrderBy(x => x.Value).ToList();
            List<string> alldate = new List<string>();
            Chart.Category = alldate;
            foreach (var date in AllDates)
            {
                var Year = (date.Value.Year - patient.DateOfBirth.Value.Year);
                var Month = (date.Value.Month - patient.DateOfBirth.Value.Month);
                if (Month < 0 || (Month == 0 && date.Value.Day < patient.DateOfBirth.Value.Day))
                {
                    Year--;
                    Month = 12 + Month;
                }

                string cat = date.Value.ToShortDateString() + "\nyear: " + Year + "\nmonth: " + Month;
                Chart.Category.Add(cat);
            }

            List<List<ChartViewModel>> allValues = new List<List<ChartViewModel>>();

            Chart.AllValues = allValues;

            foreach (var cli in AllClinicSectionChoosenValues)
            {
                List<ChartViewModel> chartList = new List<ChartViewModel>();
                IEnumerable<PatientVariablesValueViewModel> valueList = patientConstantVariablesValue.Where(x => x.PatientVariableId == cli.PatientVariableId).OrderBy(x => x.VariableInsertedDate);

                foreach (var val in valueList)
                {
                    ChartViewModel chart = new();
                    chart.Date = val.VariableInsertedDate;
                    chart.Value = Convert.ToDecimal(val.Value);
                    chart.Name = val.PatientVariableVariableName;
                    chart.Unit = val.PatientVariableVariableUnit;
                    chartList.Add(chart);
                }
                Chart.AllValues.Add(chartList);
            }
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/mdPatientVariablesChartModal.cshtml", Chart);
        }

        public JsonResult GetAllPatientVariableValues(Guid patientId)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<PatientVariablesValueViewModel> patientConstantVariablesValue = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(patientId);
                IEnumerable<ClinicSectionChoosenValueViewModel> AllClinicSectionChoosenValues = _IDUNIT.clinicSectionChoosenValue.GetAllNumericClinicSectionChoosenValues(ClinicSectionId);

                List<DateTime?> AllDates = patientConstantVariablesValue.Select(x => x.VariableInsertedDate).Distinct().ToList();

                List<List<ChartViewModel>> allValues = new List<List<ChartViewModel>>();

                foreach (var cli in AllClinicSectionChoosenValues)
                {
                    List<ChartViewModel> chartList = new List<ChartViewModel>();
                    IEnumerable<PatientVariablesValueViewModel> valueList = patientConstantVariablesValue.Where(x => x.PatientVariableId == cli.PatientVariableId).ToList();
                    int i = 1;
                    foreach (var val in valueList)
                    {
                        ChartViewModel chart = new();
                        chart.Date = val.VariableInsertedDate;
                        chart.Value = Convert.ToDecimal(val.Value);
                        chart.Name = val.PatientVariableVariableName;
                        chart.Year = i;
                        chartList.Add(chart);
                        i++;
                    }
                    allValues.Add(chartList);
                }
                return Json(allValues);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetPatientsAnalysisResults()
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Patient/PatientAnalysisResults.cshtml");
        }
    }
}

