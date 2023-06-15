using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisMvcMockingService
    {
        IEnumerable<AnalysisViewModel> GetAllAnalysis();
        IEnumerable<AnalysisWithAnalysisItemViewModel> GetAllAnalysisWithAnalysisItems(Guid clinicSectionId, int DestCurrencyId);
        void GetModalsViewBags(dynamic viewBag);
        void SwapPriority(Guid AnalysisId, string type);
        void ActiveDeactiveAnalysis(Guid analysisId);
        OperationStatus RemoveAnalysisItemFromAnalysis(Guid id);
        void AddNewAnalysisItemToAnalysis(Analysis_AnalysisItemViewModel analysis);
        IEnumerable<Analysis_AnalysisItemViewModel> GetAllAnalysisAnalysisItem(Guid analysisId);
        OperationStatus RemoveAnalysis(Guid AnalysisId);
        AnalysisViewModel GetAnalysis(Guid AnalysisId);
        Guid AddNewAnalysis(AnalysisViewModel analysis);
        Guid UpdateAnalysis(AnalysisViewModel analysis);
        void UpdateAnalysisButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisWithAnalysisItemViewModel> allAnalysis);
        IEnumerable<AnalysisViewModel> GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(Guid groupId);
        List<AnalysisViewModel> GetAllAnalysisByClinicSectionId(Guid clinicSectionId);
        IEnumerable<AnalysisViewModel> GetAllAnalysisAndAnalysisItem(Guid clinicSectionId);
    }
}
