using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using ASP.Models;
using ASP.Models.DB;
using Newtonsoft.Json;

using TestApp;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ASP.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TestController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private readonly string key = "P45a6pzCMKZQuFkWAkwL";
        private readonly DBContextReport db = new DBContextReport();

        #region Get Reports Request

        /// <summary>Index is a method in the <seealso cref="TestController"/> class.
        /// Which call the method <seealso cref="Prog.Main"/> to send the http Requests with Reports collection
        /// </summary>
        [AllowAnonymous]
        public string Index(string id)
        {
            TestApp.Prog.Main();
            return "Send";
        }

        /// <summary>Report is a method in the <seealso cref="TestController"/> class.
        /// Which Get the http Requests with Reports collection and write it to a DataBase 
        /// </summary>
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
                    item.Time = DateTime.Today.Date;
                }
                await db.Create(items, path);
            }
            return "Ok";
        }

        #endregion

        #region GetReports

        /// <summary>GetReports is a Get method in the TestController class. Returns the view where user can send post request to get Reports
        /// <para><seealso cref="TestController"/></para>
        /// </summary>
        [HttpGet]
        public IActionResult GetReports()
        {
            return View();
        }

        /// <summary>GetReports is a Post method with parametr <seealso cref="DateTime"/> in the TestController class. Returns the view where user can send post request to get Reports
        /// <para><seealso cref="TestController"/></para>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GetReports(DateTime date)
        {
            if (date > DateTime.Today)
                return View(null);

            string s = $"{date:yyyyMMdd}";

            IEnumerable<ReportItem> items = await db.GetReports(s);

            return View(items);
        }

        #endregion

        #region Get Reports for one Account
        [HttpGet]
        public IActionResult AccountReports()
        {
            return View();
        }

        /// <summary>
        /// Post method which get Reports from db for selectid Account
        /// </summary>
        /// <param name="AccountNo">Account number</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AccountReports(string AccountNo)
        {
            DateTime dt = DateTime.Now;
            var list =  await db.AccountReports(AccountNo);
            TimeSpan time = DateTime.Now - dt;
            ViewBag.timespan = time;
            return View(list);
        }

        #endregion

        #region Upload File

        /// <summary>UploadFile is a Get method in the TestController class. Returns the view for uploading file
        ///<para><seealso cref="TestController"/></para>
        /// </summary>
        [HttpGet]
        public IActionResult UploadFile()
        {
            return View();
        }

        /// <summary>UploadFile is a Post method in the TestController class. Writes Reports form file to DB
        /// <para><seealso cref="TestController"/></para>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            DateTime dt = DateTime.Now;

           var name = file.FileName;
           name = name.Substring(name.IndexOf('_')+1);
           name = name.Substring(0, name.LastIndexOf('.'));
           name = name.Replace("_", "-");
           DateTime reportDate = DateTime.Parse(name);
            long size = file.Length;
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "data" , file.FileName);
            
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            ReportItem[] items;
            using (var streamReader = System.IO.File.OpenText(path))
            {
                var content = streamReader.ReadToEnd().Replace("\"", "");
                var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                items = new ReportItem[lines.Length - 3];
                for (int i = 1; i < lines.Length - 2; i++)
                {
                    var data = lines[i].Split(new[] { "," }, StringSplitOptions.None);
                    var item = new ReportItem
                    {
                        AccountNo = data[0],
                        TotalMarketValue = decimal.Parse(data[1].Trim('$')),
                        Cash = decimal.Parse(data[2].Trim('$')),
                        Quantity = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  decimal.Parse(data[3])},
                            {"BND" ,  decimal.Parse(data[4])},
                            {"VXUS" ,  decimal.Parse(data[5])},
                            {"BNDX" ,  decimal.Parse(data[6])},
                        },
                        MarketValue = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  decimal.Parse(data[7].Trim('$'))},
                            {"BND" ,  decimal.Parse(data[8].Trim('$'))},
                            {"VXUS" ,  decimal.Parse(data[9].Trim('$'))},
                            {"BNDX" ,  decimal.Parse(data[10].Trim('$'))},
                        },
                        PercentCash = decimal.Parse(data[11].Trim('%')),
                        Percent = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  decimal.Parse(data[12].Trim('%'))},
                            {"BND" ,  decimal.Parse(data[13].Trim('%'))},
                            {"VXUS" ,  decimal.Parse(data[14].Trim('%'))},
                            {"BNDX" ,  decimal.Parse(data[15].Trim('%'))},
                        },
                        TargetCashAlloc = decimal.Parse(data[16]),
                        TargetRiskScoreUS = decimal.Parse(data[17]),
                        TargetRiskScoreIntl = decimal.Parse(data[18]),
                        TargetIntlAlloc = decimal.Parse(data[19]),
                        ActualCashAlloc = decimal.Parse(data[20]),
                        ActualRiskScoreUS = decimal.Parse(data[21]),
                        ActualRiskScoreIntl = decimal.Parse(data[22]),
                        ActualIntlAlloc = decimal.Parse(data[23]),
                        NeedsRebalance = bool.Parse(data[24]),
                        NeedsCashRebalance = bool.Parse(data[25]),
                        Time = reportDate

                    };
                    items[i - 1] = item;
                }
            }
            await db.Create(items , $"{reportDate:yyyyMMdd}");
            TimeSpan time = DateTime.Now - dt;
            
            return Ok(new { count = 1, size , time });
        }

        #endregion
    }
}