namespace Util.FileStorage.Minio;

/// <summary>
/// Proporciona la configuración necesaria para conectarse a un servidor MinIO.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IMinioConfigProvider"/> y se encarga de 
/// gestionar la configuración relacionada con el acceso a MinIO, incluyendo la 
/// recuperación de credenciales y la configuración del cliente.
/// </remarks>
public class MinioConfigProvider : IMinioConfigProvider {
    private readonly MinioOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MinioConfigProvider"/>.
    /// </summary>
    /// <param name="options">Opciones de configuración de Minio que se utilizarán para inicializar el proveedor.</param>
    public MinioConfigProvider( IOptions<MinioOptions> options ) : this( options.Value ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MinioConfigProvider"/>.
    /// </summary>
    /// <param name="options">Las opciones de configuración de Minio que se utilizarán.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es null.</exception>
    public MinioConfigProvider( MinioOptions options ) {
        options.CheckNull( nameof( options ) );
        _options = options;
    }

    /// <summary>
    /// Obtiene la configuración de opciones de Minio de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene un objeto <see cref="MinioOptions"/> 
    /// con la configuración de Minio.
    /// </returns>
    public Task<MinioOptions> GetConfigAsync() {
        return Task.FromResult( _options );
    }
}