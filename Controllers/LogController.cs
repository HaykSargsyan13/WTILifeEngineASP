using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        
        public IActionResult Register()
        {
                return Content(User.Identity.Name);
        }
        public IActionResult About()
        {
            return Content("Authorized");
        }
    }
}
