namespace Util.Microservices;

/// <summary>
/// Interfaz que define un gestor de estados.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IStateManagerBase{T}"/> y permite la gestión de estados
/// específicos en una aplicación que implementa la lógica de estado.
/// </remarks>
/// <typeparam name="T">El tipo de estado que se gestiona.</typeparam>
public interface IStateManager : IStateManagerBase<IStateManager> {
    /// <summary>
    /// Ordena los elementos de una colección según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="expression">Una expresión que especifica la propiedad por la cual se ordenarán los elementos.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que representa el estado actual del gestor.</returns>
    /// <remarks>
    /// Este método permite aplicar un ordenamiento a los elementos de una colección utilizando una expresión lambda.
    /// La expresión debe devolver un valor que se utilizará para el ordenamiento.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    IStateManager OrderBy<T>( Expression<Func<T, object>> expression );
    /// <summary>
    /// Ordena los elementos en orden descendente según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están ordenando.</typeparam>
    /// <param name="expression">La expresión que especifica la propiedad por la cual se ordenarán los elementos.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que representa el estado actual después de aplicar el ordenamiento.</returns>
    /// <remarks>
    /// Este método permite realizar un ordenamiento en orden descendente sobre una colección de elementos,
    /// utilizando una expresión lambda que indica la propiedad a utilizar para el ordenamiento.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    IStateManager OrderByDescending<T>( Expression<Func<T, object>> expression );
    /// <summary>
    /// Establece una condición de igualdad para una propiedad específica de un objeto.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que representa el estado actualizado.</returns>
    /// <remarks>
    /// Este método permite filtrar objetos basándose en la igualdad de una propiedad específica.
    /// Asegúrese de que la expresión proporcionada apunte a una propiedad válida del tipo especificado.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    IStateManager Equal<T>( Expression<Func<T, object>> expression, object value );
    /// <summary>
    /// Interfaz que define un método para gestionar el estado de un objeto.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se está gestionando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto cuyo estado se desea gestionar.</param>
    /// <param name="values">Una colección de valores que se utilizarán para actualizar el estado del objeto.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que permite gestionar el estado del objeto.</returns>
    /// <remarks>
    /// Este método permite establecer el estado de un objeto basado en la expresión proporcionada y los valores especificados.
    /// Es útil en escenarios donde se necesita manipular el estado de propiedades de manera dinámica.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    IStateManager In<T>( Expression<Func<T, object>> expression, IEnumerable<object> values );
    /// <summary>
    /// Obtiene una instancia de <see cref="IStateManager"/> utilizando una expresión que 
    /// especifica la propiedad a la que se aplicará el estado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto.</param>
    /// <param name="values">Valores opcionales que pueden ser utilizados en la obtención del estado.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que representa el estado de la propiedad especificada.</returns>
    /// <remarks>
    /// Este método permite acceder al estado de un objeto de tipo <typeparamref name="T"/> 
    /// a través de una expresión lambda que indica la propiedad deseada. Los valores 
    /// adicionales pueden ser utilizados para personalizar el comportamiento de la 
    /// obtención del estado.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    IStateManager In<T>( Expression<Func<T, object>> expression, params object[] values );
}

/// <summary>
/// Define un contrato para un gestor de estado que opera sobre un tipo genérico.
/// </summary>
/// <typeparam name="T">El tipo de objeto que el gestor de estado manejará.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IStateManagerBase{T}"/> y proporciona métodos específicos 
/// para gestionar el estado de objetos de tipo <typeparamref name="T"/>.
/// </remarks>
public interface IStateManager<T> : IStateManagerBase<IStateManager<T>> {
    /// <summary>
    /// Ordena los elementos de una colección en función de una expresión especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="expression">Una expresión que especifica la propiedad por la cual se debe ordenar los elementos.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que representa la colección ordenada.</returns>
    /// <remarks>
    /// Este método permite aplicar un orden ascendente a los elementos de la colección 
    /// utilizando la propiedad definida en la expresión.
    /// </remarks>
    /// <seealso cref="IStateManager{T}"/>
    IStateManager<T> OrderBy( Expression<Func<T, object>> expression );
    /// <summary>
    /// Ordena los elementos de una colección en orden descendente según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="expression">Una expresión que especifica el criterio de ordenación.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que representa la colección ordenada.</returns>
    /// <remarks>
    /// Este método permite aplicar un orden descendente a los elementos de la colección
    /// basándose en el valor devuelto por la expresión. La expresión puede ser cualquier
    /// propiedad o campo del tipo <typeparamref name="T"/> que se desee utilizar para la
    /// ordenación.
    /// </remarks>
    /// <seealso cref="IStateManager{T}"/>
    IStateManager<T> OrderByDescending( Expression<Func<T, object>> expression );
    /// <summary>
    /// Establece una condición de igualdad para el estado del gestor.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se está gestionando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto de tipo <typeparamref name="T"/> que se va a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad especificada en la expresión.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que representa el estado actualizado.</returns>
    /// <remarks>
    /// Este método permite agregar una condición de igualdad a la consulta del estado del gestor,
    /// facilitando la filtración de objetos según el valor de una propiedad específica.
    /// </remarks>
    IStateManager<T> Equal( Expression<Func<T, object>> expression, object value );
    /// <summary>
    /// Define un método para establecer el estado de un objeto de tipo <typeparamref name="T"/> 
    /// utilizando una expresión y un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto cuyo estado se está gestionando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto <typeparamref name="T"/> 
    /// que se desea modificar.</param>
    /// <param name="values">Una colección de valores que se aplicarán a la propiedad especificada 
    /// en la expresión.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que permite continuar gestionando el estado.</returns>
    /// <remarks>
    /// Este método permite la manipulación dinámica del estado de un objeto, facilitando la 
    /// configuración de propiedades a partir de una expresión y un conjunto de valores.
    /// </remarks>
    /// <seealso cref="IStateManager{T}"/>
    IStateManager<T> In( Expression<Func<T, object>> expression, IEnumerable<object> values );
    /// <summary>
    /// Define un método que permite establecer un estado en un objeto de tipo <typeparamref name="T"/> 
    /// utilizando una expresión que representa una propiedad del objeto y un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto cuyo estado se va a gestionar.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto <typeparamref name="T"/> 
    /// que se desea modificar.</param>
    /// <param name="values">Un conjunto de valores que se aplicarán a la propiedad especificada por la expresión.</param>
    /// <returns>Una instancia de <see cref="IStateManager{T}"/> que permite continuar gestionando el estado.</returns>
    /// <remarks>
    /// Este método es útil para aplicar cambios en las propiedades de un objeto de manera dinámica 
    /// y puede ser utilizado en escenarios donde se requiera manipular el estado de forma flexible.
    /// </remarks>
    IStateManager<T> In( Expression<Func<T, object>> expression, params object[] values );
}