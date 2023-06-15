using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.HumanResource;
using WPH.Models.HumanResourceSalary;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class HumanResourceMvcMockingService : IHumanResourceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HumanResourceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<HumanResourceViewModel> GetAllHuman(List<Guid> sections)
        {
            IEnumerable<HumanResource> humanResources = _unitOfWork.HumanResources.GetAllHuman(sections);
            List<HumanResourceViewModel> humanResourceViewModels = ConvertModelsLists(humanResources);
            Indexing<HumanResourceViewModel> indexing = new Indexing<HumanResourceViewModel>();
            return indexing.AddIndexing(humanResourceViewModels);
        }

        public IEnumerable<HumanResourceViewModel> GetAllTreatmentStaff(List<Guid> sections)
        {
            IEnumerable<HumanResource> humanResources = _unitOfWork.HumanResources.GetAllTreatmentStaff(sections);
            List<HumanResourceViewModel> humanResourceViewModels = ConvertModelsLists(humanResources);
            Indexing<HumanResourceViewModel> indexing = new Indexing<HumanResourceViewModel>();
            return indexing.AddIndexing(humanResourceViewModels);
        }

        public IEnumerable<HumanResourceViewModel> GetAllHumanwithPerids(List<Guid> sections, int periodId, DateTime dateFrom, DateTime dateTo, Guid humanId)
        {
            IEnumerable<HumanResource> humanResources;
            if (humanId == Guid.Empty)
            {
                humanResources = _unitOfWork.HumanResources.GetAllHuman(sections);
            }
            else
            {
                humanResources = _unitOfWork.HumanResources.GetAllHuman(sections, p => p.Gu.Guid == humanId);
            }

            List<HumanResourceViewModel> humanResourceViewModels = ConvertModelsLists(humanResources);
            Indexing<HumanResourceViewModel> indexing = new Indexing<HumanResourceViewModel>();
            return indexing.AddIndexing(humanResourceViewModels);
        }

        public string AddNewHuman(HumanResourceViewModel viewModel)
        {

            if (!string.IsNullOrWhiteSpace(viewModel.Gu.PhoneNumber) && viewModel.Gu.PhoneNumber.Length < 8)
                return "WrongMobile";

            if (!string.IsNullOrWhiteSpace(viewModel.Gu.Email) && !viewModel.Gu.Email.IsValidEmail())
                return "WrongEmail";

            if (viewModel.FixSalary.GetValueOrDefault(0) <= 0 || viewModel.ExtraSalaryPh.GetValueOrDefault(0) <= 0 || viewModel.RoleTypeId == null
                || viewModel.MinWorkTime.GetValueOrDefault(0) <= 0 || string.IsNullOrWhiteSpace(viewModel.Gu.Name))
                return "DataNotValid";

            if (viewModel.CurrencyId == null)
                viewModel.CurrencyId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("IQD", "Currency");

            viewModel.Gu.Name = viewModel.Gu.Name.Trim();

            var userType = _unitOfWork.BaseInfoGenerals.GetByIdAndType(viewModel.RoleTypeId.Value, "UserType");

            var user = _unitOfWork.Users.GetUserByName(viewModel.Gu.Name, viewModel.Gu.ClinicSectionId.Value);
            if (user == null)
            {
                var newUser = new User
                {
                    Guid = Guid.NewGuid(),
                    Name = viewModel.Gu.Name,
                    PhoneNumber = viewModel.Gu.PhoneNumber,
                    GenderId = viewModel.Gu.GenderId,
                    Email = viewModel.Gu.Email,
                    UserTypeId = viewModel.RoleTypeId,
                    ClinicSectionId = viewModel.Gu.ClinicSectionId,
                    AccessTypeId = viewModel.Gu.AccessTypeId,
                };

                if (viewModel.IsUser)
                {
                    if (viewModel.Gu.Pass3 == null || viewModel.Gu.Pass3 != viewModel.Gu.Pass4)
                        return "PassNotMatch";

                    var userName = _unitOfWork.Users.CheckUserExistBaseOnUserName(viewModel.Gu.ClinicSectionId.Value, viewModel.Gu.UserName.Trim());
                    if (userName != null)
                        return "TheUserNameIsDuplicated";

                    Random ra = new Random();
                    newUser.UserName = viewModel.Gu.UserName.Trim();
                    newUser.Pass1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                    newUser.Pass2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                    newUser.Pass3 = Crypto.Hash(viewModel.Gu.Pass3, "MD5");
                    newUser.Pass4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                    newUser.Active = true;
                    newUser.IsUser = true;
                }
                else
                {
                    newUser.UserName = " ";
                    newUser.Pass1 = "humanresource";
                    newUser.IsUser = false;
                }

                _unitOfWork.Users.Add(newUser);

                if (userType != null && userType.Name == "Doctor")
                {
                    var doctor = new Doctor
                    {
                        Guid = newUser.Guid,
                        MedicalSystemCode = viewModel.Doctor.MedicalSystemCode,
                        ClinicSectionId = viewModel.Gu.ClinicSectionId
                    };

                    if (!string.IsNullOrWhiteSpace(viewModel.Doctor.SpecialityName))
                    {
                        Guid? specialityId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.Doctor.SpecialityName, "DoctorSpeciality", viewModel.Gu.ClinicSectionId.Value);
                        doctor.SpecialityId = specialityId;

                        if (specialityId == null || specialityId == Guid.Empty)
                        {
                            var baseInfo = new BaseInfo
                            {
                                Name = viewModel.Doctor.SpecialityName,
                                TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("DoctorSpeciality"),
                                ClinicSectionId = viewModel.Gu.ClinicSectionId
                            };

                            doctor.Speciality = baseInfo;
                        }

                    }

                    _unitOfWork.Doctor.Add(doctor);
                }

                var humanResource = new HumanResource
                {
                    Guid = newUser.Guid,
                    FixSalary = viewModel.FixSalary,
                    Explanation = viewModel.Explanation,
                    CurrencyId = viewModel.CurrencyId,
                    RoleTypeId = newUser.UserTypeId,
                    Duty = viewModel.Duty,
                    ExtraSalaryPh = viewModel.ExtraSalaryPh,
                    MinWorkTime = viewModel.MinWorkTime
                };

                _unitOfWork.HumanResources.Add(humanResource);
            }
            else
            {
                var exists = _unitOfWork.HumanResources.GetById(user.Guid);

                if (exists != null)
                    return "ValueIsRepeated";

                if (viewModel.IsUser)
                {
                    if (viewModel.Gu.Pass3 == null || viewModel.Gu.Pass3 != viewModel.Gu.Pass4)
                        return "PassNotMatch";

                    var userName = _unitOfWork.Users.CheckUserExistBaseOnUserName(viewModel.Gu.ClinicSectionId.Value, viewModel.Gu.UserName.Trim(), user.UserName);
                    if (userName != null)
                        return "TheUserNameIsDuplicated";

                    Random ra = new Random();
                    user.UserName = viewModel.Gu.UserName.Trim();
                    user.Pass1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                    user.Pass2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                    user.Pass3 = Crypto.Hash(viewModel.Gu.Pass3, "MD5");
                    user.Pass4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                    user.Active = true;
                    user.IsUser = true;

                    _unitOfWork.Users.UpdateUser(user);
                }

                var humanResource = new HumanResource
                {
                    Guid = user.Guid,
                    FixSalary = viewModel.FixSalary,
                    Explanation = viewModel.Explanation,
                    CurrencyId = viewModel.CurrencyId,
                    RoleTypeId = user.UserTypeId,
                    Duty = viewModel.Duty,
                    ExtraSalaryPh = viewModel.ExtraSalaryPh,
                    MinWorkTime = viewModel.MinWorkTime
                };

                _unitOfWork.HumanResources.Add(humanResource);
            }

            _unitOfWork.Complete();
            return "";
        }

        public string UpdateHuman(HumanResourceViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.Gu.PhoneNumber) && viewModel.Gu.PhoneNumber.Length < 8)
                return "WrongMobile";

            if (!string.IsNullOrWhiteSpace(viewModel.Gu.Email) && !viewModel.Gu.Email.IsValidEmail())

            if (viewModel.FixSalary.GetValueOrDefault(0) <= 0 || viewModel.ExtraSalaryPh.GetValueOrDefault(0) <= 0 || viewModel.MinWorkTime.GetValueOrDefault(0) <= 0
                || viewModel.RoleTypeIdHolder == null)
                return "DataNotValid";


            var humanResource = _unitOfWork.HumanResources.Get(viewModel.Guid);
            var user = _unitOfWork.Users.Get(viewModel.Guid);


            var userType = _unitOfWork.BaseInfoGenerals.GetByIdAndType(viewModel.RoleTypeIdHolder.Value, "UserType");
            if (userType != null && userType.Name == "Doctor")
            {
                var doctor = _unitOfWork.Doctor.Get(viewModel.Guid);
                doctor.MedicalSystemCode = viewModel.Doctor.MedicalSystemCode;

                if (!string.IsNullOrWhiteSpace(viewModel.Doctor.SpecialityName))
                {
                    Guid? specialityId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.Doctor.SpecialityName, "DoctorSpeciality", viewModel.Gu.ClinicSectionId.Value);
                    doctor.SpecialityId = specialityId;

                    if (specialityId == null || specialityId == Guid.Empty)
                    {
                        var baseInfo = new BaseInfo
                        {
                            Name = viewModel.Doctor.SpecialityName,
                            TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("DoctorSpeciality"),
                            ClinicSectionId = viewModel.Gu.ClinicSectionId
                        };

                        _unitOfWork.BaseInfos.Add(baseInfo);

                        doctor.Speciality = baseInfo;
                    }

                }

                _unitOfWork.Doctor.UpdateState(doctor);
            }


            user.PhoneNumber = viewModel.Gu.PhoneNumber;
            user.GenderId = viewModel.Gu.GenderId;
            user.Email = viewModel.Gu.Email;

            humanResource.FixSalary = viewModel.FixSalary;
            humanResource.Explanation = viewModel.Explanation;
            humanResource.CurrencyId = viewModel.CurrencyId;
            humanResource.Duty = viewModel.Duty;
            humanResource.ExtraSalaryPh = viewModel.ExtraSalaryPh;
            humanResource.MinWorkTime = viewModel.MinWorkTime;

            if (viewModel.IsUser)
            {
                if (viewModel.Gu.UserName != user.UserName)
                {
                    var userName = _unitOfWork.Users.CheckUserExistBaseOnUserName(user.ClinicSectionId.Value, viewModel.Gu.UserName.Trim(), user.UserName);
                    if (userName != null)
                        return "TheUserNameIsDuplicated";

                    user.UserName = viewModel.Gu.UserName.Trim();
                }

                if (viewModel.Gu.Pass3 != user.Pass3)
                {
                    if (viewModel.Gu.Pass3 == null || viewModel.Gu.Pass3 != viewModel.Gu.Pass4)
                        return "PassNotMatch";

                    Random ra = new Random();
                    user.UserName = viewModel.Gu.UserName.Trim();
                    user.Pass1 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFGH", "MD5");
                    user.Pass2 = Crypto.Hash(ra.Next(1, 10000).ToString() + "PPWEWSHSH9899", "MD5");
                    user.Pass3 = Crypto.Hash(viewModel.Gu.Pass3, "MD5");
                    user.Pass4 = Crypto.Hash(ra.Next(1, 10000).ToString() + "A19BNMFAKJSALGH", "MD5");
                    user.Active = true;
                    user.IsUser = true;
                }
            }
            else
            {
                user.UserName = " ";
                user.Pass1 = "humanresource";
                user.Active = false;
                user.IsUser = false;
            }

            _unitOfWork.Users.UpdateState(user);
            _unitOfWork.HumanResources.UpdateState(humanResource);
            _unitOfWork.Complete();
            return "";

        }

        public OperationStatus RemoveHuman(Guid HumanId)
        {
            try
            {
                HumanResource Human = _unitOfWork.HumanResources.Get(HumanId);
                _unitOfWork.HumanResources.Remove(Human);
                //if (Human.RoleType.Name == "Doctor")
                //{
                //    Doctor doctor = _unitOfWork.Doctor.Get(HumanId);
                //    _unitOfWork.Doctor.Remove(doctor);
                //}
                //User user = _unitOfWork.Users.Get(HumanId);
                //_unitOfWork.Users.Remove(user);
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

        public HumanResourceViewModel GetHuman(Guid HumanId)
        {
            try
            {
                HumanResource Human = _unitOfWork.HumanResources.GetHumanById(HumanId);
                HumanResourceViewModel HumanDto = ConvertReverseModels(Human);
                if (HumanDto.Doctor == null)
                    HumanDto.Doctor = new DoctorViewModel();
                return HumanDto;
            }
            catch (Exception) { return null; }
        }

        public Guid GetHumanByName(string humanName)
        {
            try
            {
                HumanResource Human = _unitOfWork.HumanResources.GetHumanByName(humanName);

                return Human.Guid;
            }
            catch { return Guid.Empty; }
        }

        public HumanResourceViewModel ConvertReverseModels(HumanResource human)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>()
                .ForMember(a => a.SpecialityName, b => b.MapFrom(c => c.Speciality.Name));
                cfg.CreateMap<HumanResource, HumanResourceViewModel>()
                .ForMember(a => a.Doctor, b => b.MapFrom(c => c.Gu.Doctor))
                .ForMember(a => a.RoleTypeId, b => b.MapFrom(c => c.Gu.UserTypeId))
                .ForMember(a => a.IsUser, b => b.MapFrom(c => c.Gu.IsUser));
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResource, HumanResourceViewModel>(human);
        }

        public static HumanResource ConvertModel(HumanResourceViewModel meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceViewModel, HumanResource>();
                cfg.CreateMap<UserInformationViewModel, User>();
                cfg.CreateMap<DoctorViewModel, Doctor>();
                cfg.CreateMap<BaseInfoGeneralViewModel, BaseInfoGeneral>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResourceViewModel, HumanResource>(meds);

        }

        public static List<HumanResourceViewModel> ConvertModelsLists(IEnumerable<HumanResource> humanResources)
        {

            List<HumanResourceViewModel> HumanResourcesViewList = new List<HumanResourceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<HumanResource, HumanResourceViewModel>()
                .ForMember(a => a.RoleTypeName, b => b.MapFrom(c => c.RoleType.Name))
                .ForMember(a => a.HumanName, b => b.MapFrom(c => c.Gu.Name));
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<Doctor, DoctorViewModel>();
                cfg.CreateMap<HumanResourceSalary, HumanResourceSalaryViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            HumanResourcesViewList = mapper.Map<IEnumerable<HumanResource>, List<HumanResourceViewModel>>(humanResources);
            return HumanResourcesViewList;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/HumanResource/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }
    }
}
