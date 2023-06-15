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
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public RoomRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Room> GetAllRoomsWithChild(Expression<Func<Room, bool>> predicate = null)
        {
            IQueryable<Room> result = Context.Rooms.AsNoTracking().
                Include(p => p.ClinicSection).
                Include(p => p.Status).
                Include(p => p.RoomBeds).ThenInclude(p => p.Status).
                Include(p => p.Type);

            if (predicate != null)
                result = result.Where(predicate);

            return result.
                OrderByDescending(p => p.Id);
        }

        public Room GetRoomWithSection(Guid roomId)
        {
            return Context.Rooms.AsNoTracking().Include(p => p.ClinicSection).
                FirstOrDefault(p => p.Guid.Equals(roomId));
        }

        public IEnumerable<Room> GetAllRoomsWithChildByUserId(Guid userId, Expression<Func<Room, bool>> predicate = null)
        {
            IQueryable<Room> result = Context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                .Include(x => x.ClinicSection.Rooms).ThenInclude(p => p.ClinicSection)
                .Include(x => x.ClinicSection.Rooms).ThenInclude(p => p.Status)
                .Include(x => x.ClinicSection.Rooms).ThenInclude(p => p.Type)
                .Include(x => x.ClinicSection.Rooms).ThenInclude(p => p.RoomBeds).ThenInclude(p => p.Status)
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "NotLab")
                .SelectMany(x => x.ClinicSection.Rooms);

            if (predicate != null)
                result = result.Where(predicate);

            return result.
                OrderByDescending(p => p.Id);
        }

        public IEnumerable<Room> GetAllRooms(Expression<Func<Room, bool>> predicate = null)
        {
            IQueryable<Room> result = Context.Rooms.AsNoTracking().
                Include(p => p.ClinicSection);

            if (predicate != null)
                result = result.Where(predicate);

            return result.
                OrderByDescending(p => p.Id);
        }

        public IEnumerable<Room> GetAllRoomsByUserId(Guid userId, Expression<Func<Room, bool>> predicate = null)
        {
            IQueryable<Room> result = Context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection).ThenInclude(x => x.ClinicSectionType)
                .Include(x => x.ClinicSection.Rooms).ThenInclude(p => p.ClinicSection)
                .Where(x => x.UserId == userId && x.ClinicSection.ClinicSectionType.Name == "NotLab")
                .SelectMany(x => x.ClinicSection.Rooms);

            if (predicate != null)
                result = result.Where(predicate);

            return result.
                OrderByDescending(p => p.Id);
        }

        public int? GetRoomCapacityRemined(Guid? roomId)
        {
            return Context.Rooms.AsNoTracking()
                .Include(p => p.RoomBeds)
                .Where(p => p.Guid == roomId)
                .Select(x => x.MaxCapacity - x.RoomBeds.Count)
                .SingleOrDefault();
        }


        public Room GetRoomWithBeds(Guid roomId)
        {
            return _context.Rooms.AsNoTracking()
                .Include(p => p.RoomBeds)
                .SingleOrDefault(p => p.Guid == roomId);
        }
    }

}
