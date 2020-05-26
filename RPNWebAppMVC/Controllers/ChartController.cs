using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using RPNWebAppMVC.Models;
namespace RPNWebAppMVC.Controllers
{
    public class ChartController : Controller
    {
        private readonly ILogger<ChartController> _logger;

        public ChartController(ILogger<ChartController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> ChartAsync(string formula = "sin(x)", string from = "-40", string to = "40", string n = "500")
        {
            var klient = new HttpClient();
            HttpResponseMessage response = klient.GetAsync("http://localhost:5420/api/calculate/xy?formula="+formula+"&from="+from+"+&to="+to+"+&n="+n+"").Result;
            var jsonstr = await response.Content.ReadAsStringAsync();
            if(response.StatusCode==HttpStatusCode.OK)
            {
            //Console.WriteLine(jsonstr);
            var jsonobj2 = JsonSerializer.Deserialize<responseXYRange>(jsonstr);
            //var jsonobj = JsonConvert.DeserializeObject<RPNWebAppMVC.Models.responseXYRange>(jsonstr);
            responseXYRange myres = (responseXYRange)jsonobj2;
            ViewBag.arr = new String[] {formula, from, to, n};
            //ViewBag.arr = jsonobj.result;
            //Console.WriteLine(response.ToString());
            return View(myres);
            }

            
            
            
            return View(new ErrorViewModel());
        }
        
        public IActionResult funchart()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
