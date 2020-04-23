using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPNWebApi;

namespace RPNWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class RPNController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [Route("tokens")]
        public IActionResult Get(string formula){
            RPN r = new RPN(formula);
            if(r.properEquation()){
                var data = new {
                    status="ok",
                    results= new {
                        infix=r.generateInfixTokens(),
                        postfix = r.generatePostfixTokens()
                        }
                };
                return Ok(data);
            }else{
                var data = new {
                    status="error",
                    results="invalid formula"
                };
                return Ok(data);
            }
            
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate")]
        public IActionResult Get(string formula, int id){
            RPN r = new RPN(formula);
            var data = new {
                status="ok",
                results= "nothingyet"
            };
            return Ok(data);
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate/xy")]
        public IActionResult Get(string formula, int id2, int id3){
            RPN r = new RPN(formula);
            var data = new {
                status="ok",
                results="nothigyet"
            };
            return Ok(data);
        }
    }
}
