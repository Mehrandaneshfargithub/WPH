using System.Collections.Generic;
using WPH.Areas.Admin.Models.LicenceKeyManagement;
using WPH.Helper;


namespace WPH.MvcMockingServices.Interface
{
    public interface ILicenceKeyMvcMockingService
    {
        OperationStatus RemoveLicenceKey(int id);
        string AddNewLicenceKey(string licenceKey);
        string CheckLicence();
        IEnumerable<LicenceKeyManagementViewModel> GetAll();
    }
}
