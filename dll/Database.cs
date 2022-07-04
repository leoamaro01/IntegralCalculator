namespace lib;

public static class Database
{
    public static Dictionary<string, Func<decimal, decimal, decimal>> BynaryStringToExpression = new()
    {
        {"sum" , (x,y)=>x+y},
        {"sub" , (x,y)=>x-y},
        {"mult" , (x,y)=>x*y},
        {"div" , (x,y)=>x/y},
        {"pow" , (x,y)=>(decimal)Math.Pow((double)x,(double)y)}
        };
    public static Dictionary<string, Func<decimal, decimal>> UnaryStringToExpression = new()
    {
        { "sin",(x)=> (decimal)Math.Sin((double)x)},
        { "cos",(x)=> (decimal)Math.Cos((double)x)},
        { "tan",(x)=> (decimal)Math.Tan((double)x)},
        { "cot",(x)=> (decimal)(1/Math.Tan((double)x))},
        { "sec",(x)=> (decimal)(1/Math.Cos((double)x))},
        { "csc",(x)=> (decimal)(1/Math.Sin((double)x))},
        {"log2" , (x)=>(decimal)Math.Log((double)x,2)},
        {"log10" , (x)=>(decimal)Math.Log10((double)x)},
        {"log" , (x)=>(decimal)Math.Log10((double)x)},
        {"ln" , (x)=>(decimal)Math.Log((double)x)},
        {"arccos",(x)=>(decimal)Math.Acos((double)x)},
        {"arcsin",(x)=>(decimal)Math.Asin((double)x)},
        {"sinh",(x)=>(decimal)Math.Sinh((double)x)},
        {"cosh",(x)=>(decimal)Math.Cosh((double)x)},
        {"arcsinh",(x)=>(decimal)Math.Asinh((double)x)},
        {"arccosh",(x)=>(decimal)Math.Acosh((double)x)}

    };
    public static Dictionary<string, string> binaryOperators = new()
    {
        { "+", "sum" },
        { "-", "sub" },
        { "*", "mult" },
        { "/", "div" },
        { "^", "pow" }
    };
    public static Dictionary<string, string> unaryOperators = new()
    {
        { "log2", "log2" },
        { "log10", "log10" },
        { "log", "log" },
        { "ln", "ln" },
        { "sin", "sin" },
        { "cos", "cos" },
        { "tan", "tan" },
        { "cot", "cot" },
        { "sec", "sec" },
        { "csc", "csc" },
        {"arccos","arccos"},
        {"arcsin","arcsin"},
        {"sinh","sinh"},
        {"cosh","cosh"},
        {"arcsinh","arcsinh"},
        {"arccosh","arccosh"}

    };
}