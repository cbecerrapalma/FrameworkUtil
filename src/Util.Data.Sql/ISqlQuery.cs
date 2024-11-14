using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;
using Util.Data.Sql.Configs;

namespace Util.Data.Sql; 

/// <summary>
/// Define una interfaz para operaciones de consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlQueryOperation"/> y <see cref="ISqlOptions"/> 
/// y proporciona funcionalidades adicionales relacionadas con la gestión de recursos.
/// </remarks>
public interface ISqlQuery : ISqlQueryOperation, ISqlOptions, IDisposable {
    /// <summary>
    /// Obtiene el identificador del contexto.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser utilizado para rastrear o identificar el contexto actual
    /// en el que se está ejecutando el código. Es un valor de solo lectura.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador del contexto.
    /// </value>
    string ContextId { get; }
    /// <summary>
    /// Obtiene el resultado de la consulta SQL anterior.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="SqlBuilderResult"/> que representa el resultado de la consulta SQL previa.
    /// </value>
    SqlBuilderResult PreviousSql { get; }
    /// <summary>
    /// Obtiene una instancia de un constructor de consultas SQL.
    /// </summary>
    /// <value>
    /// Una implementación de <see cref="ISqlBuilder"/> que se utiliza para construir consultas SQL.
    /// </value>
    ISqlBuilder SqlBuilder { get; }
    /// <summary>
    /// Verifica si existe una entidad o recurso específico.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la entidad o recurso existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para comprobar la existencia de un recurso antes de realizar operaciones que dependen de su presencia.
    /// </remarks>
    bool ExecuteExists();
    /// <summary>
    /// Ejecuta una operación asincrónica para verificar si un elemento existe.
    /// </summary>
    /// <returns>
    /// Un <see cref="Task{Boolean}"/> que representa la operación asincrónica. 
    /// El valor devuelto es <c>true</c> si el elemento existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para verificar la existencia de un elemento en una base de datos, 
    /// un archivo o cualquier otro recurso que se esté gestionando.
    /// </remarks>
    /// <seealso cref="ExecuteAsync"/>
    Task<bool> ExecuteExistsAsync();
    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el valor de la primera columna de la primera fila 
    /// del conjunto de resultados devuelto por la consulta. Si la consulta no devuelve filas, 
    /// se devuelve null.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos que se espera para que la consulta se ejecute. 
    /// Si se omite, se utilizará el valor predeterminado de tiempo de espera.</param>
    /// <returns>Un objeto que representa el valor de la primera columna de la primera fila 
    /// del conjunto de resultados. Puede ser null si no se devuelven filas.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que devuelven un único valor, como 
    /// una suma, un conteo o un valor específico de una tabla. 
    /// Asegúrese de manejar adecuadamente el caso en que no se devuelvan filas.
    /// </remarks>
    /// <seealso cref="ExecuteReader"/>
    /// <seealso cref="ExecuteNonQuery"/>
    object ExecuteScalar( int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer valor de la primera fila del conjunto de resultados.
    /// </summary>
    /// <typeparam name="T">El tipo de dato del valor que se espera obtener como resultado.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>El primer valor de la primera fila del conjunto de resultados, convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para consultas que devuelven un solo valor, como funciones de agregación o consultas que devuelven un único campo.
    /// </remarks>
    /// <seealso cref="ExecuteReader"/>
    /// <seealso cref="ExecuteNonQuery"/>
    T ExecuteScalar<T>( int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer valor de la primera fila de resultados.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El valor devuelto es el primer valor de la primera fila de resultados, o null si no hay resultados.</returns>
    /// <remarks>
    /// Este método es útil para consultas que devuelven un solo valor, como una suma o un conteo.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="ExecuteReaderAsync"/>
    /// <seealso cref="ExecuteNonQueryAsync"/>
    Task<object> ExecuteScalarAsync( int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el resultado de la primera columna de la primera fila 
    /// de los resultados devueltos, o un valor nulo si no hay filas. Esta operación se realiza de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo de dato que se espera como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se espera para que la operación se complete. 
    /// Si es nulo, se utilizará el valor predeterminado de tiempo de espera.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea contiene el valor 
    /// de la primera columna de la primera fila de los resultados.</returns>
    /// <remarks>
    /// Este método es útil para consultas que devuelven un solo valor, como una suma, un conteo o un valor de configuración.
    /// </remarks>
    /// <seealso cref="ExecuteReaderAsync"/>
    /// <seealso cref="ExecuteNonQueryAsync"/>
    Task<T> ExecuteScalarAsync<T>( int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un valor escalar.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se espera para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Un objeto que representa el valor escalar devuelto por el procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven un único valor, 
    /// como un conteo o una suma. Asegúrese de que el procedimiento almacenado esté diseñado para 
    /// devolver un valor escalar.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureScalar(string, int?)"/>
    object ExecuteProcedureScalar( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un valor escalar.
    /// </summary>
    /// <typeparam name="T">El tipo de dato del valor que se espera recibir como resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>Un valor del tipo especificado <typeparamref name="T"/> que representa el resultado del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos que devuelven un solo valor, como conteos o sumas.
    /// Asegúrese de que el procedimiento almacenado esté diseñado para devolver un valor escalar.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureScalar{T}(string, int?)"/>
    T ExecuteProcedureScalar<T>( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un resultado escalar de forma asíncrona.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es <c>null</c>, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es un objeto que representa el valor escalar devuelto por el procedimiento.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven un único valor, como un conteo o un valor agregado.
    /// Asegúrese de que el procedimiento almacenado esté diseñado para devolver un resultado escalar.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="procedure"/> es <c>null</c> o vacío.</exception>
    /// <seealso cref="ExecuteProcedureScalarAsync(string, int?)"/>
    Task<object> ExecuteProcedureScalarAsync( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un valor escalar de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor escalar que se espera recibir como resultado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se espera para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor de tipo <typeparamref name="T"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven un solo valor, como un conteo, suma o cualquier otro tipo de dato escalar.
    /// Asegúrese de que el procedimiento almacenado esté diseñado para devolver un valor escalar.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureAsync{T}(string, object[], int?)"/>
    Task<T> ExecuteProcedureScalarAsync<T>( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta una operación que devuelve una única entidad del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a devolver.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la operación. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una instancia de <typeparamref name="TEntity"/> que representa la entidad obtenida.</returns>
    /// <remarks>
    /// Este método es útil cuando se espera que la operación devuelva exactamente una entidad. 
    /// Si se devuelve más de una entidad, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="ExecuteSingleOrDefault{TEntity}(int?)"/>
    TEntity ExecuteSingle<TEntity>( int? timeout = null );
    /// <summary>
    /// Ejecuta una operación asíncrona que devuelve un único resultado de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la operación.</typeparam>
    /// <param name="timeout">El tiempo máximo en milisegundos que se espera para completar la operación. Si es <c>null</c>, se utilizará un tiempo de espera predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una instancia de <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para obtener un único registro de una fuente de datos de manera asíncrona, permitiendo especificar un tiempo de espera opcional.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<TEntity> ExecuteSingleAsync<TEntity>( int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un único resultado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se espera para la ejecución del procedimiento. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>Una instancia de <typeparamref name="TEntity"/> que representa el resultado del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos que devuelven un solo registro. 
    /// Asegúrese de que el procedimiento almacenado esté diseñado para devolver un único resultado.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureSingle{TEntity}(string, int?)"/>
    TEntity ExecuteProcedureSingle<TEntity>( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y devuelve un único resultado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una instancia de <typeparamref name="TEntity"/> que representa el resultado del procedimiento.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven un único registro. 
    /// Asegúrese de que el procedimiento almacenado esté diseñado para devolver un solo resultado.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureAsync{TEntity}(string, int?)"/>
    Task<TEntity> ExecuteProcedureSingleAsync<TEntity>( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta y devuelve una lista de resultados dinámicos.
    /// </summary>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si deben ser leídos de forma diferida (false).</param>
    /// <returns>Una lista de objetos dinámicos que representan los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas SQL y obtener resultados sin necesidad de definir una clase específica para los datos retornados.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<dynamic> ExecuteQuery( int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y devuelve una lista de entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera devolver.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permite para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria (true) o si se deben procesar de forma diferida (false).</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas a la base de datos y obtener los resultados en forma de lista.
    /// Asegúrese de que el tipo de entidad especificado esté mapeado correctamente en el contexto de datos.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<TEntity> ExecuteQuery<TEntity>( int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si se deben procesar de forma diferida (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método permite ejecutar una consulta que puede involucrar múltiples tipos y mapear los resultados a una lista de entidades.
    /// Asegúrese de que la función de mapeo esté correctamente implementada para evitar errores en el mapeo de datos.
    /// </remarks>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> ExecuteQuery<T1, T2, TEntity>( Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que devuelven múltiples resultados y transformarlos en una lista de entidades.
    /// Asegúrese de que la función de mapeo pueda manejar correctamente los tipos de entrada y salida.
    /// </remarks>
    /// <seealso cref="ExecuteQuery{TEntity}(Func{TEntity})"/>
    List<TEntity> ExecuteQuery<T1, T2, T3, TEntity>( Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para ejecutar la consulta. Si es nulo, se usará el valor por defecto.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si se deben procesar de forma diferida (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método permite ejecutar consultas que requieren múltiples parámetros y mapear los resultados a una entidad específica.
    /// Es útil en escenarios donde se necesita transformar datos complejos en objetos de negocio.
    /// </remarks>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> ExecuteQuery<T1, T2, T3, T4, TEntity>( Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades resultantes del mapeo.</returns>
    /// <remarks>
    /// Este método permite ejecutar una consulta y transformar los resultados en una lista de entidades 
    /// utilizando una función de mapeo personalizada. Se puede especificar un tiempo de espera y si los 
    /// resultados deben ser almacenados en memoria.
    /// </remarks>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, TEntity>( Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante.</typeparam>
    /// <param name="map">Función que mapea los parámetros a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades mapeadas a partir de los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas complejas donde se requiere mapear múltiples resultados a una sola entidad.
    /// Asegúrese de que la función de mapeo maneje correctamente los tipos de los parámetros.
    /// </remarks>
    /// <seealso cref="ExecuteQuery{TEntity}(Func{TEntity})"/>
    List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, T6, TEntity>( Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros de entrada a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si deben ser procesados de forma diferida (false).</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método permite ejecutar consultas que devuelven múltiples resultados y los transforma en una lista de entidades utilizando la función de mapeo proporcionada.
    /// </remarks>
    /// <seealso cref="ExecuteQuery{TEntity}(Func{TEntity})"/>
    List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, T6, T7, TEntity>( Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de manera asíncrona y devuelve una lista de resultados dinámicos.
    /// </summary>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de resultados dinámicos como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que pueden devolver un número variable de resultados 
    /// y donde el tipo de los resultados no es conocido en tiempo de compilación.
    /// </remarks>
    /// <exception cref="Exception">Se lanzará una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    Task<List<dynamic>> ExecuteQueryAsync( int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta asíncrona y devuelve una lista de entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permite para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de entidades del tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas a bases de datos o fuentes de datos que soportan operaciones asíncronas.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="Task"/>
    /// <seealso cref="List{T}"/>
    Task<List<TEntity>> ExecuteQueryAsync<TEntity>( int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a un tipo específico.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro utilizado en el mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro utilizado en el mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que requieren un mapeo personalizado de los resultados a un tipo de entidad específico.
    /// Asegúrese de que la función de mapeo pueda manejar correctamente los tipos de los parámetros proporcionados.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, TEntity>( Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante después de aplicar el mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros de entrada a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que requieren múltiples parámetros y devolver un conjunto de resultados mapeados a un tipo específico.
    /// </remarks>
    /// <seealso cref="ExecuteQueryAsync{T1, T2, TEntity}(Func{T1, T2, TEntity}, int?, bool)"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, TEntity>( Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="map">Función que mapea los parámetros de tipo <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/> y <typeparamref name="T4"/> a una instancia de <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que requieren múltiples parámetros y transformar los resultados en una lista de entidades.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, TEntity>( Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de entrada para la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros de entrada a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos que se permite para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene una lista de entidades mapeadas.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que requieren múltiples parámetros y transformar los resultados en un tipo de entidad específico.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, TEntity>( Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas complejas que requieren múltiples parámetros y transformar los resultados en un formato específico.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, T6, TEntity>( Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas complejas que requieren múltiples parámetros y transformar los resultados en una forma más útil.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, T6, T7, TEntity>( Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve los resultados en una lista dinámica.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si se deben procesar de forma secuencial (false).</param>
    /// <returns>Una lista dinámica que contiene los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven conjuntos de resultados.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido en la base de datos.
    /// </remarks>
    /// <seealso cref="System.Dynamic.DynamicObject"/>
    List<dynamic> ExecuteProcedureQuery( string procedure, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y devuelve una lista de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven conjuntos de resultados.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido en la base de datos y que
    /// el tipo de entidad coincida con la estructura de los datos devueltos.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<TEntity> ExecuteProcedureQuery<TEntity>( string procedure, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro que se espera en el procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro que se espera en el procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. Si es verdadero, los resultados se almacenan en memoria.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en objetos de dominio.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de datos coincidan con los parámetros esperados.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, TEntity>( string procedure, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser procesados. El valor predeterminado es verdadero.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en una lista de objetos.
    /// Asegúrese de que el procedimiento almacenado devuelva los tipos de datos correctos que coincidan con los tipos de los parámetros de salida.
    /// </remarks>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, T3, TEntity>( string procedure, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento almacenado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento almacenado a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento almacenado. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados que devuelven múltiples resultados y los mapea a una lista de entidades.
    /// Asegúrese de que la función de mapeo proporcionada maneje correctamente los tipos de los parámetros y los resultados.
    /// </remarks>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, TEntity>( string procedure, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento almacenado a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en objetos de dominio.
    /// Asegúrese de que la función de mapeo proporcionada coincida con la estructura de los resultados devueltos por el procedimiento almacenado.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureQuery{T1, T2, T3, T4, T5, TEntity}"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para ejecutar la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es true.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y mapearlos a un tipo de entidad específico.
    /// Asegúrese de que el procedimiento almacenado y la función de mapeo estén correctamente definidos para evitar errores en la ejecución.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureQuery{TEntity}(string, Func{TEntity})"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, T6, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si deben ser leídos de forma diferida (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y mapearlos a una lista de entidades.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de los parámetros coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureQuery{T1, T2, T3, T4, T5, T6, T7, TEntity}"/>
    List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, T6, T7, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de manera asíncrona.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se desea ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene una lista dinámica con los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método permite la ejecución de procedimientos almacenados en la base de datos y devuelve los resultados en forma de lista dinámica.
    /// Se recomienda manejar excepciones que puedan surgir durante la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<dynamic>> ExecuteProcedureQueryAsync( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de forma asíncrona y devuelve una lista de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados en una base de datos y obtener los resultados en forma de lista de entidades.
    /// Asegúrese de que el procedimiento almacenado devuelva un conjunto de resultados que pueda ser mapeado al tipo de entidad especificado.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<TEntity>( string procedure, int? timeout = null );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento almacenado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento almacenado a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. Si es verdadero, los resultados se almacenan en memoria; de lo contrario, se devuelven de forma secuencial.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en una lista de entidades.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de salida coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, TEntity>( string procedure, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento almacenado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento almacenado a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. Si es verdadero, los resultados se almacenan en memoria; de lo contrario, se devuelven de forma diferida.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en objetos de dominio.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de datos coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, TEntity>( string procedure, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de salida del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento almacenado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los parámetros de salida del procedimiento almacenado a una entidad.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución del procedimiento. Si es null, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es true.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de entidades generadas a partir de los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en objetos de entidad.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de datos coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="Task{T}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, TEntity>( string procedure, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta un procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad a la que se mapearán los resultados.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que define cómo mapear los resultados del procedimiento a una entidad.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. Si es verdadero, se almacenarán; de lo contrario, se devolverán de forma secuencial.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en una lista de entidades.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de los parámetros coincidan con los tipos esperados.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de salida del procedimiento.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad a la que se mapearán los resultados.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los parámetros de salida del procedimiento a una entidad.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples valores y convertirlos en una lista de entidades.
    /// Asegúrese de que el procedimiento almacenado esté correctamente definido y que los tipos de los parámetros coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="Task{T}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, T6, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true );
    /// <summary>
    /// Ejecuta una consulta a un procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento almacenado.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución del procedimiento. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es true.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos almacenados que devuelven múltiples resultados y transformarlos en objetos de dominio.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureQueryAsync{T1, T2, T3, T4, T5, T6, T7, TEntity}"/>
    Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, T6, T7, TEntity>( string procedure, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true );
}