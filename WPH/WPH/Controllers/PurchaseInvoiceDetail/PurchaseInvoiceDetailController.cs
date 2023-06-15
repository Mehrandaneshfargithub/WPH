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
using WPH.Models.PurchaseInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PurchaseInvoiceDetail
{
    [SessionCheck]
    public class PurchaseInvoiceDetailController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PurchaseInvoiceDetailController> _logger;

        public PurchaseInvoiceDetailController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PurchaseInvoiceDetailController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public JsonResult AddOrUpdateMaterial(PurchaseInvoiceDetailViewModel purchaseInvoiceDetail)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                purchaseInvoiceDetail.PurchaseType = "Material";
                if (purchaseInvoiceDetail.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    purchaseInvoiceDetail.ModifiedUserId = userId;
                    string a = _IDUNIT.purchaseInvoiceDetail.UpdatePurchaseInvoiceDetail(purchaseInvoiceDetail, clinicSectionId);
                    return Json(a);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    purchaseInvoiceDetail.CreatedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoiceDetail.AddNewPurchaseInvoiceDetail(purchaseInvoiceDetail, clinicSectionId));
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
        
        public JsonResult AddOrUpdate(PurchaseInvoiceDetailViewModel purchaseInvoiceDetail)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                purchaseInvoiceDetail.PurchaseType = "Medicine";
                if (purchaseInvoiceDetail.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "PurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    purchaseInvoiceDetail.ModifiedUserId = userId;
                    string a = _IDUNIT.purchaseInvoiceDetail.UpdatePurchaseInvoiceDetail(purchaseInvoiceDetail, clinicSectionId);
                    return Json(a);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "PurchaseInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    purchaseInvoiceDetail.CreatedUserId = userId;

                    return Json(_IDUNIT.purchaseInvoiceDetail.AddNewPurchaseInvoiceDetail(purchaseInvoiceDetail, clinicSectionId));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid purchaseInvoiceId)
        {
            try
            {
                IEnumerable<PurchaseInvoiceDetailViewModel> AllPurchaseInvoiceDetail = _IDUNIT.purchaseInvoiceDetail.GetAllPurchaseInvoiceDetails(purchaseInvoiceId);
                return Json(AllPurchaseInvoiceDetail.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }

        }

        public ActionResult AddMaterialModal()
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "MaterialStoreroom");
            var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                PurchaseInvoiceDetailViewModel hos = new PurchaseInvoiceDetailViewModel
                {
                    NumTxt = "1",
                    PurchasePriceTxt = "0",
                    WholePurchasePriceTxt = "0",
                    DiscountTxt = "0",
                };

                ViewBag.AccessNewStoreroom = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "MaterialStoreroom");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetail/mdMaterialPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }
        
        public ActionResult AddModal()
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "SubStoreroom", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "CanUseNetPrice");
            var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                PurchaseInvoiceDetailViewModel hos = new PurchaseInvoiceDetailViewModel
                {
                    NumTxt = "1",
                    FreeNumTxt = "0",
                    PurchasePriceTxt = "0",
                    WholePurchasePriceTxt = "0",
                    DiscountTxt = "0",
                    SellingPriceTxt = "0",
                    WholeSellPriceTxt = "0",
                    MiddleSellPriceTxt = "0",
                };

                ViewBag.AccessNewStoreroom = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");
                ViewBag.CanUseWholeSellPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
                ViewBag.CanUseMiddleSellPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");
                ViewBag.CanUseNetPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseNetPrice");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetail/mdPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public ActionResult EditMaterialModal(Guid Id)
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "MaterialStoreroom");
            var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                PurchaseInvoiceDetailViewModel hos = _IDUNIT.purchaseInvoiceDetail.GetPurchaseInvoiceDetailForEdit(Id);

                ViewBag.PurchasePaid = _IDUNIT.purchaseInvoice.CheckPurchaseInvoicePaid(hos.MasterId.Value);
                ViewBag.AccessNewStoreroom = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "MaterialStoreroom");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetail/mdMaterialPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }
        
        public ActionResult EditModal(Guid Id)
        {
            var _access = _IDUNIT.subSystem.GetUserSubSystemAccess("PurchaseInvoiceDetails", "SubStoreroom", "CanUseWholeSellPrice", "CanUseMiddleSellPrice", "CanUseNetPrice");
            var access = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "PurchaseInvoiceDetails");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                PurchaseInvoiceDetailViewModel hos = _IDUNIT.purchaseInvoiceDetail.GetPurchaseInvoiceDetailForEdit(Id);

                ViewBag.PurchasePaid = _IDUNIT.purchaseInvoice.CheckPurchaseInvoicePaid(hos.MasterId.Value);
                ViewBag.AccessNewStoreroom = _access.Any(p => p.AccessName == "New" && p.SubSystemName == "SubStoreroom");
                ViewBag.CanUseWholeSellPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
                ViewBag.CanUseMiddleSellPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");
                ViewBag.CanUseNetPrice = _access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseNetPrice");
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PurchaseInvoiceDetail/mdPurchaseInvoiceDetailNewModal.cshtml", hos);
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "PurchaseInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                var oStatus = _IDUNIT.purchaseInvoiceDetail.RemovePurchaseInvoiceDetail(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public JsonResult GetPurchaseInvoiceDetailSalePrice(Guid PurchaseInvoiceDetailId,int CurrencyId,string PriceType,string SaleType)
        {
            try
            {
                var oStatus = _IDUNIT.purchaseInvoiceDetail.GetPurchaseInvoiceDetailSalePrice(PurchaseInvoiceDetailId, CurrencyId, PriceType, SaleType);
                return Json(oStatus);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetPurchaseHistory([DataSourceRequest] DataSourceRequest request, Guid productId)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var result = _IDUNIT.purchaseInvoiceDetail.GetPurchaseHistory(originalClinicSectionId, productId);
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

        public ActionResult GetDetailsForReturn([DataSourceRequest] DataSourceRequest request, Guid masterId, Guid productId, bool like)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var result = _IDUNIT.purchaseInvoiceDetail.GetDetailsForReturn(masterId, productId, originalClinicSectionId, like);
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

    }
}
