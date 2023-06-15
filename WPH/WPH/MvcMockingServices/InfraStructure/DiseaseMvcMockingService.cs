using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.Medicine;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DiseaseMvcMockingService : IDiseaseMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiseaseMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<DiseaseViewModel> GetAllDiseaseForListBox(Guid clinicSectionId, Guid patientId)
        {
            IEnumerable<Disease> DesDtos = _unitOfWork.Diseases.GetAllDiseaseForListBox(clinicSectionId, patientId);
            List<DiseaseViewModel> deses = ConvertModelsLists(DesDtos).ToList();
            Indexing<DiseaseViewModel> indexing = new Indexing<DiseaseViewModel>();
            return indexing.AddIndexing(deses);
        }

        public IEnumerable<DiseaseViewModel> GetAllDiseases(Guid clinicSectionId)
        {
            IEnumerable<Disease> DesDtos = _unitOfWork.Diseases.GetAllDiseases(clinicSectionId);
            List<DiseaseViewModel> deses = ConvertModelsLists(DesDtos).ToList();
            Indexing<DiseaseViewModel> indexing = new Indexing<DiseaseViewModel>();
            return indexing.AddIndexing(deses);
        }



        public OperationStatus RemoveDisease(Guid Diseaseid)
        {
            try
            {
                Disease des = _unitOfWork.Diseases.Get(Diseaseid);
                IEnumerable<MedicineDisease> medicineDiseases = _unitOfWork.MedicineDiseases.Find(x => x.DiseaseId == Diseaseid);
                IEnumerable<DiseaseSymptom> diseaseSymptom = _unitOfWork.Disease_Symptoms.Find(x => x.DiseaseId == Diseaseid);
                if (medicineDiseases.Any())
                    _unitOfWork.MedicineDiseases.RemoveRange(medicineDiseases);
                if (diseaseSymptom.Any())
                    _unitOfWork.Disease_Symptoms.RemoveRange(diseaseSymptom);
                _unitOfWork.Diseases.Remove(des);
                _unitOfWork.Complete();
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



        public string AddNewDisease(DiseaseViewModel disease)
        {
            if (string.IsNullOrWhiteSpace(disease.Name) || disease.DiseaseTypeId == null)
                return "DataNotValid";

            Disease diseaseDto = Common.ConvertModels<Disease, DiseaseViewModel>.convertModels(disease);

            if (disease.AllMedsForDisease != null && disease.AllMedsForDisease.Any())
            {
                diseaseDto.MedicineDiseases = new List<MedicineDisease>();
                foreach (var id in disease.AllMedsForDisease)
                {
                    MedicineDisease med = new()
                    {
                        MedicineId = id,
                    };

                    diseaseDto.MedicineDiseases.Add(med);
                }
            }

            if (disease.AllSymptomsForDisease != null && disease.AllSymptomsForDisease.Any())
            {
                diseaseDto.DiseaseSymptoms = new List<DiseaseSymptom>();
                foreach (var id in disease.AllSymptomsForDisease)
                {
                    DiseaseSymptom dise = new()
                    {
                        SymptomId = id
                    };

                    diseaseDto.DiseaseSymptoms.Add(dise);
                }
            }

            _unitOfWork.Diseases.Add(diseaseDto);
            _unitOfWork.Complete();
            return diseaseDto.Guid.ToString();
        }



        public string UpdateDisease(DiseaseViewModel disease)
        {
            if (disease.Guid == Guid.Empty || string.IsNullOrWhiteSpace(disease.Name) || disease.DiseaseTypeId == null)
                return "DataNotValid";

            Disease diseaseDto = Common.ConvertModels<Disease, DiseaseViewModel>.convertModels(disease);

            IEnumerable<MedicineDisease> medicineDiseases = _unitOfWork.MedicineDiseases.Find(x => x.DiseaseId == diseaseDto.Guid);
            if (medicineDiseases.Any())
                _unitOfWork.MedicineDiseases.RemoveRange(medicineDiseases);

            IEnumerable<DiseaseSymptom> diseaseSymptom = _unitOfWork.Disease_Symptoms.Find(x => x.DiseaseId == diseaseDto.Guid);
            if (diseaseSymptom.Any())
                _unitOfWork.Disease_Symptoms.RemoveRange(diseaseSymptom);

            if (disease.AllMedsForDisease != null && disease.AllMedsForDisease.Any())
            {

                foreach (var id in disease.AllMedsForDisease)
                {
                    MedicineDisease med = new()
                    {
                        DiseaseId = diseaseDto.Guid,
                        MedicineId = id,
                    };

                    _unitOfWork.MedicineDiseases.Add(med);
                }
            }

            if (disease.AllSymptomsForDisease != null && disease.AllSymptomsForDisease.Any())
            {
                foreach (var id in disease.AllSymptomsForDisease)
                {
                    DiseaseSymptom dise = new()
                    {
                        DiseaseId = diseaseDto.Guid,
                        SymptomId = id
                    };

                    _unitOfWork.Disease_Symptoms.Add(dise);
                }
            }

            _unitOfWork.Diseases.UpdateState(diseaseDto);
            _unitOfWork.Complete();
            return diseaseDto.Guid.ToString();

        }



        public DiseaseViewModel GetDisease(Guid DiseaseId)
        {
            try
            {
                Disease DiseaseDto = _unitOfWork.Diseases.Get(DiseaseId);
                return ConvertModel(DiseaseDto);
            }
            catch { return null; }
        }



        public IEnumerable<Medicine_DiseaseViewModel> GetAllMedicinesForDisease(Guid DiseaseId)
        {
            try
            {
                IEnumerable<MedicineDisease> MFDDto = _unitOfWork.MedicineDiseases.GetAllMedicine_Diseases(DiseaseId);
                IEnumerable<Medicine_DiseaseViewModel> MFD = ConvertModelsListsMedicineDisease(MFDDto);
                return MFD;
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<DiseaseViewModel> GetAllDiseasesJustNameAndGuid(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<Disease> DesDtos = _unitOfWork.Diseases.GetAllDiseasesJustNameAndGuid(clinicSectionId);
                return Common.ConvertModels<DiseaseViewModel, Disease>.convertModelsLists(DesDtos);

            }
            catch (Exception e) { throw e; }

        }

        public void AddAllSymptomForDisease(string itemList, Guid diseaseId)
        {
            string[] symptomsId = itemList.Split(',');
            List<Disease_SymptomViewModel> symptomForDiseaseViewModels = new List<Disease_SymptomViewModel>();
            if (itemList != "")
            {
                foreach (string syId in symptomsId)
                {
                    Disease_SymptomViewModel symfordes = new Disease_SymptomViewModel
                    {
                        DiseaseId = diseaseId,
                        SymptomId = new Guid(syId)
                    };
                    symptomForDiseaseViewModels.Add(symfordes);
                }
                List<DiseaseSymptom> SymptomForDiseaseDtos = Common.ConvertModels<DiseaseSymptom, Disease_SymptomViewModel>.convertModelsLists(symptomForDiseaseViewModels);
                _unitOfWork.Disease_Symptoms.AddAllSymptomForDisease(SymptomForDiseaseDtos);
            }
            else
            {
                IEnumerable<DiseaseSymptom> Medicine_Disease = _unitOfWork.Disease_Symptoms.GetAllDisease_Symptoms(diseaseId);
                _unitOfWork.Disease_Symptoms.RemoveRange(Medicine_Disease);
                _unitOfWork.Complete();
            }
        }



        public void AddAllMedicineForDisease(string itemList, Guid diseaseId)
        {
            string[] MedicinesId = itemList.Split(',');
            List<Medicine_DiseaseViewModel> medicineForDiseaseViewModels = new List<Medicine_DiseaseViewModel>();
            if (itemList != "")
            {
                foreach (string medId in MedicinesId)
                {
                    Medicine_DiseaseViewModel medDes = new Medicine_DiseaseViewModel
                    {
                        DiseaseId = diseaseId,
                        MedicineId = new Guid(medId)
                    };
                    medicineForDiseaseViewModels.Add(medDes);
                }
                List<MedicineDisease> SymptomForDiseaseDtos = Common.ConvertModels<MedicineDisease, Medicine_DiseaseViewModel>.convertModelsLists(medicineForDiseaseViewModels);
                _unitOfWork.MedicineDiseases.AddAllMedicinesForDisease(SymptomForDiseaseDtos);
            }
            else
            {
                IEnumerable<MedicineDisease> Medicine_Disease = _unitOfWork.MedicineDiseases.GetAllMedicine_Diseases(diseaseId);
                _unitOfWork.MedicineDiseases.RemoveRange(Medicine_Disease);
                _unitOfWork.Complete();
            }
        }


        public PieChartViewModel GetMostUsedDisease(Guid userId)
        {
            try
            {
                IEnumerable<PieChartModel> allMed = _unitOfWork.Diseases.GetMostUsedDisease(userId).OrderByDescending(a=>a.Value).Take(10);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = allMed.Select(a => a.Label).ToArray(),
                    Value = allMed.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;

            }
            catch (Exception e) { throw e; }
        }


        public bool CheckRepeatedDiseaseName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                Disease disease = null;
                if (NewOrUpdate)
                {
                    disease = _unitOfWork.Diseases.GetSingle(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
                }
                else
                {
                    disease = _unitOfWork.Diseases.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
                }
                if (disease != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Disease/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }


        /////////////////////////////converts


        public List<Medicine_DiseaseViewModel> ConvertModelsListsMedicineDisease(IEnumerable<MedicineDisease> Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MedicineDisease, Medicine_DiseaseViewModel>();
                cfg.CreateMap<Disease, DiseaseViewModel>();
                cfg.CreateMap<Medicine, MedicineViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<MedicineDisease>, List<Medicine_DiseaseViewModel>>(Users);
        }

        public List<DiseaseViewModel> ConvertModelsLists(IEnumerable<Disease> Users)
        {
            List<DiseaseViewModel> UserDtoList = new List<DiseaseViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Disease, DiseaseViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            UserDtoList = mapper.Map<IEnumerable<Disease>, List<DiseaseViewModel>>(Users);
            return UserDtoList;
        }

        public DiseaseViewModel ConvertModel(Disease Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Disease, DiseaseViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Disease, DiseaseViewModel>(Users);
        }

        
    }
}
