using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IClinicSectionSettingValueRepository : IRepository<ClinicSectionSettingValue>
    {
        IEnumerable<ClinicSectionSettingValue> GetAllClinicSectionSettingValueWithSetting(Guid clinicSectionId);
        IEnumerable<ClinicSectionSetting> GetAllClinicSectionSettings();
        IEnumerable<ClinicSectionSettingValue> GetSettingIdBySettingName(Guid clinicSectionId, params string[] settingsName);
        IEnumerable<ClinicSectionSetting> GetAllClinicSectionSettingsBasedOnSectionType(Guid clinicSectionId, int? sectionTypeId);
        int GetClinicSectionDefaultCurrencyDecimal(Guid clinicSectionId, int sectionTypeId);
        ClinicSectionSetting GetSettingByName(string settingName, int sectionTypeId);
        ClinicSectionSettingValue GetBySettingName(Guid clinicSectionId, string settingName);
        ClinicSectionSetting GetByNameAndSectionType(string settingName, int? sectionTypeId);
    }
}
