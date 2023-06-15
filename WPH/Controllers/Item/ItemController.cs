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
using WPH.Models.Item;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Item
{
    [SessionCheck]
    public class ItemController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ItemController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Item");
                ViewBag.AccessNewItem = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditItem = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteItem = access.Any(p => p.AccessName == "Delete");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.item.GetModalsViewBags(ViewBag);
                return View("/Views/Shared/PartialViews/AppWebForms/Item/wpItem.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Item");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            ItemViewModel des = new ItemViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/Item/mdItemNewModal.cshtml", des);
        }
        public JsonResult AddOrUpdate(ItemViewModel Item)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                if (Item.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Item");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.item.CheckRepeatedItemName(clinicSectionId, Item.Name, false, Item.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }

                    Item.ClinicSectionId = clinicSectionId;

                    return Json(_IDUNIT.item.UpdateItem(Item));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "Item");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.item.CheckRepeatedItemName(clinicSectionId, Item.Name, true))
                    {
                        return Json("ValueIsRepeated");
                    }

                    Item.ClinicSectionId = clinicSectionId;

                    return Json(_IDUNIT.item.AddNewItem(Item));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ItemViewModel> AllItem = _IDUNIT.item.GetAllItems(clinicSectionId);
                return Json(AllItem.ToDataSourceResult(request));
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
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "Item");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ItemViewModel hos = _IDUNIT.item.GetItem(Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Item/mdItemNewModal.cshtml", hos);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/Item/mdItemNewModal.cshtml", new ItemViewModel()); }

        }
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Item");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.item.RemoveItem(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetAllItems()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<ItemViewModel> items = _IDUNIT.item.GetAllItems(clinicSectionId);
                return Json(items);
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
