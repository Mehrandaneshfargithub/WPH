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
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class GroupAnalysisMvcMockingService : IGroupAnalysisMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly AnalysisItemWorker _worker;

        public GroupAnalysisMvcMockingService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, AnalysisItemWorker worker)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _memoryCache = memoryCache;
            _worker = worker;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/GroupAnalysis/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public IEnumerable<GroupAnalysisViewModel> GetAllGroupAnalysis(Guid clinicSectionId, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<GroupAnalysis> groupAnalysisDtos = new List<GroupAnalysis>();

                DateTime dateFrom = DateTime.Now;
                DateTime dateTo = DateTime.Now;
                groupAnalysisDtos = _unitOfWork.GroupAnalysis.GetAllGroupAnalysis(clinicSectionId, dateFrom, dateTo).OrderBy(x => x.Priority).ToList();
                List<GroupAnalysisViewModel> groupAnalysises = ConvertModelsLists(groupAnalysisDtos).ToList();
                Indexing<GroupAnalysisViewModel> indexing = new Indexing<GroupAnalysisViewModel>();
                return indexing.AddIndexing(groupAnalysises);
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<GroupAnalysisViewModel> GetAllGroupAnalysis()
        {
            List<GroupAnalysis> groupAnalysisDtos = _unitOfWork.GroupAnalysis.GetAllGroupAnalysis().ToList();
            List<GroupAnalysisViewModel> groupAnalysises = ConvertModelsLists(groupAnalysisDtos).ToList();
            Indexing<GroupAnalysisViewModel> indexing = new Indexing<GroupAnalysisViewModel>();
            return indexing.AddIndexing(groupAnalysises);
        }

        public List<GroupAnalysisJustNameAndGuid> GetAllGroupAnalysisWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId)
        {
            try
            {
                var Canalysis = _memoryCache.Get<List<GroupAnalysis>>("groupAnalysis");
                List<GroupAnalysisJustNameAndGuid> analysis = new List<GroupAnalysisJustNameAndGuid>();

                if (Canalysis is null || Canalysis.Count == 0)
                {
                    IEnumerable<GroupAnalysis> Analysiss = _unitOfWork.GroupAnalysis.GetAllGroupAnalysisWithNameAndGuidOnly(clinicSectionId, DestCurrencyId);
                    analysis = ConvertToGroupAnalysisJustNameAndGuidModelsLists(Analysiss);
                    CancellationToken s = new CancellationToken();
                    _worker.StartAsync(s);
                }
                else
                {
                    analysis = ConvertToGroupAnalysisJustNameAndGuidModelsLists(Canalysis.Where(x => x.ClinicSectionId == clinicSectionId));
                }

                return analysis;
            }
            catch (Exception e) { throw e; }
        }


        public void UpdateGroupAnalysisButtonAndPriority(Guid clinicSectionId, IEnumerable<GroupAnalysisJustNameAndGuid> allGroup)
        {
            try
            {
                IEnumerable<GroupAnalysis> analysisList = _unitOfWork.GroupAnalysis.Find(p => p.ClinicSectionId == clinicSectionId);
                foreach (var item in analysisList)
                {
                    var ana = allGroup.FirstOrDefault(x => x.Guid == item.Guid);
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

        public void ActiveDeactiveAnalysis(Guid analysisId)
        {
            try
            {
                GroupAnalysis analysis = _unitOfWork.GroupAnalysis.Get(analysisId);
                analysis.IsActive = !analysis.IsActive;
                _unitOfWork.GroupAnalysis.UpdateState(analysis);
                _unitOfWork.Complete();
            }
            catch (Exception e) { throw e; }
        }

        public void SwapPriority(Guid Id, Guid ClinicSectionId, string type)
        {
            try
            {
                GroupAnalysis currentGroupAnalysis = _unitOfWork.GroupAnalysis.GetSingle(x => x.Guid == Id);
                int? currentGroupAnalysisPriority = currentGroupAnalysis.Priority;
                GroupAnalysis swapGroupAnalysis = new GroupAnalysis();
                if (type == "Up")
                {
                    swapGroupAnalysis = _unitOfWork.GroupAnalysis.GetSingle(x => x.Priority == currentGroupAnalysisPriority - 1);
                    if (swapGroupAnalysis == null)
                    {
                        return;
                    }
                }
                else if (type == "Down")
                {

                    swapGroupAnalysis = _unitOfWork.GroupAnalysis.GetSingle(x => x.Priority == currentGroupAnalysisPriority + 1);
                    if (swapGroupAnalysis == null)
                    {
                        return;
                    }
                }
                currentGroupAnalysis.Priority = swapGroupAnalysis.Priority;
                swapGroupAnalysis.Priority = currentGroupAnalysisPriority;
                _unitOfWork.GroupAnalysis.UpdatePriority(currentGroupAnalysis);
                _unitOfWork.GroupAnalysis.UpdatePriority(swapGroupAnalysis);
                _unitOfWork.Complete();
            }
            catch { }
        }


        public OperationStatus RemoveGroupAnalysis(Guid GroupAnalysisId)
        {
            try
            {
                GroupAnalysis GroupAnalysis = _unitOfWork.GroupAnalysis.Get(GroupAnalysisId);
                _unitOfWork.GroupAnalysis.Remove(GroupAnalysis);
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

        public GroupAnalysisViewModel GetGroupAnalysis(Guid GroupAnalysisId)
        {
            try
            {
                GroupAnalysis GroupAnalysisDto = _unitOfWork.GroupAnalysis.Get(GroupAnalysisId);
                decimal dDiscount = GroupAnalysisDto.Discount == null ? 0 : decimal.Parse(GroupAnalysisDto.Discount.ToString());
                GroupAnalysisDto.Discount = decimal.Parse(dDiscount.ToString("G29"));
                return ConvertModels(GroupAnalysisDto);
            }
            catch { return null; }
        }


        public Guid AddNewGroupAnalysis(GroupAnalysisViewModel groupAnalysis)
        {
            try
            {
                GroupAnalysis groupAnalysisDto = Common.ConvertModels<GroupAnalysis, GroupAnalysisViewModel>.convertModels(groupAnalysis);
                IEnumerable<GroupAnalysis> groupanaDtos = _unitOfWork.GroupAnalysis.GetAllGroupAnalysis(groupAnalysis.ClinicSectionId.Value);
                if (groupanaDtos.Any())
                {
                    var maxp = groupanaDtos.Max(n => n.Priority);
                    groupAnalysisDto.Priority = maxp + 1;
                }
                else
                {
                    groupAnalysisDto.Priority = 1;
                }

                groupAnalysisDto.Priority = Convert.ToInt32(_unitOfWork.GroupAnalysis.Count()) + 1;
                groupAnalysisDto.IsButton = false;
                _unitOfWork.GroupAnalysis.Add(groupAnalysisDto);
                _unitOfWork.Complete();
                return groupAnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateGroupAnalysis(GroupAnalysisViewModel groupAnalysis)
        {
            try
            {
                GroupAnalysis groupAnalysisDto = Common.ConvertModels<GroupAnalysis, GroupAnalysisViewModel>.convertModels(groupAnalysis);
                _unitOfWork.GroupAnalysis.UpdateState(groupAnalysisDto);
                _unitOfWork.Complete();
                return groupAnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public GroupAnalysisViewModel ConvertModels(GroupAnalysis GroupAnalysis)
        {
            GroupAnalysisViewModel GroupAnalysisView = new GroupAnalysisViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysisView = mapper.Map<GroupAnalysis, GroupAnalysisViewModel>(GroupAnalysis);
            return GroupAnalysisView;
        }

        public static List<GroupAnalysisJustNameAndGuid> ConvertToGroupAnalysisJustNameAndGuidModelsLists(IEnumerable<GroupAnalysis> GroupAnalysis)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupAnalysis, GroupAnalysisJustNameAndGuid>()
                .ForMember(a => a.Price, b => b.MapFrom(c => c.TotalAmount))
                .ForMember(a => a.AnalysisItems, b => b.MapFrom(c => c.GroupAnalysisItems))
                ;
                cfg.CreateMap<GroupAnalysisItem, AnalysisItemJustNameAndGuidViewModel>()
                .ForMember(a => a.Guid, b => b.MapFrom(c => c.AnalysisItemId))
                ;


            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<GroupAnalysis>, List<GroupAnalysisJustNameAndGuid>>(GroupAnalysis);
        }


        public static List<GroupAnalysisViewModel> ConvertModelsLists(IEnumerable<GroupAnalysis> GroupAnalysis)
        {
            List<GroupAnalysisViewModel> GroupAnalysisViewModelList = new List<GroupAnalysisViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserInformationViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore());

            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysisViewModelList = mapper.Map<IEnumerable<GroupAnalysis>, List<GroupAnalysisViewModel>>(GroupAnalysis);
            return GroupAnalysisViewModelList;
        }

    }


}
