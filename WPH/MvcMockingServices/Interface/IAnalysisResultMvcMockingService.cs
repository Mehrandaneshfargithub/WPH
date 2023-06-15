using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisResult;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisResultMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        void AddAnalysisResultToPatientReception(IEnumerable<AnalysisResultViewModel> analysisResults);
        IEnumerable<AnalysisResultViewModel> GetAnalysisResultForReport(Guid patientReceptionId);
        Guid AddNewAnalysisResult(AnalysisResultViewModel analysisResult);
        Guid AddNewAnalysisResult(AnalysisResultViewModel AnalysisResult, Guid clinicSectionId, Guid userId);
        IEnumerable<PastValuesViewModel> GetPastAnalysisResults(Guid patientId, List<Guid> analysisItemId);
        Guid UpdateAnalysisResult(AnalysisResultViewModel AnalysisResult);
        OperationStatus RemoveAnalysisResult(Guid AnalysisResultId);
        AnalysisResultViewModel GetAnalysisResult(Guid AnalysisResultId);
        IEnumerable<AnalysisResultViewModel> GetAllAnalysisResult(Guid analysisResultMasterId);
    }
}
