using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;
namespace RPNRest31
{
    [Serializable]
    public class DomainErrorException : Exception{
        public DomainErrorException()
        {
        }
        public DomainErrorException(string message) : base(message) 
        {
        }
        public DomainErrorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class RPN
    {
        string equation;
        public List<string> infixTokens = new List<string>();
        public List<string> postfixTokens = new List<string>();
        public bool invalidTokens = false;
        public bool evalError = false;
        static Dictionary<string, int> properDict = new Dictionary<string, int> 
        {
                { "sin", 4 }, { "cos", 4 }, { "abs", 4 }, { "exp", 4 }, { "log", 4 }, {"sqrt", 4 },{"tan", 4 }, {"cosh", 4 },{"sinh", 4 },{"tanh", 4 } ,{"acos", 4 },{"asin", 4 },{"atan", 4 },
                {"^", 3 },{ "-u",3 },
                { "*", 2 },{"/", 2 },
                {"+", 1 },{"-", 1 },
                { "(", 0 },{")", 0}

            };
        
        public RPN(string input)
        {
            this.equation = input;
            this.equation= this.equation.Replace(" ", "");
        }
        public bool properEquation()
        {
            string eq = this.equation;
            eq.Trim();
            int count = 0;
            foreach(char c in eq)
            {
                if (c == '(') count++;
                else 
                    if (c == ')') count--;
            }
            if (count != 0)
            {
                return false;
            }
            for(int i = 0; i<eq.Length-1; i++)
            {
                if(eq[i] == '(' && eq[i+1] == ')')
                {
                    return false;
                }
            }
            return true;
        }
        public string[] generateInfixTokens()
        {
            List<string> possibleTokens = new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan"};
            List<string> possibleSingleTokens = new List<string> { "^", "*", "/", "+", "-", "(", ")","x"};
            List<string> tokens = new List<string>();
            for(int i = 0; i < equation.Length;i++){
                if (equation[i] == '-' && (tokens.Count == 0 || isOperator(tokens[tokens.Count - 1])))
                {
                    tokens.Add("-u");
                    continue;
                }
                else
                if ( int.TryParse(equation[i].ToString(), out _))
                {
                    int tmpi = i;
                    int count = 0;
                    string intBuilder = "";
                    while(tmpi<equation.Length && (int.TryParse(equation[tmpi].ToString(), out _) || equation[tmpi] == '.' || equation[tmpi] == ','))
                    {
                        intBuilder = intBuilder + equation[tmpi];
                        tmpi++;
                        count++;
                    }
                    tokens.Add(intBuilder);
                    i = tmpi-1;
                    continue;
                }
                else
                if(possibleSingleTokens.Contains(equation[i].ToString()))
                {
                    tokens.Add(equation[i].ToString());
                    continue;
                }
                else
                if(possibleTokens.Contains(equation[i].ToString() + equation[i+1].ToString() + equation[i+2].ToString() + equation[i+3].ToString()))
                {
                    tokens.Add(equation[i].ToString() + equation[i+1].ToString() + equation[i+2].ToString() + equation[i+3].ToString());
                    i+=3;
                    continue;
                }
                else
                if(possibleTokens.Contains(equation[i].ToString() + equation[i+1].ToString() + equation[i+2].ToString()))
                {
                    tokens.Add(equation[i].ToString() + equation[i+1].ToString() + equation[i+2].ToString());
                    i+=2;
                    continue;
                }else
                {
                    throw new Exception("Invalid formula");
                }
            }
            foreach(string t in tokens)
            {
                this.infixTokens.Add(t);
            }
            if (!checkTokensValidity(this.infixTokens))
            { 
                this.invalidTokens = true;
            }

            return tokens.ToArray();
        }
        public string[] generatePostfixTokens()
        {
            List<string> tokens = this.infixTokens;
            Stack S = new Stack();
            Queue Q = new Queue();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "(") 
                { 
                    S.Push(tokens[i]); 
                    continue;
                }

                if (tokens[i] == ")")
                {
                    while (S.Peek().ToString() != "(")
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Pop();
                    continue;
                }
                if (getPriority(tokens[i]) < 10)
                {
                    while (S.Count > 0 && getPriority(tokens[i]) <= getPriority(S.Peek().ToString()))
                    {
                        Q.Enqueue(S.Pop());
                    }
                    S.Push(tokens[i]);
                    continue;
                }
                tokens[i] = tokens[i].Replace(',', '.');
                if (int.TryParse(tokens[i].ToString(), out _) || double.TryParse(tokens[i].ToString(), out _) || float.TryParse(tokens[i].ToString(), out _) || tokens[i] == "x")
                { 
                    Q.Enqueue(tokens[i]); 
                    continue; 
                }
            }
            while (S.Count > 0) Q.Enqueue(S.Pop());
            int tabsize = Q.Count;
            string[] postfix = new string[tabsize];
            int count = 0;
            while (count != tabsize)
            {
                postfix[count] = Q.Dequeue().ToString();
                count++;
            }
            foreach(string t in postfix)
            {
                this.postfixTokens.Add(t);
            }
            return postfix;
        }
        public List<string> returnInfixTokens()
        {
            return this.infixTokens;
        }
        public List<string> returnPostfixTokens()
        {
            return this.postfixTokens;
        }
        public double evaluatePostfix(double x)
        {
            string[] tokens = this.postfixTokens.ToArray();
            Stack S = new Stack();
            for (int i = 0; i < tokens.Length; i++)
            {
                if (isNumber(tokens[i]))
                {
                    S.Push(tokens[i]);
                }
                if (getPriority(tokens[i]) == 4 || tokens[i] == "-u")
                {
                    double temp = parseDouble(S.Pop().ToString());
                    S.Push(evalFun(temp, tokens[i]));
                }
                if (getPriority(tokens[i]) >= 1 && getPriority(tokens[i]) <= 3 && tokens[i] != "-u")
                {
                    double a = parseDouble(S.Pop().ToString());
                    double b = parseDouble(S.Pop().ToString());
                    a = evalOp(a, b, tokens[i]);
                    S.Push(a);
                }
                else if (tokens[i] == "x")
                {

                    S.Push(x);
                }

            }
            return parseDouble(S.Pop().ToString());
        }
        public List<string> evaluatePostfix(double x_min, double x_max, int n)
        {
            List<string> resultRange = new List<string>();
            double dx = (x_max - x_min) / (n-1);
            string limiter = x_min.ToString() + "#";
            for (int j = 0; j < n; j++)
            {
                limiter = x_min.ToString() + "#";
                try
                {
                    limiter += evaluatePostfix(x_min).ToString();
                }
                catch (Exception ex)
                {
                    limiter += ex.Message;
                    evalError = true;
                }
                resultRange.Add(limiter);
                x_min += dx;
            }
            return resultRange;
        }
        public static bool isOperator(string op)
        {
            switch (op)
            {
                case "+": return true;
                case "-": return true;
                case "*": return true;
                case "/": return true;
                case "^": return true;
                case "(": return true;
                default: return false;

            }
        }
        public static int getPriority(string op)
        {
            if (properDict.TryGetValue(op, out _)) 
                return properDict[op];
            else 
                return 10;
        }
        public static bool isNumber(string token)
        {
            return double.TryParse(token, out _);
             
        }
        public double evalFun(double a, string fun)
        {
            switch (fun)
            {
                case "-u": 
                    return (-1) * a;
                case "sin": 
                    return Math.Sin(a);
                case "cos": 
                    return Math.Cos(a);
                case "sinh":
                    return Math.Sinh(a);
                case "cosh":
                    return Math.Cosh(a);
                case "abs": 
                    return Math.Abs(a);
                case "exp": 
                    return Math.Exp(a);
                case "log":
                    if (a > 0)
                        return Math.Log(a);
                    else 
                    { 
                        throw new DomainErrorException("Domain error for log function, check your formula");
                    }
                case "sqrt": 
                    if (a >= 0) 
                        return Math.Sqrt(a);
                    else
                    { 
                        throw new DomainErrorException("Domain error for sqrt function, check your formula");
                    }
                case "tan": 
                    return Math.Tan(a);
                case "tanh": 
                    return Math.Tanh(a);
                case "acos": 
                    if(a>=-1 && a<=1) return Math.Acos(a);
                    else 
                    { 
                        throw new DomainErrorException("Domain error for acos function, check your formula");
                    }
                case "asin": 
                    if(a>=-1 & a<=1) 
                        return Math.Asin(a);
                    else
                    { 
                         //this.domainError = true;
                         throw new DomainErrorException("Domain error for asin function, check your formula");
                    }
                case "atan": return Math.Atan(a);
            }
            return 0.0;
        }
        public double evalOp(double a, double b, string op)
        {
            switch (op)
            {
                case "+": 
                    return a + b;
                case "-": 
                    return b - a;
                case "*": 
                    return a * b;
                case "/": 
                    if( a!=0) 
                        return b / a; 
                    else 
                    { 
                        throw new DivideByZeroException("Division error, can't divide by zero");
                    }
                case "^": return Math.Pow(b, a);
            }
            return -3.42;
        }
        public bool checkTokensValidity(List<string> tokens)
        {
            List<string> possibleTokens = new List<string> { "abs", "cos", "exp", "log", "sin", "sqrt", "tan", "cosh", "sinh", "tanh", "acos", "asin", "atan", "^", "*", "/", "+", "-", "(", ")", "x", "-u" };
            foreach (string t in tokens)
            {
                string t2 = t.Replace(',', '.');
                if (possibleTokens.Contains(t2) || double.TryParse(t2, out _) || int.TryParse(t2,out _)) 
                    continue; 
                else 
                    throw new Exception("Invalid formula");
            }
            return true;
        }
        public static double parseDouble(string value){
            try
            {
                double result;

                // Try parsing in the current culture
                if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                    // Then try in US english
                    !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                    // Then in neutral language
                    !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                {
                    throw new Exception("Bad Number: " + value);
                }
                return result;  
                //return double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            catch
            { 
                throw new Exception("Bad number: " + value);
            }
        }
    }
}
