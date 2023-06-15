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
using WPH.Models.RoomBed;
using WPH.MvcMockingServices;

namespace WPH.Controllers.RoomBed
{
    [SessionCheck]
    public class RoomBedController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<RoomBedController> _logger;


        public RoomBedController(IDIUnit dIUnit, ILogger<RoomBedController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid RoomId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("RoomBed");
                ViewBag.AccessNewRoomBed = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditRoomBed = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteRoomBed = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.roomBed.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/RoomBed/wpRoomBed.cshtml", RoomId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }
        public ActionResult AddAndNewModal(Guid RoomId)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "RoomBed");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                RoomBedViewModel des = new()
                {
                    RoomId = RoomId,
                    RoomName = _IDUNIT.room.GetRoom(RoomId).Name
                };
                return PartialView("/Views/Shared/PartialViews/AppWebForms/RoomBed/mdRoomBedNewModal.cshtml", des);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }
        public JsonResult AddOrUpdate(RoomBedViewModel RoomBed)
        {
            try
            {
                if (RoomBed.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "RoomBed");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.roomBed.CheckRepeatedRoomBedName(RoomBed.Name, false, RoomBed.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    var oldRoomBed = _IDUNIT.roomBed.GetRoomBedWithPatient(RoomBed.Guid);

                    if (oldRoomBed != null && !string.IsNullOrWhiteSpace(oldRoomBed.PatientName) && oldRoomBed.BedStatus.Equals("Full") && oldRoomBed.BedStatusId != RoomBed.BedStatusId)
                        return Json("AreYouSure");


                    return Json(_IDUNIT.roomBed.UpdateRoomBed(RoomBed));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "RoomBed");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.roomBed.CheckRepeatedRoomBedName(RoomBed.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    string roomBedid = _IDUNIT.roomBed.AddNewRoomBed(RoomBed);
                    return Json(roomBedid);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0); }
        }

        public JsonResult ConfirmUpdate(RoomBedViewModel RoomBed)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "RoomBed");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                if (_IDUNIT.roomBed.CheckRepeatedRoomBedName(RoomBed.Name, false, RoomBed.NameHolder))
                {
                    return Json("ValueIsRepeated");
                }

                return Json(_IDUNIT.roomBed.UpdateRoomBedWithReceptionRoomBed(RoomBed));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json("SomeThingWentWrong"); }
        }

        public ActionResult EditModal(Guid Id)
        {

            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "RoomBed");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                RoomBedViewModel hos = _IDUNIT.roomBed.GetRoomBed(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/RoomBed/mdRoomBedNewModal.cshtml", hos);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0); }

        }

        public JsonResult RoomName(Guid Id)
        {
            try
            {
                RoomBedViewModel hos = _IDUNIT.roomBed.GetRoomBed(Id);
                return Json($"{hos.RoomName}|{hos.Name}");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0); }

        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "RoomBed");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.roomBed.RemoveRoomBed(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json("ERROR_SomeThingWentWrong"); }
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid RoomId)
        {
            try
            {
                IEnumerable<RoomBedViewModel> AllRoomItem = _IDUNIT.roomBed.GetAllRoomBedsWithChildByRoomId(RoomId);
                return Json(AllRoomItem.ToDataSourceResult(request));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        public ActionResult GetEmptyBedByClinicSectionId(Guid ClinicSectionId)
        {
            try
            {
                IEnumerable<RoomBedViewModel> AllRoomItem = _IDUNIT.roomBed.GetEmptyBedByClinicSectionId(ClinicSectionId);
                return Json(AllRoomItem);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("");
            }
        }

        [HttpPost]
        public ActionResult SetPatientRoomBed(PatientRoomBedViewModel viewModel)
        {
            try
            {
                viewModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                _IDUNIT.roomBed.SetPatientRoomBed(viewModel);
                return Json(0);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json("SomeThingWentWrong"); }
        }

        public ActionResult GetRoomBedByRoomId(Guid roomId)
        {
            try
            {
                IEnumerable<RoomBedViewModel> roomBeds = _IDUNIT.roomBed.GetAllRoomBedsByRoomId(roomId);
                return Json(roomBeds);
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
