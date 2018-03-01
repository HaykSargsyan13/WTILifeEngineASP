using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASP.Models.DB;

namespace ASP.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private readonly DBContextReport db = new DBContextReport();
        
        [ValidateAntiForgeryToken]
        public IActionResult Post()
        {
            return new JsonResult("Hello");
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult About()
        {
            return Content("Success");
        }
    }
}
