namespace lib;

public abstract class Expression
{
    public abstract decimal Evaluate();
}
public class BinaryGeneric : Expression
{
    Expression left;
    Expression right;
    Func<decimal, decimal, decimal> func;
    public BinaryGeneric(Expression left, Expression right, Func<decimal, decimal, decimal> func)
    {
        this.left = left;
        this.right = right;
        this.func = func;
    }
    public override decimal Evaluate()
    {
        return Evaluate(left.Evaluate(), right.Evaluate());
    }
    private decimal Evaluate(decimal left, decimal right)
    {
        return func(left, right);
    }
}
public class UnaryGeneric : Expression
{
    Expression expression;
    Func<decimal, decimal> func;
    public UnaryGeneric(Expression e, Func<decimal, decimal> func)
    {
        this.expression = e;
        this.func = func;
    }
    public override decimal Evaluate()
    {
        return Evaluate(expression.Evaluate());
    }
    protected decimal Evaluate(decimal value)
    {
        return this.func(value);
    }
}
public class Const : Expression
{
    readonly decimal value;
    public Const(decimal value)
    {
        this.value = value;
    }

    public override decimal Evaluate()
    {
        return value;
    }
}
public class Variable : Expression
{
    private decimal x;
    public Variable()
    {
        Evaluator.ChangeValue += WatchValue;
    }

    public override decimal Evaluate()
    {
        return x;
    }

    private void WatchValue(decimal x)
    {
        this.x = x;
    }
}
