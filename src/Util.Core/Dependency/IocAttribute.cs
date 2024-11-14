namespace Util.Dependency; 

/// <summary>
/// Indica que la clase a la que se aplica este atributo tiene un comportamiento especial.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para marcar clases que requieren un tratamiento específico por parte del sistema.
/// </remarks>
/// <param name="AttributeTargets.Class">Especifica que el atributo solo puede aplicarse a clases.</param>
[AttributeUsage(AttributeTargets.Class)]
public class IocAttribute : Attribute {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IocAttribute"/>.
    /// </summary>
    /// <param name="priority">El nivel de prioridad asociado con este atributo.</param>
    public IocAttribute( int priority ) {
        Priority = priority;
    }

    /// <summary>
    /// Obtiene o establece la prioridad.
    /// </summary>
    /// <value>
    /// Un entero que representa la prioridad.
    /// </value>
    public int Priority { get; set; }
}