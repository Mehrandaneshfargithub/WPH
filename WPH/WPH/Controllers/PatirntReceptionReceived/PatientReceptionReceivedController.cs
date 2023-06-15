using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientReceptionReceived;
using WPH.MvcMockingServices;

namespace WPH.Controllers.PatientReceptionReceived
{
    [SessionCheck]
    public class PatientReceptionReceivedController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<PatientReceptionReceivedController> _logger;

        public PatientReceptionReceivedController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<PatientReceptionReceivedController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {

            return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReceptionReceived/wpPatientReceptionReceived.cshtml");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(PatientReceptionReceivedViewModel PatientReceptionReceived)
        {
            try
            {

                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                PatientReceptionReceived.CreatedDate = DateTime.Now;
                PatientReceptionReceived.CreatedUserId = UserId;
                _IDUNIT.patientReceptionReceived.AddNewPatientReceptionReceived(PatientReceptionReceived);
                return Json(1);
                //}
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                throw e;
            }
        }


        public ActionResult AddNewModal()
        {
            try
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/PatientReceptionReceived/mdPatientReceptionReceivedNewModal.cshtml");
            }
            catch (Exception e) { throw e; }
        }



        //[HttpPost]
        //public JsonResult Remove(Guid Id)
        //{
        //    try
        //    {
        //        OperationStatus oStatus = _IDUNIT.patientReceptionReceived.RemovePatientReceptionReceived(Id);
        //        return Json(oStatus.ToString());
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
        //                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
        //                               "\t Message: " + e.Message);
        //        throw e;
        //    }
        //}


    }
}