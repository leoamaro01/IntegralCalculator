﻿using System;
using System.Linq;
namespace lib;
public class IntegralCalculator
{
    public static double midpoint = 0.5;
    public static int optimalDivisionsPerUnit = 100;

    /// <summary>
    ///  Obtiene el rango de división óptimo para dividir el intervalo dado
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static int GetOptimalPrecission(double intervalLength)
    => (int)Math.Ceiling(intervalLength * optimalDivisionsPerUnit);

    public static double OptimalCalculate(
        double[] lowerLimits,
        double[] higherLimits,
        Func<double[], double> function,
        ref int[] startingPrecissions,
        int precissionIncrementFactor = 10,
        double optimalError = 0.0001,
        int midpoints = 3
    )
    {
        double tempMidpoint = midpoint;
        double[] integrals = new double[midpoints];

        void CalculateIntegrals(int[] precissions)
        {
            for (int i = 0; i < midpoints; i++)
            {
                midpoint = i / (midpoints - 1.0);

                integrals[i] = Calculate(lowerLimits, higherLimits, precissions, function);
            }
        }
        bool AreIntegralsWithinError()
        {
            return (integrals.Max() - integrals.Min()) <= optimalError;
        }

        CalculateIntegrals(startingPrecissions);
        if (AreIntegralsWithinError())
            return integrals.Average();

        do
        {
            for (int i = 0; i < startingPrecissions.Length; i++)
                startingPrecissions[i] *= precissionIncrementFactor;

            CalculateIntegrals(startingPrecissions);
        }
        while (!AreIntegralsWithinError());

        midpoint = tempMidpoint;

        return integrals.Average();
    }

    public static double Calculate(
        double[] lowerLimits,
        double[] higherLimits,
        int[] precissions,
        Func<double[], double> function)
    {
        if (higherLimits == lowerLimits)
            return 0.0;

        double interval = CalculateHyperIntervals(lowerLimits, higherLimits, precissions);

        double[][] values = CalculateHyperValues(lowerLimits, interval, precissions);

        double sum = 0;

        foreach (var value in values)
            sum += function(value);

        return sum * interval;
    }

    public static double[][] CalculateHyperValues(double[] lowerLimits, double interval, int[] precissions)
    {
        void RecursiveIterator(List<double[]> points,
                                  double[] currentPoint,
                                  int currentDimension)
        {
            if (currentDimension == lowerLimits.Length)
            {
                points.Add(currentPoint);
                return;
            }

            double[] basePoint = (double[])currentPoint.Clone();

            for (int i = 0; i < precissions[currentDimension]; i++)
            {
                double dimensionValue = lowerLimits[currentDimension] + (i * interval) + (midpoint * interval);
                basePoint[currentDimension] = dimensionValue;
                RecursiveIterator(points, basePoint, i + 1);
            }
        }

        List<double[]> points = new();
        RecursiveIterator(points, new double[lowerLimits.Length], 0);

        return points.ToArray();
    }
    public static double CalculateHyperIntervals(double[] lowerLimits, double[] higherLimits, int[] precissions)
    {
        double[] intervals = new double[lowerLimits.Length];

        for (int i = 0; i < intervals.Length; i++)
            intervals[i] = (lowerLimits[i] - higherLimits[i]) / precissions[i];

        double space = 1;

        foreach (var interval in intervals)
            space *= interval;

        return space;
    }
}
