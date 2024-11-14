namespace Util.Helpers; 

/// <summary>
/// Clase estática que proporciona métodos para trabajar con direcciones IP.
/// </summary>
public static class Ip {
    private static readonly AsyncLocal<string> _ip = new();

    /// <summary>
    /// Establece la dirección IP en el contexto actual.
    /// </summary>
    /// <param name="ip">La dirección IP que se va a establecer.</param>
    /// <remarks>
    /// Este método asigna el valor de la dirección IP a una variable de contexto.
    /// Asegúrese de que la dirección IP proporcionada sea válida antes de llamar a este método.
    /// </remarks>
    public static void SetIp( string ip ) {
        _ip.Value = ip;
    }

    /// <summary>
    /// Restablece el valor de la propiedad <c>_ip</c> a <c>null</c>.
    /// </summary>
    /// <remarks>
    /// Este método es útil para reiniciar el estado de la variable <c>_ip</c>
    /// en situaciones donde se requiere limpiar o reiniciar la información almacenada.
    /// </remarks>
    public static void Reset() {
        _ip.Value = null;
    }

    /// <summary>
    /// Obtiene la dirección IP del cliente que realiza la solicitud.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la dirección IP del cliente. Si no se puede determinar la dirección IP, se devuelve la dirección IP de la red local.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si la dirección IP ya ha sido almacenada en caché. 
    /// Si no es así, intenta obtener la dirección IP remota de la conexión HTTP. 
    /// Si la dirección IP remota es local (127.0.0.1 o ::1), se obtiene la dirección IP de la red local.
    /// </remarks>
    /// <seealso cref="GetLanIp"/>
    /// <seealso cref="GetLanIp(NetworkInterfaceType)"/>
    public static string GetIp() {
        if ( string.IsNullOrWhiteSpace( _ip.Value ) == false )
            return _ip.Value;
        var list = new[] { "127.0.0.1", "::1" };
        var result = Web.HttpContext?.Connection.RemoteIpAddress.SafeString();
        if ( string.IsNullOrWhiteSpace( result ) || list.Contains( result ) )
            result = Common.IsWindows ? GetLanIp() : GetLanIp( NetworkInterfaceType.Ethernet );
        return result;
    }

    /// <summary>
    /// Obtiene la dirección IP de la red local del host actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la dirección IP en formato de texto. 
    /// Si no se encuentra ninguna dirección IP de la red local, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método itera a través de todas las direcciones IP asociadas al nombre del host actual 
    /// y devuelve la primera dirección que pertenece a la familia de direcciones IPv4.
    /// </remarks>
    private static string GetLanIp() {
        foreach ( var hostAddress in Dns.GetHostAddresses( Dns.GetHostName() ) ) {
            if ( hostAddress.AddressFamily == AddressFamily.InterNetwork )
                return hostAddress.ToString();
        }
        return string.Empty;
    }

    /// <summary>
    /// Obtiene la dirección IP local de un tipo de interfaz de red específico.
    /// </summary>
    /// <param name="type">El tipo de interfaz de red para el cual se desea obtener la dirección IP.</param>
    /// <returns>La dirección IP local en formato de cadena, o una cadena vacía si no se encuentra ninguna dirección IP.</returns>
    /// <remarks>
    /// Este método recorre todas las interfaces de red disponibles y verifica si están activas y si su tipo coincide con el especificado.
    /// Solo se devuelve la dirección IP de la primera interfaz que cumpla con los criterios.
    /// Si ocurre un error durante la obtención de la dirección IP, se captura la excepción y se devuelve una cadena vacía.
    /// </remarks>
    private static string GetLanIp( NetworkInterfaceType type ) {
        try {
            foreach ( var item in NetworkInterface.GetAllNetworkInterfaces() ) {
                if ( item.NetworkInterfaceType != type || item.OperationalStatus != OperationalStatus.Up )
                    continue;
                var ipProperties = item.GetIPProperties();
                if ( ipProperties.GatewayAddresses.FirstOrDefault() == null )
                    continue;
                foreach ( var ip in ipProperties.UnicastAddresses ) {
                    if ( ip.Address.AddressFamily == AddressFamily.InterNetwork )
                        return ip.Address.ToString();
                }
            }
        }
        catch {
            return string.Empty;
        }
        return string.Empty;
    }
}