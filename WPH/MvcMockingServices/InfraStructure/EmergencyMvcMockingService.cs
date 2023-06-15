using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Emergency;
using WPH.Models.PatientImage;
using WPH.Models.Reception;
using WPH.Models.ReceptionAmbulance;
using WPH.Models.ReceptionClinicSection;
using WPH.Models.Surgery;
using WPH.Models.SurgeryDoctor;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class EmergencyMvcMockingService : IEmergencyMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmergencyMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveEmergency(Guid Emergencyid, string rootPath)
        {
            try
            {
                if (_unitOfWork.Receptions.GetReceptionDischargeStatus(Emergencyid))
                    return OperationStatus.CanNotDelete;

                var images = _unitOfWork.Emergences.RemoveEmergency(Emergencyid);
                FileAttachments deleteIamge = new();
                var imageDto = Common.ConvertModels<PatientImageViewModel, PatientImage>.convertModelsLists(images);
                _unitOfWork.Complete();
                deleteIamge.DeleteAllFiles(imageDto, rootPath);

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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Emergency/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public Guid AddNewEmergency(EmergencyViewModel Emergency)
        {
            try
            {
                Emergency Emergency1 = Common.ConvertModels<Emergency, EmergencyViewModel>.convertModels(Emergency);
                
                _unitOfWork.Emergences.Add(Emergency1);
                _unitOfWork.Complete();
                return Emergency1.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateEmergency(EmergencyViewModel hosp)
        {
            try
            {
                Emergency Emergency2 = Common.ConvertModels<Emergency, EmergencyViewModel>.convertModels(hosp);
                _unitOfWork.Emergences.UpdateState(Emergency2);
                _unitOfWork.Complete();
                return Emergency2.Guid;
            }
            catch (Exception ex) { throw ex; }

        }


        public EmergencyViewModel GetEmergencyById(Guid emergencyId)
        {
            try
            {
                Emergency Emergencygu = _unitOfWork.Emergences.Get(emergencyId);
                return Common.ConvertModels<EmergencyViewModel, Emergency>.convertModels(Emergencygu);
            }
            catch (Exception e) { throw e; }
        }

        public ReceptionViewModel GetEmergency(Guid EmergencyId)
        {
            try
            {
                Reception Emergencygu = _unitOfWork.Emergences.GetEmergency(EmergencyId);
                return convertModel(Emergencygu);
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<PatientReceptionViewModel> GetAllEmergencyReceptionsByClinicSection(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<Reception> PatientReceptionDtos = new List<Reception>();
                if (periodId == (int)Periods.FromDateToDate)
                {
                    PatientReceptionDtos = _unitOfWork.Emergences.GetAllReceptionsByClinicSection(clinicSectionId, DateFrom, DateTo).ToList();
                }
                else
                {
                    DateTime dateFrom = DateTime.Now;
                    DateTime dateTo = DateTime.Now;
                    CommonWas.GetPeriodDateTimes(ref dateFrom, ref dateTo, periodId);
                    PatientReceptionDtos = _unitOfWork.Emergences.GetAllReceptionsByClinicSection(clinicSectionId, dateFrom, dateTo).ToList();
                }

                List<PatientReceptionViewModel> PatientReception = convertReceptionToPatientReceptionLists(PatientReceptionDtos).OrderByDescending(x => x.InvoiceDate).ToList();
                Indexing<PatientReceptionViewModel> indexing = new Indexing<PatientReceptionViewModel>();
                return indexing.AddIndexing(PatientReception);
            }
            catch (Exception ) { return null; }
        }

        public List<PatientReceptionViewModel> convertReceptionToPatientReceptionLists(IEnumerable<Reception> Receptions)
        {
            List<PatientReceptionViewModel> ReceptionDtoList = new List<PatientReceptionViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, PatientReceptionViewModel>()
                .ForMember(a => a.InvoiceDate, b => b.MapFrom(c => c.ReceptionDate))
                .ForMember(a => a.InvoiceNum, b => b.MapFrom(c => c.ReceptionNum))
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.Age, b => b.MapFrom(c => c.DateOfBirth.GetAge()));
                cfg.CreateMap<User, UserInformationViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            ReceptionDtoList = mapper.Map<IEnumerable<Reception>, List<PatientReceptionViewModel>>(Receptions);
            return ReceptionDtoList;
        }


        // Begin Convert 
        public ReceptionViewModel convertModel(Reception Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reception, ReceptionViewModel>();
                cfg.CreateMap<Surgery, SurgeryViewModel>();
                cfg.CreateMap<SurgeryDoctor, SurgeryDoctorViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<ReceptionAmbulance, ReceptionAmbulanceViewModel>();
                cfg.CreateMap<ReceptionClinicSection, ReceptionClinicSectionViewModel>();
                cfg.CreateMap<Emergency, EmergencyViewModel>();
                cfg.CreateMap<Patient, PatientViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
            });


            IMapper mapper = config.CreateMapper();
            return mapper.Map<Reception, ReceptionViewModel>(Users);
        }
        public List<EmergencyViewModel> convertModelsLists(IEnumerable<Emergency> Emergencys)
        {
            List<EmergencyViewModel> EmergencyDtoList = new List<EmergencyViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Emergency, EmergencyViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            EmergencyDtoList = mapper.Map<IEnumerable<Emergency>, List<EmergencyViewModel>>(Emergencys);
            return EmergencyDtoList;
        }




        // End Convert
    }
}
