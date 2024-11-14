namespace Util.Caching.EasyCaching;

/// <summary>
/// Representa las opciones de configuración para el sistema de almacenamiento en caché.
/// </summary>
internal class CachingOptions {
    private static List<ServerEndPoint> _redisEndPoints = new();

    /// <summary>
    /// Agrega los puntos finales de Redis a la colección de puntos finales de Redis.
    /// </summary>
    /// <param name="configuration">La configuración que contiene la sección de puntos finales de Redis.</param>
    /// <param name="section">El nombre de la sección en la configuración que contiene la configuración de la base de datos.</param>
    /// <remarks>
    /// Este método busca la sección especificada en la configuración y extrae los puntos finales de Redis.
    /// Cada punto final se compone de un host y un puerto, donde el puerto tiene un valor predeterminado de 6379 si no se especifica.
    /// </remarks>
    /// <seealso cref="IConfiguration"/>
    /// <seealso cref="ServerEndPoint"/>
    public static void AddRedisEndPoints( IConfiguration configuration, string section ) {
        var config = configuration.GetSection( $"{section}:DbConfig:Endpoints" );
        var endpoints = config.GetChildren();
        foreach( var endpoint in endpoints ) {
            var host = endpoint["Host"];
            var port = endpoint["Port"].ToIntOrNull() ?? 6379;
            _redisEndPoints.Add( new ServerEndPoint( host, port ) );
        }
    }

    /// <summary>
    /// Agrega puntos finales de Redis a la configuración actual.
    /// </summary>
    /// <param name="setupAction">Una acción que configura las opciones de Redis.</param>
    /// <remarks>
    /// Este método permite personalizar la configuración de los puntos finales de Redis 
    /// mediante una acción que recibe un objeto de tipo <see cref="RedisOptions"/>.
    /// Asegúrese de que la acción no sea nula antes de llamarla.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="setupAction"/> es nulo.</exception>
    public static void AddRedisEndPoints( Action<RedisOptions> setupAction ) {
        setupAction.CheckNull( nameof( setupAction ) );
        var options = new RedisOptions();
        setupAction( options );
        _redisEndPoints.AddRange( options.DBConfig.Endpoints );
    }

    /// <summary>
    /// Obtiene una lista de puntos finales de Redis.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="ServerEndPoint"/> que representan los puntos finales de Redis.
    /// </returns>
    public static List<ServerEndPoint> GetRedisEndPoints() {
        return _redisEndPoints;
    }

    /// <summary>
    /// Limpia la configuración de los puntos finales de Redis.
    /// </summary>
    /// <remarks>
    /// Este método establece la variable de puntos finales de Redis en null,
    /// lo que efectivamente elimina cualquier configuración previa.
    /// </remarks>
    public static void Clear() {
        _redisEndPoints = null;
    }
}