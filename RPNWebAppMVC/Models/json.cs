using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace RPNWebAppMVC.Models
{
    public class xy
    {
        public double x { get; set; }
        public double y { get; set; }
    }
    public class xyErrors
    {
        public double x { get; set; }
        public double y { get; set; }
        public string error { get; set; }
    }
    public class responseTokens 
    {
        public string status { get; set; }
        public responseArr result { get; set; } 
        public responseTokens()
        {
            this.result = new responseArr();
        }
    }
    
    public class responseArr
    {
        public string[] infix { get; set; }
        public string[] rpn { get; set; }
    }
    public class responseCalculate
    {
        public string status { get; set; }
        public double result { get; set; }
    }
    public class responseError
    {
        public string status { get; set; }
        public string result  {get; set; }
    }
    public class responseXYRange
    {
        public string status { get; set; }
        public xy[] result { get; set; }
    }
    public class responseXYRangeError 
    {
        public string status { get; set; }
        public xyErrors[] result {get; set;}
    }


}