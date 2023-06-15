using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Chart;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Dashboard
{
    public class DashboardController : Controller
    {

        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDIUnit dIUnit, ILogger<DashboardController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }


        public ActionResult Form()
        {

            var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Reserve", "TotalReserves", "Visit", "TotalVisits", "Patient", "NewAnalysisReception", "AllReception", "Patient", "NewNormalReception", "AllSurgeries", "Room", "HospitalPatient", "Children", "Service");
            ViewBag.AccessReserve = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Reserve");
            ViewBag.AccessTotalReserves = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TotalReserves");
            ViewBag.AccessVisit = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Visit");
            ViewBag.AccessTotalVisits = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TotalVisits");
            ViewBag.AccessNewReception = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "NewAnalysisReception");
            ViewBag.AccessAllReception = access1.Any(p => p.AccessName == "View" && p.SubSystemName == "AllReception");
            ViewBag.AccessPatient = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Patient");

            ViewBag.AccessNewNormalReception = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "NewNormalReception");
            ViewBag.AccessAllSurgeries = access1.Any(p => p.AccessName == "View" && p.SubSystemName == "AllSurgeries");
            ViewBag.AccessRoom = access1.Any(p => p.AccessName == "New" && p.SubSystemName == "Room");
            ViewBag.AccessHospitalPatient = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "HospitalPatient");
            ViewBag.AccessChildren = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Children");
            ViewBag.AccessService = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "Service");

            if (HttpContext.Session.GetString("SectionTypeName") == "Hospital")
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Home/HospitalDashboard.cshtml");
            }
            else if (HttpContext.Session.GetString("SectionTypeName") == "Store")
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Home/StoreDashboard.cshtml");
            }
            else if (HttpContext.Session.GetString("SectionTypeName") == "Clinic")
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Home/ClinicDashboard.cshtml");
            }
            else if (HttpContext.Session.GetString("SectionTypeName") == "Lab" || HttpContext.Session.GetString("SectionTypeName") == "Rad")
            {
                return PartialView("/Views/Shared/PartialViews/AppWebForms/Home/LabDashboard.cshtml");
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetMostSaledProducts()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                PieChartViewModel product = _IDUNIT.product.GetMostSaledProducts(clinicSectionId);
                return Json(product);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult GetExpiredList(string Type)
        {
            try
            {
                //Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                //var product = _IDUNIT.product.GetExpiredList(clinicSectionId, Type);
                //return Json(product);

                return PartialView("/Views/Shared/PartialViews/AppWebForms/Product/dgExpiredProductGrid.cshtml", Type);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetProductStocks()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                PieChartViewModel product = _IDUNIT.product.GetProductStocks(clinicSectionId);
                return Json(product);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetExpiredProducts()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var product = _IDUNIT.product.GetExpiredProducts(clinicSectionId);
                return Json(product);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetMostReturnedProducts()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                PieChartViewModel product = _IDUNIT.returnSaleInvoiceDetail.GetMostReturnedProducts(clinicSectionId);
                return Json(product);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetMostUsedMedicine()
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var medicines = _IDUNIT.medicine.GetMostUsedMedicine(ClinicSectionId);
                return Json(medicines);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetMostUsedDisease()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var medicines = _IDUNIT.disease.GetMostUsedDisease(UserId);

                

                return Json(medicines);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetMostUsedAnalysis()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var medicines = _IDUNIT.patientReceptionAnalysis.GetMostUsedAnalysis(UserId);
                return Json(medicines);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetMostOperations()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var medicines = _IDUNIT.receptionService.GetMostOperations(UserId);
                return Json(medicines);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public JsonResult GetPatientsCount()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var patinets = _IDUNIT.reception.GetReceptionCount(UserId);
                return Json(patinets);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetNearestOperations()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var patinets = _IDUNIT.surgery.GetNearestOperations(UserId);
                return Json(patinets);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllClinicInCome(string Type)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var patinets = _IDUNIT.receptionServiceReceived.GetAllClinicInCome(UserId, Type);

                return Json(patinets);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: " + e.Message);
                return Json(0);
            }
        }


        public JsonResult GetAllStoreInCome(string Type)
        {
            try
            {
                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var incomes = _IDUNIT.saleInvoice.GetAllStoreInCome(ClinicSectionId, Type);

                return Json(incomes);
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
