using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class PatientDiseaseMvcMockingService : IPatientDiseaseMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public PatientDiseaseMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void AddNewDiseasesForPatient(IEnumerable<PatientDiseaseRecordViewModel> patintDisease)
        {
            try
            {
                IEnumerable<PatientDiseaseRecord> disease = Common.ConvertModels<PatientDiseaseRecord, PatientDiseaseRecordViewModel>.convertModelsLists(patintDisease);

                _unitOfWork.PatientDiseaseRecords.AddRange(disease);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }


        public void RemoveDiseasesFromPatient(Guid[] DiseaseId, Guid PatientId)
        {
            try
            {
                List<PatientDiseaseRecord> diseases = _unitOfWork.PatientDiseaseRecords.RemoveDiseasesFromPatient(DiseaseId, PatientId).ToList();

                _unitOfWork.PatientDiseaseRecords.RemoveRange(diseases);
                _unitOfWork.Complete();

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<PatientDiseaseRecordViewModel> GetAllDiseaseForPatient(Guid PatientId)
        {
            try
            {
                IEnumerable<PatientDiseaseRecord> DFPDto = _unitOfWork.PatientDiseaseRecords.GetAllPatientDiseaseRecord(PatientId);
                return ConvertPatientDiseaseRecordModelsLists(DFPDto);

            }
            catch (Exception e) { throw e; }
        }


        public List<PatientDiseaseRecordViewModel> GetAllDiseaseForPatientByType(Guid PatientId, string DiseaseType)
        {
            try
            {
                IEnumerable<PatientDiseaseRecord> MFDDto = _unitOfWork.PatientDiseaseRecords.GetAllPatientDiseaseRecordByType(PatientId, DiseaseType);
                return ConvertModelsLists(MFDDto);
            }
            catch (Exception e) { throw e; }
        }



        ////////////////////////////////////////////////////////////////////////////////////converters
        ///


        public static List<PatientDiseaseRecordViewModel> ConvertPatientDiseaseRecordModelsLists(IEnumerable<PatientDiseaseRecord> ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientDiseaseRecord, PatientDiseaseRecordViewModel>()
                .ForMember(a => a.Disease, b => b.Ignore())
                .ForMember(a => a.Patient, b => b.Ignore());

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PatientDiseaseRecord>, List<PatientDiseaseRecordViewModel>>(ress);
        }

        public static List<PatientDiseaseRecordViewModel> ConvertModelsLists(IEnumerable<PatientDiseaseRecord> ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientDiseaseRecord, PatientDiseaseRecordViewModel>();

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PatientDiseaseRecord>, List<PatientDiseaseRecordViewModel>>(ress);
        }

    }

}
