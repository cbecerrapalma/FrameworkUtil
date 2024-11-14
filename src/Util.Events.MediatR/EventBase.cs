namespace Util.Events; 

/// <summary>
/// Clase base abstracta que representa un evento en el sistema.
/// Implementa las interfaces <see cref="IEvent"/> e <see cref="INotification"/>.
/// </summary>
/// <remarks>
/// Esta clase sirve como base para todos los eventos que se generen en el sistema,
/// proporcionando una estructura común y funcionalidades compartidas.
/// </remarks>
public abstract class EventBase : IEvent, INotification {
}