using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.PatientReceptionReceived;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class PatientReceptionReceivedMvcMockingService : IPatientReceptionReceivedMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public PatientReceptionReceivedMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        //public static List<PatientReceptionReceivedViewModel> convertModelsLists(IEnumerable<PatientReceptionReceived> PatientReceptionReceived)
        //{
        //    List<PatientReceptionReceivedViewModel> PatientReceptionReceivedViewModelList = new List<PatientReceptionReceivedViewModel>();
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<User, UserInformationViewModel>();
        //        cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
        //        cfg.CreateMap<PatientReceptionReceived, PatientReceptionReceivedViewModel>();

        //    });
        //    IMapper mapper = config.CreateMapper();
        //    PatientReceptionReceivedViewModelList = mapper.Map<IEnumerable<PatientReceptionReceived>, List<PatientReceptionReceivedViewModel>>(PatientReceptionReceived);
        //    return PatientReceptionReceivedViewModelList;
        //}


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientReceptionReceived/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        //public OperationStatus RemovePatientReceptionReceived(Guid PatientReceptionReceivedId)
        //{
        //    try
        //    {
        //        _unitOfWork.PatientReceptionReceived.RemovePatientReceptionReceived(PatientReceptionReceivedId);
        //        return OperationStatus.SUCCESSFUL;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
        //        {
        //            return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
        //        }
        //        else
        //        {
        //            return OperationStatus.ERROR_SomeThingWentWrong;
        //        }
        //    }
        //}

        //public PatientReceptionReceivedViewModel convertModels(PatientReceptionReceived PatientReceptionReceived)
        //{
        //    PatientReceptionReceivedViewModel PatientReceptionReceivedView = new PatientReceptionReceivedViewModel();
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
        //        cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
        //        cfg.CreateMap<PatientReceptionReceived, PatientReceptionReceivedViewModel>();

        //    });
        //    IMapper mapper = config.CreateMapper();
        //    PatientReceptionReceivedView = mapper.Map<PatientReceptionReceived, PatientReceptionReceivedViewModel>(PatientReceptionReceived);
        //    return PatientReceptionReceivedView;
        //}

        public Guid AddNewPatientReceptionReceived(PatientReceptionReceivedViewModel PatientReceptionReceived)
        {
            try
            {
                //PatientReceptionReceived PatientReceptionReceivedDto = Common.ConvertModels<PatientReceptionReceived, PatientReceptionReceivedViewModel>.convertModels(PatientReceptionReceived);

                //PatientReceptionReceivedDto.Guid = Guid.NewGuid();
                //_unitOfWork.PatientReceptionReceived.Add(PatientReceptionReceivedDto);
                //_unitOfWork.Complete();
                //return PatientReceptionReceivedDto.Guid;
                throw new Exception("AddNewPatientReceptionReceived");
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
