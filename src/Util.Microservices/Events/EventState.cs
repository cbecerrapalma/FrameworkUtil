namespace Util.Microservices.Events;

/// <summary>
/// Representa los diferentes estados de un evento.
/// </summary>
public enum EventState
{
    /// <summary>
    /// Publicado.
    /// </summary>
    [Description("util.eventState.published")]
    Published = 1,
    /// <summary>
    /// En proceso.
    /// </summary>
    [Description("util.eventState.processing")]
    Processing = 2,
    /// <summary>
    /// Todas las suscripciones se completaron con éxito.
    /// </summary>
    [Description("util.eventState.success")]
    Success = 3,
    /// <summary>
    ///  fracaso
    /// </summary>
    [Description("util.eventState.fail")]
    Fail = 4
}