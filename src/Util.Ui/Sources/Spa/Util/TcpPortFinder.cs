// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.Util;

/// <summary>
/// Clase estática que proporciona métodos para encontrar puertos TCP disponibles.
/// </summary>
/// <remarks>
/// Esta clase permite a los desarrolladores identificar puertos TCP que están libres para su uso,
/// lo que es útil al configurar servidores o aplicaciones que requieren comunicación de red.
/// </remarks>
internal static class TcpPortFinder
{
    /// <summary>
    /// Busca un puerto disponible en la máquina local.
    /// </summary>
    /// <returns>
    /// Un número entero que representa un puerto disponible.
    /// </returns>
    /// <remarks>
    /// Este método crea un TcpListener en la dirección de bucle invertido (localhost) y en un puerto asignado automáticamente 
    /// (0). Al iniciar el listener, se obtiene el puerto que ha sido asignado. Después de obtener el puerto, el listener 
    /// se detiene para liberar el recurso.
    /// </remarks>
    public static int FindAvailablePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }
}