using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisItemValuesRangeRepository : Repository<AnalysisItemValuesRange>, IAnalysisItemValuesRangeRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisItemValuesRangeRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<AnalysisItemValuesRange> GetAllAnalysisItemValuesRange(Guid analysisItemId)
        {
            return Context.AnalysisItemValuesRanges.AsNoTracking().Where(x => x.AnalysisItemId == analysisItemId).Include(x => x.AnalysisItem);
        }

    }
}
