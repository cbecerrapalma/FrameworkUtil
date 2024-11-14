namespace Util.Localization; 

/// <summary>
/// Representa un atributo que se utiliza para indicar que un tipo está localizado.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum, Inherited = false)]
public class LocalizedTypeAttribute : Attribute {
    /// <summary>
    /// Obtiene el tipo asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el tipo.
    /// </value>
    public string Type { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalizedTypeAttribute"/>.
    /// </summary>
    /// <param name="type">El tipo de localización que se asignará al atributo.</param>
    public LocalizedTypeAttribute( string type ) {
        Type = type;
    }

    /// <summary>
    /// Obtiene el tipo de recurso asociado a un tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo genérico del cual se desea obtener el tipo de recurso.</typeparam>
    /// <returns>
    /// Una cadena que representa el tipo de recurso asociado al tipo especificado.
    /// </returns>
    public static string GetResourceType<T>() {
        return GetResourceType( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el tipo de recurso asociado a un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el tipo de recurso.</param>
    /// <returns>
    /// Devuelve el nombre del tipo de recurso si se encuentra un atributo <see cref="LocalizedTypeAttribute"/> 
    /// asociado al tipo; de lo contrario, devuelve el nombre del tipo. Si el tipo es <c>null</c>, devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado tiene un atributo <see cref="LocalizedTypeAttribute"/> 
    /// y devuelve el tipo especificado en el atributo si está presente y no está vacío. 
    /// En caso contrario, devuelve el nombre del tipo original.
    /// </remarks>
    /// <seealso cref="LocalizedTypeAttribute"/>
    public static string GetResourceType( Type type ) {
        if ( type == null )
            return null;
        var attribute = type.GetCustomAttribute<LocalizedTypeAttribute>();
        return attribute == null || attribute.Type.IsEmpty() ? type.Name : attribute.Type;
    }
}