using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicSectionSettingValueRepository : Repository<ClinicSectionSettingValue>, IClinicSectionSettingValueRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicSectionSettingValueRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<ClinicSectionSettingValue> GetAllClinicSectionSettingValueWithSetting(Guid clinicSectionId)
        {
            return _context.ClinicSectionSettingValues.AsNoTracking().Where(x => x.ClinicSectionId == clinicSectionId).Include(x => x.Setting).ToList();
        }


        public IEnumerable<ClinicSectionSetting> GetAllClinicSectionSettings()
        {
            return _context.ClinicSectionSettings.AsNoTracking().Include(x => x.ClinicSectionSettingValues);
        }

        public IEnumerable<ClinicSectionSettingValue> GetSettingIdBySettingName(Guid clinicSectionId, params string[] settingsName)
        {
            return _context.ClinicSectionSettingValues.AsNoTracking()
                .Include(p => p.Setting)
                .Where(x => settingsName.Contains(x.Setting.Sname) && x.ClinicSectionId == clinicSectionId)
                .Select(p => new ClinicSectionSettingValue
                {
                    Guid = p.Guid,
                    Id = p.Id,
                    SettingId = p.SettingId,
                    Svalue = p.Svalue,
                    Setting = new ClinicSectionSetting
                    {
                        Sname = p.Setting.Sname
                    }
                });
        }

        public IEnumerable<ClinicSectionSetting> GetAllClinicSectionSettingsBasedOnSectionType(Guid clinicSectionId, int? sectionTypeId)
        {
            IEnumerable<ClinicSectionSetting> allset = _context.ClinicSectionSettings.AsNoTracking()
                                                                                     .Include(x => x.ClinicSectionSettingValues.Where(s => s.ClinicSectionId == clinicSectionId))
                                                                                     .Where(x => x.SectionTypeId == sectionTypeId);

            return allset;
        }

        public ClinicSectionSetting GetByNameAndSectionType(string settingName, int? sectionTypeId)
        {
            return _context.ClinicSectionSettings.AsNoTracking()
                                                 .Where(x => x.SectionTypeId == sectionTypeId)
                                                 .SingleOrDefault(p => p.Sname == settingName);
        }

        public int GetClinicSectionDefaultCurrencyDecimal(Guid clinicSectionId, int sectionTypeId)
        {
            return _context.FN_GetDecimalDefaultCurrency(clinicSectionId, sectionTypeId).FirstOrDefault().Amount ?? 0;
        }


        public ClinicSectionSetting GetSettingByName(string settingName, int sectionTypeId)
        {
            return _context.ClinicSectionSettings.AsNoTracking()
                .Include(p => p.ClinicSectionSettingValues)
                .SingleOrDefault(p => p.Sname == settingName && p.SectionTypeId == sectionTypeId);
        }
        public ClinicSectionSettingValue GetBySettingName(Guid clinicSectionId, string settingName)
        {
            return _context.ClinicSectionSettingValues.AsNoTracking()
                .Include(p => p.Setting)
                .SingleOrDefault(x => x.Setting.Sname == settingName && x.ClinicSectionId == clinicSectionId);
        }
    }
}
