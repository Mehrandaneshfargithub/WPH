using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisItemValuesRangeMvcMockingService
    {
        void AddAnalysisOfAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId, Guid analysisItemId);
        IEnumerable<AnalysisItemValuesRangeViewModel> GetAllAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId);
        void GetModalsViewBags(dynamic viewBag);
        OperationStatus RemoveAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId);
        OperationStatus RemoveAllWithAnalysisItemId(Guid AnalysisItemId);
        AnalysisItemValuesRangeViewModel GetAnalysisItemValuesRange(Guid AnalysisItemValuesRangeId);
        Guid AddNewAnalysisItemValuesRange(AnalysisItemValuesRangeViewModel AnalysisItemValuesRange);
        Guid UpdateAnalysisItemValuesRange(AnalysisItemValuesRangeViewModel AnalysisItemValuesRange);





    }
}
