using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WPH.MvcMockingServices;
using WPH;
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.BaseInfo;

namespace WPH.Controllers.ClinicSectionChoosenValue
{
    public class ClinicSectionChoosenValueController : Controller
    {
        string userName = string.Empty;
        Guid clinicSectionId = Guid.Empty;

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;

        public ClinicSectionChoosenValueController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
        }


        public ActionResult Form()
        {
            var access = _IDUNIT.subSystem.GetUserSubSystemAccess("Children");
            ViewBag.AccessNewChild = access.Any(p => p.AccessName == "New");
            ViewBag.AccessEditChild = access.Any(p => p.AccessName == "Edit");
            ViewBag.AccessDeleteChild = access.Any(p => p.AccessName == "Delete");
            ViewBag.AccessPrintChild = access.Any(p => p.AccessName == "Print");

            string userName = "";
            _IDUNIT.setting.GetRightToLeftSettings(ViewBag, userName);
            _IDUNIT.child.GetModalsViewBags(ViewBag);
            return View("/Views/Shared/PartialViews/AppWebForms/Child/wpChild.cshtml");
        }




        public ActionResult ClinicSectionChoosenValueModal()
        {
            
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicSectionChoosenValue/mdClinicSectionChoosenValueModal.cshtml");

        }

        




        public ActionResult GetAllPatientVariables()
        {
            //IEnumerable<PatientVariableViewModel> AllVariables = _IDUNIT.patientVariable.GetAllPatientVariables().OrderBy(x => x.VariableName);

            //IEnumerable < ClinicSectionChoosenValueViewModel > AllChoosenVariables = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(clinicSectionId);
            //foreach (PatientVariableViewModel pv in AllVariables)
            //{
            //    ClinicSectionChoosenValueViewModel c = AllChoosenVariables.SingleOrDefault(x => x.PatientVariableId == pv.Id);
            //    if (c != null)
            //    {
            //        pv.Value = true;
            //    }
            //    else
            //        pv.Value = false;
            //}

            //ViewBag.AllPatientVariables = AllVariables;
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicSectionChoosenValue/wpAllPatientVariablesForm.cshtml");

        }


        public ActionResult GetAllChoosenVariablesValues()
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/ClinicSectionChoosenValue/dgClinicSectionChoosenValueGrid.cshtml");

        }


        public JsonResult AddClinicSectionChoosenValues(IEnumerable<ClinicSectionChoosenValueViewModel> ClinicSectionChoosenValue)
        {
            try
            {

                _IDUNIT.clinicSectionChoosenValue.AddClinicSectionChoosenValues(ClinicSectionChoosenValue, clinicSectionId);
                return Json(1);
                
            }
            catch (Exception ex) { return Json(0); }
        }




        public ActionResult GetAllChoosenVariables([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ClinicSectionChoosenValueViewModel> AllClinicSectionChoosenValue = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(clinicSectionId).OrderBy(x=>x.Priority);
            foreach (ClinicSectionChoosenValueViewModel cs in AllClinicSectionChoosenValue)
            {
                if (cs.VariableDisplayName == "ShowInVisit")
                    cs.ShowInVisit = true;
                else if(cs.VariableDisplayName == "ShowInPatient")
                    cs.ShowInPatient = true;
                else if(cs.VariableDisplayName == "ShowInVisitAndPatient")
                {
                    cs.ShowInVisit = true;
                    cs.ShowInPatient = true;
                }
                if (cs.VariableStatusName == "VariableValueIsConstant")
                    cs.VariableChangePerVisit = false;
                else if (cs.VariableStatusName == "VariableValueIsVariable")
                    cs.VariableChangePerVisit = true;
                
            }


            return Json(AllClinicSectionChoosenValue.ToDataSourceResult(request));
        }

        public JsonResult ChangePriority(Guid Id, string Priority)
        {
            try
            {
                ClinicSectionChoosenValueViewModel newCli = _IDUNIT.clinicSectionChoosenValue.GetClinicSectionChoosenValueById(Id);
                IEnumerable<ClinicSectionChoosenValueViewModel> all = _IDUNIT.clinicSectionChoosenValue.GetAllClinicSectionChoosenValues(clinicSectionId);

                if (Priority == "de")
                {

                    if (newCli.Priority == all.Count())
                    {
                        return Json(0);
                    }
                    else
                    {
                        newCli.Priority++;
                    }
                    ClinicSectionChoosenValueViewModel oldSlide = all.SingleOrDefault(x => x.Priority == newCli.Priority);
                    oldSlide.Priority--;
                    _IDUNIT.clinicSectionChoosenValue.UpdateClinicSectionChoosenValue(newCli);
                    _IDUNIT.clinicSectionChoosenValue.UpdateClinicSectionChoosenValue(oldSlide);

                }
                else
                {

                    if (newCli.Priority == 1)
                    {
                        return Json(0);
                    }
                    else
                    {
                        newCli.Priority--;
                    }
                    ClinicSectionChoosenValueViewModel oldSlide = all.SingleOrDefault(x => x.Priority == newCli.Priority);
                    oldSlide.Priority++;
                    _IDUNIT.clinicSectionChoosenValue.UpdateClinicSectionChoosenValue(newCli);
                    _IDUNIT.clinicSectionChoosenValue.UpdateClinicSectionChoosenValue(oldSlide);
                }
                return Json(1);

            }
            catch (Exception ex) { return Json(0); }

        }



        [AcceptVerbs]
        public ActionResult ChoosenVariables_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ClinicSectionChoosenValueViewModel> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    IEnumerable<BaseInfoGeneralViewModel> VariableDisplay = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("VariableDisplay");
                    int ShowInVisit = VariableDisplay.SingleOrDefault(x => x.Name == "ShowInVisit").Id;
                    int ShowInPatient = VariableDisplay.SingleOrDefault(x => x.Name == "ShowInPatient").Id;
                    int ShowInVisitAndPatient = VariableDisplay.SingleOrDefault(x => x.Name == "ShowInVisitAndPatient").Id;
                    IEnumerable<BaseInfoGeneralViewModel> VariableStatus = _IDUNIT.baseInfo.GetAllBaseInfoGenerals("VariableStatus");
                    int VariableValueIsVariable = VariableStatus.SingleOrDefault(x => x.Name == "VariableValueIsVariable").Id;
                    int VariableValueIsConstant = VariableStatus.SingleOrDefault(x => x.Name == "VariableValueIsConstant").Id;
                    if (product.ShowInPatient)
                        product.VariableDisplayId = ShowInPatient;
                    if(product.ShowInVisit)
                        product.VariableDisplayId = ShowInVisit;
                    if(product.ShowInPatient && product.ShowInVisit)
                        product.VariableDisplayId = ShowInVisitAndPatient;
                    if (product.VariableChangePerVisit)
                        product.VariableStatusId = VariableValueIsVariable;
                    else
                        product.VariableStatusId = VariableValueIsConstant;
                    _IDUNIT.clinicSectionChoosenValue.UpdateClinicSectionChoosenValue(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs]
        public ActionResult ChoosenVariables_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ClinicSectionChoosenValueViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    _IDUNIT.clinicSectionChoosenValue.RemoveClinicSectionChoosenValue(product.Guid);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                //OperationStatus oStatus = _ClinicSectionChoosenValueMvcService.rem(Id);
                return Json(1);
            }
            catch { return Json(0); }
        }

        
    }
}