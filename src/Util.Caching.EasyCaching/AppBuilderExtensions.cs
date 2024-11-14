namespace Util.Caching.EasyCaching;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions
{

    #region Campo

    public static int MaxRdSecond = 1200;

    #endregion

    #region AddMemoryCache

    /// <summary>
    /// Agrega un servicio de caché en memoria al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le añadirá el servicio de caché en memoria.</param>
    /// <returns>El constructor de la aplicación con el servicio de caché en memoria agregado.</returns>
    /// <remarks>
    /// Este método permite configurar opciones específicas para el caché en memoria,
    /// como el tiempo máximo de retención y si se deben almacenar valores nulos.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    public static IAppBuilder AddMemoryCache(this IAppBuilder builder)
    {
        return builder.AddMemoryCache(options =>
        {
            options.MaxRdSecond = MaxRdSecond;
            options.CacheNulls = true;
        });
    }

    /// <summary>
    /// Agrega un servicio de caché en memoria al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el servicio de caché.</param>
    /// <param name="configuration">La configuración que se utilizará para configurar el caché.</param>
    /// <param name="section">La sección de configuración que se utilizará para la configuración del caché. Por defecto es "EasyCaching:Memory".</param>
    /// <returns>El mismo <see cref="IAppBuilder"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método configura un servicio de caché en memoria utilizando EasyCaching y lo registra como un servicio singleton.
    /// </remarks>
    /// <seealso cref="ILocalCache"/>
    /// <seealso cref="MemoryCacheManager"/>
    public static IAppBuilder AddMemoryCache(this IAppBuilder builder, IConfiguration configuration, string section = "EasyCaching:Memory")
    {
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            ConfigCommonService(services);
            services.TryAddSingleton<ILocalCache, MemoryCacheManager>();
            services.AddEasyCaching(options => options.UseInMemory(configuration, CacheProviderKey.MemoryCache, section));
        });
        return builder;
    }

    /// <summary>
    /// Configura los servicios comunes para la inyección de dependencias.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán los servicios comunes.</param>
    private static void ConfigCommonService(IServiceCollection services)
    {
        services.TryAddSingleton<ICache, CacheManager>();
        services.TryAddSingleton<ICacheKeyGenerator, CacheKeyGenerator>();
        services.TryAddSingleton<IEasyCachingKeyGenerator, DefaultEasyCachingKeyGenerator>();
    }

    /// <summary>
    /// Agrega un servicio de caché en memoria al contenedor de servicios.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de la caché en memoria.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para permitir la configuración de la caché en memoria.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para utilizar una caché en memoria.
    /// Se asegura de que el servicio de caché local esté disponible y lo configura
    /// utilizando las opciones proporcionadas en <paramref name="setupAction"/>.
    /// </remarks>
    /// <seealso cref="InMemoryOptions"/>
    /// <seealso cref="ILocalCache"/>
    /// <seealso cref="MemoryCacheManager"/>
    public static IAppBuilder AddMemoryCache(this IAppBuilder builder, Action<InMemoryOptions> setupAction)
    {
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            ConfigCommonService(services);
            services.TryAddSingleton<ILocalCache, MemoryCacheManager>();
            services.AddEasyCaching(options => options.UseInMemory(setupAction));
        });
        return builder;
    }

    #endregion

    #region AddRedisCache

    /// <summary>
    /// Agrega un servicio de caché Redis a la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el servicio de caché.</param>
    /// <param name="host">El nombre del host donde se encuentra el servidor Redis.</param>
    /// <param name="port">El puerto en el que se está ejecutando el servidor Redis. El valor predeterminado es 6379.</param>
    /// <param name="keyPrefix">Un prefijo opcional que se añadirá a las claves almacenadas en la caché.</param>
    /// <returns>El constructor de la aplicación con el servicio de caché Redis agregado.</returns>
    /// <remarks>
    /// Este método configura las opciones del caché Redis, incluyendo el tiempo máximo de respuesta, 
    /// la posibilidad de almacenar valores nulos y la configuración del endpoint del servidor.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    public static IAppBuilder AddRedisCache(this IAppBuilder builder, string host, int port = 6379, string keyPrefix = null)
    {
        return builder.AddRedisCache(options =>
        {
            options.MaxRdSecond = MaxRdSecond;
            options.CacheNulls = true;
            options.DBConfig.AllowAdmin = true;
            options.DBConfig.KeyPrefix = keyPrefix;
            options.DBConfig.Endpoints.Add(new ServerEndPoint(host, port));
        });
    }

    /// <summary>
    /// Agrega la funcionalidad de caché Redis al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El builder de la aplicación que se está configurando.</param>
    /// <param name="configuration">La configuración de la aplicación que contiene los parámetros necesarios para la conexión a Redis.</param>
    /// <param name="section">La sección de configuración que se utilizará para obtener los parámetros de Redis. Por defecto es "EasyCaching:Redis".</param>
    /// <returns>El builder de la aplicación con la funcionalidad de caché Redis añadida.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para utilizar Redis como proveedor de caché,
    /// incluyendo la configuración de los puntos finales de Redis y el serializador.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="IConfiguration"/>
    /// <seealso cref="IRedisCache"/>
    public static IAppBuilder AddRedisCache(this IAppBuilder builder, IConfiguration configuration, string section = "EasyCaching:Redis")
    {
        builder.CheckNull(nameof(builder));
        CachingOptions.AddRedisEndPoints(configuration, section);
        builder.Host.ConfigureServices((context, services) =>
        {
            ConfigCommonService(services);
            services.TryAddSingleton<IRedisCache, RedisCacheManager>();
            services.AddEasyCaching(options =>
            {
                var serializerNameKey = $"{section}:SerializerName";
                var serializerName = configuration[serializerNameKey];
                if (serializerName.IsEmpty())
                    configuration[serializerNameKey] = CacheProviderKey.SystemTextJson;
                options.UseRedis(configuration, CacheProviderKey.RedisCache, section);
                ConfigRedisSerializer(options);
            });
        });
        return builder;
    }

    /// <summary>
    /// Configura el serializador de Redis utilizando System.Text.Json.
    /// </summary>
    /// <param name="options">Las opciones de EasyCaching que se van a configurar.</param>
    private static void ConfigRedisSerializer(EasyCachingOptions options)
    {
        options.WithSystemTextJson(t => t.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, CacheProviderKey.SystemTextJson);
    }

    /// <summary>
    /// Agrega la funcionalidad de caché de Redis al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de Redis.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado con la funcionalidad de caché de Redis.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para utilizar Redis como sistema de caché.
    /// Se asegura de que las dependencias requeridas estén registradas en el contenedor de servicios.
    /// </remarks>
    /// <seealso cref="RedisOptions"/>
    /// <seealso cref="IRedisCache"/>
    /// <seealso cref="RedisCacheManager"/>
    public static IAppBuilder AddRedisCache(this IAppBuilder builder, Action<RedisOptions> setupAction)
    {
        builder.CheckNull(nameof(builder));
        CachingOptions.AddRedisEndPoints(setupAction);
        builder.Host.ConfigureServices((context, services) =>
        {
            ConfigCommonService(services);
            services.TryAddSingleton<IRedisCache, RedisCacheManager>();
            services.AddEasyCaching(options =>
            {
                ConfigRedisSerializer(options);
                options.UseRedis(redisOptions =>
                {
                    redisOptions.SerializerName = CacheProviderKey.SystemTextJson;
                    setupAction(redisOptions);
                });
            });
        });
        return builder;
    }

    #endregion

    #region AddHybridCache

    /// <summary>
    /// Agrega un caché híbrido al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le añadirá el caché híbrido.</param>
    /// <param name="topicName">El nombre del tema utilizado para la comunicación entre cachés. Por defecto es "EasyCachingHybridCache".</param>
    /// <returns>El constructor de la aplicación con el caché híbrido añadido.</returns>
    /// <remarks>
    /// Este método configura un caché híbrido que utiliza un proveedor de caché local y un proveedor de caché distribuido.
    /// El proveedor de caché local se establece en <see cref="CacheProviderKey.MemoryCache"/> y el proveedor de caché distribuido se establece en <see cref="CacheProviderKey.RedisCache"/>.
    /// Además, se configuran los puntos finales de Redis y el nombre del serializador.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="CacheProviderKey"/>
    public static IAppBuilder AddHybridCache(this IAppBuilder builder, string topicName = "EasyCachingHybridCache")
    {
        return builder.AddHybridCache(hybridOptions =>
        {
            hybridOptions.LocalCacheProviderName = CacheProviderKey.MemoryCache;
            hybridOptions.DistributedCacheProviderName = CacheProviderKey.RedisCache;
            hybridOptions.TopicName = topicName;
        }, redisBusOptions =>
        {
            CachingOptions.GetRedisEndPoints().ForEach(endpoint =>
            {
                redisBusOptions.Endpoints.Add(endpoint);
            });
            redisBusOptions.SerializerName = CacheProviderKey.SystemTextJson;
        });
    }

    /// <summary>
    /// Extiende la funcionalidad de <see cref="IAppBuilder"/> para agregar un sistema de caché híbrido.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> a la que se le añadirá el caché híbrido.</param>
    /// <param name="configuration">La configuración que se utilizará para establecer el caché.</param>
    /// <param name="hybridSection">La sección de configuración para el caché híbrido. Por defecto es "EasyCaching:Hybrid".</param>
    /// <param name="redisBusSection">La sección de configuración para el bus de Redis. Por defecto es "EasyCaching:RedisBus".</param>
    /// <returns>La instancia de <see cref="IAppBuilder"/> con el caché híbrido configurado.</returns>
    /// <remarks>
    /// Este método permite configurar un sistema de caché híbrido utilizando EasyCaching, 
    /// integrando tanto un caché híbrido como un bus de Redis según las secciones de configuración proporcionadas.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="IConfiguration"/>
    /// <seealso cref="EasyCaching.Core.EasyCachingOptions"/>
    public static IAppBuilder AddHybridCache(this IAppBuilder builder, IConfiguration configuration,
            string hybridSection = "EasyCaching:Hybrid", string redisBusSection = "EasyCaching:RedisBus")
    {
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            services.AddEasyCaching(options =>
            {
                options.UseHybrid(configuration, CacheProviderKey.HybridCache, hybridSection);
                options.WithRedisBus(configuration, CacheProviderKey.RedisBus, redisBusSection);
            });
        });
        return builder;
    }

    /// <summary>
    /// Agrega un sistema de caché híbrido al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="hybridConfigAction">Acción para configurar las opciones de caché híbrido.</param>
    /// <param name="redisBusConfigAction">Acción para configurar las opciones del bus de Redis.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para permitir la configuración encadenada.</returns>
    /// <remarks>
    /// Este método permite integrar un sistema de caché híbrido utilizando EasyCaching,
    /// configurando tanto las opciones de caché híbrido como las del bus de Redis.
    /// </remarks>
    /// <seealso cref="HybridCachingOptions"/>
    /// <seealso cref="RedisBusOptions"/>
    public static IAppBuilder AddHybridCache(this IAppBuilder builder, Action<HybridCachingOptions> hybridConfigAction, Action<RedisBusOptions> redisBusConfigAction)
    {
        builder.CheckNull(nameof(builder));
        builder.Host.ConfigureServices((context, services) =>
        {
            services.AddEasyCaching(options =>
            {
                options.UseHybrid(hybridConfigAction);
                options.WithRedisBus(redisBusConfigAction);
            });
        });
        return builder;
    }

    #endregion
}