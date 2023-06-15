using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisResult;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class AnalysisResultMvcMockingService : IAnalysisResultMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public AnalysisResultMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            
        }


        public static List<AnalysisResultViewModel> convertModelsLists(IEnumerable<AnalysisResult> AnalysisResults)
        {
            List<AnalysisResultViewModel> AnalysisResultViewModelList = new List<AnalysisResultViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();

            });
            IMapper mapper = config.CreateMapper();
            AnalysisResultViewModelList = mapper.Map<IEnumerable<AnalysisResult>, List<AnalysisResultViewModel>>(AnalysisResults);
            return AnalysisResultViewModelList;
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisResult/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }



        public void AddAnalysisResultToPatientReception(IEnumerable<AnalysisResultViewModel> analysisResults)
        {
            try
            {
                IEnumerable<AnalysisResult> AnalysisResultDto = Common.ConvertModels<AnalysisResult, AnalysisResultViewModel>.convertModelsLists(analysisResults);

                _unitOfWork.AnalysisResult.AddNewRange(AnalysisResultDto);
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<AnalysisResultViewModel> GetAnalysisResultForReport(Guid patientReceptionId)
        {
            try
            {
                


                IEnumerable<AnalysisResult> AnalysisResultDto = _unitOfWork.AnalysisResult.GetAnalysisResultForReport(patientReceptionId);
                return Common.ConvertModels<AnalysisResultViewModel, AnalysisResult>.convertModelsLists(AnalysisResultDto);


            }
            catch (Exception ex) { throw ex; }
        }

        public Guid AddNewAnalysisResult(AnalysisResultViewModel analysisResult)
        {
            throw new NotImplementedException();
        }


        public Guid AddNewAnalysisResult(AnalysisResultViewModel AnalysisResult, Guid clinicSectionId, Guid userId)
        {
            try
            {
                AnalysisResult AnalysisResultDto = Common.ConvertModels<AnalysisResult, AnalysisResultViewModel>.convertModels(AnalysisResult);

                AnalysisResult.Guid = Guid.NewGuid();
                _unitOfWork.AnalysisResult.Add(AnalysisResultDto);
                _unitOfWork.Complete();
                return AnalysisResult.Guid;
            }
            catch (Exception ex) { throw ex; }
        }


        public IEnumerable<AnalysisResultViewModel> GetAllAnalysisResult(Guid analysisResultMasterId)
        {
            IEnumerable<AnalysisResult> AnalysisResultDto = _unitOfWork.AnalysisResult.Find(a=>a.AnalysisResultMasterId == analysisResultMasterId);
            return Common.ConvertModels<AnalysisResultViewModel, AnalysisResult>.convertModelsLists(AnalysisResultDto);
        }


        public IEnumerable<PastValuesViewModel> GetPastAnalysisResults(Guid patientId, List<Guid> analysisItemId)
        {
            try
            {
                IEnumerable<AnalysisResult> AnalysisResult = _unitOfWork.AnalysisResult.GetPastAnalysisResults(patientId, analysisItemId);
                List<PastValuesViewModel> allPastValues = new List<PastValuesViewModel>();


                foreach (var item in AnalysisResult)
                {
                    string name = item.AnalysisItem.Name;
                    if (allPastValues.Contains(new PastValuesViewModel { Name = name }))
                    {
                        PastValuesViewModel pa = allPastValues.FirstOrDefault(x => x.Name == name);
                        if (pa.Value2 != null)
                        {
                            pa.Value2 = item.Value;
                            pa.Date2 = item.CreatedDate.Value.ToShortDateString();
                        }
                        if (pa.Value3 != null)
                        {
                            pa.Value3 = item.Value;
                            pa.Date3 = item.CreatedDate.Value.ToShortDateString();
                        }
                    }
                    else
                    {
                        allPastValues.Add(new PastValuesViewModel { Name = item.AnalysisItem.Name, Value = item.Value, Date = item.CreatedDate.Value.ToShortDateString() });
                    }

                }
                return allPastValues;
            }
            catch (Exception e) { throw e; }
        }

        public Guid UpdateAnalysisResult(AnalysisResultViewModel AnalysisResult)
        {
            try
            {
                AnalysisResult AnalysisResultDto = Common.ConvertModels<AnalysisResult, AnalysisResultViewModel>.convertModels(AnalysisResult);
                _unitOfWork.AnalysisResult.UpdateState(AnalysisResultDto);
                _unitOfWork.Complete();
                return AnalysisResult.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public OperationStatus RemoveAnalysisResult(Guid AnalysisResultId)
        {
            try
            {
                AnalysisResult AnalysisResult = _unitOfWork.AnalysisResult.Get(AnalysisResultId);
                _unitOfWork.AnalysisResult.Remove(AnalysisResult);
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

        

        public AnalysisResultViewModel GetAnalysisResult(Guid AnalysisResultId)
        {
            try
            {
                AnalysisResult AnalysisResult = _unitOfWork.AnalysisResult.Get(AnalysisResultId);
                return convertModels(AnalysisResult);
            }
            catch { return null; }
        }
        public AnalysisResultViewModel convertModels(AnalysisResult AnalysisResult)
        {
            AnalysisResultViewModel AnalysisResultView = new AnalysisResultViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisResult, AnalysisResultViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisResultView = mapper.Map<AnalysisResult, AnalysisResultViewModel>(AnalysisResult);
            return AnalysisResultView;
        }

        
    }


}
