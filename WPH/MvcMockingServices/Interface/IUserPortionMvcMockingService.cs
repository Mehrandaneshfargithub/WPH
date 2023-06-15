using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.UserPortion;

namespace WPH.MvcMockingServices.Interface
{
    public interface IUserPortionMvcMockingService
    {
        IEnumerable<UserPortionViewModel> GetAllUserPortionsByClinicSection(Guid clinicSectionId);
        OperationStatus RemoveUserPortion(Guid UserPortionid);
        string AddNewUserPortion(UserPortionViewModel UserPortion);
        string UpdateUserPortion(UserPortionViewModel UserPortion);
        UserPortionViewModel GetUserPortion(Guid UserPortion);
        IEnumerable<UserPortionViewModel> GetAllUserPortionsBySpecification(Guid clinicSectionId, bool specification, Guid ReceptionId);
        IEnumerable<ReceptionDetailPayViewModel> GetAllReceptionDetailPayBySpecification(Guid receptionId, bool specification);
        void AddReceptionDetailPay(ReceptionDetailPayViewModel portion);
        OperationStatus RemoveReceptionDetailPay(Guid id);
        IEnumerable<UserPortionReportViewModel> GetAllUserPortionForReport(Guid userId, DateTime fromDate, DateTime toDate, bool detail);
        IEnumerable<UserPortionReportViewModel> GetPortionReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string status, Guid doctorId);
    }
}
