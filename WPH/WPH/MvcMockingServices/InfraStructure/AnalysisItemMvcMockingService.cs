using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class AnalysisItemMvcMockingService : IAnalysisItemMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly AnalysisItemWorker _worker;
        public AnalysisItemMvcMockingService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, AnalysisItemWorker worker)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _memoryCache = memoryCache;
            _worker = worker;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisItem/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public void AddAnalysisOfAnalysisItem(Guid analysisItemId, Guid analysisId)
        {
            AnalysisItem analysisItem = _unitOfWork.AnalysisItem.Get(analysisItemId);
            _unitOfWork.AnalysisItem.Detach(analysisItem);
            _unitOfWork.Complete();
            _unitOfWork.AnalysisItem.UpdateState(analysisItem);
            _unitOfWork.Complete();
        }

        public IEnumerable<AnalysisItemViewModel> GetAllAnalysisItem(Guid clinicSectionId)
        {
            try
            {
                List<AnalysisItem> AnalysisItems = new List<AnalysisItem>();

                var output = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
                if (output is not null)
                {

                    AnalysisItems = output.Where(x => x.ClinicSectionId == clinicSectionId).ToList();
                }
                else
                {
                    AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItem(clinicSectionId).ToList();
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);
                }

                List<AnalysisItemViewModel> analysisItems = ConvertModelsLists(AnalysisItems).ToList();
                Indexing<AnalysisItemViewModel> indexing = new Indexing<AnalysisItemViewModel>();
                return indexing.AddIndexing(analysisItems);

            }
            catch (Exception e) { throw e; }

        }

        public IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemByClinicSectionId(Guid clinicSectionId)
        {
            try
            {
                List<AnalysisItemViewModel> analysisItems = new List<AnalysisItemViewModel>();

                var output = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
                if (output is null || output.Count == 0)
                {
                    var AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItemByClinicSectionId(clinicSectionId);
                    analysisItems = ConvertModelsLists(AnalysisItems);
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);

                }
                else
                {
                    analysisItems = ConvertModelsLists(output.Where(x => x.ClinicSectionId == clinicSectionId));
                }

                Indexing<AnalysisItemViewModel> indexing = new Indexing<AnalysisItemViewModel>();
                return indexing.AddIndexing(analysisItems);

            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemWithoutInAnalysisByUserId(Guid analysisId, Guid userId)
        {

            List<AnalysisItem> AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItemWithoutInAnalysisByUserId(analysisId, userId);
            List<AnalysisItemViewModel> analysisItems = ConvertModelsLists(AnalysisItems);
            Indexing<AnalysisItemViewModel> indexing = new Indexing<AnalysisItemViewModel>();
            return indexing.AddIndexing(analysisItems);
        }


        public IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(Guid groupId)
        {
            List<AnalysisItem> AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(groupId).OrderByDescending(x => x.CreatedDate).ToList();
            List<AnalysisItemViewModel> analysisItems = ConvertModelsLists(AnalysisItems).ToList();
            Indexing<AnalysisItemViewModel> indexing = new Indexing<AnalysisItemViewModel>();
            return indexing.AddIndexing(analysisItems);
        }

        public List<AnalysisItemJustNameAndGuidViewModel> GetAllAnalysisItemsWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId)
        {
            var CanalysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
            List<AnalysisItemJustNameAndGuidViewModel> analysis = new List<AnalysisItemJustNameAndGuidViewModel>();

            if (CanalysisItem is null || CanalysisItem.Count == 0)
            {
                List<AnalysisItem> AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItemsWithNameAndGuidOnly(clinicSectionId, DestCurrencyId);
                analysis = ConvertToAnalysisItemJustNameAndGuidModelsLists(AnalysisItems);
                CancellationToken s = new CancellationToken();
                _worker.StartAsync(s);
            }
            else
            {
                analysis = ConvertToAnalysisItemJustNameAndGuidModelsLists(CanalysisItem.Where(x => x.ClinicSectionId == clinicSectionId));
            }

            Indexing<AnalysisItemJustNameAndGuidViewModel> indexing = new Indexing<AnalysisItemJustNameAndGuidViewModel>();
            return indexing.AddIndexing(analysis);
        }

        public void SwapPriority(Guid AnalysisItemId, string type)
        {
            try
            {
                AnalysisItem currentAnalysisItem = _unitOfWork.AnalysisItem.GetSingle(x => x.Guid == AnalysisItemId);
                int? currentAnalysisItemPriority = currentAnalysisItem.Priority;
                AnalysisItem swapAnalysisItem = new AnalysisItem();
                if (type == "Up")
                {
                    swapAnalysisItem = _unitOfWork.AnalysisItem.GetSingle(x => x.Priority == currentAnalysisItemPriority - 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                else if (type == "Down")
                {

                    swapAnalysisItem = _unitOfWork.AnalysisItem.GetSingle(x => x.Priority == currentAnalysisItemPriority + 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                currentAnalysisItem.Priority = swapAnalysisItem.Priority;
                swapAnalysisItem.Priority = currentAnalysisItemPriority;
                _unitOfWork.AnalysisItem.UpdatePriority(currentAnalysisItem);
                _unitOfWork.AnalysisItem.UpdatePriority(swapAnalysisItem);
                _unitOfWork.Complete();
                _worker.RemoveCach();
            }
            catch { }
        }

        public void AnalysisAnalysisItemSwapPriority(Guid AnalysisId, string type)
        {
            try
            {
                AnalysisAnalysisItem currentAnalysisItem = _unitOfWork.AnalysisAnalysisItem.GetSingle(x => x.Guid == AnalysisId);
                int? currentAnalysisItemPriority = currentAnalysisItem.Priority;
                AnalysisAnalysisItem swapAnalysisItem = new AnalysisAnalysisItem();
                if (type == "Up")
                {
                    swapAnalysisItem = _unitOfWork.AnalysisAnalysisItem.GetSingle(x => x.AnalysisId == currentAnalysisItem.AnalysisId && x.Priority == currentAnalysisItemPriority - 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                else if (type == "Down")
                {

                    swapAnalysisItem = _unitOfWork.AnalysisAnalysisItem.GetSingle(x => x.AnalysisId == currentAnalysisItem.AnalysisId && x.Priority == currentAnalysisItemPriority + 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                currentAnalysisItem.Priority = swapAnalysisItem.Priority;
                swapAnalysisItem.Priority = currentAnalysisItemPriority;
                _unitOfWork.AnalysisAnalysisItem.UpdatePriority(currentAnalysisItem);
                _unitOfWork.AnalysisAnalysisItem.UpdatePriority(swapAnalysisItem);
                _unitOfWork.Complete();
                _worker.RemoveCach();
            }
            catch { }
        }

        public OperationStatus RemoveAnalysisItem(Guid AnalysisItemId)
        {
            try
            {
                _unitOfWork.AnalysisItem.RemoveAnalysisItem(AnalysisItemId);
                //_unitOfWork.Complete();
                _worker.RemoveCach();
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

        public AnalysisItemViewModel GetAnalysisItem(Guid AnalysisItemId)
        {
            try
            {
                AnalysisItem AnalysisItem = _unitOfWork.AnalysisItem.GetAnalysisItemBasedOnId(AnalysisItemId);
                return ConvertModels(AnalysisItem);
            }
            catch (Exception e) { return null; }
        }

        public void UpdateAnalysisItemButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisItemViewModel> allAnalysisItem)
        {
            try
            {
                IEnumerable<AnalysisItem> analysisList = _unitOfWork.AnalysisItem.Find(p => p.ClinicSectionId == clinicSectionId);
                foreach (var item in analysisList)
                {
                    var ana = allAnalysisItem.FirstOrDefault(x => x.Guid == item.Guid);
                    if (ana != null)
                    {
                        item.IsButton = ana.IsButton;
                        item.Priority = (int?)ana.Priority;
                    }
                    else
                    {
                        item.IsButton = false;
                    }
                }
                _unitOfWork.Complete();
                _worker.RemoveCach();
            }
            catch (Exception e) { throw e; }
        }


        public Guid AddNewAnalysisItem(AnalysisItemViewModel analysisItem)
        {
            try
            {
                AnalysisItemMinMaxValue ammDto = null;

                if (analysisItem.AnalysisItemMinMaxValues != null)
                {
                    ammDto = Common.ConvertModels<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>.convertModels(analysisItem.AnalysisItemMinMaxValues);
                    analysisItem.AnalysisItemMinMaxValues = null;
                }

                AnalysisItem analysisItemDto = ConvertModelsListsViewModelToDto(analysisItem);

                analysisItemDto.Guid = Guid.NewGuid();
                analysisItemDto.Priority = Convert.ToInt32(_unitOfWork.AnalysisItem.Count()) + 1;
                analysisItemDto.IsButton = false;
                _unitOfWork.AnalysisItem.Add(analysisItemDto);

                if (ammDto != null)
                {
                    ammDto.AnalysisItemId = analysisItemDto.Guid;
                    _unitOfWork.AnalysisItemMinMaxValue.Add(ammDto);
                }
                _unitOfWork.Complete();

                _worker.RemoveCach();

                return analysisItemDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateAnalysisItem(AnalysisItemViewModel analysisItem)
        {
            AnalysisItemMinMaxValue ammDto = null;

            if (analysisItem.AnalysisItemMinMaxValues != null)
            {
                ammDto = Common.ConvertModels<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>.convertModels(analysisItem.AnalysisItemMinMaxValues);
                analysisItem.AnalysisItemMinMaxValues = null;
            }

            AnalysisItem analysisItemDto = ConvertModelsListsViewModelToDto(analysisItem);
            if (ammDto?.Guid != Guid.Empty && ammDto!= null)
                analysisItemDto.AnalysisItemMinMaxValues.Add(ammDto);

            _unitOfWork.AnalysisItemMinMaxValue.RemoveRange(_unitOfWork.AnalysisItemMinMaxValue.Find(x => x.AnalysisItemId == analysisItemDto.Guid));
            _unitOfWork.AnalysisItemValuesRange.RemoveRange(_unitOfWork.AnalysisItemValuesRange.Find(x => x.AnalysisItemId == analysisItemDto.Guid));

            if (ammDto != null)
            {
                ammDto.AnalysisItemId = analysisItemDto.Guid;
                _unitOfWork.AnalysisItemMinMaxValue.Add(ammDto);
            }

            if (analysisItemDto.AnalysisItemValuesRanges.Count != 0)
            {
                foreach (var a in analysisItemDto.AnalysisItemValuesRanges)
                {
                    a.AnalysisItemId = analysisItemDto.Guid;
                }
                _unitOfWork.AnalysisItemValuesRange.AddRange(analysisItemDto.AnalysisItemValuesRanges);
            }
            _unitOfWork.AnalysisItem.UpdateState(analysisItemDto);
            _unitOfWork.Complete();
            _worker.RemoveCach();
            return analysisItemDto.Guid;

        }

        public IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemName()
        {
            return _unitOfWork.AnalysisItem.GetAll().Select(a => new AnalysisItemViewModel
            {
                Name = a.Name
            });
        }

        public string CreateChartInResult(Guid analysisItemId)
        {
            var res = _unitOfWork.AnalysisItem.GetWithValueType(analysisItemId);
            if (res.ValueType.Name == "Optional")
                return "OptionalTypeCannotShowInChart";

            res.ShowChart = !res.ShowChart.GetValueOrDefault(false);

            _unitOfWork.AnalysisItem.UpdateState(res);
            _unitOfWork.Complete();


            var AllAnalysisItems = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
            if (AllAnalysisItems != null && AllAnalysisItems.Any())
            {
                var index = AllAnalysisItems.FindIndex(p => p.Guid == analysisItemId);
                if (index != -1)
                {
                    AllAnalysisItems[index].ShowChart = res.ShowChart;
                    _memoryCache.Remove("analysisItems");
                    _memoryCache.Set("analysisItems", AllAnalysisItems);

                }
            }

            return "1";
        }

        public AnalysisItemViewModel ConvertModels(AnalysisItem AnalysisItem)
        {
            AnalysisItemViewModel AnalysisItemView = new AnalysisItemViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.MapFrom(c => c.AnalysisItemMinMaxValues.FirstOrDefault()));
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemView = mapper.Map<AnalysisItem, AnalysisItemViewModel>(AnalysisItem);
            return AnalysisItemView;
        }

        public static List<AnalysisItemViewModel> ConvertModelsLists(IEnumerable<AnalysisItem> AnalysisItem)
        {
            List<AnalysisItemViewModel> AnalysisItemViewModelList = new List<AnalysisItemViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(x => x.AnalysisItemMinMaxValues, a => a.Ignore())
                .ForMember(x => x.AnalysisItemValuesRanges, a => a.Ignore());
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemViewModelList = mapper.Map<IEnumerable<AnalysisItem>, List<AnalysisItemViewModel>>(AnalysisItem);
            return AnalysisItemViewModelList;
        }


        public static List<AnalysisItemJustNameAndGuidViewModel> ConvertToAnalysisItemJustNameAndGuidModelsLists(IEnumerable<AnalysisItem> AnalysisItem)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<AnalysisItem, AnalysisItemJustNameAndGuidViewModel>()
                .ForMember(a => a.Code, b => b.MapFrom(c => c.Code))
                .ForMember(a => a.Guid, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.Price, b => b.MapFrom(c => c.Amount))
                .ForMember(a => a.Priority, b => b.MapFrom(c => c.Priority))
                .ForMember(a => a.IsButton, b => b.MapFrom(c => c.IsButton))
                .ForAllOtherMembers(a => a.Ignore())
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<AnalysisItem>, List<AnalysisItemJustNameAndGuidViewModel>>(AnalysisItem);
        }


        public static AnalysisItem ConvertModelsListsViewModelToDto(AnalysisItemViewModel AnalysisItem)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<AnalysisItemViewModel, AnalysisItem>();

                cfg.CreateMap<AnalysisItemMinMaxValueViewModel, AnalysisItemMinMaxValue>();
                cfg.CreateMap<AnalysisItemValuesRangeViewModel, AnalysisItemValuesRange>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<AnalysisItemViewModel, AnalysisItem>(AnalysisItem);
        }


    }
}
