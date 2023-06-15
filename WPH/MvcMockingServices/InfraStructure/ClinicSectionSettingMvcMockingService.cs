using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.AdminClinicSectionSetting;
using WPH.Helper;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ClinicSectionSettingMvcMockingService : IClinicSectionSettingMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicSectionSettingMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();

        }

        public OperationStatus RemoveClinicSetionSetting(int id)
        {
            try
            {
                var setting = _unitOfWork.ClinicSectionSettings.GetClinicSectionSettingWithValue(id);
                _unitOfWork.ClinicSectionSettingValues.RemoveRange(setting.ClinicSectionSettingValues);

                _unitOfWork.ClinicSectionSettings.Remove(setting);

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

        public string AddNewClinicSetionSetting(AdminClinicSectionSettingViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Sname.Trim()) || viewModel.SectionTypeId == null)
                return "DataNotValid";

            var exist = _unitOfWork.ClinicSectionSettings.GetSingle(p => p.Sname == viewModel.Sname && p.SectionTypeId == viewModel.SectionTypeId);
            if (exist != null)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<ClinicSectionSetting, AdminClinicSectionSettingViewModel>.convertModels(viewModel);

            _unitOfWork.ClinicSectionSettings.Add(result);
            _unitOfWork.Complete();

            return "";
        }

        public string UpdateClinicSectionSetting(AdminClinicSectionSettingViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Sname.Trim()) || viewModel.SectionTypeId == null)
                return "DataNotValid";

            var exist = _unitOfWork.ClinicSectionSettings.GetSingle(p => p.Sname == viewModel.Sname && p.SectionTypeId == viewModel.SectionTypeId && p.Id != viewModel.Id);
            if (exist != null)
                return "ValueIsRepeated";

            var result = Common.ConvertModels<ClinicSectionSetting, AdminClinicSectionSettingViewModel>.convertModels(viewModel);

            _unitOfWork.ClinicSectionSettings.UpdateState(result);
            _unitOfWork.Complete();

            return "";
        }

        public List<AdminClinicSectionSettingViewModel> GetAllClinicSectionSettings()
        {
            var setting = _unitOfWork.ClinicSectionSettings.GetAllClinicSectionSettings();

            var result = Common.ConvertModels<AdminClinicSectionSettingViewModel, ClinicSectionSetting>.convertModelsLists(setting);

            Indexing<AdminClinicSectionSettingViewModel> indexing = new Indexing<AdminClinicSectionSettingViewModel>();
            return indexing.AddIndexing(result);
        }

        public AdminClinicSectionSettingViewModel GetClinicSectionSetting(int id)
        {
            var setting = _unitOfWork.ClinicSectionSettings.GetSingle(p => p.Id == id);

            var result = Common.ConvertModels<AdminClinicSectionSettingViewModel, ClinicSectionSetting>.convertModels(setting);

            return result;
        }

        public int GetSettingIdByName(string settingName, int sectionTypeId)
        {
            return _unitOfWork.ClinicSectionSettings.GetSingle(a=>a.SectionTypeId == sectionTypeId && a.Sname == settingName).Id;
        }
    }
}
