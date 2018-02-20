using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.Models;
using ASP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DBContext db = new DBContext();

        public IActionResult Index()
        {
            return View();
        }

        private ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        private ActionResult Create(LoginViewModel c)
        {
            if (ModelState.IsValid)
            {
               // await db.Create(c);
                return RedirectToAction("Index");
            }
            return View(c);
        }

        private ActionResult Edit(string name, string password)
        {
            LoginViewModel c =  db.GetAccount(name, password);
            if (c == null)
                return NotFound();
            return View(c);
        }
        [HttpPost]
        private ActionResult Edit(LoginViewModel c)
        {
            if (ModelState.IsValid)
            {
               // await db.Update(c);
                return RedirectToAction("Index");
            }
            return View(c);
        }

       
    }
}