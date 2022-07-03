using System;
namespace lib;
public class IntegralCalculator
{
    public static decimal midpoint = 0.5m;
    public static int optimalDivisionsPerUnit = 100;

    /// <summary>
    ///  Obtiene el rango de división óptimo para dividir el intervalo dado
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static int GetOptimalPrecission(decimal intervalLength)
    => (int)Math.Ceiling(intervalLength * optimalDivisionsPerUnit);

    public static decimal Calculate(
        decimal lowerLimit,
        decimal higherLimit,
        int precission,
        Func<decimal, decimal> function)
    {
        if (higherLimit == lowerLimit)
            return 0m;
        decimal delta = (higherLimit - lowerLimit) / precission;
        decimal sum = 0;

        for (int i = 0; i < precission; i++)
            sum += function(lowerLimit + (delta * i) + (delta * midpoint)) * delta;

        return sum;
    }
}
