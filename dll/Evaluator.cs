using System.Linq;
namespace lib;
public static class Evaluator
{
    public static event Action<string, double>? ChangeValue;
    public static List<string> IDs = new();

    /// <summary>
    ///  Resetea a valores por defecto las propiedades del Evaluator
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static void Reset()
    {
        expression = null;
    }
    private static Expression? expression;
    public static void SetExpresion(Expression exp)
    {
        expression = exp;
    }
    public static double Evaluate(double[] values)
    {
        if (expression == null)
        {
            throw new Exception("Expression is not assigned");
        }
        //Le asigna el valor de la x actual a la(s) variable(s) contenida(s) en la expresión para obtener el f(x1,...,xn)
        for (int i = 0; i < values.Length; i++)
        {
            ChangeValue?.Invoke(IDs[i], values[i]);
        }
        return expression.Evaluate();
    }
}