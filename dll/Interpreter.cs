using System.Runtime.InteropServices;
using System.Linq;
namespace lib;
public class Interpreter
{
    static readonly Dictionary<string, string> binaryOperators;
    static readonly Dictionary<string, string> unaryOperators;

    static Interpreter()
    {
        binaryOperators = new()
        {
            { "+", "sum" },
            { "-", "sub" },
            { "*", "mult" },
            { "/", "div" },
            { "^", "pow" },
            { "log", "log" }
        };
        unaryOperators = new()
        {
            { "ln", "ln" },
            { "arcsin", "arcsin" },
            { "arccos", "arccos" },
            { "arctan", "arctan" },
            { "arccot", "arccot" },
            { "sin", "sin" },
            { "cos", "cos" },
            { "tan", "tan" },
            { "cot", "cot" },
            { "sec", "sec" },
            { "csc", "csc" }
        };
    }
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
            return $"{unaryOperators[op]}({GetParse(lhs)})";

        return $"{binaryOperators[op]}({GetParse(lhs)},{GetParse(rhs)})";
    }
    private static bool IsConstant(string s)
        => binaryOperators.Keys.All(op => !s.Contains(op)) &&
                unaryOperators.Keys.All(op => !s.Contains(op));
    private static (string lhs, string? rhs) GetExpressions(string s, out string op)
    {
        foreach (var oper in binaryOperators.Keys)
        {
            int i = s.IndexOf(oper);
            if (i >= 0)
            {
                op = oper;
                return (s[..i], s[(i + 1)..]);
            }
        }

        foreach (var oper in unaryOperators.Keys)
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
}