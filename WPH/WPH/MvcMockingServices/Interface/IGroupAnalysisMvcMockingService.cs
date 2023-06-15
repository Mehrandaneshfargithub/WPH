using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.GroupAnalysis;

namespace WPH.MvcMockingServices.Interface
{
    public interface IGroupAnalysisMvcMockingService
    {
        IEnumerable<GroupAnalysisViewModel> GetAllGroupAnalysis(Guid clinicSectionId, DateTime DateFrom, DateTime DateTo);
        IEnumerable<GroupAnalysisViewModel> GetAllGroupAnalysis();
        List<GroupAnalysisJustNameAndGuid> GetAllGroupAnalysisWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId);
        void GetModalsViewBags(dynamic viewBag);
        void UpdateGroupAnalysisButtonAndPriority(Guid clinicSectionId,IEnumerable<GroupAnalysisJustNameAndGuid> allGroup);
        void ActiveDeactiveAnalysis(Guid analysisId);
        
        void SwapPriority(Guid Id, Guid ClinicSectionId, string type);
        OperationStatus RemoveGroupAnalysis(Guid GroupAnalysisId);
        GroupAnalysisViewModel GetGroupAnalysis(Guid GroupAnalysisId);
        Guid AddNewGroupAnalysis(GroupAnalysisViewModel groupAnalysis);
        Guid UpdateGroupAnalysis(GroupAnalysisViewModel groupAnalysis);



    }
}
