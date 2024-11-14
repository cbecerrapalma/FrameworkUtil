namespace Util.Sessions; 

/// <summary>
/// Proporciona métodos de extensión para trabajar con sesiones.
/// </summary>
public static class SessionExtensions {
    /// <summary>
    /// Obtiene el identificador de usuario a partir de la sesión actual.
    /// </summary>
    /// <param name="session">La sesión desde la cual se extrae el identificador de usuario.</param>
    /// <returns>Un <see cref="Guid"/> que representa el identificador de usuario.</returns>
    /// <remarks>
    /// Este método es una extensión de la interfaz <see cref="ISession"/> y permite obtener el 
    /// identificador de usuario de manera sencilla y directa.
    /// </remarks>
    public static Guid GetUserId( this ISession session ) {
        return session.UserId.ToGuid();
    }

    /// <summary>
    /// Obtiene el identificador de usuario de la sesión actual y lo convierte al tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir el identificador de usuario.</typeparam>
    /// <param name="session">La sesión desde la cual se obtiene el identificador de usuario.</param>
    /// <returns>El identificador de usuario convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ISession"/> y permite obtener el UserId
    /// de manera segura y convertirlo al tipo deseado utilizando el método de conversión de la clase <see cref="Util.Helpers.Convert"/>.
    /// </remarks>
    public static T GetUserId<T>( this ISession session ) {
        return Util.Helpers.Convert.To<T>( session.UserId );
    }
}