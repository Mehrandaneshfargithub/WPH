using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Child;

namespace WPH.MvcMockingServices.Interface
{
    public interface IChildMvcMockingService
    {
        OperationStatus RemoveChild(Guid Childid);
        string RemoveFromHospitalPatient(Guid Childid, Guid userId);
        string AddToHospitalPatient(ChildHospitalPatientViewModel viewModel);
        string AddNewChild(ChildViewModel Child);
        string UpdateChild(ChildViewModel Child);
        bool CheckRepeatedChildName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<ChildViewModel> GetAllChildren(Guid clinicSectionId);
        IEnumerable<ChildHospitalPatientViewModel> GetAllUnknownChildren(Guid clinicSectionId);
        ChildViewModel GetChild(Guid ChildId);
        void GetModalsViewBags(dynamic viewBag);
        ChildReportResultViewModel ChildReport(ChildReportViewModel reportViewModel);
        IEnumerable<ChildHospitalPatientViewModel> GetAllHospitalPatientChildren(Guid receptionId);
        IEnumerable<NewBornBabiesReportViewModel> GetAllHospitalPatientChildrenReport(Guid receptionId);
    }
}
