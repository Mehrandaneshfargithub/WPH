using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Reminder;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReminderMvcMockingService
    {
        OperationStatus RemoveReminder(Guid Reminderid);
        string AddNewReminder(ReminderViewModel Reminder);
        string UpdateReminder(ReminderViewModel Reminder);
        IEnumerable<ReminderViewModel> GetAllReminders(Guid clinicSectionId);
        ReminderViewModel GetReminder(Guid ReminderId);
        void ChangeReminderActivation(Guid reminderId);
        void GetModalsViewBags(dynamic viewBag);
        int GetUnReadCount(Guid clinicSectionId, DateTime date);
    }
}
