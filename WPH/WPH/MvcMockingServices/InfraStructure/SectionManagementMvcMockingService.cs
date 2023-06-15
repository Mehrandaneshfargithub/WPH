using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Areas.Admin.Models.SectionManagement;
using WPH.Helper;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class SectionManagementMvcMockingService : ISectionManagementMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectionManagementMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveSection(int sectionId)
        {
            try
            {
                var section = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == sectionId);
                _unitOfWork.BaseInfoGenerals.Remove(section);
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

        public OperationStatus RemoveClinicSection(Guid clinicSectionId)
        {
            try
            {
                var clinicSection = _unitOfWork.ClinicSections.Get(clinicSectionId);
                _unitOfWork.ClinicSections.Remove(clinicSection);
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

        public List<SectionsNameViewModel> GetAllSections()
        {
            var sections = _unitOfWork.BaseInfoGenerals.GetAllNamesByType("SectionType");
            var result = Common.ConvertModels<SectionsNameViewModel, BaseInfoGeneral>.convertModelsLists(sections);
            Indexing<SectionsNameViewModel> indexing = new Indexing<SectionsNameViewModel>();

            return indexing.AddIndexing(result);
        }

        public List<SectionsNameViewModel> GetAllClinicSections()
        {
            var sections = _unitOfWork.BaseInfoGenerals.GetAllNamesByType("ClinicSectionType");
            var result = Common.ConvertModels<SectionsNameViewModel, BaseInfoGeneral>.convertModelsLists(sections);
            Indexing<SectionsNameViewModel> indexing = new Indexing<SectionsNameViewModel>();

            return indexing.AddIndexing(result);
        }

        public SectionsNameViewModel GetSection(int id)
        {
            var section = _unitOfWork.BaseInfoGenerals.GetSingle(p => p.Id == id);
            var result = Common.ConvertModels<SectionsNameViewModel, BaseInfoGeneral>.convertModels(section);

            return result;
        }

        public SubsystemViewModel GetSubsystem(int id)
        {
            var subsystem = _unitOfWork.SubSystems.GetSingle(p => p.Id == id);
            var result = Common.ConvertModels<SubsystemViewModel, SubSystem>.convertModels(subsystem);

            return result;
        }

        public List<ClinicSectionNamesViewModel> GetAllClinicSectionsBySectionTypeId(int sectionTypeId)
        {
            var clinicSections = _unitOfWork.ClinicSections.GetAllClinicSectionsBySectionTypeId(sectionTypeId == 0 ? null : (p => p.SectionTypeId == sectionTypeId));
            var result = Common.ConvertModels<ClinicSectionNamesViewModel, ClinicSection>.convertModelsLists(clinicSections);
            Indexing<ClinicSectionNamesViewModel> indexing = new Indexing<ClinicSectionNamesViewModel>();

            return indexing.AddIndexing(result);
        }

        public List<ClinicSectionNamesViewModel> GetClinicSectionParents()
        {
            var parents = _unitOfWork.ClinicSections.GetClinicSectionParents();
            var result = Common.ConvertModels<ClinicSectionNamesViewModel, ClinicSection>.convertModelsLists(parents);

            return result;
        }

        public List<SubsystemViewModel> GetSubsystemParents()
        {
            var parents = _unitOfWork.SubSystems.GetSubsystemParents();
            var result = Common.ConvertModels<SubsystemViewModel, SubSystem>.convertModelsLists(parents);

            return result;
        }

        public string AddNewSection(SectionsNameViewModel section)
        {
            if (string.IsNullOrWhiteSpace(section.Name) || section.Priority == null)
                return "DataNotValid";

            var exists = _unitOfWork.BaseInfoGenerals.CheckNameExists(section.Name.Trim(), "SectionType");
            if (exists)
                return "ValueIsRepeated";

            var baseInfoGeneral = Common.ConvertModels<BaseInfoGeneral, SectionsNameViewModel>.convertModels(section);
            var type = _unitOfWork.BaseInfoGenerals.GetBaseInfoGeneralType(p => p.Ename == "SectionType");
            baseInfoGeneral.TypeId = type.Id;

            _unitOfWork.BaseInfoGenerals.Add(baseInfoGeneral);
            _unitOfWork.Complete();

            return baseInfoGeneral.Id.ToString();
        }

        public string UpdateSection(SectionsNameViewModel section)
        {
            if (string.IsNullOrWhiteSpace(section.Name) || section.Priority == null)
                return "DataNotValid";

            var exists = _unitOfWork.BaseInfoGenerals.CheckNameExists(section.Name.Trim(), "SectionType", p => p.Id != section.Id);
            if (exists)
                return "ValueIsRepeated";

            var baseInfoGeneral = Common.ConvertModels<BaseInfoGeneral, SectionsNameViewModel>.convertModels(section);
            var type = _unitOfWork.BaseInfoGenerals.GetBaseInfoGeneralType(p => p.Ename == "SectionType");
            baseInfoGeneral.TypeId = type.Id;

            _unitOfWork.BaseInfoGenerals.UpdateState(baseInfoGeneral);
            _unitOfWork.Complete();

            return baseInfoGeneral.Id.ToString();
        }

        public ClinicSectionNamesViewModel GetClinicSection(Guid id)
        {
            var clinicSection = _unitOfWork.ClinicSections.Get(id);

            var result = Common.ConvertModels<ClinicSectionNamesViewModel, ClinicSection>.convertModels(clinicSection);

            return result;
        }

        public string AddNewClinicSection(ClinicSectionNamesViewModel clinicSection)
        {
            if (string.IsNullOrWhiteSpace(clinicSection.Name) || clinicSection.SectionTypeId == null || clinicSection.ParentId == null || clinicSection.Priority == null)
                return "DataNotValid";

            var exist = _unitOfWork.ClinicSections.CheckNameExists(clinicSection.Name, clinicSection.ClinicSectionTypeId);
            if (exist)
                return "ValueIsRepeated";

            if (clinicSection.ParentId != null)
            {
                var parent = _unitOfWork.ClinicSections.Get(clinicSection.ParentId.Value);

                if (parent.ParentId != null && parent.ClinicSectionTypeId != null)
                    return "DataNotValid";
            }

            var result = Common.ConvertModels<ClinicSection, ClinicSectionNamesViewModel>.convertModels(clinicSection);
            var clinic = _unitOfWork.Clinics.GetFirst();
            result.ClinicId = clinic.Guid;
            result.ClinicSectionUsers.Add(new ClinicSectionUser
            {
                UserId = clinicSection.UserId.GetValueOrDefault()
            });
            _unitOfWork.ClinicSections.Add(result);
            _unitOfWork.Complete();

            return result.Guid.ToString();
        }

        public string UpdateClinicSection(ClinicSectionNamesViewModel clinicSection)
        {
            if (string.IsNullOrWhiteSpace(clinicSection.Name) || clinicSection.SectionTypeId == null || clinicSection.ParentId == null || clinicSection.Priority == null)
                return "DataNotValid";

            var exist = _unitOfWork.ClinicSections.CheckNameExists(clinicSection.Name, clinicSection.ClinicSectionTypeId, p => p.Guid != clinicSection.Guid);
            if (exist)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<ClinicSection, ClinicSectionNamesViewModel>.convertModels(clinicSection);
            var clinic = _unitOfWork.Clinics.GetFirst();
            result.ClinicId = clinic.Guid;

            _unitOfWork.ClinicSections.UpdateState(result);
            _unitOfWork.Complete();

            return result.Guid.ToString();
        }

        public List<SubsystemAccessViewModel> GetSubsystemAccess(int subSystemId)
        {
            var access = _unitOfWork.Accesses.GetSubsystemAccess(subSystemId);

            var result = ConvertAccessList(access);
            return result;
        }

        public void AddSubsystemAccess(int subSystemId, List<SubsystemAccessViewModel> accessList, List<SubsystemAccessViewModel> sectionTypes)
        {
            var old_access = _unitOfWork.SubSystems.GetSubSystemAccessBySubSystemId(subSystemId).ToList();
            var old_list = old_access.Select(p => p.AccessId).ToList();
            var new_list = accessList.Select(p => p.AccessId).ToList();

            var delete_access = old_access.Where(p => !(new_list.Contains(p.AccessId))).ToList();
            var add_access = accessList.Where(p => !(old_list.Contains(p.AccessId))).ToList();

            _unitOfWork.UserSubSystemAccesses.RemoveRange(delete_access.SelectMany(p => p.UserSubSystemAccesses));
            _unitOfWork.SubSystems.RemoveRangeSubSystemAccess(delete_access);

            var old_section = _unitOfWork.SubSystemSection.GetSubSystemSectionBySubSystemId(subSystemId).ToList();
            _unitOfWork.SubSystemSection.RemoveRange(old_section);

            var access_result = ReverseConvertSubsystemAccessList(add_access);
            var section_result = ReverseConvertSubSystemSectionList(sectionTypes);

            _unitOfWork.SubSystems.AddRangeSubSystemAccess(access_result);
            _unitOfWork.SubSystemSection.AddRange(section_result);
            _unitOfWork.Complete();
        }

        public List<SubsystemAccessViewModel> ConvertAccessList(IEnumerable<Access> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Access, SubsystemAccessViewModel>()
                .ForMember(a => a.AccessName, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.AccessId, b => b.MapFrom(c => c.Id))
                .ForMember(a => a.Checked, b => b.MapFrom(c => c.SubSystemAccesses.Any()))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Access>, List<SubsystemAccessViewModel>>(baseInfoGenerals);
        }

        public List<SubSystemAccess> ReverseConvertSubsystemAccessList(IEnumerable<SubsystemAccessViewModel> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubsystemAccessViewModel, SubSystemAccess>()
                .ForMember(a => a.AccessId, b => b.MapFrom(c => c.AccessId))
                .ForMember(a => a.SubSystemId, b => b.MapFrom(c => c.SubSystemId))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<SubsystemAccessViewModel>, List<SubSystemAccess>>(baseInfoGenerals);
        }

        public List<SubSystemSection> ReverseConvertSubSystemSectionList(IEnumerable<SubsystemAccessViewModel> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubsystemAccessViewModel, SubSystemSection>()
                .ForMember(a => a.SectionTypeId, b => b.MapFrom(c => c.SectionTypeId))
                .ForMember(a => a.SubSystemId, b => b.MapFrom(c => c.SubSystemId))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<SubsystemAccessViewModel>, List<SubSystemSection>>(baseInfoGenerals);
        }
    }
}
