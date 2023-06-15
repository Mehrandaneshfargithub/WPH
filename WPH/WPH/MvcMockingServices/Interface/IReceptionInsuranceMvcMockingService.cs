using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.ReceptionInsurance;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceptionInsuranceMvcMockingService
    {
        ReceptionInsuranceViewModel GetReceptionInsurance(Guid ReceptionId);
        ReceptionInsuranceViewModel GetReceptionInsuranceWithReceiveds(Guid receptionId);
    }
}
