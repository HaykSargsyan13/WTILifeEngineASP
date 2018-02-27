using ASP.Infrastructure;
using ASP.Models;
using ASP.Models.DB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly SyncQueue<Action> ActionsQueue = new SyncQueue<Action>();
        private static readonly SemaphoreSlim ThreadsSem = new SemaphoreSlim(Environment.ProcessorCount * 2);

        public TestController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            ActionsQueue.NewItem += ActionsQueue_OnNewItem;
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
                    data = await reader.ReadToEndAsync();
                }
                var path = RouteData.Values["Id"].ToString();
                ReportItem[] items = JsonConvert.DeserializeObject<ReportItem[]>(data);
                foreach (var item in items)
                {
                    item.Time = DateTime.Today.Date;
                }
                var action = new Action(() =>
                {
                    db.Create(items, path);
                });
                ActionsQueue.Enqueue(action);
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
        /// <summary>
        /// Get method which returns View for serch
        /// </summary>
        /// <returns></returns>
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
                List<ReportItem> items = new List<ReportItem>();
                using (var streamReader = System.IO.File.OpenText(path))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var data = line.Split(new[] { "," }, StringSplitOptions.None);
                        data = data.Select(s => s.Trim('\"', '$', '%')).ToArray();

                        string accauntNo = data[0];
                        if (string.IsNullOrEmpty(accauntNo) || accauntNo == "AccountNo")
                            continue;
                        decimal.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal totalMarketValue);
                        decimal.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cash);
                        decimal.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity1);
                        decimal.TryParse(data[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity2);
                        decimal.TryParse(data[5], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity3);
                        decimal.TryParse(data[6], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity4);
                        decimal.TryParse(data[7], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal marketValue1);
                        decimal.TryParse(data[8], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal marketValue2);
                        decimal.TryParse(data[9], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal marketValue3);
                        decimal.TryParse(data[10], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal marketValue4);
                        decimal.TryParse(data[11], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percentCash);
                        decimal.TryParse(data[12], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percent1);
                        decimal.TryParse(data[13], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percent2);
                        decimal.TryParse(data[14], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percent3);
                        decimal.TryParse(data[15], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percent4);
                        decimal.TryParse(data[16], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal targetCashAlloc);
                        decimal.TryParse(data[17], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal targetRiskScoreUS);
                        decimal.TryParse(data[18], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal targetRiskScoreIntl);
                        decimal.TryParse(data[19], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal targetIntlAlloc);
                        decimal.TryParse(data[20], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal actualCashAlloc);
                        decimal.TryParse(data[21], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal actualRiskScoreUS);
                        decimal.TryParse(data[22], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal actualRiskScoreIntl);
                        decimal.TryParse(data[23], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal actualIntlAlloc);
                        bool.TryParse(data[24], out bool needsRebalance);
                        bool.TryParse(data[25], out bool needsCashRebalance);



                        var item = new ReportItem
                        {
                            AccountNo = accauntNo,
                            TotalMarketValue = totalMarketValue,
                            Cash = cash,
                            Quantity = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  quantity1},
                            {"BND" ,  quantity2},
                            {"VXUS" ,  quantity3},
                            {"BNDX" ,  quantity4},
                        },
                            MarketValue = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  marketValue1},
                            {"BND" ,  marketValue2},
                            {"VXUS" ,  marketValue3},
                            {"BNDX" ,  marketValue4},
                        },
                            PercentCash = percentCash,
                            Percent = new Dictionary<string, decimal>
                        {
                            {"VTI" ,  percent1 },
                            {"BND" ,  percent2 },
                            {"VXUS" ,  percent3},
                            {"BNDX" ,  percent4},
                        },
                            TargetCashAlloc = targetCashAlloc,
                            TargetRiskScoreUS = targetRiskScoreUS,
                            TargetRiskScoreIntl = targetRiskScoreIntl,
                            TargetIntlAlloc = targetIntlAlloc,
                            ActualCashAlloc = actualCashAlloc,
                            ActualRiskScoreUS = actualRiskScoreUS,
                            ActualRiskScoreIntl = actualRiskScoreIntl,
                            ActualIntlAlloc = actualIntlAlloc,
                            NeedsRebalance = needsRebalance,
                            NeedsCashRebalance = needsCashRebalance,
                            Time = reportDate

                        };
                        items.Add(item);
                    }
                }
            var action = new Action(() =>
            {
                db.Create(items, $"{reportDate:yyyyMMdd}");
            });
              //  db.Create(items, $"{reportDate:yyyyMMdd}");
            TimeSpan time = DateTime.Now - dt;
            ActionsQueue.Enqueue(action);
            return Ok(new { count = 1, size , time  });
        }

        #endregion

        private void ActionsQueue_OnNewItem(object sender, Action action)
        {
            var act = action;

            ThreadsSem.Wait();
            Task.Run(() =>
            {
                try
                {
                    act();
                }
                finally
                {
                    ThreadsSem.Release();
                }
            });
        }
    }
}