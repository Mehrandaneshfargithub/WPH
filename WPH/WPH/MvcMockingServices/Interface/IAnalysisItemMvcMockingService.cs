using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItem;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisItemMvcMockingService
    {
        IEnumerable<AnalysisItemViewModel> GetAllAnalysisItem(Guid clinicSectionId);
        void GetModalsViewBags(dynamic viewBag);
        List<AnalysisItemJustNameAndGuidViewModel> GetAllAnalysisItemsWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId);
        void SwapPriority(Guid AnalysisItemId, string type);
        void AnalysisAnalysisItemSwapPriority(Guid AnalysisItemId, string type);
        OperationStatus RemoveAnalysisItem(Guid AnalysisItemId);
        AnalysisItemViewModel GetAnalysisItem(Guid AnalysisItemId);
        void UpdateAnalysisItemButtonAndPriority(Guid clinicSectionId, IEnumerable<AnalysisItemViewModel> allAnalysisItem);
        Guid AddNewAnalysisItem(AnalysisItemViewModel analysisItem);
        Guid UpdateAnalysisItem(AnalysisItemViewModel analysisItem);
        void AddAnalysisOfAnalysisItem(Guid analysisItemId, Guid analysisId);
        IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemWithoutInAnalysisByUserId(Guid analysisId, Guid userId);
        IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(Guid groupId);
        IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemByClinicSectionId(Guid clinicSectionId);
        IEnumerable<AnalysisItemViewModel> GetAllAnalysisItemName();
        string CreateChartInResult(Guid analysisItemId);
    }
}
