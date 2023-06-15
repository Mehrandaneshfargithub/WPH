using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class AnalysisItemMinMaxValueMvcMockingService : IAnalysisItemMinMaxValueMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AnalysisItemWorker _worker;

        public AnalysisItemMinMaxValueMvcMockingService(IUnitOfWork unitOfWork, AnalysisItemWorker worker)
        {
            _unitOfWork = unitOfWork;
            _worker = worker;
        }

        public static List<AnalysisItemMinMaxValueViewModel> convertModelsLists(IEnumerable<AnalysisItemMinMaxValue> AnalysisItemMinMaxValue)
        {
            List<AnalysisItemMinMaxValueViewModel> AnalysisItemMinMaxValueViewModelList = new List<AnalysisItemMinMaxValueViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();

            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemMinMaxValueViewModelList = mapper.Map<IEnumerable<AnalysisItemMinMaxValue>, List<AnalysisItemMinMaxValueViewModel>>(AnalysisItemMinMaxValue);
            return AnalysisItemMinMaxValueViewModelList;
        }

        public IEnumerable<AnalysisItemMinMaxValueViewModel> GetAllAnalysisItemMinMaxValue(Guid analysisItemId)
        {
            try
            {
                List<AnalysisItemMinMaxValue> AnalysisItemMinMaxValues = _unitOfWork.AnalysisItemMinMaxValue.GetAllAnalysisItemMinMaxValue(analysisItemId).ToList();
                List<AnalysisItemMinMaxValueViewModel> AnalysisItemMinMaxValuess = convertModelsLists(AnalysisItemMinMaxValues);
                Indexing<AnalysisItemMinMaxValueViewModel> indexing = new Indexing<AnalysisItemMinMaxValueViewModel>();
                return indexing.AddIndexing(AnalysisItemMinMaxValuess);

            }
            catch (Exception e)
            {

                throw e;
            }

            
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisItemMinMaxValue/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public OperationStatus RemoveAnalysisItemMinMaxValue(Guid AnalysisItemMinMaxValueId)
        {
            try
            {
                AnalysisItemMinMaxValue AnalysisItemMinMaxValue = _unitOfWork.AnalysisItemMinMaxValue.Get(AnalysisItemMinMaxValueId);
                _unitOfWork.AnalysisItemMinMaxValue.Remove(AnalysisItemMinMaxValue);
                _unitOfWork.Complete();
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

        public OperationStatus RemoveAllWithAnalysisItemId(Guid AnalysisItemId)
        {
            try
            {
                IEnumerable<AnalysisItemMinMaxValue> AnalysisItemMinMaxValue = _unitOfWork.AnalysisItemMinMaxValue.Find(x => x.AnalysisItemId == AnalysisItemId);
                if (AnalysisItemMinMaxValue.Count() != 0)
                    _unitOfWork.AnalysisItemMinMaxValue.RemoveRange(AnalysisItemMinMaxValue);
                _unitOfWork.Complete();
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


        public AnalysisItemMinMaxValueViewModel GetAnalysisItemMinMaxValue(Guid AnalysisItemMinMaxValueId)
        {
            try
            {
                AnalysisItemMinMaxValue AnalysisItemMinMaxValue = _unitOfWork.AnalysisItemMinMaxValue.Get(AnalysisItemMinMaxValueId);
                return convertModels(AnalysisItemMinMaxValue);
            }
            catch { return null; }
        }
        public AnalysisItemMinMaxValueViewModel convertModels(AnalysisItemMinMaxValue AnalysisItemMinMaxValue)
        {
            AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValueView = new AnalysisItemMinMaxValueViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
                cfg.CreateMap<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemMinMaxValueView = mapper.Map<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>(AnalysisItemMinMaxValue);
            return AnalysisItemMinMaxValueView;
        }

        public Guid AddNewAnalysisItemMinMaxValue(AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue)
        {
            try
            {
                AnalysisItemMinMaxValue AnalysisItemMinMaxValueDto = Common.ConvertModels<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>.convertModels(AnalysisItemMinMaxValue);
                AnalysisItemMinMaxValue.Guid = Guid.NewGuid();
                _unitOfWork.AnalysisItemMinMaxValue.Add(AnalysisItemMinMaxValueDto);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return AnalysisItemMinMaxValue.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateAnalysisItemMinMaxValue(AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue)
        {
            try
            {
                AnalysisItemMinMaxValue AnalysisItemMinMaxValueDto = Common.ConvertModels<AnalysisItemMinMaxValue, AnalysisItemMinMaxValueViewModel>.convertModels(AnalysisItemMinMaxValue);
                _unitOfWork.AnalysisItemMinMaxValue.UpdateState(AnalysisItemMinMaxValueDto);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return AnalysisItemMinMaxValue.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
