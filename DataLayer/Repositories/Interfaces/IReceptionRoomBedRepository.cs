using DataLayer.EntityModels;
using DataLayer.FunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    
    public interface IReceptionRoomBedRepository : IRepository<ReceptionRoomBed>
    {
        IEnumerable<ReceptionRoomBed> GetAllReceptionRoomBed();
        ReceptionRoomBed GetReceptionRMWithReceptionByRoomBedId(Guid roomBedId);
        ReceptionRoomBed GetReceptionRMWithReceptionByReceptionId(Guid receptionId);
        ReceptionRoomBed GetReceptionRMWithReceptionAndBedByReceptionId(Guid receptionId);
        IEnumerable<ReceptionRoomBed> FilterReceptionByRoomAndBedAndPatientAndUser(Guid userId, Guid roomId, Guid roomBedId, Guid patientId,Expression<Func<ReceptionRoomBed, bool>> predicate = null);
        IEnumerable<ReceptionRoomBed> FilterReceptionByRoomAndBedAndPatient(Guid roomId, Guid roomBedId, Guid patientId, Expression<Func<ReceptionRoomBed, bool>> predicate = null);
        ReceptionRoomBed GetReceptionRoomBedName(Guid receptionId);
        IEnumerable<ReceptionRoomBed> GetReceptionRoomBedForDischarge(Guid receptionId);
        IEnumerable<HospitalDashboardModel> GetAllRoomsWithPatient(IEnumerable<Guid> clinicSectionId);
    }
}
