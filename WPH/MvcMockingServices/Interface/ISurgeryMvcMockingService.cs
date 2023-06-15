using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.Service;
using WPH.Models.Surgery;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISurgeryMvcMockingService
    {
        OperationStatus RemoveSurgery(Guid Surgeryid);
        string UpdateSurgery(SurgeryViewModel hosp, Guid clinicSectionId);
        SurgeryViewModel GetSurgery(Guid SurgeryId);
        IEnumerable<SurgeryGridViewModel> GetAllSurgeryByClinicSectionId(Guid id, int periodId, DateTime fromDate, DateTime toDate, Guid? doctorId, Guid? operationId);
        SurgeryViewModel GetSurgeryReportForPrint(Guid surgeryId);
        SurgeryViewModel GetSurgeryByReceptionId(Guid receptionId);
        ServiceViewModel GetReceptionOperation(Guid receptionId);
        PieChartViewModel GetNearestOperations(Guid userId);
    }
}
