using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices.Interface;
using WPH.WorkerServices;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class AnalysisItemValuesRangeMvcMockingService : IAnalysisItemValuesRangeMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AnalysisItemWorker _worker;


        public AnalysisItemValuesRangeMvcMockingService(IUnitOfWork unitOfWork, AnalysisItemWorker worker)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _worker = worker;
        }

        public void AddAnalysisOfAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId, Guid analysisItemId)
        {
            AnalysisItemValuesRange AnalysisItemValuesRange = _unitOfWork.AnalysisItemValuesRange.Get(AnalysisItemValuesRangeId);
            _unitOfWork.AnalysisItemValuesRange.Detach(AnalysisItemValuesRange);
            _unitOfWork.Complete();
            AnalysisItemValuesRange.AnalysisItemId = analysisItemId;
            _unitOfWork.AnalysisItemValuesRange.UpdateState(AnalysisItemValuesRange);
            _unitOfWork.Complete();
            _worker.RemoveCach();
        }
        public static List<AnalysisItemValuesRangeViewModel> convertModelsLists(IEnumerable<AnalysisItemValuesRange> AnalysisItemValuesRange)
        {
            List<AnalysisItemValuesRangeViewModel> AnalysisItemValuesRangeViewModelList = new List<AnalysisItemValuesRangeViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AnalysisItem, AnalysisItemViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();

            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemValuesRangeViewModelList = mapper.Map<IEnumerable<AnalysisItemValuesRange>, List<AnalysisItemValuesRangeViewModel>>(AnalysisItemValuesRange);
            return AnalysisItemValuesRangeViewModelList;
        }

        public IEnumerable<AnalysisItemValuesRangeViewModel> GetAllAnalysisItemValuesRange( Guid AnalysisItemValuesRangeId)
        {


            List<AnalysisItemValuesRange> AnalysisItemValuesRanges = _unitOfWork.AnalysisItemValuesRange.GetAllAnalysisItemValuesRange(AnalysisItemValuesRangeId).ToList();
            List<AnalysisItemValuesRangeViewModel> AnalysisItemValuesRangess = convertModelsLists(AnalysisItemValuesRanges).ToList();
            Indexing<AnalysisItemValuesRangeViewModel> indexing = new Indexing<AnalysisItemValuesRangeViewModel>();
            return indexing.AddIndexing(AnalysisItemValuesRangess);
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisItemValuesRange/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public OperationStatus RemoveAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId)
        {
            try
            {
                AnalysisItemValuesRange AnalysisItemValuesRange = _unitOfWork.AnalysisItemValuesRange.Get(AnalysisItemValuesRangeId);
                _unitOfWork.AnalysisItemValuesRange.Remove(AnalysisItemValuesRange);
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
                IEnumerable<AnalysisItemValuesRange> AnalysisItemValuesRange = _unitOfWork.AnalysisItemValuesRange.Find(x => x.AnalysisItemId == AnalysisItemId);
                if (AnalysisItemValuesRange.Count() != 0)
                    _unitOfWork.AnalysisItemValuesRange.RemoveRange(AnalysisItemValuesRange);
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


        public AnalysisItemValuesRangeViewModel GetAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId)
        {
            try
            {
                AnalysisItemValuesRange AnalysisItemValuesRange = _unitOfWork.AnalysisItemValuesRange.Get(AnalysisItemValuesRangeId);
                AnalysisItemValuesRangeViewModel AnalysisItemValuesRangeDto = convertModels(AnalysisItemValuesRange);
                return AnalysisItemValuesRangeDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AnalysisItemValuesRangeViewModel convertModels(AnalysisItemValuesRange AnalysisItemValuesRange)
        {
            AnalysisItemValuesRangeViewModel AnalysisItemValuesRangeView = new AnalysisItemValuesRangeViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
                cfg.CreateMap<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            AnalysisItemValuesRangeView = mapper.Map<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>(AnalysisItemValuesRange);
            return AnalysisItemValuesRangeView;
        }

        public Guid AddNewAnalysisItemValuesRange(AnalysisItemValuesRangeViewModel AnalysisItemValuesRange)
        {
            try
            {
                AnalysisItemValuesRange AnalysisItemValuesRangeNew = Common.ConvertModels<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>.convertModels(AnalysisItemValuesRange);
                AnalysisItemValuesRangeNew.Guid = Guid.NewGuid();
                _unitOfWork.AnalysisItemValuesRange.Add(AnalysisItemValuesRangeNew);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return AnalysisItemValuesRange.Guid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid UpdateAnalysisItemValuesRange(AnalysisItemValuesRangeViewModel AnalysisItemValuesRange)
        {
            try
            {
                AnalysisItemValuesRange AnalysisItemValuesRangeDto = Common.ConvertModels<AnalysisItemValuesRange, AnalysisItemValuesRangeViewModel>.convertModels(AnalysisItemValuesRange);
                _unitOfWork.AnalysisItemValuesRange.UpdateState(AnalysisItemValuesRangeDto);
                _unitOfWork.Complete();
                _worker.RemoveCach();
                return AnalysisItemValuesRange.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
