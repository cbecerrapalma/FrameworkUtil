namespace Util.Caching; 

/// <summary>
/// Define una interfaz para el manejo de caché.
/// </summary>
/// <remarks>
/// Esta interfaz permite implementar diferentes estrategias de almacenamiento en caché,
/// facilitando la gestión de datos temporales para mejorar el rendimiento de las aplicaciones.
/// </remarks>
public interface ICache {
    /// <summary>
    /// Determina si una clave de caché específica existe en el sistema.
    /// </summary>
    /// <param name="key">La clave de caché que se desea verificar.</param>
    /// <returns>Devuelve <c>true</c> si la clave de caché existe; de lo contrario, devuelve <c>false</c>.</returns>
    /// <remarks>
    /// Este método es útil para comprobar la existencia de datos en caché antes de intentar acceder a ellos,
    /// lo que puede ayudar a evitar excepciones o errores en tiempo de ejecución.
    /// </remarks>
    bool Exists( CacheKey key );
    /// <summary>
    /// Determina si una clave especificada existe.
    /// </summary>
    /// <param name="key">La clave que se va a buscar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la clave existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica la existencia de la clave en la colección o estructura de datos
    /// correspondiente. Es útil para evitar excepciones al intentar acceder a un elemento
    /// que podría no estar presente.
    /// </remarks>
    bool Exists( string key );
    /// <summary>
    /// Verifica si una clave de caché específica existe de manera asíncrona.
    /// </summary>
    /// <param name="key">La clave de caché que se desea verificar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si la clave de caché existe.</returns>
    /// <remarks>
    /// Este método permite comprobar la existencia de una clave en la caché sin bloquear el hilo actual.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    Task<bool> ExistsAsync( CacheKey key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Verifica si existe un elemento asociado a la clave especificada de manera asíncrona.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea verificar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor devuelto es <c>true</c> si el elemento existe; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite comprobar la existencia de un elemento sin bloquear el hilo de ejecución actual.
    /// Se recomienda utilizar el <paramref name="cancellationToken"/> para gestionar la cancelación de la operación si es necesario.
    /// </remarks>
    /// <seealso cref="GetAsync(string, CancellationToken)"/>
    Task<bool> ExistsAsync( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener del caché.</typeparam>
    /// <param name="key">La clave del caché que se utiliza para recuperar el valor.</param>
    /// <returns>El valor del tipo especificado almacenado en el caché, o el valor predeterminado de <typeparamref name="T"/> si no se encuentra ningún valor asociado a la clave.</returns>
    /// <remarks>
    /// Este método permite acceder a los datos almacenados en caché de manera eficiente,
    /// evitando la necesidad de realizar operaciones costosas para recuperar datos que ya han sido almacenados.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    T Get<T>( CacheKey key );
    /// <summary>
    /// Obtiene un valor del almacenamiento asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave del valor que se desea recuperar.</param>
    /// <returns>El valor asociado a la clave especificada, convertido al tipo T.</returns>
    /// <remarks>
    /// Este método lanzará una excepción si la clave no existe en el almacenamiento
    /// o si el valor asociado no se puede convertir al tipo T.
    /// </remarks>
    /// <seealso cref="Set{T}(string, T)"/>
    T Get<T>( string key );
    /// <summary>
    /// Recupera una lista de elementos del caché utilizando las claves especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se recuperarán del caché.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <returns>Una lista de elementos del tipo especificado que se han recuperado del caché.</returns>
    /// <remarks>
    /// Este método permite obtener múltiples elementos del caché en función de las claves proporcionadas.
    /// Si una clave no se encuentra en el caché, el elemento correspondiente en la lista resultante será <c>default(T)</c>.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    List<T> Get<T>( IEnumerable<CacheKey> keys );
    /// <summary>
    /// Obtiene una lista de elementos del tipo especificado a partir de una colección de claves.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves que se utilizarán para recuperar los elementos.</param>
    /// <returns>Una lista de elementos del tipo especificado.</returns>
    /// <remarks>
    /// Este método busca los elementos correspondientes a las claves proporcionadas en la colección
    /// y devuelve una lista con los resultados. Si una clave no se encuentra, se omitirá en la lista resultante.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<T> Get<T>( IEnumerable<string> keys );
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada. Si el valor no está en caché,
    /// se ejecuta la acción proporcionada para obtener el valor y se almacena en caché.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave del caché asociada al valor que se desea obtener.</param>
    /// <param name="action">La función que se ejecutará para obtener el valor si no está en caché.</param>
    /// <param name="options">Opciones adicionales para la gestión del caché. Puede ser nulo.</param>
    /// <returns>El valor obtenido del caché o el resultado de la acción si no estaba en caché.</returns>
    /// <remarks>
    /// Este método es útil para optimizar el rendimiento al evitar cálculos costosos o
    /// llamadas a recursos externos si los resultados ya están disponibles en caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    T Get<T>( CacheKey key, Func<T> action, CacheOptions options = null );
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada. Si el valor no se encuentra en el caché,
    /// se ejecuta la acción proporcionada para obtener el valor y se almacena en el caché.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor en el caché.</param>
    /// <param name="action">Una función que se ejecuta para obtener el valor si no se encuentra en el caché.</param>
    /// <param name="options">Opciones adicionales para la gestión del caché. Puede ser nulo.</param>
    /// <returns>El valor asociado a la clave en el caché, o el resultado de la acción si no estaba en el caché.</returns>
    /// <remarks>
    /// Este método es útil para implementar un patrón de caché que evita la ejecución de operaciones costosas
    /// al almacenar en caché los resultados de dichas operaciones.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    T Get<T>( string key, Func<T> action, CacheOptions options = null );
    /// <summary>
    /// Obtiene un objeto de forma asíncrona utilizando una clave y un tipo especificado.
    /// </summary>
    /// <param name="key">La clave que se utilizará para recuperar el objeto.</param>
    /// <param name="type">El tipo del objeto que se desea obtener.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un objeto del tipo especificado como resultado.</returns>
    /// <remarks>
    /// Este método permite la recuperación de datos de manera eficiente y no bloqueante.
    /// Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/> para permitir la cancelación de la operación.
    /// </remarks>
    /// <seealso cref="Task{T}"/>
    /// <seealso cref="CancellationToken"/>
    Task<object> GetAsync( string key, Type type, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un elemento de forma asíncrona del caché utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del elemento que se va a recuperar.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para recuperar el elemento.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el elemento recuperado del caché.</returns>
    /// <remarks>
    /// Este método permite recuperar datos de un caché de manera eficiente y no bloqueante.
    /// Si el elemento no se encuentra en el caché, se puede manejar la lógica de recuperación en el código que llama a este método.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    Task<T> GetAsync<T>( CacheKey key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un objeto asíncrono de tipo <typeparamref name="T"/> asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al objeto que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Este método permite recuperar datos de manera asíncrona, lo que puede ser útil para operaciones de entrada/salida que podrían bloquear el hilo actual.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<T> GetAsync<T>( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> de la caché 
    /// utilizando las claves especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos a recuperar.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de elementos de tipo <typeparamref name="T"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite la recuperación asíncrona de elementos desde una caché, 
    /// lo que puede mejorar el rendimiento al evitar operaciones de bloqueo 
    /// en el hilo de ejecución actual.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    Task<List<T>> GetAsync<T>( IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> de manera asíncrona 
    /// utilizando una colección de claves especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="keys">Una colección de claves que se utilizarán para recuperar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de elementos de tipo <typeparamref name="T"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite realizar una recuperación asíncrona de datos, lo que puede mejorar la 
    /// eficiencia y la capacidad de respuesta de la aplicación al evitar bloqueos en el hilo principal.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<List<T>> GetAsync<T>( IEnumerable<string> keys, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un valor de tipo <typeparamref name="T"/> de manera asíncrona, utilizando una clave de caché.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener.</typeparam>
    /// <param name="key">La clave de caché utilizada para almacenar y recuperar el valor.</param>
    /// <param name="action">Una función asíncrona que se ejecutará si el valor no se encuentra en la caché.</param>
    /// <param name="options">Opciones adicionales para la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el valor obtenido de tipo <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Este método primero intentará recuperar el valor asociado a la clave de caché especificada.
    /// Si el valor no se encuentra, se ejecutará la función <paramref name="action"/> para obtener el valor
    /// y se almacenará en la caché para futuras solicitudes.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    Task<T> GetAsync<T>( CacheKey key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un valor de forma asíncrona desde una fuente de datos, utilizando una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de dato que se espera obtener.</typeparam>
    /// <param name="key">La clave utilizada para identificar el valor en la caché.</param>
    /// <param name="action">Una función asíncrona que se ejecutará para obtener el valor si no se encuentra en la caché.</param>
    /// <param name="options">Opciones adicionales para la operación de caché. Puede ser nulo.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Este método primero intentará recuperar el valor asociado a la clave proporcionada desde la caché.
    /// Si el valor no está disponible, se ejecutará la función <paramref name="action"/> para obtener el valor.
    /// El resultado se almacenará en la caché para futuras solicitudes.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    Task<T> GetAsync<T>( string key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> que comienzan con el prefijo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la lista.</typeparam>
    /// <param name="prefix">El prefijo que deben tener los elementos para ser incluidos en la lista resultante.</param>
    /// <returns>Una lista de elementos de tipo <typeparamref name="T"/> que cumplen con la condición del prefijo.</returns>
    /// <remarks>
    /// Este método es útil para filtrar colecciones basadas en un criterio de coincidencia de prefijo.
    /// Asegúrese de que el tipo <typeparamref name="T"/> tenga una implementación adecuada para la comparación de cadenas,
    /// especialmente si se utiliza en un contexto donde el prefijo se compara con propiedades de cadena.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<T> GetByPrefix<T>( string prefix );
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> que coinciden con el prefijo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="prefix">El prefijo que se utilizará para filtrar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de elementos que coinciden con el prefijo.</returns>
    /// <remarks>
    /// Este método es asíncrono y puede ser utilizado para realizar operaciones de búsqueda en una base de datos o en una colección de datos.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<List<T>> GetByPrefixAsync<T>( string prefix, CancellationToken cancellationToken = default );
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo de valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <returns>Devuelve true si el valor se estableció correctamente; de lo contrario, false.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera segura, 
    /// verificando si la operación se realizó con éxito.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    bool TrySet<T>( CacheKey key, T value, CacheOptions options = null );
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <returns>Devuelve <c>true</c> si el valor se estableció correctamente; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera segura, 
    /// verificando si la operación se realizó con éxito. Si la clave ya existe 
    /// en la caché, el valor se actualizará con el nuevo valor proporcionado.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    bool TrySet<T>( string key, T value, CacheOptions options = null );
    /// <summary>
    /// Intenta establecer un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave del caché donde se almacenará el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelación para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea es verdadero si el valor se estableció correctamente; de lo contrario, falso.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de forma asíncrona, 
    /// manejando posibles conflictos y condiciones de carrera. 
    /// Si el valor ya existe en la caché y no se permite la sobrescritura, 
    /// el método devolverá falso.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    Task<bool> TrySetAsync<T>( CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Intenta establecer un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se va a almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es null, se aplicarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token de cancelación para permitir la cancelación de la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene true si el valor se estableció correctamente; de lo contrario, false.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de forma asíncrona, lo que puede ser útil para evitar bloqueos en la interfaz de usuario o en operaciones de I/O.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(string, CacheOptions, CancellationToken)"/>
    Task<bool> TrySetAsync<T>( string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Establece un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché, lo que puede mejorar el rendimiento de la aplicación al evitar cálculos o accesos repetidos a datos que no cambian con frecuencia.
    /// </remarks>
    /// <seealso cref="Get{T}(CacheKey)"/>
    /// <seealso cref="CacheOptions"/>
    void Set<T>( CacheKey key, T value, CacheOptions options = null );
    /// <summary>
    /// Establece un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave bajo la cual se almacenará el valor en la caché.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar cualquier tipo de objeto en la caché, facilitando la gestión de datos temporales
    /// que pueden ser recuperados rápidamente sin necesidad de acceder a la fuente de datos original.
    /// </remarks>
    /// <seealso cref="Get{T}(string)"/>
    /// <seealso cref="Remove(string)"/>
    void Set<T>( string key, T value, CacheOptions options = null );
    /// <summary>
    /// Establece un conjunto de elementos en el caché utilizando las claves y valores proporcionados.
    /// </summary>
    /// <typeparam name="T">El tipo de los valores que se almacenarán en el caché.</typeparam>
    /// <param name="items">Un diccionario que contiene las claves del caché y los elementos a almacenar.</param>
    /// <param name="options">Opciones adicionales para la configuración del caché. Si no se proporciona, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en el caché de manera eficiente.
    /// Asegúrese de que las claves sean únicas para evitar sobrescribir elementos existentes.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    void Set<T>( IDictionary<CacheKey,T> items, CacheOptions options = null );
    /// <summary>
    /// Establece un conjunto de elementos en el caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en el caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones de caché que pueden personalizar el comportamiento del almacenamiento. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite agregar o actualizar elementos en el caché utilizando un diccionario.
    /// Asegúrese de que las claves sean únicas para evitar sobrescribir elementos existentes.
    /// </remarks>
    /// <seealso cref="Get{T}(IDictionary{string, T})"/>
    void Set<T>( IDictionary<string, T> items, CacheOptions options = null );
    /// <summary>
    /// Establece un valor en la caché de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave bajo la cual se almacenará el valor en la caché.</param>
    /// <param name="value">El valor que se va a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera asíncrona, lo que es útil para evitar bloqueos en la aplicación mientras se realiza la operación de almacenamiento.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(CacheKey)"/>
    /// <seealso cref="RemoveAsync(CacheKey)"/>
    Task SetAsync<T>( CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Establece un valor en la caché de manera asíncrona utilizando una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave única asociada al valor que se va a almacenar.</param>
    /// <param name="value">El valor que se va a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser null.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de forma asíncrona, lo que es útil para 
    /// mejorar el rendimiento de las aplicaciones al evitar accesos repetidos a fuentes de datos 
    /// más lentas. Asegúrese de manejar adecuadamente el token de cancelación para evitar 
    /// operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(string, CacheOptions, CancellationToken)"/>
    Task SetAsync<T>( string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Establece de manera asíncrona un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene las claves y los elementos a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de forma eficiente.
    /// Asegúrese de que las claves en el diccionario sean únicas para evitar sobrescribir elementos existentes.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(IEnumerable{CacheKey}, CacheOptions, CancellationToken)"/>
    Task SetAsync<T>( IDictionary<CacheKey, T> items, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Establece un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones de caché que permiten configurar el comportamiento de la operación. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede utilizarse para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de manera asíncrona.
    /// Es útil para optimizar el rendimiento al reducir el número de accesos a fuentes de datos externas.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(IEnumerable{string}, CacheOptions, CancellationToken)"/>
    Task SetAsync<T>( IDictionary<string, T> items, CacheOptions options = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un elemento del caché utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar del caché.</param>
    /// <remarks>
    /// Este método busca el elemento en el caché y, si se encuentra, lo elimina. 
    /// Si la clave no existe, no se realizará ninguna acción.
    /// </remarks>
    void Remove(CacheKey key);
    /// <summary>
    /// Elimina el elemento asociado con la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <remarks>
    /// Si la clave no existe, no se realiza ninguna acción.
    /// </remarks>
    void Remove(string key);
    /// <summary>
    /// Elimina un conjunto de claves de caché especificadas.
    /// </summary>
    /// <param name="keys">Una colección de claves de caché que se deben eliminar.</param>
    /// <remarks>
    /// Este método permite eliminar múltiples entradas de caché al mismo tiempo, 
    /// lo que puede ser útil para optimizar el rendimiento y la gestión de la memoria.
    /// Asegúrese de que las claves proporcionadas existan en la caché antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="Add(IEnumerable{CacheKey})"/>
    /// <seealso cref="Clear()"/>
    void Remove( IEnumerable<CacheKey> keys );
    /// <summary>
    /// Elimina los elementos correspondientes a las claves especificadas de la colección.
    /// </summary>
    /// <param name="keys">Una colección de cadenas que representan las claves de los elementos a eliminar.</param>
    /// <remarks>
    /// Este método recorre la colección de claves proporcionada y elimina cada elemento que coincide 
    /// con las claves en la colección. Si una clave no existe en la colección, no se produce ningún error.
    /// </remarks>
    /// <example>
    /// <code>
    /// var keysToRemove = new List<string> { "clave1", "clave2" };
    /// Remove(keysToRemove);
    /// </code>
    /// </example>
    void Remove( IEnumerable<string> keys );
    /// <summary>
    /// Elimina un elemento de la caché de manera asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar de la caché.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar elementos de la caché sin bloquear el hilo de ejecución actual.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="AddAsync(CacheKey, object, CancellationToken)"/>
    /// <seealso cref="GetAsync(CacheKey, CancellationToken)"/>
    Task RemoveAsync( CacheKey key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método es útil para eliminar elementos de un almacenamiento en caché o de una base de datos.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la operación.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveAsync( string key, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina de forma asíncrona un conjunto de claves de caché especificadas.
    /// </summary>
    /// <param name="keys">Una colección de claves de caché que se desean eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples entradas de caché de manera eficiente. 
    /// Asegúrese de que las claves proporcionadas sean válidas y existan en la caché.
    /// </remarks>
    /// <seealso cref="AddAsync(IEnumerable{CacheKey}, CancellationToken)"/>
    /// <seealso cref="GetAsync(IEnumerable{CacheKey}, CancellationToken)"/>
    Task RemoveAsync( IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina de manera asíncrona un conjunto de elementos identificados por sus claves.
    /// </summary>
    /// <param name="keys">Una colección de claves que identifican los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos de forma eficiente. 
    /// Si se proporciona un <paramref name="cancellationToken"/>, se puede cancelar la operación en cualquier momento.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="keys"/> es null.</exception>
    /// <seealso cref="AddAsync(IEnumerable{string}, CancellationToken)"/>
    /// <seealso cref="UpdateAsync(IEnumerable{string}, CancellationToken)"/>
    Task RemoveAsync( IEnumerable<string> keys, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina todos los elementos que tienen un prefijo específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Este método busca en una colección y elimina todos los elementos cuyo nombre comienza con el prefijo proporcionado.
    /// Si no se encuentran elementos que coincidan con el prefijo, no se realiza ninguna acción.
    /// </remarks>
    /// <example>
    /// <code>
    /// RemoveByPrefix("ejemplo");
    /// </code>
    /// </example>
    void RemoveByPrefix( string prefix );
    /// <summary>
    /// Elimina de manera asíncrona los elementos que tienen un prefijo específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Token opcional que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos que comienzan con el prefijo proporcionado.
    /// Si no se encuentran elementos que coincidan con el prefijo, la operación se completará sin realizar cambios.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveByPrefixAsync( string prefix, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina elementos que coinciden con el patrón especificado.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Este método busca en una colección y elimina todos los elementos que coinciden con el patrón proporcionado.
    /// Asegúrese de que el patrón sea válido y esté en el formato correcto para evitar errores en la eliminación.
    /// </remarks>
    /// <seealso cref="AddElement(string)"/>
    /// <seealso cref="GetElements()"/>
    void RemoveByPattern( string pattern );
    /// <summary>
    /// Elimina de forma asíncrona los elementos que coinciden con el patrón especificado.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para filtrar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos que coinciden con el patrón dado.
    /// Asegúrese de que el patrón sea válido y que la operación se pueda realizar en el contexto actual.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveByPatternAsync( string pattern, CancellationToken cancellationToken = default );
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos o datos almacenados, dejando el estado inicial.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Asynchronously limpia los recursos o datos asociados.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de limpieza.
    /// </returns>
    /// <remarks>
    /// Este método permite realizar la limpieza de manera asíncrona, lo que puede ser útil
    /// en situaciones donde se requiere liberar recursos sin bloquear el hilo de ejecución.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si la operación es cancelada mediante el <paramref name="cancellationToken"/>.
    /// </exception>
    Task ClearAsync( CancellationToken cancellationToken = default );
}