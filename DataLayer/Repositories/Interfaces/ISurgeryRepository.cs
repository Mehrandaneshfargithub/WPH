using DataLayer.EntityModels;
using DataLayer.FunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISurgeryRepository : IRepository<Surgery>
    {
        IEnumerable<Surgery> GetAllSurgery();
        
        Surgery GetSurgery(Guid SurgeryId);
        void RemoveSurgery(Guid Surgeryid);
        IEnumerable<SurgeryGrid> GetAllSurgeryByClinicSectionId(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? doctorId, Guid? operationId);
        void UpdateSurgery(Surgery surgery2);
        Surgery GetSurgeryReportForPrint(Guid surgeryId);
        Surgery GetSurgeryByReceptionId(Guid receptionId);
        Surgery GetSimpleSurgeryByReceptionId(Guid receptionId);
        object GetNearestOperations(Guid userId);
    }
}
