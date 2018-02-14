using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    public class LogController : Controller
    {

        public IActionResult Register()
        {
            return View();
        }
    }
}
