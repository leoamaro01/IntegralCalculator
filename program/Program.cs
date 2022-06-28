using System.Diagnostics;
using lib;
namespace cnsle;

public static class Program
{
    static string function = "";
    static decimal Function(decimal input)
    {
        return Evaluator.Evaluate(input);
    }
    static void SetEvaluator()
    {
        Evaluator.Reset();
        string? read = Request("Introduzca la funcion:").ToLower();
        function = (read == null) ? throw new Exception("Invalid function") : read;
        Interpreter interpreter = new();
        Evaluator.SetExpresion(interpreter.GetExpression(Parse(interpreter, function)));
    }
    static void Main(string[] args)
    {
        SetEvaluator();
        IntegralCalculator.optimalDivisionsPerUnit = 10000;

        decimal lower = decimal.Parse(Request("Limite inferior:"));
        decimal higher = decimal.Parse(Request("Limite superior:"));

        Console.Write($"La integral definida de la funcion {function} entre {lower} y {higher} es: ");

        Stopwatch watch = new();
        watch.Start();

        decimal integral = IntegralCalculator.Calculate(lower,
                            higher,
                            IntegralCalculator.GetOptimalPrecission(higher - lower),
                            Function);
        watch.Stop();

        Console.WriteLine($"{integral} ~ {Math.Round(integral, 3)}");
        Console.WriteLine($"Calculado en {watch.ElapsedMilliseconds}ms");
    }
    static string Request(string query)
    {
        Console.WriteLine(query);
        return Console.ReadLine() ?? "";
    }
    static Expression SetExpression(decimal input)
    {
        Expression e = new BinaryGeneric(
            new UnaryGeneric(
                new Const((decimal)input),
                ((x) => (decimal)Math.Exp((double)x))
            ),
            new UnaryGeneric(
                new Const(input),
                ((x) => (decimal)Math.Sin((double)x))
            ),
            ((x, y) => x * y)
        );
        return e;
    }
    static string Parse(Interpreter interpreter, string s)
    {
        s = interpreter.Prepare(s);
        var stringParsed = interpreter.Parse(s);
        return stringParsed;
    }
}
