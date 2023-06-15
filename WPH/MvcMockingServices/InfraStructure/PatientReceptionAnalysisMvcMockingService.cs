using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices.Interface;
using WPH.Models.Chart;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PatientReceptionAnalysisMvcMockingService : IPatientReceptionAnalysisMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public PatientReceptionAnalysisMvcMockingService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _memoryCache = memoryCache;
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientReceptionAnalysis/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public Guid AddNewPatientReceptionAnalysis(PatientReceptionAnalysisViewModel newPatientReceptionAnalysis, Guid clinicSectionId)
        {
            try
            {
                //newPatientReceptionAnalysis.ClinicSectionId = clinicSectionId;
                PatientReceptionAnalysis PatientReceptionAnalysisDto = Common.ConvertModels<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>.convertModels(newPatientReceptionAnalysis);
                PatientReceptionAnalysisDto.Guid = Guid.NewGuid();
                _unitOfWork.PatientReceptionAnalysis.Add(PatientReceptionAnalysisDto);
                _unitOfWork.Complete();
                return PatientReceptionAnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdatePatientReceptionAnalysis(PatientReceptionAnalysisViewModel PatientReceptionAnalysis)
        {
            try
            {
                PatientReceptionAnalysis sPatientReceptionAnalysisDto = Common.ConvertModels<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>.convertModels(PatientReceptionAnalysis);

                _unitOfWork.PatientReceptionAnalysis.UpdateState(sPatientReceptionAnalysisDto);
                _unitOfWork.Complete();
                return sPatientReceptionAnalysisDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public OperationStatus RemovePatientReceptionAnalysis(Guid PatientReceptionAnalysisId)
        {
            try
            {
                _unitOfWork.PatientReceptionAnalysis.Remove(_unitOfWork.PatientReceptionAnalysis.Get(PatientReceptionAnalysisId));
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

        public List<PatientReceptionAnalysisViewModel> GetPatientReceptionAnalysisByReceptionId(Guid receptionId)
        {
            try
            {
                List<PatientReceptionAnalysis> PatientReceptionAnalysisDtos = _unitOfWork.PatientReceptionAnalysis.GetPatientReceptionAnalysisByReceptionId(receptionId);
                return convertModelsLists(PatientReceptionAnalysisDtos);

            }
            catch (Exception e) { return null; }
        }

        public PatientReceptionAnalysisReportViewModel GetPatientAnalysisReportByReceptionId(Guid receptionId)
        {
            try
            {
                List<PatientReceptionAnalysis> PatientReceptionAnalysisDtos = _unitOfWork.PatientReceptionAnalysis.GetPatientReceptionAnalysisByReceptionId(receptionId);
                var items = PatientReceptionAnalysisDtos.Select(p => new PatientReceptionAnalysisReportItemViewModel
                {
                    Name = p.AnalysisItem?.Name ?? p.GroupAnalysis?.Name ?? p.Analysis?.Name ?? "",
                    Temp_Price = p.Amount.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0)
                }).ToList();

                var result = new PatientReceptionAnalysisReportViewModel
                {
                    Items = items,
                    Total = items.Sum(s => s.Temp_Price).ToString("N0")
                };
                return result;

            }
            catch (Exception e) { return null; }
        }

        public PieChartViewModel GetMostUsedAnalysis(Guid userId)
        {
            try
            {
                IEnumerable<PieChartModel> allMed = _unitOfWork.PatientReceptionAnalysis.GetMostUsedAnalysis(userId).OrderByDescending(a => a.Value).Take(10);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = allMed.Select(a => a.Label).ToArray(),
                    Value = allMed.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;

            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<PatientReceptionAnalysisViewModel> GetAllPatientReceptionAnalysis(Guid receptionId)
        {
            try
            {
                var analysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
                var analysis = _memoryCache.Get<List<Analysis>>("analysis");
                var groupAnalysis = _memoryCache.Get<List<GroupAnalysis>>("groupAnalysis");
                List<PatientReceptionAnalysis> PatientReceptionAnalysisDtos = new List<PatientReceptionAnalysis>();


                if (analysisItem is not null)
                {
                    PatientReceptionAnalysisDtos = _unitOfWork.PatientReceptionAnalysis.GetPatientReceptionAnalysisByReceptionIdJustIds(receptionId);
                    foreach (var pra in PatientReceptionAnalysisDtos)
                    {
                        if (pra.AnalysisId != null)
                        {
                            pra.Analysis = analysis.SingleOrDefault(x => x.Guid == pra.AnalysisId);
                        }
                        if (pra.AnalysisItemId != null)
                        {
                            pra.AnalysisItem = analysisItem.SingleOrDefault(x => x.Guid == pra.AnalysisItemId);
                        }
                        if (pra.GroupAnalysisId != null)
                        {
                            pra.GroupAnalysis = groupAnalysis.SingleOrDefault(x => x.Guid == pra.GroupAnalysisId);
                        }
                    }


                }
                else
                {
                    PatientReceptionAnalysisDtos = _unitOfWork.PatientReceptionAnalysis.GetAllPatientReceptionAnalysis(receptionId);
                }



                return convertModelsListsAnalysis(PatientReceptionAnalysisDtos);

            }
            catch (Exception e) { return null; }
        }

        public static List<PatientReceptionAnalysisViewModel> convertModelsListsAnalysis(IEnumerable<PatientReceptionAnalysis> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {


                cfg.CreateMap<Analysis, AnalysisViewModel>()
                .ForMember(a => a.Analysis_AnalysisItem, b => b.MapFrom(c => c.AnalysisAnalysisItems));
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.MapFrom(c => c.AnalysisItemMinMaxValues.FirstOrDefault()))
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>()
                .ForMember(a => a.GroupAnalysisAnalyses, b => b.MapFrom(c => c.GroupAnalysisAnalyses))
                .ForMember(a => a.GroupAnalysisItems, b => b.MapFrom(c => c.GroupAnalysisItems));

                cfg.CreateMap<GroupAnalysisAnalysis, GroupAnalysis_AnalysisViewModel>();
                cfg.CreateMap<GroupAnalysisItem, GroupAnalysisItemViewModel>();
                cfg.CreateMap<AnalysisAnalysisItem, Analysis_AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();

                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>();


                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<User, UserInformationViewModel>();

            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<PatientReceptionAnalysis>, List<PatientReceptionAnalysisViewModel>>(ress);

        }


        public static List<PatientReceptionAnalysisViewModel> convertModelsLists(IEnumerable<PatientReceptionAnalysis> ress)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Analysis, AnalysisViewModel>();
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>()
                .ForMember(a => a.AnalysisItemMinMaxValues, b => b.Ignore())
                .ForMember(a => a.AnalysisItemValuesRanges, b => b.Ignore())
                ;
                cfg.CreateMap<GroupAnalysis, GroupAnalysisViewModel>();
                cfg.CreateMap<PatientReceptionAnalysis, PatientReceptionAnalysisViewModel>()
                .ForMember(a => a.AmountCurrency, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<PatientReceptionAnalysis>, List<PatientReceptionAnalysisViewModel>>(ress);

        }

        
    }
}
