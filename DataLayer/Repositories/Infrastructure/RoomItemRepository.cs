using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class RoomItemRepository : Repository<RoomItem>, IRoomItemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public RoomItemRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<RoomItem> GetAllRoomItem()
        {
            return Context.RoomItems.AsNoTracking().
                Include(p => p.Room).
                Include(p => p.Item);
        }

        public IEnumerable<RoomItem> GetAllRoomItemByRoomId(Guid roomId)
        {
            return Context.RoomItems.AsNoTracking().
                Include(p => p.Room).
                Include(p => p.Item.ItemType).
                Where(p => p.RoomId.Equals(roomId));
        }

        public RoomItem GetWithRoomAndStatus(Guid id)
        {
            return Context.RoomItems.AsNoTracking().
                Include(p => p.Room).
                Include(p => p.Item.ItemType).
                SingleOrDefault(p => p.Guid.Equals(id));
        }

        public bool CheckDuplicateCode(string code, Expression<Func<RoomItem, bool>> predicate = null)
        {
            IQueryable<RoomItem> result = _context.RoomItems.AsNoTracking()
                .Where(p => p.Code == code);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }
    }
}
