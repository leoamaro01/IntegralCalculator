using System.Collections.Immutable;
namespace lib;
public class Interpreter
{
    private Dictionary<string, string> binaryOperators;
    private Dictionary<string, string> unaryOperators;

    private Dictionary<char, string> sustitutions;
    private char actualLetter;

    public Interpreter()
    {
        sustitutions = new();
        actualLetter = 'A';
        unaryOperators = new();
        binaryOperators = new();
        binaryOperators.Add("+", "sum");
        binaryOperators.Add("-", "sub");
        binaryOperators.Add("*", "mult");
        binaryOperators.Add("/", "div");
        binaryOperators.Add("^", "pow");
        binaryOperators.Add("log", "log");//It´s going to ask it´s base

        unaryOperators.Add("ln", "ln");
        unaryOperators.Add("sin", "sin");
        unaryOperators.Add("cos", "cos");
        unaryOperators.Add("tan", "tan");
        unaryOperators.Add("cot", "cot");
        unaryOperators.Add("sec", "sec");
        unaryOperators.Add("csc", "csc");
        unaryOperators.Add("arcsin", "arcsin");
        unaryOperators.Add("arccos", "arccos");
        unaryOperators.Add("arctan", "arctan");
        unaryOperators.Add("arccot", "arccot");
    }
    public string Prepare(string s)
    {
        s = DelSpaces(s);
        s = SearchParenthesis(s);
        return s;
    }
    private string DelSpaces(string s)
    {
        var deletedSpaces = s.Where((x) => (x != ' '));
        if (deletedSpaces == null)
            return "";
        string res = "";
        foreach (var c in deletedSpaces)
        {
            res += c;
        }
        return res;
    }
    public string Parse(string s)
    {
        s = GetParse(s);
        string past = "";
        string actual = s;
        while (actual != past)
        {
            past = actual;
            for (int i = 0; i < actual.Length; i++)
            {
                if (sustitutions.ContainsKey(actual[i]))
                {
                    var temp = GetParse(sustitutions[actual[i]]);
                    actual = actual.Remove(i, 1);
                    actual = actual.Insert(i, temp);
                }
            }
        }
        return actual;
    }
    private string GetParse(string s)
    {
        s = DelParenthesis(s);
        if (IsConstant(s))
        {
            return s;
        }
        string op = "";
        (string left, string right) = GetExpressions(s, out op);
        if (right == "")
            return $"{unaryOperators[op]}({GetParse(left)})";
        return $"{binaryOperators[op]}({GetParse(left)},{GetParse(right)})";
    }
    private bool IsConstant(string s)
    {
        string op = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] > 'a' && s[i] < 'z')
            {
                op += s[i];
                continue;
            }
            else
            {
                if (op != "")
                {
                    if (binaryOperators.ContainsKey(op) || unaryOperators.ContainsKey(op))
                        return false;
                }
            }
            op = s[i].ToString();
            if (binaryOperators.ContainsKey(op) || unaryOperators.ContainsKey(op))
                return false;
            op = "";
        }
        return true;
    }
    private (string, string) GetExpressions(string s, out string op)
    {
        int index = -1;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '+' || s[i] == '-')
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            op = s[index].ToString();
            string left = s[..(index)];
            string right = s[(index + 1)..];
            return (DelParenthesis(left), DelParenthesis(right));
        }
        else
        {
            return GetAdvancedExpressions(s, out op);
        }
    }
    private (string, string) GetAdvancedExpressions(string s, out string op)
    {
        op = "";
        bool unary = false;
        int index = -1;
        int opIndex = int.MaxValue;
        string actualOp = "";
        for (int i = 0; i < s.Length; i++)
        {
            actualOp = "";
            if (s[i] > 'a' && s[i] < 'z')
            {

                for (int j = i; j < s.Length; j++)
                {
                    if (s[j] > 'a' && s[j] < 'z')
                        actualOp += s[j];
                    else
                    {
                        i = j - 1;
                        break;
                    }
                }
            }
            else
                actualOp = s[i].ToString();
            if (binaryOperators.ContainsKey(actualOp))
            {
                int newIndex = binaryOperators.Keys.ToList().IndexOf(actualOp);
                if (newIndex < opIndex)
                {
                    op = actualOp;
                    index = i;
                    opIndex = newIndex;
                }
            }
            if (unaryOperators.ContainsKey(actualOp) && op == "")
            {
                unary = true;
                int newIndex = unaryOperators.Keys.ToList().IndexOf(actualOp);
                if (newIndex < opIndex)
                {
                    op = actualOp;
                    index = i;
                    opIndex = newIndex;
                }
            }
        }
        if (unary)
            return (DelParenthesis(s[(index + 1)..]), "");
        return (DelParenthesis(s[0..index]), DelParenthesis(s[(index + 1)..]));
    }
    private string DelParenthesis(string s)
    {
        if (s[0] == '(' && s[s.Length - 1] == ')')
        {
            string temp = s;
            temp = s[1..(s.Length - 1)];
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
                return temp;
        }
        return s;
    }
    #region Sustitution
    private string SearchParenthesis(string s)
    {
        string past = "";
        string actual = s;
        while (actual != past)
        {
            int from = -1;
            int to = actual.Length;
            for (int i = 0; i < actual.Length; i++)
            {
                if (actual[i] == '(')
                    from = i;
                if (actual[i] == ')')
                {
                    to = i;
                    break;
                }
            }
            if (from == -1)
                break;
            past = actual;
            actual = Sustitution(actual, from, to + 1);
        }
        return actual;
    }
    private string Sustitution(string s, int from, int to)
    {
        sustitutions.Add(actualLetter, s[from..to]);
        string res = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (i == from)
                res += actualLetter.ToString();
            if (i >= from && i < to)
                continue;
            res += s[i];
        }
        actualLetter++;
        return res;
    }
    #endregion
}