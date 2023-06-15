using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.MvcMockingServices;

namespace WPH.Controllers.BaseInfo
{
    [SessionCheck]
    public class BaseInfoController : Controller
    {
        string userName = string.Empty;

        // GET: MedicinePage
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<BaseInfoController> _logger;


        public BaseInfoController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<BaseInfoController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }


        public ActionResult Form()
        {
            try
            {

                var access = _IDUNIT.subSystem.GetUserSubSystemAccess("BaseInfoSub");
                ViewBag.AccessNewBaseInfo = access.Any(p => p.AccessName == "New");
                ViewBag.AccessEditBaseInfo = access.Any(p => p.AccessName == "Edit");
                ViewBag.AccessDeleteBaseInfo = access.Any(p => p.AccessName == "Delete");

                _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
                _IDUNIT.baseInfo.GetModalsViewBags(ViewBag);
                int sectionTypeId = Convert.ToInt32(HttpContext.Session.GetString("SectionTypeId"));
                IEnumerable<BaseInfoTypeViewModel> baseInfoTypes = _IDUNIT.baseInfo.GetAllBaseInfoTypesForSpecificSection(sectionTypeId, _localizer);
                return PartialView("/Views/Shared/PartialViews/AppWebForms/BaseInfo/wpBaseInfo.cshtml", baseInfoTypes);

            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(BaseInfoViewModel baseinfo)
        {
            Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
            try
            {
                if (baseinfo.Guid != Guid.Empty)
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "BaseInfoSub");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.baseInfo.CheckRepeatedInfoBaseName(baseinfo.Name, clinicSectionId, false, baseinfo.TypeId ?? Guid.Empty, baseinfo.NameHolder))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid baseInfoId = _IDUNIT.baseInfo.UpdateBaseInfo(baseinfo);
                    return Json(baseInfoId);
                }
                else
                {
                    var access = _IDUNIT.subSystem.CheckUserAccess("New", "BaseInfoSub");
                    if (!access)
                    {
                        _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                               "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                               "\t Message: AccessDenied");
                        return Json("");
                    }

                    if (_IDUNIT.baseInfo.CheckRepeatedInfoBaseName(baseinfo.Name, clinicSectionId, true, baseinfo.TypeId ?? Guid.Empty))
                    {
                        return Json("ValueIsRepeated");
                    }
                    Guid serviceId = _IDUNIT.baseInfo.AddNewBaseInfo(baseinfo, clinicSectionId);
                    return Json(serviceId);
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public ActionResult AddNewModal()
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("New", "BaseInfoSub");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            BaseInfoViewModel baseinfo = new BaseInfoViewModel();
            return PartialView("/Views/Shared/PartialViews/AppWebForms/BaseInfo/mdBaseInfoModal.cshtml", baseinfo);
        }

        public ActionResult EditModal(Guid Id)
        {
            var access = _IDUNIT.subSystem.CheckUserAccess("Edit", "BaseInfoSub");
            if (!access)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                       "\t Message: AccessDenied");
                return Json("");
            }

            try
            {
                BaseInfoViewModel baseinfo = _IDUNIT.baseInfo.GetBaseInfo(Id);
                baseinfo.NameHolder = baseinfo.Name;
                return PartialView("/Views/Shared/PartialViews/AppWebForms/BaseInfo/mdBaseInfoModal.cshtml", baseinfo);
            }
            catch { return PartialView("/Views/Shared/PartialViews/AppWebForms/BaseInfo/mdBaseInfoModal.cshtml", new BaseInfoViewModel()); }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                var access = _IDUNIT.subSystem.CheckUserAccess("Delete", "BaseInfoSub");
                if (!access)
                {
                    _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                           "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                           "\t Message: AccessDenied");
                    return Json("");
                }

                OperationStatus oStatus = _IDUNIT.baseInfo.RemoveBaseInfo(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request, Guid Id)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> baseInfos = _IDUNIT.baseInfo.GetAllBaseInfos(Id, clinicSectionId);

                return Json(baseInfos.ToDataSourceResult(request));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }


        }

        public JsonResult AddBaseInfoType(Guid BaseInfoId, Guid BaseInfoTypeId)
        {
            try
            {
                _IDUNIT.baseInfo.AddBaseInfoTypeOfBaseInfo(BaseInfoId, BaseInfoTypeId);
                return Json(1);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }


        }

        public JsonResult GetAllBaseInfoTypes()
        {
            try
            {
                IEnumerable<BaseInfoTypeViewModel> baseInfoTypes = _IDUNIT.baseInfo.GetAllBaseInfoTypes();
                return Json(baseInfoTypes);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllDiseaseTypes()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> baseInfoGeneral = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("DiseaseType");
                return Json(baseInfoGeneral);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllPaymentStatus()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> baseInfoGeneral = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("PaymentStatus");
                return Json(baseInfoGeneral);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllSalaryTypes()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> baseInfoGeneral = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("SalaryType");
                return Json(baseInfoGeneral);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
        public JsonResult GetAllReasons()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> producers = _IDUNIT.baseInfo.GetAllBaseInfos("Reason", clinicSectionId);
                return Json(producers);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllProducers()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> producers = _IDUNIT.baseInfo.GetAllBaseInfos("Producer", clinicSectionId);
                return Json(producers);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllJobs()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Jobs = _IDUNIT.baseInfo.GetAllBaseInfos("Job", clinicSectionId);
                return Json(Jobs);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllMedicineForm()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> medicineForms = _IDUNIT.baseInfo.GetAllBaseInfos("MedicineForm", clinicSectionId);
                return Json(medicineForms);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllCurrenciesExcept(int currencyId)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoGeneralViewModel> currencies = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("Currency").Where(p => p.Id != currencyId).OrderBy(a => a.Id);

                var deffCurrency = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId")?.FirstOrDefault();
                var deffCurrencyId = Convert.ToInt32(deffCurrency?.SValue);

                if (deffCurrencyId != 0)
                {
                    var res = currencies.Where(p => p.Id == deffCurrencyId).Select(p => { p.Index = 0; return p; }).ToList();

                    res.AddRange(currencies.Where(p => p.Id != deffCurrencyId).Select(p => { p.Index = 1; return p; }));

                    return Json(res.OrderBy(a => a.Index));
                }

                return Json(currencies);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllCurrencies()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoGeneralViewModel> currencies = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("Currency").OrderBy(a => a.Id);

                var deffCurrency = _IDUNIT.clinicSection.GetClinicSectionSettingValueBySettingName(clinicSectionId, "CurrencyTypeId")?.FirstOrDefault();
                var deffCurrencyId = Convert.ToInt32(deffCurrency?.SValue);

                if (deffCurrencyId != 0)
                {
                    var res = currencies.Where(p => p.Id == deffCurrencyId).Select(p => { p.Index = 0; return p; }).ToList();

                    res.AddRange(currencies.Where(p => p.Id != deffCurrencyId).Select(p => { p.Index = 1; return p; }));

                    return Json(res.OrderBy(a => a.Index));
                }

                return Json(currencies);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllValueTypeForAnalysisItem()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> ValueTypes = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("ValueType");
                return Json(ValueTypes);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
        public JsonResult GetAllUnitsForAnalysisItem()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> UnitForAnalysisItems = _IDUNIT.baseInfo.GetAllBaseInfos("Unit", clinicSectionId);
                return Json(UnitForAnalysisItems);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllGenders()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> Genders = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("Gender");
                return Json(Genders);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
        public JsonResult GetAllUserTypes()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> Genders = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("UserType");
                return Json(Genders);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
        public JsonResult GetAllSearchHumanSalary()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> SearchHumanSalary = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("SearchHumanSalaryType");
                return Json(SearchHumanSalary);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
        public JsonResult GetAllCostTypes()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> CostTypes = _IDUNIT.baseInfo.GetAllBaseInfos("CostType", clinicSectionId);
                return Json(CostTypes);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllBloodTypes()
        {
            try
            {
                List<BaseInfoGeneralViewModel> Bloods = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("BloodType").ToList();

                return Json(Bloods);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllTests()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos("Test", clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllSpecialities()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos("DoctorSpeciality", clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllAddress()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos("Address", clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllAccessTypes()
        {

            try
            {
                List<BaseInfoGeneralViewModel> Bloods = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("AccessType").ToList();

                return Json(Bloods);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetCustomInvoiceDetailTypes()
        {

            try
            {
                List<BaseInfoGeneralViewModel> Bloods = _IDUNIT.baseInfo.GetCustomInvoiceDetailTypes("InvoiceType").ToList();

                return Json(Bloods);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllDoctorSpecialities()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos("DoctorSpeciality", clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllProductTypes()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos("ProductType", clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public JsonResult GetAllArrivals()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> baseInfoGeneral = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("ArrivalType");
                return Json(baseInfoGeneral);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllCritically()
        {
            try
            {
                IEnumerable<BaseInfoGeneralViewModel> baseInfoGeneral = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("CriticalType");
                return Json(baseInfoGeneral);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllItemTypes()
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfos("ItemType", clinicSectionId);
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllPatientHealth()
        {
            try
            {
                IEnumerable<BaseInfoViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfos("PatientHealthType");
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllRoomTypes()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("RoomType").ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllRoomStatus()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("RoomStatus").ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllClinicSectionTypes()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("ClinicSectionType").ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllServiceTypes()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("ServiceType").ToList();
                if (HttpContext.Session.GetString("SectionTypeName") != "Hospital")
                    items.RemoveAll(a => a.Name != "Other");
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllSalePriceType()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("SalePriceType").ToList();
                var access1 = _IDUNIT.subSystem.GetUserSubSystemAccess("CanUseWholeSellPrice", "CanUseMiddleSellPrice");

                var whole = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
                var middle = access1.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

                if (!whole)
                {
                    items.Remove(items.FirstOrDefault(a => a.Name == "WholePrice"));
                }

                if (!middle)
                {
                    items.Remove(items.FirstOrDefault(a => a.Name == "MiddelPrice"));
                }

                items = items.Select(p =>
                {
                    p.ShowName = _localizer[p.Name];
                    return p;
                }).ToList();

                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }


        public JsonResult GetAllServiceTypesExceptOperation()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGeneralsExcept("ServiceType", "Operation").ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetBaseInfoTypeByName(string BaseInfoName)
        {
            try
            {
                Guid items = _IDUNIT.baseInfo.GetBaseInfoTypeIdByName(BaseInfoName);
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }

        public JsonResult GetAllStockSelection()
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("StockSelectionType").ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetBaseInfoGeneralsByName(string BaseInfoGeneralName)
        {
            try
            {
                List<BaseInfoGeneralViewModel> items = _IDUNIT.baseInfo.GetAllBaseInfoGenerals(BaseInfoGeneralName).ToList();
                return Json(items);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public JsonResult GetAllBaseInfosByBaseInfoTypeName(string TypeName)
        {
            try
            {
                Guid clinicSectionId = Guid.Parse(HttpContext.Session.GetString("ClinicSectionId"));
                IEnumerable<BaseInfoViewModel> Tests = _IDUNIT.baseInfo.GetAllBaseInfos(TypeName, clinicSectionId);
                return Json(Tests);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }
        }
    }
}