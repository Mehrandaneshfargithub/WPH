using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.Chart;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.Models.ReceptionServiceReceived;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceptionServiceReceivedMvcMockingService
    {
        string PayService(ReceptionServiceReceivedViewModel viewModel, bool InvoiceNumAndPayerNameRequired);
        string PayAllServices(PayAllServiceViewModel viewModel, bool InvoiceNumAndPayerNameRequired);
        IEnumerable<ReceptionInsuranceReceivedViewModel> GetAllReceptionInsuranceReceived(Guid receptionId);
        void PayInstallment(PayAllServiceViewModel viewModel);
        IEnumerable<ReceptionServiceReceivedViewModel> GetAllReceptionServiceRecievedForInstallment(Guid receptionId);
        OperationStatus RemoveInstallment(Guid id);
        PieChartViewModel GetAllClinicInCome(Guid userId, string type);
    }
}
