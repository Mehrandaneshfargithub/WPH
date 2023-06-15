using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH;
using WPH.Models.CustomDataModels.Visit;
using Microsoft.AspNetCore.Http;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientMedicine;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.Visit_Patient_Disease;
using WPH.Models.CustomDataModels.PrescrptionDetail;
using WPH.Models.CustomDataModels.PrescriptionTest;
using WPH.Models.PatientImage;
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WPH.Models.Visit;
using System.Drawing;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.ReceptionClinicSection;
using WPH.Models.Reception;
using System.Globalization;

namespace WPH.Controllers.Visit
{
    [SessionCheck]
    public class VisitController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<VisitController> _logger;
        private readonly IConfiguration _configuration;

        public VisitController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, IWebHostEnvironment hostingEnvironment, ILogger<VisitController> logger, IConfiguration configuration)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _configuration = configuration;
        }

        ///////////////////////////////////////////////////////////////////////Visit Forms

        public async Task<IActionResult> Form()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                _IDUNIT.medicine.GetModalsViewBags(ViewBag);

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("BaseInfoSub");
                ViewBag.AccessEditBaseInfo = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteBaseInfo = access.Any(p => p.AccessName == "Delete");


                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "PatientPhoneNumberRequired", "UseAnalysisInClinic", "ShowMedicineForDiseaseInChiefComplain");
                string pat = "";
                try { pat = sval?.FirstOrDefault(p => p.ShowSName == "PatientPhoneNumberRequired")?.SValue; } catch { }

                bool pat1 = false;
                try { pat1 = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseAnalysisInClinic")?.SValue ?? "false"); } catch { }

                ViewBag.UseAnalysis = pat1;

                HttpContext.Session.SetString("PatientPhoneRequired", pat ?? "flase");

                string ShowMedicineForDiseaseInChiefComplain = "";
                try { ShowMedicineForDiseaseInChiefComplain = sval?.FirstOrDefault(p => p.ShowSName == "ShowMedicineForDiseaseInChiefComplain")?.SValue; } catch { }

                if (ShowMedicineForDiseaseInChiefComplain == "true")
                    ViewBag.ShowMedicineForDisease = true;

                IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
                ViewBag.TestId = test.FirstOrDefault(x => x.EName == "Test").Guid;
                ViewBag.JobId = test.FirstOrDefault(x => x.EName == "Job").Guid;

                ViewBag.VisitNumber = 0;

                var accessOtherAnalysis = _IDUNIT.subSystem.GetUserSubSystemAccess("CanSendAnalysisToOtherClinicSection");
                ViewBag.OtherAnalysis = Convert.ToBoolean(accessOtherAnalysis.Any(p => p.AccessName == "View" && p.SubSystemName == "CanSendAnalysisToOtherClinicSection"));

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/NewVisit.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        //////reception
        public JsonResult GetAllTodayVisitsForVisit()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                DateTime Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                IEnumerable<VisitViewModel> visitDetailTodayList = _IDUNIT.visit.GetAllVisitForOneDayBasedOnDoctorIdJustStatusAndVisitNum(userId, Today);

                return Json(visitDetailTodayList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetAllVisitDisease(Guid visitId)
        {
            try
            {
                var allDiseaseForVisitList = _IDUNIT.visit.GetAllVisitDiseaseWithJustDiseaseID(visitId).ToList();
                return Json(allDiseaseForVisitList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllVisitSymptom(Guid visitId)
        {
            try
            {
                var allSymptomForVisitList = _IDUNIT.visit.GetAllVisitSymptomWithJustSymptomID(visitId).ToList();
                return Json(allSymptomForVisitList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        //////reception
        public async Task<JsonResult> GetTodayVisitAsync(int visitNumber, int current)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            DateTime Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            VisitViewModel visit = _IDUNIT.visit.GetTodayTheVisitThatMustVisitingByDoctorId(userId, Today, visitNumber);

            var sval2 = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseFormNumber").FirstOrDefault();

            string useform = "";
            try
            {
                useform = sval2?.SValue;
            }
            catch { }

            if (useform == "true")
                visit.ReserveDetail.Patient.FileNum = visit.ReserveDetail.Patient.FormNumber;

            return Json(visit);
        }

        public async Task<IActionResult> ChangeVisit(int visitNumber, int current)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("BaseInfoSub");
            ViewBag.AccessEditBaseInfo = access.Any(p => p.AccessName == "Edit");
            ViewBag.AccessDeleteBaseInfo = access.Any(p => p.AccessName == "Delete");

            _IDUNIT.medicine.GetModalsViewBags(ViewBag);

            IEnumerable<BaseInfoTypeViewModel> test = _IDUNIT.baseInfo.GetAllBaseInfoType();
            ViewBag.TestId = test.FirstOrDefault(x => x.EName == "Test").Guid;
            ViewBag.JobId = test.FirstOrDefault(x => x.EName == "Job").Guid;

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "ShowMedicineForDiseaseInChiefComplain", "UseAnalysisInClinic");

            string ShowMedicineForDiseaseInChiefComplain = "";
            try { ShowMedicineForDiseaseInChiefComplain = sval?.FirstOrDefault(p => p.ShowSName == "ShowMedicineForDiseaseInChiefComplain")?.SValue; } catch { }

            if (ShowMedicineForDiseaseInChiefComplain == "true")
                ViewBag.ShowMedicineForDisease = true;

            bool pat1 = false;
            try { pat1 = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseAnalysisInClinic")?.SValue); } catch { }

            ViewBag.UseAnalysis = pat1;

            ViewBag.VisitNumber = visitNumber;

            var accessOtherAnalysis = _IDUNIT.subSystem.GetUserSubSystemAccess("CanSendAnalysisToOtherClinicSection");
            ViewBag.OtherAnalysis = Convert.ToBoolean(accessOtherAnalysis.Any(p => p.AccessName == "View" && p.SubSystemName == "CanSendAnalysisToOtherClinicSection"));

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/NewVisit.cshtml");
        }

        public ActionResult GetAllNormalDiseaseForPatient([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PatientDiseaseRecordViewModel> AllMeds = _IDUNIT.patientDisease.GetAllDiseaseForPatientByType(Id, "NormalDisease").ToList();
                Indexing<PatientDiseaseRecordViewModel> indexing = new Indexing<PatientDiseaseRecordViewModel>();
                indexing.AddIndexing(AllMeds);

                return Json(AllMeds.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllAllergicDiseaseForPatient([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PatientDiseaseRecordViewModel> AllMeds = _IDUNIT.patientDisease.GetAllDiseaseForPatientByType(Id, "AllergicDisease").ToList();
                Indexing<PatientDiseaseRecordViewModel> indexing = new Indexing<PatientDiseaseRecordViewModel>();
                indexing.AddIndexing(AllMeds);

                return Json(AllMeds.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllSocialDiseaseForPatient([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PatientDiseaseRecordViewModel> AllMeds = _IDUNIT.patientDisease.GetAllDiseaseForPatientByType(Id, "SocialDisease").ToList();
                Indexing<PatientDiseaseRecordViewModel> indexing = new Indexing<PatientDiseaseRecordViewModel>();
                indexing.AddIndexing(AllMeds);

                return Json(AllMeds.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllMedicinesRecordForPatient([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PatientMedicineRecordViewModel> AllMeds = _IDUNIT.patientMedicine.GetAllMedicineRecordForPatientGrid(Id);
                Indexing<PatientMedicineRecordViewModel> indexing = new Indexing<PatientMedicineRecordViewModel>();
                indexing.AddIndexing(AllMeds);

                return Json(AllMeds.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllVisitsForPatient([DataSourceRequest] DataSourceRequest request, Guid Id, DateTime visitDate)
        {
            try
            {
                List<VisitViewModel> visitDetailList = _IDUNIT.visit.GetAllPatientVisitByClinicSection(Id).OrderByDescending(x => x.VisitDate).ToList();

                visitDetailList = visitDetailList.Where(x => x.VisitDate.Value.Date != visitDate.Date).ToList();
                foreach (VisitViewModel visit in visitDetailList)
                {
                    visit.AllPrescriptionDetail = _IDUNIT.visit.GetAllPrescriptionForHistory(visit.Guid);
                    visit.AllPrescriptionTestDetail = _IDUNIT.visit.GetAllPrescriptionTestForHistory(visit.Guid);

                }
                Indexing<VisitViewModel> indexing = new Indexing<VisitViewModel>();
                indexing.AddIndexing(visitDetailList);
                return Json(visitDetailList.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllVisitsForPatientInReserve([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<VisitViewModel> visitDetailList = _IDUNIT.visit.GetAllPatientVisitByClinicSection(Id).OrderByDescending(x => x.VisitDate).ToList();
                Indexing<VisitViewModel> indexing = new Indexing<VisitViewModel>();
                indexing.AddIndexing(visitDetailList);

                return Json(visitDetailList.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetAllMedicine()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<MedicineForVisitViewModel> AllMeds = _IDUNIT.medicine.GetAllMedicinesForVisitPrescription(clinicSectionId);
                var jsonResult = Json(AllMeds);

                return jsonResult;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }


        }

        public ActionResult GetAllMedicineForDisease([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                IEnumerable<Medicine_DiseaseViewModel> medicines = _IDUNIT.disease.GetAllMedicinesForDisease(Id);
                return Json(medicines.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetAllVisitDiseasePatient(Guid visitId)
        {
            try
            {
                List<Visit_Patient_DiseaseViewModel> visitDisease = _IDUNIT.visit.GetAllVisitDisease(visitId);
                List<Guid> AllDiseaseForVisitList = new List<Guid>();
                for (int i = 0; i < visitDisease.Count; i++)
                {
                    AllDiseaseForVisitList.Add(visitDisease[i].DiseaseId);
                }


                return Json(AllDiseaseForVisitList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult GetMedicineForDisease(string[] diseaseId, Guid visitId)
        {
            try
            {
                Visit_Patient_DiseaseViewModel visitDiseasePatient = new();
                visitDiseasePatient.DiseaseId = new Guid(diseaseId[0]);
                visitDiseasePatient.Guid = Guid.NewGuid();
                visitDiseasePatient.VisitId = visitId;

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                List<MedicineViewModel> AllMeds = new List<MedicineViewModel>();
                List<Medicine_DiseaseViewModel> medicines = new List<Medicine_DiseaseViewModel>();
                Guid medId = Guid.Empty;
                if (diseaseId[0] != "")
                    for (int i = 0; i < diseaseId.Length; i++)
                    {
                        medId = new Guid(diseaseId[i]);
                        medicines = _IDUNIT.disease.GetAllMedicinesForDisease(medId).ToList();
                        foreach (Medicine_DiseaseViewModel medfordi in medicines)
                        {
                            MedicineViewModel med = _IDUNIT.medicine.GetMedicine(medfordi.MedicineId);
                            PrescriptionDetailViewModel pre = _IDUNIT.prescription.GetAllPrescriptionDetai(visitId).Where(x => x.ClinicSectionId == ClinicSectionId && x.MedicineId == medfordi.MedicineId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (pre != null)
                            {
                                med.Num = pre.Num;
                                med.Consumption = pre.ConsumptionInstruction;
                                med.Explanation = pre.Explanation;
                            }

                            if (!AllMeds.Contains(med))
                            {
                                AllMeds.Add(med);
                            }
                        }
                    }

                return Json(AllMeds);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }


        }

        public JsonResult RemoveDiseaseFromVisit(string diseaseId, Guid visitId)
        {
            try
            {
                _IDUNIT.visit.RemoveDiseaseFromVisit(diseaseId, visitId);
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

        public JsonResult AddSymptomToVisit(Guid symptomId, Guid visitId)
        {
            try
            {
                _IDUNIT.visit.AddSymptomToVisit(symptomId, visitId);
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


        public JsonResult RemoveSymptomFromVisit(Guid symptomId, Guid visitId)
        {
            try
            {
                _IDUNIT.visit.RemoveSymptomFromVisit(symptomId, visitId);
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


        public JsonResult EventChangeStatus(Guid visitId)
        {
            try
            {

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                VisitViewModel visit = _IDUNIT.visit.GetVisitById(visitId);
                int statu = _IDUNIT.baseInfo.GetBaseInfoGeneralByName("Visited");
                visit.ReserveDetail.StatusId = statu;

                _IDUNIT.reserveDetail.UpdateReserveDetailStatus(visit.ReserveDetail);

                visit.StatusId = statu;
                visit.CreateUserId = userId;
                _IDUNIT.visit.UpdateVisit(visit);

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


        public ActionResult MedicineForDiseaseModal(Guid diseaseId, Guid visitId)
        {
            try
            {
                Visit_Patient_DiseaseViewModel vpd = new();
                vpd.DiseaseId = diseaseId;
                vpd.VisitId = visitId;
                _IDUNIT.visit.AddVisitDiseasePatient(vpd);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/dgMedicineForDiseaseGrid.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult AddDiseaseToVisit(Guid diseaseId, Guid visitId)
        {
            try
            {
                Visit_Patient_DiseaseViewModel vpd = new();
                vpd.DiseaseId = diseaseId;
                vpd.VisitId = visitId;
                _IDUNIT.visit.AddVisitDiseasePatient(vpd);
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

        public JsonResult UpdateMedicineInVisit(Guid prescriptionId, string change, string exp)
        {
            try
            {
                PrescriptionDetailViewModel prescription = _IDUNIT.prescription.GetPrescriptionDetailById(prescriptionId);

                if (change == "")
                {
                    change = null;
                }
                if (exp == "amount")
                {
                    prescription.Num = change;
                }
                else if (exp == "consume")
                {
                    prescription.ConsumptionInstruction = change;
                }
                else
                {
                    prescription.Explanation = change;
                }
                prescription.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.ModifiedDate = DateTime.Now;
                _IDUNIT.prescription.UpdatePrescriptionDetail(prescription);
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

        public JsonResult DeleteMedicineInVisit(Guid Id)
        {
            try
            {
                PrescriptionDetailViewModel medicine = _IDUNIT.prescription.GetPrescriptionDetailById(Id);
                _IDUNIT.prescription.RemovePrescriptionDetail(Id);
                _IDUNIT.medicine.UpdateMedicineNum(Id, medicine.MedicineId, medicine.Num, "Increase");
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

        public JsonResult GetAllTest()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> AllTest = _IDUNIT.baseInfo.GetAllBaseInfos("Test", ClinicSectionId);

                return Json(AllTest);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        [AcceptVerbs]
        public ActionResult PrescriptionDetail_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionDetailViewModel> products)
        {
            var results = new List<PrescriptionDetailViewModel>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.AddPrescriptionDetail(product, "");
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }


        public JsonResult AddMedicineToPrescription(PrescriptionDetailViewModel viewModel)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                viewModel.ClinicSectionId = clinicSectionId;
                viewModel.CreatedUserId = userId;
                viewModel.ModifiedUserId = userId;
                viewModel.CreatedDate = DateTime.Now;
                _IDUNIT.prescription.AddPrescriptionDetail(viewModel, "Dicrease");
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

        public JsonResult UpdateMedicineInPrescription(PrescriptionDetailViewModel viewModel)
        {
            try
            {
                _IDUNIT.medicine.UpdateMedicineNum(viewModel.Guid, viewModel.MedicineId, viewModel.Num, "Update");
                _IDUNIT.prescription.UpdateMedicineInPrescription(viewModel);

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

        [AcceptVerbs]
        public ActionResult PrescriptionDetail_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionDetailViewModel> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.UpdatePrescriptionDetail(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs]
        public ActionResult PrescriptionDetail_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionDetailViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.RemovePrescriptionDetail(product.Guid);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs]
        public ActionResult PrescriptionTest_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionTestDetailViewModel> products)
        {
            var results = new List<PrescriptionTestDetailViewModel>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.AddPrescriptionTest(product);
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> AddTestToPrescriptionTest(PrescriptionTestDetailViewModel test)
        {
            try
            {
                test.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var sval1 = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(test.ClinicSectionId, "UseAnalysisInClinic").FirstOrDefault();
                bool pat1 = false;
                try
                {
                    pat1 = bool.Parse(sval1.SValue);
                }
                catch { }

                if (pat1)
                {
                    test.TestId = null;
                    test.TestName = null;
                }
                else
                {
                    test.AnalysisName = null;
                }

                test.CreatedUserId = userId;
                test.ModifiedUserId = userId;
                test.CreatedDate = DateTime.Now;
                _IDUNIT.prescription.AddPrescriptionTest(test);
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


        [AcceptVerbs]
        public ActionResult PrescriptionTest_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionTestDetailViewModel> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.UpdatePrescriptionTest(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs]
        public ActionResult PrescriptionTest_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<PrescriptionTestDetailViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    _IDUNIT.prescription.RemovePrescriptionTest(product.Guid);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }


        public JsonResult AddAnalysisToVisit(Guid VisitId, Guid? AnalysisId, Guid? AnalysisItemId, Guid ClinicSectionId)
        {
            Guid originalclinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

            if(AnalysisId != null && AnalysisItemId != null)
            {
                AnalysisItemId = null;
            }

            try
            {
                PatientReceptionAnalysisViewModel newAnalysis = new PatientReceptionAnalysisViewModel()
                {
                    AnalysisId = AnalysisId,
                    AnalysisItemId = AnalysisItemId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = userId,
                    ReceptionId = VisitId
                };

                _IDUNIT.patientReceptionAnalysis.AddNewPatientReceptionAnalysis(newAnalysis, originalclinicSectionId);

                var RCS = _IDUNIT.receptionClinicSection.GetAllReceptionClinicSectionByReceptionId(VisitId);

                if(!RCS.Any())
                {
                    var reception = _IDUNIT.reception.GetReception(VisitId);

                    Guid parentClinicSectionId = _IDUNIT.clinicSection.GetClinicSectionById(ClinicSectionId).ParentId??Guid.Empty;

                    ReceptionViewModel patientReception = new ReceptionViewModel()
                    {
                        AutoPay = false,
                        BaseCurrencyId = 11,
                        ClinicSectionId = ClinicSectionId,
                        ClinicId = Guid.Parse(HttpContext.Session.GetString("ClinicId")),
                        ClinicSectionName = HttpContext.Session.GetString("ClinicSectionName"),
                        CreatedDate = DateTime.Now,
                        Discount = 0,
                        DiscountCurrencyId = 11,
                        CreatedUserId = userId,
                        Patient = new PatientViewModel
                        {
                            DateOfBirth = reception.Patient.DateOfBirth,
                            //ClinicSectionId = ClinicSectionId,
                            GenderId = reception.Patient.User.GenderId,
                            Name = reception.Patient.User.Name,
                            PhoneNumber = reception.Patient.User.PhoneNumber,
                            UserName = "123",
                            Pass1 = "123"
                        },
                        ReceptionDate = DateTime.Now,
                        UserId = userId,
                        ReceptionNum = reception.ReceptionNum,
                        OrginalClinicSectionId = parentClinicSectionId
                    };

                    PatientReceptionAnalysisViewModel Analysis = new PatientReceptionAnalysisViewModel()
                    {
                        AnalysisId = AnalysisId,
                        AnalysisItemId = AnalysisItemId,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = userId,
                        
                    };

                    patientReception.PatientReceptionAnalyses = new List<PatientReceptionAnalysisViewModel>();

                    patientReception.PatientReceptionAnalyses.Add(Analysis);

                    ReceptionClinicSectionViewModel RCS2 = new ReceptionClinicSectionViewModel()
                    {
                        ClinicSectionId = ClinicSectionId,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = userId,
                        ReceptionId = VisitId,
                        
                    };

                    patientReception.ReceptionClinicSectionDestinations = new List<ReceptionClinicSectionViewModel>();
                    patientReception.ReceptionClinicSectionDestinations.Add(RCS2);
                    _IDUNIT.patientReception.AddOrUpdate(patientReception);
                    
                }
                else
                {

                    PatientReceptionAnalysisViewModel Analysis = new PatientReceptionAnalysisViewModel()
                    {
                        AnalysisId = AnalysisId,
                        AnalysisItemId = AnalysisItemId,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = userId,
                        ReceptionId = RCS.FirstOrDefault().DestinationReceptionId??Guid.Empty
                    };
                    _IDUNIT.patientReceptionAnalysis.AddNewPatientReceptionAnalysis(Analysis, ClinicSectionId);
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


        public JsonResult UpdateTestInVisit(Guid prescriptionId, string change)
        {
            try
            {
                PrescriptionTestDetailViewModel prescription = _IDUNIT.prescription.GetPrescriptionTestById(prescriptionId);
                if (change == "")
                {
                    change = null;
                }
                prescription.Explanation = change;
                prescription.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.ModifiedDate = DateTime.Now;

                _IDUNIT.prescription.UpdatePrescriptionTest(prescription);
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

        public JsonResult UpdateVisitVariables(Guid? Vi, string Property, Guid VisitId, string Value, Guid PatientId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                VisitViewModel visit = _IDUNIT.visit.GetVisitById(VisitId);
                if (Property == "Explanation")
                {

                    visit.CreateUserId = userId;
                    visit.Explanation = Value;
                    _IDUNIT.visit.UpdateVisit(visit);
                    return Json(1);
                }

                if (Property == "BirthDay" || Property == "BirthDayDate")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(PatientId);
                    string[] date = Value.Split('/');
                    DateTime da = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), 0, 0, 0);
                    patient.DateOfBirth = da;
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);

                }
                else if (Property == "Gender")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(PatientId);
                    patient.GenderId = Convert.ToInt32(Value);
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);
                }

                else if (Property == "FatherJob")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(PatientId);
                    patient.FatherJobId = new Guid(Value);
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);
                }

                else if (Property == "MotherJob")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(PatientId);
                    patient.MotherJobId = new Guid(Value);
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);
                }


                if (Vi == null)
                {
                    PatientVariablesValueViewModel inserted = new PatientVariablesValueViewModel
                    {
                        ClinicSectionId = clinicSectionId,
                        Guid = Guid.NewGuid(),
                        PatientId = PatientId,
                        PatientVariableId = Convert.ToInt32(Property),
                        Value = Value,
                        VariableInsertedDate = visit.VisitDate,
                        VisitId = VisitId
                    };
                    _IDUNIT.patientVariablesValue.AddPatientVariablesValue(inserted);
                    return Json(inserted.Guid);
                }

                else
                {
                    PatientVariablesValueViewModel previouseValue = _IDUNIT.patientVariablesValue.GetPatientVariablesValueBasedOnGuid(Vi ?? Guid.Empty);
                    previouseValue.Value = Value;
                    _IDUNIT.patientVariablesValue.UpdatePatientVariablesValue(previouseValue);
                    return Json(Vi);
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



        public JsonResult UpdateVisitConstantVariables(string property, Guid visitId, string amount, Guid patientId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                if (amount == "")
                {
                    return Json(1);
                }


                if (property == "BirthDay")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(patientId);
                    patient.DateOfBirth = DateTime.Parse(amount);
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);

                }
                else if (property == "Gender")
                {
                    PatientViewModel patient = _IDUNIT.patient.GetPatient(patientId);
                    patient.GenderId = Convert.ToInt32(amount);
                    _IDUNIT.patient.UpdatePatient(patient);
                    return Json(1);
                }



                IEnumerable<PatientVariablesValueViewModel> patientVariablesValue = _IDUNIT.patientVariablesValue.GetAllPatientVariablesValueBasedOnPatientId(patientId);
                PatientVariablesValueViewModel previouseValue = patientVariablesValue.SingleOrDefault(x => x.PatientVariableId == Convert.ToInt32(property));

                if (previouseValue != null)
                {
                    previouseValue.Value = amount;
                    _IDUNIT.patientVariablesValue.UpdatePatientVariablesValue(previouseValue);
                }
                else
                {
                    PatientVariablesValueViewModel inserted = new PatientVariablesValueViewModel
                    {
                        ClinicSectionId = clinicSectionId,
                        Guid = Guid.NewGuid(),
                        PatientId = patientId,
                        PatientVariableId = Convert.ToInt32(property),
                        Value = amount,
                        VariableInsertedDate = DateTime.Now
                    };
                    _IDUNIT.patientVariablesValue.AddPatientVariablesValue(inserted);
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


        public JsonResult DeleteTestInVisit(Guid Id)
        {


            _IDUNIT.prescription.RemovePrescriptionTest(Id);
            return Json(1);
        }

        public JsonResult DeleteOtherAnalysisInVisit(Guid Id)
        {


            _IDUNIT.patientReceptionAnalysis.RemovePatientReceptionAnalysis(Id);
            return Json(1);
        }


        public ActionResult GetAllVisitPrescriptionDetail([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PrescriptionDetailViewModel> prescriptionDetailList = _IDUNIT.prescription.GetAllPrescriptionDetai(Id);
                Indexing<PrescriptionDetailViewModel> indexing = new Indexing<PrescriptionDetailViewModel>();
                indexing.AddIndexing(prescriptionDetailList);

                return Json(prescriptionDetailList.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetAllVisitPrescriptionTest([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PrescriptionTestDetailViewModel> prescriptionDetailList = _IDUNIT.prescription.GetAllPrescriptonTests(Id);
                Indexing<PrescriptionTestDetailViewModel> indexing = new Indexing<PrescriptionTestDetailViewModel>();
                indexing.AddIndexing(prescriptionDetailList);

                return Json(prescriptionDetailList.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }


        }

        public ActionResult GetAllVisitPrescriptionOtherAnalysis([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                List<PrescriptionTestDetailViewModel> prescriptionDetailList = _IDUNIT.prescription.GetAllVisitPrescriptionOtherAnalysis(Id);
                Indexing<PrescriptionTestDetailViewModel> indexing = new Indexing<PrescriptionTestDetailViewModel>();
                indexing.AddIndexing(prescriptionDetailList);

                return Json(prescriptionDetailList.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }


        }


        public async Task<ActionResult> GetPrescription(Guid Id)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sval1 = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseAnalysisInClinic").FirstOrDefault();
            bool pat1 = false;
            try
            {
                pat1 = bool.Parse(sval1.SValue);
            }
            catch { }

            ViewBag.UseAnalysis = pat1;

            var visit = _IDUNIT.visit.GetVisitBasedOnReserveDetailId(Id);

            if (visit == null)
                return Json("DoNotHaveVisit");

            var accessOtherAnalysis = _IDUNIT.subSystem.GetUserSubSystemAccess("CanSendAnalysisToOtherClinicSection");
            ViewBag.OtherAnalysis = Convert.ToBoolean(accessOtherAnalysis.Any(p => p.AccessName == "View" && p.SubSystemName == "CanSendAnalysisToOtherClinicSection"));


            return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/mdPrescriptionModal.cshtml", visit.Guid);

        }

        public IActionResult GetPrescriptionByReceptionId(Guid receptionId)
        {
            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            var sval1 = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseAnalysisInClinic").FirstOrDefault();
            bool pat1 = false;
            try
            {
                pat1 = bool.Parse(sval1.SValue);
            }
            catch { }

            ViewBag.UseAnalysis = pat1;

            var visit = _IDUNIT.visit.GetVisitBasedOnReceptionId(receptionId);

            if (visit == null)
                return Json("DoNotHaveVisit");

            var accessOtherAnalysis = _IDUNIT.subSystem.GetUserSubSystemAccess("CanSendAnalysisToOtherClinicSection");
            ViewBag.OtherAnalysis = Convert.ToBoolean(accessOtherAnalysis.Any(p => p.AccessName == "View" && p.SubSystemName == "CanSendAnalysisToOtherClinicSection"));


            ViewBag.ShowHistory = true;

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Reserve/mdPrescriptionModal.cshtml", receptionId);
        }


        public JsonResult AddMedicineToVisit(Guid visitId, Guid medicineId)
        {
            try
            {
                PrescriptionDetailViewModel pre = _IDUNIT.prescription.GetLastMedicinePrescription(medicineId);
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                PrescriptionDetailViewModel prescription = new PrescriptionDetailViewModel();

                if (pre != null)
                {
                    prescription.ConsumptionInstruction = pre.ConsumptionInstruction;
                    prescription.Num = pre.Num;
                    prescription.Explanation = pre.Explanation;
                }
                prescription.Guid = Guid.NewGuid();

                prescription.MedicineId = medicineId;
                prescription.VisitId = visitId;
                prescription.ClinicSectionId = ClinicSectionId;
                prescription.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.CreatedDate = DateTime.Now;


                _IDUNIT.prescription.AddPrescriptionDetail(prescription, "");
                return Json(prescription);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult AddMedicineDiseaseToVisit(Guid[] medicineDisease, Guid visitId)
        {
            try
            {
                List<PrescriptionDetailViewModel> allPre = new List<PrescriptionDetailViewModel>();
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                foreach (Guid md in medicineDisease)
                {
                    PrescriptionDetailViewModel prescription = new PrescriptionDetailViewModel();
                    PrescriptionDetailViewModel pre = _IDUNIT.prescription.GetLastMedicinePrescription(md);

                    if (pre != null)
                    {
                        prescription.ConsumptionInstruction = pre.ConsumptionInstruction;
                        prescription.Num = pre.Num;
                        prescription.Explanation = pre.Explanation;
                    }

                    prescription.Guid = Guid.NewGuid();

                    prescription.MedicineId = md;
                    prescription.VisitId = visitId;
                    prescription.ClinicSectionId = ClinicSectionId;
                    allPre.Add(prescription);
                }

                _IDUNIT.prescription.AddPrescriptionDetailRange(allPre);
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

        public JsonResult AddTestToVisit(Guid visitId, Guid testId)
        {
            try
            {
                PrescriptionTestDetailViewModel prescription = new PrescriptionTestDetailViewModel();
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                prescription.Guid = Guid.NewGuid();

                prescription.TestId = testId;
                prescription.VisitId = visitId;
                prescription.ClinicSectionId = ClinicSectionId;
                prescription.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.ModifiedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                prescription.CreatedDate = DateTime.Now;


                _IDUNIT.prescription.AddPrescriptionTest(prescription);
                return Json(prescription);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        ///////////////////////////////////////////////////////////////////////////////////Reports

        private async Task<StiReport> MedicinesReport(Guid visitId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            StiReport report = new StiReport();
            string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
            string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
            Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
            Stimulsoft.Base.StiFontCollection.AddFontFile(font2);

            CultureInfo cultures = new CultureInfo("en-US");
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "VisitPrescription.mrt");

            report.Load(path);
            var visit = _IDUNIT.visit.GetVisitForReportById(visitId);
            List<PrescriptionDetailViewModel> AllVisitPrescription = _IDUNIT.prescription.GetAllPrescriptionDetai(visit.Guid).ToList();
            report.Dictionary.Variables["Doctor"].Value = _localizer["Doctor"];
            report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
            report.Dictionary.Variables["PrescriptionNumber"].Value = _localizer["PrescriptionNumber"];
            report.Dictionary.Variables["VisitDate"].Value = _localizer["VisitDate"];
            report.Dictionary.Variables["Age"].Value = _localizer["Age"];
            report.Dictionary.Variables["DoctorSignature"].Value = _localizer["DoctorSignature"];


            report.Dictionary.Variables["vDoctor"].Value = visit.DoctorName;
            report.Dictionary.Variables["Explanation"].Value = visit.Explanation;
            report.Dictionary.Variables["vPatientName"].Value = visit.PatientName;
            report.Dictionary.Variables["UniqueVisitNum"].Value = visit.UniqueVisitNum;
            report.Dictionary.Variables["vVisitDate"].Value = visit.VisitDate.Value.ToString("dd/MM/yyyy", cultures);
            try
            {
                report.Dictionary.Variables["vAge"].Value = visit.DateOfBirth.GetAge()?.ToString();
            }
            catch { }


            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "LabPhoneNumber", "LabAddress", "LabDesc", "LabName");

            string LabPhoneNumber = "";
            try { LabPhoneNumber = sval?.FirstOrDefault(p => p.ShowSName == "LabPhoneNumber")?.SValue; } catch { }

            string LabAddress = "";
            try { LabAddress = sval?.FirstOrDefault(p => p.ShowSName == "LabAddress")?.SValue; } catch { }

            string LabDesc = "";
            try { LabDesc = sval?.FirstOrDefault(p => p.ShowSName == "LabDesc")?.SValue; } catch { }

            string LabName = "";
            try { LabName = sval?.FirstOrDefault(p => p.ShowSName == "LabName")?.SValue; } catch { }

            report.Dictionary.Variables["PhoneNumber"].Value = LabPhoneNumber;
            report.Dictionary.Variables["Address"].Value = LabAddress;
            report.Dictionary.Variables["Explanation"].Value = LabDesc;
            report.Dictionary.Variables["vDoctor"].Value = LabName;

            string rootPath = _hostingEnvironment.WebRootPath;
            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + visit.LogoAddress));
                report.Dictionary.Variables["Logo"].ValueObject = (Image)banner;
            }
            catch { }

            report.RegBusinessObject("PrescriptionDetail", AllVisitPrescription);
            return report;
        }

        public async Task<IActionResult> VisitPrescriptionWithmedicinesPrint(Guid visitId)
        {
            try
            {
                StiReport report = await MedicinesReport(visitId);
                report.Render();
                List<byte[]> allb = new List<byte[]>();

                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });

                    allb.Add(stream.ToArray());
                }


                var jsonResult = Json(new { allb });
                return jsonResult;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        private async Task<StiReport> TestReport(Guid visitId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            StiReport report = new StiReport();
            string font1 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
            string font2 = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
            Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
            Stimulsoft.Base.StiFontCollection.AddFontFile(font2);

            CultureInfo cultures = new CultureInfo("en-US");
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Content", "Reports", "VisitPrescriptionTest.mrt");

            report.Load(path);
            var visit = _IDUNIT.visit.GetVisitForReportById(visitId);
            List<PrescriptionTestDetailViewModel> AllVisitPrescription = _IDUNIT.prescription.GetAllPrescriptonTests(visit.Guid).ToList();
            report.Dictionary.Variables["Doctor"].Value = _localizer["Dcotor"];
            report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
            report.Dictionary.Variables["PrescriptionNumber"].Value = _localizer["PrescriptionNumber"];
            report.Dictionary.Variables["VisitDate"].Value = _localizer["VisitDate"];
            report.Dictionary.Variables["Age"].Value = _localizer["Age"];
            report.Dictionary.Variables["DoctorSignature"].Value = _localizer["DoctorSignature"];


            report.Dictionary.Variables["vDoctor"].Value = visit.DoctorName;
            report.Dictionary.Variables["Explanation"].Value = visit.Explanation;
            report.Dictionary.Variables["vPatientName"].Value = visit.PatientName;
            report.Dictionary.Variables["UniqueVisitNum"].Value = visit.UniqueVisitNum;
            report.Dictionary.Variables["vVisitDate"].Value = visit.VisitDate.Value.ToString("dd/MM/yyyy", cultures);
            try
            {
                report.Dictionary.Variables["vAge"].Value = visit.DateOfBirth.GetAge()?.ToString();
            }
            catch { }


            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "LabPhoneNumber", "LabAddress");

            string LabPhoneNumber = "";
            try { LabPhoneNumber = sval?.FirstOrDefault(p => p.ShowSName == "LabPhoneNumber")?.SValue; } catch { }

            string LabAddress = "";
            try { LabAddress = sval?.FirstOrDefault(p => p.ShowSName == "LabAddress")?.SValue; } catch { }


            report.Dictionary.Variables["PhoneNumber"].Value = LabPhoneNumber;
            report.Dictionary.Variables["Address"].Value = LabAddress;

            string rootPath = _hostingEnvironment.WebRootPath;
            try
            {
                Bitmap banner = new Bitmap(Path.Combine(rootPath + visit.LogoAddress));
                report.Dictionary.Variables["Logo"].ValueObject = (Image)banner;
            }
            catch { }

            report.RegBusinessObject("Test", AllVisitPrescription);
            return report;
        }


        public async Task<IActionResult> VisitPrescriptionTestPrint(Guid visitId)
        {
            try
            {
                StiReport report = await TestReport(visitId);
                report.Render();
                List<byte[]> allb = new List<byte[]>();


                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });

                    allb.Add(stream.ToArray());
                }


                var jsonResult = Json(new { allb });
                return jsonResult;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public async Task<IActionResult> TotalVisit()
        {
            ViewBag.AccessDeleteTotalVisits = _IDUNIT.subSystem.CheckUserAccess("Delete", "TotalVisits");

            Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            ViewBag.periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
            ViewBag.FromToId = (int)Periods.FromDateToDate;
            ViewBag.FirstPeriodId = (int)Periods.Day;

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseFormNumber").FirstOrDefault();

            string useform = sval?.SValue ?? "";

            if (useform == "true")
                ViewBag.useform = true;
            else
                ViewBag.useform = false;

            return PartialView("/Views/Shared/PartialViews/AppWebForms/TotalVisits/wpTotalVisitsForm.cshtml");
        }

        public ActionResult GetAllVisits([DataSourceRequest] DataSourceRequest request, int periodId, DateTime dateFrom, DateTime dateTo, Guid doctorId)
        {
            try
            {

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var user = _IDUNIT.user.GetUserWithRole(userId);

                DateTime fromDate = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                DateTime toDate = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);


                if (user.UserTypeName.ToLower() == "doctor")
                {

                    List<VisitViewModel> allVisit = _IDUNIT.visit.GetAllVisitForSpecificDateByDoctorId(userId, periodId, fromDate, toDate);

                    return Json(allVisit.ToDataSourceResult(request));
                }
                else
                {
                    List<Guid> doctors = new List<Guid>();

                    if (doctorId == Guid.Empty)
                    {
                        Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                        var clinicSectionAccess = _IDUNIT.clinicSection.GetAllClinicSectionsChild(clinicSectionId, userId);

                        var doctorList = _IDUNIT.doctor.GetDoctorsBasedOnUserSection(clinicSectionAccess.Select(p => p.Guid).ToList());
                        doctors.AddRange(doctorList.Select(p => p.Guid).ToList());
                    }
                    else
                    {
                        doctors.Add(doctorId);
                    }

                    List<VisitViewModel> allVisit = _IDUNIT.visit.GetAllVisitForSpecificDateBasedOnUserAccess(doctors, periodId, fromDate, toDate);

                    return Json(allVisit.ToDataSourceResult(request));
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }


        public async Task<IActionResult> VisitTodayList()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(ClinicSectionId, "UseFormNumber").FirstOrDefault();

                string useform = sval?.SValue ?? "";

                if (useform == "true")
                    ViewBag.useform = true;
                else
                    ViewBag.useform = false;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/dgTodayVisitListGrid.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public ActionResult GetAllTodayVisits([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                DateTime time = DateTime.Now;
                DateTime today = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                IEnumerable<VisitViewModel> allVisit = _IDUNIT.visit.GetAllVisitForOneDayBasedOnDoctorId(userId, today);

                return Json(allVisit.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "TotalVisits");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.visit.RemoveVisit(Id);

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


        public JsonResult GetAllVisitImages(Guid visitId)
        {
            try
            {
                IEnumerable<PatientImageViewModel> visitImages = _IDUNIT.patientImage.GetAllVisitImages(visitId);
                return Json(visitImages);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult SendVisitToServer(Guid visitId)
        {
            try
            {
                string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);

                var result = _IDUNIT.visit.SendVisitToServer(visitId, baseurl);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("0");
            }
        }

        public JsonResult SendAnalysisToServer(Guid visitId)
        {
            try
            {
                string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);

                var result = _IDUNIT.visit.SendAnalysisToServer(visitId, baseurl);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("0");
            }
        }

        public ActionResult ShowAnalysisFromServerModal()
        {

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/dgAnalysisFromServerGrid.cshtml");
        }

        public ActionResult ShowVisitFromServerModal()
        {

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/dgMedicineFromServerGrid.cshtml");
        }

        public IActionResult GetVisitFromServer(Guid visitId)
        {
            try
            {
                string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);

                var result = _IDUNIT.visit.GetVisitFromServer(visitId, baseurl);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("0");
            }
        }

        public IActionResult GetAnalysisFromServerForReception(long? analysisServerVisitNum, string nameMobile)
        {
            try
            {
                string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);

                var result = _IDUNIT.visit.GetAnalysisFromServerByNameMobile(baseurl, analysisServerVisitNum, nameMobile);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("0");
            }
        }
        public IActionResult GetAnalysisFromServer(Guid visitId)
        {
            try
            {
                string apiServer = _configuration.GetValue<string>("ConnectionStrings:ServerConnection");
                string baseurl = ConnectionStringDecrypt.Decrypt(apiServer);

                var result = _IDUNIT.visit.GetAnalysisFromServerByVisitId(visitId, baseurl);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("0");
            }
        }


        public async Task<IActionResult> PayVisitModal(Guid reserveDetailId)
        {
            var has_visit = _IDUNIT.visit.CheckVisitExistByReserveDetailId(reserveDetailId);
            if (has_visit == null)
                return Json("NoVisit");

            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("PayVisit", "Service", "SubStoreroom", "CanConsumeProduct", "CanConsumeService", "PayService");
            ViewBag.AccessNewPayVisit = access.Any(p => p.AccessName == "New" && p.SubSystemName == "PayVisit");
            ViewBag.AccessDeletePayVisit = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayVisit");

            ViewBag.AccessNewPayService = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PayService");
            ViewBag.AccessDeletePayService = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayService");

            ViewBag.AccessNewService = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Service");
            ViewBag.AccessNewMedicineProduct = access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");
            ViewBag.CanConsumeProduct = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanConsumeProduct");
            ViewBag.CanConsumeService = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanConsumeService");

            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "VisitPrice").FirstOrDefault();
            ViewBag.VisitPrice = sval?.SValue ?? "1";

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/mdPayVisitModal.cshtml", has_visit);
        }

        public async Task<IActionResult> PayVisitContainer(Guid visitId)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("PayVisit", "Service", "SubStoreroom", "CanConsumeProduct", "CanConsumeService", "PayService");
            var AccessNewPayVisit = access.Any(p => p.AccessName == "New" && p.SubSystemName == "PayVisit");
            var AccessDeletePayVisit = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayVisit");

            ViewBag.AccessNewPayService = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PayService");
            ViewBag.AccessDeletePayService = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayService");

            var AccessNewService = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Service");
            var AccessNewMedicineProduct = access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");
            var CanConsumeProduct = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanConsumeProduct");
            var CanConsumeService = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanConsumeService");

            if (!AccessNewPayVisit && !CanConsumeProduct && !CanConsumeService)
                return Json("0");

            ViewBag.AccessNewPayVisit = AccessNewPayVisit;
            ViewBag.AccessDeletePayVisit = AccessDeletePayVisit;

            ViewBag.AccessNewService = AccessNewService;
            ViewBag.AccessNewMedicineProduct = AccessNewMedicineProduct;
            ViewBag.CanConsumeProduct = CanConsumeProduct;
            ViewBag.CanConsumeService = CanConsumeService;

            var visitPrice = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "VisitPrice").FirstOrDefault();
            ViewBag.VisitPrice = visitPrice?.SValue ?? "1";


            return PartialView("/Views/Shared/PartialViews/AppWebForms/Visit/mdPayVisitModal.cshtml", visitId);
        }

        public IActionResult AddPayVisit(PayVisitViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "PayVisit");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                viewModel.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var result = _IDUNIT.visit.AddPayVisit(viewModel);

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

        public IActionResult PayVisit(Guid receptionServiceId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "PayVisit");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var result = _IDUNIT.visit.PayVisit(receptionServiceId, userId);

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

        public JsonResult RemovePayVisit(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "PayVisit");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.receptionService.RemoveReceptionService(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("ERROR_SomeThingWentWrong");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayAllVisit(Guid receptionId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "PayVisit");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var result = _IDUNIT.visit.PayAllVisit(receptionId, userId);

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
    }
}