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
using WPH.Models.MaterialStoreroom;
using WPH.Models.Product;
using WPH.MvcMockingServices;

namespace WPH.Controllers.MaterialStoreroom
{
    [SessionCheck]
    public class MaterialStoreroomController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<MaterialStoreroomController> _logger;

        public MaterialStoreroomController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<MaterialStoreroomController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }
        public ActionResult Form()
        {
            try
            {
                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("MaterialStoreroom");
                ViewBag.AccessNewMaterialStoreroom = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditMaterialStoreroom = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteMaterialStoreroom = access.Any(p => p.AccessName == "Delete");

                return View("/Views/Shared/PartialViews/AppWebForms/MaterialStoreroom/wpMaterialStoreroom.cshtml");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
            
        }

        public ActionResult AddAndNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "MaterialStoreroom");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            ProductViewModel product = new ProductViewModel();

            return PartialView("/Views/Shared/PartialViews/AppWebForms/MaterialStoreroom/mdMaterialStoreroomNewModal.cshtml", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(ProductViewModel product)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                product.ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                product.MaterialTypeId = _IDUNIT.baseInfo.GetIdByNameAndType("Material", "MaterialType");

                if (product.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "MaterialStoreroom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    product.ModifiedUserId = userId;
                    return Json(_IDUNIT.product.UpdateProduct(product));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "MaterialStoreroom");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("AccessDenied");
                    }

                    product.CreateUserId = userId;
                    return Json(_IDUNIT.product.AddNewProductWithOutBarcode(product));
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

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid clinicSectionId, Guid originalClinicSectionId, string ClinicSectionName)
        {
            try
            {
                //Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<MaterialStoreroomViewModel> products;

                //if (ClinicSectionName == "Main")
                //{
                //    products = _IDUNIT.product.GetAllClinicSectionsMaterialProducts(originalClinicSectionId, clinicSectionId);
                //}
                //else
                //{
                    products = _IDUNIT.product.GetAllMaterialProducts(originalClinicSectionId, clinicSectionId);
                //}

                //IEnumerable<MaterialStoreroomViewModel> AllMaterialStoreroom = _IDUNIT.product.GetAllMaterialProducts(originalClinicSectionId, clinicSectionId);
                return Json(products.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public ActionResult EditModal(Guid MasterId, Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "MaterialStoreroom");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("AccessDenied");
            }

            try
            {
                ProductViewModel hos = _IDUNIT.product.GetProduct(MasterId,Id);
                hos.NameHolder = hos.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/MaterialStoreroom/mdMaterialStoreroomNewModal.cshtml", hos);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "MaterialStoreroom");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("AccessDenied");
                }

                OperationStatus oStatus = _IDUNIT.product.RemoveProduct(Id);
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

        public ActionResult MaterialProductHistoryModal(Guid Id)
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/MaterialStoreroom/dgMaterialProductHistoryGrid.cshtml", Id);
        }

        public ActionResult GetMaterialProductHistory([DataSourceRequest] DataSourceRequest request, Guid clinicSectionId, Guid productId)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<MaterialProductHistoryViewModel> AllMaterialStoreroom = _IDUNIT.product.GetAllMaterialProductHistory(originalClinicSectionId, clinicSectionId, productId);
                return Json(AllMaterialStoreroom.ToDataSourceResult(request));
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
