using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysis_AnalysisItemMvcMockingService
    {
        IEnumerable<AnalysisItemMinMaxValueViewModel> GetAllAnalysisItemMinMaxValue(Guid analysisItemId);
        void GetModalsViewBags(dynamic viewBag);
        OperationStatus RemoveAnalysisItemMinMaxValue(Guid AnalysisItemMinMaxValueId);
        OperationStatus RemoveAllWithAnalysisItemId(Guid AnalysisItemId);
        AnalysisItemMinMaxValueViewModel GetAnalysisItemMinMaxValue(Guid AnalysisItemMinMaxValueId);
        Guid AddNewAnalysisItemMinMaxValue(AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue);
        Guid UpdateAnalysisItemMinMaxValue(AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValue);
    }
}
