using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.PatientImage;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class PatientImageMvcMockingService : IPatientImageMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientImageMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<PatientImageViewModel> GetAllPatientImages(Guid patientId)
        {
            IEnumerable<PatientImage> medDtos = _unitOfWork.PatientImage.Find(x => x.PatientId == patientId);
            return ConvertModelsLists(medDtos);


        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/PatientImage/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public Guid AddPatientImage(PatientImageViewModel PatientImage)
        {
            PatientImage petDto = Common.ConvertModels<PatientImage, PatientImageViewModel>.convertModels(PatientImage);

            _unitOfWork.PatientImage.Add(petDto);
            _unitOfWork.Complete();
            return petDto.Guid;
        }


        public Guid UpdatePatientImage(PatientImageViewModel med)
        {
            try
            {
                PatientImage updatedPatientImage = Common.ConvertModels<PatientImage, PatientImageViewModel>.convertModels(med);
                PatientImage oldPatientImage = _unitOfWork.PatientImage.Get(updatedPatientImage.Guid);

                _unitOfWork.PatientImage.Detach(oldPatientImage);
                _unitOfWork.PatientImage.UpdateState(updatedPatientImage);
                _unitOfWork.Complete();
                return updatedPatientImage.Guid;
            }
            catch (Exception ex) { throw ex; }

        }


        public OperationStatus RemovePatientImage(Guid PatientImageId, string rootPath)
        {
            try
            {
                PatientImage des = _unitOfWork.PatientImage.Get(PatientImageId);
                _unitOfWork.PatientImage.Remove(des);
                FileAttachments fileAttachments = new();
                fileAttachments.DeleteFile(new PatientImageViewModel
                {
                    ImageAddress = Path.Combine(rootPath + des.ImageAddress),
                    ThumbNailAddress = Path.Combine(rootPath + des.ThumbNailAddress),
                });
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

        public PatientImageViewModel GetPatientImage(Guid PatientImageId)
        {
            try
            {
                PatientImage PatientImageDto = _unitOfWork.PatientImage.Get(PatientImageId);
                PatientImageViewModel med = Common.ConvertModels<PatientImageViewModel, PatientImage>.convertModels(PatientImageDto);
                return med;
            }
            catch { return null; }
        }


        public IEnumerable<PatientImageViewModel> GetAllVisitImages(Guid visitId)
        {
            IEnumerable<PatientImage> medDtos = _unitOfWork.PatientImage.Find(x => x.ReceptionId == visitId || x.VisitId == visitId);
            return ConvertModelsLists(medDtos);
        }

        public IEnumerable<PatientImageViewModel> GetMainAttachmentsByPatientId(Guid patientId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("MainAttachment", "AttachmentType");
            IEnumerable<PatientImage> patientDtos = _unitOfWork.PatientImage.GetAttachmentsByPatientAndTypeId(patientId, typeId);
            return ConvertModelsLists(patientDtos);
        }

        public IEnumerable<PatientImageViewModel> GetPoliceReportAttachmentsByPatientId(Guid patientId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("PoliceReport", "AttachmentType");
            IEnumerable<PatientImage> patientDtos = _unitOfWork.PatientImage.GetAttachmentsByPatientAndTypeId(patientId, typeId);
            return ConvertModelsLists(patientDtos);
        }

        public IEnumerable<PatientImageViewModel> GetMainAttachmentsByReceptionId(Guid receptionId)
        {
            //var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("MainAttachment", "AttachmentType");
            //IEnumerable<PatientImage> patientDtos = _unitOfWork.PatientImage.GetAttachmentsByReceptionAndTypeId(receptionId, typeId);
            //return ConvertModelsLists(patientDtos);
            var patientId = _unitOfWork.Receptions.Get(receptionId).PatientId;
            return GetMainAttachmentsByPatientId(patientId.Value);
        }

        public IEnumerable<PatientImageViewModel> GetOtherAttachmentsByReceptionId(Guid receptionId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("OtherAttachment", "AttachmentType");
            IEnumerable<PatientImage> patientDtos = _unitOfWork.PatientImage.GetAttachmentsByReceptionAndTypeId(receptionId, typeId);
            return ConvertModelsLists(patientDtos);
        }

        public IEnumerable<PatientImageViewModel> GetPoliceReportAttachmentsByReceptionId(Guid receptionId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("PoliceReport", "AttachmentType");
            IEnumerable<PatientImage> patientDtos = _unitOfWork.PatientImage.GetAttachmentsByReceptionAndTypeId(receptionId, typeId);
            return ConvertModelsLists(patientDtos);
        }

        public List<PatientImageViewModel> ConvertModelsLists(IEnumerable<PatientImage> Users)
        {
            List<PatientImageViewModel> UserDtoList = new List<PatientImageViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientImage, PatientImageViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            UserDtoList = mapper.Map<IEnumerable<PatientImage>, List<PatientImageViewModel>>(Users);
            return UserDtoList;
        }


        public PatientImageViewModel ConvertModel(PatientImage Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PatientImage, PatientImageViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PatientImage, PatientImageViewModel>(Users);
        }

    }
}
