using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientReceptionAnalysisRepository : IRepository<PatientReceptionAnalysis>
    {
        List<PatientReceptionAnalysis> GetPatientReceptionAnalysisByReceptionId(Guid receptionId);
        List<PatientReceptionAnalysis> GetAllPatientReceptionAnalysis(Guid receptionId);
        List<PatientReceptionAnalysis> GetPatientReceptionAnalysisByReceptionIdJustIds(Guid receptionId);
        IEnumerable<PatientReceptionAnalysis> GetAllPatientReceptionAnalysisByClinicSectionIds(IEnumerable<Guid> allReceptionIds, DateTime dateFrom, DateTime dateTo);
        IEnumerable<PieChartModel> GetMostUsedAnalysis(Guid userId);
    }
}
