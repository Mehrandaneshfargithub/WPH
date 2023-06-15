using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAnalysisResultMasterRepository : IRepository<AnalysisResultMaster>
    {
        
        IEnumerable<AnalysisResultMaster> GetAllAnalysisResultMaster(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo);

        AnalysisResultMaster GetAnalysisResultMasterByIdForAnalysisResult(Guid AnalysisResultMasterId);
        void UpdateAnalysisResultMaster(AnalysisResultMaster updatedAnalysisResultMaster);
        AnalysisResultMaster GetAnalysisResultMasterForAnalysisResultReport(Guid analysisResultMasterId);
        void IncreasePrintNumber(Guid analysisResultMasterId);
        IEnumerable<FN_GetPastAnalysisResult_Result> GetPastAnalysisResults(Guid analysisResultMasterId, Guid patientId);
        AnalysisResultMaster GetAnalysisResultMasterByInvoiceNum(Guid clinicSectionId, string invoiceNum);
        IEnumerable<AnalysisResultMasterGrid> GetAllAnalysisResultMasterByUserId(Guid userId, DateTime dateFrom, DateTime dateTo);
        IEnumerable<AnalysisResultMasterGrid> GetAnalysisResultByPatientId(Guid patientId);
        AnalysisResultMaster GetWithPatientAndItem(Guid analysisResultMasterId);
        void UpdateAnalysisResultMasterForServerNumByReceptionId(Guid receptionId, int serverNum, DateTime? date);
        int GetReceptionServerNumber(Guid patientReceptionId);
    }
}
