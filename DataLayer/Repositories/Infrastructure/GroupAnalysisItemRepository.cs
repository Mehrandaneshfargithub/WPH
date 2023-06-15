using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class GroupAnalysisItemRepository : Repository<GroupAnalysisItem>, IGroupAnalysisItemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public GroupAnalysisItemRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<GroupAnalysisItem> GetAllGroupAnalysisItem(Guid groupAnalysisId)
        {
            return Context.GroupAnalysisItems
                .AsNoTracking()
                //.Include(x => x.GroupAnalysis)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.AnalysisItem).ThenInclude(x=>x.ValueType)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.AmountCurrency)
                .Where(x=>x.GroupAnalysisId==groupAnalysisId).OrderBy(x => x.Priority);

        }

        public IEnumerable<GroupAnalysisItem> GetAllGroupAnalysisItem()
        {
            return Context.GroupAnalysisItems
                .AsNoTracking()
                .Include(x => x.GroupAnalysis)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.ValueType)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.AmountCurrency)
                .OrderBy(x => x.Priority);
        }

        public void UpdatePriority(GroupAnalysisItem currentGroupAnalysisItem)
        {
            Context.Entry(currentGroupAnalysisItem).State = EntityState.Modified;
            Context.Entry(currentGroupAnalysisItem).Property(x => x.Priority).IsModified = true;
        }
    }
}
