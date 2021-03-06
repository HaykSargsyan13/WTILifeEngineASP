﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.Models;
using ASP.Models.DB;
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

        [AllowAnonymous]
        public async Task<ActionResult> Create()
        {
            LoginViewModel admin = new LoginViewModel
            {
                Name = "admin",
                Password = "admin"
            };
            bool success = await db.Create(admin);
            return Ok(success ? "success" : "faild");
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