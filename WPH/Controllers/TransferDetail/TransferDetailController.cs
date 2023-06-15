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
using WPH.Models.TransferDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.TransferDetail
{
    [SessionCheck]
    public class TransferDetailController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<TransferDetailController> _logger;

        public TransferDetailController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<TransferDetailController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid transferId)
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("TransferDetail", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "TransferDetailSalePrice");

                ViewBag.AccessNewTransferDetail = access.Any(p => p.AccessName == "New" && p.SubSystemName == "TransferDetail");
                ViewBag.AccessEditTransferDetail = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TransferDetail");
                ViewBag.AccessDeleteTransferDetail = access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "TransferDetail");

                ViewBag.AccessCanUseWholeSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
                ViewBag.AccessCanUseMiddleSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.transferDetail.GetModalsViewBags(ViewBag);

                var hos = _IDUNIT.transfer.GetTransfer(transferId);
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                ViewBag.AccessTransferDetailSalePrice = access.Any(p => p.SubSystemName == "TransferDetailSalePrice") && hos.DestinationClinicSectionId == clinicSectionId;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/TransferDetail/wpTransferDetail.cshtml", transferId);
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
        public JsonResult AddList(List<TransferDetailViewModel> viewModels)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                var access = _IDUNIT.subSystem.CheckUserAccess("New", "TransferDetail");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                return Json(_IDUNIT.transferDetail.AddNewTransferDetailList(viewModels, userId));

            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult AddOrUpdate(TransferDetailViewModel TransferDetail)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                if (TransferDetail.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "TransferDetail");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    TransferDetail.ModifiedUserId = userId;

                    return Json(_IDUNIT.transferDetail.UpdateTransferDetail(TransferDetail));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "TransferDetail");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    TransferDetail.CreatedUserId = userId;

                    return Json(_IDUNIT.transferDetail.AddNewTransferDetail(TransferDetail));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid transferId)
        {
            try
            {
                var result = _IDUNIT.transferDetail.GetAllTransferDetails(transferId);
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }

        public ActionResult GetUnreceivedTransferDetail([DataSourceRequest] DataSourceRequest request, Guid transferId)
        {
            try
            {
                IEnumerable<TransferDetailGridViewModel> AllTransferDetail = _IDUNIT.transferDetail.GetUnreceivedTransferDetail(transferId);
                return Json(AllTransferDetail.ToDataSourceResult(request));
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
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "TransferDetail");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                TransferDetailViewModel hos = _IDUNIT.transferDetail.GetTransferDetailForUpdate(Id);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/TransferDetail/mdTransferDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "TransferDetail");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                string oStatus = _IDUNIT.transferDetail.RemoveTransferDetail(Id);
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

        public IActionResult SelectModal()
        {
            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("CanUseWholeSellPrice", "CanUseMiddleSellPrice");

            ViewBag.AccessCanUseWholeSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
            ViewBag.AccessCanUseMiddleSellPrice = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

            return PartialView("/Views/Shared/PartialViews/AppWebForms/TransferDetail/dgTransferDetailSelectGrid.cshtml");
        }
    }
}
