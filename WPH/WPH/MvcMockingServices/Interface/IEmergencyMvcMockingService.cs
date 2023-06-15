using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.Emergency;
using WPH.Models.Reception;

namespace WPH.MvcMockingServices.Interface
{
    public interface IEmergencyMvcMockingService
    {
        OperationStatus RemoveEmergency(Guid Emergencyid, string rootPath);
        Guid AddNewEmergency(EmergencyViewModel Emergency);
        Guid UpdateEmergency(EmergencyViewModel Hosp);
        //EmergencyViewModel GetEmergency(Guid EmergencyId);
        ReceptionViewModel GetEmergency(Guid EmergencyId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<PatientReceptionViewModel> GetAllEmergencyReceptionsByClinicSection(Guid id, int periodId, DateTime fromDate, DateTime toDate);
        EmergencyViewModel GetEmergencyById(Guid emergencyId);
    }
}
