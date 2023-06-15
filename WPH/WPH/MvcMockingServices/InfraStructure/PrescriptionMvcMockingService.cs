using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PrescriptionTest;
using WPH.Models.CustomDataModels.PrescrptionDetail;
using WPH.MvcMockingServices.Interface;
using AutoMapper;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PrescriptionMvcMockingService : IPrescriptionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrescriptionMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public Guid AddPrescriptionDetail(PrescriptionDetailViewModel prescription, string status)
        {
            PrescriptionDetail preDto = Common.ConvertModels<PrescriptionDetail, PrescriptionDetailViewModel>.convertModels(prescription);
            preDto.ReceptionId = prescription.VisitId;
            _unitOfWork.PrescriptionDetails.Add(preDto);

            if (!string.IsNullOrWhiteSpace(status))
            {
                _unitOfWork.Medicines.UpdateMedicineNum(prescription.Guid, prescription.MedicineId, prescription.Num, status);
            }

            _unitOfWork.Complete();
            return preDto.Guid;
        }

        public void AddPrescriptionDetailRange(List<PrescriptionDetailViewModel> prescription)
        {
            List<PrescriptionDetail> preDto = Common.ConvertModels<PrescriptionDetail, PrescriptionDetailViewModel>.convertModelsLists(prescription);

            foreach (PrescriptionDetail pre in preDto)
            {
                pre.Guid = Guid.NewGuid();
            }

            _unitOfWork.PrescriptionDetails.AddRange(preDto);
            _unitOfWork.Complete();

        }

        public Guid UpdatePrescriptionDetail(PrescriptionDetailViewModel prescription)
        {

            try
            {
                PrescriptionDetail preDto = Common.ConvertModels<PrescriptionDetail, PrescriptionDetailViewModel>.convertModels(prescription);
                _unitOfWork.PrescriptionDetails.UpdateState(preDto);
                _unitOfWork.Complete();
                return preDto.Guid;
            }
            catch (Exception ex) { throw ex; }

        }

        public List<PrescriptionTestDetailViewModel> GetAllVisitPrescriptionOtherAnalysis(Guid id)
        {
            IEnumerable<PatientReceptionAnalysis> preDtoList = _unitOfWork.PatientReceptionAnalysis.GetPatientReceptionAnalysisByReceptionId(id).OrderByDescending(x => x.Id);
            return convertModelAnalysisEntity(preDtoList);
        }

        public List<PrescriptionDetailViewModel> GetAllPrescriptionDetai(Guid VisitId)
        {
            IEnumerable<PrescriptionDetail> preDtoList = _unitOfWork.PrescriptionDetails.GetAllPrescriptionDetail(VisitId).OrderByDescending(x => x.Id);
            return convertModelEntityToDto(preDtoList);
        }

        public List<PrescriptionDetailViewModel> convertModelEntityToDto(IEnumerable<PrescriptionDetail> patient)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrescriptionDetail, PrescriptionDetailViewModel>()
                .ForMember(a => a.MedicineJoineryName, b => b.MapFrom(a => a.Medicine.JoineryName))
                .ForMember(a => a.ModifiedUserName, b => b.MapFrom(a => a.ModifiedUser.Name))
                .ForMember(a => a.Medicine, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                ;

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PrescriptionDetail>, List<PrescriptionDetailViewModel>>(patient);
        }

        public PrescriptionDetailViewModel GetLastMedicinePrescription(Guid MedicineId)
        {
            PrescriptionDetail preDtoList = _unitOfWork.PrescriptionDetails.Find(x => x.MedicineId == MedicineId).OrderByDescending(x => x.Id).FirstOrDefault();
            return Common.ConvertModels<PrescriptionDetailViewModel, PrescriptionDetail>.convertModels(preDtoList);
        }

        public void RemovePrescriptionDetail(Guid preGuid)
        {
            try
            {

                PrescriptionDetail PrescriptionDetail = _unitOfWork.PrescriptionDetails.Get(preGuid);
                _unitOfWork.PrescriptionDetails.Remove(PrescriptionDetail);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }

        public PrescriptionDetailViewModel GetPrescriptionDetailById(Guid prescriptionId)
        {
            PrescriptionDetail preDto = _unitOfWork.PrescriptionDetails.Get(prescriptionId);
            return Common.ConvertModels<PrescriptionDetailViewModel, PrescriptionDetail>.convertModels(preDto);
        }

        public List<PrescriptionTestDetailViewModel> GetAllPrescriptonTests(Guid visitId)
        {
            IEnumerable<PrescriptionTestDetail> preDtoList = _unitOfWork.PrescriptionTests.GetAllPrescriptionTestDetail(visitId).OrderByDescending(x => x.Id);
            return convertModelTestEntityToDto(preDtoList);
        }

        public List<PrescriptionTestDetailViewModel> convertModelTestEntityToDto(IEnumerable<PrescriptionTestDetail> patient)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrescriptionTestDetail, PrescriptionTestDetailViewModel>()
                .ForMember(a => a.TestName, b => b.MapFrom(a => (a.Test == null) ? a.AnalysisName : a.Test.Name))
                .ForMember(a => a.ModifiedUserName, b => b.MapFrom(a => a.ModifiedUser.Name))
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                
                ;
                        

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PrescriptionTestDetail>, List<PrescriptionTestDetailViewModel>>(patient);
        }

        public List<PrescriptionTestDetailViewModel> convertModelAnalysisEntity(IEnumerable<PatientReceptionAnalysis> patient)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientReceptionAnalysis, PrescriptionTestDetailViewModel>()
                .ForMember(a => a.TestName, b => b.MapFrom(a => (a.AnalysisId == null) ? a.AnalysisItem.Name : a.Analysis.Name))
                .ForMember(a => a.ModifiedUserName, b => b.MapFrom(a => a.ModifiedUser.Name))
                .ForMember(a => a.Guid, b => b.MapFrom(a => a.Guid))
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForAllOtherMembers(a=>a.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PatientReceptionAnalysis>, List<PrescriptionTestDetailViewModel>>(patient);
        }

        public Guid AddPrescriptionTest(PrescriptionTestDetailViewModel viewModel)
        {
            PrescriptionTestDetail preTestDto = Common.ConvertModels<PrescriptionTestDetail, PrescriptionTestDetailViewModel>.convertModels(viewModel);


            Guid? TName = null;
            if (viewModel.TestName != null)
            {
                TName = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.TestName, "Test", viewModel.ClinicSectionId);

                if(TName == null)
                {
                    var baseInfo = new BaseInfo
                    {
                        Name = viewModel.TestName,
                        TypeId = viewModel.TestId,
                        ClinicSectionId = viewModel.ClinicSectionId
                    };

                    preTestDto.Test = baseInfo;
                }
                preTestDto.TestId = TName ?? Guid.Empty;
            }
            else
            {
                preTestDto.TestId = TName;
            }
            
            preTestDto.ReceptionId = viewModel.VisitId;
            _unitOfWork.PrescriptionTests.Add(preTestDto);
            _unitOfWork.Complete();
            return preTestDto.Guid;
        }

        public PrescriptionTestDetailViewModel GetPrescriptionTestById(Guid prescriptionId)
        {
            PrescriptionTestDetail preDto = _unitOfWork.PrescriptionTests.Get(prescriptionId);
            return Common.ConvertModels<PrescriptionTestDetailViewModel, PrescriptionTestDetail>.convertModels(preDto);
        }

        public Guid UpdatePrescriptionTest(PrescriptionTestDetailViewModel prescription)
        {
            try
            {
                PrescriptionTestDetail preDto = Common.ConvertModels<PrescriptionTestDetail, PrescriptionTestDetailViewModel>.convertModels(prescription);
                _unitOfWork.PrescriptionTests.UpdateState(preDto);
                _unitOfWork.Complete();
                return preDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public void RemovePrescriptionTest(Guid prescriptionId)
        {
            try
            {
                PrescriptionTestDetail PrescriptionTestDetail = _unitOfWork.PrescriptionTests.Get(prescriptionId);
                _unitOfWork.PrescriptionTests.Remove(PrescriptionTestDetail);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }

        

        public bool VisitHasPrescription(Guid Guid)
        {
            return _unitOfWork.PrescriptionDetails.VisitHasPrescription(Guid);
        }

        public void UpdateMedicineInPrescription(PrescriptionDetailViewModel oldPre)
        {
            PrescriptionDetail preDto = Common.ConvertModels<PrescriptionDetail, PrescriptionDetailViewModel>.convertModels(oldPre);
            _unitOfWork.PrescriptionDetails.UpdateMedicineInPrescription(preDto);
        }

        
    }

}
