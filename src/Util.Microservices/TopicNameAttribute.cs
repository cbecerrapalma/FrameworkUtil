namespace Util.Microservices;

/// <summary>
/// Representa un atributo que se utiliza para asignar un nombre de tema a una clase o miembro.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class TopicNameAttribute : Attribute {
    /// <summary>
    /// Obtiene el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de texto.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TopicNameAttribute"/>.
    /// </summary>
    /// <param name="name">El nombre del tema que se asignará al atributo.</param>
    public TopicNameAttribute( string name ) {
        Name = name;
    }

    /// <summary>
    /// Obtiene el nombre del tipo especificado por el parámetro genérico.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener el nombre.</typeparam>
    /// <returns>
    /// Un <see cref="string"/> que representa el nombre del tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el nombre del tipo.
    /// </remarks>
    /// <seealso cref="GetName(Type)"/>
    public static string GetName<T>() {
        return GetName( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el nombre de un tipo, utilizando un atributo personalizado si está presente.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el nombre.</param>
    /// <returns>
    /// El nombre del tipo si el atributo <see cref="TopicNameAttribute"/> no está presente o si su propiedad <see cref="TopicNameAttribute.Name"/> está vacía,
    /// de lo contrario, devuelve el valor de la propiedad <see cref="TopicNameAttribute.Name"/>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado es nulo y, en caso afirmativo, devuelve nulo.
    /// Si el tipo tiene un atributo <see cref="TopicNameAttribute"/>, se utiliza su propiedad <see cref="TopicNameAttribute.Name"/> 
    /// para determinar el nombre a devolver. Si el atributo no está presente o su nombre está vacío, se devuelve el nombre del tipo.
    /// </remarks>
    public static string GetName( Type type ) {
        if ( type == null )
            return null;
        var attribute = type.GetCustomAttribute<TopicNameAttribute>();
        return attribute == null || attribute.Name.IsEmpty() ? type.Name : attribute.Name;
    }
}