namespace Util.Caching.EasyCaching;

/// <summary>
/// Clase que gestiona el almacenamiento en caché de datos.
/// Implementa la interfaz <see cref="ICache"/>.
/// </summary>
public class CacheManager : ICache
{

    #region campo

    private readonly IEasyCachingProviderBase _provider;
    private readonly IEasyCachingProvider _cachingProvider;

    #endregion

    #region Método constructor

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CacheManager"/>.
    /// </summary>
    /// <param name="provider">El proveedor de caché que se utilizará para las operaciones de caché.</param>
    /// <param name="hybridProvider">Un proveedor de caché híbrido opcional que se utilizará si se proporciona.</param>
    /// <remarks>
    /// Este constructor establece las opciones de caché a su estado inicial y verifica que el proveedor de caché no sea nulo.
    /// Si se proporciona un proveedor híbrido, este se utilizará como el proveedor de caché.
    /// </remarks>
    public CacheManager(IEasyCachingProvider provider, IHybridCachingProvider hybridProvider = null)
    {
        CachingOptions.Clear();
        if (provider != null)
        {
            _provider = provider;
            _cachingProvider = provider;
        }
        if (hybridProvider != null)
            _provider = hybridProvider;
        _provider.CheckNull(nameof(provider));
    }

    #endregion

    #region Exists

    /// <inheritdoc />
    /// <summary>
    /// Verifica si una clave de caché existe.
    /// </summary>
    /// <param name="key">La clave de caché que se va a validar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la clave de caché existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="key"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método primero valida la clave de caché proporcionada antes de verificar su existencia.
    /// </remarks>
    public bool Exists(CacheKey key)
    {
        key.Validate();
        return Exists(key.Key);
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si una clave existe en el proveedor.
    /// </summary>
    /// <param name="key">La clave que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la clave existe; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <seealso cref="SomeOtherClass"/>
    public bool Exists(string key)
    {
        return _provider.Exists(key);
    }

    #endregion

    #region ExistsAsync

    /// <inheritdoc />
    /// <summary>
    /// Verifica si una clave de caché existe de manera asíncrona.
    /// </summary>
    /// <param name="key">La clave de caché que se va a verificar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si la clave existe.</returns>
    /// <remarks>
    /// Este método valida la clave proporcionada antes de realizar la verificación.
    /// Si la clave no es válida, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="CacheKey.Validate"/>
    public async Task<bool> ExistsAsync(CacheKey key, CancellationToken cancellationToken = default)
    {
        key.Validate();
        return await ExistsAsync(key.Key, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si un elemento existe de manera asíncrona en el proveedor utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea verificar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un valor booleano que indica si el elemento existe (true) o no (false).
    /// </returns>
    /// <remarks>
    /// Este método llama a la implementación del proveedor para determinar la existencia del elemento.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _provider.ExistsAsync(key, cancellationToken);
    }

    #endregion

    #region Get

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener del caché.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para recuperar el valor.</param>
    /// <returns>El valor del caché asociado a la clave especificada, convertido al tipo T.</returns>
    /// <remarks>
    /// Este método valida la clave antes de intentar recuperar el valor del caché.
    /// Asegúrese de que la clave proporcionada sea válida y esté correctamente formateada.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la clave es nula.</exception>
    /// <seealso cref="CacheKey"/>
    public T Get<T>(CacheKey key)
    {
        key.Validate();
        return Get<T>(key.Key);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del proveedor asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea recuperar.</param>
    /// <returns>El valor del tipo especificado asociado a la clave.</returns>
    /// <remarks>
    /// Este método utiliza un proveedor para obtener el valor. Asegúrese de que la clave proporcionada
    /// exista en el proveedor, de lo contrario, el comportamiento puede ser indefinido.
    /// </remarks>
    /// <seealso cref="IProvider{T}"/>
    public T Get<T>(string key)
    {
        var result = _provider.Get<T>(key);
        return result.Value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos del caché utilizando las claves especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener del caché.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <returns>
    /// Una lista de elementos del tipo especificado que se han recuperado del caché.
    /// </returns>
    /// <remarks>
    /// Este método convierte las claves proporcionadas en un formato adecuado antes de realizar la operación de obtención.
    /// </remarks>
    /// <seealso cref="Get{T}(IEnumerable{CacheKey})"/>
    public List<T> Get<T>(IEnumerable<CacheKey> keys)
    {
        return Get<T>(ToKeys(keys));
    }

    /// <summary>
    /// Convierte una colección de objetos <see cref="CacheKey"/> en una colección de claves de tipo <see cref="string"/>.
    /// </summary>
    /// <param name="keys">Una colección de objetos <see cref="CacheKey"/> que se van a convertir.</param>
    /// <returns>
    /// Una colección de claves de tipo <see cref="string"/> extraídas de los objetos <see cref="CacheKey"/> proporcionados.
    /// </returns>
    /// <remarks>
    /// Este método verifica que la colección de claves no sea nula y valida cada objeto <see cref="CacheKey"/> 
    /// antes de extraer su clave.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="keys"/> es nulo.</exception>
    /// <seealso cref="CacheKey"/>
    private IEnumerable<string> ToKeys(IEnumerable<CacheKey> keys)
    {
        keys.CheckNull(nameof(keys));
        var cacheKeys = keys.ToList();
        cacheKeys.ForEach(t => t.Validate());
        return cacheKeys.Select(t => t.Key);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos del caché utilizando las claves proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves que se utilizarán para recuperar los elementos del caché.</param>
    /// <returns>
    /// Una lista de elementos del tipo especificado, obtenidos del caché.
    /// </returns>
    /// <remarks>
    /// Este método valida el estado del objeto antes de intentar recuperar los elementos del caché.
    /// Si las claves proporcionadas no se encuentran en el caché, la lista resultante puede estar vacía.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="_cachingProvider"/>
    public List<T> Get<T>(IEnumerable<string> keys)
    {
        Validate();
        var result = _cachingProvider.GetAll<T>(keys);
        return result.Values.Select(t => t.Value).ToList();
    }

    /// <summary>
    /// Valida si el proveedor de caché está configurado.
    /// </summary>
    /// <remarks>
    /// Este método lanza una excepción si el proveedor de caché es nulo,
    /// indicando que la operación no es soportada en el contexto actual.
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// Se lanza cuando el proveedor de caché es nulo.
    /// </exception>
    private void Validate()
    {
        if (_cachingProvider == null)
            throw new NotSupportedException("La caché de nivel 2 no admite esta operación.");
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché o lo genera si no está disponible.
    /// </summary>
    /// <typeparam name="T">El tipo del valor a obtener o generar.</typeparam>
    /// <param name="key">La clave del caché que se utilizará para almacenar o recuperar el valor.</param>
    /// <param name="action">Una función que genera el valor si no se encuentra en el caché.</param>
    /// <param name="options">Opciones adicionales para la gestión del caché. Puede ser null.</param>
    /// <returns>El valor del caché o el valor generado por la función <paramref name="action"/>.</returns>
    /// <remarks>
    /// Este método valida la clave proporcionada antes de intentar obtener el valor del caché.
    /// Si el valor no está presente en el caché, se invocará la función <paramref name="action"/> 
    /// para generar el valor y almacenarlo en el caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public T Get<T>(CacheKey key, Func<T> action, CacheOptions options = null)
    {
        key.Validate();
        return Get(key.Key, action, options);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor del caché asociado a la clave especificada. Si el valor no existe, se ejecuta la acción proporcionada para generar el valor.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a obtener del caché.</typeparam>
    /// <param name="key">La clave asociada al valor en el caché.</param>
    /// <param name="action">Una función que se ejecuta para generar el valor si no se encuentra en el caché.</param>
    /// <param name="options">Opciones adicionales para la configuración del caché. Puede ser null.</param>
    /// <returns>El valor asociado a la clave en el caché, o el valor generado por la acción si no existe.</returns>
    /// <remarks>
    /// Este método utiliza un proveedor de caché para obtener el valor. Si el valor no está en caché, se invoca la función <paramref name="action"/> 
    /// para calcular el valor y almacenarlo en el caché.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public T Get<T>(string key, Func<T> action, CacheOptions options = null)
    {
        var result = _provider.Get(key, action, GetExpiration(options));
        return result.Value;
    }

    /// <summary>
    /// Obtiene el tiempo de expiración de las opciones de caché.
    /// </summary>
    /// <param name="options">Las opciones de caché que contienen la configuración de expiración.</param>
    /// <returns>
    /// Un <see cref="TimeSpan"/> que representa el tiempo de expiración. 
    /// Si no se especifica ninguna expiración en las opciones, se devuelve un valor predeterminado de 8 horas.
    /// </returns>
    /// <remarks>
    /// Este método verifica si las opciones de caché son nulas y, en caso afirmativo, 
    /// asigna un tiempo de expiración predeterminado. 
    /// Luego, se asegura de que el valor devuelto sea seguro mediante el método <see cref="SafeValue"/>.
    /// </remarks>
    private TimeSpan GetExpiration(CacheOptions options)
    {
        var result = options?.Expiration;
        result ??= TimeSpan.FromHours(8);
        return result.SafeValue();
    }

    #endregion

    #region GetAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un objeto de forma asíncrona a partir de una clave y un tipo especificado.
    /// </summary>
    /// <param name="key">La clave del objeto que se desea obtener.</param>
    /// <param name="type">El tipo del objeto que se desea obtener.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto que representa el resultado de la operación asíncrona.
    /// </returns>
    /// <remarks>
    /// Este método llama a un proveedor para obtener el objeto asociado a la clave y el tipo especificados.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<object> GetAsync(string key, Type type, CancellationToken cancellationToken = default)
    {
        return await _provider.GetAsync(key, type, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un elemento de forma asíncrona desde la caché utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del elemento que se va a obtener.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para recuperar el elemento.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene el elemento de tipo <typeparamref name="T"/> obtenido de la caché.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="key"/> es nulo.</exception>
    /// <remarks>
    /// Este método valida la clave antes de intentar obtener el elemento de la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    public async Task<T> GetAsync<T>(CacheKey key, CancellationToken cancellationToken = default)
    {
        key.Validate();
        return await GetAsync<T>(key.Key, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de forma asíncrona asociado a la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea recuperar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, 
    /// conteniendo el valor asociado a la clave especificada.
    /// </returns>
    /// <remarks>
    /// Este método llama a un proveedor para obtener el valor correspondiente 
    /// a la clave dada. Si el valor no se encuentra, el resultado puede ser nulo 
    /// dependiendo de la implementación del proveedor.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var result = await _provider.GetAsync<T>(key, cancellationToken);
        return result.Value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> de la caché 
    /// utilizando las claves proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se desean obtener.</typeparam>
    /// <param name="keys">Una colección de claves de caché que se utilizarán para recuperar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede utilizarse para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, con una lista de elementos de tipo <typeparamref name="T"/> 
    /// obtenidos de la caché.
    /// </returns>
    /// <remarks>
    /// Este método llama a otro método <c>GetAsync</c> que realiza la operación real de obtención 
    /// de los elementos de la caché, transformando las claves antes de la llamada.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(IEnumerable{CacheKey}, CancellationToken)"/>
    public async Task<List<T>> GetAsync<T>(IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default)
    {
        return await GetAsync<T>(ToKeys(keys), cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> a partir de las claves proporcionadas.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos a recuperar.</typeparam>
    /// <param name="keys">Una colección de cadenas que representan las claves de los elementos a obtener.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una lista de elementos de tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método valida la operación antes de intentar recuperar los elementos del proveedor de caché.
    /// </remarks>
    /// <seealso cref="Validate"/>
    public async Task<List<T>> GetAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        Validate();
        var result = await _cachingProvider.GetAllAsync<T>(keys, cancellationToken);
        return result.Values.Select(t => t.Value).ToList();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de la caché de manera asíncrona, o ejecuta una acción para obtener el valor si no está en caché.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se está obteniendo.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar y recuperar el valor.</param>
    /// <param name="action">La acción asíncrona que se ejecutará para obtener el valor si no está en caché.</param>
    /// <param name="options">Opciones adicionales para la caché. Puede ser <c>null</c> si no se requieren opciones específicas.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="T"/> que representa el valor obtenido de la caché o el resultado de la acción ejecutada.
    /// </returns>
    /// <remarks>
    /// Este método valida la clave de caché antes de intentar obtener el valor. Si el valor no se encuentra en la caché,
    /// se ejecutará la acción proporcionada para obtener el valor y se almacenará en la caché para futuras solicitudes.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="key"/> o <paramref name="action"/> son <c>null</c>.</exception>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        key.Validate();
        return await GetAsync(key.Key, action, options, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor de la caché de manera asíncrona. Si el valor no está en caché, se ejecuta la acción proporcionada para obtenerlo.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave asociada al valor en la caché.</param>
    /// <param name="action">Una función asíncrona que se ejecutará para obtener el valor si no está en caché.</param>
    /// <param name="options">Opciones adicionales para la caché, como la duración de la expiración. Si es null, se usarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Un <see cref="Task{T}"/> que representa la operación asíncrona, con el valor obtenido de la caché o el resultado de la acción si no estaba en caché.
    /// </returns>
    /// <remarks>
    /// Este método es útil para implementar un patrón de caché donde se desea minimizar el acceso a recursos costosos.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public async Task<T> GetAsync<T>(string key, Func<Task<T>> action, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        var result = await _provider.GetAsync(key, action, GetExpiration(options), cancellationToken);
        return result.Value;
    }

    #endregion

    #region GetByPrefix

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos del tipo especificado que coinciden con el prefijo dado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a recuperar.</typeparam>
    /// <param name="prefix">El prefijo que se utilizará para filtrar los elementos.</param>
    /// <returns>
    /// Una lista de elementos del tipo <typeparamref name="T"/> que coinciden con el prefijo especificado.
    /// Si el prefijo está vacío, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método valida el estado antes de realizar la búsqueda y utiliza un proveedor de caché para obtener los elementos.
    /// Solo se incluyen en el resultado aquellos elementos que tienen un valor asociado.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="_cachingProvider"/>
    public List<T> GetByPrefix<T>(string prefix)
    {
        if (prefix.IsEmpty())
            return new List<T>();
        Validate();
        return _cachingProvider.GetByPrefix<T>(prefix).Where(t => t.Value.HasValue).Select(t => t.Value.Value).ToList();
    }

    #endregion

    #region GetByPrefixAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de elementos de tipo <typeparamref name="T"/> que coinciden con el prefijo especificado de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están recuperando.</typeparam>
    /// <param name="prefix">El prefijo que se utilizará para filtrar los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado contiene una lista de elementos de tipo <typeparamref name="T"/> 
    /// que tienen un valor asociado y coinciden con el prefijo especificado.
    /// </returns>
    /// <remarks>
    /// Si el prefijo está vacío, se devolverá una lista vacía.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="_cachingProvider"/>
    public async Task<List<T>> GetByPrefixAsync<T>(string prefix, CancellationToken cancellationToken = default)
    {
        if (prefix.IsEmpty())
            return new List<T>();
        Validate();
        var result = await _cachingProvider.GetByPrefixAsync<T>(prefix, cancellationToken);
        return result.Where(t => t.Value.HasValue).Select(t => t.Value.Value).ToList();
    }

    #endregion

    #region TrySet

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <returns>
    /// <c>true</c> si el valor se estableció correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método valida la clave antes de intentar establecer el valor en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public bool TrySet<T>(CacheKey key, T value, CacheOptions options = null)
    {
        key.Validate();
        return TrySet(key.Key, value, options);
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché asociado a una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el valor se estableció correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un proveedor de caché para intentar almacenar el valor.
    /// Si el valor ya existe en la caché, este método puede sobrescribirlo dependiendo de la implementación del proveedor.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public bool TrySet<T>(string key, T value, CacheOptions options = null)
    {
        return _provider.TrySet(key, value, GetExpiration(options));
    }

    #endregion

    #region TrySetAsync

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de la caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// <c>true</c> si el valor se estableció correctamente en la caché; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método valida la clave antes de intentar establecer el valor en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public async Task<bool> TrySetAsync<T>(CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        key.Validate();
        return await TrySetAsync(key.Key, value, options, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta establecer un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea almacenar en la caché.</typeparam>
    /// <param name="key">La clave asociada al valor que se desea almacenar.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Devuelve un valor booleano que indica si el valor se estableció correctamente en la caché.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un proveedor de caché para intentar almacenar el valor asociado a la clave especificada.
    /// Si la operación se cancela, se lanzará una excepción de operación cancelada.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public async Task<bool> TrySetAsync<T>(string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        return await _provider.TrySetAsync(key, value, GetExpiration(options), cancellationToken);
    }

    #endregion

    #region Set

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si no se especifica, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método valida la clave antes de intentar almacenar el valor en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>(CacheKey key, T value, CacheOptions options = null)
    {
        key.Validate();
        Set(key.Key, value, options);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché asociado a una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave bajo la cual se almacenará el valor en la caché.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser nulo.</param>
    /// <remarks>
    /// Este método utiliza un proveedor de caché para almacenar el valor y puede aplicar opciones de expiración
    /// según se especifique en el parámetro <paramref name="options"/>.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>(string key, T value, CacheOptions options = null)
    {
        _provider.Set(key, value, GetExpiration(options));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están almacenando en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene las claves y los elementos a almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método convierte el diccionario de elementos en un formato adecuado antes de almacenarlos en la caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>(IDictionary<CacheKey, T> items, CacheOptions options = null)
    {
        Set(ToItems(items), options);
    }

    /// <summary>
    /// Convierte un diccionario de tipo <see cref="IDictionary{CacheKey, T}"/> a un diccionario de tipo <see cref="IDictionary{string, T}"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de los valores en el diccionario.</typeparam>
    /// <param name="items">El diccionario de entrada que contiene claves de tipo <see cref="CacheKey"/> y valores de tipo <typeparamref name="T"/>.</param>
    /// <returns>Un nuevo diccionario que contiene las claves como cadenas y los mismos valores del diccionario de entrada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="items"/> es nulo.</exception>
    /// <remarks>
    /// Este método valida cada clave del diccionario de entrada antes de realizar la conversión.
    /// Asegúrese de que todas las claves sean válidas para evitar excepciones durante la conversión.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    private IDictionary<string, T> ToItems<T>(IDictionary<CacheKey, T> items)
    {
        items.CheckNull(nameof(items));
        return items.Select(item =>
        {
            item.Key.Validate();
            return new KeyValuePair<string, T>(item.Key.Key, item.Value);
        }).ToDictionary(t => t.Key, t => t.Value);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se van a almacenar en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es nulo, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método utiliza el proveedor de caché para almacenar todos los elementos del diccionario.
    /// La expiración de los elementos se determina a partir de las opciones proporcionadas.
    /// </remarks>
    /// <seealso cref="CacheOptions"/>
    public void Set<T>(IDictionary<string, T> items, CacheOptions options = null)
    {
        _provider.SetAll(items, GetExpiration(options));
    }

    #endregion

    #region SetAsync

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave de la caché que se utilizará para almacenar el valor.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser <c>null</c>.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método valida la clave antes de intentar almacenar el valor en la caché.
    /// Si la clave no es válida, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    /// <seealso cref="CacheOptions"/>
    public async Task SetAsync<T>(CacheKey key, T value, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        key.Validate();
        await SetAsync(key.Key, value, options, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un valor en la caché de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a almacenar en la caché.</typeparam>
    /// <param name="key">La clave bajo la cual se almacenará el valor en la caché.</param>
    /// <param name="value">El valor que se desea almacenar en la caché.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Puede ser null.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea no contiene un valor.
    /// </returns>
    /// <remarks>
    /// Este método permite almacenar un valor en la caché de manera asíncrona, lo que es útil para no bloquear el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="GetAsync{T}(string, CacheOptions, CancellationToken)"/>
    public async Task SetAsync<T>(string key, T value, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        await _provider.SetAsync(key, value, GetExpiration(options), cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece de manera asíncrona un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene las claves y los elementos a almacenar.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método convierte el diccionario de elementos a un formato adecuado antes de almacenarlos.
    /// </remarks>
    /// <seealso cref="SetAsync{T}(IEnumerable{CacheItem}, CacheOptions, CancellationToken)"/>
    public async Task SetAsync<T>(IDictionary<CacheKey, T> items, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        await SetAsync(ToItems(items), options, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece de manera asíncrona un conjunto de elementos en la caché.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se almacenarán en la caché.</typeparam>
    /// <param name="items">Un diccionario que contiene los elementos a almacenar, donde la clave es una cadena y el valor es del tipo especificado.</param>
    /// <param name="options">Opciones adicionales para la configuración de la caché. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de establecer los elementos en la caché.
    /// </returns>
    /// <remarks>
    /// Este método permite almacenar múltiples elementos en la caché de manera eficiente.
    /// Asegúrese de que el diccionario no esté vacío antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="GetExpiration(CacheOptions)"/>
    public async Task SetAsync<T>(IDictionary<string, T> items, CacheOptions options = null, CancellationToken cancellationToken = default)
    {
        await _provider.SetAllAsync(items, GetExpiration(options), cancellationToken);
    }

    #endregion

    #region Remove

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento del caché utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar del caché.</param>
    /// <exception cref="ArgumentNullException">Se lanza si la clave es nula.</exception>
    /// <remarks>
    /// Este método valida la clave antes de proceder a eliminar el elemento correspondiente del caché.
    /// </remarks>
    /// <seealso cref="CacheKey"/>
    public void Remove(CacheKey key)
    {
        key.Validate();
        Remove(key.Key);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento del proveedor utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <remarks>
    /// Este método invoca el método <see cref="_provider.Remove(string)"/> 
    /// del proveedor subyacente para realizar la eliminación.
    /// </remarks>
    /// <seealso cref="_provider"/>
    public void Remove(string key)
    {
        _provider.Remove(key);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un conjunto de claves de la caché.
    /// </summary>
    /// <param name="keys">Una colección de claves que se utilizarán para eliminar entradas de la caché.</param>
    /// <remarks>
    /// Este método convierte la colección de claves proporcionadas en un formato adecuado 
    /// y luego llama al método <see cref="Remove(IEnumerable{CacheKey})"/> para realizar la eliminación.
    /// </remarks>
    public void Remove(IEnumerable<CacheKey> keys)
    {
        Remove(ToKeys(keys));
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un conjunto de claves del proveedor.
    /// </summary>
    /// <param name="keys">Una colección de claves que se eliminarán.</param>
    /// <remarks>
    /// Este método llama al método <see cref="_provider.RemoveAll"/> para realizar la eliminación.
    /// Asegúrese de que las claves proporcionadas existan en el proveedor antes de llamar a este método.
    /// </remarks>
    public void Remove(IEnumerable<string> keys)
    {
        _provider.RemoveAll(keys);
    }

    #endregion

    #region RemoveAsync

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento del caché de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del caché que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación del caché.</returns>
    /// <remarks>
    /// Este método valida la clave antes de proceder a eliminar el elemento del caché.
    /// Si la clave no es válida, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="CacheKey.Validate"/>
    /// <seealso cref="RemoveAsync(string, CancellationToken)"/>
    public async Task RemoveAsync(CacheKey key, CancellationToken cancellationToken = default)
    {
        key.Validate();
        await RemoveAsync(key.Key, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método llama a un proveedor subyacente para realizar la eliminación.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la operación.
    /// </remarks>
    /// <seealso cref="System.Threading.Tasks.Task"/>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _provider.RemoveAsync(key, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona un conjunto de claves de caché.
    /// </summary>
    /// <param name="keys">Una colección de claves de caché que se desean eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación. 
    /// El resultado de la tarea no contiene valor, ya que es un método de tipo void.
    /// </returns>
    /// <remarks>
    /// Este método llama a otra sobrecarga de <c>RemoveAsync</c> que realiza la eliminación real de las claves convertidas.
    /// </remarks>
    /// <seealso cref="RemoveAsync(IEnumerable{string}, CancellationToken)"/>
    public async Task RemoveAsync(IEnumerable<CacheKey> keys, CancellationToken cancellationToken = default)
    {
        await RemoveAsync(ToKeys(keys), cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona un conjunto de elementos identificados por sus claves.
    /// </summary>
    /// <param name="keys">Una colección de claves que identifican los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede usarse para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método llama a <see cref="_provider.RemoveAllAsync"/> para realizar la eliminación.
    /// Asegúrese de que las claves proporcionadas sean válidas y existan en el proveedor.
    /// </remarks>
    /// <seealso cref="_provider"/>
    public async Task RemoveAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        await _provider.RemoveAllAsync(keys, cancellationToken);
    }

    #endregion

    #region RemoveByPrefix

    /// <summary>
    /// Elimina elementos que comienzan con el prefijo especificado.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Si el prefijo está vacío, no se realizará ninguna acción.
    /// </remarks>
    public void RemoveByPrefix(string prefix)
    {
        if (prefix.IsEmpty())
            return;
        _provider.RemoveByPrefix(prefix);
    }

    #endregion

    #region RemoveByPrefixAsync

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona los elementos que tienen un prefijo específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se utilizará para identificar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Si el prefijo está vacío, la operación no se llevará a cabo.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    /// <seealso cref="CancellationToken"/>
    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        if (prefix.IsEmpty())
            return;
        await _provider.RemoveByPrefixAsync(prefix, cancellationToken);
    }

    #endregion

    #region RemoveByPattern

    /// <inheritdoc />
    /// <summary>
    /// Elimina elementos que coinciden con el patrón especificado.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para identificar los elementos a eliminar.</param>
    /// <remarks>
    /// Si el patrón está vacío, no se realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="RemoveByPattern(string)"/>
    public void RemoveByPattern(string pattern)
    {
        if (pattern.IsEmpty())
            return;
        _provider.RemoveByPattern(pattern);
    }

    #endregion

    #region RemoveByPatternAsync

    /// <inheritdoc />
    /// <summary>
    /// Elimina de forma asíncrona los elementos que coinciden con el patrón especificado.
    /// </summary>
    /// <param name="pattern">El patrón que se utilizará para filtrar los elementos a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Si el patrón está vacío, la operación no realizará ninguna acción.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        if (pattern.IsEmpty())
            return;
        await _provider.RemoveByPatternAsync(pattern, cancellationToken);
    }

    #endregion

    #region Clear

    /// <inheritdoc />
    /// <summary>
    /// Limpia el caché utilizando el proveedor de caché.
    /// </summary>
    /// <remarks>
    /// Este método valida el estado actual antes de proceder a limpiar el caché.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la validación falla.
    /// </exception>
    public void Clear()
    {
        Validate();
        _cachingProvider.Flush();
    }

    #endregion

    #region ClearAsync

    /// <inheritdoc />
    /// <summary>
    /// Limpia el caché de manera asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método valida el estado actual antes de proceder a limpiar el caché utilizando el proveedor de caché.
    /// </remarks>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la validación falla.
    /// </exception>
    /// <seealso cref="Validate"/>
    /// <seealso cref="_cachingProvider.FlushAsync"/>
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        Validate();
        await _cachingProvider.FlushAsync(cancellationToken);
    }

    #endregion
}