using Util.Data;

namespace Util.Helpers;

/// <summary>
/// Clase estática que contiene métodos relacionados con expresiones lambda.
/// </summary>
public static class Lambda
{

    #region GetType(Obtener tipo)

    /// <summary>
    /// Obtiene el tipo del miembro representado por la expresión proporcionada.
    /// </summary>
    /// <param name="expression">La expresión de la que se desea obtener el tipo del miembro.</param>
    /// <returns>El tipo del miembro representado por la expresión, o null si la expresión no es un miembro válido.</returns>
    public static Type GetType(Expression expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return memberExpression?.Type;
    }

    #endregion

    #region GetMember(Obtener miembros)

    /// <summary>
    /// Obtiene información sobre el miembro de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener información del miembro.</param>
    /// <returns>
    /// Un objeto <see cref="MemberInfo"/> que representa el miembro de la expresión, 
    /// o <c>null</c> si la expresión no es un <see cref="MemberExpression"/> válido.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="GetMemberExpression"/> para extraer el 
    /// <see cref="MemberExpression"/> de la expresión proporcionada antes de 
    /// devolver el miembro correspondiente.
    /// </remarks>
    /// <seealso cref="GetMemberExpression"/>
    public static MemberInfo GetMember(Expression expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return memberExpression?.Member;
    }

    /// <summary>
    /// Obtiene una expresión de miembro a partir de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener el miembro.</param>
    /// <param name="right">Indica si se debe obtener el miembro de la parte derecha de una expresión binaria.</param>
    /// <returns>
    /// Un <see cref="MemberExpression"/> que representa el miembro obtenido de la expresión, o <c>null</c> si la expresión es <c>null</c> o no se puede obtener un miembro.
    /// </returns>
    /// <remarks>
    /// Este método recorre diferentes tipos de expresiones para encontrar un <see cref="MemberExpression"/>.
    /// Soporta expresiones de tipo lambda, conversiones, negaciones, accesos a miembros, y comparaciones.
    /// </remarks>
    /// <seealso cref="Expression"/>
    /// <seealso cref="MemberExpression"/>
    /// <seealso cref="BinaryExpression"/>
    /// <seealso cref="UnaryExpression"/>
    /// <seealso cref="LambdaExpression"/>
    public static MemberExpression GetMemberExpression(Expression expression, bool right = false)
    {
        if (expression == null)
            return null;
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                return GetMemberExpression(((LambdaExpression)expression).Body, right);
            case ExpressionType.Convert:
            case ExpressionType.Not:
                return GetMemberExpression(((UnaryExpression)expression).Operand, right);
            case ExpressionType.MemberAccess:
                return (MemberExpression)expression;
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.LessThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThanOrEqual:
                return GetMemberExpression(right ? ((BinaryExpression)expression).Right : ((BinaryExpression)expression).Left, right);
            case ExpressionType.Call:
                return GetMethodCallExpressionName(expression);
        }
        return null;
    }

    /// <summary>
    /// Obtiene la expresión de miembro de una llamada a método dada una expresión de entrada.
    /// </summary>
    /// <param name="expression">La expresión que representa la llamada al método.</param>
    /// <returns>
    /// Un <see cref="MemberExpression"/> que representa el miembro asociado a la llamada al método,
    /// o el miembro del objeto de la llamada si no se encuentra un argumento adecuado.
    /// </returns>
    /// <remarks>
    /// Este método asume que la expresión proporcionada es de tipo <see cref="MethodCallExpression"/>.
    /// Si el objeto de la llamada es una colección genérica, se intentará obtener el primer argumento
    /// que sea un acceso a un miembro. Si no se encuentra un argumento adecuado, se devolverá el
    /// miembro del objeto de la llamada.
    /// </remarks>
    /// <exception cref="InvalidCastException">
    /// Se lanzará si la expresión proporcionada no es un <see cref="MethodCallExpression"/> 
    /// o si el objeto de la llamada no es un <see cref="MemberExpression"/>.
    /// </exception>
    private static MemberExpression GetMethodCallExpressionName(Expression expression)
    {
        var methodCallExpression = (MethodCallExpression)expression;
        var left = (MemberExpression)methodCallExpression.Object;
        if (Reflection.IsGenericCollection(left?.Type))
        {
            var argumentExpression = methodCallExpression.Arguments.FirstOrDefault();
            if (argumentExpression != null && argumentExpression.NodeType == ExpressionType.MemberAccess)
                return (MemberExpression)argumentExpression;
        }
        return left;
    }

    #endregion

    #region GetName(Obtener el nombre del miembro.)

    /// <summary>
    /// Obtiene el nombre del miembro a partir de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se extraerá el nombre del miembro.</param>
    /// <returns>El nombre del miembro como una cadena.</returns>
    /// <remarks>
    /// Este método asume que la expresión proporcionada es un tipo de expresión de miembro.
    /// Si la expresión no es válida, puede lanzar una excepción.
    /// </remarks>
    /// <seealso cref="GetMemberExpression(Expression)"/>
    /// <seealso cref="GetMemberName(MemberExpression)"/>
    public static string GetName(Expression expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return GetMemberName(memberExpression);
    }

    /// <summary>
    /// Obtiene el nombre del miembro a partir de una expresión de miembro.
    /// </summary>
    /// <param name="memberExpression">La expresión de miembro de la cual se extraerá el nombre.</param>
    /// <returns>
    /// Un string que representa el nombre del miembro. Si la expresión de miembro es nula, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la representación en cadena de la expresión de miembro para extraer el nombre real,
    /// eliminando cualquier prefijo que preceda al nombre del miembro.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="memberExpression"/> es nulo.</exception>
    public static string GetMemberName(MemberExpression memberExpression)
    {
        if (memberExpression == null)
            return string.Empty;
        string result = memberExpression.ToString();
        return result.Substring(result.IndexOf(".", StringComparison.Ordinal) + 1);
    }

    #endregion

    #region GetNames(Obtener lista de nombres.)

    /// <summary>
    /// Obtiene una lista de nombres a partir de una expresión que representa un arreglo de objetos.
    /// </summary>
    /// <typeparam name="T">El tipo de los objetos que se están procesando en la expresión.</typeparam>
    /// <param name="expression">La expresión que contiene un arreglo de objetos del tipo especificado.</param>
    /// <returns>Una lista de cadenas que representan los nombres extraídos de la expresión. Si la expresión es nula o no es un arreglo, se devuelve una lista vacía.</returns>
    /// <remarks>
    /// Este método analiza la expresión proporcionada y extrae los nombres de cada uno de los elementos del arreglo.
    /// Si la expresión no es válida o no contiene elementos, se retornará una lista vacía.
    /// </remarks>
    /// <seealso cref="GetName(Expression)"/>
    public static List<string> GetNames<T>(Expression<Func<T, object[]>> expression)
    {
        var result = new List<string>();
        if (expression == null)
            return result;
        if (!(expression.Body is NewArrayExpression arrayExpression))
            return result;
        foreach (var each in arrayExpression.Expressions)
        {
            var name = GetName(each);
            if (string.IsNullOrWhiteSpace(name) == false)
                result.Add(name);
        }
        return result;
    }

    #endregion

    #region GetLastName(Obtener el nombre del último nivel de miembro.)

    /// <summary>
    /// Obtiene el último nombre de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se extraerá el último nombre.</param>
    /// <param name="right">Indica si se debe buscar el nombre desde la derecha.</param>
    /// <returns>El último nombre de la expresión, o una cadena vacía si no se puede determinar.</returns>
    /// <remarks>
    /// Este método utiliza una expresión de miembro para extraer el último nombre.
    /// Si la expresión no es válida o es un valor, se devuelve una cadena vacía.
    /// </remarks>
    public static string GetLastName(Expression expression, bool right = false)
    {
        var memberExpression = GetMemberExpression(expression, right);
        if (memberExpression == null)
            return string.Empty;
        if (IsValueExpression(memberExpression))
            return string.Empty;
        string result = memberExpression.ToString();
        return result.Substring(result.LastIndexOf(".", StringComparison.Ordinal) + 1);
    }

    /// <summary>
    /// Determina si la expresión proporcionada es una expresión de valor.
    /// </summary>
    /// <param name="expression">La expresión a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la expresión es una expresión de valor; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Una expresión se considera de valor si es un acceso a un miembro o una constante.
    /// </remarks>
    private static bool IsValueExpression(Expression expression)
    {
        if (expression == null)
            return false;
        switch (expression.NodeType)
        {
            case ExpressionType.MemberAccess:
                return IsValueExpression(((MemberExpression)expression).Expression);
            case ExpressionType.Constant:
                return true;
        }
        return false;
    }

    #endregion

    #region GetLastNames(Obtener la lista de nombres de los miembros del último nivel.)

    /// <summary>
    /// Obtiene una lista de apellidos a partir de una expresión que representa un arreglo de objetos.
    /// </summary>
    /// <typeparam name="T">El tipo de los objetos que se están procesando.</typeparam>
    /// <param name="expression">Una expresión que representa un arreglo de objetos de tipo <typeparamref name="T"/>.</param>
    /// <returns>
    /// Una lista de cadenas que contiene los apellidos extraídos de la expresión proporcionada.
    /// Si la expresión es nula o no es un arreglo, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método evalúa la expresión dada y extrae los apellidos utilizando el método <see cref="GetLastName"/>.
    /// Solo se añaden a la lista aquellos apellidos que no son nulos o espacios en blanco.
    /// </remarks>
    public static List<string> GetLastNames<T>(Expression<Func<T, object[]>> expression)
    {
        var result = new List<string>();
        if (expression == null)
            return result;
        if (!(expression.Body is NewArrayExpression arrayExpression))
            return result;
        foreach (var each in arrayExpression.Expressions)
        {
            var name = GetLastName(each);
            if (string.IsNullOrWhiteSpace(name) == false)
                result.Add(name);
        }
        return result;
    }

    #endregion

    #region GetValue(Obtener valor.)

    /// <summary>
    /// Obtiene el valor de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener el valor. Puede ser de varios tipos, incluyendo lambda, binaria, llamada a método, acceso a miembro o constante.</param>
    /// <returns>
    /// El valor de la expresión, o null si la expresión es null o no se puede determinar el valor.
    /// </returns>
    /// <remarks>
    /// Este método evalúa diferentes tipos de nodos de expresión y devuelve el valor correspondiente.
    /// Si la expresión es un lambda, se evalúa su cuerpo. 
    /// Para expresiones binarias, se determina si hay un parámetro y se evalúa el lado correspondiente.
    /// Las expresiones de llamada a método y acceso a miembro se manejan de manera específica para obtener su valor.
    /// </remarks>
    /// <seealso cref="Expression"/>
    /// <seealso cref="LambdaExpression"/>
    /// <seealso cref="BinaryExpression"/>
    /// <seealso cref="MethodCallExpression"/>
    /// <seealso cref="MemberExpression"/>
    /// <seealso cref="ConstantExpression"/>
    public static object GetValue(Expression expression)
    {
        if (expression == null)
            return null;
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                return GetValue(((LambdaExpression)expression).Body);
            case ExpressionType.Convert:
                return GetValue(((UnaryExpression)expression).Operand);
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.LessThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThanOrEqual:
                var hasParameter = HasParameter(((BinaryExpression)expression).Left);
                if (hasParameter)
                    return GetValue(((BinaryExpression)expression).Right);
                return GetValue(((BinaryExpression)expression).Left);
            case ExpressionType.Call:
                return GetMethodCallExpressionValue(expression);
            case ExpressionType.MemberAccess:
                return GetMemberValue((MemberExpression)expression);
            case ExpressionType.Constant:
                return GetConstantExpressionValue(expression);
            case ExpressionType.Not:
                if (expression.Type == typeof(bool))
                    return false;
                return null;
        }
        return null;
    }

    /// <summary>
    /// Determina si una expresión contiene un parámetro.
    /// </summary>
    /// <param name="expression">La expresión que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la expresión contiene un parámetro; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método recorre la expresión y verifica si alguno de sus nodos es de tipo <see cref="ExpressionType.Parameter"/>.
    /// Si encuentra un nodo de este tipo, devuelve <c>true</c>. Si la expresión es nula o no contiene parámetros, devuelve <c>false</c>.
    /// </remarks>
    private static bool HasParameter(Expression expression)
    {
        if (expression == null)
            return false;
        switch (expression.NodeType)
        {
            case ExpressionType.Convert:
                return HasParameter(((UnaryExpression)expression).Operand);
            case ExpressionType.MemberAccess:
                return HasParameter(((MemberExpression)expression).Expression);
            case ExpressionType.Parameter:
                return true;
        }
        return false;
    }

    /// <summary>
    /// Obtiene el valor de una expresión de llamada a un método.
    /// </summary>
    /// <param name="expression">La expresión de llamada al método que se va a evaluar.</param>
    /// <returns>El valor resultante de la evaluación de la expresión, o null si no se puede obtener un valor.</returns>
    /// <remarks>
    /// Este método analiza la expresión de llamada al método y trata de obtener el valor de los argumentos pasados a la llamada.
    /// Si no se encuentra un valor en el primer argumento, se intentará obtener el valor del segundo argumento si está presente.
    /// Si la expresión no tiene un objeto asociado, se invocará el método directamente utilizando el nombre del método y su tipo.
    /// </remarks>
    private static object GetMethodCallExpressionValue(Expression expression)
    {
        var methodCallExpression = (MethodCallExpression)expression;
        var value = GetValue(methodCallExpression.Arguments.FirstOrDefault());
        if (value != null)
            return value;
        if (methodCallExpression.Arguments.Count > 1)
            return GetValue(methodCallExpression.Arguments[1]);
        if (methodCallExpression.Object == null)
            return methodCallExpression.Type.InvokeMember(methodCallExpression.Method.Name, BindingFlags.InvokeMethod, null, null, null);
        return GetValue(methodCallExpression.Object);
    }

    /// <summary>
    /// Obtiene el valor de un miembro (campo o propiedad) a partir de una expresión de miembro.
    /// </summary>
    /// <param name="expression">La expresión de miembro que se va a evaluar.</param>
    /// <returns>El valor del miembro especificado, o null si no se puede obtener el valor.</returns>
    /// <remarks>
    /// Este método maneja tanto campos como propiedades y puede trabajar con expresiones que son constantes.
    /// Si el miembro es una propiedad y la expresión es nula, se intenta obtener el valor estático de la propiedad.
    /// Si el miembro es una propiedad y la expresión es una expresión constante, se obtiene el valor de la propiedad
    /// a partir del valor constante. Si la expresión es otro miembro, se evalúa recursivamente.
    /// </remarks>
    /// <seealso cref="FieldInfo"/>
    /// <seealso cref="PropertyInfo"/>
    /// <seealso cref="MemberExpression"/>
    private static object GetMemberValue(MemberExpression expression)
    {
        if (expression == null)
            return null;
        var field = expression.Member as FieldInfo;
        if (field != null)
        {
            var constValue = GetConstantExpressionValue(expression.Expression);
            return field.GetValue(constValue);
        }
        var property = expression.Member as PropertyInfo;
        if (property == null)
            return null;
        if (expression.Expression == null)
            return property.GetValue(null);
        if (expression.Expression is ConstantExpression)
        {
            var constValue = GetConstantExpressionValue(expression.Expression);
            return property.GetValue(constValue);
        }
        var value = GetMemberValue(expression.Expression as MemberExpression);
        if (value == null)
        {
            if (property.PropertyType == typeof(bool))
                return true;
            return null;
        }
        return property.GetValue(value);
    }

    /// <summary>
    /// Obtiene el valor de una expresión constante.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener el valor constante.</param>
    /// <returns>El valor de la expresión constante.</returns>
    /// <exception cref="InvalidCastException">Se lanza si la expresión no es de tipo <see cref="ConstantExpression"/>.</exception>
    private static object GetConstantExpressionValue(Expression expression)
    {
        var constantExpression = (ConstantExpression)expression;
        return constantExpression.Value;
    }

    #endregion

    #region GetOperator(Obtener operador de consulta)

    /// <summary>
    /// Obtiene el operador correspondiente a una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener el operador.</param>
    /// <returns>
    /// Un operador que representa el tipo de operación que se está realizando en la expresión, 
    /// o null si la expresión es nula o no se puede determinar el operador.
    /// </returns>
    /// <remarks>
    /// Este método evalúa el tipo de nodo de la expresión y devuelve el operador correspondiente 
    /// según el tipo de expresión que se esté procesando. Si la expresión es una lambda o una 
    /// conversión, se evalúa la expresión interna. Para expresiones de comparación, se devuelven 
    /// los operadores de comparación adecuados.
    /// </remarks>
    /// <seealso cref="Operator"/>
    public static Operator? GetOperator(Expression expression)
    {
        if (expression == null)
            return null;
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                return GetOperator(((LambdaExpression)expression).Body);
            case ExpressionType.Convert:
                return GetOperator(((UnaryExpression)expression).Operand);
            case ExpressionType.Equal:
                return Operator.Equal;
            case ExpressionType.NotEqual:
                return Operator.NotEqual;
            case ExpressionType.GreaterThan:
                return Operator.Greater;
            case ExpressionType.LessThan:
                return Operator.Less;
            case ExpressionType.GreaterThanOrEqual:
                return Operator.GreaterEqual;
            case ExpressionType.LessThanOrEqual:
                return Operator.LessEqual;
            case ExpressionType.Call:
                return GetMethodCallExpressionOperator(expression);
        }
        return null;
    }

    /// <summary>
    /// Obtiene el operador correspondiente a una expresión de llamada a método.
    /// </summary>
    /// <param name="expression">La expresión de llamada a método que se va a evaluar.</param>
    /// <returns>
    /// Devuelve un operador de tipo <see cref="Operator"/> si el método es "contains", "endswith" o "startswith"; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método realiza una conversión de la expresión a <see cref="MethodCallExpression"/> y evalúa el nombre del método
    /// en minúsculas para determinar el operador correspondiente.
    /// </remarks>
    private static Operator? GetMethodCallExpressionOperator(Expression expression)
    {
        var methodCallExpression = (MethodCallExpression)expression;
        switch (methodCallExpression?.Method?.Name?.ToLower())
        {
            case "contains":
                return Operator.Contains;
            case "endswith":
                return Operator.Ends;
            case "startswith":
                return Operator.Starts;
        }
        return null;
    }

    #endregion

    #region GetParameter(Obtener parámetros)

    /// <summary>
    /// Obtiene el parámetro de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se desea obtener el parámetro. Puede ser de varios tipos de expresión.</param>
    /// <returns>
    /// Un objeto <see cref="ParameterExpression"/> que representa el parámetro encontrado en la expresión, 
    /// o <c>null</c> si no se encuentra ningún parámetro o si la expresión es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método recorre la expresión y busca el primer parámetro que encuentra. 
    /// Puede manejar expresiones de tipo lambda, conversiones, comparaciones, acceso a miembros y llamadas a métodos.
    /// </remarks>
    public static ParameterExpression GetParameter(Expression expression)
    {
        if (expression == null)
            return null;
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                return GetParameter(((LambdaExpression)expression).Body);
            case ExpressionType.Convert:
                return GetParameter(((UnaryExpression)expression).Operand);
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.LessThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThanOrEqual:
                return GetParameter(((BinaryExpression)expression).Left);
            case ExpressionType.MemberAccess:
                return GetParameter(((MemberExpression)expression).Expression);
            case ExpressionType.Call:
                return GetParameter(((MethodCallExpression)expression).Object);
            case ExpressionType.Parameter:
                return (ParameterExpression)expression;
        }
        return null;
    }

    #endregion

    #region GetGroupPredicates(Obtener la expresión de predicado del grupo.)

    /// <summary>
    /// Obtiene un grupo de predicados a partir de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión de la cual se extraerán los predicados.</param>
    /// <returns>Una lista de listas de expresiones que representan los predicados agrupados.</returns>
    /// <remarks>
    /// Si la expresión proporcionada es nula, se devolverá una lista vacía.
    /// Este método utiliza un método auxiliar llamado <see cref="AddPredicates"/> 
    /// para agregar los predicados a la lista de resultados.
    /// </remarks>
    public static List<List<Expression>> GetGroupPredicates(Expression expression)
    {
        var result = new List<List<Expression>>();
        if (expression == null)
            return result;
        AddPredicates(expression, result, CreateGroup(result));
        return result;
    }

    /// <summary>
    /// Crea un nuevo grupo de expresiones y lo agrega a la lista de resultados.
    /// </summary>
    /// <param name="result">La lista de listas de expresiones donde se añadirá el nuevo grupo.</param>
    /// <returns>Una lista de expresiones que representa el nuevo grupo creado.</returns>
    private static List<Expression> CreateGroup(List<List<Expression>> result)
    {
        var gourp = new List<Expression>();
        result.Add(gourp);
        return gourp;
    }

    /// <summary>
    /// Agrega predicados a una lista de resultados a partir de una expresión dada.
    /// </summary>
    /// <param name="expression">La expresión que se va a analizar y de la cual se extraerán los predicados.</param>
    /// <param name="result">La lista de listas donde se almacenarán los grupos de expresiones.</param>
    /// <param name="group">La lista actual que contiene las expresiones agrupadas.</param>
    /// <remarks>
    /// Este método recorre la expresión y clasifica los predicados en función de su tipo de nodo.
    /// Si encuentra una expresión de tipo Lambda, analiza su cuerpo. 
    /// Para expresiones de tipo OrElse, divide la expresión en sus partes izquierda y derecha, 
    /// creando un nuevo grupo para la parte derecha. 
    /// Para expresiones de tipo AndAlso, ambas partes se agregan al mismo grupo.
    /// Cualquier otro tipo de expresión se agrega directamente al grupo actual.
    /// </remarks>
    private static void AddPredicates(Expression expression, List<List<Expression>> result, List<Expression> group)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                AddPredicates(((LambdaExpression)expression).Body, result, group);
                break;
            case ExpressionType.OrElse:
                AddPredicates(((BinaryExpression)expression).Left, result, group);
                AddPredicates(((BinaryExpression)expression).Right, result, CreateGroup(result));
                break;
            case ExpressionType.AndAlso:
                AddPredicates(((BinaryExpression)expression).Left, result, group);
                AddPredicates(((BinaryExpression)expression).Right, result, group);
                break;
            default:
                group.Add(expression);
                break;
        }
    }

    #endregion

    #region GetConditionCount(Obtener el número de condiciones de consulta.)

    /// <summary>
    /// Obtiene el conteo de condiciones en una expresión lambda.
    /// </summary>
    /// <param name="expression">La expresión lambda de la cual se desea contar las condiciones.</param>
    /// <returns>El número de condiciones encontradas en la expresión. Retorna 0 si la expresión es nula.</returns>
    /// <remarks>
    /// Este método reemplaza las operaciones lógicas "AndAlso" y "OrElse" por un delimitador 
    /// para poder contar cuántas condiciones hay en la expresión. 
    /// Se utiliza el carácter '|' como delimitador para separar las condiciones.
    /// </remarks>
    public static int GetConditionCount(LambdaExpression expression)
    {
        if (expression == null)
            return 0;
        var result = expression.ToString().Replace("AndAlso", "|").Replace("OrElse", "|");
        return result.Split('|').Count();
    }

    #endregion

    #region GetAttribute(Obtener características)

    /// <summary>
    /// Obtiene un atributo específico de un miembro basado en una expresión.
    /// </summary>
    /// <typeparam name="TAttribute">El tipo del atributo que se desea obtener. Debe heredar de <see cref="Attribute"/>.</typeparam>
    /// <param name="expression">La expresión que representa el miembro del cual se desea obtener el atributo.</param>
    /// <returns>El atributo del tipo especificado si se encuentra; de lo contrario, <c>null</c>.</returns>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para determinar el miembro correspondiente 
    /// y luego intenta recuperar el atributo del tipo especificado. 
    /// Asegúrese de que la expresión apunte a un miembro válido que tenga el atributo deseado.
    /// </remarks>
    /// <seealso cref="GetMember(Expression)"/>
    /// <seealso cref="GetCustomAttribute{TAttribute}()"/>
    public static TAttribute GetAttribute<TAttribute>(Expression expression) where TAttribute : Attribute
    {
        var memberInfo = GetMember(expression);
        return memberInfo.GetCustomAttribute<TAttribute>();
    }

    /// <summary>
    /// Obtiene un atributo específico de una propiedad de una entidad.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que contiene la propiedad.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad de la entidad.</typeparam>
    /// <typeparam name="TAttribute">El tipo del atributo que se desea obtener.</typeparam>
    /// <param name="propertyExpression">Una expresión que representa la propiedad de la entidad.</param>
    /// <returns>
    /// El atributo especificado de la propiedad, o null si no se encuentra.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder a los atributos definidos en las propiedades de una entidad 
    /// utilizando expresiones lambda para identificar la propiedad deseada.
    /// </remarks>
    /// <seealso cref="GetAttribute{TAttribute}(Expression{Func{TEntity, TProperty}})"/>
    public static TAttribute GetAttribute<TEntity, TProperty, TAttribute>(Expression<Func<TEntity, TProperty>> propertyExpression) where TAttribute : Attribute
    {
        return GetAttribute<TAttribute>(propertyExpression);
    }

    /// <summary>
    /// Obtiene el atributo especificado de una propiedad a partir de una expresión lambda que representa la propiedad.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad de la que se desea obtener el atributo.</typeparam>
    /// <typeparam name="TAttribute">El tipo del atributo que se desea obtener. Debe ser una clase que derive de <see cref="Attribute"/>.</typeparam>
    /// <param name="propertyExpression">Una expresión lambda que representa la propiedad de la cual se quiere obtener el atributo.</param>
    /// <returns>El atributo especificado de la propiedad, o <c>null</c> si no se encuentra el atributo.</returns>
    /// <seealso cref="GetAttribute{TAttribute}(Expression{Func{TProperty}})"/>
    public static TAttribute GetAttribute<TProperty, TAttribute>(Expression<Func<TProperty>> propertyExpression) where TAttribute : Attribute
    {
        return GetAttribute<TAttribute>(propertyExpression);
    }

    #endregion

    #region GetAttributes(Obtener lista de características.)

    /// <summary>
    /// Obtiene los atributos personalizados de un miembro específico de una entidad.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que contiene el miembro.</typeparam>
    /// <typeparam name="TProperty">El tipo del miembro del que se obtendrán los atributos.</typeparam>
    /// <typeparam name="TAttribute">El tipo del atributo que se desea obtener.</typeparam>
    /// <param name="propertyExpression">Una expresión que representa el miembro del que se desean obtener los atributos.</param>
    /// <returns>
    /// Una colección de atributos del tipo especificado que están aplicados al miembro.
    /// </returns>
    /// <remarks>
    /// Este método utiliza reflexión para obtener los atributos personalizados del miembro
    /// especificado en la expresión. Asegúrese de que el tipo de atributo especificado
    /// derive de la clase <see cref="Attribute"/>.
    /// </remarks>
    /// <seealso cref="GetMember{TEntity, TProperty}(Expression{Func{TEntity, TProperty}})"/>
    public static IEnumerable<TAttribute> GetAttributes<TEntity, TProperty, TAttribute>(Expression<Func<TEntity, TProperty>> propertyExpression) where TAttribute : Attribute
    {
        var memberInfo = GetMember(propertyExpression);
        return memberInfo.GetCustomAttributes<TAttribute>();
    }

    #endregion

    #region Constant(Obtener constante)

    /// <summary>
    /// Crea una expresión constante a partir del valor proporcionado.
    /// </summary>
    /// <param name="value">El valor que se convertirá en una expresión constante.</param>
    /// <param name="expression">Una expresión opcional que se utiliza para determinar el tipo de la constante. Si es nula, se utiliza el tipo por defecto del valor.</param>
    /// <returns>Una expresión constante que representa el valor proporcionado.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="expression"/> es nulo, se crea una expresión constante utilizando el tipo del valor directamente.
    /// De lo contrario, se utiliza el tipo obtenido de la expresión proporcionada.
    /// </remarks>
    /// <seealso cref="Expression.Constant(object)"/>
    public static ConstantExpression Constant(object value, Expression expression = null)
    {
        var type = GetType(expression);
        if (type == null)
            return Expression.Constant(value);
        return Expression.Constant(value, type);
    }

    #endregion

    #region CreateParameter(Crear expresiones de parámetros)

    /// <summary>
    /// Crea un nuevo objeto <see cref="ParameterExpression"/> con el tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del parámetro que se va a crear.</typeparam>
    /// <returns>
    /// Un objeto <see cref="ParameterExpression"/> que representa un parámetro del tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para construir expresiones dinámicas donde se necesita un parámetro de un tipo específico.
    /// </remarks>
    /// <seealso cref="ParameterExpression"/>
    public static ParameterExpression CreateParameter<T>()
    {
        return Expression.Parameter(typeof(T), "t");
    }

    #endregion

    #region Equal(igual a la expresión)

    /// <summary>
    /// Crea una expresión que representa una comparación de igualdad para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a comparar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>
    /// Una expresión que representa una función que toma un objeto de tipo <typeparamref name="T"/> 
    /// y devuelve un valor booleano que indica si la propiedad especificada es igual al valor dado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un parámetro dinámico para construir la expresión de comparación. 
    /// Asegúrese de que el nombre de la propiedad proporcionado sea válido y que el valor sea del tipo correcto.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> Equal<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).Equal(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region NotEqual(No igual a expresión)

    /// <summary>
    /// Crea una expresión que representa una comparación de desigualdad para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a comparar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la comparación de desigualdad.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para construir filtros dinámicos en consultas LINQ, permitiendo excluir objetos que tienen un valor específico en una propiedad.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}(Expression{Func{T, bool}})"/>
    public static Expression<Func<T, bool>> NotEqual<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).NotEqual(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region Greater(expresión mayor que)

    /// <summary>
    /// Crea una expresión que representa una comparación de "mayor que" para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a evaluar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la comparación de "mayor que" para la propiedad especificada.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza un parámetro dinámico para construir la expresión de comparación.
    /// Asegúrese de que el tipo de la propiedad y el valor sean compatibles para evitar excepciones en tiempo de ejecución.
    /// </remarks>
    public static Expression<Func<T, bool>> Greater<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).Greater(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region GreaterEqual(Expresión mayor o igual.)

    /// <summary>
    /// Crea una expresión que representa una comparación de mayor o igual (>=) 
    /// para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la comparación de mayor o igual 
    /// para la propiedad especificada.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza un parámetro de tipo genérico y permite la creación 
    /// de expresiones de comparación dinámicamente basadas en el nombre de la propiedad 
    /// y el valor proporcionado.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> GreaterEqual<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).GreaterEqual(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region Less(Expresión menor que)

    /// <summary>
    /// Crea una expresión que representa una comparación de "menor que" para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a evaluar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor contra el cual se comparará la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la comparación de "menor que" para la propiedad especificada.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza un parámetro dinámico para construir la expresión de comparación.
    /// Asegúrese de que el tipo de la propiedad sea compatible con el tipo del valor proporcionado.
    /// </remarks>
    public static Expression<Func<T, bool>> Less<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).Less(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region LessEqual(Expresión menor o igual que)

    /// <summary>
    /// Crea una expresión que representa una comparación de "menor o igual que" 
    /// para una propiedad específica de un tipo dado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se va a comparar la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la comparación de "menor o igual que".
    /// </returns>
    /// <remarks>
    /// Esta función utiliza un parámetro dinámico para construir la expresión 
    /// y se basa en un método auxiliar para obtener la propiedad y aplicar la comparación.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> LessEqual<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).LessEqual(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region Starts(Llamar al método StartsWith.)

    /// <summary>
    /// Crea una expresión que representa una condición de búsqueda que verifica si el valor de una propiedad 
    /// de un objeto de tipo <typeparamref name="T"/> comienza con un valor específico.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a evaluar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a evaluar.</param>
    /// <param name="value">El valor que se utilizará para comparar si la propiedad comienza con este valor.</param>
    /// <returns>
    /// Una expresión que representa la condición de búsqueda.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para construir dinámicamente consultas que filtran objetos basados en el 
    /// valor inicial de una propiedad de cadena.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> Starts<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).StartsWith(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region Ends(Llamar al método EndsWith.)

    /// <summary>
    /// Crea una expresión que verifica si el valor de una propiedad de un objeto de tipo T termina con un valor específico.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a evaluar.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a evaluar.</param>
    /// <param name="value">El valor con el que se comparará el final de la propiedad.</param>
    /// <returns>
    /// Una expresión que representa una función que toma un objeto de tipo T y devuelve un valor booleano
    /// indicando si la propiedad especificada termina con el valor dado.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para realizar filtrados en colecciones de objetos donde se necesita verificar
    /// el sufijo de una propiedad de tipo cadena.
    /// </remarks>
    /// <seealso cref="System.Linq.Expressions.Expression"/>
    public static Expression<Func<T, bool>> Ends<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).EndsWith(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region Contains(Llamar al método Contains.)

    /// <summary>
    /// Crea una expresión que verifica si una propiedad de un objeto contiene un valor específico.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a evaluar.</param>
    /// <param name="value">El valor que se buscará dentro de la propiedad.</param>
    /// <returns>
    /// Una expresión que representa la condición de que la propiedad especificada contiene el valor dado.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para construir consultas dinámicas donde se necesita filtrar objetos basados en si una propiedad contiene un valor específico.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> Contains<T>(string propertyName, object value)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).Contains(value).ToPredicate<T>(parameter);
    }

    #endregion

    #region ParsePredicate(analiza en expresión de predicado)

    /// <summary>
    /// Analiza un predicado basado en el nombre de una propiedad, un valor y un operador.
    /// </summary>
    /// <typeparam name="T">El tipo de la entidad sobre la que se aplica el predicado.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que se va a evaluar.</param>
    /// <param name="value">El valor que se utilizará en la comparación.</param>
    /// <param name="operator">El operador que se utilizará para la comparación.</param>
    /// <returns>
    /// Una expresión que representa el predicado que evalúa la propiedad especificada con el valor y el operador dados.
    /// </returns>
    /// <remarks>
    /// Este método permite construir dinámicamente expresiones de predicado que pueden ser utilizadas en consultas LINQ.
    /// </remarks>
    /// <seealso cref="CreateParameter{T}"/>
    /// <seealso cref="Operation"/>
    /// <seealso cref="ToPredicate{T}"/>
    public static Expression<Func<T, bool>> ParsePredicate<T>(string propertyName, object value, Operator @operator)
    {
        var parameter = CreateParameter<T>();
        return parameter.Property(propertyName).Operation(@operator, value).ToPredicate<T>(parameter);
    }

    #endregion
}