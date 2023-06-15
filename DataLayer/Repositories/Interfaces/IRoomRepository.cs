using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        IEnumerable<Room> GetAllRoomsWithChild(Expression<Func<Room, bool>> predicate = null);
        IEnumerable<Room> GetAllRoomsWithChildByUserId(Guid userId, Expression<Func<Room, bool>> predicate = null);
        Room GetRoomWithSection(Guid roomId);
        IEnumerable<Room> GetAllRooms(Expression<Func<Room, bool>> predicate = null);
        IEnumerable<Room> GetAllRoomsByUserId(Guid userId, Expression<Func<Room, bool>> predicate = null);
        int? GetRoomCapacityRemined(Guid? roomId);
        Room GetRoomWithBeds(Guid roomId);
    }
}
