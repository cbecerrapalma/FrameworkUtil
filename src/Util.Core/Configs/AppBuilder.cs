using Util.Helpers;

namespace Util.Configs;

/// <summary>
/// Representa un constructor de aplicaciones que permite configurar y construir una aplicación.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IAppBuilder"/> y proporciona métodos para 
/// agregar componentes y middleware a la aplicación.
/// </remarks>
public class AppBuilder : IAppBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AppBuilder"/>.
    /// </summary>
    /// <param name="host">El constructor de host que se utilizará para configurar la aplicación.</param>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="host"/> es nulo.</exception>
    public AppBuilder( IHostBuilder host ) {
        Host = host ?? throw new ArgumentNullException( nameof( host ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el constructor de host que se utiliza para configurar y crear una instancia de host.
    /// </summary>
    /// <remarks>
    /// Este objeto permite la configuración de servicios y la inicialización de la aplicación.
    /// </remarks>
    /// <value>
    /// Un <see cref="IHostBuilder"/> que representa el constructor de host.
    /// </value>
    public IHostBuilder Host { get; }

    /// <summary>
    /// Construye y configura un host para la aplicación.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IHost"/> que representa el host construido.
    /// </returns>
    public IHost Build() {
        var result = Host.Build();
        Ioc.SetServiceProviderAction( () => result.Services );
        return result;
    }
}