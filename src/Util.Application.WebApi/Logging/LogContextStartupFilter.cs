namespace Util.Applications.Logging; 

/// <summary>
/// Filtro de inicio para configurar el contexto de registro.
/// </summary>
public class LogContextStartupFilter : IStartupFilter {
    /// <summary>
    /// Configura el middleware de la aplicación.
    /// </summary>
    /// <param name="next">La acción que se ejecutará después de aplicar la configuración.</param>
    /// <returns>Una acción que toma un <see cref="IApplicationBuilder"/> y aplica la configuración de middleware.</returns>
    /// <remarks>
    /// Este método agrega un contexto de registro a la aplicación antes de ejecutar la siguiente acción en la cadena de middleware.
    /// </remarks>
    public Action<IApplicationBuilder> Configure( Action<IApplicationBuilder> next ) {
        return app => {
            app.UseLogContext();
            next( app );
        };
    }
}