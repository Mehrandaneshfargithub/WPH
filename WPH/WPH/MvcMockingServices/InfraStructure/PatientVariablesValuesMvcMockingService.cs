using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.Models.CustomDataModels.PatientVariableValue;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    
    public class PatientVariablesValuesMvcMockingService : IPatientVariablesValuesMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientVariablesValuesMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientVariablesValues/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.AccessLink = controllerName + "AccessModal?Id=";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public IEnumerable<PatientVariablesValueViewModel> GetAllPatientVariablesValueBasedOnPatientId(Guid? patientId)
        {
            try
            {
                List<PatientVariablesValue> allVariables = _unitOfWork.PatientVariablesValue.Find(x => x.PatientId == patientId).ToList();
                return convertModelsLists(allVariables);

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<PatientVariablesValueViewModel> GetAllPatientVariablesValueBasedOnVisitId(Guid VisitId)
        {
            try
            {
                List<PatientVariablesValue> allVariables = _unitOfWork.PatientVariablesValue.GetAllPatientVariablesValueBasedOnVisitId(VisitId);
                return convertModelsLists(allVariables);

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<PatientVariablesValueViewModel> GetAllPatientSpeceficVariable(Guid receptionId, string variableName)
        {
            try
            {
                IEnumerable<PatientVariablesValue> allVariables = _unitOfWork.PatientVariablesValue.GetAllPatientSpeceficVariable(receptionId, variableName);
                Indexing<PatientVariablesValueViewModel> indexing = new Indexing<PatientVariablesValueViewModel>();
                return indexing.AddIndexing(convertModelsLists(allVariables));

            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<PatientVariablesValueViewModel> GetAllReceptionVariable(Guid receptionId)
        {
            try
            {
                IEnumerable<PatientVariablesValue> allVariables = _unitOfWork.PatientVariablesValue.GetAllReceptionVariable(receptionId);
                Indexing<PatientVariablesValueViewModel> indexing = new Indexing<PatientVariablesValueViewModel>();
                return indexing.AddIndexing(convertModelsLists(allVariables));

            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdatePatientVariablesValue(PatientVariablesValueViewModel PatientVariablesValue)
        {
            try
            {
                PatientVariablesValue updatedPatientVariablesValue = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModels(PatientVariablesValue);
                PatientVariablesValue oldPatientVariablesValue = _unitOfWork.PatientVariablesValue.Get(updatedPatientVariablesValue.Guid);
                _unitOfWork.PatientVariablesValue.Detach(oldPatientVariablesValue);
                _unitOfWork.PatientVariablesValue.UpdateState(updatedPatientVariablesValue);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }


        public static List<PatientVariablesValueViewModel> convertModelsLists(IEnumerable<PatientVariablesValue> ress)
        {


            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<PatientVariablesValue, PatientVariablesValueViewModel>()
                .ForMember(a => a.ClinicSection, b => b.Ignore())
                .ForMember(a => a.Patient, b => b.Ignore())
                //.ForMember(a => a.PatientVariable, b => b.Ignore())
                .ForMember(a => a.Visit, b => b.Ignore())
                ;
                cfg.CreateMap<PatientVariable, PatientVariableViewModel>()
                .ForMember(a => a.ClinicSectionChoosenValues, b => b.Ignore())
                .ForMember(a => a.PatientVariablesValues, b => b.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<PatientVariablesValue>, List<PatientVariablesValueViewModel>>(ress);


        }


        public Guid AddPatientVariablesValue(PatientVariablesValueViewModel PatientVariablesValue)
        {
            try
            {
                PatientVariablesValue res = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModels(PatientVariablesValue);
                _unitOfWork.PatientVariablesValue.Add(res);
                _unitOfWork.Complete();
                return res.Guid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddPatientVariablesValueRange(IEnumerable<PatientVariablesValueViewModel> all)
        {
            try
            {
                IEnumerable<PatientVariablesValue> res = Common.ConvertModels<PatientVariablesValue, PatientVariablesValueViewModel>.convertModelsLists(all);
                
                _unitOfWork.PatientVariablesValue.AddRange(res);
                _unitOfWork.Complete();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientVariablesValueViewModel GetPatientVariablesValueBasedOnGuid(Guid Guid)
        {
            PatientVariablesValue patientVariable = _unitOfWork.PatientVariablesValue.Get(Guid);
            return Common.ConvertModels<PatientVariablesValueViewModel, PatientVariablesValue>.convertModels(patientVariable);
        }

        public void RemoveRange(IEnumerable<PatientVariablesValueViewModel> mustRemove)
        {

            try
            {
                List<PatientVariablesValue> all = new List<PatientVariablesValue>();
                foreach (PatientVariablesValueViewModel pv in mustRemove)
                {
                    all.Add(_unitOfWork.PatientVariablesValue.Get(pv.Guid));
                    //_unitOfWork.PatientVariablesValue.Remove(des);
                }
                
                _unitOfWork.PatientVariablesValue.RemoveRange(all);
                _unitOfWork.Complete();
            }
            catch (Exception e) { throw e; }

        }


        public OperationStatus Remove(Guid Guid)
        {
            try
            {
                PatientVariablesValue des = _unitOfWork.PatientVariablesValue.Get(Guid);
                _unitOfWork.PatientVariablesValue.Remove(des);
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

        public void UpdatePatientVariablesValueBasedOnGuid(Guid Guid, string Value)
        {
            _unitOfWork.PatientVariablesValue.UpdatePatientVariablesValueBasedOnGuid(Guid, Value);
        }

        
    }
}
