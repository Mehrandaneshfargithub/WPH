using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.ReceptionInsurance;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.Models.ReceptionServiceReceived;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReceptionServiceReceived
{
    [SessionCheck]
    public class ReceptionServiceReceivedController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReceptionServiceReceivedController> _logger;


        public ReceptionServiceReceivedController(IDIUnit dIUnit, ILogger<ReceptionServiceReceivedController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PayService(ReceptionServiceReceivedViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "Cash");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }
                viewModel.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "InvoiceNumAndPayerNameRequired").FirstOrDefault();

                var InvoiceNumAndPayerNameRequired = (sval?.SValue == null) ? bool.Parse("false") : bool.Parse(sval.SValue.ToLower());

                var result = _IDUNIT.receptionServiceReceived.PayService(viewModel, InvoiceNumAndPayerNameRequired);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetAllReceptionInsuranceReceived([DataSourceRequest] DataSourceRequest request, ReceptionInsuranceViewModel receptionI)
        {
            try
            {
                IEnumerable<ReceptionInsuranceReceivedViewModel> AllRoomItem = _IDUNIT.receptionServiceReceived.GetAllReceptionInsuranceReceived(receptionI.ReceptionId ?? Guid.Empty);
                return Json(AllRoomItem.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayAllServices(PayAllServiceViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "Cash");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                viewModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "InvoiceNumAndPayerNameRequired").FirstOrDefault();

                var InvoiceNumAndPayerNameRequired = (sval?.SValue == null) ? bool.Parse("false") : bool.Parse(sval.SValue.ToLower());
                var result = _IDUNIT.receptionServiceReceived.PayAllServices(viewModel, InvoiceNumAndPayerNameRequired);
                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnInsurance(PayAllServiceViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "Cash");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                viewModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                _IDUNIT.receptionInsuranceReceived.ReturnInsurance(viewModel);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public IActionResult PayInstallment(PayAllServiceViewModel viewModel)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("New", "UserPortion");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                viewModel.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                _IDUNIT.receptionServiceReceived.PayInstallment(viewModel);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }

        public ActionResult GetAllReceptionServiceRecievedForInstallment([DataSourceRequest] DataSourceRequest request, Guid ReceptionId)
        {
            try
            {
                IEnumerable<ReceptionServiceReceivedViewModel> AllClinicSection = _IDUNIT.receptionServiceReceived.GetAllReceptionServiceRecievedForInstallment(ReceptionId);
                return Json(AllClinicSection.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                return Json(0);
            }

        }


        public JsonResult RemoveInstallment(Guid Id)
        {
            try
            {
                //var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "Customer");
                //if (!access)
                //{
                //    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                //                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                //                           "\t Message: AccessDenied");
                //    return Json("");
                //}

                OperationStatus oStatus = _IDUNIT.receptionServiceReceived.RemoveInstallment(Id);
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
    }
}
