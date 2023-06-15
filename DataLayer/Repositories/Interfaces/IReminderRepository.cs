using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        IEnumerable<Reminder> GetAllReminder(Guid clinicSectionId);
        Reminder GetWithType(Guid reminderId);
        int GetUnReadCount(Guid clinicSectionId, DateTime date);
    }
}
