using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.MvcMockingServices;

namespace WPH.Helper
{
    public class AdminLoginCheck : ActionFilterAttribute
    {
        private IDIUnit _IDUNIT;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _IDUNIT = (IDIUnit)filterContext.HttpContext.RequestServices.GetService(typeof(IDIUnit));

            var admin_pass = filterContext.HttpContext.Session.GetString("AdminPass");
            var user_name = filterContext.HttpContext.Session.GetString("UserName");

            var userExist = _IDUNIT.user.CheckAdminExist(user_name, admin_pass);

            if (userExist == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "UserHandler" },
                                { "Action", "Logout" }
                                });
            }
        }

    }
}
