namespace Util.Microservices.Dapr.StateManagements;

/// <summary>
/// Clase que representa un administrador de estado para Dapr, 
/// implementando la interfaz <see cref="IStateManager"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona funcionalidades para gestionar el estado 
/// en aplicaciones que utilizan Dapr, permitiendo la persistencia 
/// y recuperación de datos de estado.
/// </remarks>
public class DaprStateManager : DaprStateManagerBase<IStateManager>, IStateManager {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprStateManager"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr que se utilizará para las operaciones de estado.</param>
    /// <param name="options">Las opciones de configuración de Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de registradores.</param>
    /// <param name="keyGenerator">El generador de claves utilizado para crear claves únicas.</param>
    public DaprStateManager( DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory, IKeyGenerator keyGenerator ) 
        : base(client,options,loggerFactory,keyGenerator){
    }

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están ordenando.</typeparam>
    /// <param name="expression">Una expresión que especifica el criterio de ordenación.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que permite la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método utiliza la expresión para determinar el orden de los elementos. 
    /// Asegúrese de que la expresión sea válida para el tipo de datos que se está utilizando.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    public IStateManager OrderBy<T>( Expression<Func<T, object>> expression ) {
        Sort.OrderBy( expression );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Ordena los elementos en orden descendente según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están ordenando.</typeparam>
    /// <param name="expression">Una expresión que especifica la propiedad por la cual se ordenarán los elementos.</param>
    /// <returns>Una instancia de <see cref="IStateManager"/> que permite la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para determinar el criterio de ordenación.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    public IStateManager OrderByDescending<T>( Expression<Func<T, object>> expression ) {
        Sort.OrderByDescending( expression );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición de igualdad en el filtro utilizando una expresión.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se está filtrando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad especificada en la expresión.</param>
    /// <returns>Una instancia del gestor de estado.</returns>
    /// <remarks>
    /// Este método permite agregar una condición de igualdad a un filtro, facilitando la construcción de consultas dinámicas.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    public IStateManager Equal<T>( Expression<Func<T, object>> expression, object value ) {
        Filter.Equal( expression, value );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión para el estado actual utilizando una expresión y un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se está filtrando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto que se va a filtrar.</param>
    /// <param name="values">Una colección de valores que se utilizarán para el filtro de inclusión.</param>
    /// <returns>Una instancia del <see cref="IStateManager"/> que permite encadenar llamadas.</returns>
    /// <remarks>
    /// Este método permite filtrar los resultados basándose en si la propiedad especificada en la expresión
    /// contiene alguno de los valores proporcionados en la colección.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    public IStateManager In<T>( Expression<Func<T, object>> expression, IEnumerable<object> values ) {
        Filter.In( expression, values );
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un filtro de inclusión para el estado actual utilizando una expresión.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se está filtrando.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad del objeto que se va a evaluar.</param>
    /// <param name="values">Una lista de valores que se utilizarán para la comparación de inclusión.</param>
    /// <returns>Devuelve el administrador de estado actual.</returns>
    /// <remarks>
    /// Este método permite agregar un filtro que verifica si el valor de la propiedad especificada está incluido en la lista de valores proporcionada.
    /// </remarks>
    /// <seealso cref="IStateManager"/>
    public IStateManager In<T>( Expression<Func<T, object>> expression, params object[] values ) {
        Filter.In( expression, values );
        return this;
    }
}