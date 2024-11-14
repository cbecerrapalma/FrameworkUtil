namespace Util.Events;

/// <summary>
/// Clase que contiene las opciones de configuración para MediatR.
/// </summary>
public static class MediatROptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si se está realizando un escaneo.
    /// </summary>
    /// <value>
    /// <c>true</c> si se está realizando un escaneo; de lo contrario, <c>false</c>.
    /// </value>
    public static bool IsScan { get; set; }
}