using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories.Infrastructure
{
    public class SoftwareSettingRepository : Repository<SoftwareSetting>, ISoftwareSettingRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SoftwareSettingRepository(WASContext context)
            : base(context)
        {
        }

    }
}
