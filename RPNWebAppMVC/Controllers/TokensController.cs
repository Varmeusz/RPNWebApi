using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using RPNWebAppMVC.Models;
using System.Text.Json;
namespace RPNWebAppMVC.Controllers
{
    public class TokensController : Controller
    {
        private readonly ILogger<TokensController> _logger;

        public TokensController(ILogger<TokensController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> TokensAsync(string formula)
        {
            var klient = new HttpClient();
            HttpResponseMessage response = klient.GetAsync("http://localhost:5420/api/tokens?formula="+formula+"").Result;
            var jsonstr = await response.Content.ReadAsStringAsync();
            if(response.StatusCode==HttpStatusCode.OK)
            {

            var jsonobj = JsonSerializer.Deserialize<RPNWebAppMVC.Models.responseTokens>(jsonstr);
            ViewBag.arr = jsonobj.result;
            Console.WriteLine(response.ToString());
            return View(new TokensViewModel{
                rpn = jsonobj.result.rpn,
                infix = jsonobj.result.infix
            });
            }

            
            
            
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
