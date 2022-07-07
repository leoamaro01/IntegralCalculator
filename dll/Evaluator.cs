using System.Linq;
namespace lib;
public class Evaluator
{
    public List<string> IDs;
    private Expression? expression;
    public static event Action<string, double>? ChangeValue;
    public Evaluator(Expression exp, List<string> IDs)
    {
        IDs.Sort();
        this.IDs = IDs;
        this.expression = exp;
    }
    public double Evaluate(double[] values)
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
    /// <summary>
    ///  Evalua una expresion q no tenga variables
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public double Evaluate()
    {
        if (expression == null)
        {
            throw new Exception("Expression is not assigned");
        }
        return expression.Evaluate();
    }
}