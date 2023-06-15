using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ClinicSectionChoosenValueMvcMockingService : IClinicSectionChoosenValueMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _dIUnit;

        public ClinicSectionChoosenValueMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            
        }





        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ClinicSectionChoosenValues/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.AccessLink = controllerName + "AccessModal?Id=";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public void RemoveClinicSectionChoosenValue(Guid ClinicSectionChoosenValueId)
        {
            ClinicSectionChoosenValue ClinicSectionChoosenValue = _unitOfWork.ClinicSectionChoosenValue.Get(ClinicSectionChoosenValueId);
            _unitOfWork.ClinicSectionChoosenValue.Remove(ClinicSectionChoosenValue);
            _unitOfWork.Complete();
        }

        public ClinicSectionChoosenValueViewModel GetClinicSectionChoosenValueById(Guid id)
        {
            ClinicSectionChoosenValue ClinicSectionChoosenDto = _unitOfWork.ClinicSectionChoosenValue.Get(id);

            return Common.ConvertModels<ClinicSectionChoosenValueViewModel, ClinicSectionChoosenValue>.convertModels(ClinicSectionChoosenDto);
        }




        public IEnumerable<ClinicSectionChoosenValueViewModel> GetAllClinicSectionChoosenValues(Guid clinicSectionId)
        {

            IEnumerable<ClinicSectionChoosenValue> ClinicSectionChoosenDto = _unitOfWork.ClinicSectionChoosenValue.GetAllClinicSectionChoosenValue(clinicSectionId);

            return convertModelsLists(ClinicSectionChoosenDto);
        }


        public IEnumerable<ClinicSectionChoosenValueViewModel> GetAllFillBySecretaryClinicSectionChoosenValues(Guid clinicSectionId)
        {
            IEnumerable<ClinicSectionChoosenValue> ClinicSectionChoosenDto = _unitOfWork.ClinicSectionChoosenValue.GetAllFillBySecretaryClinicSectionChoosenValues(clinicSectionId);

            return convertModelsLists(ClinicSectionChoosenDto);
        }

        public IEnumerable<ClinicSectionChoosenValueViewModel> GetAllNumericClinicSectionChoosenValues(Guid clinicSectionId)
        {
            IEnumerable<ClinicSectionChoosenValue> ClinicSectionChoosenDto = _unitOfWork.ClinicSectionChoosenValue.GetAllNumericClinicSectionChoosenValues(clinicSectionId);

            return convertModelsLists(ClinicSectionChoosenDto);
        }

        public void AddClinicSectionChoosenValues(IEnumerable<ClinicSectionChoosenValueViewModel> clinicSectionChoosenValueDto, Guid clinicSectionId)
        {
            try
            {
                List<ClinicSectionChoosenValue> ClinicSectionChoosenValue = Common.ConvertModels<ClinicSectionChoosenValue, ClinicSectionChoosenValueViewModel>.convertModelsLists(clinicSectionChoosenValueDto);

                IEnumerable<ClinicSectionChoosenValue> AllClinicSectionChoosenValue = _unitOfWork.ClinicSectionChoosenValue.Find(x => x.ClinicSectionId == clinicSectionId);

                List<ClinicSectionChoosenValue> newClinicSectionChoosenValue = ClinicSectionChoosenValue.ToList();
                List<ClinicSectionChoosenValue> MostRemoved = new List<ClinicSectionChoosenValue>();

                foreach (var cli in AllClinicSectionChoosenValue)
                {
                    if (newClinicSectionChoosenValue.Exists(x => x.PatientVariableId == cli.PatientVariableId))
                    {
                        newClinicSectionChoosenValue.RemoveAll(x => x.PatientVariableId == cli.PatientVariableId);
                    }
                    else
                    {

                        MostRemoved.Add(cli);
                    }
                }

                IEnumerable<BaseInfoGeneralViewModel> VariableDisplay = _dIUnit.baseInfo.GetAllBaseInfoGenerals("VariableDisplay");
                int display = VariableDisplay.SingleOrDefault(x => x.Name == "ShowInVisit").Id;
                IEnumerable<BaseInfoGeneralViewModel> VariableStatus = _dIUnit.baseInfo.GetAllBaseInfoGenerals("VariableStatus");
                int status = VariableStatus.SingleOrDefault(x => x.Name == "VariableValueIsVariable").Id;


                foreach (var cs in newClinicSectionChoosenValue)
                {
                    cs.ClinicSectionId = clinicSectionId;
                    cs.Guid = Guid.NewGuid();
                    cs.FillBySecretary = false;
                    //cs.Priority = prority;
                    cs.VariableDisplayId = display;
                    cs.VariableStatusId = status;
                    //prority++;
                }

                _unitOfWork.ClinicSectionChoosenValue.AddRange(newClinicSectionChoosenValue);
                _unitOfWork.ClinicSectionChoosenValue.RemoveRange(MostRemoved);

                IEnumerable<ClinicSectionChoosenValue> mostUpdatedClinicSectionChoosenValue = _unitOfWork.ClinicSectionChoosenValue.Find(x => x.ClinicSectionId == clinicSectionId);
                int i = 1;
                foreach (var choseenValue in mostUpdatedClinicSectionChoosenValue)
                {

                    _unitOfWork.ClinicSectionChoosenValue.Detach(choseenValue);
                    choseenValue.Priority = i;
                    _unitOfWork.ClinicSectionChoosenValue.UpdateState(choseenValue);
                    i++;
                }

                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateClinicSectionChoosenValue(ClinicSectionChoosenValueViewModel ClinicSectionChoosenValue)
        {
            try
            {
                ClinicSectionChoosenValue.PatientVariable = null;
                ClinicSectionChoosenValue updatedClinicSectionChoosenValue = Common.ConvertModels<ClinicSectionChoosenValue, ClinicSectionChoosenValueViewModel>.convertModels(ClinicSectionChoosenValue);
                ClinicSectionChoosenValue oldClinicSectionChoosenValue = _unitOfWork.ClinicSectionChoosenValue.Get(updatedClinicSectionChoosenValue.Guid);
                _unitOfWork.ClinicSectionChoosenValue.Detach(oldClinicSectionChoosenValue);
                _unitOfWork.ClinicSectionChoosenValue.UpdateState(updatedClinicSectionChoosenValue);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }


        public void RemoveClinicSectionChoosenValueRange(List<ClinicSectionChoosenValueViewModel> mostRemoved)
        {
            //ClinicSectionChoosenValue.PatientVariable = null;
            List<ClinicSectionChoosenValue> ClinicSectionChoosenValueDto = convertModelsLists(mostRemoved);
            foreach (var cl in ClinicSectionChoosenValueDto)
            {
                _unitOfWork.ClinicSectionChoosenValue.Remove(cl);
            }

            _unitOfWork.Complete();
            IEnumerable<ClinicSectionChoosenValue> mostUpdatedClinicSectionChoosenValue = _unitOfWork.ClinicSectionChoosenValue.Find(x => x.ClinicSectionId == ClinicSectionChoosenValueDto.FirstOrDefault().ClinicSectionId);
            int i = 1;
            foreach (var choseenValue in mostUpdatedClinicSectionChoosenValue.OrderBy(x => x.PatientVariable.VariableName))
            {

                _unitOfWork.ClinicSectionChoosenValue.Detach(choseenValue);
                choseenValue.Priority = i;
                _unitOfWork.ClinicSectionChoosenValue.UpdateState(choseenValue);
                i++;
            }
            _unitOfWork.Complete();
        }


        public void AddClinicSectionChoosenValue(ClinicSectionChoosenValueViewModel today)
        {
            try
            {
                ClinicSectionChoosenValue reseDto = Common.ConvertModels<ClinicSectionChoosenValue, ClinicSectionChoosenValueViewModel>.convertModels(today);
                reseDto.Guid = Guid.NewGuid();
                _unitOfWork.ClinicSectionChoosenValue.Add(reseDto);
                _unitOfWork.Complete();
            }
            catch (Exception ex) { throw ex; }
        }


        public static List<ClinicSectionChoosenValueViewModel> convertModelsLists(IEnumerable<ClinicSectionChoosenValue> meds)
        {

            List<ClinicSectionChoosenValueViewModel> MedicineDtoList = new List<ClinicSectionChoosenValueViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClinicSectionChoosenValue, ClinicSectionChoosenValueViewModel>()
                .ForMember(x => x.VariableDisplayName, b => b.MapFrom(c => c.VariableDisplay.Name))
                .ForMember(x => x.VariableStatusName, b => b.MapFrom(c => c.VariableStatus.Name))

                ;
                cfg.CreateMap<PatientVariable, PatientVariableViewModel>()
                ;
            });

            IMapper mapper = config.CreateMapper();
            MedicineDtoList = mapper.Map<IEnumerable<ClinicSectionChoosenValue>, List<ClinicSectionChoosenValueViewModel>>(meds);

            return MedicineDtoList;
        }



        public static List<ClinicSectionChoosenValue> convertModelsLists(IEnumerable<ClinicSectionChoosenValueViewModel> meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClinicSectionChoosenValueViewModel, ClinicSectionChoosenValue>()


                ;
                cfg.CreateMap<PatientVariableViewModel, PatientVariable>()
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ClinicSectionChoosenValueViewModel>, List<ClinicSectionChoosenValue>>(meds);

        }


    }
}
