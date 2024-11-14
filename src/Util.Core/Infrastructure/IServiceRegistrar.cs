namespace Util.Infrastructure; 

/// <summary>
/// Define un contrato para el registro de servicios.
/// </summary>
public interface IServiceRegistrar {
    /// <summary>
    /// Obtiene el identificador único de la orden.
    /// </summary>
    /// <value>
    /// Un entero que representa el identificador de la orden.
    /// </value>
    int OrderId { get; }

    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    bool Enabled { get; }

    /// <summary>
    /// Registra un nuevo servicio en el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto del servicio donde se registrará el nuevo servicio.</param>
    /// <remarks>
    /// Este método se utiliza para inicializar y configurar un servicio dentro del contexto especificado.
    /// Asegúrese de que el contexto esté correctamente configurado antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="ServiceContext"/>
    Action Register( ServiceContext context );
}