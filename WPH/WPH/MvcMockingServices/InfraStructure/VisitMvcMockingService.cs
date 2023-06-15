using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Analysis;
using WPH.Models.Cash;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.Visit;
using WPH.Models.CustomDataModels.Visit_Patient_Disease;
using WPH.Models.CustomDataModels.Visit_Symptom;
using WPH.Models.Medicine;
using WPH.Models.Reception;
using WPH.Models.ReceptionServiceReceived;
using WPH.Models.Visit;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class VisitMvcMockingService : IVisitMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idiunit;

        public VisitMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idiunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idiunit = idiunit;
        }

        public async Task<Guid> AddNewVisit(VisitViewModel viewModel)
        {
            try
            {
                viewModel.ReserveDetail = null;
                List<PatientVariablesValue> add = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModelsLists(viewModel.AddVariables);
                List<PatientVariablesValue> update = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModelsLists(viewModel.UpdatedVariables);
                Reception vis = ConvertFromViewModelToDto(viewModel);
                vis.CreatedDate = DateTime.Now;
                vis.Discount = 0;
                vis.ReceptionTypeId = _idiunit.baseInfo.GetIdByNameAndType("VisitReception", "ReceptionType");
                vis.ReceptionNum = _idiunit.patientReception.GetLatestReceptionInvoiceNum(viewModel.ClinicSectionId);

                _unitOfWork.Visits.AddNewVisit(vis, add, update);

                var images = _unitOfWork.PatientImage.Find(p => p.VisitId == viewModel.Guid);
                foreach (var item in images)
                {
                    item.ReceptionId = item.VisitId;
                    _unitOfWork.PatientImage.UpdateState(item);
                }

                var receptionService = _unitOfWork.ReceptionServices.HasDoctorVisit(vis.Guid);
                if (!receptionService)
                {
                    var sval = _idiunit.clinicSection.GetClinicSectionSettingValueBySettingName(viewModel.OriginalClinicSectionId, "VisitPrice").FirstOrDefault();

                    PayVisitViewModel payVisit = new PayVisitViewModel
                    {
                        ReceptionId = vis.Guid,
                        CreatedUserId = viewModel.CreateUserId.GetValueOrDefault(),
                        VisitPrice = decimal.Parse(sval?.SValue ?? "0")
                    };

                    AddPayVisit(payVisit);
                }
                _unitOfWork.Complete();
                return vis.Guid;
            }
            catch (Exception ex) { throw ex; }

        }

        public VisitViewModel GetVisitBasedOnReserveDetailId(Guid reserveDetailId)
        {
            try
            {
                Reception visits = _unitOfWork.Visits.GetVisitBasedOnReserveDetailId(reserveDetailId);
                return ConvertModelForReserve(visits);
            }
            catch (Exception ex) { throw ex; }
        }
        
        public VisitViewModel GetVisitBasedOnReceptionId(Guid receptionId)
        {
            try
            {
                Reception visits = _unitOfWork.Visits.GetVisitDetailBasedOnId(receptionId);
                return ConvertModelForReserve(visits);
            }
            catch (Exception ex) { throw ex; }
        }


        public IEnumerable<VisitViewModel> GetAllVisitForOneDayBasedOnDoctorId(Guid doctorId, DateTime Date)
        {
            try
            {
                IEnumerable<Reception> visitList = _unitOfWork.Visits.GetAllVisitForOneDayBasedOnDoctorId(doctorId, Date);
                return ConvertModelsLists(visitList);
            }
            catch (Exception e) { throw e; }
        }


        public IEnumerable<VisitViewModel> GetAllPatientVisitByClinicSection(Guid PatientId)
        {
            IEnumerable<Reception> visitList = _unitOfWork.Visits.GetAllPatientVisitByClinicSection(PatientId);
            return ConvertReceptionToVisitViewModelLists(visitList);
        }


        public VisitViewModel GetVisitById(Guid visitId)
        {

            Reception preDto = _unitOfWork.Visits.GetVisitById(visitId);
            return ConvertModel(preDto);
        }

        public VisitForReportViewModel GetVisitForReportById(Guid visitId)
        {
            Reception preDto = _unitOfWork.Visits.GetVisitForReportById(visitId);
            return ConvertModelReport(preDto);
        }

        public IEnumerable<VisitViewModel> GetAllVisitForOneDayBasedOnDoctorIdJustStatusAndVisitNum(Guid doctorId, DateTime date)
        {
            try
            {
                IEnumerable<VisitViewModel> visitList = _unitOfWork.Receptions.GetAllReceptionForOneDayBasedOnDoctorIdJustStatusAndVisitNum(doctorId, date)
                    .Select(p => new VisitViewModel
                    {
                        VisitNum = p.VisitNum.Value,
                        StatusName = p.Status.Name
                    });
                return visitList;
            }
            catch (Exception e) { throw e; }
        }

        public VisitViewModel GetTodayTheVisitThatMustVisitingByDoctorId(Guid doctorId, DateTime today, int visitNum = 0)
        {
            try
            {
                Reception result = _unitOfWork.Receptions.GetTodayReceptionThatMustVisitingByDoctorId(doctorId, today, false, visitNum);

                if (result == null)
                    result = _unitOfWork.Receptions.GetTodayReceptionThatMustVisitingByDoctorId(doctorId, today, true, visitNum);

                return ConvertReceptionToVisitViewModel(result);
            }
            catch (Exception e) { throw e; }
        }

        public List<VisitViewModel> GetAllVisitForSpecificDateByDoctorId(Guid doctorId, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
            }


            List<Reception> visitDtos = _unitOfWork.Visits.GetAllVisitForSpecificDateByDoctorId(doctorId, dateFrom, dateTo).ToList();

            List<VisitViewModel> visits = ConvertModelsLists(visitDtos).ToList();
            Indexing<VisitViewModel> indexing = new Indexing<VisitViewModel>();
            return indexing.AddIndexing(visits);
        }

        public List<VisitViewModel> GetAllVisitForSpecificDateBasedOnUserAccess(List<Guid> doctors, int periodId, DateTime dateFrom, DateTime dateTo)
        {
            if (periodId != (int)Periods.FromDateToDate)
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
            }


            List<Reception> visitDtos = _unitOfWork.Visits.GetAllVisitForSpecificDateBasedOnUserAccess(doctors, dateFrom, dateTo).ToList();

            List<VisitViewModel> visits = ConvertModelsLists(visitDtos).ToList();
            Indexing<VisitViewModel> indexing = new Indexing<VisitViewModel>();
            return indexing.AddIndexing(visits);
        }

        public IEnumerable<Guid> GetAllVisitDiseaseWithJustDiseaseID(Guid visitId)
        {
            IEnumerable<VisitPatientDisease> visitDisease = _unitOfWork.VisitDiseasePatients.GetAllVisitDiseaseWithJustDiseaseID(visitId);
            return visitDisease.Select(p => p.DiseaseId);
        }



        public List<Visit_SymptomViewModel> GetAllVisitSymptom(Guid visitId)
        {
            List<VisitSymptom> visitDisease = _unitOfWork.Visit_Symptoms.Find(x => x.ReceptionId == visitId).ToList();
            return Common.ConvertModels<Visit_SymptomViewModel, VisitSymptom>.convertModelsLists(visitDisease);
        }


        public IEnumerable<Guid> GetAllVisitSymptomWithJustSymptomID(Guid visitId)
        {
            IEnumerable<VisitSymptom> visitDisease = _unitOfWork.Visit_Symptoms.GetAllVisitSymptomWithJustSymptomID(visitId);
            return visitDisease.Select(p => p.SymptomId);
        }

        public void UpdateVisit(VisitViewModel viewModel)
        {
            try
            {
                viewModel.ReserveDetail = null;
                viewModel.Status = null;
                List<PatientVariablesValue> add = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModelsLists(viewModel.AddVariables);
                List<PatientVariablesValue> update = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModelsLists(viewModel.UpdatedVariables);

                Reception vis = _unitOfWork.Receptions.Get(viewModel.Guid);
                vis.StatusId = viewModel.StatusId;
                vis.ModifiedDate = DateTime.Now;
                vis.ModifiedUserId = viewModel.CreateUserId;
                vis.Description = viewModel.Explanation;

                _unitOfWork.Visits.UpdateVisit(vis, add, update);
            }
            catch (Exception ex) { throw ex; }
        }

        public List<Visit_Patient_DiseaseViewModel> GetAllVisitDisease(Guid visitId)
        {
            List<VisitPatientDisease> visitDisease = _unitOfWork.VisitDiseasePatients.Find(x => x.ReceptionId == visitId).ToList();
            return Common.ConvertModels<Visit_Patient_DiseaseViewModel, VisitPatientDisease>.convertModelsLists(visitDisease);

        }

        public void RemoveVisitDiseasePatientRange(List<Visit_Patient_DiseaseViewModel> visitDisease)
        {
            List<VisitPatientDisease> visitDiseaseDto = Common.ConvertModels<VisitPatientDisease, Visit_Patient_DiseaseViewModel>.convertModelsLists(visitDisease);
            _unitOfWork.VisitDiseasePatients.RemoveRange(visitDiseaseDto);
            _unitOfWork.Complete();
        }

        public void AddVisitDiseasePatientRange(List<Visit_Patient_DiseaseViewModel> visitDisease)
        {
            List<VisitPatientDisease> visitDiseaseDto = Common.ConvertModels<VisitPatientDisease, Visit_Patient_DiseaseViewModel>.convertModelsLists(visitDisease);
            foreach (VisitPatientDisease v in visitDiseaseDto)
            {
                v.Guid = Guid.NewGuid();
            }
            _unitOfWork.VisitDiseasePatients.AddRange(visitDiseaseDto);
            _unitOfWork.Complete();
        }

        public void RemoveDiseaseFromVisit(string diseaseId, Guid visitId)
        {
            VisitPatientDisease Visit_Patient_Disease = _unitOfWork.VisitDiseasePatients.Find(x => x.DiseaseId == new Guid(diseaseId) && x.ReceptionId == visitId).SingleOrDefault();
            _unitOfWork.VisitDiseasePatients.Remove(Visit_Patient_Disease);
            _unitOfWork.Complete();
        }

        public void AddSymptomToVisit(Guid symptomId, Guid visitId)
        {
            VisitSymptom vs = new VisitSymptom
            {
                SymptomId = symptomId,
                ReceptionId = visitId
            };

            _unitOfWork.Visit_Symptoms.Add(vs);
            _unitOfWork.Complete();
        }

        public void RemoveSymptomFromVisit(Guid symptomId, Guid visitId)
        {
            VisitSymptom Visit_Symptom = _unitOfWork.Visit_Symptoms.Find(x => x.SymptomId == symptomId && x.ReceptionId == visitId).SingleOrDefault();
            _unitOfWork.Visit_Symptoms.Remove(Visit_Symptom);
            _unitOfWork.Complete();
        }

        public Guid AddVisitDiseasePatient(Visit_Patient_DiseaseViewModel vpd)
        {
            try
            {
                VisitPatientDisease Visit_Patient_Disease = Common.ConvertModels<VisitPatientDisease, Visit_Patient_DiseaseViewModel>.convertModels(vpd);
                Visit_Patient_Disease.ReceptionId = vpd.VisitId;

                _unitOfWork.VisitDiseasePatients.Add(Visit_Patient_Disease);
                _unitOfWork.Complete();
                return Visit_Patient_Disease.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public OperationStatus RemoveVisit(Guid VisitId)
        {
            try
            {
                _unitOfWork.Visits.RemoveVisit(VisitId);
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }

        public string GetAllPrescriptionForHistory(Guid VisitId)
        {
            IEnumerable<PrescriptionDetail> visitDiseaseDto = _unitOfWork.PrescriptionDetails.GetAllPrescriptionDetail(VisitId);
            string allPrescription = "";
            foreach (PrescriptionDetail pre in visitDiseaseDto)
            {
                allPrescription = allPrescription + pre.Medicine.JoineryName + " , ";
            }
            return allPrescription;
        }

        public string GetAllPrescriptionTestForHistory(Guid VisitId)
        {
            IEnumerable<PrescriptionTestDetail> visitDiseaseDto = _unitOfWork.PrescriptionTests.GetAllPrescriptionTestDetail(VisitId);
            string allPrescription = "";
            foreach (PrescriptionTestDetail pre in visitDiseaseDto)
            {
                allPrescription = allPrescription + pre.Test.Name + " , ";
            }
            return allPrescription;
        }


        public void UpdateReceptionNums(Guid doctorId)
        {
            _unitOfWork.Visits.UpdateReceptionNums(doctorId);
        }

        public async Task UpdateVisitStatus(VisitViewModel viewModel)
        {
            var reception = _unitOfWork.Visits.GetVisitWithReserveDetailId(viewModel.Guid);
            reception.StatusId = viewModel.StatusId;
            reception.ReceptionDate = viewModel.VisitDate;

            _unitOfWork.Visits.UpdateVisitStatus(reception);

            var status = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == reception.StatusId);
            if (!reception.ReserveDetail.LastVisit.GetValueOrDefault(false) && (status.Name == "Visited" || status.Name == "Visiting" || status.Name == "InQueue"))
            {
                var receptionService = _unitOfWork.ReceptionServices.HasDoctorVisit(viewModel.Guid);
                if (!receptionService)
                {
                    var sval = _idiunit.clinicSection.GetClinicSectionSettingValueBySettingName(viewModel.OriginalClinicSectionId, "VisitPrice").FirstOrDefault();

                    PayVisitViewModel payVisit = new PayVisitViewModel
                    {
                        ReceptionId = viewModel.Guid,
                        CreatedUserId = viewModel.ModifiedUserId.GetValueOrDefault(),
                        VisitPrice = decimal.Parse(sval?.SValue ?? "0")
                    };

                    AddPayVisit(payVisit);
                }
            }

            _unitOfWork.Complete();
        }

        public string SendVisitToServer(Guid visitId, string baseurl)
        {
            //baseurl = "http://localhost:63809/api/";
            var visit = _unitOfWork.Receptions.GetMedicineReceptionForServer(visitId);
            if (visit == null)
                return "VisitNotExists";

            if (!visit.PrescriptionDetails.Any())
                return "EmptyMedicines";

            if (string.IsNullOrWhiteSpace(visit.ReserveDetail?.Patient?.User?.PhoneNumber) && string.IsNullOrWhiteSpace(visit.Patient?.User?.PhoneNumber))
                return "EmptyPatientMobile";

            var send = new SendVisitToServerViewModel()
            {
                MasterId = visit.ServerVisitNum.GetValueOrDefault(0),
                PatientName = (visit.ReserveDetail == null)? visit.Patient?.User?.Name : visit.ReserveDetail?.Patient?.User?.Name,
                DoctorName = visit.ReserveDetail?.Doctor?.User?.Name ?? visit.ClinicSection?.Name,
                Mobile = (visit.ReserveDetail == null) ? visit.Patient?.User?.PhoneNumber : visit.ReserveDetail.Patient.User.PhoneNumber,
                AddMedicines = visit.PrescriptionDetails.Select(p => new SendMedicineToServerViewModel
                {
                    Number = p.Num,
                    ConsumptionInstruction = p.ConsumptionInstruction,
                    Explanation = p.Explanation,
                    MedicineName = p.Medicine?.ScientificName ?? p.Medicine?.JoineryName ?? "",
                    MedicineForm = p.Medicine?.MedicineForm?.Name ?? ""
                }).ToList()
            };


            try
            {
                string jsonMedicine = JsonConvert.SerializeObject(send);

                using (var client = new HttpClient())
                {
                    string Baseurl = $"{baseurl}TblMaster";

                    var data = new StringContent(jsonMedicine, Encoding.UTF8, "application/json");

                    if (send.MasterId == 0)
                    {
                        var responseTask = client.PostAsync(Baseurl, data);
                        responseTask.Wait();

                        var apiResult = responseTask.Result;
                        if (apiResult.IsSuccessStatusCode)
                        {
                            var serverId = apiResult.Content.ReadAsStringAsync().Result;

                            var updateVisit = _unitOfWork.Visits.Get(visitId);

                            updateVisit.ServerVisitNum = long.Parse(serverId);

                            _unitOfWork.Visits.UpdateState(updateVisit);
                            _unitOfWork.Complete();

                            return serverId;
                        }
                        else
                        {
                            var message = apiResult.Content.ReadAsStringAsync().Result;

                            return message;
                        }
                    }
                    else
                    {
                        Baseurl += "/Update";
                        var responseTask = client.PostAsync(Baseurl, data);
                        responseTask.Wait();

                        var apiResult = responseTask.Result;
                        if (apiResult.IsSuccessStatusCode)
                        {
                            var serverId = apiResult.Content.ReadAsStringAsync().Result;

                            return serverId;
                        }
                        else
                        {
                            var message = apiResult.Content.ReadAsStringAsync().Result;

                            return message;
                        }
                    }
                }
            }
            catch (Exception e) { return "CantConnectToServer"; }
        }

        public string SendAnalysisToServer(Guid visitId, string baseurl)
        {
            var visit = _unitOfWork.Receptions.GetAnalysisReceptionForServer(visitId);
            if (visit == null)
                return "VisitNotExists";

            if (!visit.PrescriptionTestDetails.Any())
                return "EmptyAnalysis";

            if (string.IsNullOrWhiteSpace(visit.ReserveDetail.Patient.User.PhoneNumber))
                return "EmptyPatientMobile";

            var send = new SendVisitToServerViewModel()
            {
                MasterId = visit.AnalysisServerVisitNum.GetValueOrDefault(0),
                PatientName = visit.ReserveDetail?.Patient?.User?.Name,
                DoctorName = visit.ReserveDetail?.Doctor?.User?.Name ?? visit.ClinicSection?.Name,
                Mobile = visit.ReserveDetail.Patient.User.PhoneNumber,
                AddAnalysis = visit.PrescriptionTestDetails.Select(p => new SendAnalysisToServerViewModel
                {
                    AnalysisName = p.AnalysisName ?? p.Test.Name,
                    Description = p.Explanation,
                }).ToList()
            };


            try
            {
                string jsonMedicine = JsonConvert.SerializeObject(send);

                using (var client = new HttpClient())
                {
                    string Baseurl = $"{baseurl}AnalysisMaster";

                    var data = new StringContent(jsonMedicine, Encoding.UTF8, "application/json");

                    if (send.MasterId == 0)
                    {
                        var responseTask = client.PostAsync(Baseurl, data);
                        responseTask.Wait();

                        var apiResult = responseTask.Result;
                        if (apiResult.IsSuccessStatusCode)
                        {
                            var serverId = apiResult.Content.ReadAsStringAsync().Result;

                            var updateVisit = _unitOfWork.Visits.Get(visitId);

                            updateVisit.AnalysisServerVisitNum = long.Parse(serverId);

                            _unitOfWork.Visits.UpdateState(updateVisit);
                            _unitOfWork.Complete();

                            return serverId;
                        }
                        else
                        {
                            var message = apiResult.Content.ReadAsStringAsync().Result;

                            return message;
                        }
                    }
                    else
                    {

                        var responseTask = client.PutAsync(Baseurl, data);
                        responseTask.Wait();

                        var apiResult = responseTask.Result;
                        if (apiResult.IsSuccessStatusCode)
                        {
                            var serverId = apiResult.Content.ReadAsStringAsync().Result;

                            return serverId;
                        }
                        else
                        {
                            var message = apiResult.Content.ReadAsStringAsync().Result;

                            return message;
                        }
                    }
                }
            }
            catch (Exception e) { return "CantConnectToServer"; }
        }

        public GetVisitFromServerViewModel GetVisitFromServer(Guid visitId, string baseurl)
        {
            var visit = _unitOfWork.Visits.GetVisitById(visitId);

            var result = new GetVisitFromServerViewModel();

            if (visit == null)
            {
                result.Message = "NotFound";
                return result;
            }

            if (visit.ServerVisitNum.GetValueOrDefault(0) == 0)
            {
                result.Message = "MedicinesNotSend";
                return result;
            }

            string patientName = (visit.ReserveDetail == null)? visit.Patient.User.Name : visit.ReserveDetail.Patient.User.Name;

            try
            {
                using (var client = new HttpClient())
                {
                    string Baseurl = $"{baseurl}TblMaster";

                    var responseTask = client.GetStringAsync($"{Baseurl}/{visit.ServerVisitNum}/{patientName}").Result;

                    if (responseTask == "0")
                    {
                        result.Message = "ServerMedicinesNotSend";
                        return result;
                    }
                    else if (responseTask == "SomthingWentWrong")
                    {
                        result.Message = "SomthingWentWrong";
                        return result;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<GetVisitFromServerViewModel>(responseTask);
                        result.Message = "Success";

                        return result;
                    }

                }
            }
            catch (Exception)
            {
                result.Message = "CantConnectToServer";
                return result;
            }
        }


        public GetVisitFromServerViewModel GetAnalysisFromServerByVisitId(Guid visitId, string baseurl)
        {
            var visit = _unitOfWork.Visits.GetVisitById(visitId);

            var result = new GetVisitFromServerViewModel();

            if (visit == null)
            {
                result.Message = "NotFound";
                return result;
            }

            if (visit.AnalysisServerVisitNum.GetValueOrDefault(0) == 0)
            {
                result.Message = "AnalysisNotSend";
                return result;
            }

            string patientName = visit.ReserveDetail.Patient.User.Name;

            result = GetAnalysisFromServerByNameMobile(baseurl, visit.AnalysisServerVisitNum, patientName);

            return result;
        }

        public GetVisitFromServerViewModel GetAnalysisFromServerByNameMobile(string baseurl, long? analysisServerVisitNum, string patientMobileName)
        {
            var result = new GetVisitFromServerViewModel();

            try
            {
                using (var client = new HttpClient())
                {
                    string Baseurl = $"{baseurl}AnalysisMaster";

                    var responseTask = client.GetStringAsync($"{Baseurl}/{analysisServerVisitNum}/{patientMobileName}").Result;

                    if (responseTask == "0")
                    {
                        result.Message = "ServerAnalysisNotSend";
                        return result;
                    }
                    else if (responseTask == "SomthingWentWrong")
                    {
                        result.Message = "SomthingWentWrong";
                        return result;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<GetVisitFromServerViewModel>(responseTask);
                        result.Message = "Success";

                        return result;
                    }

                }
            }
            catch (Exception)
            {
                result.Message = "CantConnectToServer";
                return result;
            }
        }


        public Guid? CheckVisitExistByReserveDetailId(Guid reserveDetailId)
        {
            var result = _unitOfWork.Visits.CheckVisitExistByReserveDetailId(reserveDetailId);
            return result;
        }

        public string AddPayVisit(PayVisitViewModel viewModel)
        {
            if (viewModel.ReceptionId == null)
                return "ERROR_Data";

            var today = DateTime.Now;
            var serviceId = _unitOfWork.Services.GetServiceByName(null, "DoctorVisit")?.Guid;
            ReceptionService service = new()
            {
                ServiceId = serviceId,
                ReceptionId = viewModel.ReceptionId,
                Number = 1,
                Price = viewModel.VisitPrice,
                StatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus"),
                CreatedDate = today,
                ServiceDate = today,
                CreatedUserId = viewModel.CreatedUserId
            };

            if (serviceId == null || serviceId == Guid.Empty)
            {
                service.Service = new Service
                {
                    Name = "DoctorVisit",
                    Price = 1,
                    TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType")
                };
            }

            _unitOfWork.ReceptionServices.Add(service);
            _unitOfWork.Complete();

            return service.Guid.ToString();
        }

        public string PayVisit(Guid receptionServiceId, Guid userId)
        {
            var reception = _unitOfWork.ReceptionServices.GetReceptionServiceWithPatient(receptionServiceId);
            if (reception == null)
                return "InvalidReceptionService";

            if (reception.Status.Name == "Paid")
                return "";

            ReceptionServiceReceivedViewModel vm = new ReceptionServiceReceivedViewModel
            {
                ReceptionServiceId = receptionServiceId,
                Amount = (reception.Number * reception.Price),
                ReceptionInvoiceNum = reception.Reception.ReceptionNum,
                PayerName = reception.Reception.Patient.User.Name,
                AmountStatus = false,
                CreatedUserId = userId,
            };

            var result = _idiunit.receptionServiceReceived.PayService(vm,false);
            return result;
        }

        public string PayAllVisit(Guid receptionId, Guid userId)
        {
            var reception = _unitOfWork.Receptions.GetReceptionWithPatient(receptionId);
            if (reception == null)
                return "InvalidReception";

            if (reception.Status.Name == "Paid")
                return "";

            PayAllServiceViewModel vm = new PayAllServiceViewModel
            {
                ReceptionInvoiceNum = reception.ReceptionNum,
                PayerName = reception.Patient.User.Name,
                UserId = userId,
                ReceptionId = receptionId,
                Insurance = 0
            };

            var result = _idiunit.receptionServiceReceived.PayAllServices(vm,false);
            return result;
        }

        ////////////////////////////////////////////////////////////////////Converts
        ///

        public static Reception ConvertVisitViewModelToDto(VisitViewModel ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VisitViewModel, Reception>();
                cfg.CreateMap<ReserveViewModel, Reserve>();
                cfg.CreateMap<ReserveDetailViewModel, ReserveDetail>();
                cfg.CreateMap<PatientViewModel, Patient>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<VisitViewModel, Reception>(ress);
        }


        public static List<VisitViewModel> ConvertReceptionToVisitViewModelLists(IEnumerable<Reception> ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitViewModel>()
                .ForMember(a => a.StatusName, b => b.MapFrom(c => c.Status.Name))
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.PrescriptionDetails, b => b.Ignore())
                .ForMember(a => a.PrescriptionTestDetails, b => b.Ignore())
                .ForMember(a => a.Visit_Symptom, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                ;
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                .ForMember(a => a.UserGenderName, b => b.MapFrom(c => c.User.Gender.Name))

                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<Reception>, List<VisitViewModel>>(ress);
        }

        public static List<VisitViewModel> ConvertModelsLists(IEnumerable<Reception> ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitViewModel>()
                .ForMember(a => a.StatusName, b => b.MapFrom(c => c.Status.Name))
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.PrescriptionDetails, b => b.Ignore())
                .ForMember(a => a.PrescriptionTestDetails, b => b.Ignore())
                .ForMember(a => a.Visit_Symptom, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Reception, ReceptionViewModel>()
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                .ForMember(a => a.UserGenderName, b => b.MapFrom(c => c.User.Gender.Name))

                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<Reception>, List<VisitViewModel>>(ress);
        }

        public static VisitViewModel ConvertReceptionToVisitViewModel(Reception ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitViewModel>()
                .ForMember(a => a.PatientDateOfBirth, b => b.MapFrom(c => c.ReserveDetail.Patient.DateOfBirth))
                .ForMember(a => a.PatientId, b => b.MapFrom(c => c.ReserveDetail.PatientId))
                .ForMember(a => a.FileNum, b => b.MapFrom(c => c.ReserveDetail.Patient.FileNum))
                .ForMember(a => a.PatientName, b => b.MapFrom(c => c.ReserveDetail.Patient.User.Name))
                .ForMember(a => a.GenderId, b => b.MapFrom(c => c.ReserveDetail.Patient.User.GenderId))
                .ForMember(a => a.ReserveStartTime, b => b.MapFrom(c => c.ReserveDetail.ReserveStartTime))
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate.Value.Date))
                .ForMember(a => a.UniqueVisitNum, b => b.MapFrom(c => c.ReceptionNum))
                .ForMember(a => a.VisitTime, b => b.MapFrom(c => c.ReceptionDate.Value.TimeOfDay))
                .ForMember(a => a.Explanation, b => b.MapFrom(c => c.Description))

                .ForMember(a => a.PrescriptionDetails, b => b.Ignore())
                .ForMember(a => a.PrescriptionTestDetails, b => b.Ignore())
                .ForMember(a => a.Visit_Symptom, b => b.Ignore())
                .ForMember(a => a.Reception, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                //.ForMember(a => a.Patient, b => b.Ignore())
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(x => x.User.Name))
                .ForMember(a => a.GenderId, b => b.MapFrom(x => x.User.GenderId))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, VisitViewModel>(ress);
        }

        public static VisitForReportViewModel ConvertModelReport(Reception ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitForReportViewModel>()
                .ForMember(a => a.DoctorName, b => b.MapFrom(c => c.ReserveDetail.Doctor.User.Name))
                .ForMember(a => a.Explanation, b => b.MapFrom(c => c.ReserveDetail.Doctor.Explanation))
                .ForMember(a => a.LogoAddress, b => b.MapFrom(c => c.ReserveDetail.Doctor.LogoAddress))
                .ForMember(a => a.PatientName, b => b.MapFrom(c => c.ReserveDetail.Patient.User.Name))
                .ForMember(a => a.DateOfBirth, b => b.MapFrom(c => c.ReserveDetail.Patient.DateOfBirth))
                .ForMember(a => a.UniqueVisitNum, b => b.MapFrom(c => c.ReceptionNum))
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate))
                ;

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, VisitForReportViewModel>(ress);
        }
        
        public static VisitViewModel ConvertModel(Reception ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitViewModel>()
                .ForMember(a => a.PatientDateOfBirth, b => b.MapFrom(c => c.ReserveDetail.Patient.DateOfBirth))
                .ForMember(a => a.PatientId, b => b.MapFrom(c => c.ReserveDetail.PatientId))
                .ForMember(a => a.FileNum, b => b.MapFrom(c => c.ReserveDetail.Patient.FileNum))
                .ForMember(a => a.PatientName, b => b.MapFrom(c => c.ReserveDetail.Patient.User.Name))
                .ForMember(a => a.GenderId, b => b.MapFrom(c => c.ReserveDetail.Patient.User.GenderId))
                .ForMember(a => a.ReserveStartTime, b => b.MapFrom(c => c.ReserveDetail.ReserveStartTime))
                .ForMember(a => a.UniqueVisitNum, b => b.MapFrom(c => c.ReceptionNum))
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.Explanation, b => b.MapFrom(c => c.Description))
                .ForMember(a => a.PrescriptionDetails, b => b.Ignore())
                .ForMember(a => a.PrescriptionTestDetails, b => b.Ignore())
                .ForMember(a => a.Visit_Symptom, b => b.Ignore())
                .ForMember(a => a.Reception, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                //.ForMember(a => a.Patient, b => b.Ignore())
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Name, b => b.MapFrom(x => x.User.Name))
                .ForMember(a => a.GenderId, b => b.MapFrom(x => x.User.GenderId))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, VisitViewModel>(ress);
        }

        public static VisitViewModel ConvertModelForReserve(Reception ress)
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, VisitViewModel>()
                .ForMember(a => a.VisitDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.UniqueVisitNum, b => b.MapFrom(c => c.ReceptionNum))
                .ForMember(a => a.PrescriptionDetails, b => b.Ignore())
                .ForMember(a => a.PrescriptionTestDetails, b => b.Ignore())
                .ForMember(a => a.Visit_Patient_Disease, b => b.Ignore())
                .ForMember(a => a.Visit_Symptom, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                ;
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Status, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<Reception, VisitViewModel>(ress);
        }


        public static Reception ConvertFromViewModelToDto(VisitViewModel ress)
        {


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VisitViewModel, Reception>()
                .ForMember(a => a.ReceptionDate, b => b.MapFrom(c => c.VisitDate))

                ;
                cfg.CreateMap<PatientVariablesValueViewModel, PatientVariablesValue>()

                ;


            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<VisitViewModel, Reception>(ress);
        }


    }


}
