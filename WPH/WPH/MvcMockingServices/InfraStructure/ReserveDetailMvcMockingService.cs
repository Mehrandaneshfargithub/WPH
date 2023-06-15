using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.Visit;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ReserveDetailMvcMockingService : IReserveDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ReserveDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public Guid GetReserveDetailDoctorId(Guid reserveDetailId)
        {
            var res = _unitOfWork.ReserveDetails.GetReserveDetailDoctorId(reserveDetailId);

            return res?.DoctorId ?? Guid.Empty;
        }

        public List<EventViewModel> GetAllReservesBetweenTwoDate(Guid originalClinicSectionId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, DateTime calDate, DateTime today, Guid doctorId)
        {
            try
            {

                IEnumerable<FN_GetAllEventsForCalendar_Result> eventPoco = _unitOfWork.ReserveDetails.GetAllReservesBetweenTwoDateForCalendar(originalClinicSectionId, clinicSectionId, fromDate, toDate, doctorId);

                return Common.ConvertModels<EventViewModel, FN_GetAllEventsForCalendar_Result>.convertModelsLists(eventPoco);
            }
            catch (Exception e) { throw e; }
        }




        public ReserveDetailViewModel GetReserveAllDetail(Guid resAllD)
        {
            ReserveDetail resAllDDto = _unitOfWork.ReserveDetails.GetReserveDetail(resAllD);
            return convertModel(resAllDDto);

        }


        public void AddNewReserveDetail(ReserveDetailViewModel resDetail, bool newPatient, Guid clinicSectionId)
        {
            ReserveDetail reseDto = ConvertViewModelToDto(resDetail);
            reseDto.Patient.User = Common.ConvertModels<User, PatientViewModel>.convertModels(resDetail.Patient);
            if (!string.IsNullOrWhiteSpace(resDetail.Patient.AddressName))
            {
                var address = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.AddressName, "Address", clinicSectionId);
                reseDto.Patient.AddressId = address?.BaseInfos?.FirstOrDefault()?.Guid;

                if (reseDto.Patient.AddressId == null)
                {
                    reseDto.Patient.Address = new BaseInfo
                    {
                        Name = resDetail.Patient.AddressName,
                        ClinicSectionId = clinicSectionId,
                        TypeId = address.Guid
                    };

                    _unitOfWork.BaseInfos.Add(reseDto.Patient.Address);
                }
            }

            if (!string.IsNullOrWhiteSpace(resDetail.Patient.FatherJobName))
            {
                var fatherJob = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.FatherJobName, "Job", clinicSectionId);
                reseDto.Patient.FatherJobId = fatherJob?.BaseInfos?.FirstOrDefault()?.Guid;

                if (reseDto.Patient.FatherJobId == null)
                {
                    reseDto.Patient.FatherJob = new BaseInfo
                    {
                        Name = resDetail.Patient.FatherJobName,
                        ClinicSectionId = clinicSectionId,
                        TypeId = fatherJob.Guid
                    };

                    _unitOfWork.BaseInfos.Add(reseDto.Patient.FatherJob);
                }
            }

            if (!string.IsNullOrWhiteSpace(resDetail.Patient.MotherJobName))
            {
                var motherJob = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.MotherJobName, "Job", clinicSectionId);
                reseDto.Patient.MotherJobId = motherJob?.BaseInfos?.FirstOrDefault()?.Guid;
                if (reseDto.Patient.MotherJobId == null)
                {
                    reseDto.Patient.MotherJob = new BaseInfo
                    {
                        Name = resDetail.Patient.MotherJobName,
                        ClinicSectionId = clinicSectionId,
                        TypeId = motherJob.Guid
                    };

                    _unitOfWork.BaseInfos.Add(reseDto.Patient.MotherJob);
                }
            }

            reseDto.Patient.User.Pass1 = "123";
            if (newPatient)
            {
                reseDto.Patient.FileNum = _idunit.patient.GetPatientFileNum(clinicSectionId, reseDto.Patient.User.ClinicSectionId ?? Guid.Empty);
            }
            reseDto.Patient.User.PhoneNumber = resDetail.Patient.PhoneNumber;
            _unitOfWork.ReserveDetails.AddNewReserveDetail(reseDto, newPatient);
        }


        public string RemoveReserveDetail(Guid reserveDetailId)
        {

            try
            {
                var result = _unitOfWork.ReserveDetails.RemoveReserveDetail(reserveDetailId);

                return string.IsNullOrWhiteSpace(result) ? OperationStatus.SUCCESSFUL.ToString() : result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity.ToString();
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong.ToString();
                }
            }

        }

        public string UpdateReserveDetail(ReserveDetailViewModel resDetail, bool newPatient, Guid clinicSectionId)
        {
            try
            {
                var oldReserveDetail = _unitOfWork.ReserveDetails.GetNoTracking(resDetail.Guid);
                var status = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == oldReserveDetail.StatusId);
                if (status?.Name != "NotVisited" && oldReserveDetail.PatientId != resDetail.PatientId)
                    return "VisitedChange";

                ReserveDetail reserDetailDto = ConvertViewModelToDto(resDetail);
                reserDetailDto.Patient.User = Common.ConvertModels<User, PatientViewModel>.convertModels(resDetail.Patient);
                reserDetailDto.Patient.User.Pass1 = "123";
                
                if (!string.IsNullOrWhiteSpace(resDetail.Patient.AddressName))
                {
                    var address = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.AddressName, "Address", clinicSectionId);
                    reserDetailDto.Patient.AddressId = address?.BaseInfos?.FirstOrDefault()?.Guid;

                    if (reserDetailDto.Patient.AddressId == null)
                    {
                        reserDetailDto.Patient.Address = new BaseInfo
                        {
                            Name = resDetail.Patient.AddressName,
                            ClinicSectionId = clinicSectionId,
                            TypeId = address.Guid
                        };

                        _unitOfWork.BaseInfos.Add(reserDetailDto.Patient.Address);
                    }
                }

                if (!string.IsNullOrWhiteSpace(resDetail.Patient.FatherJobName))
                {
                    var fatherJob = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.FatherJobName, "Job", clinicSectionId);
                    reserDetailDto.Patient.FatherJobId = fatherJob?.BaseInfos?.FirstOrDefault()?.Guid;

                    if (reserDetailDto.Patient.FatherJobId == null)
                    {
                        reserDetailDto.Patient.FatherJob = new BaseInfo
                        {
                            Name = resDetail.Patient.FatherJobName,
                            ClinicSectionId = clinicSectionId,
                            TypeId = fatherJob.Guid
                        };

                        _unitOfWork.BaseInfos.Add(reserDetailDto.Patient.FatherJob);
                    }
                }

                if (!string.IsNullOrWhiteSpace(resDetail.Patient.MotherJobName))
                {
                    var motherJob = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(resDetail.Patient.MotherJobName, "Job", clinicSectionId);
                    reserDetailDto.Patient.MotherJobId = motherJob?.BaseInfos?.FirstOrDefault()?.Guid;
                    if (reserDetailDto.Patient.MotherJobId == null)
                    {
                        reserDetailDto.Patient.MotherJob = new BaseInfo
                        {
                            Name = resDetail.Patient.MotherJobName,
                            ClinicSectionId = clinicSectionId,
                            TypeId = motherJob.Guid
                        };

                        _unitOfWork.BaseInfos.Add(reserDetailDto.Patient.MotherJob);
                    }
                }


                if (newPatient)
                {
                    reserDetailDto.Patient.FileNum = _idunit.patient.GetPatientFileNum(clinicSectionId, reserDetailDto.Patient.User.ClinicSectionId ?? Guid.Empty);
                }

                reserDetailDto.Patient.User.PhoneNumber = resDetail.Patient.PhoneNumber;
                _unitOfWork.ReserveDetails.UpdateReserveDetail(reserDetailDto, newPatient);

                return "1";
            }
            catch (Exception ex) { throw ex; }
        }


        public void UpdateReserveDetailStatus(ReserveDetailViewModel resAllD)
        {
            try
            {
                resAllD.Patient = null;
                //resDetail.Status = null;
                ReserveDetail reserDetailDto = ConvertViewModelToDto(resAllD);
                _unitOfWork.ReserveDetails.UpdateReserveDetailStatus(reserDetailDto);
            }
            catch (Exception ex) { throw ex; }
        }


        public DateTime? GetLastPatientVisitDate(Guid patientId, bool recieved)
        {
            return _unitOfWork.ReserveDetails.GetLastPatientVisitDate(patientId, recieved);
        }

        public void UpdateReserveDetailTime(Guid Reserveid, string start, string end)
        {
            _unitOfWork.ReserveDetails.UpdateReserveDetailTime(Reserveid, start, end);
        }


        public PatientViewModel GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId)
        {

            return _idunit.patient.GetPatientIdAndNameFromReserveDetailId(reserveDetailId);

        }

        ///////////////////////////////////////////////////////////////////////Converts
        ///


        public static ReserveDetailViewModel convertModel(ReserveDetail ress)
        {
            //List<ReserveAllDetailViewModel> ReserveDtoList = new List<ReserveAllDetailViewModel>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReserveDetail, ReserveDetailViewModel>()
                .ForMember(a => a.Reserve, b => b.Ignore())
                .ForMember(a => a.Visits, b => b.Ignore())
                ;
                cfg.CreateMap<Patient, PatientViewModel>()
                .ForMember(a => a.AddressName, b => b.MapFrom(c => c.Address.Name))
                .ForMember(a => a.MotherJobName, b => b.MapFrom(c => c.MotherJob.Name))
                .ForMember(a => a.FatherJobName, b => b.MapFrom(c => c.FatherJob.Name))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.User.Name))
                .ForMember(a => a.PhoneNumber, b => b.MapFrom(c => c.User.PhoneNumber))
                .ForMember(a => a.BaseInfoGeneral, b => b.Ignore())
                .ForMember(a => a.Address, b => b.Ignore())
                .ForMember(a => a.FatherJob, b => b.Ignore())
                .ForMember(a => a.MotherJob, b => b.Ignore())
                .ForMember(a => a.BloodType, b => b.Ignore())
                .ForMember(a => a.PatientDiseaseRecords, b => b.Ignore())
                .ForMember(a => a.PatientMedicineRecords, b => b.Ignore())
                .ForMember(a => a.ReserveDetails, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();


            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<ReserveDetail, ReserveDetailViewModel>(ress);

        }

        public static ReserveDetail ConvertViewModelToDto(ReserveDetailViewModel ress)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReserveDetailViewModel, ReserveDetail>()
                ;
                cfg.CreateMap<PatientViewModel, Patient>()
                ;
                cfg.CreateMap<BaseInfoGeneralViewModel, BaseInfoGeneral>();
                cfg.CreateMap<BaseInfoViewModel, BaseInfo>();
                cfg.CreateMap<UserInformationViewModel, User>();


            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<ReserveDetailViewModel, ReserveDetail>(ress);

        }


    }

}
