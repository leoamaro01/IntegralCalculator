using System.Diagnostics;
using lib;
using System;
using System.Threading;
namespace cnsle;

public static class Program
{
    static string functionName = "e^(x) * sin(x)";
    static decimal Function(decimal input)
    {
        return (decimal)(SetExpression(input).Evaluate());
        // Insert function here
        //return (decimal)(Math.Exp((double)input) * Math.Sin((double)input));
    }
    static void Main(string[] args)
    {
        StartParsing("x*(3+2)");
        IntegralCalculator.optimalDivisionsPerUnit = 10000;

        decimal lower = decimal.Parse(Request("Limite inferior:"));
        decimal higher = decimal.Parse(Request("Limite superior:"));

        Console.Write($"La integral definida de la funcion {functionName} entre {lower} y {higher} es: ");

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
    static void StartParsing(string s)
    {
        Interpreter interpreter = new();
        s = interpreter.Prepare(s);
        var stringParsed = interpreter.Parse(s);
        Console.WriteLine(stringParsed);

    }
}
