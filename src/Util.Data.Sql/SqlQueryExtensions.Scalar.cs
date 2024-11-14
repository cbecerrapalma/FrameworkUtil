using Util.Helpers;
using Convert = Util.Helpers.Convert;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para facilitar la construcción y ejecución de consultas SQL.
/// </summary>
public static partial class SqlQueryExtensions
{

    #region ToStringAsync(Obtener el valor de la cadena.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una representación de cadena de forma asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una cadena que representa el resultado de la consulta SQL.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método utiliza <see cref="ISqlQuery.ExecuteScalarAsync"/> para obtener el resultado de la consulta
    /// y luego lo convierte a una cadena segura utilizando <see cref="SafeString"/>.
    /// </remarks>
    public static async Task<string> ToStringAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return result.SafeString();
    }

    #endregion

    #region ToInt(Obtener un valor entero.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un entero.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>El resultado de la consulta convertido a un entero.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y ejecuta la consulta utilizando <see cref="ISqlQuery.ExecuteScalar"/>.
    /// Luego, convierte el resultado a un entero utilizando <see cref="Convert.ToInt32"/>.
    /// </remarks>
    public static int ToInt(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToInt(source.ExecuteScalar());
    }

    #endregion

    #region ToIntAsync(Obtener un valor entero.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un entero de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un <see cref="Task{Int32}"/> que representa el resultado de la conversión a entero.</returns>
    /// <remarks>
    /// Este método verifica si la fuente es nula antes de ejecutar la consulta. 
    /// Si la consulta se ejecuta correctamente, el resultado se convierte a un entero 
    /// utilizando <see cref="Convert.ToInt32(object)"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static async Task<int> ToIntAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToInt(result);
    }

    #endregion

    #region ToIntOrNull(Obtener un valor entero nullable.)

    /// <summary>
    /// Intenta convertir el resultado de una consulta SQL a un entero nullable.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un entero nullable que representa el resultado de la consulta, o null si la conversión falla.
    /// </returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y verifica si la fuente es nula antes de intentar ejecutar la consulta.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static int? ToIntOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToIntOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToIntOrNullAsync(Obtener un valor entero nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un entero nullable de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un entero nullable que representa el resultado de la consulta, o null si el resultado es nulo o no se puede convertir.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método utiliza <see cref="ISqlQuery.ExecuteScalarAsync"/> para ejecutar la consulta y obtener el resultado.
    /// Luego, utiliza <see cref="Convert.ToIntOrNull(object)"/> para convertir el resultado en un entero nullable.
    /// </remarks>
    public static async Task<int?> ToIntOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToIntOrNull(result);
    }

    #endregion

    #region ToLong(Obtener un valor entero.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor de tipo <see cref="long"/>.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>El resultado de la consulta convertido a <see cref="long"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método ejecuta la consulta SQL y utiliza <see cref="Convert.ToLong(object)"/> 
    /// para convertir el resultado a un valor de tipo <see cref="long"/>. 
    /// Asegúrese de que la consulta SQL retorne un valor que pueda ser convertido a <see cref="long"/>.
    /// </remarks>
    public static long ToLong(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToLong(source.ExecuteScalar());
    }

    #endregion

    #region ToLongAsync(Obtener un valor entero.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor de tipo <see cref="long"/> de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un <see cref="Task{long}"/> que representa el resultado de la consulta convertido a <see cref="long"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método verifica si la fuente de la consulta es nula antes de ejecutar la consulta. 
    /// Si la consulta se ejecuta correctamente, el resultado se convierte a tipo <see cref="long"/> y se devuelve.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<long> ToLongAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToLong(result);
    }

    #endregion

    #region ToLongOrNull(Obtener un valor entero nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="long"/> o <c>null</c>.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> si la consulta devuelve un resultado válido, 
    /// de lo contrario, <c>null</c> si no hay resultados o si el resultado no se puede convertir a <see cref="long"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToLongOrNull(object)"/>
    public static long? ToLongOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToLongOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToLongOrNullAsync(Obtener un valor entero nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="long"/> o null de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> si la conversión es exitosa; de lo contrario, null.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo antes de ejecutar la consulta.
    /// Si la consulta no devuelve un resultado que se pueda convertir a <see cref="long"/>, se retornará null.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToLongOrNull(object)"/>
    public static async Task<long?> ToLongOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToLongOrNull(result);
    }

    #endregion

    #region ToGuid(Obtener el valor de Guid.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un objeto Guid.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un objeto Guid que representa el resultado de la consulta.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y ejecuta la consulta
    /// para obtener un valor escalar, que luego se convierte a Guid.
    /// </remarks>
    public static Guid ToGuid(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToGuid(source.ExecuteScalar());
    }

    #endregion

    #region ToGuidAsync(Obtener valor de Guid.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un <see cref="Guid"/> de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un <see cref="Task{Guid}"/> que representa el resultado de la consulta como un <see cref="Guid"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo antes de ejecutar la consulta.
    /// Luego, ejecuta la consulta de forma asíncrona y convierte el resultado en un <see cref="Guid"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<Guid> ToGuidAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToGuid(result);
    }

    #endregion

    #region ToGuidOrNull(Obtener un valor Guid nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="Guid"/> o devuelve <c>null</c> si el resultado es nulo.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un objeto <see cref="Guid"/> si la consulta devuelve un valor válido; de lo contrario, <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToGuidOrNull(object)"/>
    public static Guid? ToGuidOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToGuidOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToGuidOrNullAsync(Obtener un valor Guid nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="Guid"/> o devuelve <c>null</c> si no se puede convertir.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un objeto <see cref="Guid"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ISqlQuery"/> y permite ejecutar una consulta
    /// de forma asíncrona, obteniendo un valor que se puede convertir a <see cref="Guid"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToGuidOrNull(object)"/>
    public static async Task<Guid?> ToGuidOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToGuidOrNull(result);
    }

    #endregion

    #region ToBool(Obtener valor booleano.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor booleano.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor booleano que representa el resultado de la consulta SQL.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el parámetro <paramref name="source"/> es nulo antes de ejecutar la consulta.
    /// Si el resultado de la consulta no puede convertirse a un booleano, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static bool ToBool(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToBool(source.ExecuteScalar());
    }

    #endregion

    #region ToBoolAsync(Obtener un valor booleano.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor booleano de forma asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica el resultado de la consulta.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método ejecuta la consulta SQL y convierte el resultado en un valor booleano. 
    /// Se espera que la consulta devuelva un valor que se pueda convertir a booleano.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<bool> ToBoolAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToBool(result);
    }

    #endregion

    #region ToBoolOrNull(Obtener un valor booleano nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor booleano nullable.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor booleano nullable que representa el resultado de la consulta. 
    /// Devuelve <c>null</c> si el resultado es nulo o no se puede convertir a booleano.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToBoolOrNull(object)"/>
    public static bool? ToBoolOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToBoolOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToBoolOrNullAsync(Obtener un valor booleano nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor booleano o nulo de forma asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un <see cref="bool?"/> que representa el resultado de la consulta, 
    /// donde puede ser verdadero, falso o nulo si no hay un resultado válido.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método utiliza la extensión <c>CheckNull</c> para validar que la consulta no sea nula 
    /// antes de ejecutar la consulta y convertir el resultado.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToBoolOrNull(object)"/>
    public static async Task<bool?> ToBoolOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToBoolOrNull(result);
    }

    #endregion

    #region ToFloat(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un número de punto flotante.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>El resultado de la consulta convertido a un número de punto flotante.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y permite ejecutar una consulta SQL
    /// y obtener el resultado como un valor de tipo <see cref="float"/>. 
    /// Asegúrese de que la consulta SQL devuelva un valor que pueda ser convertido a <see cref="float"/>.
    /// </remarks>
    public static float ToFloat(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToFloat(source.ExecuteScalar());
    }

    #endregion

    #region ToFloatAsync(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="float"/> de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, 
    /// con un valor de tipo <see cref="float"/> que contiene el resultado de la consulta.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo antes de intentar ejecutar la consulta.
    /// Si el resultado de la consulta no se puede convertir a <see cref="float"/>, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<float> ToFloatAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToFloat(result);
    }

    #endregion

    #region ToFloatOrNull(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor de tipo <see cref="float"/> o null.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor de tipo <see cref="float"/> si la consulta devuelve un resultado válido; de lo contrario, null.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToFloatOrNull(object)"/>
    public static float? ToFloatOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToFloatOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToFloatOrNullAsync(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor flotante o null de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un valor flotante que representa el resultado de la consulta, o null si el resultado es nulo.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y permite ejecutar una consulta SQL
    /// que devuelve un único valor, convirtiéndolo a un tipo flotante. Si la consulta no devuelve ningún
    /// resultado o el resultado es nulo, el método devolverá null.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="Convert.ToFloatOrNull(object)"/>
    public static async Task<float?> ToFloatOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToFloatOrNull(result);
    }

    #endregion

    #region ToDouble(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor de tipo <see cref="double"/>.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>El resultado de la consulta convertido a <see cref="double"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método utiliza el método <see cref="ISqlQuery.ExecuteScalar"/> para obtener el resultado de la consulta
    /// y luego lo convierte a un valor de tipo <see cref="double"/>. Asegúrese de que el resultado de la consulta
    /// sea convertible a <see cref="double"/> para evitar excepciones en tiempo de ejecución.
    /// </remarks>
    public static double ToDouble(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToDouble(source.ExecuteScalar());
    }

    #endregion

    #region ToDoubleAsync(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="double"/> de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, 
    /// con un valor de tipo <see cref="double"/> que contiene el resultado de la consulta.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método verifica que la fuente de la consulta no sea nula antes de ejecutar la consulta.
    /// Luego, ejecuta la consulta de forma asíncrona y convierte el resultado a un valor de tipo <see cref="double"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<double> ToDoubleAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToDouble(result);
    }

    #endregion

    #region ToDoubleOrNull(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="double"/> o <see langword="null"/> si no se puede convertir.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un valor de tipo <see cref="double"/> si la conversión es exitosa; de lo contrario, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <see langword="null"/>.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y proporciona una forma conveniente de obtener un valor numérico
    /// a partir de una consulta SQL, manejando la posibilidad de que el resultado no sea convertible a <see cref="double"/>.
    /// </remarks>
    public static double? ToDoubleOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToDoubleOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToDoubleOrNullAsync(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor de tipo <see cref="double"/> o <c>null</c> si no es posible.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor de tipo <see cref="double"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ISqlQuery"/> y permite ejecutar una consulta SQL de manera asíncrona,
    /// obteniendo el resultado y convirtiéndolo a un valor de tipo <see cref="double"/> si es posible.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<double?> ToDoubleOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToDoubleOrNull(result);
    }

    #endregion

    #region ToDecimal(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor decimal.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor decimal que representa el resultado de la consulta SQL.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método utiliza el método <c>ExecuteScalar</c> de la interfaz <c>ISqlQuery</c> 
    /// para obtener el resultado de la consulta y luego lo convierte a decimal.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static decimal ToDecimal(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToDecimal(source.ExecuteScalar());
    }

    #endregion

    #region ToDecimalAsync(Obtener valor de punto flotante.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor decimal de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor decimal que representa el resultado de la consulta SQL.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método utiliza <see cref="ISqlQuery.ExecuteScalarAsync"/> para ejecutar la consulta y obtener el resultado,
    /// que luego se convierte a decimal. Asegúrese de que el resultado de la consulta sea convertible a decimal.
    /// </remarks>
    public static async Task<decimal> ToDecimalAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToDecimal(result);
    }

    #endregion

    #region ToDecimalOrNull(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL a un valor decimal nullable.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor decimal nullable que representa el resultado de la consulta SQL, 
    /// o null si el resultado es nulo o no puede ser convertido a decimal.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="source"/> es null.
    /// </exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> y proporciona una forma 
    /// conveniente de obtener un valor decimal a partir de una consulta SQL ejecutada.
    /// </remarks>
    public static decimal? ToDecimalOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return Convert.ToDecimalOrNull(source.ExecuteScalar());
    }

    #endregion

    #region ToDecimalOrNullAsync(Obtener un valor de punto flotante nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un valor decimal o nulo de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un valor decimal que representa el resultado de la consulta, o null si el resultado es nulo o no se puede convertir a decimal.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la fuente de la consulta es nula antes de ejecutar la consulta.
    /// Si la consulta devuelve un resultado que no puede ser convertido a decimal, se devolverá null.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="Convert.ToDecimalOrNull(object)"/>
    public static async Task<decimal?> ToDecimalOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return Convert.ToDecimalOrNull(result);
    }

    #endregion

    #region ToDateTime(Obtener valor de fecha.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="DateTime"/>.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa el resultado de la consulta en hora local.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> para facilitar la conversión
    /// de resultados de consultas SQL a un tipo de dato <see cref="DateTime"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ToLocalTime(object)"/>
    public static DateTime ToDateTime(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = source.ExecuteScalar();
        return ToLocalTime(result);
    }

    /// <summary>
    /// Convierte un objeto de fecha y hora a la hora local.
    /// </summary>
    /// <param name="date">El objeto que representa la fecha y hora a convertir.</param>
    /// <returns>
    /// Devuelve la fecha y hora en formato local. Si la conversión falla, se retorna <see cref="DateTime.MinValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el objeto proporcionado a un valor de tipo <see cref="DateTime"/>. 
    /// Si la conversión no es exitosa, se devolverá el valor mínimo de <see cref="DateTime"/>.
    /// </remarks>
    private static DateTime ToLocalTime(object date)
    {
        var result = Convert.ToDateTimeOrNull(date);
        if (result == null)
            return DateTime.MinValue;
        return Time.UtcToLocalTime(result.Value);
    }

    #endregion

    #region ToDateTimeAsync(Obtener valor de fecha.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="DateTime"/> de forma asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un <see cref="Task{DateTime}"/> que representa la operación asíncrona, 
    /// con un <see cref="DateTime"/> que contiene el resultado de la consulta en hora local.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método ejecuta una consulta SQL y espera un resultado escalar, 
    /// que se convierte a la hora local antes de ser devuelto.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ToLocalTime(object)"/>
    public static async Task<DateTime> ToDateTimeAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return ToLocalTime(result);
    }

    #endregion

    #region ToDateTimeOrNull(Obtener un valor de fecha nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="DateTime"/> 
    /// o devuelve <c>null</c> si el resultado es nulo.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>Un objeto <see cref="DateTime"/> que representa el resultado de la consulta, 
    /// o <c>null</c> si el resultado es nulo.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ToLocalTimeOrNull(object)"/>
    public static DateTime? ToDateTimeOrNull(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        return ToLocalTimeOrNull(source.ExecuteScalar());
    }

    /// <summary>
    /// Convierte un objeto de fecha y hora a la hora local si es posible.
    /// </summary>
    /// <param name="date">El objeto que representa la fecha y hora a convertir.</param>
    /// <returns>
    /// Un <see cref="DateTime?"/> que representa la fecha y hora en hora local, 
    /// o null si la conversión no es posible.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el objeto proporcionado a un valor de tipo 
    /// <see cref="DateTime"/>. Si la conversión es exitosa, se transforma a hora 
    /// local utilizando el método <see cref="Time.UtcToLocalTime"/>. Si la conversión 
    /// falla, se devuelve null.
    /// </remarks>
    private static DateTime? ToLocalTimeOrNull(object date)
    {
        var result = Convert.ToDateTimeOrNull(date);
        if (result == null)
            return null;
        return Time.UtcToLocalTime(result.Value);
    }

    #endregion

    #region ToDateTimeOrNullAsync(Obtener un valor de fecha nullable.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en un objeto <see cref="DateTime"/> o devuelve null si no hay resultado.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa el resultado de la consulta, o null si el resultado es nulo.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método ejecuta una consulta SQL de manera asíncrona y convierte el resultado en la hora local.
    /// Si la consulta no devuelve un resultado, se retornará null.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ToLocalTimeOrNull(object)"/>
    public static async Task<DateTime?> ToDateTimeOrNullAsync(this ISqlQuery source)
    {
        source.CheckNull(nameof(source));
        var result = await source.ExecuteScalarAsync();
        return ToLocalTimeOrNull(result);
    }

    #endregion
}