using System;
using System.Collections.Generic;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAnalysisItemValuesRangeRepository : IRepository<AnalysisItemValuesRange>
    {
        IEnumerable<AnalysisItemValuesRange> GetAllAnalysisItemValuesRange(Guid AnalysisItemId);
    }
}
