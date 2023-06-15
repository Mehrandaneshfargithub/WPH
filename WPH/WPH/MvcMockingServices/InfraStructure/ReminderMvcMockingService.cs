using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using WPH.Helper;
using WPH.Models.Reminder;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReminderMvcMockingService : IReminderMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReminderMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        public OperationStatus RemoveReminder(Guid Reminderid)
        {
            try
            {
                Reminder Hos = _unitOfWork.Reminders.Get(Reminderid);
                _unitOfWork.Reminders.Remove(Hos);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Reminder/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }
        public string AddNewReminder(ReminderViewModel reminder)
        {
            if (string.IsNullOrWhiteSpace(reminder.Explanation) || string.IsNullOrWhiteSpace(reminder.TxtReminderDate))
                return "DataNotValid";

            if (!DateTime.TryParseExact(reminder.TxtReminderDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
                return "DateNotValid";

            reminder.ReminderDate = date;
            reminder.CreateDate = DateTime.Now;
            reminder.Active = true;

            Reminder reminder1 = Common.ConvertModels<Reminder, ReminderViewModel>.convertModels(reminder);

            _unitOfWork.Reminders.Add(reminder1);
            _unitOfWork.Complete();
            return reminder1.Guid.ToString();
        }


        public string UpdateReminder(ReminderViewModel reminder)
        {
            if (string.IsNullOrWhiteSpace(reminder.Explanation) || string.IsNullOrWhiteSpace(reminder.TxtReminderDate))
                return "DataNotValid";

            if (!DateTime.TryParseExact(reminder.TxtReminderDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
                return "DateNotValid";

            Reminder reminder1 = _unitOfWork.Reminders.Get(reminder.Guid);

            reminder1.Explanation = reminder.Explanation;
            reminder1.ReminderDate = date;
            reminder1.ModifiedUserId = reminder.ModifiedUserId;
            reminder1.ModifiedDate = DateTime.Now;

            _unitOfWork.Reminders.UpdateState(reminder1);
            _unitOfWork.Complete();
            return reminder1.Guid.ToString();

        }

        public IEnumerable<ReminderViewModel> GetAllReminders(Guid clinicSectionId)
        {
            IEnumerable<Reminder> hosp = _unitOfWork.Reminders.GetAllReminder(clinicSectionId);
            List<ReminderViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<ReminderViewModel> indexing = new Indexing<ReminderViewModel>();
            return indexing.AddIndexing(hospconvert);
        }

        public ReminderViewModel GetReminder(Guid ReminderId)
        {
            try
            {
                Reminder Remindergu = _unitOfWork.Reminders.GetWithType(ReminderId);
                return ConvertModel(Remindergu);
            }
            catch { return null; }
        }

        public int GetUnReadCount(Guid clinicSectionId, DateTime date)
        {
            return _unitOfWork.Reminders.GetUnReadCount(clinicSectionId, date);
        }

        public void ChangeReminderActivation(Guid reminderId)
        {
            var reminder = _unitOfWork.Reminders.Get(reminderId);

            reminder.Active = !reminder.Active.GetValueOrDefault(false);

            _unitOfWork.Reminders.UpdateState(reminder);
            _unitOfWork.Complete();
        }

        // Begin Convert 
        public ReminderViewModel ConvertModel(Reminder Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reminder, ReminderViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Reminder, ReminderViewModel>(Users);
        }
        public List<ReminderViewModel> ConvertModelsLists(IEnumerable<Reminder> reminders)
        {
            List<ReminderViewModel> reminderDtoList = new List<ReminderViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reminder, ReminderViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            reminderDtoList = mapper.Map<IEnumerable<Reminder>, List<ReminderViewModel>>(reminders);
            return reminderDtoList;
        }
        // End Convert
    }
}
