using System.Runtime.InteropServices;
using System.Linq;
namespace lib;
public class Interpreter
{
    public List<string> IDs;
    public Interpreter()
    {
        IDs = new();
    }
    #region Parsing
    //Lista de las expresiones contenidas dentro de parentesis en la funcion introducida por el usuario
    List<string> substitutions = new();
    /// <summary>
    ///  Prepara la función al eliminar espacios en blanco y extrae las expresiones que estén dentro de los paréntesis 
    ///  para posterior conversión
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    private string Prepare(string s)
    {
        s = s.Replace(" ", "");
        s = SearchParenthesis(s);
        return s;
    }
    /// <summary>
    ///  Convierte la función con operadores en notación infija a notación prefija
    ///  para reducir la dificultad al crear una función del tipo Expression
    /// </summary>
    /// <param name=""></param>
    /// <returns>Retorna una función de la forma "operador"(a,b)</returns>
    public string Parse(string function)
    {
        try
        {
            function = Prepare(function);
            function = GetParse(function);

            //Parsear cada sustición hecha en el método "Prepare"
            string[] subsArray = substitutions.ToArray();
            for (int i = 0; i < subsArray.Length; i++)
            {
                subsArray[i] = GetParse(subsArray[i]);

                if (i != 0)
                    subsArray[i] = string.Format(subsArray[i], subsArray[..i]);
            }

            function = string.Format(function, subsArray);

            substitutions.Clear();
            IDs.Sort();
        }
        catch
        {
            throw new IntegralException();
        }
        return function;
    }
    //Parsea la funcion pasada como parámetro
    private string GetParse(string s)
    {
        //Elimina paréntesis innecesarios introducidos por el usuario
        s = DelParenthesis(s);

        //Condición de parada al comprobar si es una constante la expresión actual
        if (IsConstant(s))
            return s;

        (string lhs, string? rhs) = GetExpressions(s, out string op);

        //Si el operador solamente tiene una expresion entonces es unario
        if (rhs == null)
            return $"{Database.unaryOperators[op]}({GetParse(lhs)})";

        //Se añade expresiones neutras para evitar imprecisiones del usuario y para permitir el reconocimiento de expresiones negativas
        if (lhs == "")
        {
            if (op == "-" || op == "+")
                lhs = "0";
            if (op == "*" || op == "/")
                lhs = "1";
        }
        return $"{Database.binaryOperators[op]}({GetParse(lhs)},{GetParse(rhs)})";
    }

    /// <summary>
    ///  Combrueba si existe un operador de la base de datos en la expresion pasada como parámetro
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    private static bool IsConstant(string s)
        => Database.binaryOperators.Keys.All(op => !s.Contains(op)) &&
                Database.unaryOperators.Keys.All(op => !s.Contains(op));

    /// <summary>
    ///  Obtiene los miembros a los q relaciona el operador de la expresión
    /// </summary>
    /// <param name="op">string del operador obtenido al analizar la expresión</param>
    /// <returns>Retorna el miembro izquierdo y derecho del operador</returns>
    private static (string lhs, string? rhs) GetExpressions(string s, out string op)
    {
        //Comprueba si es un operador binario
        foreach (var oper in Database.binaryOperators.Keys)
        {
            int i = s.IndexOf(oper);
            if (i >= 0)
            {
                op = oper;
                return (s[..i], s[(i + 1)..]);
            }
        }
        //Comprueba si es un operador unario al no encontrar ningún binario
        foreach (var oper in Database.unaryOperators.Keys)
        {
            if (s.Contains(oper))
            {
                if (oper == s)
                    continue;
                op = oper;
                return (s[oper.Length..], null);
            }
        }
        throw new ArgumentException("Invalid expression");
    }
    /// <summary>
    ///  Elimina cuantos paréntesis externos pueda mientras se mantenga balanceada la expresión
    /// Ejemplo: Se introduce ((a+b)) y se retorna a+b
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
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
    /// <summary>
    ///  Busca expresiones anidadas y separadas por paréntesis, las sustituye por variables y las guarda 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    private string SearchParenthesis(string s)
    {
        string prev = "";
        string curr = s;
        //Se ejecuta hasta que no cambie la expresión luego de extraer sus paréntesis
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
    /// <summary>
    ///  Transforma una expresión ya parseada al construir un objeto del tipo Expression
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public Expression GetExpression(string function)
    {
        //Si no aparece ningún paréntesis entonces se está conviertiendo una constante
        if (!function.Any(x => x == '('))
        {
            if (Database.constToNumber.ContainsKey(function))
            {
                return new Const(Database.constToNumber[function]);
            }
            double number = -1;
            if (double.TryParse(function, out number))
                return new Const(number);
            if (!IDs.Contains(function))
                IDs.Add(function);
            return new Variable(function);

        }
        string oper = "";
        int comaIndex = -1;
        int from = 0;
        int to = function.Length - 1;
        //Iteración destinada a obtener el operador de la expresión y sus miembros que están separados por una coma
        for (int i = 0; i < function.Length; i++)
        {
            if (function[i] == '(')
            {
                from = i + 1;
                int count = 0;
                for (int j = i + 1; j < function.Length; j++)
                {
                    if (function[j] == '(')
                        count++;
                    if (function[j] == ')')
                        count--;
                    if (function[j] == ',' && count == 0)
                    {
                        to = j;
                        comaIndex = j;
                        i = function.Length;
                        break;
                    }
                }
                i = function.Length;
            }
            else
                oper += function[i];
        }
        if (comaIndex != -1)
        {
            //Obtiene la expresión de cada miembro
            Expression left = GetExpression(function[from..to]);
            Expression right = GetExpression(function[(to + 1)..^1]);
            return new BinaryGeneric(left, right, Database.BynaryStringToExpression[oper]);
        }
        else
            //Si no encuentra una coma entonces se trata de una expresión unaria
            return new UnaryGeneric(GetExpression(function[from..to]), Database.UnaryStringToExpression[oper]);
    }
    #endregion
}