using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAnalysisResultRepository : IRepository<AnalysisResult>
    {
        void AddNewRange(IEnumerable<AnalysisResult> analysisResult);
        IEnumerable<AnalysisResult> GetAnalysisResultForReport(Guid patientReceptionId);
        IEnumerable<AnalysisResult> GetPastAnalysisResults(Guid patientId, List<Guid> analysisItemId);
        List<AnalysisResult> GetAnalysisResultForAnalysisResultReport(Guid analysisResultMasterId);
        IEnumerable<AnalysisResult> GetAnalysisResultHistoryForChart(Guid patientId, DateTime invoiceDate, List<Guid?> analysisItems);
    }
}
