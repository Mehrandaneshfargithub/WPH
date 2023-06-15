using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PatientMedicine;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class PatientMedicineMvcMockingService : IPatientMedicineMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientMedicineMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public List<PatientMedicineRecordViewModel> GetAllMedicineRecordForPatient(Guid id)
        {
            try
            {
                IEnumerable<PatientMedicineRecord> DFPDto = _unitOfWork.PatientMedicineRecords.GetAllPatientMedicineRecord(id);
                List<PatientMedicineRecordViewModel> DFP = ConvertModelsLists(DFPDto);

                return DFP;
            }
            catch { return null; }
        }

        public List<PatientMedicineRecordViewModel> GetAllMedicineRecordForPatientGrid(Guid id)
        {
            try
            {
                IEnumerable<PatientMedicineRecord> DFPDto = _unitOfWork.PatientMedicineRecords.GetAllPatientMedicineRecordGrid(id);
                List<PatientMedicineRecordViewModel> DFP = ConvertModelsLists(DFPDto);

                return DFP;
            }
            catch { return null; }
        }

        public void AddMedicineToPatient(List<PatientMedicineRecordViewModel> diseases)
        {
            try
            {
                IEnumerable<PatientMedicineRecord> medicines = Common.ConvertModels<PatientMedicineRecord, PatientMedicineRecordViewModel>.convertModelsLists(diseases);

                _unitOfWork.PatientMedicineRecords.AddRange(medicines);
                _unitOfWork.Complete();

            }
            catch (Exception ex) { throw ex; }
        }


        public void RemoveMedicineFromPatient(Guid[] medicineId, Guid patientId)
        {
            try
            {
                List<PatientMedicineRecord> medicines = _unitOfWork.PatientMedicineRecords.RemoveMedicineFromPatient(medicineId, patientId).ToList();

                _unitOfWork.PatientMedicineRecords.RemoveRange(medicines);
                _unitOfWork.Complete();

            }
            catch (Exception ex) { throw ex; }
        }

        public static List<PatientMedicineRecordViewModel> ConvertModelsLists(IEnumerable<PatientMedicineRecord> ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientMedicineRecord, PatientMedicineRecordViewModel>()
                .ForMember(a => a.MedicineId, b => b.MapFrom(c => c.MedicineId))
                .ForMember(a => a.MedicineName, b => b.MapFrom(c => c.Medicine.JoineryName))
                .ForAllOtherMembers(a => a.Ignore());

            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PatientMedicineRecord>, List<PatientMedicineRecordViewModel>>(ress);
        }
    }
}
