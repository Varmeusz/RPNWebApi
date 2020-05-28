using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using RPNRest31;

namespace RPNRest31.Controllers
{
    [EnableCors("MVCPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get(string formula){
            string message = "";
            RPN r = new RPN(formula);
            string[] infix;
            string[] rpn;
            try
            {
                infix = r.generateInfixTokens();
                rpn = r.generatePostfixTokens();
            }
            catch(Exception ex)
            {
                message=ex.Message;
                goto end;
            }
            if(r.properEquation())
            {
                responseTokens respTokens = new responseTokens();
                respTokens.status = "ok";
                respTokens.result.infix = infix;
                for(int i = 0; i < rpn.Length; i++)
                {
                    rpn[i] = rpn[i].Replace(',','.');
                }
                respTokens.result.rpn = rpn;
                return Ok(respTokens);
            }
            else
            {
                message="invalid formula";
                goto end;
            }
            end:
                responseError responseError = new responseError();
                responseError.status = "error";
                responseError.result = message;
                return Ok(responseError);
        }
    }
}
