using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.SaleInvoiceDetail;
using WPH.MvcMockingServices;

namespace WPH.Controllers.SaleInvoiceDetail
{
    [SessionCheck]
    public class SaleInvoiceDetailController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<SaleInvoiceDetailController> _logger;

        public SaleInvoiceDetailController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<SaleInvoiceDetailController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form(Guid saleInvoiceId)
        {
            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("SaleInvoiceDetails");
            ViewBag.AccessNewSaleInvoiceDetail = access.Any(p => p.AccessName == "New");
            ViewBag.AccessEditSaleInvoiceDetail = access.Any(p => p.AccessName == "Edit");
            ViewBag.AccessDeleteSaleInvoiceDetail = access.Any(p => p.AccessName == "Delete");

            string userName = "";
            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            _IDUNIT.saleInvoiceDetail.GetModalsViewBags(ViewBag);
            return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDetail/wpSaleInvoiceDetail.cshtml", saleInvoiceId);
        }


        public JsonResult AddOrUpdate(SaleInvoiceDetailViewModel SaleInvoiceDetail)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                SaleInvoiceDetail.ClinicSectionId = clinicSectionId;


                if (string.Compare(SaleInvoiceDetail.InvoiceType, "PurchaseInvoiceDetail", true) != 0)
                {
                    SaleInvoiceDetail.TransferDetailId = SaleInvoiceDetail.PurchaseInvoiceDetailId;
                    SaleInvoiceDetail.PurchaseInvoiceDetailId = null;
                }

                if(SaleInvoiceDetail.SalePrice == null)
                {
                    SaleInvoiceDetail.SalePriceTxt.Replace(',', '.');
                    SaleInvoiceDetail.SalePrice = decimal.Parse(SaleInvoiceDetail.SalePriceTxt, CultureInfo.InvariantCulture);
                }

                if (SaleInvoiceDetail.Discount == null)
                {
                    SaleInvoiceDetail.DiscountTxt.Replace(',', '.');
                    SaleInvoiceDetail.Discount = decimal.Parse(SaleInvoiceDetail.DiscountTxt, CultureInfo.InvariantCulture);
                }

                if (SaleInvoiceDetail.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "SaleInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    SaleInvoiceDetail.ModifiedUserId = userId;
                    SaleInvoiceDetail.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.saleInvoiceDetail.UpdateSaleInvoiceDetail(SaleInvoiceDetail));
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "SaleInvoiceDetails");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }
                    SaleInvoiceDetail.CreatedUserId = userId;
                    SaleInvoiceDetail.ModifiedUserId = userId;
                    SaleInvoiceDetail.CreatedDate = DateTime.Now;
                    SaleInvoiceDetail.ModifiedDate = DateTime.Now;

                    return Json(_IDUNIT.saleInvoiceDetail.AddNewSaleInvoiceDetail(SaleInvoiceDetail));
                }
            }
            catch (Exception e) { return Json(0); }
        }


        public ActionResult GetAll(Guid saleInvoiceId)
        {
            IEnumerable<SaleInvoiceDetailViewModel> AllSaleInvoiceDetail = _IDUNIT.saleInvoiceDetail.GetAllSaleInvoiceDetails(saleInvoiceId);

            return Json(AllSaleInvoiceDetail);
        }

        public ActionResult GetAllDetail(string saleInvoiceDetailIds)
        {
            string[] all = saleInvoiceDetailIds.Split(',');
            List<Guid> allgu = new List<Guid>();
            for (int i = 0; i < all.Length; i++)
            {
                allgu.Add(Guid.Parse(all[i]));
            }

            IEnumerable<SaleInvoiceDetailViewModel> AllSaleInvoiceDetail = _IDUNIT.saleInvoiceDetail.GetAllDetail(allgu);

            return Json(AllSaleInvoiceDetail);
        }
       
        public ActionResult CurrencyModal(int Id, string Name, int Number)
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/SaleInvoiceDetail/SaleInvoiceDetailRecieveCurrency.cshtml",new BaseInfoGeneralViewModel() { Id = Id,Name = Name,Index = 12/ Number });
            }
            catch { return Json(0); }
        }

        public JsonResult Remove(string Guids)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "SaleInvoiceDetails");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                string[] all = Guids.Split(',');
                List<Guid> allgu = new List<Guid>();
                for (int i = 0; i < all.Length; i++)
                {
                    allgu.Add(Guid.Parse(all[i]));
                }

                var oStatus = _IDUNIT.saleInvoiceDetail.RemoveSaleInvoiceDetail(allgu);
                return Json(oStatus.ToString());
            }
            catch { return Json(0); }
        }

        public ActionResult GetDetailsForReturn([DataSourceRequest] DataSourceRequest request, Guid masterId, Guid productId, bool like)
        {
            try
            {
                Guid originalClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var result = _IDUNIT.saleInvoiceDetail.GetDetailsForReturn(masterId, productId, originalClinicSectionId, like);
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
