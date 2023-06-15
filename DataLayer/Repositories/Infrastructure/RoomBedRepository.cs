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
    public class RoomBedRepository : Repository<RoomBed>, IRoomBedRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public RoomBedRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<RoomBed> GetAllRoomBedByRoomId(Guid roomId)
        {
            return Context.RoomBeds.AsNoTracking().
                Where(p => p.RoomId.Equals(roomId));
        }

        public IEnumerable<RoomBed> GetAllRoomBedWithChildByRoomId(Guid roomId)
        {
            return Context.RoomBeds.AsNoTracking().
                Include(p => p.Room).
                Include(p => p.Status).
                Include(p => p.ReceptionRoomBeds).ThenInclude(s => s.Reception.Patient.User).
                Where(p => p.RoomId.Equals(roomId)).
                OrderByDescending(p => p.Id);
        }

        public RoomBed GetRoomBedWithPatientById(Guid id)
        {
            return Context.RoomBeds.AsNoTracking()
                .Include(p => p.Room)
                .Include(p => p.Status)
                .Include(p => p.ReceptionRoomBeds).ThenInclude(s => s.Reception.Patient.User)
                .Where(p => p.Guid.Equals(id))
                .Select(s => new RoomBed
                {
                    StatusId = s.StatusId,
                    Status = new BaseInfoGeneral
                    {
                        Name = s.Status.Name
                    },
                    ReceptionRoomBeds = (ICollection<ReceptionRoomBed>)s.ReceptionRoomBeds.Where(w => w.ExitDate == null).Select(n => new ReceptionRoomBed
                    {
                        Reception = new Reception
                        {
                            Patient = new Patient
                            {
                                User = new User
                                {
                                    Name = n.Reception.Patient.User.Name
                                }
                            }
                        }
                    })
                })
                .SingleOrDefault();
        }

        public IEnumerable<RoomBed> GetEmptyBedByClinicSectionId(Guid clinicSectionId)
        {
            return Context.RoomBeds.AsNoTracking().
                Include(p => p.Room.ClinicSection).
                Include(p => p.Status).
                Where(p => p.Room.ClinicSectionId.Equals(clinicSectionId) && (p.Status.Name.ToLower().Equals("clean") /*|| p.Status.Name.ToLower().Equals("dirty")*/));
        }

        public RoomBed GetWithRoomAndStatus(Guid id)
        {

            return Context.RoomBeds.AsNoTracking().
                Include(p => p.Room).
                Include(p => p.Status).
                SingleOrDefault(p => p.Guid.Equals(id));
        }

        public int GetBedCountByRoomId(Guid roomId)
        {
            return _context.RoomBeds.AsNoTracking()
                .Where(p => p.RoomId == roomId)
                .Count();
        }

        public bool CheckDuplicateCode(string code, Expression<Func<RoomBed, bool>> predicate = null)
        {
            IQueryable<RoomBed> result = _context.RoomBeds.AsNoTracking()
                .Where(p => p.Code == code);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }
    }
}
