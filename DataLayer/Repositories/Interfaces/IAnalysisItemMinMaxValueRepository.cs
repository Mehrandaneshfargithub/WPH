using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Infrastructure
{
    public interface IAnalysisItemMinMaxValueRepository : IRepository<AnalysisItemMinMaxValue>
    {
        IEnumerable<AnalysisItemMinMaxValue> GetAllAnalysisItemMinMaxValue(Guid analysisId);

    }
}