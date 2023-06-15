using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    
    public class GroupAnalysisItemMvcMokingService : IGroupAnalysisItemMvcMokingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupAnalysisItemMvcMokingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/GroupAnalysisItem/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItem(Guid groupAnalysisId)
        {
            List<GroupAnalysisItem> groupAnalysisItemDtos = _unitOfWork.GroupAnalysisItem.GetAllGroupAnalysisItem(groupAnalysisId).ToList();
            List<GroupAnalysisItemViewModel> groupAnalysisItemes = ConvertModelsLists(groupAnalysisItemDtos).ToList();
            Indexing<GroupAnalysisItemViewModel> indexing = new Indexing<GroupAnalysisItemViewModel>();
            return indexing.AddIndexing(groupAnalysisItemes);
        }

        public IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItem()
        {
            List<GroupAnalysisItem> groupAnalysisItemDtos = _unitOfWork.GroupAnalysisItem.GetAllGroupAnalysisItem().ToList();
            List<GroupAnalysisItemViewModel> groupAnalysisItemes = ConvertModelsLists(groupAnalysisItemDtos).ToList();
            Indexing<GroupAnalysisItemViewModel> indexing = new Indexing<GroupAnalysisItemViewModel>();
            return indexing.AddIndexing(groupAnalysisItemes);
        }

        public void SwapPriority(Guid Id, Guid groupAnalysisId, string type)
        {
            try
            {
                GroupAnalysisItem currentGroupAnalysisItem = _unitOfWork.GroupAnalysisItem.GetSingle(x => x.Guid == Id);
                decimal? currentGroupAnalysisItemPriority = currentGroupAnalysisItem.Priority;
                GroupAnalysisItem swapGroupAnalysisItem = new GroupAnalysisItem();
                if (type == "Up")
                {
                    swapGroupAnalysisItem = _unitOfWork.GroupAnalysisItem.Find(x => x.GroupAnalysisId == groupAnalysisId && x.Priority < currentGroupAnalysisItemPriority).OrderByDescending(x => x.Priority).FirstOrDefault();
                }
                else if (type == "Down")
                {
                    swapGroupAnalysisItem = _unitOfWork.GroupAnalysisItem.Find(x => x.GroupAnalysisId == groupAnalysisId && x.Priority > currentGroupAnalysisItemPriority).OrderBy(x => x.Priority).FirstOrDefault();
                }
                currentGroupAnalysisItem.Priority = swapGroupAnalysisItem.Priority;
                swapGroupAnalysisItem.Priority = currentGroupAnalysisItemPriority;
                _unitOfWork.GroupAnalysisItem.UpdatePriority(currentGroupAnalysisItem);
                _unitOfWork.GroupAnalysisItem.UpdatePriority(swapGroupAnalysisItem);
                _unitOfWork.Complete();
            }
            catch { }
        }


        public OperationStatus Remove(Guid GroupAnalysisItemId)
        {
            try
            {
                GroupAnalysisItem GroupAnalysisItem = _unitOfWork.GroupAnalysisItem.Get(GroupAnalysisItemId);
                _unitOfWork.GroupAnalysisItem.Remove(GroupAnalysisItem);
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

        public OperationStatus RemoveGroupAnalysisItemWithGroupAnalysisId(Guid GroupAnalysisId)
        {
            try
            {
                IEnumerable<GroupAnalysisItem> groupAnalysisItem = _unitOfWork.GroupAnalysisItem.Find(x => x.GroupAnalysisId == GroupAnalysisId);
                if (groupAnalysisItem.Count() != 0)
                    _unitOfWork.GroupAnalysisItem.RemoveRange(groupAnalysisItem);
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

        public GroupAnalysisItemViewModel GetGroupAnalysisItem(Guid GroupAnalysisItemId)
        {
            try
            {
                GroupAnalysisItem GroupAnalysisItemDto = _unitOfWork.GroupAnalysisItem.Get(GroupAnalysisItemId);
                return ConvertModels(GroupAnalysisItemDto);
            }
            catch { return null; }
        }


        public Guid AddNewGroupAnalysisItem(GroupAnalysisItemViewModel groupAnalysisItem, Guid groupAnalysisId)
        {
            try
            {
                GroupAnalysisItem groupAnalysisItemDto = Common.ConvertModels<GroupAnalysisItem, GroupAnalysisItemViewModel>.convertModels(groupAnalysisItem);
                IEnumerable<GroupAnalysisItem> groupanaDtos = _unitOfWork.GroupAnalysisItem.GetAllGroupAnalysisItem(groupAnalysisId);
                if (groupanaDtos.Count() != 0)
                {
                    var maxp = groupanaDtos.Max(n => n.Priority);
                    groupAnalysisItemDto.Priority = maxp + 1;
                }
                else
                {
                    groupAnalysisItemDto.Priority = 1;
                }
                
                _unitOfWork.GroupAnalysisItem.Add(groupAnalysisItemDto);
                _unitOfWork.Complete();
                return groupAnalysisItemDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateGroupAnalysisItem(GroupAnalysisItemViewModel groupAnalysisItem)
        {
            try
            {
                GroupAnalysisItem groupAnalysisItemDto = Common.ConvertModels<GroupAnalysisItem, GroupAnalysisItemViewModel>.convertModels(groupAnalysisItem);
                _unitOfWork.GroupAnalysisItem.UpdateState(groupAnalysisItemDto);
                _unitOfWork.Complete();
                return groupAnalysisItemDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<GroupAnalysisItemViewModel> ConvertModelsLists(IEnumerable<GroupAnalysisItem> GroupAnalysisItem)
        {
            List<GroupAnalysisItemViewModel> GroupAnalysisItemViewModelList = new List<GroupAnalysisItemViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore())
                .ForMember(a => a.ValueType, b => b.Ignore())
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                .ForMember(a => a.AnalysisInfo, b => b.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysisItemViewModelList = mapper.Map<IEnumerable<GroupAnalysisItem>, List<GroupAnalysisItemViewModel>>(GroupAnalysisItem);
            return GroupAnalysisItemViewModelList;
        }

        public GroupAnalysisItemViewModel ConvertModels(GroupAnalysisItem GroupAnalysisItem)
        {
            GroupAnalysisItemViewModel GroupAnalysisItemView = new GroupAnalysisItemViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.ValueType, b => b.Ignore())
                .ForMember(a => a.AnalysisInfo, b => b.Ignore());

            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysisItemView = mapper.Map<GroupAnalysisItem, GroupAnalysisItemViewModel>(GroupAnalysisItem);
            return GroupAnalysisItemView;
        }
    }

}
