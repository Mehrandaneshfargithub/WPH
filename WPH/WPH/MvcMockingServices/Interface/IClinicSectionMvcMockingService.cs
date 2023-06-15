using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.Clinic;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Dashboard;

namespace WPH.MvcMockingServices.Interface
{
    public interface IClinicSectionMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        List<ClinicSectionSettingValueViewModel> GetAllClinicSectionSettingValues(Guid ClinicSectionId);
        List<ClinicSectionSettingViewModel> GetAllClinicSectionSettingsBasedOnSectionType(Guid clinicSectionId, int? sectionTypeId);
        OperationStatus RemoveClinicSection(Guid id);
        ClinicSectionViewModel GetClinicSectionById(Guid ClinicSectionId);
        IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsBasedOnClinicId(Guid clinicId);
        bool CheckRepeatedClinicNameAndCode(string name, bool NewOrUpdate, string systemCode, string nameHolder = "", string systemCodeHolder = "");
        List<ClinicSectionViewModel> GetClinicSectionsForUser(Guid userId, string type, Guid? parentId = null);
        Guid UpdateClinicSection(ClinicSectionViewModel clinicSection);
        IEnumerable<DashboardViewModel> GetAllDashboardDatas(Guid clinicSectionId);
        Guid AddNewClinicSection(ClinicSectionViewModel clinicSection);
        List<ClinicSectionViewModel> GetAllClinicSectionsBasedOnClinicSectionId(Guid clinicSectionId);
        void SaveClinicSectionSettings(IEnumerable<ClinicSectionSettingValueViewModel> cssvmList, string rootPath, List<ClinicSectionSettingBannerViewModel> banners, Guid clinicSectionId);
        IEnumerable<SectionViewModel> GetAllUserClinicSectionsJustNameAndGuid(Guid userId, string type);
        void SaveClinicSectionSettingValue(Guid clinicSectionId, string value, string name);
        string GetClinicSectionNameById(Guid clinicSectionId);
        IEnumerable<ClinicSectionSettingValueViewModel> GetClinicSectionSettingValueBySettingName(Guid clinicSectionId, params string[] settingsName);
        void UpdateClinicSectionName(Guid clinicSectionId, string name);
        Guid GetClinicSectionIdByName(string Name);
        List<ClinicSectionViewModel> GetAllClinicSectionWithType(Guid clinicId);
        IEnumerable<Guid> GetClinicSectionChilds(List<Guid> clinicSections, Guid? UserId = null);
        string GetBanner(Guid clinicSectionId, string settingName);
        bool ClinicSectionHasChild(Guid cliniSectionId, Guid userId);
        IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsChild(Guid cliniSectionId, Guid userId);
        IEnumerable<ClinicSectionViewModel> GetAllParentClinicSections();
        IEnumerable<ClinicSectionViewModel> GetAllMainClinicSectionsExceptOne(Guid cliniSectionId);
        IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsChildForTransferSource(Guid clinicSectionId, Guid userId);
        IEnumerable<ClinicSectionViewModel> GetAllUserClinicSections(Guid userId, Guid clinicSectionId);
        List<ClinicSectionViewModel> GetAllClinicSectionParentsForUser(Guid userId, Guid clinicId);
        List<ClinicSectionViewModel> GetAllClinicSectionsWithChilds(Guid userId, bool showChild);
        List<ClinicSectionViewModel> GetAllAccessedUserClinicSectionWithChilds(Guid userId, bool showChild);
    }
}
