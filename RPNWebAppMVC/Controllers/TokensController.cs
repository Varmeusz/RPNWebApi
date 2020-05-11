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

namespace RPNWebAppMVC.Controllers
{
    public class TokensController : Controller
    {
        private readonly ILogger<TokensController> _logger;

        public TokensController(ILogger<TokensController> logger)
        {
            _logger = logger;
        }
        public IActionResult Tokens(string formula)
        {
            var klient = new HttpClient();
            
            
            
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
