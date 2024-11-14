namespace Util.Data.Sql;

/// <summary>
/// Clase estática que contiene extensiones para facilitar la construcción y ejecución de consultas SQL.
/// </summary>
public static partial class SqlQueryExtensions
{

    #region ToEntity(Obtener una sola entidad.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una entidad del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad a la que se convertirá el resultado de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera opcional para la ejecución de la consulta, en segundos.</param>
    /// <returns>Una instancia de la entidad del tipo especificado que representa el resultado de la consulta.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ExecuteSingle{TEntity}(int?)"/>
    public static TEntity ToEntity<TEntity>(this ISqlQuery source, int? timeout = null)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteSingle<TEntity>(timeout);
    }

    #endregion

    #region ToEntityAsync(Obtener una sola entidad.)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una entidad de tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad a la que se convertirá el resultado de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene la entidad de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para obtener una única entidad de una consulta SQL asíncrona. 
    /// Asegúrese de que la consulta SQL esté diseñada para devolver un solo resultado.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<TEntity> ToEntityAsync<TEntity>(this ISqlQuery source, int? timeout = null)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteSingleAsync<TEntity>(timeout);
    }

    #endregion

    #region ToList(Obtener conjunto de entidades)

    /// <summary>
    /// Convierte una consulta SQL en una lista dinámica.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a convertir en lista.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>
    /// Una lista dinámica que contiene los resultados de la consulta SQL.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> para facilitar la conversión de resultados en una lista.
    /// </remarks>
    public static List<dynamic> ToList(this ISqlQuery source, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(timeout, buffered);
    }

    /// <summary>
    /// Convierte una consulta SQL en una lista de entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se desea obtener de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades del tipo especificado obtenidas a partir de la consulta SQL.</returns>
    /// <exception cref="ArgumentNullException">Se lanzará si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<TEntity>(this ISqlQuery source, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery<TEntity>(timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro que se utilizará en el mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro que se utilizará en el mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades generadas a partir de los resultados de la consulta.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para transformar los resultados de una consulta SQL en objetos de tipo <typeparamref name="TEntity"/> 
    /// utilizando una función de mapeo que toma dos parámetros de tipo <typeparamref name="T1"/> y <typeparamref name="T2"/>.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, TEntity>(this ISqlQuery source, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante.</typeparam>
    /// <param name="source">La fuente de la consulta SQL que se va a ejecutar.</param>
    /// <param name="map">Función de mapeo que toma tres parámetros y devuelve una entidad del tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades del tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlQuery"/> para facilitar la conversión de resultados de consultas SQL en listas de entidades.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, T3, TEntity>(this ISqlQuery source, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método permite ejecutar una consulta SQL y mapear los resultados a una lista de entidades utilizando una función de mapeo proporcionada.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, T3, T4, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte una consulta SQL en una lista de entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados de la consulta a una entidad del tipo especificado.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria.</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, T3, T4, T5, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La consulta SQL de origen que se ejecutará.</param>
    /// <param name="map">Una función que mapea los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades generadas a partir de los resultados de la consulta SQL.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para transformar los resultados de una consulta SQL en una lista de objetos de tipo <typeparamref name="TEntity"/> 
    /// utilizando una función de mapeo que define cómo se deben convertir los resultados de la consulta.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, T3, T4, T5, T6, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte una consulta SQL en una lista de entidades de tipo especificado.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para ejecutar consultas SQL y mapear los resultados a una lista de entidades de manera eficiente.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static List<TEntity> ToList<T1, T2, T3, T4, T5, T6, T7, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return source.ExecuteQuery(map, timeout, buffered);
    }

    #endregion

    #region ToListAsync(Obtener conjunto de entidades)

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de objetos dinámicos de manera asíncrona.
    /// </summary>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera opcional en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con una lista de objetos dinámicos como resultado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ISqlQuery"/> y permite obtener los resultados de la consulta de forma asíncrona.
    /// </remarks>
    public static async Task<List<dynamic>> ToListAsync(this ISqlQuery source, int? timeout = null)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(timeout);
    }

    /// <summary>
    /// Convierte un objeto <see cref="ISqlQuery"/> en una lista de entidades de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera en la lista resultante.</typeparam>
    /// <param name="source">La consulta SQL que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera opcional para la ejecución de la consulta, en segundos.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    /// <seealso cref="ExecuteQueryAsync{TEntity}(int?)"/>
    public static async Task<List<TEntity>> ToListAsync<TEntity>(this ISqlQuery source, int? timeout = null)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync<TEntity>(timeout);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro que se utilizará en el mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro que se utilizará en el mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La consulta SQL que se ejecutará.</param>
    /// <param name="map">Función que define cómo mapear los resultados de tipo <typeparamref name="T1"/> y <typeparamref name="T2"/> a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asincrónica, que contiene una lista de entidades de tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método permite ejecutar una consulta SQL y mapear los resultados a una lista de entidades utilizando una función de mapeo proporcionada.
    /// Asegúrese de que la función de mapeo pueda manejar correctamente los tipos de datos devueltos por la consulta.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, TEntity>(this ISqlQuery source, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro en la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro en la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro en la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La fuente de consulta SQL desde la cual se obtendrán los datos.</param>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado de la tarea es una lista de entidades de tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas SQL y mapear los resultados a objetos de dominio.
    /// Asegúrese de que la función de mapeo maneje correctamente los tipos y la lógica de conversión.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, T3, TEntity>(this ISqlQuery source, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo especificado.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La fuente de consulta SQL desde la cual se obtendrán los datos.</param>
    /// <param name="map">Una función que define cómo mapear los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una lista de entidades de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas SQL y mapear los resultados a objetos de dominio en aplicaciones que utilizan acceso a datos asincrónico.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, T3, T4, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo especificado.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La fuente de consulta SQL desde la cual se obtendrán los datos.</param>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de entidades de tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas asíncronas a una base de datos y mapear los resultados a un tipo de entidad específico.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, T3, T4, T5, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo especificado.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="source">La fuente de consulta SQL desde la cual se obtendrán los datos.</param>
    /// <param name="map">Una función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si deben ser procesados de forma diferida (false).</param>
    /// <returns>Una tarea que representa la operación asincrónica, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas SQL y mapear los resultados a una lista de entidades de manera asincrónica.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, T3, T4, T5, T6, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    /// <summary>
    /// Convierte el resultado de una consulta SQL en una lista de entidades de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de entidad que se generará a partir de la consulta.</typeparam>
    /// <param name="source">La fuente de consulta SQL desde la cual se obtendrán los datos.</param>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para ejecutar la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite ejecutar consultas SQL de manera asincrónica y mapear los resultados a una lista de entidades.
    /// Asegúrese de que la función de mapeo proporcione la lógica adecuada para transformar los resultados de la consulta.
    /// </remarks>
    /// <seealso cref="ISqlQuery"/>
    public static async Task<List<TEntity>> ToListAsync<T1, T2, T3, T4, T5, T6, T7, TEntity>(this ISqlQuery source, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        source.CheckNull(nameof(source));
        return await source.ExecuteQueryAsync(map, timeout, buffered);
    }

    #endregion
}