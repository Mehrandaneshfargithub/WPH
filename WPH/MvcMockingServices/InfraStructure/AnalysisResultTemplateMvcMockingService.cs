using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.AnalysisResultTemplate;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class AnalysisResultTemplateMvcMockingService : IAnalysisResultTemplateMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalysisResultTemplateMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/AnalysisResultTemplate/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public bool CheckRepeatedAnalysisResultTemplateName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                AnalysisResultTemplate AnalysisResultTemplate = null;
                if (NewOrUpdate)
                {
                    AnalysisResultTemplate = _unitOfWork.AnalysisResultTemplates.GetSingle(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
                }
                else
                {
                    AnalysisResultTemplate = _unitOfWork.AnalysisResultTemplates.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
                }
                if (AnalysisResultTemplate != null)
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

        public Guid AddNewAnalysisResultTemplate(AnalysisResultTemplateViewModel newAnalysisResultTemplate)
        {
            try
            {

                AnalysisResultTemplate AnalysisResultTemplateDto = Common.ConvertModels<AnalysisResultTemplate, AnalysisResultTemplateViewModel>.convertModels(newAnalysisResultTemplate);
                AnalysisResultTemplateDto.Guid = Guid.NewGuid();
                _unitOfWork.AnalysisResultTemplates.Add(AnalysisResultTemplateDto);
                _unitOfWork.Complete();
                return AnalysisResultTemplateDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public Guid UpdateAnalysisResultTemplate(AnalysisResultTemplateViewModel AnalysisResultTemplate)
        {
            try
            {
                AnalysisResultTemplate sAnalysisResultTemplateDto = Common.ConvertModels<AnalysisResultTemplate, AnalysisResultTemplateViewModel>.convertModels(AnalysisResultTemplate);

                _unitOfWork.AnalysisResultTemplates.UpdateState(sAnalysisResultTemplateDto);
                _unitOfWork.Complete();
                return sAnalysisResultTemplateDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }

        public OperationStatus RemoveAnalysisResultTemplate(Guid AnalysisResultTemplateId)
        {
            try
            {
                AnalysisResultTemplate AnalysisResultTemplateDto = _unitOfWork.AnalysisResultTemplates.Get(AnalysisResultTemplateId);
                _unitOfWork.AnalysisResultTemplates.Remove(AnalysisResultTemplateDto);
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

        public AnalysisResultTemplateViewModel GetAnalysisResultTemplate(Guid AnalysisResultTemplateId)
        {
            try
            {
                AnalysisResultTemplate AnalysisResultTemplateDto = _unitOfWork.AnalysisResultTemplates.Get(AnalysisResultTemplateId);
                return Common.ConvertModels<AnalysisResultTemplateViewModel, AnalysisResultTemplate>.convertModels(AnalysisResultTemplateDto);
            }
            catch { return null; }
        }

        public IEnumerable<AnalysisResultTemplateViewModel> GetAllAnalysisResultTemplate(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<AnalysisResultTemplate> AnalysisResultTemplateDtos = _unitOfWork.AnalysisResultTemplates.GetAll();
                List<AnalysisResultTemplateViewModel> AnalysisResultTemplate = Common.ConvertModels<AnalysisResultTemplateViewModel, AnalysisResultTemplate>.convertModelsLists(AnalysisResultTemplateDtos);
                Indexing<AnalysisResultTemplateViewModel> indexing = new Indexing<AnalysisResultTemplateViewModel>();
                return indexing.AddIndexing(AnalysisResultTemplate);
            }
            catch (Exception e) { return null; }

        }

        public IEnumerable<AnalysisResultTemplateViewModel> GetAllAnalysisResultTemplateByUserId(Guid userId)
        {
            try
            {
                IEnumerable<AnalysisResultTemplate> AnalysisResultTemplateDtos = _unitOfWork.AnalysisResultTemplates.GetAllAnalysisResultTemplateByUserId(userId);
                List<AnalysisResultTemplateViewModel> AnalysisResultTemplate = Common.ConvertModels<AnalysisResultTemplateViewModel, AnalysisResultTemplate>.convertModelsLists(AnalysisResultTemplateDtos);
                Indexing<AnalysisResultTemplateViewModel> indexing = new Indexing<AnalysisResultTemplateViewModel>();
                return indexing.AddIndexing(AnalysisResultTemplate);
            }
            catch (Exception e) { return null; }

        }






    }
}
