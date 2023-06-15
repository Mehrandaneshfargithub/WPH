using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Ambulance;
using WPH.Models.Hospital;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{


    public class AmbulanceMvcMockingService : IAmbulanceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmbulanceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationStatus RemoveAmbulance(Guid Ambulanceid)
        {
            try
            {
                Ambulance Hos = _unitOfWork.Ambulances.Get(Ambulanceid);
                _unitOfWork.Ambulances.Remove(Hos);
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
            string controllerName = "/Ambulance/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewAmbulance(AmbulanceViewModel ambulance)
        {
            if (string.IsNullOrWhiteSpace(ambulance.HospitalName) || string.IsNullOrWhiteSpace(ambulance.Name))
                return "DataNotValid";

            var hospitalId = _unitOfWork.Hospitals.GetHospitalByName(ambulance.HospitalName)?.Guid;
            ambulance.HospitalId = hospitalId;
            Ambulance Ambulance1 = Common.ConvertModels<Ambulance, AmbulanceViewModel>.convertModels(ambulance);

            if (hospitalId == null || hospitalId == Guid.Empty)
            {
                var hospital = new Hospital
                {
                    Name = ambulance.HospitalName,
                };

                Ambulance1.Hospital = hospital;
            }

            _unitOfWork.Ambulances.Add(Ambulance1);
            _unitOfWork.Complete();
            return Ambulance1.Guid.ToString();
        }


        public string UpdateAmbulance(AmbulanceViewModel ambulance)
        {
            if (string.IsNullOrWhiteSpace(ambulance.HospitalName) || string.IsNullOrWhiteSpace(ambulance.Name))
                return "DataNotValid";


            var hospitalId = _unitOfWork.Hospitals.GetHospitalByName(ambulance.HospitalName)?.Guid;
            ambulance.HospitalId = hospitalId;
            Ambulance Ambulance1 = Common.ConvertModels<Ambulance, AmbulanceViewModel>.convertModels(ambulance);

            if (hospitalId == null || hospitalId == Guid.Empty)
            {
                var hospital = new Hospital
                {
                    Name = ambulance.HospitalName,
                };

                _unitOfWork.Hospitals.Add(hospital);

                Ambulance1.Hospital = hospital;
            }

            _unitOfWork.Ambulances.UpdateState(Ambulance1);
            _unitOfWork.Complete();
            return Ambulance1.Guid.ToString();

        }

        public bool CheckRepeatedAmbulanceName(string name, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                Ambulance Ambulance = null;
                if (NewOrUpdate)
                {
                    Ambulance = _unitOfWork.Ambulances.GetSingle(x => x.Name.Trim() == name.Trim());
                }
                else
                {
                    Ambulance = _unitOfWork.Ambulances.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName);
                }
                if (Ambulance != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public IEnumerable<AmbulanceViewModel> GetAllAmbulances()
        {
            IEnumerable<Ambulance> hosp = _unitOfWork.Ambulances.GetAllAmbulance();
            List<AmbulanceViewModel> hospconvert = ConvertModelsLists(hosp).ToList();
            Indexing<AmbulanceViewModel> indexing = new Indexing<AmbulanceViewModel>();
            return indexing.AddIndexing(hospconvert);
        }
        public AmbulanceViewModel GetAmbulance(Guid AmbulanceId)
        {
            try
            {
                Ambulance Ambulancegu = _unitOfWork.Ambulances.GetAmbulance(AmbulanceId);
                return ConvertModel(Ambulancegu);
            }
            catch { return null; }
        }

        // Begin Convert 
        public AmbulanceViewModel ConvertModel(Ambulance Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ambulance, AmbulanceViewModel>();
                cfg.CreateMap<Hospital, HospitalViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Ambulance, AmbulanceViewModel>(Users);
        }
        public List<AmbulanceViewModel> ConvertModelsLists(IEnumerable<Ambulance> Ambulances)
        {
            List<AmbulanceViewModel> AmbulanceDtoList = new List<AmbulanceViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ambulance, AmbulanceViewModel>();
                cfg.CreateMap<Hospital, HospitalViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            AmbulanceDtoList = mapper.Map<IEnumerable<Ambulance>, List<AmbulanceViewModel>>(Ambulances);
            return AmbulanceDtoList;
        }
        // End Convert
    }
}
