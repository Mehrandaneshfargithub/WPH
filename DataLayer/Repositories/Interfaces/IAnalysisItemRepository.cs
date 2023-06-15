using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public interface IAnalysisItemRepository : IRepository<AnalysisItem>
    {
        IEnumerable<AnalysisItem> GetAllAnalysisItem(Guid analysisId);
        void UpdatePriority(AnalysisItem currentAnalysisItem);
        AnalysisItem GetAnalysisItemBasedOnId(Guid analysisItemId);
        List<AnalysisItem> GetAllAnalysisItemsWithNameAndGuidOnly(Guid clinicSectionId, int destCurrencyId);
        List<AnalysisItem> GetAllAnalysisItemWithoutInAnalysisByUserId(Guid analysisId, Guid userId);
        List<AnalysisItem> GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(Guid groupId);
        
        void RemoveAnalysisItem(Guid AnalysisItemId);
        IEnumerable<AnalysisItem> GetAllAnalysisItemByClinicSectionId(Guid clinicSectionId);
        Task<List<AnalysisItem>> GetAllAnalysisItemAsync();
        AnalysisItem GetWithValueType(Guid analysisItemId);
    }
}