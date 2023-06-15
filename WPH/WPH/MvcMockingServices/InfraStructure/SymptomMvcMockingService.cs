using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.CustomDataModels.Disease;
using WPH.Models.CustomDataModels.Symptom;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SymptomMvcMockingService : ISymptomMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SymptomMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Symptom/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public bool CheckRepeatedSymptomName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "")
        {
            Symptom symptom = null;
            if (NewOrUpdate)
            {
                symptom = _unitOfWork.Symptoms.GetSingle(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
            }
            else
            {
                symptom = _unitOfWork.Symptoms.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
            }
            if (symptom != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Guid AddNewSymptom(SymptomViewModel newsymptom )
        {
            Symptom symptomDto = Common.ConvertModels<Symptom, SymptomViewModel>.convertModels(newsymptom);
            
            _unitOfWork.Symptoms.Add(symptomDto);
            _unitOfWork.Complete();
            return symptomDto.Guid;
        }

        public Guid UpdateSymptom(SymptomViewModel symptom)
        {
            Symptom ssymptomDto = Common.ConvertModels<Symptom, SymptomViewModel>.convertModels(symptom);

            _unitOfWork.Symptoms.UpdateState(ssymptomDto);
            _unitOfWork.Complete();
            return ssymptomDto.Guid;
        }

        public OperationStatus RemoveSymptom(Guid symptomId)
        {
            try
            {
                _unitOfWork.Symptoms.RemoveSymptom(symptomId);
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

        public SymptomViewModel GetSymptom(Guid symptomId)
        {
            Symptom SymptomDto = _unitOfWork.Symptoms.Get(symptomId);
            return ConvertModelforGrid(SymptomDto);
        }

        public IEnumerable<SymptomViewModel> GetAllSymptom(Guid clinicSectionId)
        {
            IEnumerable<Symptom> symptomDtos = _unitOfWork.Symptoms.GetAll();
            List<SymptomViewModel> symptom = ConvertModelsListsforGrid(symptomDtos);
            Indexing<SymptomViewModel> indexing = new Indexing<SymptomViewModel>();
            return indexing.AddIndexing(symptom);

        }

        public IEnumerable<SymptomViewModel> GetAllSymptomForDisease(Guid clinicSectionId, Guid diseaseId, bool all)
        {
            IEnumerable<Symptom> symptomDtos = _unitOfWork.Symptoms.GetAllSymptomsForDisease(clinicSectionId, diseaseId, all);
            return ConvertModelsLists(symptomDtos);
        }


        public IEnumerable<SymptomViewModel> GetAllSymptomJustNameAndGuid(Guid clinicSectionId)
        {
            IEnumerable<Symptom> symptomDtos = _unitOfWork.Symptoms.GetAllSymptomJustNameAndGuid(clinicSectionId);
            return Common.ConvertModels<SymptomViewModel, Symptom>.convertModelsLists(symptomDtos);

        }


        public static List<SymptomViewModel> ConvertModelsLists(IEnumerable<Symptom> meds)
        {

            List<SymptomViewModel> MedicineDtoList = new List<SymptomViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Symptom, SymptomViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.Visit_Symptom, b => b.Ignore())
                ;
                cfg.CreateMap<DiseaseSymptom, Disease_SymptomViewModel>()
                .ForMember(x => x.Symptom, b => b.Ignore())
                ;
                cfg.CreateMap<Disease, DiseaseViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.DiseaseType, b => b.Ignore())
                .ForMember(x => x.Disease_Symptom, b => b.Ignore())
                .ForMember(x => x.Medicine_Disease, b => b.Ignore())
                .ForMember(x => x.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(x => x.Visit_Patient_Disease, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            MedicineDtoList = mapper.Map<IEnumerable<Symptom>, List<SymptomViewModel>>(meds);

            return MedicineDtoList;
        }



        public static List<SymptomViewModel> ConvertModelsListsforGrid(IEnumerable<Symptom> meds)
        {

            List<SymptomViewModel> MedicineDtoList = new List<SymptomViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Symptom, SymptomViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.Visit_Symptom, b => b.Ignore())
                .ForMember(x => x.Disease_Symptom, b => b.Ignore())
                ;

            });

            IMapper mapper = config.CreateMapper();
            MedicineDtoList = mapper.Map<IEnumerable<Symptom>, List<SymptomViewModel>>(meds);

            return MedicineDtoList;
        }


        public static SymptomViewModel ConvertModelforGrid(Symptom meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Symptom, SymptomViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.Visit_Symptom, b => b.Ignore())
                .ForMember(x => x.Disease_Symptom, b => b.Ignore())
                ;

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Symptom, SymptomViewModel>(meds);

        }


    }

}
