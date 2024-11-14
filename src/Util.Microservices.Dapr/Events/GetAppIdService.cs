using Microsoft.AspNetCore.Hosting;

namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Clase que implementa la interfaz <see cref="IGetAppId"/>.
/// Proporciona servicios para obtener el identificador de la aplicación.
/// </summary>
public class GetAppIdService : IGetAppId {
    private readonly DaprOptions _options;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetAppIdService"/>.
    /// </summary>
    /// <param name="options">Opciones de configuración de Dapr.</param>
    /// <param name="environment">El entorno de hospedaje de la aplicación.</param>
    /// <remarks>
    /// Si <paramref name="options"/> es nulo, se inicializa con una nueva instancia de <see cref="DaprOptions"/>.
    /// </remarks>
    public GetAppIdService( IOptions<DaprOptions> options, IWebHostEnvironment environment = null ) {
        _options = options?.Value ?? new DaprOptions();
        _environment = environment;
    }

    /// <summary>
    /// Obtiene el identificador de la aplicación.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador de la aplicación. 
    /// Si el identificador obtenido de Dapr está vacío, se retorna el nombre de la aplicación del entorno 
    /// o el AppId de las opciones, dependiendo de cuál esté disponible.
    /// </returns>
    public string GetAppId() {
        var result = DaprHelper.GetAppId();
        if ( result.IsEmpty() == false )
            return result;
        return _options.AppId.IsEmpty() ? _environment?.ApplicationName : _options.AppId;
    }
}