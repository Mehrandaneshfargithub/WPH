using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WPH.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("ClinicId", "5e48353e-20d8-ea11-b5eb-801934ca48af");
            return View();
        }
    }
}
