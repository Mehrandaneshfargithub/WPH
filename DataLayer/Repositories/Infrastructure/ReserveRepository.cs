using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    class ReserveRepository : Repository<Reserve>, IReserveRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReserveRepository(WASContext context)
            : base(context)
        {
        }

        public Reserve GetDate(DateTime date, Guid ClinicSectionId)
        {
            return _context.Reserves
                .AsNoTracking()
                .SingleOrDefault(x => x.ClinicSectionId == ClinicSectionId && x.Date == date);
        }

        public IEnumerable<Reserve> GetClinicCalendarDaysByClinicSection(Guid clinicSectionId, DateTime fromDate, DateTime toDate)
        {
            return _context.Reserves
                .AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId && x.Date >= fromDate && x.Date <= toDate);
        }

        public async Task SaveClinicCalendar(List<Reserve> allReserve, Guid clinicSectionId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                //_context.Reserves.RemoveRange(_context.Reserves
                //.AsNoTracking()
                //.Where(x => x.ClinicSectionId == clinicSectionId && x.Date >= fromDate && x.Date <= toDate));

                foreach (var day in allReserve)
                {
                    if (day.Guid != Guid.Empty)
                    {
                        _context.Reserves.Attach(day);
                        _context.Entry(day).Property(x => x.StartTime).IsModified = true;
                        _context.Entry(day).Property(x => x.EndTime).IsModified = true;
                        _context.Entry(day).Property(x => x.RoundTime).IsModified = true;
                    }
                }

                IEnumerable<Reserve> allNewDays = allReserve.Where(x => x.Guid == Guid.Empty);

                await _context.Reserves.AddRangeAsync(allNewDays);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { throw e; }

        }

        public IEnumerable<Reserve> GetClinicSectionCalendar(Guid clinicSectionId)
        {
            DateTime today = DateTime.Now;
            return _context.Reserves.AsNoTracking()
                .Include(x => x.ReserveDetails)
                .Where(x => x.ClinicSectionId == clinicSectionId && x.Date >= today).Select(a => new Reserve
                {
                    Guid = a.Guid,
                    Date = a.Date,
                    EndTime = a.EndTime,
                    StartTime = a.StartTime,
                    RoundTime = a.RoundTime,
                    ReserveDetails = a.ReserveDetails == null ? null : a.ReserveDetails.Select(b => new ReserveDetail
                    {

                        ReserveEndTime = b.ReserveEndTime,
                        ReserveStartTime = b.ReserveStartTime

                    }).ToList()

                });
        }


    }
}
