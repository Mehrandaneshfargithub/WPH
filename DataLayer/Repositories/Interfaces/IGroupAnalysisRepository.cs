using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IGroupAnalysisRepository : IRepository<GroupAnalysis>
    {
        IEnumerable<GroupAnalysis> GetAllGroupAnalysis(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo);
        IEnumerable<GroupAnalysis> GetAllGroupAnalysis(Guid clinicSectionId);
        IEnumerable<GroupAnalysis> GetAllGroupAnalysis();
        void UpdatePriority(GroupAnalysis currentGroupAnalysis);
        List<GroupAnalysis> GetAllGroupAnalysisWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId);
        Task<List<GroupAnalysis>> GetAllGroupAnalysisWithAnalysisAndAnalysisItemAsync();
        List<GroupAnalysis> GetAllGroupAnalysisWithAnalysisAndAnalysisItem();
    }
}
