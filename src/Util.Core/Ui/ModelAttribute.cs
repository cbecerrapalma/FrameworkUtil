namespace Util.Ui; 

/// <summary>
/// Indica que la clase a la que se aplica este atributo tiene un comportamiento especial o requiere un tratamiento específico.
/// </summary>
/// <remarks>
/// Este atributo se puede utilizar para marcar clases que necesitan ser procesadas de una manera particular por el sistema.
/// </remarks>
/// <example>
/// [MiAtributo]
/// 
[AttributeUsage( AttributeTargets.Class )]
public class ModelAttribute : Attribute {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ModelAttribute"/>.
    /// </summary>
    /// <param name="model">El modelo que se asignará a la propiedad <see cref="Model"/>.</param>
    public ModelAttribute( string model ) {
        Model = model;
    }

    /// <summary>
    /// Obtiene o establece el modelo.
    /// </summary>
    /// <value>
    /// El modelo como una cadena de texto.
    /// </value>
    public string Model { get; set; }
}