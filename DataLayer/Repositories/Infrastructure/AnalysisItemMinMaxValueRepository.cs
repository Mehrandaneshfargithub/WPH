using DataLayer.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisItemMinMaxValueRepository : Repository<AnalysisItemMinMaxValue>, IAnalysisItemMinMaxValueRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisItemMinMaxValueRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<AnalysisItemMinMaxValue> GetAllAnalysisItemMinMaxValue(Guid analysisItemId)
        {
            return Context.AnalysisItemMinMaxValues.Include(x => x.AnalysisItem).AsNoTracking().Where(x => x.AnalysisItemId == analysisItemId);
        }
    }
}
