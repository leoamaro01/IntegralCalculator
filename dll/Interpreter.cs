using System.Runtime.InteropServices;
using System.Linq;
namespace lib;
public class Interpreter
{
    static Interpreter()
    { }
    #region Parsing
    List<string> substitutions = new();
    public string Prepare(string s)
    {
        s = s.Replace(" ", "");
        s = SearchParenthesis(s);
        return s;
    }
    public string Parse(string s)
    {
        //sum(sin(x+1), {5})
        s = GetParse(s);

        string[] subsArray = substitutions.ToArray();
        for (int i = 0; i < subsArray.Length; i++)
        {
            subsArray[i] = GetParse(subsArray[i]);

            if (i != 0)
                subsArray[i] = string.Format(subsArray[i], subsArray[..i]);
        }

        s = string.Format(s, subsArray);

        substitutions.Clear();

        return s;
    }
    private string GetParse(string s)
    {
        s = DelParenthesis(s);
        if (IsConstant(s))
            return s;

        (string lhs, string? rhs) = GetExpressions(s, out string op);

        if (rhs == null)
            return $"{Database.unaryOperators[op]}({GetParse(lhs)})";

        return $"{Database.binaryOperators[op]}({GetParse(lhs)},{GetParse(rhs)})";
    }
    private static bool IsConstant(string s)
        => Database.binaryOperators.Keys.All(op => !s.Contains(op)) &&
                Database.unaryOperators.Keys.All(op => !s.Contains(op));
    private static (string lhs, string? rhs) GetExpressions(string s, out string op)
    {
        foreach (var oper in Database.binaryOperators.Keys)
        {
            int i = s.IndexOf(oper);
            if (i >= 0)
            {
                op = oper;
                return (s[..i], s[(i + 1)..]);
            }
        }

        foreach (var oper in Database.unaryOperators.Keys)
        {
            if (s.Contains(oper))
            {
                op = oper;
                return (s[oper.Length..], null);
            }
        }

        throw new ArgumentException("Invalid expression");
    }

    private static string DelParenthesis(string s)
    {
        while (s[0] == '(' && s[^1] == ')')
        {
            string temp = s[1..^1];
            int count = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (count < 0)
                {
                    return s;
                }
                if (temp[i] == '(')
                    count++;
                if (temp[i] == ')')
                    count--;
            }
            if (count != 0)
                return s;
            else
                s = temp;
        }
        return s;
    }
    private string SearchParenthesis(string s)
    {
        string prev = "";
        string curr = s;
        while (curr != prev)
        {
            int from = -1;
            int to = curr.Length;
            for (int i = 0; i < curr.Length; i++)
            {
                if (curr[i] == '(')
                    from = i;
                if (curr[i] == ')')
                {
                    to = i + 1;
                    break;
                }
            }
            if (from == -1)
                break;
            prev = curr;
            substitutions.Add(curr[from..to]);
            curr = curr.Replace(substitutions[^1], $"{{{substitutions.Count - 1}}}");
        }
        return curr;
    }
    #endregion

    #region  ConvertToExpression
    public Expression GetExpression(string s)
    {
        if (!s.Any(x => x == '('))
        {
            if (s == "e")
                return new Const((decimal)Math.E);
            if (s == "π")
                return new Const((decimal)Math.PI);
            decimal number = -1;
            if (decimal.TryParse(s, out number))
                return new Const(number);
            else
                return new Variable();
        }
        string oper = "";
        int comaIndex = -1;
        int from = 0;
        int to = s.Length - 1;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(')
            {
                from = i + 1;
                int count = 0;
                for (int j = i + 1; j < s.Length; j++)
                {
                    if (s[j] == '(')
                        count++;
                    if (s[j] == ')')
                        count--;
                    if (s[j] == ',' && count == 0)
                    {
                        to = j;
                        comaIndex = j;
                        i = s.Length;
                        break;
                    }
                }
                i = s.Length;
            }
            else
                oper += s[i];
        }
        if (comaIndex != -1)
            return new BinaryGeneric(GetExpression(s[from..to]), GetExpression(s[(to + 1)..^1]), Database.BynaryStringToExpression[oper]);
        else
            return new UnaryGeneric(GetExpression(s[from..to]), Database.UnaryStringToExpression[oper]);
    }
    #endregion
}