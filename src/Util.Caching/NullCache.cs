namespace Util.Caching; 

/// <summary>
/// Representa una implementación de caché que no almacena ningún dato.
/// Esta clase es útil en situaciones donde se desea deshabilitar el almacenamiento en caché.
/// </summary>
public class NullCache : ILocalCache {
    public static readonly ILocalCache Instance = new NullCache();

    /// <inheritdoc />
    /// <summary>
    /// Verifica si la clave de caché especificada existe.
    /// </summary>
    /// <param name="key">La clave de caché que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la clave de caché existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <seealso cref="CacheKey"/>
    public bool Exists( CacheKey key ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si existe un elemento asociado a la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el elemento existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c> en su implementación actual.
    /// </remarks>
    public bool Exists( string key ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si una clave de caché existe de manera asíncrona.
    /// </summary>
    /// <param name="key">La clave de caché que se desea verificar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor booleano que indica si la clave de caché existe o no.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c> en la implementación actual.
    /// </remarks>
    public Task<bool> ExistsAsync( CacheKey key, CancellationToken cancellationToken = default ) {
        return Task.FromResult( false );
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si un elemento existe de manera asíncrona utilizando la clave proporcionada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea verificar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene un valor booleano que indica si el elemento existe o no.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve false, indicando que el elemento no existe.
    /// </remarks>
    public Task<bool> ExistsAsync( string key, CancellationToken cancellationToken = default ) {
        return Task.FromResult( false );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener del caché.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para recuperar el valor.</param>
    /// <returns>
    /// El valor del caché asociado a la clave especificada, o el valor predeterminado de <typeparamref name="T"/> si no se encuentra el valor.
    /// </returns>
    /// <remarks>
    /// Este método devuelve el valor predeterminado del tipo especificado si la clave no existe en el caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    public T Get<T>( CacheKey key ) {
        return default;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del almacenamiento asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea recuperar.</param>
    /// <returns>
    /// El valor asociado a la clave especificada, o el valor predeterminado del tipo <typeparamref name="T"/> si la clave no existe.
    /// </returns>
    /// <remarks>
    /// Este método devuelve el valor predeterminado del tipo <typeparamref name="T"/> si la clave no se encuentra en el almacenamiento.
    /// </remarks>
    public T Get<T>( string key ) {
        return default;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos del caché utilizando las claves proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <returns>Una lista de elementos del tipo especificado <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Este método devuelve una lista vacía si no se encuentran elementos asociados a las claves proporcionadas.
    /// </remarks>
    public List<T> Get<T>( IEnumerable<CacheKey> keys ) {
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos del tipo especificado a partir de una colección de claves.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves que se utilizarán para recuperar los elementos.</param>
    /// <returns>
    /// Una lista de elementos del tipo <typeparamref name="T"/> que corresponden a las claves proporcionadas.
    /// </returns>
    /// <remarks>
    /// Este método actualmente devuelve una lista vacía. La implementación real debe incluir la lógica
    /// para recuperar los elementos basados en las claves proporcionadas.
    /// </remarks>
    public List<T> Get<T>( IEnumerable<string> keys ) {
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché o lo genera mediante la función proporcionada si no está presente.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener o generar.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para almacenar o recuperar el valor.</param>
    /// <param name="action">La función que se ejecutará para generar el valor si no se encuentra en el caché.</param>
    /// <param name="options">Opciones adicionales para la configuración del caché. Puede ser nulo.</param>
    /// <returns>
    /// El valor obtenido del caché o el valor generado por la función <paramref name="action"/> si no se encuentra en el caché.
    /// </returns>
    /// <remarks>
    /// Si <paramref name="action"/> es nulo, se devolverá el valor predeterminado del tipo <typeparamref name="T"/>.
    /// </remarks>
    public T Get<T>( CacheKey key, Func<T> action, CacheOptions options = null ) {
        return action == null ? default : action();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché o lo genera utilizando la función proporcionada si no está presente.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener o generar.</typeparam>
    /// <param name="key">La clave asociada al valor en el caché.</param>
    /// <param name="action">Una función que genera el valor si no se encuentra en el caché.</param>
    /// <param name="options">Opciones adicionales para el manejo del caché. Puede ser nulo.</param>
    /// <returns>
    /// El valor obtenido del caché o el resultado de la función <paramref name="action"/> si no se encuentra en el caché.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la función <paramref name="action"/> es nula. Si es nula, devuelve el valor predeterminado para el tipo <typeparamref name="T"/>.
    /// De lo contrario, ejecuta la función para obtener el valor.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public T Get<T>( string key, Func<T> action, CacheOptions options = null ) {
        return action == null ? default : action();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un objeto de forma asíncrona a partir de una clave especificada.
    /// </summary>
    /// <param name="key">La clave utilizada para recuperar el objeto.</param>
    /// <param name="type">El tipo del objeto que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un objeto del tipo especificado como resultado.
    /// </returns>
    /// <remarks>
    /// Este método devuelve <c>null</c> de forma predeterminada. 
    /// Se espera que la implementación realice la lógica necesaria para recuperar el objeto correspondiente a la clave.
    /// </remarks>
    /// <seealso cref="Task"/>
    public Task<object> GetAsync( string key, Type type, CancellationToken cancellationToken = default ) {
        return null;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de caché de manera asíncrona utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener de la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para recuperar el valor.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, 
    /// que contiene el valor de tipo <typeparamref name="T"/> recuperado de la caché.
    /// </returns>
    /// <remarks>
    /// Este método devuelve el valor predeterminado de <typeparamref name="T"/> si no se encuentra un valor en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    public async Task<T> GetAsync<T>( CacheKey key, CancellationToken cancellationToken = default ) {
        await Task.CompletedTask;
        return default;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de forma asíncrona asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene el valor asociado a la clave especificada.
    /// </returns>
    /// <remarks>
    /// Este método devuelve el valor predeterminado del tipo especificado si no se encuentra un valor asociado a la clave.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<T> GetAsync<T>( string key, CancellationToken cancellationToken = default ) {
        await Task.CompletedTask;
        return default;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> a partir de las claves de caché proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con una lista de elementos de tipo <typeparamref name="T"/> como resultado.
    /// </returns>
    /// <remarks>
    /// Este método es asíncrono y puede ser utilizado para realizar operaciones de recuperación de datos sin bloquear el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<List<T>> GetAsync<T>( IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default ) {
        await Task.CompletedTask;
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> de manera asíncrona utilizando las claves proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="keys">Una colección de claves que se utilizarán para recuperar los elementos.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de elementos de tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método no realiza ninguna operación real en este momento y siempre devuelve una lista vacía.
    /// </remarks>
    /// <seealso cref="IEnumerable{T}"/>
    public async Task<List<T>> GetAsync<T>( IEnumerable<string> keys, CancellationToken cancellationToken = default ) {
        await Task.CompletedTask;
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de manera asíncrona, ejecutando una acción si es necesario.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener.</typeparam>
    /// <param name="key">La clave de caché asociada al valor.</param>
    /// <param name="action">La acción asíncrona que se ejecutará para obtener el valor si no está en caché.</param>
    /// <param name="options">Opciones adicionales para la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// El valor obtenido de manera asíncrona, o el valor predeterminado de <typeparamref name="T"/> si <paramref name="action"/> es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método permite obtener un valor de caché o calcularlo mediante una acción proporcionada.
    /// Si la acción es <c>null</c>, se devolverá el valor predeterminado del tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public async Task<T> GetAsync<T>( CacheKey key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return action == null ? default : await action();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de forma asíncrona utilizando una clave y una acción proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener.</typeparam>
    /// <param name="key">La clave asociada al valor en caché.</param>
    /// <param name="action">La acción asíncrona que se ejecutará para obtener el valor si no se encuentra en caché.</param>
    /// <param name="options">Opciones de caché que pueden influir en el comportamiento de la obtención del valor.</param>
    /// <param name="cancellationToken">Token de cancelación para permitir la cancelación de la operación asíncrona.</param>
    /// <returns>
    /// El valor obtenido de forma asíncrona, o el valor predeterminado si la acción es nula.
    /// </returns>
    /// <remarks>
    /// Este método permite obtener un valor de manera eficiente, utilizando una acción que se ejecuta solo si es necesario.
    /// Si la acción es nula, se devolverá el valor predeterminado del tipo especificado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la clave es nula o vacía.</exception>
    /// <seealso cref="CacheOptions"/>
    public async Task<T> GetAsync<T>( string key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return action == null ? default : await action();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> que comienzan con el prefijo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos a recuperar.</typeparam>
    /// <param name="prefix">El prefijo que deben tener los elementos para ser incluidos en la lista.</param>
    /// <returns>Una lista de elementos de tipo <typeparamref name="T"/> que cumplen con el criterio del prefijo.</returns>
    /// <remarks>
    /// Este método devuelve una lista vacía si no se encuentran elementos que coincidan con el prefijo especificado.
    /// </remarks>
    public List<T> GetByPrefix<T>( string prefix ) {
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> que coinciden con el prefijo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="prefix">El prefijo que se utilizará para filtrar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asincrónica.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, con una lista de elementos de tipo <typeparamref name="T"/> que coinciden con el prefijo.
    /// </returns>
    /// <remarks>
    /// Este método es asincrónico y puede ser utilizado para realizar operaciones de búsqueda que no bloqueen el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<List<T>> GetByPrefixAsync<T>( string prefix, CancellationToken cancellationToken = default ) {
        await Task.CompletedTask;
        return new List<T>();
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el valor se estableció correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método permite intentar almacenar un valor en la caché de manera segura,
    /// devolviendo un indicador de éxito o fracaso. Si el valor no se puede establecer,
    /// se puede manejar el error según sea necesario.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public bool TrySet<T>( CacheKey key, T value, CacheOptions options = null ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave bajo la cual se almacenará el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el valor se estableció correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera segura, 
    /// verificando si la operación se puede realizar antes de intentar establecer el valor.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public bool TrySet<T>( string key, T value, CacheOptions options = null ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor booleano que indica si la operación fue exitosa.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve false, lo que indica que no se ha podido establecer el valor en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public Task<bool> TrySetAsync<T>( CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.FromResult( false );
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor booleano que indica si el valor se estableció con éxito.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve false, lo que indica que no se pudo establecer el valor en la caché.
    /// </remarks>
    public Task<bool> TrySetAsync<T>( string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.FromResult( false );
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché, lo que puede mejorar el rendimiento al evitar cálculos repetitivos o accesos a datos costosos.
    /// </remarks>
    /// <seealso cref="Get{T}(CacheKey)"/>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>( CacheKey key, T value, CacheOptions options = null ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se va a almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché, lo que puede mejorar el rendimiento
    /// al evitar la necesidad de realizar operaciones costosas repetidamente.
    /// </remarks>
    /// <seealso cref="Get{T}(string)"/>
    /// <seealso cref="Remove(string)"/>
    public void Set<T>( string key, T value, CacheOptions options = null ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es de tipo <see cref="CacheKey"/>.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de manera eficiente.
    /// Asegúrese de que las claves en el diccionario sean únicas para evitar sobrescribir elementos existentes.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>( IDictionary<CacheKey, T> items, CacheOptions options = null ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se proporciona, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de manera eficiente.
    /// Asegúrese de que las claves en el diccionario sean únicas para evitar sobrescribir elementos existentes.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>( IDictionary<string, T> items, CacheOptions options = null ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se va a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de establecer el valor en la caché.</returns>
    /// <remarks>
    /// Este método no realiza ninguna operación real en la caché y simplemente completa la tarea.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(CacheKey, CacheOptions, CancellationToken)"/>
    public Task SetAsync<T>( CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se va a almacenar.</param>
    /// <param name="value">El valor que se va a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de establecimiento del valor en la caché.</returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera asíncrona, 
    /// lo que permite que la aplicación continúe ejecutándose mientras se realiza la operación.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(string, CacheOptions, CancellationToken)"/>
    public Task SetAsync<T>( string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece de manera asíncrona un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene las claves y los elementos a almacenar.</param>
    /// <param name="options">Opciones de configuración para el almacenamiento en caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de establecer los elementos en la caché.</returns>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de forma eficiente.
    /// Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/> para permitir la cancelación de la operación.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(IDictionary{CacheKey, T}, CacheOptions, CancellationToken)"/>
    public Task SetAsync<T>( IDictionary<CacheKey, T> items, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un conjunto de elementos en caché de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar en caché, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de establecer los elementos en caché.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en la implementación actual y siempre completa la tarea de forma exitosa.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public Task SetAsync<T>( IDictionary<string, T> items, CacheOptions options = null, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento del caché utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar del caché.</param>
    /// <remarks>
    /// Este método busca el elemento en el caché que corresponde a la clave proporcionada
    /// y lo elimina si se encuentra. Si la clave no existe, no se realiza ninguna acción.
    /// </remarks>
    public void Remove( CacheKey key ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento de la colección utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <remarks>
    /// Este método busca el elemento correspondiente a la clave proporcionada y lo elimina de la colección.
    /// Si la clave no se encuentra, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="Add(string, T)"/>
    /// <seealso cref="ContainsKey(string)"/>
    public void Remove( string key ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un conjunto de claves de caché especificadas.
    /// </summary>
    /// <param name="keys">Una colección de claves de caché que se eliminarán.</param>
    /// <remarks>
    /// Este método permite eliminar múltiples claves de caché de manera eficiente.
    /// Asegúrese de que las claves proporcionadas existan en la caché antes de llamar a este método.
    /// </remarks>
    public void Remove( IEnumerable<CacheKey> keys ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un conjunto de elementos identificados por sus claves.
    /// </summary>
    /// <param name="keys">Una colección de cadenas que representan las claves de los elementos a eliminar.</param>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos de una colección o estructura de datos
    /// utilizando sus claves correspondientes. Es importante asegurarse de que las claves proporcionadas
    /// existen en la colección para evitar excepciones o comportamientos inesperados.
    /// </remarks>
    public void Remove( IEnumerable<string> keys ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina el elemento asociado a la clave especificada del caché de manera asíncrona.
    /// </summary>
    /// <param name="key">La clave del caché que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción y completa la tarea inmediatamente.
    /// </remarks>
    /// <seealso cref="AddAsync(CacheKey, object, CancellationToken)"/>
    /// <seealso cref="GetAsync(CacheKey, CancellationToken)"/>
    public Task RemoveAsync( CacheKey key, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en su implementación actual y completa la tarea inmediatamente.
    /// </remarks>
    public Task RemoveAsync( string key, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona un conjunto de claves de caché.
    /// </summary>
    /// <param name="keys">Una colección de claves de caché que se deben eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en la implementación actual y completa la tarea inmediatamente.
    /// </remarks>
    public Task RemoveAsync( IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de forma asíncrona un conjunto de elementos identificados por sus claves.
    /// </summary>
    /// <param name="keys">Una colección de claves que identifican los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que representa la operación de eliminación asíncrona.
    /// </returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en la implementación actual y completa la tarea de forma inmediata.
    /// </remarks>
    public Task RemoveAsync( IEnumerable<string> keys, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina los elementos que tienen un prefijo específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Este método busca en la colección de elementos y elimina aquellos que comienzan con el prefijo proporcionado.
    /// </remarks>
    /// <seealso cref="Add(string)"/>
    /// <seealso cref="Contains(string)"/>
    public void RemoveByPrefix( string prefix ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de forma asíncrona los elementos que tienen un prefijo específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en su implementación actual y completa la tarea inmediatamente.
    /// </remarks>
    /// <seealso cref="Task"/>
    public Task RemoveByPrefixAsync( string prefix, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina elementos que coinciden con el patrón especificado.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Este método busca y elimina todos los elementos que coinciden con el patrón dado.
    /// Asegúrese de que el patrón esté correctamente definido para evitar la eliminación no deseada.
    /// </remarks>
    public void RemoveByPattern( string pattern ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina elementos de forma asíncrona que coinciden con un patrón específico.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para identificar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción en la implementación actual y completa la tarea de inmediato.
    /// </remarks>
    /// <seealso cref="Task"/>
    public Task RemoveByPatternAsync( string pattern, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Limpia todos los elementos de la colección.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección, dejándola vacía.
    /// </remarks>
    /// <inheritdoc />
    public void Clear() {
    }

    /// <inheritdoc />
    /// <summary>
    /// Limpia de manera asíncrona los recursos o datos asociados.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de limpieza.
    /// </returns>
    /// <remarks>
    /// Este método no realiza ninguna acción y completa inmediatamente.
    /// </remarks>
    public Task ClearAsync( CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }
}