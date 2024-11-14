using Util.Helpers;

namespace Util.Microservices.Dapr.StateManagements.Queries; 

/// <summary>
/// Clase auxiliar para realizar consultas relacionadas con el estado.
/// </summary>
public static class StateQueryHelper {
    /// <summary>
    /// Obtiene el nombre de la propiedad a partir de una expresión lambda.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="expression">La expresión lambda que representa la propiedad.</param>
    /// <returns>
    /// El nombre de la propiedad en formato de cadena. 
    /// Devuelve <c>null</c> si el nombre de la propiedad está vacío.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una función auxiliar para extraer el nombre de la propiedad 
    /// y lo transforma a minúsculas, separando las propiedades anidadas con un punto.
    /// </remarks>
    /// <seealso cref="Lambda.GetName(Expression{Func{T, object}})"/>
    /// <seealso cref="Util.Helpers.String.FirstLowerCase(string)"/>
    public static string GetProperty<T>( Expression<Func<T, object>> expression ) {
        var property = Lambda.GetName( expression );
        return property.IsEmpty() ? null : property.Split( '.' ).Select( Util.Helpers.String.FirstLowerCase ).Join( separator: "." );
    }
}