using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class SubSystemSectionRepository : Repository<SubSystemSection>, ISubSystemSectionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SubSystemSectionRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<SubSystem> GetSubSystemBySectionTypeId(int sectionTypeId)
        {
            return Context.SubSystemSections.AsNoTracking().Include(x => x.SubSystem).Where(x => x.SectionTypeId == sectionTypeId).Select(x => x.SubSystem);
        }

        public IEnumerable<SubSystemSection> GetSubSystemSectionBySubSystemId(int subSystemId)
        {
            return _context.SubSystemSections.AsNoTracking()
                .Where(p => p.SubSystemId == subSystemId);
        }
    }
}
