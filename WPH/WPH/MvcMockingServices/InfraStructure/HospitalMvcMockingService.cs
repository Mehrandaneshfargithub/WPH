using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Hospital;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class HospitalMvcMockingService : IHospitalMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HospitalMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveHospital(Guid Hospitalid)
        {
            try
            {
                Hospital Hos = _unitOfWork.Hospitals.Get(Hospitalid);
                _unitOfWork.Hospitals.Remove(Hos);
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
            string controllerName = "/Hospital/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public Guid AddNewHospital(HospitalViewModel hospital)
        {
            try
            {
                Hospital hospital1 = Common.ConvertModels<Hospital, HospitalViewModel>.convertModels(hospital);
                
                _unitOfWork.Hospitals.Add(hospital1);
                _unitOfWork.Complete();
                return hospital1.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateHospital(HospitalViewModel hosp)
        {
            try
            {
                Hospital hospital2 = Common.ConvertModels<Hospital, HospitalViewModel>.convertModels(hosp);
                _unitOfWork.Hospitals.UpdateState(hospital2);
                _unitOfWork.Complete();
                return hospital2.Guid;
            }
            catch (Exception ex) { throw ex; }

        }
        //public HospitalViewModel GetHospital(Guid Hospitalid)
        //{
        //    try
        //    {
        //        Hospital hospital3 = _unitOfWork.Hospitals.Get(Hospitalid);
        //        return hospital3;
        //    }
        //    catch { return null; }
        //}
        public bool CheckRepeatedHospitalName(string name, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                Hospital hospital = null;
                if (NewOrUpdate)
                {
                    hospital = _unitOfWork.Hospitals.GetSingle(x => x.Name.Trim() == name.Trim() );
                }
                else
                {
                    hospital = _unitOfWork.Hospitals.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName );
                }
                if (hospital != null)
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
        public IEnumerable<HospitalViewModel> GetAllHospitals()
        {
            IEnumerable<Hospital> hosp = _unitOfWork.Hospitals.GetAllHospital();
            List<HospitalViewModel> hospconvert = convertModelsLists(hosp).ToList();
            Indexing<HospitalViewModel> indexing = new Indexing<HospitalViewModel>();
            return indexing.AddIndexing(hospconvert);
        }
        public HospitalViewModel GetHospital(Guid HospitalId)
        {
            try
            {
                Hospital Hospitalgu = _unitOfWork.Hospitals.Get(HospitalId);
                return convertModel(Hospitalgu);
            }
            catch { return null; }
        }

        // Begin Convert 
        public HospitalViewModel convertModel(Hospital Users)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Hospital, HospitalViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Hospital, HospitalViewModel>(Users);
        }
        public List<HospitalViewModel> convertModelsLists(IEnumerable<Hospital> hospitals)
        {
            List<HospitalViewModel> hospitalDtoList = new List<HospitalViewModel>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Hospital, HospitalViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            hospitalDtoList = mapper.Map<IEnumerable<Hospital>, List<HospitalViewModel>>(hospitals);
            return hospitalDtoList;
        }
        // End Convert
    }
}
