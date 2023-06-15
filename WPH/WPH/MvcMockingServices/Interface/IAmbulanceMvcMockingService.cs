using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Ambulance;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAmbulanceMvcMockingService
    {
        OperationStatus RemoveAmbulance(Guid Ambulanceid);
        string AddNewAmbulance(AmbulanceViewModel Ambulance);
        string UpdateAmbulance(AmbulanceViewModel Hosp);
        bool CheckRepeatedAmbulanceName(string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<AmbulanceViewModel> GetAllAmbulances();
        AmbulanceViewModel GetAmbulance(Guid AmbulanceId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
