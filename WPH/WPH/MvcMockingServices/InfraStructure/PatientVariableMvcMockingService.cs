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
    

    public class PatientVariableMvcMockingService : IPatientVariableMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientVariableMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientVariables/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.AccessLink = controllerName + "AccessModal?Id=";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public bool CheckRepeatedPatientVariableName(Guid userId, string name, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                PatientVariable item = null;
                if (NewOrUpdate)
                {
                    item = _unitOfWork.PatientVariable.GetSingle(x => x.VariableName.Trim() == name.Trim() && x.DoctorId == userId);
                }
                else
                {
                    item = _unitOfWork.PatientVariable.GetSingle(x => x.VariableName.Trim() == name.Trim() && x.VariableName.Trim() != oldName && x.DoctorId == userId);
                }
                if (item != null)
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


        public IEnumerable<PatientVariableViewModel> GetAllPatientVariables(Guid DoctorId)
        {

            IEnumerable<PatientVariable> AllPatientVariableDto = _unitOfWork.PatientVariable.GetAllPatientVariables(DoctorId);
            List<PatientVariableViewModel> AllPatientVariable = Common.ConvertModels<PatientVariableViewModel, PatientVariable>.convertModelsLists(AllPatientVariableDto);
            Indexing<PatientVariableViewModel> indexing = new Indexing<PatientVariableViewModel>();
            return indexing.AddIndexing(AllPatientVariable);

        }

        public IEnumerable<PatientVariableViewModel> GetAllVariablesForPatient(Guid doctorId, Guid patientId, Guid receptionId, string DisplayType)
        {
            IEnumerable<PatientVariable> AllPatientVariableDto = _unitOfWork.PatientVariable.GetAllVariablesForPatient(doctorId,patientId, receptionId, DisplayType);
            return ConvertModelsList(AllPatientVariableDto);
        }

        public string UpdatePatientVariable(PatientVariableViewModel PatientVariable)
        {

            if (string.IsNullOrWhiteSpace(PatientVariable.VariableName))
                return "DataNotValid";

            PatientVariable item1 = Common.ConvertModels<PatientVariable, PatientVariableViewModel>.convertModels(PatientVariable);

            _unitOfWork.PatientVariable.UpdateState(item1);
            _unitOfWork.Complete();
            return item1.Id.ToString();
        }


        public string AddPatientVariable(PatientVariableViewModel PatientVariable)
        {

            if (string.IsNullOrWhiteSpace(PatientVariable.VariableName))
                return "DataNotValid";

            PatientVariable item1 = Common.ConvertModels<PatientVariable, PatientVariableViewModel>.convertModels(PatientVariable);

            _unitOfWork.PatientVariable.Add(item1);
            _unitOfWork.Complete();
            return item1.Id.ToString();
        }

        public int? GetPatientVariableIdByName(string variableName)
        {

            try
            {
                return _unitOfWork.PatientVariable.GetSingle(x => x.VariableName == variableName).Id;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientVariableViewModel GetPatientVariableById(int id)
        {
            PatientVariable AllPatientVariableDto = _unitOfWork.PatientVariable.GetSingle(a=>a.Id == id);
            PatientVariableViewModel AllPatientVariable = Common.ConvertModels<PatientVariableViewModel, PatientVariable>.convertModels(AllPatientVariableDto);
            return AllPatientVariable;
        }

        public OperationStatus RemovePatientVariable(int id)
        {
            try
            {
                PatientVariable Hos = _unitOfWork.PatientVariable.GetSingle(a=>a.Id == id);
                _unitOfWork.PatientVariable.Remove(Hos);
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

        public List<PatientVariableViewModel> ConvertModelsList(IEnumerable<PatientVariable> AnalysisItem)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientVariable, PatientVariableViewModel>();
                cfg.CreateMap<PatientVariablesValue, PatientVariablesValueViewModel>();
                
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PatientVariable>, List<PatientVariableViewModel>>(AnalysisItem);
        }

        public void InsertOrUpdatePatientVariableValue(int variableId, Guid receptionId, Guid PatientId, string Value, string status)
        {
            PatientVariablesValue pv = new PatientVariablesValue();

            if (status.ToLower() == "constant")
            {
                pv = _unitOfWork.PatientVariablesValue.GetSingle(a => a.PatientVariableId == variableId && a.PatientId == PatientId);

                if (pv == null)
                {
                    _unitOfWork.PatientVariablesValue.Add(new PatientVariablesValue
                    {
                        PatientId = PatientId,
                        PatientVariableId = variableId,
                        Value = Value,
                        VariableInsertedDate = DateTime.Now
                    });
                }
                else
                {
                    pv.VariableInsertedDate = DateTime.Now;
                    pv.PatientId = PatientId;
                    pv.Value = Value;
                    _unitOfWork.PatientVariablesValue.UpdateState(pv);
                }
                _unitOfWork.Complete();
            }
            else
            {
                pv = _unitOfWork.PatientVariablesValue.GetSingle(a => a.PatientVariableId == variableId && a.ReceptionId == receptionId);

                if (pv == null)
                {
                    _unitOfWork.PatientVariablesValue.Add(new PatientVariablesValue
                    {
                        PatientId = PatientId,
                        ReceptionId = receptionId,
                        PatientVariableId = variableId,
                        Value = Value,
                        VariableInsertedDate = DateTime.Now
                    });
                }
                else
                {
                    pv.VariableInsertedDate = DateTime.Now;
                    pv.PatientId = PatientId;
                    pv.ReceptionId = receptionId;
                    pv.Value = Value;
                    _unitOfWork.PatientVariablesValue.UpdateState(pv);
                }
                _unitOfWork.Complete();

            }
            
        }

        
    }
}
