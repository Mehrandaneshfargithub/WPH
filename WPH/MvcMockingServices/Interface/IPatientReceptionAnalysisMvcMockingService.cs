using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.PatientReceptionAnalysis;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientReceptionAnalysisMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        Guid AddNewPatientReceptionAnalysis(PatientReceptionAnalysisViewModel newPatientReceptionAnalysis, Guid clinicSectionId);
        Guid UpdatePatientReceptionAnalysis(PatientReceptionAnalysisViewModel PatientReceptionAnalysis);
        OperationStatus RemovePatientReceptionAnalysis(Guid PatientReceptionAnalysisId);
        List<PatientReceptionAnalysisViewModel> GetPatientReceptionAnalysisByReceptionId(Guid receptionId);
        IEnumerable<PatientReceptionAnalysisViewModel> GetAllPatientReceptionAnalysis(Guid receptionId);
        PatientReceptionAnalysisReportViewModel GetPatientAnalysisReportByReceptionId(Guid receptionId);
        PieChartViewModel GetMostUsedAnalysis(Guid userId);
    }
}
