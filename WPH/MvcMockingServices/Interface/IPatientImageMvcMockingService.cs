using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.PatientImage;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientImageMvcMockingService
    {
        IEnumerable<PatientImageViewModel> GetAllPatientImages(Guid patientId);
        void GetModalsViewBags(dynamic viewBag);
        Guid AddPatientImage(PatientImageViewModel PatientImage);
        Guid UpdatePatientImage(PatientImageViewModel med);
        OperationStatus RemovePatientImage(Guid PatientImageId, string rootPath);
        PatientImageViewModel GetPatientImage(Guid PatientImageId);
        IEnumerable<PatientImageViewModel> GetAllVisitImages(Guid visitId);
        IEnumerable<PatientImageViewModel> GetMainAttachmentsByPatientId(Guid patientId);
        IEnumerable<PatientImageViewModel> GetPoliceReportAttachmentsByPatientId(Guid patientId);
        IEnumerable<PatientImageViewModel> GetMainAttachmentsByReceptionId(Guid receptionId);
        IEnumerable<PatientImageViewModel> GetOtherAttachmentsByReceptionId(Guid receptionId);
        IEnumerable<PatientImageViewModel> GetPoliceReportAttachmentsByReceptionId(Guid receptionId);
    }
}
