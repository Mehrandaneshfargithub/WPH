using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.AnalysisItem;
using WPH.Models.AnalysisResultMaster;
using WPH.Models.CustomDataModels.AnalysisResult;
using WPH.Models.CustomDataModels.AnalysisResultMaster;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisResultMasterMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        AnalysisResultMasterViewModel GetAnalysisResultMasterByInvoiceNum(Guid clinicSectionId, string invoiceNum);
        void UpdateAnalysisResultMaster(AnalysisResultMasterViewModel analysisResultMaster);
        void IncreasePrintNumber(Guid analysisResultMasterId);
        IEnumerable<AnalysisResultMasterViewModel> GetAllAnalysisResultMaster(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo);
        OperationStatus RemoveAnalysisResultMaster(Guid AnalysisResultMasterId);
        AnalysisResultMasterViewModel GetAnalysisResultMasterByIdForAnalysisResult(Guid AnalysisResultMasterId);
        AnalysisResultMasterViewModel GetAnalysisResultMasterForAnalysisResultReport(Guid AnalysisResultMasterId);
        IEnumerable<PastValuesViewModel> GetPastAnalysisResults(Guid analysisResultMasterId, Guid patientId);
        IEnumerable<AnalysisResultMasterGridViewModel> GetAllAnalysisResultMasterByUserId(Guid userId, int periodId, DateTime fromDate, DateTime toDate);
        IEnumerable<AnalysisResultMasterGridViewModel> GetAnalysisResultByPatientId(Guid patientId);
        IEnumerable<AnalysisResultMasterGridViewModel> GetAllPatientAnalysisResult(Guid patientId);
        List<AnalysisItemInChartViewModel> GetAnalysisItemForChart(Guid analysisResultMasterId);
        AnalysisResultMasterViewModel GetAnalysisResultMaster(Guid analysisResultMasterId);
        AnalysisResultMasterViewModel GetAnalysisResultMasterByReceptionId(Guid receptionId);
        void UpdateAnalysisResultMasterForServerNumByReceptionId(Guid receptionId, int v,DateTime? date);
        int GetReceptionServerNumber(Guid patientReceptionId);
    }
}
