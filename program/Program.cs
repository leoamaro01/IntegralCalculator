using System.Diagnostics;
using lib;
namespace cnsle;

public static class Program
{
    static string function = string.Empty;
    static double Function(double input)
    {
        return Evaluator.Evaluate(input);
    }
    static void SetEvaluator(double lower, double higher)
    {
        Evaluator.Reset();
        Interpreter interpreter = new();
        Evaluator.SetExpresion(interpreter.GetExpression(interpreter.Parse(function)));
    }
    static void PaintMenu()//Imprime la parte de los operadores a utilizar 
    {
        Console.Clear();
        Console.WriteLine("Esta calculadora se basa en la integración mediante las sumas de Riemann.");
        Console.WriteLine("Si desea usar un límite infinito positivo introduzca la palabra inf o -inf para el negativo.");
        Console.WriteLine("Operadores disponibles:");
        var binaryOperators = Database.binaryOperators.Keys.ToList();
        var unaryOperators = Database.unaryOperators.Keys.ToList();
        Console.WriteLine("Binary Operators:");
        binaryOperators.ForEach((x) => Console.Write(x + " "));
        Console.WriteLine("");
        Console.WriteLine("Unary Operators:");
        unaryOperators.ForEach((x) => Console.Write(x + " "));
        Console.WriteLine("");
    }
    static void Main(string[] args)
    {
        PaintMenu();
        string? read = Request("Introduzca la función a integrar:").ToLower();
        function = (read == null) ? throw new Exception("Invalid function") : read;

        IntegralCalculator.optimalDivisionsPerUnit = 1000000;

        string sLower = Request("Límite inferior:").ToLower();
        string sHigher = Request("Límite superior:").ToLower();
        double lower = GetLimit(sLower);
        double higher = GetLimit(sHigher);
        SetEvaluator(lower, higher);
        Console.Write($"La integral definida de la funcion {function} entre {sLower} y {sHigher} es: ");

        Stopwatch watch = new();
        watch.Start();

        double integral = IntegralCalculator.Calculate(lower,
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
    static Expression SetExpression(double input)
    {
        Expression e = new BinaryGeneric(
            new UnaryGeneric(
                new Const((double)input),
                ((x) => (double)Math.Exp((double)x))
            ),
            new UnaryGeneric(
                new Const(input),
                ((x) => (double)Math.Sin((double)x))
            ),
            ((x, y) => x * y)
        );
        return e;
    }
    static double GetLimit(string limit)
    {
        double result = 0m;
        switch (limit)
        {
            case "-inf":
                return -(double)(Math.Pow(10, 2));
            case "inf":
                return (double)(Math.Pow(10, 2));
            default:
                {
                    if (!double.TryParse(limit, out result))
                    {
                        throw new Exception("Límite inválido");
                    }
                    return result;
                }
        }
    }
}