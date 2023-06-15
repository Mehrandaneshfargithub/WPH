using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAnalysisRepository : IRepository<Analysis>
    {
        IEnumerable<Analysis> GetAllAnalysis(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo);
        IEnumerable<Analysis> GetAllAnalysis();
        IEnumerable<Analysis> GetAllAnalysisWithAnalysisItems(Guid clinicSectionId, int DestCurrencyId);
        IEnumerable<Analysis> GetAllAnalysisWithoutInGroupAnalysisAnalyses(Guid groupId, Guid clinicSectionId);
        List<Analysis> GetAllAnalysisByUserId(Guid userId);
        IEnumerable<Analysis> GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(Guid groupId);
        void RemoveAnalysis(Guid analysisId);
        IEnumerable<Analysis> GetAllAnalysisByClinicSectionId(Guid clinicSectionId);
        Task<List<Analysis>> GetAllAnalysisWithAnalysisItemAsync();
        List<Analysis> GetAllAnalysisWithAnalysisItem();
    }
}
