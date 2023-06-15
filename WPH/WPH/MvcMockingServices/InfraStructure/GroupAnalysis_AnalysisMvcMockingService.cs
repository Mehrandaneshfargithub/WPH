using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class GroupAnalysis_AnalysisMvcMockingService : IGroupAnalysis_AnalysisMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupAnalysis_AnalysisMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        public static List<GroupAnalysis_AnalysisViewModel> convertModelsLists(IEnumerable<GroupAnalysisAnalysis> GroupAnalysis_Analysis)
        {
            List<GroupAnalysis_AnalysisViewModel> GroupAnalysis_AnalysisViewModelList = new List<GroupAnalysis_AnalysisViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());
                cfg.CreateMap<Analysis, AnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysis_AnalysisViewModelList = mapper.Map<IEnumerable<GroupAnalysisAnalysis>, List<GroupAnalysis_AnalysisViewModel>>(GroupAnalysis_Analysis);
            return GroupAnalysis_AnalysisViewModelList;
        }

        public IEnumerable<GroupAnalysis_AnalysisViewModel> GetAllGroupAnalysis_Analysis(Guid groupAnalysisId)
        {

            List<GroupAnalysisAnalysis> groupAnalysis_AnalysisDtos = new List<GroupAnalysisAnalysis>();
            groupAnalysis_AnalysisDtos = _unitOfWork.GroupAnalysisAnalyses.GetAllGroupAnalysisAnalyses(groupAnalysisId).ToList();
            List<GroupAnalysis_AnalysisViewModel> groupAnalysis_Analysises = convertModelsLists(groupAnalysis_AnalysisDtos).ToList();
            Indexing<GroupAnalysis_AnalysisViewModel> indexing = new Indexing<GroupAnalysis_AnalysisViewModel>();
            return indexing.AddIndexing(groupAnalysis_Analysises);
        }

        public IEnumerable<GroupAnalysis_AnalysisViewModel> GetAllGroupAnalysis_Analysis()
        {
            List<GroupAnalysisAnalysis> groupAnalysis_AnalysisDtos = new List<GroupAnalysisAnalysis>();
            groupAnalysis_AnalysisDtos = _unitOfWork.GroupAnalysisAnalyses.GetAllGroupAnalysisAnalyses().ToList();
            List<GroupAnalysis_AnalysisViewModel> groupAnalysis_Analysises = convertModelsLists(groupAnalysis_AnalysisDtos).ToList();
            Indexing<GroupAnalysis_AnalysisViewModel> indexing = new Indexing<GroupAnalysis_AnalysisViewModel>();
            return indexing.AddIndexing(groupAnalysis_Analysises);
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/GroupAnalysis_Analysis/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItemForSpecificDate(Guid clinicSectionId, Guid GroupAnalysisItemTypeId, int periodId, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public void SwapPriority(Guid Id, Guid GroupAnalysisId, string type)
        {
            try
            {
                GroupAnalysisAnalysis currentGroupAnalysis_Analysis = _unitOfWork.GroupAnalysisAnalyses.GetSingle(x => x.Guid == Id);
                decimal? currentGroupAnalysis_AnalysisPriority = currentGroupAnalysis_Analysis.Priority;
                GroupAnalysisAnalysis swapGroupAnalysis_Analysis = new GroupAnalysisAnalysis();
                if (type == "Up")
                {
                    swapGroupAnalysis_Analysis = _unitOfWork.GroupAnalysisAnalyses.Find(x => x.GroupAnalysisId == GroupAnalysisId && x.Priority < currentGroupAnalysis_AnalysisPriority).OrderByDescending(x => x.Priority).FirstOrDefault();
                }
                else if (type == "Down")
                {
                    swapGroupAnalysis_Analysis = _unitOfWork.GroupAnalysisAnalyses.Find(x => x.GroupAnalysisId == GroupAnalysisId && x.Priority > currentGroupAnalysis_AnalysisPriority).OrderBy(x => x.Priority).FirstOrDefault();
                }
                currentGroupAnalysis_Analysis.Priority = swapGroupAnalysis_Analysis.Priority;
                swapGroupAnalysis_Analysis.Priority = currentGroupAnalysis_AnalysisPriority;
                _unitOfWork.GroupAnalysisAnalyses.UpdatePriority(currentGroupAnalysis_Analysis);
                _unitOfWork.GroupAnalysisAnalyses.UpdatePriority(swapGroupAnalysis_Analysis);
                _unitOfWork.Complete();
            }
            catch { }
        }


        public OperationStatus Remove(Guid GroupAnalysis_AnalysisId)
        {
            try
            {
                GroupAnalysisAnalysis GroupAnalysis_Analysis = _unitOfWork.GroupAnalysisAnalyses.Get(GroupAnalysis_AnalysisId);
                _unitOfWork.GroupAnalysisAnalyses.Remove(GroupAnalysis_Analysis);
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

        public OperationStatus RemoveGroupAnalysis_AnalysisWithGroupAnalysisId(Guid GroupAnalysisId)
        {
            try
            {
                IEnumerable<GroupAnalysisAnalysis> groupAnalysis_Analysis = _unitOfWork.GroupAnalysisAnalyses.Find(x => x.GroupAnalysisId == GroupAnalysisId);
                if (groupAnalysis_Analysis.Count() != 0)
                    _unitOfWork.GroupAnalysisAnalyses.RemoveRange(groupAnalysis_Analysis);
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

        public GroupAnalysis_AnalysisViewModel GetGroupAnalysis_Analysis(Guid GroupAnalysis_AnalysisId)
        {
            try
            {
                GroupAnalysisAnalysis GroupAnalysis_AnalysisDto = _unitOfWork.GroupAnalysisAnalyses.Get(GroupAnalysis_AnalysisId);
                return convertModels(GroupAnalysis_AnalysisDto);
            }
            catch { return null; }
        }
        public GroupAnalysis_AnalysisViewModel convertModels(GroupAnalysisAnalysis GroupAnalysis_Analysis)
        {
            GroupAnalysis_AnalysisViewModel GroupAnalysis_AnalysisView = new GroupAnalysis_AnalysisViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());
                cfg.CreateMap<Analysis, AnalysisViewModel>().ForMember(a => a.CreatedUser, b => b.Ignore())
                .ForMember(a => a.ModifiedUser, b => b.Ignore()).ForMember(a => a.DiscountCurrency, b => b.Ignore());

            });
            IMapper mapper = config.CreateMapper();
            GroupAnalysis_AnalysisView = mapper.Map<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>(GroupAnalysis_Analysis);
            return GroupAnalysis_AnalysisView;
        }

        public Guid AddNewGroupAnalysis_Analysis(GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis, Guid groupAnalysisId)
        {
            try
            {
                GroupAnalysisAnalysis groupAnalysis_AnalysisDto = Common.ConvertModels<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>.convertModels(groupAnalysis_Analysis);
                IEnumerable<GroupAnalysisAnalysis> groupanaDtos = _unitOfWork.GroupAnalysisAnalyses.GetAllGroupAnalysisAnalyses(groupAnalysisId);
                if (groupanaDtos.Count() != 0)
                {
                    var maxp = groupanaDtos.Max(n => n.Priority);
                    groupAnalysis_AnalysisDto.Priority = maxp + 1;
                }
                else
                {
                    groupAnalysis_AnalysisDto.Priority = 1;
                }
                
                _unitOfWork.GroupAnalysisAnalyses.Add(groupAnalysis_AnalysisDto);
                _unitOfWork.Complete();
                return groupAnalysis_AnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateGroupAnalysis_Analysis(GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis)
        {
            try
            {
                GroupAnalysisAnalysis groupAnalysis_AnalysisDto = Common.ConvertModels<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>.convertModels(groupAnalysis_Analysis);
                _unitOfWork.GroupAnalysisAnalyses.UpdateState(groupAnalysis_AnalysisDto);
                _unitOfWork.Complete();
                return groupAnalysis_AnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
