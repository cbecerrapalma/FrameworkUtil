namespace Util.Domain.Events; 

/// <summary>
/// Define un contrato para la gestión de eventos de dominio.
/// </summary>
public interface IDomainEventManager {
    /// <summary>
    /// Obtiene una colección de eventos de dominio que son de solo lectura.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a los eventos de dominio generados en el contexto actual,
    /// garantizando que no se puedan modificar directamente desde fuera de la clase.
    /// </remarks>
    /// <returns>
    /// Una colección de solo lectura que contiene los eventos de dominio.
    /// </returns>
    IReadOnlyCollection<IEvent> DomainEvents { get; }
    /// <summary>
    /// Agrega un evento de dominio a la colección de eventos.
    /// </summary>
    /// <param name="event">El evento de dominio que se va a agregar.</param>
    /// <remarks>
    /// Este método permite registrar un evento que puede ser procesado posteriormente 
    /// por un manejador de eventos. Es útil para implementar patrones de diseño como 
    /// Event Sourcing o CQRS.
    /// </remarks>
    void AddDomainEvent(IEvent @event);
    /// <summary>
    /// Elimina un evento de dominio específico.
    /// </summary>
    /// <param name="event">El evento de dominio que se desea eliminar.</param>
    /// <remarks>
    /// Este método se utiliza para gestionar la lista de eventos de dominio, asegurando que se eliminen los eventos que ya han sido procesados.
    /// </remarks>
    /// <seealso cref="IEvent"/>
    void RemoveDomainEvent( IEvent @event );
    /// <summary>
    /// Limpia los eventos de dominio asociados.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para eliminar todos los eventos de dominio que han sido 
    /// acumulados, permitiendo que el sistema comience un nuevo ciclo de eventos.
    /// </remarks>
    void ClearDomainEvents();
}