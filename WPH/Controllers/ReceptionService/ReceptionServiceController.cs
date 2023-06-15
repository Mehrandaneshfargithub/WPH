using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.ReceptionInsurance;
using WPH.Models.ReceptionService;
using WPH.MvcMockingServices;

namespace WPH.Controllers.ReceptionService
{
    [SessionCheck]
    public class ReceptionServiceController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ReceptionServiceController> _logger;
        protected readonly IWebHostEnvironment HostingEnvironment;

        public ReceptionServiceController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ReceptionServiceController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
            HostingEnvironment = hostingEnvironment;
        }

        public ActionResult Form(Guid receptionId)
        {
            try
            {
                string userName = "";
                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.roomBed.GetModalsViewBags(ViewBag);

                ViewBag.AccessNewCash = _IDUNIT.subSystem.CheckUserAccess("New", "Cash");

                return View("/Views/Shared/PartialViews/AppWebForms/ReceptionService/wpReceptionService.cshtml", receptionId);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message); throw e; }

            
        }
        public async Task<ActionResult> EditModalAsync(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "Cash");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                ReceptionServiceViewModel hos = _IDUNIT.receptionService.GetReceptionService(Id);

                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                var sval = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "UseDollar", "InvoiceNumAndPayerNameRequired");
                ViewBag.useDollar = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "UseDollar")?.SValue ?? "false");
                ViewBag.InvoiceNumAndPayerNameRequired = bool.Parse(sval?.FirstOrDefault(p => p.ShowSName == "InvoiceNumAndPayerNameRequired")?.SValue ?? "false");

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ReceptionService/mdReceptionServiceNewModal.cshtml", hos);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0); }


        }

        public async Task<ActionResult> GetAll([DataSourceRequest] DataSourceRequest request, Guid receptionId)
        {
            try
            {
                List<ReceptionServiceViewModel> AllServices = _IDUNIT.receptionService.GetReceptionServicesByReceptionId(receptionId, "false").ToList();

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("PayVisit", "PayService");
                bool viewVisit = Convert.ToBoolean(access.Any(p => p.AccessName == "View" && p.SubSystemName == "PayVisit")) ;
                bool deleteVisit = Convert.ToBoolean(access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayVisit")) ;
                bool payVisit = Convert.ToBoolean(access.Any(p => p.AccessName == "New" && p.SubSystemName == "PayVisit")) ;

                bool viewService = Convert.ToBoolean(access.Any(p => p.AccessName == "View" && p.SubSystemName == "PayService"));
                bool deleteService = Convert.ToBoolean(access.Any(p => p.AccessName == "Delete" && p.SubSystemName == "PayService"));
                bool payService = Convert.ToBoolean(access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PayService"));

                if (!viewVisit)
                {
                    AllServices.RemoveAll(a => a.ServiceName == "DoctorVisit");
                }
                else
                {
                    if (deleteVisit)
                    {
                        AllServices.ForEach(delegate (ReceptionServiceViewModel Service)
                        {
                            if (Service.ServiceName == "DoctorVisit")
                                Service.ShowDelete = true;
                        });
                    }
                    if (payVisit)
                    {
                        AllServices.ForEach(delegate (ReceptionServiceViewModel Service)
                        {
                            if (Service.ServiceName == "DoctorVisit")
                                Service.ShowPay = true;
                        });
                    }
                }
                

                if (!viewService)
                {
                    AllServices.RemoveAll(a => a.ServiceName != "DoctorVisit");
                }
                else
                {
                    if (deleteService)
                    {
                        AllServices.ForEach(delegate (ReceptionServiceViewModel Service)
                        {
                            if (Service.ServiceName != "DoctorVisit")
                                Service.ShowDelete = true;
                        });
                    }
                    if (payService)
                    {
                        AllServices.ForEach(delegate (ReceptionServiceViewModel Service)
                        {
                            if (Service.ServiceName != "DoctorVisit")
                                Service.ShowPay = true;
                        });
                    }
                }


                return Json(AllServices.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
            
        }


        public ActionResult GetAllSpeceficService([DataSourceRequest] DataSourceRequest request, Guid receptionId, string ServiceType)
        {
            try
            {
                IEnumerable<ReceptionServiceViewModel> AllRoomItem = _IDUNIT.receptionService.GetReceptionSpecificServicesByReceptionId(receptionId, ServiceType);
                return Json(AllRoomItem.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public async Task<ActionResult> GetAllReceptionProducts([DataSourceRequest] DataSourceRequest request, Guid receptionId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));

                IEnumerable<ReceptionServiceViewModel> AllReceptionServices;

                AllReceptionServices = _IDUNIT.receptionService.GetAllReceptionProducts(receptionId, "");

                return Json(AllReceptionServices.ToDataSourceResult(request));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
        }

        public ActionResult AddReceptionService(ReceptionServiceViewModel Service)
        {

            Service.ServiceDate = new DateTime(Convert.ToInt32(Service.ServiceDateYear), Convert.ToInt32(Service.ServiceDateMonth),
                Convert.ToInt32(Service.ServiceDateDay), Convert.ToInt32(Service.ServiceDateHour), Convert.ToInt32(Service.ServiceDateMin), 0);
            Service.CreatedUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Service.CreatedDate = DateTime.Now;
            try
            {
                _IDUNIT.receptionService.AddReceptionService(Service);
                return Json(1);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                
                if (e.Message == "NotEnoughProductCount")
                    return Json("NotEnoughProductCount");

                return Json(0);
            }

        }


        public JsonResult GetReceptionOperationService(Guid ReceptionId)
        {
            try
            {
                Guid Services = _IDUNIT.receptionService.GetReceptionOperationService(ReceptionId);

                return Json(Services);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
            
        }

        public JsonResult GetReceptionExceptOperationService(Guid ReceptionId)
        {
            try
            {
                ReceptionServiceViewModel Services = _IDUNIT.receptionService.GetReceptionExceptOperationService(ReceptionId);

                return Json(Services);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveReceptionService(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.receptionService.RemoveReceptionService(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDoctorWage(DoctorWageViewModel viewModel)
        {
            try
            {
                viewModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var oStatus = _IDUNIT.receptionService.AddDoctorWage(viewModel);
                return Json(oStatus);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                return Json(0);
            }
        }


        private StiReport ReceptionInsuranceReport(Guid ReceptionId)
        {
            try
            {
                StiReport report = new StiReport();
                string path = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "Reports", "VatanInsuranceReport8cm.mrt");
                report.Load(path);

                ReceptionInsuranceViewModel ReceptionInsurance = _IDUNIT.receptionInsurance.GetReceptionInsuranceWithReceiveds(ReceptionId);

                report.Dictionary.Variables["InvoiceNum"].Value = ReceptionInsurance?.ReceptionReceptionNum;
                report.Dictionary.Variables["ReceptionDate"].Value = _localizer["ReceptionDate"];
                report.Dictionary.Variables["vDate"].Value = ReceptionInsurance?.ReceptionReceptionDate.Value.Day + "/" + ReceptionInsurance?.ReceptionReceptionDate.Value.Month + "/" + ReceptionInsurance?.ReceptionReceptionDate.Value.Year;
                report.Dictionary.Variables["PatientName"].Value = _localizer["PatientName"];
                report.Dictionary.Variables["vPatientName"].Value = ReceptionInsurance?.ReceptionPatientUserName;
                report.Dictionary.Variables["Age"].Value = _localizer["Age"];
                report.Dictionary.Variables["vAge"].Value = ReceptionInsurance?.ReceptionPatientDateOfBirth.GetAge().ToString();
                report.Dictionary.Variables["Insurance"].Value = _localizer["Insurances"];

                report.Dictionary.Variables["VisitDate"].Value = _localizer["Date"];
                report.Dictionary.Variables["Price"].Value = _localizer["Price"];

                report.RegBusinessObject("SelectedAnalysis", ReceptionInsurance?.ReceptionInsuranceReceiveds);

                return report;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
            
        }


        public ActionResult PrintReceptionInsurance(Guid ReceptionId)
        {
            try
            {
                string font1 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "22_Sarchia_Baran.ttf");
                string font2 = Path.Combine(this.HostingEnvironment.WebRootPath, "Content", "assets", "fonts", "72_Sarchia_Qaisy.ttf");
                Stimulsoft.Base.StiFontCollection.AddFontFile(font1);
                Stimulsoft.Base.StiFontCollection.AddFontFile(font2);
                StiReport report = ReceptionInsuranceReport(ReceptionId);

                //StiImageExportSettings set = new StiImageExportSettings() { ImageFormat = StiImageFormat.Color, CutEdges = true, ImageResolution = 200, ImageType = StiImageType.Jpeg };
                report.Render();
                List<byte[]> allb = new List<byte[]>();

                for (int i = 0; i < report.RenderedPages.Count; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    report.ExportDocument(StiExportFormat.ImageJpeg, stream, new StiPngExportSettings()
                    {
                        PageRange = new StiPagesRange(StiRangeType.Pages, (i + 1).ToString(), i + 1),
                        MultipleFiles = true,
                        //CutEdges = true,
                        ImageResolution = 200,
                        ImageFormat = StiImageFormat.Color
                    });


                    allb.Add(stream.ToArray());
                }


                return Json(new { allb });
            }
            catch (Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message: " + e.Message);
                throw e;
            }
        }



    }
}
