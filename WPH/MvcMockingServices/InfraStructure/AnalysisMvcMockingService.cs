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
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class AnalysisMvcMockingService : IAnalysisMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly AnalysisItemWorker _worker;

        public AnalysisMvcMockingService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, AnalysisItemWorker worker)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _memoryCache = memoryCache;
            _worker = worker;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Analysis/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public List<AnalysisViewModel> GetAllAnalysisByClinicSectionId(Guid clinicSectionId)
        {
            try
            {

                var Canalysis = _memoryCache.Get<List<Analysis>>("analysis");
                List<AnalysisViewModel> analysis = new List<AnalysisViewModel>();

                if (Canalysis is null || Canalysis.Count == 0)
                {
                    IEnumerable<Analysis> Analysiss = _unitOfWork.Analysis.GetAllAnalysisByClinicSectionId(clinicSectionId);
                    analysis = ConvertModelsLists(Analysiss);
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);
                }
                else
                {
                    analysis = ConvertModelsLists(Canalysis.Where(x => x.ClinicSectionId == clinicSectionId));
                }

                Indexing<AnalysisViewModel> indexing = new Indexing<AnalysisViewModel>();
                return indexing.AddIndexing(analysis);

            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<AnalysisViewModel> GetAllAnalysis()
        {

            List<Analysis> Analysiss = _unitOfWork.Analysis.GetAllAnalysis().OrderByDescending(x => x.CreateDate).ToList();
            List<AnalysisViewModel> analysises = ConvertModelsLists(Analysiss);
            Indexing<AnalysisViewModel> indexing = new Indexing<AnalysisViewModel>();
            return indexing.AddIndexing(analysises);
        }

        public IEnumerable<AnalysisWithAnalysisItemViewModel> GetAllAnalysisWithAnalysisItems(Guid clinicSectionId, int DestCurrencyId)
        {
            try
            {
                var Canalysis = _memoryCache.Get<List<Analysis>>("analysis");
                List<AnalysisWithAnalysisItemViewModel> analysis = new List<AnalysisWithAnalysisItemViewModel>();

                if (Canalysis is null || Canalysis.Count == 0)
                {
                    IEnumerable<Analysis> Analysiss = _unitOfWork.Analysis.GetAllAnalysisWithAnalysisItems(clinicSectionId, DestCurrencyId);
                    analysis = ConvertModelListForGetAllAnalysisWithAnalysisItems(Analysiss);
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);
                }
                else
                {
                    analysis = ConvertModelListForGetAllAnalysisWithAnalysisItems(Canalysis.Where(x => x.ClinicSectionId == clinicSectionId));
                }

                return analysis;
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<AnalysisViewModel> GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(Guid groupId)
        {
            IEnumerable<Analysis> Analysiss = _unitOfWork.Analysis.GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(groupId).OrderByDescending(x => x.CreateDate);
            List<AnalysisViewModel> analysis = ConvertModelsLists(Analysiss).ToList();
            Indexing<AnalysisViewModel> indexing = new Indexing<AnalysisViewModel>();
            return indexing.AddIndexing(analysis);
        }


        public void SwapPriority(Guid AnalysisId, string type)
        {
            try
            {
                Analysis currentAnalysisItem = _unitOfWork.Analysis.GetSingle(x => x.Guid == AnalysisId);
                int? currentAnalysisItemPriority = currentAnalysisItem.Priority;
                Analysis swapAnalysisItem = new Analysis();
                if (type == "Up")
                {
                    swapAnalysisItem = _unitOfWork.Analysis.GetSingle(x => x.Priority == currentAnalysisItemPriority - 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                else if (type == "Down")
                {

                    swapAnalysisItem = _unitOfWork.Analysis.GetSingle(x => x.Priority == currentAnalysisItemPriority + 1);
                    if (swapAnalysisItem == null)
                    {
                        return;
                    }
                }
                currentAnalysisItem.Priority = swapAnalysisItem.Priority;
                swapAnalysisItem.Priority = currentAnalysisItemPriority;
                _unitOfWork.Analysis.UpdateState(currentAnalysisItem);
                _unitOfWork.Analysis.UpdateState(swapAnalysisItem);
                _unitOfWork.Complete();
                _worker.RemoveCach();
            }
            catch { }
        }

        public void ActiveDeactiveAnalysis(Guid analysisId)
        {
            Analysis analysis = _unitOfWork.Analysis.Get(analysisId);
            analysis.IsActive = !analysis.IsActive;

            _unitOfWork.Analysis.UpdateState(analysis);
            _unitOfWork.Complete();

            var AllAnalysis = _memoryCache.Get<List<Analysis>>("analysis");
            if (AllAnalysis != null && AllAnalysis.Any())
            {
                var index = AllAnalysis.FindIndex(p => p.Guid == analysisId);
                if (index != -1)
                {
                    AllAnalysis[index].IsActive = analysis.IsActive;
                    _memoryCache.Remove("analysis");
                    _memoryCache.Set("analysis", AllAnalysis);

                }
            }
        }

        public void UpdateAnalysisButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisWithAnalysisItemViewModel> allAnalysis)
        {
            try
            {
                IEnumerable<Analysis> analysisList = _unitOfWork.Analysis.Find(p => p.ClinicSectionId == clinicSectionId);
                foreach (var item in analysisList)
                {
                    var ana = allAnalysis.FirstOrDefault(x => x.Guid == item.Guid);
                    if (ana != null)
                    {
                        item.IsButton = ana.IsButton;
                        item.Priority = ana.Priority;
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

        public OperationStatus RemoveAnalysisItemFromAnalysis(Guid id)
        {

            try
            {
                _unitOfWork.AnalysisAnalysisItem.RemoveAnalysisItemFromAnalysis(id);
                //_unitOfWork.AnalysisAnalysisItem.Remove(Analysis);
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

        public void AddNewAnalysisItemToAnalysis(Analysis_AnalysisItemViewModel analysis)
        {
            try
            {
                AnalysisAnalysisItem AnalysisItem = Common.ConvertModels<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>.convertModels(analysis);
                AnalysisItem.Guid = Guid.NewGuid();
                AnalysisItem.Priority = Convert.ToInt32(_unitOfWork.AnalysisAnalysisItem.Count(x => x.AnalysisId == AnalysisItem.AnalysisId)) + 1;
                _unitOfWork.AnalysisAnalysisItem.Add(AnalysisItem);
                _unitOfWork.Complete();
                _worker.RemoveCach();
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<Analysis_AnalysisItemViewModel> GetAllAnalysisAnalysisItem(Guid analysisId)
        {
            IEnumerable<AnalysisAnalysisItem> Analysiss = _unitOfWork.AnalysisAnalysisItem.GetAllAnalysisAnalysisItem(analysisId);
            List<Analysis_AnalysisItemViewModel> analysis = ConvertModelsListsAnalysisItem(Analysiss);
            Indexing<AnalysisItemViewModel> indexing = new Indexing<AnalysisItemViewModel>();
            indexing.AddIndexing(analysis.Select(x => x.AnalysisItem).ToList());
            return analysis;
        }

        public OperationStatus RemoveAnalysis(Guid AnalysisId)
        {
            try
            {
                _unitOfWork.Analysis.RemoveAnalysis(AnalysisId);
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

        public AnalysisViewModel GetAnalysis(Guid AnalysisId)
        {
            try
            {
                Analysis Analysis = _unitOfWork.Analysis.Get(AnalysisId);
                decimal dDiscount = Analysis.Discount == null ? 0 : decimal.Parse(Analysis.Discount.ToString());
                Analysis.Discount = decimal.Parse(dDiscount.ToString("G29"));
                return ConvertModels(Analysis);
            }
            catch { return null; }
        }

        public Guid AddNewAnalysis(AnalysisViewModel analysis)
        {
            try
            {
                Analysis Analysis = Common.ConvertModels<Analysis, AnalysisViewModel>.convertModels(analysis);
                Analysis.Guid = Guid.NewGuid();
                Analysis.Priority = Convert.ToInt32(_unitOfWork.Analysis.Count()) + 1;
                Analysis.IsButton = true;
                _unitOfWork.Analysis.Add(Analysis);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return Analysis.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateAnalysis(AnalysisViewModel analysis)
        {
            try
            {
                Analysis Analysis = Common.ConvertModels<Analysis, AnalysisViewModel>.convertModels(analysis);
                _unitOfWork.Analysis.UpdateState(Analysis);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return Analysis.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<AnalysisViewModel> GetAllAnalysisAndAnalysisItem(Guid clinicSectionId)
        {
            try
            {

                var Canalysis = _memoryCache.Get<List<Analysis>>("analysis");

                List<AnalysisViewModel> analysis = new List<AnalysisViewModel>();

                if (Canalysis is null || Canalysis.Count() == 0)
                {
                    IEnumerable<Analysis> Analysiss = _unitOfWork.Analysis.GetAllAnalysisByClinicSectionId(clinicSectionId);
                    analysis = ConvertModelsLists(Analysiss);
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);
                }
                else
                {
                    analysis = ConvertModelsLists(Canalysis.Where(x => x.ClinicSectionId == clinicSectionId));
                }

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

                List<AnalysisViewModel> allAnalysis = new List<AnalysisViewModel>();

                allAnalysis.AddRange(analysis.Select(a => new AnalysisViewModel { Name = a.Name, Guid = a.Guid }));
                allAnalysis.AddRange(AnalysisItems.Select(a => new AnalysisViewModel { Name = a.Name, Guid = a.Guid }));

                return allAnalysis;

            }
            catch (Exception e) { throw e; }
        }

        public static List<AnalysisViewModel> ConvertModelsLists(IEnumerable<Analysis> Analysis)
        {

            List<AnalysisViewModel> AnalysisViewModelList = new List<AnalysisViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Analysis, AnalysisViewModel>()
                .ForMember(a => a.Guid, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.Note, b => b.MapFrom(c => c.Note))
                .ForMember(a => a.IsActive, b => b.MapFrom(c => c.IsActive))
                .ForMember(a => a.Code, b => b.MapFrom(c => c.Code))
                .ForMember(a => a.Abbreviation, b => b.MapFrom(c => c.Abbreviation))
                .ForMember(a => a.Discount, b => b.MapFrom(c => c.Discount))
                .ForMember(a => a.CreatedUserName, b => b.MapFrom(c => c.CreateUser.Name))
                .ForMember(a => a.ModifiedUserName, b => b.MapFrom(c => c.ModifiedUser.Name))
                .ForMember(a => a.DiscountCurrencyName, b => b.MapFrom(c => c.DiscountCurrency.Name))
                .ForMember(a => a.TotalAmount, b => b.MapFrom(c => c.TotalAmount))
                .ForMember(a => a.TotalAmountWithDiscount, b => b.MapFrom(c => c.TotalAmountWithDiscount))
                .ForAllOtherMembers(a => a.Ignore());
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.Guid, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.Code, b => b.MapFrom(c => c.Code))
                .ForMember(a => a.Amount, b => b.MapFrom(c => c.Amount))
                .ForAllOtherMembers(a => a.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            AnalysisViewModelList = mapper.Map<IEnumerable<Analysis>, List<AnalysisViewModel>>(Analysis);
            return AnalysisViewModelList;
        }

        public static List<Analysis_AnalysisItemViewModel> ConvertModelsListsAnalysisItem(IEnumerable<AnalysisAnalysisItem> AnalysisItem)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AmountCurrency, b => b.Ignore())
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                .ForMember(a => a.BaseInfoUnit, b => b.Ignore())
                .ForMember(a => a.ValueType, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<AnalysisAnalysisItem>, List<Analysis_AnalysisItemViewModel>>(AnalysisItem);
        }

        public List<AnalysisWithAnalysisItemViewModel> ConvertModelListForGetAllAnalysisWithAnalysisItems(IEnumerable<Analysis> Analysis)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Analysis, AnalysisWithAnalysisItemViewModel>()
                .ForMember(a => a.Price, b => b.MapFrom(c => c.TotalAmount))
                .ForMember(a => a.Guid, b => b.MapFrom(c => c.Guid))
                .ForMember(a => a.Name, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.Code, b => b.MapFrom(c => c.Code))
                .ForMember(a => a.Priority, b => b.MapFrom(c => c.Priority))
                .ForMember(a => a.Discount, b => b.MapFrom(c => c.Discount))
                .ForMember(a => a.IsButton, b => b.MapFrom(c => c.IsButton))
                .ForAllOtherMembers(a => a.Ignore())
                ;
                cfg.CreateMap<AnalysisItem, AnalysisItemJustNameAndGuidViewModel>()
                .ForMember(a => a.Price, b => b.MapFrom(c => c.Amount))
                .ForAllOtherMembers(a => a.Ignore())
                ;

            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Analysis>, List<AnalysisWithAnalysisItemViewModel>>(Analysis);
        }

        public AnalysisViewModel ConvertModels(Analysis Analysis)
        {
            AnalysisViewModel AnalysisView = new AnalysisViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisView = mapper.Map<Analysis, AnalysisViewModel>(Analysis);
            return AnalysisView;
        }

        
    }
}
