using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.BaseInfoType;
using WPH.Helper;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class BaseInfoTypeMvcMockingService : IBaseInfoTypeMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseInfoTypeMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public OperationStatus RemoveBaseInfoType(Guid id)
        {
            try
            {
                var baseInfo = _unitOfWork.BaseInfos.GetBaseInfoType(p => p.Guid == id);
                _unitOfWork.BaseInfos.RemoveBaseInfoType(baseInfo);

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

        public string AddNewBaseInfoType(BaseInfoTypeViewModel baseInfoType)
        {
            if (string.IsNullOrWhiteSpace(baseInfoType.Ename.Trim()) || string.IsNullOrWhiteSpace(baseInfoType.Fname.Trim()))
                return "DataNotValid";

            var exist = _unitOfWork.BaseInfos.GetBaseInfoType(p => p.Ename == baseInfoType.Ename);
            if (exist != null)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<BaseInfoType, BaseInfoTypeViewModel>.convertModels(baseInfoType);

            _unitOfWork.BaseInfos.AddBaseInfoType(result);
            _unitOfWork.Complete();

            return "";
        }

        public string UpdateBaseInfoType(BaseInfoTypeViewModel baseInfoType)
        {
            if (string.IsNullOrWhiteSpace(baseInfoType.Ename.Trim()) || string.IsNullOrWhiteSpace(baseInfoType.Fname.Trim()))
                return "DataNotValid";

            var exist = _unitOfWork.BaseInfos.GetBaseInfoType(p => p.Ename == baseInfoType.Ename && p.Guid != baseInfoType.Guid);
            if (exist != null)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<BaseInfoType, BaseInfoTypeViewModel>.convertModels(baseInfoType);

            _unitOfWork.BaseInfos.UpdateBaseInfoType(result);
            _unitOfWork.Complete();

            return "";
        }

        public List<BaseInfoTypeViewModel> GetAllBaseInfoTypes()
        {
            var baseInfoTypes = _unitOfWork.BaseInfos.GetAllBaseInfoTypes();

            var result = Common.ConvertModels<BaseInfoTypeViewModel, BaseInfoType>.convertModelsLists(baseInfoTypes);

            Indexing<BaseInfoTypeViewModel> indexing = new Indexing<BaseInfoTypeViewModel>();
            return indexing.AddIndexing(result);
        }

        public BaseInfoTypeViewModel GetBaseInfoType(Guid id)
        {
            var baseInfo = _unitOfWork.BaseInfos.GetBaseInfoType(p => p.Guid == id);

            var result = Common.ConvertModels<BaseInfoTypeViewModel, BaseInfoType>.convertModels(baseInfo);

            return result;
        }


        public List<BaseInfoSectioTypeViewModel> GetBaseInfoSectioType(Guid baseInfoTypeId)
        {
            var baseInfoTypes = _unitOfWork.BaseInfoGenerals.GetSectionType(baseInfoTypeId);

            var result = ConvertBaseInfoSectioTypeList(baseInfoTypes);

            return result;
        }


        public List<BaseInfoSectioTypeViewModel> GetSubsystemSectioType(int subSystemId)
        {
            var baseInfoTypes = _unitOfWork.BaseInfoGenerals.GetSubsystemSectioType(subSystemId);

            var result = ConvertSubsystemSectioTypeList(baseInfoTypes);

            return result;
        }

        public void AddBaseInfoSectioType(Guid baseInfoTypeId, List<BaseInfoSectioTypeViewModel> sectionTypes)
        {
            var old_sections = _unitOfWork.BaseInfos.GetBaseInfoSectionTypeByBaseInfoTypeId(baseInfoTypeId).ToList();
            _unitOfWork.BaseInfos.RemoveRangeBaseInfoSectionTypes(old_sections);

            var result = ReverseConvertBaseInfoSectioTypeList(sectionTypes);
            _unitOfWork.BaseInfos.AddRangeBaseInfoSectionTypes(result);
            _unitOfWork.Complete();
        }

        public List<BaseInfoSectioTypeViewModel> ConvertBaseInfoSectioTypeList(IEnumerable<BaseInfoGeneral> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoGeneral, BaseInfoSectioTypeViewModel>()
                .ForMember(a => a.SectionTypeName, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.SectionTypeId, b => b.MapFrom(c => c.Id))
                .ForMember(a => a.BaseInfoTypeId, b => b.MapFrom(c => c.BaseInfoSectionTypes.FirstOrDefault().BaseInfoTypeId))
                .ForMember(a => a.Checked, b => b.MapFrom(c => c.BaseInfoSectionTypes.Any()))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<BaseInfoGeneral>, List<BaseInfoSectioTypeViewModel>>(baseInfoGenerals);
        }

        public List<BaseInfoSectioTypeViewModel> ConvertSubsystemSectioTypeList(IEnumerable<BaseInfoGeneral> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoGeneral, BaseInfoSectioTypeViewModel>()
                .ForMember(a => a.SectionTypeName, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.SectionTypeId, b => b.MapFrom(c => c.Id))
                .ForMember(a => a.Checked, b => b.MapFrom(c => c.SubSystemSections.Any()))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<BaseInfoGeneral>, List<BaseInfoSectioTypeViewModel>>(baseInfoGenerals);
        }


        public List<BaseInfoSectionType> ReverseConvertBaseInfoSectioTypeList(IEnumerable<BaseInfoSectioTypeViewModel> baseInfoGenerals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfoSectioTypeViewModel, BaseInfoSectionType>()
                .ForMember(a => a.SectionTypeId, b => b.MapFrom(c => c.SectionTypeId))
                .ForMember(a => a.BaseInfoTypeId, b => b.MapFrom(c => c.BaseInfoTypeId))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<BaseInfoSectioTypeViewModel>, List<BaseInfoSectionType>>(baseInfoGenerals);
        }
    }
}
