using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReceptionAmbulance;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceptionAmbulanceMvcMockingService
    {
        OperationStatus RemoveReceptionAmbulance(Guid ReceptionAmbulanceid);
        Guid AddNewReceptionAmbulance(ReceptionAmbulanceViewModel ReceptionAmbulance);
        Guid UpdateReceptionAmbulance(ReceptionAmbulanceViewModel Hosp);
        //ReceptionAmbulanceViewModel GetReceptionAmbulance(Guid ReceptionAmbulanceId);
        IEnumerable<ReceptionAmbulanceViewModel> GetAllReceptionAmbulances();
        ReceptionAmbulanceViewModel GetReceptionAmbulance(Guid ReceptionAmbulanceId);
        void GetModalsViewBags(dynamic viewBag);
        ReceptionAmbulanceViewModel GetReceptionAmbulanceByReceptionId(Guid receptionId);
    }
}
