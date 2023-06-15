using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicSectionSettingRepository : Repository<ClinicSectionSetting>, IClinicSectionSettingRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicSectionSettingRepository(WASContext context)
            : base(context)
        {
        }

        public ClinicSectionSetting GetClinicSectionSettingWithValue(int id)
        {
            return _context.ClinicSectionSettings.AsNoTracking()
                .Include(p => p.ClinicSectionSettingValues)
                .SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<ClinicSectionSetting> GetAllClinicSectionSettings()
        {
            return _context.ClinicSectionSettings.AsNoTracking()
                .Include(p => p.SectionType)
                .Select(p => new ClinicSectionSetting
                {
                    Id = p.Id,
                    Sname = p.Sname,
                    InputType = p.InputType,
                    SectionType = new BaseInfoGeneral
                    {
                        Name = p.SectionType.Name
                    }
                });
        }
    }
}
