using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WPH;
using WPH.Helper;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.MvcMockingServices;

namespace WPH.Controllers.User
{

    public class UserHandlerController : Controller
    {
        string userName = string.Empty;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDIUnit _IDUNIT;
        private readonly ILogger<UserHandlerController> _logger;


        public UserHandlerController(IStringLocalizer<SharedResource> localizer, IDIUnit dIUnit, ILogger<UserHandlerController> logger)
        {
            _localizer = localizer;
            _IDUNIT = dIUnit;
            _logger = logger;
        }

        public ActionResult Index()
        {
            try
            {

                string name = _IDUNIT.clinic.GetClinicName();
                string[] clinicName = name.Split(' ');
                ViewBag.Name1 = clinicName[0];

                try
                {
                    ViewBag.Name2 = clinicName[1];
                }
                catch { }

                ViewBag.ClinicName = name;


                //var licenceAccess = _IDUNIT.licenceKey.CheckLicence();
                //if (!long.TryParse(licenceAccess, out long remDay))
                //    return View("Licence", licenceAccess);

                ViewBag.RemDays = 10;

                //if(remDay < 20)
                //{

                //}

                return View();
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
        public IActionResult AddLicenceKey(string licenceKey)
        {
            try
            {
                _IDUNIT.licenceKey.AddNewLicenceKey(licenceKey);

                return Json(Url.Action("Index", "UserHandler"));
            }
            catch
            {
                return Json(0);
            }
        }

        public JsonResult GetSerial()
        {
            CheckLicence checkLicence = new CheckLicence();
            return Json(checkLicence.GetSerial());
        }


        public ActionResult All()
        {
            return PartialView("/Views/Shared/PartialViews/AppWebForms/UserHandler/View.cshtml");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoginUser(LoginViewModel model)
        {
            try
            {
                bool result = false;
                //bool multiClinicSection = false;
                string clinicAdmin = "";
                Guid[] clinicSectionGuid = { };

                bool isShowCaptcha = false;
                bool isShowClinicCode = false;
                if (isShowCaptcha)
                {
                    //string userInput = HttpContext.Request.Form["CaptchaCode"];
                    //MvcCaptcha mvcCaptcha = new MvcCaptcha("ExampleCaptcha");
                    //if (ModelState.IsValid)
                    //{
                    //    if (mvcCaptcha.Validate(userInput))
                    //    {
                    //        String password = Crypto.Hash(model.Pass3, "MD5");
                    //        if (model.UserName == "developer")
                    //        {
                    //            UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, Guid.Empty);
                    //            if (userExist != null)
                    //            {
                    //                DetrmineRightToLeft(model.UserName);
                    //                //Session["UserName"] = model.UserName;
                    //                HttpContext.Session.SetString("UserName", model.UserName);
                    //                HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                    //                //Session["UserId"] = userExist.Guid;
                    //                //Session.Timeout = 10000;
                    //                result = true;
                    //                clinicAdmin = userExist.AccessTypeName;
                    //                clinicSectionGuid = userExist.ClinicSectionGuids;

                    //                //if (userExist.ClinicSectionGuids.Length > 1)
                    //                //{
                    //                //    multiClinicSection = true;
                    //                //}
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (isShowClinicCode)
                    //            {
                    //                Guid clinicId = _IDUNIT.user.CheckClinicExist(model.Code);
                    //                if (clinicId != Guid.Empty)
                    //                {
                    //                    UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, clinicId);
                    //                    if (userExist != null)
                    //                    {
                    //                        DetrmineRightToLeft(model.UserName);
                    //                        HttpContext.Session.SetString("UserName", model.UserName);
                    //                        HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                    //                        //Session.Timeout = 10000;
                    //                        result = true;
                    //                        clinicAdmin = userExist.AccessTypeName;
                    //                        clinicSectionGuid = userExist.ClinicSectionGuids;

                    //                        //if (userExist.ClinicSectionGuids.Length > 1)
                    //                        //{
                    //                        //    multiClinicSection = true;
                    //                        //}
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, Guid.Empty);
                    //                if (userExist != null)
                    //                {
                    //                    DetrmineRightToLeft(model.UserName);
                    //                    HttpContext.Session.SetString("UserName", model.UserName);
                    //                    HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                    //                    ///Session.Timeout = 10000;
                    //                    result = true;
                    //                    clinicAdmin = userExist.AccessTypeName;
                    //                    clinicSectionGuid = userExist.ClinicSectionGuids;

                    //                    //if (userExist.ClinicSectionGuids.Length > 1)
                    //                    //{
                    //                    //    multiClinicSection = true;
                    //                    //}
                    //                }
                    //            }

                    //        }
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("CaptchaCode", "Wrong Captcha!");
                    //    }
                    //}
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        String password = Crypto.Hash(model.Pass3, "MD5");
                        if (model.UserName == "developer")
                        {
                            var count = _IDUNIT.user.CheckUserCount(model.UserName, password, Guid.Empty, model.UserCode);
                            if (count > 1)
                                return Json("EnterUserCode");

                            UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, Guid.Empty, model.UserCode);
                            if (userExist != null)
                            {
                                DetrmineRightToLeft(model.UserName);
                                HttpContext.Session.SetString("UserName", model.UserName);
                                HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                                //HttpContext.Session.SetString("ClinicSectionId", userExist.ClinicSectionId.ToString());
                                result = true;
                                //clinicAdmin = userExist.AccessTypeName;
                                clinicAdmin = "Normal";
                                //clinicSectionGuid = us2erExist.ClinicSectionGuids;

                                Guid[] clinicSectionGuid2 = { userExist.ClinicSectionId ?? Guid.Empty };
                                clinicSectionGuid = clinicSectionGuid2;

                            }
                        }
                        else
                        {
                            if (isShowClinicCode)
                            {
                                Guid clinicId = _IDUNIT.user.CheckClinicExist(model.Code);
                                if (clinicId != Guid.Empty)
                                {
                                    var count = _IDUNIT.user.CheckUserCount(model.UserName, password, clinicId, model.UserCode);
                                    if (count > 1)
                                        return Json("EnterUserCode");

                                    UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, clinicId, model.UserCode);
                                    if (userExist != null)
                                    {
                                        DetrmineRightToLeft(model.UserName);
                                        HttpContext.Session.SetString("UserName", model.UserName);
                                        HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                                        //Session.Timeout = 10000;
                                        result = true;
                                        clinicAdmin = "Normal";
                                        //clinicSectionGuid = userExist.ClinicSectionGuids;
                                        clinicSectionGuid[0] = Guid.Parse("94306861-20d8-ea11-b5eb-801934ca48af");

                                        //if (userExist.ClinicSectionGuids.Length > 1)
                                        //{
                                        //    multiClinicSection = true;
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                var count = _IDUNIT.user.CheckUserCount(model.UserName, password, Guid.Empty, model.UserCode);
                                if (count > 1)
                                    return Json("EnterUserCode");

                                UserInformationViewModel userExist = _IDUNIT.user.CheckUserExist(model.UserName, password, Guid.Empty, model.UserCode);
                                if (userExist != null)
                                {
                                    DetrmineRightToLeft(model.UserName);
                                    HttpContext.Session.SetString("UserName", model.UserName);
                                    HttpContext.Session.SetString("UserId", userExist.Guid.ToString());
                                    
                                    result = true;
                                    clinicAdmin = "Normal";
                                    Guid[] clinicSectionGuid2 = { userExist.ClinicSectionId ?? Guid.Empty };
                                    clinicSectionGuid = clinicSectionGuid2;

                                }
                            }
                        }

                    }
                }

                return Json(new { result, clinicAdmin, clinicSectionGuid });
            }
            catch(Exception e)
            {
                _logger.LogInformation("Controller: " + ControllerContext.RouteData.Values["controller"] +
                                                       "\t Action: " + ControllerContext.RouteData.Values["action"] +
                                                       "\t Message: " + e.Message);
                throw e;
            }
           

        }
        public ActionResult Logout()
        {
            //Session.Clear();
            //Session.Abandon();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "UserHandler");
        }

        public JsonResult chooseLanguage(string language)
        {
            //_IDUNIT.user.saveCookie("languageCulture", "culture", language, Response);
            //_languageMvcService = new LanguageMvcService(Request, Response);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    }
            );
            return Json("");
        }

        public JsonResult chooseLoginTheme(string theme)
        {
            //_IDUNIT.user.saveCookie("Theme", "loginTheme", theme, Response);
            return Json("");
        }
        private void DetrmineRightToLeft(string userName)
        {
            //SettingMvcService settingService = new SettingMvcService();
            //_IDUNIT.setting.DetrmineRightToLeftBasedOnCulture(culture, userName);
        }

        public JsonResult ChangeClinicSection()
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                UserInformationViewModel userExist = _IDUNIT.user.GetUser(UserId);
                string clinicAdmin = userExist.AccessTypeName;
                Guid[] clinicSectionGuid = userExist.ClinicSectionGuids;
                bool result = false;
                if (userExist.ClinicSectionGuids.Length > 1)
                    result = true;
                return Json(new { result, clinicAdmin, clinicSectionGuid });



            }
            catch { return Json(""); }
        }

    }
}