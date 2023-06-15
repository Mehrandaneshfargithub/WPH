using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAnalysisAnalysisItemRepository : IRepository<AnalysisAnalysisItem>
    {
        IEnumerable<AnalysisAnalysisItem> GetAllAnalysisAnalysisItem(Guid analysisId);
        void RemoveAnalysisAnalysisItem(Guid analysisItemId);
        void UpdatePriority(AnalysisAnalysisItem currentAnalysisItem);
        void RemoveAnalysisItemFromAnalysis(Guid id);
    }
}
