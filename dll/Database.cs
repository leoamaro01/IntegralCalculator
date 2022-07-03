namespace lib;

public static class Database
{
    //Añadir un operador nuevo se limita con esta configuración a solamente definir su conversión a la forma prefija 
    //y el delegado que contiene su definición

    //Key = operador, Value= delegado que contiene la función del operador binario
    public static Dictionary<string, Func<decimal, decimal, decimal>> BynaryStringToExpression = new()
    {
        {"sum" , (x,y)=>x+y},
        {"sub" , (x,y)=>x-y},
        {"mult" , (x,y)=>x*y},
        {"div" , (x,y)=>x/y},
        {"pow" , (x,y)=>(decimal)Math.Pow((double)x,(double)y)}
        };

    //Key = operador, Value= delegado que contiene la función del operador unario
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
        {"ln" , (x)=>(decimal)Math.Log((double)x)}
    };
    //Conversión del operador binario infijo al prefijo
    public static Dictionary<string, string> binaryOperators = new()
    {
        { "+", "sum" },
        { "-", "sub" },
        { "*", "mult" },
        { "/", "div" },
        { "^", "pow" }
    };
    //Conversión del operador unario infijo al prefijo
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
        { "csc", "csc" }
    };
}