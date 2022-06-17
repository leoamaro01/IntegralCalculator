using System.Diagnostics;
using lib;
using System;
using System.Threading;
namespace cnsle;

public static class Program
{
    static string functionName = "e^x * sinx";
    static decimal Function(decimal input)
    {
        // Insert function here
        return (decimal)(Math.Exp((double)input) * Math.Sin((double)input));
    }
    static void Main(string[] args)
    {
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
}
