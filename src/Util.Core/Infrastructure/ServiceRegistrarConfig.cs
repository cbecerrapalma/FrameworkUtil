namespace Util.Infrastructure; 

/// <summary>
/// Representa la configuración para el registro de servicios.
/// </summary>
public class ServiceRegistrarConfig {
    public static readonly ServiceRegistrarConfig Instance = new ();

    /// <summary>
    /// Desactiva un servicio especificado por su nombre.
    /// </summary>
    /// <param name="serviceName">El nombre del servicio que se desea desactivar.</param>
    public static void Disable( string serviceName ) {
        AppContext.SetSwitch( serviceName, false );
    }

    /// <summary>
    /// Habilita un servicio especificado mediante su nombre.
    /// </summary>
    /// <param name="serviceName">El nombre del servicio que se desea habilitar.</param>
    public static void Enable( string serviceName ) {
        AppContext.SetSwitch( serviceName, true );
    }

    /// <summary>
    /// Determina si un servicio está habilitado basado en su nombre.
    /// </summary>
    /// <param name="serviceName">El nombre del servicio que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el servicio está habilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza <see cref="AppContext.TryGetSwitch"/> para comprobar el estado del servicio.
    /// Si el servicio no está habilitado, se devolverá <c>false</c>. En caso contrario, se devolverá <c>true</c>.
    /// </remarks>
    public static bool IsEnabled( string serviceName ) {
        var result = AppContext.TryGetSwitch( serviceName, out bool isEnabled );
        if ( result && isEnabled == false )
            return false;
        return true;
    }
}