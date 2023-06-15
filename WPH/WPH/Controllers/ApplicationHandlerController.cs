using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices;

namespace WPH.Controllers
{
    [SessionCheck]
    public class ApplicationHandlerController : Controller
    {
        string userName = string.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<ApplicationHandlerController> _logger;
        public ApplicationHandlerController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<ApplicationHandlerController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Index(Guid? clinicSectionId)
        {
            try
            {
                List<SubSystemViewModel> menuList = new List<SubSystemViewModel>();
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                Guid ClinicSectionId = clinicSectionId ?? Guid.Empty;
                UserInformationViewModel user = _IDUNIT.user.GetUserWithAccess(UserId);
                HttpContext.Session.SetString("ClinicSectionId", ClinicSectionId.ToString());
                HttpContext.Session.SetString("UserAccessType", user.AccessTypeName.ToString());


                var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var culture = rqf.RequestCulture.Culture.Name;

                HttpContext.Session.SetString("culture", culture);
                Guid? userClinicSection = ClinicSectionId;

                ClinicSectionViewModel cs = _IDUNIT.clinicSection.GetClinicSectionById(userClinicSection ?? Guid.Empty);
                ViewBag.ClinicSectionName = cs.Name;
                ViewBag.NameOfUser = user.Name;


                HttpContext.Session.SetString("SectionTypeId", cs.SectionTypeId.ToString());
                HttpContext.Session.SetString("SectionTypeName", cs.SectionTypeName);
                HttpContext.Session.SetString("ClinicSectionName", cs.Name);
                HttpContext.Session.SetString("ParentId", cs.ParentId.GetValueOrDefault().ToString());
                HttpContext.Session.SetString("CSId", cs.Id.ToString());
                HttpContext.Session.SetString("ClinicId", cs.ClinicId.ToString());
                HttpContext.Session.SetString("AccessTypeName", user.AccessTypeName);



                ViewBag.SectionType = cs.SectionTypeName;
                _IDUNIT.setting.loadSettings(UserId);
                ViewBag.FullAccess = user.AccessTypeName;
                if (userName == "mehran" || user.AccessTypeName == "FullAccessClinicAdmin")
                {
                    menuList = _IDUNIT.subSystem.GetAllSubSystemsForDev(cs.SectionTypeId??39).OrderBy(x => x.Priority).ToList();

                    foreach (var menu in menuList)
                    {
                        menu.ShowName = _localizer[menu.ShowName];
                    }

                }
                else
                {
                    menuList = _IDUNIT.subSystem.GetMenuItems(UserId, cs.SectionTypeId ?? 39, cs.Guid).OrderBy(x => x.Priority).ToList();
                    foreach (var menu in menuList)
                    {
                        menu.ShowName = _localizer[menu.ShowName];
                    }
                }
                _logger.LogInformation("The main page has been accessed");

                ViewBag.UnReadMessage = _IDUNIT.reminder.GetUnReadCount(ClinicSectionId, DateTime.Now);

                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("Reserve", "TotalReserves", "Visit", "TotalVisits", "Patient",
                    "NewAnalysisReception", "AllReception", "Patient", "NewNormalReception", "AllSurgeries", "Room", "HospitalPatient", 
                    "Children", "Service", "DamageDetails", "PurchaseInvoiceDetails", "TransferDetail", "ReturnSaleInvoiceDetails", "ReturnPurchaseInvoiceDetails", "SaleInvoiceDetails");
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
                ViewBag.AccessDamageDetails = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "DamageDetails");
                ViewBag.AccessPurchaseInvoiceDetails = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "PurchaseInvoiceDetails");
                ViewBag.AccessTransferDetail = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "TransferDetail");
                ViewBag.AccessReturnSaleInvoiceDetails = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnSaleInvoiceDetails");
                ViewBag.AccessReturnPurchaseInvoiceDetails = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "ReturnPurchaseInvoiceDetails");
                ViewBag.AccessSaleInvoiceDetails = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "SaleInvoiceDetails");
                return View("/Views/ApplicationHandler/Index.cshtml", menuList);
            }
            catch (Exception ex) { return null; }
        }


        public ActionResult ChooseClinicSection()
        {

            try
            {

                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

                Guid ClinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                Guid? ParentId = Guid.Parse(HttpContext.Session.GetString("ParentId"));

                Guid? CS = Guid.Empty;

                if (ParentId == Guid.Empty)
                {
                    CS = ClinicSectionId;

                }
                else
                {
                    CS = ParentId;
                }

                List<ClinicSectionViewModel> AllClinicSections = _IDUNIT.clinicSection.GetClinicSectionsForUser(UserId, "", CS);
                AllClinicSections.Add(_IDUNIT.clinicSection.GetClinicSectionById(CS ?? Guid.Empty));
                AllClinicSections.Remove(AllClinicSections.SingleOrDefault(a => a.Guid == ClinicSectionId));
                ViewBag.clinicSectionNames = AllClinicSections;

                return PartialView("/Views/Shared/PartialViews/AppWebForms/ChooseClinicSection/ChooseClinicSectionModal.cshtml");
                //return Json(AllClinicSections);
            }
            catch { return Json(0); }


        }

        //public ActionResult ChooseClinicSection(string clinicSections)
        //{
        //    try
        //    {
        //        string[] Ids = clinicSections.Split(',');

        //        List<Guid> clinicSectionIds = new List<Guid>();

        //        for(int i=0;i< Ids.Length; i++)
        //        {
        //            clinicSectionIds.Add(new Guid(Ids[i]));
        //        }

        //        ViewBag.clinicSectionNames = _IDUNIT.clinicSection.GetClinicSectionsByIds(clinicSectionIds);
        //        //List<ClinicSectionViewModel> clinicSectionNames = _clinicSectionMvcService.GetClinicSectionsByIds(clinicSectionIds);


        //        return PartialView("/Views/Shared/PartialViews/AppWebForms/ChooseClinicSection/ChooseClinicSectionModal.cshtml");
        //    }
        //    catch (Exception ex) { return null; }
        //}


        public ActionResult ClinicAdmin()
        {
            try
            {
                List<SubSystemViewModel> menuList = new List<SubSystemViewModel>();
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                UserInformationViewModel user = _IDUNIT.user.GetUserWithAccess(UserId);
                //HttpContext.Session.SetString("ClinicId", user.ClinicId.ToString());
                HttpContext.Session.SetString("UserAccessType", user.AccessTypeName.ToString());

                _IDUNIT.setting.loadSettings(UserId);
                if (user.UserName == "Mehran")
                {

                    menuList.AddRange(_IDUNIT.subSystem.GetSubSystemByName("Clinic"));
                    menuList.AddRange(_IDUNIT.subSystem.GetSubSystemByName("ClinicManagment"));
                }
                else
                {
                    menuList = _IDUNIT.subSystem.GetSubSystemByName("UserManagment");
                }

                List<SubSystemViewModel> translatedSubSystem = new List<SubSystemViewModel>();
                ViewBag.FullAccess = user.AccessTypeName;

                return View("/Views/ApplicationHandler/Index.cshtml", menuList);
            }
            catch (Exception ex) { return null; }
        }


    }


}