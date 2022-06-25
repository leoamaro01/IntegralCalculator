namespace lib;

public abstract class Expression
{
    public abstract decimal Evaluate();
}
public abstract class BinaryExpression : Expression
{
    readonly Expression left;
    readonly Expression right;

    public BinaryExpression(Expression left, Expression right)
    {
        this.left = left;
        this.right = right;
    }
    public override decimal Evaluate()
    {
        return Evaluate(left.Evaluate(), right.Evaluate());
    }
    protected abstract decimal Evaluate(decimal left, decimal right);
}
public abstract class UnaryExpression : Expression
{
    readonly Expression expression;
    public UnaryExpression(Expression e)
    {
        this.expression = e;
    }
    public override decimal Evaluate()
    {
        return Evaluate(expression.Evaluate());
    }
    protected abstract decimal Evaluate(decimal value);
}
public class BinaryGeneric : BinaryExpression
{
    Func<decimal, decimal, decimal> func;
    public BinaryGeneric(Expression left, Expression right, Func<decimal, decimal, decimal> func) : base(left, right)
    {
        this.func = func;
    }

    protected override decimal Evaluate(decimal left, decimal right)
    {
        return func(left, right);
    }
}
public class UnaryGeneric : UnaryExpression
{
    Func<decimal, decimal> func;
    public UnaryGeneric(Expression e, Func<decimal, decimal> func) : base(e)
    {
        this.func = func;
    }

    protected override decimal Evaluate(decimal value)
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
