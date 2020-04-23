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
            string[] infix;
            string[] postfix;
            try{
                infix = r.generateInfixTokens();
                postfix = r.generatePostfixTokens();
            }
            catch(Exception ex){
                goto end;
            }
            if(r.properEquation()){
                
                return Ok(new {
                    status="ok",
                    results= new {
                        infix,
                        postfix
                        }
                });
            }else{
                goto end;
            }
            end:
                var data = new {
                    status="error",
                    results="invalid formula"
                };
                return Ok(data);
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate")]
        public IActionResult Get(string formula = null, string x = null)
            {
            string message="";
            double result;
            if(String.IsNullOrEmpty(formula) || String.IsNullOrEmpty(x)) {
                message = "wrong url";
                goto end;
            }
            double xd = double.Parse(x);
            RPN myRPN = new RPN(formula);
            
            if (!myRPN.properEquation()) goto end;
            try{
                myRPN.generateInfixTokens();
            }
            catch(Exception ex){
                message = (ex.Message);
                goto end;
            }
            
            if (myRPN.invalidTokens)
            {
                message = "invalid tokens";
                goto end;
            }
            myRPN.generatePostfixTokens();
            try{
                double test = myRPN.evaluatePostfix(xd);
                result = test;
            }
            catch(Exception ex){
               message = ex.Message;
               goto end;
            }

            return Ok(new {
                status="ok",
                result = result
            });

            end:
                return Ok(new {
                    status="error",
                    result=message
                });
        }
        [HttpGet]
        [Produces("application/json")]
        [Route("calculate/xy")]
        public IActionResult Get(string formula, string from, string to, string n){
            string message="";
            double result;
            if(String.IsNullOrEmpty(formula) || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) || String.IsNullOrEmpty(n)) {
                message = "wrong url";
                goto end;
            }
            double fromDouble=0;
            double toDouble=0;
            int nInt=0;
            try{
                 fromDouble = double.Parse(from);
                 toDouble = double.Parse(to);
                 nInt = int.Parse(n);
            }
            catch(Exception ex){
                message = ex.Message;
                goto end;
            }
            RPN myRPN = new RPN(formula);
            if (!myRPN.properEquation()) goto end;
            try{
                myRPN.generateInfixTokens();
            }
            catch(Exception ex){
                message = (ex.Message);
                goto end;
            }
            if (myRPN.invalidTokens)
            {
                message = "invalid tokens";
                goto end;
            }
            myRPN.generatePostfixTokens();

            
                
            try{
                List<string> results = myRPN.evaluatePostfix(fromDouble, toDouble, nInt);
                List<xy> pairs = new List<xy>();
                string JSONdelimited = "{";
                
                for(int i = 0; i < results.Count; i++){
                    string[] delimitedParts = results[i].Split('#');
                    pairs.Add(new xy {x = double.Parse(delimitedParts[0]), y = double.Parse(delimitedParts[1]) });
                }
                
                // for(int i =0; i < delimitedStrings.Length; i++){

                // }
                return Ok(new {
                            status="ok",
                            result = pairs
                            
                });
            }
            catch(Exception ex){
               message = ex.Message;
               goto end;
            }

            

            end:
                return Ok(new {
                    status="error",
                    result=message
                });
        }
    }
}
