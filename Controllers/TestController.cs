using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using ASP.Models;
using Newtonsoft.Json;

using TestApp;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Controllers
{
    [Authorize]
    public class TestController : Controller
    {

        private readonly string key = "P45a6pzCMKZQuFkWAkwL";
        private readonly DBContextReport db = new DBContextReport();

        [AllowAnonymous]
        public string Index(string id)
        {
            TestApp.Prog.Main();
            return "Send";
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<string>  Report()
        {
            if (HttpContext.Request.Headers["authkey"] == key)
            {
                string data;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                   data= await reader.ReadToEndAsync();
                }
                var path = RouteData.Values["Id"].ToString();

                ReportItem[] items = JsonConvert.DeserializeObject<ReportItem[]>(data);
                foreach (var item in items)
                {
                    await db.Create(item, path);
                }
            }
            return "Ok";
        }

        [HttpGet]
        public IActionResult GetReports()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetReports(DateTime date)
        {
            if (date > DateTime.Today)
                return View(null);

            string s = $"{date:yyyyMMdd}";
            IEnumerable<ReportItem> items = await db.GetReports(s);
            return View(items);
        }
    }
}