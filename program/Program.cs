using System.Linq.Expressions;
using System.Diagnostics;
using lib;
using Expression = lib.Expression;
namespace cnsle;

public static class Program
{
    static string function = string.Empty;
    static double Function(double[] input)
    {
        return Evaluator.Evaluate(input);
    }
    static void SetEvaluator()
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
        function = read ?? throw new Exception("Invalid function");

        IntegralCalculator.optimalDivisionsPerUnit = 1000000;
        SetEvaluator();
        string sLower = Request("Límite inferior:").ToLower();
        string sHigher = Request("Límite superior:").ToLower();

        double[] lower = GetLimit(sLower);
        double[] higher = GetLimit(sHigher);
        Console.Write($"La integral definida de la funcion {function} entre {sLower} y {sHigher} es: ");

        Stopwatch watch = new();
        watch.Start();

        IntegralCalculator.optimalDivisionsPerUnit = 5;
        int[] startingPrecissions = new int[lower.Length];
        for (int i = 0; i < startingPrecissions.Length; i++)
        {
            startingPrecissions[i] = IntegralCalculator.GetOptimalPrecission(Math.Abs(higher[i] - lower[i]));
        }

        double integral = IntegralCalculator.OptimalCalculate(lower,
                            higher,
                            Function,
                            ref startingPrecissions);

        watch.Stop();

        Console.WriteLine($"{integral} ~ {Math.Round(integral, 3)}");
        Console.WriteLine($"Calculado en {watch.ElapsedMilliseconds}ms");
    }
    static string Request(string query)
    {
        Console.WriteLine(query);
        return Console.ReadLine() ?? "";
    }

    static string RequestLimits(string query)
    {
        string result = "";
        Console.WriteLine(query);
        for (int i = 0; i < Evaluator.IDs.Count; i++)
        {
            Console.Write($"{Evaluator.IDs[i]}: ");
            result += Console.ReadLine() + ",";
            Console.WriteLine("");
        }
        return result[..^1];


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
    static double[] GetLimit(string limit)
    {
        string[] limits = limit.Split(',');
        double[] result = new double[limits.Length];
        for (int i = 0; i < result.Length; i++)
        {
            switch (limits[i])
            {
                case "-inf":
                    {
                        result[i] = -(double)(Math.Pow(10, 2));
                        break;
                    }
                case "inf":
                    {
                        result[i] = (double)(Math.Pow(10, 2));
                        break;
                    }
                default:
                    {
                        if (!double.TryParse(limits[i], out result[i]))
                        {
                            throw new Exception("Límite inválido");
                        }
                        break;
                    }
            }
        }
        return result;
    }
}