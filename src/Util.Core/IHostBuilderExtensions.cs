using Util.Configs;
using Util.Infrastructure;

namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IHostBuilder"/>.
/// </summary>
public static class IHostBuilderExtensions {
    /// <summary>
    /// Agrega utilidades al constructor de host especificado.
    /// </summary>
    /// <param name="hostBuilder">El constructor de host al que se le agregarán las utilidades.</param>
    /// <returns>El mismo constructor de host con las utilidades agregadas.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="hostBuilder"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad del <see cref="IHostBuilder"/> al inicializar un 
    /// objeto <see cref="Bootstrapper"/> y llamar a su método <c>Start</c>. 
    /// Asegúrese de que el <paramref name="hostBuilder"/> no sea nulo antes de invocar este método.
    /// </remarks>
    /// <seealso cref="Bootstrapper"/>
    public static IHostBuilder AddUtil( this IHostBuilder hostBuilder ) {
        hostBuilder.CheckNull( nameof( hostBuilder ) );
        var bootstrapper = new Bootstrapper( hostBuilder );
        bootstrapper.Start();
        return hostBuilder;
    }

    /// <summary>
    /// Convierte un <see cref="IHostBuilder"/> en un <see cref="IAppBuilder"/>.
    /// </summary>
    /// <param name="hostBuilder">El <see cref="IHostBuilder"/> que se desea convertir.</param>
    /// <returns>Una instancia de <see cref="IAppBuilder"/> que representa la construcción de la aplicación.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="hostBuilder"/> es nulo.</exception>
    /// <seealso cref="IHostBuilder"/>
    /// <seealso cref="IAppBuilder"/>
    public static IAppBuilder AsBuild( this IHostBuilder hostBuilder ) {
        hostBuilder.CheckNull( nameof( hostBuilder ) );
        return new AppBuilder( hostBuilder );
    }

    /// <summary>
    /// Agrega utilidades al constructor de la aplicación.
    /// </summary>
    /// <param name="appBuilder">El constructor de la aplicación al que se le agregarán las utilidades.</param>
    /// <returns>El mismo constructor de la aplicación con las utilidades agregadas.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="appBuilder"/> es nulo.</exception>
    /// <remarks>
    /// Este método inicializa un nuevo <see cref="Bootstrapper"/> utilizando el host del <paramref name="appBuilder"/> 
    /// y llama al método <c>Start</c> en el bootstrapper para iniciar las utilidades.
    /// </remarks>
    public static IAppBuilder AddUtil( this IAppBuilder appBuilder ) {
        appBuilder.CheckNull( nameof( appBuilder ) );
        var bootstrapper = new Bootstrapper( appBuilder.Host );
        bootstrapper.Start();
        return appBuilder;
    }
}