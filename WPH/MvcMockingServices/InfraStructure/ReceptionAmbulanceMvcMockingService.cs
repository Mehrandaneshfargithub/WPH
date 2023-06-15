using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Hospital;
using WPH.Models.ReceptionAmbulance;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ReceptionAmbulanceMvcMockingService : IReceptionAmbulanceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceptionAmbulanceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveReceptionAmbulance(Guid ReceptionAmbulanceid)
        {
            try
            {
                ReceptionAmbulance Hos = _unitOfWork.ReceptionAmbulances.Get(ReceptionAmbulanceid);
                _unitOfWork.ReceptionAmbulances.Remove(Hos);
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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ReceptionAmbulance/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public Guid AddNewReceptionAmbulance(ReceptionAmbulanceViewModel ReceptionAmbulance)
        {
            try
            {
                ReceptionAmbulance ReceptionAmbulance1 = Common.ConvertModels<ReceptionAmbulance, ReceptionAmbulanceViewModel>.convertModels(ReceptionAmbulance);
                ReceptionAmbulance1.Guid = Guid.NewGuid();
                _unitOfWork.ReceptionAmbulances.Add(ReceptionAmbulance1);
                _unitOfWork.Complete();
                return ReceptionAmbulance1.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateReceptionAmbulance(ReceptionAmbulanceViewModel hosp)
        {
            try
            {
                ReceptionAmbulance ReceptionAmbulance2 = Common.ConvertModels<ReceptionAmbulance, ReceptionAmbulanceViewModel>.convertModels(hosp);
                _unitOfWork.ReceptionAmbulances.UpdateState(ReceptionAmbulance2);
                _unitOfWork.Complete();
                return ReceptionAmbulance2.Guid;
            }
            catch (Exception ex) { throw ex; }

        }
        //public ReceptionAmbulanceViewModel GetReceptionAmbulance(Guid ReceptionAmbulanceid)
        //{
        //    try
        //    {
        //        ReceptionAmbulance ReceptionAmbulance3 = _unitOfWork.ReceptionAmbulances.Get(ReceptionAmbulanceid);
        //        return ReceptionAmbulance3;
        //    }
        //    catch { return null; }
        //}

        public IEnumerable<ReceptionAmbulanceViewModel> GetAllReceptionAmbulances()
        {
            IEnumerable<ReceptionAmbulance> hosp = _unitOfWork.ReceptionAmbulances.GetAllReceptionAmbulance();
            List<ReceptionAmbulanceViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<ReceptionAmbulanceViewModel> indexing = new Indexing<ReceptionAmbulanceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }
        public ReceptionAmbulanceViewModel GetReceptionAmbulance(Guid ReceptionAmbulanceId)
        {
            try
            {
                ReceptionAmbulance ReceptionAmbulancegu = _unitOfWork.ReceptionAmbulances.Get(ReceptionAmbulanceId);
                return ConvertModel(ReceptionAmbulancegu);
            }
            catch { return null; }
        }

        public ReceptionAmbulanceViewModel GetReceptionAmbulanceByReceptionId(Guid receptionId)
        {
            ReceptionAmbulance ReceptionAmbulancegu = _unitOfWork.ReceptionAmbulances.GetReceptionAmbulanceWithHospital(receptionId);
            return ConvertModel(ReceptionAmbulancegu);
        }

        // Begin Convert 
        public ReceptionAmbulanceViewModel ConvertModel(ReceptionAmbulance reception)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionAmbulance, ReceptionAmbulanceViewModel>()
                .ForMember(a => a.FromHospital, b => b.Ignore())
                .ForMember(a => a.FromHospitalName, b => b.MapFrom(c => c.FromHospital.Name))
                .ForMember(a => a.ToHospital, b => b.Ignore())
                .ForMember(a => a.ToHospitalName, b => b.MapFrom(c => c.ToHospital.Name));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionAmbulance, ReceptionAmbulanceViewModel>(reception);
        }
        public List<ReceptionAmbulanceViewModel> ConvertModelsLists(IEnumerable<ReceptionAmbulance> ReceptionAmbulances)
        {
            List<ReceptionAmbulanceViewModel> ReceptionAmbulanceDtoList = new List<ReceptionAmbulanceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionAmbulance, ReceptionAmbulanceViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            ReceptionAmbulanceDtoList = mapper.Map<IEnumerable<ReceptionAmbulance>, List<ReceptionAmbulanceViewModel>>(ReceptionAmbulances);
            return ReceptionAmbulanceDtoList;
        }


        // End Convert
    }
}
