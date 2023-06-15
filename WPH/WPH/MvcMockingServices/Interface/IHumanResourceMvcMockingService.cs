using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.HumanResource;

namespace WPH.MvcMockingServices.Interface
{
    public interface IHumanResourceMvcMockingService
    {
        IEnumerable<HumanResourceViewModel> GetAllHuman(List<Guid> sections);
        IEnumerable<HumanResourceViewModel> GetAllTreatmentStaff(List<Guid> sections);
        IEnumerable<HumanResourceViewModel> GetAllHumanwithPerids(List<Guid> sections, int periodId, DateTime dateFrom, DateTime dateTo, Guid humanId);
        OperationStatus RemoveHuman(Guid HumanId);
        string UpdateHuman(HumanResourceViewModel newHuman);
        HumanResourceViewModel GetHuman(Guid HumanId);
        void GetModalsViewBags(dynamic viewBag);
        Guid GetHumanByName(string humanName);
        string AddNewHuman(HumanResourceViewModel viewModel);
    }
}
