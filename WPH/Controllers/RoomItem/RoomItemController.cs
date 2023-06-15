using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.RoomItem;
using WPH.MvcMockingServices;

namespace WPH.Controllers.RoomItem
{
    [SessionCheck]
    public class RoomItemController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<RoomItemController> _logger;


        public RoomItemController(IDIUnit dIUnit, ILogger<RoomItemController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid RoomId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("RoomItem");
                ViewBag.AccessNewRoomItem = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditRoomItem = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteRoomItem = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.roomItem.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/RoomItem/wpRoomItem.cshtml", RoomId);
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
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "RoomItem");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                RoomItemViewModel des = new()
                {
                    RoomId = RoomId,
                    RoomName = _IDUNIT.room.GetRoom(RoomId).Name
                };

                return PartialView("/Views/Shared/PartialViews/AppWebForms/RoomItem/mdRoomItemNewModal.cshtml", des);

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }
        }

        public JsonResult AddOrUpdate(RoomItemViewModel RoomItem)
        {
            try
            {
                if (RoomItem.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "RoomItem");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.roomItem.CheckRepeatedRoomItem(RoomItem.RoomId, RoomItem.ItemId, false, RoomItem.RoomIdHolder, RoomItem.ItemHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.roomItem.UpdateRoomItem(RoomItem));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "RoomItem");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.roomItem.CheckRepeatedRoomItem(RoomItem.RoomId, RoomItem.ItemId, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    return Json(_IDUNIT.roomItem.AddNewRoomItem(RoomItem));
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


        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid RoomId)
        {
            try
            {

                IEnumerable<RoomItemViewModel> AllRoomItem = _IDUNIT.roomItem.GetAllRoomItemsByRoomId(RoomId);
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

        public ActionResult EditModal(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "RoomItem");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                RoomItemViewModel hos = _IDUNIT.roomItem.GetRoomItem(Id);
                hos.RoomIdHolder = hos.RoomId;
                hos.ItemHolder = hos.ItemHolder;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/RoomItem/mdRoomItemNewModal.cshtml", hos);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);

                return Json(0);
            }

        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "RoomItem");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.roomItem.RemoveRoomItem(Id);
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
    }
}
