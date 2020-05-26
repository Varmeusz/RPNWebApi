using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPNRest31;
using Microsoft.AspNetCore.Cors;
namespace RPNRest31.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get(string formula = null, string x = null)
            {
            string message = "";
            double result = 0;
            if(String.IsNullOrEmpty(formula) || String.IsNullOrEmpty(x)) 
            {
                message = "wrong url";
                goto end;
            }
            double xD = double.Parse(x.Replace('.',','));
            RPN myRPN = new RPN(formula);
            
            if (!myRPN.properEquation()) 
            {
                message = "invalid formula";
                goto end;
            }
            try
            {
                myRPN.generateInfixTokens();
            }
            catch(Exception ex)
            {
                message = (ex.Message);
                goto end;
            }
            
            if (myRPN.invalidTokens)
            {
                message = "invalid tokens";
                goto end;
            }
            myRPN.generatePostfixTokens();
            try
            {
                result = myRPN.evaluatePostfix(xD);

                responseCalculate responseCalc = new responseCalculate();
                responseCalc.status = "ok";
                responseCalc.result = result;
                return Ok(responseCalc);
            }
            catch(Exception ex)
            {
               message = ex.Message;
               goto end;
            }
            end:
                responseError response = new responseError();
                response.status = "error";
                response.result = message;
                return BadRequest(response);
        }
        [EnableCors("MVCPolicy")]
        
        [HttpGet("xy")]
        [Produces("application/json")]
        public IActionResult Get(string formula, string from, string to, string n){
            string message="";
            if(String.IsNullOrEmpty(formula) || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) || String.IsNullOrEmpty(n)) 
            {
                message = "wrong url";
                goto end;
            }
            double fromDouble = 0;
            double toDouble = 0;
            int nInt = 0;
            try
            {
                 fromDouble = double.Parse(from.Replace('.',','));
                 toDouble = double.Parse(to.Replace('.',','));
                 nInt = int.Parse(n);
            }
            catch(Exception ex)
            {
                message = ex.Message;
                goto end;
            }
            RPN myRPN = new RPN(formula);
            if (!myRPN.properEquation()) 
            {
                message = "invalid formula";
                goto end;
            }
            try
            {
                myRPN.generateInfixTokens();
            }
            catch(Exception ex)
            {
                message = (ex.Message);
                goto end;
            }
            if (myRPN.invalidTokens)
            {
                message = "invalid tokens";
                goto end;
            }
            myRPN.generatePostfixTokens();
            try
            {
                List<string> results = myRPN.evaluatePostfix(fromDouble, toDouble, nInt);
                if (myRPN.evalError==false)
                {
                    List<xy> pairs = new List<xy>();
                    for(int i = 0; i < results.Count; i++){
                        string[] delimitedParts = results[i].Split('#');
                        pairs.Add(new xy {x = double.Parse(delimitedParts[0]), y = double.Parse(delimitedParts[1]) });
                    }

                    responseXYRange returnResponseXYRange = new responseXYRange();
                    returnResponseXYRange.status = "ok";
                    returnResponseXYRange.result = pairs.ToArray();
                    return Ok(returnResponseXYRange);
                }
                else
                {
                    List<xyErrors> pairs = new List<xyErrors>();
                    for(int i = 0; i < results.Count; i++){
                        string[] delimitedParts = results[i].Split('#');
                        
                        if(double.TryParse(delimitedParts[1], out double parsed))
                        {
                            pairs.Add(new xyErrors {x = double.Parse(delimitedParts[0]), y = parsed });
                        }
                        else
                        {
                            pairs.Add(new xyErrors {x = double.Parse(delimitedParts[0]), error = delimitedParts[1] });
                        }
                    }
                    responseXYRangeError errorPairs = new responseXYRangeError();
                    errorPairs.status = "error";
                    errorPairs.result = pairs.ToArray();

                    return StatusCode(222, errorPairs);
                }
                
            }
            catch(Exception ex)
            {
               message = ex.Message;
               goto end;
            }
            end:
                responseError responseError = new responseError();
                responseError.status = "error";
                responseError.result = message;
                return BadRequest(responseError);
        }
    }
}
