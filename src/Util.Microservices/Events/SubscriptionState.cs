namespace Util.Microservices.Events;

/// <summary>
/// Representa los diferentes estados de una suscripción.
/// </summary>
public enum SubscriptionState
{
    /// <summary>
    /// Procesando
    /// </summary>
    [Description("util.subscriptionState.processing")]
    Processing = 1,
    /// <summary>
    /// Éxito completo.
    /// </summary>
    [Description("util.subscriptionState.success")]
    Success = 2,
    /// <summary>
    ///  fracaso
    /// </summary>
    [Description("util.subscriptionState.fail")]
    Fail = 3
}