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
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.Dashboard;
using WPH.Models.Room;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Room
{
    [SessionCheck]
    public class RoomController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<RoomController> _logger;

        public RoomController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<RoomController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.room.GetModalsViewBags(ViewBag);

                BaseInfosAndPeriodsViewModel baseInfosAndPeriods = new BaseInfosAndPeriodsViewModel();
                IEnumerable<PeriodsViewModel> periods = _IDUNIT.baseInfo.GetAllPeriods(_localizer);
                baseInfosAndPeriods.periods = periods;
                ViewBag.FromToId = (int)Periods.FromDateToDate;

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Room", "RoomBed", "RoomItem");
                ViewBag.AccessNewRoom = access.Any(p => p.AccessName == "New" && p.SubSystemName == "Room");
                ViewBag.AccessEditRoom = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Room");
                ViewBag.AccessDeleteRoom = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Room");

                ViewBag.AccessNewRoomBed = access.Any(p => p.AccessName == "New" && p.SubSystemName == "RoomBed");
                ViewBag.AccessEditRoomBed = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "RoomBed");
                ViewBag.AccessDeleteRoomBed = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "RoomBed");

                ViewBag.AccessNewRoomItem = access.Any(p => p.AccessName == "New" && p.SubSystemName == "RoomItem");
                ViewBag.AccessEditRoomItem = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "RoomItem");
                ViewBag.AccessDeleteRoomItem = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "RoomItem");

                return View("/Views/Shared/PartialViews/AppWebForms/Room/wpRoom.cshtml", baseInfosAndPeriods);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Room");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            RoomViewModel rom = new RoomViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/Room/mdRoomNewModal.cshtml", rom);
        }

        public ActionResult SelectModal(Guid ClinicSectionId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Room", "RoomBed");
                ViewBag.AccessEditRoom = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Room");
                ViewBag.AccessDeleteRoom = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "Room");

                ViewBag.AccessNewRoomBed = access.Any(p => p.AccessName == "New" && p.SubSystemName == "RoomBed");
                ViewBag.AccessEditRoomBed = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "RoomBed");
                ViewBag.AccessDeleteRoomBed = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "RoomBed");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Room/mdRoomSelectModal.cshtml", ClinicSectionId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public JsonResult AddOrUpdate(RoomViewModel room)
        {
            try
            {
                if (room.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Room");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.room.CheckRepeatedRoomName(room.Name, false, room.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.room.UpdateRoom(room));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Room");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.room.CheckRepeatedRoomName(room.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.room.AddNewRoom(room));
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid SectionId)
        {
            try
            {

                string username = HttpContext.Session.GetString("UserName");
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<RoomViewModel> AllRoom;
                if (username == "developer")
                {
                    AllRoom = _IDUNIT.room.GetAllRoomsWithChildBySectionId(SectionId);
                }
                else
                {
                    AllRoom = _IDUNIT.room.GetAllRoomsWithChildForUserBySectionId(userId, SectionId);
                }

                return Json(AllRoom.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        public ActionResult RoomGrid(Guid SectionId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Room");
                ViewBag.AccessEditRoom = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteRoom = access.Any(p => p.AccessName == "Delete");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Room/dgRoomGrid.cshtml", SectionId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Room");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                RoomViewModel room = _IDUNIT.room.GetRoom(Id);
                room.NameHolder = room.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Room/mdRoomNewModal.cshtml", room);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Room");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.room.RemoveRoom(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json("ERROR_SomeThingWentWrong");
            }
        }


        public JsonResult GetAllRooms()
        {
            try
            {
                string username = HttpContext.Session.GetString("UserName");
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                IEnumerable<RoomViewModel> allRoom;
                if (username == "developer")
                {
                    allRoom = _IDUNIT.room.GetAllRoomsBySectionId(Guid.Empty);
                }
                else
                {
                    allRoom = _IDUNIT.room.GetAllRoomsForUserBySectionId(userId, Guid.Empty);
                }
                return Json(allRoom);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllRoomsWithPatients()
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Room");
                if (access)
                {
                    Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                    Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    IEnumerable<Guid> clinicsections = _IDUNIT.clinicSection.GetClinicSectionsForUser(userId, "", ClinicSectionId).Select(a => a.Guid);
                    IEnumerable<HospitalDashboardViewModel> all = _IDUNIT.receptionRoomBed.GetAllRoomsWithPatient(clinicsections);
                    return Json(all);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


    }
}
