namespace lib;
public static class Evaluator
{
    public static event Action<decimal>? ChangeValue;
    public static void Reset()
    {
        expression = null;
    }
    private static Expression? expression;
    public static void SetExpresion(Expression exp)
    {
        expression = exp;
    }
    public static decimal Evaluate(decimal x)
    {
        if (expression == null)
        {
            throw new Exception("Expression is not assigned");
        }
        ChangeValue?.Invoke(x);
        return expression.Evaluate();
    }
}