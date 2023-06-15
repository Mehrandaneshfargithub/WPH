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
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReminderRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<Reminder> GetAllReminder(Guid clinicSectionId)
        {
            return Context.Reminders.AsNoTracking()
                .Where(p => p.ClinicSectionId == clinicSectionId)
                .OrderByDescending(o => o.ReminderDate)
                .OrderByDescending(o => o.Active);
        }

        public Reminder GetWithType(Guid reminderId)
        {
            return Context.Reminders.AsNoTracking()
                .SingleOrDefault(p => p.Guid == reminderId);
        }

        public int GetUnReadCount(Guid clinicSectionId, DateTime date)
        {
            return Context.Reminders.AsNoTracking()
                .Where(p => p.ClinicSectionId == clinicSectionId && p.Active != null && p.Active.Value && p.ReminderDate <= date.Date)
                .Count();
        }
    }
}
