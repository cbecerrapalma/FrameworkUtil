namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para trabajar con la reflexión en tipos.
/// </summary>
public static class ReflectionExtensions {
    /// <summary>
    /// Obtiene el valor de una propiedad de un objeto dado a partir de la información del miembro.
    /// </summary>
    /// <param name="member">El miembro del que se desea obtener el valor de la propiedad.</param>
    /// <param name="instance">La instancia del objeto del cual se obtendrá el valor de la propiedad.</param>
    /// <returns>El valor de la propiedad del objeto especificado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="member"/> o <paramref name="instance"/> son nulos.</exception>
    /// <remarks>
    /// Este método utiliza la reflexión para acceder a la propiedad del objeto. 
    /// Asegúrese de que el nombre del miembro coincide con una propiedad pública del tipo de la instancia.
    /// </remarks>
    public static object GetPropertyValue( this MemberInfo member, object instance ) {
        if( member == null )
            throw new ArgumentNullException( nameof( member ) );
        if( instance == null )
            throw new ArgumentNullException( nameof( instance ) );
        return instance.GetType().GetProperty( member.Name )?.GetValue( instance );
    }
}