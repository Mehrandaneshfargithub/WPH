using System;
using System.Collections.Generic;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IGroupAnalysisAnalysesRepository : IRepository<GroupAnalysisAnalysis>
    {
        IEnumerable<GroupAnalysisAnalysis> GetAllGroupAnalysisAnalyses(Guid groupAnalysis);
        IEnumerable<GroupAnalysisAnalysis> GetAllGroupAnalysisAnalyses();
        void UpdatePriority(GroupAnalysisAnalysis currentGroupAnalysisAnalyses);

    }
}
