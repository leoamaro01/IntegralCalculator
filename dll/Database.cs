namespace lib;

public static class Database
{
    public static Dictionary<string, Func<double, double, double>> BynaryStringToExpression = new()
    {
        {"sum" , (x,y)=>x+y},
        {"sub" , (x,y)=>x-y},
        {"mult" , (x,y)=>x*y},
        {"div" , (x,y)=>x/y},
        {"pow" , (x,y)=>(double)Math.Pow((double)x,(double)y)}
        };
    public static Dictionary<string, Func<double, double>> UnaryStringToExpression = new()
    {
        {"arccos",(x)=>(double)Math.Acos((double)x)},
        {"arcsin",(x)=>(double)Math.Asin((double)x)},
        {"sinh",(x)=>(double)Math.Sinh((double)x)},
        {"cosh",(x)=>(double)Math.Cosh((double)x)},
        {"arcsinh",(x)=>(double)Math.Asinh((double)x)},
        {"arccosh",(x)=>(double)Math.Acosh((double)x)},
        { "sin",(x)=> (double)Math.Sin((double)x)},
        { "cos",(x)=> (double)Math.Cos((double)x)},
        { "tan",(x)=> (double)Math.Tan((double)x)},
        { "cot",(x)=> (double)(1/Math.Tan((double)x))},
        { "sec",(x)=> (double)(1/Math.Cos((double)x))},
        { "csc",(x)=> (double)(1/Math.Sin((double)x))},
        {"log2" , (x)=>(double)Math.Log((double)x,2)},
        {"log10" , (x)=>(double)Math.Log10((double)x)},
        {"log" , (x)=>(double)Math.Log10((double)x)},
        {"ln" , (x)=>(double)Math.Log((double)x)}
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
        {"arccos","arccos"},
        {"arcsin","arcsin"},
        {"sinh","sinh"},
        {"cosh","cosh"},
        {"arcsinh","arcsinh"},
        {"arccosh","arccosh"},
        { "log2", "log2" },
        { "log10", "log10" },
        { "log", "log" },
        { "ln", "ln" },
        { "sin", "sin" },
        { "cos", "cos" },
        { "tan", "tan" },
        { "cot", "cot" },
        { "sec", "sec" },
        { "csc", "csc" }
    };
    public static Dictionary<string, double> constToNumber = new()
    {
        {"e",(double)Math.E},
        {"pi",(double)Math.PI},
        {"inf",(double)double.PositiveInfinity}
    };
}