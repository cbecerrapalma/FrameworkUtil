namespace Util.Dates; 

/// <summary>
/// Clase estática que contiene opciones relacionadas con el tiempo.
/// </summary>
public static class TimeOptions {
    /// <summary>
    /// Indica si se debe utilizar la hora UTC.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite configurar el uso de la hora UTC en la aplicación.
    /// Si se establece en <c>true</c>, se utilizará la hora UTC; de lo contrario, se utilizará la hora local.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se utiliza UTC; de lo contrario, <c>false</c>.
    /// </value>
    public static bool IsUseUtc { get; set; } = false;
}