using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.Reminder;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Reminder
{
    [SessionCheck]
    public class ReminderController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReminderController> _logger;

        public ReminderController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReminderController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Reminder");
                ViewBag.AccessNewReminder = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditReminder = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteReminder = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.reminder.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Reminder/wpReminder.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }

            
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Reminder");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ReminderViewModel des = new ReminderViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Reminder/mdReminderNewModal.cshtml", des);
        }
        public JsonResult AddOrUpdate(ReminderViewModel Reminder)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                Reminder.ClinicSectionId = clinicSectionId;

                if (Reminder.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reminder");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    Reminder.ModifiedUserId = userId;

                    return Json(_IDUNIT.reminder.UpdateReminder(Reminder));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Reminder");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    Reminder.CreateUserId = userId;

                    return Json(_IDUNIT.reminder.AddNewReminder(Reminder));
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ReminderViewModel> AllReminder = _IDUNIT.reminder.GetAllReminders(clinicSectionId);
                return Json(AllReminder.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Reminder");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ReminderViewModel hos = _IDUNIT.reminder.GetReminder(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Reminder/mdReminderNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Reminder/mdReminderNewModal.cshtml", new ReminderViewModel()); }

        }
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Reminder");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.reminder.RemoveReminder(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public JsonResult ChangeReminderActivation(Guid id)
        {
            try
            {
                _IDUNIT.reminder.ChangeReminderActivation(id);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

    }
}
