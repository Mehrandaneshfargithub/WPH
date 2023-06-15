using System;
using System.Collections.Generic;
using WPH.Areas.Admin.Models.AdminClinicSectionSetting;
using WPH.Helper;

namespace WPH.MvcMockingServices.Interface
{
    public interface IClinicSectionSettingMvcMockingService
    {
        OperationStatus RemoveClinicSetionSetting(int id);
        string AddNewClinicSetionSetting(AdminClinicSectionSettingViewModel viewModel);
        string UpdateClinicSectionSetting(AdminClinicSectionSettingViewModel viewModel);
        List<AdminClinicSectionSettingViewModel> GetAllClinicSectionSettings();
        AdminClinicSectionSettingViewModel GetClinicSectionSetting(int id);
        int GetSettingIdByName(string settingName, int sectionTypeId);
    }
}
