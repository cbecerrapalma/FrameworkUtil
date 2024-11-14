using Topic = Dapr.TopicAttribute;

namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Indica que un atributo puede aplicarse a cualquier elemento de programación.
/// </summary>
/// <remarks>
/// Este atributo permite que múltiples instancias del atributo se apliquen a un mismo elemento.
/// </remarks>
/// <attributeUsage>
/// <param name="AttributeTargets">Especifica los tipos de elementos a los que se puede aplicar este atributo.</param>
/// <param name="AllowMultiple">Indica si se permite aplicar múltiples instancias del atributo.</param>
/// </attributeUsage>
[AttributeUsage( AttributeTargets.All, AllowMultiple = true )]
public class TopicAttribute : Topic {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TopicAttribute"/>.
    /// </summary>
    /// <param name="name">El nombre del tema que se asignará al atributo.</param>
    public TopicAttribute( string name ) : base( "pubsub", name ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TopicAttribute"/>.
    /// </summary>
    /// <param name="pubsubName">El nombre del sistema de 
    public TopicAttribute( string pubsubName, string name ) : base( pubsubName, name ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TopicAttribute"/>.
    /// </summary>
    /// <param name="pubsubName">El nombre del sistema de 
    public TopicAttribute( string pubsubName, string name, string match, int priority ) : base( pubsubName, name, match, priority ) {
    }
}