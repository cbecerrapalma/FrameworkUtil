namespace Util.Microservices.Dapr;

/// <summary>
/// Proporciona métodos de extensión para gestionar el estado en aplicaciones.
/// </summary>
public static class StateManagerExtensions {

    #region EqualIfNotEmpty

    /// <summary>
    /// Compara una propiedad de un objeto de estado con un valor dado, 
    /// y solo realiza la comparación si el valor no está vacío.
    /// </summary>
    /// <typeparam name="TStateManager">El tipo del administrador de estado que implementa <see cref="IStateManagerBase{TStateManager}"/>.</typeparam>
    /// <param name="source">El objeto de estado sobre el cual se realiza la comparación.</param>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>
    /// El objeto de estado original si el valor está vacío; 
    /// de lo contrario, devuelve el resultado de la comparación de igualdad.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión que permite realizar comparaciones de manera más 
    /// segura, evitando errores al trabajar con valores nulos o vacíos.
    /// </remarks>
    /// <seealso cref="IStateManagerBase{TStateManager}"/>
    public static TStateManager EqualIfNotEmpty<TStateManager>( this TStateManager source, string property, object value ) where TStateManager : IStateManagerBase<TStateManager> {
        source.CheckNull( nameof( source ) );
        return value.SafeString().IsEmpty() ? source : source.Equal( property, value );
    }

    #endregion

    #region InIf

    /// <summary>
    /// Extiende la funcionalidad de un administrador de estado para incluir un conjunto de valores en una propiedad específica,
    /// solo si se cumple una condición.
    /// </summary>
    /// <typeparam name="TStateManager">El tipo del administrador de estado que implementa <see cref="IStateManagerBase{TStateManager}"/>.</typeparam>
    /// <param name="source">El administrador de estado actual sobre el cual se aplica la extensión.</param>
    /// <param name="property">El nombre de la propiedad en la que se desea incluir los valores.</param>
    /// <param name="values">Una colección de valores que se incluirán en la propiedad especificada.</param>
    /// <param name="condition">Una condición que determina si se deben incluir los valores o no.</param>
    /// <returns>
    /// El administrador de estado original si la condición es falsa; de lo contrario, el administrador de estado modificado
    /// con los valores incluidos en la propiedad especificada.
    /// </returns>
    /// <remarks>
    /// Este método es útil para aplicar filtros condicionales en la gestión de estados, permitiendo una mayor flexibilidad
    /// en la manipulación de propiedades de estado.
    /// </remarks>
    /// <seealso cref="IStateManagerBase{TStateManager}"/>
    public static TStateManager InIf<TStateManager>( this TStateManager source, string property, IEnumerable<object> values, bool condition ) where TStateManager : IStateManagerBase<TStateManager> {
        source.CheckNull( nameof( source ) );
        return condition ? source.In( property, values ) : source;
    }

    #endregion
}