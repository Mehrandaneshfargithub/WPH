using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.Chart;
using WPH.Models.ReceptionService;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceptionServiceMvcMockingService
    {
        IEnumerable<ReceptionServiceViewModel> GetReceptionServicesByReceptionId(Guid receptionId, string DMS);
        ReceptionServiceViewModel GetReceptionService(Guid id);
        Guid GetReceptionOperationService(Guid receptionId);
        ReceptionServiceViewModel GetReceptionExceptOperationService(Guid receptionId);
        IEnumerable<ReceptionServiceViewModel> GetReceptionSpecificServicesByReceptionId(Guid receptionId, string serviceType);
        void AddReceptionService(ReceptionServiceViewModel service);
        OperationStatus RemoveReceptionService(Guid id);
        ReceptionServiceViewModel GetReceptionOperation(Guid receptionId);
        IEnumerable<ReceptionServiceViewModel> GetAllReceptionProducts(Guid receptionId, string DMS);
        DoctorWageViewModel GetReceptionOperationAndDoctor(Guid receptionId);
        string AddDoctorWage(DoctorWageViewModel viewModel);
        PieChartViewModel GetMostOperations(Guid userId);
    }
}
