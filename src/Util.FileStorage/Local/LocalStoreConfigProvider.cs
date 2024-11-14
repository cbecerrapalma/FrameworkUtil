namespace Util.FileStorage.Local;

/// <summary>
/// Proporciona la configuración del almacenamiento local.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ILocalStoreConfigProvider"/> 
/// y se encarga de gestionar la configuración relacionada con el almacenamiento local.
/// </remarks>
public class LocalStoreConfigProvider : ILocalStoreConfigProvider {
    private readonly LocalStoreOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalStoreConfigProvider"/>.
    /// </summary>
    /// <param name="options">Opciones de configuración para el almacenamiento local.</param>
    public LocalStoreConfigProvider( IOptions<LocalStoreOptions> options ) : this( options.Value ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalStoreConfigProvider"/>.
    /// </summary>
    /// <param name="options">Las opciones de configuración para el almacenamiento local.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es nulo.</exception>
    public LocalStoreConfigProvider( LocalStoreOptions options ) {
        options.CheckNull( nameof( options ) );
        _options = options;
    }

    /// <summary>
    /// Obtiene la configuración de opciones de almacenamiento local de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene las opciones de almacenamiento local.
    /// </returns>
    public Task<LocalStoreOptions> GetConfigAsync() {
        return Task.FromResult( _options );
    }
}