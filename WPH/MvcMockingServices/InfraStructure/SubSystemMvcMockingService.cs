using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.SectionManagement;
using WPH.Helper;
using WPH.Models.Access;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SubSystemMvcMockingService : ISubSystemMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _contextAccessor;

        public SubSystemMvcMockingService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _contextAccessor = contextAccessor;
        }

        public List<SubSystemViewModel> GetMenuItems(Guid userId, int sectionTypeId, Guid clinicSectionId)
        {

            int LanguageId = 3;
            List<SubSystemViewModel> subSystemViewModels = new List<SubSystemViewModel>();
            var subSystems = _unitOfWork.SubSystems.GetAllUserSubSystems(userId, sectionTypeId, clinicSectionId, LanguageId)/*.OrderBy(x => x.Priority)*/.ToList();
            List<SubSystemViewModel> MenuItems = Common.ConvertModels<SubSystemViewModel, SubSystem>.convertModelsLists(subSystems);
            return MenuItems;
        }

        public List<SubSystemViewModel> GetAllSubSystemsForDev(int SectionTypeId)
        {
            List<SubSystem> subSystems = _unitOfWork.SubSystemSection.GetSubSystemBySectionTypeId(SectionTypeId).ToList();
            return Common.ConvertModels<SubSystemViewModel, SubSystem>.convertModelsLists(subSystems);
        }

        public List<SubSystemViewModel> GetSubSystemByName(string SubSystemName)
        {
            List<SubSystem> subSystems = _unitOfWork.SubSystems.Find(x => x.Name == SubSystemName).ToList();
            return Common.ConvertModels<SubSystemViewModel, SubSystem>.convertModelsLists(subSystems);
        }


        public void DeleteAllUserAccess(Guid userId, Guid clinicSectionId)
        {
            IEnumerable<UserSubSystemAccess> userSubSystemAccess = _unitOfWork.UserSubSystemAccesses.Find(x => x.UserId == userId && x.ClinicSectionId == clinicSectionId);
            _unitOfWork.UserSubSystemAccesses.RemoveRange(userSubSystemAccess);
            _unitOfWork.Complete();
        }

        public void SaveAccessForUser(Guid userId, List<string> subSystemAccessId, Guid clinicSectionId)
        {
            var access_list = _unitOfWork.Accesses.GetAll().Select(p => p.Name).ToList();
            //var list = subSystemAccessId.Where(p => access_list.Any(a => p.Contains(a)));
            var list = subSystemAccessId.Select(p => p.Split("_")).Where(p => p.Length > 1).Select(p => new
            {
                access = p[0],
                id = p[1]
            });

            List<UserSubSystemAccess> userSubSystemAccess = list.Where(p => access_list.Contains(p.access)).Select(p => new UserSubSystemAccess
            {
                Guid = Guid.NewGuid(),
                SubSystemAccessId = int.Parse(p.id),
                UserId = userId,
                ClinicSectionId = clinicSectionId
            }).ToList();


            _unitOfWork.UserSubSystemAccesses.SaveAccessForUser(userId, userSubSystemAccess, clinicSectionId);
            //_unitOfWork.UserSubSystemAccesses.Add(userSubSystemAccess);
            //_unitOfWork.Complete();
        }

        public IEnumerable<AllSubSystemsWithAccess> GetAllSubSystemsWithAccessNames(Guid userId, int SectionTypeId, Guid clinicSectionId, int LanguageId, Guid parentUserId)
        {
            List<AllSubSystemsWithAccess> allSubSystems = _unitOfWork.SubSystems.GetAllSubSystemWithAccess(userId, SectionTypeId, clinicSectionId, LanguageId, parentUserId).ToList();
            return allSubSystems;
        }

        public bool CheckUserAccess(string accessName, string subSystemName)
        {
            try
            {
                var context = _contextAccessor.HttpContext;
                Guid userId = Guid.Parse(context.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(context.Session.GetString("ClinicSectionId"));
                return _unitOfWork.UserSubSystemAccesses.CheckUserAccess(userId, clinicSectionId, accessName, subSystemName);
            }
            catch
            {
                return false;
            }
        }

        public List<AccessViewModel> GetUserSubSystemAccess(params string[] subSystemsName)
        {
            try
            {
                var context = _contextAccessor.HttpContext;
                Guid userId = Guid.Parse(context.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(context.Session.GetString("ClinicSectionId"));
                var result = _unitOfWork.UserSubSystemAccesses.GetUserSubSystemAccess(userId, clinicSectionId, subSystemsName).ToList();
                return result.Select(p => new AccessViewModel
                {
                    SubSystemName = p.SubSystem.Name,
                    AccessName = p.Access.Name
                }).ToList();
            }
            catch
            {
                return new List<AccessViewModel>();
            }
        }

        public List<SubsystemViewModel> GetAllSubsystemsWithParent()
        {
            var subsystems = _unitOfWork.SubSystems.GetAllSubsystemsWithParent();
            List<SubsystemViewModel> result = ConvertModelsLists(subsystems);

            Indexing<SubsystemViewModel> indexing = new Indexing<SubsystemViewModel>();
            return indexing.AddIndexing(result);
        }

        public OperationStatus RemoveSubsystem(int subsystemId)
        {
            try
            {
                var subsystem = _unitOfWork.SubSystems.GetSingle(p => p.Id == subsystemId);
                _unitOfWork.SubSystems.Remove(subsystem);
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

        public string AddNewSubsystem(SubsystemViewModel subsystem)
        {
            if (string.IsNullOrWhiteSpace(subsystem.Name) || string.IsNullOrWhiteSpace(subsystem.ShowName) || subsystem.Priority == null)
                return "DataNotValid";

            var exist = _unitOfWork.SubSystems.CheckNameExists(subsystem.Name);
            if (exist)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<SubSystem, SubsystemViewModel>.convertModels(subsystem);
            result.ParentRelationId = subsystem.Parent;

            if (subsystem.Parent == null)
            {
                subsystem.Parent = 0;
            }

            _unitOfWork.SubSystems.Add(result);
            _unitOfWork.Complete();

            return result.Id.ToString();
        }

        public string UpdateSubsystem(SubsystemViewModel subsystem)
        {
            if (string.IsNullOrWhiteSpace(subsystem.Name) || string.IsNullOrWhiteSpace(subsystem.ShowName) || subsystem.Priority == null)
                return "DataNotValid";

            var exist = _unitOfWork.SubSystems.CheckNameExists(subsystem.Name, p => p.Id != subsystem.Id);
            if (exist)
                return "ValueIsRepeated";

            if (subsystem.Parent == subsystem.Id)
                return "DataNotValid";

            var result = Common.ConvertModels<SubSystem, SubsystemViewModel>.convertModels(subsystem);
            result.ParentRelationId = subsystem.Parent;

            if (subsystem.Parent == null)
            {
                subsystem.Parent = 0;
            }

            _unitOfWork.SubSystems.UpdateState(result);
            _unitOfWork.Complete();

            return result.Id.ToString();
        }

        public void ChangeSubsystemActivation(int subsystemId)
        {
            var subSystem = _unitOfWork.SubSystems.GetSingle(p => p.Id == subsystemId);

            subSystem.Active = !subSystem.Active.GetValueOrDefault(false);

            _unitOfWork.SubSystems.UpdateState(subSystem);
            _unitOfWork.Complete();
        }

        public List<SubsystemViewModel> ConvertModelsLists(IEnumerable<SubSystem> subsystems)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubSystem, SubsystemViewModel>()
                .ForMember(a => a.ParentName, b => b.MapFrom(c => c.ParentRelation.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<SubSystem>, List<SubsystemViewModel>>(subsystems);
        }
    }


}
