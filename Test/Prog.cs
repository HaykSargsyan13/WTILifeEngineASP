using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Timers;
using Newtonsoft.Json;
using ASP.Models;

namespace TestApp
{
    internal class Prog
    {
            private const string ReportPath = "/Test";

            private static readonly ReportItem[] Report =
            {
            new ReportItem
            {
                AccountNo = "DWVQ000610",
                TotalMarketValue = 30.56m,
                Cash = 2.1m,
                PercentCash = 6.43m,
                Quantity = new Dictionary<string, decimal>
                {
                    {"VTI", 0.1003m},
                    {"BND", 0.1139m},
                    {"VXUS", 0.0806m},
                    {"BNDX", 0.0559m}
                },
                MarketValue = new Dictionary<string, decimal>
                {
                    {"VTI", 13.89m},
                    {"BND", 9.04m},
                    {"VXUS", 4.62m},
                    {"BNDX", 3.01m}
                },
                Percent = new Dictionary<string, decimal>
                {
                    {"VTI", 42.53m},
                    {"BND", 27.68m},
                    {"VXUS", 14.15m},
                    {"BNDX", 9.22m}
                },
                TargetCashAlloc = 0,
                TargetRiskScoreUS = 0.6m,
                TargetRiskScoreIntl = 0.6m,
                TargetIntlAlloc = 0.25m,
                ActualCashAlloc = 0.06m,
                ActualRiskScoreUS = 0.61m,
                ActualRiskScoreIntl = 0.61m,
                ActualIntlAlloc = 0.33m,
                NeedsRebalance = false,
                NeedsCashRebalance = false,
            },
            new ReportItem
            {
                AccountNo = "DWES000609",
                TotalMarketValue = 2.21m,
                Cash = 7.74m,
                PercentCash = 77.79m,
                Quantity = new Dictionary<string, decimal>
                {
                    {"VTI", 0},
                    {"BND", 0.0278m},
                    {"VXUS", 0},
                    {"BNDX", 0}
                },
                MarketValue = new Dictionary<string, decimal>
                {
                    {"VTI", 0},
                    {"BND", 2.21m},
                    {"VXUS", 0},
                    {"BNDX", 0}
                },
                Percent = new Dictionary<string, decimal>
                {
                    {"VTI", 0},
                    {"BND", 22.21m},
                    {"VXUS", 0},
                    {"BNDX", 0}
                },
                TargetCashAlloc = 0,
                TargetRiskScoreUS = 0.6m,
                TargetRiskScoreIntl = 0.6m,
                TargetIntlAlloc = 0.25m,
                ActualCashAlloc = 0.78m,
                ActualRiskScoreUS = 0,
                ActualRiskScoreIntl = 0,
                ActualIntlAlloc = 0,
                NeedsRebalance = true,
                NeedsCashRebalance = true,
            },
            new ReportItem
            {
                AccountNo = "DWCK000564",
                TotalMarketValue = 10.82m,
                Cash = 13.19m,
                PercentCash = 54.94m,
                Quantity = new Dictionary<string, decimal>
                {
                    {"VTI", 0.0355m},
                    {"BND", 0.0401m},
                    {"VXUS", 0.029m},
                    {"BNDX", 0.0197m}
                },
                MarketValue = new Dictionary<string, decimal>
                {
                    {"VTI", 4.92m},
                    {"BND", 3.18m},
                    {"VXUS", 1.66m},
                    {"BNDX", 1.06m}
                },
                Percent = new Dictionary<string, decimal>
                {
                    {"VTI", 20.49m},
                    {"BND", 13.24m},
                    {"VXUS", 6.91m},
                    {"BNDX", 4.41m}
                },
                TargetCashAlloc = 0,
                TargetRiskScoreUS = 0.6m,
                TargetRiskScoreIntl = 0.6m,
                TargetIntlAlloc = 0.25m,
                ActualCashAlloc = 0.55m,
                ActualRiskScoreUS = 0.61m,
                ActualRiskScoreIntl = 0.61m,
                ActualIntlAlloc = 0.34m,
                NeedsRebalance = false,
                NeedsCashRebalance = true,
            },
        };

            public static void Main(/*object sender, ElapsedEventArgs elapsedEventArgs*/)


        {
                var path = $"{ReportPath}/Report/{DateTime.Today:yyyyMMdd}";

                var data = JsonConvert.SerializeObject(Report);
            //JsonConvert.DeserializeObject<ReportItem[]>()

                var result = SendHttpRequest(path, data);

                Console.WriteLine(result);
                Console.ReadLine();
            }

            private static string SendHttpRequest(string path, string data)
            {
                using (var client = new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                })
                {
                    BaseAddress = new Uri("http://localhost:50698"),
                })
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, path);
                    requestMessage.Headers.TryAddWithoutValidation("authkey", "P45a6pzCMKZQuFkWAkwL");

                requestMessage.Content = new StringContent(data);

                    var responseMessage = client.SendAsync(requestMessage).Result;
                    if (responseMessage == null)
                        return null;
                    responseMessage.EnsureSuccessStatusCode();
                    return responseMessage.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
