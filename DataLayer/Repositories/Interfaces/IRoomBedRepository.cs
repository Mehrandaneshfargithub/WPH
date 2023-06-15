using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IRoomBedRepository : IRepository<RoomBed>
    {
        IEnumerable<RoomBed> GetAllRoomBedByRoomId(Guid roomId); 
        IEnumerable<RoomBed> GetAllRoomBedWithChildByRoomId(Guid roomId);
        IEnumerable<RoomBed> GetEmptyBedByClinicSectionId(Guid clinicSectionId);
        RoomBed GetWithRoomAndStatus(Guid id);
        RoomBed GetRoomBedWithPatientById(Guid id);
        int GetBedCountByRoomId(Guid roomId);
        bool CheckDuplicateCode(string code, Expression<Func<RoomBed, bool>> predicate = null);
    }
}
