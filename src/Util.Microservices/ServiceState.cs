namespace Util.Microservices; 

/// <summary>
/// Representa el estado de un servicio.
/// </summary>
/// <remarks>
/// Este enumerador se utiliza para indicar el resultado de una operación de servicio.
/// </remarks>
public enum ServiceState {
    Fail,
    Ok,
    Unauthorized
}