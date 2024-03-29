﻿using System.Diagnostics;
using lib;
namespace cnsle;

public static class Program
{
    const int _precissionIncrementFactor = 5;
    const double _optimalError = 0.01;
    const int _midpoints = 2;
    const int _maxIntervals = 1000000;
    static string function = string.Empty;
    static Interpreter interpreter = new Interpreter();
    static Evaluator? evaluator;
    static double Function(double[] input)
    {
        if (evaluator == null)
        {
            throw new Exception("Expression is not assigned");
        }
        return evaluator.Evaluate(input);
    }
    static Evaluator SetEvaluator()
    {
        return new(interpreter.GetExpression(interpreter.Parse(function)), interpreter.IDs);
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
        while (true)
        {
            try
            {
                PaintMenu();
                string? read = Request("Introduzca la función a integrar:").ToLower();
                function = read ?? throw new Exception("Invalid function");

                evaluator = SetEvaluator();
                string sLower = RequestLimits("Límite inferior:").ToLower();
                string sHigher = RequestLimits("Límite superior:").ToLower();

                double[] lower = GetLimit(sLower);
                double[] higher = GetLimit(sHigher);
                Console.Write($"La integral definida de la funcion {function} entre {sLower} y {sHigher} es: ");

                Stopwatch watch = new();
                watch.Start();

                IntegralCalculator.optimalDivisionsPerUnit = 10;
                int[] startingPrecissions = new int[lower.Length];
                for (int i = 0; i < startingPrecissions.Length; i++)
                {
                    startingPrecissions[i] = IntegralCalculator.GetOptimalPrecission(Math.Abs(higher[i] - lower[i]));
                }

                double integral = IntegralCalculator.OptimalCalculate(lower,
                                    higher,
                                    Function,
                                    ref startingPrecissions, optimalError: _optimalError, midpoints: _midpoints, maxIntervals: _maxIntervals, precissionIncrementFactor: _precissionIncrementFactor);

                watch.Stop();

                Console.WriteLine($"{integral} ~ {Math.Round(integral, 3)}");
                Console.WriteLine($"Calculado con un total de {startingPrecissions.Aggregate((a, e) => a * e)} intervalos de suma de Riemann.");
                Console.WriteLine($"Calculado en {watch.ElapsedMilliseconds}ms");
                return;
            }
            catch (IntegralException)
            {
                Console.WriteLine("Invalid function");
                Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(500);
            }
        }
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
        for (int i = 0; i < interpreter.IDs.Count; i++)
        {
            Console.Write($"{interpreter.IDs[i]}: ");
            result += Console.ReadLine() + ",";
        }
        Console.WriteLine("");
        return result[..^1];


    }
    static double[] GetLimit(string limit)
    {
        string[] limits = limit.Split(',');
        double[] result = new double[limits.Length];
        for (int i = 0; i < limits.Length; i++)
        {
            Interpreter interpreter = new();
            Evaluator evaluator = new(interpreter.GetExpression(interpreter.Parse(limits[i])), interpreter.IDs);
            result[i] = evaluator.Evaluate();
        }
        return result;
    }
}