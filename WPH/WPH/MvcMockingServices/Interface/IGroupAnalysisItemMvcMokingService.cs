using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.GroupAnalysisItem;

namespace WPH.MvcMockingServices.Interface
{
    public interface IGroupAnalysisItemMvcMokingService
    {
        IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItem(Guid groupAnalysisId);
        IEnumerable<GroupAnalysisItemViewModel> GetAllGroupAnalysisItem();
        void GetModalsViewBags(dynamic viewBag);
        void SwapPriority(Guid Id, Guid groupAnalysisId, string type);
        OperationStatus Remove(Guid GroupAnalysisItemId);
        OperationStatus RemoveGroupAnalysisItemWithGroupAnalysisId(Guid GroupAnalysisId);
        GroupAnalysisItemViewModel GetGroupAnalysisItem(Guid GroupAnalysisItemId);
        Guid AddNewGroupAnalysisItem(GroupAnalysisItemViewModel groupAnalysisItem, Guid groupAnalysisId);
        Guid UpdateGroupAnalysisItem(GroupAnalysisItemViewModel groupAnalysisItem);



    }
}
