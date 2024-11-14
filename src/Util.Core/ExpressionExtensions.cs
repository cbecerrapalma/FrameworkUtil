using Util.Data;
using Util.Expressions;
using Util.Helpers;

namespace Util;

/// <summary>
/// Proporciona métodos de extensión para trabajar con expresiones.
/// </summary>
public static class ExpressionExtensions
{

    #region Property(Expresión de atributos)

    /// <summary>
    /// Extiende la clase <see cref="Expression"/> para obtener una propiedad de un objeto dado,
    /// permitiendo el acceso a propiedades anidadas utilizando una cadena de nombres de propiedades.
    /// </summary>
    /// <param name="expression">La expresión que representa el objeto del cual se desea obtener la propiedad.</param>
    /// <param name="propertyName">El nombre de la propiedad o propiedades anidadas, separadas por puntos.</param>
    /// <returns>
    /// Una expresión que representa la propiedad especificada. Si <paramref name="propertyName"/> 
    /// no contiene puntos, se devuelve la propiedad directa. Si contiene puntos, se accede a las 
    /// propiedades anidadas en el orden especificado.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder a propiedades de objetos de forma dinámica, lo que es útil en 
    /// escenarios como la construcción de expresiones para consultas LINQ.
    /// </remarks>
    /// <seealso cref="Expression"/>
    public static Expression Property(this Expression expression, string propertyName)
    {
        if (propertyName.All(t => t != '.'))
            return Expression.Property(expression, propertyName);
        var propertyNameList = propertyName.Split('.');
        Expression result = null;
        for (int i = 0; i < propertyNameList.Length; i++)
        {
            if (i == 0)
            {
                result = Expression.Property(expression, propertyNameList[0]);
                continue;
            }
            result = result.Property(propertyNameList[i]);
        }
        return result;
    }

    /// <summary>
    /// Crea una expresión que representa el acceso a una propiedad o campo especificado.
    /// </summary>
    /// <param name="expression">La expresión que representa el objeto del cual se accede a la propiedad o campo.</param>
    /// <param name="member">El miembro (propiedad o campo) al que se desea acceder.</param>
    /// <returns>
    /// Una expresión que representa el acceso al miembro especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="Expression"/> y permite construir expresiones de acceso a miembros de manera más sencilla.
    /// </remarks>
    public static Expression Property(this Expression expression, MemberInfo member)
    {
        return Expression.MakeMemberAccess(expression, member);
    }

    #endregion

    #region And(con expresión)

    /// <summary>
    /// Combina dos expresiones lógicas utilizando la operación AND.
    /// </summary>
    /// <param name="left">La expresión de la izquierda que se combinará.</param>
    /// <param name="right">La expresión de la derecha que se combinará.</param>
    /// <returns>
    /// Una nueva expresión que representa la combinación de las dos expresiones 
    /// utilizando la operación AND. Si la expresión de la izquierda es nula, 
    /// se devuelve la expresión de la derecha. Si la expresión de la derecha 
    /// es nula, se devuelve la expresión de la izquierda.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="Expression"/> y permite 
    /// combinar expresiones de manera más fluida en consultas LINQ o en la construcción 
    /// de árboles de expresiones.
    /// </remarks>
    public static Expression And(this Expression left, Expression right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;
        return Expression.AndAlso(left, right);
    }

    /// <summary>
    /// Combina dos expresiones booleanas en una sola utilizando la operación lógica "AND".
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están evaluando en las expresiones.</typeparam>
    /// <param name="left">La expresión de la izquierda que se va a combinar.</param>
    /// <param name="right">La expresión de la derecha que se va a combinar.</param>
    /// <returns>
    /// Una nueva expresión que representa la combinación de las dos expresiones originales.
    /// Si alguna de las expresiones es nula, se devuelve la otra expresión.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite combinar expresiones de manera fluida.
    /// Utiliza la operación lógica "AND" para crear una nueva expresión que evalúa a verdadero
    /// solo si ambas expresiones originales son verdaderas.
    /// </remarks>
    /// <seealso cref="Expression"/>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;
        return left.Compose(right, Expression.AndAlso);
    }

    #endregion

    #region Or(o expresión)

    /// <summary>
    /// Combina dos expresiones lógicas utilizando la operación OR.
    /// </summary>
    /// <param name="left">La expresión de la izquierda.</param>
    /// <param name="right">La expresión de la derecha.</param>
    /// <returns>
    /// Una nueva expresión que representa la operación lógica OR entre las dos expresiones proporcionadas.
    /// Si la expresión de la izquierda es <c>null</c>, se devuelve la expresión de la derecha.
    /// Si la expresión de la derecha es <c>null</c>, se devuelve la expresión de la izquierda.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="Expression"/> y permite simplificar la combinación de expresiones lógicas.
    /// </remarks>
    public static Expression Or(this Expression left, Expression right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;
        return Expression.OrElse(left, right);
    }

    /// <summary>
    /// Combina dos expresiones booleanas utilizando la operación lógica OR.
    /// </summary>
    /// <typeparam name="T">El tipo de los parámetros de las expresiones.</typeparam>
    /// <param name="left">La primera expresión a combinar.</param>
    /// <param name="right">La segunda expresión a combinar.</param>
    /// <returns>
    /// Una nueva expresión que representa la combinación de las dos expresiones 
    /// originales utilizando la operación lógica OR. Si alguna de las expresiones 
    /// es nula, se devuelve la otra expresión.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite crear expresiones complejas de 
    /// manera más sencilla al combinar condiciones lógicas.
    /// </remarks>
    /// <seealso cref="Expression"/>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;
        return left.Compose(right, Expression.OrElse);
    }

    #endregion

    #region Value(Obtener el valor de una expresión lambda.)

    /// <summary>
    /// Obtiene el valor de una expresión lambda que representa una condición.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se está evaluando en la expresión.</typeparam>
    /// <param name="expression">La expresión lambda que contiene la condición a evaluar.</param>
    /// <returns>El valor resultante de evaluar la expresión.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de las expresiones lambda permitiendo obtener el valor
    /// asociado a la condición definida en la expresión. Es útil en situaciones donde se requiere
    /// evaluar dinámicamente condiciones en tiempo de ejecución.
    /// </remarks>
    /// <seealso cref="Lambda.GetValue(Expression{Func{T, bool}})"/>
    public static object Value<T>(this Expression<Func<T, bool>> expression)
    {
        return Lambda.GetValue(expression);
    }

    #endregion

    #region Equal(igual a la expresión)

    /// <summary>
    /// Compara dos expresiones para determinar si son iguales.
    /// </summary>
    /// <param name="left">La expresión de la izquierda que se va a comparar.</param>
    /// <param name="right">La expresión de la derecha que se va a comparar.</param>
    /// <returns>
    /// Una expresión que representa la comparación de igualdad entre las dos expresiones.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que facilita la creación de expresiones de igualdad
    /// utilizando el método estático <see cref="Expression.Equal(Expression, Expression)"/>.
    /// </remarks>
    public static Expression Equal(this Expression left, Expression right)
    {
        return Expression.Equal(left, right);
    }

    /// <summary>
    /// Compara una expresión con un valor dado para determinar si son iguales.
    /// </summary>
    /// <param name="left">La expresión a comparar.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la comparación de igualdad entre la expresión y el valor.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la clase <see cref="Expression"/> 
    /// para facilitar la comparación de expresiones con valores constantes.
    /// </remarks>
    public static Expression Equal(this Expression left, object value)
    {
        return left.Equal(Lambda.Constant(value, left));
    }

    #endregion

    #region NotEqual(No igual a expresión.)

    /// <summary>
    /// Crea una expresión que representa la operación de desigualdad entre dos expresiones.
    /// </summary>
    /// <param name="left">La expresión de la izquierda que se comparará.</param>
    /// <param name="right">La expresión de la derecha que se comparará.</param>
    /// <returns>
    /// Una expresión que representa la operación de desigualdad entre las expresiones proporcionadas.
    /// </returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="Expression"/> para facilitar la creación de expresiones de desigualdad.
    /// </remarks>
    public static Expression NotEqual(this Expression left, Expression right)
    {
        return Expression.NotEqual(left, right);
    }

    /// <summary>
    /// Crea una expresión que representa la comparación de desigualdad entre la expresión dada y un valor especificado.
    /// </summary>
    /// <param name="left">La expresión a la que se le aplicará la comparación de desigualdad.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una expresión que representa la operación de desigualdad entre la expresión <paramref name="left"/> y el <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite realizar comparaciones de desigualdad de manera más legible y concisa.
    /// </remarks>
    public static Expression NotEqual(this Expression left, object value)
    {
        return left.NotEqual(Lambda.Constant(value, left));
    }

    #endregion

    #region Greater(Mayor que expresión)

    /// <summary>
    /// Compara dos expresiones y determina si la expresión de la izquierda es mayor que la expresión de la derecha.
    /// </summary>
    /// <param name="left">La expresión a la izquierda de la comparación.</param>
    /// <param name="right">La expresión a la derecha de la comparación.</param>
    /// <returns>
    /// Una expresión que representa la operación de comparación, que evalúa si <paramref name="left"/> es mayor que <paramref name="right"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite realizar comparaciones de manera más legible y fluida.
    /// </remarks>
    public static Expression Greater(this Expression left, Expression right)
    {
        return Expression.GreaterThan(left, right);
    }

    /// <summary>
    /// Compara la expresión dada con un valor especificado para determinar si es mayor.
    /// </summary>
    /// <param name="left">La expresión que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la comparación de si la expresión es mayor que el valor especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite realizar comparaciones de manera más fluida 
    /// al trabajar con expresiones en C#.
    /// </remarks>
    public static Expression Greater(this Expression left, object value)
    {
        return left.Greater(Lambda.Constant(value, left));
    }

    #endregion

    #region GreaterEqual(Expresión mayor o igual.)

    /// <summary>
    /// Compara dos expresiones y determina si la expresión de la izquierda es mayor o igual que la expresión de la derecha.
    /// </summary>
    /// <param name="left">La expresión a la izquierda de la comparación.</param>
    /// <param name="right">La expresión a la derecha de la comparación.</param>
    /// <returns>
    /// Una expresión que representa la comparación de mayor o igual entre las dos expresiones proporcionadas.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="Expression"/> y permite construir expresiones de comparación de manera más legible.
    /// </remarks>
    public static Expression GreaterEqual(this Expression left, Expression right)
    {
        return Expression.GreaterThanOrEqual(left, right);
    }

    /// <summary>
    /// Compara una expresión con un valor dado y devuelve una nueva expresión que representa la operación de "mayor o igual".
    /// </summary>
    /// <param name="left">La expresión que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la comparación de "mayor o igual".
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="Expression"/> que permite realizar comparaciones de manera más fluida.
    /// </remarks>
    public static Expression GreaterEqual(this Expression left, object value)
    {
        return left.GreaterEqual(Lambda.Constant(value, left));
    }

    #endregion

    #region Less(Menor que expresión)

    /// <summary>
    /// Compara dos expresiones y determina si la expresión izquierda es menor que la expresión derecha.
    /// </summary>
    /// <param name="left">La expresión de la izquierda que se va a comparar.</param>
    /// <param name="right">La expresión de la derecha que se va a comparar.</param>
    /// <returns>
    /// Una expresión que representa la operación de comparación, que evaluará a verdadero si la expresión izquierda es menor que la expresión derecha; de lo contrario, evaluará a falso.
    /// </returns>
    public static Expression Less(this Expression left, Expression right)
    {
        return Expression.LessThan(left, right);
    }

    /// <summary>
    /// Compara la expresión dada con un valor y devuelve una nueva expresión que representa la operación "menor que".
    /// </summary>
    /// <param name="left">La expresión a la que se le aplicará la comparación.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la comparación "menor que" entre la expresión y el valor proporcionado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="Expression"/> que permite realizar comparaciones de manera más fluida.
    /// </remarks>
    public static Expression Less(this Expression left, object value)
    {
        return left.Less(Lambda.Constant(value, left));
    }

    #endregion

    #region LessEqual(Expresión menor o igual que.)

    /// <summary>
    /// Compara dos expresiones y determina si la primera es menor o igual que la segunda.
    /// </summary>
    /// <param name="left">La expresión a la izquierda de la comparación.</param>
    /// <param name="right">La expresión a la derecha de la comparación.</param>
    /// <returns>
    /// Una expresión que representa la comparación de menor o igual entre las dos expresiones proporcionadas.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que utiliza el método <see cref="Expression.LessThanOrEqual(Expression, Expression)"/> 
    /// para realizar la comparación.
    /// </remarks>
    public static Expression LessEqual(this Expression left, Expression right)
    {
        return Expression.LessThanOrEqual(left, right);
    }

    /// <summary>
    /// Compara una expresión con un valor dado y devuelve una nueva expresión que representa la operación de "menor o igual".
    /// </summary>
    /// <param name="left">La expresión a la que se le aplicará la comparación.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la comparación de "menor o igual" entre la expresión <paramref name="left"/> y el <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="Expression"/> para facilitar la comparación de expresiones con valores.
    /// </remarks>
    public static Expression LessEqual(this Expression left, object value)
    {
        return left.LessEqual(Lambda.Constant(value, left));
    }

    #endregion

    #region StartsWith(Comienza con)

    /// <summary>
    /// Crea una expresión que representa una llamada al método "StartsWith".
    /// </summary>
    /// <param name="left">La expresión a la que se le aplicará el método "StartsWith".</param>
    /// <param name="value">El valor con el que se comparará el inicio de la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la llamada al método "StartsWith".
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="Expression"/> y permite 
    /// construir de manera fluida expresiones que verifican si una cadena comienza 
    /// con un valor específico.
    /// </remarks>
    public static Expression StartsWith(this Expression left, object value)
    {
        return left.Call("StartsWith", new[] { typeof(string) }, value);
    }

    #endregion

    #region EndsWith(Termina con)

    /// <summary>
    /// Crea una expresión que representa una llamada al método <c>EndsWith</c> 
    /// para verificar si la cadena de la expresión <paramref name="left"/> 
    /// termina con el valor especificado.
    /// </summary>
    /// <param name="left">La expresión que representa la cadena a evaluar.</param>
    /// <param name="value">El valor que se desea comprobar si es el sufijo de la cadena.</param>
    /// <returns>Una expresión que representa la llamada al método <c>EndsWith</c>.</returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="Expression"/> y permite 
    /// construir dinámicamente expresiones que se pueden utilizar en consultas LINQ 
    /// o en otras operaciones que requieren la evaluación de expresiones.
    /// </remarks>
    public static Expression EndsWith(this Expression left, object value)
    {
        return left.Call("EndsWith", new[] { typeof(string) }, value);
    }

    #endregion

    #region Contains(Contiene)

    /// <summary>
    /// Verifica si la expresión de la izquierda contiene el valor especificado.
    /// </summary>
    /// <param name="left">La expresión en la que se busca el valor.</param>
    /// <param name="value">El valor que se desea buscar dentro de la expresión.</param>
    /// <returns>
    /// Una nueva expresión que representa la operación de búsqueda del valor en la expresión de la izquierda.
    /// </returns>
    public static Expression Contains(this Expression left, object value)
    {
        return left.Call("Contains", new[] { typeof(string) }, value);
    }

    #endregion

    #region Operation(Operación)

    /// <summary>
    /// Realiza una operación de comparación entre una expresión y un valor dado utilizando un operador especificado.
    /// </summary>
    /// <param name="left">La expresión sobre la cual se realizará la operación.</param>
    /// <param name="@operator">El operador que define el tipo de comparación a realizar.</param>
    /// <param name="value">El valor con el que se comparará la expresión.</param>
    /// <returns>Una nueva expresión que representa el resultado de la operación de comparación.</returns>
    /// <exception cref="NotImplementedException">Se lanza si el operador especificado no está implementado.</exception>
    public static Expression Operation(this Expression left, Operator @operator, object value)
    {
        switch (@operator)
        {
            case Operator.Equal:
                return left.Equal(value);
            case Operator.NotEqual:
                return left.NotEqual(value);
            case Operator.Greater:
                return left.Greater(value);
            case Operator.GreaterEqual:
                return left.GreaterEqual(value);
            case Operator.Less:
                return left.Less(value);
            case Operator.LessEqual:
                return left.LessEqual(value);
            case Operator.Starts:
                return left.StartsWith(value);
            case Operator.Ends:
                return left.EndsWith(value);
            case Operator.Contains:
                return left.Contains(value);
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// Realiza una operación de comparación entre dos expresiones utilizando el operador especificado.
    /// </summary>
    /// <param name="left">La expresión de la izquierda que se va a comparar.</param>
    /// <param name="@operator">El operador que define el tipo de comparación a realizar.</param>
    /// <param name="value">La expresión de la derecha que se va a comparar con la expresión de la izquierda.</param>
    /// <returns>Una expresión que representa el resultado de la operación de comparación.</returns>
    /// <exception cref="NotImplementedException">Se lanza si el operador especificado no está implementado.</exception>
    public static Expression Operation(this Expression left, Operator @operator, Expression value)
    {
        switch (@operator)
        {
            case Operator.Equal:
                return left.Equal(value);
            case Operator.NotEqual:
                return left.NotEqual(value);
            case Operator.Greater:
                return left.Greater(value);
            case Operator.GreaterEqual:
                return left.GreaterEqual(value);
            case Operator.Less:
                return left.Less(value);
            case Operator.LessEqual:
                return left.LessEqual(value);
        }
        throw new NotImplementedException();
    }

    #endregion

    #region Call(Llamada de método de expresión)

    /// <summary>
    /// Llama a un método en una instancia de expresión dada utilizando el nombre del método y los parámetros especificados.
    /// </summary>
    /// <param name="instance">La instancia de expresión sobre la cual se llama al método.</param>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="values">Los parámetros que se pasarán al método.</param>
    /// <returns>
    /// Una expresión que representa la llamada al método especificado, o <c>null</c> si el método no se encuentra.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si la instancia es <c>null</c>.</exception>
    public static Expression Call(this Expression instance, string methodName, params Expression[] values)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        var methodInfo = instance.Type.GetMethod(methodName);
        if (methodInfo == null)
            return null;
        return Expression.Call(instance, methodInfo, values);
    }

    /// <summary>
    /// Llama a un método especificado en una instancia de expresión.
    /// </summary>
    /// <param name="instance">La instancia de expresión sobre la cual se llama al método.</param>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="values">Los parámetros que se pasarán al método, si los hay.</param>
    /// <returns>
    /// Una expresión que representa la llamada al método especificado, o <c>null</c> si el método no se encuentra.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="instance"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método utiliza reflexión para buscar el método en el tipo de la instancia y crea una expresión de llamada.
    /// Si no se proporcionan valores, se llamará al método sin parámetros.
    /// </remarks>
    public static Expression Call(this Expression instance, string methodName, params object[] values)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        var methodInfo = instance.Type.GetMethod(methodName);
        if (methodInfo == null)
            return null;
        if (values == null || values.Length == 0)
            return Expression.Call(instance, methodInfo);
        return Expression.Call(instance, methodInfo, values.Select(Expression.Constant));
    }

    /// <summary>
    /// Llama a un método en una instancia de expresión dada, utilizando el nombre del método, 
    /// los tipos de parámetros y los valores proporcionados.
    /// </summary>
    /// <param name="instance">La instancia de expresión sobre la cual se llama al método.</param>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="paramTypes">Una matriz de tipos que representa los tipos de los parámetros del método.</param>
    /// <param name="values">Los valores que se pasarán como argumentos al método. 
    /// Puede ser un número variable de argumentos.</param>
    /// <returns>
    /// Una expresión que representa la llamada al método, o <c>null</c> si el método no se encuentra.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="instance"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método busca un método en el tipo de la instancia dada y crea una expresión para 
    /// invocar ese método. Si no se encuentran valores, se llamará al método sin argumentos.
    /// </remarks>
    public static Expression Call(this Expression instance, string methodName, Type[] paramTypes, params object[] values)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        var methodInfo = instance.Type.GetMethod(methodName, paramTypes);
        if (methodInfo == null)
            return null;
        if (values == null || values.Length == 0)
            return Expression.Call(instance, methodInfo);
        return Expression.Call(instance, methodInfo, values.Select(Expression.Constant));
    }

    #endregion

    #region Compose(Expresión combinada)

    /// <summary>
    /// Combina dos expresiones lambda en una nueva expresión utilizando una función de fusión especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la expresión lambda.</typeparam>
    /// <param name="first">La primera expresión lambda que se va a combinar.</param>
    /// <param name="second">La segunda expresión lambda que se va a combinar.</param>
    /// <param name="merge">La función que define cómo fusionar los cuerpos de las expresiones.</param>
    /// <returns>
    /// Una nueva expresión lambda que resulta de la fusión de las dos expresiones proporcionadas.
    /// </returns>
    /// <remarks>
    /// Este método toma dos expresiones y las combina de acuerdo a la lógica definida en el delegado <paramref name="merge"/>.
    /// Se asegura de que los parámetros de la segunda expresión se reemplacen adecuadamente por los de la primera expresión.
    /// </remarks>
    /// <seealso cref="Expression"/>
    /// <seealso cref="ParameterRebinder"/>
    internal static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    #endregion

    #region ToLambda(Crear una expresión Lambda.)

    /// <summary>
    /// Convierte una expresión en un delegado de tipo especificado.
    /// </summary>
    /// <typeparam name="TDelegate">El tipo del delegado que se generará.</typeparam>
    /// <param name="body">La expresión que se convertirá en un delegado.</param>
    /// <param name="parameters">Los parámetros que se utilizarán en el delegado.</param>
    /// <returns>
    /// Un objeto <see cref="Expression{TDelegate}"/> que representa el delegado generado,
    /// o <c>null</c> si la expresión de entrada es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="Expression"/> que permite
    /// crear fácilmente un delegado a partir de una expresión y sus parámetros.
    /// </remarks>
    public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body, params ParameterExpression[] parameters)
    {
        if (body == null)
            return null;
        return Expression.Lambda<TDelegate>(body, parameters);
    }

    #endregion

    #region ToPredicate(Crear expresiones de predicado.)

    /// <summary>
    /// Convierte una expresión en un predicado que puede ser utilizado para filtrar colecciones.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a filtrar.</typeparam>
    /// <param name="body">La expresión que define la lógica de filtrado.</param>
    /// <param name="parameters">Los parámetros de la expresión que se utilizarán en el predicado.</param>
    /// <returns>Una expresión que representa un predicado que toma un parámetro de tipo T y devuelve un valor booleano.</returns>
    /// <remarks>
    /// Este método es una extensión que permite crear un predicado a partir de una expresión y un conjunto de parámetros.
    /// Es útil en situaciones donde se necesita construir dinámicamente consultas basadas en expresiones.
    /// </remarks>
    /// <seealso cref="ToLambda{T}"/>
    public static Expression<Func<T, bool>> ToPredicate<T>(this Expression body, params ParameterExpression[] parameters)
    {
        return ToLambda<Func<T, bool>>(body, parameters);
    }

    #endregion
}