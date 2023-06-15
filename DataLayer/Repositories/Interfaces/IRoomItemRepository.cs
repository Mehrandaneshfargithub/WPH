using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IRoomItemRepository : IRepository<RoomItem>
    {
        IEnumerable<RoomItem> GetAllRoomItem();
        IEnumerable<RoomItem> GetAllRoomItemByRoomId(Guid roomId);
        RoomItem GetWithRoomAndStatus(Guid id);
        bool CheckDuplicateCode(string code, Expression<Func<RoomItem, bool>> predicate = null);
    }
}
