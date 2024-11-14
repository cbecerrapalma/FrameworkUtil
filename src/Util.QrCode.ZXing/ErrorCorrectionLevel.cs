namespace Util.QrCode;

/// <summary>
/// Nivel de tolerancia a fallos
/// </summary>
public enum ErrorCorrectionLevel
{
    /// <summary>
    /// Se pueden corregir errores de hasta un 7%.
    /// </summary>
    L,
    /// <summary>
    /// Se pueden corregir hasta un 15% de los errores.
    /// </summary>
    M,
    /// <summary>
    /// Se pueden corregir hasta el 25% de los errores.
    /// </summary>
    Q,
    /// <summary>
    /// Se pueden corregir hasta un 30% de los errores.
    /// </summary>
    H
}