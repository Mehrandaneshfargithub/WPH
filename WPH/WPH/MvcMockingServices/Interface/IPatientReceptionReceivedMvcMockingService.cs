using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientReceptionReceived;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientReceptionReceivedMvcMockingService
    {
        //IEnumerable<PatientReceptionReceivedViewModel> GetAllPatientReceptionReceived(Guid patientReceptionId);
        //void GetModalsViewBags(dynamic viewBag);
        ///OperationStatus RemovePatientReceptionReceived(Guid PatientReceptionReceivedId);
        //PatientReceptionReceivedViewModel GetPatientReceptionReceived(Guid PatientReceptionReceivedId);
        Guid AddNewPatientReceptionReceived(PatientReceptionReceivedViewModel PatientReceptionReceived);
        //Guid UpdatePatientReceptionReceived(PatientReceptionReceivedViewModel PatientReceptionReceived);

    }
}
