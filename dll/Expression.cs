namespace lib;

public abstract class Expression
{
    public abstract double Evaluate();
}
public class BinaryGeneric : Expression
{
    Expression left;
    Expression right;
    Func<double, double, double> func;
    public BinaryGeneric(Expression left, Expression right, Func<double, double, double> func)
    {
        this.left = left;
        this.right = right;
        this.func = func;
    }
    public override double Evaluate()
    {
        return Evaluate(left.Evaluate(), right.Evaluate());
    }
    private double Evaluate(double left, double right)
    {
        return this.func(left, right);
    }
}
public class UnaryGeneric : Expression
{
    Expression expression;
    Func<double, double> func;
    public UnaryGeneric(Expression e, Func<double, double> func)
    {
        this.expression = e;
        this.func = func;
    }
    public override double Evaluate()
    {
        return Evaluate(expression.Evaluate());
    }
    protected double Evaluate(double value)
    {
        return this.func(value);
    }
}
public class Const : Expression
{
    readonly double value;
    public Const(double value)
    {
        this.value = value;
    }

    public override double Evaluate()
    {
        return value;
    }
}
public class Variable : Expression
{
    public readonly string ID;
    private double x;
    public Variable(string ID)
    {
        this.ID = ID;
        Evaluator.ChangeValue += WatchValue;
    }

    public override double Evaluate()
    {
        return x;
    }


    private void WatchValue(string ID, double x)
    {
        if (ID == this.ID)
            this.x = x;
    }
}
