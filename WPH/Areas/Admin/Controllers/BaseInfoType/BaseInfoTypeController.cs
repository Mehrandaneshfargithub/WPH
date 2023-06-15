using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.BaseInfoType;
using WPH.Helper;
using WPH.MvcMockingServices;

namespace WPH.Areas.Admin.Controllers.BaseInfoType
{
    [Area("Admin")]
    [AdminLoginCheck]
    public class BaseInfoTypeController : Controller
    {
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<BaseInfoTypeController> _logger;

        public BaseInfoTypeController(IDIUnit dIUnit, ILogger<BaseInfoTypeController> logger)
        {
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Form()
        {

            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/BaseInfoType/wpBaseInfoTypeForm.cshtml");
        }

        public ActionResult AddNewModal()
        {
            BaseInfoTypeViewModel baseInfo = new BaseInfoTypeViewModel();


            return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/BaseInfoType/mdBaseInfoTypeNewModal.cshtml", baseInfo);
        }


        public ActionResult EditModal(Guid Id)
        {
            try
            {

                BaseInfoTypeViewModel baseInfoType = _IDUNIT.baseInfoType.GetBaseInfoType(Id);

                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/BaseInfoType/mdBaseInfoTypeNewModal.cshtml", baseInfoType);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        public ActionResult BaseInfoSectioTypeModal(Guid Id)
        {
            try
            {

                return PartialView("/Areas/Admin/Views/Shared/PartialViews/AppWebForms/BaseInfoType/mdBaseInfoSectioTypeModal.cshtml", Id);
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOrUpdate(BaseInfoTypeViewModel baseInfoType)
        {
            try
            {
                if (baseInfoType.Guid != Guid.Empty)
                {

                    return Json(_IDUNIT.baseInfoType.UpdateBaseInfoType(baseInfoType));
                }
                else
                {

                    return Json(_IDUNIT.baseInfoType.AddNewBaseInfoType(baseInfoType));
                }
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public IActionResult GetBaseInfoSectioType(Guid baseInfoTypeId)
        {
            try
            {
                return Json(_IDUNIT.baseInfoType.GetBaseInfoSectioType(baseInfoTypeId));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public IActionResult GetSubsystemSectioType(int subSystemId)
        {
            try
            {
                return Json(_IDUNIT.baseInfoType.GetSubsystemSectioType(subSystemId));
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBaseInfoSectioType(Guid baseInfoTypeId, List<BaseInfoSectioTypeViewModel> sectionTypes)
        {
            try
            {
                _IDUNIT.baseInfoType.AddBaseInfoSectioType(baseInfoTypeId, sectionTypes);
                return Json(1);

            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            List<BaseInfoTypeViewModel> allBaseInfoTypes = _IDUNIT.baseInfoType.GetAllBaseInfoTypes();

            return Json(allBaseInfoTypes.ToDataSourceResult(request));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Remove(Guid Id)
        {
            try
            {
                OperationStatus oStatus = _IDUNIT.baseInfoType.RemoveBaseInfoType(Id);
                return Json(oStatus.ToString());
            }
            catch (Exception e) { _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] + "\t Action: " + ControllerContext.RouteData.Values["action"] + "\t Message:" + e.Message); return Json(0); }

        }
    }
}
