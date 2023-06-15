using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Cash;
using WPH.Models.ReceptionInsuranceReceived;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceptionInsuranceReceivedMvcMockingService
    {
        void AddNewReceptionInsuranceReceived(ReceptionInsuranceReceivedViewModel rIR);
        void ReturnInsurance(PayAllServiceViewModel viewModel);
    }
}
