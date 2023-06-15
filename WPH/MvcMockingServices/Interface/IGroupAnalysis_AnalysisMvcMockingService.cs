using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;

namespace WPH.MvcMockingServices.Interface
{
    public interface IGroupAnalysis_AnalysisMvcMockingService
    {
        IEnumerable<GroupAnalysis_AnalysisViewModel> GetAllGroupAnalysis_Analysis(Guid groupAnalysisId);
        IEnumerable<GroupAnalysis_AnalysisViewModel> GetAllGroupAnalysis_Analysis();
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItemForSpecificDate(Guid clinicSectionId, Guid GroupAnalysisItemTypeId, int periodId, DateTime fromDate, DateTime toDate);
        void SwapPriority(Guid Id, Guid GroupAnalysisId, string type);
        OperationStatus Remove(Guid GroupAnalysis_AnalysisId);
        OperationStatus RemoveGroupAnalysis_AnalysisWithGroupAnalysisId(Guid GroupAnalysisId);
        GroupAnalysis_AnalysisViewModel GetGroupAnalysis_Analysis(Guid GroupAnalysis_AnalysisId);
        Guid AddNewGroupAnalysis_Analysis(GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis, Guid groupAnalysisId);
        Guid UpdateGroupAnalysis_Analysis(GroupAnalysis_AnalysisViewModel groupAnalysis_Analysis);





    }
}
