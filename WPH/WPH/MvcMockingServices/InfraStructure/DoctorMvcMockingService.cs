using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Doctor;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DoctorMvcMockingService : IDoctorMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Doctor/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public IEnumerable<DoctorViewModel> GetAllDoctorsWithCombinedNameAndSpeciallity(bool forGrid, Guid clinicSectionId)
        {
            try
            {

                IEnumerable<Doctor> doctors = _unitOfWork.Doctor.GetAllDoctors(forGrid, clinicSectionId);
                return ConvertModelsListsDoctorDtoToDoctorViewModelForPatientReception(doctors);
            }
            catch (Exception e) { throw e; }
        }

        public List<DoctorViewModel> GetAllDoctor(bool forGrid, Guid? clinicSectionId = null)
        {
            IEnumerable<Doctor> doctors = _unitOfWork.Doctor.GetAllDoctors(forGrid, clinicSectionId);
            List<DoctorViewModel> doc = ConvertModelsList(doctors);
            Indexing<DoctorViewModel> indexing = new Indexing<DoctorViewModel>();
            return indexing.AddIndexing(doc);
        }

        public List<DoctorFilterViewModel> GetAllDoctorsForFilter(Guid clinicSectionId)
        {
            IEnumerable<Doctor> doctors = _unitOfWork.Doctor.GetAllDoctorsForFilter(clinicSectionId);
            List<DoctorFilterViewModel> doc = ConvertFilterModelsList(doctors);
            return doc;
        }

        public List<DoctorViewModel> GetDoctorsBasedOnUserSection(List<Guid> sections)
        {
            IEnumerable<Doctor> doctors = _unitOfWork.Doctor.GetDoctorsBasedOnUserSection(sections);
            List<DoctorViewModel> doc = ConvertModelsList(doctors);
            Indexing<DoctorViewModel> indexing = new Indexing<DoctorViewModel>();
            return indexing.AddIndexing(doc);
        }


        public string AddNewDoctor(DoctorViewModel newDoctor)
        {
            if (!string.IsNullOrWhiteSpace(newDoctor.User.PhoneNumber) && newDoctor.User.PhoneNumber.Length < 8)
                return "WrongMobile"; 

            if (/*string.IsNullOrEmpty(newDoctor.SpecialityName) ||*/ string.IsNullOrWhiteSpace(newDoctor.User.Name))
                return "DataNotValid";

            newDoctor.User.UserTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType");

            Doctor DoctorDto;
            if (!string.IsNullOrWhiteSpace(newDoctor.SpecialityName))
            {
                var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(newDoctor.SpecialityName, "DoctorSpeciality", newDoctor.ClinicSectionId.Value);
                newDoctor.SpecialityId = typeId;
                DoctorDto = ConvertModel(newDoctor);

                if (typeId == null || typeId == Guid.Empty)
                {
                    var baseInfo = new BaseInfo
                    {
                        Name = newDoctor.SpecialityName,
                        TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("DoctorSpeciality"),
                        ClinicSectionId = newDoctor.ClinicSectionId
                    };

                    DoctorDto.Speciality = baseInfo;
                }
            }
            else
            {
                DoctorDto = ConvertModel(newDoctor);
                DoctorDto.SpecialityId = null;
            }


            _unitOfWork.Doctor.Add(DoctorDto);
            _unitOfWork.Complete();
            return DoctorDto.Guid.ToString();
        }

        public string UpdateDoctor(DoctorViewModel doctor)
        {
            if (!string.IsNullOrWhiteSpace(doctor.User.PhoneNumber) && doctor.User.PhoneNumber.Length < 8)
                return "WrongMobile";

            if (/*string.IsNullOrEmpty(Doctor.SpecialityName) ||*/ string.IsNullOrWhiteSpace(doctor.User.Name))
                return "DataNotValid";


            Doctor sDoctorDto;
            if (!string.IsNullOrWhiteSpace(doctor.SpecialityName))
            {
                var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(doctor.SpecialityName, "DoctorSpeciality", doctor.ClinicSectionId.Value);
                doctor.SpecialityId = typeId;
                sDoctorDto = ConvertModel(doctor);

                if (typeId == null || typeId == Guid.Empty)
                {
                    var baseInfo = new BaseInfo
                    {
                        Name = doctor.SpecialityName,
                        TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("DoctorSpeciality"),
                        ClinicSectionId = doctor.ClinicSectionId
                    };

                    _unitOfWork.BaseInfos.Add(baseInfo);

                    sDoctorDto.Speciality = baseInfo;
                }
            }
            else
            {
                sDoctorDto = ConvertModel(doctor);
                sDoctorDto.SpecialityId = null;
            }

            var user = _unitOfWork.Users.Get(doctor.Guid);
            user.Name = doctor.User.Name;
            user.PhoneNumber = doctor.User.PhoneNumber;

            _unitOfWork.Doctor.UpdateState(sDoctorDto);
            _unitOfWork.Users.UpdateState(user);
            _unitOfWork.Complete();
            return doctor.Guid.ToString();
        }


        public OperationStatus RemoveDoctor(Guid DoctorId)
        {
            try
            {
                Doctor Doctor = _unitOfWork.Doctor.Get(DoctorId);
                User user = _unitOfWork.Users.Get(DoctorId);
                _unitOfWork.Doctor.Remove(Doctor);
                _unitOfWork.Users.Remove(user);
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

        public DoctorViewModel GetDoctor(Guid DoctorId)
        {
            try
            {
                Doctor doctor = _unitOfWork.Doctor.GetDoctorById(DoctorId);
                DoctorViewModel DoctorDto = ConvertModels(doctor);
                return DoctorDto;
            }
            catch { return null; }
        }

        public DoctorViewModel GetDoctorLogoAddress(Guid doctorId)
        {
            try
            {
                Doctor doctor = _unitOfWork.Doctor.Get(doctorId);
                DoctorViewModel DoctorDto = ConvertJustModel(doctor);
                return DoctorDto;
            }
            catch { return null; }
        }

        public string SaveDoctorReportLogo(Guid doctorId, IFormFile reportLogo, string rootPath)
        {
            if (reportLogo == null)
                return "ImageNotSelected";

            string path = "";
            FileAttachments fileAttachments = new();
            try
            {
                Doctor doctor = _unitOfWork.Doctor.Get(doctorId);

                path = fileAttachments.ReduceOpacity(reportLogo, rootPath, doctor.LogoAddress ?? "", doctor.Guid.ToString(), $"\\Uploads\\DoctorReportLogo\\");

                if (string.IsNullOrWhiteSpace(path))
                    return "SomeThingWentWrong";

                doctor.LogoAddress = path;

                _unitOfWork.Doctor.UpdateState(doctor);
                _unitOfWork.Complete();
                return "";
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(path))
                    fileAttachments.DeleteBanner(Path.Combine(rootPath + path));

                return "SomeThingWentWrong";
            }
        }

        public void RemoveDoctorReportLogo(Guid doctorId, string rootPath)
        {
            Doctor doctor = _unitOfWork.Doctor.Get(doctorId);

            if (!string.IsNullOrWhiteSpace(doctor.LogoAddress))
            {
                FileAttachments fileAttachments = new();
                fileAttachments.DeleteBanner(Path.Combine(rootPath + doctor.LogoAddress));
            }

            doctor.LogoAddress = "";
            _unitOfWork.Doctor.UpdateState(doctor);
            _unitOfWork.Complete();
        }

        public string ConvertDoctorToUser(Guid doctorId)
        {
            var user = _unitOfWork.Users.Get(doctorId);
            if (user == null)
                return "NotFound";

            user.IsUser = !user.IsUser.GetValueOrDefault(false);

            _unitOfWork.Users.UpdateState(user);
            _unitOfWork.Complete();

            return user.IsUser.ToString().ToLower();
        }

        public bool CheckRepeatedDoctorNameAndPhone(string name, string phoneNumber, Guid clinicSectionId, bool newOrUpdate, string nameHolder = "", string phoneNumberHolder = "")
        {
            try
            {
                Doctor exist = null;
                if (newOrUpdate)
                {
                    exist = _unitOfWork.Doctor.CheckRepeatedDoctorNameAndPhone(clinicSectionId, name, phoneNumber);
                }
                else
                {
                    exist = _unitOfWork.Doctor.CheckRepeatedDoctorNameAndPhone(clinicSectionId, name, phoneNumber, nameHolder, phoneNumberHolder);
                }
                if (exist != null)
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

        public bool CheckRepeatedNameAndSpeciallity(string name, Guid? specialityId, Guid clinicSectionId, bool newOrUpdate, string nameHolder = "", Guid? specialityIdHolder = null)
        {
            try
            {
                Doctor exist = null;
                if (newOrUpdate)
                {
                    exist = _unitOfWork.Doctor.CheckRepeatedDoctorNameAndSpeciallity(clinicSectionId, name.Trim(), specialityId);
                }
                else
                {
                    exist = _unitOfWork.Doctor.CheckRepeatedDoctorNameAndSpeciallity(clinicSectionId, name.Trim(), specialityId, nameHolder.Trim(), specialityIdHolder);
                }
                if (exist != null)
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

        public DoctorViewModel ConvertModels(Doctor Doctor)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>()
                .ForMember(a => a.SpecialityName, b => b.MapFrom(c => c.Speciality.Name));
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<Doctor, DoctorViewModel>(Doctor);
        }

        public DoctorViewModel ConvertJustModel(Doctor Doctor)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Doctor, DoctorViewModel>()
                .ForMember(a => a.Speciality, b => b.Ignore())
                .ForMember(a => a.User, b => b.Ignore())
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<Doctor, DoctorViewModel>(Doctor);
        }


        public List<DoctorViewModel> ConvertModelsList(IEnumerable<Doctor> Doctor)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Doctor>, List<DoctorViewModel>>(Doctor);

        }

        public List<DoctorFilterViewModel> ConvertFilterModelsList(IEnumerable<Doctor> Doctor)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorFilterViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Doctor>, List<DoctorFilterViewModel>>(Doctor);

        }

        public static Doctor ConvertModel(DoctorViewModel meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<UserInformationViewModel, User>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<DoctorViewModel, Doctor>(meds);

        }



        public List<DoctorViewModel> ConvertModelsListsDoctorDtoToDoctorViewModelForPatientReception(IEnumerable<Doctor> doctors)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Doctor>, List<DoctorViewModel>>(doctors);
        }

    }
}
